Imports GTA
Imports GTA.base
Imports GTA.Native.Function

Public Class NativeVehicles
    Public Enum EngineState
        EngineON = 0
        EngineOFF = 1
        OutOfFuel = 2
    End Enum
    Shared Sub AttachCarToCar(ByVal Car1 As Vehicle, ByVal Car2 As Vehicle, ByVal AbOffs As Boolean, ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single, ByVal RotX As Single, ByVal RotY As Single, ByVal RotZ As Single)
        [Call]("ATTACH_CAR_TO_CAR", Car1, Car2, AbOffs, PosX, PosY, PosZ, RotX, RotY, RotZ)
    End Sub
    Shared Sub AttachCarToCarPhisically(Car1 As Vehicle, Car2 As Vehicle, UnknownBool As Boolean, UnknownInt As Integer, Offsets As Vector3, Buffer As Vector3, Car1RotX As Single, Car1RotY As Single)
        [Call]("ATTACH_CAR_TO_CAR_PHYSICALLY", Car1, Car2, UnknownBool, UnknownInt, Offsets.X, Offsets.Y, Offsets.Z, Buffer.X, Buffer.Y, Buffer.Z, Car1RotX, Car1RotY)
    End Sub
    Shared Sub BreakDoor(Car As Vehicle, Door As VehicleDoor, InstantVanish As Boolean)
        [Call]("BREAK_CAR_DOOR", Car, Door, InstantVanish)
    End Sub
    Shared Sub ControlDoor(ByVal Car As Vehicle, ByVal CarDoor As GTA.VehicleDoor, ByVal UnknownBool As Boolean, ByVal DoorAngle As Double)
        [Call]("CONTROL_CAR_DOOR", Car, CarDoor, UnknownBool, DoorAngle)
    End Sub
    Shared Sub ControlAmbientVehicles(Planes As Boolean, PoliceHelis As Boolean, Trains As Boolean, Boats As Boolean, GarbageTrucks As Boolean, MadDrivers As Boolean)
        [Call]("SWITCH_AMBIENT_PLANES", Planes)
        [Call]("SWITCH_POLICE_HELIS", PoliceHelis)
        [Call]("SWITCH_RANDOM_BOATS", Boats)
        [Call]("SWITCH_RANDOM_TRAINS", Trains)
        [Call]("SWITCH_GARBAGE_TRUCKS", GarbageTrucks)
        [Call]("SWITCH_MAD_DRIVERS", MadDrivers)
        If Trains = False Then [Call]("DELETE_ALL_TRAINS") 'forse serve un tick?
    End Sub
    Shared Sub ControlTrafficAndPedsMultiplier(CarDensity As Single, PedDensity As Single, SwitchRoadsOFF As Boolean, Planes As Boolean, PoliceHelis As Boolean, Trains As Boolean, Boats As Boolean, GarbageTrucks As Boolean, MadDrivers As Boolean)
        [Call]("SET_CAR_DENSITY_MULTIPLIER", CarDensity)
        [Call]("SET_PARKED_CAR_DENSITY_MULTIPLIER", CarDensity)
        [Call]("SET_RANDOM_CAR_DENSITY_MULTIPLIER", CarDensity)
        [Call]("SET_PED_DENSITY_MULTIPLIER", PedDensity)
        ControlAmbientVehicles(Planes, PoliceHelis, Trains, Boats, GarbageTrucks, MadDrivers)
        If SwitchRoadsOFF Then NativeGeneric.SwitchRoadsOFF(New Vector3(0, 0, 0), New Vector3(5000, 5000, 200)) Else NativeGeneric.SwitchRoadsON(New Vector3(0, 0, 0), New Vector3(5000, 5000, 200))
        'If DisableRadio = True Then EnableFrontedRadio(False)
    End Sub
    Shared Function getNumberOfPassenger(Car As Vehicle) As Integer
        Dim Num As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call]("GET_NUMBER_OF_PASSENGERS", Car, Num)
        Return Num.Value
    End Function
    Shared Function isDoorDamaged(Car As Vehicle, Door As VehicleDoor) As Boolean
        Return [Call](Of Boolean)("IS_CAR_DOOR_DAMAGED", Car, Door)
    End Function
    Shared Function isDoorFullyOpen(Car As Vehicle, Door As VehicleDoor) As Boolean
        Return [Call](Of Boolean)("IS_CAR_DOOR_FULLY_OPEN", Car, Door)
    End Function

    'IS_CAR_DEAD

    Shared Sub DetachCar(Car As Vehicle)
        [Call]("DETACH_CAR", Car)

    End Sub
    Shared Function isEngineRunning(Car As Vehicle) As Boolean
        ' Dim HealthPointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        ' [Call]("GET_PETROL_TANK_HEALTH", Car, HealthPointer)
        Dim Fuel As Integer = Car.PetrolTankHealth
        If Fuel > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Shared Function isOnFire(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_ON_FIRE")
    End Function
    Shared Function getEngineState(Car As Vehicle) As EngineState
        'Dim HealthPointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        '[Call]("GET_PETROL_TANK_HEALTH", Car, HealthPointer)
        Dim Fuel As Integer = Car.PetrolTankHealth
        If Fuel = -1 Then
            Return EngineState.OutOfFuel
        ElseIf Fuel > 0 Then
            Return EngineState.EngineON
        ElseIf Fuel = -1000 Then
            Return EngineState.EngineOFF
        End If
    End Function
    Shared Sub SetEngineON(Car As Vehicle, Optional FuelAmount As Integer = 1000)
        '[Call]("SET_PETROL_TANK_HEALTH", Car, FuelAmount)
        Car.PetrolTankHealth = FuelAmount
    End Sub
    Shared Sub SetEngineOFF(Car As Vehicle, Optional OutOfFuel As Boolean = False)
        If OutOfFuel Then
            ' [Call]("SET_PETROL_TANK_HEALTH", Car, -1)
            Car.PetrolTankHealth = -1
            [Call]("SET_CAR_ENGINE_ON", Car, 0, 0)
        Else
            '[Call]("SET_PETROL_TANK_HEALTH", Car, -1000)
            Car.PetrolTankHealth = -1000
            [Call]("SET_CAR_ENGINE_ON", Car, 0, 0)
        End If
    End Sub
    Shared Function isStopped(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_STOPPED", Car)
    End Function
    Shared Function isWindowIntact(Car As Vehicle, Window As GTA.VehicleWindow) As Boolean
        Return [Call](Of Boolean)("IS_VEH_WINDOW_INTACT", Car, Window)
    End Function
    Shared Function hasBeenDamagedByCar(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("HAS_CAR_BEEN_DAMAGED_BY_CAR", Car, World.GetClosestVehicle(Car.Position, 6.0F))
    End Function
    Shared Sub RemoveWindows(Car As Vehicle, Window As GTA.VehicleWindow, Smash As Boolean)
        If Smash Then [Call]("SMASH_CAR_WINDOW", Car, Window) Else [Call]("REMOVE_CAR_WINDOW", Car, Window)
    End Sub
    Shared Sub ResetWheelsPosition(Car As Vehicle)
        [Call]("RESET_CAR_WHEELS", Car)
    End Sub
    Public Shared Function SecondaryLights(Car As Vehicle, TurnON As Boolean) As Boolean
        [Call]("SET_TAXI_LIGHTS", Car, TurnON)
        Return [Call](Of Boolean)("ARE_TAXI_LIGHTS_ON", Car)
    End Function 'Taxi Lights
    Shared Sub SetAlarm(Car As Vehicle, Optional Duration As Integer = -1)
        [Call]("SET_VEH_ALARM", Car, True)
        If Duration > 0 Then [Call]("SET_VEH_ALARM_DURATION", Car, Duration)
    End Sub
    Shared Sub TriggerAlarm(Car As Vehicle, Optional Duration As Integer = -1)
        [Call]("SET_VEH_ALARM", Car, True)
        If Duration > 0 Then [Call]("SET_VEH_ALARM_DURATION", Car, Duration)
        [Call]("TRIGGER_VEH_ALARM", Car, True)
    End Sub
    Public Shared Sub SetAlpha(Car As Vehicle, Alpha_0_to_255 As Integer)
        [Call]("SET_VEHICLE_ALPHA", Car, Alpha_0_to_255)
    End Sub
    Public Shared Sub SetCollision(Car As Vehicle, Collision As Boolean)
        [Call]("SET_CAR_COLLISION", Car, Collision)
    End Sub
    Public Shared Sub SetHazardLights(Car As Vehicle, HazardLights As Boolean, Optional Blinking As Boolean = True)
        If HazardLights Then
            If Blinking Then
                [Call]("SET_VEH_INDICATORLIGHTS", Car, False)
                [Call]("SET_VEH_HAZARDLIGHTS", Car, True)
            Else
                [Call]("SET_VEH_HAZARDLIGHTS", Car, False)
                [Call]("SET_VEH_INDICATORLIGHTS", Car, True)
            End If
        Else
            [Call]("SET_VEH_HAZARDLIGHTS", Car, False)
            [Call]("SET_VEH_INDICATORLIGHTS", Car, False)
        End If
    End Sub
    Public Shared Sub SetHeliBladesFullSpeed(Heli As Vehicle)
        [Call]("SET_HELI_BLADES_FULL_SPEED", Heli)
    End Sub
    Shared Sub SetHeliStabilizer(Heli As Vehicle)
        [Call]("SET_HELI_STABILISER", Heli)
    End Sub
    Public Shared Sub SetLights(Car As Vehicle, AutomaticLights As Boolean, LightsON As Boolean)
        If AutomaticLights Then
            [Call]("FORCE_CAR_LIGHTS", Car, 0)
        Else
            If LightsON Then [Call]("FORCE_CAR_LIGHTS", Car, 2) Else [Call]("FORCE_CAR_LIGHTS", Car, 1)
        End If
    End Sub
    Public Shared Sub SetHeadlightsMultiplier(Car As Vehicle, LightMultiplier As Single)
        [Call]("SET_CAR_LIGHT_MULTIPLIER", Car, LightMultiplier)
    End Sub
    Public Shared Sub SetGPSEnabled(Car As Vehicle, Enable As Boolean)
        [Call]("ENABLE_GPS_IN_VEHICLE", Car, Enable)
    End Sub
    Public Shared Sub SetGPSRoute(ByVal Blip As Blip, ByVal Enable As Boolean)
        Dim state As Integer
        If Enable = True Then state = 1 Else state = 0
        [Call]("SET_ROUTE", Blip, state)
    End Sub
    Public Shared Sub SetExtra(Car As Vehicle, ExtraID As Integer, Enabled As Boolean)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, ExtraID, Not Enabled)
    End Sub
    Public Shared Sub SetExtraWithDoorAngle(Car As Vehicle, ExtraID As Integer, Enabled As Boolean)
        'Dim DoorAngle0 As Double = Car.Door(VehicleDoor.LeftFront)
        'Dim DoorAngle1 As Double = Car.Door(VehicleDoor.RightFront)
        'Dim DoorAngle2 As Double = Car.Door(VehicleDoor.LeftRear)
        'Dim DoorAngle3 As Double = Car.Door(VehicleDoor.RightRear)
        'Dim DoorAngle4 As Double = Car.Door(VehicleDoor.Hood)
        'Dim DoorAngle5 As Double = Car.Door(VehicleDoor.Trunk)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, ExtraID, Not Enabled)
        'Car.Door(VehicleDoor.LeftFront).Angle = DoorAngle0
        'Car.Door(VehicleDoor.RightFront).Angle = DoorAngle1
        'Car.Door(VehicleDoor.LeftRear).Angle = DoorAngle2
        'Car.Door(VehicleDoor.RightRear).Angle = DoorAngle3
        'Car.Door(VehicleDoor.Hood).Angle = DoorAngle4
        'Car.Door(VehicleDoor.Trunk).Angle = DoorAngle5
        'ControlCarDoor(Car, VehicleDoor.LeftFront, False, DoorAngle0)
        'ControlCarDoor(Car, VehicleDoor.RightFront, False, DoorAngle1)
        'ControlCarDoor(Car, VehicleDoor.LeftRear, False, DoorAngle2)
        'ControlCarDoor(Car, VehicleDoor.RightRear, False, DoorAngle3)
        'ControlCarDoor(Car, VehicleDoor.Hood, False, DoorAngle4)
        'ControlCarDoor(Car, VehicleDoor.Trunk, False, DoorAngle5)
    End Sub
    'Shared Sub StartFire(Car As Vehicle)
    '    [Call]("START_CAR_FIRE", Car)
    'End Sub
    Shared Function SpawnVehicle(ModelID As Integer, Position As Vector3, Optional MsgText As String = "", Optional LeftUpMsgType As Boolean = False) As GTA.Vehicle
        [Call]("REQUEST_MODEL", ModelID)
        Do
            Game.WaitInCurrentScript(1)
            If MsgText <> "" Then
                If LeftUpMsgType Then
                    Game.DisplayText(MsgText, 1500)
                Else
                    NativeGeneric.Msg(MsgText, 1000)
                End If
            End If
        Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", ModelID)
        Dim SpawnedCar = World.CreateVehicle(ModelID, Position)
        Return SpawnedCar
    End Function
End Class