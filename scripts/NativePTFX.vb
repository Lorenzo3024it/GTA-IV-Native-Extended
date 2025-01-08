Imports GTA
Imports GTA.Native.Function

Public Class NativePTFX
    Shared Function StartPTFXOnVehicle(PTFX As String, Car As Vehicle, Position As Vector3, Rotation As Vector3, FxScale As Double) As Integer
        Return [Call](Of Integer)("START_PTFX_ON_VEH", PTFX, Car, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, FxScale)
    End Function
    Shared Function StartPTFXOnObject(PTFX As String, Obj As GTA.Object, Position As Vector3, Rotation As Vector3, FxScale As Double) As Integer
        Return [Call](Of Integer)("START_PTFX_ON_OBJ", PTFX, Obj, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, FxScale)
    End Function
    Shared Function StartPTFXOnPed(PTFX As String, Ped As GTA.Ped, Position As Vector3, Rotation As Vector3, FxScale As Double) As Integer
        Return [Call](Of Integer)("START_PTFX_ON_PED", PTFX, Ped, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, FxScale)
    End Function
    Shared Function StartPTFXOnPedBone(PTFX As String, Ped As GTA.Ped, Position As Vector3, Rotation As Vector3, BoneID As Bone, FxScale As Double) As Integer
        Return [Call](Of Integer)("START_PTFX_ON_PED", PTFX, Ped, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, BoneID, FxScale)
    End Function
    Shared Sub EvolvePTFX(PTFX As Integer, Optional Effect As String = "fade", Optional Value As Single = 0)
        [Call]("EVOLVE_PTFX", PTFX, Effect, Value)
    End Sub
    Shared Sub StopPTFX(PTFX As Integer)
        [Call]("STOP_PTFX", PTFX)
    End Sub
    Shared Sub UpdatePTFXOffsets(PTFX As Integer, Position As Vector3, Rotation As Vector3)
        [Call]("UPDATE_PTFX_OFFSETS", PTFX, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z)
    End Sub
End Class