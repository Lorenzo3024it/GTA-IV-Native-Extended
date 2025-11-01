Imports GTA
Imports GTA.Native.Function
Public Class NativeTimer
    '========================================================================================================================
    ' Native Timers A, B & C
    '-------------------------
    ''' <summary>
    ''' Returns played time in milliseconds.
    ''' </summary>
    Public Shared ReadOnly Property GameTimer As Integer
        Get
            Dim point As Native.Pointer = New Native.Pointer(GetType(Integer))
            [Call]("GET_GAME_TIMER", point)
            Return point.Value
        End Get
    End Property
    ''' <summary>
    ''' The first native Timer, used by many vanilla scripts.
    ''' </summary>
    ''' <returns>Time elapsed (ms) since last reset</returns>
    Public Shared Function GetTimerA() As Integer
        Return [Call](Of Integer)("TIMERA")
    End Function
    ''' <summary>
    ''' The second native Timer, used by many vanilla scripts.
    ''' </summary>
    ''' <returns>Time elapsed (ms) since last reset</returns>
    Public Shared Function GetTimerB() As Integer
        Return [Call](Of Integer)("TIMERB")
    End Function
    ''' <summary>
    ''' The third native Timer, used only in vanilla Multiplayer scripts.
    ''' It should be safe to set and reset value, but my suggestion is to
    ''' create a new istance of NativeTimer, which uses the TimerC to 
    ''' precisely Tick a sub, but without really resetting it.
    ''' </summary>
    ''' <returns>Time elapsed (ms) since last reset</returns>
    Public Shared Function GetTimerC() As Integer
        Return [Call](Of Integer)("TIMERC")
    End Function
    ''' <summary>
    ''' Sets the "elapsed time" on the Timer A. Not recommended!
    ''' It could interfere with some vanilla scripts! Please use
    ''' TimerC or a new instance of Native Timer instead.
    ''' </summary>
    ''' <param name="Value">Milliseconds to set on the Timer</param>
    Public Shared Sub SetTimerA(Value As Integer)
        [Call]("SETTIMERA", Value)
    End Sub
    ''' <summary>
    ''' Sets the "elapsed time" on the Timer B. Not recommended!
    ''' It could interfere with some vanilla scripts! Please use
    ''' TimerC or a new instance of NativeTimer instead.
    ''' </summary>
    ''' <param name="Value">Milliseconds to set on the Timer</param>
    Public Shared Sub SetTimerB(Value As Integer)
        [Call]("SETTIMERB", Value)
    End Sub
    ''' <summary>
    ''' Sets the "elapsed time" on the Timer C. It should be safe 
    ''' to set and reset value, but my suggestion is to create 
    ''' a new istance of NativeTimer, which uses the TimerC to 
    ''' precisely Tick a sub, but without really resetting it.
    ''' </summary>
    ''' <param name="Value">Milliseconds to set on the Timer</param>
    Public Shared Sub SetTimerC(Value As Integer)
        [Call]("SETTIMERC", Value)
    End Sub
    '========================================================================================================================
    Private _lastTick As Integer
    Private _interval As Integer
    Private _startTime As Integer
    Private _onTick As Action
    Private _enabled As Boolean = False
    Private ReadOnly _tickAction As Action
    Private _maxTicksCount As Integer
    Private _CurrentTickCount As Integer
    Private _totTime As Integer
    Public Property FunctionToTick As Action
        Get
            Return _onTick
        End Get
        Set(value As Action)
            _onTick = value
        End Set
    End Property
    Public Property MaxTicks As Integer
        Get
            Return _maxTicksCount
        End Get
        Set(value As Integer)
            _maxTicksCount = value
        End Set
    End Property
    ''' <summary>
    ''' Interval in milliseconds.
    ''' </summary>
    Public Property Interval As Integer
        Get
            Return _interval
        End Get
        Set(value As Integer)
            If value < 0 Then value = 0
            _interval = value
        End Set
    End Property
    ''' <summary>
    ''' Total time elapsed from Tick start.
    ''' </summary>
    Public ReadOnly Property TotalTime As Integer
        Get
            If _enabled Then
                Return getTimerC() - _startTime
            Else
                _totTime = 0
                Return _totTime
            End If
        End Get
    End Property
    ''' <summary>
    ''' Time elapsed since last Tick.
    ''' </summary>
    Public ReadOnly Property TimeSinceLastTick As Integer
        Get
            Return getTimerC() - _lastTick
        End Get
    End Property
    ''' <summary>
    ''' Constructor to create custom Timers.
    ''' </summary>
    ''' <param name="Interval">Value in milliseconds.</param>
    ''' <param name="FunctionToTick">The function do you want to Tick.</param>
    Public Sub New(Optional Interval As Integer = 500, Optional FunctionToTick As Action = Nothing)
        _interval = Interval
        _onTick = FunctionToTick
        _lastTick = getTimerC()
        _startTime = _lastTick
        _tickAction = AddressOf Me.OnTick
        _maxTicksCount = 0
    End Sub
    ''' <summary>
    ''' Enables the Timer removing from TickHelper
    ''' </summary>
    Public Sub Enable()
        If Not _enabled Then
            _lastTick = getTimerC()
            _startTime = _lastTick
            TickHelper.Add_Internal(_tickAction)
            _enabled = True
        End If
    End Sub
    ''' <summary>
    ''' Disables the Timer removing from TickHelper
    ''' </summary>
    Public Sub Disable()
        If _enabled Then
            TickHelper.Remove_Internal(_tickAction)
            _enabled = False
        End If
    End Sub
    Private Sub OnTick()
        Dim nowMs As Integer = getTimerC()
        Dim delta As Integer = nowMs - _lastTick

        If delta >= _interval Then
            _lastTick += _interval
            _onTick?.Invoke()
            If _maxTicksCount > 0 Then
                _CurrentTickCount += 1
                If _CurrentTickCount = _maxTicksCount Then
                    TickHelper.Remove_Internal(_tickAction)
                End If
            End If
        End If
    End Sub
    '========================================================================================================================
End Class
