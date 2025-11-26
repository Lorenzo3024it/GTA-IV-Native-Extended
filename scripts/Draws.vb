Imports GTA
Imports GTA.Native.Function
Imports NativeExtended.Tools

Public Class NativeDraws
    '--------------------------------------------------------------------------------------------------------
    'Brief / News Scrollbar
    '-----------------------
    Public Shared Sub AddTextToBrief(Text As String, Optional Underscore As Boolean = False)
        If Underscore = False Then [Call]("ADD_TO_PREVIOUS_BRIEF", Text) Else [Call]("ADD_TO_PREVIOUS_BRIEF_WITH_UNDERSCORE", Text)
    End Sub
    Public Shared Sub AddTextToNewsScrollbar(Text As String)
        [Call]("ADD_STRING_TO_NEWS_SCROLLBAR", Text)
    End Sub
    Public Shared Sub ClearBrief()
        [Call]("CLEAR_BRIEF")
    End Sub
    Public Shared Sub ClearNewsScrollbar()
        [Call]("CLEAR_NEWS_SCROLLBAR")
    End Sub
    '--------------------------------------------------------------------------------------------------------
    'Custom Texts
    '-------------
    ''' <summary>
    ''' Display a custom message (bottom-center of screen, like a subtitle).
    ''' </summary>
    ''' <param name="BriefDisplay">If False the message won't be added to Briefing</param>
    ''' <param name="DisplayNow">If False the message will be added to a queue and displayed only if there aren't other messages on screen</param>
    ''' <param name="UnknownFlag">Set 1 by default (True). Not sure about this, I always used 1</param>
    Public Shared Sub DisplayText(Text As String, Optional Duration As Integer = 3000, Optional BriefDisplay As Boolean = False,
                                  Optional DisplayNow As Boolean = True, Optional UnknownFlag As Boolean = True)
        Dim flag As Integer
        If UnknownFlag Then flag = 1 Else flag = 0
        If Duration < 5 Then Duration = 5
        [Call]("ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS", BriefDisplay)
        If DisplayNow Then
            [Call]("PRINT_STRING_WITH_LITERAL_STRING_NOW", "STRING", Text, Duration, flag)
        Else
            [Call]("PRINT_STRING_WITH_LITERAL_STRING", "STRING", Text, Duration, flag)
        End If
    End Sub
    ''' <summary>
    ''' Display a text with custom style and position on screen.
    ''' You must call TickHelper.ProcessAll in a Tick to set the display duration.
    ''' Otherwise you can set Duration = 1 and call directly in you main Tick.
    '''</summary>
    Public Shared Sub DisplayTextAdvanced(Text As String, Optional Duration As Integer = 3000, Optional PositionPercent As Vector2 = Nothing, Optional Style As TextStyle = Nothing)
        Dim FinalStyle As TextStyle
        If Style = Nothing Then
            FinalStyle = TextStyle.Default
        Else
            FinalStyle = Style
        End If
        If PositionPercent = Nothing Then PositionPercent = New Vector2(0.0, 0.0)
        If Duration < 5 Then
            TextStyle.SetTextStyle(FinalStyle)
            [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", PositionPercent.X, PositionPercent.Y, "STRING", Text)
        Else
            DrawManager.DisplayTextAdvanced(Text, Duration, PositionPercent, FinalStyle)
        End If
    End Sub
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Untested
    '''</summary>
    Public Shared Sub DrawCylinder(Coordinates As GTA.Vector3, Unknown01 As Single, Unknown02 As Single, Color As ColorRGBA)
        [Call]("DRAW_COLOURED_CYLINDER", Coordinates.X, Coordinates.Y, Coordinates.Z, Unknown01, Unknown02, Color.Red, Color.Green, Color.Blue, Color.Alpha)
    End Sub
    ''' <summary>
    '''Draws a checkpoint. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawCheckpoint(Coordinates As GTA.Vector3, Radius As Single, Color As ColorRGBA)
        If Color.Alpha = 255 Then
            [Call]("DRAW_CHECKPOINT", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Color.Red, Color.Green, Color.Blue)
        Else
            [Call]("DRAW_CHECKPOINT_WITH_ALPHA", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        End If
    End Sub
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Draws a curved window. You must call TickHelper.ProcessAll in a Tick to set the display duration.
    ''' Otherwise you can set Duration = 1 and call directly in you main Tick.
    '''</summary>
    Public Shared Sub DrawCurvedWindow(PercentagePos As Vector2, PercentageSize As Vector2, Alpha As Byte, Optional Duration As Integer = 1)
        If Duration < 5 Then
            [Call]("DRAW_CURVED_WINDOW", PercentagePos.X, PercentagePos.Y, PercentageSize.X, PercentageSize.Y, CInt(Alpha))
        Else
            DrawCurvedWindow(PercentagePos, PercentageSize, Alpha, Duration)
        End If
    End Sub
    ''' <summary>
    ''' Draws a rectangle. You must call TickHelper.ProcessAll in a Tick to set the display duration.
    ''' Otherwise you can set Duration = 1 and call directly in you main Tick.
    '''</summary>
    Public Shared Sub DrawRectangle(PercentagePos As Vector2, PercentageSize As Vector2, Color As ColorRGBA, Optional Duration As Integer = 1, Optional CenteredX As Boolean = True, Optional CenteredY As Boolean = True)

        Dim posX As Single = PercentagePos.X
        Dim posY As Single = PercentagePos.Y
        Dim sizeX As Single = PercentageSize.X
        Dim sizeY As Single = PercentageSize.Y

        If CenteredX = False Then
            posX = PercentagePos.X + (sizeX / 2.0F)
        End If
        If CenteredY = False Then
            posY = PercentagePos.Y + (sizeY / 2.0F)
        End If

        If Duration < 5 Then
            [Call]("DRAW_RECT", posX, posY, sizeX, sizeY,
               Color.Red, Color.Green, Color.Blue, Color.Alpha)
        Else
            DrawManager.DrawRect(New Vector2(posX, posY), PercentageSize, Color, Duration)
        End If
    End Sub

    ''' <summary>
    ''' Draws the game screen. You must call TickHelper.ProcessAll in a Tick to set the display duration.
    ''' Otherwise you can set Duration = 1 and call directly in you main Tick.
    '''</summary>
    Public Shared Sub DrawSpriteFrontBuff(PercentagePos As Vector2, PercentageSize As Vector2, RotationInDegrees As Single, Color As ColorRGBA, Optional Duration As Integer = 1)

        If Duration < 5 Then
            [Call]("DRAW_SPRITE_FRONT_BUFF", PercentagePos.X, PercentagePos.Y, PercentageSize.X, PercentageSize.Y, RotationInDegrees, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        Else
            DrawManager.DrawFrontBuff(PercentagePos, PercentageSize, RotationInDegrees, Color, Duration)
        End If
    End Sub
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Lights and Coronas
    '-------------------
    ''' <summary>
    '''Draws a corona fx. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawCorona(Position As GTA.Vector3, Radius As Single, Color As ColorRGB, Optional Intensity_0_To_100 As Integer = 10)
        Dim R, G, B As Integer
        R = Color.Red * (Intensity_0_To_100 / 100)
        G = Color.Green * (Intensity_0_To_100 / 100)
        B = Color.Blue * (Intensity_0_To_100 / 100)
        [Call]("DRAW_CORONA", Position.X, Position.Y, Position.Z, Radius, 0, 0.0F, R, G, B)
    End Sub
    ''' <summary>
    '''Draws a light with range. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawLight(Position As GTA.Vector3, Color As ColorRGB, Intensity As Single, Range As Single)
        [Call]("DRAW_LIGHT_WITH_RANGE", Position.X, Position.Y, Position.Z, Color.Red, Color.Green, Color.Blue, Intensity, Range)
    End Sub
    ''' <summary>
    ''' Two combined functions to easily draw a corona with proper light. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawLightWithCorona(Position As GTA.Vector3, Color As ColorRGB, LightIntensity As Single, LightRange As Single, CoronaRadius As Single, Optional CoronaIntensity_0_To_100 As Integer = 10)
        Dim R, G, B As Integer
        R = Color.Red * (CoronaIntensity_0_To_100 / 100)
        G = Color.Green * (CoronaIntensity_0_To_100 / 100)
        B = Color.Blue * (CoronaIntensity_0_To_100 / 100)
        [Call]("DRAW_CORONA", Position.X, Position.Y, Position.Z, CoronaRadius, 0, 0.0F, R, G, B)
        [Call](Of Integer)("DRAW_LIGHT_WITH_RANGE", Position.X, Position.Y, Position.Z, Color.Red, Color.Green, Color.Blue, LightIntensity, LightRange)
    End Sub


    '''' <summary>
    '''' Draws a customizable HUD progress bar, with background, text and screen anchoring.
    '''' </summary>
    'Public Class ProgressBarHUD

    '    Public Enum ScreenAnchor
    '        Left
    '        Right
    '        ' Top
    '        ' Bottom
    '    End Enum

    '    '========================
    '    '   PROPERTIES
    '    '========================
    '    Public Property Anchor As ScreenAnchor
    '    Public Property Position As Vector2        ' in percentuale (0–1)
    '    Public Property Size As Vector2            ' in percentuale (0–1)
    '    Public Property Color As ColorRGBA
    '    Public Property BackColor As ColorRGBA
    '    Public Property Text As String
    '    Public Property TextOffsetX As Single = 0.02F
    '    Public Property Percentage As Single        ' 0.0 – 1.0
    '    Public Property Alpha As Integer = 180

    '    '========================
    '    '   COSTRUTTORE
    '    '========================
    '    Public Sub New(anchor As ScreenAnchor, pos As Vector2, size As Vector2, color As ColorRGBA)

    '        Me.Anchor = anchor
    '        Me.Position = pos
    '        Me.Size = size
    '        Me.Color = color

    '        ' background leggermente più scuro
    '        Me.BackColor = New ColorRGBA(color.Red \ 2, color.Green \ 2, color.Blue \ 2, 180)

    '        Me.Percentage = 1.0F
    '        Me.Text = ""
    '    End Sub

    '    '========================
    '    '   DRAW
    '    '========================
    '    Public Sub Draw(Optional Duration As Integer = 1)

    '        '-------------------------------------
    '        ' 1) DRAW FINESTRA SFONDO (Curved Window)
    '        '-------------------------------------
    '        If Duration < 5 Then
    '            [Call]("DRAW_CURVED_WINDOW",
    '               Position.X,
    '               Position.Y,
    '               Size.X,
    '               Size.Y,
    '               Alpha)
    '        Else
    '            DrawCurvedWindow(Position, Size, Alpha, Duration)
    '        End If

    '        '-------------------------------------
    '        ' 2) DRAW BAR BACKGROUND (rettangolo scuro)
    '        '-------------------------------------
    '        Dim barPos As Vector2 = Position
    '        Dim barSize As Vector2 = New Vector2(Size.X * 0.75F, Size.Y * 0.55F)

    '        Select Case Anchor
    '            Case ScreenAnchor.Left
    '            ' tutto normale
    '            Case ScreenAnchor.Right
    '                ' inverti la posizione per ancorare a destra
    '                barPos.X = 1.0F - Position.X
    '        End Select

    '        DrawRectangle(barPos, barSize, BackColor, Duration, False)

    '        '-------------------------------------
    '        ' 3) DRAW FILL BAR (rettangolo pieno)
    '        '-------------------------------------
    '        Dim fillSize As New Vector2(barSize.X * Percentage, barSize.Y)
    '        DrawRectangle(barPos, fillSize, Color, Duration, False)

    '        '-------------------------------------
    '        ' 4) DRAW TEXT (a destra o sinistra)
    '        '-------------------------------------
    '        If Text <> "" Then
    '            Dim textX As Single

    '            If Anchor = ScreenAnchor.Left Then
    '                textX = barPos.X + barSize.X + TextOffsetX
    '            Else
    '                textX = barPos.X - TextOffsetX
    '            End If

    '            '  [Call]("PRINT_STRING_WITH_LITERAL_STRING", "STRING", Text, 700, textX, Position.Y)
    '        End If

    '    End Sub

    'End Class

    '-------------------------------------------------------------------------------------------------------
    Public Structure ColorRGBA
        Private _r As Integer
        Private _g As Integer
        Private _b As Integer
        Private _a As Integer
        Public Property Red As Byte
            Get
                Return CByte(ClampInteger(_r, 0, 255))
            End Get
            Set(value As Byte)
                _r = value
            End Set
        End Property
        Public Property Green As Byte
            Get
                Return CByte(ClampInteger(_g, 0, 255))
            End Get
            Set(value As Byte)
                _g = value
            End Set
        End Property
        Public Property Blue As Byte
            Get
                Return CByte(ClampInteger(_b, 0, 255))
            End Get
            Set(value As Byte)
                _b = value
            End Set
        End Property
        Public Property Alpha As Byte
            Get
                Return CByte(ClampInteger(_a, 0, 255))
            End Get
            Set(value As Byte)
                _a = value
            End Set
        End Property
        Public Shared ReadOnly Property Black As ColorRGBA
            Get
                Return New ColorRGBA(0, 0, 0, 255)
            End Get
        End Property
        Public Shared ReadOnly Property DefaultColor As ColorRGBA
            Get
                Return New ColorRGBA(0, 0, 0, 120)
            End Get
        End Property
        Public Shared ReadOnly Property White As ColorRGBA
            Get
                Return New ColorRGBA(255, 255, 255, 255)
            End Get
        End Property
        Public Sub New(R As Byte, G As Byte, B As Byte, A As Byte)
            _r = R
            _g = G
            _b = B
            _a = A
        End Sub
        Public Overrides Function ToString() As String
            Return $"Color RGBA: R={Red}, G={Green}, B={Blue}, A={Alpha}"
        End Function
        Public Function ToHex() As Integer
            Return (_a << 24) Or (_r << 16) Or (_g << 8) Or _b
        End Function
        Public Overrides Function Equals(obj As Object) As Boolean
            If Not TypeOf obj Is ColorRGBA Then Return False
            Dim c As ColorRGBA = CType(obj, ColorRGBA)
            Return Red = c.Red AndAlso Green = c.Green AndAlso Blue = c.Blue AndAlso Alpha = c.Alpha
        End Function

        Public Shared Operator =(Color1 As ColorRGBA, Color2 As ColorRGBA) As Boolean
            Return Color1.Red = Color2.Red AndAlso Color1.Green = Color2.Green AndAlso Color1.Blue = Color2.Blue AndAlso Color1.Alpha = Color2.Alpha
        End Operator

        Public Shared Operator <>(Color1 As ColorRGBA, Color2 As ColorRGBA) As Boolean
            Return Not (Color1 = Color2)
        End Operator
        Public Shared Function FromHex(ARGB As Integer) As ColorRGBA
            Dim a As Byte = CByte((ARGB >> 24) And &HFF)
            Dim r As Byte = CByte((ARGB >> 16) And &HFF)
            Dim g As Byte = CByte((ARGB >> 8) And &HFF)
            Dim b As Byte = CByte(ARGB And &HFF)
            Return New ColorRGBA(r, g, b, a)
        End Function
    End Structure
    Public Structure ColorRGB
        Private _r As Integer
        Private _g As Integer
        Private _b As Integer
        Private _a As Integer
        Public Property Red As Byte
            Get
                Return CByte(ClampInteger(_r, 0, 255))
            End Get
            Set(value As Byte)
                _r = value
            End Set
        End Property
        Public Property Green As Byte
            Get
                Return CByte(ClampInteger(_g, 0, 255))
            End Get
            Set(value As Byte)
                _g = value
            End Set
        End Property
        Public Property Blue As Byte
            Get
                Return CByte(ClampInteger(_b, 0, 255))
            End Get
            Set(value As Byte)
                _b = value
            End Set
        End Property
        Public Shared ReadOnly Property Black As ColorRGB
            Get
                Return New ColorRGB(0, 0, 0)
            End Get
        End Property
        Public Shared ReadOnly Property White As ColorRGB
            Get
                Return New ColorRGB(255, 255, 255)
            End Get
        End Property
        Public Sub New(R As Byte, G As Byte, B As Byte)
            _r = R
            _g = G
            _b = B
            _a = 255
        End Sub
        Public Overrides Function ToString() As String
            Return $"Color RGBA: R={Red}, G={Green}, B={Blue}"
        End Function
        Public Function ToHex() As Integer
            Return (_a << 24) Or (_r << 16) Or (_g << 8) Or _b
        End Function
        Public Shared Function FromHext(ARGB As Integer) As ColorRGBA
            Dim a As Byte = CByte((ARGB >> 24) And &HFF)
            Dim r As Byte = CByte((ARGB >> 16) And &HFF)
            Dim g As Byte = CByte((ARGB >> 8) And &HFF)
            Dim b As Byte = CByte(ARGB And &HFF)
            Return New ColorRGBA(r, g, b, a)
        End Function
        Public Overrides Function Equals(obj As Object) As Boolean
            If Not TypeOf obj Is ColorRGBA Then Return False
            Dim c As ColorRGB = CType(obj, ColorRGB)
            Return Red = c.Red AndAlso Green = c.Green AndAlso Blue = c.Blue
        End Function

        Public Shared Operator =(Color1 As ColorRGB, Color2 As ColorRGB) As Boolean
            Return Color1.Red = Color2.Red AndAlso Color1.Green = Color2.Green AndAlso Color1.Blue = Color2.Blue
        End Operator

        Public Shared Operator <>(Color1 As ColorRGB, Color2 As ColorRGB) As Boolean
            Return Not (Color1 = Color2)
        End Operator
    End Structure
    '-------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Provides manual fade-in/out control without native functions.
    ''' Automatically hooks into TickHelper and supports color and minimum/maximum alpha.
    ''' </summary>
    Public Class CustomFades
        Private Enum FadeDirection
            None
            Inward
            Outward
        End Enum

        Private Shared fadeState As FadeDirection = FadeDirection.None
        Private Shared fadeStart As DateTime
        Private Shared fadeDuration As Integer
        Private Shared holdDuration As Integer
        Private Shared currentAlpha As Integer = 0
        Private Shared startAlpha As Integer = 0
        Private Shared endAlpha As Integer = 0
        Private Shared fadeColor As ColorRGB = New ColorRGB(0, 0, 0)
        Private Shared registeredInTick As Boolean = False
        Private Shared isHolding As Boolean = False
        Private Shared holdStartTime As DateTime
        Private Shared isPauseMenuDisabled As Boolean = False
        Private Shared shouldReenablePauseMenu As Boolean = False
        ' Store last used alpha limits for reference
        Private Shared lastMinAlpha As Integer = 0
        Private Shared lastMaxAlpha As Integer = 255

        ''' <summary>
        ''' Returns True if a fade effect (or hold) is currently active.
        ''' </summary>
        Public Shared ReadOnly Property IsFading As Boolean
            Get
                Return fadeState <> FadeDirection.None OrElse isHolding
            End Get
        End Property

        ''' <summary>
        ''' Returns True if the screen is fully visible (no fade, alpha = minAlpha).
        ''' </summary>
        Public Shared ReadOnly Property IsFadedInCompletely As Boolean
            Get
                Return Not IsFading AndAlso currentAlpha <= lastMinAlpha
            End Get
        End Property

        ''' <summary>
        ''' Returns True if the screen is fully darkened (no fade, alpha = maxAlpha).
        ''' </summary>
        Public Shared ReadOnly Property IsFadedOutCompletely As Boolean
            Get
                Return Not IsFading AndAlso currentAlpha >= lastMaxAlpha
            End Get
        End Property

        ''' <summary>
        ''' Returns current fade opacity (0–255).
        ''' </summary>
        Public Shared ReadOnly Property Opacity As Integer
            Get
                Return currentAlpha
            End Get
        End Property

        ''' <summary>
        ''' Starts a fade-in (from maxAlpha → minAlpha).
        ''' Duration in ms. If Duration = 0 it applies instantly and remains (drawn each frame) until CancelAll() or another fade.
        ''' Color: ColorRGB (if Nothing -> Black).
        ''' MinAlpha/MaxAlpha: limit range 0-255 (min = visible, max = opaque).
        ''' </summary>
        Public Shared Sub FadeIn(Optional Duration As Integer = 2000, Optional ByVal Color As ColorRGB = Nothing, Optional MinAlpha As Integer = 0, Optional MaxAlpha As Integer = 255)

            If Color = Nothing Then
                fadeColor = New ColorRGB(0, 0, 0)
            Else
                fadeColor = Color
            End If

            fadeState = FadeDirection.Inward
            fadeStart = DateTime.Now
            fadeDuration = Math.Max(0, Duration)
            startAlpha = Tools.ClampInteger(MaxAlpha, 0, 255)
            endAlpha = Tools.ClampInteger(MinAlpha, 0, 255)
            currentAlpha = startAlpha
            lastMinAlpha = endAlpha
            lastMaxAlpha = startAlpha
            isHolding = holdDuration > 0
            holdStartTime = DateTime.Now

            ' Caso istantaneo: impostiamo il valore e lasciamo il draw permanente se opaco (se vuoi che sia istantaneo e non persistente, chiama CancelAll)
            If fadeDuration <= 0 AndAlso holdDuration <= 0 Then
                currentAlpha = endAlpha
                fadeState = FadeDirection.None

                If currentAlpha > 0 Then
                    RegisterToTick()
                Else
                    UnregisterFromTick()
                    If isPauseMenuDisabled Then
                        NativeGeneric.DisablePauseMenu(False)
                        isPauseMenuDisabled = False
                        shouldReenablePauseMenu = False
                    End If
                End If
                Return
            End If

            If isPauseMenuDisabled Then
                shouldReenablePauseMenu = True
            End If

            RegisterToTick()
        End Sub

        ''' <summary>
        ''' Starts a fade-out (from minAlpha → maxAlpha).
        ''' Duration in ms. HideRadar: if True hides the radar immediately.
        ''' Color: ColorRGB (if Nothing -> Black).
        ''' </summary>
        Public Shared Sub FadeOut(Optional Duration As Integer = 2000, Optional ByVal Color As ColorRGB = Nothing, Optional MinAlpha As Integer = 0, Optional MaxAlpha As Integer = 255, Optional DisablePauseMenu As Boolean = False)

            If Color = Nothing Then
                fadeColor = New ColorRGB(0, 0, 0)
            Else
                fadeColor = Color
            End If

            fadeState = FadeDirection.Outward
            fadeStart = DateTime.Now
            fadeDuration = Math.Max(0, Duration)
            startAlpha = Tools.ClampInteger(MinAlpha, 0, 255)
            endAlpha = Tools.ClampInteger(MaxAlpha, 0, 255)
            currentAlpha = startAlpha
            lastMinAlpha = startAlpha
            lastMaxAlpha = endAlpha
            isHolding = holdDuration > 0
            holdStartTime = DateTime.Now

            If fadeDuration <= 0 AndAlso holdDuration <= 0 Then
                currentAlpha = endAlpha
                fadeState = FadeDirection.None

                If currentAlpha > 0 Then
                    RegisterToTick()
                Else
                    If isPauseMenuDisabled Then
                        NativeGeneric.DisablePauseMenu(False)
                        isPauseMenuDisabled = False
                    End If
                    UnregisterFromTick()
                End If
                Return
            End If

            RegisterToTick()

            If DisablePauseMenu AndAlso Not isPauseMenuDisabled Then
                NativeGeneric.DisablePauseMenu(True)
                isPauseMenuDisabled = True
            End If
        End Sub

        ''' <summary>
        ''' Cancels any fade and clears the overlay immediately.
        ''' </summary>
        Public Shared Sub CancelAll()
            fadeState = FadeDirection.None
            currentAlpha = 0
            isHolding = False
            If isPauseMenuDisabled Then
                NativeGeneric.DisablePauseMenu(False)
                isPauseMenuDisabled = False
            End If
            shouldReenablePauseMenu = False
            UnregisterFromTick()
        End Sub

        ''' <summary>
        ''' Internal update called every tick. If not fading but currentAlpha>0 we still allow draw to keep overlay persistent.
        ''' </summary>
        Private Shared Sub update()
            If isHolding Then
                Dim held As Integer = CInt((DateTime.Now - holdStartTime).TotalMilliseconds)
                If held < holdDuration Then
                    Return
                Else
                    isHolding = False
                    fadeStart = DateTime.Now
                End If
            End If

            If fadeState = FadeDirection.None Then
                Return
            End If

            Dim elapsed As Integer = CInt((DateTime.Now - fadeStart).TotalMilliseconds)
            Dim progress As Single = If(fadeDuration > 0, Math.Min(1.0F, elapsed / fadeDuration), 1.0F)

            currentAlpha = CInt(startAlpha + (endAlpha - startAlpha) * progress)

            If progress >= 1.0F Then
                Dim finalAlpha As Integer = currentAlpha
                fadeState = FadeDirection.None

                If shouldReenablePauseMenu AndAlso finalAlpha <= lastMinAlpha Then
                    NativeGeneric.DisablePauseMenu(False)
                    isPauseMenuDisabled = False
                    shouldReenablePauseMenu = False
                End If


                If finalAlpha <= 0 Then
                    currentAlpha = 0
                    UnregisterFromTick()
                Else

                End If
            End If
        End Sub

        ''' <summary>
        ''' Draws the fade rectangle every tick if currentAlpha > 0.
        ''' </summary>
        Private Shared Sub draw()
            If currentAlpha <= 0 Then Return

            ' disegna un rettangolo fullscreen con il colore e alpha corrente
            Dim c As ColorRGB = fadeColor
            Dim alphaSingle As Single = Tools.ClampInteger(currentAlpha, 0, 255) / 255.0F
            ' supponendo NativeDraws.DrawRectangle accetti ColorRGBA con alpha oppure un overload:
            NativeDraws.DrawRectangle(New Vector2(0.5F, 0.5F), New Vector2(1.0F, 1.0F), New ColorRGBA(c.Red, c.Green, c.Blue, currentAlpha), 1)
        End Sub
        ''' <summary>
        ''' Internal Tick callback.
        ''' </summary>
        Private Shared Sub internalTick()
            update()
            draw()
        End Sub
        ''' <summary>
        ''' Automatically registers this class to TickHelper.
        ''' </summary>
        Private Shared Sub RegisterToTick()
            If registeredInTick Then Return
            TickHelper.Add_Internal(AddressOf internalTick)
            registeredInTick = True
        End Sub
        ''' <summary>
        ''' Automatically unregisters this class from TickHelper.
        ''' </summary>
        Private Shared Sub UnregisterFromTick()
            If Not registeredInTick Then Return
            TickHelper.Remove_Internal(AddressOf internalTick)
            registeredInTick = False
        End Sub
    End Class
    Private Class Cutscenes

    End Class
    ''' <summary>
    ''' Not fully tested.
    ''' Can be used to draw any game texture. You have to call TickHelper.ProcessAll
    ''' in a Tick to set the display duration (must be more than 5). 
    ''' Otherwise you can manually call on a Tick.
    ''' </summary>
    Public Class GameTexture
        Private _txdHandle As Integer
        Private _textureHandle As Integer
        Private _streamed As Boolean

        Public ReadOnly Property TXD As String
        Public ReadOnly Property Texture As String
        Public ReadOnly Property TXDHandler As Integer
            Get
                Return _txdHandle
            End Get
        End Property
        Public ReadOnly Property TextureHandler As Integer
            Get
                Return _textureHandle
            End Get
        End Property
        Public ReadOnly Property StreamedTXD As Boolean
            Get
                Return _streamed
            End Get
        End Property
        Public Property IsLoaded As Boolean
        Public Sub New(TXD As String, Texture As String, Optional StreamedTXD As Boolean = False)
            Me.TXD = TXD
            Me.Texture = Texture
            _streamed = StreamedTXD
        End Sub
        Public Sub Load()
            If IsLoaded Then Exit Sub

            If _streamed Then
                [Call]("REQUEST_STREAMED_TXD", TXD, True)
            Else
                _txdHandle = [Call](Of Integer)("LOAD_TXD", TXD)
            End If

            _textureHandle = [Call](Of Integer)("GET_TEXTURE", _txdHandle, Texture)

            IsLoaded = True
        End Sub
        Public Sub Draw(pos As Vector2, size As Vector2, rotation As Single, color As ColorRGBA, Optional duration As Integer = 1)
            If Not IsLoaded Then Load()

            [Call]("DRAW_SPRITE", TXD, Texture, pos.X, pos.Y, size.X, size.Y, rotation, color.Red, color.Green, color.Blue, color.Alpha)
        End Sub
        Public Sub Release(Optional releaseTXD As Boolean = True)
            If _textureHandle <> 0 Then
                [Call]("RELEASE_TEXTURE", _textureHandle)
                _textureHandle = 0
            End If

            If releaseTXD Then
                If _streamed Then
                    [Call]("MARK_STREAMED_TXD_AS_NO_LONGER_NEEDED", TXD)
                Else
                    If _txdHandle <> 0 Then
                        [Call]("REMOVE_TXD", _txdHandle)
                    End If
                End If
            End If

            _txdHandle = 0
            IsLoaded = False
        End Sub
    End Class
    Public Class GXT
        ''' <summary>
        ''' Displays a GXT text with Input Text (bottom-left corner). Must be used in a tick (cycle).
        ''' You can display two or more lines contemporarily.
        ''' </summary>
        ''' <param name="TextKey">A string from TextKeys structure.</param>
        Public Shared Sub Draw_InputHelperText(GXT_Table As String, GXT_Entry As String, TextKey As String, OnTheSameLine As Boolean, Optional Unknown As UInteger = 1)
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, Unknown)
            [Call]("DRAW_FRONTEND_HELPER_TEXT", GXT_Entry, TextKey, Not OnTheSameLine)
        End Sub
        ''' <summary>
        ''' Displays a GXT text. Must be used in a tick (cycle).
        ''' </summary>
        Public Shared Sub Print(PercentagePos As Vector2, GXT_Table As String, GXT_Entry As String, Optional Unknown As Integer = 1)
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, Unknown)
            [Call]("DISPLAY_TEXT", PercentagePos.X, PercentagePos.Y, GXT_Entry)
        End Sub
        ''' <summary>
        ''' Displays a GXT text with additional Float value. Must be used in a tick (cycle).
        ''' </summary>
        Public Shared Sub Print_WithAdditionalFloat(PercentagePos As Vector2, GXT_Table As String, GXT_Entry As String, Float As Single, DecimalToShow As Integer)
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
            [Call]("DISPLAY_TEXT_WITH_FLOAT", PercentagePos.X, PercentagePos.Y, GXT_Entry, Float, DecimalToShow)
        End Sub
        ''' <summary>
        ''' Displays a GXT text with additional numbers. Must be used in a tick (cycle).
        ''' </summary>
        ''' <param name="NumbersCount_1_to_3"></param>
        Public Shared Sub Print_WithAdditionalNumbers(PercentagePos As Vector2, GXT_Table As String, GXT_Entry As String, NumbersCount_1_to_3 As Integer, Number1 As Integer, Optional Number2 As Integer = 0, Optional Number3 As Integer = 0)
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
            Select Case NumbersCount_1_to_3
                Case 1
                    [Call]("DISPLAY_TEXT_WITH_NUMBER", PercentagePos.X, PercentagePos.Y, GXT_Entry, Number1)
                Case 2
                    [Call]("DISPLAY_TEXT_WITH_2_NUMBERS", PercentagePos.X, PercentagePos.Y, GXT_Entry, Number1, Number2)
                Case 3
                    [Call]("DISPLAY_TEXT_WITH_3_NUMBERS", PercentagePos.X, PercentagePos.Y, GXT_Entry, Number1, Number2, Number2)
                Case Else
                    Exit Sub
            End Select
        End Sub
        ''' <summary>
        ''' Displays a GXT text with additional string. Must be used in a tick (cycle).
        ''' </summary>
        ''' <param name="LiteralString">True if you want to add a string from GXT, False if you want to add a custom string</param>
        Public Shared Sub Print_WithAdditionalString(PercentagePos As Vector2, GXT_Table As String, GXT_Entry As String, AdditionalString As String, Optional LiteralString As Boolean = True)
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
            '  If String2 = "" Then
            If LiteralString Then
                [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", PercentagePos.X, PercentagePos.Y, GXT_Entry, AdditionalString)
            Else
                [Call]("DISPLAY_TEXT_WITH_STRING", PercentagePos.X, PercentagePos.Y, GXT_Entry, AdditionalString)
            End If
            'Else
            '    If LiteralStrings Then
            '        [Call]("DISPLAY_TEXT_WITH_TWO_STRINGS", PercentagePos.X, PercentagePos.Y, GXT_Entry, String1)
            '    Else
            '        [Call]("DISPLAY_TEXT_WITH_TWO_LITERAL_STRINGS", PercentagePos.X, PercentagePos.Y, GXT_Entry, String1)
            '    End If
            ' End If
        End Sub
        Public Class HelpMessage
            Public Shared Sub Clear()
                [Call]("CLEAR_HELP")
            End Sub
            Public Shared Sub EnableNonMinigameHelpMessages(Optional Enable As Boolean = True)
                [Call]("DISPLAY_NON_MINIGAME_HELP_MESSAGES", Enable)
            End Sub
            Public Shared Function GetMessageBoxSize() As GTA.Vector2
                Dim pointer1, pointer2 As Native.Pointer
                pointer1 = New Native.Pointer(GetType(Single))
                pointer2 = New Native.Pointer(GetType(Single))
                [Call]("GET_HELP_MESSAGE_BOX_SIZE", pointer1, pointer2)
                Return New GTA.Vector2(pointer1.Value, pointer2.Value)
            End Function
            Public Shared Sub Hide_ThisFrame()
                [Call]("HIDE_HELP_TEXT_THIS_FRAME")
            End Sub
            Public Shared Sub InitFrontedHelperText()
                [Call]("INIT_FRONTEND_HELPER_TEXT")
            End Sub
            Public Shared Function isPrintingAnyHelp() As Boolean
                Return [Call](Of Boolean)("IS_HELP_MESSAGE_BEING_DISPLAYED")
            End Function
            Public Shared Function isPrintingHelp(GXT_Entry As String) As Boolean
                Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_BEING_DISPLAYED", GXT_Entry)
            End Function
            Public Shared Function isPrintingHelp_WithNumber(GXT_Entry As String, Number As Integer) As Boolean
                Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_WITH_NUMBER_BEING_DISPLAYED", GXT_Entry, Number)
            End Function
            Public Shared Function isPrintingHelp_WithString(GXT_Entry As String, AdditionalString As String) As Boolean
                Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_WITH_STRING_BEING_DISPLAYED", GXT_Entry, AdditionalString)
            End Function
            Public Shared Sub Print(GXT_Table As String, GXT_Entry As String, Optional PrintForever As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                If Not PrintForever Then
                    [Call]("PRINT_HELP", GXT_Entry)
                Else
                    [Call]("PRINT_HELP_FOREVER", GXT_Entry)
                End If
            End Sub
            Public Shared Sub Print_ThisFrame(GXT_Table As String, GXT_Entry As String, Optional Unknown As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                [Call]("DISPLAY_HELP_TEXT_THIS_FRAME", GXT_Entry, Unknown)
            End Sub
            Public Shared Sub Print_WithAdditionalNumber(GXT_Table As String, GXT_Entry As String, AdditionalNumber As Integer, Optional PrintForever As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                If Not PrintForever Then
                    [Call]("PRINT_HELP_WITH_NUMBER", GXT_Entry, AdditionalNumber)
                Else
                    [Call]("PRINT_HELP_FOREVER_WITH_NUMBER", GXT_Entry, AdditionalNumber)
                End If
            End Sub
            Public Shared Sub Print_WithAdditionalString(GXT_Table As String, GXT_Entry As String, AdditionalString_GXT As String, Optional PrintForever As Boolean = False, Optional NoSound As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                If Not PrintForever Then
                    [Call]("PRINT_HELP_WITH_STRING", GXT_Entry, AdditionalString_GXT)
                Else
                    If Not NoSound Then
                        [Call]("PRINT_HELP_FOREVER_WITH_STRING", GXT_Entry, AdditionalString_GXT)
                    Else
                        [Call]("PRINT_HELP_FOREVER_WITH_STRING_NO_SOUND", GXT_Entry, AdditionalString_GXT)
                    End If
                End If
            End Sub
            'Public Shared Sub SetMessageBoxSize(Size As Single)
            '    [Call]("SET_HELP_MESSAGE_BOX_SIZE", value unknown type)
            'End Sub
        End Class
    End Class
    'Public Class Lights
    '    Public Property Position As Vector3
    '    Public Property Color As ColorRGB
    '    Public Property Intensity As Single = 1.0F
    '    Public Property Range As Single = 20.0F
    '    Public Property FadeInTime As Integer
    '    Public Property FadeOutTime As Integer

    '    Private currentIntensity As Single = 0.0F
    '    Private targetIntensity As Single = 0.0F
    '    Private fadeStartTime As Integer
    '    Private fadingIn As Boolean = False
    '    Private fadingOut As Boolean = False
    '    Private registeredTick As Boolean = False

    '    ' === Variabili per Attach ===
    '    Private attachedPed As Ped = Nothing
    '    Private attachedVehicle As Vehicle = Nothing
    '    Private attachedProp As GTA.Object = Nothing
    '    Private offset As Vector3 = Vector3.Zero
    '    Public Sub New(Position As Vector3, Color As ColorRGB, Range As Single, Intensity As Single, Optional FadeIn As Integer = 500, Optional FadeOut As Integer = 500)

    '        Me.Position = Position
    '        Me.Color = If(Color = Nothing, New ColorRGB(255, 255, 255), Color)
    '        FadeInTime = FadeIn
    '        FadeOutTime = FadeOut
    '    End Sub
    '    Public ReadOnly Property IsOn As Boolean
    '        Get
    '            Return targetIntensity > 0.0F OrElse fadingIn
    '        End Get
    '    End Property
    '    Public ReadOnly Property IsAttached As Boolean
    '        Get
    '            Return attachedPed IsNot Nothing OrElse attachedVehicle IsNot Nothing OrElse attachedProp IsNot Nothing
    '        End Get
    '    End Property
    '    Public Sub AttachTo(Ped As Ped, Optional localOffset As Vector3 = Nothing)
    '        attachedPed = Ped
    '        attachedVehicle = Nothing
    '        attachedProp = Nothing
    '        offset = If(localOffset = Nothing, Vector3.Zero, localOffset)
    '    End Sub
    '    Public Sub AttachTo(Vehicle As Vehicle, Optional localOffset As Vector3 = Nothing)
    '        attachedVehicle = Vehicle
    '        attachedPed = Nothing
    '        attachedProp = Nothing
    '        offset = If(localOffset = Nothing, Vector3.Zero, localOffset)
    '    End Sub
    '    Public Sub AttachTo(Obj As GTA.Object, Optional localOffset As Vector3 = Nothing)
    '        attachedProp = Obj
    '        attachedPed = Nothing
    '        attachedVehicle = Nothing
    '        offset = If(localOffset = Nothing, Vector3.Zero, localOffset)
    '    End Sub
    '    Public Sub Detach()
    '        attachedPed = Nothing
    '        attachedVehicle = Nothing
    '        attachedProp = Nothing
    '        offset = Vector3.Zero
    '    End Sub
    '    Public Sub TurnOn(Optional OverrideFade As Integer? = Nothing)
    '        If IsOn Then Return
    '        targetIntensity = Intensity
    '        fadeStartTime = NativeTimer.TotalGameTime
    '        fadingIn = True
    '        fadingOut = False
    '        If Not registeredTick Then RegisterTick()
    '        If OverrideFade.HasValue Then FadeInTime = OverrideFade.Value
    '    End Sub
    '    Public Sub TurnOff(Optional OverrideFade As Integer? = Nothing)
    '        If Not IsOn Then Return
    '        targetIntensity = 0.0F
    '        fadeStartTime = NativeTimer.TotalGameTime
    '        fadingOut = True
    '        fadingIn = False
    '        If Not registeredTick Then RegisterTick()
    '        If OverrideFade.HasValue Then FadeOutTime = OverrideFade.Value
    '    End Sub
    '    '------------------------------------------------------------------------------------
    '    Private Sub OnTick()
    '        UpdateAttachment()
    '        UpdateFade()
    '        Draw()
    '        If Not fadingIn AndAlso Not fadingOut AndAlso currentIntensity <= 0.0F Then
    '            UnregisterTick()
    '        End If
    '    End Sub
    '    Private Sub UpdateFade()
    '        Dim now As Integer = NativeTimer.TotalGameTime
    '        Dim elapsed As Integer = now - fadeStartTime

    '        If fadingIn Then
    '            Dim progress As Single = If(FadeInTime > 0, CSng(elapsed) / FadeInTime, 1.0F)
    '            currentIntensity = Math.Min(Intensity, Intensity * progress)
    '            If progress >= 1.0F Then fadingIn = False

    '        ElseIf fadingOut Then
    '            Dim progress As Single = If(FadeOutTime > 0, CSng(elapsed) / FadeOutTime, 1.0F)
    '            currentIntensity = Math.Max(0.0F, Intensity * (1.0F - progress))
    '            If progress >= 1.0F Then fadingOut = False
    '        End If
    '    End Sub
    '    Private Sub UpdateAttachment()
    '        If attachedPed IsNot Nothing AndAlso attachedPed.Exists() Then
    '            Position = attachedPed.GetOffsetPosition(offset)
    '        ElseIf attachedVehicle IsNot Nothing AndAlso attachedVehicle.Exists() Then
    '            Position = attachedVehicle.GetOffsetPosition(offset)
    '        ElseIf attachedProp IsNot Nothing AndAlso attachedProp.Exists() Then
    '            Position = attachedProp.GetOffsetPosition(offset)
    '        End If
    '    End Sub
    '    Private Sub Draw()
    '        If currentIntensity <= 0.0F Then Return
    '        NativeDraws.DrawLight(Position, Color, currentIntensity, Range)
    '    End Sub
    '    Private Sub RegisterTick()
    '        TickHelper.Add_Internal(AddressOf OnTick)
    '        registeredTick = True
    '    End Sub
    '    Private Sub UnregisterTick()
    '        TickHelper.Remove_Internal(AddressOf OnTick)
    '        registeredTick = False
    '    End Sub
    'End Class
    Public Class Movies
        ' Private Shared _currMovie As String
        Public Shared Sub Draw(PercentagePos As Vector2, PercentageSize As Vector2, Rotation As Single, Color As ColorRGBA)
            [Call]("DRAW_MOVIE", PercentagePos.X, PercentagePos.Y, PercentageSize.X, PercentageSize.Y, Rotation, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        End Sub
        Public Shared Sub Play()
            [Call]("PLAY_MOVIE")
        End Sub
        Public Shared Sub Relase()
            [Call]("RELASE_MOVIE")
        End Sub
        Public Shared WriteOnly Property CurrentMovie As String
            Set(value As String)
                [Call]("SET_CURRENT_MOVIE", value)
            End Set
        End Property
        Public Shared WriteOnly Property Time As Single
            Set(value As Single)
                [Call]("SET_MOVIE_TIME", value)
            End Set
        End Property
        Public Shared WriteOnly Property Volume As Single
            Set(value As Single)
                [Call]("SET_MOVIE_VOLUME", value)
            End Set
        End Property
        Public Shared Sub [Stop](Optional Relase As Boolean = False)
            [Call]("STOP_MOVIE")
            If Relase Then [Call]("RELASE_MOVIE")
        End Sub
    End Class
    Public Class NativeFonts
        Public Enum Fonts
            Standard = 0
            Dotted = 1
            DottedAndGTAfont = 3
            StandardAndDartHandwrite = 4
            Large = 5
            Narrow = 6
        End Enum
        Public Shared Sub Change(FontID As Fonts)
            If Not [Call](Of Boolean)("IS_FONT_LOADED", FontID) Then [Call]("LOAD_TEXT_FONT", FontID)
            [Call]("SET_TEXT_FONT", FontID)
        End Sub
        Public Shared Sub Reset()
            [Call]("USE_PREVIOUS_FONT_SETTINGS")
        End Sub
        'Public Shared Function isFontLoaded(FontID As Fonts)
        '    Return [Call](Of Boolean)("IS_FONT_LOADED", FontID)
        'End Function
        'Public Shared Sub Request(FontID As Fonts)
        '    [Call]("LOAD_TEXT_FONT", FontID)
        'End Sub
    End Class
    Public Structure TextBlips
        Public Const Destination1 As String = "~BLIP_0~"
        Public Const Destination2 As String = "~BLIP_1~"
        Public Const Destination3 As String = "~BLIP_2~"
        Public Const Objective1 As String = "~BLIP_3~"
        Public Const Objective2 As String = "~BLIP_4~"
        Public Const Objective3 As String = "~BLIP_5~"
        Public Const Player As String = "~BLIP_6~"
        Public Const North As String = "~BLIP_77~"
        Public Const Waypoint As String = "~BLIP_8~"
        Public Const Weapon_Pistol As String = "~BLIP_9~"
        Public Const Weapon_Shotgun As String = "~BLIP_10~"
        Public Const Weapon_SMG As String = "~BLIP_11~"
        Public Const Weapon_Rifle As String = "~BLIP_~12"
        Public Const Weapon_RocketLauncher As String = "~BLIP_13~"
        Public Const Weapon_Grenade As String = "~BLIP_14~"
        Public Const Weapon_Molotov As String = "~BLIP_15~"
        Public Const Weapon_Sniper As String = "~BLIP_16~"
        Public Const Weapon_BaseballBat As String = "~BLIP_17~"
        Public Const Weapon_Knife As String = "~BLIP_18~"
        Public Const Weapon_Health As String = "~BLIP_19~"
        Public Const Weapon_Armor As String = "~BLIP_20~"
        Public Const BurgerShot As String = "~BLIP_21~"
        Public Const CluckinBell As String = "~BLIP_22~"
        Public Const Vlad As String = "~BLIP_23~"
        Public Const Internet As String = "~BLIP_24~"
        Public Const Manny As String = "~BLIP_25~"
        Public Const LittleJacob As String = "~BLIP_26~"
        Public Const Roman As String = "~BLIP_27~"
        Public Const Faustin As String = "~BLIP_28~"
        Public Const Safehouse As String = "~BLIP_29~"
        Public Const Taxi As String = "~BLIP_30~"
        Public Const Bernie As String = "~BLIP_31~"
        Public Const Brucie As String = "~BLIP_32~"
        Public Const UnknownContact As String = "~BLIP_33~"
        Public Const Dwayne As String = "~BLIP_34~"
        Public Const Elizabeta As String = "~BLIP_35~"
        Public Const Gambetti As String = "~BLIP_36~"
        Public Const JimmyPegorino As String = "~BLIP_37~"
        Public Const Derrick As String = "~BLIP_38~"
        Public Const Francis As String = "~BLIP_39~"
        Public Const Gerry As String = "~BLIP_40~"
        Public Const Kate As String = "~BLIP_41~"
        Public Const Packie As String = "~BLIP_42~"
        Public Const PhilBell As String = "~BLIP_43~"
        Public Const PlayboyX As String = "~BLIP_44~"
        Public Const RayBoccino As String = "~BLIP_45~"
        Public Const EightPool As String = "~BLIP_46~"
        Public Const Bar As String = "~BLIP_47~"
        Public Const BoatTour As String = "~BLIP_48~"
        Public Const Bowling As String = "~BLIP_49~"
        Public Const ClothesShop As String = "~BLIP_50~"
        Public Const Club As String = "~BLIP_51~"
        Public Const Darts As String = "~BLIP_52~"
        Public Const DwayneRed As String = "~BLIP_53~"
        Public Const DateGirl As String = "~BLIP_54~"
        Public Const PlayboyXRed As String = "~BLIP_55~"
        Public Const Helitour As String = "~BLIP_56~"
        Public Const Restaurant As String = "~BLIP_57~"
        Public Const TrainStation As String = "~BLIP_58~"
        Public Const WeaponShop As String = "~BLIP_59~"
        Public Const PoliceStation As String = "~BLIP_60~"
        Public Const FireStation As String = "~BLIP_61~"
        Public Const Hospital As String = "~BLIP_62~"
        Public Const FriendMale As String = "~BLIP_63~"
        Public Const FriendFemale As String = "~BLIP_64~"
        Public Const RaceFinishLine As String = "~BLIP_65~"
        Public Const StripClub As String = "~BLIP_66~"
        Public Const ConsoleGame As String = "~BLIP_67~"
        Public Const CopCar As String = "~BLIP_68~"
        Public Const Dimitri As String = "~BLIP_69~"
        Public Const ComedyClub As String = "~BLIP_70~"
        Public Const CabaretClub As String = "~BLIP_71~"
        Public Const Ransom As String = "~BLIP_72~"
        Public Const CopHeli As String = "~BLIP_73~"
        Public Const Michelle As String = "~BLIP_74~"
        Public Const PayNSpray As String = "~BLIP_75~"
        Public Const Assassin As String = "~BLIP_76~"
        Public Const Revenge As String = "~BLIP_77~"
        Public Const Deal As String = "~BLIP_78~"
        Public Const Garage As String = "~BLIP_79~"
        Public Const Lawyer As String = "~BLIP_80~"
        Public Const Trophy As String = "~BLIP_81~"
        Public Const MultiplayerTutorial As String = "~BLIP_82~"
        Public Const Station3 As String = "~BLIP_83~"
        Public Const Station8 As String = "~BLIP_84~"
        Public Const StationA As String = "~BLIP_85~"
        Public Const StationB As String = "~BLIP_86~"
        Public Const StationC As String = "~BLIP_87~"
        Public Const StationE As String = "~BLIP_88~"
        Public Const StationJ As String = "~BLIP_89~"
        Public Const StationK As String = "~BLIP_90~"
        Public Const CarWash As String = "~BLIP_91~"
        Public Const UnitedLibertyPaper As String = "~BLIP_92~"
        Public Const ArrowAbove As String = "~BLIP_93~"
        Public Const ArrowBelow As String = "~BLIP_94~"
    End Structure
    Public Structure TextColors
        Public Const Red As String = "~r~"
        Public Const Green As String = "~g~"
        Public Const Blue As String = "~b~"
        Public Const Yellow As String = "~y~"
        Public Const Orange As String = "~o~"
        Public Const Purple As String = "~p~"
        Public Const White As String = "~w~"
        Public Const Gray As String = "~m~"
        Public Const Black As String = "~l~"
        Public Const LightGray As String = "~c~"
        'Public Const LightBlue As String = "~e~"
        Public Const LightPink As String = "~s~"

        ' Multiplayer
        Public Const MP_Orange As String = "~COL_NET_1~"
        Public Const MP_Purple As String = "~COL_NET_2~"
        Public Const MP_LightGreen As String = "~COL_NET_3~"
        Public Const MP_Red As String = "~COL_NET_4~"
        Public Const MP_Pink As String = "~COL_NET_5~"
        Public Const MP_Brown As String = "~COL_NET_6~"
        Public Const MP_Agua As String = "~COL_NET_7~"
        Public Const MP_VeryLightBlue As String = "~COL_NET_8~"
        Public Const MP_Yellow As String = "~COL_NET_9~"
        Public Const MP_AguaDark As String = "~COL_NET_10~"
        Public Const MP_Violet As String = "~COL_NET_11~"
        Public Const MP_Pink2 As String = "~COL_NET_12~"
        Public Const MP_LightOrange As String = "~COL_NET_13~"
        Public Const MP_Yellow2 As String = "~COL_NET_14~"
        Public Const MP_Yellow3 As String = "~COL_NET_15~"
        Public Const MP_Purple2 As String = "~COL_NET_16~"

        ' FX colors / special
        Public Const FX_Unknown02 As String = "~n~"
        Public Const FX_StopColorFxHere As String = "~s~"
        Public Const Fx_Highlighted As String = "~h~"
        Public Const Fx_String As String = "~a~"
        Public Const Fx_Number As String = "~1~"
        Public Const Fx_Subtitle As String = "~z~"
        Public Const RockstarLogo As String = "¬"
        '  Public Shared FivePointStar As String = "^"
        'Public Shared FivePointStarSmall As String = "*"
    End Structure
    Public Structure TextKeys
        Public Const Action As String = "~INPUT_PICKUP~"
        Public Const Pickup As String = "~INPUT_PICKUP~"
        Public Const Enter As String = "~INPUT_ENTER~"
        Public Const Jump As String = "~INPUT_JUMP~"
        Public Const Reload As String = "~INPUT_RELOAD~"
        Public Const MoveUp As String = "~INPUT_MOVE_UP~"
        Public Const MoveLeft As String = "~INPUT_MOVE_LEFT~"
        Public Const MoveDown As String = "~INPUT_MOVE_DOWN~"
        Public Const MoveRight As String = "~INPUT_MOVE_RIGHT~"
        Public Const TextChatUniversal As String = "~INPUT_TEXT_CHAT_UNIVERSAL~"
        Public Const TextChatResultsToggle As String = "~INPUT_TEXT_CHAT_RESULTS_TOGGLE~"
        Public Const TextChatY As String = "~INPUT_TEXT_CHAT_Y~"
        Public Const NextTrack As String = "~INPUT_NEXT_TRACK~"
        Public Const PrevTrack As String = "~INPUT_PREV_TRACK~"
        Public Const FrontendRefresh As String = "~INPUT_FRONTEND_REFRESH~"
        Public Const FrontendLockLobby As String = "~INPUT_FRONTEND_LOCK_LOBBY~"
        Public Const FrontendAccept As String = "~INPUT_FRONTEND_ACCEPT~"
        Public Const FrontendCancel As String = "~INPUT_FRONTEND_CANCEL~"
        Public Const FrontendLT As String = "~INPUT_FRONTEND_LT~"
        Public Const FrontendRT As String = "~INPUT_FRONTEND_RT~"
        Public Const FrontendLB As String = "~INPUT_FRONTEND_LB~"
        Public Const FrontendRB As String = "~INPUT_FRONTEND_RB~"
        Public Const FrontendPause As String = "~INPUT_FRONTEND_PAUSE~"
        Public Const FrontendModelScreen As String = "~INPUT_FRONTEND_MODEL_SCREEN~"
        Public Const FrontendLegend As String = "~INPUT_FRONTEND_LEGEND~"
        Public Const FrontendReplayScreenshot As String = "~INPUT_FRONTEND_REPLAY_SCREENSHOT~"
        Public Const FrontendDeleteFilter As String = "~INPUT_FRONTEND_DELETE_FILTER~"
        Public Const FrontendApply As String = "~INPUT_FRONTEND_APPLY~"
        Public Const FrontendVehicleToggle As String = "~INPUT_FRONTEND_VEHICLE_TOGGLE~"
        Public Const ReplaySaveToHdd As String = "~INPUT_REPLAY_SAVE_TO_HDD~"
        Public Const MouseUd As String = "~INPUT_MOUSE_UD~"
        Public Const FreeAim As String = "~INPUT_FREE_AIM~"
        Public Const VehicleHorn As String = "~INPUT_VEH_HORN~"
        Public Const KbUp As String = "~INPUT_KB_UP~"
        Public Const KbDown As String = "~INPUT_KB_DOWN~"
        Public Const KbLeft As String = "~INPUT_KB_LEFT~"
        Public Const KbRight As String = "~INPUT_KB_RIGHT~"
        Public Const Sprint As String = "~INPUT_SPRINT~"
        Public Const VehicleExit As String = "~INPUT_VEH_EXIT~"
        Public Const PhoneAccept As String = "~INPUT_PHONE_ACCEPT~"
        Public Const Attack As String = "~INPUT_ATTACK~"
        Public Const DropWeapon As String = "~INPUT_DROP_WEAPON~"
        Public Const VehicleHandbrake As String = "~INPUT_VEH_HANDBRAKE~"
        Public Const VehicleHeadlight As String = "~INPUT_VEH_HEADLIGHT~"
        Public Const MouseRmb As String = "~INPUT_MOUSE_RMB~"
        Public Const Aim As String = "~INPUT_AIM~"
        Public Const Cover As String = "~INPUT_COVER~"
        Public Const Duck As String = "~INPUT_DUCK~"
        Public Const FeReplayPreview As String = "~INPUT_FE_REPLAY_PREVIEW~"
        Public Const FeReplayToggletime As String = "~INPUT_FE_REPLAY_TOGGLETIME~"
        Public Const FeReplayToggletips As String = "~INPUT_FE_REPLAY_TOGGLETIPS~"
        Public Const FrontendAxisBack As String = "~INPUT_FRONTEND_AXIS_BACK~"
        Public Const FrontendAxisFwd As String = "~INPUT_FRONTEND_AXIS_FWD~"
        Public Const FrontendAxisLeft As String = "~INPUT_FRONTEND_AXIS_LEFT~"
        Public Const FrontendAxisRight As String = "~INPUT_FRONTEND_AXIS_RIGHT~"
        Public Const FrontendCopy As String = "~INPUT_FRONTEND_COPY~"
        Public Const FrontendCut As String = "~INPUT_FRONTEND_CUT~"
        Public Const FrontendF6 As String = "~INPUT_FRONTEND_F6~"
        Public Const FrontendMarkerDelete As String = "~INPUT_FRONTEND_MARKER_DELETE~"
        Public Const FrontendMinigame1 As String = "~INPUT_FRONTEND_MINIGAME_1~"
        Public Const FrontendMinigame2 As String = "~INPUT_FRONTEND_MINIGAME_2~"
        Public Const FrontendMinigame3 As String = "~INPUT_FRONTEND_MINIGAME_3~"
        Public Const FrontendMinigame4 As String = "~INPUT_FRONTEND_MINIGAME_4~"
        Public Const FrontendPaste As String = "~INPUT_FRONTEND_PASTE~"
        Public Const FrontendPlayerList As String = "~INPUT_FRONTEND_PLAYER_LIST~"
        Public Const FrontendReplayAdvance As String = "~INPUT_FRONTEND_REPLAY_ADVANCE~"
        Public Const FrontendReplayBack As String = "~INPUT_FRONTEND_REPLAY_BACK~"
        Public Const FrontendReplayCyclemarkerleft As String = "~INPUT_FRONTEND_REPLAY_CYCLEMARKERLEFT~"
        Public Const FrontendReplayCyclemarkerright As String = "~INPUT_FRONTEND_REPLAY_CYCLEMARKERRIGHT~"
        Public Const FrontendReplayFfwd As String = "~INPUT_FRONTEND_REPLAY_FFWD~"
        Public Const FrontendReplayHidehud As String = "~INPUT_FRONTEND_REPLAY_HIDEHUD~"
        Public Const FrontendReplayNewmarker As String = "~INPUT_FRONTEND_REPLAY_NEWMARKER~"
        Public Const FrontendReplayPause As String = "~INPUT_FRONTEND_REPLAY_PAUSE~"
        Public Const FrontendReplayRestart As String = "~INPUT_FRONTEND_REPLAY_RESTART~"
        Public Const FrontendReplayRewind As String = "~INPUT_FRONTEND_REPLAY_REWIND~"
        Public Const FrontendReplayShowhotkey As String = "~INPUT_FRONTEND_REPLAY_SHOWHOTKEY~"
        Public Const FrontendReplayTools As String = "~INPUT_FRONTEND_REPLAY_TOOLS~"
        Public Const FrontendX As String = "~INPUT_FRONTEND_X~"
        Public Const FrontendY As String = "~INPUT_FRONTEND_Y~"
        Public Const MeleeAttack1 As String = "~INPUT_MELEE_ATTACK1~"
        Public Const MeleeAttack2 As String = "~INPUT_MELEE_ATTACK2~"
        Public Const MeleeAttack3 As String = "~INPUT_MELEE_ATTACK3~"
        Public Const MeleeBlock As String = "~INPUT_MELEE_BLOCK~"
        Public Const MeleeKick As String = "~INPUT_MELEE_KICK~"
        Public Const MouseLmb As String = "~INPUT_MOUSE_LMB~"
        Public Const MouseWheelDown As String = "~INPUT_MOUSE_WHEEL_DOWN~"
        Public Const MouseWheelUp As String = "~INPUT_MOUSE_WHEEL_UP~"
        Public Const NextCamera As String = "~INPUT_NEXT_CAMERA~"
        Public Const NextWeapon As String = "~INPUT_NEXT_WEAPON~"
        Public Const PhoneCancel As String = "~INPUT_PHONE_CANCEL~"
        Public Const PhonePutAway As String = "~INPUT_PHONE_PUT_AWAY~"
        Public Const PhoneTakeOut As String = "~INPUT_PHONE_TAKE_OUT~"
        Public Const PrevWeapon As String = "~INPUT_PREV_WEAPON~"
        Public Const SniperZoomInAlternate As String = "~INPUT_SNIPER_ZOOM_IN_ALTERNATE~"
        Public Const SniperZoomIn As String = "~INPUT_SNIPER_ZOOM_IN~"
        Public Const SniperZoomOutAlternate As String = "~INPUT_SNIPER_ZOOM_OUT_ALTERNATE~"
        Public Const SniperZoomOut As String = "~INPUT_SNIPER_ZOOM_OUT~"
        Public Const TurnOffRadio As String = "~INPUT_TURN_OFF_RADIO~"
        Public Const VehicleAccelerate As String = "~INPUT_VEH_ACCELERATE~"
        Public Const VehicleAttack2 As String = "~INPUT_VEH_ATTACK2~"
        Public Const VehicleAttack As String = "~INPUT_VEH_ATTACK~"
        Public Const VehicleBrake As String = "~INPUT_VEH_BRAKE~"
        Public Const VehicleCinCam As String = "~INPUT_VEH_CIN_CAM~"
        Public Const VehicleFlyPitchDown As String = "~INPUT_VEH_FLY_PITCH_DOWN~"
        Public Const VehicleFlyPitchUp As String = "~INPUT_VEH_FLY_PITCH_UP~"
        Public Const VehicleFlyRollLeft As String = "~INPUT_VEH_FLY_ROLL_LEFT~"
        Public Const VehicleFlyRollRight As String = "~INPUT_VEH_FLY_ROLL_RIGHT~"
        Public Const VehicleFlyThrottleDown As String = "~INPUT_VEH_FLY_THROTTLE_DOWN~"
        Public Const VehicleFlyThrottleUp As String = "~INPUT_VEH_FLY_THROTTLE_UP~"
        Public Const VehicleFlyYawLeft As String = "~INPUT_VEH_FLY_YAW_LEFT~"
        Public Const VehicleFlyYawRight As String = "~INPUT_VEH_FLY_YAW_RIGHT~"
        Public Const VehicleHotwire1 As String = "~INPUT_VEH_HOTWIRE_LEFT~"
        Public Const VehicleHotwire2 As String = "~INPUT_VEH_HOTWIRE_RIGHT~"
        Public Const VehicleKeyUd As String = "~INPUT_VEH_KEY_UD~"
        Public Const VehicleLookBehind As String = "~INPUT_VEH_LOOK_BEHIND~"
        Public Const VehicleLookLeft As String = "~INPUT_VEH_LOOK_LEFT~"
        Public Const VehicleLookRight As String = "~INPUT_VEH_LOOK_RIGHT~"
        Public Const VehicleNextRadio As String = "~INPUT_VEH_NEXT_RADIO~"
        Public Const VehicleNextWeapon As String = "~INPUT_VEH_NEXT_WEAPON~"
        Public Const VehiclePrevRadio As String = "~INPUT_VEH_PREV_RADIO~"
        Public Const ZoomRadar As String = "~INPUT_ZOOM_RADAR~"
        Public Const MouseWheel As String = "~MOUSE_WHEEL~"
        Public Const Mouse As String = "~MOUSE~"
        Public Const Joypad_A As String = "~PAD_A~"
        Public Const Joypad_B As String = "~PAD_B~"
        Public Const Joypad_Down As String = "~PAD_DOWN~"
        Public Const Joypad_DpadAll As String = "~PAD_DPAD_ALL~"
        Public Const Joypad_DpadLeftright As String = "~PAD_DPAD_LEFTRIGHT~"
        Public Const Joypad_DpadLeft As String = "~PAD_DPAD_LEFT~"
        Public Const Joypad_DpadNone As String = "~PAD_DPAD_NONE~"
        Public Const Joypad_DpadRight As String = "~PAD_DPAD_RIGHT~"
        Public Const Joypad_DpadUpdown As String = "~PAD_DPAD_UPDOWN~"
        Public Const Joypad_Lb As String = "~PAD_LB~"
        Public Const Joypad_Left As String = "~PAD_LEFT~"
        Public Const Joypad_Lshock As String = "~PAD_LSHOCK~"
        Public Const Joypad_LstickAll As String = "~PAD_LSTICK_ALL~"
        Public Const Joypad_LstickDown As String = "~PAD_LSTICK_DOWN~"
        Public Const Joypad_LstickLeftrightVehicle As String = "~PAD_LSTICK_LEFTRIGHT_VEH~"
        Public Const Joypad_LstickLeftright As String = "~PAD_LSTICK_LEFTRIGHT~"
        Public Const Joypad_LstickLeft As String = "~PAD_LSTICK_LEFT~"
        Public Const Joypad_LstickNone As String = "~PAD_LSTICK_NONE~"
        Public Const Joypad_LstickRight As String = "~PAD_LSTICK_RIGHT~"
        Public Const Joypad_LstickUpdown As String = "~PAD_LSTICK_UPDOWN~"
        Public Const Joypad_LstickUp As String = "~PAD_LSTICK_UP~"
        Public Const Joypad_LT As String = "~PAD_LT~"
        Public Const Joypad_RB As String = "~PAD_RB~"
        Public Const Joypad_Right As String = "~PAD_RIGHT~"
        Public Const Joypad_Rshock As String = "~PAD_RSHOCK~"
        Public Const Joypad_RstickAll As String = "~PAD_RSTICK_ALL~"
        Public Const Joypad_RstickDown As String = "~PAD_RSTICK_DOWN~"
        Public Const Joypad_RstickLeftright As String = "~PAD_RSTICK_LEFTRIGHT~"
        Public Const Joypad_RstickLeft As String = "~PAD_RSTICK_LEFT~"
        Public Const Joypad_RstickNone As String = "~PAD_RSTICK_NONE~"
        Public Const Joypad_RstickUpdownOnly As String = "~PAD_RSTICK_UPDOWN_ONLY~"
        Public Const Joypad_RstickUpdown As String = "~PAD_RSTICK_UPDOWN~"
        Public Const Joypad_RstickUp As String = "~PAD_RSTICK_UP~"
        Public Const Joypad_RT As String = "~PAD_RT~"
        Public Const Joypad_Select As String = "~PAD_SELECT~"
        Public Const Joypad_SixaxisDrive As String = "~PAD_SIXAXIS_DRIVE~"
        Public Const Joypad_SixaxisPitch As String = "~PAD_SIXAXIS_PITCH~"
        Public Const Joypad_SixaxisReload As String = "~PAD_SIXAXIS_RELOAD~"
        Public Const Joypad_SixaxisRoll As String = "~PAD_SIXAXIS_ROLL~"
        Public Const Joypad_Start As String = "~PAD_START~"
        Public Const Joypad_Up As String = "~PAD_UP~"
        Public Const Joypad_X As String = "~PAD_X~"
        Public Const Joypad_Y As String = "~PAD_Y~"
        Public Const Joypad_DpadUp As String = "~PAD_DPAD_UP~"
        Public Const Joypad_DpadDown As String = "~PAD_DPAD_DOWN~"
        Public Const Joypad_Back As String = "~PAD_BACK~"
    End Structure
    Public Class TextStyle
        Private _Col As ColorRGBA
        Private _cent As Boolean
        Private _backgr As Boolean
        ' Private _unders As Boolean
        Private _proport As Boolean
        Private _size As Vector2
        ' Private _shadowDist As Single
        Private _ShadowEnabled As Boolean
        Private _Bordertick As Integer
        Private _BorderCol As ColorRGBA
        'Private _textLimits As Vector2
        Public Shared Property [Default] As New TextStyle(ColorRGBA.White, New Vector2(0.26, 0.26), False, ColorRGBA.Black)
        Public Property Background As Boolean
            Get
                Return _backgr
            End Get
            Set(value As Boolean)
                _backgr = value
            End Set
        End Property
        ''' <summary>\
        ''' If nothing the text will display without borders.
        ''' Uses the function SET_TEXT_EDGE
        ''' </summary>
        Public Property Borders As ColorRGBA
            Get
                Return _BorderCol
            End Get
            Set(value As ColorRGBA)
                _BorderCol = value
            End Set
        End Property
        Public Property Center As Boolean
            Get
                Return _cent
            End Get
            Set(value As Boolean)
                _cent = value
            End Set
        End Property
        Public Property Color As ColorRGBA
            Get
                Return _Col
            End Get
            Set(value As ColorRGBA)
                _Col = value
            End Set
        End Property
        Public Property Proportional As Boolean
            Get
                Return _proport
            End Get
            Set(value As Boolean)
                _proport = value
            End Set
        End Property
        Public Property Scale As Vector2
            Get
                Return _size
            End Get
            Set(value As Vector2)
                _size = value
            End Set
        End Property
        '''' <summary>
        '''' Set distance and color of text's shadow.
        '''' Uses the function SET_TEXT_DROPSHADOW
        '''' </summary>
        'Public Property Shadow As Boolean
        '    Get
        '        Return _ShadowEnabled
        '    End Get
        '    Set(value As Boolean)
        '        _ShadowEnabled = value
        '    End Set
        'End Property
        '''' <summary>
        '''' Set the "box" where text will be drawn. If the text is longer, it will be
        '''' display on many lines
        '''' Uses the function SET_TEXT_WRAP
        '''' </summary>
        'Public Property TextBoxLimits As Vector2
        '    Get
        '        Return _textLimits
        '    End Get
        '    Set(value As Vector2)
        '        _textLimits = value
        '    End Set
        'End Property
        'Public Property Underscore As Boolean
        '    Get
        '        Return _unders
        '    End Get
        '    Set(value As Boolean)
        '        _unders = value
        '    End Set
        'End Property
        Public Sub New(Color As ColorRGBA, Scale As Vector2, Optional Center As Boolean = False, Optional BorderColor As ColorRGBA = Nothing,
                       Optional Background As Boolean = False, Optional Proportional As Boolean = False)
            If Color = Nothing Then _Col = New ColorRGBA(255, 255, 255, 255) Else _Col = Color
            _size = Scale
            _cent = Center
            '  If TextBoxLimits = Nothing Then _textLimits = New Vector2(0.99, 0.99) Else _textLimits = TextBoxLimits
            '  _unders = Underscore
            _Bordertick = 1.1
            _BorderCol = BorderColor
            _backgr = Background
            _proport = Proportional
        End Sub
        Friend Shared Sub SetTextStyle(Style As TextStyle)
            [Call]("SET_TEXT_SCALE", Style._size.X, Style._size.Y)
            [Call]("SET_TEXT_BACKGROUND", Style._backgr)
            [Call]("SET_TEXT_CENTRE", Style._cent)
            [Call]("SET_TEXT_COLOUR", Style._Col.Red, Style._Col.Green, Style._Col.Blue, Style._Col.Alpha)

            'If Style._ShadowEnabled = False Then
            '    [Call]("SET_TEXT_DROPSHADOW", 0, 0, 0, 0, 0)
            'Else
            '    [Call]("SET_TEXT_DROPSHADOW", 0, 0, 0, 0, 255)
            'End If

            If Style._BorderCol <> Nothing Then
                [Call]("SET_TEXT_EDGE", Style._Bordertick, Style._BorderCol.Red, Style._BorderCol.Green, Style._BorderCol.Blue, Style._BorderCol.Alpha)
            Else
                [Call]("SET_TEXT_EDGE", 0, 0, 0, 0, 0)
            End If
            [Call]("SET_TEXT_PROPORTIONAL", Not Style._proport)
            '  [Call]("SET_TEXT_USE_UNDERSCORE", Not Style._unders)
            ' [Call]("SET_TEXT_WRAP", Style._textLimits.X, Style._textLimits.Y)
        End Sub

        Public Shared Operator =(Style1 As TextStyle, Style2 As TextStyle) As Boolean
            If ReferenceEquals(Style1, Style2) Then Return True
            If Style1 Is Nothing OrElse Style2 Is Nothing Then Return False

            Return Style1._backgr = Style2._backgr AndAlso
                Style1._Bordertick = Style2._Bordertick AndAlso
                Style1._BorderCol = Style2._BorderCol AndAlso
                Style1._cent = Style2._cent AndAlso
                Style1._Col = Style2._Col AndAlso
                Style1._proport = Style2._proport AndAlso
                Style1._size = Style2._size 'AndAlso
            'Style1._unders = Style2._unders ' AndAlso
            ' Style1._textLimits = Style2._textLimits
            'Style1._shadowDist = Style2._shadowDist AndAlso
        End Operator

        Public Shared Operator <>(Style1 As TextStyle, Style2 As TextStyle) As Boolean
            Return Not (Style1 = Style2)
        End Operator
        Public Overrides Function Equals(Style2 As Object) As Boolean
            If ReferenceEquals(Me, Style2) Then Return True
            If Style2 Is Nothing OrElse Style2.GetType() IsNot Me.GetType() Then Return False

            Dim other As TextStyle = CType(Style2, TextStyle)

            Return _backgr = other._backgr AndAlso
           _Bordertick = other._Bordertick AndAlso
           _BorderCol.Equals(other._BorderCol) AndAlso
           _cent = other._cent AndAlso
           _Col.Equals(other._Col) AndAlso
           _proport = other._proport AndAlso
           _size.Equals(other._size) 'AndAlso
            ' _unders = other._unders 'AndAlso
            '  _textLimits.Equals(other._textLimits)
            '  _shadowDist = other._shadowDist AndAlso
        End Function

        'SET_TEXT_DRAW_BEFORE_FADE
        'SET_TEXT_INPUT_ACTIVE
        'SET_TEXT_JUSTIFY
        'SET_TEXT_LINE_DISPLAY
        'SET_TEXT_LINE_HEIGHT_MULT
        'SET_TEXT_RENDER_ID
        'SET_TEXT_RIGHT_JUSTIFY
    End Class

    '''' <summary>
    '''' Helper class for easily drawing custom images on screen with normalized coordinates, rotation, color, and alpha.
    '''' Includes fade in/out support, debug editing (move/resize/rotate), and saving configuration to file.
    '''' </summary>
    'Public Class CustomTexture

    '    Private Shared ReadOnly activeImages As New List(Of CustomTexture)
    '    Private Shared isTickAttached As Boolean = False

    '    Private ReadOnly texture As Texture
    '    Private pos As Vector2
    '    Private _size As Vector2
    '    Private rot As Single
    '    Private color As ColorRGBA
    '    Private alpha As Integer = 255

    '    Private isVisible As Boolean = True
    '    Private isFading As Boolean = False
    '    Private fadeDirection As Integer = 0 ' 1 = in, -1 = out
    '    Private fadeSpeed As Single = 1.0F

    '    ' Debug mode
    '    Private debugEnabled As Boolean = False
    '    Private moveSpeed As Single = 0.002F
    '    Private sizeSpeed As Single = 0.002F
    '    Private rotateSpeed As Single = 1.0F
    '    Private savePath As String
    '    Public Shared Property DebugLogPath As String = "\NativeExtended-IV.log"

    '    ''' <summary>
    '    ''' Creates a new image helper with a given texture path.
    '    ''' </summary>
    '    Public Sub New(Path As String)


    '        ' If DebugLogPath = "" Then DebugLogPath = Game.InstallFolder & "\NativeExtended-IV.log"

    '        Me.texture = New Texture(Path)
    '        activeImages.Add(Me)
    '        AttachTick()
    '    End Sub

    '    '---------------------------------------------
    '    ' 🔹 Draw
    '    '---------------------------------------------
    '    Public Sub Draw(Position As Vector2, Optional SizePercent As Vector2 = Nothing, Optional Color As ColorRGBA = Nothing)
    '        If Not isVisible Then Return

    '        If Color = Nothing Then Color = Color.White
    '        If SizePercent = Nothing Then SizePercent = New Vector2(0.1F, 0.1F)
    '        Me.pos = Position
    '        Me._size = SizePercent
    '        Me.Rotation = 0.0F
    '        Me.color = Color

    '        Dim screenRes As  = Game.Resolution
    '        Dim pos As New Vector2(screenRes.Width * Position.X,
    '                               screenRes.Height * Position.Y)
    '        Dim sz As New SizeF(screenRes.Width * size.X,
    '                            screenRes.Height * size.Y)

    '        Dim drawColor As Color = Color.FromArgb(alpha, Color)
    '        texture.Draw(pos, sz, Rotation, drawColor)

    '        If debugEnabled Then
    '            Game.DisplayText($"[DEBUG MODE]" & vbCrLf &
    '                             $"Pos: X={Position.X:F3}, Y={Position.Y:F3}" & vbCrLf &
    '                             $"Size: W={size.X:F3}, H={size.Y:F3}" & vbCrLf &
    '                             $"Rot: {Rotation:F1}°",
    '                             25)
    '        End If
    '    End Sub

    '    '---------------------------------------------
    '    ' 🔹 Fade-in / Fade-out
    '    '---------------------------------------------
    '    Public Sub FadeIn(Optional durationMs As Integer = 1000)
    '        fadeDirection = 1
    '        fadeSpeed = 255.0F / Math.Max(durationMs, 1)
    '        isFading = True
    '        alpha = 0
    '        isVisible = True
    '    End Sub

    '    Public Sub FadeOut(Optional durationMs As Integer = 1000)
    '        fadeDirection = -1
    '        fadeSpeed = 255.0F / Math.Max(durationMs, 1)
    '        isFading = True
    '    End Sub

    '    Private Sub ProcessFade()
    '        If Not isFading Then Return

    '        alpha += CInt(fadeDirection * fadeSpeed * Game.LastFrameTime * 1000.0F)
    '        alpha = Math.Clamp(alpha, 0, 255)

    '        If (fadeDirection = 1 AndAlso alpha >= 255) OrElse
    '           (fadeDirection = -1 AndAlso alpha <= 0) Then
    '            isFading = False
    '            If fadeDirection = -1 Then isVisible = False
    '        End If
    '    End Sub

    '    '---------------------------------------------
    '    ' 🔹 Debug Mode Controls
    '    '---------------------------------------------
    '    Private Sub ProcessDebug()
    '        If Not debugEnabled Then Return

    '        ' Move image
    '        If Game.isKeyPressed(Keys.Up) Then position.Y -= moveSpeed
    '        If Game.isKeyPressed(Keys.Down) Then position.Y += moveSpeed
    '        If Game.isKeyPressed(Keys.Left) Then position.X -= moveSpeed
    '        If Game.isKeyPressed(Keys.Right) Then position.X += moveSpeed

    '        ' Resize image
    '        If Game.isKeyPressed(Keys.PageUp) Then
    '            size.X += sizeSpeed
    '            size.Y += sizeSpeed
    '        End If
    '        If Game.isKeyPressed(Keys.PageDown) Then
    '            size.X -= sizeSpeed
    '            size.Y -= sizeSpeed
    '        End If

    '        ' Rotate image (Q/E)
    '        If Game.isKeyPressed(Keys.Q) Then rotation -= rotateSpeed
    '        If Game.isKeyPressed(Keys.E) Then rotation += rotateSpeed

    '        ' Clamp values
    '        position.X = Math.Clamp(position.X, 0.0F, 1.0F)
    '        position.Y = Math.Clamp(position.Y, 0.0F, 1.0F)
    '        size.X = Math.Clamp(size.X, 0.01F, 2.0F)
    '        size.Y = Math.Clamp(size.Y, 0.01F, 2.0F)
    '        rotation = (rotation + 360.0F) Mod 360.0F

    '        ' Save current settings (End key)
    '        If Game.isKeyPressed(Keys.End) Then SaveCurrentSettings()
    '    End Sub

    '    Private Sub SaveCurrentSettings()
    '        Try
    '            Dim line As String = $"[NativeDrawImage Saved @ {DateTime.Now:HH:mm:ss}]" & vbCrLf &
    '                                 $"Position = X:{position.X:F4}, Y:{position.Y:F4}" & vbCrLf &
    '                                 $"Size = W:{size.X:F4}, H:{size.Y:F4}" & vbCrLf &
    '                                 $"Rotation = {rotation:F2}" & vbCrLf &
    '                                 $"Alpha = {alpha}" & vbCrLf &
    '                                 $"--------------------------------------------------" & vbCrLf

    '            File.AppendAllText(savePath, line)
    '            Game.DisplayText("Saved image debug data to: " & Path.GetFileName(savePath), 2500)
    '        Catch ex As Exception
    '            Game.DisplayText("Error saving image data: " & ex.Message, 2500)
    '        End Try
    '    End Sub

    '    '---------------------------------------------
    '    ' 🔹 Properties
    '    '---------------------------------------------
    '    Public Property Visible As Boolean
    '        Get
    '            Return isVisible
    '        End Get
    '        Set(value As Boolean)
    '            isVisible = value
    '        End Set
    '    End Property

    '    Public Property Position As Vector2
    '        Get
    '            Return Position
    '        End Get
    '        Set(value As Vector2)
    '            position = value
    '        End Set
    '    End Property

    '    Public Property Size As Vector2
    '        Get
    '            Return Size
    '        End Get
    '        Set(value As Vector2)
    '            size = value
    '        End Set
    '    End Property

    '    Public Property Rotation As Single
    '        Get
    '            Return Rotation
    '        End Get
    '        Set(value As Single)
    '            rotation = value
    '        End Set
    '    End Property

    '    Public Property ImageColor As Color
    '        Get
    '            Return color
    '        End Get
    '        Set(value As Color)
    '            color = value
    '        End Set
    '    End Property

    '    Public ReadOnly Property CurrentAlpha As Integer
    '        Get
    '            Return alpha
    '        End Get
    '    End Property

    '    Public ReadOnly Property IsCompletelyFadedOut As Boolean
    '        Get
    '            Return alpha <= 0
    '        End Get
    '    End Property

    '    Public ReadOnly Property IsCompletelyFadedIn As Boolean
    '        Get
    '            Return alpha >= 255
    '        End Get
    '    End Property

    '    Public Property DebugMode As Boolean
    '        Get
    '            Return debugEnabled
    '        End Get
    '        Set(value As Boolean)
    '            debugEnabled = value
    '        End Set
    '    End Property

    '    '---------------------------------------------
    '    ' 🔹 Management
    '    '---------------------------------------------
    '    Public Sub Dispose()
    '        activeImages.Remove(Me)
    '        texture.Dispose()
    '    End Sub

    '    Private Shared Sub Tick()
    '        For Each img As NativeDrawImage In activeImages
    '            img.ProcessFade()
    '            img.ProcessDebug()
    '            img.Draw()
    '        Next
    '    End Sub

    '    Private Shared Sub AttachTick()
    '        If isTickAttached Then Return
    '        TickHelper.Add(AddressOf Tick)
    '        isTickAttached = True
    '    End Sub

    'End Class

    '-----------------------------------------------------------------------------------------------------------------------------------------------------
    'TICK Helper


    Friend Class DrawManager

        Private Shared ReadOnly ActiveDraws As New List(Of DrawRequest)
        Private Shared isRunning As Boolean = False

        Public Shared Sub DisplayTextAdvanced(text As String, duration As Integer, pos As Vector2, style As TextStyle)

            Dim req As New DrawRequest(text, duration, pos, style)
            ActiveDraws.Add(req)

            If Not isRunning Then
                isRunning = True
                TickHelper.Add_Internal(AddressOf Tick)
            End If
        End Sub
        Public Shared Sub DrawRect(pos As Vector2, size As Vector2, color As ColorRGBA, duration As Integer)

            Dim req As New DrawRequest(pos, size, duration, color)
            ActiveDraws.Add(req)

            If Not isRunning Then
                isRunning = True
                TickHelper.Add_Internal(AddressOf Tick)
            End If
        End Sub
        Public Shared Sub DrawGameTexture(Texture As GameTexture, pos As Vector2, size As Vector2, rot As Single, color As ColorRGBA, duration As Integer)
            Dim req As New DrawRequest(Texture, pos, size, rot, color, duration)
            ActiveDraws.Add(req)

            If Not isRunning Then
                isRunning = True
                TickHelper.Add_Internal(AddressOf Tick)
            End If
        End Sub
        Public Shared Sub DrawCurvedWindow(pos As Vector2, size As Vector2, alpha As Integer, duration As Integer)

            Dim req As New DrawRequest(pos, size, duration, alpha)
            ActiveDraws.Add(req)

            If Not isRunning Then
                isRunning = True
                TickHelper.Add_Internal(AddressOf Tick)
            End If
        End Sub
        Public Shared Sub DrawFrontBuff(pos As Vector2, size As Vector2, rot As Single, color As ColorRGBA, duration As Integer)

            Dim req As New DrawRequest(pos, size, duration, rot, color)
            ActiveDraws.Add(req)

            If Not isRunning Then
                isRunning = True
                TickHelper.Add_Internal(AddressOf Tick)
            End If
        End Sub
        '-----------------------------------------------------------------------------------------------------------------------------
        Private Shared Sub Tick()
            ' rimuove tutti i testi scaduti
            ActiveDraws.RemoveAll(Function(t) t.IsExpired)

            ' se non ci sono testi attivi → disattiva tick
            If ActiveDraws.Count = 0 Then
                isRunning = False
                TickHelper.Remove_Internal(AddressOf Tick)
                Return
            End If

            For Each t In ActiveDraws
                'TextAdvanced
                If t.Type = DrawRequest.DrawType.TextAdvanced Then
                    TextStyle.SetTextStyle(t.Style)
                    [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", t.Position.X, t.Position.Y, "STRING", t.Text)
                End If
                'Rectangle
                If t.Type = DrawRequest.DrawType.Rectangle Then
                    [Call]("DRAW_RECT", t.Position.X, t.Position.Y, t.Size.X, t.Size.Y, t.Color_NoText.Red, t.Color_NoText.Green, t.Color_NoText.Blue, t.Color_NoText.Alpha)
                End If
                'CurvedWindows
                If t.Type = DrawRequest.DrawType.CurvedWindow Then
                    [Call]("DRAW_CURVED_WINDOW", t.Position.X, t.Position.Y, t.Size.X, t.Size.Y, t.CurvedWindowAlpha)
                End If
                'Game Texture
                If t.Type = DrawRequest.DrawType.GameTexture Then
                    [Call]("DRAW_SPRITE", t.GameTexture.TextureHandler, t.Position.X, t.Position.Y, t.Size.X, t.Size.Y, t.Rotation, t.Color_NoText.Red, t.Color_NoText.Green, t.Color_NoText.Blue, t.Color_NoText.Alpha)
                End If
                'FrontBuff
                If t.Type = DrawRequest.DrawType.FrontBuff Then
                    [Call]("DRAW_SPRITE_FRONT_BUFF", t.Position.X, t.Position.Y, t.Size.X, t.Size.Y, t.Rotation, t.Color_NoText.Red, t.Color_NoText.Green, t.Color_NoText.Blue, t.Color_NoText.Alpha)
                End If
            Next
        End Sub
        Private Class DrawRequest
            Public Enum DrawType
                TextAdvanced = 0
                Rectangle = 1
                CurvedWindow = 2
                FrontBuff = 3
                GameTexture = 4
            End Enum
            Public ReadOnly Text As String
            Public ReadOnly Position As Vector2
            Public ReadOnly Style As TextStyle
            Public ReadOnly Duration As Integer
            Public ReadOnly StartTime As Integer
            Public ReadOnly Type As DrawType
            Public ReadOnly Size As Vector2
            Public ReadOnly Color_NoText As ColorRGBA
            Public ReadOnly CurvedWindowAlpha As Integer
            Public ReadOnly GameTexture As GameTexture
            Public ReadOnly Rotation As Single

            Public Sub New(text As String, duration As Integer, pos As Vector2, style As TextStyle)
                Me.Text = text
                Position = pos
                Me.Style = style
                Me.Duration = duration
                StartTime = NativeTimer.TotalGameTime
                Type = DrawType.TextAdvanced
            End Sub
            Public Sub New(RectPos As Vector2, RectSize As Vector2, Duration As Integer, Color As ColorRGBA)
                Position = RectPos
                Size = RectSize
                Me.Duration = Duration
                Color_NoText = Color
                StartTime = NativeTimer.TotalGameTime
                Type = DrawType.Rectangle
            End Sub
            Public Sub New(FrontBuffPos As Vector2, FrontBuffSize As Vector2, Duration As Integer, rotation As Single, Color As ColorRGBA)
                Position = FrontBuffPos
                Size = FrontBuffSize
                Me.Duration = Duration
                Color_NoText = Color
                Me.Rotation = rotation
                StartTime = NativeTimer.TotalGameTime
                Type = DrawType.FrontBuff
            End Sub
            Public Sub New(Texture As GameTexture, GameTexturePos As Vector2, GameTextureSize As Vector2, Rotation As Single, Color As ColorRGBA, Duration As Integer)
                Position = GameTexturePos
                Size = GameTextureSize
                Me.Duration = Duration
                Color_NoText = Color
                StartTime = NativeTimer.TotalGameTime
                GameTexture = Texture
                Me.Rotation = Rotation
                Type = DrawType.GameTexture
            End Sub
            Public Sub New(CurvWindowsPos As Vector2, CurvWindowsSize As Vector2, Duration As Integer, Alpha As Integer)
                Position = CurvWindowsPos
                Size = CurvWindowsSize
                Me.Duration = Duration
                CurvedWindowAlpha = Alpha
                StartTime = NativeTimer.TotalGameTime
                Type = DrawType.CurvedWindow
            End Sub
            Public ReadOnly Property IsExpired As Boolean
                Get
                    Return (NativeTimer.TotalGameTime - StartTime) >= Duration
                End Get
            End Property
        End Class
    End Class

End Class

'Public Shared Function AddSphere(Coordinates As GTA.Vector3, Radius As Single, Unknown As Integer) As Integer
'    Return [Call](Of Integer)("ADD_SPHERE", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Unknown)
'End Function
'Public Shared Sub DrawSphere(X As Single, Y As Single, Z As Single, Radius As Single)
'    [Call]("DRAW_SPHERE", X, Y, Z, Radius)
'End Sub