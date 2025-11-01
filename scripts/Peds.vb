Imports System.Diagnostics.Eventing.Reader
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports System.Runtime.Remoting.Messaging
Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Imports GTA.value
Imports NativeExtended.NativeGeneric
Imports NativeExtended.NativePeds.Animations
Imports NativeExtended.NativePeds.Clothes

Public Class NativePeds
    ''' <summary>
    ''' They are not real native value, only used in many function to select
    ''' the ped's gender while spawning.
    ''' </summary>
    Public Enum Gender
        Male = 0
        Female = 1
        Random = -1
    End Enum
    ''' <summary>
    ''' The value "None = -1" is not a real native value, only used in
    ''' many functions to use the default Movement Type.
    ''' </summary>
    Public Enum MovementType
        Walk = 2
        Run = 3
        Sprint = 4
        None = -1
    End Enum
    Public Enum RomanMood
        Normal = 0
        Sad = 1
        ShakenUp = 2
        Drunk = 3
    End Enum
    '----------------------------
    Public Shared Sub AttachToShimmyEdge(Ped As Ped, Position As Vector3, Unknown As Double)
        [Call]("ATTACH_PED_TO_SHIMMY_EDGE", Ped, Position.X, Position.Y, Position.Z, Unknown)
    End Sub
    Public Shared Sub CanLookAtDuringConversation(Ped As Ped, ConversationID As Integer)
        [Call]("ALLOW_AUTO_CONVERSATION_LOOKATS", Ped, ConversationID)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Ped"></param>
    ''' <param name="Alpha"></param>
    ''' Values between 0 (fully transparent) and 255 (fully visible).
    Public Shared Sub SetAlpha(Ped As Ped, Alpha As Byte)
        [Call]("SET_PED_ALPHA", Ped, CInt(Alpha))
    End Sub
    Public Shared Sub ReviveInjuredPed(Ped As Ped)
        [Call]("REVIVE_INJURED_PED", Ped)
    End Sub
    Public Shared Sub SetAllowedReactionAnims(Ped As Ped)
        [Call]("ALLOW_REACTION_ANIMS", Ped)
    End Sub
    Public Shared Sub CanCrouch(Ped As Ped, CrouchAllowed As Boolean)
        [Call]("SET_CHAR_ALLOWED_TO_DUCK", Ped, CrouchAllowed)
    End Sub
    Public Shared Sub SetDrunk(Ped As Ped, Drunk As Boolean)
        [Call]("SET_PED_IS_DRUNK", Ped, Drunk)
    End Sub
    Public Shared Sub SetMoveSpeed(Ped As Ped, Multiplier As Single)
        [Call]("SET_CHAR_MOVE_ANIM_SPEED_MULTIPLIER", Ped, Multiplier)
    End Sub
    ''' <summary>
    ''' Not working?
    ''' </summary>
    ''' <param name="Ped"></param>
    ''' <param name="MoveStyle"></param>
    ''' Use the enum MovementType (not the value "None = -1")
    Public Shared Sub SetMoveStyle(Ped As Ped, MoveStyle As MovementType)
        [Call]("MODIFY_CHAR_MOVE_STATE", Ped, MoveStyle)
    End Sub 'not working?
    Public Shared Sub SetPaths(Ped As Ped, ClimbOvers As Boolean, DropFromHeight As Boolean, Ladders As Boolean)
        [Call]("SET_PED_PATH_MAY_USE_CLIMBOVERS", Ped, ClimbOvers)
        [Call]("SET_PED_PATH_MAY_DROP_FROM_HEIGHT", Ped, DropFromHeight)
        [Call]("SET_PED_PATH_MAY_USE_LADDERS", Ped, ClimbOvers)
    End Sub
    Public Shared Sub SedProvideCoveringFire(Ped As Ped, CoveringFire As Boolean)
        [Call]("SET_CHAR_PROVIDE_COVERING_FIRE", Ped, CoveringFire)
    End Sub
    Public Shared Function isTryingToEnterInLockedCar(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_TRYING_TO_ENTER_A_LOCKED_CAR", Ped)
    End Function
    Public Shared Sub SetSenseRange(Ped As Ped, Range As Single)
        [Call]("SET_SENSE_RANGE", Ped, Range)
    End Sub
    Public Shared Sub SetSwimSpeed(Ped As Ped, Speed As Single)
        [Call]("SET_SWIM_SPEED", Ped, Speed)
    End Sub
    Public Shared Sub SetMoveWhenInjured(Ped As Ped, WillMove As Boolean)
        [Call]("SET_CHAR_WILL_MOVE_WHEN_INJURED", Ped, WillMove)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    ''' <param name="Ped"></param>
    ''' <param name="AnimSet"></param>
    ''' <param name="AnimName"></param>
    ''' <param name="UnknownInt"></param>
    ''' <param name="Offsets"></param>
    Public Shared Sub BlendFromNMWithAnim(Ped As Ped, AnimSet As String, AnimName As String, UnknownInt As Integer, Offsets As Vector3)
        [Call]("BLEND_FROM_NM_WITH_ANIM", Ped, AnimSet, AnimName, UnknownInt, Offsets.X, Offsets.Y, Offsets.Z)
    End Sub
    'Public Shared Function SpawnPed(Position As Vector3, Model As String, Optional RelationshipGroup As RelationshipGroup = RelationshipGroup.Player, Optional MsgText As String = "", Optional MsgTime As Integer = 2000, Optional LeftUpMsgType As Boolean = False) As Ped
    '    Dim SpawnedPed As Ped
    '    Dim TempPedPointer As Native.Pointer = New Native.Pointer(GetType(Ped))
    '    [Call]("REQUEST_MODEL", NativeModels.getHashKey(Model))
    '    'Do
    '    '    Game.WaitInCurrentScript(1)
    '    'Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", NativeGeneric.getHashKey(Model))
    '    SpawnedPed = World.CreatePed(Model, Position, RelationshipGroup)
    '    ' [Call]("CREATE_CHAR", 1, NativeModels.getHashKey(Model), Position.X, Position.Y, Position.Z, TempPedPointer, True)
    '    ' SpawnedPed = TempPedPointer.Value
    '    ' If Visible = False Then SpawnedPed.Visible = False Else SpawnedPed.Visible = True
    '    If SpawnedPed.Exists Then Return SpawnedPed
    'End Function
    'Public Shared Function SpawnPedInvisible(Position As Vector3, Optional RelationshipGroup As RelationshipGroup = RelationshipGroup.Player) As Ped
    '    Dim InvisiblePed As Ped
    '    Dim TempPedPointer As Native.Pointer = New Native.Pointer(GetType(Ped))
    '    [Call]("REQUEST_MODEL", NativeModels.getHashKey("superlod"))
    '    'Do
    '    '    Game.WaitInCurrentScript(1)
    '    'Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", NativeGeneric.getHashKey("superlod"))
    '    InvisiblePed = World.CreatePed("superlod", Position, RelationshipGroup)
    '    '[Call]("CREATE_CHAR", 1, NativeModels.getHashKey("superlod"), Position.X, Position.Y, Position.Z, TempPedPointer, True)
    '    InvisiblePed = TempPedPointer.Value
    '    InvisiblePed.Visible = False
    '    Return InvisiblePed
    'End Function
    Public Shared Function SpawnRandomPed(Position As GTA.Vector3, Optional Gender As Gender = Gender.Random)
        Dim SpawnedPed As Native.Pointer = New Native.Pointer(GetType(Ped))
        Select Case Gender
            Case 0
                [Call](Of Ped)("CREATE_RANDOM_MALE_CHAR", Position.X, Position.Y, Position.Z, SpawnedPed)
                Return SpawnedPed.Value
            Case 1
                [Call](Of Ped)("CREATE_RANDOM_FEMALE_CHAR", Position.X, Position.Y, Position.Z, SpawnedPed)
                Return SpawnedPed.Value
            Case Else
                [Call](Of Ped)("CREATE_RANDOM_CHAR", Position.X, Position.Y, Position.Z, SpawnedPed)
                Return SpawnedPed.Value
        End Select
    End Function
    Public Shared Function SpawnPedInsideVehicle(Car As Vehicle, Seat As VehicleSeat) As Ped
        Dim SpawnedPed As Native.Pointer = New Native.Pointer(GetType(Ped))
        If Seat = VehicleSeat.Driver Then
            [Call](Of Ped)("CREATE_RANDOM_CHAR_AS_DRIVER", Car, SpawnedPed)
            Return SpawnedPed.Value
        Else
            [Call](Of Ped)("CREATE_RANDOM_CHAR_AS_PASSENGER", Car, Seat, SpawnedPed)
            Return SpawnedPed.Value
        End If
        'Return [Call](Of Ped)("CREATE_RANDOM_CHAR",)
    End Function
    Public Shared Sub ForcePinnedDown(Ped As Ped, PinnedDown As Boolean, Time As Integer)
        [Call]("FORCE_PED_PINNED_DOWN", Ped, PinnedDown, Time)
    End Sub
    Public Shared Sub DeletePed(Ped As Ped, Optional Elegantly As Boolean = True)
        If Elegantly Then [Call]("REMOVE_CHAR_ELEGANTLY", Ped) Else [Call]("DELETE_CHAR", Ped)
    End Sub
    Public Shared Sub SetDropWeaponsWhenDead(Ped As Ped, Optional DropWeapons As Boolean = True)
        [Call]("SET_CHAR_DROPS_WEAPONS_WHEN_DEAD", Ped, DropWeapons)

    End Sub
    Public Shared Sub SetSuffersCriticalHits(Ped As Ped, Suffers As Boolean)
        [Call]("SET_CHAR_SUFFERS_CRITICAL_HITS", Ped, Suffers)
    End Sub
    Public Shared Sub CanLeaveCarDuringCombat(Ped As Ped, CanLeaveCar As Boolean)
        [Call]("SET_CHAR_WILL_LEAVE_CAR_IN_COMBAT", Ped, CanLeaveCar)
    End Sub
    Public Shared Function isFatallyInjured(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_FATALLY_INJURED", Ped)
    End Function
    Public Shared Function isGettingUp(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_GETTING_UP", Ped)
    End Function
    Public Shared Function isFacingChar(Ped As Ped, PedToLook As Ped, Angle As Double) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_FACING_CHAR", Ped, PedToLook, Angle)
    End Function
    Public Shared Function getGender(Ped As Ped) As Gender
        If [Call](Of Boolean)("IS_CHAR_MALE", Ped) Then Return Gender.Male Else Return Gender.Female
    End Function
    Public Shared Function isInTaxi(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_IN_TAXI", Ped)
    End Function
    Public Shared Function isStuckedUnderCar(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_STUCK_UNDER_CAR", Ped)
    End Function
    Public Shared Sub BlockPeekingInCover(Ped As Ped, Optional BlockCover As Boolean = True)
        [Call]("BLOCK_PEEKING_IN_COVER", Ped, BlockCover)
    End Sub
    Public Shared Function CanPedSeeDeadChar(Ped As Ped, DeadChar As Ped) As Boolean
        Return [Call](Of Boolean)("CAN_CHAR_SEE_DEAD_CHAR", Ped, DeadChar)
    End Function
    Public Shared Function getCurrentVehicleSeat(Ped As Ped) As GTA.VehicleSeat
        Dim CurrentVehicleSeat
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
    ''' <summary>
    ''' Returns the height (in meters) of the specified entity above the ground level.
    ''' </summary>
    ''' <returns>Height above the ground in meters. Returns 0 if ground not found.</returns>
    Public Shared Function getHeightAboveGround(Ped As Ped) As Single
        Try
            If Ped Is Nothing OrElse Not Ped.Exists Then Return 0.0F

            Dim pos As GTA.Vector3 = Ped.Position
            Dim groundZ As Single = 0.0F
            Dim pointerGround As Native.Pointer = New Native.Pointer(GetType(Single))
            [Call](Of Boolean)("GET_GROUND_Z_FOR_3D_COORD", pos.X, pos.Y, pos.Z, pointerGround)

            If pointerGround.Value > 0 Then
                Dim height As Single = pos.Z - pointerGround.Value
                Return If(height < 0, 0, height) ' evita valori negativi per sicurezza
            Else
                Return 0.0F
            End If

        Catch ex As Exception
            Return 0.0F
        End Try
    End Function
    Public Shared Function getNumberOfInjuredPeds(Position As Vector3, Radius As Single) As Integer
        Return [Call](Of Integer)("GET_NUMBER_OF_INJURED_PEDS_IN_RANGE", Position.X, Position.Y, Position.Z, Radius)
    End Function
    Public Shared Sub SetHeadOnHornWhenDeadInVehicle(Ped As Ped, AlwaysHeadOnHorn As Boolean)
        [Call]("ALWAYS_USE_HEAD_ON_HORN_ANIM_WHEN_DEAD_IN_CAR", Ped, AlwaysHeadOnHorn)
    End Sub
    Public Shared Sub SetDoEvasiveDives(Ped As Ped, DoEvasiveDives As Boolean)
        [Call]("SET_PED_DONT_DO_EVASIVE_DIVES", Ped, Not DoEvasiveDives)
    End Sub
    Public Shared Sub SetDrugged(Ped As Ped, Drugged As Boolean)
        [Call]("SET_CHAR_DRUGGED_UP", Ped, Drugged)
    End Sub
    Public Shared Sub SetDrownsInWater(Ped As Ped, Drowns As Boolean, DrownsInSinkingVehicle As Boolean)
        [Call]("SET_CHAR_DROWNS_IN_WATER", Ped, Drowns)
        [Call]("SET_CHAR_DROWNS_IN_SINKING_VEHICLE", Ped, DrownsInSinkingVehicle)
    End Sub
    Public Shared Sub SetNeverGetTargettedByPlayer(Ped As Ped, Optional NeverTargetted As Boolean = True)
        Native.Function.Call("SET_CHAR_NEVER_TARGETTED", Ped, NeverTargetted)
    End Sub
    Public Shared Sub SetNoRagdollFromPlayerImpact(Ped As Ped, NoRagdoll As Boolean)
        [Call]("SET_DONT_ACTIVATE_RAGDOLL_FROM_PLAYER_IMPACT", Ped, NoRagdoll)

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Ped"></param>
    ''' <param name="Rate"></param>
    ''' Values between 0 and 100
    Public Shared Sub SetShootRate(Ped As Ped, Rate As Integer)
        [Call]("SET_CHAR_SHOOT_RATE", Ped, Rate)
    End Sub
    Public Shared Sub SetBleeding(Ped As Ped, Bleeding As Boolean)
        [Call]("SET_CHAR_BLEEDING", Ped, Bleeding)
    End Sub
    Public Shared Sub DamageBodyPart(Ped As Ped, BodyPart As UInteger, HitPoints As UInteger)
        [Call]("DAMAGE_PED_BODY_PART", Ped, BodyPart, HitPoints)
    End Sub
    Public Shared Sub SetRomansMood(Mood As RomanMood)
        [Call]("SET_ROMANS_MOOD", Mood)
    End Sub
    'GET_CHAR_SWIM_STATE
    'GIVE_PED_AMBIENT_OBJECT(ped, Hashkey as integer, unknownbool) 'load object first
    ' GIVE_PED_PICKUP_OBJECT(ped, object, unknownbool) 'load object first
    'SET_CHAR_FIRE_DAMAGE_MULTIPLIER

    ' ADD_FOLLOW_NAVMESH_TO_PHONE_TASK( l_U481, 478.60000000, 1564.86000000, 15.22110000 ); jacob2 and elizabeta3 -->in elizabeta 3 fir par is player index

    Public Class Animations
        Public Structure AnimGroups
            'Male
            Public Shared ReadOnly MoveMaleCop As String = "move_cop"
            Public Shared ReadOnly MoveMaleCopFat As String = "move_cop_fat"
            Public Shared ReadOnly MoveMaleRomanInj As String = "move_m@roman_inj"
            Public Shared ReadOnly MoveMaleBernie As String = "move_m@bernie"
            Public Shared ReadOnly MoveMalePlayboy As String = "move_m@playboy"
            Public Shared ReadOnly MoveMaleCasual As String = "move_m@casual"
            Public Shared ReadOnly MoveMaleCasualB As String = "move_m@casual_b"
            Public Shared ReadOnly MoveMaleCasualC As String = "move_m@casual_c"
            Public Shared ReadOnly MoveMaleBum As String = "move_m@bum"
            Public Shared ReadOnly MoveMaleFat As String = "move_m@fat"
            Public Shared ReadOnly MoveMaleOldA As String = "move_m@old_a"
            Public Shared ReadOnly MoveMaleOldB As String = "move_m@old_b"
            Public Shared ReadOnly MoveMaleOldC As String = "move_m@old_c"
            Public Shared ReadOnly MoveMaleTourist As String = "move_m@tourist"
            Public Shared ReadOnly MoveMaleCase As String = "move_m@case"
            Public Shared ReadOnly MoveMaleHCuffed As String = "move_m@h_cuffed"
            Public Shared ReadOnly MoveMaleEddie As String = "move_m@eddie"
            Public Shared ReadOnly MoveMaleMultiplyr As String = "move_m@multiplyr"
            Public Shared ReadOnly MoveMaleCutsceneSwat As String = "move_m@cs_swat"
            Public Shared ReadOnly MoveMaleBnessA As String = "move_m@bness_a"
            Public Shared ReadOnly MoveMaleBnessB As String = "move_m@bness_b"
            Public Shared ReadOnly MoveMaleBnessC As String = "move_m@bness_c"

            'Female
            Public Shared ReadOnly MoveFemaleBnessA As String = "move_f@bness_a"
            Public Shared ReadOnly MoveFemaleBnessB As String = "move_f@bness_b"
            Public Shared ReadOnly MoveFemaleBnessC As String = "move_f@bness_c"
            Public Shared ReadOnly MoveFemaleBnessD As String = "move_f@bness_d"
            Public Shared ReadOnly MoveFemaleBnessE As String = "move_f@bness_e"
            Public Shared ReadOnly MoveFemaleCasual As String = "move_f@casual"
            Public Shared ReadOnly MoveFemaleCasualB As String = "move_f@casual_b"
            Public Shared ReadOnly MoveFemaleCasualC As String = "move_f@casual_c"
            Public Shared ReadOnly MoveFemaleSexy As String = "move_f@sexy"
            Public Shared ReadOnly MoveFemaleFat As String = "move_f@fat"
            Public Shared ReadOnly MoveFemaleOldA As String = "move_f@old_a"
            Public Shared ReadOnly MoveFemaleOldB As String = "move_f@old_b"
            Public Shared ReadOnly MoveFemaleOldC As String = "move_f@old_c"
            Public Shared ReadOnly MoveFemaleOldD As String = "move_f@old_d"
            Public Shared ReadOnly MoveFemalePuffer As String = "move_f@puffer"
            Public Shared ReadOnly MoveFemaleMichelle As String = "move_f@michelle"
            Public Shared ReadOnly MoveFemaleMultiplyr As String = "move_f@multiplyr"

            'Gang
            Public Shared ReadOnly MoveGangAfroA As String = "move_gng@afro_a"
            Public Shared ReadOnly MoveGangAfroB As String = "move_gng@afro_b"
            Public Shared ReadOnly MoveGangAfroC As String = "move_gng@afro_c"
            Public Shared ReadOnly MoveGangGenericA As String = "move_gng@generic_a"
            Public Shared ReadOnly MoveGangGenericB As String = "move_gng@generic_b"
            Public Shared ReadOnly MoveGangGenericC As String = "move_gng@generic_c"
            Public Shared ReadOnly MoveGangLatinoA As String = "move_gng@latino_a"
            Public Shared ReadOnly MoveGangLatinoB As String = "move_gng@latino_b"
            Public Shared ReadOnly MoveGangLatinoC As String = "move_gng@latino_c"
            Public Shared ReadOnly MoveGangJamA As String = "move_gng@jam_a"
            Public Shared ReadOnly MoveGangJamB As String = "move_gng@jam_b"
            Public Shared ReadOnly MoveGangJamC As String = "move_gng@jam_c"
        End Structure
        Public Shared Sub BlendOutMoveAnims(Ped As Ped)
            [Call]("BLEND_OUT_CHAR_MOVE_ANIMS", Ped)
        End Sub
        Public Shared Sub Play(ByVal Ped As Ped, ByVal AnimSet As String, ByVal AnimName As String, ByVal Speed As Single, Optional IsLoop As Boolean = False, Optional InsideVehicle As Boolean = False, Optional CanWalk As Boolean = False, Optional Duration As Integer = -1,
                                       Optional FreezePlayerControl As Boolean = False, Optional FreezeOnLastAnimFrame As Boolean = False, Optional ComeBackToOriginalPosition As Boolean = False, Optional FreezeCamDuringAnim As Boolean = False, Optional ReturnToLastAnimation As Boolean = False)
            [Call]("REQUEST_ANIMS", AnimSet)

            If InsideVehicle Then
                [Call]("TASK_PLAY_ANIM_SECONDARY_IN_CAR", Ped, AnimName, AnimSet, CSng(Speed), IsLoop, FreezePlayerControl, FreezePlayerControl, FreezeOnLastAnimFrame, Duration)
            ElseIf CanWalk Then
                [Call]("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY", Ped, AnimName, AnimSet, CSng(Speed), IsLoop, FreezePlayerControl, FreezePlayerControl, FreezeOnLastAnimFrame, Duration)
            Else
                [Call]("TASK_PLAY_ANIM_WITH_ADVANCED_FLAGS", Ped, AnimName, AnimSet, CSng(Speed), IsLoop, ReturnToLastAnimation, ReturnToLastAnimation, ComeBackToOriginalPosition, FreezeOnLastAnimFrame, ComeBackToOriginalPosition, FreezeCamDuringAnim, Duration)
            End If
        End Sub
        Public Shared Sub [Stop](ByVal Ped As Ped, OnlyUpperBody As Boolean, Optional StopLoop As Boolean = False)
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
        Public Shared Function haveAnimsLoaded(AnimSet As String) As Boolean
            Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
        End Function
        Public Shared Function isPlayingAnim(ByVal Ped As Ped, ByVal AnimSet As String, ByVal AnimName As String) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_PLAYING_ANIM", Ped, AnimSet, AnimName)
        End Function
        Public Shared Sub SetAnimCurrentTime(Ped As Ped, AnimSet As String, AnimName As String, Speed As Single)
            [Call]("SET_CHAR_ANIM_CURRENT_TIME", Ped, AnimSet, AnimName, Speed)
        End Sub
        Public Shared Sub SetAnimGroup(Ped As Ped, AnimGroup As String)
            [Call]("REQUEST_ANIMS", AnimGroup)
            'Do
            '    Game.WaitInCurrentScript(1)
            'Loop Until [Call](Of Boolean)("HAS_ANIMS_LOADED")
            [Call]("SET_ANIM_GROUP_FOR_CHAR", Ped, AnimGroup)
        End Sub
        Public Shared Sub SetAnimSpeed(Ped As Ped, AnimSet As String, AnimName As String, Speed As Single)
            [Call]("SET_CHAR_ANIM_SPEED", Ped, AnimSet, AnimName, Speed)
        End Sub
        Public Shared Sub SetAllAnimSpeed(Ped As Ped, Speed As Single)
            [Call]("SET_CHAR_ALL_ANIMS_SPEED", Ped, Speed)
        End Sub
        Public Shared Sub BlockAmbientAnims(Ped As Ped, BlockAnim As Boolean)
            [Call]("BLOCK_CHAR_AMBIENT_ANIMS", Ped, BlockAnim)
        End Sub
        Public Shared Sub BlockHeadIK(Ped As Ped, Optional BlockIK As Boolean = True)
            [Call]("BLOCK_CHAR_HEAD_IK", Ped, BlockIK)
        End Sub
        Public Shared Sub BlockLegIK(Ped As Ped, Optional BlockIK As Boolean = True)
            [Call]("SET_PED_ENABLE_LEG_IK", Ped, Not BlockIK)
        End Sub
        Public Shared Sub Relase(AnimSet As String)
            [Call]("REMOVE_ANIMS", AnimSet)
        End Sub
        Public Shared Sub Request(AnimSet As String)
            [Call]("REQUEST_ANIMS", AnimSet)
        End Sub
        Public Class Gestures
            Public ReadOnly AnimSet As String
            Public ReadOnly AnimName As String
            Public Sub New(AnimSet As String, AnimName As String)
                Me.AnimSet = AnimSet
                Me.AnimName = AnimName
            End Sub
            Public Shared Sub BlockGestures(Ped As Ped, BlockAnim As Boolean)
                [Call]("BLOCK_CHAR_GESTURE_ANIMS", Ped, BlockAnim)
            End Sub
            Public Shared Function haveGesturesLoaded(GestureSet As String) As Boolean
                Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", GestureSet)
            End Function
            Public Shared Function isGesturing(Ped As Ped) As Boolean
                Return [Call](Of Boolean)("IS_CHAR_GESTURING", Ped)
            End Function
            Public Shared Sub Play(Ped As Ped, Gesture As Gestures, Optional Speed As Single = 1.0)
                [Call]("REQUEST_ANIMS", Gesture.AnimSet)
                'Do
                '    Game.WaitInCurrentScript(1)
                'Loop Until [Call](Of Boolean)("HAS_ANIMS_LOADED")
                If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
                    [Call]("TASK_PLAY_ANIM_SECONDARY_IN_CAR", Ped, Gesture.AnimName, Gesture.AnimSet, Speed, False, False, False, False)
                Else
                    [Call]("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY", Ped, Gesture.AnimName, Gesture.AnimSet, Speed, False, False, False, False)
                End If
            End Sub
            Public Shared Sub Relase(GestureSet As String)
                [Call]("REMOVE_ANIMS", GestureSet)
            End Sub
            Public Shared Sub Request(GestureSet As String)
                [Call]("REQUEST_ANIMS", GestureSet)
            End Sub
            Public Structure HandMale
                Public Const AnimSet As String = "gestures@mp_male"

                Public Shared ReadOnly Finger As New Gestures(AnimSet, "finger")
                Public Shared ReadOnly Rock As New Gestures(AnimSet, "rock")
                Public Shared ReadOnly Hello As New Gestures(AnimSet, "wave")
                Public Shared ReadOnly Salute As New Gestures(AnimSet, "salute")
            End Structure
            Public Structure HandFemale
                Public Const AnimSet As String = "gestures@mp_female"

                Public Shared ReadOnly Finger As New Gestures(AnimSet, "finger")
                Public Shared ReadOnly Rock As New Gestures(AnimSet, "rock")
                Public Shared ReadOnly Hello As New Gestures(AnimSet, "wave")
                Public Shared ReadOnly Salute As New Gestures(AnimSet, "salute")
            End Structure
            Public Structure Male
                Public Const AnimSet As String = "gestures@male"

                Public Shared ReadOnly Absolutely As New Gestures(AnimSet, "absolutely")
                Public Shared ReadOnly Agree As New Gestures(AnimSet, "agree")
                Public Shared ReadOnly Amazing As New Gestures(AnimSet, "amazing")
                Public Shared ReadOnly AngerA As New Gestures(AnimSet, "anger_a")
                Public Shared ReadOnly AreYouIn As New Gestures(AnimSet, "are_you_in")
                Public Shared ReadOnly BringItOn As New Gestures(AnimSet, "bring_it_on")
                Public Shared ReadOnly BringItToMe As New Gestures(AnimSet, "bring_it_to_me")
                Public Shared ReadOnly ButWhy As New Gestures(AnimSet, "but_why")
                Public Shared ReadOnly ComeHere As New Gestures(AnimSet, "come_here")
                Public Shared ReadOnly ComeOn As New Gestures(AnimSet, "come_on")
                Public Shared ReadOnly Damn As New Gestures(AnimSet, "damn")
                Public Shared ReadOnly Despair As New Gestures(AnimSet, "despair")
                Public Shared ReadOnly Disbelief As New Gestures(AnimSet, "disbelief")
                Public Shared ReadOnly DontHitMe As New Gestures(AnimSet, "dont_hit_me")
                Public Shared ReadOnly DontKnow As New Gestures(AnimSet, "dont_know")
                Public Shared ReadOnly DoIt As New Gestures(AnimSet, "do_it")
                Public Shared ReadOnly EasyNow As New Gestures(AnimSet, "easy_now")
                Public Shared ReadOnly Enough As New Gestures(AnimSet, "enough")
                Public Shared ReadOnly Exactly As New Gestures(AnimSet, "exactly")
                Public Shared ReadOnly FoldArmsOhYeah As New Gestures(AnimSet, "fold_arms_oh_yeah")
                Public Shared ReadOnly ForgetIt As New Gestures(AnimSet, "forget_it")
                Public Shared ReadOnly GiveMeABreak As New Gestures(AnimSet, "give_me_a_break")
                Public Shared ReadOnly Goddamn As New Gestures(AnimSet, "goddamn")
                Public Shared ReadOnly Good As New Gestures(AnimSet, "good")
                Public Shared ReadOnly GoAway As New Gestures(AnimSet, "go_away")
                Public Shared ReadOnly Hello As New Gestures(AnimSet, "hello")
                Public Shared ReadOnly Hey As New Gestures(AnimSet, "hey")
                Public Shared ReadOnly How As New Gestures(AnimSet, "how")
                Public Shared ReadOnly HowCouldYou As New Gestures(AnimSet, "how_could_you")
                Public Shared ReadOnly HowMuch As New Gestures(AnimSet, "how_much")
                Public Shared ReadOnly IfUSaySo As New Gestures(AnimSet, "if_u_say_so")
                Public Shared ReadOnly IllDoIt As New Gestures(AnimSet, "ill_do_it")
                Public Shared ReadOnly ImTalking2You As New Gestures(AnimSet, "im talking_2_you")
                Public Shared ReadOnly ImBeggingYou As New Gestures(AnimSet, "im_begging_you")
                Public Shared ReadOnly ImNotSure As New Gestures(AnimSet, "im_not_sure")
                Public Shared ReadOnly ImSorry As New Gestures(AnimSet, "im_sorry")
                Public Shared ReadOnly ImTellingYou As New Gestures(AnimSet, "im_telling_you")
                Public Shared ReadOnly IndicateBack As New Gestures(AnimSet, "indicate_back")
                Public Shared ReadOnly IndicateLeft As New Gestures(AnimSet, "indicate_left")
                Public Shared ReadOnly IndicateListener As New Gestures(AnimSet, "indicate_listener")
                Public Shared ReadOnly IndicateRightB As New Gestures(AnimSet, "indicate_right_b")
                Public Shared ReadOnly IndicateRightC As New Gestures(AnimSet, "indicate_right_c")
                Public Shared ReadOnly IsThisIt As New Gestures(AnimSet, "is_this_it")
                Public Shared ReadOnly ItsDone As New Gestures(AnimSet, "its_done")
                Public Shared ReadOnly ItsMine As New Gestures(AnimSet, "its_mine")
                Public Shared ReadOnly ItsOk As New Gestures(AnimSet, "its_ok")
                Public Shared ReadOnly IveForgot As New Gestures(AnimSet, "ive_forgot")
                Public Shared ReadOnly ICantSay As New Gestures(AnimSet, "i_cant_say")
                Public Shared ReadOnly ICouldnt As New Gestures(AnimSet, "i_couldnt")
                Public Shared ReadOnly IDontHave As New Gestures(AnimSet, "i_dont_have")
                Public Shared ReadOnly IDontThinkSo As New Gestures(AnimSet, "i_dont_think_so")
                Public Shared ReadOnly IGetIt As New Gestures(AnimSet, "i_get_it")
                Public Shared ReadOnly IGiveUp As New Gestures(AnimSet, "i_give_up")
                Public Shared ReadOnly ISaidNo As New Gestures(AnimSet, "i_said_no")
                Public Shared ReadOnly IWill As New Gestures(AnimSet, "i_will")
                Public Shared ReadOnly KissMyAss As New Gestures(AnimSet, "kiss_my_ass")
                Public Shared ReadOnly Later As New Gestures(AnimSet, "later")
                Public Shared ReadOnly LeaveIt2Me As New Gestures(AnimSet, "leave_it_2_me")
                Public Shared ReadOnly LetMeThink As New Gestures(AnimSet, "let_me_think")
                Public Shared ReadOnly LikeThis As New Gestures(AnimSet, "like_this")
                Public Shared ReadOnly MeGest As New Gestures(AnimSet, "me")
                Public Shared ReadOnly Natuarally As New Gestures(AnimSet, "natuarally")
                Public Shared ReadOnly Negative As New Gestures(AnimSet, "negative")
                Public Shared ReadOnly NodNo As New Gestures(AnimSet, "nod_no")
                Public Shared ReadOnly NodYes As New Gestures(AnimSet, "nod_yes")
                Public Shared ReadOnly NotMe As New Gestures(AnimSet, "not_me")
                Public Shared ReadOnly NotSure As New Gestures(AnimSet, "not_sure")
                Public Shared ReadOnly NoChance As New Gestures(AnimSet, "no_chance")
                Public Shared ReadOnly NoReally As New Gestures(AnimSet, "no_really")
                Public Shared ReadOnly Numbnuts As New Gestures(AnimSet, "numbnuts")
                Public Shared ReadOnly OfCourse As New Gestures(AnimSet, "of_course")
                Public Shared ReadOnly OhShit As New Gestures(AnimSet, "oh_shit")
                Public Shared ReadOnly Ok As New Gestures(AnimSet, "ok")
                Public Shared ReadOnly OkOk As New Gestures(AnimSet, "ok_ok")
                Public Shared ReadOnly OverThere As New Gestures(AnimSet, "over_there")
                Public Shared ReadOnly PissOff As New Gestures(AnimSet, "piss_off")
                Public Shared ReadOnly Please As New Gestures(AnimSet, "please")
                Public Shared ReadOnly PointFwd As New Gestures(AnimSet, "point_fwd")
                Public Shared ReadOnly PointRight As New Gestures(AnimSet, "point_right")
                Public Shared ReadOnly Positive As New Gestures(AnimSet, "positive")
                Public Shared ReadOnly RaiseHands As New Gestures(AnimSet, "raise_hands")
                Public Shared ReadOnly SayAgain As New Gestures(AnimSet, "say_again")
                Public Shared ReadOnly ScrewYou As New Gestures(AnimSet, "screw_you")
                Public Shared ReadOnly Shit As New Gestures(AnimSet, "shit")
                Public Shared ReadOnly Shock As New Gestures(AnimSet, "shock")
                Public Shared ReadOnly ShutUp As New Gestures(AnimSet, "shut_up")
                Public Shared ReadOnly StopIt As New Gestures(AnimSet, "stop")
                Public Shared ReadOnly Sure As New Gestures(AnimSet, "sure")
                Public Shared ReadOnly TellMeAboutIt As New Gestures(AnimSet, "tell_me_about_it")
                Public Shared ReadOnly Test As New Gestures(AnimSet, "test")
                Public Shared ReadOnly That As New Gestures(AnimSet, "that")
                Public Shared ReadOnly ThatWay As New Gestures(AnimSet, "that_way")
                Public Shared ReadOnly ThisAndThat As New Gestures(AnimSet, "this_and_that")
                Public Shared ReadOnly ThisBig As New Gestures(AnimSet, "this_big")
                Public Shared ReadOnly Threaten As New Gestures(AnimSet, "threaten")
                Public Shared ReadOnly Time As New Gestures(AnimSet, "time")
                Public Shared ReadOnly Tosser As New Gestures(AnimSet, "tosser")
                Public Shared ReadOnly TouchFace As New Gestures(AnimSet, "touch_face")
                Public Shared ReadOnly ToHellWithIt As New Gestures(AnimSet, "to_hell_with_it")
                Public Shared ReadOnly Unbelievable As New Gestures(AnimSet, "unbelievable")
                Public Shared ReadOnly Uptight As New Gestures(AnimSet, "uptight")
                Public Shared ReadOnly UCantDoThat As New Gestures(AnimSet, "u_cant_do_that")
                Public Shared ReadOnly USerious As New Gestures(AnimSet, "u_serious")
                Public Shared ReadOnly UTalkin2Me As New Gestures(AnimSet, "u_talkin_2_me")
                Public Shared ReadOnly UThinImStupid As New Gestures(AnimSet, "u_thin_i'm_stupid")
                Public Shared ReadOnly UUnderstand As New Gestures(AnimSet, "u_understand")
                Public Shared ReadOnly WantSomeOfThis As New Gestures(AnimSet, "want_some_of_this")
                Public Shared ReadOnly We As New Gestures(AnimSet, "we")
                Public Shared ReadOnly Well As New Gestures(AnimSet, "well")
                Public Shared ReadOnly WellAlright As New Gestures(AnimSet, "well_alright")
                Public Shared ReadOnly WeCanDoIt As New Gestures(AnimSet, "we_can_do_it")
                Public Shared ReadOnly What As New Gestures(AnimSet, "what")
                Public Shared ReadOnly Whatever As New Gestures(AnimSet, "whatever")
                Public Shared ReadOnly WhateverC As New Gestures(AnimSet, "whatever_c")
                Public Shared ReadOnly Why As New Gestures(AnimSet, "why")
                Public Shared ReadOnly WotTheFuck As New Gestures(AnimSet, "wot_the_fuck")
                Public Shared ReadOnly YeahIGotIt As New Gestures(AnimSet, "yeah_i_got_it")
                Public Shared ReadOnly Yes As New Gestures(AnimSet, "yes")
                Public Shared ReadOnly YoureRight As New Gestures(AnimSet, "youre_right")
                Public Shared ReadOnly YouDig As New Gestures(AnimSet, "you_dig")
                Public Shared ReadOnly YouWillLovethis As New Gestures(AnimSet, "you_will_love this")
            End Structure
            Public Structure Female
                Public Const AnimSet As String = "gestures@female"

                Public Shared ReadOnly Absolutely As New Gestures(AnimSet, "absolutely")
                Public Shared ReadOnly Agree As New Gestures(AnimSet, "agree")
                Public Shared ReadOnly AllGone As New Gestures(AnimSet, "all_gone")
                Public Shared ReadOnly CantBe As New Gestures(AnimSet, "cant_be")
                Public Shared ReadOnly Clap As New Gestures(AnimSet, "clap")
                Public Shared ReadOnly DefinitelyNot As New Gestures(AnimSet, "definitely_not")
                Public Shared ReadOnly Dont As New Gestures(AnimSet, "dont")
                Public Shared ReadOnly DontEven As New Gestures(AnimSet, "dont_even")
                Public Shared ReadOnly DontKnow As New Gestures(AnimSet, "dont_know")
                Public Shared ReadOnly DontUDare As New Gestures(AnimSet, "dont_u_dare")
                Public Shared ReadOnly GetLost As New Gestures(AnimSet, "get_lost")
                Public Shared ReadOnly GetThisStraight As New Gestures(AnimSet, "get_this_straight")
                Public Shared ReadOnly Gimme As New Gestures(AnimSet, "gimme")
                Public Shared ReadOnly GoOver As New Gestures(AnimSet, "go_over")
                Public Shared ReadOnly HiYa As New Gestures(AnimSet, "hi_ya")
                Public Shared ReadOnly IndicateBwd As New Gestures(AnimSet, "indicate_bwd")
                Public Shared ReadOnly IndicateFwdA As New Gestures(AnimSet, "indicate_fwd_a")
                Public Shared ReadOnly IndicateFwdB As New Gestures(AnimSet, "indicate_fwd_b")
                Public Shared ReadOnly IndicateLeft As New Gestures(AnimSet, "indicate_left")
                Public Shared ReadOnly IndicateRight As New Gestures(AnimSet, "indicate_right")
                Public Shared ReadOnly ItsSoLike As New Gestures(AnimSet, "its_so_like")
                Public Shared ReadOnly ICant As New Gestures(AnimSet, "i_cant")
                Public Shared ReadOnly IDontCare As New Gestures(AnimSet, "i_dont_care")
                Public Shared ReadOnly IDontThinkSo As New Gestures(AnimSet, "i_dont_think_so")
                Public Shared ReadOnly IOughta As New Gestures(AnimSet, "i_oughta")
                Public Shared ReadOnly JustGo As New Gestures(AnimSet, "just_go")
                Public Shared ReadOnly LeaveIt As New Gestures(AnimSet, "leave_it")
                Public Shared ReadOnly LetMeTellU As New Gestures(AnimSet, "let_me_tell_u")
                Public Shared ReadOnly LookAtMe As New Gestures(AnimSet, "look_at_me")
                Public Shared ReadOnly MaybeU As New Gestures(AnimSet, "maybe_u")
                Public Shared ReadOnly MeGest As New Gestures(AnimSet, "me")
                Public Shared ReadOnly NeverOnYourLife As New Gestures(AnimSet, "never_on_your_life")
                Public Shared ReadOnly [No] As New Gestures(AnimSet, "no")
                Public Shared ReadOnly NoThanks As New Gestures(AnimSet, "no_thanks")
                Public Shared ReadOnly NoUCant As New Gestures(AnimSet, "no_u_cant")
                Public Shared ReadOnly NoWay As New Gestures(AnimSet, "no_way")
                Public Shared ReadOnly NoYourWrong As New Gestures(AnimSet, "no_your_wrong")
                Public Shared ReadOnly OfCoarse As New Gestures(AnimSet, "of_coarse")
                Public Shared ReadOnly OhComeOn As New Gestures(AnimSet, "oh_come_on")
                Public Shared ReadOnly OhMyGod As New Gestures(AnimSet, "oh_my_god")
                Public Shared ReadOnly OhNo As New Gestures(AnimSet, "oh_no")
                Public Shared ReadOnly Ok As New Gestures(AnimSet, "ok")
                Public Shared ReadOnly OverThere As New Gestures(AnimSet, "over_there")
                Public Shared ReadOnly Possibly As New Gestures(AnimSet, "possibly")
                Public Shared ReadOnly Shocked As New Gestures(AnimSet, "shocked")
                Public Shared ReadOnly SoTough As New Gestures(AnimSet, "so_tough")
                Public Shared ReadOnly Sure As New Gestures(AnimSet, "sure")
                Public Shared ReadOnly TakeIt As New Gestures(AnimSet, "take_it")
                Public Shared ReadOnly ThankU As New Gestures(AnimSet, "thank_u")
                Public Shared ReadOnly TheirSoFat As New Gestures(AnimSet, "their_so_fat")
                Public Shared ReadOnly ThisBig As New Gestures(AnimSet, "this_big")
                Public Shared ReadOnly ThisHigh As New Gestures(AnimSet, "this_high")
                Public Shared ReadOnly Threaten As New Gestures(AnimSet, "threaten")
                Public Shared ReadOnly Upset As New Gestures(AnimSet, "upset")
                Public Shared ReadOnly UpYours As New Gestures(AnimSet, "up_yours")
                Public Shared ReadOnly UJustWatch As New Gestures(AnimSet, "u_just_watch")
                Public Shared ReadOnly UShould As New Gestures(AnimSet, "u_should")
                Public Shared ReadOnly WhatUWish As New Gestures(AnimSet, "what_u_wish")
                Public Shared ReadOnly WhyNot As New Gestures(AnimSet, "why_not")
                Public Shared ReadOnly YeahOk As New Gestures(AnimSet, "yeah_ok")
                Public Shared ReadOnly YeahSure As New Gestures(AnimSet, "yeah_sure")
                Public Shared ReadOnly YourRight As New Gestures(AnimSet, "your_right")
            End Structure
            Public Structure CarMale
                Public Const AnimSet As String = "gestures@car"

                Public Shared ReadOnly AreYouIn As New Gestures(AnimSet, "are_you_in")
                Public Shared ReadOnly ButWhy As New Gestures(AnimSet, "but_why")
                Public Shared ReadOnly ComeOn As New Gestures(AnimSet, "come_on")
                Public Shared ReadOnly Despair As New Gestures(AnimSet, "despair")
                Public Shared ReadOnly Disbelief As New Gestures(AnimSet, "disbelief")
                Public Shared ReadOnly DontHitMe As New Gestures(AnimSet, "dont_hit_me")
                Public Shared ReadOnly DontKnow As New Gestures(AnimSet, "dont_know")
                Public Shared ReadOnly EasyNow As New Gestures(AnimSet, "easy_now")
                Public Shared ReadOnly ForgetIt As New Gestures(AnimSet, "forget_it")
                Public Shared ReadOnly Goddamn As New Gestures(AnimSet, "goddamn")
                Public Shared ReadOnly Good As New Gestures(AnimSet, "good")
                Public Shared ReadOnly HowCouldYou As New Gestures(AnimSet, "how_could_you")
                Public Shared ReadOnly Imtalking2You As New Gestures(AnimSet, "im talking_2_you")
                Public Shared ReadOnly ImNotSure As New Gestures(AnimSet, "im_not_sure")
                Public Shared ReadOnly ImTellingYou As New Gestures(AnimSet, "im_telling_you")
                Public Shared ReadOnly IndicateListener As New Gestures(AnimSet, "indicate_listener")
                Public Shared ReadOnly ItsDone As New Gestures(AnimSet, "its_done")
                Public Shared ReadOnly IGetIt As New Gestures(AnimSet, "i_get_it")
                Public Shared ReadOnly ISaidNo As New Gestures(AnimSet, "i_said_no")
                Public Shared ReadOnly IWill As New Gestures(AnimSet, "i_will")
                Public Shared ReadOnly LetMeThink As New Gestures(AnimSet, "let_me_think")
                Public Shared ReadOnly NodNo As New Gestures(AnimSet, "nod_no")
                Public Shared ReadOnly NodYes As New Gestures(AnimSet, "nod_yes")
                Public Shared ReadOnly OfCourse As New Gestures(AnimSet, "of_course")
                Public Shared ReadOnly OhShit As New Gestures(AnimSet, "oh_shit")
                Public Shared ReadOnly OkOk As New Gestures(AnimSet, "ok_ok")
                Public Shared ReadOnly Please As New Gestures(AnimSet, "please")
                Public Shared ReadOnly Shit As New Gestures(AnimSet, "shit")
                Public Shared ReadOnly Shock As New Gestures(AnimSet, "shock")
                Public Shared ReadOnly ShutUp As New Gestures(AnimSet, "shut_up")
                Public Shared ReadOnly Smoke As New Gestures(AnimSet, "smoke")
                Public Shared ReadOnly StopIt As New Gestures(AnimSet, "stop")
                Public Shared ReadOnly Test As New Gestures(AnimSet, "Test")
                Public Shared ReadOnly Uptight As New Gestures(AnimSet, "uptight")
                Public Shared ReadOnly UThinImStupid As New Gestures(AnimSet, "u_thin_i'm_stupid")
                Public Shared ReadOnly Whatever As New Gestures(AnimSet, "whatever")
                Public Shared ReadOnly YeahIGotIt As New Gestures(AnimSet, "yeah_i_got_it")
                Public Shared ReadOnly Yes As New Gestures(AnimSet, "yes")
                Public Shared ReadOnly YoureRight As New Gestures(AnimSet, "youre_right")
                Public Shared ReadOnly YouDig As New Gestures(AnimSet, "you_dig")
                Public Shared ReadOnly YouWillLovethis As New Gestures(AnimSet, "you_will_love this")
            End Structure
            Public Structure CarFemale
                Public Const AnimSet As String = "gestures@car_f"

                Public Shared ReadOnly Absolutely As New Gestures(AnimSet, "absolutely")
                Public Shared ReadOnly AllGone As New Gestures(AnimSet, "all_gone")
                Public Shared ReadOnly CantBe As New Gestures(AnimSet, "cant_be")
                Public Shared ReadOnly Dont As New Gestures(AnimSet, "dont")
                Public Shared ReadOnly DontUDare As New Gestures(AnimSet, "dont_u_dare")
                Public Shared ReadOnly GetThisStraight As New Gestures(AnimSet, "get_this_straight")
                Public Shared ReadOnly Gimme As New Gestures(AnimSet, "gimme")
                Public Shared ReadOnly ItsSoLike As New Gestures(AnimSet, "its_so_like")
                Public Shared ReadOnly ICant As New Gestures(AnimSet, "i_cant")
                Public Shared ReadOnly IDontCare As New Gestures(AnimSet, "i_dont_care")
                Public Shared ReadOnly IOughta As New Gestures(AnimSet, "i_oughta")
                Public Shared ReadOnly JustGo As New Gestures(AnimSet, "just_go")
                Public Shared ReadOnly LeaveIt As New Gestures(AnimSet, "leave_it")
                Public Shared ReadOnly LookAtMe As New Gestures(AnimSet, "look_at_me")
                Public Shared ReadOnly No As New Gestures(AnimSet, "no")
                Public Shared ReadOnly NoThanks As New Gestures(AnimSet, "no_thanks")
                Public Shared ReadOnly NoWay As New Gestures(AnimSet, "no_way")
                Public Shared ReadOnly OfCoarse As New Gestures(AnimSet, "of_coarse")
                Public Shared ReadOnly OhComeOn As New Gestures(AnimSet, "oh_come_on")
                Public Shared ReadOnly OhMyGod As New Gestures(AnimSet, "oh_my_god")
                Public Shared ReadOnly OhNo As New Gestures(AnimSet, "oh_no")
                Public Shared ReadOnly Ok As New Gestures(AnimSet, "ok")
                Public Shared ReadOnly Possibly As New Gestures(AnimSet, "possibly")
                Public Shared ReadOnly Shocked As New Gestures(AnimSet, "shocked")
                Public Shared ReadOnly SoTough As New Gestures(AnimSet, "so_tough")
                Public Shared ReadOnly Test As New Gestures(AnimSet, "Test")
                Public Shared ReadOnly ThisHigh As New Gestures(AnimSet, "this_high")
                Public Shared ReadOnly Threaten As New Gestures(AnimSet, "threaten")
                Public Shared ReadOnly Upset As New Gestures(AnimSet, "upset")
                Public Shared ReadOnly UShould As New Gestures(AnimSet, "u_should")
                Public Shared ReadOnly WhatUWish As New Gestures(AnimSet, "what_u_wish")
                Public Shared ReadOnly YeahSure As New Gestures(AnimSet, "yeah_sure")
                Public Shared ReadOnly YourRight As New Gestures(AnimSet, "your_right")
            End Structure
            Public Structure Seated
                Public Const AnimSet As String = "gestures@m_seated"

                Public Shared ReadOnly Absolutely As New Gestures(AnimSet, "absolutely")
                Public Shared ReadOnly Agree As New Gestures(AnimSet, "agree")
                Public Shared ReadOnly Amazing As New Gestures(AnimSet, "amazing")
                Public Shared ReadOnly AngerA As New Gestures(AnimSet, "anger_a")
                Public Shared ReadOnly AreYouIn As New Gestures(AnimSet, "are_you_in")
                Public Shared ReadOnly BringItOn As New Gestures(AnimSet, "bring_it_on")
                Public Shared ReadOnly BringItToMe As New Gestures(AnimSet, "bring_it_to_me")
                Public Shared ReadOnly ButWhy As New Gestures(AnimSet, "but_why")
                Public Shared ReadOnly ComeHere As New Gestures(AnimSet, "come_here")
                Public Shared ReadOnly ComeOn As New Gestures(AnimSet, "come_on")
                Public Shared ReadOnly Damn As New Gestures(AnimSet, "damn")
                Public Shared ReadOnly Despair As New Gestures(AnimSet, "despair")
                Public Shared ReadOnly Disbelief As New Gestures(AnimSet, "disbelief")
                Public Shared ReadOnly DontHitMe As New Gestures(AnimSet, "dont_hit_me")
                Public Shared ReadOnly DontKnow As New Gestures(AnimSet, "dont_know")
                Public Shared ReadOnly DoIt As New Gestures(AnimSet, "do_it")
                Public Shared ReadOnly EasyNow As New Gestures(AnimSet, "easy_now")
                Public Shared ReadOnly Enough As New Gestures(AnimSet, "enough")
                Public Shared ReadOnly Exactly As New Gestures(AnimSet, "exactly")
                Public Shared ReadOnly FoldArmsOhYeah As New Gestures(AnimSet, "fold_arms_oh_yeah")
                Public Shared ReadOnly ForgetIt As New Gestures(AnimSet, "forget_it")
                Public Shared ReadOnly Goddamn As New Gestures(AnimSet, "goddamn")
                Public Shared ReadOnly Good As New Gestures(AnimSet, "good")
                Public Shared ReadOnly GoAway As New Gestures(AnimSet, "go_away")
                Public Shared ReadOnly HowCouldYou As New Gestures(AnimSet, "how_could_you")
                Public Shared ReadOnly HowMuch As New Gestures(AnimSet, "how_much")
                Public Shared ReadOnly ImTellingYou As New Gestures(AnimSet, "im_telling_you")
                Public Shared ReadOnly IndicateListener As New Gestures(AnimSet, "indicate_listener")
                Public Shared ReadOnly ItsOk As New Gestures(AnimSet, "its_ok")
                Public Shared ReadOnly ICantSay As New Gestures(AnimSet, "i_cant_say")
                Public Shared ReadOnly ICouldnt As New Gestures(AnimSet, "i_couldnt")
                Public Shared ReadOnly IGiveUp As New Gestures(AnimSet, "i_give_up")
                Public Shared ReadOnly ISaidNo As New Gestures(AnimSet, "i_said_no")
                Public Shared ReadOnly IWill As New Gestures(AnimSet, "i_will")
                Public Shared ReadOnly LetMeThink As New Gestures(AnimSet, "let_me_think")
                Public Shared ReadOnly Negative As New Gestures(AnimSet, "negative")
                Public Shared ReadOnly NodNo As New Gestures(AnimSet, "nod_no")
                Public Shared ReadOnly NotMe As New Gestures(AnimSet, "not_me")
                Public Shared ReadOnly NoChance As New Gestures(AnimSet, "no_chance")
                Public Shared ReadOnly OhShit As New Gestures(AnimSet, "oh_shit")
                Public Shared ReadOnly Ok As New Gestures(AnimSet, "ok")
                Public Shared ReadOnly Please As New Gestures(AnimSet, "please")
                Public Shared ReadOnly RaiseHands As New Gestures(AnimSet, "raise_hands")
                Public Shared ReadOnly Shit As New Gestures(AnimSet, "shit")
                Public Shared ReadOnly Shock As New Gestures(AnimSet, "shock")
                Public Shared ReadOnly StopIt As New Gestures(AnimSet, "stop")
                Public Shared ReadOnly Test As New Gestures(AnimSet, "Test")
                Public Shared ReadOnly ThatWay As New Gestures(AnimSet, "that_way")
                Public Shared ReadOnly ThisAndThat As New Gestures(AnimSet, "this_and_that")
                Public Shared ReadOnly Unbelievable As New Gestures(AnimSet, "unbelievable")
                Public Shared ReadOnly USerious As New Gestures(AnimSet, "u_serious")
                Public Shared ReadOnly UThinImStupid As New Gestures(AnimSet, "u_thin_i'm_stupid")
                Public Shared ReadOnly UUnderstand As New Gestures(AnimSet, "u_understand")
                Public Shared ReadOnly We As New Gestures(AnimSet, "we")
                Public Shared ReadOnly WeCanDoIt As New Gestures(AnimSet, "we_can_do_it")
                Public Shared ReadOnly Whatever As New Gestures(AnimSet, "whatever")
                Public Shared ReadOnly YeahIGotIt As New Gestures(AnimSet, "yeah_i_got_it")
                Public Shared ReadOnly Yes As New Gestures(AnimSet, "yes")
                Public Shared ReadOnly YoureRight As New Gestures(AnimSet, "youre_right")
                Public Shared ReadOnly YouDig As New Gestures(AnimSet, "you_dig")
                Public Shared ReadOnly YouWillLovethis As New Gestures(AnimSet, "you_will_love this")
            End Structure
            Public Structure Niko
                Public Const AnimSet As String = "gestures@niko"

                Public Shared ReadOnly Absolutely As New Gestures(AnimSet, "absolutely")
                Public Shared ReadOnly Agree As New Gestures(AnimSet, "agree")
                Public Shared ReadOnly Amazing As New Gestures(AnimSet, "amazing")
                Public Shared ReadOnly AngerA As New Gestures(AnimSet, "anger_a")
                Public Shared ReadOnly AreYouIn As New Gestures(AnimSet, "are_you_in")
                Public Shared ReadOnly BringItOn As New Gestures(AnimSet, "bring_it_on")
                Public Shared ReadOnly BringItToMe As New Gestures(AnimSet, "bring_it_to_me")
                Public Shared ReadOnly ButWhy As New Gestures(AnimSet, "but_why")
                Public Shared ReadOnly ComeHere As New Gestures(AnimSet, "come_here")
                Public Shared ReadOnly ComeOn As New Gestures(AnimSet, "come_on")
                Public Shared ReadOnly Damn As New Gestures(AnimSet, "damn")
                Public Shared ReadOnly Despair As New Gestures(AnimSet, "despair")
                Public Shared ReadOnly Disbelief As New Gestures(AnimSet, "disbelief")
                Public Shared ReadOnly DontHitMe As New Gestures(AnimSet, "dont_hit_me")
                Public Shared ReadOnly DontKnow As New Gestures(AnimSet, "dont_know")
                Public Shared ReadOnly DoIt As New Gestures(AnimSet, "do_it")
                Public Shared ReadOnly EasyNow As New Gestures(AnimSet, "easy_now")
                Public Shared ReadOnly Enough As New Gestures(AnimSet, "enough")
                Public Shared ReadOnly Exactly As New Gestures(AnimSet, "exactly")
                Public Shared ReadOnly FoldArmsOhYeah As New Gestures(AnimSet, "fold_arms_oh_yeah")
                Public Shared ReadOnly ForgetIt As New Gestures(AnimSet, "forget_it")
                Public Shared ReadOnly GiveMeABreak As New Gestures(AnimSet, "give_me_a_break")
                Public Shared ReadOnly Goddamn As New Gestures(AnimSet, "goddamn")
                Public Shared ReadOnly Good As New Gestures(AnimSet, "good")
                Public Shared ReadOnly GoAway As New Gestures(AnimSet, "go_away")
                Public Shared ReadOnly Hello As New Gestures(AnimSet, "hello")
                Public Shared ReadOnly Hey As New Gestures(AnimSet, "hey")
                Public Shared ReadOnly HoldsUpFingers As New Gestures(AnimSet, "holds_up_fingers")
                Public Shared ReadOnly How As New Gestures(AnimSet, "how")
                Public Shared ReadOnly HowCouldYou As New Gestures(AnimSet, "how_could_you")
                Public Shared ReadOnly HowMuch As New Gestures(AnimSet, "how_much")
                Public Shared ReadOnly IfUSaySo As New Gestures(AnimSet, "if_u_say_so")
                Public Shared ReadOnly IllDoIt As New Gestures(AnimSet, "ill_do_it")
                Public Shared ReadOnly Imtalking2You As New Gestures(AnimSet, "im talking_2_you")
                Public Shared ReadOnly ImBeggingYou As New Gestures(AnimSet, "im_begging_you")
                Public Shared ReadOnly ImNotSure As New Gestures(AnimSet, "im_not_sure")
                Public Shared ReadOnly ImSorry As New Gestures(AnimSet, "im_sorry")
                Public Shared ReadOnly ImTellingYou As New Gestures(AnimSet, "im_telling_you")
                Public Shared ReadOnly IndicateBack As New Gestures(AnimSet, "indicate_back")
                Public Shared ReadOnly IndicateLeft As New Gestures(AnimSet, "indicate_left")
                Public Shared ReadOnly IndicateListener As New Gestures(AnimSet, "indicate_listener")
                Public Shared ReadOnly IndicateRightB As New Gestures(AnimSet, "indicate_right_b")
                Public Shared ReadOnly IndicateRightC As New Gestures(AnimSet, "indicate_right_c")
                Public Shared ReadOnly IsThisIt As New Gestures(AnimSet, "is_this_it")
                Public Shared ReadOnly ItsDone As New Gestures(AnimSet, "its_done")
                Public Shared ReadOnly ItsMine As New Gestures(AnimSet, "its_mine")
                Public Shared ReadOnly ItsOk As New Gestures(AnimSet, "its_ok")
                Public Shared ReadOnly IveForgot As New Gestures(AnimSet, "ive_forgot")
                Public Shared ReadOnly ICantSay As New Gestures(AnimSet, "i_cant_say")
                Public Shared ReadOnly ICouldnt As New Gestures(AnimSet, "i_couldnt")
                Public Shared ReadOnly IDontHave As New Gestures(AnimSet, "i_dont_have")
                Public Shared ReadOnly IDontThinkSo As New Gestures(AnimSet, "i_dont_think_so")
                Public Shared ReadOnly IGetIt As New Gestures(AnimSet, "i_get_it")
                Public Shared ReadOnly IGiveUp As New Gestures(AnimSet, "i_give_up")
                Public Shared ReadOnly ISaidNo As New Gestures(AnimSet, "i_said_no")
                Public Shared ReadOnly IWill As New Gestures(AnimSet, "i_will")
                Public Shared ReadOnly KissMyAss As New Gestures(AnimSet, "kiss_my_ass")
                Public Shared ReadOnly Later As New Gestures(AnimSet, "later")
                Public Shared ReadOnly LeaveIt2Me As New Gestures(AnimSet, "leave_it_2_me")
                Public Shared ReadOnly LetMeThink As New Gestures(AnimSet, "let_me_think")
                Public Shared ReadOnly LikeThis As New Gestures(AnimSet, "like_this")
                Public Shared ReadOnly MeGest As New Gestures(AnimSet, "me")
                Public Shared ReadOnly Natuarally As New Gestures(AnimSet, "natuarally")
                Public Shared ReadOnly Negative As New Gestures(AnimSet, "negative")
                Public Shared ReadOnly NodNo As New Gestures(AnimSet, "nod_no")
                Public Shared ReadOnly NodYes As New Gestures(AnimSet, "nod_yes")
                Public Shared ReadOnly NotMe As New Gestures(AnimSet, "not_me")
                Public Shared ReadOnly NotSure As New Gestures(AnimSet, "not_sure")
                Public Shared ReadOnly NoChance As New Gestures(AnimSet, "no_chance")
                Public Shared ReadOnly NoReally As New Gestures(AnimSet, "no_really")
                Public Shared ReadOnly Numbnuts As New Gestures(AnimSet, "numbnuts")
                Public Shared ReadOnly OfCourse As New Gestures(AnimSet, "of_course")
                Public Shared ReadOnly OhShit As New Gestures(AnimSet, "oh_shit")
                Public Shared ReadOnly Ok As New Gestures(AnimSet, "ok")
                Public Shared ReadOnly OkOk As New Gestures(AnimSet, "ok_ok")
                Public Shared ReadOnly OverThere As New Gestures(AnimSet, "over_there")
                Public Shared ReadOnly PissOff As New Gestures(AnimSet, "piss_off")
                Public Shared ReadOnly Please As New Gestures(AnimSet, "please")
                Public Shared ReadOnly PointFwd As New Gestures(AnimSet, "point_fwd")
                Public Shared ReadOnly PointRight As New Gestures(AnimSet, "point_right")
                Public Shared ReadOnly Positive As New Gestures(AnimSet, "positive")
                Public Shared ReadOnly RaiseHands As New Gestures(AnimSet, "raise_hands")
                Public Shared ReadOnly SayAgain As New Gestures(AnimSet, "say_again")
                Public Shared ReadOnly ScrewYou As New Gestures(AnimSet, "screw_you")
                Public Shared ReadOnly Shit As New Gestures(AnimSet, "shit")
                Public Shared ReadOnly Shock As New Gestures(AnimSet, "shock")
                Public Shared ReadOnly ShutUp As New Gestures(AnimSet, "shut_up")
                Public Shared ReadOnly StopIt As New Gestures(AnimSet, "stop")
                Public Shared ReadOnly Sure As New Gestures(AnimSet, "sure")
                Public Shared ReadOnly TellMeAboutIt As New Gestures(AnimSet, "tell_me_about_it")
                Public Shared ReadOnly Test As New Gestures(AnimSet, "Test")
                Public Shared ReadOnly That As New Gestures(AnimSet, "that")
                Public Shared ReadOnly ThatWay As New Gestures(AnimSet, "that_way")
                Public Shared ReadOnly ThisAndThat As New Gestures(AnimSet, "this_and_that")
                Public Shared ReadOnly ThisBig As New Gestures(AnimSet, "this_big")
                Public Shared ReadOnly Threaten As New Gestures(AnimSet, "threaten")
                Public Shared ReadOnly Time As New Gestures(AnimSet, "time")
                Public Shared ReadOnly Tosser As New Gestures(AnimSet, "tosser")
                Public Shared ReadOnly TouchFace As New Gestures(AnimSet, "touch_face")
                Public Shared ReadOnly ToHellWithIt As New Gestures(AnimSet, "to_hell_with_it")
                Public Shared ReadOnly Unbelievable As New Gestures(AnimSet, "unbelievable")
                Public Shared ReadOnly Uptight As New Gestures(AnimSet, "uptight")
                Public Shared ReadOnly UCantDoThat As New Gestures(AnimSet, "u_cant_do_that")
                Public Shared ReadOnly USerious As New Gestures(AnimSet, "u_serious")
                Public Shared ReadOnly UTalkin2Me As New Gestures(AnimSet, "u_talkin_2_me")
                Public Shared ReadOnly UThinImStupid As New Gestures(AnimSet, "u_thin_i'm_stupid")
                Public Shared ReadOnly UUnderstand As New Gestures(AnimSet, "u_understand")
                Public Shared ReadOnly WantSomeOfThis As New Gestures(AnimSet, "want_some_of_this")
                Public Shared ReadOnly We As New Gestures(AnimSet, "we")
                Public Shared ReadOnly Well As New Gestures(AnimSet, "well")
                Public Shared ReadOnly WellAlright As New Gestures(AnimSet, "well_alright")
                Public Shared ReadOnly WeCanDoIt As New Gestures(AnimSet, "we_can_do_it")
                Public Shared ReadOnly What As New Gestures(AnimSet, "what")
                Public Shared ReadOnly Whatever As New Gestures(AnimSet, "whatever")
                Public Shared ReadOnly WhateverC As New Gestures(AnimSet, "whatever_c")
                Public Shared ReadOnly Why As New Gestures(AnimSet, "why")
                Public Shared ReadOnly WotTheFuck As New Gestures(AnimSet, "wot_the_fuck")
                Public Shared ReadOnly YeahIGotIt As New Gestures(AnimSet, "yeah_i_got_it")
                Public Shared ReadOnly Yes As New Gestures(AnimSet, "yes")
                Public Shared ReadOnly YoureRight As New Gestures(AnimSet, "youre_right")
                Public Shared ReadOnly YouDig As New Gestures(AnimSet, "you_dig")
                Public Shared ReadOnly YouWillLovethis As New Gestures(AnimSet, "you_will_love this")
            End Structure
            Public Structure BikeEFLC
                Public Const AnimSet As String = "gestures@bike"
                Public Shared ReadOnly AndISaid As New Gestures(AnimSet, "and_i_said")
                Public Shared ReadOnly AndIWasLike As New Gestures(AnimSet, "and_i_was_like")
                Public Shared ReadOnly BlahBlah As New Gestures(AnimSet, "blah_blah")
                Public Shared ReadOnly Damn As New Gestures(AnimSet, "damn")
                Public Shared ReadOnly GetTo As New Gestures(AnimSet, "get_to")
                Public Shared ReadOnly Hey As New Gestures(AnimSet, "hey")
                Public Shared ReadOnly PunchAir As New Gestures(AnimSet, "punch_air")
                Public Shared ReadOnly ThumbsUp As New Gestures(AnimSet, "thumbup")
                Public Shared ReadOnly YeahRight As New Gestures(AnimSet, "yearight")
                Public Shared ReadOnly YeahWhatever As New Gestures(AnimSet, "yeah_whatever")
            End Structure
        End Class
        Public Class Facials
            Public ReadOnly AnimSet As String
            Public ReadOnly AnimName As String
            Public Sub New(AnimSet As String, AnimName As String)
                Me.AnimSet = AnimSet
                Me.AnimName = AnimName
            End Sub
            Public Shared Sub Play(Ped As Ped, Facial As Facials, Optional Duration As Integer = -1, Optional PlayInLoop As Boolean = False, Optional Flag2 As Boolean = False, Optional Speed As Single = 1.00001)
                'Dim AnimString As String
                'Select Case AnimFacial
                '    Case 0
                '        AnimString = "gest_think_loop"
                '    Case 1
                '        AnimString = "gest_surprised_loop"
                '    Case 2
                '        AnimString = "chew"
                '    Case 3
                '        AnimString = "gun_aim"
                '    Case 4
                '        If AnimSet = "facials@m_hi" Then
                '            AnimString = "aim_cue"
                '        End If
                '    Case 5
                '        AnimString = "gest_angry_loop"
                '    Case 6
                '        AnimString = "angry_a"
                '    Case 7
                '        AnimString = "angry_b"
                '    Case 8
                '        AnimString = "angry_c"
                '    Case 9
                '        AnimString = "blow"
                '    Case 10
                '        AnimString = "heavybreath"
                '    Case 11
                '        AnimString = "lookaround"
                '    Case 12
                '        AnimString = "look_down"
                '    Case 13
                '        AnimString = "look_left"
                '    Case 14
                '        AnimString = "look_right"
                '    Case 15
                '        AnimString = "pain_a"
                '    Case 16
                '        AnimString = "pain_b"
                '    Case 17
                '        AnimString = "pain_c"
                '    Case 18
                '        AnimString = "shocked"
                '    Case 19
                '        AnimString = "whatever"
                '    Case 20
                '        AnimString = "dead_a"
                '    Case 21
                '        AnimString = "dead_b"
                '    Case 22
                '        AnimString = "die_a"
                'End Select

                [Call]("REQUEST_ANIMS", Facial.AnimSet)
                'Do
                '    Game.WaitInCurrentScript(1)
                'Loop Until [Call](Of Boolean)("HAS_ANIMS_LOADED")
                [Call]("TASK_PLAY_ANIM_FACIAL", Ped, Facial.AnimName, Facial.AnimSet, Speed, PlayInLoop, Flag2, Duration)
            End Sub
            Public Shared Sub BlockFacialAnims(Ped As Ped, BlockAnim As Boolean)
                [Call]("BLOCK_CHAR_VISEME_ANIMS", Ped, BlockAnim)
            End Sub
            Public Shared Function haveFacialLoaded(FacialSet As String) As Boolean
                Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", FacialSet)
            End Function
            Public Shared Sub Relase(FacialSet As String)
                [Call]("REMOVE_ANIMS", FacialSet)
            End Sub
            Public Shared Sub Request(FacialSet As String)
                [Call]("REQUEST_ANIMS", FacialSet)
            End Sub
            Public Structure Player
                Public Const AnimSet As String = "facials@m_hi"

                Public Shared ReadOnly Shocked As New Facials(AnimSet, "shocked")
                Public Shared ReadOnly GestThinkOutro As New Facials(AnimSet, "gest_think_outro")
                Public Shared ReadOnly Chew As New Facials(AnimSet, "chew")
                Public Shared ReadOnly GestSurprisedOutro As New Facials(AnimSet, "gest_surprised_outro")
                Public Shared ReadOnly PainB As New Facials(AnimSet, "pain_b")
                Public Shared ReadOnly GestNormalLoop As New Facials(AnimSet, "gest_normal_loop")
                Public Shared ReadOnly PainA As New Facials(AnimSet, "pain_a")
                Public Shared ReadOnly GestAngryLoop As New Facials(AnimSet, "gest_angry_loop")
                Public Shared ReadOnly Keystart As New Facials(AnimSet, "keystart")
                Public Shared ReadOnly GestSurprisedIntro As New Facials(AnimSet, "gest_surprised_intro")
                Public Shared ReadOnly DeadB As New Facials(AnimSet, "dead_b")
                Public Shared ReadOnly DieA As New Facials(AnimSet, "die_a")
                Public Shared ReadOnly Whatever As New Facials(AnimSet, "whatever")
                Public Shared ReadOnly GestThinkLoop As New Facials(AnimSet, "gest_think_loop")
                Public Shared ReadOnly Blow As New Facials(AnimSet, "blow")
                Public Shared ReadOnly DeadA As New Facials(AnimSet, "dead_a")
                Public Shared ReadOnly GestSurprisedLoop As New Facials(AnimSet, "gest_surprised_loop")
                Public Shared ReadOnly PoliceChase As New Facials(AnimSet, "police_chase")
                Public Shared ReadOnly PlyrMoodNormal As New Facials(AnimSet, "plyr_mood_normal")
                Public Shared ReadOnly LookRight As New Facials(AnimSet, "look_right")
                Public Shared ReadOnly AngryC As New Facials(AnimSet, "angry_c")
                Public Shared ReadOnly LookDown As New Facials(AnimSet, "look_down")
                Public Shared ReadOnly GestAngryOutro As New Facials(AnimSet, "gest_angry_outro")
                Public Shared ReadOnly PainC As New Facials(AnimSet, "pain_c")
                Public Shared ReadOnly GunAim As New Facials(AnimSet, "gun_aim")
                Public Shared ReadOnly GestAngryIntro As New Facials(AnimSet, "gest_angry_intro")
                Public Shared ReadOnly LookLeft As New Facials(AnimSet, "look_left")
                Public Shared ReadOnly PlyrMoodHappy As New Facials(AnimSet, "plyr_mood_happy")
                Public Shared ReadOnly PlyrMoodAngry As New Facials(AnimSet, "plyr_mood_angry")
                Public Shared ReadOnly AimCue As New Facials(AnimSet, "aim_cue")
                Public Shared ReadOnly HeavyBreath As New Facials(AnimSet, "heavybreath")
                Public Shared ReadOnly AngryA As New Facials(AnimSet, "angry_a")
                Public Shared ReadOnly GestThinkIntro As New Facials(AnimSet, "gest_think_intro")
                Public Shared ReadOnly AngryB As New Facials(AnimSet, "angry_b")
                Public Shared ReadOnly LookAround As New Facials(AnimSet, "lookaround")
            End Structure
            Public Structure Male
                Public Const AnimSet As String = "facials@m_lo"

                Public Shared ReadOnly Shocked As New Facials(AnimSet, "shocked")
                Public Shared ReadOnly GestThinkOutro As New Facials(AnimSet, "gest_think_outro")
                Public Shared ReadOnly Chew As New Facials(AnimSet, "chew")
                Public Shared ReadOnly Skinning As New Facials(AnimSet, "skinning")
                Public Shared ReadOnly GestSurprisedOutro As New Facials(AnimSet, "gest_surprised_outro")
                Public Shared ReadOnly LjCreateSpliff As New Facials(AnimSet, "lj_create_spliff")
                Public Shared ReadOnly PainB As New Facials(AnimSet, "pain_b")
                Public Shared ReadOnly GestNormalLoop As New Facials(AnimSet, "gest_normal_loop")
                Public Shared ReadOnly PainA As New Facials(AnimSet, "pain_a")
                Public Shared ReadOnly GestAngryLoop As New Facials(AnimSet, "gest_angry_loop")
                Public Shared ReadOnly Keystart As New Facials(AnimSet, "keystart")
                Public Shared ReadOnly GestSurprisedIntro As New Facials(AnimSet, "gest_surprised_intro")
                Public Shared ReadOnly DeadB As New Facials(AnimSet, "dead_b")
                Public Shared ReadOnly MoodInjured As New Facials(AnimSet, "mood_injured")
                Public Shared ReadOnly GestThinkLoop As New Facials(AnimSet, "gest_think_loop")
                Public Shared ReadOnly Blow As New Facials(AnimSet, "blow")
                Public Shared ReadOnly DeadA As New Facials(AnimSet, "dead_a")
                Public Shared ReadOnly GestSurprisedLoop As New Facials(AnimSet, "gest_surprised_loop")
                Public Shared ReadOnly MusicListen As New Facials(AnimSet, "music_listen")
                Public Shared ReadOnly Yawn As New Facials(AnimSet, "yawn")
                Public Shared ReadOnly LookRight As New Facials(AnimSet, "look_right")
                Public Shared ReadOnly LookDown As New Facials(AnimSet, "look_down")
                Public Shared ReadOnly GestAngryOutro As New Facials(AnimSet, "gest_angry_outro")
                Public Shared ReadOnly PainC As New Facials(AnimSet, "pain_c")
                Public Shared ReadOnly GunAim As New Facials(AnimSet, "gun_aim")
                Public Shared ReadOnly MoodAngry As New Facials(AnimSet, "mood_angry")
                Public Shared ReadOnly GestAngryIntro As New Facials(AnimSet, "gest_angry_intro")
                Public Shared ReadOnly LookLeft As New Facials(AnimSet, "look_left")
                Public Shared ReadOnly MoodNormal As New Facials(AnimSet, "mood_normal")
                Public Shared ReadOnly LjSmokeSpliff As New Facials(AnimSet, "lj_smoke_spliff")
                Public Shared ReadOnly MoodScared As New Facials(AnimSet, "mood_scared")
                Public Shared ReadOnly AngryA As New Facials(AnimSet, "angry_a")
                Public Shared ReadOnly LookUp As New Facials(AnimSet, "look_up")
                Public Shared ReadOnly GestThinkIntro As New Facials(AnimSet, "gest_think_intro")
                Public Shared ReadOnly AngryB As New Facials(AnimSet, "angry_b")
                Public Shared ReadOnly LookAround As New Facials(AnimSet, "lookaround")
            End Structure
            Public Structure Female
                Public Shared ReadOnly AnimSet As String = "facials@f_lo"

                Public Shared ReadOnly Shocked As New Facials(AnimSet, "shocked")
                Public Shared ReadOnly GestThinkOutro As New Facials(AnimSet, "gest_think_outro")
                Public Shared ReadOnly Chew As New Facials(AnimSet, "chew")
                Public Shared ReadOnly Skinning As New Facials(AnimSet, "skinning")
                Public Shared ReadOnly GestSurprisedOutro As New Facials(AnimSet, "gest_surprised_outro")
                Public Shared ReadOnly LjCreateSpliff As New Facials(AnimSet, "lj_create_spliff")
                Public Shared ReadOnly PainB As New Facials(AnimSet, "pain_b")
                Public Shared ReadOnly GestNormalLoop As New Facials(AnimSet, "gest_normal_loop")
                Public Shared ReadOnly PainA As New Facials(AnimSet, "pain_a")
                Public Shared ReadOnly GestAngryLoop As New Facials(AnimSet, "gest_angry_loop")
                Public Shared ReadOnly KeyStart As New Facials(AnimSet, "keystart")
                Public Shared ReadOnly GestSurprisedIntro As New Facials(AnimSet, "gest_surprised_intro")
                Public Shared ReadOnly DeadB As New Facials(AnimSet, "dead_b")
                Public Shared ReadOnly MoodInjured As New Facials(AnimSet, "mood_injured")
                Public Shared ReadOnly GestThinkLoop As New Facials(AnimSet, "gest_think_loop")
                Public Shared ReadOnly Blow As New Facials(AnimSet, "blow")
                Public Shared ReadOnly DeadA As New Facials(AnimSet, "dead_a")
                Public Shared ReadOnly GestSurprisedLoop As New Facials(AnimSet, "gest_surprised_loop")
                Public Shared ReadOnly MusicListen As New Facials(AnimSet, "music_listen")
                Public Shared ReadOnly Yawn As New Facials(AnimSet, "yawn")
                Public Shared ReadOnly LookRight As New Facials(AnimSet, "look_right")
                Public Shared ReadOnly LookDown As New Facials(AnimSet, "look_down")
                Public Shared ReadOnly GestAngryOutro As New Facials(AnimSet, "gest_angry_outro")
                Public Shared ReadOnly PainC As New Facials(AnimSet, "pain_c")
                Public Shared ReadOnly GunAim As New Facials(AnimSet, "gun_aim")
                Public Shared ReadOnly MoodAngry As New Facials(AnimSet, "mood_angry")
                Public Shared ReadOnly GestAngryIntro As New Facials(AnimSet, "gest_angry_intro")
                Public Shared ReadOnly LookLeft As New Facials(AnimSet, "look_left")
                Public Shared ReadOnly MoodNormal As New Facials(AnimSet, "mood_normal")
                Public Shared ReadOnly LjSmokeSpliff As New Facials(AnimSet, "lj_smoke_spliff")
                Public Shared ReadOnly MoodScared As New Facials(AnimSet, "mood_scared")
                Public Shared ReadOnly AngryA As New Facials(AnimSet, "angry_a")
                Public Shared ReadOnly LookUp As New Facials(AnimSet, "look_up")
                Public Shared ReadOnly GestThinkIntro As New Facials(AnimSet, "gest_think_intro")
                Public Shared ReadOnly AngryB As New Facials(AnimSet, "angry_b")
                Public Shared ReadOnly AbeAngryB As New Facials(AnimSet, "abe_angry_b")
                Public Shared ReadOnly LookAround As New Facials(AnimSet, "lookaround")
            End Structure

            'Public Enum PedFacials
            '    Think = 0
            '    Suprised = 1
            '    Chew = 2
            '    GunAim = 3
            '    CueAim = 4
            '    Angry1 = 5
            '    Angry2 = 6
            '    Angry3 = 7
            '    Angry4 = 8
            '    Blow = 9
            '    HeavyBreath = 10
            '    LookAround = 11
            '    LookDown = 12
            '    LookLeft = 13
            '    LookRight = 14
            '    Pain1 = 15
            '    Pain2 = 16
            '    Pain3 = 17
            '    Shocked = 18
            '    Whatever = 19
            '    Dead1 = 20
            '    Dead2 = 21
            '    Dead3 = 22
            'End Enum
        End Class
    End Class
    Public Class Clothes
        Public Shared Sub ChangeHelmetTexture(Ped As Ped, TextureIndex As Integer)
            [Call]("SET_PED_HELMET_TEXTURE_INDEX", Ped, TextureIndex)
        End Sub
        Public Shared Sub ChangeClothesComponent(Ped As Ped, ComponentID As Components, ModelVariation As Integer, TextureVariation As Integer)
            [Call]("SET_CHAR_COMPONENT_VARIATION", Ped, ComponentID, ModelVariation, TextureVariation)
        End Sub
        Public Shared Sub EnableAllPedHelmets(Enable As Boolean)
            [Call]("ENABLE_ALL_PED_HELMETS", Enable)
        End Sub
        Public Shared Sub EnableHelmet(Ped As Ped, Optional Enable As Boolean = True)
            [Call]("ENABLE_PED_HELMET", Ped, Enable)
        End Sub
        Public Shared Function getCurrentPropIndex(Ped As Ped, Prop As Props) As UInteger
            Dim Pointer As Native.Pointer = New Native.Pointer(GetType(UInteger))
            [Call]("GET_CHAR_PROP_INDEX", Ped, Prop, Pointer)
            Return Pointer.Value
        End Function
        Public Shared Function getCurrentModelVariation(Ped As Ped, ComponentID As Components) As Integer
            Return [Call](Of Integer)("GET_CHAR_DRAWABLE_VARIATION", Ped, ComponentID)
        End Function
        Public Shared Function getCurrentTextureVariation(Ped As Ped, ComponentID As Components) As Integer
            Return [Call](Of Integer)("GET_CHAR_TEXTURE_VARIATION", Ped, ComponentID)
        End Function
        Public Shared Function getNumberOfModelVariations(Ped As Ped, ComponentID As Components) As Integer
            Return [Call](Of Integer)("GET_NUMBER_OF_CHAR_DRAWABLE_VARIATIONS", Ped, ComponentID)
        End Function
        Public Shared Function getNumberOfTextureVariations(Ped As Ped, ComponentID As Components, ModelVariation As Integer) As Integer
            Return [Call](Of Integer)("GET_NUMBER_OF_CHAR_DRAWABLE_VARIATIONS", Ped, ComponentID, ModelVariation)
        End Function
        Public Shared Sub HidePlayerComponent(ComponentID As Components, Optional Hide As Boolean = True)
            [Call]("SET_DRAW_PLAYER_COMPONENT", ComponentID, Not Hide)
        End Sub
        Public Shared Sub GiveHelmet(Ped As Ped, Optional KeepHelmetOn As Boolean = True)
            [Call]("GIVE_PED_HELMET_WITH_OPTS", Ped, KeepHelmetOn)
        End Sub
        Public Shared Sub GiveHelmetToPlayer(Helmet As Boolean)
            [Call]("GIVE_PLAYER_HELMET", NativePlayer.Index, Helmet)
        End Sub
        ''' <summary>
        ''' Two combined SET_CHAR_PROP_INDEX functions to give hat and/or glasses to ped.
        ''' </summary>
        ''' <param name="Ped"></param>
        ''' <param name="HatIndex">Hat model index, or -1 to give no hat.</param>
        ''' <param name="GlassesIndex">Glasses model index, or -1 to give no glasses.</param>
        Public Shared Sub GiveProps(Ped As GTA.Ped, Optional HatIndex As Integer = -1, Optional GlassesIndex As Integer = -1)
            [Call]("SET_CHAR_PROP_INDEX", Ped, 0, HatIndex)
            [Call]("SET_CHAR_PROP_INDEX", Ped, 1, GlassesIndex)
        End Sub
        Public Shared Sub RemoveHelmetFromPlayer()
            [Call]("REMOVE_PLAYER_HELMET", NativePlayer.Index)
        End Sub
        Public Shared Sub RemoveHelmet(Ped As Ped) 'testare
            [Call]("REMOVE_PED_HELMET", Ped)
        End Sub
        Public Shared Sub RemoveProps(Ped As Ped, Optional RemoveHat As Boolean = True, Optional RemoveGlasses As Boolean = True)
            If RemoveHat Then [Call]("CLEAR_CHAR_PROP", Ped, Props.Hat)
            If RemoveGlasses Then [Call]("CLEAR_CHAR_PROP", Ped, Props.Glasses)
        End Sub
        Public Shared Sub SetRandomClothes(Ped As Ped)
            [Call]("SET_CHAR_RANDOM_COMPONENT_VARIATION", Ped)
        End Sub
        Public Enum Components
            Head = 0
            Torso = 1
            Legs = 2
            Suse = 3
            Hands = 4
            Shoes = 5
            Jacket = 6
            Hair = 7
            Sus2 = 8
            Teeth = 9
            Face = 10
        End Enum
        Public Enum Props
            Hat = 0
            Glasses = 1
        End Enum
    End Class
    Public Class Speeches
        Public Shared Sub CancelAmbientSpeech(Ped As Ped)
            [Call]("CANCEL_CURRENTLY_PLAYING_AMBIENT_SPEECH", Ped)
        End Sub
        Public Shared Function isSayingAmbientSpeech(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_AMBIENT_SPEECH_PLAYING", Ped)
        End Function
        Public Shared Function isSayingAnySpeech(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_ANY_SPEECH_PLAYING", Ped)
        End Function
        Public Shared Sub SayAmbientSpeech(Ped As Ped, SpeechName As String, PlayAfterAWhile As Boolean, Optional Voice As String = "", Optional CancelMegaphone As Integer = 1)
            Dim p3 As Integer
            Dim p4 As Integer
            If PlayAfterAWhile = False Then
                p3 = 1
                p4 = 0
            Else
                p3 = 0
                p4 = 1
            End If
            If Voice = "" Then [Call]("SAY_AMBIENT_SPEECH", Ped, SpeechName, p3, p4, CancelMegaphone) Else [Call]("SAY_AMBIENT_SPEECH_WITH_VOICE", Ped, SpeechName, Voice, p3, p4, CancelMegaphone)
        End Sub
        Public Shared Sub Scream(Ped As Ped, ScreamType As Screams)
            If ScreamType = Screams.PanicScream_NotPlayer Then
                [Call]("PANIC_SCREAM", Ped)
            ElseIf ScreamType = Screams.OnFire Then
                [Call]("ON_FIRE_SCREAM", Ped)
            Else
                [Call]("HIGH_FALL_SCREAM", Ped)
            End If
        End Sub
        Public Shared Sub SetAmbientVoiceName(Ped As Ped, AudioName As String)
            [Call]("SET_AMBIENT_VOICE_NAME", Ped, AudioName)
        End Sub
        Public Enum Screams
            PanicScream_NotPlayer = -1
            OnFire = 0
            HighFall = 1
        End Enum
        Public Class Scripted
            Public Shared Function isSayingScriptedSpeech(Ped As Ped) As Boolean
                Return [Call](Of Boolean)("IS_SCRIPTED_SPEECH_PLAYING", Ped)
            End Function
        End Class
    End Class
    Public Class Tasks
        Public Enum Actions
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
        Public Shared Sub AimAt(Ped As Ped, TargetPed As Ped, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_AIM_GUN_AT_CHAR", Ped, TargetPed, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub AimAt(Ped As Ped, TargetCoords As GTA.Vector3, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_AIM_GUN_AT_CHAR", Ped, TargetCoords.X, TargetCoords.Y, TargetCoords.Z, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub ArrestChar(Cop As Ped, Guilty As Ped, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CHAR_ARREST_CHAR", Cop, Guilty)
            AvoidTasksOverride(Cop, KeepTask, BlockPermanentEvents)
            ' AvoidTasksOverride(Guilty, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Two function in one to avoid ped AI override after "TASK" functions. It's yet used in almost all Tasks function.
        ''' </summary>
        ''' <param name="Ped"></param>
        ''' <param name="KeepTask">Uses the function SET_CHAR_KEEP_TASK</param>
        ''' <param name="BlockPermanentEvents">Uses the function SET_BLOCKING_OF_NON_TEMPORARY_EVENTS</param>
        Public Shared Sub AvoidTasksOverride(Ped As Ped, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("SET_CHAR_KEEP_TASK", Ped, KeepTask)
            [Call]("SET_BLOCKING_OF_NON_TEMPORARY_EVENTS", Ped, BlockPermanentEvents)
        End Sub
        Public Shared Sub CarAction(Ped As Ped, Car As Vehicle, CarAction As Actions, Duration As Integer, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CAR_TEMP_ACTION", Ped, Car, CarAction, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub 'Not working?
        Public Shared Sub ChangeCarSeat(Ped As Ped)
            If Ped.isSittingInVehicle Then
                [Call]("TASK_SHUFFLE_TO_NEXT_CAR_SEAT", Ped, Ped.CurrentVehicle)
            Else
                Exit Sub
            End If
        End Sub
        Public Shared Sub ClearAll(Ped As Ped, Optional Immediately As Boolean = False)
            If Immediately Then
                If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
                    Dim CurrVeh = Ped.CurrentVehicle
                    Dim CurrSeat As VehicleSeat = getCurrentVehicleSeat(Ped)
                    [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                    If CurrVeh.Exists Then Ped.WarpIntoVehicle(CurrVeh, CurrSeat)
                Else
                    [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                End If
            Else
                [Call]("CLEAR_CHAR_TASKS", Ped)
            End If
        End Sub
        Public Shared Sub CombatHatedTargetsAround(Ped As Ped, Radius As Double, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_COMBAT_HATED_TARGETS_AROUND_CHAR", Ped, Radius)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub Combat(Ped As Ped, Target As Ped, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_COMBAT", Ped, Target)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub Crouch(Ped As Ped, Crouch As Boolean, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("SET_CHAR_DUCKING", Ped, Crouch)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub CruiseSpeed(Ped As Ped, Speed As Single)
            [Call]("SET_DRIVE_TASK_CRUISE_SPEED", Ped, Speed)
        End Sub
        Public Shared Sub DriveAround(Ped As Ped, Car As Vehicle, Speed As Single, Optional DrivingStyle As DrivingStyle = DrivingStyle.Normal, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CAR_DRIVE_WANDER", Ped, Car, Speed, DrivingStyle)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub DriveBy(Ped As Ped, Target As Ped, Optional UnknownP3 As Integer = 0, Optional UnknownP4 As Single = 0.0, Optional UnknownP5 As Single = 0.0, Optional UnknownP6 As Single = 0.0, Optional UnknownP7 As Double = 500.0, Optional UnknownP8 As Integer = 8, Optional UnknownP9 As Integer = 1, Optional UnknownP10 As Integer = 250, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_DRIVE_BY", Ped, Target, UnknownP3, UnknownP4, UnknownP5, UnknownP6, UnknownP7, UnknownP8, UnknownP9, UnknownP10)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub DropObject(Ped As Ped, Optional Unknown As Boolean = True)
            [Call]("DROP_OBJECT", Ped, Unknown)
        End Sub
        Public Shared Sub DropWeapon(Ped As Ped)
            [Call]("FORCE_CHAR_TO_DROP_WEAPON", Ped)
        End Sub
        Public Shared Sub EnterVehicle(Ped As Ped, Car As Vehicle, Seat As VehicleSeat, Optional Duration As Integer = -1, Optional MoveStyle As MovementType = MovementType.None, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If MoveStyle <> MovementType.None Then [Call]("SET_NEXT_DESIRED_MOVE_STATE", MoveStyle)
            If Seat = VehicleSeat.Driver Then [Call]("TASK_ENTER_CAR_AS_DRIVER", Ped, Car, Duration) Else [Call]("TASK_ENTER_CAR_AS_PASSENGER", Ped, Car, Duration, Seat)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub EveryoneLeaveVehicle(Car As Vehicle)
            If Car.Exists Then [Call]("TASK_EVERYONE_LEAVE_CAR", Car)
        End Sub
        Public Shared Sub FireWeapon(Ped As Ped, Target As Vector3)
            [Call]("FIRE_PED_WEAPON", Ped, Target.X, Target.Y, Target.Z)
        End Sub
        Public Shared Sub FlyHelicopterTo(Ped As Ped, Heli As Vehicle, p3_Usually_0 As Integer, UnknownRole As Integer, Destination As Vector3, p8_0_to_19 As Integer, Speed As Single, p10_0_to_30 As Integer, Optional p11_Up_To_90 As Single = -1, Optional UnknownAltitude As Integer = -1, Optional p13 As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_HELI_MISSION", Ped, Heli, p3_Usually_0, UnknownRole, Destination.X, Destination.Y, Destination.Z, p8_0_to_19, Speed, p10_0_to_30, p11_Up_To_90, UnknownAltitude, p13)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        ''' <param name="MoveType">If is set to "None = -1", il will be apply the flag MovementType.Walk</param>
        Public Shared Sub FollowNavMesh(Ped As Ped, Coordinate As Vector3, Radius As Single, HeadingAngle As Single, Optional MoveType As MovementType = MovementType.Run, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True, Optional Unknown As Single = -2)
            [Call]("TASK_FOLLOW_NAV_MESH_AND_SLIDE_TO_COORD", Ped, Coordinate.X, Coordinate.Y, Coordinate.Z, MoveType, Radius, HeadingAngle)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub ForceFlyThroughWindScreen_TLaD(Ped As Ped)
            [Call]("SET_PED_FORCE_FLY_THROUGH_WINDSCREEN", Ped)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        ''' <param name="MoveType">If is set to "None = -1", il will be apply the flag MovementType.Walk</param>
        Public Shared Sub GoToCoordStraight(Ped As Ped, Coordinates As Vector3, Optional MoveType As MovementType = MovementType.None, Optional Unknown As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If MoveType = MovementType.None Then MoveType = MovementType.Walk
            [Call]("TASK_GO_STRAIGHT_TO_COORD", Ped, Coordinates.X, Coordinates.Y, Coordinates.Z, MoveType, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        ''' <param name="MoveType">If is set to "None = -1", il will be apply the flag MovementType.Walk</param>
        Public Shared Sub GoToCoordWhileAiming(Ped As Ped, CoordinatesToGo As Vector3, CoordinatesToAim As Vector3, Optional MoveType As MovementType = MovementType.None, Optional UnknownP8_Set2_to_aim_char As Integer = 1, Optional UnknowP12_char As Integer = 0, Optional UnknownP6 As Integer = 0, Optional UnknownP7 As Integer = 0, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_GO_TO_COORD_WHILE_AIMING", Ped, CoordinatesToGo.X, CoordinatesToGo.Y, CoordinatesToGo.Z, MoveType, UnknownP6, UnknownP7, UnknownP8_Set2_to_aim_char, CoordinatesToAim.X, CoordinatesToAim.Y, CoordinatesToAim.Z, UnknowP12_char)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Sub GoToPed(Ped As Ped, TargetPed As Ped, Optional UnknownInt As Integer = -1, Optional UnknownFloat As Single = 3.0, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_GO_TO_CHAR", Ped, TargetPed, UnknownInt, UnknownFloat)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Sub GoToPedAiming(Ped1 As Ped, Ped2 As Ped, Flag1 As Boolean, Flag2 As Boolean, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_GOTO_CHAR_AIMING", Ped1, Ped2, Flag1, Flag2)
            AvoidTasksOverride(Ped1, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Sub GuardCurrentPosition(Ped As Ped, Duration_in_ms As Integer, Optional Unknown1 As Integer = 15, Optional Unknown2 As Integer = 10, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_GUARD_CURRENT_POSITION", Ped, Unknown1, Unknown2, Duration_in_ms)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Sub GuardDefensiveArea(Ped As Ped, DefensiveArea As Vector3, Duration_in_ms As Integer, Optional Unknown1 As Integer = 15, Optional Unknown2 As Integer = 10, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_GUARD_ASSIGNED_DEFENSIVE_AREA", Ped, DefensiveArea.X, DefensiveArea.Y, DefensiveArea.Z, Unknown1, Unknown2, Duration_in_ms)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub

        Public Shared Sub HandsUp(Ped As Ped, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_HANDS_UP", Ped)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Function isCrouching(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_DUCKING", Ped)
        End Function
        Public Shared Function isRespondingToAnyEvent(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_RESPONDING_TO_ANY_EVENT", Ped)
        End Function
        Public Shared Function isShooting(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_SHOOTING", Ped)
        End Function
        Public Shared Function isSittingIdle(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_SITTING_IDLE", Ped)
        End Function
        Public Shared Sub Jump(Ped As Ped) 'testare
            [Call]("TASK_JUMP", Ped)
        End Sub
        Public Shared Sub LeaveVehicle(Ped As Ped, Optional DontCloseDoor As Boolean = False, Optional Immediately As Boolean = False)
            Dim Car As Vehicle
            If Ped.Exists Then
                If Ped.isSittingInVehicle Then
                    Car = Ped.CurrentVehicle
                    If DontCloseDoor AndAlso Immediately = False Then
                        [Call]("TASK_LEAVE_CAR_DONT_CLOSE_DOOR", Ped, Car)
                    ElseIf Immediately Then
                        [Call]("TASK_LEAVE_CAR_IMMEDIATELY", Ped, Car)
                    ElseIf DontCloseDoor = False AndAlso Immediately = False Then
                        [Call]("TASK_LEAVE_CAR", Ped, Car)
                    End If
                End If
            End If
        End Sub
        Public Shared Sub LandHelicopter(Ped As Ped, Heli As Vehicle, Position As Vector3, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped.isSittingInVehicle AndAlso Ped.CurrentVehicle.Model.isHelicopter Then
                Ped.Task.LandHelicopter(Heli, Position)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End If
        End Sub
        Public Shared Sub LookAt(Ped As Ped, Vehicle As Vehicle, Optional Duration As Integer = -1, Optional Unknown As Boolean = False, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped.Exists AndAlso Vehicle.Exists Then [Call]("TASK_LOOK_AT_VEHICLE", Ped, Vehicle, Duration, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub LookAt(Ped As Ped, TargetPed As Ped, Optional Duration As Integer = -1, Optional Unknown As Boolean = False, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped.Exists AndAlso TargetPed.Exists Then [Call]("TASK_LOOK_AT_CHAR", Ped, TargetPed, Duration, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub LookAt(Ped As Ped, Obj As GTA.Object, Optional Duration As Integer = -1, Optional Unknown As Boolean = False, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped.Exists AndAlso Obj.Exists Then [Call]("TASK_LOOK_AT_OBJECT", Ped, Obj, Duration, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub LookAt(Ped As Ped, Coords As Vector3, Optional Duration As Integer = -1, Optional Unknown As Boolean = False, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped.Exists Then [Call]("TASK_LOOK_AT_COORD", Ped, Coords.X, Coords.Y, Coords.Z, Duration, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub

        ''' <summary>
        ''' If PassengerDoor=False, the player will prefer to open the driver door, but it will open also the passenger one if closer. 
        ''' If PassengerDoor=True, the player will only open the passenger door
        ''' </summary>
        Public Shared Sub OpenCarDoor(Ped As Ped, Car As Vehicle, Optional PassengerDoor As Boolean = False, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If PassengerDoor Then [Call]("TASK_OPEN_PASSENGER_DOOR", Ped, Car) Else [Call]("TASK_OPEN_DRIVER_DOOR", Ped, Car)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub RemoveCharDefensiveArea(Ped As Ped)
            [Call]("REMOVE_CHAR_DEFENSIVE_AREA", Ped)
        End Sub
        Public Shared Sub SetDefensiveArea(Ped As Ped, Position As Vector3, Radius As Single)
            [Call]("SET_CHAR_SPHERE_DEFENSIVE_AREA", Ped, Position.X, Position.Y, Position.Z, Radius)
        End Sub
        Public Shared Sub SlideToCoord(Ped As Ped, Coordinates As Vector3, Heading As Single, SpeedMultiplier As Single, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CHAR_SLIDE_TO_COORD", Ped, Coordinates.X, Coordinates.Y, Coordinates.Z, Heading, SpeedMultiplier)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        '------------------------------------------------------------------
        'Task Sit Down
        Public Enum SittingType 'p1, p2
            Chair_Front '0, 0
            Chair_Down '0, 1
            Chair_Left '0, 2
            Chair_Right '0, 3
            Stool_Left '1, 2
            Stool_Right '1, 3
            Couch_Front '2, 0
            Couch_Down '2, 1
            Wall '3, 0
            Step_Front '4, 0
            Step_DownBugged '4, 1
        End Enum
        'change animation idle -- > aggiungere funzione (ed enum sitting idles?)
        Public Shared Sub SitDown(Ped As Ped, SittingType As SittingType, Optional Time As Integer = -1, Optional Instantly As Boolean = False, Optional FacingHeading As Single = 180, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            getSittingFlags(SittingType)

            If Instantly Then
                Ped.Heading += FacingHeading
                [Call]("TASK_SIT_DOWN_INSTANTLY", Ped, pAnimSet, pAnimIndex, Time)
            Else
                [Call]("TASK_SIT_DOWN", Ped, pAnimSet, pAnimIndex, Time)
            End If
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub SitDownOnNearestObject(Ped As Ped, ObjPositiion As GTA.Vector3, SittingType As SittingType, ObjModel As String, FacingHeading As Single, Optional Time As Integer = -2, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            Dim objHash As Integer
            If ObjModel = "" Then
                objHash = -1
            Else
                objHash = NativeModels.GetHashKey(ObjModel)
            End If
            getSittingFlags(SittingType)

            [Call]("TASK_SIT_DOWN_ON_SEAT", Ped, pAnimSet, pAnimIndex, ObjPositiion.X, ObjPositiion.Y, ObjPositiion.Z, objHash, FacingHeading, Time, 0)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub SitDownOnSeat(Ped As Ped, SeatPosition As GTA.Vector3, SittingType As SittingType, FacingHeading As Single, Optional Time As Integer = -2, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            getSittingFlags(SittingType)

            [Call]("TASK_SIT_DOWN_ON_SEAT", Ped, pAnimSet, pAnimIndex, SeatPosition.X, SeatPosition.Y, SeatPosition.Z, FacingHeading, Time)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub StandUp(Ped As Ped, Optional Immediately As Boolean = False)
            NativePeds.Tasks.ClearAll(Ped, Immediately)
        End Sub
        Private Shared pAnimSet, pAnimIndex As Integer
        Private Shared Function getSittingFlags(SittingType As SittingType) As (Integer, Integer)
            Select Case SittingType
                Case SittingType.Chair_Front
                    pAnimSet = 0
                    pAnimIndex = 0
                Case SittingType.Chair_Down
                    pAnimSet = 0
                    pAnimIndex = 1
                Case SittingType.Chair_Left
                    pAnimSet = 0
                    pAnimIndex = 2
                Case SittingType.Chair_Right
                    pAnimSet = 0
                    pAnimIndex = 3
                Case SittingType.Stool_Left
                    pAnimSet = 1
                    pAnimIndex = 2
                Case SittingType.Stool_Right
                    pAnimSet = 1
                    pAnimIndex = 3
                Case SittingType.Couch_Front
                    pAnimSet = 2
                    pAnimIndex = 0
                Case SittingType.Couch_Down
                    pAnimSet = 2
                    pAnimIndex = 1
                Case SittingType.Wall
                    pAnimSet = 3
                    pAnimIndex = 0
                Case SittingType.Step_Front
                    pAnimSet = 4
                    pAnimIndex = 0
                Case SittingType.Step_DownBugged
                    pAnimSet = 4
                    pAnimIndex = 1
            End Select
            Return (pAnimSet, pAnimIndex)
        End Function
        Public Shared Sub TurnTo(Ped As Ped, TargetPed As Ped)
            If Ped.Exists AndAlso TargetPed.Exists Then [Call]("TASK_TURN_CHAR_TO_FACE_CHAR", Ped, TargetPed)
        End Sub
        Public Shared Sub TurnTo(Ped As Ped, Coords As GTA.Vector3)
            If Ped.Exists Then [Call]("TASK_TURN_CHAR_TO_FACE_COORD", Ped, Coords.X, Coords.Y, Coords.Z)
        End Sub
        Public Shared Sub TurnTo(Ped As Ped, Vehicle As Vehicle)
            If Ped.Exists AndAlso Vehicle.Exists Then [Call]("TASK_TURN_CHAR_TO_FACE_COORD", Ped, Vehicle.Position.X, Vehicle.Position.Y, Vehicle.Position.Z)
        End Sub
        Public Shared Sub TurnTo(Ped As Ped, Obj As GTA.Object)
            If Ped.Exists AndAlso Obj.Exists Then [Call]("TASK_TURN_CHAR_TO_FACE_COORD", Ped, Obj.Position.X, Obj.Position.Y, Obj.Position.Z)
        End Sub
        Public Shared Sub UseMobilePhone(Ped As Ped, Optional Duration As Integer = -1)
            [Call]("TASK_USE_MOBILE_PHONE_TIMED", Ped, Duration)
        End Sub
        Public Shared Sub Wait(Ped As Ped, Optional Time As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_PAUSE", Ped, Time)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub WalkAround(Ped As Ped, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_WANDER_STANDARD", Ped)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        'TASK_PERFORM_SEQUENCE
        'OPEN_SEQUENCE_TASK
        'CLOSE_SEQUENCE_TASK
        'CLEAR_SEQUENCE_TASK

        'GET_SEQUENCE_PROGRESS

        ' TASK_TIRED
        'TASK_SHOOT_AT_CHAR
        'TASK_SHOOT_AT_COORD
        'TASK_GO_STRAIGHT_TO_COORD
        'TASK_GO_TO_COORD_WHILE_AIMING
        'TASK_GO_TO_COORD_WHILE_SHOOTING
        'TASK_CAR_MISSION
        'TASK_SWIM_TO_COORD
        'SET_CHAR_COMPONENT_VARIATION
        'TASK_GUARD_ASSIGNED_DEFENSIVE_AREA
        'TASK_SWIM_TO_COORD

        'SWITCH_PED_PATHS_OFF
        'SWITCH_PED_PATHS_ON
        'SWITCH_PED_ROADS_BACK_TO_ORIGINAL
        'ARE_ANY_CHARS_NEAR_CHAR
        'ARE_ENEMY_PEDS_IN_AREA
        'IS_CHAR_WAITING_FOR_WORLD_COLLISION (ped)

        'SET_SENSE_RANGE(ped, Radius_float)
        'Public Shared Function CreateTasksSequence() As Integer 'testare
        '    Dim Pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        '    Dim TaskSequenceID As Integer
        '    TaskSequenceID = Pointer.Value
        '    [Call]("OPEN_SEQUENCE_TASK", TaskSequenceID)
        '    Return TaskSequenceID
        'End Function
        'Public Shared Sub PerformTaskSequence(Ped As Ped, TaskSequenceID As Integer, Optional ClearSequence As Boolean = True) 'testare
        '    [Call]("CLOSE_SEQUENCE_TASK", TaskSequenceID)
        '    [Call]("TASK_PERFORM_SEQUENCE", Ped, TaskSequenceID)
        '    If ClearSequence Then [Call]("CLEAR_SEQUENCE_TASK", TaskSequenceID)
        'End Sub
        'Stand_still
        'wander
    End Class
End Class