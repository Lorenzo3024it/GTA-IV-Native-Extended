Imports System.Runtime.InteropServices.WindowsRuntime
Imports GTA
Imports GTA.Native.Function
Partial Public Class NativeFire
    'Public Shared Function DoesScriptFireExists(Fire As Integer) As Boolean
    '    Return [Call](Of Boolean)("DOES_SCRIPT_FIRE_EXIST", Fire)
    'End Function
    Public Shared Sub ExtinguishFireAtCoord(Position As GTA.Vector3, Radius As Double)
        [Call]("EXTINGUISH_FIRE_AT_POINT", Position.X, Position.Y, Position.Z, Radius)
    End Sub
    'Public Shared Sub ExtinguishScriptFire(FireID As Integer)
    '    If DoesScriptFireExists(FireID) Then [Call]("REMOVE_SCRIPT_FIRE", FireID)
    'End Sub
    Public Shared Function GetNumberOfFiresInRange(Position As Vector3, Range As Single) As Integer
        Return [Call](Of Integer)("GET_NUMBER_OF_FIRES_IN_RANGE", Position.X, Position.Y, Position.Z, Range)
    End Function
    'Public Shared Function isScriptFireExtinguished(Fire As Integer) As Boolean
    '    Return [Call](Of Boolean)("IS_SCRIPT_FIRE_EXTINGUISHED", Fire)
    'End Function
    'Public Shared Function StartScriptFire(Position As GTA.Vector3, NumOfFireAllowed As Integer, Strength As Integer) As Integer
    '    Return [Call](Of Integer)("START_SCRIPT_FIRE", Position.X, Position.Y, Position.Z, NumOfFireAllowed, Strength)
    'End Function
End Class

Partial Public Class NativeFire
    Private _handle As Integer
    Private _position As Vector3
    Private _numAllowed As Integer
    Private _strength As Integer
    Public Sub New()
        _handle = Nothing
        _position = Vector3.Zero
        _numAllowed = Nothing
        _strength = Nothing
        '_position = Position
        '_numAllowed = NumAllowed
        '_strength = Strength
    End Sub
    Public Function Exists() As Boolean
        If _handle <> 0 AndAlso [Call](Of Boolean)("DOES_SCRIPT_FIRE_EXIST", _handle) Then Return True Else Return False
    End Function
    Public ReadOnly Property Handle As Integer
        Get
            Return _handle
        End Get
    End Property
    Public Function isExtinguished() As Boolean
        Return [Call](Of Boolean)("IS_SCRIPT_FIRE_EXTINGUISHED", _handle)
    End Function
    Public ReadOnly Property Position As Vector3
        Get
            Return _position
        End Get
    End Property
    Public Sub Start(Position As Vector3, Optional NumAllowed As Integer = 1, Optional Strength As Integer = 1)
        If _handle <> 0 Then [Stop]()
        _handle = [Call](Of Integer)("START_SCRIPT_FIRE", Position.X, Position.Y, Position.Z, _numAllowed, _strength)
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
