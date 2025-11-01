Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Imports NativeExtended.NativeDraws

Public Class NativeRadio
    Public Shared ReadOnly Property CurrentStation As RadioStations
        Get
            Return [Call](Of Integer)("GET_PLAYER_RADIO_STATION_INDEX")
        End Get
    End Property
    Public Shared ReadOnly Property isRadioHudON As Boolean
        Get
            Return [Call](Of Boolean)("IS_RADIO_HUD_ON")
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
    Public Shared WriteOnly Property FreezeStation As Boolean
        Set(value As Boolean)
            If value = True Then
                [Call]("FREEZE_RADIO_STATION")
            Else
                [Call]("UNFREEZE_RADIO_STATION")
            End If
        End Set
    End Property
    Public Shared Property IntegrityStationEnabled As Boolean
        Get
            Return Not [Call](Of Boolean)("IS_LAZLOW_STATION_LOCKED")
        End Get
        Set(value As Boolean)
            If value = True Then
                [Call]("UNLOCK_LAZLOW_STATION")
            Else
                [Call]("LOCK_LAZLOW_STATION")
            End If
        End Set
    End Property
    Public Shared Function CanRenderHudInMobileRadio() As Boolean
        Return [Call](Of Boolean)("CAN_RENDER_RADIOHUD_SPRITE_IN_MOBILE_PHONE")
    End Function
    Public Shared Sub EnableFrontedRadio(Optional Enabled As Boolean = True)
        If Enabled = True Then
            [Call]("ENABLE_FRONTEND_RADIO")
        Else
            [Call]("DISABLE_FRONTEND_RADIO")
        End If
    End Sub
    'FORCE_RADIO_TRACK( "VLADIVOSTOK", "RADIO_STATION_VLADIVOSTOK_INTRO_TRACK", 2, 9 )
    Public Shared Sub ForceRadioTrack(RadioStation As RadioStations, Track As String, Optional Unknown1 As Integer = 2, Optional Unknown2 As Integer = 9)
        [Call]("FORCE_RADIO_TRACK", RadioStation, Track, Unknown1, Unknown2)
    End Sub
    Public Shared Function GetStationName(RadioStation As RadioStations, Optional Coloured As Boolean = False) As String
        Select Case RadioStation
            Case 0
                If Coloured Then
                    Return TextColors.Fx_Highlighted & TextColors.MP_Yellow2 & TextColors.FX_StopColorFxHere & TextColors.MP_Yellow3 & "The Vibe 98.9"
                Else
                    Return "The Vibe 98.9"
                End If
            Case 1
                If Coloured Then
                    Return TextColors.MP_Red & "Liberty Rock Radio " & TextColors.White & "97.8"
                Else
                    Return "Liberty Rock Radio 97.8"
                End If
            Case 2
                If Coloured Then
                    Return TextColors.MP_Orange & "Jazz " & TextColors.MP_VeryLightBlue & "Nation " & TextColors.MP_LightGreen & "Radio " & TextColors.Black & "108.5"
                Else
                    Return "Jazz Nation Radio 108.5"
                End If
            Case 3
                If Coloured Then
                    Return TextColors.MP_Yellow2 & "Massive B " & TextColors.Black & "Soundsystem"
                Else
                    Return "Massive B Soundsystem"
                End If
            Case 4
                If Coloured Then
                    Return TextColors.Fx_Highlighted & TextColors.MP_Yellow & "K109 The Studio"
                Else
                    Return "K109 The Studio"
                End If
            Case 5
                If Coloured Then
                    Return TextColors.MP_Red & "WKTT " & TextColors.White & "Radio"
                Else
                    Return "WKTT Radio"
                End If
            Case 6
                If Coloured Then
                    Return TextColors.Black & "LCHC " & TextColors.MP_Red & "Hardcore Rock"
                Else
                    Return "LCHC Hardcore Rock"
                End If
            Case 7
                If Coloured Then
                    Return "The " & TextColors.Fx_Highlighted & TextColors.Yellow & "Journey"
                Else
                    Return "The Journey"
                End If
            Case 8
                If Coloured Then
                    Return TextColors.MP_Red & "Fu" & TextColors.FX_StopColorFxHere & TextColors.MP_Yellow & "si" & TextColors.MP_VeryLightBlue & "on " & TextColors.MP_LightGreen & "FM"
                    Return "Fusion FM"
                Else
                    Return "Fusion FM"
                End If
            Case 9
                If Coloured Then
                    Return "The Beat " & TextColors.MP_Yellow & "102.7"
                Else
                    Return "The Beat 102.7"
                End If
            Case 10
                If Coloured Then
                    Return TextColors.MP_Orange & "Radio " & TextColors.MP_Violet & "Broker"
                Else
                    Return "Radio Broker"
                End If
            Case 11
                If Coloured Then
                    Return TextColors.MP_Orange & "Vladivostok FM"
                Else
                    Return "Vladivostok FM"
                End If
            Case 12
                If Coloured Then
                    Return TextColors.MP_Red & "P" & TextColors.FX_StopColorFxHere & "L" & TextColors.MP_Red & "R " & "(Public " & TextColors.White & "Liberty " & TextColors.MP_Red & "Radio)"
                Else
                    Return "PLR (Public Liberty Radio)"
                End If
            Case 13
                If Coloured Then
                    Return TextColors.Blue & "San Juan " & TextColors.MP_Red & "Sounds"
                Else
                    Return "San Juan Sounds"
                End If
            Case 14
                If Coloured Then
                    Return TextColors.MP_Purple2 & "Electro-" & TextColors.Gray & "Choc"
                Else
                    Return "Electro Choc"
                End If
            Case 15
                If Coloured Then
                    Return TextColors.MP_Red & "The " & TextColors.White & "Classics " & TextColors.MP_Red & "104.1"
                Else
                    Return "The Classics 104.1"
                End If
            Case 16
                If Coloured Then
                    Return TextColors.MP_Brown & "IF " & TextColors.MP_LightOrange & "99 " & TextColors.MP_Brown & "(International " & TextColors.MP_LightOrange & "Funk)"
                Else
                    Return "IF 99 (International Funk)"
                End If
            Case 17
                If Coloured Then
                    Return TextColors.MP_LightGreen & "Tuff " & TextColors.MP_Yellow2 & TextColors.RockstarLogo & TextColors.MP_Red & " Gong"
                Else
                    Return "Tuff Gong"
                End If
            Case 18
                If Coloured Then
                    Return TextColors.MP_Yellow2 & "Independence " & TextColors.FX_StopColorFxHere & "FM"
                Else
                    Return "Independence FM"
                End If
            Case 19
                If Coloured Then
                    Return TextColors.Blue & "Integrity " & TextColors.MP_Red & "2.0"
                Else
                    Return "Integrity 2.0"
                End If
            Case Else
                Return ""
        End Select
    End Function
    Public Shared Sub RetuneTo(RadioStation As RadioStations)
        [Call]("RETUNE_RADIO_TO_STATION_INDEX", RadioStation)
    End Sub
    Public Shared Sub RetuneToPreviousStation()
        [Call]("RETUNE_RADIO_UP")
    End Sub
    Public Shared Sub RetuneToNextStation()
        [Call]("RETUNE_RADIO_DOWN")
    End Sub
    Public Shared Sub SkipTrack()
        [Call]("SKIP_RADIO_FORWARD")
    End Sub
    Public Enum RadioStations
        THE_VIBE = 0
        LIBERTY_ROCK = 1
        JAZZ_NATION = 2
        BOBBY_KONDERS = 3
        K109_THE_STUDIO = 4
        WKTT = 5
        HARDCORE = 6
        CLASSICAL_AMBIENT = 7
        FUSION_FM = 8
        BEAT_95 = 9
        DANCE_ROCK = 10
        VLADIVOSTOK = 11
        PLR = 12
        SAN_JUAN_SOUNDS = 13
        DANCE_MIX = 14
        NY_CLASSICS = 15
        AFRO_BEAT = 16
        BABYLON = 17
        NDEPENDENT = 18
        INTEGRITY = 19
    End Enum
    'Public Shared Sub DisplayCurrentStationSprite_ThisFrame(Optional PositionX As Single = 0.0, Optional PositionY As Single = 0.05, Optional SizeX As Single = 0.1, Optional SizeY As Single = 0.1, Optional Rotation As Single = 0.0, Optional Alpha As Integer = 255)
    'End Sub
    'Public Shared Sub DisableIntegrityRadioStation(Disable As Boolean)
    '    If Disable Then [Call]("LOCK_LAZLOW_STATION") Else [Call]("UNLOCK_LAZLOW_STATION")
    'End Sub
    'Public Shared Function isIntegrityRadioStationDisabled() As Boolean
    '    Return [Call](Of Boolean)("IS_LAZLOW_STATION_LOCKED")
    'End Function
    'Public Shared Sub FreezeRadioStation(Freeze As Boolean)
    '    If Freeze Then [Call]("FREEZE_RADIO_STATION") Else [Call]("UNFREEZE_RADIO_STATION")
    'End Sub
    'Public Shared Sub DisplayHudInMobileRadio_MaybeNotWorking(Display As Boolean)
    '    [Call]("RENDER_RADIOHUD_SPRITE_IN_MOBILE_PHONE", Display)
    'End Sub
    'Public Shared Function isRadioHudON() As Boolean
    '    Return [Call](Of Boolean)("IS_RADIO_HUD_ON")
    'End Function
    'Public Shared Sub EnableMobileRadio(Enable As Boolean)
    '    [Call]("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", Enable)
    '    [Call]("SET_MOBILE_PHONE_RADIO_STATE", Enable)
    'End Sub
    'Public Shared Function isMobileRadioActive() As Boolean
    '    Return [Call](Of Boolean)("IS_MOBILE_PHONE_RADIO_ACTIVE")
    'End Function
    'Public Shared Function getCurrentRadioStation() As RadioStations
    '    Return [Call](Of Integer)("GET_PLAYER_RADIO_STATION_INDEX")
    'End Function
End Class




