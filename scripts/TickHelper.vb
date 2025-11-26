Imports System.Collections.Concurrent
Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' An utility tool to process all Ticks added.
''' </summary>
Public Module TickHelper
    Friend ReadOnly _TickActions As New List(Of Action)
    Private _lastProcessed As DateTime = DateTime.MinValue
    Private _lastProcessHadTicks As Boolean = False
    Private _lastTickTime As DateTime = DateTime.UtcNow

    ''' <summary>
    ''' Return True if ProcessAll was ticked in last 500ms.
    ''' </summary>
    Public ReadOnly Property isRunning As Boolean
        Get
            Return (DateTime.UtcNow - _lastProcessed).TotalSeconds < 0.5
        End Get
    End Property

    ''' <summary>
    ''' Returns time elapsed (ms) after last tick and if 
    ''' TickHelper had any action to process.
    ''' </summary>
    Public ReadOnly Property LastTick As (Integer, Boolean)
        Get
            Return (CInt((DateTime.UtcNow - _lastTickTime).TotalMilliseconds), _lastProcessHadTicks)
        End Get
    End Property
    Public ReadOnly Property Ticks As List(Of Action)
        Get
            Return _TickActions
        End Get
    End Property

    ''' <summary>
    ''' Process all ticks added to the TickHelper.
    ''' Works with internal ticks and NativeTimer instances.
    ''' </summary>
    Public Sub ProcessAll()
        _lastProcessed = DateTime.UtcNow
        _lastProcessHadTicks = (_TickActions.Count > 0)
        _lastTickTime = DateTime.UtcNow

        If _TickActions.Count = 0 Then Exit Sub

        For Each action In _TickActions.ToArray()
            Try
                action.Invoke()
            Catch ex As Exception
                Tools.WriteLog("TickHelper error: cannot process ticks. " & vbCrLf & " " & ex.Message, WriteDateAndTime:=True)
            End Try
        Next
    End Sub

    '------------------------------------------------------------------------------------------------------------------------
    Friend Sub Add_Internal(action As Action)
        If action IsNot Nothing AndAlso Not _TickActions.Contains(action) Then
            _TickActions.Add(action)
        End If
    End Sub
    Friend Function ContainsHandler_Internal(Action As Action) As Boolean
        Return _TickActions.Contains(Action)
    End Function
    Friend Sub Remove_Internal(action As Action)
        If action IsNot Nothing AndAlso _TickActions.Contains(action) Then
            _TickActions.Remove(action)
        End If
    End Sub
End Module


'''' <summary>
'''' Adds a custom Tick action.
'''' </summary>
'''' <param name="FunctionToTick">The function do you want to add to TickHelper.</param>
'Public Sub Add(FunctionToTick As Action)
'    If FunctionToTick Is Nothing Then
'        Tools.WriteLog("TickHelper error: action " & FunctionToTick.ToString & "is not valid. Cannot be added.", WriteDateAndTime:=True)
'    ElseIf _TickActions.Contains(FunctionToTick) Then
'        Tools.WriteLog("TickHelper error: action " & FunctionToTick.ToString & "yet added.", WriteDateAndTime:=True)
'    Else
'        Add_Internal(FunctionToTick)
'    End If
'End Sub
'''' <summary>
'''' Returns True if a custom Tick action has been added to TickHelper.
'''' </summary>
'Public Function HasBeenAdded(FunctionToTick As Action) As Boolean
'    Return ContainsHandler_Internal(FunctionToTick)
'End Function
'''' <summary>
'''' Removes a custom Tick action.
'''' </summary>
'''' <param name="FunctionToTick">The function do you want to remove from TickHelper.</param>
'Public Sub Remove(FunctionToTick As Action)
'    If FunctionToTick Is Nothing Then
'        Tools.WriteLog("TickHelper error: action " & FunctionToTick.ToString & "is not valid. Cannot be removed.", WriteDateAndTime:=True)
'    ElseIf Not _TickActions.Contains(FunctionToTick) Then
'        Tools.WriteLog("TickHelper error: action " & FunctionToTick.ToString & "still not added.", WriteDateAndTime:=True)
'    Else
'        Remove_Internal(FunctionToTick)
'    End If
'End Sub




