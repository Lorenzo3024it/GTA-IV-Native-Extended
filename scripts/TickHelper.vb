Imports System.Collections.Concurrent
Imports System.ComponentModel

'Public Module TickHelperOldOld

'    Private ReadOnly TickHandlers As New List(Of Action)
'    Friend Sub Add_Internal(handler As Action)
'        If handler Is Nothing Then Exit Sub
'        If Not TickHandlers.Contains(handler) Then TickHandlers.Add(handler)
'    End Sub
'    Friend Sub Remove_Internal(handler As Action)
'        If handler Is Nothing Then Exit Sub
'        TickHandlers.Remove(handler)
'    End Sub
'    Friend Function ContainsHandler_Internal(OnTickHandler As Action) As Boolean
'        Return TickHandlers.Contains(OnTickHandler)
'    End Function
'    Public Sub ProcessAll()
'        For Each h In TickHandlers
'            Try
'                h.Invoke()
'            Catch ex As Exception
'                ' Log interno o gestione errori personalizzata
'            End Try
'        Next
'    End Sub
'    ' ▲▲▲ Fine della parte visibile pubblicamente ▲▲▲

'End Module


'Public Module TickHelperOld
'    Friend ReadOnly _TickActions As New List(Of Action)
'    Private _IsRunn As Boolean = False
'    Public ReadOnly Property isRunning As Boolean
'        Get
'            Return getIsRunning_Internal()
'        End Get
'    End Property
'    Private Function getIsRunning_Internal() As Boolean
'        Return _IsRunn AndAlso _TickActions.Count > 0
'    End Function
'    Friend Sub Add_Internal(action As Action)
'        If action IsNot Nothing AndAlso Not _TickActions.Contains(action) Then
'            _TickActions.Add(action)
'        End If
'    End Sub
'    Friend Function ContainsHandler_Internal(OnTickHandler As Action) As Boolean
'        Return _TickActions.Contains(OnTickHandler)
'    End Function
'    Friend Sub Remove_Internal(action As Action)
'        If action IsNot Nothing AndAlso _TickActions.Contains(action) Then
'            _TickActions.Remove(action)
'        End If
'    End Sub
'    Public Sub ProcessAll()
'        If _TickActions.Count = 0 Then
'            _IsRunn = False
'            Exit Sub
'        End If

'        _IsRunn = True
'        For Each action In _TickActions.ToArray()
'            Try
'                action.Invoke()
'            Catch ex As Exception

'            End Try
'        Next

'        _IsRunn = False
'    End Sub

'End Module
''' <summary>
''' An utility tool to process all Ticks added.
''' </summary>
Public Module TickHelper
    Friend ReadOnly _TickActions As New List(Of Action)
    Public Sub ProcessAll()
        If _TickActions.Count = 0 Then
            Exit Sub
        End If

        For Each action In _TickActions.ToArray()
            Try
                action.Invoke()
            Catch ex As Exception

            End Try
        Next
    End Sub
    Friend Sub Add_Internal(action As Action)
        If action IsNot Nothing AndAlso Not _TickActions.Contains(action) Then
            _TickActions.Add(action)
        End If
    End Sub
    Friend Function ContainsHandler_Internal(OnTickHandler As Action) As Boolean
        Return _TickActions.Contains(OnTickHandler)
    End Function
    Friend Sub Remove_Internal(action As Action)
        If action IsNot Nothing AndAlso _TickActions.Contains(action) Then
            _TickActions.Remove(action)
        End If
    End Sub
End Module




'Public NotInheritable Class TickHelperOLOLD
'    Private Shared ReadOnly TickActions As New ConcurrentBag(Of Action)
'    Private Shared ReadOnly ToRemove As New List(Of Action)
'    ''' <summary>
'    ''' Adds a Sub to the list of Ticks.
'    ''' </summary>
'    Public Shared Sub Add(OnTick As Action)
'        If OnTick Is Nothing Then Exit Sub
'        If Not TickActions.Contains(OnTick) Then
'            TickActions.Add(OnTick)
'        End If
'    End Sub
'    ''' <summary>
'    ''' Removes a Sub from the list.
'    ''' </summary>
'    Public Shared Sub Remove(OnTick As Action)
'        ToRemove.Add(OnTick)
'    End Sub
'    ''' <summary>
'    ''' Clear all Subs from the list.
'    ''' </summary>
'    Public Shared Sub Clear()
'        TickHelper.Clear()
'    End Sub
'    ''' <summary>
'    ''' Runs all Subs added. Must be run in a Tick inside 
'    ''' the main script.
'    ''' </summary>
'    Public Shared Sub ProcessAll()
'        ' Rimuove le funzioni in coda di rimozione
'        If ToRemove.Count > 0 Then
'            Dim remaining = TickActions.Except(ToRemove).ToList()
'            ToRemove.Clear()

'            ' Rigenera la bag (ConcurrentBag non supporta Remove)
'            Dim newBag As New ConcurrentBag(Of Action)(remaining)
'            Threading.Interlocked.Exchange(TickActions, newBag)
'        End If

'        ' Esegue tutte le funzioni registrate
'        For Each a In TickActions
'            Try
'                a.Invoke()
'            Catch ex As Exception
'                ' Opzionale: log degli errori
'                ' Game.DisplayText($"Errore Tick: {ex.Message}")
'            End Try
'        Next
'    End Sub

'End Class
'Public Module TickHelperAutoOLD
'    Private ReadOnly TickHandlers As New List(Of Action)

'    Public Sub Add(handler As Action)
'        If Not TickHandlers.Contains(handler) Then TickHandlers.Add(handler)
'    End Sub

'    Public Sub Remove(handler As Action)
'        If TickHandlers.Contains(handler) Then TickHandlers.Remove(handler)
'    End Sub

'    Public Function ContainsHandler(handler As Action) As Boolean
'        Return TickHandlers.Contains(handler)
'    End Function

'    Public Sub ProcessTick()
'        For Each h In TickHandlers
'            Try
'                h.Invoke()
'            Catch ex As Exception
'                ' Gestione errori/log
'            End Try
'        Next
'    End Sub
'End Module



