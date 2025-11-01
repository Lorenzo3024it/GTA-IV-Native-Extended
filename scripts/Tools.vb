Imports System.IO
Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Public Class Tools

    Private Shared _currentFPS As Single
    ''' <summary>
    ''' A simple non-native function which restricts an integer from a minimum to a maximum value.
    ''' </summary>
    ''' <param name="value">Value to restrict</param>
    ''' <param name="min">Minimum value allowed</param>
    ''' <param name="max">Maximum value allowed</param>
    ''' <returns></returns>
    Public Shared Function ClampInteger(Value As Integer, Min As Integer, Max As Integer) As Integer
        If Value < Min Then Return Min
        If Value > Max Then Return Max
        Return Value
    End Function
    ''' <summary>
    ''' A simple non-native function which restricts a float from a minimum to a maximum value.
    ''' </summary>
    ''' <param name="value">Value to restrict</param>
    ''' <param name="min">Minimum value allowed</param>
    ''' <param name="max">Maximum value allowed</param>
    ''' <returns></returns>
    Public Shared Function ClampFloat(Value As Single, Min As Single, Max As Single) As Single
        If Value < Min Then Return Min
        If Value > Max Then Return Max
        Return Value
    End Function
    Public Shared Function GetCoefFPS(BaseFPS As Integer, Optional ShowFPS As Boolean = False) As Single
        If ShowFPS Then Game.DisplayText(Math.Round(Game.FPS, 2))
        _currentFPS = Game.FPS
        If _currentFPS <= 0 Then _currentFPS = BaseFPS
        Return BaseFPS / _currentFPS
    End Function
    ''' <summary>
    ''' Uses the native function ASCII_INT_TO_STRING.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function IntegerToString(IntegerToConvert As Integer) As String
        Return [Call](Of String)("ASCII_INT_TO_STRING", IntegerToConvert)
    End Function
    ''' <summary>
    ''' Uses the native function STRING_TO_INT. 
    ''' Returns nothing if string can't be converted.
    ''' </summary>
    Public Shared Function StringToInteger(StringToConvert As String) As Integer
        Dim pointer As GTA.Native.Pointer = New GTA.Native.Pointer(GetType(Integer))
        Dim convertedSuccessfully As Boolean = [Call](Of Boolean)("STRING_TO_INT", StringToConvert, pointer)
        If convertedSuccessfully Then Return pointer.Value Else Return Nothing
    End Function
    Public Shared Sub WriteLog(Message As String, Optional FilePath As String = "\NativeExtended-IV.log", Optional WriteDateAndTime As Boolean = False, Optional OverwriteFile As Boolean = False)
        Dim logPath As String = Game.InstallFolder & FilePath
        Using sw As New StreamWriter(logPath, Not OverwriteFile)
            If WriteDateAndTime Then sw.WriteLine(DateTime.Now.ToString & ": " & Message) Else sw.WriteLine(Message)
        End Using
    End Sub
End Class
