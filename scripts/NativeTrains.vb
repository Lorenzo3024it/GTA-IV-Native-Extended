Imports GTA
Imports GTA.Native.Function

Public Class NativeTrains
    'TRAIN LEAVE STATION
    'SET_RAILTRACK_RESISTANCE_MULT
    Shared Function CreateTrain(Position As Vector3, Direction As Integer, Optional TrainType As Integer = 0) As Vehicle
        Dim TrainPointer As Native.Pointer = New Native.Pointer(GetType(Vehicle))
        [Call]("CREATE_MISSION_TRAIN", TrainType, Position.X, Position.Y, Position.Z, Direction, TrainPointer)
        Return TrainPointer.Value
    End Function

    Shared Sub DeleteAllTrains()
        [Call]("DELETE_ALL_TRAINS")
    End Sub
    Shared Function getTrainCarriage() As Integer
        Return [Call](Of Integer)("GET_TRAIN_CARRIAGE")
    End Function
    Shared Function getTrainDirection(Train As Vehicle) As Integer
        Return [Call](Of Integer)("FIND_TRAIN_DIRECTION", Train)
    End Function
    Shared Function isTrain(Train As Vehicle)
        Return [Call](Of Boolean)("IS_THIS_MODEL_A_TRAIN", Train.Model)
    End Function
End Class