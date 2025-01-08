Imports GTA
Imports GTA.Native.Function

Public Class NativeCamera
    Public Enum CameraShakeType
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
    Public Enum CameraShakeBehaviour
        ConstantFadeInOut = 1
        ConstantFadeIn = 2
        ExponentialFadeInOut = 3
        VerySlowExponentialFadeIn = 4
        FastExponentialFadeInOut = 5
        MediumFastExponentialFadeInOut = 6
        SlowExponentialFadeIn = 7
        MediumSlowExponentialFadeIn = 8
    End Enum
    Public Enum VehicleCameras
        Dashboard = 0
        FollowNear = 1
        FollowMid = 2
        FollowFar = 3
        HoodView = 4
        Cinematic = 5
    End Enum
    '===========================================================================================================================================================
    'Camera creation & control
    '----------------------------
    Shared Sub AttachCamToVehicle(Camera As GTA.Camera, Vehicle As Vehicle, X As Single, Y As Single, Z As Single, IsOffsetRelative As Boolean)
        [Call]("ATTACH_CAM_TO_VEHICLE", Camera, Vehicle)
        [Call]("SET_CAM_ATTACH_OFFSET", Camera, X, Y, Z)
        [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelative)
    End Sub
    Shared Sub AttachCamToPed(Camera As GTA.Camera, Ped As Ped, X As Single, Y As Single, Z As Single, IsOffsetRelative As Boolean)
        [Call]("ATTACH_CAM_TO_PED", Camera, Ped)
        [Call]("SET_CAM_ATTACH_OFFSET", Camera, X, Y, Z)
        [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelative)
    End Sub
    Shared Sub AttachCamToObject(Camera As GTA.Camera, Ped As Ped, X As Single, Y As Single, Z As Single, IsOffsetRelative As Boolean)
        [Call]("ATTACH_CAM_TO_OBJECT", Camera, Ped)
        [Call]("SET_CAM_ATTACH_OFFSET", Camera, X, Y, Z)
        [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelative)
    End Sub
    Shared Sub AttachCamToViewport(Camera As GTA.Camera, Viewport As Integer, Optional IsOffsetRelative As Boolean = False)
        [Call]("ATTACH_CAM_TO_OBJECT", Camera, Viewport)
        'Native.Function.Call("SET_CAM_ATTACH_OFFSET", Camera, X, Y, Z)
        'Native.Function.Call("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelativeInt)
    End Sub
    Shared Sub EnableCamCollisions(Cam As Camera, Collision As Boolean)
        [Call]("ENABLE_CAM_COLLISION", Cam, Collision)
    End Sub
    Shared Sub EnableCamControls(Enable As Boolean)
        [Call]("SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS", Enable)
    End Sub
    Shared Sub ForceTelescopeCam(ForceON As Boolean)
        [Call]("FORCE_GAME_TELESCOPE_CAM", ForceON)
    End Sub
    Shared Function getGameCam() As GTA.Camera
        Dim GameCam As Native.Pointer = New Native.Pointer(GetType(Camera))
        Return [Call](Of GTA.Camera)("GET_GAME_CAM", GameCam)
    End Function
    Shared Sub SetCamShake(Camera As GTA.Camera, Shake1_to_8 As Integer, Time As Integer)
        [Call]("CAM_PROCESS", GTA.Game.DefaultCamera)
        [Call]("SET_CAM_SHAKE", Camera, Shake1_to_8, Time)
    End Sub
    Shared Sub SetCamShake(Camera As GTA.Camera, Type As CameraShakeType, Behaviour As CameraShakeBehaviour, Duration As Integer, ShakeAmplitude As Single, ShakeFrequency As Single, Optional FloatMax0dot9 As Single = 0)
        Native.Function.Call("SET_CAM_COMPONENT_SHAKE", Camera, Type, Behaviour, ShakeAmplitude, ShakeFrequency, FloatMax0dot9)
    End Sub
    Shared Sub SetCamBehindPed(Ped As Ped)
        Native.Function.Call("SET_CAM_BEHIND_PED", Ped)
    End Sub
    'DETACH_CAM_FROM_VIEWPORT
    'SET_CAM_PROPAGATE 'camera, integer on/off
    '(SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS, integer on/off)
    'Shared Sub AddCamSplineNode()

    'End Sub
    Shared Sub EnableCamCollision(Camera As GTA.Camera, Enable As Boolean)
        [Call]("ENABLE_CAM_COLLISION", Enable)
    End Sub
    Shared Sub isCamSphereVisible(Camera As GTA.Camera, SphereCentrePosition As Vector3, Radius As Single)
        'The Function checks if the specified camera has a clear line-of-sight towards the specified sphere.
        'Environment(walls, buildings, ground) blocks the line Of sight. Because the Function has Not been fully tested it Is Not known If objects, peds And vehicles block the line Of sight.
        [Call]("CAM_IS_SPHERE_VISIBLE", Camera, SphereCentrePosition.X, SphereCentrePosition.Y, SphereCentrePosition.Z, Radius)
    End Sub
    Shared Function getCamState(Camera As GTA.Camera) As Integer
        Return [Call](Of Integer)("GET_CAM_STATE", Camera)
    End Function
    Shared Sub DetachCam(Camera As GTA.Camera)
        [Call]("UNATTACH_CAM", Camera)
    End Sub
    Shared Sub UnpointCam(Camera As GTA.Camera)
        [Call]("UNPOINT_CAM", Camera)
    End Sub
    Shared Function DebugCamera(Optional Enable As Boolean = False) As Boolean
        If Enable Then
            [Call]("ENABLE_DEBUG_CAM", True)
        Else
            If [Call](Of Boolean)("IS_DEBUG_CAMERA_ON") = True Then [Call]("DISABLE_DEBUG_CAM_AND_PLAYER_WARPING", True)
        End If
        Return [Call](Of Boolean)("IS_DEBUG_CAMERA_ON")
    End Function '-Mouse to rotate; -Scrollwheel to zoom; -Double Left click to teleport player
End Class