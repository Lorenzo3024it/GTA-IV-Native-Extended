Imports System.Security.Policy
Imports System.Threading
Imports System.Windows.Forms
Imports GTA
Imports GTA.base
'Imports GTA.Math
Imports GTA.Native.Function
Imports GTA.value
Imports NativeExtended.NativePlayer
'Imports NativeExtended.NativeGarage
Public NotInheritable Class NativeWorld
    Private Sub New() : End Sub
    Public Enum Islands
        Broker = 0
        Bohan = 1
        Algonquin = 2
        Alderney = 3
    End Enum
    Public Enum WeatherType
        Random = -1
        ExtraSunny = 0
        Sunny = 1
        SunnyAndWindy = 2
        Cloudy = 3
        Raining = 4
        Drizzle = 5
        Foggy = 6
        Thunderstorm = 7
        ExtraSunny2 = 8
        SunnyAndWindy2 = 9
    End Enum
    ''' <summary>
    ''' Untested. Adds a permanent stunt jump.
    ''' </summary>
    Public Shared Sub AddStuntJump(MinStartPosition As Vector3, MaxStartPosition As Vector3, MinEndPosition As Vector3, MaxEndPosition As Vector3, CameraPosition As Vector3, Optional CashReward As Integer = 5000)
        [Call]("ADD_STUNT_JUMP", MinStartPosition.X, MinStartPosition.Y, MinStartPosition.Z, MaxStartPosition.X, MaxStartPosition.Y, MaxStartPosition.Z, MinEndPosition.X, MinEndPosition.Y, MinEndPosition.Z, MaxEndPosition.X, MaxEndPosition.Y, MaxEndPosition.Z, CameraPosition.X, CameraPosition.Y, CameraPosition.Z, CashReward)
    End Sub
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared WriteOnly Property AllowStuntJumps As Boolean
        Set(value As Boolean)
            [Call]("ALLOW_STUNT_JUMPS_TO_TRIGGER", value)
        End Set
    End Property
    '''' <summary>
    '''' Untested.
    '''' </summary>
    'Public Shared Sub EnableSaveHouse(SavehouseID As UInteger, Optional Enable As Boolean = True)
    '    [Call]("ENABLE_SAVE_HOUSE", SavehouseID, Enable)
    'End Sub
    '''' <summary>
    '''' Untested.
    '''' </summary>
    'Public Shared WriteOnly Property FancyWater As Boolean
    '    Set(value As Boolean)
    '        [Call]("ENABLE_FANCY_WATER", value)
    '    End Set
    'End Property
    ''' <summary>
    ''' Untested.
    ''' </summary>
    ''' Values from -1.0 to 1.0
    Public Shared WriteOnly Property ForceWind As Single
        Set(value As Single)
            [Call]("FORCE_WIND", Tools.ClampFloat(CSng(value), -1.0F, 1.0F))
        End Set
    End Property
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared WriteOnly Property FreeHealthCare As Boolean
        Set(value As Boolean)
            [Call]("SET_FREE_HEALTH_CARE", NativePlayer.Index, value)
        End Set
    End Property

    Public Shared Function GetDistance(Position1 As Vector3, Position2 As Vector3) As Single
        Dim p As New Native.Pointer(GetType(Single))
        [Call]("GET_DISTANCE_BETWEEN_COORDS_3D", Position1.X, Position1.Y, Position1.Z, Position2.X, Position2.Y, Position2.Z, p)
        Return p.Value
    End Function

    Public Shared Function GetWaterHeight(Position As Vector3, Optional NoWaves As Boolean = False) As Single
        Dim tFloat As Native.Pointer = New Native.Pointer(GetType(Single))
        If NoWaves Then [Call]("GET_WATER_HEIGHT_NO_WAVES", Position.X, Position.Y, Position.Z, tFloat) Else [Call]("GET_WATER_HEIGHT", Position.X, Position.Y, Position.Z, tFloat)
        Return tFloat.Value
    End Function
    ''' <summary>
    ''' Mute world audio (not pause menu). Uses MUTE_GAMEWORLD_AND_POSITIONED_RADIO_FOR_TV.
    ''' </summary>
    Public Shared WriteOnly Property MuteGameWorld As Boolean
        Set(value As Boolean)
            [Call]("MUTE_GAMEWORLD_AND_POSITIONED_RADIO_FOR_TV", value)
        End Set
    End Property
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared WriteOnly Property MuteWorldRadios As Boolean
        Set(value As Boolean)
            [Call]("MUTE_POSITIONED_RADIO", value)
        End Set
    End Property
    ''' <summary>
    ''' Not fullu tested. Seems not working.
    ''' </summary>
    Public Shared WriteOnly Property MuteRomansGarageRadio As Boolean
        Set(value As Boolean)
            [Call]("SET_TAXI_GARAGE_RADIO_STATE", Not value)
        End Set
    End Property
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared Sub SwapBuildingModel(Position As Vector3, Unknown1 As Single, OldModel As String, NewModel As String)
        [Call]("SWAP_NEAREST_BUILDING_MODEL", Position.X, Position.Y, Position.Z, Unknown1, NativeModel.GetHashKey(OldModel), NativeModel.GetHashKey(NewModel))
    End Sub

    Public NotInheritable Class Area
        Private Sub New() : End Sub
        ''' <summary>
        ''' Untested. Can be reset using RemoveCoverBlocks.
        ''' </summary>
        Public Shared Sub AddCoverBlock(MinCoords As Vector3, MaxCoords As Vector3, Optional BlockPeds As Boolean = True, Optional BlockVehicles As Boolean = True, Optional BlockObjects As Boolean = True)
            [Call]("ADD_COVER_BLOCKING_AREA", MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z, BlockPeds, BlockVehicles, BlockObjects)
        End Sub
        ''' <summary>
        ''' Untested. Can be reset using RemovePedRemovalBlocks.
        ''' </summary>
        Public Shared Sub AddPedRemovalBlock(MinCoords As GTA.Vector3, MaxCoords As GTA.Vector3)
            [Call]("SET_PED_NON_REMOVAL_AREA", MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z)
        End Sub
        ''' <summary>
        ''' Untested. Can be reset using RemoveSpawnBlocks.
        ''' </summary>
        ''' <param name="Range">Not sure about this.</param>
        Public Shared Sub AddSpawnBlock(Coordinate As GTA.Vector3, Range As Single)
            [Call]("ADD_SPAWN_BLOCKING_AREA", Coordinate.X, Coordinate.Y, Coordinate.Z, Range)
        End Sub
        ''' <summary>
        ''' Untested. Can be reset using RemovePedSpawnBlocks.
        ''' </summary>
        Public Shared Sub AddPedSpawnBlock(MinCoords As GTA.Vector3, MaxCoords As GTA.Vector3)
            [Call]("SET_PED_NON_CREATION_AREA", MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z)
        End Sub
        ''' <summary>
        ''' Clears area using many native function. Optionally clears only certain type of objects.
        ''' </summary>
        ''' <param name="Position"></param>
        ''' <param name="Radius"></param>
        ''' <param name="ClearPeds"></param>
        ''' <param name="ClearCars"></param>
        ''' <param name="ClearCops"></param>
        ''' <param name="ClearObjects"></param>
        Public Shared Sub Clear(Position As Vector3, Optional Radius As Single = 100.0F, Optional ClearPeds As Boolean? = Nothing, Optional ClearCars As Boolean? = Nothing, Optional ClearCops As Boolean? = Nothing, Optional ClearObjects As Boolean? = Nothing)
            If Not ClearPeds.HasValue AndAlso Not ClearCars.HasValue AndAlso Not ClearCops.HasValue AndAlso Not ClearObjects.HasValue Then
                [Call]("CLEAR_AREA", Position.X, Position.Y, Position.Z, CSng(Radius), True)
                Exit Sub
            End If
            '---
            If ClearPeds.HasValue AndAlso ClearPeds.Value Then
                [Call]("CLEAR_AREA_OF_CHARS", Position.X, Position.Y, Position.Z, CSng(Radius))
            End If
            If ClearCars.HasValue AndAlso ClearCars.Value Then
                [Call]("CLEAR_AREA_OF_CARS", Position.X, Position.Y, Position.Z, CSng(Radius))
            End If
            If ClearCops.HasValue AndAlso ClearCops.Value Then
                [Call]("CLEAR_AREA_OF_COPS", Position.X, Position.Y, Position.Z, CSng(Radius))
            End If
            If ClearObjects.HasValue AndAlso ClearObjects.Value Then
                [Call]("CLEAR_AREA_OF_OBJECTS", Position.X, Position.Y, Position.Z, CSng(Radius))
            End If
        End Sub
        Public Shared Function GetIslandID(Position As Vector3) As Islands
            Return [Call](Of Integer)("GET_MAP_AREA_FROM_COORDS", Position.X, Position.Y, Position.Z)
        End Function
        Public Shared Function IsOccupied(MinCoords As Vector3, MaxCoords As Vector3, Optional CheckPeds As Boolean = False, Optional CheckVehicles As Boolean = True,
                                              Optional CheckObjects As Boolean = False, Optional CheckProjectiles As Boolean = False) As Boolean
            Try
                Return [Call](Of Boolean)("IS_AREA_OCCUPIED", MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z, CheckPeds, CheckVehicles, CheckObjects, CheckProjectiles, 0)
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.IsAreaOccupied.", ex)
                Return False
            End Try
        End Function
        ''' <summary>
        ''' Untested. Removes blocks added with AddCoverBlock.
        ''' </summary>
        Public Shared Sub RemoveCoverBlocks()
            [Call]("FLUSH_COVER_BLOCKING_AREAS")
        End Sub
        ''' <summary>
        ''' Untested. Removes blocks added with AddPedRemovalBlock.
        ''' </summary>
        Public Shared Sub RemovePedRemovalBlocks()
            [Call]("CLEAR_PED_NON_REMOVAL_AREA")
        End Sub
        ''' <summary>
        ''' Untested. Removes blocks added with AddSpawnBlock.
        ''' </summary>
        Public Shared Sub RemoveSpawnBlocks()
            [Call]("FLUSH_ALL_SPAWN_BLOCKING_AREAS")
        End Sub
        ''' <summary>
        ''' Untested. Removes blocks added with AddPedSpawnBlock.
        ''' </summary>
        Public Shared Sub RemovePedSpawnBlocks()
            [Call]("CLEAR_PED_NON_CREATION_AREA")
        End Sub
        ''' <summary>
        ''' Creates a Cover Point istance using a native handle. Not fully tested.
        ''' </summary>
        Public NotInheritable Class CoverPoint
            Private _handle As Integer
            Private _isCreated As Boolean
            Private _Pos As Vector3
            Private _Head As Single
            Private _Unkn1 As Integer
            Private _Unkn2 As Integer
            Private _Unkn3 As Integer
            Public Sub New()
                ResetState()
            End Sub

            Public ReadOnly Property Handle As Integer
                Get
                    Return _handle
                End Get
            End Property

            Public ReadOnly Property Position As Vector3
                Get
                    Return _Pos
                End Get
            End Property

            Public ReadOnly Property Heading As Single
                Get
                    Return _Head
                End Get
            End Property

            Public ReadOnly Property Unknown1 As Integer
                Get
                    Return _Unkn1
                End Get
            End Property

            Public ReadOnly Property Unknown2 As Integer
                Get
                    Return _Unkn2
                End Get
            End Property

            Public ReadOnly Property Unknown3 As Integer
                Get
                    Return _Unkn3
                End Get
            End Property
            Public ReadOnly Property Exists As Boolean
                Get
                    Return _isCreated AndAlso _handle <> 0
                End Get
            End Property
            Public Sub Create(Position As Vector3, Heading As Single, Unknown1 As Integer, Unknown2 As Integer, Unknown3 As Integer)
                If Exists Then
                    Throw New InvalidOperationException(
                        "Il cover point esiste già. Eliminalo prima di crearne un altro.")
                End If

                Try
                    Dim Pointer As Native.Pointer = New Native.Pointer(GetType(Integer))

                    [Call](Of Integer)("ADD_COVER_POINT", Position.X, Position.Y, Position.Z, Unknown1, Heading, Unknown2, Unknown3, Pointer)

                    Dim NewHandle As Integer = Pointer.Value
                    If NewHandle = 0 Then
                        Throw New InvalidOperationException("ADD_COVER_POINT non ha restituito un handle valido.")
                    End If

                    _handle = NewHandle
                    _Pos = Position
                    _Head = Heading
                    _Unkn1 = Unknown1
                    _Unkn2 = Unknown2
                    _Unkn3 = Unknown3
                    _isCreated = True
                Catch ex As Exception
                    ResetState()
                    Try
                        Tools.WriteLog("Error in CoverPoint " & _handle & ": failed to create cover point.", ex)
                    Catch
                    End Try
                    Throw
                End Try
            End Sub
            Public Sub Delete()
                If Not Exists Then
                    ResetState()
                    Return
                End If

                Dim OldHandle As Integer = _handle
                Try
                    [Call]("REMOVE_COVER_POINT", OldHandle)
                    ResetState()
                Catch ex As Exception
                    Try
                        Tools.WriteLog("Error in CoverPoint " & OldHandle & ": failed to remove cover point.", ex)
                    Catch
                    End Try
                    Throw
                End Try
            End Sub
            Private Sub ResetState()
                _handle = 0
                _isCreated = False
                _Pos = New Vector3(0, 0, 0)
                _Head = 0.0F
                _Unkn1 = 0
                _Unkn2 = 0
                _Unkn3 = 0
            End Sub
            Public Sub TaskPedSeekCoverToCoverPoint(Ped As Ped, Position As Vector3, Optional Duration As Integer = -1)
                NativePed.Task.SeekCoverToCoverPoint(Ped, Me, Position, Duration)
            End Sub
        End Class
    End Class
    ''' <summary>
    ''' Provides static helpers to control in-game date, time, and calendar-related features.
    ''' </summary>
    Public NotInheritable Class Calendar
        Private Sub New() : End Sub
        Private Shared pointerDay, pointerMonth As New Native.Pointer(GetType(Integer))
        Private Shared pointerHour, pointerMinute As New Native.Pointer(GetType(Integer))
        Private Shared _timeAnimActive As Boolean
        Private Shared _timeAnimTargetMinutes As Integer
        Private Shared _timeAnimStartMinutes As Integer
        Private Shared _timeAnimTotalMinutes As Integer
        Private Shared _timeAnimStartGameTime As Integer
        Private Const _timeAnimAccelDuration As Integer = 1500
        Private Shared _timeAnimCruiseSpeed As Integer = 6 'minutes per tick
        '---
        Private Const _timeScaleDefault As Single = 1.0F
        Private Shared _timeAnimAffectTimeScale As Boolean
        '---
        Private Shared _minuteDurationMs As Integer = 2000 ' default: 2 seconds
        Private Shared _minuteAccumulatorMs As Long
        Private Shared _lastRealTick As Long
        Private Shared _minuteTickActive As Boolean
        Private Shared _minuteControlUsesFreeze As Boolean = True
        Private Shared _timeDriver As TimeDriver = TimeDriver.None
        Private Shared _timeAnimStartRealTick As Long
        ' ======================
        Private Shared _startingDate As (Day As Integer, Month As Month, Year As Integer) = (Day:=29, Month:=Month.April, Year:=2008)

        ''' <summary>
        ''' Advances the in-game time to a target hour and minute.
        ''' Can be animated using TickHelper.
        ''' </summary>
        Public Shared Sub AdvanceTime(Optional TargetTime As (Hour As Integer, Minute As Integer)? = Nothing, Optional Animated As Boolean = True, Optional AffectTimeScale As Boolean = True)
            _timeAnimStartRealTick = Environment.TickCount
            Dim current = Time
            Dim currentTotalMinutes As Integer = current.Hour * 60 + current.Minute
            Dim targetTotalMinutes As Integer
            If TargetTime.HasValue Then
                targetTotalMinutes = TargetTime.Value.Hour * 60 + TargetTime.Value.Minute
            Else
                targetTotalMinutes = (current.Hour * 60 + current.Minute + 60) Mod 1440
            End If
            ' Handle wrap to next day
            If targetTotalMinutes <= currentTotalMinutes Then
                targetTotalMinutes += 1440
            End If
            If Not Animated Then
                Dim delta As Integer = targetTotalMinutes - currentTotalMinutes
                For i = 1 To delta
                    AdvanceOneMinute()
                Next
                Return
            End If
            If TickHelper.ContainsHandler_Internal(AddressOf TimeAdvanceTick) Then
                TickHelper.Remove_Internal(AddressOf TimeAdvanceTick)
            End If
            _timeDriver = TimeDriver.AdvanceAnimation
            '---
            _minuteTickActive = False
            TickHelper.Remove_Internal(AddressOf MinuteControlTick)
            Dim t = Time
            [Call]("RELEASE_TIME_OF_DAY")
            [Call]("SET_TIME_OF_DAY", t.Hour, t.Minute)
            '------------------------------------
            _timeAnimActive = True
            _timeAnimStartMinutes = currentTotalMinutes
            _timeAnimTargetMinutes = targetTotalMinutes
            _timeAnimTotalMinutes = targetTotalMinutes - currentTotalMinutes
            '  _timeAnimStartGameTime = NativeTimer.TotalGameTime
            '---
            _timeAnimAffectTimeScale = AffectTimeScale
            If _timeAnimAffectTimeScale Then
                [Call]("SET_TIME_SCALE", 3.0F)
            End If
            '---
            TickHelper.Add_Internal(AddressOf TimeAdvanceTick)
        End Sub
        '''' <summary>
        '''' Gets the simulated in-game date (day, month, year),
        '''' based on StartingDate + days elapsed.
        '''' </summary>
        'Public Shared ReadOnly Property [Date] As (Day As Integer, Month As Month, Year As Integer)
        '    Get
        '        Dim d = GetSimulatedDate()
        '        Return (d.Day, CType(d.Month, Month), d.Year)
        '    End Get
        'End Property
        ''' <summary>
        ''' Returns the original in-game date (day and month only, starting from 1/1).
        ''' </summary>
        Public Shared ReadOnly Property [Date] As (Day As Integer, Month As Month)
            Get
                [Call]("GET_CURRENT_DATE", pointerDay, pointerMonth)
                Return (pointerDay.Value, CType(pointerMonth.Value, Month))
            End Get
        End Property
        ''' <summary>
        ''' Gets the current in-game day of the week.
        ''' </summary>
        Public Shared ReadOnly Property DayOfWeek As DaysOfWeek
            Get
                Return [Call](Of Integer)("GET_CURRENT_DAY_OF_WEEK")
            End Get
        End Property
        Public Shared ReadOnly Property DaysElapsed As Integer
            Get
                Return Stats.GetInt(Stats.StatINTs.IV_DaysPassed)
            End Get
        End Property
        ''' <summary>
        ''' Freezes or releases the in-game time at the current hour and minute.
        ''' </summary>
        Public Shared WriteOnly Property FreezeTime As Boolean
            Set(value As Boolean)
                If value = True Then
                    [Call]("FORCE_TIME_OF_DAY", Time.Hour, Time.Minute)
                Else
                    [Call]("RELEASE_TIME_OF_DAY")
                    [Call]("SET_TIME_OF_DAY", Time.Hour, Time.Minute)
                End If
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the current in-game time (hours and minutes).
        ''' </summary>
        Public Shared Property Time As (Hour As Integer, Minute As Integer)
            Get
                [Call]("GET_TIME_OF_DAY", pointerHour, pointerMinute)
                Return (Hour:=pointerHour.Value, Minute:=pointerMinute.Value)
            End Get
            Set(value As (Hour As Integer, Minute As Integer))
                If value.Hour < 0 OrElse value.Hour > 23 Then
                    Throw New ArgumentOutOfRangeException(NameOf(value.Hour), "Hour must be between 0 and 23.")
                    Tools.WriteLog("Error in NativeWorld.Calendar.Time (Set): hour must be between 0 and 23.")
                End If
                If value.Minute < 0 OrElse value.Minute > 59 Then
                    Throw New ArgumentOutOfRangeException(NameOf(value.Hour), "Minute must be between 0 and 59.")
                    Tools.WriteLog("Error in NativeWorld.Calendar.Time (Set): minute must be between 0 and 59.")
                End If

                [Call]("SET_TIME_OF_DAY", value.Hour, value.Minute)
            End Set
        End Property
        ''' <summary>
        ''' Advances the in-game time by one day, optionally using animated advance instead of native one (need TickHelper running).
        ''' </summary>
        Public Shared Sub NextDay()
            [Call]("SET_TIME_ONE_DAY_FORWARD")
        End Sub
        ''' <summary>
        ''' Moves the in-game time back by one day, optionally using animated advance instead of native one (need TickHelper running).
        ''' </summary>
        Public Shared Sub PreviousDay()
            [Call]("SET_TIME_ONE_DAY_BACK")
        End Sub
        '''' <summary>
        '''' Sets an in-game simulated date without editing the native one.
        '''' </summary>
        '''' <param name="Day">Target day (1–31 depending on month)</param>
        'Public Shared Sub SetSimulatedDate(Optional Day As Integer? = Nothing, Optional Month As Month? = Nothing, Optional Year As Integer? = Nothing)

        '    Dim currentSim = GetSimulatedDate()

        '    Dim target As New DateTime(If(Year.HasValue, Year.Value, currentSim.Year),
        '                               If(Month.HasValue, CInt(Month.Value), currentSim.Month),
        '                               If(Day.HasValue, Day.Value, currentSim.Day))

        '    _startingDate = (target.AddDays(-DaysElapsed).Day, CType(target.AddDays(-DaysElapsed).Month, Month), target.AddDays(-DaysElapsed).Year)
        'End Sub
        ''' <summary>
        ''' Sets the in-game date editing native day and month.
        ''' Optionally preserves the "days passed" stat.
        ''' </summary>
        ''' <param name="Day">Target day (1–31 depending on month)</param>
        ''' <param name="Month">Target month (1–12)</param>
        Public Shared Sub SetDate(Optional Day As Integer? = Nothing, Optional Month As Month? = Nothing, Optional PreserveElapsedDays As Boolean = False)
            Dim current = [Date]
            Dim currentDay = current.Day
            Dim currentMonth = CInt(current.Month)

            Dim targetDay = If(Day.HasValue, Day.Value, currentDay)
            Dim targetMonth = If(Month.HasValue, CInt(Month.Value), currentMonth)

            Dim previousDaysPassed = DaysElapsed

            Dim currentIndex = (currentMonth - 1) * 31 + currentDay
            Dim targetIndex = (targetMonth - 1) * 31 + targetDay
            Dim delta = targetIndex - currentIndex

            If delta > 0 Then
                For i = 1 To delta : NextDay() : Next
            ElseIf delta < 0 Then
                For i = 1 To Math.Abs(delta) : PreviousDay() : Next
            End If

            If PreserveElapsedDays Then
                Stats.SetInt(Stats.StatINTs.IV_DaysPassed, previousDaysPassed, True)
            End If
        End Sub

        ' Data come stat persistente, lavorarci su!
        '''' <summary>
        '''' Sets the in-game date in 
        '''' </summary>
        '''' <param name="Mode"></param>
        'Public Shared Sub SetDate(Optional Day As Integer = 1, Optional Month As Month = Month.January, Optional Year As Integer = 2008,
        '                                  Optional Mode As StartingDateMode = StartingDateMode.LocalDefault)
        '    Select Case Mode

        '        Case StartingDateMode.LocalDefault
        '            _startingDate = (Day, Month, Year)

        '        Case StartingDateMode.PersistentStats
        '            Dim d = Stats.GetInt(Stats.StatINTs.IVEFLC_Empty14)
        '            Dim m = Stats.GetInt(Stats.StatINTs.IVEFLC_Empty15)
        '            Dim y = Stats.GetInt(Stats.StatINTs.IVEFLC_Empty16)

        '            If d = 0 AndAlso m = 0 AndAlso y = 0 Then
        '                Stats.SetInt(Stats.StatINTs.IVEFLC_Empty14, Day, True)
        '                Stats.SetInt(Stats.StatINTs.IVEFLC_Empty15, CInt(Month), True)
        '                Stats.SetInt(Stats.StatINTs.IVEFLC_Empty16, Year, True)
        '                _startingDate = (Day, Month, Year)
        '            Else
        '                _startingDate = (d, CType(m, Month), y)
        '            End If
        '    End Select
        'End Sub
        ''' <summary>
        ''' Sets the real-time duration of one in-game minute (in milliseconds) using the TickHelper
        ''' </summary>
        Public Shared Sub SetMinuteDuration(Optional Duration As Integer = 2000)
            If Duration < 100 Then
                Throw New ArgumentOutOfRangeException(NameOf(Duration), "Minute duration must be >= 100 ms.")
            End If
            If _timeDriver = TimeDriver.AdvanceAnimation Then Return
            _timeDriver = TimeDriver.MinuteControl
            _lastRealTick = Environment.TickCount

            If _minuteControlUsesFreeze Then
                Dim t = Time
                [Call]("RELEASE_TIME_OF_DAY")
                [Call]("SET_TIME_OF_DAY", t.Hour, t.Minute)
            End If
            _minuteDurationMs = Duration
            If Not _minuteTickActive Then
                _minuteAccumulatorMs = 0
                _lastRealTick = Environment.TickCount
                _minuteTickActive = True
                TickHelper.Add_Internal(AddressOf MinuteControlTick)
            End If
        End Sub
        ''' <summary>
        ''' Sets the global in-game time scale (0.05 - 3.0).
        ''' </summary>
        Public Shared WriteOnly Property TimeScale As Single
            Set(value As Single)
                [Call]("SET_TIME_SCALE", Tools.ClampFloat(value, 0.05, 3.0))
            End Set
        End Property
        Public Enum Month
            January = 1
            February = 2
            March = 3
            April = 4
            May = 5
            June = 6
            July = 7
            August = 8
            September = 9
            October = 10
            November = 11
            December = 12
        End Enum
        Public Enum DaysOfWeek
            Sunday = 0
            Monday = 1
            Tuesday = 2
            Wednesday = 3
            Thursday = 4
            Friday = 5
            Saturday = 6
        End Enum
        'Public Enum StartingDateMode
        '    LocalDefault = 0        ' solo in memoria (default)
        '    PersistentStats = 1     ' STAT INT (IVEFLC_Empty14/15/16)
        'End Enum
        '=============================================================================================================================================
        'Internals
        '===========
        Private Shared Function GetSimulatedDate() As DateTime
            Dim baseDate As New DateTime(_startingDate.Year, CInt(_startingDate.Month), _startingDate.Day)
            Return baseDate.AddDays(DaysElapsed)
        End Function
        Private Enum TimeDriver
            None = 0
            MinuteControl = 1
            AdvanceAnimation = 2
        End Enum
        Private Shared Sub TimeAdvanceTick()
            If Not _timeAnimActive Then
                TickHelper.Remove_Internal(AddressOf TimeAdvanceTick)
                Exit Sub
            End If
            ' --- Current time in total minutes (handle wrap)
            Dim current = Time
            Dim currentTotal As Integer = current.Hour * 60 + current.Minute
            If currentTotal < _timeAnimStartMinutes Then
                currentTotal += 1440
            End If

            ' --- Timing
            Dim now As Long = Environment.TickCount
            Dim elapsedMs As Long = now - _timeAnimStartRealTick
            Dim remainingMinutes As Integer = _timeAnimTargetMinutes - currentTotal

            Dim accelMs As Integer = _timeAnimAccelDuration
            Dim decelMs As Integer = _timeAnimAccelDuration

            Dim speed As Integer
            Dim normalized As Double

            ' -------------------------------------------------
            ' ACCELERATION
            ' -------------------------------------------------
            If elapsedMs < accelMs Then

                Dim t As Double = elapsedMs / accelMs
                t = Math.Max(0.0, Math.Min(1.0, t))
                normalized = t * t
                speed = CInt(1 + (_timeAnimCruiseSpeed - 1) * normalized)

                ' -------------------------------------------------
                ' DECELERATION
                ' -------------------------------------------------
            ElseIf remainingMinutes <= CInt(_timeAnimCruiseSpeed * decelMs / 1000.0) Then

                Dim t As Double = remainingMinutes / (_timeAnimCruiseSpeed * decelMs / 1000.0)
                t = Math.Max(0.0, Math.Min(1.0, t))
                normalized = t * t
                speed = CInt(1 + (_timeAnimCruiseSpeed - 1) * normalized)

                ' -------------------------------------------------
                ' CRUISE
                ' -------------------------------------------------
            Else
                normalized = 1.0
                speed = _timeAnimCruiseSpeed
            End If
            speed = Math.Max(1, speed)

            ' -------------------------------------------------
            ' ADVANCE TIME
            ' -------------------------------------------------
            For i = 1 To speed
                AdvanceOneMinute()
            Next

            ' ricalcola dopo l’avanzamento
            Dim cur2 = Time
            Dim currentTotal2 As Integer = cur2.Hour * 60 + cur2.Minute
            If currentTotal2 < _timeAnimStartMinutes Then currentTotal2 += 1440

            If currentTotal2 >= _timeAnimTargetMinutes Then
                Dim finalMinutes As Integer = _timeAnimTargetMinutes Mod 1440
                Time = (finalMinutes \ 60, finalMinutes Mod 60)

                If _timeAnimAffectTimeScale Then
                    [Call]("SET_TIME_SCALE", _timeScaleDefault)
                End If

                _timeAnimActive = False
                TickHelper.Remove_Internal(AddressOf TimeAdvanceTick)

                ' torna al minute control
                _timeDriver = TimeDriver.MinuteControl

                If _minuteDurationMs > 0 Then
                    _minuteAccumulatorMs = 0
                    _lastRealTick = Environment.TickCount
                    _minuteTickActive = True

                    If _minuteControlUsesFreeze Then
                        Dim t = Time
                        [Call]("FORCE_TIME_OF_DAY", t.Hour, t.Minute)
                    End If

                    TickHelper.Add_Internal(AddressOf MinuteControlTick)
                End If
            End If
        End Sub
        Private Shared Sub AdvanceOneMinute()
            Dim t = Time
            Dim hour As Integer = t.Hour
            Dim minute As Integer = t.Minute + 1
            If minute > 59 Then
                minute = 0
                hour += 1
                If hour > 23 Then
                    hour = 0
                    NextDay()
                End If
            End If
            Time = (hour, minute)
        End Sub
        Private Shared Sub MinuteControlTick()
            If _timeDriver <> TimeDriver.MinuteControl Then
                TickHelper.Remove_Internal(AddressOf MinuteControlTick)
                Exit Sub
            End If
            If Not _minuteTickActive Then
                TickHelper.Remove_Internal(AddressOf MinuteControlTick)
                Exit Sub
            End If
            Dim now As Long = Environment.TickCount
            Dim delta As Long = now - _lastRealTick
            _lastRealTick = now
            If delta <= 0 Then Exit Sub

            'If _minuteControlUsesFreeze Then
            '    Dim hold = Time
            '    [Call]("FORCE_TIME_OF_DAY", hold.Hour, hold.Minute)
            'End If

            _minuteAccumulatorMs += delta

            While _minuteAccumulatorMs >= _minuteDurationMs
                _minuteAccumulatorMs -= _minuteDurationMs

                If _minuteControlUsesFreeze Then
                    Dim t = Time
                    [Call]("RELEASE_TIME_OF_DAY")
                    [Call]("SET_TIME_OF_DAY", t.Hour, t.Minute)
                    AdvanceOneMinute()
                    Dim t2 = Time
                    [Call]("FORCE_TIME_OF_DAY", t2.Hour, t2.Minute)
                Else
                    AdvanceOneMinute()
                End If
            End While
        End Sub
        '=============================================================================================================================================
    End Class

    Public NotInheritable Class Interior
        Private Sub New() : End Sub
        '''' <summary>
        '''' Untested
        '''' </summary>
        'Public Shared Sub Activate(InteriorID As Integer, Optional Enable As Boolean = True)
        '    'REQUEST_INTERIOR_MODELS
        '    [Call]("ACTIVATE_INTERIOR", InteriorID, Enable)
        'End Sub
        Public Shared Function GetFromPed(Ped As Ped) As Integer
            Return [Call](Of Integer)("GET_INTERIOR_FROM_CHAR", Ped)
        End Function

        Public Shared Function GetFromVehicle(Vehicle As Vehicle) As Integer
            Return [Call](Of Integer)("GET_INTERIOR_FROM_CAR", Vehicle)
        End Function

        'GET_ROOM_KEY_FROM_PICKUP
        Public Shared Function GetDoorState(Model As String, Position As Vector3) As (LockedInPosition As Boolean, Angle As Single)
            Dim point1 As Native.Pointer = New Native.Pointer(GetType(Boolean))
            Dim point2 As Native.Pointer = New Native.Pointer(GetType(Single))
            [Call]("GET_STATE_OF_CLOSEST_DOOR_OF_TYPE", NativeModel.GetHashKey(Model), Position.X, Position.Y, Position.Z, point1, point2)
            Return (point1.Value, point2.Value)
        End Function
        ''' <param name="Model">String from "Doors" structure</param>
        ''' <param name="Position">Door's coordinates</param>
        ''' <param name="LockedInPosition">If false the door can't move, if true can move.</param>
        ''' <param name="DoorAngle">Value from -1.0 and 1.0 (open)</param>
        Public Shared Sub SetDoorState(Model As String, Position As Vector3, LockedInPosition As Boolean, Optional DoorAngle As Single = 0.0)
            [Call]("SET_STATE_OF_CLOSEST_DOOR_OF_TYPE", NativeModel.GetHashKey(Model), Position.X, Position.Y, Position.Z, LockedInPosition, CSng(DoorAngle))
        End Sub
        ''' <summary>
        ''' Terminates JimsLocks and/or Gerry£DoorLock native scripts to prevent automatic door lock after a while.
        ''' </summary>
        Public Shared Sub TerminateDoorLockScripts(Optional JimsLocks As Boolean = True, Optional Gerry3DoorLock As Boolean = True)
            NativeScript.Stop(NativeScript.IVScripts.AmbJimsLocks)
            NativeScript.Stop(NativeScript.IVScripts.AmbGerry3DoorLock)
        End Sub
        ''' <summary>
        ''' Contains all known door model names in GTA IV (not EFLC).
        ''' </summary>
        Public Structure Doors
            ' Lawyer office
            Public Shared ReadOnly BM_lawyerDoor As String = "BM_lawyerDoor"
            Public Shared ReadOnly BM_lawyerDoor_2a As String = "BM_lawyerDoor_2a"
            Public Shared ReadOnly BM_lawyerDoor_2b As String = "BM_lawyerDoor_2b"

            ' Bank of Liberty
            Public Shared ReadOnly BankOfLibertyFrontdoorL As String = "cj_bank_door_L"
            Public Shared ReadOnly BankOfLibertyFrontdoorR As String = "cj_bank_door_R"

            ' Cargo Ship
            Public Shared ReadOnly CargoshipDoor As String = "cj_boat_door"

            ' Business Shop
            Public Shared ReadOnly BusinessShopDoorL As String = "cj_BS_door_L"
            Public Shared ReadOnly BusinessShopDoorR As String = "cj_BS_door_R"

            ' Misc Doors
            Public Shared ReadOnly cj_DB_MH3_door1 As String = "cj_DB_MH3_door1"
            Public Shared ReadOnly VladsBarDoor As String = "cj_int_door_6"
            Public Shared ReadOnly RomansTaxiOfficeDoor As String = "cj_int_door_7"
            Public Shared ReadOnly BlueDoor As String = "cj_int_door_10"
            Public Shared ReadOnly cj_int_door_12_H As String = "cj_int_door_12_H"
            Public Shared ReadOnly cj_int_door_22 As String = "cj_int_door_22"
            Public Shared ReadOnly cj_int_door_24 As String = "cj_int_door_24"
            Public Shared ReadOnly cj_int_door_27L As String = "cj_int_door_27L"
            Public Shared ReadOnly cj_int_door_27R As String = "cj_int_door_27R"
            Public Shared ReadOnly cj_int_door_29 As String = "cj_int_door_29"
            Public Shared ReadOnly cj_int_door_30 As String = "cj_int_door_30"
            Public Shared ReadOnly cj_int_door_3L As String = "cj_int_door_3L"

            ' Lawyer Front Door
            Public Shared ReadOnly LawyerFrontdoorL As String = "cj_law_frontdoor_L"
            Public Shared ReadOnly LawyerFrontdoorR As String = "cj_law_frontdoor_R"

            ' Misc Interiors
            Public Shared ReadOnly cj_MC_door_1 As String = "cj_MC_door_1"
            Public Shared ReadOnly DwainesAptDoor As String = "cj_mision_door_1"
            Public Shared ReadOnly BohanFactoryCageDoor As String = "cj_MP_fact_door_2"

            ' China Shop
            Public Shared ReadOnly ChinaShopFrontdoorL As String = "cj_new_china_door_L"
            Public Shared ReadOnly ChinaShopFrontdoorR As String = "cj_new_china_door_R"

            ' Perseus Clothes Shop
            Public Shared ReadOnly PerseusFrontdoorL As String = "cj_PER_door_L"
            Public Shared ReadOnly PerseusFrontdoorR As String = "cj_PER_door_R"

            ' Russian Clothes Shop
            Public Shared ReadOnly RussianShopFrontdoorL As String = "cj_RUS_door_1"
            Public Shared ReadOnly RussianShopFrontdoorR As String = "cj_RUS_door_2"

            ' Bowling Alley
            Public Shared ReadOnly BowlingAlleyDoorL As String = "ec_ML_door_L"
            Public Shared ReadOnly BowlingAlleyDoorR As String = "ec_ML_door_R"

            Public Shared ReadOnly FaustinsFrontdoor As String = "faustinsfrontdoor"

            ' Central Park Bathroom
            Public Shared ReadOnly ToiletDoor As String = "cj_shoot_t_door"
            Public Shared ReadOnly ToiletDoorBRK As String = "cj_t_door_BRK"
            Public Shared ReadOnly ToiletDoorENG As String = "cj_t_door_ENG"
            Public Shared ReadOnly ToiletDoorVAC As String = "cj_t_door_VAC"

            ' Church
            Public Shared ReadOnly ChurchDoorL As String = "cj_church_door_L"
            Public Shared ReadOnly ChurchDoorR As String = "cj_church_door_R"

            ' Gunshop
            Public Shared ReadOnly GunshopFrontdoor As String = "cj_ext_door_11"

            ' Triangle Club
            Public Shared ReadOnly TriangleClubFrontdoor As String = "cj_ext_door_CM"

            ' Gracie Kidnapping Safehouse
            Public Shared ReadOnly GracieSafehouseDoor As String = "cj_ext_door_1"

            ' General Exteriors
            Public Shared ReadOnly cj_ext_door_10 As String = "cj_ext_door_10"
            Public Shared ReadOnly cj_ext_door_15b As String = "cj_ext_door_15b"
            Public Shared ReadOnly cj_ext_door_16 As String = "cj_ext_door_16"
            Public Shared ReadOnly cj_ext_door_17 As String = "cj_ext_door_17"
            Public Shared ReadOnly cj_ext_door_18 As String = "cj_ext_door_18"
            Public Shared ReadOnly cj_ext_door_19_L As String = "cj_ext_door_19_L"
            Public Shared ReadOnly cj_ext_door_19_R As String = "cj_ext_door_19_R"
            Public Shared ReadOnly cj_ext_door_22 As String = "cj_ext_door_22"
            Public Shared ReadOnly cj_ext_door_6 As String = "cj_ext_door_6"
            Public Shared ReadOnly cj_ext_door_9 As String = "cj_ext_door_9"

            ' Clubs
            Public Shared ReadOnly CabaretDoorL As String = "cabaret_door_L"
            Public Shared ReadOnly CabaretDoorR As String = "cabaret_door_R"

            ' Vault
            Public Shared ReadOnly VaultDoor As String = "CJ_VAULT_DOOR"
            Public Shared ReadOnly VaultDoorDam As String = "CJ_VAULT_DOOR_DAM"
            Public Shared ReadOnly VaultGate As String = "CJ_VAULT_GATE"
            Public Shared ReadOnly SafeDoor As String = "GB_safe02_door"

            ' Cluck’n Bell
            Public Shared ReadOnly CluckBellDoor As String = "KM_CBDoor"
            Public Shared ReadOnly CluckBellMensroom As String = "KM_CBDoorTM"
            Public Shared ReadOnly CluckBellWomensroom As String = "KM_CBDoorTW"
            Public Shared ReadOnly CluckBellFrontdoorL As String = "KM_CBFrontDrL"
            Public Shared ReadOnly CluckBellFrontdoorR As String = "KM_CBFrontDrR"

            ' Majestic Hotel
            Public Shared ReadOnly RitzIntDoorA As String = "ab_RitzIntDoorA"
            Public Shared ReadOnly RitzIntDoorB As String = "ab_RitzIntDoorB"
            Public Shared ReadOnly RitzMainDoor As String = "ab_ritzMaindoor"
            Public Shared ReadOnly RitzAptDoor As String = "ab_ritz_aptdoor"
            Public Shared ReadOnly RitzGlassDoor As String = "ab_ritzglassdoor"

            ' Miscellaneous
            Public Shared ReadOnly PortacabinDoor As String = "KM_PortacabinDoor"
            Public Shared ReadOnly PrisonSecurityDoor As String = "KM_PrisSecDoor"
            Public Shared ReadOnly PrisonVisitorDoor As String = "KM_PrisVisDoor"
            Public Shared ReadOnly NorthwoodProjectsDoor As String = "proj_doorRM1"
            Public Shared ReadOnly NorthwoodAptDoor As String = "proj_doorRM1d"
            Public Shared ReadOnly proj_doorRM2 As String = "proj_doorRM2"
            Public Shared ReadOnly proj_doorRM4 As String = "proj_doorRM4"
            Public Shared ReadOnly JamFrontdoor As String = "jamfrontdoor"
            Public Shared ReadOnly HospitalMainDoors As String = "nhospmaindoors"
            Public Shared ReadOnly HospitalMainDoors01 As String = "nhospmaindoors01"

            ' Diner
            Public Shared ReadOnly DinerDoor1 As String = "ndinerdoor1"
            Public Shared ReadOnly DinerDoor2 As String = "ndinerdoor2"

            ' Crackhouse
            Public Shared ReadOnly CrackhouseDoor As String = "XJ_TenDoor"

            ' Search and Delete
            Public Shared ReadOnly SearchAndDeleteDoorA As String = "Deal_doorA"
            Public Shared ReadOnly SearchAndDeleteDoorB As String = "Deal_doorB"

            ' Chinese Restaurant
            Public Shared ReadOnly ChineseFrontdoor As String = "KM_KorBlacDoor"
            Public Shared ReadOnly ChineseInteriorDoor As String = "KM_KorTDoor"

            ' Playboy
            Public Shared ReadOnly PlayboyFrontdoor As String = "playboyfrontdoor"

            ' CIA Lobby
            Public Shared ReadOnly CIALobbyDoor As String = "Blk3_LobbyDoor"
            Public Shared ReadOnly CIALobbyOfficeDoor As String = "corpdoor"

            ' Turismo Showroom
            Public Shared ReadOnly TurismoShowroomL As String = "LD_show_door_L"
            Public Shared ReadOnly TurismoShowroomR As String = "LD_show_door_R"

            ' Hospital Corridors
            Public Shared ReadOnly HospCorridorDoor1 As String = "hospdoorcorri1"
            Public Shared ReadOnly HospCorridorDoor3 As String = "hospdoorcorri3"
            Public Shared ReadOnly HospCorridorDoor4 As String = "hospdoorcorri4"
            Public Shared ReadOnly HospCorridorDoor5 As String = "hospdoorcorri5"

            ' Projects
            Public Shared ReadOnly ProjectsBottomDoor As String = "ab_projDoor2"

            ' Museum Garage Door
            Public Shared ReadOnly MuseumGarageDoor As String = "bs3missiongardoor"
            Public Shared ReadOnly ResprayDoor As String = "bs3respraydoor"

            ' Elevator Doors
            Public Shared ReadOnly LiftLeftDoor As String = "CJ_LIFT_L_DOOR"
            Public Shared ReadOnly LiftLeftDoor2 As String = "CJ_LIFT_L_DOOR_2"
            Public Shared ReadOnly LiftLeftDoorOut As String = "CJ_LIFT_L_DOOR_OUT"
            Public Shared ReadOnly LiftLeftDoorOut2 As String = "CJ_LIFT_L_DOOR_OUT_2"
            Public Shared ReadOnly LiftRightDoor As String = "CJ_LIFT_R_DOOR"
            Public Shared ReadOnly LiftRightDoor2 As String = "CJ_LIFT_R_DOOR_2"
            Public Shared ReadOnly LiftRightDoorOut As String = "CJ_LIFT_R_DOOR_OUT"
            Public Shared ReadOnly LiftRightDoorOut2 As String = "CJ_LIFT_R_DOOR_OUT_2"
        End Structure
    End Class

    'Public NotInheritable Class IPL
    '    Private Sub New() : End Sub
    '    ''' <summary>
    '    ''' Seems not working. Used in Vlad3 and Roman1 SCO script
    '    ''' </summary>
    '    Public Shared Sub Request(IPL As String)
    '        [Call]("REQUEST_IPL", IPL)
    '    End Sub
    '    ''' <summary>
    '    ''' Seems not working. REMOVE_IPL is only in Roman1 SCO script.
    '    ''' </summary>
    '    Public Shared Sub Remove(IPL As String, Optional Discreetly As Boolean = False)
    '        If Discreetly Then [Call]("REMOVE_IPL_DISCREETLY", IPL) Else [Call]("REMOVE_IPL", IPL)
    '    End Sub
    'End Class

    Public NotInheritable Class Navigation
        Public Sub New() : End Sub
        ''' <summary>
        ''' Disables GPS voice for every car.
        ''' </summary>
        Public Shared Sub DisableGpsVoice(Optional Disable As Boolean = True)
            [Call]("DISABLE_GPS", Disable)
        End Sub
        ''' <summary>
        ''' Returns travel distance between path nodes (native). Sometimes doesn't return proper distance or seems bugged.
        ''' </summary>
        ''' <param name="FromCoord">Starting coordinates</param>
        ''' <param name="ToCoord">Destination coordinates</param>
        ''' <returns></returns>
        Public Shared Function GetTravelDistance(FromCoord As Vector3, ToCoord As Vector3) As Integer
            Return CInt([Call](Of Single)("CALCULATE_TRAVEL_DISTANCE_BETWEEN_NODES", FromCoord.X, FromCoord.Y, FromCoord.Z, ToCoord.X, ToCoord.Y, ToCoord.Z))
        End Function
        '''' <summary>
        '''' Returns GPS route length (in meters).
        '''' </summary>
        'Public Shared Function GetGPSRouteLength() As Integer
        '    Return [Call](Of Integer)("GET_ROUTE_SIZE")
        'End Function

        Public Shared Sub RemoveWaypoint()
            Try
                [Call]("SWITCH_OFF_WAYPOINT")
            Catch ex As Exception
                Tools.WriteLog("Error in NativeHud.RemoveWaypoint.", ex)
            End Try
        End Sub

        Public Shared Sub SetGpsRoute(Blip As Blip, Optional Enable As Boolean = True)
            Dim state As Integer
            If Enable = True Then state = 1 Else state = 0
            [Call]("SET_ROUTE", Blip, state)
        End Sub
        ''' <summary>
        ''' Uses Game.GetWaypoint from SHDN.
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property Waypoint As Blip
            Get
                If Game.GetWaypoint() IsNot Nothing Then
                    Return Game.GetWaypoint()
                Else
                    Return Nothing
                End If
            End Get
        End Property
    End Class

    Public NotInheritable Class Population
        Private Sub New() : End Sub
        ''' <summary>
        ''' Controls ambient world vehicles such as planes, boats, trains and mad drivers.
        ''' All parameters are optional and won't be affected if nothing.
        ''' </summary>
        Public Shared Sub ControlAmbientVehicles(Optional Planes As Boolean? = Nothing, Optional PoliceHelis As Boolean? = Nothing, Optional Boats As Boolean? = Nothing, Optional GarbageTrucks As Boolean? = Nothing,
                                             Optional MadDrivers As Boolean? = Nothing, Optional Trains As Boolean? = Nothing, Optional DeleteAllTrains As Boolean? = Nothing)
            If Planes.HasValue Then
                [Call]("SWITCH_AMBIENT_PLANES", Planes.Value)
            End If
            If PoliceHelis.HasValue Then
                [Call]("SWITCH_POLICE_HELIS", PoliceHelis.Value)
            End If
            If Boats.HasValue Then
                [Call]("SWITCH_RANDOM_BOATS", Boats.Value)
            End If
            If GarbageTrucks.HasValue Then
                [Call]("SWITCH_GARBAGE_TRUCKS", GarbageTrucks.Value)
            End If
            If MadDrivers.HasValue Then
                [Call]("SWITCH_MAD_DRIVERS", MadDrivers.Value)
            End If
            If Trains.HasValue Then
                [Call]("SWITCH_RANDOM_TRAINS", Trains.Value)
            End If
            If DeleteAllTrains.HasValue AndAlso DeleteAllTrains.Value Then
                [Call]("DELETE_ALL_TRAINS")
            End If
        End Sub

        Public Shared WriteOnly Property PedDensity As Single
            Set(value As Single)
                [Call]("SET_PED_DENSITY_MULTIPLIER", Tools.ClampFloat(value, 0.0F, 1.0F))
            End Set
        End Property
        ''' <summary>
        ''' Change traffic and parked cars density..
        ''' All values between 0.0 and 1.0 (with special non-native flags -1 and -2)
        ''' </summary>
        ''' <param name="CarDensity">Values from 0.0 to 10.0</param>
        ''' <param name="ParkedCarDensity">Values from 0.0 to 10.0. If set to -1 the parked car density won't be affected.</param>
        ''' <param name="RandomCarDensity">Values from 0.0 to 10.0. If set to -1 it will the same as CarDensity, if set to -2 it won't be affected.</param>
        Public Shared Sub ControlTrafficDensity(CarDensity As Single, Optional ParkedCarDensity As Single = -1, Optional RandomCarDensity As Single = -2)
            [Call]("SET_CAR_DENSITY_MULTIPLIER", CSng(CarDensity))

            If RandomCarDensity >= -1.0F Then
                Dim randomDensity As Single
                If RandomCarDensity = -1.0F Then
                    randomDensity = CarDensity
                ElseIf RandomCarDensity >= 0.0F Then
                    randomDensity = RandomCarDensity
                Else
                    randomDensity = 0.0F
                End If

                [Call]("SET_RANDOM_CAR_DENSITY_MULTIPLIER", CSng(randomDensity))
            End If

            If ParkedCarDensity >= 0 Then [Call]("SET_PARKED_CAR_DENSITY_MULTIPLIER", CSng(ParkedCarDensity))
        End Sub
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared WriteOnly Property EmergencyServices As Boolean
            Set(value As Boolean)
                [Call]("ALLOW_EMERGENCY_SERVICES", value)
            End Set
        End Property
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared WriteOnly Property ForceParkedCarsCloseToOthers As Boolean
            Set(value As Boolean)
                [Call]("FORCE_GENERATE_PARKED_CARS_TOO_CLOSE_TO_OTHERS", value)
            End Set
        End Property
        ''' <summary>
        ''' Disables traffic and boats, but not parked cars. Easy way to disable roads without affect traffic density.
        ''' </summary>
        ''' <param name="PedsON">False to disable peds, True to switch peds on.</param>
        ''' <param name="MinCoordinates">If set to "nothing" (default value), it will be set to Vector3.Zero</param>
        ''' <param name="MaxCoordinates">If set to "nothing" (default value), it will be set to New Vector3(4000, 4000, 700), which will cover the entire map and custom maps around Liberty City</param>
        Public Shared Sub SwitchPeds(PedsON As Boolean, Optional MinCoordinates As Vector3 = Nothing, Optional MaxCoordinates As GTA.Vector3 = Nothing)
            Dim minC As Vector3 = If(MinCoordinates = Nothing, New Vector3(0.0, 0.0, 0.0), MinCoordinates)
            Dim maxC As Vector3 = If(MaxCoordinates = Nothing, New Vector3(5000.0, 5000.0, 700.0), MaxCoordinates)
            Dim Native As String
            If PedsON Then Native = "SWITCH_PED_PATHS_ON" Else Native = "SWITCH_PED_PATHS_OFF"

            [Call](Native, minC.X, minC.Y, minC.Z, maxC.X, maxC.Y, maxC.Z)
        End Sub
        ''' <summary>
        ''' Disables traffic and boats, but not parked cars. Easy way to disable roads without affect traffic density.
        ''' </summary>
        ''' <param name="RoadsON">False to disable roads, True to switch roads back to normal.</param>
        ''' <param name="MinCoordinates">If set to "nothing" (default value), it will be set to Vector3.Zero</param>
        ''' <param name="MaxCoordinates">If set to "nothing" (default value), it will be set to New Vector3(4000, 4000, 700), which will cover the entire map and custom maps around Liberty City</param>
        Public Shared Sub SwitchTraffic(RoadsON As Boolean, Optional MinCoordinates As GTA.Vector3 = Nothing, Optional MaxCoordinates As GTA.Vector3 = Nothing)
            Dim minC As Vector3 = If(MinCoordinates = Nothing, New Vector3(0.0, 0.0, 0.0), MinCoordinates)
            Dim maxC As Vector3 = If(MaxCoordinates = Nothing, New Vector3(5000.0, 5000.0, 700.0), MaxCoordinates)
            Dim Native As String
            If RoadsON Then Native = "SWITCH_ROADS_BACK_TO_ORIGINAL" Else Native = "SWITCH_ROADS_OFF"

            [Call](Native, minC.X, minC.Y, minC.Z, maxC.X, maxC.Y, maxC.Z)
        End Sub
        'OVERRIDE_NUMBER_OF_PARKED_CARS
    End Class

    Public NotInheritable Class Respawn
        Private Sub New() : End Sub
        ''' <summary>
        ''' Adds a permanent hospital respawn point (registered internally togeter with native ones).
        ''' </summary>
        Public Shared Sub AddHospital(Coordinate As GTA.Vector3, FacingAngle As Double, Island As Islands)
            If Single.IsNaN(Coordinate.X) OrElse Single.IsInfinity(Coordinate.X) OrElse Single.IsNaN(Coordinate.Y) OrElse
                Single.IsInfinity(Coordinate.Y) OrElse Single.IsNaN(Coordinate.Z) OrElse Single.IsInfinity(Coordinate.Z) Then

                Throw New ArgumentOutOfRangeException(NameOf(Coordinate))
            End If

            If Double.IsNaN(FacingAngle) OrElse Double.IsInfinity(FacingAngle) OrElse FacingAngle > Single.MaxValue OrElse FacingAngle < Single.MinValue Then
                Throw New ArgumentOutOfRangeException(NameOf(FacingAngle))
            End If

            If Coordinate = Vector3.Zero Then
                Tools.WriteLog("Error in NativeWorld.Respawn.AddHospital: coordinates are zero.")
                Return
            End If

            If Island < 0 OrElse Island > 3 Then
                Throw New ArgumentOutOfRangeException(NameOf(Island))
            End If

            EnsureNativeRespawnsInitialized()
            [Call]("ADD_HOSPITAL_RESTART", Coordinate.X, Coordinate.Y, Coordinate.Z, FacingAngle, Island)
            RegisterNativeRespawn(Coordinate, FacingAngle, Island, RespawnType.Hospital)
        End Sub
        ''' <summary>
        ''' Adds a permanent police station respawn point (registered internally togeter with native ones).
        ''' </summary>
        Public Shared Sub AddPoliceStation(Coordinate As Vector3, FacingAngle As Double, Island As Islands)
            If Single.IsNaN(Coordinate.X) OrElse Single.IsInfinity(Coordinate.X) OrElse Single.IsNaN(Coordinate.Y) OrElse
                Single.IsInfinity(Coordinate.Y) OrElse Single.IsNaN(Coordinate.Z) OrElse Single.IsInfinity(Coordinate.Z) Then

                Throw New ArgumentOutOfRangeException(NameOf(Coordinate))
            End If

            If Double.IsNaN(FacingAngle) OrElse Double.IsInfinity(FacingAngle) OrElse FacingAngle > Single.MaxValue OrElse FacingAngle < Single.MinValue Then
                Throw New ArgumentOutOfRangeException(NameOf(FacingAngle))
            End If

            If Coordinate = Vector3.Zero Then
                Tools.WriteLog("Error in NativeWorld.Respawn.AddPoliceStation: coordinates are zero.")
                Return
            End If

            If Island < 0 OrElse Island > 3 Then
                Throw New ArgumentOutOfRangeException(NameOf(Island))
            End If

            EnsureNativeRespawnsInitialized()
            [Call]("ADD_POLICE_RESTART", Coordinate.X, Coordinate.Y, Coordinate.Z, FacingAngle, Island)
            RegisterNativeRespawn(Coordinate, FacingAngle, Island, RespawnType.PoliceStation)
        End Sub
        ''' <summary>
        ''' Adds a custom respawn point of type. It could be deleted by typing name.
        ''' </summary>
        Public Shared Sub AddCustomRespawn(Coordinate As Vector3, FacingAngle As Single, Name As String, Optional Type As RespawnType = RespawnType.All)
            If String.IsNullOrWhiteSpace(Name) Then
                Throw New ArgumentException("Respawn name cannot be empty.", NameOf(Name))
            End If

            If Type <> RespawnType.Hospital AndAlso Type <> RespawnType.PoliceStation AndAlso Type <> RespawnType.All Then
                Throw New ArgumentOutOfRangeException(NameOf(Type))
            End If

            If Single.IsNaN(Coordinate.X) OrElse Single.IsInfinity(Coordinate.X) OrElse Single.IsNaN(Coordinate.Y) OrElse
                Single.IsInfinity(Coordinate.Y) OrElse Single.IsNaN(Coordinate.Z) OrElse Single.IsInfinity(Coordinate.Z) Then

                Throw New ArgumentOutOfRangeException(NameOf(Coordinate))
            End If

            If Single.IsNaN(FacingAngle) OrElse Single.IsInfinity(FacingAngle) Then
                Throw New ArgumentOutOfRangeException(NameOf(FacingAngle))
            End If

            If Coordinate = Vector3.Zero Then
                Tools.WriteLog("Error in NativeWorld.Respawn.AddCustomRespawn: coordinates are zero.")
                Return
            End If

            SyncLock CustomRestartPoints
                If CustomRestartPoints.Any(Function(Point) String.Equals(Point.Name, Name, StringComparison.OrdinalIgnoreCase)) Then

                    Throw New ArgumentException("A respawn point with this name already exists.", NameOf(Name))
                End If

                If _HasExplicitRestartOverride AndAlso _ExplicitRestartEventStarted AndAlso Not NativePlayer.IsDead AndAlso Not NativePlayer.IsBeginArrested Then
                    _HasExplicitRestartOverride = False
                    _ExplicitRestartEventStarted = False
                End If

                CustomRestartPoints.Add(New RestartPoint(Coordinate, FacingAngle, Name, Type))
                _LastHandledRestartType = Nothing
                TickHelper.Add_Internal(RestartTick)
            End SyncLock
        End Sub
        ''' <summary>
        ''' Deletes all custom respawn points.
        ''' </summary>
        Public Shared Sub ClearAllCustomRespawns()
            SyncLock CustomRestartPoints
                CustomRestartPoints.Clear()
                If (_LastHandledRestartType.HasValue OrElse _PendingPoliceRestart) AndAlso Not _HasExplicitRestartOverride Then
                    [Call]("CANCEL_OVERRIDE_RESTART")
                End If
                ResetPendingPoliceAttempt()
                _LastHandledRestartType = Nothing
                TickHelper.Remove_Internal(RestartTick)
            End SyncLock
        End Sub
        ''' <summary>
        ''' Clears the respawn override and re-enables automatic custom respawn selection.
        ''' </summary>
        Public Shared Sub ClearNextRespawnPoint()
            [Call]("CANCEL_OVERRIDE_RESTART")
            _HasExplicitRestartOverride = False
            _ExplicitRestartEventStarted = False
            _LastHandledRestartType = Nothing
            ResetPendingPoliceAttempt()
        End Sub
        ''' <summary>
        ''' Deletes a custom respawn point by name.
        ''' </summary>
        Public Shared Sub DeleteCustomRespawn(Name As String)
            If String.IsNullOrWhiteSpace(Name) Then Return
            SyncLock CustomRestartPoints
                Dim Index As Integer = CustomRestartPoints.FindIndex(Function(Point) String.Equals(Point.Name, Name, StringComparison.OrdinalIgnoreCase))
                If Index < 0 Then Return
                CustomRestartPoints.RemoveAt(Index)
                If CustomRestartPoints.Count = 0 Then
                    If (_LastHandledRestartType.HasValue OrElse _PendingPoliceRestart) AndAlso Not _HasExplicitRestartOverride Then
                        [Call]("CANCEL_OVERRIDE_RESTART")
                    End If
                    ResetPendingPoliceAttempt()
                    TickHelper.Remove_Internal(RestartTick)
                End If
                _LastHandledRestartType = Nothing
            End SyncLock
        End Sub

        Public Shared Function GetClosest(Optional Type As RespawnType = RespawnType.All) As (Position As Vector3, Heading As Single)
            Return GetClosest(NativePlayer.Character.Position, Type)
        End Function
        Public Shared Function GetClosest(Coordinates As Vector3, Optional Type As RespawnType = RespawnType.All) As (Position As Vector3, Heading As Single)
            Dim NativePoint = GetClosestNative(Coordinates, Type)
            Dim CustomPoint = GetClosestCustom(Coordinates, Type)

            If NativePoint.Position = Vector3.Zero Then Return CustomPoint
            If CustomPoint.Position = Vector3.Zero Then Return NativePoint

            If Coordinates.DistanceTo(CustomPoint.Position) <= Coordinates.DistanceTo(NativePoint.Position) Then
                Return CustomPoint
            End If

            Return NativePoint
        End Function

        Public Shared Function GetClosestCustom(Optional Type As RespawnType = RespawnType.All) As (Position As Vector3, Heading As Single)
            Return GetClosestCustom(NativePlayer.Character.Position, Type)
        End Function
        Public Shared Function GetClosestCustom(Coordinates As Vector3, Optional Type As RespawnType = RespawnType.All) As (Position As Vector3, Heading As Single)
            If Type <> RespawnType.Hospital AndAlso Type <> RespawnType.PoliceStation AndAlso Type <> RespawnType.All Then
                Throw New ArgumentOutOfRangeException(NameOf(Type))
            End If

            ValidateSearchCoordinates(Coordinates)

            Dim Found As Boolean = False
            Dim Nearest As RestartPoint
            Dim NearestDistance As Single = Single.MaxValue

            SyncLock CustomRestartPoints
                For Each Point As RestartPoint In CustomRestartPoints
                    If Type <> RespawnType.All AndAlso Point.Type <> RespawnType.All AndAlso Point.Type <> Type Then Continue For

                    Dim Distance As Single = Coordinates.DistanceTo(Point.Coordinate)

                    If Distance < NearestDistance Then
                        NearestDistance = Distance
                        Nearest = Point
                        Found = True
                    End If
                Next
            End SyncLock

            If Not Found Then Return (Position:=Vector3.Zero, Heading:=0.0F)

            Return (Position:=Nearest.Coordinate, Heading:=Nearest.FacingAngle)
        End Function

        Public Shared Function GetClosestNative(Optional Type As RespawnType = RespawnType.All) As (Position As Vector3, Heading As Single)
            Return GetClosestNative(NativePlayer.Character.Position, Type)
        End Function
        Public Shared Function GetClosestNative(Coordinates As Vector3, Optional Type As RespawnType = RespawnType.All) As (Position As Vector3, Heading As Single)
            ValidateSearchCoordinates(Coordinates)

            Dim Points As List(Of NativeRespawn) = GetNativeRespawns(Type)
            Dim Found As Boolean = False
            Dim Nearest As NativeRespawn
            Dim NearestDistance As Single = Single.MaxValue

            For Each Point As NativeRespawn In Points
                Dim Distance As Single = Coordinates.DistanceTo(Point.Coordinate)

                If Distance < NearestDistance Then
                    NearestDistance = Distance
                    Nearest = Point
                    Found = True
                End If
            Next

            If Not Found Then Return (Position:=Vector3.Zero, Heading:=0.0F)

            Return (Position:=Nearest.Coordinate, Heading:=CSng(Nearest.Heading))
        End Function
        ''' <summary>
        ''' Sets respawn point for the next death/arrest. It will override custom respawn too.
        ''' Can be canceled by ClearNextRespawnPoint.
        ''' </summary>
        Public Shared Sub SetNextRespawnPoint(Coordinate As Vector3, Heading As Single)
            If Single.IsNaN(Coordinate.X) OrElse Single.IsInfinity(Coordinate.X) OrElse Single.IsNaN(Coordinate.Y) OrElse
                Single.IsInfinity(Coordinate.Y) OrElse Single.IsNaN(Coordinate.Z) OrElse Single.IsInfinity(Coordinate.Z) Then

                Throw New ArgumentOutOfRangeException(NameOf(Coordinate))
            End If

            If Single.IsNaN(Heading) OrElse Single.IsInfinity(Heading) Then
                Throw New ArgumentOutOfRangeException(NameOf(Heading))
            End If
            If Coordinate = Vector3.Zero Then
                Tools.WriteLog("Error in NativeWorld.Respawn.SetNextRespawnPoint: Coordinate are zero.")
                Return
            End If
            Dim RestartEventAlreadyActive As Boolean = NativePlayer.IsDead OrElse NativePlayer.IsBeginArrested
            [Call]("OVERRIDE_NEXT_RESTART", Coordinate.X, Coordinate.Y, Coordinate.Z, Heading)
            _HasExplicitRestartOverride = True
            _ExplicitRestartEventStarted = RestartEventAlreadyActive
            _LastHandledRestartType = Nothing
            ResetPendingPoliceAttempt()
        End Sub

        Public Enum RespawnType
            Hospital = 0
            PoliceStation = 1
            All = 2
        End Enum

#Region "Internals"
        '=======================================================================================================================================================
        Private Shared ReadOnly CustomRestartPoints As New List(Of RestartPoint)
        '-----
        Private Shared ReadOnly NativeHospitalRespawns As New Dictionary(Of Integer, NativeRespawn)
        Private Shared ReadOnly NativePoliceRespawns As New Dictionary(Of Integer, NativeRespawn)
        Private Shared _NextNativeHospitalId As Integer = 7
        Private Shared _NextNativePoliceId As Integer = 15
        Private Shared ReadOnly _NativeRespawnsInitLock As New Object()
        Private Shared _NativeRespawnsInitialized As Boolean = False
        '-----
        Private Shared ReadOnly RestartTick As Action = AddressOf DetermineNextRestartPoint
        Private Shared _LastHandledRestartType As Nullable(Of RespawnType) = Nothing
        Private Shared _HasExplicitRestartOverride As Boolean = False
        Private Shared _ExplicitRestartEventStarted As Boolean = False
        Private Shared _PendingPoliceRestart As Boolean = False
        Private Shared _PoliceAttemptInitialTimeSinceLastArrest As Integer = -1
        Private Shared _PoliceAttemptMaxTimeSinceLastArrest As Integer = -1
        '---------------------------------------------------------------------------------------------------------
        Friend Structure NativeRespawn
            Public ReadOnly ID As Integer
            Public ReadOnly Coordinate As Vector3
            Public ReadOnly Heading As Double
            Public ReadOnly Island As Integer
            Public ReadOnly Type As RespawnType
            Public ReadOnly IsOriginal As Boolean

            Friend Sub New(ID As Integer, Coordinate As Vector3, Heading As Double, Island As Integer, Type As RespawnType, IsOriginal As Boolean)
                Me.ID = ID
                Me.Coordinate = Coordinate
                Me.Heading = Heading
                Me.Island = Island
                Me.Type = Type
                Me.IsOriginal = IsOriginal
            End Sub
        End Structure
        Private Shared Sub EnsureNativeRespawnsInitialized()
            SyncLock _NativeRespawnsInitLock
                If _NativeRespawnsInitialized Then Return

                NativeHospitalRespawns.Clear()
                NativePoliceRespawns.Clear()
                'Police station
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 0, 0, 1232.572F, -89.6976F, 27.0242F, 270.8044)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 1, 0, 896.8408F, -355.5664F, 16.6391F, 317.9279)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 2, 0, 1232.192F, 517.2877F, 27.5737F, 176.1082)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 3, 0, 2172.299F, 449.1878F, 5.0802F, 271.4256)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 4, 1, 991.0667F, 1877.504F, 22.8928F, 25.8827)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 5, 1, 435.2621F, 1594.448F, 15.3311F, 9.1745)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 6, 2, 85.57061F, 1189.126F, 13.756F, 203.1679)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 7, 2, -416.8388F, 1102.396F, 10.7221F, 201.7159)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 8, 2, 48.4178F, 679.9523F, 13.6627F, 88.5501)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 9, 2, -416.1825F, 278.2763F, 10.9502F, 200.5633)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 10, 2, 224.552F, -215.1749F, 9.7368F, 205.1785)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 11, 2, -378.1418F, -263.8691F, 10.041F, 245.0336)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 12, 3, -924.1232F, 1265.743F, 23.5726F, 137.1969)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 13, 3, -1716.644F, 276.6892F, 21.2024F, 250.0049)
                AddOriginalNativeRespawn(RespawnType.PoliceStation, 14, 3, -1223.236F, -233.321F, 2.0335F, 308.3358)

                'hospitals
                AddOriginalNativeRespawn(RespawnType.Hospital, 0, 0, 1190.965F, 201.0901F, 31.5956F, 148.3725)
                AddOriginalNativeRespawn(RespawnType.Hospital, 1, 0, 1242.939F, 497.1355F, 27.4146F, 28.4553)
                AddOriginalNativeRespawn(RespawnType.Hospital, 2, 1, 975.7433F, 1822.26F, 19.0953F, 87.8435)
                AddOriginalNativeRespawn(RespawnType.Hospital, 3, 2, -391.2282F, 1280.008F, 22.0536F, 225.1535)
                AddOriginalNativeRespawn(RespawnType.Hospital, 4, 2, 94.8677F, 147.331F, 13.772F, 185.8728)
                AddOriginalNativeRespawn(RespawnType.Hospital, 5, 3, -1306.346F, 1286.188F, 20.7793F, 312.2485)
                AddOriginalNativeRespawn(RespawnType.Hospital, 6, 3, -1512.169F, 400.1219F, 18.6796F, 315.5775)
                _NativeRespawnsInitialized = True
            End SyncLock
        End Sub
        Friend Shared Sub Initialize_Internal()
            EnsureNativeRespawnsInitialized()
        End Sub
        Private Shared Sub AddOriginalNativeRespawn(Type As RespawnType, Id As Integer, Island As Integer, X As Single, Y As Single, Z As Single, Heading As Double)
            Dim Point As New NativeRespawn(
        Id,
        New Vector3(X, Y, Z),
        Heading,
        Island,
        Type,
        True
    )

            If Type = RespawnType.Hospital Then
                NativeHospitalRespawns.Add(Id, Point)
            Else
                NativePoliceRespawns.Add(Id, Point)
            End If
        End Sub
        Private Shared Sub DetermineNextRestartPoint()
            Dim IsDead As Boolean = NativePlayer.IsDead
            Dim IsBeingArrested As Boolean = NativePlayer.IsBeginArrested
            If _PendingPoliceRestart Then
                Dim CurrentArrestTimer As Integer = NativePlayer.TimeSinceLastArrest
                If CurrentArrestTimer >= 0 AndAlso CurrentArrestTimer > _PoliceAttemptMaxTimeSinceLastArrest Then
                    _PoliceAttemptMaxTimeSinceLastArrest = CurrentArrestTimer
                End If
            End If

            ' No death/arrest: an interrupted arrest cancels
            ' the automatic police restart previously selected.
            If Not IsDead AndAlso Not IsBeingArrested Then
                If _PendingPoliceRestart Then
                    If WasRespawnedFromPendingPoliceAttempt() Then
                        ' Arresto completato e respawn già avvenuto:
                        ' non chiamare CANCEL_OVERRIDE_RESTART.
                        ResetPendingPoliceAttempt()
                    ElseIf NativePlayer.IsArrestConfirmed Then
                        ' L'arresto è stato confermato, ma il timer del
                        ' respawn non è ancora disponibile.
                        ' Mantieni attivo l'override e controlla al tick successivo.
                        Return
                    Else
                        ' Il tentativo è terminato prima della conferma
                        ' e non è stato rilevato il respawn.
                        [Call]("CANCEL_OVERRIDE_RESTART")
                        ResetPendingPoliceAttempt()
                    End If
                End If
                _LastHandledRestartType = Nothing
                If _HasExplicitRestartOverride AndAlso _ExplicitRestartEventStarted Then
                    _HasExplicitRestartOverride = False
                    _ExplicitRestartEventStarted = False
                End If
                Return
            End If

            ' Explicit overrides always take priority over automatic selection.
            If _HasExplicitRestartOverride Then
                _ExplicitRestartEventStarted = True
                Return
            End If

            ' Death takes priority over a pending police restart.
            ' Cancel the provisional police override before searching
            ' for the nearest compatible hospital restart.
            If IsDead AndAlso _PendingPoliceRestart Then
                [Call]("CANCEL_OVERRIDE_RESTART")
                ResetPendingPoliceAttempt()
                _LastHandledRestartType = Nothing
            End If

            Dim RequiredType As RespawnType
            If IsDead Then
                RequiredType = RespawnType.Hospital
            Else
                RequiredType = RespawnType.PoliceStation
            End If

            ' Avoid repeating the same selection while the event remains active.
            If _LastHandledRestartType.HasValue AndAlso _LastHandledRestartType.Value = RequiredType Then
                Return
            End If

            Dim Closest As (Position As Vector3, Heading As Single) = GetClosest(RequiredType)
            Dim FoundRestart As Boolean = Closest.Position <> Vector3.Zero
            Dim NearestRestart As New RestartPoint(Closest.Position, Closest.Heading, String.Empty, RequiredType)

            If Not FoundRestart Then
                ' If the previously selected police restart is no longer
                ' available, do not leave its override active.
                If RequiredType = RespawnType.PoliceStation AndAlso _PendingPoliceRestart Then
                    [Call]("CANCEL_OVERRIDE_RESTART")
                    ResetPendingPoliceAttempt()
                End If
                _LastHandledRestartType = RequiredType
                Return
            End If

            If RequiredType = RespawnType.PoliceStation Then
                If Not _PendingPoliceRestart Then
                    Dim ArrestTimer As Integer = NativePlayer.TimeSinceLastArrest
                    _PoliceAttemptInitialTimeSinceLastArrest = ArrestTimer
                    _PoliceAttemptMaxTimeSinceLastArrest = ArrestTimer
                End If
            Else
                ResetPendingPoliceAttempt()
            End If

            [Call]("OVERRIDE_NEXT_RESTART", NearestRestart.Coordinate.X, NearestRestart.Coordinate.Y, NearestRestart.Coordinate.Z, NearestRestart.FacingAngle)
            If RequiredType = RespawnType.PoliceStation Then
                _PendingPoliceRestart = True
            End If
            _LastHandledRestartType = RequiredType
        End Sub
        Private Shared Sub RegisterNativeRespawn(Coordinate As Vector3, Heading As Double, Island As Integer, Type As RespawnType)
            EnsureNativeRespawnsInitialized()
            If Type = RespawnType.Hospital Then
                SyncLock NativeHospitalRespawns
                    Dim Id As Integer = _NextNativeHospitalId
                    _NextNativeHospitalId += 1

                    NativeHospitalRespawns.Add(
                Id,
                New NativeRespawn(Id, Coordinate, Heading, Island, Type, False)
            )
                End SyncLock

            ElseIf Type = RespawnType.PoliceStation Then
                SyncLock NativePoliceRespawns
                    Dim Id As Integer = _NextNativePoliceId
                    _NextNativePoliceId += 1

                    NativePoliceRespawns.Add(
                Id,
                New NativeRespawn(Id, Coordinate, Heading, Island, Type, False)
            )
                End SyncLock
            End If

            SyncLock CustomRestartPoints
                If CustomRestartPoints.Count > 0 Then
                    _LastHandledRestartType = Nothing
                    TickHelper.Add_Internal(RestartTick)
                End If
            End SyncLock
        End Sub
        Private Shared Sub ResetPendingPoliceAttempt()
            _PendingPoliceRestart = False
            _PoliceAttemptInitialTimeSinceLastArrest = -1
            _PoliceAttemptMaxTimeSinceLastArrest = -1
        End Sub
        Private Shared Function WasRespawnedFromPendingPoliceAttempt() As Boolean
            If Not NativePlayer.IsRespawnedAfterArrest Then Return False
            Dim Elapsed As Integer = NativePlayer.TimeSinceLastArrest
            If _PoliceAttemptInitialTimeSinceLastArrest < 0 Then
                ' Non risultava un arresto precedente nella sessione:
                ' il timer ora valido indica un arresto completato.
                Return Elapsed >= 0
            End If
            ' Se esisteva già un timer, deve essere stato osservato
            ' un avanzamento durante il tentativo e un successivo reset.
            Return _PoliceAttemptMaxTimeSinceLastArrest > _PoliceAttemptInitialTimeSinceLastArrest AndAlso Elapsed < _PoliceAttemptMaxTimeSinceLastArrest
        End Function
        Private Structure RestartPoint
            Public Coordinate As Vector3
            Public FacingAngle As Single
            Public Name As String
            Public Type As RespawnType
            Public Sub New(Coordinate As Vector3, FacingAngle As Single, Name As String, Type As RespawnType)
                Me.Coordinate = Coordinate
                Me.FacingAngle = FacingAngle
                Me.Name = Name
                Me.Type = Type
            End Sub
        End Structure
        Friend Shared Function GetNativeRespawns(Optional Type As RespawnType = RespawnType.All) As List(Of NativeRespawn)
            If Type <> RespawnType.Hospital AndAlso
               Type <> RespawnType.PoliceStation AndAlso
               Type <> RespawnType.All Then

                Throw New ArgumentOutOfRangeException(NameOf(Type))
            End If
            EnsureNativeRespawnsInitialized()
            Dim Result As New List(Of NativeRespawn)

            If Type = RespawnType.Hospital OrElse Type = RespawnType.All Then
                SyncLock NativeHospitalRespawns
                    For Each Entry As KeyValuePair(Of Integer, NativeRespawn) In NativeHospitalRespawns.OrderBy(Function(Item) Item.Key)
                        Result.Add(Entry.Value)
                    Next
                End SyncLock
            End If

            If Type = RespawnType.PoliceStation OrElse Type = RespawnType.All Then
                SyncLock NativePoliceRespawns
                    For Each Entry As KeyValuePair(Of Integer, NativeRespawn) In NativePoliceRespawns.OrderBy(Function(Item) Item.Key)
                        Result.Add(Entry.Value)
                    Next
                End SyncLock
            End If

            Return Result
        End Function
        Private Shared Sub ValidateSearchCoordinates(Coordinates As Vector3)
            If Single.IsNaN(Coordinates.X) OrElse Single.IsInfinity(Coordinates.X) OrElse
               Single.IsNaN(Coordinates.Y) OrElse Single.IsInfinity(Coordinates.Y) OrElse
               Single.IsNaN(Coordinates.Z) OrElse Single.IsInfinity(Coordinates.Z) Then

                Throw New ArgumentOutOfRangeException(NameOf(Coordinates))
            End If
        End Sub
#End Region

    End Class

    Public NotInheritable Class Road
        Private Sub New() : End Sub
        ''' <summary>
        ''' Returns street name.
        ''' </summary>
        Public Shared Function FindStreetName(Position As Vector3) As String
            If Position = Vector3.Zero Then
                Tools.WriteLog("Error in NativeWorld.Road.GetNextPavement: coordinates are zero.")
                Return "Error: coordinates are zero."
            End If

            Dim fullName As String = World.GetStreetName(Position)

            If String.IsNullOrWhiteSpace(fullName) Then
                Return "Nothing"
            End If

            Dim separator As Integer = fullName.IndexOf(","c)
            Dim street As String = fullName.Substring(0, separator).Trim()
            Dim crossStreet As String = fullName.Substring(separator + 1).Trim()

            Return street
        End Function
        ''' <summary>
        ''' Returns next cross street name (if near to position).
        ''' </summary>
        Public Shared Function FindCrossStreetName(Position As Vector3) As String
            If Position = Vector3.Zero Then
                Tools.WriteLog("Error in NativeWorld.Road.GetNextPavement: coordinates are zero.")
                Return "Error: coordinates are zero."
            End If

            Dim fullName As String = World.GetStreetName(Position)

            If String.IsNullOrWhiteSpace(fullName) Then
                Return String.Empty
            End If

            Dim separator As Integer = fullName.IndexOf(","c)

            If separator < 0 Then
                Return String.Empty
            End If

            Dim crossStreet As String = fullName.Substring(separator + 1).Trim()

            Return crossStreet
        End Function
        ''' <summary>
        ''' Retrieves the closest major car path node to the specified position.
        ''' </summary>
        ''' <param name="Position">The world position used for the search.</param>
        ''' <returns>
        ''' A tuple containing whether a node was found and its position.
        ''' </returns>
        Public Shared Function GetClosestMajorNode(Position As Vector3) As Vector3
            Dim xPtr As New Native.Pointer(GetType(Single))
            Dim yPtr As New Native.Pointer(GetType(Single))
            Dim zPtr As New Native.Pointer(GetType(Single))
            Dim found As Boolean = [Call](Of Boolean)("GET_CLOSEST_MAJOR_CAR_NODE", Position.X, Position.Y, Position.Z, xPtr, yPtr, zPtr)

            Dim nodePosition As New Vector3(Convert.ToSingle(xPtr.Value), Convert.ToSingle(yPtr.Value), Convert.ToSingle(zPtr.Value))
            Return nodePosition
        End Function
        ''' <summary>
        ''' Retrieves the closest car path node to the specified position.
        ''' Returns the node position and heading in favour direction.
        ''' </summary>
        ''' <param name="Position">The world position used to search for the node.</param>
        ''' <returns>
        ''' A tuple containing node Position and Heading.
        ''' </returns>
        Public Shared Function GetClosestNode(Position As Vector3) As (Position As Vector3, Heading As Single) ', TypeNative As Integer)
            Dim xPtr As New Native.Pointer(GetType(Single))
            Dim yPtr As New Native.Pointer(GetType(Single))
            Dim zPtr As New Native.Pointer(GetType(Single))
            Dim headingPtr As New Native.Pointer(GetType(Single))

            Dim found As Boolean = [Call](Of Boolean)("GET_CLOSEST_CAR_NODE_WITH_HEADING", Position.X, Position.Y, Position.Z, xPtr, yPtr, zPtr, headingPtr)

            If Not found Then
                Return (New Vector3(), 0.0F)
            End If

            Dim nodePosition As New Vector3(Convert.ToSingle(xPtr.Value), Convert.ToSingle(yPtr.Value), Convert.ToSingle(zPtr.Value))
            Dim nodeHeading As Single = Convert.ToSingle(headingPtr.Value)
            Return (nodePosition, nodeHeading)
        End Function
        ''' <summary>
        ''' Retrieves the Nth closest car path node to the specified position,
        ''' optionally restricting the search to a specific island.
        ''' Returns the node position and heading (not always on favour direction).
        ''' </summary>
        ''' <param name="Position"></param>
        ''' <param name="Index">The node index, starting at 1.</param>
        ''' <param name="IslandID">The island ID from Islands enum. Use -1 to search without specifying an island.</param>
        ''' <returns></returns>
        Public Shared Function GetClosestNodeNth(Position As Vector3, Index As Integer, Optional IslandID As Integer = -1) As (Position As Vector3, Heading As Single) ', TypeNative As Integer)
            If Index < 1 Then
                Throw New ArgumentOutOfRangeException(NameOf(Index), "Index must be greater than or equal to 1.")
            End If

            If IslandID < -1 OrElse IslandID > 3 Then
                Throw New ArgumentOutOfRangeException(NameOf(IslandID), "IslandID must be between 0 and 3 or -1.")
            End If

            Dim xPtr As New Native.Pointer(GetType(Single))
            Dim yPtr As New Native.Pointer(GetType(Single))
            Dim zPtr As New Native.Pointer(GetType(Single))
            Dim headingPtr As New Native.Pointer(GetType(Single))
            Dim typeNativePtr As New Native.Pointer(GetType(Integer))

            Dim found As Boolean

            If IslandID >= 0 Then
                found = [Call](Of Boolean)("GET_NTH_CLOSEST_CAR_NODE_WITH_HEADING_ON_ISLAND", Position.X, Position.Y, Position.Z, Index, IslandID, xPtr, yPtr, zPtr, headingPtr, typeNativePtr)
            ElseIf Index >= 1 Then
                found = [Call](Of Boolean)("GET_NTH_CLOSEST_CAR_NODE_WITH_HEADING", Position.X, Position.Y, Position.Z, Index, xPtr, yPtr, zPtr, headingPtr)
            End If

            If Not found Then
                Return (New Vector3(), 0.0) ', 0)
            End If

            Dim nodePosition As New Vector3(Convert.ToSingle(xPtr.Value), Convert.ToSingle(yPtr.Value), Convert.ToSingle(zPtr.Value))
            Dim nodeHeading As Single = Convert.ToSingle(headingPtr.Value)
            Dim typeNative As Integer = Convert.ToInt32(typeNativePtr.Value)
            Return (nodePosition, nodeHeading) ', typeNative)
        End Function

        Public Shared Function GetClosestPavement(Position As Vector3) As Vector3
            If Position = Vector3.Zero Then
                Tools.WriteLog("Error in NativeWorld.Road.GetNextPavement: coordinates are zero.")
                Return Vector3.Zero
            End If
            Dim p1, p2, p3 As New Native.Pointer(GetType(Single))
            [Call]("GET_SAFE_POSITION_FOR_CHAR", Position.X, Position.Y, Position.Z, True, p1, p2, p3)
            Return New Vector3(p1.Value, p2.Value, p3.Value)
        End Function

        '----------------------------------------------------------------------------------------------------------------
        ' !!! SISTEMARE O ELIMINARE !!!
        '''' solo taxi.txt
        ''''  GET_CLOSEST_ROAD( (iParam1^), 0.00000000, iVar23, ref vVar17, ref vVar14, ref uVar24, ref uVar25, ref uVar26 );
        Friend Shared Function GetClosestRoadOLD(Position As Vector3, Optional Radius As Single = 0.0F, Optional Flags As Integer = 1) As (RoadPoint1 As Vector3, RoadPoint2 As Vector3, Unknown1 As Single, Unknown2 As Single, Unknown3 As Single)
            Dim road1Ptr As New Native.Pointer(GetType(Vector3))
            Dim road2Ptr As New Native.Pointer(GetType(Vector3))
            Dim value1Ptr As New Native.Pointer(GetType(Single))
            Dim value2Ptr As New Native.Pointer(GetType(Single))
            Dim value3Ptr As New Native.Pointer(GetType(Single))

            Dim found As Boolean = [Call](Of Boolean)("GET_CLOSEST_ROAD", Position.X, Position.Y, Position.Z, Radius, Flags, road1Ptr, road2Ptr, value1Ptr, value2Ptr, value3Ptr)

            Dim road1 As Vector3 = DirectCast(road1Ptr.Value, Vector3)
            Dim road2 As Vector3 = DirectCast(road2Ptr.Value, Vector3)
            Dim value1 As Single = Convert.ToSingle(value1Ptr.Value)
            Dim value2 As Single = Convert.ToSingle(value2Ptr.Value)
            Dim value3 As Single = Convert.ToSingle(value3Ptr.Value)

            Return (road1, road2, value1, value2, value3)
        End Function
        '----------------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieves a random car path node within the specified radius.
        ''' Returns Nothing if no suitable node is found.
        ''' </summary>
        ''' <param name="Position">The center position of the search.</param>
        ''' <param name="Radius">The radius of the search area.</param>
        ''' <returns>A nullable tuple containing the node position and heading,
        ''' or Nothing if no node is found.</returns>
        Public Shared Function GetRandomNode(Position As Vector3, Optional Radius As Single = 50.0F) As (Position As Vector3, Heading As Single)?
            Dim xPtr As New Native.Pointer(GetType(Single))
            Dim yPtr As New Native.Pointer(GetType(Single))
            Dim zPtr As New Native.Pointer(GetType(Single))
            Dim headingPtr As New Native.Pointer(GetType(Single))
            Dim found As Boolean = [Call](Of Boolean)("GET_RANDOM_CAR_NODE", Position.X, Position.Y, Position.Z, Radius, True, True, True, xPtr, yPtr, zPtr, headingPtr)

            If Not found Then
                Return Nothing
            End If

            Dim nodePosition As New Vector3(Convert.ToSingle(xPtr.Value), Convert.ToSingle(yPtr.Value), Convert.ToSingle(zPtr.Value))
            Dim nodeHeading As Single = Convert.ToSingle(headingPtr.Value)
            Return (nodePosition, nodeHeading)
        End Function
        ''' <summary>
        ''' Untested.
        ''' </summary>
        ''' <param name="NodeID">The car path node ID.</param>
        ''' <param name="Flags">The native flags used for the operation.</param>
        ''' <returns>A tuple containing the spawn position and heading.</returns>
        Public Shared Function GetSpawnCoordsForNode(NodeID As Integer, Flags As Integer) As (Position As Vector3, Heading As Single)
            Dim xPtr As New Native.Pointer(GetType(Single))
            Dim yPtr As New Native.Pointer(GetType(Single))
            Dim zPtr As New Native.Pointer(GetType(Single))
            Dim headingPtr As New Native.Pointer(GetType(Single))
            [Call]("GET_SPAWN_COORDINATES_FOR_CAR_NODE", NodeID, Flags, xPtr, yPtr, zPtr, headingPtr)

            Dim nodePosition As New Vector3(Convert.ToSingle(xPtr.Value), Convert.ToSingle(yPtr.Value), Convert.ToSingle(zPtr.Value))
            Dim nodeHeading As Single = Convert.ToSingle(headingPtr.Value)
            Return (nodePosition, nodeHeading)
        End Function
        'Public Shared Sub LoadNodes(MinCoords As Vector3, MaxCoords As Vector3)
        '    [Call]("LOAD_PATH_NODES_IN_AREA", MinCoords.X, MinCoords.Y, MaxCoords.X, MaxCoords.Y)
        'End Sub
        ''' <summary>
        ''' Loads all path nodes (LOAD_ALL_PATH_NODES native)
        ''' </summary>
        Public Shared Sub LoadAllNodes(Optional Enabled As Boolean = True)
            [Call]("LOAD_ALL_PATH_NODES", Enabled)
        End Sub
        Public Shared Sub ReleaseAllNodes()
            [Call]("RELEASE_PATH_NODES")
        End Sub
        '''' <summary>
        '''' untested
        '''' </summary>
        'Public Shared Function GetClosest(Position As Vector3) As (Position As Vector3, Heading As Single)
        '    Dim pointPos1, pointPos2, pointPos3, pointHeading As Native.Pointer
        '    pointPos1 = New Native.Pointer(GetType(Single))
        '    pointPos2 = New Native.Pointer(GetType(Single))
        '    pointPos3 = New Native.Pointer(GetType(Single))
        '    pointHeading = New Native.Pointer(GetType(Single))
        '    [Call]("GET_CLOSEST_CAR_NODE_WITH_HEADING", Position.X, Position.Y, Position.Z, pointPos1, pointPos2, pointPos3, pointHeading)
        '    Return (New Vector3(pointPos1.Value, pointPos2.Value, pointPos3.Value), pointHeading.Value)
        'End Function
        '''' <summary>
        '''' Untested
        '''' </summary>
        'Public Shared Function GetNextClosest(Position As Vector3, Optional Heading As Single? = Nothing) As (Position As Vector3, Heading As Single)
        '    Dim posPtr As New Native.Pointer(GetType(Vector3))
        '    Dim headPtr As Native.Pointer = Nothing

        '    ' priority order
        '    If Heading.HasValue Then
        '        headPtr = New Native.Pointer(GetType(Single))
        '        [Call]("GET_NEXT_CLOSEST_CAR_NODE_FAVOUR_DIRECTION", Position.X, Position.Y, Position.Z, Heading.Value, posPtr, headPtr)
        '        Return (posPtr.Value, headPtr.Value)
        '    End If

        '    headPtr = New Native.Pointer(GetType(Single))
        '    [Call]("GET_NEXT_CLOSEST_CAR_NODE_WITH_HEADING", Position.X, Position.Y, Position.Z, posPtr, headPtr)
        '    Return (posPtr.Value, headPtr.Value)
        '    '[Call]("GET_NEXT_CLOSEST_CAR_NODE", Position.X, Position.Y, Position.Z, posPtr)
        '    ' Return (posPtr.Value, Nothing)
        'End Function
        '''' <summary>
        '''' Untested
        '''' </summary>
        'Public Shared Function GetNthClosest(Position As Vector3, Index As Integer, Optional Heading As Single? = Nothing) As (Position As Vector3, Heading As Single)
        '    Dim posPtr As New Native.Pointer(GetType(Vector3))
        '    Dim headPtr As Native.Pointer = Nothing

        '    ' priority order
        '    If Heading.HasValue Then
        '        headPtr = New Native.Pointer(GetType(Single))
        '        [Call]("GET_NTH_CLOSEST_CAR_NODE_FAVOUR_DIRECTION", Position.X, Position.Y, Position.Z, Heading.Value, Index, posPtr, headPtr)

        '        Return (posPtr.Value, headPtr.Value)
        '    End If
        '    ' If Heading.HasValue Then
        '    headPtr = New Native.Pointer(GetType(Single))
        '    [Call]("GET_NTH_CLOSEST_CAR_NODE_WITH_HEADING", Position.X, Position.Y, Position.Z, Index, posPtr, headPtr)

        '    Return (posPtr.Value, headPtr.Value)
        '    ' End If
        '    ' [Call]("GET_NTH_CLOSEST_CAR_NODE", Position.X, Position.Y, Position.Z, Index, posPtr)
        '    '  Return (posPtr.Value, Nothing)
        'End Function
        '''' <summary>
        '''' Untested
        '''' </summary>
        '''' <returns></returns>
        'Public Shared Function GetNthClosestOnIsland(Position As Vector3, Index As Integer, IslandID As Integer) As (Position As Vector3, Heading As Single, NodeID As Integer?)
        '    Dim posPtr As New Native.Pointer(GetType(Vector3))
        '    Dim headPtr As Native.Pointer = Nothing
        '    Dim nodeIdPtr As New Native.Pointer(GetType(Integer))
        '    ' If heading.HasValue Then
        '    headPtr = New Native.Pointer(GetType(Single))
        '    [Call]("GET_NTH_CLOSEST_CAR_NODE_WITH_HEADING_ON_ISLAND", Position.X, Position.Y, Position.Z, Index, IslandID, posPtr, headPtr, nodeIdPtr)

        '    Return (posPtr.Value, headPtr.Value, nodeIdPtr.Value)
        '    ' End If
        '    '[Call]("GET_NTH_CLOSEST_CAR_NODE_ON_ISLAND", position.X, position.Y, position.Z, index, islandId, posPtr, nodeIdPtr)
        '    '  Return (posPtr.Value, Nothing, nodeIdPtr.Value)
        'End Function
    End Class

    Public NotInheritable Class Zone
        Private Sub New() : End Sub
        Public Shared Sub DisableCops(ZoneHash As String, Optional CopsDisabled As Boolean = True)
            Try
                If String.IsNullOrWhiteSpace(ZoneHash) Then
                    Tools.WriteLog("Error in NativeWorld.Zone.DisableCops: " & ZoneHash & " is empty.")
                Else
                    [Call]("SET_ZONE_NO_COPS", ZoneHash, CopsDisabled)
                End If
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.Zone.DisableCops.", ex)
            End Try
        End Sub
        Public Shared Function GetName(Position As Vector3) As String
            Try
                Return [Call](Of String)("GET_NAME_OF_INFO_ZONE", Position.X, Position.Y, Position.Z)
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.Zone.GetNameFromCoords.", ex)
                Return "[Error]"
            End Try
        End Function
        Public Shared Sub SetDangerLevel(ZoneHash As String, Level As DangerLevel)
            Try
                If ZoneHash = "" Then
                    Tools.WriteLog("Error in NativeWorld.Zone.SetDangerLevel: zone hash is not valid.")
                Else
                    [Call]("SET_ZONE_SCUMMINESS", ZoneHash, Tools.ClampInteger(CInt(Level), 0, 5))
                End If
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.Zone.SetDangerLevel.", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Untested.
        ''' </summary>
        ''' <param name="PopulationType">Values between 0 and 69</param>
        Public Shared Sub SetPopulationType(ZoneHash As String, PopulationType As Integer)
            Try
                If ZoneHash = "" Then
                    Tools.WriteLog("Error in NativeWorld.Zone.SetPopulationType: zone hash is not valid.")
                Else
                    [Call]("SET_ZONE_POPULATION_TYPE", ZoneHash, PopulationType)
                End If
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.Zone.SetPopulationType.", ex)
            End Try
        End Sub
        Public Enum DangerLevel
            Safest = 0
            Residential = 1
            Neutral = 2
            Urban = 3
            Degraded = 4
            Criminal = 5
        End Enum
        Public Structure ZoneHash
            Public Const Acter As String = "Zact1"
            Public Const ActerEast As String = "Zact2"
            Public Const ActerIndustrial As String = "Zacti"
            Public Const ActerIndustrialNorth As String = "Zacti1"

            Public Const AirportSouth As String = "ZAIRPT1"
            Public Const AirportCentral As String = "ZAIRPT2"
            Public Const AirportNorth As String = "ZAIRPT3"

            Public Const Alderney1 As String = "Zald1"
            Public Const Alderney2 As String = "Zald2"
            Public Const Alderney3 As String = "Zald3"
            Public Const Alderney4 As String = "Zald4"

            Public Const BeachGate1 As String = "ZBEG1"
            Public Const BeachGate2 As String = "ZBEG2"

            Public Const BohanCivicCenter1 As String = "ZBECCT1"
            Public Const BohanCivicCenter2 As String = "ZBECCT2"
            Public Const BohanCivicCenter3 As String = "ZBECCT3"
            Public Const BohanCivicCenter4 As String = "ZBECCT4"
            Public Const BohanCivicCenter5 As String = "ZBECCT5"

            Public Const Berc As String = "Zberc"
            Public Const BercNorth As String = "Zberc1"

            Public Const BohanAlleyBlock1 As String = "ZBOAB1"
            Public Const BohanAlleyBlock2 As String = "ZBOAB2"
            Public Const BohanAlleyBlock3 As String = "ZBOAB3"
            Public Const BohanAlleyBlock4 As String = "ZBOAB4"
            Public Const BohanAlleyBlock5 As String = "ZBOAB5"
            Public Const BohanAlleyBlock6 As String = "ZBOAB6"

            Public Const Boulevard1 As String = "ZBOULE1"
            Public Const Boulevard2 As String = "ZBOULE2"
            Public Const Boulevard3 As String = "ZBOULE3"
            Public Const Boulevard4 As String = "ZBOULE4"

            Public Const CervazaHeights1 As String = "ZCERV1"
            Public Const CervazaHeights2 As String = "ZCERV2"
            Public Const CervazaHeights3 As String = "ZCERV3"
            Public Const CervazaHeights4 As String = "ZCERV4"

            Public Const CityCore1 As String = "ZCity1"
            Public Const CityCore2 As String = "ZCity2"

            Public Const Downtown As String = "ZDOWNT"

            Public Const EastIsland As String = "Zeast"

            Public Const EastHolland1 As String = "ZEHOK1"
            Public Const EastHolland2 As String = "ZEHOK2"
            Public Const EastHolland3 As String = "ZEHOK3"
            Public Const EastHolland4 As String = "ZEHOK4"
            Public Const EastHolland5 As String = "ZEHOK5"
            Public Const EastHolland6 As String = "ZEHOK6"
            Public Const EastHolland7 As String = "ZEHOK7"
            Public Const EastHolland8 As String = "ZEHOK8"
            Public Const EastHolland9 As String = "ZEHOK9"
            Public Const EastHolland10 As String = "ZEHOK10"

            Public Const Industrial1 As String = "ZINDUS1"
            Public Const Industrial2 As String = "ZINDUS2"
            Public Const Industrial3 As String = "ZINDUS3"
            Public Const Industrial4 As String = "ZINDUS4"
            Public Const Industrial5 As String = "ZINDUS5"
            Public Const Industrial6 As String = "ZINDUS6"

            Public Const MiddlePark1 As String = "ZMPARK1"
            Public Const MiddlePark2 As String = "ZMPARK2"
            Public Const MiddlePark3 As String = "ZMPARK3"
            Public Const MiddlePark4 As String = "ZMPARK4"

            Public Const Port As String = "Zport"
            Public Const PortNorth As String = "Zport1"

            Public Const RichHills1 As String = "ZRHIL1"
            Public Const RichHills2 As String = "ZRHIL2"
            Public Const RichHills3 As String = "ZRHIL3"
            Public Const RichHills4 As String = "ZRHIL4"
            Public Const RichHills5 As String = "ZRHIL5"
            Public Const RichHills6 As String = "ZRHIL6"
            Public Const RichHills7 As String = "ZRHIL7"
            Public Const RichHills8 As String = "ZRHIL8"
            Public Const RichHills9 As String = "ZRHIL9"
            Public Const RichHills10 As String = "ZRHIL10"
            Public Const RichHills11 As String = "ZRHIL11"
            Public Const RichHills12 As String = "ZRHIL12"
            Public Const RichHills13 As String = "ZRHIL13"
            Public Const RichHills14 As String = "ZRHIL14"
            Public Const RichHills15 As String = "ZRHIL15"
            Public Const RichHills16 As String = "ZRHIL16"

            Public Const AirportRunway0 As String = "ZAIRU0"
            Public Const AirportRunway1 As String = "ZAIRU1"
            Public Const AirportRunway2 As String = "ZAIRU2"
            Public Const AirportRunway3 As String = "ZAIRU3"
            Public Const AirportRunway4 As String = "ZAIRU4"
            Public Const AirportRunway5 As String = "ZAIRU5"
            Public Const AirportRunway6 As String = "ZAIRU6"
            Public Const AirportRunway7 As String = "ZAIRU7"
            Public Const AirportRunway8 As String = "ZAIRU8"
            Public Const AirportRunway9 As String = "ZAIRU9"
        End Structure
    End Class

    Public NotInheritable Class Weather
        Private Sub New() : End Sub
        Private Shared ReadOnly _validModifiers As HashSet(Of String) = New HashSet(Of String)(GetType(TimecycleModifiers).GetFields(Reflection.BindingFlags.Public Or
                                                                                                                                     Reflection.BindingFlags.Static).Select(Function(f) CType(f.GetValue(Nothing), String)), StringComparer.OrdinalIgnoreCase)
        Private Shared pointerCurrWeather As New Native.Pointer(GetType(WeatherType))
        Public Shared Sub ClearTimecycleModifier()
            Try
                [Call]("CLEAR_TIMECYCLE_MODIFIER")
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.Weather.ClearTimecycleModifier.", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Controls the game weather.
        ''' </summary>
        ''' <param name="Weather">
        ''' Weather to apply. Use Weather.Random to select a random weather type.
        ''' </param>
        ''' <param name="InstantChange">
        ''' If True, the weather changes immediately.
        ''' </param>
        ''' <param name="KeepForced">
        ''' If True, the weather remains forced.
        ''' </param>
        Public Shared Sub ControlWeather(Weather As WeatherType, Optional InstantChange As Boolean = True, Optional KeepForced As Boolean = False)
            Try
                Dim weatherToApply As GTA.Weather
                If Weather = WeatherType.Random Then
                    weatherToApply = NativeGeneric.GetRandomInteger(0, 9)
                Else
                    weatherToApply = CInt(Weather)
                End If
                If InstantChange Then
                    [Call]("FORCE_WEATHER_NOW", weatherToApply)
                Else
                    [Call]("FORCE_WEATHER", weatherToApply)
                End If
                If Not KeepForced Then
                    [Call]("RELEASE_WEATHER")
                End If
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.Weather.SetWeather.", ex)
            End Try
        End Sub
        Public Shared ReadOnly Property CurrentWeather As WeatherType
            Get
                Try
                    [Call]("GET_CURRENT_WEATHER", pointerCurrWeather)
                    Return pointerCurrWeather.Value
                Catch ex As Exception
                    Tools.WriteLog("Error in NativeWorld.Weather.CurrentWeather (Get).", ex)
                    Return WeatherType.Random
                End Try
            End Get
        End Property

        Public Shared Sub SetTimecycleModifier(Modifier As String)
            If String.IsNullOrWhiteSpace(Modifier) Then
                Tools.WriteLog("Error in NativeWorld.Weather.SetTimecycleModifier: string is empty.")
                Exit Sub
            End If

            If Not _validModifiers.Contains(Modifier) Then
                Tools.WriteLog($"Error in NativeWorld.Weather.SetTimecycleModifier: modifier '{Modifier}' is not valid.")
                Exit Sub
            End If

            Try
                [Call]("SET_TIMECYCLE_MODIFIER", Modifier)
            Catch ex As Exception
                Tools.WriteLog("Error in NativeWorld.Weather.SetTimecycleModifier.", ex)
            End Try
        End Sub
        Public Structure TimecycleModifiers
            Public Const NoAmbient As String = "noambient"
            Public Const NoAmbientmult As String = "NoAmbientmult"
            Public Const Qwnomoon As String = "qwnomoon"
            Public Const Qw2nomoon As String = "qw2nomoon"
            Public Const Brook_S2_TC As String = "Brook_S2_TC"
            Public Const MH_NOMOON As String = "MH_NOMOON"
            Public Const KsS1nomoon1 As String = "KsS1nomoon1"
            Public Const KsS1nomoon2 As String = "KsS1nomoon2"
            Public Const KsS1nomoon3 As String = "KsS1nomoon3"
            Public Const Brook_N_gden As String = "Brook_N_gden"
            Public Const Buildsite_MH1 As String = "Buildsite_MH1"
            Public Const MH3carpark As String = "MH3carpark"
            Public Const Bkn2_Nomoon1 As String = "bkn2_Nomoon1"
            Public Const Bkn2_Nomoon2 As String = "bkn2_Nomoon2"
            Public Const Bks3norain As String = "bks3norain"
            Public Const MHNoMoon As String = "MHNoMoon"
            Public Const Erosware As String = "erosware"
            Public Const QM_Nomoon As String = "QM_Nomoon"
            Public Const NJ2nomoon As String = "NJ2nomoon"
            Public Const Bxwnomoon As String = "bxwnomoon"
            Public Const Star_junc As String = "star_junc"
            Public Const Raytest As String = "raytest"
            Public Const Raytest2 As String = "raytest2"
            Public Const NJ02TUNNEL As String = "NJ02TUNNEL"
            Public Const Internaldim As String = "Internaldim"
            Public Const SoosTunnel As String = "SoosTunnel"
            Public Const Nikwarehouse As String = "Nikwarehouse"
            Public Const Clam As String = "clam"
            Public Const Vlads As String = "vlads"
            Public Const Generic As String = "generic"
            Public Const Jamcafe As String = "jamcafe"
            Public Const Ten_str As String = "ten_str"
            Public Const Playboyx As String = "playboyx"
            Public Const Browner As String = "browner"
            Public Const Limo As String = "limo"
            Public Const Stevetunnel As String = "STEVETUNNEL"
            Public Const Subway As String = "SUBWAY"
            Public Const Subway_Station As String = "SUBWAY_STATION"
            Public Const Carpark As String = "CARPARK"
            Public Const RomansFl As String = "RomansFl"
            Public Const Rscafe As String = "rscafe"
            Public Const Bernies As String = "bernies"
            Public Const Factorytest As String = "Factorytest"
            Public Const Factory As String = "Factory"
            Public Const Bens2 As String = "bens2"
            Public Const Hospital As String = "Hospital"
            Public Const Museum2 As String = "Museum2"
            Public Const Badamine As String = "Badamine"
            Public Const DrugDen As String = "DrugDen"
            Public Const Bank3 As String = "Bank3"
            Public Const Chase As String = "Chase"
            Public Const Sexshop As String = "sexshop"
            Public Const Diner As String = "Diner"
            Public Const Hospitallobby As String = "hospitallobby"
            Public Const Trespass As String = "Trespass"
            Public Const Ritz As String = "ritz"
            Public Const Ritzf3 As String = "ritzf3"
            Public Const Ritzpen As String = "ritzpen"
            Public Const Intcafe As String = "intcafe"
            Public Const Firedept As String = "firedept"
            Public Const Deal As String = "deal"
            Public Const Korrest As String = "korrest"
            Public Const Korbar As String = "korbar"
            Public Const Korkitch As String = "korkitch"
            Public Const Apart As String = "apart"
            Public Const Parktoilet As String = "parktoilet"
            Public Const HarlemProjects As String = "HarlemProjects"
            Public Const HarlemDrug As String = "HarlemDrug"
            Public Const JerSave As String = "JerSave"
            Public Const Burgershot As String = "burgershot"
            Public Const Burgershotold As String = "burgershotold"
            Public Const HarlemTopFloor As String = "HarlemTopFloor"
            Public Const Irishbar As String = "Irishbar"
            Public Const Boatcabs As String = "boatcabs"
            Public Const Corplobby As String = "corplobby"
            Public Const Binco As String = "binco"
            Public Const Gazwarehouse As String = "gazwarehouse"
            Public Const Bruciechopshop As String = "bruciechopshop"
            Public Const Playboyxlobby As String = "playboyxlobby"
            Public Const Statuestair As String = "statuestair"
            Public Const Waste As String = "waste"
            Public Const Bowl As String = "Bowl"
            Public Const GunShop As String = "GunShop"
            Public Const Chinagun As String = "chinagun"
            Public Const Harlem_ten As String = "harlem_ten"
            Public Const Sw_har_decor As String = "sw_har_decor"
            Public Const Sw_har_psh As String = "sw_har_psh"
            Public Const Ten_standard As String = "ten_standard"
            Public Const Ten_ornate As String = "ten_ornate"
            Public Const Ten_modern As String = "ten_modern"
            Public Const Cluckinbell As String = "cluckinbell"
            Public Const Casino As String = "Casino"
            Public Const Limooffice As String = "limooffice"
            Public Const DrugDenStair As String = "DrugDenStair"
            Public Const Project As String = "project"
            Public Const DwayneApart As String = "DwayneApart"
            Public Const Stair1 As String = "stair1"
            Public Const Stair2 As String = "stair2"
            Public Const MH8_carpark As String = "MH8_carpark"
            Public Const MH8_Savehouse As String = "MH8_Savehouse"
            Public Const MH8_Showroom As String = "MH8_Showroom"
            Public Const Studio_Apart01 As String = "STUDIO_APART01"
            Public Const ProjectStair As String = "projectStair"
            Public Const Lightning As String = "lightning"
            Public Const Playersettings As String = "playersettings"
            Public Const Playersettings2 As String = "playersettings2"
            Public Const Sniper As String = "sniper"
            Public Const Sniper_ini As String = "sniper_ini"
            Public Const Binocular As String = "binocular"
            Public Const Injured As String = "injured"
            Public Const Fast As String = "fast"
            Public Const Death As String = "death"
            Public Const Death2 As String = "death2"
            Public Const Death3 As String = "death3"
            Public Const Train_int As String = "train_int"
            Public Const Busted As String = "busted"
            Public Const Cabaret As String = "cabaret"
            Public Const Lobby2office As String = "lobby2office"
            Public Const Lobby2 As String = "lobby2"
            Public Const Police As String = "Police"
            Public Const SUBWAYSERV As String = "SUBWAYSERV"
            Public Const SUBWAYENT As String = "SUBWAYENT"
            Public Const SUBWAY_N As String = "SUBWAY_N"
            Public Const SUBWAY_E As String = "SUBWAY_E"
            Public Const SUBWAY_S As String = "SUBWAY_S"
            Public Const SUBWAY_W As String = "SUBWAY_W"
            Public Const NIGHTSHADE As String = "NIGHTSHADE"
            Public Const PIZZAREST As String = "PIZZAREST"
            Public Const PIZZAREST2 As String = "PIZZAREST2"
            Public Const BRUCIE_STUDIO As String = "BRUCIE_STUDIO"
            Public Const Church As String = "church"
            Public Const Faustins As String = "Faustins"
            Public Const Faustinsbase As String = "Faustinsbase"
            Public Const LittleJacobs As String = "LittleJacobs"
            Public Const Prison As String = "Prison"
            Public Const BernieCrane As String = "BernieCrane"
            Public Const McRearyHouse As String = "McRearyHouse"
            Public Const CopshopOffice As String = "CopshopOffice"
            Public Const Michelles As String = "Michelles"
            Public Const Sopranos As String = "sopranos"
            Public Const CIAoffice As String = "CIAoffice"
            Public Const Portacabin As String = "portacabin"
            Public Const Comclub As String = "comclub"
            Public Const Elizabetas As String = "elizabetas"
            Public Const Bada As String = "Bada"
            Public Const Fau3_a As String = "fau3_a"
            Public Const Imbhst As String = "imbhst"
            Public Const Em_4b As String = "em_4b"
            Public Const G_1 As String = "g_1"
            Public Const G_2 As String = "g_2"
            Public Const G_3 As String = "g_3"
            Public Const Em_4 As String = "em_4"
            Public Const Df_2 As String = "df_2"
            Public Const Df_3 As String = "df_3"
            Public Const Lilj1_a As String = "lilj1_a"
            Public Const Imfau6 As String = "imfau6"
            Public Const Imfau2 As String = "imfau2"
            Public Const Wedint As String = "wedint"
            Public Const Gm_2 As String = "gm_2"
            Public Const Br_1 As String = "br_1"
            Public Const Br_4 As String = "br_4"
            Public Const Px_2 As String = "px_2"
            Public Const Pxdf As String = "pxdf"
            Public Const Rb_4b As String = "rb_4b"
            Public Const Vla1_a As String = "vla1_a"
            Public Const Vla2_a As String = "vla2_a"
            Public Const Vla4_a As String = "vla4_a"
            Public Const Rom8_b As String = "rom8_b"
            Public Const Pm_3 As String = "pm_3"
            Public Const Em_1 As String = "em_1"
            Public Const Em_2 As String = "em_2"
            Public Const Em_3 As String = "em_3"
            Public Const Em_5 As String = "em_5"
            Public Const Em_7 As String = "em_7"
            Public Const Fau4_a As String = "fau4_a"
            Public Const Show_1 As String = "show_1"
            Public Const Show_2 As String = "show_2"
            Public Const Show_3 As String = "show_3"
            Public Const Show_4 As String = "show_4"
            Public Const Show_5 As String = "show_5"
            Public Const Show_6 As String = "show_6"
            Public Const Show_7 As String = "show_7"
            Public Const Show_8 As String = "show_8"
            Public Const Rb_4 As String = "rb_4"
            Public Const J_1 As String = "j_1"
            Public Const Rp_13 As String = "rp_13"
            Public Const Rom2_a As String = "rom2_a"
            Public Const Rom3_a As String = "rom3_a"
            Public Const Rom5_a As String = "rom5_a"
            Public Const Rom6_a As String = "rom6_a"
            Public Const Rom8_a As String = "rom8_a"
            Public Const R_9 As String = "r_9"
            Public Const Classic As String = "Classic"
            Public Const Tweaked As String = "Tweaked"
            Public Const Cinema As String = "Cinema"
            Public Const Verte As String = "Verte"
            Public Const Hot As String = "Hot"
            Public Const Steel As String = "Steel"
            Public Const Psyche As String = "Psyche"
            Public Const Romantic As String = "Romantic"
            Public Const Sepia As String = "Sepia"
            Public Const Muddy As String = "Muddy"
            Public Const Neon As String = "Neon"
            Public Const Rouge As String = "Rouge"
            Public Const Bronze As String = "Bronze"
            Public Const Ulraviolet As String = "Ulraviolet"
            Public Const Eclipse As String = "Eclipse"
            Public Const Noire As String = "Noire"
            Public Const Colors As String = "colors"
            Public Const Vintage As String = "Vintage"
            Public Const Fire As String = "Fire"
            Public Const Sketch As String = "Sketch"
        End Structure
    End Class
    'REQUEST_INTERIOR_MODELS
    ' FIND_PRIMARY_POPULATION_ZONE_GROUP( ref iVar2, ref uVar3 );
End Class


