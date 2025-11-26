Imports System.IO
Imports System.Windows.Forms
Imports GTA
Imports GTA.base
Imports GTA.Native
Imports GTA.Native.Function
Imports NativeExtended.NativeControls
Public Class NativeDebug
    Public Shared Function AreWidescreenBordersActive() As Boolean
        Return [Call](Of Boolean)("ARE_WIDESCREEN_BORDERS_ACTIVE")
    End Function
    ''' <summary>
    ''' Drag mouse to rotate camera, use Scrollwheel to zoom and double left mouse click to teleport the player.
    ''' </summary>
    Public Shared Property DebugCamera As Boolean
        Get
            Return [Call](Of Boolean)("IS_DEBUG_CAMERA_ON")
        End Get
        Set(value As Boolean)
            If value = True Then
                [Call]("ENABLE_DEBUG_CAM", True)
            Else
                If [Call](Of Boolean)("IS_DEBUG_CAMERA_ON") Then [Call]("DISABLE_DEBUG_CAM_AND_PLAYER_WARPING", True)
            End If
        End Set
    End Property
    ''' <summary>
    ''' If True, will be displayed in-game messages if any script catches an Exception.
    ''' </summary>
    Public Shared Property DisplayErrors As Boolean = False
    Public Shared Sub EnableHighDOF(Enable As Boolean)
        [Call]("SET_USE_HIGHDOF", Enable)
    End Sub
    ''' <summary>
    ''' Disables rendering of all lights on vehicles.
    ''' </summary>
    Public Shared Sub ForceAllVehicleLightsOFF(Optional NoVehicleLights As Boolean = True)
        [Call]("FORCE_ALL_VEHICLE_LIGHTS_OFF", NoVehicleLights)
    End Sub
    ''' <summary>
    ''' Untested. Find only in races_cr SCO script.
    ''' </summary>
    Public Shared Sub ForceHighLOD(Optional Enable As Boolean = True)
        [Call]("FORCE_HIGH_LOD", Enable)
    End Sub
    ''' <summary>
    ''' Find only in policetest SCO script, but seems not working.
    ''' </summary>
    Public Shared Sub SetRenderFlags(Unknown1 As Boolean, Unknown2 As Boolean, Unknown3 As Boolean, Unknown4 As Boolean)
        [Call]("SET_GLOBAL_RENDER_FLAGS", Unknown1, Unknown2, Unknown3, Unknown4)
    End Sub
    Public Shared Sub SetWidescreenBorders(Enable As Boolean, Optional Immediately As Boolean = False)
        If Immediately Then [Call]("SET_INSTANT_WIDESCREEN_BORDERS", Enable) Else [Call]("SET_WIDESCREEN_BORDERS", Enable)
    End Sub

    'Public Shared Function isDebugCameraEnabled() As Boolean
    '    Return [Call](Of Boolean)("IS_DEBUG_CAMERA_ON")
    'End Function
    'Public Shared Sub EnableDebugCamera(Optional Enable As Boolean = True)
    '    If Enable Then
    '        [Call]("ENABLE_DEBUG_CAM", True)
    '    Else
    '        If [Call](Of Boolean)("IS_DEBUG_CAMERA_ON") Then [Call]("DISABLE_DEBUG_CAM_AND_PLAYER_WARPING", True)
    '    End If
    'End Sub
    ''' <summary>
    ''' Not found in SCO scripts. Seems not working.
    ''' </summary>
    Public Shared Sub EnableShadows(ShadowsON As Boolean)
        [Call]("ENABLE_SHADOWS", ShadowsON)
    End Sub

    '''' <summary>
    '''' Classe di debug per disegnare un checkpoint sulla posizione del giocatore.
    '''' Controllabile con NUMPAD + / −, gestita tramite TickHelper.
    '''' </summary>
    'Public NotInheritable Class DebugCheckpoint
    '    Public Structure DebugKeys
    '        Public Shared IncreaseSize_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.NumpadPlus
    '        Public Shared DecreaseSize_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.NumpadMinus
    '        Public Shared MoveUp_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.Numpad9
    '        Public Shared MoveDown_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.Numpad3
    '        Public Shared MoveForward_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.Numpad8
    '        Public Shared MoveBack_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.Numpad2
    '        Public Shared MoveLeft_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.Numpad4
    '        Public Shared MoveRight_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.Numpad6
    '        Public Shared Reset_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.Numpad0
    '        Public Shared SaveToFile_Key As KeyConverter.KeyValues = KeyConverter.KeyValues.EndKey
    '    End Structure


    '    ' --- Colore checkpoint ---
    '    Public Shared Property Color As NativeDraws.ColorRGBA = New NativeDraws.ColorRGBA(255, 255, 0, 255)

    '    ' --- Stato di aggancio ---
    '    Public Shared Property AttachedToPlayer As Boolean = True
    '    Public Shared Property MinRadius As Single = 0.1F
    '    ' --- Posizione fissa ---
    '    Private Shared _fixedPosition As Vector3 = Vector3.Zero
    '    Public Shared Property Position As Vector3
    '        Get
    '            If AttachedToPlayer Then
    '                Dim pl = NativePlayer.Character
    '                If pl IsNot Nothing AndAlso pl.Exists Then Return pl.Position
    '                Return _fixedPosition
    '            Else
    '                Return _fixedPosition
    '            End If
    '        End Get
    '        Set(value As Vector3)
    '            _fixedPosition = value
    '        End Set
    '    End Property

    '    Private Shared _localOffset As Vector3 = Vector3.Zero
    '    Private Shared _attachedPed As Ped
    '    Private Shared _attachedVeh As Vehicle
    '    Private Shared _attachedObj As GTA.Object
    '    Private Shared _attachedPos As Vector3?

    '    Private Shared _enabled As Boolean = False
    '    Private Shared _radius As Single = 2.0F
    '    Private Shared _checkpointHandle As Integer = -1
    '    Private Shared _sensitivity As Single = 0.05F
    '    Private Shared SaveFilePath As String = "\NativeExtended-IV.log"
    '    '   Private Shared _infoKey As String = "DebugCheckpointInfo"
    '    Public Shared ReadOnly Property Radius As Single
    '        Get
    '            Return _radius
    '        End Get
    '    End Property
    '    Public Shared ReadOnly Property OffsetPosition As Vector3
    '        Get
    '            If _attachedPed IsNot Nothing AndAlso _attachedPed.Exists Then
    '                Return _attachedPed.GetOffsetPosition(_localOffset)
    '            ElseIf _attachedVeh IsNot Nothing AndAlso _attachedVeh.Exists Then
    '                Return _attachedVeh.GetOffsetPosition(_localOffset)
    '            ElseIf _attachedObj IsNot Nothing AndAlso _attachedObj.Exists Then
    '                Return _attachedObj.GetOffsetPosition(_localOffset)
    '            ElseIf _attachedPos.HasValue Then
    '                Return _attachedPos.Value + _localOffset
    '            Else
    '                Return Position + _localOffset
    '            End If
    '        End Get
    '    End Property
    '    Public Shared Property Enabled As Boolean
    '        Get
    '            Return _enabled
    '        End Get
    '        Set(value As Boolean)
    '            If value = _enabled Then Return
    '            _enabled = value

    '            If value Then
    '                RegisterToTick()
    '                AddDebugTextInfo()
    '            Else
    '                UnregisterFromTick()
    '                RemoveDebugTextInfo()
    '            End If
    '        End Set
    '    End Property
    '    Private Sub New()
    '    End Sub
    '    ' ================================================================
    '    Public Shared Sub AttachTo(p As Ped, Optional offset As Vector3 = Nothing)
    '        Detach()
    '        If p Is Nothing OrElse Not p.Exists Then Return
    '        _attachedPed = p
    '        _localOffset = offset
    '        AttachedToPlayer = False
    '    End Sub
    '    Public Shared Sub AttachTo(v As Vehicle, Optional offset As Vector3 = Nothing)
    '        Detach()
    '        If v Is Nothing OrElse Not v.Exists Then Return
    '        _attachedVeh = v
    '        _localOffset = offset
    '        AttachedToPlayer = False
    '    End Sub
    '    Public Shared Sub AttachTo(o As GTA.Object, Optional offset As Vector3 = Nothing)
    '        Detach()
    '        If o Is Nothing OrElse Not o.Exists Then Return
    '        _attachedObj = o
    '        _localOffset = offset
    '        AttachedToPlayer = False
    '    End Sub
    '    Public Shared Sub AttachTo(pos As Vector3, Optional offset As Vector3 = Nothing)
    '        Detach()
    '        _attachedPos = pos
    '        _localOffset = offset
    '        _fixedPosition = pos
    '        AttachedToPlayer = False
    '    End Sub
    '    Public Shared Sub Detach()
    '        _attachedPed = Nothing
    '        _attachedVeh = Nothing
    '        _attachedObj = Nothing
    '        _attachedPos = Nothing
    '    End Sub
    '    ' ================================================================
    '    Private Shared Sub OnTick()
    '        Try
    '            Dim pl = NativePlayer.Character
    '            If pl Is Nothing OrElse Not pl.Exists Then Return
    '            Dim sens As Single
    '            If Radius > 0.3 Then
    '                sens = _sensitivity * Tools.GetCoefFPS(30)
    '            Else
    '                sens = _sensitivity / 3 * Tools.GetCoefFPS(30)
    '            End If

    '            ' --- Gestione radius ---
    '            If NativeControls.isKeyPressed(DebugKeys.IncreaseSize_Key, False) Then
    '                _radius = Math.Min(_radius + (0.1F * Tools.GetCoefFPS(30)), 5000.0F)
    '            ElseIf NativeControls.isKeyPressed(DebugKeys.DecreaseSize_Key, False) Then
    '                _radius = Math.Max(_radius - (0.1F * Tools.GetCoefFPS(30)), MinRadius)
    '            End If

    '            ' --- Movimento ---
    '            Dim hasMovementKey As Boolean =
    'NativeControls.isKeyPressed(DebugKeys.MoveForward_Key, False) OrElse NativeControls.isKeyPressed(DebugKeys.MoveBack_Key, False) OrElse
    'NativeControls.isKeyPressed(DebugKeys.MoveLeft_Key, False) OrElse NativeControls.isKeyPressed(DebugKeys.MoveRight_Key, False) OrElse
    'NativeControls.isKeyPressed(DebugKeys.MoveUp_Key, False) OrElse NativeControls.isKeyPressed(DebugKeys.MoveDown_Key, False)

    '            If AttachedToPlayer Then
    '                ' Se era agganciato al player e l’utente inizia a muovere → sgancia
    '                If hasMovementKey Then
    '                    AttachedToPlayer = False
    '                    _fixedPosition = pl.Position
    '                End If
    '            Else
    '                ' === Movimento relativo ===
    '                Dim dirX As Single = 0.0F
    '                Dim dirY As Single = 0.0F

    '                If NativeControls.isKeyPressed(DebugKeys.MoveForward_Key, False) Then dirY += 1.0F
    '                If NativeControls.isKeyPressed(DebugKeys.MoveBack_Key, False) Then dirY -= 1.0F
    '                If NativeControls.isKeyPressed(DebugKeys.MoveLeft_Key, False) Then dirX -= 1.0F
    '                If NativeControls.isKeyPressed(DebugKeys.MoveRight_Key, False) Then dirX += 1.0F

    '                Dim moveVec As New Vector3(dirX * sens, dirY * sens, 0.0F)

    '                ' Calcolo dell’heading di riferimento
    '                Dim refHeading As Single = 0.0F
    '                If _attachedVeh IsNot Nothing AndAlso _attachedVeh.Exists Then
    '                    refHeading = _attachedVeh.Heading
    '                ElseIf _attachedPed IsNot Nothing AndAlso _attachedPed.Exists Then
    '                    refHeading = _attachedPed.Heading
    '                ElseIf _attachedObj IsNot Nothing AndAlso _attachedObj.Exists Then
    '                    refHeading = _attachedObj.Heading
    '                End If

    '                ' Se è agganciato a qualcosa → muovi localmente
    '                If _attachedVeh IsNot Nothing OrElse _attachedPed IsNot Nothing OrElse _attachedObj IsNot Nothing OrElse _attachedPos.HasValue Then
    '                    Dim rotated = CalcWorldOffset(Vector3.Zero, refHeading, moveVec)
    '                    _localOffset += rotated - Vector3.Zero
    '                Else
    '                    ' Se non è agganciato → muove la posizione fissa
    '                    _fixedPosition += moveVec
    '                End If

    '                ' --- Movimento verticale ---
    '                If NativeControls.isKeyPressed(DebugKeys.MoveUp_Key, False) Then _fixedPosition.Z += sens
    '                If NativeControls.isKeyPressed(DebugKeys.MoveDown_Key, False) Then _fixedPosition.Z -= sens
    '            End If


    '            ' --- Reset ---
    '            If NativeControls.isKeyPressed(DebugKeys.Reset_Key, False) Then
    '                Detach()
    '                AttachedToPlayer = True
    '                _fixedPosition = pl.Position
    '                _localOffset = Vector3.Zero
    '            End If

    '            ' --- Salvataggio su file ---
    '            If NativeControls.isKeyPressed(DebugKeys.SaveToFile_Key, False) Then
    '                SaveToFile()
    '            End If

    '            ' --- Aggiorna debug text ---
    '            UpdateDebugTextInfo()

    '            ' --- Disegna checkpoint ---
    '            Dim drawPos = OffsetPosition
    '            [Call]("DRAW_CHECKPOINT_WITH_ALPHA", drawPos.X, drawPos.Y, drawPos.Z, _radius, Color.Red, Color.Green, Color.Blue, Color.Alpha)

    '        Catch ex As Exception
    '        End Try
    '    End Sub
    '    Private Shared Sub AddDebugTextInfo()
    '        Try
    '            ' If Not DebugText.CustomInfo.ContainsKey(_infoKey) Then
    '            DebugText.AddCustomInfo(Function() GetDebugTextString())
    '            ' End If
    '        Catch
    '        End Try
    '    End Sub

    '    Private Shared Sub UpdateDebugTextInfo()
    '        Try
    '            ' If DebugText.AddCustomInfo.ContainsKey(_infoKey) Then
    '            DebugText.AddCustomInfo(Function() GetDebugTextString())
    '            'End If
    '        Catch
    '        End Try
    '    End Sub

    '    Private Shared Sub RemoveDebugTextInfo()
    '        Try
    '            DebugText.RemoveCustomInfo(Function() GetDebugTextString())
    '        Catch
    '        End Try
    '    End Sub

    '    Private Shared Function GetDebugTextString() As String
    '        Dim pos = OffsetPosition
    '        Dim rel = GetRelativePositionToAttachedEntity()

    '        Dim str As String = $"Checkpoint Pos: {pos.X:0.00}, {pos.Y:0.00}, {pos.Z:0.00} | Radius: {_radius:0.0}"

    '        ' Se attaccato a un'entità, mostra anche l’offset relativo
    '        If (_attachedVeh IsNot Nothing AndAlso _attachedVeh.Exists) OrElse
    '   (_attachedPed IsNot Nothing AndAlso _attachedPed.Exists) OrElse
    '   (_attachedObj IsNot Nothing AndAlso _attachedObj.Exists) Then

    '            str &= $"Checkpoint Relative Pos: X={rel.X:0.00}, Y={rel.Y:0.00}, Z={rel.Z:0.00} | Radius: {_radius:0.0}"
    '        End If

    '        Return str
    '    End Function
    '    ''' <summary>
    '    ''' Restituisce la posizione del checkpoint relativa all'entità (es. veicolo) a cui è attaccato.
    '    ''' Se non è attaccato a un'entità valida, restituisce Vector3.Zero.
    '    ''' </summary>
    '    Private Shared Function GetRelativePositionToAttachedEntity() As Vector3
    '        Try
    '            Dim worldPos As Vector3 = OffsetPosition

    '            ' Se agganciato a un veicolo
    '            If _attachedVeh IsNot Nothing AndAlso _attachedVeh.Exists Then
    '                Dim veh = _attachedVeh

    '                ' Calcola l'offset relativo in coordinate locali del veicolo
    '                Dim local = WorldToLocal(worldPos, veh.Position, veh.Heading)
    '                Return local
    '            End If

    '            ' Se agganciato a un pedone
    '            If _attachedPed IsNot Nothing AndAlso _attachedPed.Exists Then
    '                Dim ped = _attachedPed
    '                Return WorldToLocal(worldPos, ped.Position, ped.Heading)
    '            End If

    '            ' Se agganciato a un oggetto
    '            If _attachedObj IsNot Nothing AndAlso _attachedObj.Exists Then
    '                Dim obj = _attachedObj
    '                Return WorldToLocal(worldPos, obj.Position, obj.Heading)
    '            End If

    '            ' Se è solo una posizione fissa
    '            If _attachedPos.HasValue Then
    '                Return worldPos - _attachedPos.Value
    '            End If

    '            Return Vector3.Zero
    '        Catch
    '            Return Vector3.Zero
    '        End Try
    '    End Function
    '    ''' <summary>
    '    ''' Converte una posizione da coordinate mondiali a coordinate locali rispetto a un'entità.
    '    ''' </summary>
    '    Private Shared Function WorldToLocal(worldPos As Vector3, basePos As Vector3, heading As Single) As Vector3
    '        Dim rad As Single = -Tools.ToRadians(heading) ' negativo per l'inverso della rotazione
    '        Dim dx As Single = worldPos.X - basePos.X
    '        Dim dy As Single = worldPos.Y - basePos.Y
    '        Dim dz As Single = worldPos.Z - basePos.Z

    '        Dim localX As Single = dx * Math.Cos(rad) - dy * Math.Sin(rad)
    '        Dim localY As Single = dx * Math.Sin(rad) + dy * Math.Cos(rad)
    '        Dim localZ As Single = dz

    '        Return New Vector3(localX, localY, localZ)
    '    End Function
    '    Private Shared Sub SaveToFile()
    '        Try
    '            Dim pos = OffsetPosition
    '            Dim lines As String() = {
    '            $"Position: {pos.X:0.000}, {pos.Y:0.000}, {pos.Z:0.000}",
    '            $"Radius: {_radius:0.000}"
    '        }
    '            Directory.CreateDirectory(Path.GetDirectoryName(SaveFilePath))
    '            File.WriteAllLines(SaveFilePath, lines)
    '            '  GTA.UI.Notification.Show($"~g~DebugCheckpoint saved to file.")
    '        Catch ex As Exception
    '            ' GTA.UI.Notification.Show($"~r~Save failed: {ex.Message}")
    '        End Try
    '    End Sub
    '    ' ================================================================
    '    Private Shared Sub RegisterToTick()
    '        TickHelper.Add_Internal(AddressOf OnTick)
    '    End Sub

    '    Private Shared Sub UnregisterFromTick()
    '        TickHelper.Remove_Internal(AddressOf OnTick)
    '    End Sub

    '    ' ================================================================
    '    '  UTILITY
    '    ' ================================================================
    '    Private Shared Function CalcWorldOffset(basePos As Vector3, heading As Single, localOffset As Vector3) As Vector3
    '        Dim rad = Tools.ToRadians(heading)
    '        Dim cos = CSng(Math.Cos(rad))
    '        Dim sin = CSng(Math.Sin(rad))
    '        Dim worldX = localOffset.X * cos - localOffset.Y * sin
    '        Dim worldY = localOffset.X * sin + localOffset.Y * cos
    '        Return New Vector3(basePos.X + worldX, basePos.Y + worldY, basePos.Z + localOffset.Z)
    '    End Function
    'End Classì

    ''' <summary>
    ''' A simple non-native class to draw debug infos.
    ''' You must call TickHelper.ProcessAll() in a Tick inside your
    ''' main script. Then you can enable or disable the debug text.
    ''' </summary>
    Public Class DebugText
        Public Shared Property Enabled As Boolean
            Get
                Return _textEnabled
            End Get
            Set(value As Boolean)
                If _textEnabled = value Then Exit Property

                If value = True Then
                    _textEnabled = True

                    Try
                        If Not IsRegisteredInTickHelper Then
                            TickHelper.Add_Internal(AddressOf OnTick)
                            IsRegisteredInTickHelper = True
                        End If
                    Catch ex As Exception

                    End Try
                Else
                    _textEnabled = False

                    Try
                        If IsRegisteredInTickHelper Then
                            TickHelper.Remove_Internal(AddressOf OnTick)
                            IsRegisteredInTickHelper = False
                        End If
                    Catch ex As Exception

                    End Try
                End If
            End Set
        End Property
        Public Shared Property ShowDefaultInfo As Boolean = True
        Private Shared ReadOnly CustomInfoFunctions As New List(Of Func(Of String))
        Private Shared _textEnabled As Boolean
        Private Const DisplayTime As Integer = 2000 ' ms

        Private Shared IsRegisteredInTickHelper As Boolean = False
        Private Shared Sub OnTick()
            If Not Enabled Then Exit Sub

            Dim lines As New List(Of String)

            If ShowDefaultInfo Then
                lines.AddRange(GetDefaultDebugInfo())
            End If

            For Each func In CustomInfoFunctions
                Try
                    Dim result As String = func.Invoke()
                    If Not String.IsNullOrEmpty(result) Then lines.Add(result)
                Catch ex As Exception
                    lines.Add($"[Error func]: {ex.Message}")
                End Try
            Next

            If lines.Count > 0 Then
                Dim finalText As String = String.Join(vbCrLf, lines)
                Game.DisplayText(finalText, DisplayTime)
            End If
        End Sub
        ''' <summary>
        ''' Adds a custom info to display.
        ''' </summary>
        Public Shared Sub AddCustomInfo(func As Func(Of String))
            If func IsNot Nothing AndAlso Not CustomInfoFunctions.Contains(func) Then
                CustomInfoFunctions.Add(func)
            End If
        End Sub

        ''' <summary>
        ''' Remove a custom info added to the list.
        ''' </summary>
        Public Shared Sub RemoveCustomInfo(func As Func(Of String))
            If func IsNot Nothing AndAlso CustomInfoFunctions.Contains(func) Then
                CustomInfoFunctions.Remove(func)
            End If
        End Sub
        ''' <summary>
        ''' Removes all custom infos added to the list.
        ''' </summary>
        Public Shared Sub ClearCustomInfo()
            CustomInfoFunctions.Clear()
        End Sub
        Private Shared Function GetDefaultDebugInfo() As List(Of String)
            Dim list As New List(Of String)
            Try
                Dim player = NativePlayer.Character
                Dim pos As Vector3 = player.Position
                Dim rot As Single = player.Heading

                list.Add($"Position: X={pos.X:F2} Y={pos.Y:F2} Z={pos.Z:F2} Heading: {player.Heading:F1}")
                ' list.Add($"Health: {player.Health}") '/{player.MaxHealth}")

                If player.isSittingInVehicle Then
                    Dim veh = player.CurrentVehicle
                    list.Add($"{veh.Name}  Speed: {veh.Speed:F2}  Rotation: {veh.Rotation}")
                Else
                    list.Add($"On Foot. Speed: {player.Velocity.Length():F2} m/s")
                End If

            Catch ex As Exception
                list.Add($"[Debug Error]: {ex.Message}")
            End Try
            Return list
        End Function
    End Class
End Class
