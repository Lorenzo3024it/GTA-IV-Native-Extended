Imports System.Collections.Specialized.BitVector32
Imports GTA
Imports GTA.Native.Function

Public Class NativePolice
    Public Shared WriteOnly Property DontDispatchCopsforPlayer As Boolean
        Set(value As Boolean)
            [Call]("DONT_DISPATCH_COPS_FOR_PLAYER", NativePlayer.Index, value)
        End Set
    End Property
    Public Shared Sub AddPoliceRestart(Position As Vector3, FacingAngle As Double, StationID As Integer)
        [Call]("ADD_POLICE_RESTART", Position.X, Position.Y, Position.Z, FacingAngle, StationID)
    End Sub
    'Public Shared Sub DontDispatchCopsForPlayer(DispatchOFF As Boolean)
    '    [Call]("DONT_DISPATCH_COPS_FOR_PLAYER", NativePlayer.Index, DispatchOFF)
    'End Sub 'If True, police won't dispatch for player. If False police will dispatch normally
    'Public Shared Sub EnableScannerAudio(Enable As Boolean)
    '    If Enable Then [Call]("ENABLE_POLICE_SCANNER") Else [Call]("DISABLE_POLICE_SCANNER")
    'End Sub
    'Public Shared Sub EnableChaseAudio(Enable As Boolean)
    '    [Call]("ENABLE_CHASE_AUDIO", Enable)
    'End Sub
    Public Shared WriteOnly Property ChaseAudio As Boolean
        Set(value As Boolean)
            [Call]("DONT_DISPATCH_COPS_FOR_PLAYER", NativePlayer.Index, value)
        End Set
    End Property

    Public Shared Function isPlayerWantedLevelGreaterThan(WantedLevel) As Boolean
        Return [Call](Of Boolean)("IS_WANTED_LEVEL_GREATER", NativePlayer.Index, WantedLevel)
    End Function
    Public Shared Function hasPlayerFlashingWantedLevelStars() As Boolean
        Return [Call](Of Boolean)("PLAYER_HAS_FLASHING_STARS_ABOUT_TO_DROP", NativePlayer.Index)
    End Function
    Public Shared Function hasPlayerGreyedWantedLevelStars() As Boolean
        Return [Call](Of Boolean)("PLAYER_HAS_GREYED_OUT_STARS", NativePlayer.Index)
    End Function
    'Public Shared Function areRandomCopsEnabled() As Boolean
    '    Return [Call](Of Boolean)("GET_CREATE_RANDOM_COPS")
    'End Function
    ''' <summary>
    ''' Untested. GTAMods Wiki says: "if no wanted level, cops on foot and in cars won't spawn. 
    ''' If player has wanted level, cops on foot won't spawn, cars and helis will spawn normally"
    ''' </summary>
    Public Shared Property RandomCops As Boolean
        Get
            Return [Call](Of Boolean)("GET_CREATE_RANDOM_COPS")
        End Get
        Set(value As Boolean)
            [Call]("SET_CREATE_RANDOM_COPS", value)
        End Set
    End Property
    Public Shared Sub SetFakeWantedLevel(WantedLevel As Integer)
        [Call]("SET_FAKE_WANTED_LEVEL", WantedLevel)
    End Sub
    Public Shared Sub StoreWantedLevel(Optional Bool As Boolean = True)
        [Call]("STORE_WANTED_LEVEL", NativePlayer.Index, Bool)
    End Sub
    Public Shared Sub SetMaxWantedLevel(MaxWantedLevel As Integer)
        [Call]("SET_MAX_WANTED_LEVEL", MaxWantedLevel)
    End Sub
    'Public Shared Sub SetPedWontAttakPlayerWithoutWantedLevel(Ped As Ped, Bool As Boolean)
    '    [Call]("SET_PED_WONT_ATTACK_PLAYER_WITHOUT_WANTED_LEVEL", Ped, Bool)
    'End Sub
    Public Shared WriteOnly Property IgnorePlayer As Boolean
        Set(value As Boolean)
            [Call]("SET_POLICE_IGNORE_PLAYER", NativePlayer.Index, value)
        End Set
    End Property
    'Public Shared Sub SetPoliceIgnorePlayer(Ignore As Boolean)
    '    [Call]("SET_POLICE_IGNORE_PLAYER", NativePlayer.Index, Ignore)
    'End Sub

    Public Shared WriteOnly Property ScannerAudio As Boolean
        Set(value As Boolean)
            [Call]("DONT_DISPATCH_COPS_FOR_PLAYER", NativePlayer.Index, value)
        End Set
    End Property
    Public Shared Sub SetWantedLevel(WantedLevel As Integer, Optional ApplyNow As Boolean = False)
        Dim PlayerIndex = NativePlayer.Index
        If WantedLevel = 0 Then
            [Call]("CLEAR_WANTED_LEVEL", PlayerIndex)
        Else
            [Call]("ALTER_WANTED_LEVEL", PlayerIndex, WantedLevel)
            If ApplyNow Then [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", PlayerIndex)
        End If
    End Sub
    Public Shared Sub TriggerCrimeAudio(Position As Vector3, UnknownInt As Integer)
        [Call]("TRIGGER_VIGILANTE_CRIME", UnknownInt, Position.X, Position.Y, Position.Z)
    End Sub
    Public Shared Sub TriggerPoliceReport(AudioID As Reports)
        Dim ReportString As String
        Select Case AudioID
            Case 0
                ReportString = "SCRIPTED_REPORTS_B4"
            Case 1
                ReportString = "SCRIPTED_REPORTS_B5"
            Case 2
                ReportString = "SCRIPTED_REPORTS_BC3"
            Case 3
                ReportString = "SCRIPTED_REPORTS_C4"
            Case 4
                ReportString = "SCRIPTED_REPORTS_DF2"
            Case 5
                ReportString = "SCRIPTED_REPORTS_DM2"
            Case 6
                ReportString = "SCRIPTED_REPORTS_DM3"
            Case 7
                ReportString = "SCRIPTED_REPORTS_EM4"
            Case 8
                ReportString = "SCRIPTED_REPORTS_EM6"
            Case 9
                ReportString = "SCRIPTED_REPORTS_F5"
            Case 10
                ReportString = "SCRIPTED_REPORTS_FM2"
            Case 11
                ReportString = "SCRIPTED_REPORTS_FM5"
            Case 12
                ReportString = "SCRIPTED_REPORTS_G2"
            Case 13
                ReportString = "SCRIPTED_REPORTS_GM1"
            Case 14
                ReportString = "SCRIPTED_REPORTS_PM3"
            Case 15
                ReportString = "SCRIPTED_REPORTS_PX1"
            Case 16
                ReportString = "SCRIPTED_REPORTS_R12"
            Case 17
                ReportString = "SCRIPTED_REPORTS_RB6"
            Case 18
                ReportString = "SCRIPTED_REPORTS_TWAT"
            Case Else
                Exit Sub
        End Select
        [Call]("TRIGGER_POLICE_REPORT", ReportString)
    End Sub
    Public Shared Sub SetPedWantedByPolice(Ped As Ped, Optional Wanted As Boolean = True)
        [Call]("SET_CHAR_WANTED_BY_POLICE", Ped, Wanted)
    End Sub
    ''' <summary>
    ''' This function disables police radar blips while wanted if Hide = True.
    ''' </summary>
    Public Shared Sub HidePoliceRadarBlips(Optional Hide As Boolean = True)
        [Call]("SET_POLICE_RADAR_BLIPS", Not Hide)
    End Sub
    Public Shared Sub SetZoneNoCops(Zone As String, Optional NoCops As Boolean = True)
        [Call]("SET_ZONE_NO_COPS", Zone, NoCops)
    End Sub
    Public Shared Sub WillTrackCar(Car As Vehicle, Optional Enabled As Boolean = True)
        [Call]("SET_POLICE_FOCUS_WILL_TRACK_CAR", Car, Enabled)
    End Sub
    Public Enum Reports
        TrespassFactory = 0 '"SCRIPTED_REPORTS_B4"
        ToLiveAndDieInAlderney = 1 '"SCRIPTED_REPORTS_B5"
        BuoysAhoy = 2 ' "SCRIPTED_REPORTS_BC3"
        PaperTrail = 3 '"SCRIPTED_REPORTS_C4"
        UndressToKill = 4 ' "SCRIPTED_REPORTS_DF2"
        Babysitting = 5 '"SCRIPTED_REPORTS_DM2"
        TunnelOfDeath = 6 '"SCRIPTED_REPORTS_DM3"
        BlowYourCover = 7 ' "SCRIPTED_REPORTS_EM4"
        TheSnowStorm = 8 '"SCRIPTED_REPORTS_EM6"
        RiggedToBlow = 9 '"SCRIPTED_REPORTS_F5"
        FinalInterview = 10 '"SCRIPTED_REPORTS_FM2"
        BloodBrothers = 11 '"SCRIPTED_REPORTS_FM5"
        DiningOut = 12 ' "SCRIPTED_REPORTS_G2"
        ActionsSpeaksLouderThanWords = 13 '"SCRIPTED_REPORTS_GM1"
        ThreeLeafCover = 14 '"SCRIPTED_REPORTS_PM3"
        DeconstructionForBeginners = 15 '"SCRIPTED_REPORTS_PX1"
        HostileNegotiation = 16 '"SCRIPTED_REPORTS_R12"
        LateCheckout = 17 '"SCRIPTED_REPORTS_RB6"
        SexualDeviant = 18 '"SCRIPTED_REPORTS_TWAT"
    End Enum
End Class