Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms
Imports GTA
Imports GTA.base
Imports GTA.Native.Function
Imports NativeExtended.NativeDraws
Imports NativeExtended.NativeExtended.Internal
Public NotInheritable Class Tools
    Private Sub New() : End Sub
    Private Shared _gamePath As String
    Private Shared pointWidth, pointHeight As New Native.Pointer(GetType(Integer))
    Private Shared pointPhysWidth, pointPhysHeight As New Native.Pointer(GetType(Integer))
    Private Shared screenRes As (Width As Single, Height As Single)
    Private Shared screenPhysicalRes As (Width As Single, Height As Single)
    Private Const RES_REFRESH_INTERVAL As Integer = 5000
    Private Shared tLastRecalc As Integer = -1
    Private Shared _frameCount As Integer
    Private Shared _lastCalcTime As Long
    Private Shared _screenAspectRatio As Single
    Private Shared tLastAspectRecalc As Integer = -1
    Private Shared _fps As Integer
    Private Const FPS_Interval As Integer = 250
    Private Shared _lastFrameTime As Long = 0
    Private Shared _deltaTimeMs As Single = 16.6667F ' fallback ~60 FPS
    Private Shared ReadOnly _pointerFrameTime As New Native.Pointer(GetType(Single))
    '-------
    Private Shared _showFPS As Boolean = False
    Private Shared ReadOnly _fpsPosText As New Vector2(0.005F, 0.01F)
    Private Shared ReadOnly _fpsPosValue As New Vector2(0.0343F, 0.01F)
    Private Shared ReadOnly _fpsBgPos As New Vector2(0.0F, 0.0F)
    Private Shared ReadOnly _fpsBgSize As New Vector2(0.0484F, 0.032F)
    Private Shared TextStyleFPSLabel As New TextStyle(Color:=ColorRGBA.White, Size:=11, Justification.Left, BorderColor:=ColorRGBA.Black, Font:=TextStyle.Fonts.Narrow, WidthScale:=1.3)
    Private Shared TextStyleFPSValue As New TextStyle(Color:=ColorRGBA.White, Size:=11, Justification.Center, BorderColor:=ColorRGBA.Black, Font:=TextStyle.Fonts.Narrow, WidthScale:=1.4)
    '-------
    ''' <summary>
    ''' Returns current Frames Per Second (FPS).
    ''' </summary>
    Public Shared ReadOnly Property FPS As Integer
        Get
            UpdateFPS()
            Return _fps
        End Get
    End Property
    ''' <summary>
    ''' Gets the time elapsed during the last rendered frame, in seconds.
    ''' Uses GET_FRAME_TIME native.
    ''' </summary>
    Public Shared ReadOnly Property FrameTime As Single
        Get
            [Call]("GET_FRAME_TIME", _pointerFrameTime)
            Return _pointerFrameTime.Value
        End Get
    End Property
    ''' <summary>
    ''' Gets the total number of frames rendered since the game started.
    ''' Uses GET_FRAME_COUNT native.
    ''' </summary>
    Public Shared ReadOnly Property FrameCount As Integer
        Get
            Return [Call](Of Integer)("GET_FRAME_COUNT")
        End Get
    End Property
    ''' <summary>
    ''' Enables or disables on-screen FPS display (top-left corner).
    ''' </summary>
    Public Shared Property ShowFPS As Boolean
        Get
            Return _showFPS
        End Get
        Set(value As Boolean)
            _showFPS = value
            If value = True Then
                If Not TickHelper.ContainsHandler_Internal(AddressOf DrawFPSOverlay) Then TickHelper.Add_Internal(AddressOf DrawFPSOverlay)
            Else
                If TickHelper.ContainsHandler_Internal(AddressOf DrawFPSOverlay) Then TickHelper.Remove_Internal(AddressOf DrawFPSOverlay)
            End If
        End Set
    End Property
    Friend Shared Sub DrawFPSOverlay()
        If Not _showFPS Then Return
        Dim fpsValue As Integer = FPS

        DrawRectangle(PercentagePos:=_fpsBgPos, PercentageSize:=_fpsBgSize, Color:=New ColorRGBA(0, 0, 0, 120), Duration:=1, CenteredX:=False, CenteredY:=False, BordersColor:=ColorRGBA.Black, BordersThickness:=0.0025F)

        DrawText(Text:="FPS:", Duration:=1, PositionPercent:=_fpsPosText, Style:=TextStyleFPSLabel)
        DrawText(Text:=fpsValue.ToString(), Duration:=1, PositionPercent:=_fpsPosValue, Style:=TextStyleFPSValue)
    End Sub
    Private Shared Sub UpdateDeltaTime()
        Dim now As Long = Environment.TickCount
        If _lastFrameTime > 0 Then
            Dim dt As Long = now - _lastFrameTime
            If dt > 0 AndAlso dt < 250 Then
                _deltaTimeMs = CSng(dt)
            End If
        End If
        _lastFrameTime = now
    End Sub
    Private Shared Sub UpdateFPS()
        _frameCount += 1
        Dim now As Long = Environment.TickCount
        If _lastCalcTime = 0 Then
            _lastCalcTime = now
            Return
        End If
        Dim elapsed As Long = now - _lastCalcTime
        If elapsed <= 0 Then Return
        If elapsed >= FPS_Interval Then
            _fps = CInt((_frameCount * 1000L) / elapsed)
            _frameCount = 0
            _lastCalcTime = now
        End If
    End Sub
    ''' <summary>
    ''' Gets the absolute path of the GTA IV installation directory.
    ''' </summary>
    Public Shared ReadOnly Property GamePath As String
        Get
            If String.IsNullOrEmpty(_gamePath) Then
                _gamePath = GamePathResolver.ResolveGamePath()
            End If

            Return _gamePath
        End Get
    End Property
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
    ''' <summary>
    ''' Returns a frame-time based FPS coefficient.
    ''' </summary>
    Public Shared Function GetCoefFPS(BaseFPS As Integer) As Single
        If BaseFPS <= 0 Then BaseFPS = 60
        UpdateDeltaTime()
        Dim baseFrameMs As Single = 1000.0F / BaseFPS
        Return baseFrameMs / _deltaTimeMs
    End Function
    ''' <summary>
    ''' Converts a float value to a string with a fixed number of decimals.
    ''' Ensures trailing zeros are shown according to precision.
    ''' </summary>
    Public Shared Function FloatToString(Value As Single, Optional Decimals As Integer = 2) As String
        Decimals = ClampInteger(Decimals, 0, 7)
        If Decimals = 0 Then
            Return CInt(Value).ToString()
        End If

        Dim formatStr As String = "0." & New String("0"c, Decimals)
        Return Value.ToString(formatStr)
    End Function
    ''' <summary>
    ''' Returns the current game rendering aspect ratio (Native).
    ''' The value is cached and automatically refreshed every 5 seconds.
    ''' </summary>
    Public Shared ReadOnly Property ScreenAspectRatio As Single
        Get
            Dim now As Integer = NativeTimer.TotalGameTime
            If tLastAspectRecalc < 0 OrElse now - tLastAspectRecalc >= RES_REFRESH_INTERVAL Then
                _screenAspectRatio = [Call](Of Single)("GET_ASPECT_RATIO")
                tLastAspectRecalc = now
            End If

            Return _screenAspectRatio
        End Get
    End Property
    ''' <summary>
    ''' Returns the current game rendering resolution (Native).
    ''' The value is cached and automatically refreshed every 5 seconds.
    ''' </summary>
    Public Shared ReadOnly Property ScreenResolution As (Width As Single, Height As Single)
        Get
            Dim now As Integer = NativeTimer.TotalGameTime

            If tLastRecalc < 0 OrElse now - tLastRecalc >= RES_REFRESH_INTERVAL Then
                [Call]("GET_SCREEN_RESOLUTION", pointWidth, pointHeight)
                screenRes = (CSng(pointWidth.Value), CSng(pointHeight.Value))
                tLastRecalc = now
            End If

            Return screenRes
        End Get
    End Property
    ''' <summary>
    ''' Returns the physical screen resolution (maximum monitor resolution).
    ''' This value is resolved once (Native) and cached for the entire game session.
    ''' </summary>
    Public Shared ReadOnly Property ScreenResolutionMax As (Width As Single, Height As Single)
        Get
            If screenPhysicalRes.Width <= 0 OrElse screenPhysicalRes.Height <= 0 Then
                [Call]("GET_PHYSICAL_SCREEN_RESOLUTION", pointPhysWidth, pointPhysHeight)
                screenPhysicalRes = (CSng(pointPhysWidth.Value), CSng(pointPhysHeight.Value))
            End If

            Return screenPhysicalRes
        End Get
    End Property
    ''' <summary>
    ''' Converts an integer value to a string, optionally adding leading zeros.
    ''' </summary>
    ''' <param name="Value">NumberToConvert</param>
    ''' <param name="PadLength">The minimum total length (only the numeric part) of the resulting string. Leading zeros are added when necessary.</param>
    ''' <returns>A string representation of the specified integer value, optionally padded with leading zeros.</returns>
    Public Shared Function IntegerToString(Value As Integer, Optional PadLength As Integer = 0) As String
        Dim baseStr As String = CStr(Value)
        If PadLength > 0 Then
            If Value < 0 Then
                baseStr = "-" & baseStr.Substring(1).PadLeft(PadLength, "0"c)
            Else
                baseStr = baseStr.PadLeft(PadLength, "0"c)
            End If
        End If
        Return baseStr
    End Function
    ''' <summary>
    ''' Returns True if the value is within the accepted tolerance range.
    ''' </summary>
    Public Shared Function IsValueInRange(Value As Single, Target As Single, Tolerance As Single) As Boolean
        Return Math.Abs(Value - Target) <= Tolerance
    End Function
    ''' <summary>
    ''' Linearly interpolation between two numeric values.
    ''' </summary>
    Public Shared Function Lerp(a As Single, b As Single, t As Single) As Single
        t = Math.Min(Math.Max(t, 0.0F), 1.0F)
        Return a + (b - a) * t
    End Function
    ''' <summary>
    ''' Linearly interpolation between two 3D vectors.
    ''' </summary>
    Public Shared Function Lerp(a As Vector3, b As Vector3, t As Single) As Vector3
        t = Math.Min(Math.Max(t, 0.0F), 1.0F)
        Return New Vector3(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t)
    End Function
    ''' <summary>
    ''' Linearly interpolation between two angles (in degrees), avoiding the "jump" between 359° and 0°.
    ''' </summary>
    Public Shared Function LerpAngle(a As Single, b As Single, t As Single) As Single
        t = Math.Min(Math.Max(t, 0.0F), 1.0F)
        Dim delta As Single = ((b - a + 540.0F) Mod 360.0F) - 180.0F
        Return (a + delta * t + 360.0F) Mod 360.0F
    End Function
    ''' <summary>
    ''' Returns real elapsed time in milliseconds, not affected by game time scale.
    ''' Uses Environment.TickCount (32-bit, wraps automatically).
    ''' </summary>
    Public Shared ReadOnly Property RealTimeMilliseconds As Integer
        Get
            Return Environment.TickCount
        End Get
    End Property
    ''' <summary>
    ''' Returns the duration of the last rendered frame in seconds,
    ''' scaled by the game time scale.
    ''' This value is intended for frame-based movement and interpolation,
    ''' not for timers or delays. Uses TIMESTEP native.
    ''' </summary>
    Public Shared ReadOnly Property TimeStep As Single
        Get
            Return [Call](Of Single)("TIMESTEP")
        End Get
    End Property
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
    ''' <summary>
    ''' Converts a string to a Keys enum safely.
    ''' </summary>
    ''' <returns>Keys.None if the conversion fails</returns>
    ''' <param name="Value">String (F1, D5 etc.) or Integer</param>
    Public Shared Function StringToKey(Value As String) As Keys
        If String.IsNullOrWhiteSpace(Value) Then Return Keys.None
        Try
            Return CType([Enum].Parse(GetType(Keys), Value, True), Keys)
        Catch
            Return Keys.None
        End Try
    End Function
    ''' <summary>
    ''' Uses the native function STRING_TO_INT. 
    ''' </summary>
    ''' <returns>Nothing (or zero) if string can't be converted.</returns>
    Public Shared Function StringToInteger(StringToConvert As String) As Integer
        Dim pointer As GTA.Native.Pointer = New GTA.Native.Pointer(GetType(Integer))
        Dim convertedSuccessfully As Boolean = [Call](Of Boolean)("STRING_TO_INT", StringToConvert, pointer)
        If convertedSuccessfully Then Return pointer.Value Else Return Nothing
    End Function
    '------------------------------------------------------------------------------------------------
    Private Shared _nextWaitId As Integer = 0
    Private Shared ReadOnly _waitStates As New Dictionary(Of Integer, Integer)
    Private Shared Function GetNextWaitID() As Integer
        _nextWaitId += 1
        If _nextWaitId = Integer.MaxValue Then _nextWaitId = 1
        Return _nextWaitId
    End Function
    Public Enum RoundMode
        ToEven = 0
        AwayFromZero = 1
        Ceiling = 3
        Floor = 4
    End Enum
    Public Shared Function RoundFloat(Value As Single, Optional Decimals As Integer = 2, Optional Mode As RoundMode = RoundMode.ToEven) As Single

        If Decimals < 0 OrElse Decimals > 15 Then
            Throw New ArgumentOutOfRangeException(NameOf(Decimals))
        End If

        Select Case Mode
            Case RoundMode.ToEven
                Return CSng(Math.Round(CDbl(Value), Decimals, MidpointRounding.ToEven))

            Case RoundMode.AwayFromZero
                Return CSng(Math.Round(CDbl(Value), Decimals, MidpointRounding.AwayFromZero))

            Case RoundMode.Ceiling
                Dim Factor As Double = Math.Pow(10, Decimals)
                Return CSng(Math.Ceiling(CDbl(Value) * Factor) / Factor)

            Case RoundMode.Floor
                Dim Factor As Double = Math.Pow(10, Decimals)
                Return CSng(Math.Floor(CDbl(Value) * Factor) / Factor)

            Case Else
                Return CSng(Math.Round(CDbl(Value), Decimals, MidpointRounding.ToEven))
        End Select
    End Function
    ''' Performs a non-blocking wait for the specified amount of time.
    ''' Must be called repeatedly (e.g. every Tick) until it returns True.
    ''' Safe for multiple concurrent waits across different scripts.
    ''' </summary>
    Public Shared Function Wait(Time As Integer, Optional RelatedToGameTime As Boolean = False, Optional waitId As Integer = 0) As Boolean
        If Time <= 0 Then Return True
        Dim nowMs As Integer = If(RelatedToGameTime, NativeTimer.TotalGameTime, Environment.TickCount)
        If waitId = 0 Then
            waitId = GetNextWaitID()
            _waitStates(waitId) = nowMs + Time
            Return False
        End If
        If Not _waitStates.ContainsKey(waitId) Then
            _waitStates(waitId) = nowMs + Time
            Return False
        End If
        If nowMs >= _waitStates(waitId) Then
            _waitStates.Remove(waitId)
            Return True
        End If
        Return False
    End Function
    ''' <summary>
    ''' Executes the specified action after a delay without blocking the caller.
    ''' Safe to call from non-Tick contexts.
    ''' </summary>
    Public Shared Sub WaitAndInvoke(Action As Action, timeMs As Integer, Optional gameTimeRelated As Boolean = False)
        If Action Is Nothing Then Return
        If timeMs < 0 Then timeMs = 0

        Dim startTime As Integer =
        If(gameTimeRelated,
           NativeTimer.TotalGameTime,
           Environment.TickCount)

        Dim targetTime As Integer = startTime + timeMs

        Dim tickAction As Action = Nothing

        tickAction = Sub()
                         Try
                             Dim nowMs As Integer = If(gameTimeRelated, NativeTimer.TotalGameTime, Environment.TickCount)
                             If nowMs >= targetTime Then
                                 TickHelper.Remove_Internal(tickAction)
                                 Action.Invoke()
                             End If
                         Catch ex As Exception
                             TickHelper.Remove_Internal(tickAction)
                             Tools.WriteLog("Error in Tools.WaitAndInvoke.", ex)
                         End Try
                     End Sub
        TickHelper.Add_Internal(tickAction)
    End Sub
    ''' <summary>
    ''' Performs a non-blocking wait until a condition becomes True
    ''' or an optional timeout expires.
    ''' Safe for multiple concurrent waits across scripts.
    ''' </summary>
    Public Shared Function WaitUntil(Condition As Func(Of Boolean), Optional TimeOut As Integer = 0, Optional RelatedToGameTime As Boolean = False, Optional waitId As Integer = 0) As Boolean
        If Condition Is Nothing Then Return True
        If Condition() Then
            If waitId <> 0 Then _waitStates.Remove(waitId)
            Return True
        End If

        If TimeOut <= 0 Then Return False
        Dim nowMs As Integer = If(RelatedToGameTime, NativeTimer.TotalGameTime, Environment.TickCount)

        If waitId = 0 Then
            waitId = GetNextWaitID()
            _waitStates(waitId) = nowMs + TimeOut
            Return False
        End If
        If Not _waitStates.ContainsKey(waitId) Then
            _waitStates(waitId) = nowMs + TimeOut
            Return False
        End If
        If nowMs >= _waitStates(waitId) Then
            _waitStates.Remove(waitId)
            Return True
        End If
        Return False
    End Function
    '------------------------------------------------------------------------------------------------------------------
    Friend Const DefaultLogPath As String = "NativeExtended-IV.log"
    Private Shared _hasWrittenWelcomeMessage As Boolean = False
    Private Shared _firstClean As Boolean = False
    Private Shared _isWriting As Boolean = False
    Friend Shared Sub WriteLog(Message As String, Optional Exception As Exception = Nothing, Optional WriteDateAndTime As Boolean = False)
        Try
            If _isWriting Then Return
            _isWriting = True

            If Not _hasWrittenWelcomeMessage Then
                TickHelper.WriteHeaderMessage()
                _firstClean = True
            End If

            Dim completeLogPath As String = GetFullLogPath("")

            Using sw As New StreamWriter(completeLogPath, False)
                If WriteDateAndTime AndAlso Exception Is Nothing Then
                    sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {Message}")
                Else
                    If Not WriteDateAndTime Then sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {Message}")
                    sw.WriteLine(Message)
                    If Exception IsNot Nothing Then
                        WriteExceptionRecursive(Exception, sw)
                        sw.WriteLine(New String("-"c, 80))
                    End If
                End If
            End Using

        Catch

        Finally
            _isWriting = False
        End Try
    End Sub
    Friend Shared Function GetFullLogPath(Optional CustomLogPath As String = "") As String
        Try
            If Not String.IsNullOrEmpty(CustomLogPath) AndAlso IO.Path.IsPathRooted(CustomLogPath) Then
                Return CustomLogPath
            End If

            Dim folder As String = GamePath
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
    '------------------------------------------------------------------------------------------------------------------
    Public NotInheritable Class LogFile
        Private ReadOnly _logPath As String
        Private ReadOnly _sync As New Object()
        Public ReadOnly Property Path As String
            Get
                Return _logPath
            End Get
        End Property
        Private _hasWrittenWelcomeMessage As Boolean = False
        Private _hasWrittenWelcomeMessageOnDefaultLog As Boolean = False
        Private _overwriteBeforeWelcomeMessage As Boolean = True
        Private _isWriting As Boolean = False
        Private _CustomHeaderLines As IEnumerable(Of String)
        Public Sub New(Path As String)
            If String.IsNullOrWhiteSpace(Path) Then
                _logPath = GamePath & "\" & "NativeExtended-IV.log"
            Else
                _logPath = GamePath & "\" & Path
            End If
        End Sub
        Public Sub SetHeaderMessage(Lines As IEnumerable(Of String), Optional OverwriteFile As Boolean = True)
            If Lines Is Nothing OrElse Not Lines.Any() Then Return
            _overwriteBeforeWelcomeMessage = OverwriteFile
            _CustomHeaderLines = Lines
        End Sub
        Public Sub WriteLog(Message As String, Optional WriteDateAndTime As Boolean = False, Optional OverwriteFile As Boolean = False)
            SyncLock _sync
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
            End SyncLock
        End Sub
        Public Sub WriteLog(Message As String, Exception As Exception)
            SyncLock _sync
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
                End Try
            End SyncLock
        End Sub
        '====================================================================================================================================================
        ' -------------------------------
        ' Operators & Override functions
        ' -------------------------------
        Public Overrides Function Equals(obj As Object) As Boolean
            If ReferenceEquals(Me, obj) Then Return True
            If obj Is Nothing OrElse obj.GetType() IsNot GetType(LogFile) Then Return False
            Dim other As LogFile = DirectCast(obj, LogFile)
            Return String.Equals(IO.Path.GetFullPath(Me._logPath), IO.Path.GetFullPath(other._logPath), StringComparison.OrdinalIgnoreCase)
        End Function
        Public Overrides Function GetHashCode() As Integer
            If String.IsNullOrEmpty(_logPath) Then Return 0
            Return IO.Path.GetFullPath(_logPath).ToLowerInvariant().GetHashCode()
        End Function
        Public Shared Operator =(left As LogFile, right As LogFile) As Boolean
            If ReferenceEquals(left, right) Then Return True
            If left Is Nothing OrElse right Is Nothing Then Return False
            Return left.Equals(right)
        End Operator
        Public Shared Operator <>(left As LogFile, right As LogFile) As Boolean
            Return Not (left = right)
        End Operator
        '====================================================================================================================================================
        ' -------------------------------
        ' Internal
        ' -------------------------------
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

                Dim folder As String = GamePath
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
                sw.WriteLine("| -- bttf4thebigRelease@gmail.com / lorenzo3024@hotmail.it  - |")
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
    ''' <summary>
    ''' Lightweight and standalone INI settings file manager.
    ''' Automatically loads the file on creation and saves changes immediately.
    ''' </summary>
    Public NotInheritable Class SettingsFile
        Private ReadOnly _sync As New Object()

        Private _fullPath As String = String.Empty
        Friend _log As LogFile = Nothing
        Private _path As String = String.Empty
        Private _fileName As String = String.Empty
        Private _data As New Dictionary(Of String, Dictionary(Of String, String))(StringComparer.OrdinalIgnoreCase)

        Friend ReadOnly Property FullPath As String
            Get
                Return _fullPath
            End Get
        End Property
        ''' <summary>
        ''' When enabled, missing or empty values accessed through Get functions
        ''' are automatically written to the settings file using the provided default value.
        ''' </summary>
        Public Property AutoWriteDefaults As Boolean = False
        ''' <summary>
        ''' Gets file directory path.
        ''' </summary>
        Public ReadOnly Property Path As String
            Get
                Return _path
            End Get
        End Property
        ''' <summary>
        ''' Gets file name.
        ''' </summary>
        Public ReadOnly Property File As String
            Get
                Return _fileName
            End Get
        End Property
        ''' <summary>
        ''' Optional custom log file used to report errors and exceptions.
        ''' If not set, the default log is used (NativeExtendedIV.log).
        ''' </summary>
        Friend Property LogFile As LogFile
            Get
                Return _log
            End Get
            Set(value As LogFile)
                _log = value
            End Set
        End Property
        ''' <summary>
        ''' Initializes a new INI settings file instance.
        ''' </summary>
        ''' <param name="FilePath">If path is not absolute, it will be based on NativeExtendedIV path (main game folder)</param>
        Public Sub New(FilePath As String, Optional CreateIfDoestExist As Boolean = True, Optional LogFile As LogFile = Nothing)
            If String.IsNullOrWhiteSpace(FilePath) Then
                LogError("Error in SettingsFile.Constructor: file path is missing.")
            End If

            Dim resolved As String = ResolvePathFromGameFolder(FilePath)

            _fullPath = resolved
            _path = IO.Path.GetDirectoryName(_fullPath)
            _fileName = IO.Path.GetFileName(_fullPath)
            _log = LogFile

            If String.IsNullOrWhiteSpace(_path) OrElse String.IsNullOrWhiteSpace(_fileName) Then
                LogError("Error in SettingsFile.Constructor: file path not valid.")
            End If

            EnsureFileExists(CreateIfDoestExist)
            Reload()
        End Sub
        Public Sub ClearAll()
            SyncLock _sync
                _data.Clear()
                SaveToDiskInternal()
            End SyncLock
        End Sub
        Public Sub ClearCategory(Category As String)
            If String.IsNullOrWhiteSpace(Category) Then Exit Sub

            SyncLock _sync
                If _data.Remove(Category) Then
                    SaveToDiskInternal()
                End If
            End SyncLock
        End Sub
        ''' <summary>
        ''' Reloads the settings file from disk.
        ''' </summary>
        Public Sub Reload()
            SyncLock _sync
                _data = New Dictionary(Of String, Dictionary(Of String, String))(StringComparer.OrdinalIgnoreCase)
                LoadFromDiskInternal()
            End SyncLock
        End Sub
        ''' <summary>
        ''' Completely removes a value from the specified category.
        ''' </summary>
        Public Sub RemoveValue(Name As String, Category As String)
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim section As Dictionary(Of String, String) = Nothing
                If _data.TryGetValue(Category, section) Then
                    If section.Remove(Name) Then
                        ' Se la sezione resta vuota, la puoi anche rimuovere (scelta utile per "pulizia")
                        If section.Count = 0 Then
                            _data.Remove(Category)
                        End If
                        SaveToDiskInternal()
                    End If
                End If
            End SyncLock
        End Sub
        ''' <returns>List of categories (file sections)</returns>
        Public Function GetCategories() As IEnumerable(Of String)
            SyncLock _sync
                Return _data.Keys.ToList()
            End SyncLock
        End Function
        ''' <returns>List of keys from file section</returns>
        Public Function GetKeys(Category As String) As IEnumerable(Of String)
            ValidateCategoriaNome(Category, "_")

            SyncLock _sync
                Dim section As Dictionary(Of String, String) = Nothing
                If _data.TryGetValue(Category, section) Then
                    Return section.Keys.ToList()
                End If
                Return Enumerable.Empty(Of String)()
            End SyncLock
        End Function
        ''' <summary>
        ''' Returns a string value from the specified category.
        ''' When AutoWriteDefaults is enabled, the default value is also written to the file.
        ''' </summary>
        ''' <returns>Default value if key is missing or invalid</returns>
        Public Function GetValueString(Name As String, Category As String, DefaultValue As String) As String
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim raw As String = GetRawValueInternal(Category, Name)
                If String.IsNullOrEmpty(raw) Then
                    If AutoWriteDefaults Then
                        SetRawValueInternal(Category, Name, DefaultValue)
                        SaveToDiskInternal()
                    End If
                    Return DefaultValue
                End If

                Return raw
            End SyncLock
        End Function
        ''' <summary>
        ''' Returns a boolean value from the specified category.
        ''' When AutoWriteDefaults is enabled, the default value is also written to the file.
        ''' </summary>
        ''' <returns>Default value if key is missing or invalid</returns>
        Public Function GetValueBool(Name As String, Category As String, DefaultValue As Boolean, Optional WriteDefaultValue As Boolean? = Nothing) As Boolean
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim raw As String = GetRawValueInternal(Category, Name)

                If Not String.IsNullOrEmpty(raw) Then
                    Dim b As Boolean
                    If Boolean.TryParse(raw, b) Then Return b

                    Select Case raw.Trim().ToLowerInvariant()
                        Case "1", "yes", "y", "on", "true" : Return True
                        Case "0", "no", "n", "off", "false" : Return False
                    End Select
                End If

                Dim shouldWriteDefault As Boolean =
            If(WriteDefaultValue.HasValue, WriteDefaultValue.Value, AutoWriteDefaults)

                If shouldWriteDefault Then
                    SetRawValueInternal(Category, Name, If(DefaultValue, "true", "false"))
                    SaveToDiskInternal()
                End If

                Return DefaultValue
            End SyncLock
        End Function
        ''' <summary>
        ''' Returns an integer value from the specified category.
        ''' When AutoWriteDefaults is enabled, the default value is also written to the file.
        ''' </summary>
        ''' <returns>Default value if key is missing or invalid</returns>
        Public Function GetValueInteger(Name As String, Category As String, DefaultValue As Integer, Optional WriteDefaultValue As Boolean? = Nothing) As Integer
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim raw As String = GetRawValueInternal(Category, Name)

                Dim i As Integer
                If Not String.IsNullOrEmpty(raw) AndAlso
           Integer.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, i) Then
                    Return i
                End If

                Dim shouldWriteDefault As Boolean =
            If(WriteDefaultValue.HasValue, WriteDefaultValue.Value, AutoWriteDefaults)

                If shouldWriteDefault Then
                    SetRawValueInternal(Category, Name, DefaultValue.ToString(CultureInfo.InvariantCulture))
                    SaveToDiskInternal()
                End If

                Return DefaultValue
            End SyncLock
        End Function
        ''' <summary>
        ''' Returns a float value from the specified category.
        ''' When AutoWriteDefaults is enabled, the default value is also written to the file.
        ''' </summary>
        ''' <returns>Default value if key is missing or invalid</returns>
        Public Function GetValueFloat(Name As String, Category As String, DefaultValue As Single, Optional WriteDefaultValue As Boolean? = Nothing) As Single
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim raw As String = GetRawValueInternal(Category, Name)

                Dim s As Single
                If Not String.IsNullOrEmpty(raw) Then
                    If Single.TryParse(raw, NumberStyles.Float Or NumberStyles.AllowThousands, CultureInfo.InvariantCulture, s) _
               OrElse Single.TryParse(raw.Replace(","c, "."c),
                                      NumberStyles.Float Or NumberStyles.AllowThousands,
                                      CultureInfo.InvariantCulture, s) Then
                        Return s
                    End If
                End If

                Dim shouldWriteDefault As Boolean = If(WriteDefaultValue.HasValue, WriteDefaultValue.Value, AutoWriteDefaults)

                If shouldWriteDefault Then
                    SetRawValueInternal(Category, Name, DefaultValue.ToString(CultureInfo.InvariantCulture))
                    SaveToDiskInternal()
                End If

                Return DefaultValue
            End SyncLock
        End Function
        ''' <summary>
        ''' Returns a keyboard key value from the specified category.
        ''' When AutoWriteDefaults is enabled, the default value is also written to the file.
        ''' </summary>
        ''' <returns>Default value if key is missing or invalid</returns>
        Public Function GetValueKey(Name As String, Category As String, DefaultValue As Keys, Optional WriteDefaultValue As Boolean? = Nothing) As Keys
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim raw As String = GetRawValueInternal(Category, Name)

                If Not String.IsNullOrEmpty(raw) Then
                    Try
                        Return CType([Enum].Parse(GetType(Keys), raw, True), Keys)
                    Catch
                    End Try

                    Dim num As Integer
                    If Integer.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, num) Then
                        Return CType(num, Keys)
                    End If
                End If

                Dim shouldWriteDefault As Boolean = If(WriteDefaultValue.HasValue, WriteDefaultValue.Value, AutoWriteDefaults)

                If shouldWriteDefault Then
                    SetRawValueInternal(Category, Name, DefaultValue.ToString())
                    SaveToDiskInternal()
                End If

                Return DefaultValue
            End SyncLock
        End Function
        ''' <summary>
        ''' Returns a 64-bit integer value from the specified category.
        ''' When AutoWriteDefaults is enabled, the default value is also written to the file.
        ''' </summary>
        ''' <returns>Default value if key is missing or invalid</returns>
        Public Function GetValueLong(Name As String, Category As String, DefaultValue As Long, Optional WriteDefaultValue As Boolean? = Nothing) As Long
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim raw As String = GetRawValueInternal(Category, Name)

                Dim v As Long
                If Not String.IsNullOrEmpty(raw) AndAlso Long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, v) Then
                    Return v
                End If

                Dim shouldWriteDefault As Boolean = If(WriteDefaultValue.HasValue, WriteDefaultValue.Value, AutoWriteDefaults)
                If shouldWriteDefault Then
                    SetRawValueInternal(Category, Name, DefaultValue.ToString(CultureInfo.InvariantCulture))
                    SaveToDiskInternal()
                End If

                Return DefaultValue
            End SyncLock
        End Function

        ''' <summary>
        ''' Returns a double-precision floating-point value from the specified category.
        ''' When AutoWriteDefaults is enabled, the default value is also written to the file.
        ''' </summary>
        ''' <returns>Default value if key is missing or invalid</returns>
        Public Function GetValueDouble(Name As String, Category As String, DefaultValue As Double, Optional WriteDefaultValue As Boolean? = Nothing) As Double
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim raw As String = GetRawValueInternal(Category, Name)

                Dim d As Double
                If Not String.IsNullOrEmpty(raw) Then
                    If Double.TryParse(raw, NumberStyles.Float Or NumberStyles.AllowThousands, CultureInfo.InvariantCulture, d) _
                       OrElse Double.TryParse(raw.Replace(","c, "."c),
                                      NumberStyles.Float Or NumberStyles.AllowThousands,
                                      CultureInfo.InvariantCulture, d) Then
                        Return d
                    End If
                End If

                Dim shouldWriteDefault As Boolean = If(WriteDefaultValue.HasValue, WriteDefaultValue.Value, AutoWriteDefaults)
                If shouldWriteDefault Then
                    SetRawValueInternal(Category, Name, DefaultValue.ToString(CultureInfo.InvariantCulture))
                    SaveToDiskInternal()
                End If

                Return DefaultValue
            End SyncLock
        End Function
        ''' <summary>
        ''' Returns a list of all key/value pairs from a specific INI section.
        ''' </summary>
        Public Function GetValues_FromSection(Category As String) As Dictionary(Of String, String)

            Dim result As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            Try
                Dim currentSection As String = ""
                Dim targetSection As String = Category.Trim()

                For Each rawLine In IO.File.ReadAllLines(FullPath)
                    Dim line As String = rawLine.Trim()

                    ' skip empty or comments
                    If String.IsNullOrWhiteSpace(line) Then Continue For
                    If line.StartsWith(";") OrElse line.StartsWith("#") Then Continue For

                    ' section
                    If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                        currentSection = line.Substring(1, line.Length - 2).Trim()
                        Continue For
                    End If

                    ' wrong section
                    If Not currentSection.Equals(targetSection, StringComparison.OrdinalIgnoreCase) Then
                        Continue For
                    End If

                    ' key=value
                    Dim idx As Integer = line.IndexOf("="c)
                    If idx <= 0 Then Continue For

                    Dim key As String = line.Substring(0, idx).Trim()
                    Dim value As String = line.Substring(idx + 1).Trim()

                    If Not result.ContainsKey(key) Then
                        result.Add(key, value)
                    End If
                Next
            Catch ex As Exception
                LogError($"Error in SettingsFile.GetValues_FromSection: category '{Category}'.", ex)
            End Try

            Return result
        End Function
        Public Function HasValue(Name As String, Category As String) As Boolean
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                Dim section As Dictionary(Of String, String) = Nothing
                Return _data.TryGetValue(Category, section) AndAlso section.ContainsKey(Name)
            End SyncLock
        End Function
        Public Sub SetValue(Name As String, Category As String, Value As String)
            ValidateCategoriaNome(Category, Name)

            If Value Is Nothing Then Value = String.Empty

            SyncLock _sync
                SetRawValueInternal(Category, Name, Value)
                If Not _batchMode Then SaveToDiskInternal() 'SaveToDiskInternal()
            End SyncLock
        End Sub
        Public Sub SetValue(Name As String, Category As String, Value As Boolean)
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                SetRawValueInternal(Category, Name, If(Value, "True", "False"))
                If Not _batchMode Then SaveToDiskInternal() '    SaveToDiskInternal()
            End SyncLock
        End Sub
        Public Sub SetValue(Name As String, Category As String, Value As Integer)
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                SetRawValueInternal(Category, Name, Value.ToString(CultureInfo.InvariantCulture))
                If Not _batchMode Then SaveToDiskInternal() '     SaveToDiskInternal()
            End SyncLock
        End Sub
        Public Sub SetValue(Name As String, Category As String, Value As Single)
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                SetRawValueInternal(Category, Name, Value.ToString(CultureInfo.InvariantCulture))
                If Not _batchMode Then SaveToDiskInternal() ' SaveToDiskInternal()
            End SyncLock
        End Sub
        Public Sub SetValue(Name As String, Category As String, Value As Keys)
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                SetRawValueInternal(Category, Name, Value.ToString())
                If Not _batchMode Then SaveToDiskInternal() '  SaveToDiskInternal()
            End SyncLock
        End Sub
        Public Sub SetValue(Name As String, Category As String, Value As Long)
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                SetRawValueInternal(Category, Name, Value.ToString(CultureInfo.InvariantCulture))
                If Not _batchMode Then SaveToDiskInternal() '   SaveToDiskInternal()
            End SyncLock
        End Sub
        Public Sub SetValue(Name As String, Category As String, Value As Double)
            ValidateCategoriaNome(Category, Name)

            SyncLock _sync
                SetRawValueInternal(Category, Name, Value.ToString(CultureInfo.InvariantCulture))
                If Not _batchMode Then SaveToDiskInternal() '    SaveToDiskInternal()
            End SyncLock
        End Sub
        '====================================================================================================================================================
        ' -------------------------------
        ' Operators & Override functions
        ' -------------------------------
        Public Shared Operator =(left As SettingsFile, right As SettingsFile) As Boolean
            If ReferenceEquals(left, right) Then Return True
            If left Is Nothing OrElse right Is Nothing Then Return False
            Return left.Equals(right)
        End Operator
        Public Shared Operator <>(left As SettingsFile, right As SettingsFile) As Boolean
            Return Not (left = right)
        End Operator
        Public Overrides Function Equals(obj As Object) As Boolean
            If ReferenceEquals(Me, obj) Then Return True
            If obj Is Nothing OrElse obj.GetType() IsNot GetType(SettingsFile) Then Return False
            Dim other As SettingsFile = DirectCast(obj, SettingsFile)
            Return String.Equals(IO.Path.GetFullPath(Me._fullPath), IO.Path.GetFullPath(other._fullPath), StringComparison.OrdinalIgnoreCase)
        End Function
        Public Overrides Function GetHashCode() As Integer
            If String.IsNullOrEmpty(_fullPath) Then Return 0
            Return IO.Path.GetFullPath(_fullPath).ToLowerInvariant().GetHashCode()
        End Function
        '====================================================================================================================================================
        ' -------------------------
        ' Internals
        ' -------------------------
        Private _batchMode As Boolean
        Private Sub ValidateCategoriaNome(categoria As String, nome As String)
            If String.IsNullOrWhiteSpace(categoria) Then
                Throw New ArgumentException("La categoria è obbligatoria.", NameOf(categoria))
            End If
            If String.IsNullOrWhiteSpace(nome) Then
                Throw New ArgumentException("Il nome del valore è obbligatorio.", NameOf(nome))
            End If
        End Sub
        Private Function ResolvePathFromGameFolder(inputPath As String) As String
            If IO.Path.IsPathRooted(inputPath) Then
                Return IO.Path.GetFullPath(inputPath)
            End If
            Dim basePath As String = Tools.GamePath

            If String.IsNullOrWhiteSpace(basePath) Then
                '  Throw New InvalidOperationException("Tools.GamePath non è valorizzato.")
            End If

            Return IO.Path.GetFullPath(IO.Path.Combine(basePath, inputPath))
        End Function
        Private Sub EnsureFileExists(createifnotexists As Boolean)
            Try
                If Not Directory.Exists(_path) Then
                    Directory.CreateDirectory(_path)
                End If

                If Not IO.File.Exists(_fullPath) Then
                    If createifnotexists Then
                        Using fs = New FileStream(_fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.Read)
                        End Using
                    End If
                End If
            Catch ex As Exception
                LogError($"Error in SettingsFile.EnsureFileExists: cannot create or initialize settings file '{_fileName}'.", ex)
            End Try
        End Sub
        Private Sub LoadFromDiskInternal()
            Dim currentSection As String = "DEFAULT"

            If Not _data.ContainsKey(currentSection) Then
                _data(currentSection) = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            End If

            Dim lines() As String
            Try
                lines = IO.File.ReadAllLines(_fullPath, Encoding.UTF8)
            Catch ex As Exception
                LogError("Error in SettingsFile.LoadFromDiskInternal: cannot read settings file.", ex)
                Throw
            End Try

            For Each line In lines
                Dim t As String = line.Trim()

                If t.Length = 0 Then Continue For
                If t.StartsWith(";"c) OrElse t.StartsWith("#"c) Then Continue For

                ' Sezione: [Section]
                If t.StartsWith("["c) AndAlso t.EndsWith("]"c) AndAlso t.Length >= 2 Then
                    currentSection = t.Substring(1, t.Length - 2).Trim()
                    If currentSection.Length = 0 Then currentSection = "DEFAULT"

                    If Not _data.ContainsKey(currentSection) Then
                        _data(currentSection) = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
                    End If
                    Continue For
                End If

                ' Key=Value
                Dim idx As Integer = t.IndexOf("="c)
                If idx <= 0 Then Continue For

                Dim key As String = t.Substring(0, idx).Trim()
                Dim value As String = t.Substring(idx + 1).Trim()

                If key.Length = 0 Then Continue For

                Dim sectionDict = _data(currentSection)
                sectionDict(key) = value
            Next
        End Sub
        Private Sub SaveToDiskInternal()
            Try
                Dim sb As New StringBuilder(4096)
                If _data.ContainsKey("DEFAULT") Then
                    WriteSection(sb, "DEFAULT", _data("DEFAULT"), writeHeader:=False)
                End If

                For Each sectionName In _data.Keys
                    If sectionName.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase) Then Continue For
                    sb.AppendLine()
                    WriteSection(sb, sectionName, _data(sectionName), writeHeader:=True)
                Next

                IO.File.WriteAllText(_fullPath, sb.ToString(), Encoding.UTF8)
            Catch ex As Exception
                LogError("Error in SettingsFile.SaveToDiskInternal: cannot save settings file.", ex)
            End Try
        End Sub
        Private Sub WriteSection(sb As StringBuilder, sectionName As String, section As Dictionary(Of String, String), writeHeader As Boolean)
            If writeHeader Then
                sb.Append("["c).Append(sectionName).AppendLine("]"c)
            End If

            ' Ordinamento stabile per file più “pulito”
            Dim keys As New List(Of String)(section.Keys)
            keys.Sort(StringComparer.OrdinalIgnoreCase)

            For Each k In keys
                sb.Append(k).Append("="c).AppendLine(section(k))
            Next
        End Sub
        Private Function GetRawValueInternal(section As String, key As String) As String
            Dim sec As Dictionary(Of String, String) = Nothing
            If _data.TryGetValue(section, sec) Then
                Dim v As String = Nothing
                If sec.TryGetValue(key, v) Then Return v
            End If
            Return Nothing
        End Function
        Private Sub SetRawValueInternal(section As String, key As String, value As String)
            Dim sec As Dictionary(Of String, String) = Nothing
            If Not _data.TryGetValue(section, sec) Then
                sec = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
                _data(section) = sec
            End If
            sec(key) = value
        End Sub
        Private Sub LogError(Message As String, Optional ex As Exception = Nothing)
            If LogFile IsNot Nothing Then
                If ex IsNot Nothing Then
                    LogFile.WriteLog(Message, ex)
                Else
                    LogFile.WriteLog(Message, True)
                End If
            Else
                WriteLog(Message)
            End If
        End Sub
        Friend Sub BeginBatch()
            _batchMode = True
        End Sub
        Friend Sub EndBatch()
            _batchMode = False
            SaveToDiskInternal()
        End Sub
    End Class
    '------------------------------------------------------------------------------------------------------------------
End Class
