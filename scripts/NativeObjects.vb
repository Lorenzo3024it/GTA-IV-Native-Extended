Imports GTA
Imports GTA.Native.Function

Public Class NativeObjects
    Shared Function getClosestStealableObject(Position As Vector3, Radius As Single)
        Dim ObjPointer As Native.Pointer = New Native.Pointer(GetType(GTA.Object))
        Dim Obj As GTA.Object
        [Call]("GET_CLOSEST_STEALABLE_OBJECT", Position.X, Position.Y, Position.Z, Radius, ObjPointer)
        Obj = ObjPointer.Value
        Return Obj
    End Function
    Shared Function hasBeenDamaged(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("HAS_OBJECT_BEEN_DAMAGED", Obj)
    End Function
    Shared Function hasCollidedWithAnything(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("HAS_OBJECT_COLLIDED_WITH_ANYTHING", Obj)
    End Function
    Shared Function isObjectAttached(Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("IS_OBJECT_ATTACHED")
    End Function
    'IS_OBJECT_IN_WATER
    'IS_OBJECT_ON_FIRE
    'IS_OBJECT_ON_SCREEN
    'IS_OBJECT_TOUCHING_OBJECT

    Shared Sub SetAlpha(Obj As GTA.Object, Alpha_0_to_255 As Integer)
        [Call]("SET_OBJECT_ALPHA", Obj, Alpha_0_to_255)
    End Sub
    Shared Sub SetAsStealable(Obj As GTA.Object, Stealable As Boolean)
        [Call]("SET_OBJECT_AS_STEALABLE", Obj, Stealable)
    End Sub
    Shared Sub SetCollision(Obj As GTA.Object, Collision As Boolean)
        [Call]("SET_OBJECT_COLLISION", Obj, Collision)
    End Sub
    Shared Sub SetLights(Obj As GTA.Object, LightsON As Boolean)
        [Call]("SET_OBJECT_LIGHTS", Obj, LightsON)
    End Sub
    Shared Sub SetRecordsCollisions(Obj As GTA.Object, RecordsCollisions As Boolean)
        [Call]("SET_OBJECT_RECORDS_COLLISIONS", Obj, RecordsCollisions)
    End Sub
    Shared Function SpawnObject(ModelID As Integer, Position As Vector3, Optional MsgText As String = "", Optional RecordsCollisions As Boolean = False) As GTA.Object
        [Call]("REQUEST_MODEL", ModelID)
        Do
            Game.WaitInCurrentScript(1)
            If MsgText <> "" Then
                NativeGeneric.Msg(MsgText, 1000)
            End If
        Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", ModelID)
        Dim SpawnedObject = World.CreateObject(ModelID, Position)
        If RecordsCollisions Then [Call]("SET_OBJECT_RECORDS_COLLISIONS", SpawnedObject, True)
        Return SpawnedObject
    End Function
End Class