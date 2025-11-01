Imports System.ComponentModel.Design
Imports System.Reflection
Imports System.Security.Cryptography.X509Certificates
Imports GTA
Imports GTA.base
Imports GTA.Native
Imports GTA.Native.Function
Imports System.Windows.Forms
#If USE_SLIMDX Then
Imports SlimDX.XInput
#End If
'Imports Microsoft.Xna.Framework.Input
'Public Class NativeKeys
'Public Class test
'    Sub New()

'    End Sub
'End Class
Public Class NativeControls
    Public Shared ReadOnly Property isUsingStandardControls As Boolean
        Get
            Return [Call](Of Boolean)("USING_STANDARD_CONTROLS")
        End Get
    End Property
    Public Shared ReadOnly Property MousePosition As GTA.Vector2
        Get
            Dim PosX, PosY As Native.Pointer
            PosX = New Native.Pointer(GetType(Single))
            PosY = New Native.Pointer(GetType(Single))
            [Call]("GET_MOUSE_POSITION", PosX, PosY)
            Return New GTA.Vector2(PosX.Value, PosY.Value)
        End Get
    End Property
    Public Shared ReadOnly Property MouseSensitivity As Single
        Get
            Return [Call](Of Single)("GET_MOUSE_SENSITIVITY")
        End Get
    End Property
    'Public Shared Function isUsingStandardControls() As Boolean
    '    Return [Call](Of Boolean)("USING_STANDARD_CONTROLS")
    'End Function
    Public Shared Function isGameKeyPressed(GameKey As NativeKeys, Optional JustPressed As Boolean = True) As Boolean
        If JustPressed Then
            Return [Call](Of Boolean)("IS_CONTROL_JUST_PRESSED", 2, GameKey)
        Else
            Return [Call](Of Boolean)("IS_CONTROL_PRESSED", 2, GameKey)
        End If
    End Function
    Public Shared Function isKeyPressed(Key As KeyValues, Optional JustPressed As Boolean = True) ', Optional GameKeyboard As Boolean = True) As Boolean
        Dim GameKeyboard As Boolean = True
        If GameKeyboard Then
            If JustPressed Then
                Return [Call](Of Boolean)("IS_GAME_KEYBOARD_KEY_JUST_PRESSED", Key)
            Else
                Return [Call](Of Boolean)("IS_GAME_KEYBOARD_KEY_PRESSED", Key)
            End If
        Else
            If JustPressed Then
                Return [Call](Of Boolean)("IS_KEYBOARD_KEY_JUST_PRESSED", Key)
            Else
                Return [Call](Of Boolean)("IS_KEYBOARD_KEY_PRESSED", Key)
            End If
        End If
    End Function
    Public Shared Function isNavKeyPressed(Key As NavKeys) As Boolean
        Select Case Key
            Case NavKeys.Up
                Return [Call](Of Boolean)("IS_GAME_KEYBOARD_NAV_UP_PRESSED", 0)
            Case NavKeys.Down
                Return [Call](Of Boolean)("IS_GAME_KEYBOARD_NAV_DOWN_PRESSED", 0)
            Case NavKeys.Left
                Return [Call](Of Boolean)("IS_GAME_KEYBOARD_NAV_LEFT_PRESSED", 0)
            Case NavKeys.Right
                Return [Call](Of Boolean)("IS_GAME_KEYBOARD_NAV_RIGHT_PRESSED", 0)
            Case Else
                Return False
        End Select
    End Function
    'Private Shared Function getMousePosition() As gta.Vector2
    '    Dim PosX, PosY As Native.Pointer
    '    PosX = New Native.Pointer(GetType(Single))
    '    PosY = New Native.Pointer(GetType(Single))
    '    [Call]("GET_MOUSE_POSITION", PosX, PosY)
    '    Return New gta.Vector2(PosX.Value, PosY.Value)
    'End Function
    Public Shared Function isMouseButtonPressed(Button As MouseInputs, Optional JustPressed As Boolean = True) As Boolean
        If Button <> MouseInputs.WheelUp AndAlso Button <> MouseInputs.WheelDown Then
            If JustPressed Then
                Return [Call](Of Boolean)("IS_MOUSE_BUTTON_JUST_PRESSED", Button)
            Else
                Return [Call](Of Boolean)("IS_MOUSE_BUTTON_PRESSED", Button)
            End If
        Else
            Dim Input As Native.Pointer = New Native.Pointer(GetType(MouseInputs))
            [Call]("GET_MOUSE_WHEEL", Input)
            If Input.Value = Button Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    'Public Shared Function getMouseSensitivity() As Single
    '    Return [Call](Of Single)("GET_MOUSE_SENSITIVITY")
    'End Function
    Public Shared Function GetMouseInput() As GTA.Vector2
        Dim pointer1 As Native.Pointer = New Native.Pointer(GetType(Single))
        Dim pointer2 As Native.Pointer = New Native.Pointer(GetType(Single))
        [Call]("GET_MOUSE_INPUT", pointer1, pointer2)
        Return New GTA.Vector2(pointer1.Value, pointer2.Value)
    End Function
    Private Enum KeyType
        Menu = 0
        OnFoot = 2
    End Enum
    Public Enum KeyValues
        Escape = 1
        D1 = 2
        D2 = 3
        D3 = 4
        D4 = 5
        D5 = 6
        D6 = 7
        D7 = 8
        D8 = 9
        D9 = 10
        D0 = 11
        Minus = 12
        Equal = 13
        Backspace = 14
        Tab = 15
        Q = 16
        W = 17
        E = 18
        R = 19
        T = 20
        Y = 21
        U = 22
        I = 23
        O = 24
        P = 25
        LeftSquareBracket = 26
        RightSquareBracket = 27
        Enter = 28
        LeftCtrl = 29
        A = 30
        S = 31
        D = 32
        F = 33
        G = 34
        H = 35
        J = 36
        K = 37
        L = 38
        Semicolon = 39
        Apostrophe = 40
        Grave = 41
        LeftShift = 42
        Backslash = 43
        Z = 44
        X = 45
        C = 46
        V = 47
        B = 48
        N = 49
        M = 50
        Comma = 51
        Period = 52
        ForwardSlash = 53
        RightShift = 54
        Asterisk = 55
        LeftAlt = 56
        Space = 57
        CapsLock = 58
        F1 = 59
        F2 = 60
        F3 = 61
        F4 = 62
        F5 = 63
        F6 = 64
        F7 = 65
        F8 = 66
        F9 = 67
        F10 = 68
        NumLock = 69
        ScrollLock = 70
        Numpad7 = 71
        Numpad8 = 72
        Numpad9 = 73
        NumpadMinus = 74
        Numpad4 = 75
        Numpad5 = 76
        Numpad6 = 77
        NumpadPlus = 78
        Numpad1 = 79
        Numpad2 = 80
        Numpad3 = 81
        Numpad0 = 82
        NumpadPeriod = 83
        F11 = 87
        F12 = 88
        LeftWindows = 91
        RightWindows = 92
        Menu = 93
        NumpadEnter = 156
        RightCtrl = 157
        NumpadForwardSlash = 181
        RightAlt = 184
        Home = 199
        UpArrow = 200
        PageUp = 201
        LeftArrow = 203
        RightArrow = 205
        EndKey = 207
        DownArrow = 208
        PageDown = 209
        Insert = 210
        Delete = 211
    End Enum
    Public Enum MouseInputs
        WheelUp = -127
        None = 0
        LeftButton = 1
        RightButton = 2
        CenterButton = 3
        WheelDown = 127
    End Enum
    Public Enum NativeKeys
        ChangeCamera = 0
        Sprint = 1
        Jump = 2
        EnterVehicle = 3
        AttackOnFoot = 4
        LookBehind = 7
        NextWeaponOnFoot = 8
        PreviousWeaponOnFoot = 9
        '  MouseWheelUp = 10
        '  MouseWheelDown = 11
        'Unknown --> da 12 a 22
        Crouch = 20
        Phone = 21
        Action = 23
        TakeCover = 28
        Reload = 29
        ' MouseLeftButton = 39
        VehicleAccelerate = 40
        VehicleBrake = 41
        VehicleHeadlight = 42
        EnterVehicleOnFoot = 43
        VehicleHotwire1 = 46
        VehicleHotwire2 = 47
        Unknown1 = 48
        Unknown2 = 49
        VehicleLookBehind = 50
        VehicleCinematicCamera = 51
        VehicleNextRadioStation = 52
        VehiclePreviousRadioStation = 53
        VehicleHorn = 54
        HelicopterThrottleUp = 55
        HelicopterThrottleDown = 56
        HelicopterRotateLeft = 57
        HelicopterRotateRight = 58
        CombatPunch1 = 59
        CombatPunch2 = 60
        Esc = 61
        CombatKicked = 62
        CombatBlock = 63
        NavDown = 64
        NavUp = 65
        NavLeft = 66
        NavRight = 67
        NavEnter = 77
        NavLeave = 76
        NavBack = 78
        ' Unknown01 = 81 'Type 2
        ' Unknown02 = 82 'Type 2
        ' Unknown03 = 83 'Type 2 [MouseWheelUp]
        ' Unknown04 = 84 'Type 2 [MouseWheelDown]
        RadarZoom = 86
        Aim = 87
        'MoveForward = 1090
        'MoveBackward = 1091
        'MoveLeft = 1092
        'MoveRight = 1093
    End Enum
    Public Enum NavKeys
        Up
        Down
        Left
        Right
    End Enum
    Partial Public Class Joypad
        Public Const ID As Integer = 0
        Private Shared _LTrigger As Single = 0.0F
        Private Shared _RTrigger As Single = 0.0F
        Private Shared _pluginLoaded As Boolean = False
        Private Shared _controller As Object = Nothing
        Private Shared _getStateMethod As MethodInfo = Nothing
        Private Shared state As Object
        Private Shared gamepadProp As PropertyInfo
        Private Shared gamepad As Object
        Public Shared ReadOnly Property isUsingController As Boolean
            Get
                Return [Call](Of Boolean)("IS_USING_CONTROLLER")
            End Get
        End Property
        Public Shared ReadOnly Property LeftStick As GTA.Vector2
            Get
                Return getAnalogPosition(Sticks.Left)
            End Get
        End Property
        Public Shared ReadOnly Property RightStick As GTA.Vector2
            Get
                Return getAnalogPosition(Sticks.Right)
            End Get
        End Property
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared WriteOnly Property ShakePadWhenControllerDisabled As Boolean
            Set(value As Boolean)
                If value = True Then
                    [Call]("SHAKE_PLAYERPAD_WHEN_CONTROLLER_DISABLED")
                Else
                    [Call]("CLEAR_SHAKE_PLAYERPAD_WHEN_CONTROLLER_DISABLED")
                End If
            End Set
        End Property
        Public Enum Inputs
            Back = 13
            Start = 12
            X = 14
            Y = 15
            A = 16
            B = 17
            Up = 8
            Down = 9
            Left = 10
            Right = 11
            LB = 4
            RB = 6
            RTrigger = 7
            Left_Stick = 18
            Right_Stick = 19
            LeftTrigger = 5
            RightTrigger = 7
        End Enum
        Private Enum Sticks
            Left = 1
            Right = 2
        End Enum
        Public Shared Function isButtonPressed(Button As Inputs, Optional JustPressed As Boolean = True) As Boolean
            If Not isUsingController() Then
                Return False
            Else
                If JustPressed Then Return [Call](Of Boolean)("IS_BUTTON_JUST_PRESSED", ID, Button) Else Return [Call](Of Boolean)("IS_BUTTON_PRESSED", ID, Button)
            End If
        End Function
        Private Shared Function getAnalogPosition(Stick As Sticks) As GTA.Vector2
            If Not isUsingController() Then Exit Function

            Dim LeftStick_X, LeftStick_Y, RightStick_X, RightStick_y As Native.Pointer
            LeftStick_X = New Native.Pointer(GetType(Int32))
            LeftStick_Y = New Native.Pointer(GetType(Int32))
            RightStick_X = New Native.Pointer(GetType(Int32))
            RightStick_y = New Native.Pointer(GetType(Int32))
            [Call]("GET_POSITION_OF_ANALOGUE_STICKS", 0, LeftStick_X, LeftStick_Y, RightStick_X, RightStick_y)
            If Stick = Sticks.Left Then
                ' StickPosition = New gta.Vector2(LeftStick_X.Value, LeftStick_Y.Value)
                'Return StickPosition
                Return New GTA.Vector2(LeftStick_X.Value, LeftStick_Y.Value)
            ElseIf Stick = Sticks.Right Then
                ' StickPosition = New gta.Vector2(RightStick_X.Value, RightStick_y.Value)
                'Return StickPosition
                Return New GTA.Vector2(RightStick_X.Value, RightStick_y.Value)
            Else
                ' StickPosition = New gta.Vector2(0, 0)
                Return New GTA.Vector2(0, 0)
            End If
        End Function
        Public Shared Sub ShakePad(Intensity As Integer, Duration As Integer)
            [Call]("SHAKE_PAD", 0, Intensity, Duration)
        End Sub
    End Class
    Partial Public Class Joypad
#If USE_SLIMDX Then
        Private Shared controller As New Controller(UserIndex.One)

        ''' <summary>
        ''' Gets the current position of the left trigger (0.0 - 1.0)
        ''' </summary>
        Public Shared ReadOnly Property LeftTrigger As Single
            Get
                If Not controller.IsConnected Then Return 0.0F
                Return controller.GetState().Gamepad.LeftTrigger / 255.0F
            End Get
        End Property

        ''' <summary>
        ''' Gets the current position of the right trigger (0.0 - 1.0)
        ''' </summary>
        Public Shared ReadOnly Property RightTrigger As Single
            Get
                If Not controller.IsConnected Then Return 0.0F
                Return controller.GetState().Gamepad.RightTrigger / 255.0F
            End Get
        End Property
        '#ElseIf USE_SHARPDX Then
        '    Private Shared controller As New SharpDX.XInput.Controller(SharpDX.XInput.UserIndex.One)

        '    Public Shared ReadOnly Property LeftTrigger As Single
        '        Get
        '            If Not controller.IsConnected Then Return 0.0F
        '            Return controller.GetState().Gamepad.LeftTrigger / 255.0F
        '        End Get
        '    End Property

        '    Public Shared ReadOnly Property RightTrigger As Single
        '        Get
        '            If Not controller.IsConnected Then Return 0.0F
        '            Return controller.GetState().Gamepad.RightTrigger / 255.0F
        '        End Get
        '    End Property
#End If
    End Class
End Class

'End Class

