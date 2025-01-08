Imports GTA
Imports GTA.Native.Function

Public Class NativeRadio
    Shared Sub EnableMobileRadio(Enable As Boolean)
        [Call]("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", Enable)
        [Call]("SET_MOBILE_PHONE_RADIO_STATE", Enable)
    End Sub
    Shared Function isMobileRadioActive() As Boolean
        Return [Call](Of Boolean)("IS_MOBILE_PHONE_RADIO_ACTIVE")
    End Function
    Public Shared Sub EnableFrontedRadio(ByVal Enabled As Boolean)
        If Enabled = True Then
            [Call]("ENABLE_FRONTEND_RADIO")
        Else
            [Call]("DISABLE_FRONTEND_RADIO")
        End If
    End Sub
    Public Shared Function getCurrentRadioStation() As RadioStation
        Return [Call](Of Integer)("GET_PLAYER_RADIO_STATION_INDEX")
    End Function
    Public Shared Function isRadioHudON() As Boolean
        Return [Call](Of Boolean)("IS_RADIO_HUD_ON")
    End Function 'Testare
    Public Shared Sub RetuneRadionUP()
        [Call]("RETUNE_RADIO_UP")
    End Sub
    Public Shared Sub RetuneRadionDown()
        [Call]("RETUNE_RADIO_DOWN")
    End Sub
    Public Shared Sub SetRadioStation(RadioStation As RadioStation)
        [Call]("RETUNE_RADIO_TO_STATION_INDEX", RadioStation)
    End Sub
    Shared Sub SkipRadioForward()
        [Call]("SKIP_RADIO_FORWARD")
    End Sub
    Shared Sub DisableIntegrityRadioStation(Disable As Boolean)
        If Disable Then [Call]("LOCK_LAZLOW_STATION") Else [Call]("UNLOCK_LAZLOW_STATION")
    End Sub
    Shared Function isIntegrityRadioStationDisabled() As Boolean
        Return [Call](Of Boolean)("IS_LAZLOW_STATION_LOCKED")
    End Function
    'FREEZE_RADIO_STATION
    'UNFREEZE_RADIO_STATION
End Class