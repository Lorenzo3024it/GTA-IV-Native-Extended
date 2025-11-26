Imports System.Diagnostics.Eventing.Reader
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports System.Runtime.Remoting.Messaging
Imports GTA
Imports GTA.base
Imports GTA.Euphoria
Imports GTA.Native.Function
Imports GTA.value
Imports NativeExtended.NativeGeneric
Imports NativeExtended.Tools
Imports NativeExtended.NativeDraws
Imports NativeExtended.NativePeds.Animations
Imports NativeExtended.NativePeds.Clothes
'====================================================================================================================================================================================
'====================================================================================================================================================================================
' --------------- SHARED ---------------
Partial Public Class NativePeds
    ''' <summary>
    ''' They are not real native value, only used in many function to select
    ''' the ped's gender while spawning.
    ''' </summary>
    Public Enum Gender
        Male = 0
        Female = 1
        Random = -1
        Player = -2
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
    Public Shared Sub BlockCoweringInCover(Ped As Ped, Optional Enabled As Boolean = True)
        [Call]("BLOCK_COWERING_IN_COVER", Ped, Enabled)
    End Sub
    Public Shared Sub CanLookAtDuringConversation(Ped As Ped, ConversationID As Integer)
        [Call]("ALLOW_AUTO_CONVERSATION_LOOKATS", Ped, ConversationID)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub ClearLastDamageBone(Ped As Ped)
        [Call]("CLEAR_CHAR_LAST_DAMAGE_BONE", Ped)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub ClearLastDamageEntity(Ped As Ped)
        [Call](" 'CLEAR_CHAR_LAST_DAMAGE_ENTITY", Ped)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub ClearLastWeaponDamage(Ped As Ped)
        [Call]("CLEAR_CHAR_LAST_WEAPON_DAMAGE", Ped)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Ped"></param>
    ''' <param name="Alpha"></param>
    ''' Values between 0 (fully transparent) and 255 (fully visible).
    Public Shared Sub SetAlpha_TLaD(Ped As Ped, Alpha As Byte)
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
    ''' <summary>
    ''' Not working?
    ''' </summary>
    ''' <param name="Ped"></param>
    ''' <param name="MovementType">Use the enum MovementType (not the value "None = -1")</param>
    Public Shared Sub SetMovementType(Ped As Ped, MovementType As MovementType)
        [Call]("MODIFY_CHAR_MOVE_STATE", Ped, MovementType)
    End Sub
    Public Shared Sub SetMoveSpeed(Ped As Ped, Multiplier As Single)
        [Call]("SET_CHAR_MOVE_ANIM_SPEED_MULTIPLIER", Ped, Multiplier)
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub SetPaths(Ped As Ped, ClimbOvers As Boolean, DropFromHeight As Boolean, Ladders As Boolean)
        [Call]("SET_PED_PATH_MAY_USE_CLIMBOVERS", Ped, ClimbOvers)
        [Call]("SET_PED_PATH_MAY_DROP_FROM_HEIGHT", Ped, DropFromHeight)
        [Call]("SET_PED_PATH_MAY_USE_LADDERS", Ped, ClimbOvers)
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub SedProvideCoveringFire(Ped As Ped, CoveringFire As Boolean)
        [Call]("SET_CHAR_PROVIDE_COVERING_FIRE", Ped, CoveringFire)
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isTryingToEnterInLockedCar(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_TRYING_TO_ENTER_A_LOCKED_CAR", Ped)
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isStuckUnderCar(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_STUCK_UNDER_CAR", Ped)
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub SetSenseRange(Ped As Ped, Range As Single)
        [Call]("SET_SENSE_RANGE", Ped, Range)
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub SetSwimSpeed(Ped As Ped, Speed As Single)
        [Call]("SET_SWIM_SPEED", Ped, Speed)
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub SetMoveWhenInjured(Ped As Ped, WillMove As Boolean)
        [Call]("SET_CHAR_WILL_MOVE_WHEN_INJURED", Ped, WillMove)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
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
    Public Shared Function SpawnRandomPed(Position As GTA.Vector3, Optional Gender As Gender = Gender.Random) As Ped
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
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub ForcePinnedDown(Ped As Ped, PinnedDown As Boolean, Time As Integer)
        [Call]("FORCE_PED_PINNED_DOWN", Ped, PinnedDown, Time)
    End Sub
    Public Shared Sub DeletePed(Ped As Ped, Optional Elegantly As Boolean = True)
        If Ped.Exists Then
            If Elegantly Then [Call]("REMOVE_CHAR_ELEGANTLY", Ped) Else [Call]("DELETE_CHAR", Ped)
        Else
            WriteLog("Error in NativePeds.DeletePed: Ped doesn't exist.", True)
        End If
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub SetDropWeaponsWhenDead(Ped As Ped, Optional DropWeapons As Boolean = True)
        [Call]("SET_CHAR_DROPS_WEAPONS_WHEN_DEAD", Ped, DropWeapons)

    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub SetSuffersCriticalHits(Ped As Ped, Suffers As Boolean)
        [Call]("SET_CHAR_SUFFERS_CRITICAL_HITS", Ped, Suffers)
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub CanLeaveCarDuringCombat(Ped As Ped, CanLeaveCar As Boolean)
        [Call]("SET_CHAR_WILL_LEAVE_CAR_IN_COMBAT", Ped, CanLeaveCar)
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isFatallyInjured(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_FATALLY_INJURED", Ped)
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isGettingUp(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_GETTING_UP", Ped)
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isFacingChar(Ped As Ped, PedToLook As Ped, Angle As Double) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_FACING_CHAR", Ped, PedToLook, Angle)
    End Function
    Public Shared Function GetGender(Ped As Ped) As Gender
        If [Call](Of Boolean)("IS_CHAR_MALE", Ped) Then Return Gender.Male Else Return Gender.Female
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub GiveAmbientObject(Ped As Ped, ObjectModel As String, Optional Unknown As Boolean = True)
        If Ped IsNot Nothing Then
            NativeModels.Request(ObjectModel)
            If NativeModels.HasModelLoaded(ObjectModel) Then
                [Call]("GIVE_PED_AMBIENT_OBJECT", Ped, NativeModels.GetHashKey(ObjectModel), Unknown)
            Else
                WriteLog("Error in NativePeds.GiveAmbientObject. You have to request ObjectModel first!")
            End If
        Else
            WriteLog("Error in NativePeds.GiveAmbientObject. Ped doesn't exists!")
        End If
    End Sub
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isInCover(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_PED_IN_COVER", Ped)
    End Function
    Public Shared Function isInTaxi(Ped As Ped) As Boolean
        If Ped.Exists Then
            Return [Call](Of Boolean)("IS_CHAR_IN_TAXI", Ped)
        Else
            WriteLog("Error in NativePeds.isInTaxi: Ped doesn't exist.", True)
            Return Nothing
        End If
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isStuckedUnderCar(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_STUCK_UNDER_CAR", Ped)
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Function isWaitingForWorldCollision(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_WAITING_FOR_WORLD_COLLISION", Ped)
    End Function
    ''' <summary>
    ''' untested
    ''' </summary>
    Public Shared Sub BlockPeekingInCover(Ped As Ped, Optional BlockCover As Boolean = True)
        [Call]("BLOCK_PEEKING_IN_COVER", Ped, BlockCover)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Function CanPedSeeDeadChar(Ped As Ped, DeadChar As Ped) As Boolean
        Return [Call](Of Boolean)("CAN_CHAR_SEE_DEAD_CHAR", Ped, DeadChar)
    End Function
    ''' <summary>
    ''' Non-native function to get the ped's current vehicle seat.
    ''' </summary>
    ''' <returns>404 is ped doesn't exist; 127 if not sitting in vehicle.</returns>
    Public Shared Function GetCurrentVehicleSeat(Ped As Ped) As GTA.VehicleSeat
        If Ped.Exists Then
            If Ped.isSittingInVehicle Then
                Dim CurrentVehicleSeat As VehicleSeat

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
                Return 127
            End If
        Else
            Return 404
        End If
    End Function
    ''' <summary>
    ''' Non-native function.
    ''' </summary>
    ''' <returns>Height above the ground in meters. Returns -1 if ground not found.</returns>
    Public Shared Function GetHeightAboveGround(Ped As Ped) As Single
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
                Return -1.0F
            End If

        Catch ex As Exception
            Return -1.0F
        End Try
    End Function
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Function GetNumberOfInjuredPeds(Position As Vector3, Radius As Single) As Integer
        Return [Call](Of Integer)("GET_NUMBER_OF_INJURED_PEDS_IN_RANGE", Position.X, Position.Y, Position.Z, Radius)
    End Function
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetHeadOnHornWhenDeadInVehicle(Ped As Ped, AlwaysHeadOnHorn As Boolean)
        [Call]("ALWAYS_USE_HEAD_ON_HORN_ANIM_WHEN_DEAD_IN_CAR", Ped, AlwaysHeadOnHorn)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetDoEvasiveDives(Ped As Ped, DoEvasiveDives As Boolean)
        [Call]("SET_PED_DONT_DO_EVASIVE_DIVES", Ped, Not DoEvasiveDives)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetDrugged(Ped As Ped, Drugged As Boolean)
        [Call]("SET_CHAR_DRUGGED_UP", Ped, Drugged)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetDrownsInWater(Ped As Ped, Drowns As Boolean, DrownsInSinkingVehicle As Boolean)
        [Call]("SET_CHAR_DROWNS_IN_WATER", Ped, Drowns)
        [Call]("SET_CHAR_DROWNS_IN_SINKING_VEHICLE", Ped, DrownsInSinkingVehicle)
    End Sub
    Public Shared Sub SetNeverGetTargettedByPlayer(Ped As Ped, Optional NeverTargetted As Boolean = True)
        Native.Function.Call("SET_CHAR_NEVER_TARGETTED", Ped, NeverTargetted)
    End Sub
    Public Shared Sub SetNoRagdollFromPlayerImpact(Ped As Ped, NoRagdoll As Boolean)
        If Ped.Exists Then [Call]("SET_DONT_ACTIVATE_RAGDOLL_FROM_PLAYER_IMPACT", Ped, NoRagdoll) Else WriteLog("Error in NativePeds.SetNoRagdollFromPlayerImpact: ped doesn't exist.", True)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    ''' <param name="Rate">Values between 0 and 100</param>
    Public Shared Sub SetShootRate(Ped As Ped, Rate As Integer)
        [Call]("SET_CHAR_SHOOT_RATE", Ped, Rate)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetBleeding(Ped As Ped, Bleeding As Boolean)
        [Call]("SET_CHAR_BLEEDING", Ped, Bleeding)
    End Sub
    Public Shared Sub SetCollision(Ped As Ped, Collision As Boolean)
        [Call]("SET_CHAR_COLLISION", Ped, Collision)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetIgnoreWeaponRange(Ped As Ped, Optional Enabled As Boolean = True)
        [Call]("TASK_SET_IGNORE_WEAPON_RANGE_FLAG", Ped, Enabled)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub DamageBodyPart(Ped As Ped, BodyPart As UInteger, HitPoints As UInteger)
        [Call]("DAMAGE_PED_BODY_PART", Ped, BodyPart, HitPoints)
    End Sub
    Public Shared Sub SetRomansMood(Mood As RomanMood)
        [Call]("SET_ROMANS_MOOD", Mood)
    End Sub
    Public Shared Sub Freeze(Ped As Ped, Optional Freeze As Boolean = True, Optional DontLoadCollision As Boolean = False, Optional Visible As Boolean? = Nothing)
        If Freeze Then
            If DontLoadCollision Then
                [Call]("FREEZE_CHAR_POSITION_AND_DONT_LOAD_COLLISION", Ped, True)
            Else
                [Call]("FREEZE_CHAR_POSITION", Ped, True)
            End If
            If Visible.HasValue Then Ped.Visible = Visible.Value
        Else
            [Call]("FREEZE_CHAR_POSITION", Ped, False)
            [Call]("FREEZE_CHAR_POSITION_AND_DONT_LOAD_COLLISION", Ped, False)
            If Visible.HasValue Then Ped.Visible = Visible.Value
        End If
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetTargetPriority(Ped As Ped, Optional Enabled As Boolean = True)
        [Call]("SET_CHAR_IS_TARGET_PRIORITY", Ped, Enabled)
    End Sub
    Public Shared Sub WarpFromCarToCar(Ped As Ped, Car As Vehicle, Optional Unknown As Integer = -1)
        [Call]("WARP_CHAR_FROM_CAR_TO_CAR", Ped, Car, Unknown)
    End Sub
    Public Shared Sub WarpFromCarToCoords(Ped As Ped, Coords As Vector3)
        [Call]("WARP_CHAR_FROM_CAR_TO_COORD", Ped, Coords.X, Coords.Y, Coords.Z)
    End Sub
    Public Shared Sub WarpIntoVehicle(Ped As Ped, Car As Vehicle, Seat As VehicleSeat, Optional UseTaskFunction As Boolean = False)
        If UseTaskFunction Then
            If Seat = VehicleSeat.Driver Then
                [Call]("TASK_WARP_CHAR_INTO_CAR_AS_DRIVER", Ped, Car, Seat)
            Else
                [Call]("TASK_WARP_CHAR_INTO_CAR_AS_PASSENGER", Ped, Car, Seat)
            End If
        Else
            If Seat = VehicleSeat.Driver Then
                [Call]("WARP_CHAR_INTO_CAR", Ped, Car)
            Else
                [Call]("WARP_CHAR_INTO_CAR_AS_PASSENGER", Ped, Car, Seat)
            End If
        End If
    End Sub
    'GET_CHAR_SWIM_STATE
    'GIVE_PED_AMBIENT_OBJECT(ped, Hashkey as integer, unknownbool) 'load object first
    ' GIVE_PED_PICKUP_OBJECT(ped, object, unknownbool) 'load object first
    'SET_CHAR_FIRE_DAMAGE_MULTIPLIER

    ' ADD_FOLLOW_NAVMESH_TO_PHONE_TASK( l_U481, 478.60000000, 1564.86000000, 15.22110000 ); jacob2 and elizabeta3 -->in elizabeta 3 fir par is player index

    Public Class Animations
        Public ReadOnly AnimSet As String
        Public ReadOnly AnimName As String
        Private onlyUpperBody As Boolean
        Private isloop As Boolean
        Public Sub New(AnimSet As String, AnimName As String)
            Me.AnimSet = AnimSet
            Me.AnimName = AnimName
        End Sub
        Public Sub Play(ByVal Ped As Ped, ByVal Speed As Single, Optional IsLoop As Boolean = False, Optional InsideVehicle As Boolean = False, Optional CanWalk As Boolean = False, Optional Duration As Integer = -1,
                                       Optional FreezePlayerControl As Boolean = False, Optional FreezeOnLastAnimFrame As Boolean = False, Optional ComeBackToOriginalPosition As Boolean = False, Optional FreezeCamDuringAnim As Boolean = False, Optional ReturnToLastAnimation As Boolean = False)
            [Call]("REQUEST_ANIMS", AnimSet)
            Me.isloop = IsLoop
            Me.onlyUpperBody = CanWalk
            If InsideVehicle Then
                [Call]("TASK_PLAY_ANIM_SECONDARY_IN_CAR", Ped, AnimName, AnimSet, CSng(Speed), IsLoop, FreezePlayerControl, FreezePlayerControl, FreezeOnLastAnimFrame, Duration)
            ElseIf CanWalk Then
                [Call]("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY", Ped, AnimName, AnimSet, CSng(Speed), IsLoop, FreezePlayerControl, FreezePlayerControl, FreezeOnLastAnimFrame, Duration)
            Else
                [Call]("TASK_PLAY_ANIM_WITH_ADVANCED_FLAGS", Ped, AnimName, AnimSet, CSng(Speed), IsLoop, ReturnToLastAnimation, ReturnToLastAnimation, ComeBackToOriginalPosition, FreezeOnLastAnimFrame, ComeBackToOriginalPosition, FreezeCamDuringAnim, Duration)
            End If
        End Sub
        ''' <summary>
        ''' Returns 404 if Ped doesn't not exists or 999 if the function
        ''' catches an error (with logging in NativeExtended-IV.log).
        ''' </summary>
        Public Function GetCurrentTime(Ped As Ped) As Integer
            Try
                If Ped Is Nothing Then
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...." & AnimName & ".GetCurrentTime: Ped not found.")
                    Return 404
                Else
                    Dim pointer As Native.Pointer = New Native.Pointer(GetType(Single))
                    [Call]("GET_CHAR_ANIM_CURRENT_TIME", AnimSet, AnimName, pointer)
                    Return pointer.Value
                End If
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...." & AnimName & ".GetCurrentTime. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended...." & AnimName & ".GetCurrentTime.", ex)
                Return 999
            End Try
        End Function
        Public Function HasLoaded() As Boolean
            Try
                Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimSet & ".HasLoaded. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended.." & AnimSet & ".HasLoaded. ", ex)
            End Try
        End Function
        Public Function isPlaying(Ped As Ped) As Boolean
            Try
                If Ped Is Nothing Then
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".isPlaying: Ped not found.")
                    Return False
                Else
                    Return [Call](Of Boolean)("IS_CHAR_PLAYING_ANIM", Ped, AnimSet, AnimName)
                End If
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".isPlaying. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".isPlaying.", ex)
                Return False
            End Try
        End Function
        Public Sub Relase()
            Try
                [Call]("REMOVE_ANIMS", AnimSet)
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimSet & ".Relase. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended.." & AnimSet & ".Relase.", ex)
            End Try
        End Sub
        Public Sub Request()
            Try
                [Call]("REQUEST_ANIMS", AnimSet)
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimSet & ".Request. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended.." & AnimSet & ".Request.", ex)
            End Try
        End Sub
        Public Sub SetCurrentTime(Ped As Ped, Speed As Single)
            Try
                If Ped Is Nothing Then
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".SetCurrentTime: Ped not found.")
                Else
                    [Call]("SET_CHAR_ANIM_CURRENT_TIME", Ped, AnimSet, AnimName, Speed)
                End If
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".SetCurrentTime. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".SetCurrentTime.", ex)
            End Try
        End Sub
        Public Sub SetSpeed(Ped As Ped, Speed As Single)
            Try
                If Ped Is Nothing Then
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".SetSpeed: Ped not found.")
                Else
                    [Call]("SET_CHAR_ANIM_SPEED", Ped, AnimSet, AnimName, Speed)
                End If
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".SetSpeed. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".SetSpeed.", ex)
            End Try
        End Sub
        Public Sub [Stop](Ped As Ped)
            Try
                If Ped Is Nothing Then
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".Stop: Ped not found.")
                Else
                    Dim CurrVeh As Vehicle
                    Dim CurrSeat As VehicleSeat
                    If onlyUpperBody Then
                        [Call]("CLEAR_CHAR_SECONDARY_TASK", Ped)
                    Else
                        [Call]("CLEAR_CHAR_TASKS", Ped)
                    End If
                    If isloop Then
                        If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
                            CurrVeh = Ped.CurrentVehicle
                            CurrSeat = GetCurrentVehicleSeat(Ped)
                            [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                            If CurrVeh.Exists Then Ped.WarpIntoVehicle(CurrVeh, CurrSeat)
                        Else
                            [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                        End If
                    End If
                End If
            Catch ex As Exception
                If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".Stop. " & ex.Message)
                WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".Stop.", ex)
            End Try
        End Sub
        '--------------------------------------------------------------------------------------------------------------------
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
        Public Shared Sub BlockAmbientAnims(Ped As Ped, BlockAnim As Boolean)
            [Call]("BLOCK_CHAR_AMBIENT_ANIMS", Ped, BlockAnim)
        End Sub
        Public Shared Sub BlockHeadIK(Ped As Ped, Optional BlockIK As Boolean = True)
            [Call]("BLOCK_CHAR_HEAD_IK", Ped, BlockIK)
        End Sub
        Public Shared Sub BlockLegIK(Ped As Ped, Optional BlockIK As Boolean = True)
            [Call]("SET_PED_ENABLE_LEG_IK", Ped, Not BlockIK)
        End Sub
        Public Shared Function GetAnimCurrentTime(Ped As Ped, AnimSet As String, AnimName As String) As Integer
            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Single))
            [Call]("GET_CHAR_ANIM_CURRENT_TIME", AnimSet, AnimName, pointer)
            Return pointer.Value
        End Function
        Public Shared Function HaveAnimsLoaded(AnimSet As String) As Boolean
            Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
        End Function
        Public Shared Function isPlayingAnim(ByVal Ped As Ped, ByVal AnimSet As String, ByVal AnimName As String) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_PLAYING_ANIM", Ped, AnimSet, AnimName)
        End Function
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
        Public Shared Sub Relase(AnimSet As String)
            [Call]("REMOVE_ANIMS", AnimSet)
        End Sub
        Public Shared Sub Request(AnimSet As String)
            [Call]("REQUEST_ANIMS", AnimSet)
        End Sub
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
                    CurrSeat = GetCurrentVehicleSeat(Ped)
                    [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                    If CurrVeh.Exists Then Ped.WarpIntoVehicle(CurrVeh, CurrSeat)
                Else
                    [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                End If
            End If
        End Sub
        Public Class Gestures
            Public ReadOnly AnimSet As String
            Public ReadOnly AnimName As String
            Public Sub New(AnimSet As String, AnimName As String)
                Me.AnimSet = AnimSet
                Me.AnimName = AnimName
            End Sub
            ''' <summary>
            ''' Untested. Returns 404 if Ped doesn't not exists or 999 if the function
            ''' catches an error (with logging in NativeExtended-IV.log).
            ''' </summary>
            Public Function GetCurrentTime(Ped As Ped) As Integer
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".GetCurrentTime: Ped not found.")
                        Return 404
                    Else
                        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Single))
                        [Call]("GET_CHAR_ANIM_CURRENT_TIME", AnimSet, AnimName, pointer)
                        Return pointer.Value
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".GetCurrentTime. " & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".GetCurrentTime.", ex)
                    Return 999
                End Try
            End Function
            Public Function isPlaying(Ped As Ped) As Boolean
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".isPlaying: Ped not found.")
                        Return False
                    Else
                        Return [Call](Of Boolean)("IS_CHAR_PLAYING_ANIM", Ped, AnimSet, AnimName)
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".isPlaying. " & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".isPlaying.", ex)
                    Return False
                End Try
            End Function
            Public Sub Play(Ped As Ped, Optional Speed As Single = 1.0)
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".Play: Ped not found.")
                    Else
                        [Call]("REQUEST_ANIMS", AnimSet)

                        If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
                            [Call]("TASK_PLAY_ANIM_SECONDARY_IN_CAR", Ped, AnimName, AnimSet, Speed, False, False, False, False)
                        Else
                            [Call]("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY", Ped, AnimName, AnimSet, Speed, False, False, False, False)
                        End If
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".Play. " & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".Play.", ex)
                End Try
            End Sub
            '-------------------------------------------------------------------------------------------------------
            Public Shared Sub BlockGestures(Ped As Ped, Optional BlockGestureAnims As Boolean = True)
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...BlockGestures: Ped not found.")
                    Else
                        [Call]("BLOCK_CHAR_GESTURE_ANIMS", Ped, BlockGestureAnims)
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...BlockGestures. " & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended...BlockGestures.", ex)
                End Try
            End Sub
            Public Shared Function isGesturing(Ped As Ped) As Boolean
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...isGesturing: Ped not found.")
                    Else
                        Return [Call](Of Boolean)("IS_CHAR_GESTURING", Ped)
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...isGesturing. " & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended...isGesturing.", ex)
                End Try
            End Function
            Public Structure HandMale
                Public Const AnimSet As String = "gestures@mp_male"

                Public Shared ReadOnly Finger As New Gestures(AnimSet, "finger")
                Public Shared ReadOnly Rock As New Gestures(AnimSet, "rock")
                Public Shared ReadOnly Hello As New Gestures(AnimSet, "wave")
                Public Shared ReadOnly Salute As New Gestures(AnimSet, "salute")
                Public Shared Function HasLoaded() As Boolean?
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...HandMale.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.HandMale.HasLoaded.", ex)
                        Return Nothing
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...HandMale.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.HandMale.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...HandMale.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.HandMale.Request.", ex)
                    End Try
                End Sub
            End Structure
            Public Structure HandFemale
                Public Const AnimSet As String = "gestures@mp_female"

                Public Shared ReadOnly Finger As New Gestures(AnimSet, "finger")
                Public Shared ReadOnly Rock As New Gestures(AnimSet, "rock")
                Public Shared ReadOnly Hello As New Gestures(AnimSet, "wave")
                Public Shared ReadOnly Salute As New Gestures(AnimSet, "salute")
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...HandFemale.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.HandFemale.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...HandFemale.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.HandFemale.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...HandFemale.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.HandFemale.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Male.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Male.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Male.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Male.Relase." & ex.Message, ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Male.Request error. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Male.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Female.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Female.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Female.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Female.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Female.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Female.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...CarMale.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.CarMale.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...CarMale.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.CarMale.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...CarMale.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.CarMale.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...CarFemale.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.CarFemale.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...CarFemale.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.CarFemale.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...CarFemale.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.CarFemale.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Seated.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.CarFemale.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Seated.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Seated.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Seated.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Seated.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Niko.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Niko.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Niko.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Niko.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Niko.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.Niko.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...BikeEFLC.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.BikeEFLC.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...BikeEFLC.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.BikeEFLC.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...BikeEFLC.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Gestures.BikeEFLC.Request.", ex)
                    End Try
                End Sub
            End Structure
            'Public Shared Function HaveGesturesLoaded(GestureSet As String) As Boolean
            '    Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", GestureSet)
            'End Function
            'Public Shared Sub Play(Ped As Ped, Gesture As Gestures, Optional Speed As Single = 1.0)
            '    [Call]("REQUEST_ANIMS", Gesture.AnimSet)
            '    'Do
            '    '    Game.WaitInCurrentScript(1)
            '    'Loop Until [Call](Of Boolean)("HAS_ANIMS_LOADED")
            '    If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
            '        [Call]("TASK_PLAY_ANIM_SECONDARY_IN_CAR", Ped, Gesture.AnimName, Gesture.AnimSet, Speed, False, False, False, False)
            '    Else
            '        [Call]("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY", Ped, Gesture.AnimName, Gesture.AnimSet, Speed, False, False, False, False)
            '    End If
            'End Sub
            'Public Shared Sub Relase(GestureSet As String)
            '    [Call]("REMOVE_ANIMS", GestureSet)
            'End Sub
            'Public Shared Sub Request(GestureSet As String)
            '    [Call]("REQUEST_ANIMS", GestureSet
            'End Sub
        End Class
        Public Class Facials
            Public ReadOnly AnimSet As String
            Public ReadOnly AnimName As String
            Public Sub New(AnimSet As String, AnimName As String)
                Me.AnimSet = AnimSet
                Me.AnimName = AnimName
            End Sub
            ''' <summary>
            ''' Untested. Returns 404 if Ped doesn't not exists or 999 if the function
            ''' catches an error (with logging in NativeExtended-IV.log).
            ''' </summary>
            Public Function GetCurrentTime(Ped As Ped) As Integer
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".GetCurrentTimePed not found.")
                        Return 404
                    Else
                        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Single))
                        [Call]("GET_CHAR_ANIM_CURRENT_TIME", AnimSet, AnimName, pointer)
                        Return pointer.Value
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".GetCurrentTime error: " & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".GetCurrentTime.", ex)
                    Return 999
                End Try
            End Function
            Public Function isPlaying(Ped As Ped) As Boolean
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".isPlaying: Ped not found.")
                        Return Nothing
                    Else
                        Return [Call](Of Boolean)("IS_CHAR_PLAYING_ANIM", Ped, AnimSet, AnimName)
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".isPlaying. " & ex.Message)
                    WriteLogAdvanced("Error in Facials.." & AnimName & ".isPlaying.", ex)
                    Return Nothing
                End Try
            End Function
            Public Sub Play(Ped As Ped, Optional Duration As Integer = -1, Optional PlayInLoop As Boolean = False, Optional Flag2 As Boolean = False, Optional Speed As Single = 1.00001)
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".Play: Ped not found.")
                    Else
                        [Call]("REQUEST_ANIMS", AnimSet)
                        [Call]("TASK_PLAY_ANIM_FACIAL", Ped, AnimName, AnimSet, Speed, PlayInLoop, Flag2, Duration)
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended.." & AnimName & ".Play." & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended.." & AnimName & ".Play error. ", ex)
                End Try
            End Sub
            '----------------------------------------------------------------------------------------------------------------------------------------------------
            Public Shared Sub BlockFacialAnims(Ped As Ped, BlockAnim As Boolean)
                Try
                    If Ped Is Nothing Then
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...BlockFacialAnims: Ped not found.")
                    Else
                        [Call]("BLOCK_CHAR_VISEME_ANIMS", Ped, BlockAnim)
                    End If
                Catch ex As Exception
                    If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...BlockFacialAnims. " & ex.Message)
                    WriteLogAdvanced("Error in NativeExtended...BlockFacialAnims.", ex)
                End Try
            End Sub

            'Public Shared Function haveFacialLoaded(FacialSet As String) As Boolean
            '    Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", FacialSet)
            'End Function
            'Public Shared Sub Relase(FacialSet As String)
            '    [Call]("REMOVE_ANIMS", FacialSet)
            'End Sub
            'Public Shared Sub Request(FacialSet As String)
            '    [Call]("REQUEST_ANIMS", FacialSet)
            'End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Player.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Player.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Player.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Player.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Player.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Player.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Male.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Male.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Male.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Male.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Male.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Male.Request.", ex)
                    End Try
                End Sub
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
                Public Shared Function HasLoaded() As Boolean
                    Try
                        Return [Call](Of Boolean)("HAVE_ANIMS_LOADED", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Female.HasLoaded. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Female.HasLoaded.", ex)
                    End Try
                End Function
                Public Shared Sub Relase()
                    Try
                        [Call]("REMOVE_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Female.Relase. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Female.Relase.", ex)
                    End Try
                End Sub
                Public Shared Sub Request()
                    Try
                        [Call]("REQUEST_ANIMS", AnimSet)
                    Catch ex As Exception
                        If NativeDebug.DisplayErrors = True Then DisplayText(TextColors.MP_Red & "Error in NativeExtended...Female.Request. " & ex.Message)
                        WriteLogAdvanced("Error in NativeExtended...Facials.Female.Request.", ex)
                    End Try
                End Sub
            End Structure
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
        Public Shared Function GetCurrentPropIndex(Ped As Ped, Prop As Props) As UInteger
            Dim Pointer As Native.Pointer = New Native.Pointer(GetType(UInteger))
            [Call]("GET_CHAR_PROP_INDEX", Ped, Prop, Pointer)
            Return Pointer.Value
        End Function
        Public Shared Function GetCurrentModelVariation(Ped As Ped, ComponentID As Components) As Integer
            Return [Call](Of Integer)("GET_CHAR_DRAWABLE_VARIATION", Ped, ComponentID)
        End Function
        Public Shared Function GetCurrentTextureVariation(Ped As Ped, ComponentID As Components) As Integer
            Return [Call](Of Integer)("GET_CHAR_TEXTURE_VARIATION", Ped, ComponentID)
        End Function
        Public Shared Function GetNumberOfModelVariations(Ped As Ped, ComponentID As Components) As Integer
            Return [Call](Of Integer)("GET_NUMBER_OF_CHAR_DRAWABLE_VARIATIONS", Ped, ComponentID)
        End Function
        Public Shared Function GetNumberOfTextureVariations(Ped As Ped, ComponentID As Components, ModelVariation As Integer) As Integer
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
        'Public Shared Sub RemoveHelmetFromPlayer()
        '    [Call]("REMOVE_PLAYER_HELMET", NativePlayer.Index)
        'End Sub
        Public Shared Sub RemoveHelmet(Ped As Ped) 'testare
            If Ped = NativePlayer.Character Then
                [Call]("REMOVE_PLAYER_HELMET", NativePlayer.Index)
            Else
                If Ped IsNot Nothing Then
                    [Call]("REMOVE_PED_HELMET", Ped)
                Else
                End If
            End If
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
        Public Shared Function isAmbientSpeechDisabled(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_AMBIENT_SPEECH_DISABLED", Ped)
        End Function
        Public Shared Function isSayingAmbientSpeech(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_AMBIENT_SPEECH_PLAYING", Ped)
        End Function
        Public Shared Function isSayingAnySpeech(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_ANY_SPEECH_PLAYING", Ped)
        End Function
        Public Shared Function isSayingScriptedSpeech(Ped As Ped) As Boolean
            If Ped.Exists Then
                Return [Call](Of Boolean)("IS_SCRIPTED_SPEECH_PLAYING", Ped)
            Else
                WriteLog("Error in NativePeds...isSayingScriptedSpeech: ped doesn't exist.", True)
                Return Nothing
            End If
        End Function
        Public Shared Sub SayAmbientSpeech(Ped As Ped, SpeechName As String, PlayAfterAWhile As Boolean, Optional Voice As String = "", Optional CancelMegaphone As Boolean = True)
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
        Public Shared Sub SayAudioEvent(Ped As Ped, AudioEvent As AudioEvents)
            Dim Str As String
            Select Case AudioEvent
                Case AudioEvents.PainLow
                    Str = "PAIN_LOW"
                Case AudioEvents.PainHigh
                    Str = "PAIN_HIGH"
                Case AudioEvents.LeftFootRun_TBoGT
                    Str = "LEFT_FOOT_RUN"
                Case AudioEvents.RightFootRun_TBoGT
                    Str = "RIGHT_FOOT_RUN"
            End Select
            [Call]("HANDLE_AUDIO_ANIM_EVENT", Ped, Str)
        End Sub
        Public Shared Sub SayScream(Ped As Ped, ScreamType As Screams)
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
        Public Enum AudioEvents
            PainLow = 0
            PainHigh = 1
            LeftFootRun_TBoGT = 2
            RightFootRun_TBoGT = 3
        End Enum
        Public Enum Screams
            PanicScream_NotPlayer = -1
            OnFire = 0
            HighFall = 1
        End Enum
        'Public Class Scripted

        'End Class
    End Class
    Public Class Tasks
        Public Enum CarActions
            SlowDownSoftly = 1
            SlowDownHardly_AndUnusableForEver = 2
            SlowDownHardly_AndDriveBackwards = 3
            SlowDownHardly_AndTurnLeft = 4
            SlowDownHardly_AndTurnRight = 5
            SlowDownHardly = 6
            TurnLeft = 7
            TurnRight = 8
            DriveForwardsHighRPM = 9
            SoftTurnRight = 10
            SoftTurnLeft = 11
        End Enum
        Public Enum DrivingStyle
            NormalDriving_StopsForTrafficLights = 0
            NormalDriving_StopsForTrafficLights2 = 1
            IgnoreTraffic_DriveAround = 2
            IgnoreTraffic_StayInLane = 3
            NormalDriving_IgnoreTrafficLights = 4
        End Enum
        Public Enum DrivingFlags
            DoNothing = 0
            DriveAroundCity = 1
            FollowTarget = 2
            FollowTarget_ThenStop = 4
        End Enum
        Public Shared Sub AimAt(Ped As Ped, TargetPed As Ped, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped = Nothing Then
                [Call]("TASK_AIM_GUN_AT_CHAR", 0, TargetPed, Duration)
            Else
                [Call]("TASK_AIM_GUN_AT_CHAR", Ped, TargetPed, Duration)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End If
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
        Public Shared Sub ChangeCarSeat(Ped As Ped)
            If Ped.isSittingInVehicle Then
                [Call]("TASK_SHUFFLE_TO_NEXT_CAR_SEAT", Ped, Ped.CurrentVehicle)
            Else
                Exit Sub
            End If
        End Sub
        Public Shared Sub ChatWithPed(Ped As Ped, TargetPed As Ped, Unknown1 As Boolean, Optional Unknown2 As Boolean = True, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CHAT_WITH_CHAR", Ped, TargetPed, Unknown1, Unknown2)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub ClearAll(Ped As Ped, Optional Immediately As Boolean = False)
            If Immediately Then
                If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
                    Dim CurrVeh = Ped.CurrentVehicle
                    Dim CurrSeat As VehicleSeat = GetCurrentVehicleSeat(Ped)
                    [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                    If CurrVeh.Exists Then Ped.WarpIntoVehicle(CurrVeh, CurrSeat)
                Else
                    [Call]("CLEAR_CHAR_TASKS_IMMEDIATELY", Ped)
                End If
            Else
                [Call]("CLEAR_CHAR_TASKS", Ped)
            End If
        End Sub
        Public Shared Sub Climb(Ped As Ped, Optional Unknown As Boolean = True)
            [Call]("TASK_CLIMB", Ped, Unknown)
        End Sub
        Public Shared Sub ClimbLadder(Ped As Ped, Optional Unknown As Boolean = True)
            [Call]("TASK_CLIMB_LADDER", Ped, Unknown)
        End Sub
        Public Shared Sub Combat(Ped As Ped, Target As Ped, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Duration > 0 Then [Call]("TASK_COMBAT_TIMED", Ped, Target, Duration) Else [Call]("TASK_COMBAT", Ped, Target)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub CombatHatedTargetsAround(Ped As Ped, Radius As Double, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Duration > 0 Then [Call]("TASK_COMBAT_HATED_TARGETS_AROUND_CHAR_TIMED", Ped, Radius, Duration) Else [Call]("TASK_COMBAT_HATED_TARGETS_AROUND_CHAR", Ped, Radius)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub CombatHatedTargetsInArea(Ped As Ped, Position As Vector3, Radius As Single, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_COMBAT_HATED_TARGETS_IN_AREA", Ped, Position.X, Position.Y, Position.Z, Radius)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub CombatRoll(Ped As Ped, Optional Unknown As Boolean = True)
            [Call]("TASK_COMBAT_ROLL", Ped, Unknown)
        End Sub
        Public Shared Sub Cower(Ped As Ped, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_COWER", Ped)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub Crouch(Ped As Ped, Crouch As Boolean, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped = Nothing Then
                [Call]("SET_CHAR_DUCKING", 0, Crouch)
            Else
                [Call]("SET_CHAR_DUCKING", Ped, Crouch)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End If
        End Sub
        Public Shared Sub CruiseSpeed(Ped As Ped, Speed As Single)
            [Call]("SET_DRIVE_TASK_CRUISE_SPEED", Ped, Speed)
        End Sub
        Public Shared Sub Dead(Ped As Ped, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_DEAD", Ped)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub Die(Ped As Ped)
            [Call]("TASK_DIE", Ped)
        End Sub
        Public Shared Sub DoDriveBy(Ped As Ped, Target As Ped, Optional UnknownP3 As Integer = 0, Optional UnknownP4 As Single = 0.0, Optional UnknownP5 As Single = 0.0, Optional UnknownP6 As Single = 0.0, Optional UnknownP7 As Double = 500.0, Optional UnknownP8 As Integer = 8, Optional UnknownP9 As Integer = 1, Optional UnknownP10 As Integer = 250, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_DRIVE_BY", Ped, Target, UnknownP3, UnknownP4, UnknownP5, UnknownP6, UnknownP7, UnknownP8, UnknownP9, UnknownP10)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub DriveAction(Ped As Ped, Car As Vehicle, Action As CarActions, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CAR_TEMP_ACTION", Ped, Car, Action, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub DriveAround(Ped As Ped, Car As Vehicle, Speed As Single, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CAR_DRIVE_WANDER", Ped, Car, Speed, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)

        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub DriveMission(Ped As Ped, Car As Vehicle, TargetCar As Vehicle, Speed As Single, Optional Flag As DrivingFlags = DrivingFlags.FollowTarget,
                                       Optional DrivingStyle As DrivingStyle = DrivingStyle.NormalDriving_StopsForTrafficLights, Optional AllowDrivingAgainstTraffic As Boolean = False,
                                       Optional DistanceFromTarget As Integer = 5, Optional Unknown As Integer = 10, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If AllowDrivingAgainstTraffic Then
                [Call]("TASK_CAR_MISSION", Ped, Car, TargetCar, Flag, Speed, DrivingStyle, DistanceFromTarget, Unknown)
            Else
                [Call]("TASK_CAR_MISSION_NOT_AGAINST_TRAFFIC", Ped, Car, TargetCar, Flag, Speed, DrivingStyle, DistanceFromTarget, Unknown)
            End If
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub DriveMission(Ped As Ped, Car As Vehicle, TargetPed As Ped, Speed As Single, Optional Flag As DrivingFlags = DrivingFlags.FollowTarget,
                                       Optional DrivingStyle As DrivingStyle = DrivingStyle.NormalDriving_StopsForTrafficLights, Optional AllowDrivingAgainstTraffic As Boolean = False,
                                       Optional DistanceFromTarget As Integer = 5, Optional Unknown As Integer = 10, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If AllowDrivingAgainstTraffic Then
                [Call]("TASK_CAR_MISSION_PED_TARGET", Ped, Car, TargetPed, Flag, Speed, DrivingStyle, DistanceFromTarget, Unknown)
            Else
                [Call]("TASK_CAR_MISSION_PED_TARGET_NOT_AGAINST_TRAFFIC", Ped, Car, TargetPed, Flag, Speed, DrivingStyle, DistanceFromTarget, Unknown)
            End If
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub DriveMission(Ped As Ped, Car As Vehicle, TargetCoords As Vector3, Speed As Single, Optional Flag As DrivingFlags = DrivingFlags.FollowTarget, Optional DrivingStyle As DrivingStyle = DrivingStyle.NormalDriving_StopsForTrafficLights, Optional AllowDrivingAgainstTraffic As Boolean = False, Optional DistanceFromTarget As Integer = 5, Optional Unknown As Integer = 10, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If AllowDrivingAgainstTraffic Then
                [Call]("TASK_CAR_MISSION_COORS_TARGET", Ped, Car, TargetCoords.X, TargetCoords.Y, TargetCoords.Z, Flag, Speed, DrivingStyle, DistanceFromTarget, Unknown)
            Else
                [Call]("TASK_CAR_MISSION_COORS_TARGET_NOT_AGAINST_TRAFFIC", Ped, Car, TargetCoords.X, TargetCoords.Y, TargetCoords.Z, Flag, Speed, DrivingStyle, DistanceFromTarget, Unknown)
            End If
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub DriveTo(Ped As Ped, Car As Vehicle, Position As Vector3, Speed As Single, Optional AllowDriveAgainstTraffic As Boolean = False, Optional Flag1 As Boolean = True, Optional flag2 As Boolean = False, Optional flag3 As Boolean = False, Optional Unknown1 As Single = 10.0, Optional Unknown2 As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If AllowDriveAgainstTraffic Then
                [Call]("TASK_CAR_DRIVE_TO_COORD", Ped, Car, Position.X, Position.Y, Position.Z, Speed, Flag1, flag2, flag3, Unknown1, Unknown2)
            Else
                [Call]("TASK_CAR_DRIVE_TO_COORD_NOT_AGAINST_TRAFFIC", Ped, Car, Position.X, Position.Y, Position.Z, Speed, Flag1, flag2, flag3, Unknown1, Unknown2)
            End If
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub DropObject(Ped As Ped, Optional Unknown As Boolean = True)
            [Call]("DROP_OBJECT", Ped, Unknown)
        End Sub
        Public Shared Sub DropWeapon(Ped As Ped)
            [Call]("FORCE_CHAR_TO_DROP_WEAPON", Ped)
        End Sub
        Public Shared Sub EnterVehicle(Ped As Ped, Car As Vehicle, Seat As VehicleSeat, Optional Duration As Integer = -1, Optional MovementType As MovementType = MovementType.None, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If MovementType <> MovementType.None Then [Call]("SET_NEXT_DESIRED_MOVE_STATE", MovementType)
            If Seat = VehicleSeat.Driver Then [Call]("TASK_ENTER_CAR_AS_DRIVER", Ped, Car, Duration) Else [Call]("TASK_ENTER_CAR_AS_PASSENGER", Ped, Car, Duration, Seat)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub FireWeapon(Ped As Ped, Target As Vector3)
            [Call]("FIRE_PED_WEAPON", Ped, Target.X, Target.Y, Target.Z)
        End Sub
        Public Shared Sub FleeFromPed(Ped As Ped, PedToFlee As Ped, SafeDistance As Single, Optional Duration As Integer = -1, Optional PrefersVehicle As Boolean = True, Optional MinTimeInVehicle As Integer = -1, Optional MaxTimeInVehicle As Integer = -1, Optional Speed As Single = 60, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If MinTimeInVehicle = -1 AndAlso PrefersVehicle = True Then MinTimeInVehicle = GetRandomInteger(500, 10000) Else MinTimeInVehicle = 0
            If MaxTimeInVehicle = -1 AndAlso PrefersVehicle = True Then MaxTimeInVehicle = GetRandomInteger(MinTimeInVehicle, MinTimeInVehicle + 50000) Else MaxTimeInVehicle = 0
            [Call]("TASK_FLEE_CHAR_ANY_MEANS", Ped, PedToFlee, SafeDistance, Duration, PrefersVehicle, MinTimeInVehicle, MaxTimeInVehicle, Speed)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub FleeFromPedSmart(Ped As Ped, PedToFlee As Ped, SafeDistance As Single, Optional PrefersPavements As Boolean = True, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If PrefersPavements Then [Call]("TASK_SMART_FLEE_CHAR_PREFERRING_PAVEMENTS", Ped, PedToFlee, SafeDistance, Duration) Else [Call]("TASK_SMART_FLEE_CHAR", Ped, PedToFlee, SafeDistance, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub FlyHelicopterTo(Ped As Ped, Heli As Vehicle, p3_Usually_0 As Integer, UnknownRole As Integer, Destination As Vector3, p8_0_to_19 As Integer, Speed As Single, p10_0_to_30 As Integer, Optional p11_Up_To_90 As Single = -1, Optional UnknownAltitude As Integer = -1, Optional p13 As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_HELI_MISSION", Ped, Heli, p3_Usually_0, UnknownRole, Destination.X, Destination.Y, Destination.Z, p8_0_to_19, Speed, p10_0_to_30, p11_Up_To_90, UnknownAltitude, p13)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        ''' <param name="MovementType">If is set to "None = -1", il will be apply the flag MovementType.Walk</param>
        Public Shared Sub FollowNavMesh(Ped As Ped, Coordinate As Vector3, Radius As Single, HeadingAngle As Single, Optional MovementType As MovementType = MovementType.Run, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True, Optional Unknown As Single = -2)
            [Call]("TASK_FOLLOW_NAV_MESH_AND_SLIDE_TO_COORD", Ped, Coordinate.X, Coordinate.Y, Coordinate.Z, MovementType, Radius, HeadingAngle)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Uses the native function "TASK_GO_STRAIGHT_TO_COORD", but costantly updating Movement Type and Coordinates. 
        ''' Must be called in a tick (100ms interval works fine. Useful to use to make a ped follows player 
        ''' in custom maps without ped/vehicle paths.
        ''' <param name="MovementType">If is set to "None = -1", il will be automatically apply the proper Movement Type.</param>
        ''' </summary>
        Public Shared Sub FollowPlayerStraight(Ped As Ped, Optional MovementType As MovementType = MovementType.None, Optional Unknown As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            Dim pos As Vector3 = NativePlayer.Character.Position
            If Ped.Position.DistanceTo(pos) > 1.5 Then
                Dim playermovetype = NativePlayer.GetMovementType
                If MovementType = MovementType.None Then
                    If Ped.Position.DistanceTo(pos) >= 15.0 Then
                        MovementType = MovementType.Sprint
                    ElseIf Ped.Position.DistanceTo(pos) < 15 AndAlso Ped.Position.DistanceTo(pos) > 5 Then
                        If playermovetype = NativePlayer.MovementType.Idle Or playermovetype = NativePlayer.MovementType.Walk Then
                            MovementType = MovementType.Run
                        Else
                            MovementType = playermovetype
                        End If
                    Else MovementType = playermovetype
                        If MovementType = 0 Then
                            MovementType = MovementType.Walk
                        End If
                    End If
                End If

                [Call]("TASK_GO_STRAIGHT_TO_COORD", Ped, pos.X, pos.Y, pos.Z, MovementType, Unknown)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            Else
                Exit Sub
            End If
        End Sub
        Public Shared Sub ForceFlyThroughWindScreen_TLaD(Ped As Ped)
            [Call]("SET_PED_FORCE_FLY_THROUGH_WINDSCREEN", Ped)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Sub ForceToLoadCover(Ped As Ped, Optional Force As Boolean = True)
            [Call]("FORCE_PED_TO_LOAD_COVER", Ped, Force)
        End Sub
        ''' <summary>
        ''' Untested. Used only in coop_drugfac SCO script.
        ''' </summary>
        Public Shared Sub GetOffBoat(Ped As Ped, Optional Unknown As Integer = 0)
            [Call]("TASK_GET_OFF_BOAT", Ped, Unknown)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        ''' <param name="MovementType">If is set to "None = -1", il will be apply the flag MovementType.Walk</param>
        Public Shared Sub GoToCoordAiming(Ped As Ped, CoordinatesToGo As Vector3, CoordinatesToAim As Vector3, Optional MovementType As MovementType = MovementType.None, Optional UnknownP8_Set2_to_aim_char As Integer = 1, Optional UnknowP12_char As Integer = 0, Optional UnknownP6 As Integer = 0, Optional UnknownP7 As Integer = 0, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_GO_TO_COORD_WHILE_AIMING", Ped, CoordinatesToGo.X, CoordinatesToGo.Y, CoordinatesToGo.Z, MovementType, UnknownP6, UnknownP7, UnknownP8_Set2_to_aim_char, CoordinatesToAim.X, CoordinatesToAim.Y, CoordinatesToAim.Z, UnknowP12_char)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub GoToCoordShooting(Ped As Ped, CoordinatesToGo As Vector3, Optional MovementType As MovementType = MovementType.None, Optional Unknown1 As Single = 1.0, Optional Unknown2 As Single = 2.0, Optional Unknown3 As Boolean = True, Optional Unknown4 As Integer = True, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped = Nothing Then
                [Call]("TASK_GO_TO_COORD_WHILE_SHOOTING", 0, CoordinatesToGo.X, CoordinatesToGo.Y, CoordinatesToGo.Z, MovementType, Unknown1, Unknown2, Unknown3, Unknown4)
            Else
                [Call]("TASK_GO_TO_COORD_WHILE_SHOOTING", Ped, CoordinatesToGo.X, CoordinatesToGo.Y, CoordinatesToGo.Z, MovementType, Unknown1, Unknown2, Unknown3, Unknown4)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End If
        End Sub
        ''' <summary>
        ''' Tested this function on a tick with 100ms interval to update the "Coordinates".
        ''' Useful to make a ped follows moving entities in custom maps without ped/vehicle paths.
        ''' </summary>
        ''' <param name="MovementType">If is set to "None = -1", il will be apply the flag MovementType.Walk</param>
        Public Shared Sub GoToCoordStraight(Ped As Ped, Coordinates As Vector3, Optional MovementType As MovementType = MovementType.None, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Ped = Nothing Then
                If MovementType = MovementType.None Then MovementType = MovementType.Walk
                [Call]("TASK_GO_STRAIGHT_TO_COORD", 0, Coordinates.X, Coordinates.Y, Coordinates.Z, MovementType, Duration)
            Else
                If MovementType = MovementType.None Then MovementType = MovementType.Walk
                [Call]("TASK_GO_STRAIGHT_TO_COORD", Ped, Coordinates.X, Coordinates.Y, Coordinates.Z, MovementType, Duration)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End If
        End Sub
        '''' Tested this function on a tick with 100ms interval to update the "Coordinates".
        '''' Useful to make a ped follows moving entities in custom maps without ped/vehicle paths.
        ''' <summary>
        ''' </summary>
        ''' <param name="MovementType">If is set to "None = -1", il will be apply the flag MovementType.Walk</param>
        Public Shared Sub GoToCoordStraight_RelativeToCar(Ped As Ped, Car As Vehicle, Position As Vector3, Optional MovementType As MovementType = MovementType.None, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If MovementType = MovementType.None Then MovementType = MovementType.Walk
            [Call]("TASK_GO_STRAIGHT_TO_COORD_RELATIVE_TO_CAR", Ped, Car, Position.X, Position.Y, Position.Z, MovementType, Duration)
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
        Public Shared Sub GoToPedAiming(Ped As Ped, TargetPed As Ped, Flag1 As Boolean, Flag2 As Boolean, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_GOTO_CHAR_AIMING", Ped, TargetPed, Flag1, Flag2)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub Guard(Ped As Ped, Position As Vector3, Heading As Single, Optional Duration As Integer = -1, Optional Unknown1 As Single = 2.0, Optional Unknown2 As Integer = 0, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_STAND_GUARD", Ped, Position.X, Position.Y, Position.Z, Heading, Unknown1, Unknown2, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
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
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Function isRespondingToAnyEvent(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_RESPONDING_TO_ANY_EVENT", Ped)
        End Function
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Function isRespondingToEvent(Ped As Ped, EventID As Integer) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_RESPONDING_TO_EVENT", Ped, EventID)
        End Function
        Public Shared Function isShooting(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_SHOOTING", Ped)
        End Function
        Public Shared Function isSittingIdle(Ped As Ped) As Boolean
            Return [Call](Of Boolean)("IS_CHAR_SITTING_IDLE", Ped)
        End Function
        Public Shared Sub Jump(Ped As Ped, Optional Unknown As Boolean = True)
            [Call]("TASK_JUMP", Ped, Unknown)
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
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub LeaveVehicleAndFlee(Ped As Ped, FleeCoordinates As Vector3, Optional Scream As Boolean = False)
            Dim Car As Vehicle
            If Ped.Exists Then
                If Ped.isSittingInVehicle Then
                    Car = Ped.CurrentVehicle
                    [Call]("TASK_LEAVE_CAR_AND_FLEE", Ped, Car, FleeCoordinates.X, FleeCoordinates.Y, FleeCoordinates.Z)
                    If Scream Then Speeches.SayScream(Ped, Speeches.Screams.PanicScream_NotPlayer)
                End If
            End If
        End Sub
        Public Shared Sub LeaveVehicle_Everyone(Car As Vehicle)
            If Car.Exists Then [Call]("TASK_EVERYONE_LEAVE_CAR", Car)
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
        ''' untested
        ''' </summary>
        Public Shared Sub OpenCarDoor(Ped As Ped, Car As Vehicle, Door As GTA.VehicleDoor, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            If Door = GTA.VehicleDoor.LeftFront Then [Call]("TASK_OPEN_DRIVER_DOOR", Ped, Car, Duration) Else [Call]("TASK_OPEN_PASSENGER_DOOR", Ped, Car, Duration, Door)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub PickupAndCarryObject(Ped As Ped, Obj As GTA.Object, PedToGive As Ped, Optional Unknown As Integer = 0, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_PICKUP_AND_CARRY_OBJECT", Ped, Obj, PedToGive, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub SeekDirectlyIntoCover(Ped As Ped, Position As Vector3, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_PUT_CHAR_DIRECTLY_INTO_COVER", Ped, Position.X, Position.Y, Position.Z, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub RemoveCharDefensiveArea(Ped As Ped)
            [Call]("REMOVE_CHAR_DEFENSIVE_AREA", Ped)
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub SeekCoverFromPed(Ped As Ped, PedToFlee As Ped, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_SEEK_COVER_FROM_PED", Ped, PedToFlee, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub SeekCoverFromPosition(Ped As Ped, Position As Vector3, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_SEEK_COVER_FROM_POS", Ped, Position.X, Position.Y, Position.Z, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub SeekCoverToCoords(Ped As Ped, CoverPosition As Vector3, Position As Vector3, Optional Duration As Integer = -1)
            [Call]("TASK_SEEK_COVER_TO_COVER_POINT", Ped, CoverPosition.X, CoverPosition.Y, CoverPosition.Z, Position.X, Position.Y, Position.Z, Duration)
        End Sub
        ''' <summary>
        ''' untested
        ''' </summary>
        Public Shared Sub SeekCoverToCoverPoint(Ped As Ped, CoverPointID As Integer, Position As Vector3, Optional Duration As Integer = -1)
            [Call]("TASK_SEEK_COVER_TO_COVER_POINT", Ped, CoverPointID, Position.X, Position.Y, Position.Z, Duration)
        End Sub
        ''' <summary>
        ''' Untested. Used only in packie1 SCO script.
        ''' </summary>
        Public Shared Sub Shimmy(Ped As Ped, Optional Unknown As Boolean = True, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_SHIMMY", Ped, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested. Not found in SCO scripts.
        ''' </summary>
        Public Shared Sub ShimmyClimbUp(Ped As Ped)
            [Call]("TASK_SHIMMY_CLIMB_UP", Ped)
        End Sub
        ''' <summary>
        ''' Untested. Used only in packie1 SCO script
        ''' </summary>
        Public Shared Sub ShimmyInDirection(Ped As Ped, Optional Unknown As Boolean = True, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_SHIMMY_IN_DIRECTION", Ped, Unknown)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        ''' <summary>
        ''' Untested. Used only in packie1 SCO script
        ''' </summary>
        Public Shared Sub ShimmyLetGo(Ped As Ped)
            [Call]("TASK_SHIMMY_LET_GO", Ped)
        End Sub
        Public Shared Sub ShootAt(Ped As Ped, TargetPed As Ped, Optional Mode As ShootMode = ShootMode.Burst, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_SHOOT_AT_CHAR", Ped, TargetPed, Duration, Mode)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub ShootAt(Ped As Ped, Coords As Vector3, Optional Mode As ShootMode = ShootMode.Burst, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_SHOOT_AT_CHAR", Ped, Coords.X, Coords.Y, Coords.Z, Duration, Mode)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub SetDefensiveArea(Ped As Ped, Position As Vector3, Radius As Single)
            [Call]("SET_CHAR_SPHERE_DEFENSIVE_AREA", Ped, Position.X, Position.Y, Position.Z, Radius)
        End Sub
        Public Shared Sub SlideToCoord(Ped As Ped, Coordinates As Vector3, Heading As Single, SpeedMultiplier As Single, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_CHAR_SLIDE_TO_COORD", Ped, Coordinates.X, Coordinates.Y, Coordinates.Z, Heading, SpeedMultiplier)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub

        '================================================================================================================================
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
        Public Structure SittingIdleAnims
            Public ReadOnly Property AnimSet As String
            Public ReadOnly Property AnimName As String
            Public Shared Sub Request(AnimSet As String)
                [Call]("REQUEST_ANIMS", AnimSet)
            End Sub
            Public Sub SetToPed(Ped As Ped, Optional Unknown As Boolean = True)
                [Call]("CHANGE_CHAR_SIT_IDLE_ANIM", Ped, AnimSet, AnimName, Unknown)
            End Sub
            Public Sub New(AnimSet As String, AnimName As String)
                Me.AnimSet = AnimSet
                Me.AnimName = AnimName
            End Sub

            '---------------------------------------------------------------
            Public Shared Comedy_ClapA As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "clap_a")
            Public Shared Comedy_ClapB As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "clap_b")
            Public Shared Comedy_ClapC As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "clap_c")
            Public Shared Comedy_CheerA As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "cheer_a")
            Public Shared Comedy_CheerB As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "cheer_b")
            Public Shared Comedy_Agree As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "agree_a")
            Public Shared Comedy_LaughA As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "laugh_a")
            Public Shared Comedy_LaughB As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "laugh_b")
            Public Shared Comedy_LaughC As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "laugh_c")
            Public Shared Comedy_LaughD As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "laugh_d")
            Public Shared Comedy_LaughE As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "laugh_e")
            Public Shared Comedy_LaughF As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "laugh_f")
            Public Shared Comedy_Jeer As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "jeer")
            Public Shared Comedy_JeerA As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "jeer_a")
            Public Shared Comedy_JeerB As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "jeer_b")
            Public Shared Comedy_JeerC As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "jeer_c")
            Public Shared Comedy_Idle01 As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "idle01")
            Public Shared Comedy_Bored As SittingIdleAnims = New SittingIdleAnims("AMB@COMEDY", "bored")

        End Structure
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
        '=========================================================================================
        '=========================================================================================
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
        '=========================================================================================
        '=========================================================================================
        '====================================================================================================================================
        Public Shared Sub Stand(Ped As Ped, Optional Duration As Integer = -1, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
            [Call]("TASK_STAND_STILL", Ped, Duration)
            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        End Sub
        Public Shared Sub SwapWeapon(Ped As Ped, Optional Unknown As Boolean = True)
            [Call]("TASK_SWAP_WEAPON", Ped, Unknown)
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        ''' <param name="Unknown">Values in SCO scripts are 0, 1 and -1</param>
        Public Shared Sub ToggleDuck(Ped As Ped, Unknown As Integer)
            [Call]("TASK_TOGGLE_DUCK", Ped, Unknown)
        End Sub
        Public Shared Sub TurnTo(Ped As Ped, Heading As Single)
            [Call]("TASK_ACHIEVE_HEADING", Ped, Heading)
        End Sub
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
            If Ped = Nothing Then
                [Call]("TASK_WANDER_STANDARD", 0)
            Else
                [Call]("TASK_WANDER_STANDARD", Ped)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End If
        End Sub
        Public Class Route
            Public Shared Sub Clear()
                [Call]("TASK_FLUSH_ROUTE")
            End Sub
            Public Shared Sub Drive(Ped As Ped, Car As Vehicle, Optional Speed As Single = 35.0, Optional Unknown As Boolean = True, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
                [Call]("TASK_DRIVE_POINT_ROUTE", Ped, Car, Speed)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End Sub
            Public Shared Sub Extend(RoutePoint As Vector3)
                [Call]("TASK_EXTEND_ROUTE", RoutePoint.X, RoutePoint.Y, RoutePoint.Z)
            End Sub
            Public Shared Sub Follow(Ped As Ped, Optional MovementType As MovementType = MovementType.None, Optional Unknown As Boolean = True, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
                If MovementType = MovementType.None Then MovementType = MovementType.Walk
                [Call]("TASK_FOLLOW_POINT_ROUTE", Ped, MovementType, Unknown)
                AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
            End Sub
        End Class
        'Public Class Sequence
        '    Private _handleInternal As Integer
        '    Private _handlePointer As Native.Pointer
        '    Private _isOpened As Boolean = False

        '    Public ReadOnly Property Handle As Integer
        '        Get
        '            Return _handleInternal
        '        End Get
        '    End Property
        '    Public Shared ReadOnly List As List(Of Sequence) = New List(Of Sequence)
        '    Public ReadOnly Property CurrentProgress As Integer
        '        Get
        '            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        '            [Call]("GET_SEQUENCE_PROGRESS", _handleInternal, pointer)
        '            Return pointer.Value
        '        End Get
        '    End Property
        '    Public Sub New()
        '        _handlePointer = New Native.Pointer(GetType(Integer))
        '        [Call]("OPEN_SEQUENCE_TASK", _handlePointer)
        '        _handleInternal = _handlePointer.Value
        '        [Call]("CLOSE_SEQUENCE_TASK", _handleInternal)
        '    End Sub
        '    'Public Sub AddTask(Method As [Delegate], ParamArray Args() As Object)
        '    '    [Call]("OPEN_SEQUENCE_TASK", _handlePointer)

        '    '    Dim fullArgs As New List(Of Object)({0})
        '    '    fullArgs.AddRange(Args)

        '    '    Method.DynamicInvoke(fullArgs.ToArray())

        '    '    [Call]("CLOSE_SEQUENCE_TASK", _handleInternal)
        '    'End Sub
        '    Public Sub AddNextTasks()
        '        CloseAnySequenceOpened()
        '        [Call]("OPEN_SEQUENCE_TASK", _handlePointer)
        '        _isOpened = True
        '    End Sub
        '    Public Sub Clear()
        '        [Call]("CLEAR_SEQUENCE_TASK", _handleInternal)
        '    End Sub
        '    Public Shared Sub ClearAll()
        '        For Each seq In List.ToList()
        '            seq.Clear()
        '            List.Remove(seq)
        '        Next
        '    End Sub
        '    Private Sub CloseAnySequenceOpened()
        '        For Each seq In List.ToList
        '            If seq IsNot Me AndAlso seq._isOpened Then
        '                [Call]("CLOSE_SEQUENCE_TASK", seq._handleInternal)
        '            End If
        '        Next
        '    End Sub
        '    Public Sub Start(Ped As Ped, Optional Repeat As Boolean = False, Optional Clear As Boolean = True, Optional KeepTask As Boolean = True, Optional BlockPermanentEvents As Boolean = True)
        '        If Ped.Exists = False Then
        '            NativeDraws.DisplayText(NativeDraws.TextColors.MP_Red & "Ped doesn't exists!")
        '        Else
        '            If Repeat Then [Call]("SET_SEQUENCE_TO_REPEAT", _handleInternal, 1)
        '            [Call]("CLOSE_SEQUENCE_TASK", _handleInternal)
        '            [Call]("TASK_PERFORM_SEQUENCE", Ped, _handleInternal)
        '            AvoidTasksOverride(Ped, KeepTask, BlockPermanentEvents)
        '            If Clear Then [Call]("CLEAR_SEQUENCE_TASK", _handleInternal)
        '        End If
        '    End Sub
        'End Class
        'TASK_PERFORM_SEQUENCE
        'OPEN_SEQUENCE_TASK
        'CLOSE_SEQUENCE_TASK
        'CLEAR_SEQUENCE_TASK

        'GET_SEQUENCE_PROGRESS

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


'IS_CHAR_TOUCHING_OBJECT
'IS_CHAR_TOUCHING_OBJECT_ON_FOOT
'HAS_CHAR_SPOTTED_CHAR_IN_FRONT( l_U521[1], sub_2676()
'IS_PED_A_MISSION_PED
'IS_PED_ATTACHED_TO_ANY_CAR
'IS_PED_ATTACHED_TO_OBJECT
'IS_PED_BEING_JACKED
'IS_PED_CLIMBING
'IS_PED_DOING_DRIVEBY
'IS_PED_FLEEING
'IS_PED_HOLDING_AN_OBJECT
'IS_PED_IN_COMBAT
'IS_PED_IN_COVER
'IS_PED_IN_GROUP
'IS_PED_JACKING
'IS_PED_LOOKING_AT_CAR
'IS_PED_LOOKING_AT_OBJECT
'IS_PED_LOOKING_AT_PED
'IS_PED_PINNED_DOWN
'IS_PED_RAGDOLL
'IS_PED_RETREATING
'IS_PEDS_VEHICLE_HOT