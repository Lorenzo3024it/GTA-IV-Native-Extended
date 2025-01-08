Imports GTA
Imports GTA.Native
Imports GTA.Native.Function

Public Class NativeMouse
    Public Enum MouseWheelInput
        Down = 127
        None = 0
        Up = -127
    End Enum
    Shared Function getMousePosition() As Vector2
        Dim PosX, PosY As Native.Pointer
        PosX = New Native.Pointer(GetType(Single))
        PosY = New Native.Pointer(GetType(Single))
        [Call]("GET_MOUSE_POSITION", PosX, PosY)
        Return New Vector2(PosX.Value, PosY.Value)
    End Function
    Shared Function isButtonPressed(Button, Optional JustPressed = False) As Boolean
        If JustPressed Then
            Return [Call](Of Boolean)("IS_MOUSE_BUTTON_JUST_PRESSED", Button)
        Else
            Return [Call](Of Boolean)("IS_MOUSE_BUTTON_PRESSED", Button)
        End If
    End Function
    'Shared Function isMouseVerticalInverted() As Boolean
    '    Return [Call](Of Boolean)("IS_MOUSE_USING_VERTICAL_INVERSION")
    'End Function
    Shared Function getMouseSensitivity() As Single
        Return [Call](Of Single)("GET_MOUSE_SENSITIVITY")
    End Function
    Shared Function getMouseInput() As Boolean
        Return [Call](Of Boolean)("GET_MOUSE_INPUT")
    End Function
    Shared Function getMouseWheelInput() As MouseWheelInput
        Dim Input As Pointer = New Pointer(GetType(Integer))
        [Call](Of Integer)("GET_MOUSE_WHEEL", Input)
        Return Input.Value
    End Function
End Class