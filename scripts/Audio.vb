Imports System.Drawing
Imports GTA
Imports GTA.Native.Function
''' <summary>
''' Represents a controllable audio instance using GTA IV native sound functions.
''' Allows playing sounds from entities, positions, or frontend UI.
''' </summary>
Public Class NativeAudio
    Private Sub New() : End Sub
    '================================================================================================================
    'Play Audio Event 
    ''' <summary>
    ''' Plays a one-shot audio event.
    ''' </summary>
    Public Shared Sub Trigger(GameSound As String)
        Try
            If String.IsNullOrWhiteSpace(GameSound) Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudio: invalid sound name")
                Return
            End If
            [Call]("PLAY_AUDIO_EVENT", GameSound)
        Catch ex As Exception
            Tools.WriteLog("Error in NativeAudio.TriggerAudio.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Triggers a one-shot audio event from a GTA object.
    ''' </summary>
    Public Shared Sub Trigger(GameSound As String, [Object] As GTA.Object)
        Try
            If String.IsNullOrWhiteSpace(GameSound) Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudioFromObject: invalid sound name")
                Return
            End If
            If [Object] Is Nothing OrElse Not [Object].Exists Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudioFromObject: invalid object")
                Return
            End If
            [Call]("PLAY_AUDIO_EVENT_FROM_OBJECT", GameSound, [Object])
        Catch ex As Exception
            Tools.WriteLog("Error in NativeAudio.TriggerAudioFromObject.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Triggers a one-shot audio event from a ped.
    ''' </summary>
    Public Shared Sub Trigger(GameSound As String, Ped As GTA.Ped)
        Try
            If String.IsNullOrWhiteSpace(GameSound) Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudioFromPed: invalid sound name")
                Return
            End If
            If Ped Is Nothing OrElse Not Ped.Exists Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudioFromPed: invalid ped")
                Return
            End If
            [Call]("PLAY_AUDIO_EVENT_FROM_PED", GameSound, Ped)
        Catch ex As Exception
            Tools.WriteLog("Error in NativeAudio.TriggerAudioFromPed.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Triggers a one-shot audio event at a world position.
    ''' </summary>
    Public Shared Sub Trigger(GameSound As String, Position As GTA.Vector3)
        Try
            If String.IsNullOrWhiteSpace(GameSound) Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudioFromPosition: invalid sound name")
                Return
            End If
            [Call]("PLAY_SOUND_FROM_POSITION", -1, GameSound, Position.X, Position.Y, Position.Z)
        Catch ex As Exception
            Tools.WriteLog("Error in NativeAudio.TriggerAudioFromPosition.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Triggers a one-shot audio event from a vehicle.
    ''' </summary>
    Public Shared Sub Trigger(GameSound As String, Car As GTA.Vehicle)
        Try
            If String.IsNullOrWhiteSpace(GameSound) Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudioFromVehicle: invalid sound name")
                Return
            End If
            If Car Is Nothing OrElse Not Car.Exists Then
                Tools.WriteLog("Error in NativeAudio.TriggerAudioFromVehicle: invalid vehicle")
                Return
            End If
            [Call]("PLAY_AUDIO_EVENT_FROM_VEHICLE", GameSound, Car)
        Catch ex As Exception
            Tools.WriteLog("Error in NativeAudio.TriggerAudioFromVehicle.", ex)
        End Try
    End Sub
    '================================================================================================================
    '----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Indicates whether a mission complete audio is currently playing.
    ''' </summary>
    Public Shared ReadOnly Property IsMissionCompletePlaying As Boolean
        Get
            Return [Call](Of Boolean)("IS_MISSION_COMPLETE_PLAYING")
        End Get
    End Property
    ''' <summary>
    ''' Triggers a predefined mission complete audio sound.
    ''' </summary>
    Public Shared Sub TriggerMissionComplete(AudioID As MissionComplete)
        [Call]("TRIGGER_MISSION_COMPLETE_AUDIO", AudioID)
    End Sub

    Public Enum MissionComplete
        Sound01 = 6
        Sound02 = 7
        Sound03 = 10
        Sound04 = 11
        Sound05 = 15
        Sound06 = 18
        Sound07 = 24
        Sound08 = 25
        Sound09 = 27
        Sound10 = 28
        Sound11 = 33
        Sound12 = 35
        Sound13 = 42
        Sound14 = 43
        Sound15 = 51
        Sound16 = 52
        Sound17 = 53
        Sound18 = 54
        Sound19 = 55
        Sound20 = 56
        Sound21 = 57
        Sound22 = 58
        Sound23 = 59
        Sound24 = 60
        Sound25 = 61
        Sound26 = 62
        Sound27 = 63
        Sound28 = 64
        Sound29 = 65
        Sound30 = 66
        Sound31 = 67
        Sound32 = 68
        Sound33 = 69
        Sound34 = 71
    End Enum

    Public Structure GameSounds
        Public Const VehiclesExtrasCarDoorLock As String = "VEHICLES_EXTRAS_CAR_DOOR_LOCK"
        Public Const MobilePhoneSmsReceive As String = "MOBILE_PHONE_SMS_RECIEVE"
        Public Const Weapon_RocketLoop As String = "GENERAL_WEAPONS_ROCKET_LOOP"
        Public Const Weapon_WoodenBatPed As String = "GENERAL_MELEE_WOODEN_BAT_PED"
        Public Const Vehicle_SuvTrunkClose As String = "VEHICLES_EXTRAS_LUX_SUV_TRUNK_CLOSE"
        Public Const AnimF2PistolWhip As String = "ANIM_F2_PISTOL_WHIP"
        Public Const WeaponPickup_CarBomb As String = "WEAPON_PICKUP_CAR_BOMB"
        Public Const BodyArmourBuy As String = "BODY_ARMOUR_BUY"
        Public Const WeaponPickup_Handgun As String = "WEAPON_PICKUP_HANDGUN"
        Public Const WeaponPickup_Shotgun As String = "WEAPON_PICKUP_SHOTGUN"
        Public Const WeaponPickup_Baretta As String = "WEAPON_PICKUP_BARETTA"
        Public Const WeaponPickup_Mp5k As String = "WEAPON_PICKUP_MP5K"
        Public Const WeaponPickup_Uzi As String = "WEAPON_PICKUP_UZI"
        Public Const WeaponPickup_SniperRifle As String = "WEAPON_PICKUP_SNIPER_RIFLE"
        Public Const WeaponPickup_Knife As String = "WEAPON_PICKUP_KNIFE"
        Public Const WeaponPickup_Grenade As String = "WEAPON_PICKUP_GRENADE"
        Public Const WeaponPickup_M4 As String = "WEAPON_PICKUP_M4"
        Public Const WeaponPickup_Ak47 As String = "WEAPON_PICKUP_AK47"
        Public Const WeaponPickup_Molotov As String = "WEAPON_PICKUP_MOLOTOV"
        Public Const SafehouseLiftTone As String = "SAFEHOUSE_LIFT_TONE"
        ' Public Const TelevisionTurnOn As String = "TELEVISION_TURN_ON"    'Television AmbientBank
        ' Public Const TelevisionTurnOff As String = "TELEVISION_TURN_OFF"  'Television AmbientBank
        Public Const HelpMessage As String = "FRONTEND_OTHER_INFO"
        Public Const StreetFoodGrill As String = "STREET_FOOD_GRILL"
        Public Const PhoneTakePicture As String = "PHONE_TAKE_PICTURE"
        Public Const PhonePressKey As String = "PHONE_PRESS_KEY"
        Public Const PhonePressKeyFast As String = "PHONE_PRESS_KEY_SPEED"
        ' Public Const PhoneRemoteHangUpClick As String = "SFX_PHONE_REMOTE_HANG_UP_CLICK"
        Public Const PhoneHangUpClick As String = "FRONTEND_GAME_PHONE_DIAL_HANG_UP"
        Public Const PhoneEngagedTone As String = "PHONE_ENGAGED_TONES"
        Public Const PhonePreRingSFX As String = "MOBILE_PRERING"
        Public Const JetDistantWrapper As String = "JET_DISTANT_WRAPPER"
        Public Const JetMidrangePhase1 As String = "VEHICLES_AMBIENT_JETS_MIDRANGE_PHASE_JET_1"
        Public Const JetMidrangePhase2 As String = "VEHICLES_AMBIENT_JETS_MIDRANGE_PHASE_JET_2"
        Public Const JetIdle As String = "JETS_AMBIENT_IDLES_LIGHT_JET"
        Public Const ShopAlarmBell As String = "SHOP_ALARMS_BELL"
        Public Const WardrobeBuy As String = "WARDROBE_BUY"
        Public Const AnimBr1DoorKnock As String = "ANIM_BR1_FIND_AND_KILL_DOOR_KNOCK"
        Public Const MouseClick As String = "MOUSE_SINGLE_CLICK"
        Public Const MoneyPickup As String = "FRONTEND_GAME_PICKUP_MONEY"
        Public Const Drips As String = "DRIPS"
        Public Const CashRegisterOpen As String = "CASH_REGISTER_OPEN"
        Public Const PaynSprayCompressor As String = "PAYANDSPRAY_COMPRESSOR"
        'ALARM_CLOCK_ELECTRIC_LOOP
        'ALARM_CLOCK_MECH_MULTI
    End Structure
    '================================================================================================================
    ''' <summary>
    ''' Represents a positional fire sound played in the game world.
    ''' Manages the sound lifecycle using a native sound ID.
    ''' </summary>
    Public NotInheritable Class FireSound
        Private _isPlaying As Boolean
        Private _hasSoundId As Boolean
        Private _soundId As Integer
        Private _position As Vector3
        ''' <summary>
        ''' Creates a new fire sound at the specified world position.
        ''' </summary>
        Public Sub New()
            _soundId = -1
            _hasSoundId = False
            _isPlaying = False
            _position = Vector3.Zero
        End Sub
        ''' <summary>
        ''' Gets the world position where the fire sound is played.
        ''' </summary>
        Public ReadOnly Property Position As Vector3
            Get
                Return _position
            End Get
        End Property
        ''' <summary>
        ''' Gets the native sound ID allocated by GET_SOUND_ID.
        ''' Returns -1 if no sound ID is currently allocated.
        ''' </summary>
        Public ReadOnly Property Handle As Integer
            Get
                Return If(_hasSoundId, _soundId, -1)
            End Get
        End Property
        ''' <summary>
        ''' Indicates whether the fire sound is currently playing.
        ''' </summary>
        Public ReadOnly Property IsPlaying As Boolean
            Get
                Return _isPlaying
            End Get
        End Property
        ''' <summary>
        ''' Starts playing the fire sound from a specified position.
        ''' Automatically requests a sound ID if needed.
        ''' </summary>
        Public Sub Play(Position As Vector3)
            If Not _hasSoundId Then
                _soundId = [Call](Of Integer)("GET_SOUND_ID")
                _hasSoundId = True
            ElseIf _isPlaying Then
                [Call]("STOP_SOUND", _soundId)
                _isPlaying = False
            End If
            _position = Position
            [Call]("PLAY_FIRE_SOUND_FROM_POSITION", _soundId, _position.X, _position.Y, _position.Z)
            _isPlaying = True
        End Sub
        ''' <summary>
        ''' Stops the fire sound playback.
        ''' Optionally releases the native sound ID.
        ''' </summary>
        Public Sub [Stop](Optional Release As Boolean = True)
            If Not _hasSoundId Then Return
            [Call]("STOP_SOUND", _soundId)
            _isPlaying = False
            _position = Vector3.Zero
            If Release Then
                [Call]("RELEASE_SOUND_ID", _soundId)
                _soundId = -1
                _hasSoundId = False
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Represents an audio instance managed through GTA IV native sound IDs.
    ''' Supports playback, triggering, state monitoring and optional looping.
    ''' </summary>
    Public NotInheritable Class GameSound

        Public Enum State
            NotPlayed = 0
            Playing = 1
            Ended = 2
            Stopped = 3
        End Enum

        Private Enum PlaybackMode
            Frontend
            FromObject
            FromPed
            FromPosition
            FromVehicle
        End Enum

        Private _AttachedPed As GTA.Ped
        Private _AttachedCar As GTA.Vehicle
        Private _AttachedObj As GTA.Object

        Private _Position As GTA.Vector3
        Private _ID As Integer = 0

        Private _PlaybackState As State = State.NotPlayed
        Private _ForceLoop As Boolean = False
        Private _HasTriggered As Boolean = False

        Private _PlaybackMode As PlaybackMode = PlaybackMode.Frontend
        Private disposedValue As Boolean
        Private ReadOnly _TickAction As Action

        ''' <summary>
        ''' Creates a new managed sound using the specified game sound name.
        ''' </summary>
        Public Sub New(GameSound As String)

            If String.IsNullOrWhiteSpace(GameSound) Then
                Throw New ArgumentException(
                "The game sound name cannot be null or empty.",
                NameOf(GameSound))
            End If

            Me.Name = GameSound

            _Position = GTA.Vector3.Zero
            _TickAction = AddressOf UpdatePlayback

        End Sub

        ''' <summary>
        ''' Gets the internal game sound name.
        ''' </summary>
        Public ReadOnly Name As String

        ''' <summary>
        ''' Gets the native sound ID allocated by GET_SOUND_ID.
        ''' Returns 0 when no sound ID is currently allocated.
        ''' </summary>
        Public ReadOnly Property Handle As Integer
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Gets the current playback state.
        ''' The value is maintained independently of the sound ID.
        ''' </summary>
        Public ReadOnly Property PlaybackState As State
            Get
                Return _PlaybackState
            End Get
        End Property
        ''' <summary>
        ''' Gets or sets whether the sound should be forcibly replayed by TickHelper.
        ''' Set to False to deactivate looping without stopping the current playback.
        ''' </summary>
        Public Property ForceLoop As Boolean
            Get
                Return _ForceLoop
            End Get
            Set(value As Boolean)
                _ForceLoop = value
            End Set
        End Property

        ''' <summary>
        ''' Indicates whether a Trigger method has successfully
        ''' invoked an audio event since the last successful Play.
        ''' </summary>
        Public ReadOnly Property HasTriggered As Boolean
            Get
                Return _HasTriggered
            End Get
        End Property

        ''' <summary>
        ''' Gets the current sound position.
        ''' If attached to a valid entity, returns its current position.
        ''' </summary>
        Public ReadOnly Property Position3D As GTA.Vector3
            Get
                If _AttachedCar IsNot Nothing Then
                    If _AttachedCar.Exists Then
                        _Position = _AttachedCar.Position
                    Else
                        _AttachedCar = Nothing
                    End If

                ElseIf _AttachedObj IsNot Nothing Then
                    If _AttachedObj.Exists Then
                        _Position = _AttachedObj.Position
                    Else
                        _AttachedObj = Nothing
                    End If

                ElseIf _AttachedPed IsNot Nothing Then
                    If _AttachedPed.Exists Then
                        _Position = _AttachedPed.Position
                    Else
                        _AttachedPed = Nothing
                    End If
                End If

                Return _Position
            End Get
        End Property

        '====================================================================
        ' PLAY
        '====================================================================
        ''' <summary>
        ''' Plays the sound as frontend audio.
        ''' </summary>
        Public Sub Play(Optional ForceLoop As Boolean = False)
            ResetAttachments()
            _Position = GTA.Vector3.Zero
            _PlaybackMode = PlaybackMode.Frontend
            Me.ForceLoop = ForceLoop

            StartPlayback()
        End Sub

        ''' <summary>
        ''' Plays the sound attached to an object.
        ''' </summary>
        ''' <param name="ForceLoop">If True, a native "one-shot" sound will played in loop by TickHelper.</param>
        Public Sub Play([Object] As GTA.Object, Optional ForceLoop As Boolean = False)
            If [Object] Is Nothing OrElse Not [Object].Exists Then
                Tools.WriteLog("Error in GameSound " & Name & ".PlayFromObject: invalid object")
                Return
            End If

            ResetAttachments()
            _AttachedObj = [Object]
            _Position = [Object].Position
            _PlaybackMode = PlaybackMode.FromObject
            Me.ForceLoop = ForceLoop

            StartPlayback()
        End Sub

        ''' <summary>
        ''' Plays the sound attached to a ped.
        ''' </summary>
        Public Sub Play(Ped As GTA.Ped, Optional ForceLoop As Boolean = False)

            If Ped Is Nothing OrElse Not Ped.Exists Then
                Tools.WriteLog("Error in GameSound " & Name & ".PlayFromPed: invalid ped")
                Return
            End If

            ResetAttachments()

            _AttachedPed = Ped
            _Position = Ped.Position
            _PlaybackMode = PlaybackMode.FromPed
            Me.ForceLoop = ForceLoop

            StartPlayback()
        End Sub

        ''' <summary>
        ''' Plays the sound at a fixed world position.
        ''' </summary>
        Public Sub Play(Position As GTA.Vector3, Optional ForceLoop As Boolean = False)
            ResetAttachments()

            _Position = Position
            _PlaybackMode = PlaybackMode.FromPosition
            Me.ForceLoop = ForceLoop

            StartPlayback()
        End Sub

        ''' <summary>
        ''' Plays the sound attached to a vehicle.
        ''' </summary>
        Public Sub Play(Vehicle As GTA.Vehicle, Optional ForceLoop As Boolean = False)
            If Vehicle Is Nothing OrElse Not Vehicle.Exists Then
                Tools.WriteLog("Error in GameSound " & Name & ".PlayFromVehicle: invalid vehicle")
                Return
            End If

            ResetAttachments()

            _AttachedCar = Vehicle
            _Position = Vehicle.Position
            _PlaybackMode = PlaybackMode.FromVehicle
            Me.ForceLoop = ForceLoop

            StartPlayback()
        End Sub

        '====================================================================
        ' TRIGGER
        '====================================================================
        ''' <summary>
        ''' Triggers an audio event without allocating a managed sound ID.
        ''' Does not change PlaybackState.
        ''' </summary>
        Public Sub Trigger()
            Try
                [Call]("PLAY_AUDIO_EVENT", Name)
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".Trigger.", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Triggers an audio event from an object.
        ''' Does not allocate a managed sound ID.
        ''' </summary>
        Public Sub Trigger([Object] As GTA.Object)
            If [Object] Is Nothing OrElse Not [Object].Exists Then
                Tools.WriteLog("Error in GameSound " & Name & ".TriggerFromObject: invalid object.")
                Return
            End If

            Try
                [Call]("PLAY_AUDIO_EVENT_FROM_OBJECT", Name, [Object])
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".TriggerFromObject.", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Triggers an audio event from a ped.
        ''' Does not allocate a managed sound ID.
        ''' </summary>
        Public Sub Trigger(Ped As GTA.Ped)
            If Ped Is Nothing OrElse Not Ped.Exists Then
                Tools.WriteLog("Error in GameSound " & Name & ".TriggerFromPed: invalid ped.")
                Return
            End If

            Try
                [Call]("PLAY_AUDIO_EVENT_FROM_PED", Name, Ped)
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".TriggerFromPed.", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Triggers an audio event from a vehicle.
        ''' Does not allocate a managed sound ID.
        ''' </summary>
        Public Sub Trigger(Vehicle As GTA.Vehicle)
            If Vehicle Is Nothing OrElse Not Vehicle.Exists Then
                Tools.WriteLog("Error in GameSound " & Name & ".TriggerFromVehicle: invalid vehicle.")
                Return
            End If

            Try
                [Call]("PLAY_AUDIO_EVENT_FROM_VEHICLE", Name, Vehicle)
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".TriggerFromVehicle.", ex)
            End Try
        End Sub

        '====================================================================
        ' STOP & RELEASE
        '====================================================================

        ''' <summary>
        ''' Stops the managed playback and disables automatic looping.
        ''' By default, the native sound ID is also released.
        ''' </summary>
        Public Sub [Stop](Optional Release As Boolean = True)
            _ForceLoop = False

            If _ID = 0 Then
                TickHelper.Remove_Internal(_TickAction)
                _PlaybackState = State.Stopped
                Return
            End If

            Try
                [Call]("STOP_SOUND", _ID)
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".Stop.", ex)
            End Try

            TickHelper.Remove_Internal(_TickAction)

            _PlaybackState = State.Stopped

            If Release Then
                ReleaseCurrentSoundID()
            End If
        End Sub

        ''' <summary>
        ''' Releases the current native sound ID.
        ''' If playback is active, it is stopped first.
        ''' </summary>
        Public Sub Release()
            TickHelper.Remove_Internal(_TickAction)
            If _ID = 0 Then Return

            If _PlaybackState = State.Playing Then
                Try
                    [Call]("STOP_SOUND", _ID)
                Catch ex As Exception
                    Tools.WriteLog("Error in GameSound " & Name & ".Stop.", ex)
                End Try
                _PlaybackState = State.Stopped
            End If

            TickHelper.Remove_Internal(_TickAction)
            ReleaseCurrentSoundID()
        End Sub

        '====================================================================
        ' INTERNALS
        '====================================================================

        ''' <summary>
        ''' Starts playback using the currently selected playback mode.
        ''' If this instance already owns a sound ID, the previous
        ''' playback is stopped and its ID released before restarting.
        ''' </summary>
        Private Function StartPlayback() As Boolean
            If _ID <> 0 Then
                If Not StopAndReleaseCurrentPlayback() Then
                    Return False
                End If
            End If

            Try
                Dim NewID As Integer = [Call](Of Integer)("GET_SOUND_ID")

                If NewID = 0 Then
                    Tools.WriteLog("Error in GameSound " & Name & ".StartPlayback: GET_SOUND_ID returned 0.")
                    Return False
                End If

                _ID = NewID

                Try
                    Select Case _PlaybackMode
                        Case PlaybackMode.Frontend
                            [Call]("PLAY_SOUND_FRONTEND", _ID, Name)

                        Case PlaybackMode.FromObject
                            If _AttachedObj Is Nothing OrElse Not _AttachedObj.Exists Then
                                Tools.WriteLog("Info for GameSound " & Name & ".StartPlayback: attached object no longer exists.")
                                Me.Stop()
                                Return False
                            End If
                            [Call]("PLAY_SOUND_FROM_OBJECT", _ID, Name, _AttachedObj)

                        Case PlaybackMode.FromPed
                            If _AttachedPed Is Nothing OrElse Not _AttachedPed.Exists Then
                                Tools.WriteLog("Info for GameSound " & Name & ".StartPlayback: attached ped no longer exists.")
                                Me.Stop()
                                Return False
                            End If
                            [Call]("PLAY_SOUND_FROM_PED", _ID, Name, _AttachedPed)

                        Case PlaybackMode.FromPosition
                            [Call]("PLAY_SOUND_FROM_POSITION", _ID, Name, _Position.X, _Position.Y, _Position.Z)

                        Case PlaybackMode.FromVehicle
                            If _AttachedCar Is Nothing OrElse Not _AttachedCar.Exists Then
                                Tools.WriteLog("Info for GameSound " & Name & ".StartPlayback: attached vehicle no longer exists.")
                                Me.Stop()
                                Return False
                            End If
                            [Call]("PLAY_SOUND_FROM_VEHICLE", _ID, Name, _AttachedCar)
                    End Select

                Catch ex As Exception
                    Try
                        [Call]("RELEASE_SOUND_ID", _ID)
                        _ID = 0

                    Catch releaseEx As Exception
                        Tools.WriteLog("Error in GameSound " & Name & ".StartPlayback: cannot relase sound ID after playback failure.", releaseEx)
                    End Try
                    Throw
                End Try

                _PlaybackState = State.Playing
                _HasTriggered = False

                TickHelper.Add_Internal(_TickAction)

                Return True

            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".StartPlayback.", ex)

                Return False
            End Try
        End Function

        ''' <summary>
        ''' Stops the current playback and releases its sound ID.
        ''' </summary>
        Private Function StopAndReleaseCurrentPlayback() As Boolean
            If _ID = 0 Then
                Return True
            End If

            Dim CurrentID As Integer = _ID
            Try
                [Call]("STOP_SOUND", CurrentID)
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".StopAndReleaseCurrentPlayback while stopping previous playback.", ex)
            End Try

            _PlaybackState = State.Stopped

            Try
                [Call]("RELEASE_SOUND_ID", CurrentID)
                _ID = 0
                TickHelper.Remove_Internal(_TickAction)
                Return True
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".StopAndReleaseCurrentPlayback while releasing previous sound ID.", ex)
                Return False
            End Try
        End Function
        ''' <summary>
        ''' Releases the current native sound ID.
        ''' Does not change the playback state.
        ''' </summary>
        Private Function ReleaseCurrentSoundID() As Boolean
            If _ID = 0 Then
                Return True
            End If

            Try
                [Call]("RELEASE_SOUND_ID", _ID)
                _ID = 0
                Return True
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".ReleaseCurrentSoundID.", ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Called through TickHelper to monitor playback.
        ''' Detects the end of a sound, releases its ID and,
        ''' when looping is enabled, starts the next playback.
        ''' </summary>
        Private Sub UpdatePlayback()
            If _ID = 0 OrElse _PlaybackState <> State.Playing Then
                TickHelper.Remove_Internal(_TickAction)
                Return
            End If

            Try
                If _ForceLoop AndAlso Not CanReplayCurrentMode() Then
                    Tools.WriteLog("Info for GameSound " & Name & ".UpdatePlayback: attached entity no longer exists.")
                    Me.Stop()
                    Return
                End If

                If Not IsEnded(_ID) Then
                    Return
                End If

                If Not ReleaseCurrentSoundID() Then
                    Return
                End If

                _PlaybackState = State.Ended

                If Not _ForceLoop Then
                    TickHelper.Remove_Internal(_TickAction)
                    Return
                End If

                If Not CanReplayCurrentMode() Then
                    _ForceLoop = False
                    TickHelper.Remove_Internal(_TickAction)
                    Return
                End If

                If Not StartPlayback() Then
                    _ForceLoop = False
                    _PlaybackState = State.Ended
                    TickHelper.Remove_Internal(_TickAction)
                End If
            Catch ex As Exception
                Tools.WriteLog("Error in GameSound " & Name & ".UpdatePlayback.", ex)
            End Try
        End Sub

        Private Function CanReplayCurrentMode() As Boolean

            Select Case _PlaybackMode

                Case PlaybackMode.Frontend
                    Return True

                Case PlaybackMode.FromPosition
                    Return True

                Case PlaybackMode.FromObject
                    Return _AttachedObj IsNot Nothing AndAlso
                       _AttachedObj.Exists

                Case PlaybackMode.FromPed
                    Return _AttachedPed IsNot Nothing AndAlso
                       _AttachedPed.Exists

                Case PlaybackMode.FromVehicle
                    Return _AttachedCar IsNot Nothing AndAlso
                       _AttachedCar.Exists

                Case Else
                    Return False

            End Select

        End Function

        Private Function IsEnded(SoundID As Integer) As Boolean

            Return [Call](Of Boolean)(
            "HAS_SOUND_FINISHED",
            SoundID)

        End Function

        Private Sub ResetAttachments()

            _AttachedPed = Nothing
            _AttachedCar = Nothing
            _AttachedObj = Nothing

        End Sub
    End Class
End Class