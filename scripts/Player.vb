Imports System.Reflection
Imports GTA
Imports GTA.Native.Function

Public Class NativePlayer
    Public Enum PlayerMoods
        Normal = 0
        Happy = 1
        Angry = 2
        Angry2 = 3
        PoliceChase = 4
        Thinking = 5
        Suprised = 6
        Chewing = 7
        HeavyBreath = 8
        LookAround = 9
    End Enum
    Public Shared Sub AirDragMultiplierForCars(Multiplier As Double)
        [Call]("FORCE_AIR_DRAG_MULT_FOR_PLAYERS_CAR", NativePlayer.Index, Multiplier)
    End Sub
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
    Public Shared WriteOnly Property DisableSprint As Boolean
        Set(value As Boolean)
            [Call]("DISABLE_PLAYER_SPRINT", Index, value)
        End Set
    End Property
    Public Shared WriteOnly Property CanBeHassledByGangs As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_CAN_BE_HASSLED_BY_GANGS", index, value)
        End Set
    End Property
    Public Shared WriteOnly Property CanControlRagdoll As Boolean
        Set(value As Boolean)
            [Call]("GIVE_PLAYER_RAGDOLL_CONTROL", Index, value)
        End Set
    End Property
    Public Shared WriteOnly Property CanDoDriveBy As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_CAN_DO_DRIVE_BY", index, value)
        End Set
    End Property
    Public Shared WriteOnly Property CanUseCover As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_CAN_USE_COVER", index, value)
        End Set
    End Property
    Public Shared ReadOnly Property Character As Ped
        Get
            Dim PlayerChar As Native.Pointer = New Native.Pointer(GetType(Ped))
            [Call]("GET_PLAYER_CHAR", index, PlayerChar)
            Return PlayerChar.Value
        End Get
    End Property
    Public Shared Sub ClearHasDamagedAnyPed()
        [Call]("CLEAR_PLAYER_HAS_DAMAGED_AT_LEAST_ONE_PED", index)
    End Sub
    Public Shared Sub ClearHasDamagedAnyVehicle()
        [Call]("CLEAR_PLAYER_HAS_DAMAGED_AT_LEAST_ONE_VEHICLE", index)
    End Sub
    Public Shared Sub FakeDeathArrest()
        [Call]("FAKE_DEATHARREST")
    End Sub
    Public Shared WriteOnly Property FastReload As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_FAST_RELOAD", index, value)
        End Set
    End Property
    'Public Shared Function getplayerindex() As Integer
    '    Dim PlayerID As Integer = [Call](Of Integer)("GET_PLAYER_ID")
    '    Return [Call](Of Integer)("CONVERT_INT_TO_PLAYERINDEX", PlayerID)
    'End Function
    Public Shared Function getTrainPlayerWouldEnter() As Vehicle
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
        [Call]("GET_TRAIN_PLAYER_WOULD_ENTER", Index, pointer)
        If pointer.Value IsNot Nothing Then
            Return pointer.Value
        Else
            Return Nothing
        End If
    End Function
    Public Shared Function hasCollectedPickup(Pickup As Pickup) As Boolean
        Return [Call](Of Boolean)("HAS_PLAYER_COLLECTED_PICKUP", index, Pickup)
    End Function 'testare
    Public Shared ReadOnly Property hasDamagedAtLeastOneVehicle As Boolean
        Get
            Return [Call](Of Boolean)("HAS_PLAYER_DAMAGED_AT_LEAST_ONE_VEHICLE")
        End Get
    End Property
    Public Shared Function hasDeatharrestExecuted() As Boolean
        Return [Call](Of Boolean)("HAS_DEATHARREST_EXECUTED")
    End Function
    Public Shared ReadOnly Property hasFlashingWantedLevelStars() As Boolean
        Get
            Return [Call](Of Boolean)("PLAYER_HAS_FLASHING_STARS_ABOUT_TO_DROP", NativePlayer.Index)
        End Get
    End Property
    Public Shared ReadOnly Property hasGreyedWantedLevelStars() As Boolean
        Get
            Return [Call](Of Boolean)("PLAYER_HAS_GREYED_OUT_STARS", NativePlayer.Index)
        End Get
    End Property
    Public Shared ReadOnly Property Index As Integer
        Get
            Dim PlayerID As Integer = [Call](Of Integer)("GET_PLAYER_ID")
            Return [Call](Of Integer)("CONVERT_INT_TO_PLAYERINDEX", PlayerID)
        End Get
    End Property
    Public Shared WriteOnly Property IgnoredByPeds As Boolean
        Set(value As Boolean)
            [Call]("SET_EVERYONE_IGNORE_PLAYER", index, value)
        End Set
    End Property
    Public Shared WriteOnly Property IgnoredByPolice As Boolean
        Set(value As Boolean)
            [Call]("SET_POLICE_IGNORE_PLAYER", Index, value)
        End Set
    End Property
    Public Shared ReadOnly Property isBeingArrested As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_BEING_ARRESTED")
        End Get
    End Property '
    Public Shared ReadOnly Property isDead As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_DEAD", Index)
        End Get
    End Property
    Public Shared Function isFreeAimingAtPed(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_PLAYER_FREE_AIMING_AT_CHAR", Index, Ped)
    End Function
    Public Shared ReadOnly Property isFreeForAmbientTask As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_FREE_FOR_AMBIENT_TASK", index)
        End Get
    End Property
    Public Shared ReadOnly Property isPlayingMinigame As Boolean
        Get
            Return [Call](Of Boolean)("IS_MINIGAME_IN_PROGRESS")
        End Get
    End Property
    Public Shared ReadOnly Property isInInterior As Boolean
        Get
            Return [Call](Of Boolean)("IS_INTERIOR_SCENE")
        End Get
    End Property
    Public Shared ReadOnly Property isTargettingChar As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_CHAR", index)
        End Get
    End Property
    Public Shared ReadOnly Property isTargettingAnything As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_ANYTHING", index)
        End Get
    End Property
    Public Shared ReadOnly Property isTargettingObject As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_OBJECT", index)
        End Get
    End Property
    Public Shared ReadOnly Property isPressingHorn As Boolean
        Get
            Return [Call](Of Boolean)("IS_PLAYER_PRESSING_HORN", index)
        End Get
    End Property
    Public Shared WriteOnly Property KeepsWeaponsWhenRespawned As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_KEEPS_WEAPONS_WHEN_RESPAWNED", value)
        End Set
    End Property
    Public Shared WriteOnly Property NeverGetsTired As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_NEVER_GETS_TIRED", index, value)
        End Set
    End Property
    Public Shared ReadOnly Property NumberOfPedsKilled As Integer
        Get
            Return [Call](Of Integer)("GET_TOTAL_NUMBER_OF_PEDS_KILLED_BY_PLAYER", index)
        End Get
    End Property
    Public Shared WriteOnly Property GroupAlwaysFollow As Boolean
        Set(value As Boolean)
            [Call]("SET_PLAYER_GROUP_TO_FOLLOW_ALWAYS", Index, value)
        End Set
    End Property
    Public Shared ReadOnly Property TimeSinceLastArrest As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_LAST_ARREST", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceLastDeath As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_LAST_DEATH", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceDroveAgainstTraffic As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_DROVE_AGAINST_TRAFFIC", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceDroveOnPavement As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_DROVE_ON_PAVEMENT", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceRanLight As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_RAN_LIGHT", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceHitBuilding As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_BUILDING", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceHitPed As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_PED", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceHitObject As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_OBJECT", index)
        End Get
    End Property
    Public Shared ReadOnly Property TimeSinceHitVehicle As Integer
        Get
            Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_BUILDING", index)
        End Get
    End Property
    '    IS_PLAYER_BEING_ARRESTED
    'IS_PLAYER_CLIMBING
    'IS_PLAYER_CONTROL_ON
    'IS_PLAYER_DEAD
    'IS_PLAYER_FREE_AIMING_AT_CHAR
    'IS_PLAYER_FREE_FOR_AMBIENT_TASK
    'IS_PLAYER_IN_INFO_ZONE
    'IS_PLAYER_IN_POSITION_FOR_CONVERSATION
    'IS_PLAYER_IN_REMOTE_MODE
    'IS_PLAYER_IN_SHORTCUT_TAXI
    'IS_PLAYER_LOGGING_IN_NP
    'IS_PLAYER_ONLINE
    'IS_PLAYER_ONLINE_GAMESPY
    'IS_PLAYER_ONLINE_NP
    'IS_PLAYER_PERFORMING_STOPPIE
    'IS_PLAYER_PERFORMING_WHEELIE
    'IS_PLAYER_PLAYING
    'IS_PLAYER_PRESSING_HORN
    'IS_PLAYER_READY_FOR_CUTSCENE
    'IS_PLAYER_SCRIPT_CONTROL_ON
    'IS_PLAYER_SIGNED_IN_LOCALLY
    'IS_PLAYER_TARGETTING_ANYTHING
    'IS_PLAYER_TARGETTING_CHAR
    'IS_PLAYER_TARGETTING_OBJECT
    'IS_PLAYER_USING_JETPACK

    'PLAYER_HAS_FLASHING_STAR_AFTER_OFFENCE
    'PLAYER_HAS_FLASHING_STARS_ABOUT_TO_DROP
    'PLAYER_HAS_GREYED_OUT_STARS
    'PLAYER_IS_INTERACTING_WITH_GARAGE
    'PLAYER_IS_NEAR_FIRST_PIGEON
    'PLAYER_IS_PISSED_OFF
    'PLAYER_WANTS_TO_JOIN_NETWORK_GAME

    'REGISTER_PLAYER_RESPAWN_COORDS
    'https://gtamods.com/wiki/SET_EVERYONE_IGNORE_PLAYER
    'SET_HAS_BEEN_OWNED_BY_PLAYER

    '    SET_PLAYER_CAN_BE_HASSLED_BY_GANGS
    'SET_PLAYER_CAN_DO_DRIVE_BY
    'SET_PLAYER_CAN_DROP_WEAPONS_IN_CAR
    'SET_PLAYER_CAN_USE_COVER
    'SET_PLAYER_COLOUR
    'SET_PLAYER_CONTROL
    'SET_PLAYER_CONTROL_ADVANCED
    'SET_PLAYER_CONTROL_FOR_AMBIENT_SCRIPT
    'SET_PLAYER_CONTROL_FOR_NETWORK
    'SET_PLAYER_CONTROL_FOR_TEXT_CHAT
    'SET_PLAYER_CONTROL_ON_IN_MISSION_CLEANUP
    'SET_PLAYER_FAST_RELOAD
    'SET_PLAYER_GROUP_RECRUITMENT
    'SET_PLAYER_GROUP_TO_FOLLOW_ALWAYS
    'SET_PLAYER_GROUP_TO_FOLLOW_NEVER
    'SET_PLAYER_ICON_COLOUR
    'SET_PLAYER_INVINCIBLE
    'SET_PLAYER_INVISIBLE_TO_AI
    'SET_PLAYER_IS_IN_STADIUM
    'SET_PLAYER_KEEPS_WEAPONS_WHEN_RESPAWNED
    'SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE
    'SET_PLAYER_NEVER_GETS_TIRED
    'SET_PLAYER_PAIN_ROOT_BANK_NAME
    'SET_PLAYER_PLAYER_TARGETTING
    'SET_PLAYER_TEAM
    'SET_PLAYERS_CAN_BE_IN_SEPARATE_CARS
    'SET_PLAYERS_DROP_MONEY_IN_NETWORK_GAME
    'SET_PLAYERSETTINGS_MODEL_VARIATIONS_CHOICE

    'Public Shared Function getNumberOfPedsKilled() As Integer
    '    Return [Call](Of Integer)("GET_TOTAL_NUMBER_OF_PEDS_KILLED_BY_PLAYER", index)
    'End Function 'not working?
    'Public Shared Function getTimeSinceLastArrest() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_LAST_ARREST", index)
    'End Function
    'Public Shared Function getTimeSinceLastDeath() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_LAST_DEATH", index)
    'End Function
    'Public Shared Function getTimeSinceDroveAgainstTraffic() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_DROVE_AGAINST_TRAFFIC", index)
    'End Function
    'Public Shared Function getTimeSinceDroveOnPavement() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_DROVE_ON_PAVEMENT", index)
    'End Function
    'Public Shared Function getTimeSinceHitBuilding() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_BUILDING", index)
    'End Function
    'Public Shared Function getTimeSinceHitVehicle() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_BUILDING", index)
    'End Function
    'Public Shared Function getTimeSinceHitObject() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_OBJECT", index)
    'End Function
    'Public Shared Function getTimeSinceHitPed() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_HIT_PED", index)
    'End Function
    'Public Shared Function getTimeSinceRanLight() As Integer
    '    Return [Call](Of Integer)("GET_TIME_SINCE_PLAYER_RAN_LIGHT", index)
    'End Function

    'Public Shared Function isBeingArrested() As Boolean
    '    Return [Call](Of Boolean)("HAS_DEATHARREST_EXECUTED")
    'End Function
    'Public Shared Function isFreeForAmbientTask() As Boolean
    '    Return [Call](Of Boolean)("IS_PLAYER_FREE_FOR_AMBIENT_TASK", index)
    'End Function
    'Public Shared Function isInInterior() As Boolean
    '    Return [Call](Of Boolean)("IS_INTERIOR_SCENE")
    'End Function
    'Public Shared Function isTargettingChar() As Boolean
    '    Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_CHAR", index)
    'End Function
    'Public Shared Function isTargettingAnything() As Boolean
    '    Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_ANYTHING", index)
    'End Function
    'Public Shared Function isTargettingObject() As Boolean
    '    Return [Call](Of Boolean)("IS_PLAYER_TARGETTING_OBJECT", index)
    'End Function
    'Public Shared Function isPressingHorn() As Boolean
    '    Return [Call](Of Boolean)("IS_PLAYER_PRESSING_HORN", index)
    'End Function
    'Public Shared Sub SetKeepsWeaponsWhenRespawned(KeepWeapon As Boolean)
    '    [Call]("SET_PLAYER_KEEPS_WEAPONS_WHEN_RESPAWNED", KeepWeapon)
    'End Sub
    'Public Shared Sub SetAllowedToLockOn(LockOnAllowed As Boolean)
    '    [Call]("DISABLE_PLAYER_LOCKON", index, Not LockOnAllowed)
    'End Sub
    'Public Shared Sub SetAllowedToPickupObjects(PickupAllowed As Boolean)
    '    [Call]("ALLOW_PLAYER_TO_CARRY_NON_MISSION_OBJECTS", index, PickupAllowed)
    'End Sub
    'Public Shared Sub SetAllowedToSprint(SprintAllowed As Boolean)
    '    [Call]("DISABLE_PLAYER_SPRINT", index, Not SprintAllowed)
    'End Sub
    'Public Shared Sub SetFastReload(Enabled As Boolean)
    '    [Call]("SET_PLAYER_FAST_RELOAD", index, Enabled)
    'End Sub
    'Public Shared Sub SetIgnoredByPeds(Ignored As Boolean)
    '    [Call]("SET_EVERYONE_IGNORE_PLAYER", index, Ignored)
    'End Sub
    'Public Shared Sub SetNeverGetsTired(Enabled As Boolean)
    '    [Call]("SET_PLAYER_NEVER_GETS_TIRED", index, Enabled)
    'End Sub
    'Public Shared Sub SetCanBeHassleByGangs(Enabled As Boolean)
    '    [Call]("SET_PLAYER_CAN_BE_HASSLED_BY_GANGS", index, Enabled)
    'End Sub
    'Public Shared Sub SetCanDoDriveBy(Enabled As Boolean)
    '    [Call]("SET_PLAYER_CAN_DO_DRIVE_BY", index, Enabled)
    'End Sub
    'Public Shared Sub SetCanUseCover(Enabled As Boolean)
    '    [Call]("SET_PLAYER_CAN_USE_COVER", index, Enabled)
    'End Sub
    'Public Shared Sub SetPlayerControlDuringRagdoll(Controllable As Boolean)
    '    [Call]("GIVE_PLAYER_RAGDOLL_CONTROL", index, Controllable)
    'End Sub
    'Public Shared Sub SetMissionFlag(UnknownBool As Boolean)
    '    [Call]("SET_MISSION_FLAG", UnknownBool)
    'End Sub
    'Public Shared Sub SetPlayerGroupAlwaysFollow(Enable As Boolean)
    '    [Call]("SET_PLAYER_GROUP_TO_FOLLOW_ALWAYS", index, Enable)
    'End Sub
    'Public Shared Function getPlayerCharacter() As Ped
    '    Dim PlayerChar As Native.Pointer = New Native.Pointer(GetType(Ped))
    '    [Call]("GET_PLAYER_CHAR", index, PlayerChar)
    '    Return PlayerChar.Value
    'End Function
    Public Shared Sub SetMayOnlyEnterThisVehicle(Car As Vehicle, OnlyThisVehicle As Boolean)
        Dim PlayerIndex = index()
        If OnlyThisVehicle Then [Call]("SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE", PlayerIndex, Car) Else [Call]("SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE", PlayerIndex, 0)
    End Sub
    Public Shared Sub SetMood(Mood As PlayerMoods)
        NativePeds.Animations.Facials.Request(NativePeds.Animations.Facials.Player.AnimSet)
        Dim MoodString As String
        Select Case Mood
            Case 0
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
            Case 1
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
                MoodString = "plyr_mood_happy"
            Case 2
                ' MoodString = "plyr_mood_angry"
                [Call]("SET_PLAYER_MOOD_PISSED_OFF", index, 150000)
            Case 3
                [Call]("SET_PLAYER_MOOD_PISSED_OFF", index, 150000)
                MoodString = "gest_angry_loop"
            Case 4
                [Call]("SET_PLAYER_MOOD_PISSED_OFF", index, 150000)
                MoodString = "police_chase"
            Case 5
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
                MoodString = "gest_think_loop"
            Case 6
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
                MoodString = "gest_surprised_loop"
            Case 7
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
                MoodString = "chew"
            Case 8
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
                MoodString = "heavybreath"
            Case 9
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
                MoodString = "lookaround"
            Case Else
                [Call]("SET_PLAYER_MOOD_NORMAL", index)
                MoodString = ""
        End Select
        If Mood <> 0 AndAlso Mood <> 2 AndAlso Mood <= 9 AndAlso Mood > 0 Then
            'NativePeds.Animations.Facials.Play(getPlayerCharacter, MoodString, -1, True)
            [Call]("TASK_PLAY_ANIM_FACIAL", Character, MoodString, "facials@m_hi", 1.0, 1, 0, -1)
        End If
    End Sub
    Public Shared Sub SetPlayerControl(CanControlCharacter As Boolean, Optional CanControlCamera As Boolean = True)
        [Call]("SET_PLAYER_CONTROL", index, CanControlCharacter)
        [Call]("SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS", Not CanControlCamera)
    End Sub
    Public Shared Sub SuppressFadeInAfterDeathArrest(Optional Enabled As Boolean = True)
        [Call]("SUPPRESS_FADE_IN_AFTER_DEATH_ARREST", Enabled)
    End Sub
    Public Class Stats
        Public Shared Function canHaveString(StatID As StatFloats) As Boolean
            Return [Call](Of Boolean)("CAN_THE_STAT_HAVE_STRING", StatID)
        End Function
        Public Shared Function canHaveString(StatID As StatINTs) As Boolean
            Return [Call](Of Boolean)("CAN_THE_STAT_HAVE_STRING", StatID)
        End Function
        Public Shared Sub DecrementInt(StatID As StatINTs, DecrementValue As Integer)
            [Call]("DECREMENT_INT_STAT", StatID, DecrementValue)
        End Sub
        Public Shared Sub DecrementFloat(StatID As StatFloats, DecrementValue As Single)
            [Call]("DECREMENT_FLOAT_STAT", StatID, DecrementValue)
        End Sub
        Public Shared Function getInt(StatID As StatINTs) As Integer
            Return [Call](Of Integer)("GET_FLOAT_STAT", StatID)
        End Function
        Public Shared Function getFloat(StatID As StatFloats) As Single
            Return [Call](Of Single)("GET_FLOAT_STAT", StatID)
        End Function
        Public Shared Function getFrontedDisplayType(StatID As StatFloats) As Integer
            Return [Call](Of Integer)("GET_STAT_FRONTEND_DISPLAY_TYPE", StatID)
        End Function
        Public Shared Function getFrontedDisplayType(StatID As StatINTs) As Integer
            Return [Call](Of Integer)("GET_STAT_FRONTEND_DISPLAY_TYPE", StatID)
        End Function
        Public Shared Function getFrontedVisibility(StatID As StatFloats) As Boolean
            Return [Call](Of Boolean)("GET_STAT_FRONTEND_VISIBILITY", StatID)
        End Function
        Public Shared Function getFrontedVisibility(StatID As StatINTs) As Boolean
            Return [Call](Of Boolean)("GET_STAT_FRONTEND_VISIBILITY", StatID)
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
        Public Shared Sub SetFrontedDisplayType(StatID As StatFloats, DisplayType As Integer)
            [Call]("SET_STAT_FRONTEND_DISPLAY_TYPE", StatID, DisplayType)
        End Sub
        Public Shared Sub SetFrontedDisplayType(StatID As StatINTs, DisplayType As Integer)
            [Call]("SET_STAT_FRONTEND_DISPLAY_TYPE", StatID, DisplayType)
        End Sub
        Public Shared Sub SetFrontedVisibility(StatID As StatFloats, Visible As Boolean)
            [Call]("SET_STAT_FRONTEND_VISIBILITY", StatID, Visible)
        End Sub
        Public Shared Sub SetFrontedVisibility(StatID As StatINTs, Visible As Boolean)
            [Call]("SET_STAT_FRONTEND_VISIBILITY", StatID, Visible)
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
        'SET_STAT_FRONTEND_ALWAYS_VISIBLE
        'SET_STAT_FRONTEND_NEVER_VISIBLE
        'SET_STAT_FRONTEND_VISIBLE_AFTER_INCREMENTED
        'REGISTER_STRING_FOR_FRONTEND_STAT
        'SHOW_UPDATE_STATS
    End Class
End Class