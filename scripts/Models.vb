Imports System.CodeDom
Imports System.Windows.Forms
Imports GTA
Imports GTA.Native.Function
Imports NativeExtended.NativeVehicle
Public Class NativeModel
    ' Private Shared ReadOnly _tempRequestedModel As String
    Public Shared ReadOnly Property CurrentCopModel As Integer
        Get
            '  Return getCurrentCopModel()
            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
            [Call]("GET_CURRENT_COP_MODEL", pointer)
            Return pointer.Value
        End Get
    End Property
    Public Shared ReadOnly Property CurrentBasicCopModel As Integer
        Get
            ' Return getCurrentBasicCopModel()
            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
            [Call]("GET_CURRENT_BASIC_COP_MODEL", pointer)
            Return pointer.Value
        End Get
    End Property
    Public Shared ReadOnly Property CurrentBasicPoliceCarModel As Integer
        Get
            '  Return getCurrentBasicPoliceCarModel()
            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
            [Call]("GET_CURRENT_BASIC_POLICE_CAR_MODEL", pointer)
            Return pointer.Value
        End Get
    End Property
    Public Shared ReadOnly Property CurrentPoliceCarModel As Integer
        Get
            'Return GetCurrentPoliceCarModel()
            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
            [Call]("GET_CURRENT_POLICE_CAR_MODEL", pointer)
            Return pointer.Value
        End Get
    End Property
    Public Shared ReadOnly Property CurrentTaxiCarModel As Integer
        Get
            ' Return getCurrentTaxiCarModel()
            Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
            [Call]("GET_CURRENT_TAXI_CAR_MODEL", pointer)
            Return pointer.Value
        End Get
    End Property
    ''' <summary>
    ''' Untested. Used in Vlad's second mission (vlad2 in SCO).
    ''' </summary>
    Public Shared Sub DontSuppressAnyPedModel()
        [Call]("DONT_SUPPRESS_ANY_PED_MODELS")
    End Sub
    ''' <summary>
    ''' Untested. Not found in SCO.
    ''' </summary>
    Public Shared Sub DontSuppressAnyCarModel()
        [Call]("DONT_SUPPRESS_ANY_CAR_MODELS")
    End Sub
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared Sub DontSuppressCarModel(Model As String)
        [Call]("DONT_SUPPRESS_CAR_MODEL", GetHashKey(Model))
    End Sub
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public Shared Sub DontSuppressPedModel(Model As String)
        [Call]("DONT_SUPPRESS_PED_MODEL", GetHashKey(Model))
    End Sub
    ''' <summary>
    ''' Returns vehicle name, optionally returning raw name (GameName from IDE file).
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetDisplayNameFromVehicleModel(Model As String, Optional ReturnRawName As Boolean = False) As String
        Dim RawName As String = [Call](Of String)("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", GetHashKey(Model))
        If ReturnRawName Then
            Return RawName
        Else
            Return NativeDraws.GXT.GetString(RawName)
        End If
    End Function
    Public Shared Function GetHashKey(Model As String) As Integer
        Return [Call](Of Integer)("GET_HASH_KEY", Model)
    End Function
    'Public Shared Function GetStringFromHash(HashKey As Integer) As String
    '    Return [Call](Of String)("GET_STRING_FROM_HASH_KEY", HashKey)
    'End Function
    Public Shared Function GetRandomCarModelInMemory(Optional Cars As Boolean = True, Optional Bikes As Boolean = False, Optional Boats As Boolean = False,
                                                     Optional Helis As Boolean = False, Optional Planes As Boolean = False) As Integer

        Dim modelHash As Native.Pointer = New Native.Pointer(GetType(Integer))
        Dim modelIndex As Native.Pointer = New Native.Pointer(GetType(Integer))

        modelHash.Value = 0
        modelIndex.Value = 1

        [Call]("GET_RANDOM_CAR_MODEL_IN_MEMORY", 1, modelHash, modelIndex)

        If modelIndex.Value = -1 Then
            Return 0
        End If

        Dim isCar As Boolean = Not [Call](Of Boolean)("IS_THIS_MODEL_A_HELI", modelHash) AndAlso
                               Not [Call](Of Boolean)("IS_THIS_MODEL_A_BIKE", modelHash) AndAlso
                               Not [Call](Of Boolean)("IS_THIS_MODEL_A_BOAT", modelHash)

        Dim isBike As Boolean = [Call](Of Boolean)("IS_THIS_MODEL_A_BIKE", modelHash)
        Dim isBoat As Boolean = [Call](Of Boolean)("IS_THIS_MODEL_A_BOAT", modelHash)
        Dim isHeli As Boolean = [Call](Of Boolean)("IS_THIS_MODEL_A_HELI", modelHash)
        Dim isPlane As Boolean = [Call](Of Boolean)("IS_THIS_MODEL_A_PLANE", modelHash)

        If (isCar AndAlso Cars) OrElse
       (isBike AndAlso Bikes) OrElse
       (isBoat AndAlso Boats) OrElse
       (isHeli AndAlso Helis) OrElse
       (isPlane AndAlso Planes) Then

            Return modelHash.Value
        End If

        Return 0
    End Function
    Public Shared Function GetVehicleModel(ModelHash As Integer) As String
        Try
            Dim vehicle As IDE.VehicleData = IDE.GetVehicleData(ModelHash)

            If vehicle Is Nothing Then
                Tools.WriteLog("Error in NativeModel.GetVehicleModel(Hash " & ModelHash & "): vehicle not found or not valid.")
                Return Nothing
            End If
            Return vehicle.Model

        Catch ex As Exception
            Tools.WriteLog("Error in NativeModel.GetVehicleModel(Hash " & ModelHash & ").", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Function GetVehicleModel(Vehicle As Vehicle) As String
        Try
            If Vehicle Is Nothing Then
                Tools.WriteLog("Error in NativeModel.GetVehicleModel: vehicle not found or not valid.")
                Return Nothing
            End If

            Dim pointer1 As New Native.Pointer(GetType(Integer))
            [Call]("GET_CAR_MODEL", Vehicle, pointer1)
            Return GetVehicleModel(CInt(pointer1.Value))
        Catch ex As Exception
            Tools.WriteLog("Error in NativeModel.GetVehicleModel.", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Function IsLoaded(Model As String) As Boolean
        Return [Call](Of Boolean)("HAS_MODEL_LOADED", GetHashKey(Model))
    End Function
    ''' <summary>
    ''' Returns True if model is installed in IMG archive.
    ''' </summary>
    Public Shared Function IsValid(Model As String) As Boolean
        Try
            Return [Call](Of Boolean)("IS_MODEL_IN_CDIMAGE", GetHashKey(Model))
        Catch ex As Exception
            Tools.WriteLog("Error in NativeModel.IsValid.", ex)
            Return False
        End Try
    End Function
    Public Shared Function IsBike(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_BIKE", ModelHash)
    End Function
    Public Shared Function IsBoat(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_BOAT", ModelHash)
    End Function
    Public Shared Function IsCar(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_CAR", ModelHash)
    End Function
    Public Shared Function IsHeli(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_HELI", ModelHash)
    End Function
    Public Shared Function IsPed(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_PED", ModelHash)
    End Function
    Public Shared Function IsPlane(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_PLANE", ModelHash)
    End Function
    Public Shared Function IsTrain(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_TRAIN", ModelHash)
    End Function
    Public Shared Function IsVehicle(ModelHash As Integer) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_VEHICLE", ModelHash)
    End Function
    Public Shared Sub ReducePedModelBudget(Optional Reduce As Boolean = True)
        [Call]("SET_REDUCE_PED_MODEL_BUDGET", Reduce)
    End Sub
    Public Shared Sub ReduceVehicleModelBudget(Optional Reduce As Boolean = True)
        [Call]("SET_REDUCE_VEHICLE_MODEL_BUDGET", Reduce)
    End Sub
    Public Shared Sub Release(Model As String)
        If IsLoaded(Model) Then [Call]("MARK_MODEL_AS_NO_LONGER_NEEDED", GetHashKey(Model))
    End Sub
    Public Shared Sub Request(Model As String)
        If IsValid(Model) Then
            [Call]("REQUEST_MODEL", GetHashKey(Model))
        Else
            'NativeDraws.DisplayText(NativeDraws.TextColors.MP_Red.ToString & "Invalid model! Cannot request " & NativeDraws.TextColors.MP_Orange.ToString & Model & NativeDraws.TextColors.MP_Red.ToString & " model.", 5000)
            Tools.WriteLog("Error in NativeModel.Request: Invalid model! Cannot request '" & Model & "' model.")
        End If
    End Sub
    'Public Shared Function Request2(Model As String) As Boolean
    '    ' If doesExistsInCD_Image(Model) Then
    '    Tick.Enable()
    '    [Call]("REQUEST_MODEL", getHashKey(Model))
    '    _tempRequestedModel = Model

    '    ' Else
    '    '  NativeDraws.DisplayText(NativeDraws.TextColors.MP_Red.ToString & "Invalid model! Cannot request " & NativeDraws.TextColors.MP_Orange.ToString & Model & NativeDraws.TextColors.MP_Red.ToString & " model.", 5000)
    '    '  End If
    '    Return NativeModel.hasModelLoaded(getHashKey(Model))
    'End Function
    'Private Shared Sub OnTick()
    '    If Not [Call](Of Boolean)("HAS_MODEL_LOADED", getHashKey(_tempRequestedModel)) Then
    '        [Call]("REQUEST_MODEL", getHashKey(_tempRequestedModel))
    '        Tick.Disable()
    '    End If
    'End Sub
    'Public Class Tick
    '    Private Shared _tickHandler As Action = AddressOf OnTick
    '    Public Sub New(handler As Action)
    '        _tickHandler = handler
    '    End Sub
    '    ''' <summary>
    '    ''' Enables the Tick adding it to the TickHelper. You must call TickHelper.ProcessAll
    '    ''' in a Tick inside main script to process them.
    '    ''' </summary>
    '    Public Shared Sub Enable()
    '        TickHelper.Add_Internal(_tickHandler)
    '    End Sub
    '    ''' <summary>
    '    ''' Disables the Tick removing it from the TickHelper.
    '    ''' </summary>
    '    Public Shared Sub Disable()
    '        TickHelper.Remove_Internal(_tickHandler)
    '    End Sub
    'End Class

    '================================================================================================================================================
    Public Class SpatialHelper
        ''' <summary>
        ''' Returns minimum and maximum vector of local model's bounding box.
        ''' Uses the Native Function GET_MODEL_DIMENSIONS(Vector3, Vector3)
        ''' </summary>
        Public Shared Function GetModelHitbox(ModelHash As Integer) As (Min As Vector3, Max As Vector3)
            Dim pointer1 As Native.Pointer = New Native.Pointer(GetType(Single))
            Dim pointer2 As Native.Pointer = New Native.Pointer(GetType(Single))
            Dim pointer3 As Native.Pointer = New Native.Pointer(GetType(Single))
            Dim pointer4 As Native.Pointer = New Native.Pointer(GetType(Single))
            Dim pointer5 As Native.Pointer = New Native.Pointer(GetType(Single))
            Dim pointer6 As Native.Pointer = New Native.Pointer(GetType(Single))
            Native.Function.Call("GET_MODEL_DIMENSIONS", ModelHash, pointer1, pointer2, pointer3, pointer4, pointer5, pointer6)
            Return (New Vector3(pointer1.Value, pointer2.Value, pointer3.Value), New Vector3(pointer4.Value, pointer5.Value, pointer6.Value))
        End Function
        ''' <summary>
        ''' Returns Width, Lenght, Height
        ''' </summary>
        Public Shared Function GetSize(ModelHash As Integer) As Vector3
            Dim dims = GetModelHitbox(ModelHash)
            Return New Vector3(
                Math.Abs(dims.Max.X - dims.Min.X),
                Math.Abs(dims.Max.Y - dims.Min.Y),
                Math.Abs(dims.Max.Z - dims.Min.Z)
            )
        End Function
        Public Shared Function GetWidth(ModelHash As Integer) As Single
            Dim dims = GetModelHitbox(ModelHash)
            Return Math.Abs(dims.Max.X - dims.Min.X)
        End Function
        Public Shared Function GetlLength(ModelHash As Integer) As Single
            Dim dims = GetModelHitbox(ModelHash)
            Return Math.Abs(dims.Max.Y - dims.Min.Y)
        End Function
        Public Shared Function GetHeight(modelHash As Integer) As Single
            Dim dims = GetModelHitbox(modelHash)
            Return Math.Abs(dims.Max.Z - dims.Min.Z)
        End Function

        'Public Shared Function InFrontOf(entity As Entity, Optional distanceMultiplier As Single = 1.0F) As Vector3
        '    Dim dims = GetModelDimensions(entity.Model.Hash)
        '    Dim frontOffset As New Vector3(0.0F, dims.Max.Y * distanceMultiplier, 0.0F)
        '    Return GetOffsetFromEntityInWorldCoords(entity, frontOffset)
        'End Function

        '''' <summary>
        '''' Ritorna una posizione dietro il veicolo.
        '''' </summary>
        'Public Shared Function Behind(entity As Entity, Optional distanceMultiplier As Single = 1.0F) As Vector3
        '    Dim dims = GetModelDimensions(entity.Model.Hash)
        '    Dim backOffset As New Vector3(0.0F, dims.Min.Y * distanceMultiplier, 0.0F)
        '    Return GetOffsetFromEntityInWorldCoords(entity, backOffset)
        'End Function

        '''' <summary>
        '''' Ritorna una posizione a sinistra del veicolo.
        '''' </summary>
        'Public Shared Function ToLeftOf(entity As Entity, Optional distanceMultiplier As Single = 1.0F) As Vector3
        '    Dim dims = GetModelDimensions(entity.Model.Hash)
        '    Dim leftOffset As New Vector3(dims.Min.X * distanceMultiplier, 0.0F, 0.0F)
        '    Return GetOffsetFromEntityInWorldCoords(entity, leftOffset)
        'End Function

        '''' <summary>
        '''' Ritorna una posizione a destra del veicolo.
        '''' </summary>
        'Public Shared Function ToRightOf(entity As Entity, Optional distanceMultiplier As Single = 1.0F) As Vector3
        '    Dim dims = GetModelDimensions(entity.Model.Hash)
        '    Dim rightOffset As New Vector3(dims.Max.X * distanceMultiplier, 0.0F, 0.0F)
        '    Return GetOffsetFromEntityInWorldCoords(entity, rightOffset)
        'End Function

        '''' <summary>
        '''' Ritorna una posizione sopra l'entità (utile per spawnare effetti sopra veicoli o ped).
        '''' </summary>
        'Public Shared Function Above(entity As Entity, Optional extraHeight As Single = 0.5F) As Vector3
        '    Dim dims = GetModelDimensions(entity.Model.Hash)
        '    Dim upOffset As New Vector3(0.0F, 0.0F, dims.Max.Z + extraHeight)
        '    Return GetOffsetFromEntityInWorldCoords(entity, upOffset)
        'End Function

        '''' <summary>
        '''' Ritorna una posizione sotto l'entità (utile per effetti o spawn da sotto).
        '''' </summary>
        'Public Shared Function Below(entity As NativeEntity, Optional extraDepth As Single = 0.5F) As Vector3
        '    Dim dims = GetModelDimensions(entity.Model.Hash)
        '    Dim downOffset As New Vector3(0.0F, 0.0F, dims.Min.Z - extraDepth)
        '    Return entity.(entity, downOffset)
        'End Function

        '''' <summary>
        '''' Returns -1 = Left, 1 = Right
        '''' </summary>
        'Public Shared Function getClosestSideToPlayer(Vehicle As Vehicle, Player As Ped) As Integer
        '    Dim playerPos = Player.Position
        '    Dim leftPos = ToLeftOf(Vehicle)
        '    Dim rightPos = ToRightOf(Vehicle)
        '    Dim distLeft = playerPos.DistanceTo(leftPos)
        '    Dim distRight = playerPos.DistanceTo(rightPos)
        '    Return If(distLeft < distRight, -1, 1)
        'End Function
    End Class
End Class

'Private Shared Function GetCurrentBasicCopModel() As Integer
'    Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
'    [Call]("GET_CURRENT_BASIC_COP_MODEL", pointer)
'    Return pointer.Value
'End Function
'Private Shared Function GetCurrentCopModel() As Integer
'    Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
'    [Call]("GET_CURRENT_COP_MODEL", pointer)
'    Return pointer.Value
'End Function
'Private Shared Function GetCurrentBasicPoliceCarModel() As Integer
'    Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
'    [Call]("GET_CURRENT_BASIC_POLICE_CAR_MODEL", pointer)
'    Return pointer.Value
'End Function
'Private Shared Function GetCurrentPoliceCarModel() As Integer
'    Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
'    [Call]("GET_CURRENT_POLICE_CAR_MODEL", pointer)
'    Return pointer.Value
'End Function
'Private Shared Function GetCurrentTaxiCarModel() As Integer
'    Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
'    [Call]("GET_CURRENT_TAXI_CAR_MODEL", pointer)
'    Return pointer.Value
'End Function
