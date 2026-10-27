Imports GTA
Imports GTA.Native.Function
Imports System.Windows.Forms

Public Class NativeGarage
    Public Enum Flags
        LockedInPosition = 0
        ClosesWhenDistant = 1
        FadeOutAfterClose = 2
        ManualControl = 3
        OpensWhenNear_InsideCar = 4
    End Enum
    Private Shared _allGarages As List(Of NativeGarage)
    Private _type As Flags = Flags.LockedInPosition
    Private _pos As GTA.Vector3
    Private _dontAffectCamera As Boolean = True
    Private _targetcar As Vehicle = Nothing
    Private _name As String
    Private Shared _lastToggleKeydown As Single = 0
    Private _ownedByPlayer As Boolean = False
    Private _canOpenManually As Boolean = True
    Private _radius As Single
    Private Shared _isTickEnabled As Boolean
    Private Shared _tickHandler As Action = AddressOf UpdateGarageControl_OnTick
    Public Shared ToggleGarageDoor As Keys
    Public ReadOnly Property Blip As Blip
    Public ReadOnly Property Hash As String
    Public Shared ReadOnly Property List As List(Of NativeGarage)
        Get
            If _allGarages Is Nothing Then _allGarages = New List(Of NativeGarage)
            Return _allGarages
        End Get
    End Property
    Public Property CanBeManuallyOpened As Boolean
        Get
            Return _canOpenManually
        End Get
        Set(value As Boolean)
            _canOpenManually = value
        End Set
    End Property
    Public ReadOnly Property Position As Vector3
        Get
            Return _pos
        End Get
    End Property
    Public ReadOnly Property RadiusInside As Single
        Get
            Return _radius
        End Get
    End Property
    'Public Property IsOwnedByPlayer As Boolean
    '    Get
    '        Return _ownedByPlayer
    '    End Get
    '    Set(value As Boolean)
    '        _ownedByPlayer = value
    '    End Set
    'End Property
    Public Property TargetVehicle As Vehicle
        Get
            Return _targetcar
        End Get
        Set(value As Vehicle)
            _targetcar = value
            If value IsNot Nothing AndAlso value.Exists Then
                [Call]("SET_TARGET_CAR_FOR_MISSION_GARAGE", Hash, value)
            End If
        End Set
    End Property
    Friend Shared Property Tick As Boolean
        Get
            Return _isTickEnabled
        End Get
        Set(value As Boolean)
            If value = _isTickEnabled Then Return

            _isTickEnabled = value
            If value Then
                If Not TickHelper.ContainsHandler_Internal(_tickHandler) Then
                    TickHelper.Add_Internal(_tickHandler)
                End If
            Else
                If TickHelper.ContainsHandler_Internal(_tickHandler) Then
                    TickHelper.Remove_Internal(_tickHandler)
                End If
            End If
        End Set
    End Property
    Public ReadOnly Property IsOpen As Boolean
        Get
            Return [Call](Of Boolean)("IS_GARAGE_OPEN", Hash)
        End Get
    End Property
    Public ReadOnly Property IsClosed As Boolean
        Get
            Return [Call](Of Boolean)("IS_GARAGE_CLOSED", Hash)
        End Get
    End Property
    Public ReadOnly Property IsMoving As Boolean
        Get
            Return Not isClosed AndAlso Not isOpen
        End Get
    End Property
    Public Property DontAffectCamera As Boolean
        Get
            Return _dontAffectCamera
        End Get
        Set(value As Boolean)
            [Call]("SET_GARAGE_LEAVE_CAMERA_ALONE", Hash, value)
            _dontAffectCamera = value
        End Set
    End Property
    Public Property Behaviour As Flags
        Get
            Return _type
        End Get
        Set(value As Flags)
            [Call]("CHANGE_GARAGE_TYPE", Hash, value)
            _type = value
        End Set
    End Property
    Public Property VisibleOnMap As Boolean
        Get
            Return Blip.Display <> BlipDisplay.Hidden
        End Get
        Set(value As Boolean)
            Blip.Display = If(value, BlipDisplay.MapOnly, BlipDisplay.Hidden)
        End Set
    End Property

    Public Shared Broker As NativeGarage = New NativeGarage("bs3MG")
    Public Shared Bohan As NativeGarage = New NativeGarage("BxGRG1")
    Public Shared Unknown01 As NativeGarage = New NativeGarage("PegGar")
    Public Shared EastIsland As NativeGarage = New NativeGarage("QW2MG1")
    Public Shared JimmyUnknown As NativeGarage = New NativeGarage("QW2MG2")
    Public Shared Westminster As NativeGarage = New NativeGarage("PaulMH3")
    Public Shared Unknown02_TBoGT As NativeGarage = New NativeGarage("Lockup5")
    ' [Call]("SET_GARAGE_LEAVE_CAMERA_ALONE", "bs3MG", 1)
    Private Sub New(GarageHash As String)
        Hash = GarageHash.ToUpper()

        Select Case Hash
            Case "BS3MG"
                _pos = New Vector3(964.96, -319.88, 19.24)
                _name = "Broker Garage"
                _radius = 4.0
            Case "BxGRG1"
                _pos = New Vector3(0, 0, 0)
                _name = "Bohan Garage"
            Case "PegGar"
                _pos = New Vector3(0, 0, 0)
                _name = ""
            Case "QW2MG1"
                _pos = New Vector3(917.2, 570.43, 22.28)
                _name = "East Island Garage"
                _radius = 5.0
            Case "QW2MG2"
                _pos = New Vector3(0, 0, 0)
            Case "PaulMH3"
                _pos = New Vector3(-514.09, 332.19, 6.66)
                _name = "Westminister Garage"
            Case "Lockup5"
                _pos = New Vector3(0, 0, 0)
                _name = "Unknown TBoGT"
            Case ""
                _pos = New Vector3(0, 0, 0)
            Case Else
                _pos = New Vector3(0, 0, 0)
        End Select

        'If GarageDefinitions.ContainsKey(Hash) Then
        '    Position = GarageDefinitions(Hash).Item1
        '    _name = GarageDefinitions(Hash).Item2
        'Else
        '    Position = Vector3.Zero
        '    _name = "Unknown Garage"
        'End If

        If Position <> New Vector3(0, 0, 0) Then
            'Blip
            Blip = Blip.AddBlip(Position)
            Blip.Display = BlipDisplay.Hidden
            Blip.Friendly = True
            Blip.Icon = BlipIcon.Building_Garage
            Blip.Name = _name
            Blip.Scale = 1.0F
            Blip.Color = BlipColor.White
        End If

        DontAffectCamera = True

        If Not List.Any(Function(g) g.Hash = Me.Hash) Then
            List.Add(Me)
        End If
    End Sub
    Public Shared Sub AbortAllGarageActivities()
        [Call]("ABORT_ALL_GARAGE_ACTIVITY")
    End Sub
    Public Sub Open(Optional Flag As Flags = Flags.ClosesWhenDistant)
        If isClosed Then
            Behaviour = Flag
            [Call]("OPEN_GARAGE", Hash)
        End If
    End Sub
    Public Sub Close(Optional Flag As Flags = Flags.OpensWhenNear_InsideCar)
        If isOpen Then
            Behaviour = Flag
            [Call]("CLOSE_GARAGE", Hash)
        End If
    End Sub
    Public Function IsVehicleInside(Car As Vehicle)
        Return [Call](Of Boolean)("IS_CAR_IN_GARAGE_AREA", Hash, Car)
        'NativePTFX.
    End Function
    Public Function IsPlayerInside() As Boolean
        If NativePlayer.Character.Position.DistanceTo(Position) < _radius Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetClosestGarage(Position As Vector3) As NativeGarage
        If List.Count = 0 Then Return Nothing

        Dim closestGarage As NativeGarage = Nothing
        Dim minDist As Single = Single.MaxValue

        For Each g As NativeGarage In List
            If g.Position <> Vector3.Zero Then
                Dim dist As Single = Position.DistanceTo(g.Position)
                If dist < minDist Then
                    minDist = dist
                    closestGarage = g
                End If
            End If
        Next

        Return closestGarage
    End Function
    Public Shared Function IsPlayerInsideAnyGarage() As Boolean
        Return [Call](Of Boolean)("PLAYER_IS_INTERACTING_WITH_GARAGE", NativePlayer.Index)
    End Function
    Public Function IsInRange(Pos As Vector3, Optional Range As Single = 20.0F) As Boolean
        If Pos.DistanceTo(Position) <= Range Then
            Return True
        Else
            Return False
        End If
    End Function
    'Public Shared Function getGarageByName(StringName As String) As NativeGarage
    '    Return List.FirstOrDefault(Function(g) g.Hash.Equals(StringName.ToUpper()))
    'End Function
    Public Sub ToggleDoor(Optional Range As Single = 20.0F)
        Dim player As Ped = NativePlayer.Character
        If player Is Nothing OrElse Not player.Exists Then Exit Sub

        Dim playerPos As Vector3 = player.Position
        Dim nearestGarage As NativeGarage = getClosestGarage(playerPos)

        If nearestGarage Is Nothing OrElse nearestGarage.Hash Is Nothing Then Exit Sub
        If nearestGarage.Hash.ToUpper() <> Me.Hash.ToUpper() Then Exit Sub
        If Not CanBeManuallyOpened Then Exit Sub
        If Not isInRange(playerPos, Range) Then Exit Sub

        If isOpen Then
            Dim v As Vehicle = World.GetClosestVehicle(Position, 10.0F)
            If v IsNot Nothing AndAlso v.Exists Then
                If isVehicleInside(v) Then
                    Close(Flags.ManualControl)
                Else
                    Close(Flags.ClosesWhenDistant)
                End If
            Else
                Close(Flags.ClosesWhenDistant)
            End If

        ElseIf isClosed Then
            Open(Flags.ClosesWhenDistant)
        End If
    End Sub
    Public Shared Sub ToggleNearestGarageDoor(Optional Range As Single = 15.0F, Optional overrideCanBeOpenedManually As Boolean = False)
        Try
            Dim player As Ped = NativePlayer.Character
            If player Is Nothing OrElse Not player.Exists Then Exit Sub

            Dim playerPos As Vector3 = player.Position
            Dim nearestGarage As NativeGarage = getClosestGarage(playerPos)

            If nearestGarage Is Nothing OrElse nearestGarage.Hash Is Nothing Then Exit Sub
            If Not nearestGarage.isInRange(playerPos, Range) Then Exit Sub
            If Not overrideCanBeOpenedManually AndAlso Not nearestGarage.CanBeManuallyOpened Then Exit Sub

            If nearestGarage.isOpen Then
                Dim v As Vehicle = World.GetClosestVehicle(nearestGarage.Position, 10.0F)
                If v IsNot Nothing AndAlso v.Exists Then
                    If nearestGarage.isVehicleInside(v) Then
                        nearestGarage.Close(Flags.ManualControl)
                    Else
                        nearestGarage.Close(Flags.ClosesWhenDistant)
                    End If
                Else
                    nearestGarage.Close(Flags.ClosesWhenDistant)
                End If

            ElseIf nearestGarage.isClosed Then
                nearestGarage.Open(Flags.ClosesWhenDistant)
            End If

        Catch ex As Exception

        End Try
    End Sub
    '    Private Shared ReadOnly GarageDefinitions As New Dictionary(Of String, (Vector3, String)) From {
    '    {"BS3MG", (New Vector3(964.96, -329, 19.43), "Broker Garage")},
    '    {"BXGRG1", (Vector3.Zero, "Bohan Garage")},
    '    {"PEGGAR", (Vector3.Zero, "Unknown")},
    '    {"QW2MG2", (Vector3.Zero, "Jimmy Unknown")},
    '    {"LOCKUP5", (Vector3.Zero, "Unknown TBoGT")},
    '    {"PAULMH3", (New Vector3(-514.09, 332.19, 6.66), "Westminster Garage")},
    '    {"QW2MG1", (New Vector3(917.2, 570.43, 22.28), "East Island Garage")},
    '    {"LOCKUP5", (Vector3.Zero, "Unknown TBoGT")}
    '}
    Public Shared Function IsVehicleInAnyGarage(Car As Vehicle) As Boolean
        Try
            If Car Is Nothing OrElse Not Car.Exists Then Return False
            If List Is Nothing OrElse List.Count = 0 Then Return False

            For Each g As NativeGarage In List
                If g IsNot Nothing AndAlso g.Hash IsNot Nothing Then
                    If g.IsVehicleInside(Car) Then
                        Return True
                    End If
                End If
            Next

        Catch ex As Exception

        End Try

        Return False
    End Function
    Public Shared Function GetGarageOfVehicle(Car As Vehicle) As NativeGarage
        Try
            If Car Is Nothing OrElse Not Car.Exists Then Return Nothing
            If List Is Nothing OrElse List.Count = 0 Then Return Nothing

            For Each g As NativeGarage In List
                If g IsNot Nothing AndAlso g.Hash IsNot Nothing Then
                    If g.isVehicleInside(Car) Then
                        Return g
                    End If
                End If
            Next

        Catch ex As Exception

        End Try

        Return Nothing
    End Function
    Private Shared Sub UpdateGarageControl_OnTick()
        If ToggleGarageDoor = Keys.None Then Return
        If GTA.Game.isKeyPressed(ToggleGarageDoor) Then
            If _lastToggleKeydown + 1000 < Game.GameTime Then
                ToggleNearestGarageDoor()
                _lastToggleKeydown = Game.GameTime
            End If
        End If
    End Sub


    Public NotInheritable Class PaynSpray
        Private Sub New()
        End Sub
        Public Shared Sub AllowCarToBeResprayed(Car As Vehicle, Optional Allow As Boolean = True)
            [Call]("SET_CAN_RESPRAY_CAR", Car, Allow)
        End Sub

        Public Shared WriteOnly Property Enabled As Boolean
            Set(value As Boolean)
                [Call]("SET_NO_RESPRAYS", Not value)
            End Set
        End Property

        Public Shared WriteOnly Property FreeResprays As Boolean
            Set(value As Boolean)
                [Call]("SET_FREE_RESPRAYS", value)
            End Set
        End Property

        Public Shared ReadOnly Property IsAnyPaynSprayActive As Boolean
            Get
                Return [Call](Of Boolean)("IS_PAY_N_SPRAY_ACTIVE")
            End Get
        End Property

        Public Shared Function HasCarBeenResprayed(Car As Vehicle) As Boolean
            Return [Call](Of Boolean)("HAS_CAR_BEEN_RESPRAYED", Car)
        End Function

        Public Shared ReadOnly Property IsResprayHappened As Boolean
            Get
                Return [Call](Of Boolean)("HAS_RESPRAY_HAPPENED")
            End Get
        End Property
    End Class
    'Private Class Tick_OLD
    '    Private Shared _tickHandler As Action = AddressOf UpdateGarageControl_OnTick
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
End Class
