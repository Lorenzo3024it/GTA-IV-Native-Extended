Imports GTA
Imports GTA.Native.Function
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
    ''' Untested. Find only in races_cr SCOCL script.
    ''' </summary>
    Public Shared Sub ForceHighLOD(Optional Enable As Boolean = True)
        [Call]("FORCE_HIGH_LOD", Enable)
    End Sub
    ''' <summary>
    ''' Find only in policetest SCOCL script, but seems not working.
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
    ''' Not found in SCOCL scripts. Seems not working.
    ''' </summary>
    Public Shared Sub EnableShadows(ShadowsON As Boolean)
        [Call]("ENABLE_SHADOWS", ShadowsON)
    End Sub
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
        Public Sub AddCustomInfo(func As Func(Of String))
            If func IsNot Nothing AndAlso Not CustomInfoFunctions.Contains(func) Then
                CustomInfoFunctions.Add(func)
            End If
        End Sub

        ''' <summary>
        ''' Remove a custom info added to the list.
        ''' </summary>
        Public Sub RemoveCustomInfo(func As Func(Of String))
            If func IsNot Nothing AndAlso CustomInfoFunctions.Contains(func) Then
                CustomInfoFunctions.Remove(func)
            End If
        End Sub
        ''' <summary>
        ''' Removes all custom infos added to the list.
        ''' </summary>
        Public Sub ClearCustomInfo()
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
                    list.Add($"Vehicle: {veh.Name}  Speed: {veh.Speed:F2}  Rotation: {veh.Rotation}  Health: {veh.Health}  Engine: {veh.EngineHealth}  PetrolTank: {veh.PetrolTankHealth}")
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
