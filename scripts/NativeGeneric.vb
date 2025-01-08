Imports GTA
Imports GTA.Native.Function

Public Class NativeGeneric
    Inherits Script

    'Info Message Box
    'Public WithEvents InfoDrawing As GTA.Timer
    Public Shared isAnyInfoOnScreen As Boolean
    Public Shared hasInfoBeepPlayed As Boolean = False
    Public Shared InfoText As String
    Public Shared InfoDisplayed As Boolean = False
    Public Shared InfoLines_1_to_5 As Boolean
    Public Shared XPosBG As Integer
    Public Shared YPosBG As Integer
    Public Shared YSizeBG As Integer
    Public Shared isDrawingHelpMessage As Boolean

    Public Shared CarRadioDisabledByEra As Boolean = False
    'Public WithEvents tMessageDisplay As GTA.Timer
    'Infos Showed
    Public Shared infoDeloreanDead As Integer = 0 '0: Never Displayed; 1:On Screen; 2:Displayed, not on screen; 
    Public Shared tFlashBlip As Integer
    Public Shared bFlashBlip As Boolean = False
    Public Shared Blip As Blip
    'Weapons
    Public Shared Weapon() As Integer
    Public Shared Ammo() As Integer
    Public Shared clipAmmo() As Integer
    Public Enum RomanMood
        Normal = 0
        Sad = 1
        ShakenUp = 2
        Drunk = 3
    End Enum
    Public Sub New()
        Me.Interval = 1
        Array.Resize(Weapon, 11)
        Array.Resize(Ammo, 11)
        Array.Resize(clipAmmo, 11)
        'tMessageDisplay = New GTA.Timer
        'InfoDrawing = New GTA.Timer
        'BindKey(Keys.F10, AddressOf InfoMessage)

        'ConfFile = SettingsFile.Open("common\data\tt_settings.dat")
        'ConfFile.Load()
    End Sub
    Public Sub GeneralNative_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Tick
        'Flash Blip stop
        If bFlashBlip = True Then
            tFlashBlip += 1
            If tFlashBlip > 200 Then
                bFlashBlip = False
                tFlashBlip = 0
                FlashBlip(Blip, True)
            End If
        End If
    End Sub
    ''===========================================================================================================================================================
    ''Info Message Test
    ''----------------------
    'Public Shared Sub InfoMessageStop()
    '    'InfoDisplayed = False
    'End Sub
    'Public Shared Sub InfoMessage(ByVal Text As String, Optional NeedsTimer As Boolean = True)
    '    'Audio
    '    If isDrawingHelpMessage = False Then
    '        isDrawingHelpMessage = True

    '        'If BeepPlayed = False Then
    '        'AudioPlayer.Play("infobeep.wav", "infobeep")

    '        'BeepPlayed = True
    '        'Else
    '        Game.WaitInCurrentScript(1200)
    '        ' AudioPlayer.StopAudio("infobeep")
    '    End If
    '    'End If
    '    'If Disappare = True Then

    '    'Else
    '    'End If
    'End Sub
    'Public Shared Sub InfoMessage(ByVal TableName As String, ByVal EntryName As String, ByVal Time As Integer)

    '    'My.Computer.Audio.Play(My.Resources.infobeep, AudioPlayMode.Background)

    'End Sub
    'Public Shared Sub InfoMessage(Text As String, TextLines As Integer, BackGround As Boolean, IsLoop As Boolean, Optional Time As Integer = 10000)
    '    '' MEssagggio con 4 righe
    '    'xy Size: 430, 140
    '    'xy Pos: 310, 125
    '    Select Case TextLines
    '        Case 1
    '        Case 2
    '        Case 3
    '        Case 4
    '            XPosBG = 310
    '            YPosBG = 125
    '            YSizeBG = 140
    '    End Select
    '    If IsLoop Then
    '        InfoDisplayed = True
    '        If hasInfoBeepPlayed = False Then
    '            'AudioPlayer.Play("infobeep.wav", "infobeep")
    '            hasInfoBeepPlayed = True
    '            infoDeloreanDead = 1
    '        End If
    '    Else
    '        InfoDisplayed = True
    '        Msg("displayed", 5000)

    '        'AudioPlayer.StopAudio("infobeep")
    '        Msg("not anymore displayed", 5000)
    '        InfoDisplayed = False
    '        hasInfoBeepPlayed = False
    '        infoDeloreanDead = 2
    '    End If
    'End Sub
    'Shared Sub InfoMessageDraw_PerFrameDrawing(ByVal sender As Object, ByVal e As GTA.GraphicsEventArgs) Handles Me.PerFrameDrawing
    '    If InfoDisplayed Then
    '        e.Graphics.DrawRectangle(XPosBG, YPosBG, 430, YSizeBG, Color.FromArgb(100, 0, 0, 0))
    '    End If
    'End Sub
    'Sub InfoMessage_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Tick

    'End Sub
    '===========================================================================================================================================================
    Shared Sub AddExplosion(Position As Vector3, ExplosionType As Integer, Radius As Single, Audio As Boolean, Visible As Boolean, CameraShake As Single)
        [Call]("ADD_EXPLOSION", Position.X, Position.Y, Position.Z, ExplosionType, Radius, Audio, Not Visible, CameraShake)
    End Sub
    Shared Sub AddToPreviousBrief(Text As String, Optional Underscore As Boolean = False)
        If Underscore = False Then [Call]("ADD_TO_PREVIOUS_BRIEF", Text) Else [Call]("ADD_TO_PREVIOUS_BRIEF_WITH_UNDERSCORE", Text)
    End Sub
    Shared Sub AllowEmergencyServices(Allow As Boolean)
        [Call]("ALLOW_EMERGENCY_SERVICES", Allow)
    End Sub
    Shared Sub ClearArea(Coordinate As Vector3, Radius As Single, UnknownFlag As Boolean, Optional Peds As Boolean = True, Optional Cars As Boolean = True, Optional Cops As Boolean = True, Optional Objects As Boolean = True)
        If Peds = True AndAlso Cars = True AndAlso Cops = True Then
            [Call]("CLEAR_AREA", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius, UnknownFlag)
        Else
            If Peds Then [Call]("CLEAR_AREA_OF_CHARS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
            If Cars Then [Call]("CLEAR_AREA_OF_CARS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
            If Cops Then [Call]("CLEAR_AREA_OF_COPS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
            If Objects Then [Call]("CLEAR_AREA_OF_OBJECTS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
        End If
    End Sub
    Shared Sub ClearBrief()
        [Call]("CLEAR_BRIEF")
    End Sub
    Shared Sub DisablePauseMenu(Disable As Boolean)
        [Call]("DISABLE_PAUSE_MENU", Disable)
    End Sub
    Shared Sub DisplayHUD(GeneralHUD As Boolean, Optional Radar As Boolean = True, Optional MapBlips As Boolean = True, Optional Cash As Boolean = True, Optional Ammo As Boolean = True, Optional AreaName As Boolean = True)
        [Call]("DISPLAY_HUD", GeneralHUD)
        [Call]("DISPLAY_RADAR", Radar)
        [Call]("DISPLAY_FRONTEND_MAP_BLIPS", MapBlips)
        [Call]("DISPLAY_CASH", Cash)
        [Call]("DISPLAY_AMMO", Ammo)
        [Call]("DISPLAY_AREA_NAME", AreaName)
    End Sub
    Shared Sub DoAutoSave()
        [Call]("DO_AUTO_SAVE")
    End Sub
    Shared Sub DrawCorona(ByVal Position As Vector3, ByVal Radius As Single, ByVal Red As Int16, ByVal Green As Int16, ByVal Blue As Int16, Optional Intensity_0_To_100 As Integer = 100)
        Dim R, G, B As Integer
        Dim Intensity As Double = Intensity_0_To_100 / 100
        R = Red * Intensity
        G = Green * Intensity
        B = Blue * Intensity
        [Call]("DRAW_CORONA", Position.X, Position.Y, Position.Z, Radius, 0, 0.0F, R, G, B)
    End Sub
    Shared Sub DrawLight(ByVal Position As Vector3, ByVal Red As Int16, ByVal Green As Int16, ByVal Blue As Int16, ByVal Intensity As Single, ByVal Range As Single)
        [Call](Of Integer)("DRAW_LIGHT_WITH_RANGE", Position.X, Position.Y, Position.Z, Red, Green, Blue, Intensity, Range)
    End Sub
    Shared Sub EnableFancyWeather(Enable As Boolean)
        [Call]("ENABLE_FANCY_WATER", Enable)
    End Sub
    Shared Sub FireSingleBullet(StartPosition As Vector3, EndPosition As Vector3, Damage As Integer)
        [Call]("FIRE_SINGLE_BULLET", StartPosition.X, StartPosition.Y, StartPosition.Z, EndPosition.X, EndPosition.Y, EndPosition.Z, Damage)
    End Sub
    Shared Function FlashBlip(ByVal Blip As Blip, Optional StopFlash As Boolean = False)
        If StopFlash = False Then
            NativeGeneric.Blip = Blip
            bFlashBlip = True
        End If
        [Call]("FLASH_BLIP", Blip, Not StopFlash)
    End Function
    Shared Sub FlashHUD(Optional Radar As Boolean = False, Optional Route As Boolean = False, Optional WeaponIcon As Boolean = False)
        [Call]("FLASH_RADAR", Radar)
        [Call]("FLASH_ROUTE", Route)
        [Call]("FLASH_WEAPON_ICON", WeaponIcon)
    End Sub
    Shared Sub ForceHighLOD(Enable As Boolean)
        [Call]("FORCE_HIGH_LOD", Enable)
    End Sub
    Shared Function getHashKey(Model As String) As Integer
        Return [Call](Of Integer)("GET_HASH_KEY", Model)
    End Function
    Shared Function isHudPreferenceON() As Boolean
        Return [Call](Of Boolean)("IS_HUD_PREFERENCE_SWITCHED_ON")
    End Function
    Shared Function isHudReticuleComplex() As Boolean
        Return [Call](Of Boolean)("IS_HUD_RETICULE_COMPLEX")
    End Function
    'Shared Function isPayAndSprayActive() As Boolean
    '    Return [Call](Of Boolean)("IS_PAY_N_SPRAY_ACTIVE")
    'End Function
    Shared Function isPauseMenuActive() As Boolean
        Return [Call](Of Boolean)("IS_PAUSE_MENU_ACTIVE")
    End Function
    Shared Function isPlayingMinigame() As Boolean
        Return [Call](Of Boolean)("IS_MINIGAME_IN_PROGRESS")
    End Function
    Shared Sub Msg(ByVal Text As String, Optional Duration As Int32 = 4000, Optional BriefDisplay As Boolean = False)
        [Call]("ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS", BriefDisplay)
        [Call]("PRINT_STRING_WiTH_LITERAL_STRING_NOW", "STRING", Text, Duration, 1)
    End Sub
    Shared Function doesFireExists(Fire As Integer) As Boolean
        Return [Call](Of Boolean)("DOES_SCRIPT_FIRE_EXIST", Fire)
    End Function
    Shared Sub RemoveScriptFire(FireID As Integer)
        If doesFireExists(FireID) Then [Call]("REMOVE_SCRIPT_FIRE", FireID)
    End Sub
    Shared Function StartScriptFire(Position As Vector3, NumOfFireAllowed As Integer, Strength As Integer) As Integer
        Return [Call](Of Integer)("START_SCRIPT_FIRE", Position.X, Position.Y, NumOfFireAllowed, Strength)
    End Function
    Shared Sub SetFreeHealthCare(Free As Boolean)
        [Call]("SET_FREE_HEALTH_CARE", NativePlayer.getPlayerIndex(), Free)
    End Sub
    Shared Sub SetNoResprays(DisableResparys As Boolean)
        [Call]("SET_NO_RESPRAYS", DisableResparys)
    End Sub
    Shared Sub SetMessagesWaiting(MessageIcon As Boolean)
        [Call]("SET_MESSAGES_WAITING", MessageIcon)
    End Sub
    Shared Sub SetRadarZoom(Zoom As Single)
        [Call]("SET_RADAR_ZOOM", Zoom)
    End Sub
    Shared Sub SetRomanMood(Mood As RomanMood)
        [Call]("SET_ROMANS_MOOD", Mood)
    End Sub
    Shared Sub SetTimeOfDay(Hour As Integer, Minute As Integer, Optional ForceTimeDay As Boolean = False)
        If ForceTimeDay = False Then [Call]("SET_TIME_OF_DAY", Hour, Minute) Else [Call]("FORCE_TIME_OF_DAY", Hour, Minute)
    End Sub
    Shared Sub SetHighDOF(Enable As Boolean)
        [Call]("SET_USE_HIGHDOF", Enable)
    End Sub
    Shared Sub SetShadows(ShadowsON As Boolean)
        [Call]("ENABLE_SHADOWS", ShadowsON)
    End Sub
    Shared Sub SwitchRoadsOFF(MinCoordinate As Vector3, MaxCoordinate As Vector3)
        [Call]("SWITCH_ROADS_OFF", MinCoordinate.X, MinCoordinate.Y, MinCoordinate.Z, MaxCoordinate.X, MaxCoordinate.Y, MaxCoordinate.Z)
    End Sub 'Disables boats, but not parked cars
    Shared Sub SwitchRoadsON(MinCoordinate As Vector3, MaxCoordinate As Vector3)
        [Call]("SWITCH_ROADS_BACK_TO_ORIGINAL", MinCoordinate.X, MinCoordinate.Y, MinCoordinate.Z, MaxCoordinate.X, MaxCoordinate.Y, MaxCoordinate.Z)
    End Sub 'Same coordinates used before
    Shared Sub WeatherControl(RandomWeather As Boolean, ForceNow As Boolean, Optional Weather As GTA.Weather = 0, Optional KeepForced As Boolean = False)
        Dim WeatherSet As Weather
        If RandomWeather Then
            Dim RandomNum As New Random
            WeatherSet = RandomNum.Next(9)
            If ForceNow Then [Call]("FORCE_WEATHER_NOW", WeatherSet) Else [Call]("FORCE_WEATHER", WeatherSet)
            If KeepForced = False Then [Call]("RELEASE_WEATHER")
        Else
            If ForceNow Then [Call]("FORCE_WEATHER_NOW", Weather) Else [Call]("FORCE_WEATHER", Weather)
            If KeepForced = False Then [Call]("RELEASE_WEATHER")
        End If
    End Sub
    'REQUEST_IPL
    'REQUEST_INTERIOR_MODELS
    'REQUEST_ADDITIONAL_TEXT 
    'REQUEST_MISSION_AUDIO_BANK ==> string
    'LOAD_WEB_PAGE ==> int (pointer?), string (URL)
    'ADD_HOSPITAL_RESTART
    'ADD_POLICE_RESTART
    'ADD_CAR_TO_MISSION_DELETION_LIST ==> vehicle id
    'ADD_PED_TO_MISSION_DELETION_LIST ==> ped id, bool (Determines whether the ped should be on the mission deletion list)
    'CAN_PLAYER_START_MISSION
    'MISSION_AUDIO_BANK_NO_LONGER_NEEDED
    'REMOVE_IPL_DISCREETLY
    'REMOVE_IPL
End Class