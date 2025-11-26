Imports System.Net.Security
Imports GTA
Imports GTA.Native.Function

Partial Public Class NativeCamera
    Private Shared _handl As Integer
    'Private Shared _rot As Vector3
    'Private Shared _pos As Vector3
    Private Shared _rollPriv As Single
    Private Shared p1, p2, p3, p4, p5, p6, p7, p8 As Native.Pointer
    Public WriteOnly Property Collision As Boolean
        Set(value As Boolean)
            If Me.Exists Then
                [Call]("ENABLE_CAM_COLLISION", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...Collision.")
            End If
        End Set
    End Property
    Public Property Enabled As Boolean
        Get
            Return [Call](Of Boolean)("IS_CAM_ACTIVE", _handl) AndAlso [Call](Of Boolean)("IS_CAM_PROPAGATING", _handl)
        End Get
        Set(value As Boolean)
            If Me.Exists Then
                [Call]("SET_CAM_ACTIVE", _handl, 1)
                [Call]("SET_CAM_PROPAGATE", _handl, 1)
            Else
                Tools.WriteLog("Error in NativeCamera...Enabled (Set).")
            End If
        End Set
    End Property
    Public ReadOnly Property Handle As Integer
        Get
            Return _handl
        End Get
    End Property
    Public WriteOnly Property NearClip As Single
        Set(value As Single)
            If Me.Exists Then
                [Call]("SET_CAM_NEAR_CLIP", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...NearClip.")
            End If
        End Set
    End Property
    Public WriteOnly Property FarDOF As Single
        Set(value As Single)
            If Me.Exists Then
                [Call]("SET_CAM_FAR_DOF", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...FarDOF.")
            End If
        End Set
    End Property
    Public Property FOV As Single
        Get
            If Me.Exists Then
                [Call]("GET_CAM_FOV", _handl, p7)
            Else
                Tools.WriteLog("Error in NativeCamera...FOV (Get).")
            End If
            Return p7.Value
        End Get
        Set(value As Single)
            If Me.Exists Then
                [Call]("SET_CAM_FOV", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...FOV (Set).")
            End If
        End Set
    End Property
    Public WriteOnly Property NearDOF As Single
        Set(value As Single)
            If Me.Exists Then
                [Call]("SET_CAM_NEAR_DOF", _handl, value)
            Else
                Tools.WriteLog("Error in NativeCamera...NearDOF.")
            End If
        End Set
    End Property
    Public Property Position As Vector3
        Get
            If Me.Exists Then
                [Call]("GET_CAM_POS", _handl, p1, p2, p3)
                Return New Vector3(p1.Value, p2.Value, p3.Value)
            Else
                Tools.WriteLog("Error in NativeCamera...Position (Get).")
            End If
        End Get
        Set(value As Vector3)
            If Me.Exists Then
                [Call]("SET_CAM_POS", _handl, value.X, value.Y, value.Z)
            Else
                Tools.WriteLog("Error in NativeCamera...Position (Set).")
            End If
        End Set
    End Property
    Public Property Rotation As Vector3
        Get
            If Me.Exists Then
                [Call]("GET_CAM_ROT", _handl, p4, p5, p6)
                Return New Vector3(p4.Value, p5.Value, p6.Value)
            Else
                Tools.WriteLog("Error in NativeCamera...Rotation (Get).")
            End If
        End Get
        Set(value As Vector3)
            If Me.Exists Then
                [Call]("SET_CAM_ROT", _handl, value.X, value.Y, value.Z)
            Else
                Tools.WriteLog("Error in NativeCamera...Rotation (Set).")
            End If
        End Set
    End Property
    '  Public Property Roll As Single
    Public Property State As Integer
        Get
            If Me.Exists Then
                Return [Call](Of Integer)("GET_CAM_STATE", _handl)
            Else
                Return 404
                Tools.WriteLog("Error in NativeCamera...State (Get).")
            End If
        End Get
        Set(value As Integer)
            If Me.Exists Then
                [Call](Of Integer)("SET_CAM_STATE", _handl, State)
            Else
                Tools.WriteLog("Error in NativeCamera...State (Set).")
            End If
        End Set
    End Property
    Public Sub New(Optional Type As Type = Type.ScriptedCam, Optional Position As Vector3 = Nothing)
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(NativeCamera))
        p1 = New Native.Pointer(GetType(Single))
        p2 = New Native.Pointer(GetType(Single))
        p3 = New Native.Pointer(GetType(Single))
        p4 = New Native.Pointer(GetType(Single))
        p5 = New Native.Pointer(GetType(Single))
        p6 = New Native.Pointer(GetType(Single))
        p7 = New Native.Pointer(GetType(Single))
        [Call]("CREATE_CAM", Type, pointer)

        _handl = pointer.Value

    End Sub
    Public Sub AttachTo(Car As Vehicle, Position As Vector3, Optional IsOffsetRelative As Boolean = True)
        If Me.Exists Then
            [Call]("ATTACH_CAM_TO_VEHICLE", _handl, Car)
            [Call]("SET_CAM_ATTACH_OFFSET", _handl, Position.X, Position.Y, Position.Z)
            [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
        Else
            Tools.WriteLog("Error in NativeCamera...AttachTo (Car).")
        End If
    End Sub
    Public Sub AttachTo(Ped As Ped, Position As Vector3, Optional IsOffsetRelative As Boolean = True)
        If Me.Exists Then
            [Call]("ATTACH_CAM_TO_PED", _handl, Ped)
            [Call]("SET_CAM_ATTACH_OFFSET", _handl, Position.X, Position.Y, Position.Z)
            [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
        Else
            Tools.WriteLog("Error in NativeCamera...AttachTo (Ped).")
        End If
    End Sub
    Public Sub AttachTo(Obj As [Object], Position As Vector3, Optional IsOffsetRelative As Boolean = True)
        If Me.Exists Then
            [Call]("ATTACH_CAM_TO_OBJECT", _handl, Obj)
            [Call]("SET_CAM_ATTACH_OFFSET", _handl, Position.X, Position.Y, Position.Z)
            [Call]("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
        Else
            Tools.WriteLog("Error in NativeCamera...AttachTo (Obj).")
        End If
    End Sub
    'Public Shared Sub AttachCamToViewport(Camera As GTA.Camera, Viewport As Integer, Optional IsOffsetRelative As Boolean = False)
    '    [Call]("ATTACH_CAM_TO_OBJECT", Camera, Viewport)
    '    'Native.Function.Call("SET_CAM_ATTACH_OFFSET", Camera, X, Y, Z)
    '    'Native.Function.Call("SET_CAM_ATTACH_OFFSET_IS_RELATIVE", Camera, IsOffsetRelativeInt)
    'End Sub
    Public Sub Delete()
        If Me.Exists Then [Call]("DESTROY_CAM", _handl)
        p1 = Nothing
        p2 = Nothing
        p3 = Nothing
        p4 = Nothing
        p5 = Nothing
        p6 = Nothing
        p7 = Nothing
        p8 = Nothing
        _handl = Nothing
    End Sub
    Public Function Exists() As Boolean
        Return [Call](Of Boolean)("DOES_CAM_EXIST", _handl)
    End Function
    Public Sub PointAt(Ped As Ped, Optional Offset As Vector3 = Nothing, Optional IsOffsetRelative As Boolean = True)
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
        If Me.Exists Then
            [Call]("POINT_CAM_AT_COORD", _handl, Position.X, Position.Y, Position.Z)
            If Offset <> Nothing Then
                [Call]("SET_CAM_POINT_OFFSET", _handl, Offset.X, Offset.Y, Offset.Z)
                [Call]("SET_CAM_POINT_OFFSET_IS_RELATIVE", _handl, IsOffsetRelative)
            End If
        Else
            Tools.WriteLog("Error in NativeCamera...PointAt (Coords).")
        End If
    End Sub
    Public Sub PointAt(Obj As [Object], Optional Offset As Vector3 = Nothing, Optional IsOffsetRelative As Boolean = True)
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
    Public Sub Shake(Shake As Integer, Duration As Integer)
        If Me.Exists Then [Call]("SET_CAM_SHAKE", _handl, Shake, Duration)
    End Sub
    ''' <param name="Float">Values between 0.0 and 0.9</param>
    Public Sub ShakeAdvanced(Type As ShakeType, Behaviour As ShakeBehaviour, Duration As Integer, ShakeAmplitude As Single, ShakeFrequency As Single, Optional Float As Single = 0.0)
        If Me.Exists Then [Call]("SET_CAM_COMPONENT_SHAKE", _handl, Type, Behaviour, ShakeAmplitude, ShakeFrequency, Float)
    End Sub

    'DETACH_CAM_FROM_VIEWPORT
    'SET_CAM_PROPAGATE 'camera, integer on/off
    '(SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS, integer on/off)
    'public public shared Sub AddCamSplineNode()
    ''' <summary>
    ''' Untested. GTAMods Wiki says: "The Function checks if the specified camera has a clear line-of-sight towards the specified sphere.
    ''' Environment(walls, buildings, ground) blocks the line Of sight. Because the Function has Not been fully tested 
    ''' it Is Not known If objects, peds And vehicles block the line of sight."
    ''' </summary>
    Public Sub isSphereVisible(Position As Vector3, Radius As Single)
        If Me.Exists Then [Call]("CAM_IS_SPHERE_VISIBLE", _handl, Position.X, Position.Y, Position.Z, Radius)
    End Sub
    Public Sub Detach()
        If Me.Exists Then [Call]("UNATTACH_CAM", _handl)
    End Sub
    Public Sub Unpoint()
        If Me.Exists Then [Call]("UNPOINT_CAM", _handl)
    End Sub

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
    ''' <summary>
    ''' Known camera types used in SCO scripts. Only a few types are fully stable outside missions.
    ''' Scripted (6) and Fixed (14) are generally recommended for modding.
    ''' </summary>
    Public Enum Type As Integer
        ''' <summary>
        ''' Basic scripted camera. Fully controllable..
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
        ''' Binocular-style camera. Strongly tied to internal game actions.
        ''' Limited usability outside missions.
        ''' </summary>
        BinocularCam = 16
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
End Class
Partial Public Class NativeCamera
    Public Shared ReadOnly Property CinematicCam As GTA.Camera
        Get
            Dim CamPoint As Native.Pointer = New Native.Pointer(GetType(GTA.Camera))
            [Call]("GET_CINEMATIC_CAM", CamPoint)
            Return CamPoint.Value
        End Get
    End Property
    Public Shared Sub DisableCinematicCamKey(Optional Disable As Boolean = True)
        [Call]("SET_CINEMATIC_BUTTON_ENABLED", Not Disable)
    End Sub
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
    Public Shared ReadOnly Property GameCam As GTA.Camera
        Get
            Dim CamPoint As New Native.Pointer(GetType(Camera))
            [Call]("GET_GAME_CAM", CamPoint)
            Return CamPoint.Value
        End Get
    End Property
    Public Shared WriteOnly Property ProstituteCam As Boolean
        Set(value As Boolean)
            [Call]("PROSTITUTE_CAM_ACTIVATE", value)
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