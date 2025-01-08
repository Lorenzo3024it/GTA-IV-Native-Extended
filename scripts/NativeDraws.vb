Imports GTA
Imports GTA.Native.Function

Public Class NativeDraws
    Shared Function LoadTexture(TXD As String, Texture As String, Optional StreamedTXD As Boolean = False) As Integer
        Dim txdint As Integer
        If StreamedTXD Then
            txdint = [Call](Of Integer)("REQUEST_STREAMED_TXD", TXD)
            Return [Call](Of Integer)("GET_TEXTURE_FROM_STREAMED_TXD", txdint, Texture)
        Else
            txdint = [Call](Of Integer)("LOAD_TXD", TXD)
            Return [Call](Of Integer)("GET_TEXTURE", txdint, Texture)
        End If
    End Function
    Shared Sub DrawTexture(TXD As String, Texture As String, PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, RotationInDegrees As Single, Red As Int16, Green As Int16, Blue As Int16, Alpha As Int16)
        Dim txdint As Integer = [Call](Of Integer)("LOAD_TXD", TXD)
        Dim TextureInt As Integer = [Call](Of Integer)("GET_TEXTURE", txdint, Texture)
        If NativeGeneric.isPauseMenuActive = False Then [Call]("DRAW_SPRITE", TextureInt, PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, RotationInDegrees, Red, Green, Blue, Alpha)
    End Sub
    Shared Sub DrawHelperText(PredefinedMessage As String, GameKey As String, SameLine As Boolean)
        [Call]("DRAW_FRONTEND_HELPER_TEXT", PredefinedMessage, GameKey, Not SameLine)
    End Sub
    Shared Sub DrawScreen(PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, RotationInDegrees As Single, Red As Int16, Green As Int16, Blue As Int16, Alpha As Int16)
        [Call]("DRAW_SPRITE_FRONT_BUFF", PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, RotationInDegrees, Red, Green, Blue, Alpha)
    End Sub
    Shared Sub DrawCurvedWindow(PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, Alpha As Int16)
        [Call]("DRAW_CURVED_WINDOW", PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, Alpha)
    End Sub
    Shared Sub DrawRectangle(PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, Red As Int16, Green As Int16, Blue As Int16, Alpha As Int16)
        [Call]("DRAW_RECT", PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, Red, Green, Blue, Alpha)
    End Sub
    Shared Sub RelaseTexture(Texture As Integer)
        [Call]("RELEASE_TEXTURE", Texture)
    End Sub
End Class