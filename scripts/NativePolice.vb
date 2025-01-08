Imports GTA
Imports GTA.Native.Function

Public Class NativePolice
    Public Enum PoliceReports
        TrespassFactory = 1

    End Enum
    Shared Sub DontDispatchCopsForPlayer(DispatchOFF As Boolean)
        [Call]("DONT_DISPATCH_COPS_FOR_PLAYER", NativePlayer.getPlayerIndex, DispatchOFF)
    End Sub 'If True, police won't dispatch for player. If False police will dispatch normally
    Shared Sub EnablePoliceScanner(Enable As Boolean)
        If Enable Then [Call]("ENABLE_POLICE_SCANNER") Else [Call]("DISABLE_POLICE_SCANNER")
    End Sub
    Shared Function isWantedLevelGreaterThan(WantedLevel) As Boolean
        Return [Call](Of Boolean)("IS_WANTED_LEVEL_GREATER", NativePlayer.getPlayerIndex, WantedLevel)
    End Function
    Shared Function PlayerHasFlashingWantedLevelStars() As Boolean
        Return [Call](Of Boolean)("PLAYER_HAS_FLASHING_STARS_ABOUT_TO_DROP", NativePlayer.getPlayerIndex)
    End Function
    Shared Function PlayerHasGreyedWantedLevelStars() As Boolean
        Return [Call](Of Boolean)("PLAYER_HAS_GREYED_OUT_STARS", NativePlayer.getPlayerIndex)
    End Function
    Shared Function RandomCops() As Boolean
        Return [Call](Of Boolean)("GET_CREATE_RANDOM_COPS")
    End Function
    Shared Sub RandomCops(SetON As Boolean)
        [Call]("SET_CREATE_RANDOM_COPS", SetON)
    End Sub 'If no wanted level, cops on foot and in cars won't spawn. If player has wanted level, cops on foot won't spawn, cars and helis will.
    Shared Sub SetFakeWantedLevel(WantedLevel As Integer)
        [Call]("SET_FAKE_WANTED_LEVEL", WantedLevel)
    End Sub
    Shared Sub StoreWantedLevel(Bool As Boolean)
        [Call]("STORE_WANTED_LEVEL", NativePlayer.getPlayerIndex, Bool)
    End Sub
    Shared Sub SetMaxWantedLevel(MaxWantedLevel As Integer)
        [Call]("SET_MAX_WANTED_LEVEL", MaxWantedLevel)
    End Sub
    Shared Sub SetPedWontAttakPlayerWithoutWantedLevel(Ped As Ped, Bool As Boolean)
        [Call]("SET_PED_WONT_ATTACK_PLAYER_WITHOUT_WANTED_LEVE", Ped, Bool)
    End Sub
    Shared Sub SetPoliceIgnorePlayer(Ignore As Boolean)
        [Call]("SET_POLICE_IGNORE_PLAYER", NativePlayer.getPlayerIndex, Ignore)
    End Sub
    Shared Sub SetPoliceWillTrackCar(Car As Vehicle, TrackingCar As Boolean)
        [Call]("SET_POLICE_FOCUS_WILL_TRACK_CAR", Car, TrackingCar)
    End Sub
    Shared Sub SetWantedLevel(WantedLevel As Integer, ApplyNow As Boolean)
        Dim PlayerIndex = NativePlayer.getPlayerIndex()
        If WantedLevel = 0 Then
            [Call]("CLEAR_WANTED_LEVEL", PlayerIndex)
        Else
            [Call]("ALTER_WANTED_LEVEL", PlayerIndex, WantedLevel)
            If ApplyNow Then [Call]("APPLY_WANTED_LEVEL_CHANGE_NOW", PlayerIndex)
        End If
    End Sub
    Shared Sub TriggerCrimeAudio(Position As Vector3, UnknownInt As Integer)
        [Call]("TRIGGER_VIGILANTE_CRIME", UnknownInt, Position.X, Position.Y, Position.Z)
    End Sub
    Shared Sub TriggerPoliceReport(Audio As String)
        [Call]("TRIGGER_POLICE_REPORT", Audio)
    End Sub
End Class