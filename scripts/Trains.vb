Imports System.ComponentModel.Design
Imports GTA
Imports GTA.base
Imports GTA.Native.Function

Public Class NativeTrains
    'SET_RAILTRACK_RESISTANCE_MULT
    'SET_TRAIN_STOPS_FOR_STATIONS
    'SET_MISSION_TRAIN_COORDINATES
    'IS_CHAR_IN_ANY_TRAIN

    '1440:    GET_TRAIN_CARRIAGE( l_U704._fU0, 0, ref l_U840[0] );
    '1441:    GET_TRAIN_CARRIAGE( l_U704._fU0, 1, ref l_U840[1] );
    '1442:    GET_TRAIN_CARRIAGE( l_U704._fU0, 2, ref l_U840[2] );
    '1477:    GET_TRAIN_CABOOSE( l_U704._fU0, ref l_U844 );
    'Public Shared Function CreateMissionTrain(Position As Vector3, Direction As Integer, Optional TrainType As Integer = 0) As Vehicle
    '    Dim TrainPointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
    '    [Call]("CREATE_MISSION_TRAIN", TrainType, Position.X, Position.Y, Position.Z, Direction, TrainPointer)
    '    Return TrainPointer.Value
    'End Function
    Private Shared ReadOnly _AllTrains As New List(Of NativeTrains)
    Public Shared Function GetAllTrains() As List(Of NativeTrains)
        Return New List(Of NativeTrains)(_AllTrains)
    End Function
    Private _isDerailed As Boolean
    Private _direction As Integer
    Private _typeoftrain As Integer
    Private _stopAtStation As Boolean
    Private _isForcedToSlowDown As Boolean
    Private _audioRollOff As Single
    Private _normalspeed As Single
    Private _speedcruise As Single
    Public ReadOnly Property Handle As Vehicle
    Public Property Speed As Single
        Get
            Return _normalspeed
        End Get
        Set(value As Single)
            [Call]("SET_TRAIN_SPEED", Handle, value)
        End Set
    End Property
    Public Property CruiseSpeed As Single
        Get
            Return _speedcruise
        End Get
        Set(value As Single)
            [Call]("SET_TRAIN_CRUISE_SPEED", Handle, value)
        End Set
    End Property
    Public ReadOnly Property Direction As Integer
        Get
            Return _direction
        End Get
    End Property
    Public ReadOnly Property CurrentStation As Integer
        Get
            Return getCurrentStation(Handle)
        End Get
    End Property
    Public ReadOnly Property NextStation As Integer
        Get
            Return getNextStation(Handle)
        End Get
    End Property
    Public Property isRequiredForMission As Boolean
        Get
            Return Handle.isRequiredForMission
        End Get
        Set(value As Boolean)
            If value = True Then
                Handle.isRequiredForMission = True
            Else
                MarkTrainAsNoLongerNeeded(Handle)
            End If
        End Set
    End Property
    Public ReadOnly Property Caboose As Integer
        Get
            Return getTrainCaboose(Handle)
        End Get
    End Property
    Public Property StoppedAtStation As Boolean
        Get
            Return _stopAtStation
        End Get
        Set(value As Boolean)
            SetStoppedAtStation(Handle)
            _stopAtStation = value
        End Set
    End Property
    Public Property AudioRollOFF As Single
        Get
            Return _audioRollOff
        End Get
        Set(value As Single)
            SetAudioRollOFF(Handle, value)
            _audioRollOff = value
        End Set
    End Property
    Public ReadOnly Property Type As Integer
        Get
            Return -_typeoftrain
        End Get
    End Property
    Public ReadOnly Property isDerailed As Boolean
        Get
            Return _isDerailed
        End Get
    End Property
    Public Property ForceToSlowDown As Boolean
        Get
            Return _isForcedToSlowDown
        End Get
        Set(value As Boolean)
            SetForceToSlowDown(Handle, value)
            _isForcedToSlowDown = value
        End Set
    End Property
    Public Shared Function CreateMissionTrain(Position As Vector3, Direction As Integer, Optional TrainType As Integer = 0) As NativeTrains
        Dim TrainHandlePointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
        [Call]("CREATE_MISSION_TRAIN", TrainType, Position.X, Position.Y, Position.Z, Direction, TrainHandlePointer)
        Dim train As New NativeTrains(TrainHandlePointer.Value, Direction, TrainType)
        _AllTrains.Add(train)
        Return train
    End Function
    Sub New(TrainHandle As Vehicle, Direction As Integer, Optional TrainType As Integer = 0)
        Handle = TrainHandle
        _direction = Direction
        _typeoftrain = TrainType
        isRequiredForMission = True
    End Sub
    Public Shared Sub DeleteAllTrains()
        [Call]("DELETE_ALL_TRAINS")
    End Sub
    Public Shared Function getCurrentStation(Train As Vehicle) As Integer
        Return [Call](Of Integer)("GET_CURRENT_STATION_FOR_TRAIN", Train)
    End Function
    Public Shared Function getNextStation(Train As Vehicle) As Integer
        Return [Call](Of Integer)("GET_NEXT_STATION_FOR_TRAIN", Train)
    End Function
    Public Shared Function getTrainCaboose(Train As Vehicle) As Integer
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call](Of Integer)("GET_TRAIN_CABOOSE", Train, pointer)
        Return pointer.Value
    End Function
    Public Shared Function getTrainCarriage(Train As Vehicle, Unknown As Integer) As Integer
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call](Of Integer)("GET_TRAIN_CARRIAGE", Train, Unknown, pointer)
        Return pointer.Value
    End Function
    Public Function getTrainCarriage(Unknown As Integer) As Integer
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call](Of Integer)("GET_TRAIN_CARRIAGE", Handle, Unknown, pointer)
        Return pointer.Value
    End Function
    Public Shared Function getTrainDirection(Train As Vehicle) As Integer
        Return [Call](Of Integer)("FIND_TRAIN_DIRECTION", Train)
    End Function
    Public Shared Function isTrain(Train As Vehicle) As Boolean
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_TRAIN", Train.Model)
    End Function
    Public Shared Function isPedInTrain(Ped As Ped) As Boolean
        Return [Call](Of Boolean)("IS_CHAR_IN_ANY_TRAIN", Ped)
    End Function
    Public Shared Sub LeaveStation(Train As Vehicle)
        [Call]("TRAIN_LEAVE_STATION", Train)
    End Sub
    Public Sub LeaveStation()
        [Call]("TRAIN_LEAVE_STATION", Handle)
    End Sub
    Public Shared Sub MarkAllMissionTrainsAsNoLongerNeeded()
        [Call]("MARK_MISSION_TRAINS_AS_NO_LONGER_NEEDED")
    End Sub
    Public Shared Sub MarkTrainAsNoLongerNeeded(Train As Vehicle)
        [Call]("MARK_MISSION_TRAIN_AS_NO_LONGER_NEEDED", Train)
    End Sub
    Public Shared Sub SetStoppedAtStation(Train As Vehicle)
        [Call]("SET_TRAIN_IS_STOPPED_AT_STATION", Train)
    End Sub
    Public Shared Sub SwitchRandomTrains(TrainsON As Boolean)
        [Call]("SWITCH_RANDOM_TRAINS", TrainsON)
    End Sub
    ''' <summary>
    ''' Untested. Seems that the function SET_TRAIN_SPEED is always used with SET_TRAIN_CRUISE_SPEED, which both use the same speed value.
    ''' So this function is a combination of both original Native Functions. If CruiseSpeed is set to -1, it will be automatically
    ''' set to the same value as Speed. If CruiseSpeed is set to -2, the Native Function SET_TRAIN_CRUISE_SPEED won't be activated.
    ''' If you want to test it, you can set a different value for CruiseSpeed.
    ''' </summary>
    ''' <param name="Speed"></param>
    ''' <param name="CruiseSpeed"></param>
    Public Shared Sub SetSpeed(Train As Vehicle, Speed As Single, Optional CruiseSpeed As Single = -1)
        [Call]("SET_TRAIN_SPEED", Train, Speed)
        Select Case CruiseSpeed
            Case -2
            Case -1
                [Call]("SET_TRAIN_CRUISE_SPEED", Train, Speed)
            Case >= 0
                [Call]("SET_TRAIN_CRUISE_SPEED", Train, CruiseSpeed)
        End Select
    End Sub
    Public Shared Sub SetAudioRollOFF(Train As Vehicle, UnknownFloat As Single)
        [Call]("SET_TRAIN_AUDIO_ROLLOFF", Train, UnknownFloat)
    End Sub
    Public Shared Sub SetForceToSlowDown(Train As Vehicle, Optional Enable As Boolean = True)
        [Call]("SET_TRAIN_FORCED_TO_SLOW_DOWN", Train, Enable)
    End Sub
    Public Shared Sub RenderAsDerailed(Train As Vehicle, Optional Enable As Boolean = True)
        [Call]("SET_RENDER_TRAIN_AS_DERAILED", Train, Enable)
    End Sub
    Public Sub RenderAsDerailed(Optional Enable As Boolean = True)
        [Call]("SET_RENDER_TRAIN_AS_DERAILED", Handle, Enable)
        _isDerailed = Enable
    End Sub
End Class