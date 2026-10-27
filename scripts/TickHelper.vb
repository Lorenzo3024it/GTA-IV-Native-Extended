Imports System.Collections.Concurrent
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports GTA
Imports NativeExtended.NativeWorld

''' <summary>
''' An utility tool to process all Ticks added.
''' </summary>
Public NotInheritable Class TickHelper
    Friend Shared ReadOnly _TickActions As New List(Of Action)
    Private Shared _lastProcessed As DateTime = DateTime.MinValue
    Private Shared _lastProcessHadTicks As Boolean = False
    Private Shared _lastTickTime As DateTime = DateTime.UtcNow
    Private Shared _HasWrittenWelcomeMessage As Boolean = False
    '-----
    Private Shared ReadOnly _InitializationLock As New Object()
    Private Shared _HasInitializedLibrary As Boolean = False
    '-----
    Private Sub New() : End Sub
    ''' <summary>
    ''' Return True if ProcessAll was ticked in last 500ms.
    ''' </summary>
    Public Shared ReadOnly Property IsRunning As Boolean
        Get
            If _lastProcessed = DateTime.MinValue Then
                Return False
            End If
            Return (DateTime.UtcNow - _lastProcessed).TotalMilliseconds < 500.0
        End Get
    End Property

    ''' <summary>
    ''' Returns time elapsed (milliseconds) after last tick and if 
    ''' TickHelper had any action to process.
    ''' </summary>
    Public Shared ReadOnly Property LastTick As (TimeElapsed As Integer, HasTickToProcess As Boolean)
        Get
            Dim elapsed As Integer
            If _lastTickTime = DateTime.MinValue Then
                elapsed = Integer.MaxValue
            Else
                Dim milliseconds As Double =
                    (DateTime.UtcNow - _lastTickTime).TotalMilliseconds
                If milliseconds < 0.0 Then
                    milliseconds = 0.0
                End If
                If milliseconds > Integer.MaxValue Then
                    elapsed = Integer.MaxValue
                Else
                    elapsed = CInt(milliseconds)
                End If
            End If
            Return (elapsed, _lastProcessHadTicks)
        End Get
    End Property

    Public Shared ReadOnly Property Ticks As List(Of Action)
        Get
            SyncLock _TickActions
                Return New List(Of Action)(_TickActions)
            End SyncLock
        End Get
    End Property
    ''' <summary>
    ''' Initializes library and process all ticks (also NativeTimer instances). Must be called on tick (1ms suggested).
    ''' </summary>
    Public Shared Sub ProcessAll()
        If Not _HasInitializedLibrary Then InitializeLibrary_Internal()

        Dim TickSnapshot As Action()

        SyncLock _TickActions
            TickSnapshot = _TickActions.ToArray()
        End SyncLock

        _lastProcessed = DateTime.UtcNow
        _lastTickTime = _lastProcessed
        _lastProcessHadTicks = Array.Exists(TickSnapshot, Function(Handler) Handler IsNot Nothing)

        If TickSnapshot.Length = 0 Then Exit Sub

        For Each OnTickHandler As Action In TickSnapshot
            If OnTickHandler Is Nothing Then
                Continue For
            End If
            Try
                OnTickHandler.Invoke()
            Catch ex As Exception
                Tools.WriteLog("Error in TickHelper.ProcessAll.", ex)
            End Try
        Next
    End Sub
    '------------------------------------------------------------------------------------------------------------------------

    Friend Shared Sub Add_Internal(OnTickHandler As Action)
        If OnTickHandler Is Nothing Then Exit Sub

        Try
            SyncLock _TickActions
                If Not _TickActions.Contains(OnTickHandler) Then
                    _TickActions.Add(OnTickHandler)
                End If
            End SyncLock
        Catch ex As Exception
            Dim HandlerName As String = "Nothing"
            Try
                HandlerName = OnTickHandler.ToString()
            Catch
            End Try
            Dim c As Checkpoint
            Tools.WriteLog("Error in TickHelper.Add_Internal: cannot add " & HandlerName.ToString & "OnTick void.", ex)
        End Try
    End Sub

    Friend Shared Function ContainsHandler_Internal(OnTickHandler As Action) As Boolean
        If OnTickHandler Is Nothing Then Return False

        Try
            SyncLock _TickActions
                Return _TickActions.Contains(OnTickHandler)
            End SyncLock
        Catch ex As Exception
            Dim HandlerName As String = "Nothing"
            Try
                HandlerName = OnTickHandler.ToString()
            Catch
            End Try
            Tools.WriteLog("Error in TickHelper.ContainsHandler_Internal on void " & HandlerName.ToString, ex)
            Return False
        End Try
    End Function

    Friend Shared Sub Remove_Internal(OnTickHandler As Action)
        If OnTickHandler Is Nothing Then Exit Sub

        Try
            SyncLock _TickActions
                If _TickActions.Contains(OnTickHandler) Then
                    _TickActions.Remove(OnTickHandler)
                End If
            End SyncLock
        Catch ex As Exception
            Dim HandlerName As String = "Nothing"
            Try
                HandlerName = OnTickHandler.ToString()
            Catch
            End Try
            Tools.WriteLog("Error in TickHelper.Remove_Internal: cannot remove " & OnTickHandler.ToString & "OnTick void.", ex)
        End Try
    End Sub

    Friend Shared Sub WriteHeaderMessage()
        Try
            Dim logPath As String = Tools.GetFullLogPath(Tools.DefaultLogPath)
            Using sw As New StreamWriter(logPath, False)
                sw.WriteLine("|============================================================|")
                sw.WriteLine("| ------   NativeExtended-IV 1.2.0 by Lorenzo3024it   ------ |")
                sw.WriteLine("| ----------    Thank you for using my mods :)   ----------- |")
                sw.WriteLine("| -------     If you need help please contact me     ------- |")
                sw.WriteLine("| -- bttf4thebigRelease@gmail.com / lorenzo3024@hotmail.it   |")
                sw.WriteLine("|============================================================|")
                sw.WriteLine("")
            End Using
            _HasWrittenWelcomeMessage = True
        Catch
        End Try
    End Sub
    '------------------------------------------------------------------------------------------------------------------------
    Private Shared Sub InitializeLibrary_Internal()
        SyncLock _InitializationLock
            If _HasInitializedLibrary Then Return

            If Not _HasWrittenWelcomeMessage Then
                WriteHeaderMessage()
                Tools.WriteLog(" TickHelper started. Inizializing library...")
                _HasWrittenWelcomeMessage = True
            End If
            Try
                Respawn.Initialize_Internal()
                NativeGarage.Tick = True
                Tools.WriteLog("   All ticks initialized successfully!")
                _HasInitializedLibrary = True
            Catch ex As Exception
                Tools.WriteLog("   Error in TickHelper.InitializeLibrary_Internal.", ex)
            End Try
        End SyncLock
    End Sub
End Class




