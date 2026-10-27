Imports System.Reflection
Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Imports NativeExtended.NativeHud
Imports NativeExtended.NativeScript

Public NotInheritable Class NativePlayer
    Private Sub New()
    End Sub
    Private Shared ReadOnly pointerPlayerChar As New Native.Pointer(GetType(Ped))
    Private Shared ReadOnly pointerMoney As New Native.Pointer(GetType(Integer))

    Public Enum PlayerMoods
        Normal = 0
        Happy = 1
        PissedOff = 2
        Angry = 3
        PoliceChase = 4
        Thinking = 5
        Suprised = 6
        Chewing = 7
        HeavyBreath = 8
        LookAround = 9
    End Enum

    Public Shared Sub AirDragMultiplierForCars(Multiplier As Double)
        [Call]("FORCE_AIR_DRAG_MULT_FOR_PLAYERS_CAR", Index, Multiplier)
    End Sub

    Public Shared WriteOnly Property CanBeHassledByGangs As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_CAN_BE_HASSLED_BY_GANGS", Index, value)
        End Set
    End Property

    Public Shared ReadOnly Property CanControlCharacter As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_CONTROL_ON", Index)
        End Get
    End Property

    Public Shared WriteOnly Property CanControlRagdoll As Boolean
        Set(value As Boolean)
            [Call]("GIVE_PLAYER_RAGDOLL_CONTROL", Index, value)
        End Set
    End Property

    Public Shared WriteOnly Property CanDoDriveBy As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_CAN_DO_DRIVE_BY", Index, value)
        End Set
    End Property

    Public Shared WriteOnly Property CanLockOn As Boolean
        Set(value As Boolean)
            [Call]("DISABLE_PLAYER_LOCKON", Index, Not value)
        End Set
    End Property

    Public Shared WriteOnly Property CanPickupObjects As Boolean
        Set(value As Boolean)
            [Call]("ALLOW_PLAYER_TO_CARRY_NON_MISSION_OBJECTS", Index, value)
        End Set
    End Property

    Public Shared WriteOnly Property CanSprint As Boolean
        Set(value As Boolean)
            [Call]("DISABLE_PLAYER_SPRINT", Index, Not value)
        End Set
    End Property

    Public Shared WriteOnly Property CanUseCover As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_CAN_USE_COVER", Index, value)
        End Set
    End Property

    Public Shared ReadOnly Property Character As Ped
        Get
            [Call]("GET_PLAYER_CHAR", Index, pointerPlayerChar)
            Return pointerPlayerChar.Value
        End Get
    End Property

    Public Shared Sub ClearHasDamaged(Optional Peds As Boolean = False, Optional Vehicles As Boolean = False)
        If Peds Then [Call]("CLEAR_PLAYER_HAS_DAMAGED_AT_LEAST_ONE_PED", Index)
        If Vehicles Then [Call]("CLEAR_PLAYER_HAS_DAMAGED_AT_LEAST_ONE_VEHICLE", Index)
    End Sub

    Public Shared WriteOnly Property FastReload As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_FAST_RELOAD", Index, value)
        End Set
    End Property

    ''' <summary>
    ''' Returns current player's movement type checking current
    ''' "move_player" animation.
    ''' </summary>
    Public Shared Function GetMovementType() As MovementType
        'If NativePed.Animations.haveAnimsLoaded("move_player") Then NativePed.Animations.Request("move_player")

        If Character Is Nothing OrElse Not Character.Exists Then
            Return MovementType.Idle
        End If

        Dim animFields = GetType(MoveAnims).GetFields(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static)

        For Each f In animFields
            Dim animName As String = TryCast(f.GetValue(Nothing), String)
            If String.IsNullOrEmpty(animName) Then Continue For

            If IsPlayingAnim_MoveType(Character, animName) Then
                Dim varName As String = f.Name.ToLower()

                ' Check start/stop for walking, running, sprinting
                If varName.StartsWith("wstart") OrElse varName.StartsWith("wstop") Then
                    Return MovementType.Walk
                ElseIf varName.StartsWith("rstart") OrElse varName.StartsWith("rstop") Then
                    Return MovementType.Run
                ElseIf varName.StartsWith("sstart") OrElse varName.StartsWith("sstop") Then
                    Return MovementType.Sprint
                End If

                ' Check other general types
                If varName.StartsWith("idle") Then
                    Return MovementType.Idle
                ElseIf varName.StartsWith("walk") Then
                    Return MovementType.Walk
                ElseIf varName.StartsWith("run") Then
                    Return MovementType.Run
                ElseIf varName.StartsWith("sprint") Then
                    Return MovementType.Sprint
                    '  ElseIf varName.StartsWith("turn") OrElse varName.Contains("turn") Then
                    '   Return MoveType.Turning
                End If

                ' fallback
                Return MovementType.Idle
            End If
        Next

        ' Se nessuna animazione trovata, assumiamo Idle
        Return MovementType.Idle
    End Function

    Public Shared Function GetTrainPlayerWouldEnter() As NativeTrain
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
        [Call]("GET_TRAIN_PLAYER_WOULD_ENTER", Index, pointer)
        If pointer.Value IsNot Nothing Then
            Return pointer.Value
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function HasCollectedPickup(Pickup As Pickup) As Boolean
        Return [Call](Of Boolean)("HAS_PLAYER_COLLECTED_PICKUP", Index, Pickup)
    End Function 'testare
    Public Shared Function HasDamaged(Optional Peds As Boolean = True, Optional Vehicles As Boolean = True) As Boolean
        If Peds AndAlso [Call](Of Boolean)("HAS_PLAYER_DAMAGED_AT_LEAST_ONE_PED") Then Return True
        If Vehicles AndAlso [Call](Of Boolean)("HAS_PLAYER_DAMAGED_AT_LEAST_ONE_VEHICLE") Then Return True
        Return False
    End Function

    Private Shared _cachedID As Integer = -1
    Private Shared _cachedIndex As Integer = -1

    Public Shared ReadOnly Property Index As Integer
        Get
            If _cachedID = -1 Then _cachedID = [Call](Of Integer)("GET_PLAYER_ID")
            If _cachedIndex = -1 Then _cachedIndex = [Call](Of Integer)("CONVERT_INT_TO_PLAYERINDEX", _cachedID)
            Return _cachedIndex
        End Get
    End Property

    Public Shared WriteOnly Property IgnoredByPeds As Boolean
        Set(value As Boolean)
            [Call]("SET_EVERYONE_IGNORE_PLAYER", Index, value)
        End Set
    End Property

    Public Shared WriteOnly Property IgnoredByPolice As Boolean
        Set(value As Boolean)
            [Call]("SET_POLICE_IGNORE_PLAYER", Index, value)
        End Set
    End Property

    Public Shared ReadOnly Property IsPissedOff As Boolean
        Get
            Return [Call](Of Boolean)("PLAYER_IS_PISSED_OFF")
        End Get
    End Property
    ''' <summary>
    ''' Returns True while the game reports that the player is being arrested.
    ''' The arrest may still be interrupted.
    ''' </summary>
    Public Shared ReadOnly Property IsBeginArrested As Boolean
        Get
            EnsureArrestMonitoring()
            Return _IsBeingArrestedCached
        End Get
    End Property

    ''' <summary>
    ''' Returns True once the arrest state has confirmed (non-native).
    ''' Remains True during the transition to respawn.
    ''' </summary>
    Public Shared ReadOnly Property IsArrestConfirmed As Boolean
        Get
            EnsureArrestMonitoring()
            Return _IsArrestConfirmedCached
        End Get
    End Property
    ''' <summary>
    ''' Returns True if the player respawned after an arrest
    ''' less than 10 seconds ago.
    ''' </summary>
    Public Shared ReadOnly Property IsRespawnedAfterArrest As Boolean
        Get
            Dim Elapsed As Integer = TimeSinceLastArrest

            Return Elapsed >= 0 AndAlso Elapsed < RecentRespawnWindowMs
        End Get
    End Property

    ''' <summary>
    ''' Returns True if the player respawned after death
    ''' less than 10 seconds ago.
    ''' </summary>
    Public Shared ReadOnly Property IsRespawnedAfterDeath As Boolean
        Get
            Dim Elapsed As Integer = TimeSinceLastDeath

            Return Elapsed >= 0 AndAlso Elapsed < RecentRespawnWindowMs
        End Get
    End Property

    Public Shared ReadOnly Property IsClimbing As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_CLIMBING", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsDead As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_DEAD", Index)
        End Get
    End Property

    Public Shared Function IsFreeAimingAtPed(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_PLAYER_FREE_AIMING_AT_CHAR", Index, Ped)
    End Function

    Public Shared ReadOnly Property IsFreeForAmbientTask As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_FREE_FOR_AMBIENT_TASK", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsPerformingStoppie As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_PERFORMING_STOPPIE", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsPerformingWheelie As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_PERFORMING_WHEELIE", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsPlayingMinigame As Boolean
        Get
            Return [Call](Of Boolean)("IS_MINIGAME_IN_PROGRESS")
        End Get
    End Property

    Public Shared ReadOnly Property IsPressingHorn As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_PRESSING_HORN", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsInInterior As Boolean
        Get
            Return [Call](Of Boolean)("IS_INTERIOR_SCENE", Index)
        End Get
    End Property
    Public Shared ReadOnly Property IsOnMission As Boolean
        Get
            Return [Call](Of Boolean)("GET_MISSION_FLAG")
        End Get
    End Property
    Public Shared ReadOnly Property IsPlaying As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_PLAYING", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsReadyForCutscene As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_READY_FOR_CUTSCENE")
        End Get
    End Property

    Public Shared ReadOnly Property IsTargettingAnything As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_ANYTHING", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsTargettingChar As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_CHAR", Index)
        End Get
    End Property

    Public Shared ReadOnly Property IsTargettingObject As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_OBJECT", Index)
        End Get
    End Property

    Public Shared WriteOnly Property KeepsWeaponsWhenRespawned As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_KEEPS_WEAPONS_WHEN_RESPAWNED", value)
        End Set
    End Property

    Public Shared WriteOnly Property MaxArmour As Integer
        Set(value As Integer)
            [Call]("INCREASE_PLAYER_MAX_ARMOUR", Index, value)
        End Set
    End Property

    Public Shared WriteOnly Property MaxHealth As Integer
        Set(value As Integer)
            [Call]("INCREASE_PLAYER_MAX_HEALTH", Index, value)
        End Set
    End Property
    Public Shared Property Money As Integer
        Get
            [Call]("STORE_SCORE", NativePlayer.Index, pointerMoney)
            Return pointerMoney.Value
        End Get
        Set(value As Integer)
            Dim CurrMoney As Integer = Money
            Dim Difference As Integer = value - CurrMoney
            If Difference <> 0 Then
                [Call]("ADD_SCORE", NativePlayer.Index, value)
            End If
        End Set
    End Property


    Public Shared WriteOnly Property NeverGetsTired As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_NEVER_GETS_TIRED", Index, value)
        End Set
    End Property

    Public Shared ReadOnly Property NumberOfPedsKilled As Integer
        Get
            Return [Call](Of Integer)("GET_TOTAL_NUMBER_OF_PEDS_KILLED_BY_PLAYER", Index)
        End Get
    End Property

    Public Shared Sub SetMayOnlyEnterThisVehicle(Car As Vehicle, Enable As Boolean)
        Dim PlayerIndex = Index
        If Enable Then [Call]("SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE", PlayerIndex, Car) Else [Call]("SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE", PlayerIndex, 0)
    End Sub
    ''' <summary>
    ''' Sets player mood using facial anims and dedicated native function.
    ''' </summary>
    ''' <param name="Mood"></param>
    ''' <param name="Duration">If -1</param>
    Public Shared Sub SetMood(Mood As PlayerMoods, Optional PissedOff As Boolean? = Nothing, Optional Duration As Integer = -1)
        NativePed.Animation.Facial.Player.Request()
        Dim MoodString As String = ""
        Dim MoodDuration As Integer
        If Duration = -1 Then MoodDuration = 150000 'same value as SCO scripts
        If Duration > 0 Then MoodDuration = Duration / 1000
        Select Case Mood
            Case 0
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                If PissedOff.HasValue AndAlso PissedOff.Value = True Then
                    Tools.WriteLog("Info for NativePlayer.SetMood: PissedOff = True is not valid for 'Normal' mood.")
                End If
                If Duration > 0 Then
                    Tools.WriteLog("Info for NativePlayer.SetMood: duration cannot be set for 'Normal' mood.")
                End If
            Case 1
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                MoodString = "plyr_mood_happy"
            Case 2
                'MoodString = "plyr_angry_loop"
                [Call]("SET_PLAYER_MOOD_PISSED_OFF", Index, MoodDuration)
            Case 3
                [Call]("SET_PLAYER_MOOD_PISSED_OFF", Index, MoodDuration)
                MoodString = "gest_angry_loop"
            Case 4
                [Call]("SET_PLAYER_MOOD_PISSED_OFF", Index, MoodDuration)
                MoodString = "police_chase"
            Case 5
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                MoodString = "gest_think_loop"
            Case 6
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                MoodString = "gest_surprised_loop"
            Case 7
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                MoodString = "chew"
            Case 8
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                MoodString = "heavybreath"
            Case 9
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                MoodString = "lookaround"
            Case Else
                [Call]("SET_PLAYER_MOOD_NORMAL", Index)
                MoodString = ""
        End Select
        If Mood <> 0 AndAlso Mood <> 2 AndAlso Mood <= 9 AndAlso Mood > 0 Then
            'NativePed.Animations.Facials.Play(getPlayerCharacter, MoodString, -1, True)
            [Call]("TASK_PLAY_ANIM_FACIAL", Character, MoodString, "facials@m_hi", 1.0, 1, 0, Duration)
        End If
    End Sub
    ''' <summary>
    ''' CanControlCamera seems not working sometimes.
    ''' </summary>
    ''' <param name="CanControlCamera"></param>
    Public Shared Sub SetControl(CanControlCharacter As Boolean, Optional CanControlCamera As Boolean = True)
        [Call]("SET_PLAYER_CONTROL", Index, CanControlCharacter)
        [Call]("SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS", Not CanControlCamera)
    End Sub
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared WriteOnly Property SuppressFadeInAfterDeathArrest As Boolean
        Set(value As Boolean)
            [Call]("SUPPRESS_FADE_IN_AFTER_DEATH_ARREST", value)
        End Set
    End Property

    Public Shared ReadOnly Property TimeSinceLastArrest As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_LAST_ARREST", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceLastDeath As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_LAST_DEATH", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceDroveAgainstTraffic As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_DROVE_AGAINST_TRAFFIC", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceDroveOnPavement As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_DROVE_ON_PAVEMENT", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceRanLight As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_RAN_LIGHT", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceHitBuilding As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_BUILDING", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceHitPed As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_PED", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceHitObject As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_OBJECT", Index)
        End Get
    End Property

    Public Shared ReadOnly Property TimeSinceHitVehicle As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_VEHICLE", Index)
        End Get
    End Property

    'IS_PLAYER_IN_POSITION_FOR_CONVERSATION
    'IS_PLAYER_IN_REMOTE_MODE
    'IS_PLAYER_IN_SHORTCUT_TAXI
    'IS_PLAYER_LOGGING_IN_NP
    'IS_PLAYER_SCRIPT_CONTROL_ON
    'IS_PLAYER_SIGNED_IN_LOCALLY
    'PLAYER_WANTS_TO_JOIN_NETWORK_GAME
    'REGISTER_PLAYER_RESPAWN_COORDS
    'SET_HAS_BEEN_OWNED_BY_PLAYER
    'SET_PLAYER_CAN_BE_HASSLED_BY_GANGS
    'SET_PLAYER_CAN_DROP_WEAPONS_IN_CAR
    'SET_PLAYER_COLOUR
    'SET_PLAYER_CONTROL
    'SET_PLAYER_CONTROL_ADVANCED
    'SET_PLAYER_CONTROL_FOR_AMBIENT_SCRIPT
    'SET_PLAYER_CONTROL_FOR_NETWORK
    'SET_PLAYER_CONTROL_FOR_TEXT_CHAT
    'SET_PLAYER_CONTROL_ON_IN_MISSION_CLEANUP
    'SET_PLAYER_GROUP_RECRUITMENT
    'SET_PLAYER_GROUP_TO_FOLLOW_ALWAYS
    'SET_PLAYER_GROUP_TO_FOLLOW_NEVER
    'SET_PLAYER_ICON_COLOUR
    'SET_PLAYER_INVINCIBLE
    'SET_PLAYER_INVISIBLE_TO_AI
    'SET_PLAYER_IS_IN_STADIUM
    'SET_PLAYER_PLAYER_TARGETTING
    'SET_PLAYER_TEAM
    'SET_PLAYERS_CAN_BE_IN_SEPARATE_CARS
    'SET_PLAYERS_DROP_MONEY_IN_NETWORK_GAME
    'SET_PLAYERSETTINGS_MODEL_VARIATIONS_CHOICE
    Public Enum MovementType
        Idle = 0
        Walk = 2
        Run = 3
        Sprint = 4
    End Enum

    Public NotInheritable Class Phone
        Private Sub New()
        End Sub
        Private Shared pointerPhoneRenderID As New Native.Pointer(GetType(Integer))
        Private Shared ReadOnly Property CanUseCamera As Boolean
            Get
                If Not IsPhoneEnabled() Then Return False

                Select Case State
                    Case PhoneState.Idle, PhoneState.Menu
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property
        ''' <summary>
        ''' Gets or sets whether the mobile phone system is enabled.
        ''' Disabling the phone stops its native SCO scripts.
        ''' </summary>
        Public Shared Property Enabled As Boolean
            Get
                Return IsPhoneEnabled()
            End Get

            Set(value As Boolean)
                If value = IsPhoneEnabled() Then Return

                'Enabling
                If value Then
                    NativeScript.Start(IVScripts.SpCellphone)
                    Return
                End If

                '-------------
                'Disabling 
                If NativeScript.IsRunning(IVScripts.SpCellphoneNetwork) Then
                    NativeScript.Stop(IVScripts.SpCellphoneNetwork)
                    [Call]("DESTROY_MOBILE_PHONE")
                    Return
                End If

                Dim destroyphone As Boolean = False
                Dim clearTasks As Boolean = False

                NativeScript.Stop(IVScripts.SpCellphone)

                If NativeScript.IsRunning(IVScripts.SpCellphoneCalling) Then
                    NativeScript.Stop(IVScripts.SpCellphoneCalling)
                    destroyphone = True
                    clearTasks = True
                End If

                If NativeScript.IsRunning(IVScripts.SpCellphoneEndCall) Then
                    NativeScript.Stop(IVScripts.SpCellphoneEndCall)
                    destroyphone = True
                    clearTasks = True
                End If

                If NativeScript.IsRunning(IVScripts.SpCellphoneMain) Then
                    NativeScript.Stop(IVScripts.SpCellphoneMain)
                    destroyphone = True
                End If

                If NativeScript.IsRunning(IVScripts.SpCellphoneDebug) Then
                    NativeScript.Stop(IVScripts.SpCellphoneDebug)
                End If

                If destroyphone Then [Call]("DESTROY_MOBILE_PHONE")

                If clearTasks Then NativePed.Task.ClearAll(NativePlayer.Character, True)
            End Set
        End Property
        ''' <summary>
        ''' Forces phone close terminating many native SCO scripts.
        ''' </summary>
        Public Shared Sub ForceClose()
            Dim clearTasks As Boolean = False
            Dim destroyphone As Boolean = False

            If NativeScript.IsRunning(IVScripts.SpCellphoneCalling) Then
                NativeScript.Stop(IVScripts.SpCellphoneCalling)
                destroyphone = True
                clearTasks = True
            End If

            If NativeScript.IsRunning(IVScripts.SpCellphoneEndCall) Then
                NativeScript.Stop(IVScripts.SpCellphoneEndCall)
                destroyphone = True
                clearTasks = True
            End If

            If NativeScript.IsRunning(IVScripts.SpCellphoneMain) Then
                NativeScript.Stop(IVScripts.SpCellphoneMain)
                destroyphone = True
            End If

            If NativeScript.IsRunning(IVScripts.SpCellphoneNetwork) Then
                NativeScript.Stop(IVScripts.SpCellphoneNetwork)
            End If

            If destroyphone Then [Call]("DESTROY_MOBILE_PHONE")
            If clearTasks Then NativePed.Task.ClearAll(NativePlayer.Character, True)
        End Sub
        ''' <summary>
        ''' Checks if player's mobile phone UI is currently visible on screen.
        ''' </summary>
        Public Shared ReadOnly Property IsOnScreen As Boolean
            Get
                ' --- Single Player
                If NativeScript.IsRunning(IVScripts.SpCellphoneCalling) Then Return True
                If NativeScript.IsRunning(IVScripts.SpCellphoneMain) Then Return True
                If NativeScript.IsRunning(IVScripts.SpCellphoneNetwork) Then Return True
                Return False
            End Get
        End Property

        Public Shared WriteOnly Property MessagesWaiting As Boolean
            Set(value As Boolean)
                [Call]("SET_MESSAGES_WAITING", value)
            End Set
        End Property
        ''' <summary>
        ''' Seems returning always 1. Uses GET_MOBILE_PHONE_RENDER_ID native.
        ''' </summary>
        Public Shared ReadOnly Property RenderID As Integer
            Get
                [Call]("GET_MOBILE_PHONE_RENDER_ID", pointerPhoneRenderID)
                Return pointerPhoneRenderID.Value
            End Get
        End Property

        Public Shared Sub RemoveHudItem()
            [Call]("SET_PHONE_HUD_ITEM", 0, "", -1)
        End Sub

        Public Shared Sub SetHudItem(Icon As PhoneHudItem, NumberInFrontOfString As Integer, Optional GXT_Text As String = "")
            [Call]("SET_PHONE_HUD_ITEM", CInt(Icon), GXT_Text, NumberInFrontOfString)
        End Sub

        Public Shared WriteOnly Property SleepMode As Boolean
            Set(value As Boolean)
                [Call]("SET_SLEEP_MODE_ACTIVE", value)
            End Set
        End Property

        Public Shared Sub StartCall()
            [Call]("START_MOBILE_PHONE_CALLING")
        End Sub

        Public Shared Sub StartRinging(Optional RingtoneID As Ringtones = Ringtones.Standard)
            [Call]("START_CUSTOM_MOBILE_PHONE_RINGING", CInt(RingtoneID))
        End Sub

        Public Shared Sub StopRinging()
            [Call]("STOP_MOBILE_PHONE_RINGING")
        End Sub
        ''' <summary>
        ''' Returns phone states regarding to SCO script currently running.
        ''' </summary>
        Public Shared ReadOnly Property State As PhoneState
            Get
                'in ordine di priorità
                'Manager
                If Not NativeScript.IsRunning(IVScripts.SpCellphone) Then Return PhoneState.Disabled

                'Fine chiamata
                If NativeScript.IsRunning(IVScripts.SpCellphoneEndCall) Then Return PhoneState.EndCall

                'Chiamata
                If NativeScript.IsRunning(IVScripts.SpCellphoneCalling) Then
                    If [Call](Of Boolean)("IS_MOBILE_PHONE_CALL_ONGOING") Then
                        Return PhoneState.CallOnGoing
                    Else
                        Return PhoneState.Calling
                    End If
                End If

                'Menu
                If NativeScript.IsRunning(IVScripts.SpCellphoneMain) Then Return PhoneState.Menu

                'In tasca
                Return PhoneState.Idle

                'Telefono iniziale
                If NativeScript.IsRunning(IVScripts.SpCellphoneNetwork) Then Return PhoneState.InitialPhone
            End Get
        End Property

        Public Enum PhoneHudItem
            TextMessage = 1
            NewContact = 2
            AppointmentRing = 3
            NewAppointment = 4
            MissionReplay = 5
            PhotoSent = 6
            PhoneDisabled = 7
            RespectGained = 8
            RespectLost = 9
            InviteReceived = 10
        End Enum
        ''' <summary>
        ''' All phone states regarding to SCO script running.
        ''' </summary>
        Public Enum PhoneState
            Disabled = -2
            InitialPhone = -1
            Idle = 0
            Menu = 1
            Calling = 2
            CallOnGoing = 3
            EndCall = 4
        End Enum
        ''' <summary>
        ''' All available phone ringtones.
        ''' </summary>
        Public Enum Ringtones
            Standard = 0
            Ringing1 = 1
            Ringing2 = 2
            Countryside = 3
            Fox = 4
            HighSeas = 5
            Malfunction = 6
            Pager = 7
            Spy = 8
            Teeker = 9
            Text = 10
            CoolRoom = 11
            KatjasWaltz = 12
            Laidback = 13
            Solo = 14
            SwingIt = 15
            _109 = 16
            Bassmatic = 17
            CreditCheck = 18
            Drive = 19
            FunkInTime = 20
            GetDown = 21
            IntoSomething = 22
            MineUntilMonday = 23
            TakeThePain = 24
            TheOneForMe = 25
            Tonight = 26
            Beheading = 27
            Champagne = 28
            Diamonds = 29
            FlatLine = 30
            Hooker = 31
            Jet = 32
            Lesbians = 33
            MoneyCounter = 34
            OldBitch = 35
            StThomas = 37
            DragonBrain = 60
            ScienceOfCrime = 61
        End Enum
        ''' <summary>
        ''' Not fully tested.
        ''' </summary>
        Public NotInheritable Class Camera
            Private Sub New() : End Sub
            ''' <param name="Unknown">First parameter of CELL_CAM_ACTIVATE native. If nothing, it will be set the "Activate" value.</param>
            ''' <param name="IgnorePhoneState">Camera will be activated even if mobile phone is disabled or in any call state (non native).</param>
            Public Shared Sub Activate(Activate As Boolean, Optional Unknown As Boolean? = Nothing, Optional IgnorePhoneState As Boolean = False)
                If Not IgnorePhoneState AndAlso Not EnsureCameraAvailable() Then Return
                Dim flag As Boolean = If(Unknown.HasValue, Unknown.Value, Activate)
                [Call]("CELL_CAM_ACTIVATE", flag, Activate)
            End Sub

            Public Shared WriteOnly Property CenterPosition As Vector2
                Set(value As Vector2)
                    If Not EnsureCameraAvailable() Then Return
                    [Call]("CELL_CAM_SET_CENTRE_POS", CSng(value.X), CSng(value.Y))
                End Set
            End Property
            ''' <summary>
            ''' Untested. Uses CELL_CAM_SET_COLOUR_BRIGHTNESS (not found in SCO scripts).
            ''' </summary>
            Public Shared WriteOnly Property ColourBrightness As Single
                Set(value As Single)
                    [Call]("CELL_CAM_SET_COLOUR_BRIGHTNESS", CSng(value))
                End Set
            End Property

            Public Shared Function IsPedVisible(Ped As Ped, Optional FaceCheck As Boolean = False) As Boolean
                If Not EnsureCameraAvailable() Then Return False
                If Not Ped.Exists Then Return False

                If FaceCheck Then
                    Return [Call](Of Boolean)("CELL_CAM_IS_CHAR_VISIBLE", Ped)
                Else
                    Return [Call](Of Boolean)("CELL_CAM_IS_CHAR_VISIBLE_NO_FACE_CHECK", Ped)
                End If
            End Function

            Public Shared WriteOnly Property Zoom As Single
                Set(value As Single)
                    If Not EnsureCameraAvailable() Then Return
                    [Call]("CELL_CAM_SET_ZOOM", CSng(value))
                End Set
            End Property
        End Class
        '===========================================================================================================
        Private Shared Function IsPhoneEnabled() As Boolean
            If NativeScript.IsRunning("spcellphonenetwork") Then
                Return True
            End If

            If NativeScript.IsRunning("spcellphone") OrElse
           NativeScript.IsRunning("spcellphonecalling") OrElse
           NativeScript.IsRunning("spcellphonecall") OrElse
           NativeScript.IsRunning("spcellphoneendcall") OrElse
           NativeScript.IsRunning("spcellphonemain") OrElse
           NativeScript.IsRunning("spcellphonedebug") Then
                Return True
            End If

            Return False
        End Function
        Private Shared Function EnsureCameraAvailable() As Boolean
            If Not CanUseCamera Then
                Tools.WriteLog("NativePhone.Camera: phone is disabled or not running.")
                Return False
            End If
            Return True
        End Function
        'public public shared Sub CreateMobilePhone()
        '    [Call]("CREATE_MOBILE_PHONE", 2)
        'End Sub 'testare parametri aggiuntivi
        'Public Shared Sub AddLineToMobilePhoneCall(p1 As Integer, p2 As Integer, p3 As Integer) 'p2 and p3 unknown
        '    [Call]("ADD_LINE_TO_MOBILE_PHONE_CALL", p1, p2, p3)
        'End Sub

        'CHANGE_PLAYER_PHONE_MODEL (player index, integer --> model phone) Testato, non funziona?
    End Class

    Public NotInheritable Class Stats
        Private Sub New()
        End Sub

        Public Shared Function CanHaveString(StatID As Integer) As Boolean
            Return [Call](Of Boolean)("CAN_THE_STAT_HAVE_STRING", StatID)
        End Function

        Public Shared Sub DecrementInt(StatID As StatINTs, DecrementValue As Integer)
            [Call]("DECREMENT_INT_STAT", StatID, DecrementValue)
        End Sub

        Public Shared Sub DecrementFloat(StatID As StatFloats, DecrementValue As Single)
            [Call]("DECREMENT_FLOAT_STAT", StatID, DecrementValue)
        End Sub

        Public Shared Function GetInt(StatID As StatINTs) As Integer
            Return [Call](Of Integer)("GET_INT_STAT", StatID)
        End Function

        Public Shared Function GetFloat(StatID As StatFloats) As Single
            Return [Call](Of Single)("GET_FLOAT_STAT", StatID)
        End Function
        ''' <summary>
        ''' Returns stat display type inside stats menu.
        ''' </summary>
        Public Shared Function GetDisplayType(StatID As Integer) As StatDisplay
            Return [Call](Of Integer)("GET_STAT_FRONTEND_DISPLAY_TYPE", StatID)
        End Function

        ''' <summary>
        ''' Returns stat visibility inside stats menu.
        ''' </summary>
        Public Shared Function GetVisibility(StatID As Integer) As Boolean
            Return [Call](Of Boolean)("GET_STAT_FRONTEND_VISIBILITY", StatID)
        End Function

        Public Shared Function GetProgressPercentage() As Single
            Return [Call](Of Single)("GET_PROGRESS_PERCENTAGE")
        End Function

        Public Shared Sub IncrementInt(StatID As StatINTs, IncrementValue As Integer, Optional NoMessage As Boolean = False)
            If NoMessage Then [Call]("INCREMENT_INT_STAT_NO_MESSAGE", StatID, IncrementValue) Else [Call]("INCREMENT_INT_STAT", StatID, IncrementValue)
        End Sub

        Public Shared Sub IncrementFloat(StatID As StatFloats, IncrementValue As Single, Optional NoMessage As Boolean = False)
            If NoMessage Then [Call]("INCREMENT_FLOAT_STAT_NO_MESSAGE", StatID, IncrementValue) Else [Call]("INCREMENT_FLOAT_STAT", StatID, IncrementValue)
        End Sub

        Public Shared Sub SetInt(StatID As StatINTs, NewValue As Integer, Optional Register As Boolean = False)
            [Call]("SET_INT_STAT", StatID, NewValue)
            If Register Then [Call]("REGISTER_INT_STAT", StatID, NewValue)
        End Sub

        Public Shared Sub SetFloat(StatID As StatFloats, NewValue As Single, Optional Register As Boolean = False)
            [Call]("SET_FLOAT_STAT", StatID, NewValue)
            If Register Then [Call]("REGISTER_FLOAT_STAT", StatID, NewValue)
        End Sub
        ''' <summary>
        ''' Sets display type inside Stats menu.
        ''' </summary>
        Public Shared Sub SetDisplayType(StatID As Integer, DisplayType As StatDisplay)
            [Call]("SET_STAT_FRONTEND_DISPLAY_TYPE", StatID, DisplayType)
        End Sub
        ''' <summary>
        ''' Sets visibility inside Stats menu.
        ''' </summary>
        Public Shared Sub SetVisibility(StatID As Integer, Visible As Boolean, Optional ForceAlways As Boolean = False)
            [Call]("SET_STAT_FRONTEND_VISIBILITY", StatID, Visible)
            If ForceAlways Then
                If Visible Then
                    [Call]("SET_STAT_FRONTEND_ALWAYS_VISIBLE", StatID)
                Else
                    [Call]("SET_STAT_FRONTEND_NEVER_VISIBLE", StatID)
                End If
            End If
        End Sub

        Public Enum StatFloats
            'from 0 to 120 (IV), from 121 to 246 EFLC, from 247 to 252 empty
            IV_GameProgress = 0
            IV_RomanLike = 1
            IV_RomanRespect = 2
            IV_RomanMissionProgress = 3
            IV_VladMissionProgress = 4
            IV_JacobLike = 5
            IV_JacobRespect = 6
            IV_JacobMissionProgress = 7
            IV_FaustinMissionProgress = 8
            IV_MannyMissionProgress = 9
            IV_ElizabetaMissionProgress = 10
            IV_DwayneLike = 11
            IV_DwayneRespect = 12
            IV_DwayneMissionProgress = 13
            IV_BrucieLike = 14
            IV_BrucieRespect = 15
            IV_BrucieMissionProgress = 16
            IV_PlayboyMissionProgress = 17
            IV_FrancisMissionProgress = 18
            IV_ULPCMissionProgress = 19
            IV_PackieLike = 20
            IV_PackieRespect = 21
            IV_PackieMissionProgress = 22
            IV_RayMissionProgress = 23
            IV_GerryMissionProgress = 24
            IV_DerrickMissionProgress = 25
            IV_BernieMissionProgress = 26
            IV_BellMissionProgress = 27
            IV_GambettiMissionProgress = 28
            IV_JimmyMissionProgress = 29
            IV_CarmenOrtizFondness = 30
            IV_CarmenTrust = 31
            IV_AlexChiltonFondness = 32
            IV_AlexTrust = 33
            IV_KikiJenkinsFondness = 34
            IV_KikiTrust = 35
            IV_MichelleFondness = 36
            IV_MichelleTrust = 37
            IV_KateFondnes = 38
            IV_KateTrust = 39
            IV_ShootingAccuracy = 40
            IV_KillsPercentInFreeAim = 41
            IV_BikeCarAverageSpeed = 42
            IV_PlayingTime = 44
            IV_LongestCopChaseTime = 45
            IV_TimeWantedOn6Stars = 46
            IV_LastCopChaseTime = 47
            IV_TimeSpentInTheWater = 49
            IV_TimeSpentShopping = 50
            IV_TimeSpentInCover = 51
            IV_PoolPlayedTime = 52
            IV_DartsTimePlayed = 53
            IV_BowlingTimePlayed = 54
            IV_TotalRaceTime = 55
            IV_SouthBrokerRecord = 56
            IV_AirportRunRecord = 57
            IV_DukesBoulevardRecord = 58
            IV_SouthAlgonquinRecord = 59
            IV_StarJunctionRecord = 60
            IV_RoadToBohanRecord = 61
            IV_NorthAlderneyRecord = 62
            IV_ElevatedRecord = 63
            IV_SouthAlderneyRecord = 64
            IV_QUB3DTimePlayed = 65
            IV_BoatingTime = 66
            IV_HeliRideTime = 67
            IV_TimeSpentOnInternet = 68
            IV_TimeSpentCalling = 69
            IV_TimeSpentWalking = 70
            IV_TimeSpentDrivingCar = 71
            IV_TimeSpentDrivingHeli = 72
            IV_TimeSpentDrivingBike = 73
            IV_TimeSpentDrivingBoat = 74
            IV_TimeSpentSwiming = 75
            IV_MilesByHelicopter = 76
            IV_MilesByCar = 77
            IV_MilesByBike = 78
            IV_MilesByBoat = 79
            IV_MilesOnFoot = 80
            IV_MilesSwam = 81
            IV_Longest2wheelStunt = 82
            IV_LongestBikeStoppie = 83
            IV_LongestBikeWheelie = 84
            IV_MilesByTrain = 85
            IV_MilesAsTaxiPassenger = 86
            IV_FarthestJumpDistance = 87
            IV_HighestJumpReached = 88
            IV_LongestFreeFall = 89
            IV_SpentOnPayNSpray = 90
            IV_SpentOnDates = 91
            IV_SpentBuyingClothes = 92
            IV_MadeFromMissions = 93
            IV_SpentInBarsAndClubs = 94
            IV_SpentOnProstitutes = 95
            IV_SpentInStripClubs = 96
            IV_SpentOnFood = 97
            IV_SpentOnTaxis = 98
            IV_MoneyLostOnStreetRaces = 99
            IV_MadeFromVigilante = 100
            IV_SpentOnCopBrides = 101
            IV_SpentOnHealthCare = 102
            IV_GivenToTramps = 103
            IV_SpentOnVendors = 104
            IV_SpentOnBinoculars = 105
            IV_SpentInGunShop = 106
            IV_MostSpentOnADate = 107
            IV_CurrentMoney = 108
            IV_MadeFromStreetRaces = 109
            IV_TotalProgress = 110
            IV_LongestStoppieTime = 111
            IV_LongestWheelieTime = 112
            IV_Longest2wheelTime = 113
            IV_FlightTime = 114
            IV_RespectTotal = 115
            IV_MoneyMadeFromUSJs = 116
            IV_PickedUpOnStreet = 117
            IV_LongestNonStopGame = 118
            IV_MadeFromVehicleThefts = 119
            IV_MadeFromRandomPeds = 120
            IV_Empty1 = 121
            IV_Empty2 = 122
            IV_Empty3 = 123
            IV_Empty4 = 124
            IV_Empty5 = 125
            IV_Empty6 = 126
            IV_Empty7 = 127
            IV_Empty8 = 128
            IV_Empty9 = 129
            IV_Empty10 = 130
            IV_Empty11 = 131
            IV_Empty12 = 132
            IV_Empty13 = 133
            IV_Empty14 = 134
            IV_Empty15 = 135
            IV_Empty16 = 136
            IV_Empty17 = 137
            IV_Empty18 = 138
            IV_Empty19 = 139
            IV_Empty20 = 140
            IV_Empty21 = 141
            IV_Empty22 = 142
            IV_Empty23 = 143
            IV_Empty24 = 144
            IV_Empty25 = 145
            IV_Empty26 = 146
            IV_Empty27 = 147
            IV_Empty28 = 148
            IV_Empty29 = 149
            IV_Empty30 = 150
            IV_Empty31 = 151
            IV_Empty32 = 152
            IV_Empty33 = 153
            IV_Empty34 = 154
            IV_Empty35 = 155
            IV_Empty36 = 156
            IV_Empty37 = 157
            ' 158 IVEFLC
            IV_Empty38 = 159
            IV_Empty39 = 160
            IV_Empty40 = 161
            IV_Empty41 = 162
            IV_Empty42 = 163
            IV_Empty43 = 164
            IV_Empty44 = 165
            IV_Empty45 = 166
            IV_Empty46 = 167
            IV_Empty47 = 168
            IV_Empty48 = 169
            IV_Empty49 = 170
            IV_Empty50 = 171
            IV_Empty51 = 172
            IV_Empty52 = 173
            IV_Empty53 = 174
            IV_Empty54 = 175
            IV_Empty55 = 176
            IV_Empty56 = 177
            IV_Empty57 = 178
            IV_Empty58 = 179
            IV_Empty59 = 180
            IV_Empty60 = 181
            ' 182–186 IVEFLC
            IV_Empty61 = 187
            IV_Empty62 = 188
            IV_Empty63 = 189
            IV_Empty64 = 190
            IV_Empty65 = 191
            IV_Empty66 = 192
            IV_Empty67 = 193
            IV_Empty68 = 194
            IV_Empty69 = 195
            IV_Empty70 = 196
            IV_Empty71 = 197
            IV_Empty72 = 198
            IV_Empty73 = 199
            IV_Empty74 = 200
            IV_Empty75 = 201
            IV_Empty76 = 202
            IV_Empty77 = 203
            IV_Empty78 = 204
            IV_Empty79 = 205
            IV_Empty80 = 206
            IV_Empty81 = 207
            IV_Empty82 = 208
            IV_Empty83 = 209
            IV_Empty84 = 210
            IV_Empty85 = 211
            IV_Empty86 = 212
            IV_Empty87 = 213
            IV_Empty88 = 214
            IV_Empty89 = 215
            IV_Empty90 = 216
            IV_Empty91 = 217
            IV_Empty92 = 218
            IV_Empty93 = 219
            IV_Empty94 = 220
            IV_Empty95 = 221
            IV_Empty96 = 222
            IV_Empty97 = 223
            IV_Empty98 = 224
            IV_Empty99 = 225
            IV_Empty100 = 226
            IV_Empty101 = 227
            ' 228 IVEFLC
            IV_Empty102 = 229
            IV_Empty103 = 230
            IV_Empty104 = 231
            IV_Empty105 = 232
            IV_Empty106 = 233
            IV_Empty107 = 234
            IV_Empty108 = 235
            IV_Empty109 = 236
            IV_Empty110 = 237
            IV_Empty111 = 238
            IV_Empty112 = 239
            IV_Empty113 = 240
            IV_Empty114 = 241
            IV_Empty115 = 242
            IV_Empty116 = 243
            IV_Empty117 = 244
            IV_Empty118 = 245
            IV_Empty119 = 246

            'emptys IVEFLC
            IVEFLC_Empty1 = 43
            IVEFLC_Empty2 = 48
            IVEFLC_Empty3 = 158
            IVEFLC_Empty4 = 182
            IVEFLC_Empty5 = 183
            IVEFLC_Empty6 = 184
            IVEFLC_Empty7 = 185
            IVEFLC_Empty8 = 186
            IVEFLC_Empty9 = 228
            IVEFLC_Empty10 = 247
            IVEFLC_Empty11 = 248
            IVEFLC_Empty12 = 249
            IVEFLC_Empty13 = 250
            IVEFLC_Empty14 = 251
            IVEFLC_Empty15 = 252
        End Enum

        Public Enum StatINTs
            IV_MissionsPassed = 253
            IV_MissionsFailed = 254
            IV_MissionsAttempted = 255
            IV_ReplaysUsed = 256
            IV_PeopleKilled = 257
            IV_NumberOfTaxiFaresCompleted = 258
            IV_TimesCheated = 259
            IV_DaysPassed = 260
            IV_TimesDied = 261
            IV_PeopleRunDown = 262
            IV_Empty1 = 263
            IV_FlipsDoneInAVehicle = 264
            IV_MostVehicleAirSpins = 265
            IV_AirLaunches = 266
            IV_HelicopterToursTaken = 267
            IV_TaxisHailed = 268
            IV_StuntJumpsFound = 269
            IV_StuntJumpsCompleted = 270
            IV_KillsSinceLastSave = 271
            IV_FiresStarted = 272
            IV_CriminalsKilled = 273
            IV_CarsStolen = 274
            IV_BikesStolen = 275
            IV_BoatsStolen = 276
            IV_HelicoptersStolen = 277
            IV_StarsAttained = 278
            IV_StarsEvaded = 279
            IV_VehiclesExported = 280
            IV_NumberOfBridgesFlownUnder = 281
            IV_PayNSprayVisits = 282
            IV_TimesGotDrunk = 283
            IV_DrugPackagesDelivered = 284
            IV_CarsSoldToStevie = 285
            IV_RandomCharactersMet = 286
            IV_BulletsFired = 287
            IV_BulletsHit = 288
            IV_KillsByHeadshots = 289
            IV_MeleeKills = 290
            IV_ArmedKills = 291
            IV_SuccessfulCounters = 292
            IV_NumberOfExplosions = 293
            IV_CarsExploded = 294
            IV_BikesExploded = 295
            IV_BoatsExploded = 296
            IV_HelicoptersExploded = 297
            IV_TiresPoppedByGunshot = 298
            IV_WeaponsPickedUp = 299
            IV_TextsReceived = 300
            IV_CallsMadeFromPhone = 301
            IV_CallsReceivedOnPhone = 302
            IV_ProstituteVisits = 303
            IV_TrampsGivenMoneyTo = 304
            IV_EmailsSent = 305
            IV_MealsEaten = 306
            IV_HotdogsEaten = 307
            IV_BurgersEaten = 308
            IV_NutsEaten = 309
            IV_BinocularsUsed = 310
            IV_PlayerDiedByMelee = 311
            IV_PlayerShotToDeath = 312
            IV_PlayerWasBlownUp = 313
            IV_PlayerWasRoadkill = 314
            IV_ScoredWithGirl = 315
            IV_PoolWins = 316
            IV_PoolDefeats = 317
            IV_PoolClearancesFromBreak = 318
            IV_DartsWins = 319
            IV_DartsDefeats = 320
            IV_Darts180sHit = 321
            IV_DartsBullseye = 322
            IV_DartsShortestCheckOut = 323
            IV_BowlingHighScore = 324
            IV_BowlingWins = 325
            IV_BowlingDraws = 326
            IV_BowlingDefeats = 327
            IV_BowlingSpares = 328
            IV_BowlingPerfects = 329
            IV_BowlingStrikes = 330
            IV_NumberOfRacesLost = 331
            IV_NumberOfRacesWon = 332
            IV_SouthBrokerWins = 333
            IV_SouthBrokerRaces = 334
            IV_AirportRunWins = 335
            IV_AirportRunRaces = 336
            IV_DukesBoulevardWins = 337
            IV_DukesBoulevardRaces = 338
            IV_SouthAlgonquinWins = 339
            IV_SouthAlgonquinRaces = 340
            IV_StarJunctionWins = 341
            IV_StarJunctionRaces = 342
            IV_RoadToBohanWins = 343
            IV_RoadToBohanRaces = 344
            IV_NorthAlderneyWins = 345
            IV_NorthAlderneyRaces = 346
            IV_ElevatedWins = 347
            IV_ElevatedRaces = 348
            IV_SouthAlderneyWins = 349
            IV_SouthAlderneyRaces = 350
            IV_QUB3DHighScore = 351
            IV_BoatingDestinations = 352
            IV_HeliRideDestinations = 353
            IV_ActivitiesWithRoman = 354
            IV_ActivitiesWithJacob = 355
            IV_ActivitiesWithBrucie = 356
            IV_ActivitiesWithDwayne = 357
            IV_ActivitiesWithPackie = 358
            IV_VigilanteLevelsDone = 359
            IV_MostWantedCompleted = 360
            IV_PigeonsExterminated = 361
            IV_FirstAidCollected = 362
            IV_IslandsUnlocked = 363
            IV_TotalDates = 364
            IV_SuccessfulDates = 365
            IV_BadDates = 366
            IV_GirlsDumped = 367
            IV_EmailsReceived = 368
            IV_PhotosTaken = 369
            IV_MocapCutscenesSkipped = 370
            IV_MocapCutscenesWatched = 371
            IV_CutscenesSkipped = 372
            IV_CutscenesWatched = 373
            IV_SodaDrunk = 374
            IV_KillsWithUnarmed = 375
            IV_KillsWithBaseballBat = 376
            IV_KillsWithPoolcue = 377
            IV_KillsWithKnife = 378
            IV_KillsWithGrenade = 379
            IV_KillsWithMolotov = 380
            IV_KillsWithRocket = 381
            IV_KillsWithPistol = 382
            IV_KillsWithCombatPistol = 383
            IV_KillsWithPumpShotgun = 384
            IV_KillsWithCombatShotgun = 385
            IV_KillsWithMicroSMG = 386
            IV_KillsWithSMG = 387
            IV_KillsWithAssaultRifle = 388
            IV_KillsWithCarbineRifle = 389
            IV_KillsWithCombatSniper = 390
            IV_KillsWithSniperRifle = 391
            IV_KillsWithRPG = 392
            IV_KillsWithFlameThrower = 393
            IV_KillsWithMinigun = 394
            '--- from 395 to 418 are IVEFLC_Episodic01-24
            IV_TimesBusted = 419
            IV_SavesMade = 420
            IV_TotalLegitimateKills = 421
            IV_FlyingSkill = 422
            IV_ProgressWithDenise = 423
            IV_ProgressWithMichelle = 424
            IV_ProgressWithHelena = 425
            IV_ProgressWithBarbara = 426
            IV_ProgressWithKatie = 427
            IV_ProgressWithMillie = 428
            IV_PimpingLevel = 429
            IV_RespectMission = 430
            IV_RespectMissionTotal = 431
            IV_KillsSinceLastCheckpoint = 432
            IV_TotalMissions = 433
            IV_Energy = 434
            IV_Armour = 435
            IV_DrivingSkill = 436
            IV_BikeSkill = 437
            IV_Luck = 438
            IV_CycleSkill = 439
            IV_UnderwaterBreathStamina = 440
            IV_Calories = 441
            IV_PeopleSavedInAnAmbulance = 442
            IV_CriminalsKilledOnVigilanteMission = 443
            IV_HighestParamedicMissionLevel = 444
            IV_HighestFirefighterMissionLevel = 445
            IV_TotalFiresExtinguished = 446
            IV_KgsOfExplosivesUsed = 447
            IV_NumberOfPoliceBribes = 448
            IV_FiresStarted2 = 449
            IV_HighestNumberOfPedsKilledInOneSpree = 450
            IV_HighestNumberOfCopsKilledInOneSpree = 451
            IV_HighestNumberOfVehiclesDestroyedInOneSpree = 452
            IV_HighestNumberOfCopCarsDestroyedInOneSpree = 453
            IV_HighestNumberOfPedKillsOnSpree = 454
            IV_HighestNumberOfPoliceKillsOnSpree = 455
            IV_HighestNumberOfCivilianVehiclesDestroyedOnSpree = 456
            IV_HighestNumberOfCopVehiclesDestroyedOnSpree = 457
            IV_HighestNumberOfTanksDestroyedOnSpree = 458
            IV_RampagesAttempted = 459
            IV_RampagesPassed = 460
            IV_PeopleWastedByOthers = 461
            IV_NumberOfVehiclesImported = 462
            IV_GangMembersRecruited = 463
            IV_EnemyGangMembersKilled = 464
            IV_FriendlyGangMembersKilled = 465
            IV_RecruitedGangMembersKilled = 466
            IV_TimeSpentLookingThroughTelescope = 467
            IV_PlanesExploded = 468
            IV_NumberOfExoticExportsDone = 469
            IV_Empty2 = 470
            IV_Empty3 = 471
            IV_AddictionLevel = 473
            IV_FavoriteRadioStation = 474
            IV_LeastFavoriteStation = 475
            IV_FavoriteTransport = 476
            IV_FavoriteCar = 477
            IV_FavoriteBike = 478
            IV_FavoriteBoat = 479
            IV_FavoriteHelicopter = 480
            IV_MilesTravelled = 481
            IV_PlayerImage = 482
            IV_FavoriteCarColor = 483
            IV_TopVehicleSpeed = 484
            IV_FavoriteDatePlace = 485
            IV_FavoriteActivity = 486
            IV_NikoAndRomanHangout = 487
            IV_NikoAndJacobHangout = 488
            IV_NikoAndBrucieHangout = 489
            IV_NikoAndDwayneHangout = 490
            IV_NikoAndPackieHangout = 491
            IV_FavoriteShop = 492
            IV_FavoriteInternetSite = 493
            IV_LastMissionName = 494
            IV_Empty4 = 495
            IV_Empty5 = 496
            IV_Empty6 = 497
            IV_Empty7 = 498
            IV_Empty8 = 499
            IV_VehiclesBlownUp = 500
            IV_Health = 501
            IV_GangMemberCount = 502
            '-------------------------------------------
            'IV & EFLC
            IVEFLC_KillsWithEpisodic1 = 395
            IVEFLC_KillsWithEpisodic2 = 396
            IVEFLC_KillsWithEpisodic3 = 397
            IVEFLC_KillsWithEpisodic4 = 398
            IVEFLC_KillsWithEpisodic5 = 399
            IVEFLC_KillsWithEpisodic6 = 400
            IVEFLC_KillsWithEpisodic7 = 401
            IVEFLC_KillsWithEpisodic8 = 402
            IVEFLC_KillsWithEpisodic9 = 403
            IVEFLC_KillsWithEpisodic10 = 404
            IVEFLC_KillsWithEpisodic11 = 405
            IVEFLC_KillsWithEpisodic12 = 406
            IVEFLC_KillsWithEpisodic13 = 407
            IVEFLC_KillsWithEpisodic14 = 408
            IVEFLC_KillsWithEpisodic15 = 409
            IVEFLC_KillsWithEpisodic16 = 410
            IVEFLC_KillsWithEpisodic17 = 411
            IVEFLC_KillsWithEpisodic18 = 412
            IVEFLC_KillsWithEpisodic19 = 413
            IVEFLC_KillsWithEpisodic20 = 414
            IVEFLC_KillsWithEpisodic21 = 415
            IVEFLC_KillsWithEpisodic22 = 416
            IVEFLC_KillsWithEpisodic23 = 417
            IVEFLC_KillsWithEpisodic24 = 418
            '---
            'emptys
            IVEFLC_Empty14 = 472
            IVEFLC_Empty15 = 498
            IVEFLC_Empty16 = 499
        End Enum

        Public Enum StatDisplay
            [Integer] = 0
            Float = 1
            Percent = 2
            Money = 3
            Integer_WithRightSpacing = 4
            Weight = 5
            ''' <summary>
            ''' No measure unit (maybe raw speed value)
            ''' </summary>
            Speed = 6
            ''' <summary>
            ''' Maybe miles.
            ''' </summary>
            Distance = 7
            TimeMs = 9
            StringValue = 11
            SpeedMph = 12
            ''' <summary>
            ''' Seems not working in IV, untested EFLC.
            ''' </summary>
            BoolCheckbox = 14
        End Enum
    End Class

#Region "Internals"
    Private Shared Function IsPlayingAnim_MoveType(ped As Ped, animName As String) As Boolean
        Try
            If ped IsNot Nothing AndAlso ped.Exists Then
                Return [Call](Of Boolean)("IS_CHAR_PLAYING_ANIM", ped, "move_player", animName)
            Else
                Tools.WriteLog("Error in NativePlayer.IsPlayingAnim_MoveType: cannot find character.")
                Return False
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativePlayer.IsPlayingAnim_MoveType.", ex)
            Return False
        End Try
    End Function

    Private Structure MoveAnims
        Public Const run_turn_180_l As String = "run_turn_180_l"
        Public Const walk_turn_180_r As String = "walk_turn_180_r"
        Public Const run_down As String = "run_down"
        Public Const sprint As String = "sprint"
        Public Const run As String = "run"
        Public Const walk_turn_r As String = "walk_turn_r"
        Public Const run_start_fwd As String = "runstart_fwd"
        Public Const walk_start_turn_l90 As String = "wstart_turn_l90"
        Public Const run_strafe_l As String = "run_strafe_l"
        Public Const run_turn_180_r As String = "run_turn_180_r"
        Public Const run_start_l_90 As String = "runstart_l_90"
        Public Const walk_start_turn_r180 As String = "wstart_turn_r180"
        Public Const sprint_turn_r As String = "sprint_turn_r"
        Public Const run_start_r_180 As String = "runstart_r_180"
        Public Const walk_down As String = "walk_down"
        Public Const run_stop_r As String = "rstop_r"
        Public Const walk_stop_r As String = "wstop_r"
        Public Const run_turn_r As String = "run_turn_r"
        Public Const walk_strafe_l As String = "walk_strafe_l"
        Public Const walk_up As String = "walk_up"
        Public Const run_up As String = "run_up"
        Public Const sprint_turn_l As String = "sprint_turn_l"
        Public Const idle As String = "idle"
        Public Const run_turn_180 As String = "run_turn_180"
        Public Const walk_turn_l As String = "walk_turn_l"
        Public Const turn_360_r As String = "turn_360_r"
        Public Const walk As String = "walk"
        Public Const run_start_r_90 As String = "runstart_r_90"
        Public Const sprint_turn_180_l As String = "sprint_turn_180_l"
        Public Const sprint_turn_180_r As String = "sprint_turn_180_r"
        Public Const sprint_stop_l As String = "sstop_l"
        Public Const walk_strafe_b As String = "walk_strafe_b"
        Public Const walk_b As String = "walk_b"
        Public Const walk_start_turn_r90 As String = "wstart_turn_r90"
        Public Const run_start_l_180 As String = "runstart_l_180"
        Public Const walk_turn_l3 As String = "walk_turn_l3"
        Public Const walk_turn_180_l As String = "walk_turn_180_l"
        Public Const walk_start As String = "wstart"
        Public Const walk_stop_l As String = "wstop_l"
        Public Const run_strafe_r As String = "run_strafe_r"
        Public Const turn_360_l As String = "turn_360_l"
        Public Const walk_turn_r2 As String = "walk_turn_r2"
        Public Const walk_c As String = "walk_c"
        Public Const run_turn_l2 As String = "run_turn_l2"
        Public Const run_turn_l As String = "run_turn_l"
        Public Const walk_turn_r3 As String = "walk_turn_r3"
        Public Const walk_turn_l2 As String = "walk_turn_l2"
        Public Const run_stop_l As String = "rstop_l"
        Public Const run_strafe_b As String = "run_strafe_b"
        Public Const walk_start_turn_l180 As String = "wstart_turn_l180"
        Public Const run_turn_r2 As String = "run_turn_r2"
        Public Const walk_strafe_r As String = "walk_strafe_r"
        Public Const sprint_stop_r As String = "sstop_r"
    End Structure
    '==========================================================================================================================================
    Private Const ArrestConfirmationDelayMs As Long = 3000L
    Private Const RecentRespawnWindowMs As Integer = 10000

    Private Shared ReadOnly ArrestMonitoringTick As Action = AddressOf UpdateArrestMonitoring

    Private Shared _IsBeingArrestedCached As Boolean = False
    Private Shared _IsArrestConfirmedCached As Boolean = False
    Private Shared _ArrestStartedAtGameTime As Long = 0
    Private Shared Sub EnsureArrestMonitoring()

        If Not TickHelper.ContainsHandler_Internal(ArrestMonitoringTick) Then
            TickHelper.Add_Internal(ArrestMonitoringTick)
        End If

        ' Aggiorna subito lo stato, senza attendere il tick successivo.
        UpdateArrestMonitoring()

    End Sub
    Private Shared Sub UpdateArrestMonitoring()

        If NativePlayer.IsDead Then
            _IsBeingArrestedCached = False
            _IsArrestConfirmedCached = False
            _ArrestStartedAtGameTime = 0
            Return
        End If

        Dim CurrentlyBeingArrested As Boolean =
            [Call](Of Boolean)("IS_PLAYER_BEING_ARRESTED")

        Dim CurrentGameTime As Long =
            CLng(NativeTimer.TotalGameTime)

        If CurrentlyBeingArrested Then

            If Not _IsBeingArrestedCached Then

                ' Inizio di un nuovo tentativo di arresto.
                _ArrestStartedAtGameTime = CurrentGameTime
                _IsArrestConfirmedCached = False

            ElseIf Not _IsArrestConfirmedCached Then

                Dim Elapsed As Long =
                    GetElapsedGameMilliseconds(
                        _ArrestStartedAtGameTime,
                        CurrentGameTime)

                If Elapsed >= ArrestConfirmationDelayMs Then
                    _IsArrestConfirmedCached = True
                End If

            End If

            _IsBeingArrestedCached = True

        Else

            _IsBeingArrestedCached = False

            If _IsArrestConfirmedCached Then

                ' Conserva la conferma durante la transizione
                ' dall'arresto al respawn.
                If NativePlayer.IsRespawnedAfterArrest Then
                    _IsArrestConfirmedCached = False
                    _ArrestStartedAtGameTime = 0
                End If

            Else

                ' L'arresto non è stato confermato:
                ' il giocatore potrebbe essere riuscito a fuggire.
                _ArrestStartedAtGameTime = 0

            End If

        End If

    End Sub
    Private Shared Function GetElapsedGameMilliseconds(
    StartTime As Long,
    CurrentTime As Long) As Long

        Dim Elapsed As Long = CurrentTime - StartTime

        If Elapsed < 0 Then
            Elapsed += 4294967296L
        ElseIf Elapsed > Integer.MaxValue Then
            Elapsed -= 4294967296L
        End If

        Return Elapsed

    End Function
    '==========================================================================================================================================
#End Region

End Class