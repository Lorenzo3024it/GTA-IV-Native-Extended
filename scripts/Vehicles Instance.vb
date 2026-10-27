
Imports GTA
Imports GTA.Native.Function
Imports NativeExtended.Tools
'Aggiungere GetRPM, direction, position ecc
Partial Public Class NativeVehicle
    Private _vehicle As Vehicle
    Private _canRespray As Boolean
    Public Property Handle As Vehicle
        Get
            Return _vehicle
        End Get
        Set(value As Vehicle)
            _vehicle = value
            If _vehicle.Model.isBike OrElse _vehicle.Model.isCar Then

            Else
                CanBeResprayed = False
            End If
        End Set
    End Property

    Public Enum EngineStates
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

    Public Sub New(veh As Vehicle)
        _vehicle = veh
    End Sub

    ''' <summary>
    ''' Checks if the vehicle exists in the world.
    ''' </summary>
    Public Function Exists() As Boolean
        Return _vehicle IsNot Nothing AndAlso _vehicle.Exists
    End Function
    Public Sub AttachToCar(ByVal Car As Vehicle, ByVal AbOffs As Boolean, ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single, ByVal RotX As Single, ByVal RotY As Single, ByVal RotZ As Single)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("ATTACH_CAR_TO_CAR", _vehicle, Car, AbOffs, PosX, PosY, PosZ, RotX, RotY, RotZ)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.AttachToCar: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub AttachToCarPhisically(ByVal Car As Vehicle, ByVal AbOffs As Boolean, ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single, ByVal RotX As Single, ByVal RotY As Single, ByVal RotZ As Single)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("ATTACH_CAR_TO_CAR", _vehicle, Car, AbOffs, PosX, PosY, PosZ, RotX, RotY, RotZ)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.AttachToCarPhisically: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub BreakDoor(Door As GTA.VehicleDoor, InstantVanish As Boolean)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("BREAK_CAR_DOOR", _vehicle, Door, InstantVanish)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.BreakDoor: vehicle not found.", ex)
        End Try
    End Sub
    Public Property CanBeResprayed As Boolean
        Set(value As Boolean)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("SET_CAN_RESPRAY_CAR", value)
                _canRespray = value
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.CanBeResprayed: vehicle not found.", ex)
            End Try
        End Set
        Get
            Return _canRespray
        End Get
    End Property
    Public Sub ControlDoor(CarDoor As GTA.VehicleDoor, UnknownBool As Boolean, DoorAngle As Double)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("CONTROL_CAR_DOOR", _vehicle, CarDoor, UnknownBool, DoorAngle)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.ControlDoor: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub Detach()
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("DETACH_CAR", _vehicle)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.DetachCar: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub Freeze(Optional Freeze As Boolean = True, Optional DontLoadCollision As Boolean = False)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            If Freeze Then
                If DontLoadCollision Then
                    [Call]("FREEZE_CAR_POSITION_AND_DONT_LOAD_COLLISION", _vehicle, True)
                Else
                    [Call]("FREEZE_CAR_POSITION", _vehicle, True)
                End If
            Else
                [Call]("FREEZE_CAR_POSITION", _vehicle, False)
                [Call]("FREEZE_CAR_POSITION_AND_DONT_LOAD_COLLISION", _vehicle, False)
            End If
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.Freeze: vehicle not found.", ex)
        End Try
    End Sub
    Public Function GetAllColors() As (ColorIndex, ColorIndex, ColorIndex, ColorIndex)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            Dim Pointer1, Pointer2, Pointer3, Pointer4 As Native.Pointer
            Pointer1 = New Native.Pointer(GetType(ColorIndex))
            Pointer2 = New Native.Pointer(GetType(ColorIndex))
            Pointer3 = New Native.Pointer(GetType(ColorIndex))
            Pointer4 = New Native.Pointer(GetType(ColorIndex))
            [Call]("GET_CAR_COLOURS", _vehicle, Pointer1, Pointer2)
            [Call]("GET_EXTRA_CAR_COLOURS", _vehicle, Pointer3, Pointer4)
            Return (Pointer1.Value, Pointer2.Value, Pointer3.Value, Pointer4.Value)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.GetAllColors: vehicle not found.", ex)
            Return (Nothing, Nothing, Nothing, Nothing)
        End Try
    End Function
    Public ReadOnly Property NumberOfPassengers As Integer
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Dim Num As Native.Pointer = New Native.Pointer(GetType(Integer))
                [Call]("GET_NUMBER_OF_PASSENGERS", _vehicle, Num)
                Return Num.Value
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.GetNumberOfPassengers: vehicle not found.", ex)
                Return 0
            End Try
        End Get
    End Property
    Public ReadOnly Property isEngineRunning As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return _vehicle.PetrolTankHealth > 0
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.isEngineRunning: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public Sub SetEngineON(Optional FuelAmount As Integer = 1000)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            If _vehicle.isRequiredForMission Then
                [Call]("SET_CAR_ENGINE_ON", _vehicle, 1, 1)
            Else
                _vehicle.PetrolTankHealth = FuelAmount
            End If
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetEngineON: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub SetEngineOFF(Optional OutOfFuel As Boolean = False)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            If OutOfFuel Then
                _vehicle.PetrolTankHealth = -1
            Else
                If _vehicle.isRequiredForMission Then
                    [Call]("SET_CAR_ENGINE_ON", _vehicle, 0, 0)
                Else
                    _vehicle.PetrolTankHealth = -1000
                    [Call]("SET_CAR_ENGINE_ON", _vehicle, 0, 0)
                End If
            End If
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetEngineOFF: vehicle not found.", ex)
        End Try
    End Sub
    Public Property TaxiLights As Boolean
        Get
            Return [Call](Of Boolean)("ARE_TAXI_LIGHTS_ON", _vehicle)
        End Get
        Set(value As Boolean)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("SET_TAXI_LIGHTS", _vehicle, value)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.SetTaxiLights: vehicle not found.", ex)
            End Try
        End Set
    End Property
    Public Sub SetTraction(Traction As Single)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("SET_CAR_TRACTION", _vehicle, Traction)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetTraction: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub TriggerAlarm(Optional Duration As Integer = -1)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("SET_VEH_ALARM", _vehicle, True)
            If Duration > 0 Then [Call]("SET_VEH_ALARM_DURATION", _vehicle, Duration)
            [Call]("TRIGGER_VEH_ALARM", _vehicle, True)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.TriggerAlarm: vehicle not found.", ex)
        End Try
    End Sub
    Public WriteOnly Property Alpha As Byte
        Set(value As Byte)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("SET_VEHICLE_ALPHA", _vehicle, CInt(value))
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.SetAlpha: vehicle not found.", ex)
            End Try
        End Set
    End Property

    Public WriteOnly Property Collision As Boolean
        Set(value As Boolean)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("SET_CAR_COLLISION", _vehicle, value)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.SetCollision: vehicle not found.", ex)
            End Try
        End Set
    End Property
    Public Sub SetHazardLights(HazardLights As Boolean, Optional Blinking As Boolean = True)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            If HazardLights Then
                If Blinking Then
                    [Call]("SET_VEH_INDICATORLIGHTS", _vehicle, False)
                    [Call]("SET_VEH_HAZARDLIGHTS", _vehicle, True)
                Else
                    [Call]("SET_VEH_HAZARDLIGHTS", _vehicle, False)
                    [Call]("SET_VEH_INDICATORLIGHTS", _vehicle, True)
                End If
            Else
                [Call]("SET_VEH_HAZARDLIGHTS", _vehicle, False)
                [Call]("SET_VEH_INDICATORLIGHTS", _vehicle, False)
            End If
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetHazardLights: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub SetHeadlights(LightsState As LightsState)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("FORCE_CAR_LIGHTS", _vehicle, LightsState)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetHeadlights: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub SetInteriorLight(LightOn As Boolean)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("SET_VEH_INTERIORLIGHT", _vehicle, LightOn)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetInteriorLight: vehicle not found.", ex)
        End Try
    End Sub
    Public WriteOnly Property HeadlightsMultiplier As Single
        Set(value As Single)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("SET_CAR_LIGHT_MULTIPLIER", _vehicle, value)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.SetHeadlightsMultiplier: vehicle not found.", ex)
            End Try
        End Set
    End Property
    Public WriteOnly Property GPS As Boolean
        Set(value As Boolean)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("ENABLE_GPS_IN_VEHICLE", _vehicle, value)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.SetGPSEnabled: vehicle not found.", ex)
            End Try
        End Set
    End Property
    Public Sub SetGPSRoute(Blip As Blip, Optional Enable As Boolean = True)
        Try
            Dim state As Integer = If(Enable, 1, 0)
            [Call]("SET_ROUTE", Blip, state)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetGPSRoute: blip not found.", ex)
        End Try
    End Sub
    'Public Sub SetExtraWithDoorAngle(ExtraID As Integer, Enabled As Boolean)
    '    Try
    '        If Not Exists() Then Throw New Exception("vehicle not found.")
    '        [Call]("TURN_OFF_VEHICLE_EXTRA", _vehicle, ExtraID, Not Enabled)
    '    Catch ex As Exception
    '        WriteLogAdvanced("Error in NativeVehicle.SetExtraWithDoorAngle: vehicle not found.", ex)
    '    End Try
    'End Sub
    Public Sub SoundHorn(Duration As Integer)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("SOUND_CAR_HORN", _vehicle, Duration)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SoundCarHorn: vehicle not found.", ex)
        End Try
    End Sub
    Public WriteOnly Property StrongAxles As Boolean
        Set(value As Boolean)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("SET_VEH_HAS_STRONG_AXLES", _vehicle, value)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.SetStrongAxles: vehicle not found.", ex)
            End Try
        End Set
    End Property
    Public WriteOnly Property Strong As Boolean
        Set(value As Boolean)
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                [Call]("SET_CAR_STRONG", _vehicle, value)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.SetStrong: vehicle not found.", ex)
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Enables or disables vehicle extras.
    ''' You may specify only the extras you want to modify; all parameters are optional.
    ''' </summary>
    Public Sub SetExtras(Optional Extra0 As Boolean? = Nothing, Optional Extra1 As Boolean? = Nothing, Optional Extra2 As Boolean? = Nothing,
                         Optional Extra3 As Boolean? = Nothing, Optional Extra4 As Boolean? = Nothing, Optional Extra5 As Boolean? = Nothing,
                         Optional Extra6 As Boolean? = Nothing, Optional Extra7 As Boolean? = Nothing, Optional Extra8 As Boolean? = Nothing, Optional Extra9 As Boolean? = Nothing)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            ApplyExtra(0, Extra0)
            ApplyExtra(1, Extra1)
            ApplyExtra(2, Extra2)
            ApplyExtra(3, Extra3)
            ApplyExtra(4, Extra4)
            ApplyExtra(5, Extra5)
            ApplyExtra(6, Extra6)
            ApplyExtra(7, Extra7)
            ApplyExtra(8, Extra8)
            ApplyExtra(9, Extra9)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetExtras: vehicle not found.", ex)
        End Try
    End Sub
    Private Sub ApplyExtra(index As Integer, value As Boolean?)
        Try
            If value.HasValue Then
                [Call]("TURN_OFF_VEHICLE_EXTRA", _vehicle, index, Not value.Value)
            End If
        Catch ex As Exception
            WriteLogAdvanced($"Error in NativeVehicle.ApplyExtra at index {index}: vehicle not found.", ex)
        End Try
    End Sub
    Public Function GetCarBlocking() As Vehicle
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
            [Call](Of Vehicle)("GET_CAR_BLOCKING_CAR", _vehicle, pointer)
            Return pointer.Value
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.GetCarBlocking: vehicle not found.", ex)
            Return Nothing
        End Try
    End Function

    Public ReadOnly Property CurrentGear_TBoGT As Integer
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Integer)("GET_VEHICLE_GEAR", _vehicle)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.GetCurrentGear: vehicle not found.", ex)
                Return 0
            End Try
        End Get
    End Property

    Public Function GetDeformation(Position As Vector3) As Vector3
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            Dim Deformation As Native.Pointer = New Native.Pointer(GetType(Vector3))
            [Call]("GET_CAR_DEFORMATION_AT_POS", _vehicle, Position.X, Position.Y, Position.Z, Deformation)
            Return Deformation.Value
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.GetDeformation: vehicle not found.", ex)
            Return Vector3.Zero
        End Try
    End Function

    Public ReadOnly Property EngineRevs_TBoGT As Integer
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Integer)("GET_VEHICLE_ENGINE_REVS", _vehicle)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.GetEngineRevs: vehicle not found.", ex)
                Return 0
            End Try
        End Get
    End Property
    Public ReadOnly Property EngineState As EngineStates
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Dim Fuel As Integer = _vehicle.PetrolTankHealth
                If Fuel = -1 Then Return EngineState.OutOfFuel
                If Fuel > 0 Then Return EngineState.EngineON
                If Fuel = -1000 Then Return EngineState.EngineOFF
                Return EngineState.EngineOFF
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.GetEngineState: vehicle not found.", ex)
                Return EngineState.EngineOFF
            End Try
        End Get
        ' Set(value As EngineState)

        'End Set
    End Property
    Public ReadOnly Property HasBeenDamagedByCar As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Boolean)("HAS_CAR_BEEN_DAMAGED_BY_CAR", _vehicle, World.GetClosestVehicle(_vehicle.Position, 6.0F))
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.HasBeenDamagedByCar: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property HasBeenResprayed As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Boolean)("HAS_CAR_BEEN_RESPRAYED")
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.HasBeenResprayed: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public Function IsDoorDamaged(Door As GTA.VehicleDoor) As Boolean
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            Return [Call](Of Boolean)("IS_CAR_DOOR_DAMAGED", _vehicle, Door)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.IsDoorDamaged: vehicle not found.", ex)
            Return False
        End Try
    End Function
    Public Function IsDoorFullyOpen(Door As GTA.VehicleDoor) As Boolean
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            Return [Call](Of Boolean)("IS_CAR_DOOR_FULLY_OPEN", _vehicle, Door)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.IsDoorFullyOpen: vehicle not found.", ex)
            Return False
        End Try
    End Function
    Public ReadOnly Property IsNotTimeTravelVehicle As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Dim models As String() = {"DELNOR", "DELONE", "DELTWO", "DELHOV", "DELTHR", "DELRRO", "DELCUS", "DOCTRK", "KITTNC", "KITTSPM"}
                Return Not models.Contains(_vehicle.Model.ToString)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsNotTimeTravelVehicle: vehicle not found.", ex)
                Return True
            End Try
        End Get
    End Property
    Public ReadOnly Property IsExtraEnabled(ExtraID As Integer)
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Integer)("IS_VEHICLE_EXTRA_TURNED_ON", _vehicle, ExtraID)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsExtraEnabled: vehicle not found.", ex)
                Return 0
            End Try
        End Get
    End Property
    Public ReadOnly Property IsInAirProper As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Boolean)("IS_CAR_IN_AIR_PROPER", _vehicle)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsInAirProper: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property IsOnFire As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Boolean)("IS_CAR_ON_FIRE", _vehicle)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsOnFire: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property IsStoppedAtTrafficLight As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Boolean)("IS_CAR_STOPPED_AT_TRAFFIC_LIGHTS", _vehicle)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsStoppedAtTrafficLight: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public Function IsTouchingObject(Obj As GTA.Object) As Boolean
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            Return [Call](Of Boolean)("IS_VEHICLE_TOUCHING_OBJECT", _vehicle, Obj)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.IsTouchingObject: vehicle not found.", ex)
            Return False
        End Try
    End Function

    Public ReadOnly Property IsAnyTireBurst As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Dim wheels As VehicleWheel() = {VehicleWheel.FrontLeft, VehicleWheel.FrontRight, VehicleWheel.CenterLeft, VehicleWheel.CenterRight, VehicleWheel.RearLeft, VehicleWheel.RearRight}
                Return wheels.Any(Function(w) [Call](Of Boolean)("IS_CAR_TYRE_BURST", _vehicle, w))
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsAnyTireBurst: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property IsInAnyGarage As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return NativeGarage.List.Any(Function(g) g.isVehicleInside(_vehicle))
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsInAnyGarage: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property IsBig As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Boolean)("IS_BIG_VEHICLE")
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsBig: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property IsStopped As Boolean
        Get
            Try
                If Not Exists() Then Throw New Exception("vehicle not found.")
                Return [Call](Of Boolean)("IS_CAR_STOPPED", _vehicle)
            Catch ex As Exception
                WriteLogAdvanced("Error in NativeVehicle.IsStopped: vehicle not found.", ex)
                Return False
            End Try
        End Get
    End Property
    Public Function IsWindowIntact(Window As GTA.VehicleWindow) As Boolean
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            Return [Call](Of Boolean)("IS_VEH_WINDOW_INTACT", _vehicle, Window)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.IsWindowIntact: vehicle not found.", ex)
            Return False
        End Try
    End Function
    Public Sub RemoveWindow(Window As GTA.VehicleWindow, Smash As Boolean)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            If Smash Then
                [Call]("SMASH_CAR_WINDOW", _vehicle, Window)
            Else
                [Call]("REMOVE_CAR_WINDOW", _vehicle, Window)
            End If
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.RemoveWindow: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub ResetWheelsPosition()
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("RESET_CAR_WHEELS", _vehicle)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.ResetWheelsPosition: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub SetAlarm(Optional Duration As Integer = -1)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("SET_VEH_ALARM", _vehicle, True)
            If Duration > 0 Then [Call]("SET_VEH_ALARM_DURATION", _vehicle, Duration)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetAlarm: vehicle not found.", ex)
        End Try
    End Sub
    Public Sub SetAllColors(Color1 As ColorIndex, Color2 As ColorIndex, Color3 As ColorIndex, Color4 As ColorIndex)
        Try
            If Not Exists() Then Throw New Exception("vehicle not found.")
            [Call]("CHANGE_CAR_COLOUR", Color1.Index, Color2.Index)
            [Call]("SET_EXTRA_CAR_COLOURS", Color2.Index, Color3.Index)
        Catch ex As Exception
            WriteLogAdvanced("Error in NativeVehicle.SetAllColors: vehicle not found.", ex)
        End Try
    End Sub
    ' Continua nello stesso stile tutte le altre funzioni della classe originale...
    ' Ogni funzione deve avere Try..Catch e controllo Exists()
    ' Esempi: SetAlpha, SetHeadlights, SetTaxiLights, TriggerAlarm, SetExtras, ecc.
End Class
