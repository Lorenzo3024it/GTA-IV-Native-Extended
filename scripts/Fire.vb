Imports System.Runtime.InteropServices.WindowsRuntime
Imports GTA
Imports GTA.Native.Function

Partial Public NotInheritable Class NativeFire
    Public Shared Sub ExtinguishFireAtCoord(Position As GTA.Vector3, Radius As Double)
        [Call]("EXTINGUISH_FIRE_AT_POINT", Position.X, Position.Y, Position.Z, Radius)
    End Sub
    Public Shared Function GetNumberOfFiresInRange(Position As Vector3, Radius As Single) As Integer
        Return [Call](Of Integer)("GET_NUMBER_OF_FIRES_IN_RANGE", Position.X, Position.Y, Position.Z, Radius)
    End Function
End Class

Partial Public NotInheritable Class NativeFire
    Private _handle As Integer
    Private _position As Vector3
    Private _numAllowed As Integer
    Private _strength As Integer
    Public Sub New()
        _handle = 0
        _position = Vector3.Zero
    End Sub

    Public ReadOnly Property Exists As Boolean
        Get
            Return _handle <> 0 AndAlso
               [Call](Of Boolean)("DOES_SCRIPT_FIRE_EXIST", _handle)
        End Get
    End Property

    Public ReadOnly Property Handle As Integer
        Get
            Return _handle
        End Get
    End Property

    Public ReadOnly Property IsExtinguished
        Get
            Return [Call](Of Boolean)("IS_SCRIPT_FIRE_EXTINGUISHED", _handle)
        End Get
    End Property

    Public ReadOnly Property Position As Vector3
        Get
            Return _position
        End Get
    End Property

    Public Sub Start(Position As Vector3, Optional NumAllowed As Integer = 1, Optional Strength As Integer = 1)
        [Stop]()
        _position = Position
        _numAllowed = NumAllowed
        _strength = Strength
        _handle = [Call](Of Integer)("START_SCRIPT_FIRE", Position.X, Position.Y, Position.Z, NumAllowed, Strength)
    End Sub

    Public Sub [Stop]()
        If _handle <> 0 Then
            [Call]("REMOVE_SCRIPT_FIRE", _handle)
            _handle = 0
        End If
    End Sub
End Class


'EXTINGUISH_OBJECT_FIRE
'EXTINGUISH_CHAR_FIRE
'EXTINGUISH_CAR_FIRE
'START_CAR_FIRE
'START_OBJECT_FIRE
'GET_SCRIPT_FIRE_COORDS
