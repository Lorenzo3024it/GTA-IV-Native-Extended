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
    Public Shared Sub AttachCamToVehicle(Camera As GTA.Camera, Vehicle As Vehicle, Position As Vector3, IsOffsetRelative As Boolean)
        [Call]("ATTACH_CAM_TO_VEHICLE", Camera, Vehicle)
        [Call]("SET_CAM_ATTACH_OFFSET", Camera, Position.X, Position.Y, Position.Z)
        [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelative)
    End Sub
    Public Shared Sub AttachCamToPed(Camera As GTA.Camera, Ped As Ped, Position As Vector3, IsOffsetRelative As Boolean)
        [Call]("ATTACH_CAM_TO_PED", Camera, Ped)
        [Call]("SET_CAM_ATTACH_OFFSET", Camera, Position.X, Position.Y, Position.Z)
        [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelative)
    End Sub
    Public Shared Sub AttachCamToObject(Camera As GTA.Camera, Ped As Ped, Position As Vector3, IsOffsetRelative As Boolean)
        [Call]("ATTACH_CAM_TO_OBJECT", Camera, Ped)
        [Call]("SET_CAM_ATTACH_OFFSET", Camera, Position.X, Position.Y, Position.Z)
        [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelative)
    End Sub
    Public Shared Sub AttachCamToViewport(Camera As GTA.Camera, Viewport As Integer, Optional IsOffsetRelative As Boolean = False)
        [Call]("ATTACH_CAM_TO_OBJECT", Camera, Viewport)
        'Native.Function.Call("SET_CAM_ATTACH_OFFSET", Camera, X, Y, Z)
        'Native.Function.Call("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelativeInt)
    End Sub
    Public Shared ReadOnly Property CinematicCam As GTA.Camera
        Get
            Dim CamPoint As Native.Pointer = New Native.Pointer(GetType(GTA.Camera))
            [Call]("GET_CINEMATIC_CAM", CamPoint)
            Return CamPoint.Value
        End Get
    End Property
    Public Shared ReadOnly Property GameCam As GTA.Camera
        Get
            Dim CamPoint As Native.Pointer = New Native.Pointer(GetType(Camera))
            Return [Call](Of GTA.Camera)("GET_GAME_CAM", CamPoint)
        End Get
    End Property
    Public Shared Sub EnableCamCollisions(Cam As Camera, Collision As Boolean)
        [Call]("ENABLE_CAM_COLLISION", Cam, Collision)
    End Sub
    Public Shared Sub EnableCamControls(Enable As Boolean)
        [Call]("SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS", Enable)
    End Sub
    Public Shared Sub ForceTelescopeCam(ForceON As Boolean)
        [Call]("FORCE_GAME_TELESCOPE_CAM", ForceON)
    End Sub
    'Public Shared Function GetCinematicCam() As GTA.Camera
    '    Dim pointer As Native.Pointer = New Native.Pointer(GetType(GTA.Camera))
    '    [Call]("GET_CINEMATIC_CAM", pointer)
    '    Return pointer.Value
    'End Function
    'Public Shared Function GetGameCam() As GTA.Camera
    '    Dim GameCam As Native.Pointer = New Native.Pointer(GetType(Camera))
    '    Return [Call](Of GTA.Camera)("GET_GAME_CAM", GameCam)
    'End Function
    Public Shared Sub SetCamShake(Camera As GTA.Camera, Shake1_to_8 As Integer, Time As Integer)
        [Call]("CAM_PROCESS", GTA.Game.DefaultCamera)
        [Call]("SET_CAM_SHAKE", Camera, Shake1_to_8, Time)
    End Sub
    Public Shared Sub SetCamShake(Camera As GTA.Camera, Type As CameraShakeType, Behaviour As CameraShakeBehaviour, Duration As Integer, ShakeAmplitude As Single, ShakeFrequency As Single, Optional FloatMax0dot9 As Single = 0)
        [Call]("SET_CAM_COMPONENT_SHAKE", Camera, Type, Behaviour, ShakeAmplitude, ShakeFrequency, FloatMax0dot9)
    End Sub
    Public Shared Sub SetCamBehindPed(Ped As Ped)
        [Call]("SET_CAM_BEHIND_PED", Ped)
    End Sub
    'DETACH_CAM_FROM_VIEWPORT
    'SET_CAM_PROPAGATE 'camera, integer on/off
    '(SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS, integer on/off)
    'public public shared Sub AddCamSplineNode()

    'End Sub
    Public Shared Sub EnableCamCollision(Camera As GTA.Camera, Enable As Boolean)
        [Call]("ENABLE_CAM_COLLISION", Enable)
    End Sub
    Public Shared Sub isCamSphereVisible(Camera As GTA.Camera, SphereCentrePosition As Vector3, Radius As Single)
        'The Function checks if the specified camera has a clear line-of-sight towards the specified sphere.
        'Environment(walls, buildings, ground) blocks the line Of sight. Because the Function has Not been fully tested it Is Not known If objects, peds And vehicles block the line Of sight.
        [Call]("CAM_IS_SPHERE_VISIBLE", Camera, SphereCentrePosition.X, SphereCentrePosition.Y, SphereCentrePosition.Z, Radius)
    End Sub
    Public Shared Function getCamState(Camera As GTA.Camera) As Integer
        Return [Call](Of Integer)("GET_CAM_STATE", Camera)
    End Function
    Public Shared Sub DetachCam(Camera As GTA.Camera)
        [Call]("UNATTACH_CAM", Camera)
    End Sub
    Public Shared Sub UnpointCam(Camera As GTA.Camera)
        [Call]("UNPOINT_CAM", Camera)
    End Sub
    Public Shared Sub DisableCinematicCamKey(Optional Disable As Boolean = True)
        [Call]("SET_CINEMATIC_BUTTON_ENABLED", Not Disable)
    End Sub

    '[Call]("SET_TELESCOPE_CAM_ANGLE_LIMITS", 360)
    'SET_FOLLOW_VEHICLE_CAM_OFFSET (VehicleCamera as VehicleCameras, x, y, z) 'no mouse control
    'SET_FOLLOW_VEHICLE_CAM_SUBMODE(VehicleCamera as VehicleCameras)
    'SET_CAM_MOTION_BLUR
    '    SET_CAM_IN_FRONT_OF_PED
    'SET_CAM_INHERIT_ROLL_OBJECT
    'SET_CAM_INHERIT_ROLL_PED
    'SET_CAM_INHERIT_ROLL_VEHICLE
    'SET_CAM_INTERP_CUSTOM_SPEED_GRAPH
    'SET_CAM_INTERP_DETAIL_ROT_STYLE_ANGLES
    'SET_CAM_INTERP_DETAIL_ROT_STYLE_QUATS
    'SET_CAM_INTERP_STYLE_CORE
    'SET_CAM_INTERP_STYLE_DETAILED

    'CAM_SEQUENCE_CLOSE
    'CAM_SEQUENCE_GET_PROGRESS
    'CAM_SEQUENCE_OPEN
    'CAM_SEQUENCE_REMOVE
    'CAM_SEQUENCE_START
    'CAM_SEQUENCE_STOP
    'CAM_SEQUENCE_WAIT
    'SET_SEQUENCE_TO_REPEAT
    'IS_CAM_SHAKING
    'GET_CINEMATIC_CAM
End Class