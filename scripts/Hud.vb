Imports System.Diagnostics.Eventing
Imports System.Security.Policy
Imports GTA
Imports GTA.Native.Function
Imports NativeExtended
''' <summary>
''' Native HUD effects manager for flashing and persistent display elements.
''' Handles radar, weapon icon, GPS route, blips, HUD visibility, and interior radar display.
''' Automatically uses internal TickHelper integration and native calls.
''' </summary>
Public Class NativeHud

#Region "Native Functions & flashing effects controlled by TickHelper"
    Public Shared ReadOnly Property isHudPreferenceON As Boolean
        Get
            Return [Call](Of Boolean)("IS_HUD_PREFERENCE_SWITCHED_ON")
        End Get
    End Property
    Public Shared ReadOnly Property isHudReticuleComplex As Boolean
        Get
            Return [Call](Of Boolean)("IS_HUD_RETICULE_COMPLEX")
        End Get
    End Property
    Public Shared Sub ClearPhoneHudItem()
        [Call]("SET_PHONE_HUD_ITEM", 0, "", -1)
    End Sub
    Public Shared Sub DisplayCash(Display As Boolean)
        [Call]("DISPLAY_CASH", Display)
    End Sub
    Public Shared Sub DisplayAreaName(Display As Boolean)
        [Call]("DISPLAY_AREA_NAME", Display)
    End Sub
    Public Shared Sub DisplayFrontedMapBlips(Display As Boolean)
        [Call]("DISPLAY_FRONTEND_MAP_BLIPS", Display)
    End Sub
    Public Shared Sub DisplayAmmo(Display As Boolean)
        [Call]("DISPLAY_AMMO", Display)
    End Sub
    Public Shared Sub DisplayRadar(Display As Boolean)
        [Call]("DISPLAY_RADAR", Display)
    End Sub
    Public Shared Sub DisplayRadarAsInterior_ThisFrame()
        [Call]("SET_RADAR_AS_INTERIOR_THIS_FRAME")
    End Sub
    '''' <summary>
    '''' Displays radar as interior mode. Can persist automatically using internal TickHelper.
    '''' </summary>
    'Public Shared Sub DisplayRadarAsInterior(Optional Enable As Boolean = True)
    '    InternalStart("radar_interior", If(Enable, -1, 0))
    'End Sub
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
    Public Shared Sub FlashBlip(blip As Blip, Optional Duration As Integer = -1, Optional Alt As Boolean = False)
        If blip Is Nothing Then Exit Sub
        Dim key As String = If(Alt, $"blip_alt:{blip}", $"blip:{blip}")
        InternalStart(key, Duration, blip)
    End Sub
    '''' <summary>
    '''' Hides or shows the entire HUD. Can persist automatically using internal TickHelper.
    '''' </summary>
    'Public Shared Sub HideHUD(Optional Hide As Boolean = True)
    '    InternalStart("hide_hud", If(Hide, -1, 0))
    'End Sub

    Public Shared Sub HideHudAndRadar_ThisFrame()
        [Call]("HIDE_HUD_AND_RADAR_THIS_FRAME")
    End Sub
    'Public Shared Function isHudPreferenceON() As Boolean
    '    Return [Call](Of Boolean)("IS_HUD_PREFERENCE_SWITCHED_ON")
    'End Function
    'Public Shared Function isHudReticuleComplex() As Boolean
    '    Return [Call](Of Boolean)("IS_HUD_RETICULE_COMPLEX")
    'End Function
    Public Shared Sub RemovePhoneHudItem()
        [Call]("SET_PHONE_HUD_ITEM", 0, "", -1)
    End Sub
    Public Shared Sub SetMessagesWaiting(MessageIcon As Boolean)
        [Call]("SET_MESSAGES_WAITING", MessageIcon)
    End Sub
    Public Shared Sub SetPhoneHudItem(Icon As PhoneHudItems, NumberInFrontOfString As Integer, Optional GXT_Text As String = "UNREAD_MESSAGES")
        [Call]("SET_PHONE_HUD_ITEM", CInt(Icon), GXT_Text, NumberInFrontOfString)
    End Sub
    Public Shared Sub SetRadarZoom(Zoom As Single)
        [Call]("SET_RADAR_ZOOM", Zoom)
    End Sub
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
    Public Enum PhoneHudItems
        TextMessage = 1
        NewContact = 2
        AppointmentRing = 3
        NewAppointment = 4
        MissionReplay = 5
        PhotoSent = 6
        PhoneDisabled = 7
        RespectGained = 8
        RespectLost = 9
        InviteReceived = 10
    End Enum


#End Region

#Region "Debug tools"

#If DEBUG Then
    ''' <summary>
    ''' Draws debug text with all currently active HUD effects.
    ''' </summary>
    Private Shared Sub DrawDebugText()
        Dim y As Single = 0.25F
        For Each e In _entries.Values
            Dim label As String = $"{e.Key} (Active: {e.Active}, End: {If(e.EndTime, -1)})"
            [Call]("DRAW_TEXT", label, 0.02F, y)
            y += 0.02F
        Next
    End Sub
#End If

#End Region

#Region "Internal engine"

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
            Dim now = NativeTimer.getTimerC()
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
            Dim now = NativeTimer.getTimerC()
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


