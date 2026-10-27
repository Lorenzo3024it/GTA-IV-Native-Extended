Imports System.Windows.Forms
Imports GTA
Imports GTA.Native.Function

Public NotInheritable Class NativePolice
    Private Shared pointerMaxWantedLevel As New Native.Pointer(GetType(Integer))
    Private Shared pointerWantedLevel As New Native.Pointer(GetType(Integer))
    Private Sub New() : End Sub

    Public Enum Wanted
        ''' <summary> Not wanted.</summary>
        None = 0
        ''' <summary>Normal wanted stars.</summary>
        Active = 1
        ''' <summary>Grayed wanted stars.</summary>
        Hidden = 2
        ''' <summary>Flashing wanted stars.</summary>
        Escaping = 3
        ''' <summary>Escaping controlled by TickHelper.</summary>
        EscapingScripted = 4
    End Enum
    ''' <summary>
    ''' Gets the player's current wanted state.
    ''' Returns EscapingScripted while wanted level is clearing with ControlWantedLevel.
    ''' Otherwise, determines the state using the game's wanted-level
    ''' and wanted-star natives.
    ''' </summary>
    Public Shared ReadOnly Property WantedState As Wanted
        Get
            If Not WantedLevel > 0 Then Return Wanted.None
            If _PendingWantedAction = WantedTickAction.ClearWantedLevel Then Return Wanted.EscapingScripted

            If [Call](Of Boolean)("PLAYER_HAS_GREYED_OUT_STARS", NativePlayer.Index) Then
                Return Wanted.Hidden
            ElseIf [Call](Of Boolean)("PLAYER_HAS_FLASHING_STARS_ABOUT_TO_DROP", NativePlayer.Index) Then
                Return Wanted.Escaping
            Else
                Return Wanted.Active
            End If
        End Get
    End Property
    ''' <summary>Enables or disables police radio while wanted.</summary>
    Public Shared WriteOnly Property ChaseAudio As Boolean
        Set(value As Boolean)
            [Call]("ENABLE_CHASE_AUDIO", value)
        End Set
    End Property
    ''' <summary>
    ''' Controls the player's wanted level, optionally using a fake wanted level
    ''' and controlling police dispatch.
    ''' Real wanted-level changes can follow the game's normal transition
    ''' behavior or be applied immediately with ApplyNow.
    ''' When Level is zero and ApplyNow is false, clearing the actual wanted
    ''' level is delayed by a random interval between 15 and 30 seconds.
    ''' During this interval, WantedState reports EscapingScripted.
    ''' When FakeWantedLevel is true, the fake wanted level can represent
    ''' the current wanted level during a delayed clearing operation.
    ''' When requesting a positive fake level without ApplyNow, activation
    ''' is delayed by a random interval between 10 and 20 seconds.
    ''' Police dispatch is modified according to DontDispatchCops and the
    ''' restoration rules for real wanted-level clearing operations.
    ''' </summary>
    ''' <param name="Level">
    ''' Desired wanted level, between 0 and 6.
    ''' </param>
    ''' <param name="FakeWantedLevel">
    ''' If true, uses the fake wanted-level mechanism instead of leaving
    ''' an actual wanted level active after the transition.
    ''' </param>
    ''' <param name="DontDispatchCops">
    ''' If specified, sets whether police dispatch is disabled.
    ''' If omitted, the existing dispatch configuration is preserved,
    ''' except when a delayed real wanted-level clearing operation requires
    ''' a temporary dispatch override.
    ''' </param>
    ''' <param name="ApplyNow">
    ''' If true, bypasses the custom delays and applies the requested
    ''' wanted-level change immediately.
    ''' </param>
    Public Shared Sub ControlWantedLevel(Level As Integer, Optional FakeWantedLevel As Boolean = False, Optional DontDispatchCops As Boolean? = Nothing, Optional ApplyNow As Boolean = False)

        If Level < 0 OrElse Level > 6 Then Throw New ArgumentOutOfRangeException(NameOf(Level), "Level must be between 0 and 6.")

        Dim playerIndex As Integer = NativePlayer.Index
        CancelPendingWantedAction()

        If Level = 0 Then
            If ApplyNow Then

                If DontDispatchCops.HasValue Then
                    DontDispatchCopsforPlayer = DontDispatchCops.Value
                ElseIf _WasDispatchingCops Then
                    DontDispatchCopsforPlayer = False
                End If

                [Call]("CLEAR_WANTED_LEVEL", playerIndex)
                [Call]("SET_FAKE_WANTED_LEVEL", 0)
                [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", playerIndex)
                _WasDispatchingCops = False
                Return
            End If

            If FakeWantedLevel Then
                Dim currentLevel As Integer = WantedLevel
                currentLevel = Math.Max(0, Math.Min(6, currentLevel))

                If DontDispatchCops.HasValue Then
                    DontDispatchCopsforPlayer = DontDispatchCops.Value
                    _WasDispatchingCops = False
                End If

                [Call]("SET_FAKE_WANTED_LEVEL", currentLevel)

                Dim dispatchRestore As Boolean? = Nothing

                ScheduleWantedAction(Action:=WantedTickAction.ClearWantedLevel, DelaySeconds:=GetWantedRandomDelay(15.0F, 30.0F),
                                     FakeWantedLevel:=currentLevel, DispatchRestore:=dispatchRestore)
                Return
            End If

            Dim restoreDispatch As Boolean
            If DontDispatchCops.HasValue Then
                restoreDispatch = DontDispatchCops.Value
            ElseIf _WasDispatchingCops Then
                restoreDispatch = False
            ElseIf _DontDispatchCopsForPlayerState.HasValue Then
                restoreDispatch = _DontDispatchCopsForPlayerState.Value
            Else
                restoreDispatch = False
            End If

            [Call]("SET_FAKE_WANTED_LEVEL", 0)
            DontDispatchCopsforPlayer = True

            ScheduleWantedAction(Action:=WantedTickAction.ClearWantedLevel, DelaySeconds:=GetWantedRandomDelay(15.0F, 30.0F), FakeWantedLevel:=0, DispatchRestore:=restoreDispatch, TemporarilyForceDispatchOff:=True)
            Return
        End If

        If DontDispatchCops.HasValue Then
            DontDispatchCopsforPlayer = DontDispatchCops.Value

            If DontDispatchCops.Value Then _WasDispatchingCops = True Else _WasDispatchingCops = False
        End If

        If FakeWantedLevel Then
            If ApplyNow Then
                [Call]("CLEAR_WANTED_LEVEL", playerIndex)
                [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", playerIndex)
                [Call]("SET_FAKE_WANTED_LEVEL", Level)
            Else
                ScheduleWantedAction(Action:=WantedTickAction.ApplyFakeWantedLevel, DelaySeconds:=GetWantedRandomDelay(10.0F, 20.0F), FakeWantedLevel:=Level)
            End If

            Return
        End If

        [Call]("SET_FAKE_WANTED_LEVEL", 0)
        [Call]("ALTER_WANTED_LEVEL", playerIndex, Level)
        If ApplyNow Then [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", playerIndex)
    End Sub
    ''' <summary>
    ''' Controls whether police dispatch is disabled for the player.
    ''' </summary>
    Public Shared WriteOnly Property DontDispatchCopsforPlayer As Boolean
        Set(value As Boolean)
            [Call]("DONT_DISPATCH_COPS_FOR_PLAYER", NativePlayer.Index, value)
            _DontDispatchCopsForPlayerState = value
        End Set
    End Property
    ''' <summary>
    ''' Disables police radar blips while wanted.
    ''' </summary>
    Public Shared WriteOnly Property HidePoliceRadarBlips As Boolean
        Set(value As Boolean)
            [Call]("SET_POLICE_RADAR_BLIPS", Not value)
        End Set
    End Property

    Public Shared WriteOnly Property IgnorePlayer As Boolean
        Set(value As Boolean)
            [Call]("SET_POLICE_IGNORE_PLAYER", NativePlayer.Index, value)
        End Set
    End Property
    '''' <summary>
    '''' Returns true if player's wanted level is > than Level indicated.
    '''' </summary>
    'Public Shared Function IsWantedLevelGreater(Level As Integer) As Boolean
    '    Return [Call](Of Boolean)("IS_WANTED_LEVEL_GREATER", NativePlayer.Index, Level)
    'End Function
    ''' <summary>
    ''' Keeps the player free of wanted levels by clearing the actual
    ''' and fake wanted levels through TickHelper.
    ''' While a ControlWantedLevel operation is pending, automatic clearing
    ''' is temporarily suspended.
    ''' </summary>
    Public Shared Property NeverWanted As Boolean
        Get
            Return _forceNeverWanted
        End Get
        Set(value As Boolean)
            If value Then
                TickHelper.Add_Internal(AddressOf ForceNeverWanted)
            Else
                TickHelper.Remove_Internal(AddressOf ForceNeverWanted)
            End If
            _forceNeverWanted = value
        End Set
    End Property

    'Public Shared Property MaxWantedLevel As Integer
    '    Get
    '        [Call]("GET_MAX_WANTED_LEVEL", pointerMaxWantedLevel)
    '        Return pointerMaxWantedLevel.Value
    '    End Get
    '    Set(value As Integer)
    '        [Call]("SET_MAX_WANTED_LEVEL", value)
    '    End Set
    'End Property
    'Public Shared ReadOnly Property AreWantedStarsFlashing As Boolean
    '    Get
    '        Return [Call](Of Boolean)("PLAYER_HAS_FLASHING_STARS_ABOUT_TO_DROP", NativePlayer.Index)
    '    End Get
    'End Property

    'Public Shared ReadOnly Property AreWantedStarsGrayed As Boolean
    '    Get
    '        Return [Call](Of Boolean)("PLAYER_HAS_GREYED_OUT_STARS", NativePlayer.Index)
    '    End Get
    'End Property

    ''' <summary>
    ''' Gets or sets whether the game creates random police officers (native).
    ''' If true, cops on foot and in cars won't spawn without wanted level.
    ''' Otherwise if player has wanted level cops on foot won't spawn, cars and helis will spawn normally.
    ''' </summary>
    Public Shared Property RandomCops As Boolean
        Get
            Return [Call](Of Boolean)("GET_CREATE_RANDOM_COPS")
        End Get
        Set(value As Boolean)
            [Call]("SET_CREATE_RANDOM_COPS", value)
        End Set
    End Property

    Public Shared WriteOnly Property ScannerAudio As Boolean
        Set(value As Boolean)
            If value = True Then [Call]("ENABLE_POLICE_SCANNER") Else [Call]("DISABLE_POLICE_SCANNER")
        End Set
    End Property
    ''' <summary>
    ''' Sets whether the specified ped is considered wanted by the police.
    ''' </summary>
    Public Shared Sub SetPedWanted(Ped As Ped, Optional Wanted As Boolean = True)
        [Call]("SET_CHAR_WANTED_BY_POLICE", Ped, Wanted)
    End Sub

    Public Shared Sub SetZoneNoCops(ZoneHash As String, Optional NoCopsZone As Boolean = True)
        NativeWorld.Zone.DisableCops(ZoneHash, NoCopsZone)
    End Sub
    ''' <summary>
    ''' Triggers a crime report audio for a specified position.
    ''' Call once and wait it ended before trigger a new one, because they will be natively added
    ''' to a queue and it can't be clear.
    ''' </summary>
    ''' <param name="Type">Crime type from 'Crimes' enum.</param>
    Public Shared Sub TriggerCrimeReport(Position As Vector3, Type As Crime)
        If Type < 0 OrElse Type > 6 Then
            Tools.WriteLog("Error in NativePolice.TriggerCrimeReport: " & Type & " crime type is not valid.")
            Return
        End If
        [Call]("TRIGGER_VIGILANTE_CRIME", Type, Position.X, Position.Y, Position.Z)
    End Sub
    ''' <summary>
    ''' Triggers a mission crime report audio.
    ''' Call once and wait it ended before trigger a new one, because they will be natively added
    ''' to a queue and it can't be clear.
    ''' </summary>
    Public Shared Sub TriggerScriptedReport(Report As ScriptedReport)
        Dim ReportString As String
        Select Case Report
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
                Tools.WriteLog("Errpr in NativePolice.TriggerScriptedReport: " & Report & " report ID is not valid.")
                Return
        End Select
        [Call]("TRIGGER_POLICE_REPORT", ReportString)
    End Sub
    ''' <summary>
    ''' Gets or sets the player's actual wanted level.
    ''' Setting this property immediately applies the requested level
    ''' and resets the fake wanted level.
    ''' Any pending ControlWantedLevel operation is cancelled first.
    ''' </summary>
    Public Shared Property WantedLevel As Integer
        Get
            Dim p As New Native.Pointer(GetType(Integer))
            [Call]("STORE_WANTED_LEVEL", NativePlayer.Index, p)
            Return p.Value
        End Get
        Set(value As Integer)
            CancelPendingWantedAction()
            Dim playerIndex As Integer = NativePlayer.Index
            [Call]("SET_FAKE_WANTED_LEVEL", 0)
            If value = 0 Then
                [Call]("CLEAR_WANTED_LEVEL", playerIndex)
            Else
                [Call]("ALTER_WANTED_LEVEL", playerIndex, Tools.ClampInteger(value, 1, 6))
            End If
            [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", playerIndex)
        End Set
    End Property
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared WriteOnly Property WantedMultiplier As Single
        Set(value As Single)
            [Call]("SET_WANTED_MULTIPLIER", value)
        End Set
    End Property
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared Sub WillTrackCar(Car As Vehicle, Optional Enabled As Boolean = True)
        [Call]("SET_POLICE_FOCUS_WILL_TRACK_CAR", Car, Enabled)
    End Sub
    Public Enum Crime
        ResistingArrest = 0
        CarTheft = 1
        OnFootShooting = 2
        DriveByShooting = 4
        DrugDeal = 5
        GangViolence = 6
    End Enum

    Public Enum ScriptedReport
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

#Region "Internals"
    Private Enum WantedTickAction
        None = 0
        ClearWantedLevel = 1
        ApplyFakeWantedLevel = 2
    End Enum

    Private Shared _PendingWantedAction As WantedTickAction = WantedTickAction.None
    Private Shared _PendingWantedActionDue As DateTime =    DateTime.MinValue
    '---
    Private Shared _PendingWantedLevel As Integer = 0
    Private Shared _PendingFakeWantedLevel As Integer = 0
    '---
    Private Shared _PendingDispatchRestore As Boolean? = Nothing
    '---
    Private Shared _PendingTemporarilyForcedDispatch As Boolean = False
    '---
    Private Shared _DontDispatchCopsForPlayerState As Boolean? = Nothing
    '---
    Private Shared _WasDispatchingCops As Boolean = False
    '---
    Private Shared ReadOnly _WantedTickHandler As Action = AddressOf ProcessPendingWantedAction
    '---
    Private Shared _forceNeverWanted As Boolean = False
    '---------------------------------------------------------------------------------------------------
    Private Shared Function GetWantedRandomDelay(MinimumSeconds As Single, MaximumSeconds As Single) As Single
        Dim delay As Single = NativeGeneric.GetRandomFloat(MinimumSeconds, MaximumSeconds)

        If delay < MinimumSeconds Then
            delay = MinimumSeconds
        ElseIf delay > MaximumSeconds Then
            delay = MaximumSeconds
        End If

        Return delay
    End Function
    Private Shared Sub ScheduleWantedAction(Action As WantedTickAction, DelaySeconds As Single, Optional WantedLevel As Integer = 0, Optional FakeWantedLevel As Integer = 0,
                                            Optional DispatchRestore As Boolean? = Nothing, Optional TemporarilyForceDispatchOff As Boolean = False)

        _PendingWantedAction = Action
        _PendingWantedActionDue = DateTime.UtcNow.AddSeconds(DelaySeconds)
        _PendingWantedLevel = WantedLevel
        _PendingFakeWantedLevel = FakeWantedLevel
        _PendingDispatchRestore = DispatchRestore
        _PendingTemporarilyForcedDispatch = TemporarilyForceDispatchOff
        TickHelper.Add_Internal(_WantedTickHandler)
    End Sub
    Private Shared Sub CancelPendingWantedAction()
        If _PendingWantedAction = WantedTickAction.None Then Return

        If _PendingTemporarilyForcedDispatch AndAlso _PendingDispatchRestore.HasValue Then
            DontDispatchCopsforPlayer = _PendingDispatchRestore.Value
        End If

        _PendingWantedAction = WantedTickAction.None
        _PendingWantedActionDue = DateTime.MinValue
        _PendingWantedLevel = 0
        _PendingFakeWantedLevel = 0
        _PendingDispatchRestore = Nothing
        _PendingTemporarilyForcedDispatch = False
        TickHelper.Remove_Internal(_WantedTickHandler)
    End Sub
    Private Shared Sub ProcessPendingWantedAction()
        If _PendingWantedAction = WantedTickAction.None Then
            TickHelper.Remove_Internal(_WantedTickHandler)
            Return
        End If
        If DateTime.UtcNow < _PendingWantedActionDue Then
            Return
        End If

        Dim action As WantedTickAction = _PendingWantedAction
        Dim fakeLevel As Integer = _PendingFakeWantedLevel
        Dim dispatchRestore As Boolean? = _PendingDispatchRestore
        _PendingWantedAction = WantedTickAction.None
        _PendingWantedActionDue = DateTime.MinValue
        _PendingWantedLevel = 0
        _PendingFakeWantedLevel = 0
        _PendingDispatchRestore = Nothing
        _PendingTemporarilyForcedDispatch = False
        TickHelper.Remove_Internal(_WantedTickHandler)
        Dim playerIndex As Integer = NativePlayer.Index

        Select Case action
            Case WantedTickAction.ClearWantedLevel
                [Call]("CLEAR_WANTED_LEVEL", playerIndex)
                [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", playerIndex)
                [Call]("SET_FAKE_WANTED_LEVEL", fakeLevel)

                If dispatchRestore.HasValue Then
                    DontDispatchCopsforPlayer = dispatchRestore.Value
                    _WasDispatchingCops = False
                End If


            Case WantedTickAction.ApplyFakeWantedLevel
                [Call]("CLEAR_WANTED_LEVEL", playerIndex)
                [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", playerIndex)
                [Call]("SET_FAKE_WANTED_LEVEL", fakeLevel)
        End Select
    End Sub
    Private Shared Sub ForceNeverWanted()
        If Not TickHelper.ContainsHandler_Internal(_WantedTickHandler) Then
            Dim playerIndex As Integer = NativePlayer.Index
            If [Call](Of Boolean)("IS_WANTED_LEVEL_GREATER", playerIndex, 0) Then
                [Call]("CLEAR_WANTED_LEVEL", playerIndex)
                [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", playerIndex)
            End If
        End If
    End Sub
#End Region
End Class