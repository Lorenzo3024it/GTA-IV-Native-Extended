Imports System.Diagnostics
Imports System.Threading
Imports GTA
Imports GTA.Native.Function
Imports NativeExtended.NativePeds.Clothes
Public NotInheritable Class SpawnerHelper
    Private Class SpawnRequest
        Public Property ModelName As String
        Public Property ModelType As SpawnType
        Public Property Position As Vector3
        'Public Property Heading As Single
        Public Property Callback As Action(Of Object)
        Public Property Requested As Boolean = False
    End Class
    Private Enum SpawnType
        Vehicle
        Ped
        [Object]
    End Enum
    Private Shared ReadOnly _pendingRequests As New List(Of SpawnRequest)
    ''' <summary>
    ''' A function to process spawning requests. Must be called in a Tick.
    ''' </summary>
    Public Shared Sub ProcessPendingRequests()
        For Each req In _pendingRequests.ToList()
            If Not req.Requested Then
                [Call]("REQUEST_MODEL", NativeModels.getHashKey(req.ModelName))
                req.Requested = True
            ElseIf [Call](Of Boolean)("HAS_MODEL_LOADED", NativeModels.getHashKey(req.ModelName)) Then
                Dim spawnedObj As Object = Nothing
                Select Case req.ModelType
                    Case SpawnType.Vehicle
                        spawnedObj = World.CreateVehicle(req.ModelName, req.Position)
                    Case SpawnType.Ped
                        spawnedObj = World.CreatePed(req.ModelName, req.Position)
                    Case SpawnType.[Object]
                        spawnedObj = World.CreateObject(req.ModelName, req.Position)
                End Select

                If spawnedObj IsNot Nothing Then
                    req.Callback?.Invoke(spawnedObj)
                End If

                _pendingRequests.Remove(req)
            End If
        Next
    End Sub
    ''' <summary>
    ''' Requires and spawn a vehicle (async)
    ''' </summary>
    Public Shared Sub SpawnVehicleAsync(model As String, pos As Vector3, heading As Single, onSpawned As Action(Of Vehicle))
        _pendingRequests.Add(New SpawnRequest With {
            .ModelName = model,
            .Position = pos,
            .Callback = Sub(o) onSpawned(DirectCast(o, Vehicle)),
            .ModelType = SpawnType.Vehicle
        })
    End Sub
    ''' <summary>
    ''' Requires and spawn an object (async)
    ''' </summary>
    Public Shared Sub SpawnPedAsync(model As String, pos As Vector3, onSpawned As Action(Of Ped))
        _pendingRequests.Add(New SpawnRequest With {
            .ModelName = model,
            .Position = pos,
            .Callback = Sub(o) onSpawned(DirectCast(o, Ped)),
            .ModelType = SpawnType.Ped
        })
    End Sub
    ''' <summary>
    ''' Requires and spawn a Ped (async)
    ''' </summary>
    Public Shared Sub SpawnObjectAsync(Model As String, Position As Vector3, onSpawned As Action(Of GTA.Object))
        _pendingRequests.Add(New SpawnRequest With {
            .ModelName = Model,
            .Position = Position,
            .Callback = Sub(o) onSpawned(DirectCast(o, Object)),
            .ModelType = SpawnType.[Object]
        })
    End Sub
    ''' <summary>
    ''' Requires a model and spawns it only when loaded.
    ''' </summary>
    Private Shared Function WaitForModel(model As String, Optional timeout As Integer = 5000) As Boolean
        [Call]("REQUEST_MODEL", model)
        Dim sw As New Stopwatch()
        sw.Start()
        Do While Not [Call](Of Boolean)("HAS_MODEL_LOADED", model)
            Thread.Sleep(10)
            If sw.ElapsedMilliseconds > timeout Then
                Return False
            End If
        Loop
        Return True
    End Function
    Public Shared Function SpawnVehicle(Model As String, Position As Vector3, Optional Timeout As Integer = 5000) As Vehicle
        If Not WaitForModel(Model, Timeout) Then Return Nothing
        Dim veh = World.CreateVehicle(Model, Position)
        Return If(veh.Exists(), veh, Nothing)
    End Function
    Public Shared Function SpawnPed(Model As String, Position As Vector3, Optional Timeout As Integer = 5000) As Ped
        If Not WaitForModel(Model, Timeout) Then Return Nothing
        Dim ped = World.CreatePed(Model, Position)
        Return If(ped.Exists(), ped, Nothing)
    End Function
    Public Shared Function SpawnObject(Model As String, Position As Vector3, Optional Timeout As Integer = 5000) As GTA.Object
        If Not WaitForModel(Model, Timeout) Then Return Nothing
        Dim prop = World.CreateObject(Model, Position)
        Return If(prop.Exists(), prop, Nothing)
    End Function

End Class

