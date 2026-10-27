Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Imports NativeExtended.NativeDraws

Public NotInheritable Class NativeRadio
    Private Const ZitBaseGXT As String = "TM_82_"
    Private Sub New() : End Sub
    ''' <summary>
    ''' Returns True if MobileRadio = True (native).
    ''' </summary>
    Public Shared ReadOnly Property CanRenderHudInMobileRadio As Boolean
        Get
            Return [Call](Of Boolean)("CAN_RENDER_RADIOHUD_SPRITE_IN_MOBILE_PHONE")
        End Get
    End Property
    Public Shared Property CurrentStation As RadioStations
        Set(value As RadioStations)
            [Call]("RETUNE_RADIO_TO_STATION_INDEX", value)
        End Set
        Get
            Return [Call](Of Integer)("GET_PLAYER_RADIO_STATION_INDEX")
        End Get
    End Property
    Public Shared ReadOnly Property CurrentStationRoll As RadioStations
        Get
            Dim CurrRadioRollHash As String = [Call](Of String)("GET_PLAYER_RADIO_STATION_NAME_ROLL")
            Dim CurrRadioRollIndex As RadioStations = GetIndexFromHash(CurrRadioRollHash)
            Return CurrRadioRollIndex
        End Get
    End Property
    ''' <summary>
    ''' Enables or disables in-game radio (also mobile radio).
    ''' </summary>
    Public Shared WriteOnly Property EnableFrontendRadio As Boolean
        Set(value As Boolean)
            If value Then
                [Call]("ENABLE_FRONTEND_RADIO")
            Else
                [Call]("DISABLE_FRONTEND_RADIO")
            End If
        End Set
    End Property
    'FORCE_RADIO_TRACK( "VLADIVOSTOK", "RADIO_STATION_VLADIVOSTOK_INTRO_TRACK", 2, 9 )
    Public Shared Sub ForceRadioTrack(RadioStation As RadioStations, Track As String, Optional Unknown1 As Integer = 2, Optional Unknown2 As Integer = 9)
        [Call]("FORCE_RADIO_TRACK", RadioStation.ToString, Track, Unknown1, Unknown2)
    End Sub
    ''' <summary>
    ''' Not fully tested. Seems it causes many radio issues while is True.
    ''' </summary>
    Public Shared WriteOnly Property FreezeStation As Boolean
        Set(value As Boolean)
            If value = True Then
                [Call]("FREEZE_RADIO_STATION", CurrentStation.ToString)
            Else
                [Call]("UNFREEZE_RADIO_STATION", CurrentStation.ToString)
            End If
        End Set
    End Property
    ''' <summary>
    ''' Returns station name from GXT.
    ''' </summary>
    ''' <param name="Coloured">If True, will be applied color codes to match with radio station icons 
    ''' (full name could be different from original one).</param>
    Public Shared Function GetStationName(RadioStation As RadioStations, Optional Coloured As Boolean = False) As String
        If Not Coloured Then Return GXT.GetString([Call](Of String)("GET_RADIO_NAME", RadioStation))
        Select Case CInt(RadioStation)
            Case 0
                Return TextColors.Fx_Highlighted & TextColors.MP_Yellow2 & "The Vibe " & TextColors.FX_StopColorFxHere & TextColors.MP_Yellow3 & "98.9"
            Case 1
                Return TextColors.MP_Red & "Liberty Rock Radio " & TextColors.White & "97.8"
            Case 2
                Return TextColors.MP_Orange & "Jazz " & TextColors.MP_VeryLightBlue & "Nation " & TextColors.MP_LightGreen & "Radio " & TextColors.Black & "108.5"
            Case 3
                Return TextColors.MP_Yellow2 & "Massive B " & TextColors.Black & "Soundsystem" & TextColors.MP_Yellow2 & "96.9"
            Case 4
                Return TextColors.Fx_Highlighted & TextColors.MP_Yellow & "K109 The Studio"
            Case 5
                Return TextColors.MP_Red & "We Know The Truth"
            Case 6
                Return TextColors.Black & "Liberty City " & TextColors.MP_Red & "Hardcore Rock"
            Case 7
                Return "The " & TextColors.Fx_Highlighted & TextColors.Yellow & "Journey"
            Case 8
                Return TextColors.MP_Red & "Fu" & TextColors.FX_StopColorFxHere & TextColors.MP_Yellow & "si" & TextColors.MP_VeryLightBlue & "on " & TextColors.MP_LightGreen & "FM"
            Case 9
                Return "The Beat " & TextColors.MP_Yellow & "102.7"
            Case 10
                Return TextColors.MP_Orange & "Radio " & TextColors.MP_Violet & "Broker"
            Case 11
                Return TextColors.MP_Orange & "Vladivostok FM"
            Case 12
                Return TextColors.MP_Red & "Public " & TextColors.White & "Liberty " & TextColors.MP_Red & "Radio"
            Case 13
                Return TextColors.Blue & "San Juan " & TextColors.MP_Red & "Sounds"
            Case 14
                Return TextColors.MP_Purple2 & "Electro " & TextColors.Gray & "Choc"
            Case 15
                Return TextColors.MP_Red & "The " & TextColors.White & "Classics " & TextColors.MP_Red & "104.1"
            Case 16
                Return TextColors.MP_Brown & "International " & TextColors.MP_LightOrange & "Funk"
            Case 17
                Return TextColors.MP_LightGreen & "Tuff " & TextColors.MP_Yellow2 & " Gong " & TextColors.MP_Red & "Radio"
            Case 18
                Return TextColors.MP_Yellow2 & "Independence " & TextColors.FX_StopColorFxHere & "FM"
            Case 19
                Return TextColors.Blue & "Integrity " & TextColors.MP_Red & "2.0"
            Case 255
                Return TextColors.LightGray & "Radio OFF"
            Case Else
                Return "Radio Station Index not valid!"
        End Select
    End Function
    Public Shared Property IntegrityStationEnabled As Boolean
        Get
            Return Not [Call](Of Boolean)("IS_LAZLOW_STATION_LOCKED")
        End Get
        Set(value As Boolean)
            If value = True Then [Call]("UNLOCK_LAZLOW_STATION") Else [Call]("LOCK_LAZLOW_STATION")
        End Set
    End Property
    Public Shared ReadOnly Property IsRadioHudON As Boolean
        Get
            Return [Call](Of Boolean)("IS_RADIO_HUD_ON")
        End Get
    End Property
    Public Shared ReadOnly Property IsRetuning As Boolean
        Get
            Return [Call](Of Boolean)("IS_RADIO_RETUNING")
        End Get
    End Property
    Public Shared Property MobileRadio As Boolean
        Get
            Return [Call](Of Boolean)("IS_MOBILE_PHONE_RADIO_ACTIVE")
        End Get
        Set(value As Boolean)
            [Call]("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", value)
            [Call]("SET_MOBILE_PHONE_RADIO_STATE", value)
        End Set
    End Property
    ''' <summary>
    ''' Shows current track in-game using ZiT's GXT strings.
    ''' </summary>
    ''' <param name="HelpMessage">If true it will display an help message (GXT), otherwise it will report on phone (works only if ZiT is unlocked).</param>
    ''' <param name="AddToBrief">Sets if ZiT's string will diplay on previous briefs.</param>
    Public Shared Sub ReportCurrentTrack(Optional HelpMessage As Boolean = True, Optional AddToBrief As Boolean = True)
        Dim TrackID As Integer = [Call](Of Integer)("GET_AUDIBLE_MUSIC_TRACK_TEXT_ID")
        Dim FullGXT As String = String.Concat(ZitBaseGXT, TrackID)
        If HelpMessage Then
            GXT.PrintHelp(FullGXT, PrintForever:=False, NoSound:=False, BriefDisplay:=AddToBrief, AdditionalGxtEntry:=Nothing)
        Else
            [Call]("REPORT_TAGGED_RADIO_TRACK", TrackID)
            If AddToBrief Then
                Dim FullString = GXT.GetString(FullGXT)
                NativeGeneric.AddTextToBrief(FullString, IsGXT:=False, ShowUnderscores:=False)
            End If
        End If
        'DisplayText(FullString.Remove(0, 13))
    End Sub
    Public Shared Sub RetuneToPrevious()
        [Call]("RETUNE_RADIO_UP")
    End Sub
    Public Shared Sub RetuneToNext()
        [Call]("RETUNE_RADIO_DOWN")
    End Sub
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared Sub SetCarRadioLoud(Vehicle As Vehicle, Optional Enabled As Boolean = True)
        If Vehicle Is Nothing Then
            Tools.WriteLog("Error in NativeRadio.SetVehicleLoudRadio: vehicle not found or not valid.")
            Return
        End If
        [Call]("SET_LOUD_VEHICLE_RADIO", Vehicle, Enabled)
    End Sub
    Public Shared Sub SkipTrack()
        [Call]("SKIP_RADIO_FORWARD")
    End Sub
    ''' <summary>
    ''' List of IV Radio Stations' Index (integer) or Hash name (.ToString).
    ''' </summary>
    Public Enum RadioStations
        TheVibe = 0
        LibertyRockRadio = 1
        JazzNationRadio = 2
        MassiveBSoundsystem = 3
        K109TheStudio = 4
        WeKnowTheTruth = 5
        LibertyCityHardcoreRock = 6
        TheJourney = 7
        FusionFM = 8
        TheBeat = 9
        RadioBroker = 10
        VladivostokFM = 11
        PublicLibertyRadio = 12
        SanJuanSounds = 13
        ElectroChoc = 14
        TheClassics = 15
        InternationalFunk = 16
        TuffGongRadio = 17
        IndependenceFM = 18
        Integrity = 19
        RadioOff = 255
    End Enum
    Private Shared Function GetHashFromIndex(RadioStationIndex As RadioStations) As String
        Select Case CInt(RadioStationIndex)
            Case 0
                Return "THE_VIBE"
            Case 1
                Return "LIBERTY_ROCK"
            Case 2
                Return "JAZZ_NATION"
            Case 3
                Return "BOBBY_KONDERS"
            Case 4
                Return "K109_THE_STUDIO"
            Case 5
                Return "WKTT"
            Case 6
                Return "HARDCORE"
            Case 7
                Return "CLASSICAL_AMBIENT"
            Case 8
                Return "FUSION_FM"
            Case 9
                Return "BEAT_95"
            Case 10
                Return "DANCE_ROCK"
            Case 11
                Return "VLADIVOSTOK"
            Case 12
                Return "PLR"
            Case 13
                Return "SAN_JUAN_SOUNDS"
            Case 14
                Return "DANCE_MIX"
            Case 15
                Return "NY_CLASSICS"
            Case 16
                Return "AFRO_BEAT"
            Case 17
                Return "BABYLON"
            Case 18
                Return "NDEPENDENT"
            Case 19
                Return "INTEGRITY"
            Case 255
                Return "Radio OFF"
            Case Else
                Return "Radio Station Index not valid!"
        End Select
    End Function
    Private Shared Function GetIndexFromHash(RadioStationHash As String) As RadioStations
        Select Case CInt(RadioStationHash)
            Case "THE_VIBE"
                Return RadioStations.TheVibe
            Case "LIBERTY_ROCK"
                Return RadioStations.LibertyRockRadio
            Case "JAZZ_NATION"
                Return RadioStations.JazzNationRadio
            Case "BOBBY_KONDERS"
                Return RadioStations.MassiveBSoundsystem
            Case "K109_THE_STUDIO"
                Return RadioStations.K109TheStudio
            Case "WKTT"
                Return RadioStations.WeKnowTheTruth
            Case "HARDCORE"
                Return RadioStations.LibertyCityHardcoreRock
            Case "CLASSICAL_AMBIENT"
                Return RadioStations.TheJourney
            Case "FUSION_FM"
                Return RadioStations.FusionFM
            Case "BEAT_95"
                Return RadioStations.TheBeat
            Case "DANCE_ROCK"
                Return RadioStations.RadioBroker
            Case "VLADIVOSTOK"
                Return RadioStations.VladivostokFM
            Case "PLR"
                Return RadioStations.PublicLibertyRadio
            Case "SAN_JUAN_SOUNDS"
                Return RadioStations.SanJuanSounds
            Case "DANCE_MIX"
                Return RadioStations.ElectroChoc
            Case "NY_CLASSICS"
                Return RadioStations.TheClassics
            Case "AFRO_BEAT"
                Return RadioStations.InternationalFunk
            Case "BABYLON"
                Return RadioStations.TuffGongRadio
            Case "NDEPENDENT"
                Return RadioStations.IndependenceFM
            Case "INTEGRITY"
                Return RadioStations.Integrity
            Case "Radio OFF"
                Return RadioStations.RadioOff
            Case Else
                Return "Radio Station Index not valid!"
        End Select
    End Function
End Class
'Public Shared Sub DisplayHudInMobileRadio_MaybeNotWorking(Display As Boolean)
'    [Call]("RENDER_RADIOHUD_SPRITE_IN_MOBILE_PHONE", Display)
'End Sub




