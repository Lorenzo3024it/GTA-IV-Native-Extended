Imports System.Net.Security
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles
Imports GTA
Imports GTA.Native.Function

Partial Public NotInheritable Class NativeCamera
    Implements IDisposable
    Private _handl As Integer
    Private _coll As Boolean = True
    'Private Shared _rot As Vector3
    'Private Shared _pos As Vector3
    Private pPos1, pPos2, pPos3, pRot1, pRot2, pRot3, pFov As Native.Pointer
    Private pFarDof, pNearDof, pFarClip, pNearClip As New Native.Pointer(GetType(Single))
    Public Sub New(Optional Type As CameraType = CameraType.ScriptedCam, Optional Position As Vector3? = Nothing, Optional Name As String = Nothing)
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        pPos1 = New Native.Pointer(GetType(Single))
        pPos2 = New Native.Pointer(GetType(Single))
        pPos3 = New Native.Pointer(GetType(Single))
        pFov = New Native.Pointer(GetType(Single))
        pRot1 = New Native.Pointer(GetType(Single))
        pRot2 = New Native.Pointer(GetType(Single))
        pRot3 = New Native.Pointer(GetType(Single))
        [Call]("CREATE_CAM", Type, pointer)
        _handl = pointer.Value
        If Position.HasValue Then
            [Call]("SET_CAM_POS", _handl, Position.Value.X, Position.Value.Y, Position.Value.Z)
        End If
        [Call]("ENABLE_CAM_COLLISION", _handl, _coll)
        _type = Type
        If String.IsNullOrEmpty(Name) Then
            Name = $"Camera_{_handl}"
        End If

        [Call]("SET_CAM_NAME", _handl, Name)
        _ownsCamera = True
    End Sub
    ''' <summary>
    ''' Activates the camera.
    ''' Internally calls SET_CAM_ACTIVE and SET_CAM_PROPAGATE.
    ''' Optionally enables scripted camera rendering via ACTIVATE_SCRIPTED_CAMS.
    ''' </summary>
    ''' <param name="ShowNow">If True the camera will be rendered immediately (SET_CAM_PROPAGATE is called)</param>
    ''' <param name="ActivateScripedCams"> If True, ACTIVATE_SCRIPTED_CAMS is called to immediately render scripted cameras.</param>
    ''' <param name="RenderEffects"> If True, SET_PTFX_CAM_INSIDE_VEHICLE is called to immediately render PTFXs, light effects and proper detail distance.</param>
    Public Sub Activate(Optional ShowNow As Boolean = True, Optional ActivateScripedCams As Boolean = True, Optional RenderEffects As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Not Exists Then Return

        [Call]("SET_CAM_ACTIVE", _handl, 1)
        If ShowNow Then [Call]("SET_CAM_PROPAGATE", _handl, 1)
        If ActivateScripedCams Then [Call]("ACTIVATE_SCRIPTED_CAMS", 1, 1)
        If RenderEffects Then [Call]("SET_PTFX_CAM_INSIDE_VEHICLE", 1)
    End Sub
    Public Sub AttachTo(Car As Vehicle, Position As Vector3, Optional IsOffsetRelative As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then
            [Call]("ATTACH_CAM_TO_VEHICLE", _handl, Car)
            [Call]("SET_CAM_ATTACH_OFFSET", _handl, Position.X, Position.Y, Position.Z)
            [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
        Else
            Tools.WriteLog("Error in NativeCamera...AttachTo (Car).")
        End If
    End Sub
    Public Sub AttachTo(Ped As Ped, Position As Vector3, Optional IsOffsetRelative As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then
            [Call]("ATTACH_CAM_TO_PED", _handl, Ped)
            [Call]("SET_CAM_ATTACH_OFFSET", _handl, Position.X, Position.Y, Position.Z)
            [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
        Else
            Tools.WriteLog("Error in NativeCamera...AttachTo (Ped).")
        End If
    End Sub
    Public Sub AttachTo(Obj As [Object], Position As Vector3, Optional IsOffsetRelative As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then
            [Call]("ATTACH_CAM_TO_OBJECT", _handl, Obj)
            [Call]("SET_CAM_ATTACH_OFFSET", _handl, Position.X, Position.Y, Position.Z)
            [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
        Else
            Tools.WriteLog("Error in NativeCamera...AttachTo (Obj).")
        End If
    End Sub
    Public Property Collision As Boolean
        Get
            If Not CheckNotDisposed() Then Return False
            Return _coll
        End Get
        Set(value As Boolean)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("ENABLE_CAM_COLLISION", _handl, value)
                _coll = value
            Else
                Tools.WriteLog("Error in NativeCamera...Collision.")
            End If
        End Set
    End Property
    Public Shared ReadOnly Property CurrentCamera As NativeCamera
        Get
            Dim count As Integer = CountScriptCams(CameraType.ScriptedCam, True, True)
            If count > 0 Then
                Dim ptr As New Native.Pointer(GetType(Integer))
                [Call]("GET_SCRIPT_CAM", ptr)
                If ptr.Value <> 0 Then
                    Return WrapScriptCam(ptr.Value)
                End If
            End If

            Return DefaultCamera
        End Get
    End Property
    Public Sub Delete()
        If Not CheckNotDisposed() Then Return
        If Not _ownsCamera Then Return
        If Me.Exists Then [Call]("DESTROY_CAM", _handl)
        pPos1 = Nothing
        pPos2 = Nothing
        pPos3 = Nothing
        pRot1 = Nothing
        pRot2 = Nothing
        pRot3 = Nothing
        pFov = Nothing
        _handl = 0
    End Sub
    Public Sub Detach()
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then [Call]("UNATTACH_CAM", _handl)
    End Sub
    ''' <summary>
    ''' Returns the forward direction vector of the camera,
    ''' derived from its rotation.
    ''' </summary>
    Public ReadOnly Property Direction As Vector3
        Get
            If Not CheckNotDisposed() Then Return Vector3.Zero
            If Not Exists Then Return Vector3.Zero

            Dim rot As Vector3 = Me.Rotation

            Dim pitch As Double = rot.X * Math.PI / 180.0
            Dim yaw As Double = rot.Z * Math.PI / 180.0

            Dim x As Double = -Math.Sin(yaw) * Math.Cos(pitch)
            Dim y As Double = Math.Cos(yaw) * Math.Cos(pitch)
            Dim z As Double = Math.Sin(pitch)

            '  Return New Vector3(CSng(x), CSng(y), CSng(z))
            Dim dir As New Vector3(CSng(x), CSng(y), CSng(z))
            dir.Normalize()
            Return dir
        End Get
    End Property
    ''' <summary>
    ''' Deletes the camera and permanently releases all resources.
    ''' The object should not be used after disposal.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    ''' <summary>
    ''' Deactivates the camera.
    ''' Disables SET_CAM_PROPAGATE.
    ''' Optionally shuts down camera and general scripted camera rendering.
    ''' </summary>
    ''' <param name="ShutDown"> If True, SET_CAM_ACTIVE is set to False.</param>
    ''' <param name="DeactivateScripedCams"> If True, ACTIVATE_SCRIPTED_CAMS is called to stop rendering scripted cameras.</param>
    Public Sub Deactivate(Optional ShutDown As Boolean = False, Optional DeactivateScripedCams As Boolean = False)
        If Not CheckNotDisposed() Then Return
        If Not Exists Then Return

        [Call]("SET_CAM_PROPAGATE", _handl, 0)
        If ShutDown Then [Call]("SET_CAM_ACTIVE", _handl, 0)
        If DeactivateScripedCams Then [Call]("ACTIVATE_SCRIPTED_CAMS", 0, 0)
    End Sub
    Public ReadOnly Property Exists
        Get
            If Not CheckNotDisposed() Then Return False
            If _handl = 0 Then Return False
            Return [Call](Of Boolean)("DOES_CAM_EXIST", _handl)
        End Get
    End Property
    Public Property FarClip As Single
        Get
            If Not CheckNotDisposed() Then Return 0.0
            [Call]("GET_CAM_FAR_CLIP", _handl, pFarClip)
            Return pFarClip.Value
        End Get
        Set(value As Single)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("SET_CAM_FAR_CLIP", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...FarDOF.")
            End If
        End Set
    End Property
    Public Property FarDOF As Single
        Get
            If Not CheckNotDisposed() Then Return 0.0
            [Call]("GET_CAM_FAR_DOF", _handl, pFarDof)
            Return pFarDof.Value
        End Get
        Set(value As Single)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("SET_CAM_FAR_DOF", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...FarDOF.")
            End If
        End Set
    End Property
    Public Property FOV As Single
        Get
            If Not CheckNotDisposed() Then Return 0.0F
            If Exists Then
                [Call]("GET_CAM_FOV", _handl, pFov)
                Return pFov.Value
            End If
            Return 0.0F
        End Get
        Set(value As Single)
            If Not CheckNotDisposed() Then Return
            If Exists Then
                [Call]("SET_CAM_FOV", _handl, value)
            End If
        End Set
    End Property
    Public ReadOnly Property Handle As Integer
        Get
            If Not CheckNotDisposed() Then Return -2
            Return _handl
        End Get
    End Property
    ''' <summary>
    ''' Returns the camera heading (yaw) in degrees, normalized to 0–360.
    ''' </summary>
    Public ReadOnly Property Heading As Single
        Get
            If Not CheckNotDisposed() Then Return 0.0F
            If Not Exists Then Return 0.0F

            Dim h As Single = Me.Rotation.Z Mod 360.0F
            If h < 0.0F Then h += 360.0F
            Return h
        End Get
    End Property
    ''' <summary>
    ''' Raw camera yaw (may be outside 0–360).
    ''' </summary>
    Public ReadOnly Property HeadingRaw As Single
        Get
            If Not CheckNotDisposed() Then Return 0.0F
            If Not Exists Then Return 0.0F
            Return Me.Rotation.Z
        End Get
    End Property
    Public ReadOnly Property IsShaking As Boolean
        Get
            If Not CheckNotDisposed() Then Return False
            Return Exists AndAlso [Call](Of Boolean)("IS_CAM_SHAKING", _handl)
        End Get
    End Property
    ''' <summary>
    ''' Untested. GTAMods Wiki says: "The Function checks if the specified camera has a clear line-of-sight towards the specified sphere.
    ''' Environment(walls, buildings, ground) blocks the line Of sight. Because the Function has Not been fully tested 
    ''' it Is Not known If objects, peds And vehicles block the line of sight."
    ''' </summary>
    Public Function IsSphereVisible(Position As Vector3, Radius As Single) As Boolean
        If Not CheckNotDisposed() Then Return False
        If Not Exists Then Return False
        Return [Call](Of Boolean)("CAM_IS_SPHERE_VISIBLE", _handl, Position.X, Position.Y, Position.Z, Radius)
    End Function
    Public Property NearClip As Single
        Get
            If Not CheckNotDisposed() Then Return 0.0
            [Call]("GET_CAM_NEAR_CLIP", _handl, pNearClip)
            Return pNearClip.Value
        End Get
        Set(value As Single)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("SET_CAM_NEAR_CLIP", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...NearClip.")
            End If
        End Set
    End Property
    Public Property NearDOF As Single
        Get
            If Not CheckNotDisposed() Then Return 0.0
            [Call]("GET_CAM_NEAR_DOF", _handl, pNearDof)
            Return pNearDof.Value
        End Get
        Set(value As Single)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("SET_CAM_NEAR_DOF", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...NearDOF.")
            End If
        End Set
    End Property
    Public Sub PointAt(Ped As Ped, Optional Offset As Vector3 = Nothing, Optional IsOffsetRelative As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then
            [Call]("POINT_CAM_AT_PED", _handl, Ped)
            If Offset <> Nothing Then
                [Call]("SET_CAM_POINT_OFFSET", _handl, Offset.X, Offset.Y, Offset.Z)
                [Call]("SET_CAM_POINT_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
            End If
        Else
            Tools.WriteLog("Error in NativeCamera...PointAt (Ped).")
        End If
    End Sub
    Public Sub PointAt(Vehicle As Vehicle, Optional Offset As Vector3 = Nothing, Optional IsOffsetRelative As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then
            [Call]("POINT_CAM_AT_VEHICLE", _handl, Vehicle)
            If Offset <> Nothing Then
                [Call]("SET_CAM_POINT_OFFSET", _handl, Offset.X, Offset.Y, Offset.Z)
                [Call]("SET_CAM_POINT_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
            End If
        Else
            Tools.WriteLog("Error in NativeCamera...PointAt (Car).")
        End If
    End Sub
    Public Sub PointAt(Coords As Vector3, Optional Offset As Vector3 = Nothing, Optional IsOffsetRelative As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then
            [Call]("POINT_CAM_AT_COORD", _handl, Coords.X, Coords.Y, Coords.Z)
            If Offset <> Nothing Then
                [Call]("SET_CAM_POINT_OFFSET", _handl, Offset.X, Offset.Y, Offset.Z)
                [Call]("SET_CAM_POINT_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
            End If
        Else
            Tools.WriteLog("Error in NativeCamera...PointAt (Coords).")
        End If
    End Sub
    Public Sub PointAt(Obj As [Object], Optional Offset As Vector3 = Nothing, Optional IsOffsetRelative As Boolean = True)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then
            [Call]("POINT_CAM_AT_OBJECT", _handl, Obj)
            If Offset <> Nothing Then
                [Call]("SET_CAM_POINT_OFFSET", _handl, Offset.X, Offset.Y, Offset.Z)
                [Call]("SET_CAM_POINT_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
            End If
        Else
            Tools.WriteLog("Error in NativeCamera...PointAt (Obj).")
        End If
    End Sub
    Public Property Position As Vector3
        Get
            If Not CheckNotDisposed() Then Return Nothing
            If Me.Exists Then
                [Call]("GET_CAM_POS", _handl, pPos1, pPos2, pPos3)
                Return New Vector3(pPos1.Value, pPos2.Value, pPos3.Value)
            Else
                Tools.WriteLog("Error in NativeCamera...Position (Get).")
            End If
        End Get
        Set(value As Vector3)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("SET_CAM_POS", _handl, value.X, value.Y, value.Z)
            Else
                Tools.WriteLog("Error in NativeCamera...Position (Set).")
            End If
        End Set
    End Property
    Public Property Rotation As Vector3
        Get
            If Not CheckNotDisposed() Then Return Nothing
            If Me.Exists Then
                [Call]("GET_CAM_ROT", _handl, pRot1, pRot2, pRot3)
                Return New Vector3(pRot1.Value, pRot2.Value, pRot3.Value)
            Else
                Tools.WriteLog("Error in NativeCamera...Rotation (Get).")
            End If
        End Get
        Set(value As Vector3)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("SET_CAM_ROT", _handl, value.X, value.Y, value.Z)
            Else
                Tools.WriteLog("Error in NativeCamera...Rotation (Set).")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Returns the current logical state of the camera.
    ''' </summary>
    Public ReadOnly Property State As CameraState
        Get
            If _disposed Then Return CameraState.Disposed
            If _handl = 0 OrElse Not Exists Then Return CameraState.Invalid

            Dim active = [Call](Of Boolean)("IS_CAM_ACTIVE", _handl)
            Dim propagating = [Call](Of Boolean)("IS_CAM_PROPAGATING", _handl)

            If active AndAlso propagating Then Return CameraState.Live
            If active Then Return CameraState.Enabled
            Return CameraState.Disabled
        End Get
    End Property
    Public Property StateNative As Integer
        Get
            If Not CheckNotDisposed() Then Return -2
            If Me.Exists Then
                Return [Call](Of Integer)("GET_CAM_STATE", _handl)
            Else
                Tools.WriteLog("Error in NativeCamera...State (Get).")
                Return 404
            End If
        End Get
        Set(value As Integer)
            If Not CheckNotDisposed() Then Return
            If Me.Exists Then
                [Call]("SET_CAM_STATE", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...State (Set).")
            End If
        End Set
    End Property
    Public Sub Shake(Shake As Integer, Duration As Integer)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then [Call]("SET_CAM_SHAKE", _handl, Shake, Duration)
    End Sub
    ''' <param name="Float">Values between 0.0 and 0.9</param>
    Public Sub ShakeAdvanced(Type As ShakeType, Behaviour As ShakeBehaviour, Duration As Integer, ShakeAmplitude As Single, ShakeFrequency As Single, Optional Float As Single = 0.0)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then [Call]("SET_CAM_COMPONENT_SHAKE", _handl, Type, Behaviour, ShakeAmplitude, ShakeFrequency, Float)
    End Sub
    Public Sub ShakeDrunk(Intensity As Single, Duration As Integer)
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then [Call]("SET_DRUNK_CAM", _handl, Intensity, Duration)
    End Sub
    Public ReadOnly Property Type As CameraType
        Get
            Return _Type
        End Get
    End Property
    'DETACH_CAM_FROM_VIEWPORT
    'SET_CAM_PROPAGATE 'camera, integer on/off
    '(SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS, integer on/off)
    'public public shared Sub AddCamSplineNode()
    Public Sub Unpoint()
        If Not CheckNotDisposed() Then Return
        If Me.Exists Then [Call]("UNPOINT_CAM", _handl)
    End Sub


    Public Enum CameraState
        ''' <summary>Camera object has been disposed.</summary>
        Disposed = -2
        ''' <summary>Camera does not exist or handle invalid.</summary>
        Invalid = -1
        ''' <summary>Camera exists but is not active.</summary>
        Disabled = 0
        ''' <summary>Camera is active but not propagating.</summary>
        Enabled = 1
        ''' <summary>Camera is active and propagating (live).</summary>
        Live = 2
    End Enum
    ''' <summary>
    ''' Known camera types used in SCO scripts. Only a few types are fully stable outside missions.
    ''' Scripted (6) and Fixed (14) are generally recommended for modding.
    ''' </summary>
    Public Enum CameraType
        ''' <summary>
        ''' Basic scripted camera. Fully controllable.
        ''' Most stable and recommended for custom camera work.
        ''' </summary>
        ScriptedCam = 6
        ''' <summary>
        ''' Camera that attempts to follow a pedestrian. Used in missions.
        ''' Works outside missions but may reposition automatically.
        ''' </summary>
        FollowPedCam = 13
        ''' <summary>
        ''' Fixed camera locked in space. Ideal for cinematic shots or static angles.
        ''' Very stable and reliable.
        ''' </summary>
        FixedCam = 14
        ''' <summary>
        ''' Camera mode similar to phone/camera view. Limited control.
        ''' Works in most environments but not suitable for full custom usage.
        ''' </summary>
        PhoneCam = 25
        ''' <summary>
        ''' Legacy mission camera used in early scripted sequences.
        ''' Can conflict with gameplay camera manager.
        ''' </summary>
        LegacyCam = 1
        ''' <summary>
        ''' Sniper-style camera. Game may override FOV or handling automatically.
        ''' </summary>
        SniperCam = 2
        ''' <summary>
        ''' Dampened cinematic camera. Creates soft motion and slow transitions.
        ''' May reset when the player moves.
        ''' </summary>
        CinematicDamped = 8
        ''' <summary>
        ''' Vehicle script camera used in certain cutscenes.
        ''' Behavior depends on player state and vehicle movement.
        ''' </summary>
        VehicleScriptCam = 18
        ''' <summary>
        ''' Binocular-style camera.
        ''' </summary>
        BinocularCam = 16
    End Enum
    Public Enum ShakeBehaviour
        ConstantFadeInOut = 1
        ConstantFadeIn = 2
        ExponentialFadeInOut = 3
        VerySlowExponentialFadeIn = 4
        FastExponentialFadeInOut = 5
        MediumFastExponentialFadeInOut = 6
        SlowExponentialFadeIn = 7
        MediumSlowExponentialFadeIn = 8
    End Enum
    Public Enum ShakeType
        PitchUpDown = 0
        RollLeftRight = 1
        YawLefRight = 2
        ForwardBack = 3
        LeftRight = 4
        UpDown = 5
        LeftRight2 = 6
        ForwardBack2 = 7
        UpDown2 = 8
        PulseInOut = 9
    End Enum
    Public Enum VehicleCameras
        Unknown = -1
        Dashboard = 0
        FollowNear = 1
        FollowMid = 2
        FollowFar = 3
        HoodView = 4
        Cinematic = 5
    End Enum

    '===================================================================================================
    Private ReadOnly _ownsCamera As Boolean
    Private _disposed As Boolean
    Private ReadOnly _type As CameraType
    Private Shared ReadOnly _scriptCamCache As New Dictionary(Of Integer, NativeCamera)
    Private Sub New(handle As Integer, camType As CameraType)
        _handl = handle
        _disposed = False
        _coll = True
        _Type = camType

        pPos1 = New Native.Pointer(GetType(Single))
        pPos2 = New Native.Pointer(GetType(Single))
        pPos3 = New Native.Pointer(GetType(Single))
        pRot1 = New Native.Pointer(GetType(Single))
        pRot2 = New Native.Pointer(GetType(Single))
        pRot3 = New Native.Pointer(GetType(Single))
        pFov = New Native.Pointer(GetType(Single))
        _ownsCamera = False
    End Sub

    Public Shared Widening Operator CType(cam As NativeCamera) As Integer
        If cam Is Nothing OrElse Not cam.Exists Then Return 0
        Return cam._handl
    End Operator

    Public Shared Widening Operator CType(cam As NativeCamera) As GTA.Native.Parameter
        If cam Is Nothing OrElse Not cam.Exists Then Return New GTA.Native.Parameter(0)
        Return New GTA.Native.Parameter(cam._handl)
    End Operator

    Private Function CheckNotDisposed() As Boolean
        If _disposed Then
            If _ownsCamera Then
                Tools.WriteLog("NativeCamera: attempt to use a disposed owned camera.")
            End If
            Return False
        End If
        Return True
    End Function

    Private Shared Function WrapScriptCam(handle As Integer) As NativeCamera
        If handle = 0 Then Return Nothing
        If _scriptCamCache.ContainsKey(handle) Then
            Return _scriptCamCache(handle)
        End If

        Dim cam As New NativeCamera(handle, CameraType.ScriptedCam)
        _scriptCamCache(handle) = cam
        Return cam
    End Function

    Protected Sub Dispose(disposing As Boolean)
        If _disposed Then Return

        If _ownsCamera Then
            Delete()
        End If

        _disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
End Class


Partial Public NotInheritable Class NativeCamera
    Private Shared _cinematicCam As NativeCamera
    Private Shared _defaultCam As NativeCamera

    Public Shared ReadOnly Property CinematicCamera As NativeCamera
        Get
            Dim ptr As New Native.Pointer(GetType(Integer))
            [Call]("GET_CINEMATIC_CAM", ptr)
            Dim handle As Integer = ptr.Value
            If handle = 0 Then Return Nothing

            If _cinematicCam IsNot Nothing Then
                If _cinematicCam.Handle = handle AndAlso _cinematicCam.Exists Then
                    Return _cinematicCam
                End If
            End If

            _cinematicCam = New NativeCamera(handle, CameraType.CinematicDamped)
            Return _cinematicCam
        End Get
    End Property

    Public Shared Function CountScriptCams(Optional Type As CameraType? = Nothing, Optional Active As Boolean? = Nothing, Optional Propagating As Boolean? = Nothing) As Integer
        If Not Type.HasValue AndAlso Not Active.HasValue AndAlso Not Propagating.HasValue Then
            Return [Call](Of Integer)("COUNT_SCRIPT_CAMS")
        End If

        Dim t As Integer = If(Type.HasValue, CInt(Type.Value), -1)
        Dim a As Integer = If(Active.HasValue AndAlso Active.Value, 1, 0)
        Dim p As Integer = If(Propagating.HasValue AndAlso Propagating.Value, 1, 0)
        Return [Call](Of Integer)("COUNT_SCRIPT_CAMS_BY_TYPE_AND_OR_STATE", t, a, p)
    End Function
    ''' <summary>
    ''' Returns the game camera (GET_GAME_CAM native)
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property DefaultCamera As NativeCamera
        Get
            Dim ptr As New Native.Pointer(GetType(Integer))
            [Call]("GET_GAME_CAM", ptr)

            Dim handle As Integer = ptr.Value
            If handle = 0 Then Return Nothing

            If _defaultCam IsNot Nothing Then
                If _defaultCam.Handle = handle AndAlso _defaultCam.Exists Then
                    Return _defaultCam
                End If
            End If

            _defaultCam = New NativeCamera(handle, CameraType.LegacyCam)
            Return _defaultCam
        End Get
    End Property
    Public Shared Sub DisableCinematicCamKey(Optional Disable As Boolean = True)
        [Call]("SET_CINEMATIC_BUTTON_ENABLED", Not Disable)
    End Sub

    ''' <summary>
    ''' If True, new cams created will render PTFXx, lights and proper rendering distance. 
    ''' </summary>
    Public Shared WriteOnly Property EffectsOnCameras As Boolean
        Set(value As Boolean)
            [Call]("SET_PTFX_CAM_INSIDE_VEHICLE", value)
        End Set
    End Property

    ''' <summary>
    ''' Seems not working with some plugins like ZolikaPatch.
    ''' </summary>
    Public Shared Sub EnableCamControls_WhenPlayerControlsDisabled(Enable As Boolean)
        [Call]("SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS", Enable)
    End Sub

    ''' <param name="Anglelimits">If > 0 will be used the function SET_TELESCOPE_CAM_ANGLE_LIMITS</param>
    Public Shared Sub ForceTelescopeCam(ForceON As Boolean, Optional Anglelimits As Integer = -1)
        [Call]("FORCE_GAME_TELESCOPE_CAM", ForceON)
        If Anglelimits > -1 Then [Call]("SET_TELESCOPE_CAM_ANGLE_LIMITS", Anglelimits)
    End Sub

    Public Shared WriteOnly Property ProstituteCam As Boolean
        Set(value As Boolean)
            [Call]("PROSTITUTE_CAM_ACTIVATE", value)
        End Set
    End Property

    Public Shared WriteOnly Property ScriptedCameras As Boolean
        Set(value As Boolean)
            [Call]("ACTIVATE_SCRIPTED_CAMS", value, value)
        End Set
    End Property

    Public Shared Sub SetCamBehindPed(Ped As Ped)
        [Call]("SET_CAM_BEHIND_PED", Ped)
    End Sub

    Public Shared Sub SetCamInFrontOfPed(Ped As Ped)
        [Call]("SET_CAM_IN_FRONT_OF_PED", Ped)
    End Sub

    Public Shared Sub SetVehicleCamMode(Mode As VehicleCameras)
        Dim Cam As Integer = Tools.ClampInteger(Mode, 0, 5)
        [Call]("SET_FOLLOW_VEHICLE_CAM_SUBMODE", Cam)
    End Sub
    ''' <summary>
    ''' Disables mouse control.
    ''' </summary>
    Public Shared Sub SetVehicleCamOffset(Enabled As Boolean, Optional Offset As Vector3 = Nothing)
        If Offset = Nothing Then Offset = Vector3.Zero
        [Call]("SET_FOLLOW_VEHICLE_CAM_OFFSET", Enabled, Offset.X, Offset.Y, Offset.Z)
    End Sub
End Class


'''' <summary>
'''' Experimental: use with caution!
'''' </summary>
'Public Sub AttachTo(Viewport As Viewport, Optional IsOffsetRelative As Boolean = False)
'    If Not CheckNotDisposed() Then Return
'    If Me.Exists Then
'        [Call]("ATTACH_CAM_TO_VIEWPORT", _handl, Viewport)
'        [Call]("SET_CAM_ATTACH_OFFSET", _handl, Position.X, Position.Y, Position.Z)
'        [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
'    Else
'        Tools.WriteLog("Error in NativeCamera...AttachTo (Viewport).")
'    End If
'End Sub
'
'SET_CAM_MOTION_BLUR
'SET_CAM_INHERIT_ROLL_OBJECT
'SET_CAM_INHERIT_ROLL_PED
'SET_CAM_INHERIT_ROLL_VEHICLE
'SET_CAM_INTERP_CUSTOM_SPEED_GRAPH
'SET_CAM_INTERP_DETAIL_ROT_STYLE_ANGLES
'SET_CAM_INTERP_DETAIL_ROT_STYLE_QUATS
' SET_CAM_INHERIT_ROLL_VEHICLE( l_U2479, l_U3101 )
'SET_CAM_INTERP_STYLE_CORE( l_U2477, l_U2474, l_U2476, 6020, 0 )
' SET_CAM_INTERP_STYLE_CORE( l_U2477, l_U2474, l_U2476, 4200, 0 );
'       SET_CAM_INTERP_STYLE_DETAILED( l_U2477, 2, 2, 1, 1 );
'SET_CAM_INTERP_STYLE_DETAILED( l_U2477, 0, 0, 1, 1 )