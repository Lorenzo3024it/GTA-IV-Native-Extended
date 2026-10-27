Imports System.Reflection
Imports System.IO
Imports GTA
Imports GTA.Native.Function
Namespace NativeExtended.Internal
    Friend NotInheritable Class GamePathResolver
        Private Sub New() : End Sub
        Friend Shared Function ResolveGamePath() As String
            Try
                Dim asm As Assembly = Assembly.GetAssembly(GetType(GamePathResolver))

                If asm Is Nothing Then
                    ' DEBUG: NativeExtended assembly could not be resolved
                    Return String.Empty
                End If

                Dim location As String = asm.Location

                If String.IsNullOrEmpty(location) Then
                    ' DEBUG: NativeExtended assembly location is empty
                    Return String.Empty
                End If

                Return Path.GetDirectoryName(location)

            Catch ex As Exception
                ' DEBUG: Exception while resolving game path
                Return String.Empty
            End Try
        End Function
    End Class
    Friend NotInheritable Class Natives
        Private Sub New() : End Sub
        Friend Function GetLiteralStringWidth([String] As String) As Single
            Return [Call](Of Single)("GET_STRING_WIDTH_WITH_STRING", "STRING", [String])
        End Function
        Friend Function GetNumberOfLines(Text As String, WrapStartPos As Single, WrapEndPos As Single) As Integer
            Return [Call](Of Integer)("GET_NUMBER_LINES_WITH_LITERAL_STRINGS", WrapStartPos, WrapEndPos, "STRING", "", Text)
        End Function
        Friend Sub DisplayLiteralString(Text As String, PosX As Single, PosY As Single)
            [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", PosX, PosY, "STRING", Text)
        End Sub
    End Class
End Namespace
