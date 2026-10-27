Imports GTA.Native.Function
Imports GTA
''' <summary>
''' Provides access to GTA IV native viewports through three predefined instances:
''' Screen, Game and Radar.
''' Native viewports should be modified with caution. Changing their position,
''' size, activation state or priority may affect the game's rendering and HUD.
''' In particular, modifying the Radar viewport's position or size may cause
''' visual glitches or incorrect radar rendering. Use these features at your own risk.
''' </summary>
Public NotInheritable Class NativeViewport
    Private Sub New()
    End Sub

    Private Shared ReadOnly _screen As NativeViewport = New NativeViewport("GET_SCREEN_VIEWPORT_ID", "Screen")

    Private Shared ReadOnly _game As NativeViewport = New NativeViewport("GET_GAME_VIEWPORT_ID", "Game")

    Private Shared ReadOnly _radar As NativeViewport = New NativeViewport("GET_RADAR_VIEWPORT_ID", "Radar")

    ''' <summary>
    ''' Gets the predefined screen viewport.
    ''' </summary>
    Public Shared ReadOnly Property Screen As NativeViewport
        Get
            Return _screen
        End Get
    End Property

    ''' <summary>
    ''' Gets the predefined game viewport.
    ''' </summary>
    Public Shared ReadOnly Property Game As NativeViewport
        Get
            Return _game
        End Get
    End Property

    ''' <summary>
    ''' Gets the predefined radar viewport.
    ''' Modifying its position or size may cause visual glitches or incorrect radar rendering.
    ''' </summary>
    Public Shared ReadOnly Property Radar As NativeViewport
        Get
            Return _radar
        End Get
    End Property

    '====================================================================================================================================================================
    Private ReadOnly _nativeIdGetter As String
    Private ReadOnly _name As String

    Private ReadOnly _pID As Native.Pointer
    Private ReadOnly _pX As Native.Pointer
    Private ReadOnly _pY As Native.Pointer
    Private ReadOnly _pWidth As Native.Pointer
    Private ReadOnly _pHeight As Native.Pointer

    Private _id As Integer = -1

    Private Sub New(NativeIDGetter As String, Optional Name As String = "")
        _nativeIdGetter = NativeIDGetter
        Me.Name = Name

        _pID = New Native.Pointer(GetType(Integer))

        _pX = New Native.Pointer(GetType(Single))
        _pY = New Native.Pointer(GetType(Single))
        _pWidth = New Native.Pointer(GetType(Single))
        _pHeight = New Native.Pointer(GetType(Single))
    End Sub

    ''' <summary>
    ''' Gets the viewport's descriptive name.
    ''' </summary>
    Public ReadOnly Name As String
    ''' <summary>
    ''' Gets the viewport's native identifier.
    ''' </summary>
    Public ReadOnly Property ID As Integer
        Get
            Return ResolveID()
        End Get
    End Property
    ''' <summary>
    ''' Gets whether this viewport has valid ID and currently exists in the game.
    ''' </summary>
    Public ReadOnly Property IsValid As Boolean
        Get
            Dim viewportID As Integer = ResolveID()

            If viewportID <= 0 Then Return False

            Return [Call](Of Boolean)("DOES_VIEWPORT_EXIST", viewportID)
        End Get
    End Property
    ''' <summary>
    ''' Gets or sets whether this viewport is active.
    ''' </summary>
    Public Property Enabled As Boolean
        Get
            Dim viewportID As Integer = ResolveID()

            If viewportID <= 0 Then Return False

            If Not [Call](Of Boolean)("DOES_VIEWPORT_EXIST", viewportID) Then
                Return False
            End If

            Return [Call](Of Boolean)("IS_VIEWPORT_ACTIVE", viewportID)
        End Get
        Set(Value As Boolean)
            Dim viewportID As Integer = ResolveID()

            If viewportID <= 0 Then
                Tools.WriteLog("NativeViewport." & _name & ".Enabled: invalid viewport ID.")
                Return
            End If

            If Not [Call](Of Boolean)("DOES_VIEWPORT_EXIST", viewportID) Then
                Tools.WriteLog("NativeViewport." & _name & ".Enabled: viewport does not exist.")
                Return
            End If

            [Call]("ACTIVATE_VIEWPORT", viewportID, Value)
        End Set
    End Property
    ''' <summary>
    ''' Gets or sets the viewport's position.
    ''' Changing the radar position may cause rendering glitches.
    ''' </summary>
    Public Property Position As GTA.Vector2
        Get
            Dim positionValue As GTA.Vector2
            Dim sizeValue As GTA.Vector2

            If Not TryGetPositionAndSize(positionValue, sizeValue) Then
                Return New GTA.Vector2(0.0F, 0.0F)
            End If

            Return positionValue
        End Get
        Set(Value As GTA.Vector2)
            SetPositionAndSize(Position:=Value)
        End Set
    End Property
    ''' <summary>
    ''' Gets or sets the viewport's size.
    ''' Changing the radar size may cause rendering glitches.
    ''' </summary>
    Public Property Size As GTA.Vector2
        Get
            Dim positionValue As GTA.Vector2
            Dim sizeValue As GTA.Vector2

            If Not TryGetPositionAndSize(positionValue, sizeValue) Then
                Return New GTA.Vector2(0.0F, 0.0F)
            End If

            Return sizeValue
        End Get
        Set(Value As GTA.Vector2)
            SetPositionAndSize(Size:=Value)
        End Set
    End Property
    ''' <summary>
    ''' Sets whether this viewport is mirrored.
    ''' </summary>
    Public WriteOnly Property Mirrored As Boolean
        Set(Value As Boolean)
            Dim viewportID As Integer = ResolveID()

            If Not IsValid Then Return

            [Call]("SET_VIEWPORT_MIRRORED", viewportID, Value)
        End Set
    End Property
    ''' <summary>
    ''' Sets this viewport's priority.
    ''' Values less than or equal to zero are ignored.
    ''' </summary>
    Public WriteOnly Property Priority As Integer
        Set(Value As Integer)
            Dim viewportID As Integer = ResolveID()

            If Value <= 0 OrElse Not IsValid Then Return

            [Call]("SET_VIEWPORT_PRIORITY", viewportID, Value)
        End Set
    End Property
    '====================================================================================================================================================================
    Private Sub SetPositionAndSize(Optional Position As GTA.Vector2? = Nothing, Optional Size As GTA.Vector2? = Nothing)

        If Not Position.HasValue AndAlso Not Size.HasValue Then Return

        Dim viewportID As Integer = ResolveID()

        If viewportID <= 0 Then
            Tools.WriteLog("NativeViewport." & _name & ".SetPositionAndSize: invalid viewport ID.")
            Return
        End If

        If Not [Call](Of Boolean)("DOES_VIEWPORT_EXIST", viewportID) Then
            Tools.WriteLog("NativeViewport." & _name & ".SetPositionAndSize: viewport does not exist.")
            Return
        End If

        Dim finalPosition As GTA.Vector2
        Dim finalSize As GTA.Vector2

        If Position.HasValue AndAlso Size.HasValue Then
            finalPosition = Position.Value
            finalSize = Size.Value
        Else
            If Not TryGetPositionAndSize(finalPosition, finalSize) Then Return

            If Position.HasValue Then finalPosition = Position.Value
            If Size.HasValue Then finalSize = Size.Value
        End If

        [Call]("SET_VIEWPORT", viewportID, finalPosition.X, finalPosition.Y, finalSize.X, finalSize.Y)
    End Sub
    Private Function ResolveID() As Integer
        If _id > 0 Then Return _id

        [Call](_nativeIdGetter, _pID)
        Dim resolvedID As Integer = _pID.Value
        If resolvedID > 0 Then
            _id = resolvedID
        End If

        Return resolvedID
    End Function
    Private Function TryGetPositionAndSize(ByRef PositionValue As GTA.Vector2, ByRef SizeValue As GTA.Vector2) As Boolean
        PositionValue = New GTA.Vector2(0.0F, 0.0F)
        SizeValue = New GTA.Vector2(0.0F, 0.0F)
        Dim viewportID As Integer = ResolveID()
        If viewportID <= 0 Then Return False

        If Not [Call](Of Boolean)("DOES_VIEWPORT_EXIST", viewportID) Then
            Return False
        End If

        [Call]("GET_VIEWPORT_POS_AND_SIZE", viewportID, _pX, _pY, _pWidth, _pHeight)

        PositionValue = New GTA.Vector2(_pX.Value, _pY.Value)
        SizeValue = New GTA.Vector2(_pWidth.Value, _pHeight.Value)

        Return True
    End Function
End Class



'Partial Public NotInheritable Class NativeViewportOLD

'    Private Sub New()
'    End Sub
'    Private Shared ReadOnly pScreen As New Native.Pointer(GetType(Integer))
'    Private Shared ReadOnly pGame As New Native.Pointer(GetType(Integer))
'    Private Shared ReadOnly pRadar As New Native.Pointer(GetType(Integer))
'    '---
'    Private Shared _screenViewport As Integer = -1
'    Private Shared _gameViewport As Integer = -1
'    Private Shared _radarViewport As Integer = -1
'    Public Shared ReadOnly Property GameViewportID As Integer
'        Get
'            If _gameViewport <> -1 Then Return _gameViewport

'            [Call]("GET_GAME_VIEWPORT_ID", pGame)
'            _gameViewport = pGame.Value
'            Return _gameViewport
'        End Get
'    End Property
'    Public Shared ReadOnly Property RadarViewportID As Integer
'        Get
'            If _radarViewport <> -1 Then Return _radarViewport

'            [Call]("GET_RADAR_VIEWPORT_ID", pRadar)
'            _radarViewport = pRadar.Value
'            Return _radarViewport
'        End Get
'    End Property
'    Public Shared ReadOnly Property ScreenViewportID As Integer
'        Get
'            If _screenViewport <> -1 Then Return _screenViewport

'            [Call]("GET_SCREEN_VIEWPORT_ID", pScreen)
'            _screenViewport = pScreen.Value
'            Return _screenViewport
'        End Get
'    End Property
'    '========================
'    Public Shared Sub Activate(ViewportID As Integer, Optional Enable As Boolean = True)
'        If Not IsValid(ViewportID) Then
'            Tools.WriteLog("Viewport.Activate: invalid viewport id.")
'            Return
'        End If

'        [Call]("ACTIVATE_VIEWPORT", ViewportID, Enable)
'    End Sub
'    Public Shared Function GetPositionAndSize(ViewportID As Integer) As (Position As Vector2, Size As Vector2)
'        If Not IsValid(ViewportID) Then
'            Return (New Vector2(0.0, 0.0), New Vector2(0.0, 0.0))
'        End If

'        Dim pX, pY, pW, pH As New Native.Pointer(GetType(Single))
'        [Call]("GET_VIEWPORT_POS_AND_SIZE", ViewportID, pX, pY, pW, pH)

'        Return (New Vector2(pX.Value, pY.Value), New Vector2(pW.Value, pH.Value))
'    End Function
'    ''' <summary>
'    ''' Returns True if ViewportID is valid and viewports exists (native call).
'    ''' </summary>
'    Public Shared Function IsValid(ViewportID As Integer) As Boolean
'        If ViewportID <= 0 Then Return False
'        Return [Call](Of Boolean)("DOES_VIEWPORT_EXIST", ViewportID)
'    End Function
'    Public Shared Function IsActive(ViewportID As Integer) As Boolean
'        If Not IsValid(ViewportID) Then Return False
'        Return [Call](Of Boolean)("IS_VIEWPORT_ACTIVE", ViewportID)
'    End Function
'    Public Shared Sub SetMirrored(ViewportID As Integer, Optional Mirrored As Boolean = True)
'        If Not IsValid(ViewportID) Then Return
'        [Call]("SET_VIEWPORT_MIRRORED", ViewportID, Mirrored)
'    End Sub
'    Public Shared Sub SetPositionAndSize(ViewportID As Integer, Optional Position As Vector2? = Nothing, Optional Size As Vector2? = Nothing)
'        If Not IsValid(ViewportID) Then
'            Tools.WriteLog("NativeViewport.SetPositionAndSize for ID" & ViewportID & ": invalid viewport ID.")
'            Return
'        End If
'        Dim finalPos As Vector2
'        Dim finalSize As Vector2
'        If Not Position.HasValue OrElse Not Size.HasValue Then
'            Dim pX, pY, pW, pH As New Native.Pointer(GetType(Single))
'            [Call]("GET_VIEWPORT_POS_AND_SIZE", ViewportID, pX, pY, pW, pH)

'            finalPos = If(Position.HasValue, Position.Value, New Vector2(pX.Value, pY.Value))
'            finalSize = If(Size.HasValue, Size.Value, New Vector2(pW.Value, pH.Value))
'        Else
'            finalPos = Position.Value
'            finalSize = Size.Value
'        End If

'        [Call]("SET_VIEWPORT", ViewportID, finalPos.X, finalPos.Y, finalSize.X, finalSize.Y)
'    End Sub
'    ''' <summary>
'    ''' Untested
'    ''' </summary>
'    Public Shared Sub SetPriority(ViewportID As Integer, Priority As Integer)
'        If Not IsValid(ViewportID) OrElse Priority <= 0 Then Return
'        [Call]("SET_VIEWPORT_PRIORITY", ViewportID, Priority)
'    End Sub
'End Class






'Partial Public NotInheritable Class NativeViewport
'    Implements IDisposable
'    '========================
'    Private _handle As Integer
'    Private _disposed As Boolean
'    Private _prior As Integer
'    Private _mirror As Boolean = False
'    Public Sub New(Optional Position As Vector2? = Nothing, Optional Size As Vector2? = Nothing, Optional Priority As Integer = 100)
'        Dim p As New Native.Pointer(GetType(Integer))
'        [Call]("CREATE_VIEWPORT", p)
'        _handle = p.Value
'        If _handle <= 0 Then
'            Throw New InvalidOperationException("Failed to create viewport.")
'        End If
'        If Position.HasValue OrElse Size.HasValue Then
'            Dim pos As Vector2 = If(Position.HasValue, Position.Value, New Vector2(0.0, 0.0))
'            Dim sizeVal As Vector2 = If(Size.HasValue, Size.Value, New Vector2(0.0, 0.0))
'            [Call]("SET_VIEWPORT", _handle, pos.X, pos.Y, sizeVal.X, sizeVal.Y)
'        End If
'        If Priority > 0 Then
'            [Call]("SET_VIEWPORT_PRIORITY", _handle, Priority)
'            _prior = Priority
'        Else
'            Priority = 100
'            _prior = Priority
'        End If
'    End Sub
'    Public ReadOnly Property Handle As Integer
'        Get
'            Return _handle
'        End Get
'    End Property
'    Public ReadOnly Property Exists As Boolean
'        Get
'            Return _handle > 0 AndAlso [Call](Of Boolean)("DOES_VIEWPORT_EXIST", _handle)
'        End Get
'    End Property
'    Public Property Active As Boolean
'        Get
'            Return Exists AndAlso [Call](Of Boolean)("IS_VIEWPORT_ACTIVE", _handle)
'        End Get
'        Set(value As Boolean)
'            If Exists Then
'                [Call]("ACTIVATE_VIEWPORT", _handle, value)
'            Else
'                Tools.WriteLog("Error in NativeViewports.Active (Set) for ID " & _handle & ": viewport doesn't exists or disposed.")
'            End If
'        End Set
'    End Property
'    Public Property Position As Vector2
'        Get
'            Dim pX, pY, pW, pH As New Native.Pointer(GetType(Single))
'            [Call]("GET_VIEWPORT_POS_AND_SIZE", _handle, pX, pY, pW, pH)
'            Return New Vector2(pX.Value, pY.Value)
'        End Get
'        Set(value As Vector2)
'            Dim sizeVal = Me.Size
'            [Call]("SET_VIEWPORT", _handle, value.X, value.Y, sizeVal.X, sizeVal.Y)
'        End Set
'    End Property
'    Public Property Size As Vector2
'        Get
'            Dim pX, pY, pW, pH As New Native.Pointer(GetType(Single))
'            [Call]("GET_VIEWPORT_POS_AND_SIZE", _handle, pX, pY, pW, pH)
'            Return New Vector2(pW.Value, pH.Value)
'        End Get
'        Set(value As Vector2)
'            Dim pos = Me.Position
'            [Call]("SET_VIEWPORT", _handle, pos.X, pos.Y, value.X, value.Y)
'        End Set
'    End Property
'    Public Property Priority As Integer
'        Get
'            Return _prior
'        End Get
'        Set(value As Integer)
'            If Exists AndAlso value > 0 Then
'                [Call]("SET_VIEWPORT_PRIORITY", _handle, value)
'                _prior = value
'            End If
'        End Set
'    End Property
'    Public Property Mirrored As Boolean
'        Get
'            Return _mirror
'        End Get
'        Set(value As Boolean)
'            If Exists Then
'                [Call]("SET_VIEWPORT_MIRRORED", _handle, value)
'                _mirror = value
'            End If
'        End Set
'    End Property
'    ''' <summary>
'    ''' Deletes the viewport and permanently releases all resources.
'    ''' The object should not be used after disposal.
'    ''' </summary>
'    Public Sub Dispose() Implements IDisposable.Dispose
'        Dispose(True)
'        GC.SuppressFinalize(Me)
'    End Sub
'    '=======================================================================================================
'    Protected Sub Dispose(disposing As Boolean)
'        If _disposed Then Return

'        If Exists Then
'            [Call]("DESTROY_VIEWPORT", _handle)
'        End If

'        _handle = 0
'        _disposed = True
'    End Sub
'    Protected Overrides Sub Finalize()
'        Dispose(False)
'    End Sub
'    Public Shared Widening Operator CType(vp As NativeViewport) As Integer
'        If vp Is Nothing OrElse Not vp.Exists Then Return 0
'        Return vp._handle
'    End Operator
'    Public Shared Widening Operator CType(vp As NativeViewport) As GTA.Native.Parameter
'        If vp Is Nothing OrElse Not vp.Exists Then Return New GTA.Native.Parameter(0)
'        Return New GTA.Native.Parameter(vp._handle)
'    End Operator
'End Class
