Imports GTA.Native.Function
Imports GTA
Imports GTA.base
Public NotInheritable Class NativeScript
    Private Sub New() : End Sub
    ' Private Shared pointRenderID As New Native.Pointer(GetType(Integer))
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared Sub AllocateToObject(ScriptName As String, ObjModel As String, Radius As Single, Optional Priority As Integer = 100, Optional Type As Integer = -1)
        Try
            If String.IsNullOrWhiteSpace(ScriptName) Then
                Tools.WriteLog("Error in NativeScript.AllocateToObject: " & ObjModel & " is not valid!")
                Return
            Else
                If NativeModel.IsValid(ObjModel) Then
                    [Call]("ALLOCATE_SCRIPT_TO_OBJECT", ScriptName, NativeModel.GetHashKey(ObjModel), Priority, Tools.ClampFloat(Radius, 1.0, 4000.0), Type)
                Else
                    Tools.WriteLog("Error in NativeScript.AllocateToObject: " & ObjModel & " is not valid!")
                    Return
                End If
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeScript.AllocateToObject.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared Sub AllocateToPed(ScriptName As String, PedModel As String, Optional Priority As Integer = 100, Optional UnknownFlag As Boolean = True)
        Try
            If Not Exists(ScriptName) Then
                Tools.WriteLog("Error in NativeScript.AllocateToPed: " & ScriptName & " doesn't exist.")
                Return
            Else
                If NativeModel.IsValid(PedModel) Then
                    [Call]("ALLOCATE_SCRIPT_TO_RANDOM_PED", ScriptName, NativeModel.GetHashKey(PedModel), Priority, UnknownFlag)
                Else
                    Tools.WriteLog("Error in NativeScript.AllocateToPed: " & PedModel & " is not valid.")
                    Return
                End If
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeScript.AllocateToPed.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Returns True if ScriptName exists in game files.
    ''' </summary>
    ''' <param name="ScriptName"></param>
    ''' <returns></returns>
    Public Shared Function Exists(ScriptName As String) As Boolean
        If String.IsNullOrWhiteSpace(ScriptName) Then
            Tools.WriteLog("Error in NativeScript.Exists: ScriptName string is null or empty.")
            Return False
        End If
        Return [Call](Of Boolean)("DOES_SCRIPT_EXIST", ScriptName)
    End Function
    Public Shared Function GetNumberOfInstances(ScriptName As String) As Integer
        If Not Exists(ScriptName) Then
            Tools.WriteLog("Error in NativeScript.GetNumberOfInstances: " & ScriptName & " script doesn't exist.")
            Return Nothing
        End If
        Return [Call](Of Integer)("GET_NUMBER_OF_INSTANCES_OF_STREAMED_SCRIPT", ScriptName)
    End Function
    Public Shared Function IsLoaded(ScriptName As String) As Boolean
        If Not Exists(ScriptName) Then
            Tools.WriteLog("Error in NativeScript.IsLoaded: " & ScriptName & " script doesn't exist.")
            Return False
        End If
        Return [Call](Of Boolean)("HAS_SCRIPT_LOADED", ScriptName)
    End Function
    Public Shared Function IsRunning(ScriptName As String) As Boolean
        If Not Exists(ScriptName) Then
            Tools.WriteLog("Error in NativeScript.IsRunning: " & ScriptName & " script doesn't exist.")
            Return False
        End If
        Return [Call](Of Integer)("GET_NUMBER_OF_INSTANCES_OF_STREAMED_SCRIPT", ScriptName) > 0
    End Function
    ''' <summary>
    ''' Loads a script. Not needed if TickHelper is running.
    ''' </summary>
    Public Shared Sub Request(ScriptName As String)
        If Exists(ScriptName) Then
            If Not IsLoaded(ScriptName) Then [Call]("REQUEST_SCRIPT", ScriptName)
        Else
            Tools.WriteLog("Error in NativeScript.Request: " & ScriptName & " script doesn't exist.")
        End If
    End Sub
    ''' <summary>
    ''' Starts a specific SCO script. Can automatic load it before start using TickHelper. Use with caution!
    ''' </summary>
    Public Shared Sub Start(ScriptName As String, Optional MemoryStack As Integer = 1024)
        If Not Exists(ScriptName) Then
            Tools.WriteLog("Error in NativeScript.Start: " & ScriptName & " script doesn't exist.")
            Return
        End If
        If IsRunning(ScriptName) Then Return 'AndAlso Not ScriptName.Equals("taxi", StringComparison.OrdinalIgnoreCase) Then Return

        Dim started As Boolean = False
        Dim TickAction As Action = Nothing
        TickAction = Sub()
                         Try
                             If started OrElse IsRunning(ScriptName) Then
                                 TickHelper.Remove_Internal(TickAction)
                                 Return
                             End If

                             If Not IsLoaded(ScriptName) Then
                                 [Call]("REQUEST_SCRIPT", ScriptName)
                                 Return
                             End If

                             [Call]("START_NEW_SCRIPT", ScriptName, MemoryStack)
                             started = True
                             TickHelper.Remove_Internal(TickAction)
                         Catch ex As Exception
                             Tools.WriteLog("Error in NativeScript.Start(" & ScriptName & ").", ex)
                             TickHelper.Remove_Internal(TickAction)
                         End Try
                     End Sub

        Dim startTime As DateTime = DateTime.UtcNow
        If (DateTime.UtcNow - startTime).TotalSeconds > 5 Then
            Tools.WriteLog("Timeout loading script: " & ScriptName)
            TickHelper.Remove_Internal(TickAction)
            Return
        End If
        TickHelper.Add_Internal(TickAction)
    End Sub
    ''' <summary>
    ''' Starts a specific SCO script with args (unknown). Can automatic load it before start using TickHelper. Use with caution!
    ''' </summary>
    ''' <param name="Arg1">Reference value (don't know the type).</param>
    ''' <param name="Arg2">Unknown. SCO script use 1, 2, 3, 7, 11, 20, 29, 34 and 64.</param>
    Public Shared Sub Start(ScriptName As String, Arg1 As Native.Pointer, Arg2 As Integer, Optional MemoryStack As Integer = 1024)
        If Not Exists(ScriptName) Then
            Tools.WriteLog("Error in NativeScript.Start: " & ScriptName & " script doesn't exist.")
            Return
        End If
        If IsRunning(ScriptName) Then Return 'AndAlso Not ScriptName.Equals("taxi", StringComparison.OrdinalIgnoreCase) Then Return

        Dim started As Boolean = False
        Dim TickAction As Action = Nothing
        TickAction = Sub()
                         Try
                             If started OrElse IsRunning(ScriptName) Then
                                 TickHelper.Remove_Internal(TickAction)
                                 Return
                             End If

                             If Not IsLoaded(ScriptName) Then
                                 [Call]("REQUEST_SCRIPT", ScriptName)
                                 Return
                             End If

                             [Call]("START_NEW_SCRIPT_WITH_ARGS", ScriptName, Arg1, Arg2, MemoryStack)
                             started = True
                             TickHelper.Remove_Internal(TickAction)
                         Catch ex As Exception
                             Tools.WriteLog("Error in NativeScript.Start(" & ScriptName & ").", ex)
                             TickHelper.Remove_Internal(TickAction)
                         End Try
                     End Sub

        Dim startTime As DateTime = DateTime.UtcNow
        If (DateTime.UtcNow - startTime).TotalSeconds > 5 Then
            Tools.WriteLog("Timeout loading script: " & ScriptName)
            TickHelper.Remove_Internal(TickAction)
            Return
        End If
        TickHelper.Add_Internal(TickAction)
    End Sub
    ''' <summary>
    ''' Stops all instances of a specific SCO script. Use with caution!
    ''' </summary>
    Public Shared Sub [Stop](ScriptName As String, Optional Release As Boolean = True)
        If Not Exists(ScriptName) Then
            Tools.WriteLog("Error in NativeScript.Stop: " & ScriptName & " script doesn't exist.")
            Return
        End If

        If Not IsRunning(ScriptName) Then
            If Release AndAlso IsLoaded(ScriptName) Then [Call]("MARK_SCRIPT_AS_NO_LONGER_NEEDED", ScriptName)
            Return
        End If
        [Call]("TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME", ScriptName)
        If Release AndAlso IsLoaded(ScriptName) Then [Call]("MARK_SCRIPT_AS_NO_LONGER_NEEDED", ScriptName)
    End Sub

    Public Structure IVScripts
        Public Const AdvancedCarActions As String = "advanced_car_actions"
        Public Const AmbAirportArea As String = "ambairpotarea"
        Public Const AmbAtmq As String = "ambatmq"
        Public Const AmbBar As String = "ambbar"
        Public Const AmbBarrier As String = "ambbarrier"
        Public Const AmbBeggar As String = "ambbeggar"
        Public Const AmbBlkHawk As String = "ambblkhawk"
        Public Const AmbBouncer As String = "ambbouncer"
        Public Const AmbBridgePatrol As String = "ambbridgepatrol"
        Public Const AmbBusker As String = "ambbusker"
        Public Const AmbCabaret As String = "ambcabaret"
        Public Const AmbCargoHoldDoors As String = "ambcargoholddoors"
        Public Const AmbChurchDoors As String = "ambchurchdoors"
        Public Const AmbClubLights As String = "ambclublights"
        Public Const AmbComedyClub As String = "ambcomedyclub"
        Public Const AmbControlLoader As String = "ambcontrolloader"
        Public Const AmbControlMain As String = "ambcontrolmain"
        Public Const AmbDealer As String = "ambdealer"
        Public Const AmbDebug As String = "ambdebug"
        Public Const AmbFloater As String = "ambfloater"
        Public Const AmbGerry3DoorLock As String = "ambgerry3doorlock"
        Public Const AmbHelicopter As String = "ambhelicopter"
        Public Const AmbHomelandCordon As String = "ambhomelandcordon"
        Public Const AmbHomelandFed As String = "ambhomelandfed"
        Public Const AmbHomelandSirens As String = "ambhomelandsirens"
        Public Const AmbHotel As String = "ambhotel"
        Public Const AmbInternetCafe As String = "ambinternetcafe"
        Public Const AmbJerseyDocksGates As String = "ambjerseydocksgates"
        Public Const AmbJimsLocks As String = "ambjimslocks"
        Public Const AmbLiftDoors As String = "ambliftdoors"
        Public Const AmbNightclubExt As String = "ambnightclubext"
        Public Const AmbPimpNPros As String = "ambpimpnpros"
        Public Const AmbPoleDancer As String = "ambpoledancer"
        Public Const AmbPolRdBlk As String = "ambpolrdblk"
        Public Const AmbPreacher As String = "ambpreacher"
        Public Const AmbSaveBed As String = "ambsavebed"
        Public Const AmbShowroom As String = "ambshowroom"
        Public Const AmbStripClub As String = "ambstripclub"
        Public Const AmbTaxDpt As String = "ambtaxdpt"
        Public Const AmbTaxiHail As String = "ambtaxihail"
        Public Const AmbToiletDoors As String = "ambtoiletdoors"
        Public Const AmbTunnelCops As String = "ambtunnelcops"
        Public Const AmbTv As String = "ambtv"
        Public Const AmbUnArea As String = "ambunarea"
        Public Const AmbWardrobe As String = "ambwardrobe"
        Public Const AmbWindowLift As String = "ambwindowlift"
        Public Const ArrestEscapeHelp As String = "arrestescapehelp"
        Public Const AtmObj As String = "atmobj"
        Public Const Badman1 As String = "badman_1"
        Public Const Bell2 As String = "bell2"
        Public Const Bell3M As String = "bell3m"
        Public Const Bell3P As String = "bell3p"
        Public Const Bell5 As String = "bell5"
        Public Const Bell6 As String = "bell6"
        Public Const Bell7 As String = "bell7"
        Public Const Bernie1 As String = "bernie1"
        Public Const Bernie2 As String = "bernie2"
        Public Const Bernie3 As String = "bernie3"
        Public Const BernieCar As String = "berniecar"
        Public Const BincoBrookS As String = "binco_brook_s"
        Public Const Boating As String = "boating"
        Public Const BowlingLane As String = "bowling_lane"
        Public Const BowlTrigger As String = "bowl_trigger"
        Public Const Brian1 As String = "brian_1"
        Public Const Brian2 As String = "brian_2"
        Public Const Brian3 As String = "brian_3"
        Public Const Brucie1 As String = "brucie1"
        Public Const Brucie2 As String = "brucie2"
        Public Const Brucie3A As String = "brucie3a"
        Public Const Brucie3B As String = "brucie3b"
        Public Const Brucie3MeetGuy As String = "brucie3_meetguy"
        Public Const Brucie4 As String = "brucie4"
        Public Const Brucie5M As String = "brucie5m"
        Public Const Brucie5P As String = "brucie5p"
        Public Const BrucieBoat As String = "brucieboat"
        Public Const BrucieCopter As String = "bruciecopter"
        Public Const BrucieDrink As String = "bruciedrink"
        Public Const BrucieEat As String = "brucieeat"
        Public Const BrucieLift As String = "brucielift"
        Public Const BrucieShow As String = "brucieshow"
        Public Const BrucieStrip As String = "bruciestrip"
        Public Const BrucieTenpin As String = "brucietenpin"
        Public Const BrucieHeli As String = "brucie_heli"
        Public Const BurgerVendor As String = "burgervendor"
        Public Const CableCars As String = "cablecars"
        Public Const CarDiveHelp As String = "cardivehelp"
        Public Const CarFoodRunJack As String = "carfoodrunjack"
        Public Const CarSteal As String = "carsteal"
        Public Const CarWash As String = "carwash"
        Public Const CarTheftTexts As String = "car_theft_texts"
        Public Const Cherise As String = "cherise"
        Public Const Cia1 As String = "cia1"
        Public Const Cia2 As String = "cia2"
        Public Const Cia3 As String = "cia3"
        Public Const Cia4 As String = "cia4"
        Public Const CiaWanted As String = "ciawanted"
        Public Const Clarence As String = "clarence"
        Public Const CleanupRay5Trains As String = "cleanupray5trains"
        Public Const ComputerMain As String = "computermain"
        Public Const ComputerNews As String = "computernews"
        Public Const ComputerStreamed As String = "computerstreamed"
        Public Const CoopBombDbase As String = "coop_bombdbase"
        Public Const CoopDrugFac As String = "coop_drugfac"
        Public Const CoopSwatAsslt As String = "coop_swatasslt"
        Public Const CopBootSearch As String = "copbootsearch"
        Public Const Darts As String = "darts"
        Public Const DartsLauncher As String = "darts_launcher"
        Public Const DartsTrigger As String = "darts_trigger"
        Public Const DateActivity As String = "date_activity"
        Public Const DateFood As String = "date_food"
        Public Const DatingAlex As String = "dating_alex"
        Public Const DatingCarmen As String = "dating_carmen"
        Public Const DatingKate As String = "dating_kate"
        Public Const DatingKiki As String = "dating_kiki"
        Public Const DatingMichelle As String = "dating_michelle"
        Public Const DeathArrestHelp As String = "deatharresthelp"
        Public Const DeathMatchCr As String = "deathmatch_cr"
        Public Const Derrick1 As String = "derrick1"
        Public Const Derrick2 As String = "derrick2"
        Public Const Derrick3 As String = "derrick3"
        Public Const Dwayne1 As String = "dwayne1"
        Public Const Dwayne3 As String = "dwayne3"
        Public Const DwayneCherise As String = "dwaynecherise"
        Public Const DwayneDrink As String = "dwaynedrink"
        Public Const DwayneEat As String = "dwayneeat"
        Public Const DwayneEmail As String = "dwayneemail"
        Public Const DwayneShow As String = "dwayneshow"
        Public Const DwayneStrip As String = "dwaynestrip"
        Public Const DwayneTenpin As String = "dwaynetenpin"
        Public Const DwayneBackup As String = "dwayne_backup"
        Public Const Eddie1 As String = "eddie1"
        Public Const Eddie2 As String = "eddie2"
        Public Const Elizabeta1 As String = "elizabeta1"
        Public Const Elizabeta2 As String = "elizabeta2"
        Public Const Elizabeta3 As String = "elizabeta3"
        Public Const Elizabeta4 As String = "elizabeta4"
        Public Const EmergencyCall As String = "emergencycall"
        Public Const EmpireDown As String = "empiredown"
        Public Const EmpireTelescope As String = "empiretelescope"
        Public Const EndCreditRoll As String = "endcreditroll"
        Public Const EndGameNews As String = "endgamenews"
        Public Const Faustin1 As String = "faustin1"
        Public Const Faustin2 As String = "faustin2"
        Public Const Faustin3 As String = "faustin3"
        Public Const Faustin4 As String = "faustin4"
        Public Const Faustin5 As String = "faustin5"
        Public Const Faustin5Fire As String = "faustin5_fire"
        Public Const Faustin6 As String = "faustin6"
        Public Const Faustin7 As String = "faustin7"
        Public Const Finale1A As String = "finale1a"
        Public Const Finale1B As String = "finale1b"
        Public Const Finale1C As String = "finale1c"
        Public Const Finale1D As String = "finale1d"
        Public Const Finale2 As String = "finale2"
        Public Const FinaleAorb As String = "finaleaorb"
        Public Const FinaleIntroCut As String = "finaleintrocut"
        Public Const FinaleWedTxts As String = "finalewedtxts"
        Public Const FoodServer As String = "foodserver"
        Public Const Francis1 As String = "francis1"
        Public Const Francis2A As String = "francis2a"
        Public Const Francis2B As String = "francis2b"
        Public Const Francis2Appointment As String = "francis2_appointment"
        Public Const Francis3 As String = "francis3"
        Public Const Francis4 As String = "francis4"
        Public Const Francis5 As String = "francis5"
        Public Const Francis6 As String = "francis6"
        Public Const FreemodeCr As String = "freemode_cr"
        Public Const Gambetti1 As String = "gambetti1"
        Public Const Gambetti2 As String = "gambetti2"
        Public Const Gambetti3 As String = "gambetti3"
        Public Const GarbageTrucks As String = "garbage_trucks"
        Public Const Gerry1 As String = "gerry1"
        Public Const Gerry1Base As String = "gerry1_base"
        Public Const Gerry2 As String = "gerry2"
        Public Const Gerry3A As String = "gerry3a"
        Public Const Gerry3B As String = "gerry3b"
        Public Const Gerry3C As String = "gerry3c"
        Public Const Gerry3PhoneGirl As String = "gerry3_phonegirl"
        Public Const Gerry4 As String = "gerry4"
        Public Const Gerry5 As String = "gerry5"
        Public Const GoOnDate As String = "go_on_date"
        Public Const Gracie As String = "gracie"
        Public Const GunLockup As String = "gunlockup"
        Public Const GunLockupCt As String = "gunlockupct"
        Public Const HandbrakeHelp As String = "handbrakehelp"
        Public Const HappyTshirt As String = "happytshirt"
        Public Const Helicopter As String = "helicopter"
        Public Const HospitalPickup As String = "hospitalpickup"
        Public Const Hossan1 As String = "hossan_1"
        Public Const HotwireHelp As String = "hotwirehelp"
        Public Const Ilyena As String = "ilyena"
        Public Const IlyenaCalls As String = "ilyena_calls"
        Public Const IndependenceFm As String = "independencefm"
        Public Const Initial As String = "initial"
        Public Const InitialNet As String = "initialnet"
        Public Const InternetDating As String = "internet_dating"
        Public Const Ivan1 As String = "ivan_1"
        Public Const Jacob1 As String = "jacob1"
        Public Const Jacob2 As String = "jacob2"
        Public Const Jacob3M As String = "jacob3m"
        Public Const Jacob3P As String = "jacob3p"
        Public Const JacobDarts As String = "jacobdarts"
        Public Const JacobDrink As String = "jacobdrink"
        Public Const JacobEat As String = "jacobeat"
        Public Const JacobLift As String = "jacoblift"
        Public Const JacobPool As String = "jacobpool"
        Public Const JacobShow As String = "jacobshow"
        Public Const JacobStrip As String = "jacobstrip"
        Public Const JacobGunCar As String = "jacob_gun_car"
        Public Const Jeff1 As String = "jeff_1"
        Public Const Jeff1Mission As String = "jeff_1_mission"
        Public Const Jeff2 As String = "jeff_2"
        Public Const Jeff3 As String = "jeff_3"
        Public Const Jimmy1 As String = "jimmy1"
        Public Const Jimmy2 As String = "jimmy2"
        Public Const Jimmy3 As String = "jimmy3"
        Public Const Jimmy4 As String = "jimmy4"
        Public Const Jimmy4A As String = "jimmy4a"
        Public Const JimmySuit As String = "jimmysuit"
        Public Const LaterWeapons As String = "laterweapons"
        Public Const MagVendor As String = "magvendor"
        Public Const Main As String = "main"
        Public Const ManhatHeliTours As String = "manhat_heli_tours"
        Public Const Manny1 As String = "manny1"
        Public Const Manny2 As String = "manny2"
        Public Const Manny3 As String = "manny3"
        Public Const Marnie1 As String = "marnie1"
        Public Const Marnie2 As String = "marnie2"
        Public Const Mel As String = "mel"
        Public Const Michelle1 As String = "michelle1"
        Public Const Michelle1Queue As String = "michelle1_queue"
        Public Const MichelleIntroCalls As String = "michelle_introcalls"
        Public Const ModoManhat5 As String = "modo_manhat_5"
        Public Const MostWanted As String = "mostwanted"
        Public Const MpCellphone As String = "mpcellphone"
        Public Const MpCellphoneCalling As String = "mpcellphonecalling"
        Public Const MpCellphoneMain As String = "mpcellphonemain"
        Public Const MpCellphoneNetwork As String = "mpcellphonenetwork"
        Public Const MpCellphoneParty As String = "mpcellphoneparty"
        Public Const MpTutorialLauncher As String = "mptutoriallauncher"
        Public Const MultiplayerTutorial As String = "multiplayertutorial"
        Public Const MultiTutorial As String = "multitutorial"
        Public Const NetworkMain As String = "network_main"
        Public Const NetworkSession As String = "network_session"
        Public Const NetworkStartup As String = "network_startup"
        Public Const NutVendor As String = "nutvendor"
        Public Const OrganiserHelp As String = "organiserhelp"
        Public Const Packie1 As String = "packie1"
        Public Const Packie2 As String = "packie2"
        Public Const Packie3 As String = "packie3"
        Public Const PackieDarts As String = "packiedarts"
        Public Const PackieDrink As String = "packiedrink"
        Public Const PackiePool As String = "packiepool"
        Public Const PackieShow As String = "packieshow"
        Public Const PackieStrip As String = "packiestrip"
        Public Const PackieSuit As String = "packiesuit"
        Public Const PackieTenpin As String = "packietenpin"
        Public Const PackieBomb As String = "packie_bomb"
        Public Const PartyMode As String = "party_mode"
        Public Const Pathos1 As String = "pathos1"
        Public Const Pathos2 As String = "pathos2"
        Public Const PdbHangoutMgr As String = "pdbhangoutmgr"
        Public Const PdbHomeMgr As String = "pdbhomemgr"
        Public Const PdbMostWanted As String = "pdbmostwanted"
        Public Const PerseusManhat8 As String = "perseus_manhat_8"
        Public Const PhonePadHelp As String = "phonepadhelp"
        Public Const PhoneProfileHelp As String = "phoneprofilehelp"
        Public Const PhysicsTestBed As String = "physicstestbed"
        Public Const Placeholder As String = "placeholder"
        Public Const Playboy2 As String = "playboy2"
        Public Const Playboy3 As String = "playboy3"
        Public Const Playboy4 As String = "playboy4"
        Public Const PoliceBackup As String = "policebackup"
        Public Const PoliceTest As String = "policetest"
        Public Const PoolGame As String = "pool_game"
        Public Const PoolTable As String = "pool_table"
        Public Const Puzzle As String = "puzzle"
        Public Const PuzzleLauncher As String = "puzzle_launcher"
        Public Const PxdfCut As String = "pxdfcut"
        Public Const RacesCr As String = "races_cr"
        Public Const Ray1 As String = "ray1"
        Public Const Ray2 As String = "ray2"
        Public Const Ray3 As String = "ray3"
        Public Const Ray4 As String = "ray4"
        Public Const Ray5 As String = "ray5"
        Public Const Ray6 As String = "ray6"
        Public Const ReplayHelp As String = "replayhelp"
        Public Const ReplayTutorial As String = "replaytutorial"
        Public Const Roman1 As String = "roman1"
        Public Const Roman10M As String = "roman10m"
        Public Const Roman10P As String = "roman10p"
        Public Const Roman11 As String = "roman11"
        Public Const Roman12 As String = "roman12"
        Public Const Roman13 As String = "roman13"
        Public Const Roman14 As String = "roman14"
        Public Const Roman2 As String = "roman2"
        Public Const Roman3 As String = "roman3"
        Public Const Roman3TxtMsgRepeat As String = "roman3txtmsgrepeat"
        Public Const Roman4 As String = "roman4"
        Public Const Roman5 As String = "roman5"
        Public Const Roman6 As String = "roman6"
        Public Const Roman7 As String = "roman7"
        Public Const Roman8M As String = "roman8m"
        Public Const Roman8P As String = "roman8p"
        Public Const Roman9 As String = "roman9"
        Public Const RomanBlips As String = "romanblips"
        Public Const RomanDarts As String = "romandarts"
        Public Const RomanDrink As String = "romandrink"
        Public Const RomanEat As String = "romaneat"
        Public Const RomanMarker As String = "romanmarker"
        Public Const RomanPool As String = "romanpool"
        Public Const RomanShow As String = "romanshow"
        Public Const RomanStrip As String = "romanstrip"
        Public Const RomanTenpin As String = "romantenpin"
        Public Const RomanBleedoutIntro As String = "roman_bleedoutintro"
        Public Const RomanFakeBlip As String = "roman_fakeblip"
        Public Const RomanTaxi As String = "roman_taxi"
        Public Const RovingCr As String = "roving_cr"
        Public Const Sara2 As String = "sara2"
        Public Const Sara1 As String = "sara_1"
        Public Const SixaxisTutLauncher As String = "sixaxistutlauncher"
        Public Const SixaxisTutorial As String = "sixaxistutorial"
        Public Const SpCellphone As String = "spcellphone"
        Public Const SpCellphoneCalling As String = "spcellphonecalling"
        Public Const SpCellphoneDebug As String = "spcellphonedebug"
        Public Const SpCellphoneEndCall As String = "spcellphoneendcall"
        Public Const SpCellphoneMain As String = "spcellphonemain"
        Public Const SpCellphoneNetwork As String = "spcellphonenetwork"
        Public Const SpCellphoneTutorial As String = "spcellphonetutorial"
        Public Const SpeechControl As String = "speechcontrol"
        Public Const SpeechControlNet As String = "speechcontrol_net"
        Public Const Sprunk As String = "sprunk"
        Public Const Startup As String = "startup"
        Public Const StatTracker As String = "stattracker"
        Public Const StatBridge As String = "stat_bridge"
        Public Const StoryTimer As String = "storytimer"
        Public Const Stunt As String = "stunt"
        Public Const Taxi As String = "taxi"
        Public Const TaxiTrigger As String = "taxi_trigger"
        Public Const Telescope As String = "telescope"
        Public Const TenpinBowl As String = "tenpinbowl"
        Public Const TextLinkMission As String = "text_link_mission"
        Public Const Till As String = "till"
        Public Const TmBase As String = "tm_base"
        Public Const TmVip As String = "tm_vip"
        Public Const TollBooth As String = "tollbooth"
        Public Const TrainHelper As String = "trainhelper"
        Public Const TxtLnkMissNet As String = "txtlnkmiss_net"
        Public Const TxtMsgHelp As String = "txtmsghelp"
        Public Const Vendor As String = "vendor"
        Public Const Vigilante As String = "vigilante"
        Public Const Vlad1 As String = "vlad1"
        Public Const Vlad2 As String = "vlad2"
        Public Const Vlad3 As String = "vlad3"
        Public Const Vlad4 As String = "vlad4"
        Public Const WantedHelp As String = "wantedhelp"
        Public Const WaypointHelp As String = "waypointhelp"
        Public Const WebAlexBlog As String = "webalexblog"
        Public Const WebAutoEroticar As String = "webautoeroticar"
        Public Const WebCrapList As String = "webcraplist"
        Public Const WebDating As String = "webdating"
        Public Const WebDatingBoys As String = "webdatingboys"
        Public Const WebDatingFullProfile As String = "webdatingfullprofile"
        Public Const WebDatingGirls As String = "webdatinggirls"
        Public Const WebEddieBlog As String = "webeddieblog"
        Public Const WebEmail As String = "webemail"
        Public Const WebError As String = "weberror"
        Public Const WebEyeFind As String = "webeyefind"
        Public Const WebEyeFindSearch As String = "webeyefindsearch"
        Public Const WebLawyersCareer As String = "weblawyerscareer"
        Public Const WebLawyersCv As String = "weblawyerscv"
        Public Const WebLcpd As String = "weblcpd"
        Public Const WebLcpdProfile As String = "weblcpdprofile"
        Public Const WebLibertyTree As String = "weblibertytree"
        Public Const WebLibertyTreeNews As String = "weblibertytreenews"
        Public Const WebOurOwnReality As String = "webourownreality"
        Public Const WebPublicLiberty As String = "webpublicliberty"
        Public Const WebRealEstate As String = "webrealestate"
        Public Const WebRingtones As String = "webringtones"
        Public Const WebRingtonesMono As String = "webringtonesmono"
        Public Const WebRingtonesPoly As String = "webringtonespoly"
        Public Const WebRingtonesPurchase As String = "webringtonespurchase"
        Public Const WebRingtonesTheme As String = "webringtonestheme"
        Public Const WebWeazel As String = "webweazel"
        Public Const WebWeazelNews As String = "webweazelnews"
        Public Const WebWhatThey As String = "webwhatthey"
        Public Const WindowLiftLauncher As String = "window_lift_launcher"
    End Structure

    '''' <summary>
    '''' Untested.
    '''' </summary>
    'Public Shared Sub AllocateToObject(ScriptName As String, ModelHash As Integer, Radius As Single, Optional Priority As Integer = 100, Optional Type As Integer = -1)
    '    Try
    '        If String.IsNullOrWhiteSpace(ScriptName) Then
    '            Tools.WriteLog("Error in NativeScript.AllocateToObject: " & ModelHash & " Hash is not valid!")
    '            Return
    '        Else
    '            [Call]("ALLOCATE_SCRIPT_TO_OBJECT", ScriptName, ModelHash, Priority, Tools.ClampFloat(Radius, 1.0, 4000.0), Type)
    '        End If
    '    Catch ex As Exception
    '        Tools.WriteLog("Error in NativeScript.AllocateToObject.", ex)
    '    End Try
    'End Sub
    '''' <summary>
    '''' Untested.
    '''' </summary>
    'Public Shared Sub AllocateToPed(ScriptName As String, ModelHash As Integer, Optional Priority As Integer = 100, Optional UnknownFlag As Boolean = True)
    '    Try
    '        If String.IsNullOrWhiteSpace(ScriptName) Then
    '            Tools.WriteLog("Error in NativeScript.AllocateToPed: " & ModelHash & " Hash is not valid.")
    '            Return
    '        End If

    '        If Not Exists(ScriptName) Then
    '            Tools.WriteLog("Error in NativeScript.AllocateToPed: " & ScriptName & " doesn't exists.")
    '            Return
    '        End If

    '        [Call]("ALLOCATE_SCRIPT_TO_RANDOM_PED", ScriptName, ModelHash, Priority, UnknownFlag)
    '    Catch ex As Exception
    '        Tools.WriteLog("Error in NativeScript.AllocateToPed.", ex)
    '    End Try
    'End Sub
    'Public Shared ReadOnly Property RenderID As Integer
    '    Get
    '        [Call]("GET_SCRIPT_RENDERTARGET_RENDER_ID", pointRenderID)
    '        Return pointRenderID.Value
    '    End Get
    'End Property
End Class
