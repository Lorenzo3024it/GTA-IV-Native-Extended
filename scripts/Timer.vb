Imports GTA
Imports GTA.Native.Function
Imports System.Runtime.CompilerServices
'=================================================================================================================================
' Shared - Timer [A] [B] [C] & [GameTimer] (TotalGameTimer)
'----------------------------------------------------------------
Partial Public Class NativeTimer
    Private Shared ReadOnly pointGameTimer As New Native.Pointer(GetType(Integer))
    ''' <summary>
    ''' The first native Timer, used by many vanilla scripts.
    ''' Setting this timer is NOT RECOMMENDED because it may interfere with vanilla scripts.
    ''' </summary>
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
    ''' Setting this timer is NOT RECOMMENDED because it may interfere with vanilla scripts.
    ''' </summary>
    Public Shared Property TimerB As Integer
        Get
            Return [Call](Of Integer)("TIMERB")
        End Get
        Set(value As Integer)
            [Call]("SETTIMERB", value)
        End Set
    End Property
    ''' <summary>
    ''' The third native Timer, used mostly in vanilla multiplayer scripts.
    ''' </summary>
    Public Shared Property TimerC As Integer
        Get
            Return [Call](Of Integer)("TIMERC")
        End Get
        Set(value As Integer)
            [Call]("SETTIMERC", value)
        End Set
    End Property
    ''' <summary>
    ''' Returns played time in milliseconds (GET_GAME_TIMER).
    ''' Is affected to TimeScale.
    ''' </summary>
    Public Shared ReadOnly Property TotalGameTime As Integer
        Get
            [Call]("GET_GAME_TIMER", pointGameTimer)
            Return pointGameTimer.Value
        End Get
    End Property
End Class
'=================================================================================================================================
' Instance 
'----------
Partial Public Class NativeTimer
    ''' <summary>
    ''' Describes how to behave if the target action is already waiting.
    ''' </summary>
    Public Enum WaitConflictBehaviour
        ''' <summary>
        ''' Default. Does not enqueue or override. The request is ignored if already waiting.
        ''' </summary>
        Ignore = 0
        ''' <summary>
        ''' Enqueues the new wait and will execute it when the current wait completes.
        ''' </summary>
        Enqueue = 1
        ''' <summary>
        ''' Overrides current wait immediately, discarding the current wait and clearing the queue (by default).
        ''' </summary>
        OverrideNow = 2
    End Enum
    ''' <summary>
    ''' State of a single action within this NativeTimer.
    ''' </summary>
    Public Enum ActionState
        Stopped = 0
        Running = 1
        Waiting = 2
        WaitingUntil = 3
        WaitingBeforeInvoke = 4
    End Enum

    Private ReadOnly _actions As New Dictionary(Of Action, ActionEntry)()

    <ThreadStatic>
    Private Shared _currentExecutingAction As Action
    ''' <summary>
    ''' Creates a NativeTimer. It will run only if TickHelper.ProcessAll is called by the host script.
    ''' </summary>
    ''' <param name="Interval">Tick interval in milliseconds (> 0).</param>
    Public Sub New(Optional Interval As Integer = 1) ', Optional GameTimeRelated As Boolean = True)
        If Interval < 0 Then Interval = 0
        _interval = Interval
        _tickAction = AddressOf Me.OnTick
        _enabled = False
        _lastTick = TotalGameTime
        _startTime = _lastTick
    End Sub
    ''' <summary>
    ''' Gets or sets the tick interval (milliseconds) for this timer.
    ''' Changes take effect immediately and will affect all actions started from this timer.
    ''' </summary>
    Public Property Interval As Integer
        Get
            SyncLock _sync
                Return _interval
            End SyncLock
        End Get
        Set(value As Integer)
            If value < 0 Then value = 0
            SyncLock _sync
                _interval = value
            End SyncLock
        End Set
    End Property
    ''' <summary>
    ''' True if this NativeTimer is currently registered into TickHelper.
    ''' False if no action are added to the timer.
    ''' </summary>
    Public ReadOnly Property IsProcessing As Boolean
        Get
            SyncLock _sync
                Return _enabled AndAlso TickHelper.ContainsHandler_Internal(_tickAction)
            End SyncLock
        End Get
    End Property
    ''' <summary>
    ''' Total time elapsed since this timer was first enabled (milliseconds).
    ''' </summary>
    Public ReadOnly Property TotalTime As Integer
        Get
            SyncLock _sync
                If Not _enabled Then Return 0
                Return TotalGameTime - _startTime
            End SyncLock
        End Get
    End Property
    ''' <summary>
    ''' Starts (registers) an action on this timer.
    ''' If MaxTicks is greater than 0, the action will be automatically stopped after that many executions.
    ''' </summary>
    ''' <param name="action">The action to execute.</param>
    ''' <param name="MaxTicks">-1 for unlimited. If &gt; 0, auto-stop after N executions.</param>
    Public Sub Start(Action As Action, Optional MaxTicks As Integer = -1)
        If Action Is Nothing Then Return
        If MaxTicks = 0 Then MaxTicks = -1

        SyncLock _sync
            Try
                Dim entry As ActionEntry = Nothing
                If Not _actions.TryGetValue(Action, entry) Then
                    entry = New ActionEntry With {.Handler = Action}
                    _actions(Action) = entry
                End If

                entry.MaxTicks = MaxTicks
                entry.ExecutedTicks = 0
                entry.State = ActionState.Running

                ' Reset waits
                entry.HasActiveWait = False
                entry.WaitQueue.Clear()

                EnsureEnabled_NoLock()

                If _actions.Count > 5000 Then
                    Tools.WriteLog($"NativeTimer warning: very high action count ({_actions.Count}).")
                End If

            Catch ex As Exception
                Tools.WriteLog("Error in NativeTimer.Start(action).", ex)
            End Try
        End SyncLock
    End Sub
    ''' <summary>
    ''' Stops (unregisters) a specific action from this timer.
    ''' If no actions remain, the timer is automatically deregistered from TickHelper.
    ''' </summary>
    Public Sub [Stop](Action As Action)
        If Action Is Nothing Then Return

        SyncLock _sync
            Try
                _actions.Remove(Action)

                If _actions.Count = 0 Then
                    Disable_NoLock()
                End If

            Catch ex As Exception
                Tools.WriteLog("Error in NativeTimer.Stop(action).", ex)
            End Try
        End SyncLock
    End Sub
    ''' <summary>
    ''' Stops all actions and deregisters the timer from TickHelper.
    ''' </summary>
    Public Sub StopAll()
        SyncLock _sync
            Try
                _actions.Clear()
                Disable_NoLock()
            Catch ex As Exception
                Tools.WriteLog("Error in NativeTimer.StopAll().", ex)
            End Try
        End SyncLock
    End Sub
    ''' <summary>
    ''' Returns the state of the given action. If action is not registered, returns Stopped.
    ''' </summary>
    Public Function GetState(action As Action) As ActionState
        If action Is Nothing Then Return ActionState.Stopped
        SyncLock _sync
            Dim e As ActionEntry = Nothing
            If _actions.TryGetValue(action, e) Then Return e.State
        End SyncLock
        Return ActionState.Stopped
    End Function
    ''' <summary>
    ''' Returns remaining ticks until auto-stop for this action.
    ''' Returns -1 if maxTicks was not set (unlimited) or action not found.
    ''' </summary>
    Public Function GetRemainingTicks(action As Action) As Integer
        If action Is Nothing Then Return -1
        SyncLock _sync
            Dim e As ActionEntry = Nothing
            If Not _actions.TryGetValue(action, e) Then Return -1
            If e.MaxTicks <= 0 Then Return -1
            Dim remaining As Integer = e.MaxTicks - e.ExecutedTicks
            If remaining < 0 Then remaining = 0
            Return remaining
        End SyncLock
    End Function
    ''' <summary>
    ''' Returns True if the action is registered and is currently active.
    ''' If includeWaiting is False, only Running state counts as active.
    ''' </summary>
    Public Function IsRunning(action As Action, Optional includeWaiting As Boolean = True) As Boolean
        If action Is Nothing Then Return False
        SyncLock _sync
            Dim e As ActionEntry = Nothing
            If Not _actions.TryGetValue(action, e) Then Return False
            If includeWaiting Then
                Return e.State <> ActionState.Stopped
            Else
                Return e.State = ActionState.Running
            End If
        End SyncLock
    End Function
    ''===========
    '' WAIT
    ''======
    '''' <summary>
    '''' Pauses the specified action for a delay (non-blocking).
    '''' </summary>
    '''' <param name="Tick">
    '''' The owner action to pause.
    '''' If Nothing, the currently executing action is used.
    '''' Valid when called from within an action executed by this NativeTimer;
    '''' otherwise the call is ignored.
    '''' </param>
    '''' <param name="Time">Delay in milliseconds.</param>
    '''' <param name="Behaviour">Conflict behaviour if the action is already waiting.</param>
    'Public Sub Wait(Optional Time As Integer = 1, Optional Tick As Action = Nothing, Optional Behaviour As WaitConflictBehaviour = WaitConflictBehaviour.Ignore)
    '    If Time <= 0 Then Return

    '    Dim target = ResolveTargetAction(Tick)
    '    If target Is Nothing Then
    '        Tools.WriteLog("NativeTimer.Wait called without a valid action context.", , True)
    '        Return
    '    End If

    '    Dim req As New WaitRequest With {
    '        .Kind = WaitKind.Delay,
    '        .TargetMs = uncheckedAdd(TotalGameTime, Time)
    '    }

    '    SyncLock _sync
    '        Dim e As ActionEntry = Nothing
    '        If Not _actions.TryGetValue(target, e) Then Return
    '        EnqueueOrApplyWait_NoLock(e, req, Behaviour)
    '    End SyncLock
    'End Sub
    '''' <summary>
    '''' Pauses the specified action until the condition becomes True (non-blocking).
    '''' </summary>
    '''' <param name="Tick">
    '''' The owner action to pause.
    '''' If Nothing, the currently executing action is used.
    '''' Valid when called from within an action executed by this NativeTimer;
    '''' otherwise the call is ignored.
    '''' </param>
    '''' <param name="Condition">Condition to evaluate; when returns True the wait completes.</param>
    '''' <param name="Behaviour">Conflict behaviour if the action is already waiting.</param>
    'Public Sub WaitUntil(Condition As Func(Of Boolean), Optional Tick As Action = Nothing, Optional Behaviour As WaitConflictBehaviour = WaitConflictBehaviour.Ignore)
    '    If Condition Is Nothing Then Return

    '    Dim target = ResolveTargetAction(Tick)
    '    If target Is Nothing Then
    '        Tools.WriteLog("NativeTimer.WaitUntil called without a valid action context.", , True)
    '        Return
    '    End If

    '    Dim req As New WaitRequest With {.Kind = WaitKind.Until, .Condition = Condition}

    '    SyncLock _sync
    '        Dim e As ActionEntry = Nothing
    '        If Not _actions.TryGetValue(target, e) Then Return
    '        EnqueueOrApplyWait_NoLock(e, req, Behaviour)
    '    End SyncLock
    'End Sub
    '''' <summary>
    '''' Pauses the specified action and invokes a callback after a delay (non-blocking).
    '''' Optionally stops the owner action after invoking the callback.
    '''' </summary>
    '''' <param name="Tick">
    '''' The owner action to pause.
    '''' If Nothing, the currently executing action is used.
    '''' Valid when called from within an action executed by this NativeTimer;
    '''' otherwise the call is ignored.
    '''' </param>
    '''' <param name="ActionToInvoke">Callback to invoke when delay elapsed.</param>
    '''' <param name="Time">Delay in milliseconds.</param>
    '''' <param name="StopAfterInvoke">If True, stops OnTick action after invoking the callback.</param>
    '''' <param name="Behaviour">Conflict behaviour if the action is already waiting.</param>
    'Public Sub WaitAndInvoke(ActionToInvoke As Action, Time As Integer, Optional Tick As Action = Nothing, Optional StopAfterInvoke As Boolean = False, Optional Behaviour As WaitConflictBehaviour = WaitConflictBehaviour.Ignore)
    '    If Tick Is Nothing Then Return
    '    If Time < 0 Then Time = 0

    '    Dim target = ResolveTargetAction(Tick)
    '    If target Is Nothing Then
    '        Tools.WriteLog("NativeTimer.WaitAndInvoke called without a valid action context.", , True)
    '        Return
    '    End If

    '    Dim req As New WaitRequest With {.Kind = WaitKind.DelayThenInvoke,
    '                                     .TargetMs = uncheckedAdd(TotalGameTime, Time),
    '                                     .InvokeAction = ActionToInvoke,
    '                                     .StopAfterInvoke = StopAfterInvoke}
    '    SyncLock _sync
    '        Dim e As ActionEntry = Nothing
    '        If Not _actions.TryGetValue(target, e) Then Return
    '        EnqueueOrApplyWait_NoLock(e, req, Behaviour)
    '    End SyncLock
    'End Sub
End Class
'=================================================================================================================================
' Internals
'------------
Partial Public Class NativeTimer
    Private ReadOnly _sync As New Object()
    Private _interval As Integer
    Private _enabled As Boolean
    Private _lastTick As Integer
    Private _startTime As Integer
    Private ReadOnly _tickAction As Action
    '---
    Private Const WAIT_QUEUE_WARN_THRESHOLD As Integer = 256
    Private Const WAIT_QUEUE_HARD_LIMIT As Integer = 4096
    Private Sub EnsureEnabled_NoLock()
        If _enabled Then Return
        If _actions.Count = 0 Then Return

        _lastTick = TotalGameTime
        _startTime = _lastTick

        If Not TickHelper.ContainsHandler_Internal(_tickAction) Then
            TickHelper.Add_Internal(_tickAction)
        End If

        _enabled = True
    End Sub
    Private Sub Disable_NoLock()
        If Not _enabled Then Return

        If TickHelper.ContainsHandler_Internal(_tickAction) Then
            TickHelper.Remove_Internal(_tickAction)
        End If

        _enabled = False
    End Sub
    Private Sub OnTick()
        ' Fast exit if not enabled (should not happen if properly registered, but safe).
        SyncLock _sync
            If Not _enabled Then Return
        End SyncLock

        Dim nowGame As Integer = TotalGameTime

        SyncLock _sync
            Dim delta As Integer = nowGame - _lastTick
            If delta < _interval Then Return

            ' keep drift stable
            _lastTick = uncheckedAdd(_lastTick, _interval)
        End SyncLock

        ' Snapshot actions to avoid iterator issues if modified during execution.
        Dim snapshot As ActionEntry()
        SyncLock _sync
            snapshot = _actions.Values.ToArray()
        End SyncLock

        Dim actionsToStop As New List(Of Action)()

        For Each e In snapshot
            If e Is Nothing OrElse e.Handler Is Nothing Then Continue For

            Try
                ' Process waiting state BEFORE invoking.
                If ProcessWaitState(e, actionsToStop) Then
                    ' still waiting -> skip invoke
                    Continue For
                End If

                ' Execute.
                _currentExecutingAction = e.Handler
                e.State = ActionState.Running
                e.Handler.Invoke()
                _currentExecutingAction = Nothing

                ' Count ticks for maxTicks.
                If e.MaxTicks > 0 Then
                    e.ExecutedTicks += 1
                    If e.ExecutedTicks >= e.MaxTicks Then
                        actionsToStop.Add(e.Handler)
                        e.State = ActionState.Stopped
                    End If
                End If

            Catch ex As Exception
                _currentExecutingAction = Nothing
                Tools.WriteLog("Error in NativeTimer.OnTick for action: " & SafeMethodName(e.Handler) & ".", ex)
            End Try
        Next

        If actionsToStop.Count > 0 Then
            SyncLock _sync
                For Each a In actionsToStop
                    _actions.Remove(a)
                Next
                If _actions.Count = 0 Then
                    Disable_NoLock()
                End If
            End SyncLock
        End If
    End Sub
    Private Function ProcessWaitState(e As ActionEntry, actionsToStop As List(Of Action)) As Boolean
        If e Is Nothing Then Return False

        ' If no active wait, ensure state is Running unless Stopped.
        If Not e.HasActiveWait Then
            If e.State <> ActionState.Stopped Then e.State = ActionState.Running
            Return False
        End If

        Dim req As WaitRequest = e.ActiveWait
        Select Case req.Kind

            Case WaitKind.Delay
                If HasTimeElapsed(TotalGameTime, req.TargetMs) Then
                    CompleteCurrentWait_AdvanceQueue(e)
                    Return False
                Else
                    e.State = ActionState.Waiting
                    Return True
                End If

            Case WaitKind.Until
                Dim ok As Boolean = False
                Try
                    ok = (req.Condition IsNot Nothing AndAlso req.Condition.Invoke())
                Catch ex As Exception
                    Tools.WriteLog("Error evaluating NativeTimer.WaitUntil condition.", ex)
                    ok = True ' fail-open to avoid deadlocks
                End Try

                If ok Then
                    CompleteCurrentWait_AdvanceQueue(e)
                    Return False
                Else
                    e.State = ActionState.WaitingUntil
                    Return True
                End If

            Case WaitKind.DelayThenInvoke
                If HasTimeElapsed(TotalGameTime, req.TargetMs) Then
                    ' invoke callback
                    If req.InvokeAction IsNot Nothing Then
                        Try
                            req.InvokeAction.Invoke()
                        Catch ex As Exception
                            Tools.WriteLog("Error in NativeTimer.WaitAndInvoke callback.", ex)
                        End Try
                    End If

                    ' stop owner action if required (defer)
                    If req.StopAfterInvoke AndAlso e.Handler IsNot Nothing Then
                        actionsToStop.Add(e.Handler)
                        ' we consider it handled; no further queue needed
                        e.HasActiveWait = False
                        e.WaitQueue.Clear()
                        e.State = ActionState.Running
                        Return True ' skip invoke this tick
                    End If

                    CompleteCurrentWait_AdvanceQueue(e)
                    Return False
                Else
                    e.State = ActionState.WaitingBeforeInvoke
                    Return True
                End If

            Case Else
                ' Unknown wait kind: fail-open.
                e.HasActiveWait = False
                e.State = ActionState.Running
                Return False
        End Select
    End Function
    Private Sub CompleteCurrentWait_AdvanceQueue(e As ActionEntry)
        e.HasActiveWait = False

        ' If queued waits exist, start next immediately.
        If e.WaitQueue.Count > 0 Then
            Dim nextReq As WaitRequest = e.WaitQueue.Dequeue()
            ApplyWaitRequest_NoLock(e, nextReq)
        Else
            e.State = ActionState.Running
        End If
    End Sub
    Private Sub EnqueueOrApplyWait_NoLock(e As ActionEntry, req As WaitRequest, behaviour As WaitConflictBehaviour)
        If e Is Nothing Then Return

        ' Not waiting -> apply immediately.
        If Not e.HasActiveWait Then
            ApplyWaitRequest_NoLock(e, req)
            Return
        End If

        Select Case behaviour
            Case WaitConflictBehaviour.Ignore
                ' ignore
                Return

            Case WaitConflictBehaviour.Enqueue
                If e.WaitQueue.Count >= WAIT_QUEUE_HARD_LIMIT Then
                    Tools.WriteLog($"NativeTimer: wait queue limit reached ({WAIT_QUEUE_HARD_LIMIT}). New wait ignored. Action={SafeMethodName(e.Handler)}", , True)
                    Return
                End If

                e.WaitQueue.Enqueue(req)

                If e.WaitQueue.Count = WAIT_QUEUE_WARN_THRESHOLD Then
                    Tools.WriteLog($"NativeTimer: wait queue is growing (>= {WAIT_QUEUE_WARN_THRESHOLD}). Action={SafeMethodName(e.Handler)}", , True)
                End If

            Case WaitConflictBehaviour.OverrideNow
                ' override current wait immediately
                e.HasActiveWait = False
                e.WaitQueue.Clear()
                ApplyWaitRequest_NoLock(e, req)

            Case Else
                Return
        End Select
    End Sub
    Private Sub ApplyWaitRequest_NoLock(e As ActionEntry, req As WaitRequest)
        e.ActiveWait = req
        e.HasActiveWait = True

        Select Case req.Kind
            Case WaitKind.Delay : e.State = ActionState.Waiting
            Case WaitKind.Until : e.State = ActionState.WaitingUntil
            Case WaitKind.DelayThenInvoke : e.State = ActionState.WaitingBeforeInvoke
            Case Else : e.State = ActionState.Running
        End Select
    End Sub
    '========================================================================================================================
    ' Helpers
    '========================================================================================================================
    Private Shared Function uncheckedAdd(a As Integer, b As Integer) As Integer
        Dim ua As UInteger = CUInt(a)
        Dim ub As UInteger = CUInt(b)
        Return CInt(ua + ub)
    End Function
    Private Shared Function HasTimeElapsed(nowMs As Integer, targetMs As Integer) As Boolean
        Dim delta As UInteger = CUInt(nowMs) - CUInt(targetMs)
        Return delta < &H80000000UI
    End Function
    Private Shared Function SafeMethodName(a As Action) As String
        Try
            If a Is Nothing OrElse a.Method Is Nothing Then Return "<null>"
            Return a.Method.DeclaringType.FullName & "." & a.Method.Name
        Catch
            Return "<unknown>"
        End Try
    End Function
    Private Shared Function ResolveTargetAction(explicitAction As Action) As Action
        If explicitAction IsNot Nothing Then Return explicitAction
        Return _currentExecutingAction
    End Function
    Private Enum WaitKind
        Delay = 0
        Until = 1
        DelayThenInvoke = 2
    End Enum
    Private Structure WaitRequest
        Public Kind As WaitKind

        ' Delay
        Public TargetMs As Integer

        ' Until
        Public Condition As Func(Of Boolean)

        ' Invoke
        Public InvokeAction As Action
        Public StopAfterInvoke As Boolean
    End Structure
    Private NotInheritable Class ActionEntry
        Public Handler As Action

        Public MaxTicks As Integer = -1            ' -1 = unlimited
        Public ExecutedTicks As Integer = 0

        Public State As ActionState = ActionState.Stopped

        ' Active wait info
        Public HasActiveWait As Boolean = False
        Public ActiveWait As WaitRequest

        ' Queue of future waits (per action)
        Public ReadOnly WaitQueue As New Queue(Of WaitRequest)()
    End Class
End Class
'=================================================================================================================================


