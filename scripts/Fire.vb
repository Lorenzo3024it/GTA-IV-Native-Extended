Imports GTA
Imports GTA.Native.Function
Public Class NativeFire
    Public Shared Function DoesScriptFireExists(Fire As Integer) As Boolean
        Return [Call](Of Boolean)("DOES_SCRIPT_FIRE_EXIST", Fire)
    End Function
    Public Shared Sub ExtinguishFireAtCoord(Position As GTA.Vector3, Radius As Double)
        [Call]("EXTINGUISH_FIRE_AT_POINT", Position.X, Position.Y, Position.Z, Radius)
    End Sub
    Public Shared Sub ExtinguishScriptFire(FireID As Integer)
        If doesScriptFireExists(FireID) Then [Call]("REMOVE_SCRIPT_FIRE", FireID)
    End Sub
    Public Shared Function getNumberOfFiresInRange(Position As Vector3, Range As Single) As Integer
        Return [Call](Of Integer)("GET_NUMBER_OF_FIRES_IN_RANGE", Position.X, Position.Y, Position.Z, Range)
    End Function
    Public Shared Function isScriptFireExtinguished(Fire As Integer) As Boolean
        Return [Call](Of Boolean)("IS_SCRIPT_FIRE_EXTINGUISHED", Fire)
    End Function
    Public Shared Function StartScriptFire(Position As GTA.Vector3, NumOfFireAllowed As Integer, Strength As Integer) As Integer
        Return [Call](Of Integer)("START_SCRIPT_FIRE", Position.X, Position.Y, Position.Z, NumOfFireAllowed, Strength)
    End Function
    Public Class FireSound
        Private _playing As Boolean
        Private _ID As Integer
        Public ReadOnly Property Position As Vector3
        Public ReadOnly Property SoundID As Integer
            Get
                Return _ID
            End Get
        End Property
        Public ReadOnly Property isPlaying As Boolean
            Get
                Return _playing
            End Get
        End Property
        Sub New(Position As Vector3)
            _ID = Nothing

            Me.Position = Position
            _playing = False
        End Sub
        Public Sub Play()
            If _ID = Nothing Then _ID = [Call](Of Integer)("GET_SOUND_ID")
            [Call]("PLAY_FIRE_SOUND_FROM_POSITION", _ID, Position.X, Position.Y, Position.Z)
            _playing = True
        End Sub
        Public Sub [Stop](Optional RelaseSoundID As Boolean = False)
            [Call]("STOP_SOUND", _ID)
            _playing = False
            If RelaseSoundID Then [Call]("RELEASE_SOUND_ID", _ID)
        End Sub
    End Class
    'EXTINGUISH_OBJECT_FIRE
    'EXTINGUISH_CHAR_FIRE
    'EXTINGUISH_CAR_FIRE
    'GET NUMBER OF FIRES IN AREA
    'GET_NUMBER_OF_FIRES_IN_RANGE
    'START_CAR_FIRE
    'START_OBJECT_FIRE
    'GET_SCRIPT_FIRE_COORDS
End Class
