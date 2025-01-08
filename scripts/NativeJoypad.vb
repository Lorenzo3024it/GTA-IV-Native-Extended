Imports GTA
Imports GTA.Native.Function

Public Class NativeJoypad
    'Inherits Script
    'Public LeftStick_X, LeftStick_Y, RightStick_X, RightStick_y As Native.Pointer
    Shared PacketNumber As Integer
    Public Enum Buttons
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
        LT = 5
        RT = 7
        LB = 4
        RB = 6
        Left_Stick = 18
        Right_Stick = 19
    End Enum
    Public Enum Sticks
        LeftStick = 1
        RightStick = 2
    End Enum
    Sub New()
        'Me.Interval = 1
        'LeftStick_X = New Native.Pointer(GetType(Int32))
        'LeftStick_Y = New Native.Pointer(GetType(Int32))
        'RightStick_X = New Native.Pointer(GetType(Int32))
        'RightStick_y = New Native.Pointer(GetType(Int32))

        'LeftStick_X.SetValue(0)
        'LeftStick_Y.SetValue(0)
        'RightStick_X.SetValue(0)
        'RightStick_y.SetValue(0)

        PacketNumber = 1
    End Sub
    Shared Function isButtonPressed(Button As Buttons, Optional JustPressed As Boolean = False) As Boolean
        If isUsingController() Then
            PacketNumber += 1
            If PacketNumber > 9999 Then PacketNumber = 0

            If JustPressed Then Return [Call](Of Boolean)("IS_BUTTON_JUST_PRESSED", 0, Button) Else Return [Call](Of Boolean)("IS_BUTTON_PRESSED", 0, Button)
        Else
            Return False
        End If
    End Function
    Shared Function isUsingController() As Boolean
        Return [Call](Of Boolean)("IS_USING_CONTROLLER")
    End Function
    Shared Sub ShakePad(Intensity As Integer, Duration As Integer)
        [Call]("SHAKE_PAD", 0, Intensity, Duration)
    End Sub
    'Private Sub UpdateAnalogueSticks(sender As Object, e As System.EventArgs) Handles Me.Tick
    '    If isUsingController() AndAlso PacketNumber > 0 Then
    '        [Call]("GET_POSITION_OF_ANALOGUE_STICKS", 0, LeftStick_X, LeftStick_Y, RightStick_X, RightStick_y)
    '    End If
    'End Sub
    Shared Function getAnalogPosition(Stick As Sticks) As Vector2
        Dim LeftStick_X, LeftStick_Y, RightStick_X, RightStick_y As Native.Pointer
        Dim StickPosition As Vector2
        LeftStick_X = New Native.Pointer(GetType(Int32))
        LeftStick_Y = New Native.Pointer(GetType(Int32))
        RightStick_X = New Native.Pointer(GetType(Int32))
        RightStick_y = New Native.Pointer(GetType(Int32))
        [Call]("GET_POSITION_OF_ANALOGUE_STICKS", 0, LeftStick_X, LeftStick_Y, RightStick_X, RightStick_y)
        If Stick = Sticks.LeftStick Then
            StickPosition = New Vector2(LeftStick_X.Value, LeftStick_Y.Value)
            Return StickPosition
        ElseIf Stick = Sticks.RightStick Then
            StickPosition = New Vector2(RightStick_X.Value, RightStick_y.Value)
            Return StickPosition
        Else
            StickPosition = New Vector2(0, 0)
            Return StickPosition
        End If
    End Function
End Class