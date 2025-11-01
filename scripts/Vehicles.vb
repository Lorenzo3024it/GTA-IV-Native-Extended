Imports System.Runtime.CompilerServices
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices.WindowsRuntime
Imports System.Security.AccessControl
Imports AdvancedHookManaged
Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Imports GTA.value
Imports NativeExtended.NativeVehicles

Public Class NativeVehicles
    'Public Shared isEngineManuallySwitchedOFF_Forcing As Boolean = False
    Private Shared _tempRequestedModel As String
    Private Shared _tempSpawnedVehPos As Vector3
    Private Shared _tempSpawnedVeh As Vehicle
    Public Enum EngineState
        OutOfFuel = -1
        EngineON = 0
        EngineOFF = 1
    End Enum
    Public Enum LightsState
        Automatic = 0
        ForcedOFF = 1
        ForcedON = 2
    End Enum
    Public Enum HazardLightsState
        OFF = 0
        Blinking = 1
        StaticON = 2
    End Enum
    Public Shared Sub AttachCarToCar(ByVal Car1 As Vehicle, ByVal Car2 As Vehicle, ByVal AbOffs As Boolean, ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single, ByVal RotX As Single, ByVal RotY As Single, ByVal RotZ As Single)
        [Call]("ATTACH_CAR_TO_CAR", Car1, Car2, AbOffs, PosX, PosY, PosZ, RotX, RotY, RotZ)
    End Sub
    Public Shared Sub AttachCarToCarPhisically(Car1 As Vehicle, Car2 As Vehicle, UnknownBool As Boolean, UnknownInt As Integer, Offsets As Vector3, Buffer As Vector3, Car1RotX As Single, Car1RotY As Single)
        [Call]("ATTACH_CAR_TO_CAR_PHYSICALLY", Car1, Car2, UnknownBool, UnknownInt, Offsets.X, Offsets.Y, Offsets.Z, Buffer.X, Buffer.Y, Buffer.Z, Car1RotX, Car1RotY)
    End Sub
    Public Shared Sub BreakDoor(Car As Vehicle, Door As GTA.VehicleDoor, InstantVanish As Boolean)
        [Call]("BREAK_CAR_DOOR", Car, Door, InstantVanish)
    End Sub
    Public Shared Sub CanBeResprayed(Car As Vehicle, CanBeResprayed As Boolean)
        [Call]("SET_CAN_RESPRAY_CAR", CanBeResprayed)
    End Sub
    Public Shared Sub ControlDoor(ByVal Car As Vehicle, ByVal CarDoor As GTA.VehicleDoor, ByVal UnknownBool As Boolean, ByVal DoorAngle As Double)
        [Call]("CONTROL_CAR_DOOR", Car, CarDoor, UnknownBool, DoorAngle)
    End Sub

    Public Shared Sub DetachCar(Car As Vehicle)
        [Call]("DETACH_CAR", Car)
    End Sub
    Public Shared Function getAllColors(Car As Vehicle) As (ColorIndex, ColorIndex, ColorIndex, ColorIndex)
        Dim Pointer1, Pointer2, Pointer3, Pointer4 As Native.Pointer
        Pointer1 = New Native.Pointer(GetType(ColorIndex))
        Pointer2 = New Native.Pointer(GetType(ColorIndex))
        Pointer3 = New Native.Pointer(GetType(ColorIndex))
        Pointer4 = New Native.Pointer(GetType(ColorIndex))
        If Car.Exists Then
            [Call]("GET_CAR_COLOURS", Car, Pointer1, Pointer2)
            [Call]("GET_EXTRA_CAR_COLOURS", Car, Pointer3, Pointer4)
            Return (Pointer1.Value, Pointer2.Value, Pointer3.Value, Pointer4.Value)
        Else
            Exit Function
        End If
    End Function
    Public Shared Function getCurrentGear_TBoGT(Car As Vehicle) As Integer
        Return [Call](Of Integer)("GET_VEHICLE_GEAR", Car)
    End Function
    Public Shared Function getDeformation(Car As Vehicle, Position As Vector3) As Vector3
        Dim Deformation As Native.Pointer = New Native.Pointer(GetType(Vector3))
        [Call]("GET_CAR_DEFORMATION_AT_POS", Car, Position.X, Position.Y, Position.Z, Deformation)
        Return Deformation.Value
    End Function
    Public Shared Function getEngineRevs_TBoGT(Car As Vehicle) As Integer
        Return [Call](Of Integer)("GET_VEHICLE_ENGINE_REVS", Car)
    End Function
    Public Shared Function getEngineState(Car As Vehicle) As EngineState
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
    'Public Shared Function getForwardVector(Car As Vehicle) As Vector3
    '    Dim ForwardVector As Native.Pointer = New Native.Pointer(GetType(Vector3))
    '    [Call]("GET_CAR_FORWARD_VECTOR", Car, ForwardVector)
    '    Return ForwardVector.Value
    'End Function
    ''' <summary>
    ''' Returns the height (in meters) of the specified entity above the ground level.
    ''' </summary>
    ''' <returns>Height above the ground in meters. Returns 0 if ground not found.</returns>
    Public Shared Function getHeightAboveGround(Car As Vehicle) As Single
        Try
            If Car Is Nothing OrElse Not Car.Exists Then Return 0.0F

            Dim pos As GTA.Vector3 = Car.Position
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
    Public Shared Function getNumberOfPassengers(Car As Vehicle) As Integer
        Dim Num As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call]("GET_NUMBER_OF_PASSENGERS", Car, Num)
        Return Num.Value
    End Function

    'Public Shared Function getSpeedVector(Car As Vehicle, Optional Unknown As Boolean = False) As Vector3
    '    Dim SpeedVector As Native.Pointer = New Native.Pointer(GetType(Vector3))
    '    [Call]("GET_CAR_SPEED_VECTOR", Car, SpeedVector, Unknown)
    '    Return SpeedVector.Value
    'End Function
    Public Shared Function hasBeenDamagedByCar(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("HAS_CAR_BEEN_DAMAGED_BY_CAR", Car, World.GetClosestVehicle(Car.Position, 6.0F))
    End Function
    Public Shared Function hasBeenResprayed(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("HAS_CAR_BEEN_RESPRAYED")
    End Function
    Public Shared Function isDoorDamaged(Car As Vehicle, Door As GTA.VehicleDoor) As Boolean
        Return [Call](Of Boolean)("IS_CAR_DOOR_DAMAGED", Car, Door)
    End Function
    Public Shared Function isDoorFullyOpen(Car As Vehicle, Door As GTA.VehicleDoor) As Boolean
        Return [Call](Of Boolean)("IS_CAR_DOOR_FULLY_OPEN", Car, Door)
    End Function
    Public Shared Function isNotTimeTravel4Vehicle(Car As Vehicle) As Boolean
        If Car.Model = "DELNOR" Or Car.Model = "DELONE" Or Car.Model = "DELTWO" Or Car.Model = "DELHOV" Or Car.Model = "DELTHR" Or Car.Model = "DELRRO" Or Car.Model = "DELCUS" Or Car.Model = "DOCTRK" Or Car.Model = "KITTNC" Or Car.Model = "KITTSPM" Then
            Return False
        Else
            Return True
        End If
    End Function
    'IS_CAR_DEAD
    Public Shared Function isEngineRunning(Car As Vehicle) As Boolean
        ' Dim HealthPointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        ' [Call]("GET_PETROL_TANK_HEALTH", Car, HealthPointer)
        Dim Fuel As Integer = Car.PetrolTankHealth
        If Fuel > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function isExtraEnabled(Car As Vehicle, ExtraID As Integer) As Integer
        Return [Call](Of Integer)("IS_VEHICLE_EXTRA_TURNED_ON", Car, ExtraID)
    End Function
    Public Shared Function isInAirProper(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_IN_AIR_PROPER", Car)
    End Function
    Public Shared Function isOnFire(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_ON_FIRE", Car)
    End Function
    Public Shared Function isStoppedAtTrafficLight(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_STOPPED_AT_TRAFFIC_LIGHTS", Car)
    End Function
    Public Shared Function isTouchingObject(Car As Vehicle, Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("IS_VEHICLE_TOUCHING_OBJECT", Car, Obj)
    End Function
    Public Shared Function isAnyTireBurst(Car As Vehicle) As Boolean
        If [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.FrontLeft) Or
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.FrontRight) Or
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.CenterLeft) Or
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.CenterRight) Or
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.RearLeft) Or
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.RearRight) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function isInAnyGarage(Car As Vehicle) As Boolean
        Return NativeGarage.List.Any(Function(g) g.isVehicleInside(Car))
    End Function

    Public Shared Function isBig(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_BIG_VEHICLE")
    End Function
    Public Shared Function isStopped(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_STOPPED", Car)
    End Function
    Public Shared Function isWindowIntact(Car As Vehicle, Window As GTA.VehicleWindow) As Boolean
        Return [Call](Of Boolean)("IS_VEH_WINDOW_INTACT", Car, Window)
    End Function
    Public Shared Sub RemoveWindow(Car As Vehicle, Window As GTA.VehicleWindow, Smash As Boolean)
        If Smash Then [Call]("SMASH_CAR_WINDOW", Car, Window) Else [Call]("REMOVE_CAR_WINDOW", Car, Window)
    End Sub
    Public Shared Sub ResetWheelsPosition(Car As Vehicle)
        [Call]("RESET_CAR_WHEELS", Car)
    End Sub
    Public Shared Function areTaxiLightsOn(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("ARE_TAXI_LIGHTS_ON", Car)
    End Function 'Taxi Lights
    Public Shared Sub SetAlarm(Car As Vehicle, Optional Duration As Integer = -1)
        [Call]("SET_VEH_ALARM", Car, True)
        If Duration > 0 Then [Call]("SET_VEH_ALARM_DURATION", Car, Duration)
    End Sub
    Public Shared Sub SetAllColors(Car As Vehicle, Color1 As ColorIndex, Color2 As ColorIndex, Color3 As ColorIndex, Color4 As ColorIndex)
        If Car.Exists Then
            [Call]("CHANGE_CAR_COLOUR", Color1.Index, Color2.Index)
            [Call]("SET_EXTRA_CAR_COLOURS", Color2.Index, Color3.Index)
        End If
    End Sub
    Public Shared Sub SetEngineON(Car As Vehicle, Optional FuelAmount As Integer = 1000)
        '[Call]("SET_PETROL_TANK_HEALTH", Car, FuelAmount)
        If Car.isRequiredForMission Then
            [Call]("SET_CAR_ENGINE_ON", Car, 1, 1)
        Else
            Car.PetrolTankHealth = FuelAmount
        End If
    End Sub
    Public Shared Sub SetEngineOFF(Car As Vehicle, Optional OutOfFuel As Boolean = False)
        If OutOfFuel Then
            ' [Call]("SET_PETROL_TANK_HEALTH", Car, -1)
            Car.PetrolTankHealth = -1
        Else
            If isNotTimeTravel4Vehicle(Car) And Car.isRequiredForMission Then
                [Call]("SET_CAR_ENGINE_ON", Car, 0, 0)
            ElseIf isNotTimeTravel4Vehicle(Car) = False Or Car.isRequiredForMission = False Then
                Car.PetrolTankHealth = -1000
                '[Call]("SET_PETROL_TANK_HEALTH", Car, -1000)
                [Call]("SET_CAR_ENGINE_ON", Car, 0, 0)
            End If
        End If
    End Sub
    Public Shared Sub SetTaxiLights(Car As Vehicle, TurnON As Boolean)
        [Call]("SET_TAXI_LIGHTS", Car, TurnON)
    End Sub 'Taxi Lights
    Public Shared Sub SetTraction_TBoGT(Car As Vehicle, Traction As Single)
        [Call]("SET_CAR_TRACTION", Car, Traction)
    End Sub
    Public Shared Sub TriggerAlarm(Car As Vehicle, Optional Duration As Integer = -1)
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
    Public Shared Sub SetHeadlights(Car As Vehicle, LightsState As LightsState)
        [Call]("FORCE_CAR_LIGHTS", Car, LightsState)
    End Sub
    Public Shared Sub SetInteriorLight(Car As Vehicle, LightOn As Boolean)
        [Call]("SET_VEH_INTERIORLIGHT", Car, LightOn)
    End Sub
    Public Shared Sub SetHeliBladesFullSpeed(Heli As Vehicle)
        [Call]("SET_HELI_BLADES_FULL_SPEED", Heli)
    End Sub
    'Public Shared Sub SetHeliStabilizer(Heli As Vehicle)
    '    [Call]("SET_HELI_STABILISER", Heli)
    'End Sub
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
    Public Shared Sub SoundCarHorn(Car As Vehicle, Duration As Integer)
        [Call]("SOUND_CAR_HORN", Car, Duration)
    End Sub
    'public shared Sub StartFire(Car As Vehicle)
    '    [Call]("START_CAR_FIRE", Car)
    'End Sub
    'Public Shared Function SpawnVehicle(Model As String, Position As Vector3, Optional Visible As Boolean = True, Optional Collisions As Boolean = True,
    '                                    Optional MsgText As String = "", Optional MsgTime As Integer = 2000, Optional LeftUpMsgType As Boolean = False) As GTA.Vehicle
    '    [Call]("REQUEST_MODEL", NativeModels.getHashKey(Model))
    '    'Do
    '    'e.WaitInCurrentScript(1)
    '    If MsgText <> "" Then
    '        If LeftUpMsgType Then
    '            Game.DisplayText(MsgText, MsgTime)
    '        Else
    '            NativeDraws.DisplayText(MsgText, MsgTime)
    '        End If
    '    End If
    '    ' Loop Until [Call](Of Boolean)("HAS_MODEL_LOADED", ModelID)
    '    'Dim pointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
    '    '[Call]("CREATE_CAR", NativeModels.getHashKey(Model), Position.X, Position.Y, Position.Z, pointer, 1)
    '    Dim SpawnedCar = World.CreateVehicle(NativeModels.getHashKey(Model), Position)
    '    '  SetCollision(pointer.Value, True)
    '    '  SpawnedCar.Visible = True
    '    ' Dim veh As Vehicle = pointer.Value
    '    ' veh.Visible = Visible
    '    Return SpawnedCar 'pointer.Value
    'End Function

    Public Shared Sub SetStrongAxles(Car As Vehicle, StrongAxles As Boolean)
        [Call]("SET_VEH_HAS_STRONG_AXLES", Car, StrongAxles)
    End Sub
    Public Shared Sub SetStrong(Car As Vehicle, Strong As Boolean)
        [Call]("SET_CAR_STRONG", Car, Strong)
    End Sub
    Public Shared Sub SetVehicleExtras(Car As Vehicle, Extra0 As Boolean, Extra1 As Boolean, Extra2 As Boolean, Extra3 As Boolean, Extra4 As Boolean, Extra5 As Boolean, Extra6 As Boolean, Extra7 As Boolean, Extra8 As Boolean, Extra9 As Boolean)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 0, Not Extra0)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 1, Not Extra1)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 2, Not Extra2)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 3, Not Extra3)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 4, Not Extra4)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 5, Not Extra5)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 6, Not Extra6)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 7, Not Extra7)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 8, Not Extra8)
        [Call]("TURN_OFF_VEHICLE_EXTRA", Car, 9, Not Extra9)
    End Sub
    'Public Class VehicleExtended
    '    Private BaseVehicle As Vehicle
    '    Private Shared Registry As New Dictionary(Of Vehicle, VehicleExtended)
    '    Public ReadOnly Property Handle As Vehicle
    '        Get
    '            Return BaseVehicle
    '        End Get
    '    End Property
    '    Private _Lights As Integer = 0
    '    Private _Hazard As Integer = 0
    '    Private _LightMultiplier As Single = 1.0
    '    Private _IntiLight As Boolean = False
    '    Private _GPSenabled As Boolean = True
    '    Private _Alpha255 As Integer = 255
    '    Private _Coll As Boolean = True
    '    Private _Fuel As Integer = 100
    '    Private _AlarmDur As Integer = -1
    '    Private _AlarmEnabled As Boolean = False
    '    Private _EngState As Integer = 0
    '    Public Property isOwnedByPlayer As Boolean = False
    '    Public ReadOnly Property Driver As Ped
    '        Get
    '            Return BaseVehicle.GetPedOnSeat(VehicleSeat.Driver)
    '        End Get
    '    End Property
    '    Public Property FuelAmount As Integer
    '        Get
    '            If BaseVehicle.PetrolTankHealth > 0 Then
    '                Return BaseVehicle.PetrolTankHealth / 10
    '            ElseIf BaseVehicle.PetrolTankHealth = 0 OrElse BaseVehicle.PetrolTankHealth = -1 Then
    '                Return 0
    '            Else
    '                Return _Fuel
    '            End If
    '        End Get
    '        Set(value As Integer)
    '            BaseVehicle.PetrolTankHealth = value
    '        End Set
    '    End Property
    '    Public Property Collision As Boolean
    '        Get
    '            Return _Coll
    '        End Get
    '        Set(value As Boolean)
    '            SetCollision(BaseVehicle, value)
    '            _Coll = value
    '        End Set
    '    End Property
    '    Public Property Alpha As Integer
    '        Get
    '            Return _Alpha255
    '        End Get
    '        Set(value As Integer)
    '            SetAlpha(BaseVehicle, value)
    '            _Alpha255 = value
    '        End Set
    '    End Property
    '    Public Property GPS As Boolean
    '        Get
    '            Return _GPSenabled
    '        End Get
    '        Set(value As Boolean)
    '            SetGPSEnabled(BaseVehicle, value)
    '            _GPSenabled = value
    '        End Set
    '    End Property
    '    Public ReadOnly Property NumberOfPassengers As Integer
    '        Get
    '            Return getNumberOfPassengers(BaseVehicle)
    '        End Get
    '    End Property
    '    Public ReadOnly Property isBig As Boolean
    '        Get
    '            Return NativeVehicles.isBig(BaseVehicle)
    '        End Get
    '    End Property
    '    Public Property HeadLightsMultiplier As Single
    '        Get
    '            Return _LightMultiplier
    '        End Get
    '        Set(value As Single)
    '            SetHeadlightsMultiplier(BaseVehicle, value)
    '            _LightMultiplier = value
    '        End Set
    '    End Property
    '    Public Property EngineState As EngineState
    '        Get
    '            Return _EngState
    '        End Get
    '        Set(value As EngineState)
    '            Select Case EngineState
    '                Case -1
    '                    If BaseVehicle.isRequiredForMission = False Or isOwnedByPlayer Then BaseVehicle.PetrolTankHealth = -1
    '                    [Call]("SET_CAR_ENGINE_ON", BaseVehicle, 0, 0)
    '                    _EngState = -1
    '                Case 0
    '                    BaseVehicle.PetrolTankHealth = FuelAmount
    '                    _EngState = 0
    '                Case 1
    '                    If BaseVehicle.isRequiredForMission = False Or isOwnedByPlayer Then BaseVehicle.PetrolTankHealth = -1000
    '                    [Call]("SET_CAR_ENGINE_ON", BaseVehicle, 0, 0)
    '                    _EngState = 1
    '            End Select
    '        End Set
    '    End Property
    '    Public Property InteriorLight As Boolean
    '        Get
    '            Return _IntiLight
    '        End Get
    '        Set(value As Boolean)
    '            SetInteriorLight(BaseVehicle, value)
    '            _IntiLight = value
    '        End Set
    '    End Property
    '    Public Property LightsState As LightsState
    '        Get
    '            Return _Lights
    '        End Get
    '        Set(value As LightsState)
    '            SetHeadlights(BaseVehicle, value)
    '            _Lights = value
    '        End Set
    '    End Property
    '    Public Property HazardLights As HazardLightsState
    '        Get
    '            Return _Hazard
    '        End Get
    '        Set(value As HazardLightsState)
    '            _Lights = value
    '            Select Case value
    '                Case 0
    '                    SetHazardLights(BaseVehicle, False, False)
    '                Case 1
    '                    SetHazardLights(BaseVehicle, True, True)
    '                Case 2
    '                    SetHazardLights(BaseVehicle, True, False)
    '            End Select
    '        End Set
    '    End Property
    '    Public Property Alarm As Boolean
    '        Get
    '            Return _AlarmEnabled
    '        End Get
    '        Set(value As Boolean)
    '            [Call]("SET_VEH_ALARM", BaseVehicle, True)
    '            _AlarmEnabled = value
    '        End Set
    '    End Property
    '    Public Property AlarmDuration As Integer
    '        Get
    '            Return _AlarmDur
    '        End Get
    '        Set(value As Integer)
    '            [Call]("SET_VEH_ALARM_DURATION", BaseVehicle, _AlarmDur)
    '            _AlarmDur = value
    '        End Set
    '    End Property
    '    Public Sub New(Car As Vehicle)
    '        If Not Registry.ContainsKey(Car) Then
    '            Registry(Car) = Me
    '            BaseVehicle = Car
    '        End If
    '    End Sub
    '    Public Shared Function getVehicleExtended(Car As Vehicle, Optional CreateIfNecessary As Boolean = True) As VehicleExtended
    '        If Car Is Nothing Then Return Nothing

    '        If Registry.ContainsKey(Car) Then
    '            Return Registry(Car)
    '        Else
    '            If CreateIfNecessary Then
    '                Return New VehicleExtended(Car)
    '            Else
    '                Return Nothing
    '            End If
    '        End If
    '    End Function
    '    Public Sub TriggerHorn(Duration As Integer)
    '        [Call]("SOUND_CAR_HORN", BaseVehicle, Duration)
    '    End Sub
    '    Public Sub TriggerAlarm(Optional Duration As Integer = -1)
    '        NativeVehicles.TriggerAlarm(BaseVehicle, Duration)
    '    End Sub
    '    Public Sub PopBoot()
    '        [Call]("POP_CAR_BOOT", BaseVehicle)
    '    End Sub
    'End Class
    Public Class CarRecording
        Public Shared Function GetPositionInRecording(Car As Vehicle) As Single
            Return [Call](Of Single)("FIND_POSITION_IN_RECORDING", Car)
        End Function
        Public Shared Function GetTimePositionInRecording(Car As Vehicle) As Integer
            Return [Call](Of Integer)("FIND_TIME_POSITION_IN_RECORDING", Car)
        End Function
        Public Shared Function GetTotalDurationOfRecording(Car As Vehicle) As Single
            Return [Call](Of Single)("GET_TOTAL_DURATION_OF_CAR_RECORDING", Car)
        End Function
        Public Shared Function HasRecordingLoaded(RRR_Index As Integer) As Boolean
            Return [Call](Of Boolean)("HAS_CAR_RECORDING_BEEN_LOADED", RRR_Index)
        End Function
        Public Shared Sub Request(RRR_Index As Integer)
            [Call]("REQUEST_CAR_RECORDING", RRR_Index)
        End Sub
        Public Shared Sub Remove(RRR_Index As Integer)
            [Call]("REMOVE_CAR_RECORDING", RRR_Index)
        End Sub
        Public Shared Sub SkipTimeInPlayback(Car As Vehicle, Time As Integer)
            [Call]("SKIP_TIME_IN_PLAYBACK_RECORDED_CAR", Car, Time)
        End Sub
        Public Shared Sub SkipToEndAndStop(Car As Vehicle)
            [Call]("SKIP_TO_END_AND_STOP_PLAYBACK_RECORDED_CAR", Car)
        End Sub
        Public Shared Sub StartPlayback(RRR_Index As Integer, Car As Vehicle)
            [Call]("START_PLAYBACK_RECORDED_CAR", Car, RRR_Index)
        End Sub
        Public Shared Sub PausePlayback(Car As Vehicle)
            [Call]("PAUSE_PLAYBACK_RECORDED_CAR", Car)
        End Sub
        Public Shared Sub UnpausePlayback(Car As Vehicle)
            [Call]("UNPAUSE_PLAYBACK_RECORDED_CAR", Car)
        End Sub
        Public Shared Sub StopPlayback(Car As Vehicle)
            [Call]("STOP_PLAYBACK_RECORDED_CAR", Car)
        End Sub
    End Class

    '   GET_POSITION_OF_CAR_RECORDING_AT_TIME( l_U839[I], fVar14, ref uVar11 )  fVar14 ---> [fVar14 < (GET_TOTAL_DURATION_OF_CAR_RECORDING(car))]
    'Public Shared Function SpawnVehTest(Model As String, position As Vector3) As Vehicle
    '    Tick.Enable()
    '    If _tempSpawnedVeh.Exists = False Then
    '    Else
    '        Return _tempSpawnedVeh
    '    End If
    'End Function
    '    Private Shared Sub OnTick()
    '        If Not [Call](Of Boolean)("HAS_MODEL_LOADED", NativeModels.getHashKey(_tempRequestedModel)) Then
    '            [Call]("REQUEST_MODEL", NativeModels.getHashKey(_tempRequestedModel))
    '        Else
    '            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
    '            [Call]("CREATE_CAR", NativeModels.getHashKey(_tempRequestedModel), _tempSpawnedVehPos.X, _tempSpawnedVehPos.Y, _tempSpawnedVehPos.Z, pointer, 1)
    '            If CType(pointer.Value, Vehicle).Exists Then
    '                _tempSpawnedVeh = pointer.Value
    '                Tick.Disable()
    '            End If
    '        End If
    '    End Sub

    '    Public Class Tick
    '        Private Shared _tickHandler As Action = AddressOf OnTick
    '        Public Sub New(handler As Action)
    '            _tickHandler = handler
    '        End Sub
    '        ''' <summary>
    '        ''' Enables the Tick adding it to the TickHelper. You must call TickHelper.ProcessAll
    '        ''' in a Tick inside main script to process them.
    '        ''' </summary>
    '        Public Shared Sub Enable()
    '            TickHelper.Add_Internal(_tickHandler)
    '        End Sub
    '        ''' <summary>
    '        ''' Disables the Tick removing it from the TickHelper.
    '        ''' </summary>
    '        Public Shared Sub Disable()
    '            TickHelper.Remove_Internal(_tickHandler)
    '        End Sub
    '    End Class
End Class
'Public Module VehicleExtension
'    <Extension()>
'    Public Sub SetHazardLights(Veh As Vehicle, state As HazardLightsState)

'    End Sub
'End Module