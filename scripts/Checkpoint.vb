Imports GTA
Imports NativeExtended.NativeDraws
Public Class NativeCheckpoint
    Implements IDisposable
    '============================================================
    ' Private fields
    '============================================================

    Private _Position As GTA.Vector3
    Private _Radius As Single = 1.0F
    Private _Color As ColorRGBA
    Private _ActionToTrigger As Action
    Private _triggerIfInvisible As Boolean = False

    Private _Visible As Boolean = False
    Private _Type As CheckpointType = CheckpointType.Checkpoint

    Private _Blip As GTA.Blip = Nothing

    Private _BlipName As String = Nothing
    Private _BlipIcon As BlipIcon? = Nothing
    Private _BlipSize As Single = 1.0F
    Private _BlipShowOnlyWhenNear As Boolean = False
    Private _BlipColor As ColorRGBA? = Nothing

    Private _BlipUsesCheckpointColor As Boolean = False

    Private _TickHandler As Action
    Private _WasInside As Boolean = False
    Private _Disposed As Boolean = False
    Private Sub New() : End Sub
    Public Enum CheckpointType
        Checkpoint
        Arrow
        Both
    End Enum

    Public Sub New(Position As GTA.Vector3, Optional Radius As Single = 1.0F, Optional Color As ColorRGBA = Nothing, Optional ActionToTrigger As Action = Nothing, Optional TriggersIfInvisible As Boolean = False)
        Me.Position = Position
        Me.Radius = Radius
        Me.Color = Color
        _triggerIfInvisible = TriggersIfInvisible
        Me.ActionToTrigger = ActionToTrigger
        _TickHandler = AddressOf ProcessTick
        TickHelper.Add_Internal(_TickHandler)
    End Sub
    Public Property Position As GTA.Vector3
        Get
            Return _Position
        End Get
        Set(Value As GTA.Vector3)
            _Position = Value
            If _Blip IsNot Nothing Then
                Try
                    _Blip.Position = _Position

                Catch ex As Exception
                    Tools.WriteLog("Error in NativeCheckpoint.Position(Set) for " & _Position.ToString() & ": cannot update blip position.", ex)
                End Try
            End If
        End Set
    End Property

    Public Property Radius As Single
        Get
            Return _Radius
        End Get
        Set(Value As Single)
            If Value <= 0.0F Then
                Tools.WriteLog("Info for NativeCheckpoint.Radius(Set) for " & _Position.ToString() & ": radius must be greater than zero. Using 0.001F.")
                _Radius = 0.001F
            Else
                _Radius = Value
            End If
        End Set

    End Property

    Public Property Color As ColorRGBA
        Get
            Return _Color
        End Get
        Set(Value As ColorRGBA)
            _Color = Value
            If _Blip IsNot Nothing AndAlso _BlipUsesCheckpointColor Then
                Try
                    _Blip.SetColorRGB(GetEffectiveBlipColor.ToARGB)
                Catch ex As Exception
                    Tools.WriteLog("Error in NativeCheckpoint.Color(Set) for " & _Position.ToString() & ": cannot update blip color.", ex)
                End Try
            End If
        End Set
    End Property

    Public Property ActionToTrigger As Action
        Get
            Return _ActionToTrigger
        End Get
        Set(Value As Action)
            _ActionToTrigger = Value
        End Set
    End Property

    Public Property Visible As Boolean
        Get
            Return _Visible
        End Get
        Set(Value As Boolean)
            _Visible = Value

            If _Blip IsNot Nothing Then
                Try
                    If _Visible Then
                        _Blip.Display = GTA.BlipDisplay.MapOnly
                    Else
                        _Blip.Display = GTA.BlipDisplay.Hidden
                    End If
                Catch ex As Exception
                    Tools.WriteLog("Error in NativeCheckpoint.Visible(Set) for " & _Position.ToString() & ": cannot update blip visibility.", ex)
                End Try
            End If
        End Set
    End Property

    Public Property TriggersIfInvisible As Boolean

        Get
            Return _triggerIfInvisible
        End Get

        Set(Value As Boolean)

            If _Disposed Then Return

            _triggerIfInvisible = Value

        End Set

    End Property

    Public Property Type As CheckpointType
        Get
            Return _Type
        End Get
        Set(Value As CheckpointType)
            Select Case Value
                Case CheckpointType.Checkpoint,
                     CheckpointType.Arrow,
                     CheckpointType.Both
                    _Type = Value
                Case Else
                    _Type = CheckpointType.Checkpoint
                    Tools.WriteLog("Info for NativeCheckpoint.Type(Set) for " & _Position.ToString() & ": invalid checkpoint type " & Value & ". Set to default (Checkpoint).")
            End Select
        End Set
    End Property

    Public Sub Dispose() Implements IDisposable.Dispose

        If _Disposed Then Return

        _Disposed = True

        Try
            If _TickHandler IsNot Nothing Then
                TickHelper.Remove_Internal(_TickHandler)
            End If

        Catch ex As Exception
            Tools.WriteLog(
                "Error in NativeCheckpoint.Dispose(void) for " &
                _Position.ToString() &
                ": cannot unregister tick handler. " & ex.Message,
                ex)
        End Try

        Try
            If _Blip IsNot Nothing Then
                _Blip.Delete()
            End If

        Catch ex As Exception
            Tools.WriteLog(
                "Error in NativeCheckpoint.Dispose(void) for " &
                _Position.ToString() &
                ": cannot delete blip. " & ex.Message,
                ex)

        Finally
            _Blip = Nothing

            _BlipName = Nothing
            _BlipIcon = Nothing
            _BlipSize = 1.0F
            _BlipShowOnlyWhenNear = False
            _BlipColor = Nothing

            _BlipUsesCheckpointColor = False

            _TickHandler = Nothing
            _ActionToTrigger = Nothing
        End Try

        GC.SuppressFinalize(Me)

    End Sub

    Public Sub RemoveBlip()
        Try
            If _Blip IsNot Nothing Then
                _Blip.Delete()
            End If

        Catch ex As Exception
            Tools.WriteLog("Error in NativeCheckpoint.SetBlip for " & _Position.ToString() & ": cannot delete blip.", ex)

        Finally
            _Blip = Nothing
            _BlipName = Nothing
            _BlipIcon = Nothing
            _BlipSize = 1.0F
            _BlipShowOnlyWhenNear = False
            _BlipColor = Nothing
            _BlipUsesCheckpointColor = False
        End Try
        Return
    End Sub

    Public Sub SetBlip(Name As String, Icon As GTA.BlipIcon, Optional Size As Single? = Nothing, Optional ShowOnlyWhenNear As Boolean? = Nothing, Optional Color As ColorRGBA? = Nothing)
        If String.IsNullOrEmpty(Name) Then
            Tools.WriteLog("Error in NativeCheckpoint.Setblip for " & _Position.ToString & ": name cannot be empty.")
            Return
        End If

        Dim EffectiveSize As Single = If(Size.HasValue, Size.Value, 1.0F)
        Dim EffectiveShowOnlyWhenNear As Boolean = If(ShowOnlyWhenNear.HasValue, ShowOnlyWhenNear.Value, False)

        If EffectiveSize <= 0.0F Then
            Tools.WriteLog("Error in NativeCheckpoint.SetBlip for " & _Position.ToString() & ": size must be greater than zero. Using 1.0F.")
            EffectiveSize = 1.0F
        End If

        Try
            If _Blip Is Nothing Then
                _Blip = Blip.AddBlip(_Position)
                ' _Blip.Position = Me.Position
                _Blip.Display = GTA.BlipDisplay.MapOnly
            End If
            _BlipName = Name
            _BlipIcon = Icon
            _BlipSize = EffectiveSize
            _BlipShowOnlyWhenNear = EffectiveShowOnlyWhenNear
            _BlipColor = Color
            _BlipUsesCheckpointColor = Not Color.HasValue

            _Blip.Icon = Icon
            _Blip.Name = _BlipName
            _Blip.Scale = _BlipSize
            _Blip.ShowOnlyWhenNear = _BlipShowOnlyWhenNear
            _Blip.SetColorRGB(GetEffectiveBlipColor.ToARGB)

            If _Visible Then
                _Blip.Display = BlipDisplay.MapOnly
            Else
                _Blip.Display = BlipDisplay.Hidden
            End If

        Catch ex As Exception
            Tools.WriteLog("Error in NativeCheckpoint.SetBlip for " & _Position.ToString() & ": cannot create or update blip.", ex)
        End Try
    End Sub

    '====================================================================================================
    ' Internal
    '------------
    Private Sub ProcessTick()
        If _Disposed Then Return
        Try
            If _Visible AndAlso _Position <> Vector3.Zero Then
                Select Case _Type
                    Case CheckpointType.Checkpoint
                        NativeGeneric.DrawCheckpoint(_Position, _Radius, _Color)

                    Case CheckpointType.Arrow
                        NativeGeneric.DrawArrow(_Position, If(_Color = Nothing, New ColorRGBA(160, 0, 175, 255), _Color))

                    Case CheckpointType.Both
                        NativeGeneric.DrawCheckpoint(_Position, _Radius, _Color)
                        NativeGeneric.DrawArrow(_Position, If(_Color = Nothing, New ColorRGBA(160, 0, 175, 255), _Color))
                End Select
            End If

            '============================================================
            ' Trigger processing
            '============================================================
            If _ActionToTrigger Is Nothing Then
                _WasInside = False
                Return
            End If

            ' The trigger is disabled while invisible unless explicitly allowed.
            If Not _Visible AndAlso Not _triggerIfInvisible Then
                _WasInside = False
                Return
            End If

            If _Position = GTA.Vector3.Zero Then
                _WasInside = False
                Return
            End If

            Dim PlayerCharacter As GTA.Ped = NativePlayer.Character
            If PlayerCharacter Is Nothing Then
                _WasInside = False
                Return
            End If

            Dim PlayerPosition As GTA.Vector3 = PlayerCharacter.Position
            Const DetectionHeightOffset As Single = 0.8F

            Dim DetectionPosition As New GTA.Vector3(PlayerPosition.X, PlayerPosition.Y, PlayerPosition.Z - DetectionHeightOffset)
            Dim DX As Single = DetectionPosition.X - _Position.X
            Dim DY As Single = DetectionPosition.Y - _Position.Y
            Dim DZ As Single = DetectionPosition.Z - _Position.Z

            ' Dim DistanceSquared As Single = DX * DX + DY * DY + DZ * DZ
            '  Dim RadiusSquared As Single = _Radius * _Radius
            ' Dim IsInside As Boolean = DistanceSquared <= RadiusSquared
            Dim HorizontalDistanceSquared As Single = DX * DX + DY * DY
            Dim RadiusSquared As Single = _Radius * _Radius
            Dim IsInsideHorizontal As Boolean = HorizontalDistanceSquared <= RadiusSquared

            ' Vertical detection: independently limit the allowed height.
            ' The detection range grows with Radius, but more slowly.
            Dim VerticalRange As Single = Math.Max(1.0F, _Radius * 0.5F)
            Dim IsInsideVertical As Boolean = Math.Abs(DZ) <= VerticalRange
            Dim IsInside As Boolean = IsInsideHorizontal AndAlso IsInsideVertical

            Dim HasJustEntered As Boolean = IsInside AndAlso Not _WasInside

            _WasInside = IsInside
            If HasJustEntered Then
                Try
                    _ActionToTrigger.Invoke()
                Catch ex As Exception
                    Tools.WriteLog("Error in NativeCheckpoint.ProcessTick for " & _Position.ToString() & ": action trigger failed.", ex)
                End Try
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeCheckpoint.ProcessTick for " & _Position.ToString() & ": checkpoint processing failed.", ex)
        End Try
    End Sub
    Private Function GetEffectiveBlipColor() As ColorRGBA
        If _BlipColor.HasValue Then Return _BlipColor.Value

        If _Color = Nothing Then Return ColorRGBA.White
        Return _Color
    End Function
End Class
