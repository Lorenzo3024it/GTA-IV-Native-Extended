Imports GTA
Imports GTA.Native.Function

Public Class NativeObjects
    Public Shared Sub AttachObjectToCar(Obj As GTA.Object, Car As Vehicle, Position As Vector3, Rotation As Vector3, Optional Unknown As Integer = 0)
        [Call]("ATTACH_OBJECT_TO_CAR", Obj, Car, Unknown, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z)
    End Sub
    'Public Shared Sub AttachObjectToCarPhisically()
    '    [Call]("ATTACH_OBJECT_TO_CAR_PHYSICALLY")
    'End Sub
    Public Shared Sub AttachObjectToPed(Obj As GTA.Object, Ped As Ped, BoneID As Bone, Position As Vector3, Rotation As Vector3, Optional Unknown As Integer = 0)
        [Call]("ATTACH_OBJECT_TO_PED", Obj, Ped, BoneID, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, Unknown)
    End Sub
    'Public Shared Sub AttachObjectToPedPhisically()

    'End Sub
    'Public Shared Sub AttachObjectToObject()

    'End Sub
    'Public Shared Sub AttachObjectToObjectPhisically()

    'End Sub
    Public Shared Sub Detach(Obj As GTA.Object)
        [Call]("DETACH_OBJECT", Obj, 1)
    End Sub
    Public Shared Sub Freeze(Obj As GTA.Object, Optional Freeze As Boolean = True, Optional DontLoadCollision As Boolean = False)
        If Freeze Then
            If DontLoadCollision Then
                [Call]("FREEZE_CHAR_POSITION_AND_DONT_LOAD_COLLISION", Obj, True)
            Else
                [Call]("FREEZE_CHAR_POSITION", Obj, True)
            End If
        Else
            [Call]("FREEZE_CHAR_POSITION", Obj, False)
            [Call]("FREEZE_CHAR_POSITION_AND_DONT_LOAD_COLLISION", Obj, False)
        End If
    End Sub
    Public Shared Function getClosestStealableObject(Position As Vector3, Radius As Single)
        Dim ObjPointer As Native.Pointer = New Native.Pointer(GetType(GTA.Object))
        Dim Obj As GTA.Object
        [Call]("GET_CLOSEST_STEALABLE_OBJECT", Position.X, Position.Y, Position.Z, Radius, ObjPointer)
        Obj = ObjPointer.Value
        Return Obj
    End Function
    Public Shared Function getHeightAboveGround(Obj As Object) As Single
        Try
            If Obj Is Nothing OrElse Not Obj.Exists Then Return 0.0F

            Dim pos As GTA.Vector3 = Obj.Position
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
    Public Shared Function hasBeenDamaged(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("HAS_OBJECT_BEEN_DAMAGED", Obj)
    End Function
    Public Shared Function hasCollidedWithAnything(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("HAS_OBJECT_COLLIDED_WITH_ANYTHING", Obj)
    End Function
    Public Shared Function hasPhysics(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("DOES_OBJECT_HAVE_PHYSICS", Obj)
    End Function
    Public Shared Function isObjectAttached(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("IS_OBJECT_ATTACHED")
    End Function
    Public Shared Function isInWater(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("IS_OBJECT_IN_WATER", Obj)
    End Function
    Public Shared Sub AddToInteriorRoom(Obj As GTA.Object, InteriorID As Integer)
        [Call]("ADD_OBJECT_TO_INTERIOR_ROOM_BY_KEY", Obj, InteriorID)
    End Sub
    Public Shared Sub AddToInteriorRoom(Obj As GTA.Object, InteriorName As String)
        [Call]("ADD_OBJECT_TO_INTERIOR_ROOM_BY_NAME", Obj, InteriorName)
    End Sub
    Public Shared Sub AllocateScript(Obj As GTA.Object, ScriptName As String, Radius As Single, Optional Priority As Integer = 100, Optional Type As Integer = -1)
        [Call]("ALLOCATE_SCRIPT", ScriptName, Obj.GetHashCode, Priority, Radius, Type)
    End Sub
    Public Shared Function isTouchingObject(Obj1 As GTA.Object, Obj2 As GTA.Object) As Boolean
        Return [Call](Of Boolean)("IS_OBJECT_TOUCHING_OBJECT", Obj1, Obj2)
    End Function
    Public Shared Function isOnScreen(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("IS_OBJECT_ON_SCREEN", Obj)
    End Function
    Public Shared Sub SetAlpha(Obj As GTA.Object, Alpha_0_to_255 As Integer)
        [Call]("SET_OBJECT_ALPHA", Obj, Alpha_0_to_255)
    End Sub
    Public Shared Sub SetAsStealable(Obj As GTA.Object, Stealable As Boolean)
        [Call]("SET_OBJECT_AS_STEALABLE", Obj, Stealable)
    End Sub
    Public Shared Sub SetCollision(Obj As GTA.Object, Collisions As Boolean)
        [Call]("SET_OBJECT_COLLISION", Obj, Collisions)
    End Sub
    Public Shared Sub SetLights(Obj As GTA.Object, LightsON As Boolean)
        [Call]("SET_OBJECT_LIGHTS", Obj, LightsON)
    End Sub
    Public Shared Sub SetRecordsCollisions(Obj As GTA.Object, RecordsCollisions As Boolean)
        [Call]("SET_OBJECT_RECORDS_COLLISIONS", Obj, RecordsCollisions)
    End Sub
    'Public Shared Function SpawnObject(Model As String, Position As Vector3, Collisions As Boolean, Optional RecordsCollisions As Boolean = False, Optional MsgText As String = "") As GTA.Object
    '    [Call]("REQUEST_MODEL", NativeModels.getHashKey(Model))
    '    'Do
    '    '    Game.WaitInCurrentScript(1)
    '    '    If MsgText <> "" Then
    '    '        NativeDraws.DisplayText(MsgText, 1000)
    '    '    End If
    '    'Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", ModelID)
    '    Dim SpawnedObject = World.CreateObject(NativeModels.getHashKey(Model), Position)
    '    ' [Call]("SET_OBJECT_COLLISION", SpawnedObject, Collisions)
    '    '  If RecordsCollisions Then [Call]("SET_OBJECT_RECORDS_COLLISIONS", SpawnedObject, True)
    '    Return SpawnedObject
    'End Function
End Class