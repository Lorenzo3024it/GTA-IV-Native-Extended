Imports GTA
Imports GTA.Native.Function

Public Class NativePeds
    Public Enum CarAction
        SlowDownSoftly = 1
        SlowDownHardlyAndUnusableForEver = 2
        SlowDownHardlyAndDriveBackwards = 3
        SlowDownHardlyAndTurnLeft = 4
        SlowDownHardlyAndTurnRight = 5
        SlowDownHardly = 6
        TurnLeft = 7
        TurnRight = 8
        DriveForwardsHighRPM = 9
        SoftTurnRight = 10
        SoftTurnLeft = 11
    End Enum
    Public Enum MovementType
        Walk = 2
        Jog = 3
        Run = 4
        None = 99
    End Enum
    Public Enum PedScreams
        PanicScreamNotNiko = 0
        OnFire = 1
        HighFall = 2
    End Enum
    Public Enum PedFacials
        Think = 0
        Suprised = 1
        Chew = 2
        GunAim = 3
        CueAim = 4
        Angry1 = 5
        Angry2 = 6
        Angry3 = 7
        Angry4 = 8
        Blow = 9
        HeavyBreath = 10
        LookAround = 11
        LookDown = 12
        LookLeft = 13
        LookRight = 14
        Pain1 = 15
        Pain2 = 16
        Pain3 = 17
        Shocked = 18
        Whatever = 19
        Dead1 = 20
        Dead2 = 21
        Dead3 = 22
    End Enum
    '=============================
    'Animations
    '-----------
    Shared Sub PlayAnimation(ByVal Ped As Ped, ByVal AnimSet As String, ByVal AnimName As String, ByVal Speed As Single, Optional IsLoop As Boolean = False, Optional InsideCar As Boolean = False, Optional CanWalk As Boolean = False, Optional ComeBackToOriginalPosition As Boolean = False)
        'Dim AnimationSet As AnimationSet = New AnimationSet(AnimSet)
        'Dim f1, f2, f3, f4, f5, f6 As AnimationFlags
        'If IsLoop Then f1 = AnimationFlags.Unknown05 Else f1 = AnimationFlags.None
        'If InsideCar Then f2 = AnimationFlags.Unknown09 Else f2 = AnimationFlags.None
        'If CanWalk Then f3 = AnimationFlags.Unknown09 And f4 = AnimationFlags.Unknown11 Else f3 = AnimationFlags.None And f3 = AnimationFlags.None
        'If NoSound Then f5 = AnimationFlags.Unknown10 Else f5 = AnimationFlags.None
        'If ComeBackToOriginalPosition Then f6 = AnimationFlags.None Else f6 = AnimationFlags.Unknown01
        'Ped.Animation.Play(AnimationSet, AnimName, Speed, f1 Or f2 Or f3 Or f4 Or f5)
        Dim p2_freezeplayercontrol As Integer = 0
        Dim p3 As Integer = 0
        Dim p4_freeze_at_end = 0
        [Call]("REQUEST_ANIMS", AnimSet)
        Do
            Game.WaitInCurrentScript(1)
        Loop Until [Call](Of Boolean)("HAS_ANIMS_LOADED")
        If InsideCar Then
            [Call]("TASK_PLAY_ANIM_SECONDARY_IN_CAR", Ped, AnimName, AnimSet, Speed, False, p2_freezeplayercontrol, p3, p4_freeze_at_end)
        ElseIf CanWalk Then
            [Call]("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY", Ped, AnimName, AnimSet, Speed, IsLoop, p2_freezeplayercontrol, p3, p4_freeze_at_end)
        Else
            [Call]("TASK_PLAY_ANIM_WITH_ADVANCED_FLAGS", Ped, AnimName, AnimSet, Speed, IsLoop, True, True, ComeBackToOriginalPosition, False, ComeBackToOriginalPosition, False)
        End If
    End Sub
    Shared Sub PlayAnimationFacial(Ped As Ped, AnimFacial As PedFacials, Optional Duration As Integer = -1, Optional PlayInLoop As Boolean = False, Optional AnimSet As String = "facials@m_hi", Optional Flag2 As Boolean = False, Optional Speed As Single = 1.00001)
        Dim AnimString As String
        Select Case AnimFacial
            Case 0
                AnimString = "gest_think_loop"
            Case 1
                AnimString = "gest_surprised_loop"
            Case 2
                AnimString = "chew"
            Case 3
                AnimString = "gun_aim"
            Case 4
                AnimString = "aim_cue"
            Case 5
                AnimString = "gest_angry_loop"
            Case 6
                AnimString = "angry_a"
            Case 7
                AnimString = "angry_b"
            Case 8
                AnimString = "angry_c"
            Case 9
                AnimString = "blow"
            Case 10
                AnimString = "heavybreath"
            Case 11
                AnimString = "lookaround"
            Case 12
                AnimString = "look_down"
            Case 13
                AnimString = "look_left"
            Case 14
                AnimString = "look_right"
            Case 15
                AnimString = "pain_a"
            Case 16
                AnimString = "pain_b"
            Case 17
                AnimString = "pain_c"
            Case 18
                AnimString = "shocked"
            Case 19
                AnimString = "whatever"
            Case 20
                AnimString = "dead_a"
            Case 21
                AnimString = "dead_b"
            Case 22
                AnimString = "die_a"
        End Select
        [Call]("REQUEST_ANIMS", AnimSet)
        Do
            Game.WaitInCurrentScript(1)
        Loop Until [Call](Of Boolean)("HAS_ANIMS_LOADED")
        [Call]("TASK_PLAY_ANIM_FACIAL", Ped, AnimFacial, AnimSet, Speed, PlayInLoop, Flag2, Duration)
    End Sub
    Shared Sub StopAnimation(ByVal Ped As Ped, OnlyUpperBody As Boolean, Optional StopLoop As Boolean = False)
        Dim CurrVeh As Vehicle
        Dim CurrSeat As VehicleSeat
        If OnlyUpperBody Then
            [Call]("CLEAR_CHAR_SECONDARY_TASK", Ped)
        Else
            [Call]("CLEAR_CHAR_TASKS", Ped)
        End If
        If StopLoop Then
            If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
                CurrVeh = Ped.CurrentVehicle
                CurrSeat = getCurrentVehicleSeat(Ped)
                [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                If CurrVeh.Exists Then Ped.WarpIntoVehicle(CurrVeh, CurrSeat)
            Else
                [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
            End If
        End If
    End Sub
    Shared Function isPlayingAnim(ByVal Ped As Ped, ByVal AnimSet As String, ByVal AnimName As String) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_PLAYING_ANIM", Ped, AnimSet, AnimName)
    End Function
    Shared Sub SetAlpha(Ped As Ped, Alpha_0_to_255 As Integer)
        [Call]("SET_PED_ALPHA", Ped, Alpha_0_to_255)
    End Sub
    Shared Sub SetAnimCurrentTime(Ped As Ped, AnimSet As String, AnimName As String, Speed As Single)
        [Call]("SET_CHAR_ANIM_CURRENT_TIME", Ped, AnimSet, AnimName, Speed)
    End Sub
    Shared Sub SetAnimGroup(Ped As Ped, AnimSet As String)
        [Call]("REQUEST_ANIMS", AnimSet)
        [Call]("SET_ANIM_GROUP_FOR_CHAR", Ped, AnimSet)
    End Sub
    Shared Sub SetAnimSpeed(Ped As Ped, AnimSet As String, AnimName As String, Speed As Single)
        [Call]("SET_CHAR_ANIM_SPEED", Ped, AnimSet, AnimName, Speed)
    End Sub
    Shared Sub SetAllAnimSpeed(Ped As Ped, Speed As Single)
        [Call]("SET_CHAR_ALL_ANIMS_SPEED", Ped, Speed)
    End Sub
    '----------------------------
    Shared Sub AddPedQueue(Position As Vector3, p1 As Single, p2 As Single, p3 As Integer, p4 As Integer, p5 As Integer)
        [Call]("ADD_PED_QUEUE", Position.X, Position.Y, Position.Z, p1, p2, p3, p4, p5)
    End Sub
    'REMOVE_PED_QUEUE
    'REMOVE PED HELMET
    'SET_PED_HELMET_TEXTURE_INDEX
    Shared Sub ReviveInjuredPed(Ped As Ped)
        [Call]("REVIVE_INJURED_PED", Ped)
    End Sub
    Shared Sub SetAllowedReactionAnims(Ped As Ped)
        [Call]("ALLOW_REACTION_ANIMS", Ped)
    End Sub
    Shared Sub SetAllowedToCrouch(Ped As Ped, CrouchAllowed As Boolean)
        [Call]("SET_CHAR_ALLOWED_TO_DUCK", Ped, CrouchAllowed)
    End Sub
    Shared Sub SetPedIsDrunk(Ped As Ped, Drunk As Boolean)
        [Call]("SET_PED_IS_DRUNK", Ped, Drunk)
    End Sub
    Shared Sub SetHelmet(Ped As Ped, Enable As Boolean)
        [Call]("ENABLE_PED_HELMET", Ped, Enable)
    End Sub
    Shared Sub SetHelmetForAllPeds(Enable As Boolean)
        [Call]("ENABLE_ALL_PED_HELMETS", Enable)
    End Sub
    Shared Sub SetKeepTask(Ped As Ped, KeepTask As Boolean)
        [Call]("SET_CHAR_KEEP_TASK", Ped, KeepTask)
    End Sub
    Shared Sub SetMoveSpeed(Ped As Ped, Multiplier As Single)
        [Call]("SET_CHAR_MOVE_ANIM_SPEED_MULTIPLIER", Ped, Multiplier)
    End Sub
    Shared Sub SetMoveStyle(Ped As Ped, MoveStyle As MovementType)
        [Call]("MODIFY_CHAR_MOVE_STATE", Ped, MoveStyle)
    End Sub 'not working?
    Shared Sub SetPaths(Ped As Ped, ClimbOvers As Boolean, DropFromHeight As Boolean, Ladders As Boolean)
        [Call]("SET_PED_PATH_MAY_USE_CLIMBOVERS", Ped, ClimbOvers)
        [Call]("SET_PED_PATH_MAY_DROP_FROM_HEIGHT", Ped, DropFromHeight)
        [Call]("SET_PED_PATH_MAY_USE_LADDERS", Ped, ClimbOvers)
    End Sub
    Shared Sub SetSenseRange(Ped As Ped, Range As Single)
        [Call]("SET_SENSE_RANGE", Ped, Range)
    End Sub
    Shared Sub SetSwimSpeed(Ped As Ped, Speed As Single)
        [Call]("SET_SWIM_SPEED", Ped, Speed)
    End Sub
    Shared Sub SetWillMoveWhenInjured(Ped As Ped, WillMoveInAgony As Boolean)
        [Call]("SET_CHAR_WILL_MOVE_WHEN_INJURED", Ped, WillMoveInAgony)
    End Sub
    Shared Sub BlendFromNMWithAnim(Ped As Ped, AnimSet As String, AnimName As String, UnknownInt As Integer, Offsets As Vector3)
        [Call]("BLEND_FROM_NM_WITH_ANIM", Ped, AnimSet, AnimName, UnknownInt, Offsets.X, Offsets.Y, Offsets.Z)
    End Sub 'Testare
    'Public Shared Sub SpawnSubChar(Position As Vector3, Visible As Boolean)
    '    With DeLoreans.List(DeLoreans.ActiveCar)
    '        Native.Function.Call("REQUEST_MODEL", My.Settings.Marty1985)
    '        Do
    '            Game.WaitInCurrentScript(1)
    '        Loop Until Native.Function.Call(Of Boolean)("HAS_MODEL_LOADED", My.Settings.Marty1985)
    '        If Visible = False Then .SubChar.Visible = False Else .SubChar.Visible = True
    '        .SubChar = World.CreatePed(My.Settings.Marty1985, Position)
    '    End With
    'End Sub 'Da modificare per spawnare il modello di niko convertito a ped
    Shared Function SpawnPed(Position As Vector3, Model As String, Visible As Boolean) As Ped
        Dim SpawnedPed As Ped
        Dim TempPedPointer As Native.Pointer = New Native.Pointer(GetType(Ped))
        [Call]("REQUEST_MODEL", NativeGeneric.getHashKey(Model))
        Do
            Game.WaitInCurrentScript(1)
        Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", NativeGeneric.getHashKey(Model))
        [Call]("CREATE_CHAR", 1, NativeGeneric.getHashKey(Model), Position.X, Position.Y, Position.Z, TempPedPointer, True)
        ' SpawnedPed = World.CreatePed(Model, Position)

        SpawnedPed = TempPedPointer.Value
        If Visible = False Then SpawnedPed.Visible = False Else SpawnedPed.Visible = True
        Return SpawnedPed
    End Function
    Shared Function SpawnPedInvisible(Position As Vector3) As Ped
        Dim InvisiblePed As Ped
        Dim TempPedPointer As Native.Pointer = New Native.Pointer(GetType(Ped))
        [Call]("REQUEST_MODEL", NativeGeneric.getHashKey("superlod"))
        Do
            Game.WaitInCurrentScript(1)
        Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", NativeGeneric.getHashKey("superlod"))
        [Call]("CREATE_CHAR", 1, NativeGeneric.getHashKey("superlod"), Position.X, Position.Y, Position.Z, TempPedPointer, True)
        ' InvisiblePed = World.CreatePed(My.Settings.SuperLodPed, Position)
        InvisiblePed = TempPedPointer.Value
        InvisiblePed.Visible = False
        Return InvisiblePed
    End Function

    'SET_CHAR_SHOOT_RATE
    'SET_CHAR_DROWNS_IN_WATER
    'SET_CHAR_DROWNS_IN_SINKING_VEHICLE
    'SET_CHAR_DROPS_WEAPONS_WHEN_DEAD
    'SET_CHAR_BLEEDING
    'SET_CHAR_ACCURACY
    'SET_CHAR_SHOOT_RATE
    'IS_CHAR_FACING_CHAR
    'IS_CHAR_GESTURING
    'TASK_SWIM_TO_COORD
    'TASK_SWIM_TO_COORD
    'GET_CHAR_SWIM_STATE
    'TASK_SHOOT_AT_CHAR
    'TASK_SHOOT_AT_COORD
    'TASK_GO_STRAIGHT_TO_COORD
    'TASK_GO_TO_COORD_WHILE_AIMING
    'TASK_CAR_MISSION
    'SET_CHAR_COMPONENT_VARIATION
    'REMOVE_CHAR_ELEGANTLY
    'Task    'DROP_OBJECT
    '----------------------------------------------------------------
    'Tasks 
    '-------
    Shared Sub TaskArrestChar(Cop As Ped, Guilty As Ped)
        [Call]("TASK_CHAR_ARREST_CHAR", Cop, Guilty)
    End Sub
    Shared Sub TaskCarAction(Ped As Ped, Car As Vehicle, CarAction As CarAction, Duration As Integer)
        [Call]("TASK_CAR_TEMP_ACTION", Ped, Car, CarAction, Duration)

    End Sub 'Not working?
    Shared Sub TaskChangeCarSeat(Ped As Ped)
        If Ped.isSittingInVehicle Then
            [Call]("TASK_SHUFFLE_TO_NEXT_CAR_SEAT", Ped, Ped.CurrentVehicle)
        Else
            Exit Sub
        End If
    End Sub
    Shared Sub TaskCruiseSpeed(Ped As Ped, Speed As Single)
        [Call]("SET_DRIVE_TASK_CRUISE_SPEED", Ped, Speed)
    End Sub
    Shared Sub TaskDropWeapon(Ped As Ped)
        [Call]("FORCE_CHAR_TO_DROP_WEAPON", Ped)
    End Sub
    Shared Sub TaskFireWeapon(Ped As Ped, Target As Vector3)
        [Call]("FIRE_PED_WEAPON", Ped, Target.X, Target.Y, Target.Z)
    End Sub
    Shared Sub TaskEnterCarAsPassenger(Ped As Ped, Car As Vehicle, Seat As VehicleSeat, Duration As Integer, Optional MoveStyle As MovementType = 99)
        If MoveStyle <> 99 Then [Call]("SET_NEXT_DESIRED_MOVE_STATE", MoveStyle)
        [Call]("TASK_ENTER_CAR_AS_PASSENGER", Ped, Car, Duration, Seat)
    End Sub
    Shared Sub TaskForceFlyThroughWindScreenOnlyTLaD(Ped As Ped)
        [Call]("SET_PED_FORCE_FLY_THROUGH_WINDSCREEN", Ped)
    End Sub
    Shared Sub TaskHeliMission(Ped As Ped, Heli As Vehicle, p3_Usually_0 As Integer, UnknownRole As Integer, Destination As Vector3, p8_0_to_19 As Integer, Speed As Single, p10_0_to_30 As Integer, Optional p11_Up_To_90 As Single = -1, Optional UnknownAltitude As Integer = -1, Optional p13 As Integer = -1)
        [Call]("TASK_HELI_MISSION", Ped, Heli, p3_Usually_0, UnknownRole, Destination.X, Destination.Y, Destination.Z, p8_0_to_19, Speed, p10_0_to_30, p11_Up_To_90, UnknownAltitude, p13)
    End Sub
    Shared Sub TaskTurnToFaceChar(Ped1 As Ped, Ped2 As Ped)
        [Call]("TASK_TURN_CHAR_TO_FACE_CHAR", Ped1, Ped2)
    End Sub
    Shared Sub TaskGoTo(Ped As Ped, Coordinate As Vector3, MoveType As MovementType, RadiusOfPoint As Single, Heading As Single, Optional Unknown As Single = -2)
        [Call]("TASK_FOLLOW_NAV_MESH_AND_SLIDE_TO_COORD", Ped, Coordinate.X, Coordinate.Y, Coordinate.Z, MoveType, Unknown, RadiusOfPoint, Heading)
    End Sub
    Shared Sub TaskGoToPedAiming(Ped1 As Ped, Ped2 As Ped, Flag1 As Boolean, Flag2 As Boolean)
        [Call]("TASK_GOTO_CHAR_AIMING", Ped1, Ped2, Flag1, Flag2)
    End Sub
    Public Shared Sub TaskOpenCarDoor(Ped As Ped, Car As Vehicle, Optional PassengerDoor As Boolean = False)
        If PassengerDoor Then [Call]("TASK_OPEN_PASSENGER_DOOR", Ped, Car) Else [Call]("TASK_OPEN_DRIVER_DOOR", Ped, Car)
    End Sub 'If PassengerDoor=False, the player will prefer to open the driver door, but it will open also the passenger one if closer. If PassengerDoor=True, the player will only open the passenger door
    Public Shared Sub TaskSlideToCoord(Ped As Ped, Coordinate As Vector3, Heading As Single, SpeedMultiplier As Single)
        [Call]("TASK_CHAR_SLIDE_TO_COORD", Ped, Coordinate.X, Coordinate.Y, Coordinate.Z, Heading, SpeedMultiplier)
    End Sub
    Public Shared Sub TaskCrouch(Ped As Ped, Crouch As Boolean)
        [Call]("SET_CHAR_DUCKING", Ped, Crouch)
    End Sub
    Public Shared Function isPedCrouching(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_DUCKING", Ped)
    End Function
    Shared Function isPedInjured(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_INJURED", Ped)
    End Function
    Shared Function isPedOnFire(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_ON_FIRE", Ped)
    End Function
    Shared Function isPedOnFoot(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_ON_FOOT", Ped)
    End Function
    Shared Function isPedShooting(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_SHOOTING", Ped)
    End Function
    Shared Function isPedSittingIdle(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_SITTING_IDLE", Ped)
    End Function
    Shared Function isPedStuckedUnderCar(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_STUCK_UNDER_CAR", Ped)
    End Function
    Public Shared Sub TaskSitDown(Ped As Ped, SittingType As Integer, GetUpType As Integer, Optional Time As Integer = -1)
        [Call]("TASK_SIT_DOWN", Ped, SittingType, GetUpType, Time)
    End Sub
    Public Shared Sub TaskSitDownInstantly(Ped As Ped, SittingType As Integer, GetUpType As Integer, Optional FacingDirection As Single = 180, Optional Time As Integer = -1)
        Ped.Heading += FacingDirection
        [Call]("TASK_SIT_DOWN_INSTANTLY", Ped, SittingType, GetUpType, Time)
    End Sub
    Public Shared Sub TaskSitDownOnSeat(Ped As Ped, SeatPosition As Vector3, FlagAdjustPos As Boolean, FacingAngle As Single, Optional Time As Integer = -2)
        [Call]("TASK_SIT_DOWN_ON_SEAT", Ped, False, FlagAdjustPos, SeatPosition.X, SeatPosition.Y, SeatPosition.Z, FacingAngle, Time)
    End Sub 'If Time (ms) > 0 the ped will automatically stand up
    Public Shared Sub TaskPlayerStandUp(Player As Player)
        Player.CanControlCharacter = False
        Game.WaitInCurrentScript(1)
        Player.CanControlCharacter = True
    End Sub

    '----------------------------------------------------------------
    'Speeches and Screams
    '----------------------
    Shared Sub CancelAmbientSpeech(Ped As Ped)
        [Call]("CANCEL_CURRENTLY_PLAYING_AMBIENT_SPEECH", Ped)
    End Sub
    Shared Function isAmbientSpeechPlaying(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_AMBIENT_SPEECH_PLAYING", Ped)
    End Function
    Shared Function isAnySpeechPlaying(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_ANY_SPEECH_PLAYING", Ped)
    End Function
    Shared Sub SayAmbientSpeech(Ped As Ped, SpeechName As String, Delay As Boolean, Optional Voice As String = "", Optional CancelMegaphone As Integer = 1)
        Dim p3, p4
        If Delay = False Then p3 = 1 And p4 = 0 Else p3 = 0 And p4 = 1
        If Voice = "" Then [Call]("SAY_AMBIENT_SPEECH", Ped, SpeechName, p3, p4, CancelMegaphone) Else [Call]("SAY_AMBIENT_SPEECH_WITH_VOICE", Ped, SpeechName, Voice, p3, p4, CancelMegaphone)
    End Sub
    Shared Sub SayPanicScream(Ped As Ped, ScreamType As PedScreams)
        If ScreamType = PedScreams.PanicScreamNotNiko Then
            [Call]("PANIC_SCREAM", Ped)
        ElseIf ScreamType = PedScreams.OnFire Then
            [Call]("ON_FIRE_SCREAM", Ped)
        Else
            [Call]("HIGH_FALL_SCREAM", Ped)
        End If
    End Sub
    Shared Sub SetAmbientVoiceName(Ped As Ped, AudioName As String)
        [Call]("SET_AMBIENT_VOICE_NAME", Ped, AudioName)
    End Sub
    '----------------------------------------------------------------
    'Misc
    '------
    'Shared Sub getCharAllWeapons(Player As Ped)
    '    For slot = 0 To 10
    '        'getSingleWeapon(Player, i)
    '        GTA.Weapon(slot) = Player.Weapons.inSlot(slot)
    '        Ammo(slot) = Player.Weapons.inSlot(slot).Ammo
    '        clipAmmo(slot) = Player.Weapons.inSlot(slot).AmmoInClip
    '    Next
    'End Sub
    'Shared Sub GiveAllWeaponToChar(Player As Ped)
    '    For Slot = 0 To 10
    '        Player.Weapons.Select(Weapon(Slot))
    '        Player.Weapons.inSlot(Slot).Ammo = Ammo(Slot)
    '        Player.Weapons.inSlot(Slot).AmmoInClip = clipAmmo(Slot)
    '    Next
    'End Sub
    Shared Sub BlockAmbientAnims(Ped As Ped, BlockAnim As Boolean)
        [Call]("BLOCK_CHAR_AMBIENT_ANIMS", Ped, BlockAnim)
    End Sub
    Shared Sub BlockGestureAnims(Ped As Ped, BlockAnim As Boolean)
        [Call]("BLOCK_CHAR_GESTURE_ANIMS", Ped, BlockAnim)
    End Sub
    Shared Sub BlockHeadIK(Ped As Ped, BlockAnim As Boolean)
        [Call]("BLOCK_CHAR_HEAD_IK", Ped, BlockAnim)
    End Sub
    Shared Sub BlockFacialAnims(Ped As Ped, BlockAnim As Boolean)
        [Call]("BLOCK_CHAR_VISEME_ANIMS", Ped, BlockAnim)
    End Sub
    Shared Sub BlockPeekingInCover(Ped As Ped, BlockAnim As Boolean)
        [Call]("BLOCK_PEEKING_IN_COVER", Ped, BlockAnim)
    End Sub
    Shared Function CanPedSeeDeadChar(Ped As Ped, DeadChar As Ped) As Boolean
        Return [Call](Of Boolean)("CAN_CHAR_SEE_DEAD_CHAR", Ped, DeadChar)
    End Function
    Shared Function getCurrentVehicleSeat(Ped As Ped) As GTA.VehicleSeat
        Dim CurrentVehicleSeat
        Dim NotInCar As String = "Not In Vehicle"
        If Ped.isSittingInVehicle Then
            If Ped = Ped.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) Then
                CurrentVehicleSeat = VehicleSeat.Driver
            ElseIf Ped = Ped.CurrentVehicle.GetPedOnSeat(VehicleSeat.RightFront) Then
                CurrentVehicleSeat = VehicleSeat.RightFront
            ElseIf Ped = Ped.CurrentVehicle.GetPedOnSeat(VehicleSeat.LeftRear) Then
                CurrentVehicleSeat = VehicleSeat.LeftRear
            Else
                CurrentVehicleSeat = VehicleSeat.RightRear
            End If
            Return CurrentVehicleSeat
        Else
            Return 99
        End If
    End Function
    Shared Function getNumberOfInjuredPeds(Position As Vector3, Radius As Single) As Integer
        Return [Call](Of Integer)("GET_NUMBER_OF_INJURED_PEDS_IN_RANGE", Position.X, Position.Y, Position.Z, Radius)
    End Function
End Class