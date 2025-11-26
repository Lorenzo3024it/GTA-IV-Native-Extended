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
    ''' Uses the native function ASCII_INT_TO_STRING, with optional zeros before the number.
    ''' </summary>
    ''' <param name="value">NumberToConvert</param>
    ''' <param name="NumOfZeros">Zero to add before the number (default = 0 -> no padding).</param>
    Public Shared Function IntegerToString(Value As Integer, Optional NumOfZeros As Integer = 0) As String
        ' Conversione nativa di base
        Dim baseStr As String = [Call](Of String)("INT_TO_STRING", Value)

        ' Aggiunge zeri iniziali se richiesto
        If NumOfZeros > 0 Then
            baseStr = baseStr.PadLeft(NumOfZeros, "0"c)
        End If

        Return baseStr
    End Function
    ''' <summary>
    ''' Interpola linearmente tra due valori numerici.
    ''' </summary>
    Public Shared Function Lerp(a As Single, b As Single, t As Single) As Single
        t = Math.Min(Math.Max(t, 0.0F), 1.0F)
        Return a + (b - a) * t
    End Function
    ''' <summary>
    ''' Interpola linearmente tra due vettori 3D.
    ''' </summary>
    Public Shared Function Lerp(a As Vector3, b As Vector3, t As Single) As Vector3
        t = Math.Min(Math.Max(t, 0.0F), 1.0F)
        Return New Vector3(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t,
            a.Z + (b.Z - a.Z) * t
        )
    End Function
    ''' <summary>
    ''' Interpola tra due angoli (in gradi), evitando il salto tra 359° e 0°.
    ''' </summary>
    Public Shared Function LerpAngle(a As Single, b As Single, t As Single) As Single
        t = Math.Min(Math.Max(t, 0.0F), 1.0F)
        Dim delta As Single = ((b - a + 540.0F) Mod 360.0F) - 180.0F
        Return (a + delta * t + 360.0F) Mod 360.0F
    End Function
    ''' <summary>
    ''' Converts degrees in radians.
    ''' </summary>
    Public Shared Function ToRadians(degrees As Single) As Single
        Return CSng(degrees * (Math.PI / 180.0))
    End Function
    ''' <summary>
    ''' Converts radians in degrees.
    ''' </summary>
    Public Shared Function ToDegrees(radians As Single) As Single
        Return CSng(radians * (180.0 / Math.PI))
    End Function
    '''' <summary>
    '''' Structure to represent una rotation in Eulero's angles (Pitch, Roll, Yaw).
    '''' </summary>
    'Public Structure Euler
    '    Public Pitch As Single  ' Rotazione sull'asse X
    '    Public Roll As Single   ' Rotazione sull'asse Y
    '    Public Yaw As Single    ' Rotazione sull'asse Z (Heading)

    '    Public Sub New(pitch As Single, roll As Single, yaw As Single)
    '        Me.Pitch = pitch
    '        Me.Roll = roll
    '        Me.Yaw = yaw
    '    End Sub

    '    Public Overrides Function ToString() As String
    '        Return $"Pitch={Pitch:F2}, Roll={Roll:F2}, Yaw={Yaw:F2}"
    '    End Function
    'End Structure
    '''' <summary>
    '''' Convertes a rotation in Euler
    '''' </summary>
    'Public Shared Function ToEuler(Rotation As Vector3) As Euler
    '    Return New Euler(Rotation.X, Rotation.Y, Rotation.Z)
    'End Function
    '''' <summary>
    '''' Converte un oggetto Euler in Vector3
    '''' </summary>
    'Public Shared Function ToVector(e As Euler) As Vector3
    '    Return New Vector3(e.Pitch, e.Roll, e.Yaw)
    'End Function
    ''' <summary>
    ''' Uses the native function STRING_TO_INT. 
    ''' Returns nothing if string can't be converted.
    ''' </summary>
    Public Shared Function StringToInteger(StringToConvert As String) As Integer
        Dim pointer As GTA.Native.Pointer = New GTA.Native.Pointer(GetType(Integer))
        Dim convertedSuccessfully As Boolean = [Call](Of Boolean)("STRING_TO_INT", StringToConvert, pointer)
        If convertedSuccessfully Then Return pointer.Value Else Return Nothing
    End Function

    '------------------------------------------------------------------------------------------------------------------
    Private Const DefaultLogPath As String = "NativeExtended-IV.log"
    Private Shared _hasWrittenWelcomeMessage As Boolean = False
    Private Shared _firstClean As Boolean = False
    Private Shared _isWriting As Boolean = False

    ''' <summary>
    ''' Writes a message in a log file. If CustomLogPath is set, it will write the message there
    ''' (and all intenal errors chatced), otherwise it will be written in NativeExtended-IV.log.
    ''' You can be temporarely override the log path to write in custom file.
    ''' </summary>
    Public Shared Sub WriteLog(Message As String, Optional WriteDateAndTime As Boolean = False, Optional OverwriteFile As Boolean = False)
        Try
            ' Evita chiamate ricorsive o doppie scritture
            If _isWriting Then Return
            _isWriting = True

            If Not _hasWrittenWelcomeMessage Then
                WriteWelcomeMessage()
            End If

            Dim completeLogPath As String = GetFullLogPath("")

            Using sw As New StreamWriter(completeLogPath, Not OverwriteFile)
                If WriteDateAndTime Then
                    sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {Message}")
                Else
                    sw.WriteLine(Message)
                End If
            End Using

        Catch

        Finally
            _isWriting = False
        End Try
    End Sub

    ''' <summary>
    ''' Writes an advanced log with custom message when a script catches an Exception.
    ''' You can be temporarely override the log path to write in custom file.
    ''' </summary>
    Public Shared Sub WriteLogAdvanced(Message As String, Exception As Exception, Optional OverwriteFile As Boolean = False)
        Try
            If _isWriting Then Return
            _isWriting = True

            If Not _hasWrittenWelcomeMessage Then
                WriteWelcomeMessage()
                _firstClean = True
            End If
            Dim completeLogPath As String = GetFullLogPath("")

            Dim overwrite As Boolean
            If OverwriteFile Then
                overwrite = True
                _firstClean = True
            Else
                overwrite = Not _firstClean
                _firstClean = True
            End If

            Using sw As New StreamWriter(completeLogPath, Not overwrite)
                sw.WriteLine("[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "]")
                If Not String.IsNullOrEmpty(Message) Then sw.WriteLine(Message)
                WriteExceptionRecursive(Exception, sw)
                sw.WriteLine(New String("-"c, 80))
            End Using

        Catch

        Finally
            _isWriting = False
        End Try
    End Sub
    Private Shared Function GetFullLogPath(Optional CustomLogPath As String = "") As String
        Try
            If Not String.IsNullOrEmpty(CustomLogPath) AndAlso IO.Path.IsPathRooted(CustomLogPath) Then
                Return CustomLogPath
            End If

            Dim folder As String = Game.InstallFolder
            If String.IsNullOrEmpty(folder) Then
                folder = Environment.CurrentDirectory
            End If

            Dim logFileName As String
            If String.IsNullOrEmpty(CustomLogPath) Then
                logFileName = "NativeExtended-IV.log"
            Else
                logFileName = CustomLogPath
            End If

            Dim combinedPath As String = IO.Path.Combine(folder, logFileName)

            Dim dir As String = IO.Path.GetDirectoryName(combinedPath)
            If Not IO.Directory.Exists(dir) Then
                IO.Directory.CreateDirectory(dir)
            End If

            Return combinedPath

        Catch ex As Exception
            Return IO.Path.Combine(Environment.CurrentDirectory, "NativeExtended-IV.log")
        End Try
    End Function
    Private Shared Sub WriteExceptionRecursive(ex As Exception, sw As StreamWriter, Optional level As Integer = 0)
        Try
            Dim indent As String = New String(" "c, level * 2)
            sw.WriteLine($"{indent}Exception Type: {ex.GetType().FullName}")
            sw.WriteLine($"{indent}Message: {ex.Message}")
            sw.WriteLine($"{indent}Stack Trace: {ex.StackTrace}")
            If ex.InnerException IsNot Nothing Then
                sw.WriteLine(indent & "Inner Exception:")
                WriteExceptionRecursive(ex.InnerException, sw, level + 1)
            End If
        Catch

        End Try
    End Sub
    Private Shared Sub WriteWelcomeMessage()
        Try
            Dim logPath As String = GetFullLogPath(DefaultLogPath)
            Using sw As New StreamWriter(logPath, False)
                sw.WriteLine("|============================================================|")
                sw.WriteLine("| ------   NativeExtended-IV 1.2.0 by Lorenzo3024it   ------ |")
                sw.WriteLine("| ----------    Thank you for using my mods :)   ----------- |")
                sw.WriteLine("| -------     If you need help please contact me     ------- |")
                sw.WriteLine("| -- bttf4thebigrelase@gmail.com / lorenzo3024@hotmail.it  - |")
                sw.WriteLine("|============================================================|")
                sw.WriteLine("")
            End Using
            _hasWrittenWelcomeMessage = True
        Catch

        End Try
    End Sub

    '------------------------------------------------------------------------------------------------------------------
    Public Class LogFile
        Private ReadOnly _logPath As String
        Public ReadOnly Property Path As String
            Get
                Return _logPath
            End Get
        End Property

        Private _hasWrittenWelcomeMessage As Boolean = False
        Private _hasWrittenWelcomeMessageOnDefaultLog As Boolean = False
        Private _overwriteBeforeWelcomeMessage As Boolean = True
        Private _isWriting As Integer = 0
        Private _CustomHeaderLines As IEnumerable(Of String)
        Sub New(Path As String)
            If String.IsNullOrWhiteSpace(Path) Then
                _logPath = Game.InstallFolder & "\" & "NativeExtended-IV.log"
            Else
                _logPath = Game.InstallFolder & "\" & Path
            End If
        End Sub
        Public Sub SetHeaderMessage(Lines As IEnumerable(Of String), Optional OverwriteFile As Boolean = True)
            If Lines Is Nothing OrElse Not Lines.Any() Then Return
            _overwriteBeforeWelcomeMessage = OverwriteFile
            _CustomHeaderLines = Lines
        End Sub
        Public Sub WriteLog(Message As String, Optional WriteDateAndTime As Boolean = False, Optional OverwriteFile As Boolean = False)
            If Threading.Interlocked.Exchange(_isWriting, 1) = 1 Then Return
            Try
                EnsureWelcomeMessages()
                Dim logPath As String = GetFullLogPath(_logPath)
                Using sw As New StreamWriter(logPath, Not OverwriteFile)
                    If WriteDateAndTime Then
                        sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {Message}")
                    Else
                        sw.WriteLine(Message)
                    End If
                End Using
            Catch
            Finally
                Threading.Interlocked.Exchange(_isWriting, 0)
            End Try
        End Sub
        Public Sub WriteLogAdvanced(Message As String, Exception As Exception)
            If Threading.Interlocked.Exchange(_isWriting, 1) = 1 Then Return
            Try
                EnsureWelcomeMessages()
                Dim logPath As String = GetFullLogPath(_logPath)
                Using sw As New StreamWriter(logPath, True)
                    sw.WriteLine("[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "]")
                    If Not String.IsNullOrEmpty(Message) Then sw.WriteLine(Message)
                    WriteExceptionRecursive(Exception, sw)
                    sw.WriteLine(New String("-"c, 80))
                End Using
            Catch
            Finally
                Threading.Interlocked.Exchange(_isWriting, 0)
            End Try
        End Sub
        '--------------------------------------------------------------------------------------
        Private Sub WriteExceptionRecursive(ex As Exception, sw As StreamWriter, Optional level As Integer = 0)
            Dim indent As String = New String(" "c, level * 2)
            sw.WriteLine($"{indent}Exception Type: {ex.GetType().FullName}")
            sw.WriteLine($"{indent}Message: {ex.Message}")
            sw.WriteLine($"{indent}Stack Trace: {ex.StackTrace}")
            If ex.InnerException IsNot Nothing Then
                sw.WriteLine(indent & "Inner Exception:")
                WriteExceptionRecursive(ex.InnerException, sw, level + 1)
            End If
        End Sub
        Private Sub EnsureWelcomeMessages()
            If Not _hasWrittenWelcomeMessageOnDefaultLog AndAlso _logPath.EndsWith("NativeExtended-IV.log", StringComparison.OrdinalIgnoreCase) Then
                WriteNativeExtendedWelcomeMessage()
                _hasWrittenWelcomeMessageOnDefaultLog = True
            End If

            ' Scrive header personalizzato se previsto
            If Not _hasWrittenWelcomeMessage AndAlso _CustomHeaderLines IsNot Nothing Then
                WriteCustomHeader()
                _hasWrittenWelcomeMessage = True
            End If
        End Sub
        Private Shared Function GetFullLogPath(customLogPath As String) As String
            Try
                If IO.Path.IsPathRooted(customLogPath) Then
                    Return customLogPath
                End If

                Dim folder As String = Game.InstallFolder
                If String.IsNullOrEmpty(folder) Then
                    folder = Environment.CurrentDirectory
                End If

                Dim combinedPath As String = IO.Path.Combine(folder, customLogPath)

                Dim dir As String = IO.Path.GetDirectoryName(combinedPath)
                If Not IO.Directory.Exists(dir) Then
                    IO.Directory.CreateDirectory(dir)
                End If

                Return combinedPath
            Catch
                Return IO.Path.Combine(Environment.CurrentDirectory, "NativeExtended-IV.log")
            End Try
        End Function
        Private Sub WriteNativeExtendedWelcomeMessage()
            Dim logPath As String = GetFullLogPath("NativeExtended-IV.log")
            Using sw As New StreamWriter(logPath, False)
                sw.WriteLine("|============================================================|")
                sw.WriteLine("| ------   NativeExtended-IV 1.2.0 by Lorenzo3024it   ------ |")
                sw.WriteLine("| ----------    Thank you for using my mods :)   ----------- |")
                sw.WriteLine("| -------     If you need help please contact me     ------- |")
                sw.WriteLine("| -- bttf4thebigrelase@gmail.com / lorenzo3024@hotmail.it  - |")
                sw.WriteLine("|============================================================|")
                sw.WriteLine("")
            End Using
        End Sub
        Private Sub WriteCustomHeader()
            Dim path As String = GetFullLogPath(_logPath)
            Using sw As New StreamWriter(path, Not _overwriteBeforeWelcomeMessage)
                For Each line As String In _CustomHeaderLines
                    sw.WriteLine(line)
                Next
                sw.WriteLine(New String("-"c, 80))
            End Using
        End Sub
    End Class
    '------------------------------------------------------------------------------------------------------------------
End Class
