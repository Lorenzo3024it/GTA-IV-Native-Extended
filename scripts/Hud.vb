Imports GTA
Imports GTA.Native.Function
''' <summary>
''' Native HUD effects manager for flashing and persistent display elements.
''' Handles radar, weapon icon, GPS route, blips, HUD visibility, and interior radar display.
''' Automatically uses internal TickHelper integration and native calls.
''' </summary>
Public NotInheritable Class NativeHud
    Private Sub New() : End Sub
    Private Shared _ShouldForceRadarViewportOff As Boolean = False

#Region "Native Functions & flashing effects"
    Public Shared Sub RemovePhoneHudItem()
        [Call]("SET_PHONE_HUD_ITEM", 0, "", -1)
    End Sub

    Public Shared Sub DisplayCash(Display As Boolean)
        [Call]("DISPLAY_CASH", Display)
    End Sub

    Public Shared WriteOnly Property DisplayAreaName As Boolean
        Set(value As Boolean)
            [Call]("DISPLAY_AREA_NAME", value)
        End Set
    End Property

    Public Shared WriteOnly Property DisplayMapBlips As Boolean
        Set(value As Boolean)
            [Call]("DISPLAY_FRONTEND_MAP_BLIPS", value)
        End Set
    End Property

    Public Shared Sub DisplayAmmo(Display As Boolean)
        [Call]("DISPLAY_AMMO", Display)
    End Sub

    'Public Shared WriteOnly Property DisplayRadar As Boolean
    '    Set(value As Boolean)
    '        [Call]("DISPLAY_RADAR", value)
    '    End Set
    'End Property
    ''' <summary>
    ''' Displays radar as if the player was inside an interior. You must call TickHelper.ProcessAll in a tick.
    ''' </summary>
    Public Shared Property DisplayRadarAsInterior As Boolean
        Get
            Return _displayRadarAsInterior
        End Get
        Set(value As Boolean)
            If value = _displayRadarAsInterior Then Exit Property

            _displayRadarAsInterior = value
            If value Then
                TickHelper.Add_Internal(TickRadarInterior)
            Else
                TickHelper.Remove_Internal(TickRadarInterior)
            End If
        End Set
    End Property
    Public Shared Sub DisplayRadarAsInterior_ThisFrame()
        [Call]("SET_RADAR_AS_INTERIOR_THIS_FRAME")
    End Sub
    ''' <summary>
    ''' Starts or stops radar flashing.
    ''' Duration = -1 → continuous flash; 0 → stop; >0 → auto-stop after ms.
    ''' </summary>
    Public Shared Sub FlashRadar(Optional Duration As Integer = -1)
        InternalStart("radar", Duration)
    End Sub
    ''' <summary>
    ''' Starts or stops weapon icon flashing.
    ''' Duration = -1 → continuous flash; 0 → stop; >0 → auto-stop after ms.
    ''' </summary>
    Public Shared Sub FlashWeaponIcon(Optional Duration As Integer = -1)
        InternalStart("weapon", Duration)
    End Sub
    ''' <summary>
    ''' Starts or stops GPS route flashing.
    ''' Duration = -1 → continuous flash; 0 → stop; >0 → auto-stop after ms.
    ''' </summary>
    Public Shared Sub FlashRoute(Optional Duration As Integer = -1)
        InternalStart("gps", Duration)
    End Sub
    ''' <summary>
    ''' Starts or stops a specific blip flashing.
    ''' Alt = True uses FLASH_BLIP_ALT.
    ''' Duration = -1 → continuous flash; 0 → stop; >0 → auto-stop after ms.
    ''' </summary>
    Public Shared Sub FlashBlip(Blip As Blip, Optional Duration As Integer = -1, Optional Alt As Boolean = False)
        If Blip Is Nothing Then Exit Sub
        Dim key As String = If(Alt, $"blip_alt:{Blip}", $"blip:{Blip}")
        InternalStart(key, Duration, Blip)
    End Sub

    '''' <summary>
    '''' Hides or shows HUD and radar every frame. You must call TickHelper.ProcessAll in a tick to hide hud.
    '''' </summary>
    'Public Shared Property HideHud As Boolean
    '    Get
    '        Return _hideHud
    '    End Get
    '    Set(value As Boolean)
    '        If value = _hideHud Then Exit Property
    '        _hideHud = value
    '        If value Then
    '            TickHelper.Add_Internal(TickHideHud)
    '        Else
    '            TickHelper.Remove_Internal(TickHideHud)
    '        End If
    '    End Set
    'End Property
    ''' <summary>
    ''' Hides or show hud. It could be enabled again by SCO scripts.
    ''' </summary>
    ''' <param name="Hide"></param>
    Public Shared Sub HideHud(Hide As Boolean) ', Optional ForceHiddenState As Boolean = False)
        [Call]("DISPLAY_HUD", Not Hide)
        'If Hide Then
        'If ForceHiddenState Then
        '    TickHelper.Add_Internal(TickHideHud)
        'End If
        'Else
        '   TickHelper.Remove_Internal(TickHideHud)
        'End If
    End Sub
    ''' <summary>
    ''' Hides or shows HUD and radar on this frame. Must be called on a tick (cycle).
    ''' </summary>
    Public Shared Sub HideRadarAndHud_ThisFrame()
        [Call]("HIDE_HUD_AND_RADAR_THIS_FRAME")
    End Sub
    '''' <summary>
    '''' Hides or shows Radar deactivating his viewport. You must call TickHelper.ProcessAll in a tick to avoid automatic enabling after pause menu.
    '''' </summary>
    'Public Shared Property HideRadar As Boolean
    '    Get
    '        Return NativeViewport.Radar.Enabled OrElse _hideHud
    '    End Get
    '    Set(value As Boolean)
    '        If NativeViewport.Radar.Enabled = value Then NativeViewport.Radar.Enabled = Not value
    '    End Set
    'End Property
    ''' <summary>
    ''' Hides or shows radar, optionally disabling viewport (needs TickHelper to prevent automatic activation after pause menu).
    ''' </summary>
    ''' <param name="ForceHiddenState">If True (and Hide is True), it will be deactivate radar viewport (needs TickHelper).</param>
    Public Shared Sub HideRadar(Hide As Boolean, Optional ForceHiddenState As Boolean = False)
        [Call]("DISPLAY_RADAR", Not Hide)
        If Hide Then
            If ForceHiddenState Then
                If NativeViewport.Radar.Enabled Then NativeViewport.Radar.Enabled = False
                TickHelper.Add_Internal(AddressOf CheckRadarViewport_OnTick)
                _ShouldForceRadarViewportOff = True
            End If
        Else
            _ShouldForceRadarViewportOff = False
            TickHelper.Remove_Internal(AddressOf CheckRadarViewport_OnTick)
            If NativeViewport.Radar.Enabled = False Then NativeViewport.Radar.Enabled = True
        End If
    End Sub
    Private Shared Sub CheckRadarViewport_OnTick()
        If _ShouldForceRadarViewportOff Then
            If NativeViewport.Radar.Enabled Then NativeViewport.Radar.Enabled = False
        End If
    End Sub
    Public Shared ReadOnly Property IsPreferenceON As Boolean
        Get
            Return [Call](Of Boolean)("IS_HUD_PREFERENCE_SWITCHED_ON")
        End Get
    End Property

    Public Shared ReadOnly Property IsCrosshairComplex As Boolean
        Get
            Return [Call](Of Boolean)("IS_HUD_RETICULE_COMPLEX")
        End Get
    End Property
    'Public Shared Function isHudPreferenceON() As Boolean
    '    Return [Call](Of Boolean)("IS_HUD_PREFERENCE_SWITCHED_ON")
    'End Function
    'Public Shared Function isHudReticuleComplex() As Boolean
    '    Return [Call](Of Boolean)("IS_HUD_RETICULE_COMPLEX")
    'End Function
    Public Shared WriteOnly Property RadarZoom As Single
        Set(value As Single)
            [Call]("SET_RADAR_ZOOM", value)
        End Set
    End Property
    'SET_MISSION_PASSED_CASH (?)
    'GET_HUD_COLOUR(colorindex, pointer int red, pointer int green, pointerint blue, pointerint unknown)
    ''' <summary>
    ''' Stops all active HUD effects and flashes.
    ''' </summary>
    Public Shared Sub StopAllFlashes()
        SyncLock _lock
            For Each k In _entries.Keys.ToList()
                InternalStopKey(k)
            Next
        End SyncLock
    End Sub


#Region "Internals"

#End Region

#End Region

    '#Region "Debug tools"

    '#If DEBUG Then
    '    ''' <summary>
    '    ''' Draws debug text with all currently active HUD effects.
    '    ''' </summary>
    '    Private Shared Sub DrawDebugText()
    '        Dim y As Single = 0.25F
    '        For Each e In _entries.Values
    '            Dim label As String = $"{e.Key} (Active: {e.Active}, End: {If(e.EndTime, -1)})"
    '            [Call]("DRAW_TEXT", label, 0.02F, y)
    '            y += 0.02F
    '        Next
    '    End Sub
    '#End If

    '#End Region

#Region "Internal engine"
    '===================================================================================================================================
    ' HIDE HUD / DISPLAY RADAR AD INTERIOR
    '=======================================
    Private Shared _hideHud As Boolean = False
    Private Shared ReadOnly TickHideHudAndRadar As New Action(AddressOf HideRadarAndHud_ThisFrame)

    'Private Shared Sub OnTickHideHud()
    '    [Call]("HIDE_HUD_AND_RADAR_THIS_FRAME")
    'End Sub

    'Private Shared Sub OnTickRadarAsInterior()
    '    [Call]("DISPLAY_RADAR_AS_INTERIOR_THIS_FRAME")
    'End Sub
    Private Shared _displayRadarAsInterior As Boolean = False
    Private Shared ReadOnly TickRadarInterior As New Action(AddressOf DisplayRadarAsInterior_ThisFrame)

    Private Shared _displayAsInterior As Boolean = False
    Private Shared _timerActive As Boolean = False
    Private Shared _targetVisible As Boolean = True
    Private Shared _delayTime As Integer = 0
    Private Shared _timerStart As Integer = 0
    Private Shared ReadOnly _tickTriggerHudState As New Action(AddressOf TriggerStateTick)
    Friend Shared Sub TriggerHudState(Visible As Boolean, Optional Time As Integer = 3000, Optional DisplayRadarAsInterior As Boolean = False)
        ' Se il tempo è 0 → esegue subito
        If Time <= 0 Then
            _hideHud = Not Visible
            _displayAsInterior = DisplayRadarAsInterior
            Return
        End If

        ' Imposta i valori e avvia il timer
        _targetVisible = Visible
        _delayTime = Time
        _displayAsInterior = DisplayRadarAsInterior
        _timerStart = NativeTimer.TotalGameTime
        _timerActive = True
        If Not TickHelper.ContainsHandler_Internal(_tickTriggerHudState) Then
            TickHelper.Add_Internal(_tickTriggerHudState)
        End If
    End Sub
    Private Shared Sub TriggerStateTick()
        If Not _timerActive Then Return

        Dim elapsed As Integer = NativeTimer.TotalGameTime - _timerStart

        If elapsed >= _delayTime Then
            ' Cambia lo stato del HUD
            _hideHud = Not _targetVisible
            _timerActive = False

            TickHelper.Remove_Internal(_tickTriggerHudState)
        Else
            If _hideHud Then [Call]("HIDE_HUD_AND_RADAR_THIS_FRAME")
            If _displayAsInterior Then [Call]("DISPLAY_RADAR_AS_INTERIOR_THIS_FRAME")
        End If
    End Sub
    '===================================================================================================================================
    ' Flash
    '--------
    Private Shared ReadOnly _lock As New Object()
    Private Class FlashEntry
        Public Key As String
        Public EndTime As Integer?
        Public Active As Boolean
        Public Obj As Object
    End Class

    Private Shared ReadOnly _entries As New Dictionary(Of String, FlashEntry)
    Private Shared ReadOnly _tick As New FlashTickController()
    Private Shared Sub InternalStart(key As String, durationMs As Integer, Optional obj As Object = Nothing)
        SyncLock _lock
            Dim now = NativeTimer.TotalGameTime
            Dim endTime As Integer? = Nothing

            If durationMs = 0 Then
                InternalStopKey(key)
                Return
            ElseIf durationMs > 0 Then
                endTime = now + durationMs
            End If

            Dim entry As FlashEntry
            If _entries.ContainsKey(key) Then
                entry = _entries(key)
                entry.EndTime = endTime
                entry.Active = True
                entry.Obj = obj
            Else
                entry = New FlashEntry With {.Key = key, .EndTime = endTime, .Active = True, .Obj = obj}
                _entries.Add(key, entry)
            End If

            ApplyNative(entry, True)
            _tick.EnsureEnabled()
        End SyncLock
    End Sub
    Private Shared Sub InternalStopKey(key As String)
        SyncLock _lock
            If Not _entries.ContainsKey(key) Then Exit Sub
            Dim entry = _entries(key)
            entry.Active = False
            entry.EndTime = 0
            ApplyNative(entry, False)
            _entries.Remove(key)
            If _entries.Count = 0 Then _tick.EnsureDisabled()
        End SyncLock
    End Sub
    ''' <summary>
    ''' Executes native calls based on the effect key.
    ''' </summary>
    Private Shared Sub ApplyNative(entry As FlashEntry, enable As Boolean)
        Try
            Dim state As Integer = If(enable, 1, 0)
            Select Case True
                Case entry.Key = "radar"
                    [Call]("FLASH_RADAR", state)

                Case entry.Key = "weapon"
                    [Call]("FLASH_WEAPON_ICON", state)

                Case entry.Key = "gps"
                    [Call]("FLASH_ROUTE", state)

                Case entry.Key = "hide_hud"
                    [Call]("HIDE_HUD_AND_RADAR_THIS_FRAME", enable)

                Case entry.Key = "radar_interior"
                    [Call]("SET_RADAR_AS_INTERIOR_THIS_FRAME", enable)

                Case entry.Key.StartsWith("blip:")
                    Dim bl As Blip = TryCast(entry.Obj, Blip)
                    If bl IsNot Nothing Then [Call]("FLASH_BLIP", bl, state)

                Case entry.Key.StartsWith("blip_alt:")
                    Dim bl As Blip = TryCast(entry.Obj, Blip)
                    If bl IsNot Nothing Then [Call]("FLASH_BLIP_ALT", bl, state)
            End Select
        Catch
            ' Ignore exceptions to keep tick safe
        End Try
    End Sub
#End Region

#Region "Tick controller"
    ''' <summary>
    ''' Internal tick controller that manages automatic enabling/disabling in TickHelper.
    ''' </summary>
    Private Class FlashTickController
        Private Shared ReadOnly _process As Action = AddressOf Process
        Private _enabled As Boolean = False
        Public Sub EnsureEnabled()
            If Not _enabled Then
                TickHelper.Add_Internal(_process)
                _enabled = True
            End If
        End Sub
        Public Sub EnsureDisabled()
            If _enabled Then
                TickHelper.Remove_Internal(_process)
                _enabled = False
            End If
        End Sub
        Private Shared Sub Process()
            Dim now = NativeTimer.TotalGameTime
            Dim toRemove As New List(Of String)

            SyncLock _lock
                For Each kv In _entries.ToArray()
                    Dim e = kv.Value
                    If Not e.Active Then
                        toRemove.Add(kv.Key)
                        Continue For
                    End If

                    If e.EndTime.HasValue AndAlso now >= e.EndTime.Value Then
                        ApplyNative(e, False)
                        toRemove.Add(kv.Key)
                    End If
                Next

                For Each k In toRemove
                    _entries.Remove(k)
                Next

                If _entries.Count = 0 Then _tick.EnsureDisabled()
            End SyncLock
        End Sub
    End Class
#End Region
End Class


