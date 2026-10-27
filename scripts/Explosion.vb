Imports GTA
Imports GTA.Native.Function

Public NotInheritable Class NativeExplosion
    Private Sub New() : End Sub
    Public Enum ExplosionType
        Any = -1
        Grenade = 0
        Molotov = 1
        Rocket = 2
        HiOctane = 3
        Veh_Car = 4
        Veh_Plane = 5
        PetrolPump = 6
        Veh_Bike = 7
        Steam = 8
        Flame = 9
        WaterHydrant = 10
        GasCanister = 11
        Veh_Boat = 12
        ShipDestroy = 13
        Veh_Truck = 14
    End Enum
    Public Shared Sub Create(Position As GTA.Vector3, Damage As Single, Optional Type As ExplosionType = ExplosionType.Grenade,
                                  Optional Audio As Boolean = True, Optional Visible As Boolean = True, Optional CameraShake As Single = 1.0)
        If Type = ExplosionType.Any Then Type = ExplosionType.Grenade
        [Call]("ADD_EXPLOSION", Position.X, Position.Y, Position.Z, CInt(Type), CSng(Damage), Audio, Not Visible, CSng(CameraShake))
    End Sub
    Public Shared Function IsExplosionInArea(MinCoords As Vector3, MaxCoords As Vector3, Optional Type As ExplosionType = ExplosionType.Any) As Boolean
        Return [Call](Of Boolean)("IS_EXPLOSION_IN_AREA", CInt(Type), MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z)
    End Function
    Public Shared Function IsExplosionInSphere(MinCoords As Vector3, Radius As Single, Optional Type As ExplosionType = ExplosionType.Any) As Boolean
        Return [Call](Of Boolean)("IS_EXPLOSION_IN_SPHERE", CInt(Type), MinCoords.X, MinCoords.Y, MinCoords.Z, CSng(Radius))
    End Function
End Class
