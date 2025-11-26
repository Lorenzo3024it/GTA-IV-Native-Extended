Imports GTA
Imports GTA.Native.Function
Public Class NativeTimer
    '========================================================================================================================
    ' Native Timers [A], [B], [C] and GameTimer (TotalGameTime)
    '------------------------------------------------------------
    ''' <summary>
    ''' The first native Timer, used by many vanilla scripts.
    ''' You can set the "elapsed time" on the Timer A, BUT NOT RECOMMENDED!
    ''' It could interfere with some vanilla scripts! Please use
    ''' TimerC or a new instance of Native Timer instead.
    ''' </summary>
    ''' <returns>Time elapsed (ms) since last reset</returns>
    Public Shared Property TimerA As Integer
        Get
            Return [Call](Of Integer)("TIMERA")
        End Get
        Set(value As Integer)
            [Call]("SETTIMERA", value)
        End Set
    End Property
    ''' <summary>
    ''' The second native Timer, used by many vanilla scripts.
    ''' You can set the "elapsed time" on the Timer B, BUT NOT RECOMMENDED!
    ''' It could interfere with some vanilla scripts! Please use
    ''' TimerC or a new instance of Native Timer instead.
    ''' </summary>
    ''' <returns>Time elapsed (ms) since last reset</returns>
    Public Shared Property TimerB As Integer
        Get
            Return [Call](Of Integer)("TIMERB")
        End Get
        Set(value As Integer)
            [Call]("SETTIMERB", value)
        End Set
    End Property
    ''' <summary>
    ''' The third native Timer, used only in vanilla Multiplayer scripts.
    ''' It should be safe to set and reset "elapsed time", but my suggestion is to
    ''' create a new istance of NativeTimer, which uses the TotalGameTime to 
    ''' precisely Tick a sub, but without really resetting it.
    ''' </summary>
    ''' <returns>Time elapsed (ms) since last reset</returns>
    Public Shared Property TimerC As Integer
        Get
            Return [Call](Of Integer)("TIMERC")
        End Get
        Set(value As Integer)
            [Call]("SETTIMERC", value)
        End Set
    End Property
    ''' <summary>
    ''' Returns played time in milliseconds.
    ''' </summary>
    Public Shared ReadOnly Property TotalGameTime As Integer
        Get
            Dim point As Native.Pointer = New Native.Pointer(GetType(Integer))
            [Call]("GET_GAME_TIMER", point)
            Return point.Value
        End Get
    End Property
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
                Return TotalGameTime - _startTime
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
            Return TotalGameTime - _lastTick
        End Get
    End Property
    ''' <summary>
    ''' Constructor to create custom Timers.
    ''' </summary>
    ''' <param name="FunctionToTick">The function do you want to Tick.</param>
    ''' <param name="Interval">Value in milliseconds.</param>
    Public Sub New(FunctionToTick As Action, Optional Interval As Integer = 500)
        _interval = Interval
        _onTick = FunctionToTick
        _lastTick = TotalGameTime
        _startTime = _lastTick
        _tickAction = AddressOf Me.OnTick
        _maxTicksCount = 0
    End Sub
    ''' <summary>
    ''' Enables the Timer removing from TickHelper
    ''' </summary>
    Public Sub Enable()
        If Not _enabled Then
            _lastTick = TotalGameTime
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
        Dim nowMs As Integer = TotalGameTime
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
