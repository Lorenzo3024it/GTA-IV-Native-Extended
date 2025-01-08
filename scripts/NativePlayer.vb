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
    End Enum
    Shared Function getPlayerIndex() As Integer
        Dim PlayerID As Integer = [Call](Of Integer)("GET_PLAYER_ID")
        Return [Call](Of Integer)("CONVERT_INT_TO_PLAYERINDEX", PlayerID)
    End Function
    Shared Sub GivePlayerHelmet(Helmet As Boolean)
        [Call]("GIVE_PLAYER_HELMET", getPlayerIndex, Helmet)
    End Sub
    Shared Function isInInterior() As Boolean
        Return [Call](Of Boolean)("IS_INTERIOR_SCENE")
    End Function
    'REMOVE PLAYER HELMET
    Shared Sub SetPlayerKeepsWeaponsWhenRespawned(KeepWeapon As Boolean)
        [Call]("SET_PLAYER_KEEPS_WEAPONS_WHEN_RESPAWNED", KeepWeapon)
    End Sub
    Shared Sub SetPlayerMayOnlyEnterThisVehicle(Car As Vehicle, OnlyThisVehicle As Boolean)
        Dim PlayerIndex = getPlayerIndex()
        If OnlyThisVehicle Then [Call]("SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE", PlayerIndex, Car) Else [Call]("SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE", PlayerIndex, 0)
    End Sub
    'Shared Sub SetPlayerMoodNormal()
    '    [Call]("SET_PLAYER_MOOD_NORMAL", getPlayerIndex)
    'End Sub
    Shared Sub SetPlayerMood(Player As Ped, Mood As PlayerMoods)
        Dim MoodString As String
        Select Case Mood
            Case 0
                MoodString = "plyr_mood_normal"
            Case 1
                MoodString = "plyr_mood_happy"
            Case 2
                MoodString = "plyr_mood_angry"
            Case 3
                MoodString = "gest_angry_loop"
            Case 4
                MoodString = "police_chase"
            Case 5
                MoodString = "gest_think_loop"
            Case 6
                MoodString = "gest_surprised_loop"
            Case 7
                MoodString = "chew"
        End Select
        NativePeds.PlayAnimationFacial(Player, Mood, -1, True)
    End Sub
    Shared Sub SetPlayerAllowedToLockOn(LockOnAllowed As Boolean)
        [Call]("DISABLE_PLAYER_LOCKON", getPlayerIndex, Not LockOnAllowed)
    End Sub
    Shared Sub SetPlayerAllowedToPickupObjects(PickupAllowed As Boolean)
        [Call]("ALLOW_PLAYER_TO_CARRY_NON_MISSION_OBJECTS", getPlayerIndex, PickupAllowed)
    End Sub
    Shared Sub SetPlayerAllowedToSprint(SprintAllowed As Boolean)
        [Call]("DISABLE_PLAYER_SPRINT", getPlayerIndex, Not SprintAllowed)
    End Sub
    Shared Sub SetPlayerFastReload(Enabled As Boolean)
        [Call]("SET_PLAYER_FAST_RELOAD", getPlayerIndex, Enabled)
    End Sub
    Shared Sub SetPlayerIgnoredByPeds(Ignored As Boolean)
        [Call]("SET_EVERYONE_IGNORE_PLAYER", getPlayerIndex, Ignored)
    End Sub
    Shared Sub SetPlayerNeverGetsTired(Enabled As Boolean)
        [Call]("SET_PLAYER_NEVER_GETS_TIRED", getPlayerIndex, Enabled)
    End Sub
    Shared Sub SetPlayerCanBeHassleByGangs(Enabled As Boolean)
        [Call]("SET_PLAYER_CAN_BE_HASSLED_BY_GANGS", getPlayerIndex, Enabled)
    End Sub
    Shared Sub SetPlayerCanDoDriveBy(Enabled As Boolean)
        [Call]("SET_PLAYER_CAN_DO_DRIVE_BY", getPlayerIndex, Enabled)
    End Sub
    Shared Sub SetPlayerCanUseCover(Enabled As Boolean)
        [Call]("SET_PLAYER_CAN_USE_COVER", getPlayerIndex, Enabled)
    End Sub
End Class