Imports GTA
Imports GTA.Native.Function

Public Class NativeAudio
    'Inherits Script
    Private _PlaybState As State
    Private _AttachedPed As GTA.Ped
    Private _AttachedCar As GTA.Vehicle
    Private _AttachedObj As GTA.Object
    Private _Position As GTA.Vector3
    Private _startTime As DateTime
    Private _stoppedManually As Boolean
    Private _ID As Integer
    Public ReadOnly Property ID As Integer
        Get
            Return _ID
        End Get
    End Property
    Public ReadOnly Property Name As String
    Public Enum State
        NotPlayed = 0
        Playing = 1
        ' PlayingInLoop = 2
        Ended = 3
        Stopped = 4
    End Enum
    Public ReadOnly Property Position3D As GTA.Vector3
        Get
            If _AttachedCar IsNot Nothing Then
                _Position = _AttachedCar.Position
            ElseIf _AttachedObj IsNot Nothing Then
                _Position = _AttachedObj.Position
            ElseIf _AttachedPed IsNot Nothing Then
                _Position = _AttachedPed.Position
            End If
            Return _Position
        End Get
    End Property
    Public ReadOnly Property PlaybackState As State
        Get
            If _stoppedManually Then
                Return State.Stopped
            End If

            If _startTime = DateTime.MinValue Then ' o 0 se è un numero
                Return State.NotPlayed
            End If

            If [Call](Of Boolean)("HAS_SOUND_FINISHED", _ID) Then
                Return State.Ended
            End If

            Return State.Playing
        End Get
    End Property
    '================================================================================================================
    Sub New(GameSound As String) 'GameSound As String
        'Me.Interval = 10
        _ID = Nothing
        Name = GameSound

        _Position = GTA.Vector3.Zero
        _AttachedPed = Nothing
        _AttachedCar = Nothing
        _AttachedObj = Nothing
    End Sub
    Public Sub Play() 'As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        If _ID = Nothing Then _ID = [Call](Of Integer)("GET_SOUND_ID")
        _startTime = DateAndTime.Now
        [Call]("PLAY_SOUND_FRONTED", _ID, Name)
    End Sub
    Public Sub PlayAudioFromObject(Obj As GTA.Object) 'As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        If _ID = Nothing Then _ID = [Call](Of Integer)("GET_SOUND_ID")
        _startTime = DateAndTime.Now
        _AttachedObj = Obj
        _Position = Obj.Position
        [Call]("PLAY_SOUND_FROM_OBJECT", _ID, Name, Obj)
    End Sub
    Public Sub PlayAudioFromPed(Ped As GTA.Ped) ' As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        If _ID = Nothing Then _ID = [Call](Of Integer)("GET_SOUND_ID")
        _startTime = DateAndTime.Now
        _AttachedPed = Ped
        _Position = Ped.Position
        [Call]("PLAY_SOUND_FROM_PED", _ID, Name, Ped)
    End Sub
    Public Sub PlayAudioFromPosition(Position As GTA.Vector3)
        If _ID = Nothing Then _ID = [Call](Of Integer)("GET_SOUND_ID")
        _startTime = DateAndTime.Now
        _Position = Position
        [Call]("PLAY_SOUND_FROM_POSITION", _ID, Name, Position.X, Position.Y, Position.Z)
    End Sub
    Public Sub PlayAudioFromVehicle(Car As GTA.Vehicle) 'As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        If _ID = Nothing Then _ID = [Call](Of Integer)("GET_SOUND_ID")
        _startTime = DateAndTime.Now
        _AttachedCar = Car
        _Position = Car.Position
        [Call]("PLAY_SOUND_FROM_VEHICLE", _ID, Name, Car)
    End Sub
    '------------------------------------------------------------------
    'Stop & Relase ID
    Public Sub Relase()
        [Call]("RELEASE_SOUND_ID", _ID)
    End Sub
    Public Sub [Stop](Optional RelaseSoundID As Boolean = False)
        _stoppedManually = True
        _startTime = Nothing
        _AttachedCar = Nothing
        _AttachedObj = Nothing
        _AttachedPed = Nothing
        [Call]("STOP_SOUND", _ID)
        If RelaseSoundID Then [Call]("RELEASE_SOUND_ID", _ID)
    End Sub
    '================================================================================================================
    'Play Audio Event 
    Public Shared Sub TriggerAudio(GameSound As String)
        [Call]("PLAY_AUDIO_EVENT", GameSound)
    End Sub
    Public Shared Sub TriggerAudioFromObject(GameSound As String, Obj As GTA.Object)
        [Call]("PLAY_AUDIO_EVENT_FROM_OBJECT", GameSound, Obj)
    End Sub
    Public Shared Sub TriggerAudioFromPed(GameSound As String, Ped As GTA.Ped)
        [Call]("PLAY_AUDIO_EVENT_FROM_PED", GameSound, Ped)
    End Sub
    Public Shared Sub TriggerAudioFromPosition(GameSound As String, Position As GTA.Vector3)
        [Call]("PLAY_SOUND_FROM_POSITION", -1, GameSound, Position.X, Position.Y, Position.Z)
    End Sub
    Public Shared Sub TriggerAudioFromVehicle(GameSound As String, Car As GTA.Vehicle)
        [Call]("PLAY_AUDIO_EVENT_FROM_VEHICLE", GameSound, Car)
    End Sub
    '================================================================================================================
    'Other Audio Native Calls
    Public Shared Sub RequestMissionAudioBank(AudioString As String)
        [Call]("REQUEST_MISSION_AUDIO_BANK", AudioString)
    End Sub
    Public Shared Sub RelaseMissionAudioBank(AudioString As String)
        [Call]("MISSION_AUDIO_BANK_NO_LONGER_NEEDED")
    End Sub
    '----------------------------------------------------------------------------------------------------
    Public Shared ReadOnly Property isMissionCompleteAudioPlaying As Boolean
        Get
            Return [Call](Of Boolean)("IS_MISSION_COMPLETE_PLAYING")
        End Get
    End Property
    Public Shared Sub TriggerMissionCompleteAudio(AudioID As MissionComplete)
        [Call]("TRIGGER_MISSION_COMPLETE_AUDIO", AudioID)
    End Sub
    Public Enum MissionComplete
        Sound1 = 6
        Sound2 = 7
        Sound3 = 10
        Sound4 = 11
        Sound5 = 15
        Sound6 = 18
        Sound7 = 24
        Sound8 = 25
        Sound9 = 27
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
    '================================================================================================================
    Partial Public Class GameSounds
        Public Shared ReadOnly Vehicles_extras_car_door_lock As New NativeAudio("VEHICLES_EXTRAS_CAR_DOOR_LOCK")
        Public Shared ReadOnly Mobile_phone_sms_recieve As New NativeAudio("MOBILE_PHONE_SMS_RECIEVE")
        Public Shared ReadOnly General_weapons_rocket_loop As New NativeAudio("GENERAL_WEAPONS_ROCKET_LOOP")
        Public Shared ReadOnly General_melee_wooden_bat_ped As New NativeAudio("GENERAL_MELEE_WOODEN_BAT_PED")
        Public Shared ReadOnly Vehicles_extras_lux_suv_trunk_close As New NativeAudio("VEHICLES_EXTRAS_LUX_SUV_TRUNK_CLOSE")
        Public Shared ReadOnly Anim_f2_pistol_whip As New NativeAudio("ANIM_F2_PISTOL_WHIP")
        Public Shared ReadOnly Weapon_pickup_car_bomb As New NativeAudio("WEAPON_PICKUP_CAR_BOMB")
        Public Shared ReadOnly Gm3c_photograph_mobsters_daughter_slap As New NativeAudio("GM3C_PHOTOGRAPH_MOBSTERS_DAUGHTER_SLAP")
        Public Shared ReadOnly Gm3_kidnap_mobsters_daughter_phone_bounce As New NativeAudio("GM3_KIDNAP_MOBSTERS_DAUGHTER_PHONE_BOUNCE")
        Public Shared ReadOnly G5_sewage_shootout_aim_gun As New NativeAudio("G5_SEWAGE_SHOOTOUT_AIM_GUN")
        Public Shared ReadOnly G5_sewage_shootout_case_land As New NativeAudio("G5_SEWAGE_SHOOTOUT_CASE_LAND")
        Public Shared ReadOnly Gm4_knock_on_inside_of_boot As New NativeAudio("GM4_KNOCK_ON_INSIDE_OF_BOOT")
        Public Shared ReadOnly Body_armour_buy As New NativeAudio("BODY_ARMOUR_BUY")
        Public Shared ReadOnly Hossan_1_headbutt As New NativeAudio("HOSSAN_1_HEADBUTT")
        Public Shared ReadOnly Mobile_prering As New NativeAudio("MOBILE_PRERING")
        Public Shared ReadOnly Weapon_pickup_handgun As New NativeAudio("WEAPON_PICKUP_HANDGUN")
        Public Shared ReadOnly Weapon_pickup_shotgun As New NativeAudio("WEAPON_PICKUP_SHOTGUN")
        Public Shared ReadOnly Weapon_pickup_baretta As New NativeAudio("WEAPON_PICKUP_BARETTA")
        Public Shared ReadOnly Weapon_pickup_mp5k As New NativeAudio("WEAPON_PICKUP_MP5K")
        Public Shared ReadOnly Weapon_pickup_uzi As New NativeAudio("WEAPON_PICKUP_UZI")
        Public Shared ReadOnly Weapon_pickup_sniper_rifle As New NativeAudio("WEAPON_PICKUP_SNIPER_RIFLE")
        Public Shared ReadOnly Weapon_pickup_knife As New NativeAudio("WEAPON_PICKUP_KNIFE")
        Public Shared ReadOnly Weapon_pickup_grenade As New NativeAudio("WEAPON_PICKUP_GRENADE")
        Public Shared ReadOnly Weapon_pickup_m4 As New NativeAudio("WEAPON_PICKUP_M4")
        Public Shared ReadOnly Weapon_pickup_ak47 As New NativeAudio("WEAPON_PICKUP_AK47")
        Public Shared ReadOnly Weapon_pickup_molotov As New NativeAudio("WEAPON_PICKUP_MOLOTOV")
        Public Shared ReadOnly Jacob_gun_car_select_weapon As New NativeAudio("JACOB_GUN_CAR_SELECT_WEAPON")
        Public Shared ReadOnly Jeff_3_skid As New NativeAudio("JEFF_3_SKID")
        Public Shared ReadOnly Jeff_3_hit_by_car As New NativeAudio("JEFF_3_HIT_BY_CAR")
        Public Shared ReadOnly Jeff_3_hit_ground As New NativeAudio("JEFF_3_HIT_GROUND")
        Public Shared ReadOnly Safehouse_lift_tone As New NativeAudio("SAFEHOUSE_LIFT_TONE")
        Public Shared ReadOnly Television_turn_on As New NativeAudio("TELEVISION_TURN_ON")
        Public Shared ReadOnly Television_turn_off As New NativeAudio("TELEVISION_TURN_OFF")
        Public Shared ReadOnly Frontend_other_info As New NativeAudio("FRONTEND_OTHER_INFO")
        Public Shared ReadOnly B2p_high_end_assassin_phone_ring As New NativeAudio("B2P_HIGH_END_ASSASSIN_PHONE_RING")
        Public Shared ReadOnly B2p_high_end_assassin_dial As New NativeAudio("B2P_HIGH_END_ASSASSIN_DIAL")
        Public Shared ReadOnly B2p_high_end_assassin_phone_hang_up As New NativeAudio("B2P_HIGH_END_ASSASSIN_PHONE_HANG_UP")
        Public Shared ReadOnly B4_trespass_flaming_chopper As New NativeAudio("B4_TRESPASS_FLAMING_CHOPPER")
        Public Shared ReadOnly B4_trespass_large_explosion As New NativeAudio("B4_TRESPASS_LARGE_EXPLOSION")
        Public Shared ReadOnly B4_trespass_small_explosion As New NativeAudio("B4_TRESPASS_SMALL_EXPLOSION")
        Public Shared ReadOnly B5_truck_hustle_cab_bullet_impacts As New NativeAudio("B5_TRUCK_HUSTLE_CAB_BULLET_IMPACTS")
        Public Shared ReadOnly B6_to_live_and_die_in_nj_open_garage As New NativeAudio("B6_TO_LIVE_AND_DIE_IN_NJ_OPEN_GARAGE")
        Public Shared ReadOnly B6_to_live_and_die_in_nj_collect_h As New NativeAudio("B6_TO_LIVE_AND_DIE_IN_NJ_COLLECT_H")
        Public Shared ReadOnly Bc3_buoys_ahoy_harbour_patrol_siren As New NativeAudio("BC3_BUOYS_AHOY_HARBOUR_PATROL_SIREN")
        Public Shared ReadOnly Wardrobe_buy As New NativeAudio("WARDROBE_BUY")
        Public Shared ReadOnly Bowling_alley_ball_return As New NativeAudio("BOWLING_ALLEY_BALL_RETURN")
        Public Shared ReadOnly Bowling_alley_machine_descend As New NativeAudio("BOWLING_ALLEY_MACHINE_DESCEND")
        Public Shared ReadOnly Bowling_alley_machine_raise_pins As New NativeAudio("BOWLING_ALLEY_MACHINE_RAISE_PINS")
        Public Shared ReadOnly Bowling_alley_machine_sweep_pins As New NativeAudio("BOWLING_ALLEY_MACHINE_SWEEP_PINS")
        Public Shared ReadOnly Bowling_alley_machine_sweep_pins_return As New NativeAudio("BOWLING_ALLEY_MACHINE_SWEEP_PINS_RETURN")
        Public Shared ReadOnly Street_food_grill As New NativeAudio("STREET_FOOD_GRILL")
    End Class
End Class

'Sound strings
'"VEHICLES_EXTRAS_CAR_DOOR_LOCK"
'"MOBILE_PHONE_SMS_RECIEVE"
'"GENERAL_WEAPONS_ROCKET_LOOP"
'"GENERAL_MELEE_WOODEN_BAT_PED"
'"VEHICLES_EXTRAS_LUX_SUV_TRUNK_CLOSE"
'"ANIM_F2_PISTOL_WHIP"
'"WEAPON_PICKUP_CAR_BOMB"
'"GM3C_PHOTOGRAPH_MOBSTERS_DAUGHTER_SLAP"
'"GM3_KIDNAP_MOBSTERS_DAUGHTER_PHONE_BOUNCE"
'"G5_SEWAGE_SHOOTOUT_AIM_GUN"
'"G5_SEWAGE_SHOOTOUT_CASE_LAND"
'"GM4_KNOCK_ON_INSIDE_OF_BOOT"
'"BODY_ARMOUR_BUY"
'"HOSSAN_1_HEADBUTT"
'"MOBILE_PRERING"

'"WEAPON_PICKUP_HANDGUN"
'"WEAPON_PICKUP_SHOTGUN"
'"WEAPON_PICKUP_BARETTA"
'"WEAPON_PICKUP_MP5K"
'"WEAPON_PICKUP_UZI"
'"WEAPON_PICKUP_SNIPER_RIFLE"
'"WEAPON_PICKUP_KNIFE"
'"WEAPON_PICKUP_GRENADE"
'"WEAPON_PICKUP_M4"
'"WEAPON_PICKUP_AK47"
'"WEAPON_PICKUP_MOLOTOV"
'"BODY_ARMOUR_BUY"
'"JACOB_GUN_CAR_SELECT_WEAPON"
'"JACOB_GUN_CAR_SELECT_WEAPON"
'"JEFF_3_SKID"
'"JEFF_3_HIT_BY_CAR"
'"JEFF_3_HIT_GROUND"
'"SAFEHOUSE_LIFT_TONE"
'"TELEVISION_TURN_ON"
'"TELEVISION_TURN_OFF"
'"FRONTEND_OTHER_INFO"
'"B2P_HIGH_END_ASSASSIN_PHONE_RING"
'"B2P_HIGH_END_ASSASSIN_DIAL"
' "B2P_HIGH_END_ASSASSIN_PHONE_HANG_UP"
'"B4_TRESPASS_FLAMING_CHOPPER"
' "B4_TRESPASS_LARGE_EXPLOSION"
' "B4_TRESPASS_SMALL_EXPLOSION"
'"B5_TRUCK_HUSTLE_CAB_BULLET_IMPACTS"
'"B6_TO_LIVE_AND_DIE_IN_NJ_OPEN_GARAGE"
'"B6_TO_LIVE_AND_DIE_IN_NJ_COLLECT_H"
'"BC3_BUOYS_AHOY_HARBOUR_PATROL_SIREN"
'"WARDROBE_BUY"
'"BOWLING_ALLEY_BALL_RETURN"
'"BOWLING_ALLEY_MACHINE_DESCEND"
'"BOWLING_ALLEY_MACHINE_RAISE_PINS"
'"BOWLING_ALLEY_MACHINE_SWEEP_PINS"
'"BOWLING_ALLEY_MACHINE_SWEEP_PINS_RETURN"
'"BOWLING_ALLEY_MACHINE_DESCEND"
'"STREET_FOOD_GRILL"
'------------
' HANDLE_AUDIO_ANIM_EVENT( l_U220, "PAIN_HIGH" );