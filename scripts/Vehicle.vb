Imports System.Collections.Concurrent
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Imports GTA.value
Imports Microsoft.VisualBasic.Logging
Imports NativeExtended.NativeVehicle
Imports NativeExtended.Tools
'STOP_CAR_BREAKING
Partial Public Class NativeVehicle
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
    Public Enum GpsVoice
        Female = 0
        Male = 1
    End Enum
    Public Shared Sub AttachCarToCar(ByVal Car1 As Vehicle, ByVal Car2 As Vehicle, ByVal AbOffs As Boolean, ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single, ByVal RotX As Single, ByVal RotY As Single, ByVal RotZ As Single)
        [Call]("ATTACH_CAR_TO_CAR", Car1, Car2, AbOffs, PosX, PosY, PosZ, RotX, RotY, RotZ)
    End Sub
    Public Shared Sub AttachCarToCarPhisically(Car1 As Vehicle, Car2 As Vehicle, UnknownBool As Boolean, UnknownInt As Integer, Offsets As Vector3, Buffer As Vector3, Car1RotX As Single, Car1RotY As Single)
        [Call]("ATTACH_CAR_TO_CAR_PHYSICALLY", Car1, Car2, UnknownBool, UnknownInt, Offsets.X, Offsets.Y, Offsets.Z, Buffer.X, Buffer.Y, Buffer.Z, Car1RotX, Car1RotY)
    End Sub
    Public Shared Sub CanBeResprayed(Car As Vehicle, CanBeResprayed As Boolean)
        [Call]("SET_CAN_RESPRAY_CAR", CanBeResprayed)
    End Sub
    Public Shared Sub ClearLastDamageEntity(Car As Vehicle)
        [Call]("CLEAR_CAR_LAST_DAMAGE_ENTITY", Car)
    End Sub
    Public Shared Sub ClearLastWeaponDamage(Car As Vehicle)
        [Call]("CLEAR_CAR_LAST_WEAPON_DAMAGE", Car)
    End Sub
    Public Shared Sub Delete(Car As Vehicle)
        If Car.Exists Then [Call]("DELETE_CAR", Car)
    End Sub
    Public Shared Sub Detach(Car As Vehicle)
        [Call]("DETACH_CAR", Car)
    End Sub
    Public Shared Sub ExtinguishFire(Car As Vehicle)
        [Call]("EXTINGUISH_CAR_FIRE", Car)
    End Sub
    Public Shared Sub Freeze(Car As Vehicle, Optional Freeze As Boolean = True, Optional DontLoadCollision As Boolean = False)
        If Freeze Then
            If DontLoadCollision Then
                [Call]("FREEZE_CAR_POSITION_AND_DONT_LOAD_COLLISION", Car, True)
            Else
                [Call]("FREEZE_CAR_POSITION", Car, True)
            End If
        Else
            [Call]("FREEZE_CAR_POSITION", Car, False)
            [Call]("FREEZE_CAR_POSITION_AND_DONT_LOAD_COLLISION", Car, False)
        End If
    End Sub
    Public Shared Function GetAllColors(Car As Vehicle) As (ColorIndex, ColorIndex, ColorIndex, ColorIndex)
        Dim Pointer1, Pointer2, Pointer3, Pointer4 As Native.Pointer
        Pointer1 = New Native.Pointer(GetType(ColorIndex))
        Pointer2 = New Native.Pointer(GetType(ColorIndex))
        Pointer3 = New Native.Pointer(GetType(ColorIndex))
        Pointer4 = New Native.Pointer(GetType(ColorIndex))
        If Car.Exists Then
            [Call]("GET_CAR_COLOURS", Car, Pointer1, Pointer2)
            [Call]("GET_EXTRA_CAR_COLOURS", Car, Pointer3, Pointer4)
            Return (CType(Pointer1.Value, ColorIndex), CType(Pointer2.Value, ColorIndex), CType(Pointer3.Value, ColorIndex), CType(Pointer4.Value, ColorIndex))
        Else
            Exit Function
        End If
    End Function
    Public Shared Function GetCarBlocking(Car As Vehicle) As Vehicle
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
        [Call](Of Vehicle)("GET_CAR_BLOCKING_CAR", Car, pointer)
        Return pointer.Value
    End Function
    Public Shared Function GetCurrentGear_TBoGT(Car As Vehicle) As Integer
        Return [Call](Of Integer)("GET_VEHICLE_GEAR", Car)
    End Function
    Public Shared Function GetDeformation(Car As Vehicle, Position As Vector3) As Vector3
        Dim Deformation As Native.Pointer = New Native.Pointer(GetType(Vector3))
        [Call]("GET_CAR_DEFORMATION_AT_POS", Car, Position.X, Position.Y, Position.Z, Deformation)
        Return Deformation.Value
    End Function
    Public Shared Function GetEngineRevs_TBoGT(Car As Vehicle) As Integer
        Return [Call](Of Integer)("GET_VEHICLE_ENGINE_REVS", Car)
    End Function
    Public Shared Function GetEngineState(Car As Vehicle) As EngineState
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
    ''' Returns the height (in meters) of the specified vehicle above the ground level. Returns 0 if ground not found.
    ''' </summary>
    Public Shared Function GetHeightAboveGround(Car As Vehicle) As Single
        Try
            If Car Is Nothing OrElse Not Car.Exists Then Return 0.0F

            Dim pos As GTA.Vector3 = Car.Position
            Dim groundZ As Single = 0.0F
            Dim pointerGround As Native.Pointer = New Native.Pointer(GetType(Single))
            [Call](Of Boolean)("GET_GROUND_Z_FOR_3D_COORD", pos.X, pos.Y, pos.Z, pointerGround)

            If pointerGround.Value > 0 Then
                Dim height As Single = pos.Z - pointerGround.Value
                Return If(height < 0, 0, height)
            Else
                Return 0.0F
            End If

        Catch ex As Exception
            Return 0.0F
        End Try
    End Function
    Public Shared Function GetName(Car As Vehicle) As String
        Return NativeDraws.GXT.GetString([Call](Of String)("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", Car.Model.Hash))
    End Function
    Public Shared Function GetName(CarModel As String) As String
        Return NativeDraws.GXT.GetString([Call](Of String)("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", NativeModel.GetHashKey(CarModel)))
    End Function
    Public Shared Function GetNumberOfPassengers(Car As Vehicle) As Integer
        Dim Num As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call]("GET_NUMBER_OF_PASSENGERS", Car, Num)
        Return Num.Value
    End Function
    Public Shared Function GetPassengerOnSeat(Car As Vehicle, Seat As VehicleSeat) As Ped
        If Car Is Nothing OrElse Car.Model.isVehicle = False Then
            WriteLog("Error in NativeVehicles.GetPassengerOnSeat: vehicle not valid or not found.")
            Return Nothing
        End If
        Dim pointer As New Native.Pointer(GetType(Ped))
        If Seat = VehicleSeat.Driver Then
            [Call]("GET_DRIVER_OF_CAR", Car, pointer)
        Else
            If Seat = VehicleSeat.AnyPassengerSeat Then Seat = VehicleSeat.RightFront
            [Call]("GET_CHAR_IN_CAR_PASSENGER_SEAT", Car, Seat, pointer)
        End If
        Return pointer.Value
    End Function

    'Public Shared Function getSpeedVector(Car As Vehicle, Optional Unknown As Boolean = False) As Vector3
    '    Dim SpeedVector As Native.Pointer = New Native.Pointer(GetType(Vector3))
    '    [Call]("GET_CAR_SPEED_VECTOR", Car, SpeedVector, Unknown)
    '    Return SpeedVector.Value
    'End Function
    Public Shared Function HasBeenDamagedByCar(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("HAS_CAR_BEEN_DAMAGED_BY_CAR", Car, World.GetClosestVehicle(Car.Position, 6.0F))
    End Function
    Public Shared Function HasBeenResprayed(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("HAS_CAR_BEEN_RESPRAYED", Car)
    End Function
    Public Shared Function HasRoof(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("DOES_CAR_HAVE_ROOF", Car)
    End Function
    ''' <summary>
    ''' Checks if the vehicle is a TimeTravel-IV vehicle. It's recommended to check in your scripts to avoid conflicts with that mod.
    ''' </summary>
    Public Shared Function IsNotTimeTravel4Vehicle(Car As Vehicle) As Boolean
        If Car.Model = "DELNOR" Or Car.Model = "DELONE" Or Car.Model = "DELTWO" Or Car.Model = "DELHOV" Or Car.Model = "DELTHR" Or Car.Model = "DELRRO" Or Car.Model = "DELCUS" Or Car.Model = "DOCTRK" Or Car.Model = "KITTNC" Or Car.Model = "KITTSPM" Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function IsDead(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_DEAD", Car)
    End Function
    Public Shared Function IsDriveable(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_VEH_DRIVEABLE", Car)
    End Function
    Public Shared Function IsEngineRunning(Car As Vehicle) As Boolean
        ' Dim HealthPointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        ' [Call]("GET_PETROL_TANK_HEALTH", Car, HealthPointer)
        Dim Fuel As Integer = Car.PetrolTankHealth
        If Fuel > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function IsExtraEnabled(Car As Vehicle, ExtraID As Integer) As Integer
        Return [Call](Of Integer)("IS_VEHICLE_EXTRA_TURNED_ON", Car, ExtraID)
    End Function
    Public Shared Function IsInAirProper(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_IN_AIR_PROPER", Car)
    End Function
    Public Shared Function IsOnFire(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_ON_FIRE", Car)
    End Function
    Public Shared Function IsPoliceCar(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_EMERGENCY_SERVICES_VEHICLE", Car)
    End Function
    Public Shared Function IsSeatFree(Car As Vehicle, Seat As VehicleSeat)
        If Car Is Nothing Then
            WriteLog("Error in NativeVehicles.IsSeatFree: vehicle not valid or not found.")
            Return Nothing
        End If
        Return [Call](Of Boolean)("IS_CAR_PASSENGER_SEAT_FREE", Car, Seat)
    End Function
    Public Shared Function IsStoppedAtTrafficLight(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_STOPPED_AT_TRAFFIC_LIGHTS", Car)
    End Function
    Public Shared Function IsTouchingObject(Car As Vehicle, Obj As GTA.Object) As Boolean
        Return [Call](Of Boolean)("IS_VEHICLE_TOUCHING_OBJECT", Car, Obj)
    End Function
    Public Shared Function IsUpsideDown(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_UPSIDEDOWN", Car)
    End Function
    Public Shared Function IsAnyTireBurst(Car As Vehicle) As Boolean
        If [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.FrontLeft) OrElse
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.FrontRight) OrElse
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.CenterLeft) OrElse
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.CenterRight) OrElse
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.RearLeft) OrElse
           [Call](Of Boolean)("IS_CAR_TYRE_BURST", Car, VehicleWheel.RearRight) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function isInAnyGarage(Car As Vehicle) As Boolean
        Return NativeGarage.List.Any(Function(g) g.IsVehicleInside(Car))
    End Function
    Public Shared Function IsBig(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_BIG_VEHICLE")
    End Function
    Public Shared Function IsStopped(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_STOPPED", Car)
    End Function
    Private Shared Function EnsureVehicle(Car As Vehicle, caller As String) As Boolean
        If Car Is Nothing OrElse Not Car.Exists Then
            WriteLog($"Error in NativeVehicles.Doors.{caller}: vehicle not valid.")
            Return False
        End If
        Return True
    End Function
    Public Shared Sub ResetWheelsPosition(Car As Vehicle)
        [Call]("RESET_CAR_WHEELS", Car)
    End Sub
    Public Shared Function AreTaxiLightsOn(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("ARE_TAXI_LIGHTS_ON", Car)
    End Function
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
            If IsNotTimeTravel4Vehicle(Car) And Car.isRequiredForMission Then
                [Call]("SET_CAR_ENGINE_ON", Car, 0, 0)
            ElseIf IsNotTimeTravel4Vehicle(Car) = False Or Car.isRequiredForMission = False Then
                Car.PetrolTankHealth = -1000
                '[Call]("SET_PETROL_TANK_HEALTH", Car, -1000)
                [Call]("SET_CAR_ENGINE_ON", Car, 0, 0)
            End If
        End If
    End Sub
    Public Shared Sub SetProvideCover(Car As Vehicle, Enable As Boolean)
        [Call]("VEHICLE_DOES_PROVIDE_COVER", Car, Enable)
    End Sub
    Public Shared Sub SetTaxiLights(Car As Vehicle, TurnON As Boolean)
        [Call]("SET_TAXI_LIGHTS", Car, TurnON)
    End Sub
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
    Public Shared Sub SetHeadlightsMultiplier(Car As Vehicle, LightMultiplier As Single)
        [Call]("SET_CAR_LIGHT_MULTIPLIER", Car, LightMultiplier)
    End Sub
    Public Shared Sub SetGPSEnabled(Car As Vehicle, Enable As Boolean, Optional Voice? As GPSVoice = Nothing)
        If Car Is Nothing Then
            Tools.WriteLog("Error in NativeVehicles.SetGPSEnabled: vehicle not found or not valid.")
            Return
        End If
        If Voice.HasValue Then
            If Voice.Value <> GPSVoice.Female AndAlso Voice.Value <> GPSVoice.Male Then
                Tools.WriteLog("Error in NativeVehicles.SetGPSEnabled: Voice ID" & Voice.Value & " is not valid.")
                Return
            End If
        End If

        [Call]("ENABLE_GPS_IN_VEHICLE", Car, Enable)
        If Voice.HasValue Then
            [Call]("SET_GPS_VOICE_FOR_VEHICLE", Car, CInt(Voice.Value))
        End If
    End Sub
    Public Shared Sub SetSiren(Car As Vehicle, Enable As Boolean, Optional SirenWithoutDriver As Boolean? = Nothing)
        [Call]("SWITCH_CAR_SIREN", Car, Enable)
        If SirenWithoutDriver.HasValue Then [Call]("SET_SIREN_WITH_NO_DRIVER", Car, SirenWithoutDriver.Value)
    End Sub
    Public Shared Function IsSirenOn(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_CAR_SIREN_ON", Car)
    End Function
    Public Shared Sub SoundHorn(Car As Vehicle, Duration As Integer)
        [Call]("SOUND_CAR_HORN", Car, Duration)
    End Sub
    Public Shared Sub SpawnRandomCarParked(Position As Vector3, Radius As Single)
        Dim pos As Vector3 = Position
        [Call]("CREATE_RANDOM_CAR_FOR_CAR_PARK", pos.X, pos.Y, pos.Z, Radius)
    End Sub
    'public shared Sub StartFire(Car As Vehicle)
    '    [Call]("START_CAR_FIRE", Car)
    'End Sub
    'Public Shared Function SpawnVehicle(Model As String, Position As Vector3, Optional Visible As Boolean = True, Optional Collisions As Boolean = True,
    '                                    Optional MsgText As String = "", Optional MsgTime As Integer = 2000, Optional LeftUpMsgType As Boolean = False) As GTA.Vehicle
    '    [Call]("REQUEST_MODEL", NativeModel.getHashKey(Model))
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
    '    '[Call]("CREATE_CAR", NativeModel.getHashKey(Model), Position.X, Position.Y, Position.Z, pointer, 1)
    '    Dim SpawnedCar = World.CreateVehicle(NativeModel.getHashKey(Model), Position)
    '    '  SetCollision(pointer.Value, True)
    '    '  SpawnedCar.Visible = True
    '    ' Dim veh As Vehicle = pointer.Value
    '    ' veh.Visible = Visible
    '    Return SpawnedCar 'pointer.Value
    'End Function
    ''' <summary>
    ''' Forces car steer wheel (with animation on ped).
    ''' SteerAngle: -1.0 (Left) to 1.0 (Right)
    ''' </summary>
    Public Shared Sub ForceSteerWheel(Car As Vehicle, SteerAngle As Single)
        [Call]("SET_VEHICLE_STEER_BIAS", Car, SteerAngle)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub SetPetrolTankWeakpoint(Car As Vehicle, Optional Enabled As Boolean = True)
        [Call]("SET_PETROL_TANK_WEAKPOINT", Car, Enabled)
    End Sub
    Public Shared Sub SetStrongAxles(Car As Vehicle, Enabled As Boolean)
        [Call]("SET_VEH_HAS_STRONG_AXLES", Car, Enabled)
    End Sub
    Public Shared Sub SetStrong(Car As Vehicle, Enabled As Boolean)
        [Call]("SET_CAR_STRONG", Car, Enabled)
    End Sub
    ''' <summary>
    ''' Enables or disables vehicle extras.
    ''' You may specify only the extras you want to modify; all parameters are optional.
    ''' </summary>
    Public Shared Sub SetExtras(Car As Vehicle, Optional Extra0 As Boolean? = Nothing, Optional Extra1 As Boolean? = Nothing, Optional Extra2 As Boolean? = Nothing,
                                       Optional Extra3 As Boolean? = Nothing, Optional Extra4 As Boolean? = Nothing, Optional Extra5 As Boolean? = Nothing,
                                       Optional Extra6 As Boolean? = Nothing, Optional Extra7 As Boolean? = Nothing, Optional Extra8 As Boolean? = Nothing, Optional Extra9 As Boolean? = Nothing)
        ApplyExtra(Car, 0, Extra0)
        ApplyExtra(Car, 1, Extra1)
        ApplyExtra(Car, 2, Extra2)
        ApplyExtra(Car, 3, Extra3)
        ApplyExtra(Car, 4, Extra4)
        ApplyExtra(Car, 5, Extra5)
        ApplyExtra(Car, 6, Extra6)
        ApplyExtra(Car, 7, Extra7)
        ApplyExtra(Car, 8, Extra8)
        ApplyExtra(Car, 9, Extra9)
    End Sub
    Private Shared Sub ApplyExtra(Car As Vehicle, Index As Integer, Value As Boolean?)
        If Value.HasValue Then
            [Call]("TURN_OFF_VEHICLE_EXTRA", Car, Index, Not Value.Value)
        End If
    End Sub
    Public NotInheritable Class Door
        Private Sub New()
        End Sub
        Private Shared _controlledCloseVehicle As Vehicle
        Private Shared _controlledCloseDoor As GTA.VehicleDoor = CType(-1, GTA.VehicleDoor)
        Private Shared _controlledClosingActive As Boolean
        Public Shared Function AreLocked(Car As Vehicle) As DoorLock
            If Not EnsureVehicle(Car, NameOf(AreLocked)) Then Return DoorLock.None

            Try
                Dim nativeValue As Integer = [Call](Of Integer)("GET_CAR_DOOR_LOCK_STATUS", Car)
                Return FromNative(nativeValue)
            Catch ex As Exception
                WriteLog("Error in Doors.AreLocked.", ex)
                Return DoorLock.None
            End Try
        End Function
        Public Shared Sub Break(Car As Vehicle, Door As GTA.VehicleDoor, Optional InstantVanish As Boolean = False)
            [Call]("BREAK_CAR_DOOR", Car, Door, InstantVanish)
        End Sub
        ''' <summary>
        ''' Closes a vehicle door. Uses TickHelper to latch the door (needed for Hood and Trunk).
        ''' </summary>
        Public Shared Sub Close(Car As Vehicle, Door As GTA.VehicleDoor, Optional Instant As Boolean = False)
            If Car Is Nothing OrElse Not Car.Exists Then Return

            If Instant Then
                Try
                    [Call]("SHUT_CAR_DOOR", Car, Door)
                Catch
                End Try
                Return
            End If

            Dim key As New DoorKey With {.Vehicle = Car, .Door = Door}

            SyncLock _controlledDoors
                _controlledDoors(key) = New DoorCloseState With {.Vehicle = Car, .Door = Door}
            End SyncLock

            If Not _controlledTickActive Then
                _controlledTickActive = True
                TickHelper.Add_Internal(AddressOf CloseDoorsControlledTick)
            End If
        End Sub
        Public Shared Sub CloseAll(Car As Vehicle)
            [Call]("CLOSE_ALL_CAR_DOORS", Car)
        End Sub
        Public Shared Function GetAngle(Car As Vehicle, Door As GTA.VehicleDoor) As Single
            If Not EnsureVehicle(Car, NameOf(GetAngle)) Then Return 0.0F
            Try
                Dim p As New Native.Pointer(GetType(Single))
                [Call]("GET_DOOR_ANGLE_RATIO", Car, Door, p)
                Return p.Value
            Catch ex As Exception
                WriteLog("Error in Doors.GetAngle.", ex)
                Return 0.0F
            End Try
        End Function
        Public Shared Function IsDamaged(Car As Vehicle, Door As GTA.VehicleDoor) As Boolean
            Return [Call](Of Boolean)("IS_CAR_DOOR_DAMAGED", Car, Door)
        End Function
        ''' <summary>
        ''' Returns True if door angle > 0. Returns -404.0 if vehicle doesn't exists or is not valid.
        ''' </summary>
        Public Shared Function IsOpen(Car As Vehicle, Door As GTA.VehicleDoor) As Boolean
            If EnsureVehicle(Car, NameOf(IsOpen)) Then
                Return GetAngle(Car, Door) > 0.01F
            Else
                Return -404.0
            End If
        End Function
        Public Shared Function IsFullyOpen(Car As Vehicle, Door As GTA.VehicleDoor) As Boolean
            Return [Call](Of Boolean)("IS_CAR_DOOR_FULLY_OPEN", Car, Door)
        End Function
        Public Shared Sub Open(Car As Vehicle, Door As GTA.VehicleDoor)
            [Call]("OPEN_CAR_DOOR", Car, Door)
        End Sub
        Public Shared Sub Lock(Car As Vehicle, LockType As DoorLock)
            Try
                If Car IsNot Nothing AndAlso Car.Exists Then
                    [Call]("LOCK_CAR_DOORS", Car, LockType)
                Else
                    WriteLog("Error in NativeVehicles.LockDoors: vehicle not found or not valid.")
                End If
            Catch ex As Exception
                WriteLog("Error in NativeVehicles.LockDoors.", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Unlatches car door.
        ''' </summary>
        Public Shared Sub Unlatch(Car As Vehicle, Door As GTA.VehicleDoor)
            [Call]("SET_CAR_DOOR_LATCHED", Car, Door, False, True)
        End Sub
        Public Enum DoorLock
            None
            Unlocked
            Locked
            LockedForPlayer
            PlayerStuckInside
            PlayerCanBreakIn
        End Enum
        '==========================================================================================================================ù
        'Internals
        '===========
        Const BASE_CLOSE_FORCE As Single = 0.03F
        Private Structure DoorKey
            Public Vehicle As Vehicle
            Public Door As GTA.VehicleDoor

            Public Overrides Function Equals(obj As Object) As Boolean
                If Not TypeOf obj Is DoorKey Then Return False
                Dim other = CType(obj, DoorKey)
                Return Vehicle Is other.Vehicle AndAlso Door = other.Door
            End Function

            Public Overrides Function GetHashCode() As Integer
                Dim h1 = If(Vehicle IsNot Nothing, Vehicle.GetHashCode(), 0)
                Dim h2 = Door.GetHashCode()
                Return (h1 * 397) Xor h2
            End Function
        End Structure
        Private Structure DoorCloseState
            Public Vehicle As Vehicle
            Public Door As GTA.VehicleDoor
        End Structure
        Private Shared ReadOnly _controlledDoors As New Dictionary(Of DoorKey, DoorCloseState)
        Private Shared _controlledTickActive As Boolean
        Private Shared Sub CloseDoorsControlledTick()
            Try
                If _controlledDoors.Count = 0 Then
                    StopControlledTick()
                    Exit Sub
                End If

                Dim keysToRemove As New List(Of DoorKey)
                Dim coef As Single = Tools.GetCoefFPS(32)
                SyncLock _controlledDoors
                    For Each kvp In _controlledDoors
                        Dim key = kvp.Key
                        Dim state = kvp.Value
                        Dim veh = state.Vehicle

                        If veh Is Nothing OrElse Not veh.Exists Then
                            keysToRemove.Add(key)
                            Continue For
                        End If

                        Dim doorObj = veh.Door(state.Door)

                        If Not doorObj.isOpen Then
                            keysToRemove.Add(key)
                            Continue For
                        End If

                        [Call]("CONTROL_CAR_DOOR", veh, state.Door, False, 0.0F)

                        If doorObj.Angle <= 0.07F Then
                            [Call]("SHUT_CAR_DOOR", veh, state.Door)
                            keysToRemove.Add(key)
                        End If
                    Next

                    For Each k In keysToRemove
                        _controlledDoors.Remove(k)
                    Next
                End SyncLock

            Catch ex As Exception
                StopControlledTick()
                WriteLog("Error in Doors.Close (multi-vehicle controlled).", ex)
            End Try
        End Sub
        Private Shared Sub StopControlledTick()
            _controlledDoors.Clear()
            _controlledTickActive = False
            TickHelper.Remove_Internal(AddressOf CloseDoorsControlledTick)
        End Sub
        Friend Shared Function FromNative(value As Integer) As DoorLock
            Dim mapped As DoorLock
            Return If(NativeToManaged.TryGetValue(value, mapped), mapped, DoorLock.None)
        End Function
        Friend Shared Function ToNative(value As DoorLock) As Integer
            Dim mapped As Integer
            Return If(ManagedToNative.TryGetValue(value, mapped), mapped, 0)
        End Function
        Friend Shared ReadOnly NativeToManaged As IReadOnlyDictionary(Of Integer, DoorLock) =
        New Dictionary(Of Integer, DoorLock) From {
            {0, DoorLock.None},
            {1, DoorLock.Unlocked},
            {2, DoorLock.Locked},
            {3, DoorLock.LockedForPlayer},
            {4, DoorLock.PlayerStuckInside},
            {7, DoorLock.PlayerCanBreakIn}
        }
        Friend Shared ReadOnly ManagedToNative As IReadOnlyDictionary(Of DoorLock, Integer) =
            New Dictionary(Of DoorLock, Integer) From {
                {DoorLock.None, 0},
                {DoorLock.Unlocked, 1},
                {DoorLock.Locked, 2},
                {DoorLock.LockedForPlayer, 3},
                {DoorLock.PlayerStuckInside, 4},
                {DoorLock.PlayerCanBreakIn, 7}
            }
        '==========================================================================================================================
    End Class
    Public NotInheritable Class StuckCheck
        Private Sub New()
        End Sub
        ''' <summary>
        ''' Untested.
        ''' </summary>
        Public Shared Sub AddStuckCheck(Car As Vehicle, Optional Duration As Integer = 10000, Optional RestoreIfStuck As Boolean = True,
                                        Optional RestoreIfUpsideDown As Boolean = True, Optional Unknown As Boolean = True, Optional PathID As Integer = -1, Optional Distance As Single = 1.5)
            If HasStuckCheck(Car) Then Exit Sub

            If RestoreIfStuck Or RestoreIfUpsideDown Or Unknown Then
                [Call]("ADD_STUCK_CAR_CHECK_WITH_WARP", Car, Duration, Distance)
            Else
                [Call]("ADD_STUCK_CAR_CHECK", Car, Duration, Distance, RestoreIfStuck, RestoreIfUpsideDown, Unknown, PathID)
            End If
        End Sub
        ''' <summary>
        ''' Untested.
        ''' </summary>
        Public Shared Sub AddUpsideDownStuckCheck(Car As Vehicle)
            [Call]("ADD_UPSIDEDOWN_CAR_CHECK", Car)
        End Sub
        ''' <summary>
        ''' Untested.
        ''' </summary>
        ''' <param name="Flag">Values between 0 and 3</param>
        ''' <param name="ElapsedTime">Duration in Ms or zero</param>
        Public Shared Function CheckStuckTimer(Car As Vehicle, Flag As Integer, ElapsedTime As Integer) As Boolean
            Return [Call](Of Boolean)("CHECK_STUCK_TIMER", Car, Flag, ElapsedTime)
        End Function
        ''' <summary>
        ''' Untested.
        ''' </summary>
        Public Shared Function HasStuckCheck(Car As Vehicle) As Boolean
            Return [Call](Of Boolean)("DOES_CAR_HAVE_STUCK_CAR_CHECK", Car)
        End Function
        Public Shared Function IsStuckOnRoof(Car As Vehicle) As Boolean
            Return [Call](Of Boolean)("IS_CAR_STUCK_ON_ROOF", Car)
        End Function
        ''' <summary>
        ''' Untested.
        ''' </summary>
        ''' <param name="Flag">Values between 0 and 3</param>
        Public Shared Sub ResetStuckTimer(Car As Vehicle, Flag As Integer)
            [Call]("RESET_STUCK_TIMER", Car, Flag)
        End Sub
        ''' <summary>
        ''' Untested.
        ''' </summary>
        Public Shared Sub RemoveStuckTimer(Car As Vehicle)
            [Call]("REMOVE_STUCK_TIMER", Car)
        End Sub
    End Class
    Public NotInheritable Class Window
        Private Sub New() : End Sub
        Public Shared Sub Break(Car As Vehicle, Window As GTA.VehicleWindow, Optional InstantVanish As Boolean = False)
            If InstantVanish Then [Call]("SMASH_CAR_WINDOW", Car, Window) Else [Call]("REMOVE_CAR_WINDOW", Car, Window)
        End Sub
        Public Shared Function IsIntact(Car As Vehicle, Window As GTA.VehicleWindow) As Boolean
            Return [Call](Of Boolean)("IS_VEH_WINDOW_INTACT", Car, Window)
        End Function
    End Class
    Public NotInheritable Class IDE
        Private Sub New() : End Sub

        Private Shared ReadOnly _modelCache As New Dictionary(Of Integer, VehicleData)
        Private Shared _initialized As Boolean = False
        Private Shared ReadOnly _initializationLock As New Object()
        ''' <summary>
        ''' Contains all values contained in a GTA IV vehicles.ide "cars" row.
        ''' All original IDE values are preserved as strings.
        ''' </summary>
        Public Class VehicleData
            Public Property Model As String
            Public Property TXD As String
            Public Property Type As String
            Public Property HandlingID As String
            Public Property GameName As String
            Public Property Anims1 As String
            Public Property Anims2 As String
            Public Property Frequency As String
            Public Property MaxNum As String
            Public Property WheelRadius_Front As String
            Public Property WheelRadius_Rear As String
            Public Property DefaultDirtness As String
            Public Property Swakness As String
            Public Property LOD_Multiplier As String
            Public Property Unknown As String
            Public Property Flags As String
            Public Property ModelHash As Integer
            ''' <summary>
            ''' IDE file from which this definition was loaded.
            ''' </summary>
            Public Property SourceFile As String
            ''' <summary>
            ''' Returns the flags as an array.
            ''' </summary>
            Public Function GetFlags() As String()
                If String.IsNullOrEmpty(Flags) OrElse Flags = "-" Then
                    Return New String() {}
                End If
                Return Flags.Split("+"c)
            End Function
        End Class
        ''' <summary>
        ''' Vehicle IDE flags.
        ''' </summary>
        Public Structure Flags
            ''' <summary>Vehicle has van style rear doors</summary>
            Public Const Van = "isvan"

            ''' <summary>If the car has multiple different textures, it will randomize between them</summary>
            Public Const Livery = "livery"

            ''' <summary>You won't see the car parked at the side of the road with a ped looking inside the trunk/hood</summary>
            Public Const NoBoot = "noboot"

            ''' <summary>The car will only spawn during office hours and not at night</summary>
            Public Const OnlyDuringOfficeHours = "onlyduringofficehours"

            ''' <summary>Truck engine</summary>
            Public Const TruckEngineFx = "truckenginefx"

            ''' <summary>More dust when changing gears</summary>
            Public Const TruckExhaustFx = "truckexhaustfx"

            ''' <summary>Larger flames when vehicle catches fire</summary>
            Public Const TruckFireFx = "truckfirefx"

            ''' <summary>If the car is big, like bus, it will avoid sharp turns</summary>
            Public Const AvoidSharpTurns = "avoidturns"

            ''' <summary>Dirty smoke</summary>
            Public Const DirtyExhaustFx = "bangerexhaustfx"

            ''' <summary>The car is big</summary>
            Public Const Big = "big"

            ''' <summary>It's a delivery car so only spawns in the daytime</summary>
            Public Const Delivery = "delivery"

            ''' <summary>Spawns with all extras</summary>
            Public Const Extra_All = "ext_all"

            ''' <summary>Convertible, driver can enter without opening door</summary>
            Public Const Extra_Covertable = "ext_conv"

            ''' <summary>The car will be spawned with all extras from 5 to 9</summary>
            Public Const Extra_AllFrom5To9 = "ext_gang"

            ''' <summary>Spawning with extras will be rare</summary>
            Public Const Extra_Rare = "ext_rare"

            ''' <summary>Will always spawn with an extra between 0 and 4</summary>
            Public Const Extra_OneFrom0_To_4 = "ext_requi"

            ''' <summary>Extras won't come off easily when crashing</summary>
            Public Const Extra_Strong = "ext_strong"

            ''' <summary>One or more extras are a sunroof</summary>
            Public Const Extra_Sunroof = "ext_sunroof"

            ''' <summary>Will always spawn with an extra between 5 and 9</summary>
            Public Const Extra_OneFrom5_To_9 = "ext_taxi"

            ''' <summary>Extra decorations activated by external script</summary>
            Public Const Extra_Script = "ext_script"

            ''' <summary>Used only on forklift</summary>
            Public Const SmallWorker = "smallworker"

            ''' <summary>Unknown behavior</summary>
            Public Const Sports = "sports"

        End Structure
        Private Shared Sub EnsureInitialized()
            If _initialized Then
                Return
            End If

            SyncLock _initializationLock
                If _initialized Then
                    Return
                End If
                Try
                    Initialize()
                    _initialized = True
                Catch ex As Exception
                    Tools.WriteLog("Error in IDE.Initialize.", ex)
                    _initialized = True
                End Try
            End SyncLock
        End Sub
        Private Shared Sub Initialize()
            Dim dataPath As String = Path.Combine(GamePath, "common", "data")

            If Not Directory.Exists(dataPath) Then
                Tools.WriteLog("Error in IDE.Initialize: common\data directory not found: " & dataPath)
                Return
            End If

            Dim ideFiles() As String
            Try
                ideFiles = Directory.GetFiles(dataPath, "*.ide", SearchOption.TopDirectoryOnly)
            Catch ex As Exception
                Tools.WriteLog("Error in IDE.Initialize: cannot scan IDE files in '" & dataPath & "'.", ex)
                Return
            End Try

            For Each filePath As String In ideFiles
                Try
                    ParseVehicleFile(filePath)
                Catch ex As Exception
                    Tools.WriteLog("Error in IDE.Initialize: cannot parse file '" & filePath & "'.", ex)
                End Try
            Next
        End Sub
        Private Shared Sub ParseVehicleFile(filePath As String)
            Dim inCarsSection As Boolean = False

            Try
                For Each line As String In File.ReadLines(filePath, Encoding.UTF8)
                    If String.IsNullOrWhiteSpace(line) Then
                        Continue For
                    End If

                    Dim cleanLine As String = line.Trim()
                    If cleanLine.StartsWith("#", StringComparison.Ordinal) Then
                        Continue For
                    End If

                    Dim commentIndex As Integer = cleanLine.IndexOf("#"c)
                    If commentIndex >= 0 Then
                        cleanLine = cleanLine.Substring(0, commentIndex).Trim()
                    End If

                    If cleanLine.Length = 0 Then
                        Continue For
                    End If

                    'Cars section
                    If String.Equals(cleanLine, "cars", StringComparison.OrdinalIgnoreCase) Then
                        inCarsSection = True
                        Continue For
                    End If
                    If String.Equals(cleanLine, "end", StringComparison.OrdinalIgnoreCase) Then
                        If inCarsSection Then
                            inCarsSection = False
                        End If
                        Continue For
                    End If
                    If Not inCarsSection Then
                        Continue For
                    End If

                    'Lines
                    If Not cleanLine.Contains(","c) Then
                        Continue For
                    End If

                    Dim fields As String() = ParseIdeLine(cleanLine)
                    If fields Is Nothing OrElse fields.Length < 16 Then

                        Tools.WriteLog("Error in IDE.ParseVehicleFile: invalid vehicle row in file '" & filePath & "'. Line='" & cleanLine & "' | Fields=" &
                                       If(fields Is Nothing, "Nothing", fields.Length.ToString()))
                        Continue For
                    End If

                    Dim modelName As String = fields(0)
                    If String.IsNullOrWhiteSpace(modelName) Then
                        Tools.WriteLog("Error in IDE.ParseVehicleFile: empty vehicle model in file '" & filePath & "'.")
                        Continue For
                    End If

                    Dim modelHash As Integer
                    Try
                        modelHash = NativeModel.GetHashKey(modelName)
                    Catch ex As Exception
                        Tools.WriteLog("Error in IDE.ParseVehicleFile: cannot obtain hash for model '" & modelName &
                                               "' in file '" & filePath & "'.", ex)
                        Continue For
                    End Try

                    If modelHash = 0 Then
                        Tools.WriteLog("Error in IDE.ParseVehicleFile: GET_HASH_KEY returned 0 for model '" &
                                       modelName & "' in file '" & filePath & "'.")
                        Continue For
                    End If

                    Dim flags As String = fields(15)

                    If String.Equals(flags, "-", StringComparison.Ordinal) Then
                        flags = String.Empty
                    End If

                    Dim vehicle As New VehicleData With {.Model = fields(0),
                                                         .TXD = fields(1),
                                                         .Type = fields(2),
                                                         .HandlingID = fields(3),
                                                         .GameName = fields(4),
                                                         .Anims1 = fields(5),
                                                         .Anims2 = fields(6),
                                                         .Frequency = fields(7),
                                                         .MaxNum = fields(8),
                                                         .WheelRadius_Front = fields(9),
                                                         .WheelRadius_Rear = fields(10),
                                                         .DefaultDirtness = fields(11),
                                                         .Swakness = fields(12),
                                                         .LOD_Multiplier = fields(13),
                                                         .Unknown = fields(14),
                                                         .Flags = flags,
                                                         .ModelHash = modelHash,
                                                         .SourceFile = filePath}


                    If _modelCache.ContainsKey(modelHash) Then
                        Tools.WriteLog("Error in IDE.ParseVehicleFile: duplicate model hash " & modelHash.ToString() &
                                       " for model '" & modelName & "' in file '" & filePath & "'. Existing definition was preserved.")
                        Continue For
                    End If
                    _modelCache.Add(modelHash, vehicle)
                Next
            Catch ex As Exception
                Tools.WriteLog("Error in IDE.ParseVehicleFile: cannot read file '" & filePath & "'.", ex)
            End Try
        End Sub
        Private Shared Function ParseIdeLine(line As String) As String()

            If String.IsNullOrWhiteSpace(line) Then
                Return Nothing
            End If

            Dim clean As String = line.Split("#"c)(0).Trim()

            If clean.Length = 0 Then
                Return Nothing
            End If

            Dim rawFields As String() = clean.Split(","c)

            If rawFields.Length < 16 Then
                Return Nothing
            End If

            Dim fields(rawFields.Length - 1) As String

            For i As Integer = 0 To rawFields.Length - 1
                Dim value As String = rawFields(i).Trim()
                value = value.Replace(" ", "")
                fields(i) = value
            Next
            Return fields
        End Function
        ''' <summary>
        ''' Gets the complete VehicleData associated with a GTA IV model hash.
        ''' </summary>
        Public Shared Function GetVehicleData(ModelHash As Integer) As VehicleData
            EnsureInitialized()

            If ModelHash = 0 Then
                Tools.WriteLog("Error in IDE.GetVehicleData: invalid model hash (0).")
                Return Nothing
            End If

            Dim vehicle As VehicleData = Nothing
            If _modelCache.TryGetValue(ModelHash, vehicle) Then
                Return vehicle
            End If

            Tools.WriteLog("Error in IDE.GetVehicleData: vehicle not found for model hash " & ModelHash.ToString() & ".")
            Return Nothing
        End Function
        ''' <summary>
        ''' Gets the complete VehicleData associated with a GTA IV vehicle.
        ''' </summary>
        Public Shared Function GetVehicleData(Vehicle As Vehicle) As VehicleData
            Try
                If Vehicle Is Nothing Then
                    Tools.WriteLog("Error in IDE.GetVehicleData: vehicle is Nothing.")
                    Return Nothing
                End If

                Dim pointer1 As New Native.Pointer(GetType(Integer))

                [Call]("GET_CAR_MODEL", Vehicle, pointer1)
                Dim modelHash As Integer = pointer1.Value
                If modelHash = 0 Then
                    Tools.WriteLog("Error in IDE.GetVehicleData: GET_CAR_MODEL returned an invalid model hash (0).")
                    Return Nothing
                End If
                Return GetVehicleData(modelHash)
            Catch ex As Exception
                Tools.WriteLog("Error in IDE.GetVehicleData.", ex)
                Return Nothing
            End Try
        End Function
        Public Shared Function GetFlags(VehicleModel As String) As String()
            Try
                If String.IsNullOrWhiteSpace(VehicleModel) Then
                    Tools.WriteLog("Error in IDE.GetFlags: VehicleModel is Nothing or empty.")
                    Return New String() {}
                End If

                Dim modelHash As Integer
                Try
                    modelHash = NativeModel.GetHashKey(VehicleModel)
                Catch ex As Exception
                    Tools.WriteLog("Error in IDE.GetFlags: cannot obtain hash for model '" & VehicleModel & "'.", ex)
                    Return New String() {}
                End Try

                If modelHash = 0 Then
                    Tools.WriteLog("Error in IDE.GetFlags: GET_HASH_KEY returned 0 for model '" & VehicleModel & "'.")
                    Return New String() {}
                End If

                Dim vehicle As VehicleData = GetVehicleData(modelHash)

                If vehicle Is Nothing Then
                    Tools.WriteLog("Error in IDE.GetFlags: vehicle not found for model '" & VehicleModel & "' (Hash " & modelHash.ToString() & ").")
                    Return New String() {}
                End If
                Return vehicle.GetFlags()
            Catch ex As Exception
                Tools.WriteLog("Error in IDE.GetFlags for model '" & VehicleModel & "'.", ex)
                Return New String() {}
            End Try
        End Function
        Public Shared Function GetFlags(ModelHash As Integer) As String()
            Try
                If ModelHash = 0 Then
                    Tools.WriteLog("Error in IDE.GetFlags: invalid model hash (0).")
                    Return New String() {}
                End If

                Dim vehicle As VehicleData = GetVehicleData(ModelHash)

                If vehicle Is Nothing Then
                    Tools.WriteLog("Error in IDE.GetFlags: vehicle not found for model hash " & ModelHash.ToString() & ".")
                    Return New String() {}
                End If
                Return vehicle.GetFlags()
            Catch ex As Exception
                Tools.WriteLog("Error in IDE.GetFlags for model hash " & ModelHash.ToString() & ".", ex)
                Return New String() {}
            End Try
        End Function
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
    '        If Not [Call](Of Boolean)("HAS_MODEL_LOADED", NativeModel.getHashKey(_tempRequestedModel)) Then
    '            [Call]("REQUEST_MODEL", NativeModel.getHashKey(_tempRequestedModel))
    '        Else
    '            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
    '            [Call]("CREATE_CAR", NativeModel.getHashKey(_tempRequestedModel), _tempSpawnedVehPos.X, _tempSpawnedVehPos.Y, _tempSpawnedVehPos.Z, pointer, 1)
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
