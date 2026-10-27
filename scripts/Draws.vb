Imports System.Net.Security
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Xml.Schema
Imports GTA
Imports GTA.Native.Function
Imports NativeExtended.NativeDraws.GXT
Imports NativeExtended.NativeDraws.TextStyle
Imports NativeExtended.Tools
Public NotInheritable Class NativeDraws
    Private Sub New()
    End Sub
    Private Shared HelpTextStyle As New TextStyle(Color:=ColorRGBA.White, Size:=21, Justification:=NativeDraws.Justification.Left,
                                           BorderColor:=ColorRGBA.Black, Font:=Fonts.Standard, Shadow:=False, WidthScale:=0.8606F)
    Private Shared HelpTextPosition As New Vector2(0.054, 0.055)
    Private Shared HelpTextWrap As (Length As Single, LineHeightMultiplier As Single) = (Length:=0.202, LineHeightMultiplier:=1.0)
    Private Const HelpTextBoxLeft As Single = 0.05F
    Private Const BackgroundHorizontalPadding As Single = 0.006F
    Private Const BackgroundVerticalPadding As Single = 0.005F
    Private Const HelpTextRightPadding As Single = 0.02F
    Private Shared _lockSettingStyle As New Object
    ''' <summary>All native justifications for texts.</summary>
    Public Enum Justification
        Left = 0
        Center = 1
        ''' <summary>Needs a wrap to work properly.</summary>
        Right = 2
        ''' <summary>Needs a wrap to work properly.</summary>
        Justify = 3
    End Enum
    ''' <summary> Native values. There are also values between 4 and 7, which changes position Y. All of them are natively independent.</summary>
    Public Enum BigPrintType
        Center_GtaNumbers = 1
        ''' <summary>
        ''' Hides car name, street and zone.
        ''' </summary>
        BottomRight_Narrow = 2
        Center_Standard = 3
    End Enum
    Public Enum QueueMode
        DisplayNow = 0
        AddToQueue = 1
        IgnoreIfBusy = 2
    End Enum
    '======================================================================================================================================================================
    'Custom Texts
    '-------------
    ''' <summary>
    ''' Clears every custom PrintBigText currently displayed or queued, optionally clearing
    ''' native one too (type 1 to 3).
    ''' </summary>
    Public Shared Sub ClearAllBigPrints(Optional ClearNative As Boolean = False)
        DrawManager.ClearAllBigPrintTexts()
        If ClearNative Then
            Dim i As Integer
            For i = 1 To 3
                [Call]("PRINT_BIG", "STRING", 1, i)
            Next
        End If
    End Sub
    ''' <summary>
    ''' Clears the custom PrintBigText associated with the specified type,
    ''' including all queued messages of that type. Optionally clears the
    ''' corresponding native slot.
    ''' </summary>
    ''' <param name="Type">The BigPrintType to clear.</param>
    Public Shared Sub ClearBigPrint(Type As BigPrintType, Optional ClearNative As Boolean = False)
        DrawManager.ClearBigPrintText(Type)
        If ClearNative Then GXT.ClearBigPrint(Type)
    End Sub
    ''' <summary>
    ''' Clears all custom helps in queue, optionally clearing native one too.
    ''' </summary>
    ''' <param name="ClearNative">If true will call GXT.HelpMessage.Clear</param>
    Public Shared Sub ClearHelp(Optional ClearNative As Boolean = False)
        DrawManager.ClearHelpRequests()
        If ClearNative Then GXT.ClearHelp()
    End Sub
    ''' <summary>
    ''' Draws a 2D debug grid with center displayed on bottom of screen.
    ''' You can move with arrows (Left Shift to increase precision).
    ''' </summary>
    Public Shared Property DrawDebugGrid As Boolean
        Get
            Return _drawDebugGrid
        End Get
        Set(value As Boolean)
            If _drawDebugGrid = value Then Return

            _drawDebugGrid = value
            If value Then
                StartDebugGrid()
            Else
                StopDebugGrid()
            End If
        End Set
    End Property
    ''' <summary>
    ''' Display a custom message (bottom-center of screen, like a subtitle).
    '''</summary>
    ''' <param name="BriefDisplay">If False the message won't be added to Briefing</param>
    ''' <param name="DisplayNow">If False the message will be added to a queue and displayed only if there aren't other messages on screen</param>
    Public Shared Sub DisplayText(Text As String, Optional Duration As Integer = 4000, Optional BriefDisplay As Boolean = False, Optional DisplayNow As Boolean = True)
        Dim UnknownFlag As Integer = 1
        If Duration < 5 Then Duration = 5
        [Call]("ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS", BriefDisplay)
        If DisplayNow Then
            [Call]("PRINT_STRING_WITH_LITERAL_STRING_NOW", "STRING", Text, Duration, UnknownFlag)
        Else
            [Call]("PRINT_STRING_WITH_LITERAL_STRING", "STRING", Text, Duration, UnknownFlag)
        End If
    End Sub
    '======================================================================================================================================================================
    ''' <summary>
    ''' Draws a line (horizontal or vertical) using DRAW_RECT.
    ''' Optimized using the current aspect ratio.
    ''' </summary>
    ''' <param name="Position">Line position.</param>
    ''' <param name="Length">Line length (from 0.0 to 1.0).</param>
    ''' <param name="Thickness">Line thickness (suggested 0.002 -> 0.01).</param>
    ''' <param name="Color">RGBA color.</param>
    ''' <param name="Vertical">If True draws a vertical line, otherwise horizontal.</param>
    ''' <param name="Center">If True the position is the center of the line, otherwise it is the start.</param>
    ''' <param name="Duration">Draw duration (>=5 uses DrawManager).</param>
    Public Shared Sub DrawLine(Position As Vector2, Length As Single, Optional Thickness As Single = 0.005F, Optional Color As ColorRGBA = Nothing,
                               Optional Vertical As Boolean = False, Optional Center As Boolean = False, Optional Duration As Integer = 1)

        If Length <= 0.0F OrElse Thickness <= 0.0F Then Exit Sub

        If Color = Nothing Then Color = ColorRGBA.White
        Dim aspect As Single = Tools.ScreenAspectRatio ' [Call](Of Single)("GET_ASPECT_RATIO")

        Dim sizeX As Single
        Dim sizeY As Single
        Dim centerX As Single = Position.X
        Dim centerY As Single = Position.Y

        If Vertical Then
            sizeX = Thickness
            sizeY = Length

            If Not Center Then centerY += sizeY / 2.0F
        Else
            sizeX = Length
            sizeY = Thickness * aspect

            If Not Center Then centerX += sizeX / 2.0F
        End If

        If Duration < 5 Then
            [Call]("DRAW_RECT", centerX, centerY, sizeX, sizeY, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        Else
            DrawManager.DrawRect(New Vector2(centerX, centerY), New Vector2(sizeX, sizeY), Color, Duration)
        End If
    End Sub
    ''' <summary>
    ''' Draws a rectangle with optional borders.
    ''' You must call TickHelper.ProcessAll in a Tick to set the display duration. Please note that this function calls 5 rectangles, so it could be heavy while drawing many elements.
    ''' </summary>
    ''' <param name="PercentagePos">Rectangle position.</param>
    ''' <param name="PercentageSize">Rectangle size.</param>
    ''' <param name="Color">Fill color.</param>
    ''' <param name="Duration">Draw duration.</param>
    ''' <param name="CenteredX">If True X position is centered.</param>
    ''' <param name="CenteredY">If True Y position is centered.</param>
    ''' <param name="BordersColor">Border color (defaults to black).</param>
    ''' <param name="BordersThickness">Border thickness (0 = no borders; suggested values from 0.002 to 0.01).</param>
    Public Shared Sub DrawRectangle(PercentagePos As Vector2, PercentageSize As Vector2, Color As ColorRGBA, Optional CenteredX As Boolean = True,
                                    Optional CenteredY As Boolean = True, Optional Duration As Integer = 1,
                                    Optional BordersThickness As Single = 0.0F, Optional BordersColor As ColorRGBA = Nothing)

        If PercentageSize.X <= 0.0F OrElse PercentageSize.Y <= 0.0F Then Return

        Dim centerX As Single = PercentagePos.X
        Dim centerY As Single = PercentagePos.Y

        Dim sizeX As Single = PercentageSize.X
        Dim sizeY As Single = PercentageSize.Y

        If Not CenteredX Then centerX += sizeX / 2.0F
        If Not CenteredY Then centerY += sizeY / 2.0F

        If Duration < 5 Then
            [Call]("DRAW_RECT", centerX, centerY, sizeX, sizeY, Color.Red, Color.Green, Color.Blue, Color.Alpha)

            If BordersThickness > 0.0F Then
                Dim aspect As Single = Tools.ScreenAspectRatio '[Call](Of Single)("GET_ASPECT_RATIO")
                If BordersColor = Nothing Then BordersColor = ColorRGBA.Black
                Dim thicknessX As Single = BordersThickness
                Dim thicknessY As Single = BordersThickness * aspect

                Dim halfX As Single = sizeX / 2.0F
                Dim halfY As Single = sizeY / 2.0F

                Dim halfThicknessX As Single = thicknessX / 2.0F
                Dim halfThicknessY As Single = thicknessY / 2.0F

                Dim horizontalWidth As Single = sizeX + thicknessX * 2
                Dim horizontalCenterX As Single = centerX
                Dim topCenterY As Single = centerY - halfY - halfThicknessY
                Dim bottomCenterY As Single = centerY + halfY + halfThicknessY

                [Call]("DRAW_RECT", horizontalCenterX, topCenterY, horizontalWidth, thicknessY, BordersColor.Red, BordersColor.Green, BordersColor.Blue, BordersColor.Alpha)
                [Call]("DRAW_RECT", horizontalCenterX, bottomCenterY, horizontalWidth, thicknessY, BordersColor.Red, BordersColor.Green, BordersColor.Blue, BordersColor.Alpha)

                Dim verticalHeight As Single = sizeY
                Dim leftCenterX As Single = centerX - halfX - halfThicknessX
                Dim rightCenterX As Single = centerX + halfX + halfThicknessX
                Dim verticalCenterY As Single = centerY

                [Call]("DRAW_RECT", leftCenterX, verticalCenterY, thicknessX, verticalHeight, BordersColor.Red, BordersColor.Green, BordersColor.Blue, BordersColor.Alpha)
                [Call]("DRAW_RECT", rightCenterX, verticalCenterY, thicknessX, verticalHeight, BordersColor.Red, BordersColor.Green, BordersColor.Blue, BordersColor.Alpha)
            End If
        Else
            DrawManager.DrawRect(New Vector2(centerX, centerY), PercentageSize, Color, Duration, BordersThickness, BordersColor)
        End If
    End Sub
    ''' <summary>
    ''' Draws a square with optional borders using "DRAW_RECT".
    ''' The specified size represents the square height in screen percentage.
    ''' The width is automatically adjusted using the current aspect ratio.
    ''' You must call TickHelper.ProcessAll in a Tick to set the display duration.
    ''' </summary>
    ''' <param name="PercentagePos">Square position.</param>
    ''' <param name="Size">Square size based on screen height.</param>
    ''' <param name="Color">Fill color.</param>
    ''' <param name="Duration">Draw duration.</param>
    ''' <param name="CenteredX">If True X position is centered.</param>
    ''' <param name="CenteredY">If True Y position is centered.</param>
    ''' <param name="BordersColor">Border color (defaults to black).</param>
    ''' <param name="BordersThickness">Border thickness (0 = no borders; suggested values from 0.002 to 0.01).</param>
    Public Shared Sub DrawSquare(PercentagePos As Vector2, Size As Single, Color As ColorRGBA, Optional CenteredX As Boolean = True,
                                 Optional CenteredY As Boolean = True, Optional Duration As Integer = 1, Optional BordersThickness As Single = 0.0F,
                                 Optional BordersColor As ColorRGBA = Nothing)

        If Size <= 0.0F Then Exit Sub
        Dim aspect As Single = Tools.ScreenAspectRatio
        If aspect <= 0.0F Then Return
        Dim SquareSize As New Vector2(Size / aspect, Size)

        DrawRectangle(PercentagePos, SquareSize, Color, Duration, CenteredX, CenteredY, BordersThickness, BordersColor)
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
    '=========================================================================================================================================================================================
    ''' <summary>
    ''' Draws text using a custom style and normalized screen position.
    ''' Durations shorter than 5 milliseconds draw immediately.
    ''' Longer durations are managed by DrawManager and require
    ''' TickHelper.ProcessAll to be called from a Tick.
    ''' Returns the measured text width and total line-box height.
    ''' The returned height includes the line-height multiplier,
    ''' but does not include the optional background padding.
    ''' </summary>
    ''' <param name="Text">The text to display.</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    ''' <param name="PositionPercent">Normalized screen position.</param>
    ''' <param name="Style">Optional text style.</param>
    ''' <param name="Wrap">Optional wrapping length and line-height multiplier.</param>
    ''' <param name="ViewportID">Optional native text viewport identifier.</param>
    ''' <param name="RenderID">Optional native text render identifier.</param>
    ''' <returns>
    ''' A tuple containing Width and Height, in normalized screen coordinates.
    ''' </returns>
    Public Shared Function DrawText(Text As String, PositionPercent As Vector2, Optional Duration As Integer = 3000, Optional Style As TextStyle = Nothing,
                                    Optional Wrap As (Length As Single?, LineHeightMultiplier As Single?) = Nothing, Optional ViewportID As Integer? = Nothing,
                                    Optional RenderID As Integer? = Nothing) As (Width As Single, Height As Single, LineCount As Integer)

        Dim FinalText As String = If(Text, String.Empty)
        Dim FinalStyle As TextStyle
        If Style Is Nothing Then FinalStyle = TextStyle.Default Else FinalStyle = Style

        If PositionPercent = Nothing Then PositionPercent = New Vector2(0.5F, 0.9F)

        Dim X As Single = PositionPercent.X
        Dim FinalWrap As (StartPos As Single, EndPos As Single, LineHeightMultiplier As Single)

        If Wrap.LineHeightMultiplier.HasValue Then
            FinalWrap.LineHeightMultiplier =
            ClampFloat(Wrap.LineHeightMultiplier.Value, 0.7F, 10.0F)
        Else
            FinalWrap.LineHeightMultiplier = 1.0F
        End If

        If Wrap.Length.HasValue Then
            Dim RequestedLength As Single = Math.Max(0.0F, Wrap.Length.Value)

            Select Case FinalStyle.Justification
                Case Justification.Center
                    FinalWrap.StartPos = X - (RequestedLength / 2.0F)
                    FinalWrap.EndPos = X + (RequestedLength / 2.0F)

                Case Justification.Right
                    FinalWrap.StartPos = X - RequestedLength
                    FinalWrap.EndPos = X

                Case Justification.Justify
                    FinalWrap.StartPos = X
                    FinalWrap.EndPos = X + RequestedLength

                Case Else
                    FinalWrap.StartPos = X
                    FinalWrap.EndPos = X + RequestedLength
            End Select
        Else
            FinalWrap.StartPos = 0.0F
            FinalWrap.EndPos = 1.0F
        End If

        Dim SingleLineHeight As Single = 0.0F
        SingleLineHeight = TextStyle.SetTextStyle(FinalStyle, FinalWrap, ViewportID, RenderID)

        Dim ExplicitLines As String() = Nothing
        Dim ExplicitLineCounts As Integer() = Nothing

        Dim StringWidth As Single = 0.0F
        Dim NumberOfLines As Integer = 0

        If FinalText.IndexOf(TextColors.FX_LineBreak, StringComparison.Ordinal) >= 0 Then

            ExplicitLines = SplitTextAtLineBreak(FinalText)

            ReDim ExplicitLineCounts(ExplicitLines.Length - 1)

            For i As Integer = 0 To ExplicitLines.Length - 1

                Dim CurrentWidth As Single =
            GetLiteralTextLineWidth(ExplicitLines(i))

                StringWidth = Math.Max(StringWidth, CurrentWidth)

                ExplicitLineCounts(i) = GetLiteralTextLineCount(
            ExplicitLines(i),
            FinalWrap.StartPos,
            FinalWrap.EndPos)

                NumberOfLines += ExplicitLineCounts(i)

            Next

        Else
            StringWidth = [Call](Of Single)("GET_STRING_WIDTH_WITH_STRING", "STRING", FinalText)
            If FinalText.Length > 0 Then
                NumberOfLines = [Call](Of Integer)("GET_NUMBER_LINES_WITH_LITERAL_STRINGS", FinalWrap.StartPos, FinalWrap.EndPos, "STRING", "", FinalText)
                NumberOfLines = Math.Max(1, NumberOfLines)
            End If
        End If

        Dim TotalHeight As Single = CSng(NumberOfLines) * (SingleLineHeight * FinalWrap.LineHeightMultiplier)
        Dim BackgroundSize As New Vector2(0.0F, 0.0F)
        Dim BackgroundPosition As Vector2? = Nothing

        If FinalStyle.Background.Alpha > 0 AndAlso NumberOfLines > 0 Then

            Dim BackgroundWidth As Single

            If FinalStyle.Justification = NativeDraws.Justification.Justify Then

                ' L'intero wrap, non soltanto la larghezza del testo.
                BackgroundWidth = Math.Max(
            0.0F,
            FinalWrap.EndPos - FinalWrap.StartPos)

            Else

                BackgroundWidth = Math.Max(0.0F, StringWidth)

                If Wrap.Length.HasValue AndAlso Wrap.Length.Value > 0.0F Then
                    BackgroundWidth = Math.Min(
                BackgroundWidth,
                Wrap.Length.Value)
                End If

            End If

            BackgroundSize = New Vector2(
        BackgroundWidth + (BackgroundHorizontalPadding * 2.0F),
        TotalHeight + (BackgroundVerticalPadding * 2.0F))

            If BackgroundSize.X > 0.0F Then

                BackgroundPosition = CalculateTextBackgroundPosition(
            PositionPercent,
            BackgroundSize,
            FinalStyle.Justification,
            FinalWrap.StartPos,
            FinalWrap.EndPos,
            HorizontalPadding:=BackgroundHorizontalPadding)

            End If

        End If

        If Duration < 5 Then
            If BackgroundPosition.HasValue Then DrawRectangle(BackgroundPosition.Value, BackgroundSize, FinalStyle.Background, Duration:=1)

            If ExplicitLines IsNot Nothing Then

                RenderLiteralTextLines(
        ExplicitLines,
        PositionPercent,
        FinalStyle,
        FinalWrap,
        ViewportID,
        RenderID,
        ExplicitLineCounts)

            Else

                TextStyle.SetTextStyle(
        FinalStyle,
        FinalWrap,
        ViewportID,
        RenderID)

                [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", PositionPercent.X, PositionPercent.Y, "STRING", FinalText)
            End If
        Else
            DrawManager.DrawText(FinalText, Duration, PositionPercent, FinalStyle, BackgroundSize, FinalWrap, ViewportID, RenderID, BackgroundPosition)
        End If

        Return (Width:=StringWidth, Height:=TotalHeight, LineCount:=NumberOfLines)
    End Function
    '=========================================================================================================================================================================================
    ''' <summary>
    ''' Draws a rectangular window with optional borders and title area.
    ''' The position is the top-left anchor by default.
    ''' CenteredX and CenteredY can be enabled independently.
    ''' For durations of at least 5 milliseconds, the window and title
    ''' area share a single DrawManager request and the same lifetime.
    ''' </summary>
    ''' <param name="PercentagePos">The normalized window position.</param>
    ''' <param name="PercentageSize">The normalized window size.</param>
    ''' <param name="Color">The window fill color. Defaults to translucent black.</param>
    ''' <param name="Duration">The requested display duration in milliseconds.</param>
    ''' <param name="CenteredX">If True, X specifies the horizontal center.</param>
    ''' <param name="CenteredY">If True, Y specifies the vertical center.</param>
    ''' <param name="TitleLabelHeight">Optional height of the title area.</param>
    Public Shared Sub DrawWindow(PercentagePos As Vector2, PercentageSize As Vector2, Optional Color As ColorRGBA = Nothing, Optional CenteredX As Boolean = False,
                                 Optional CenteredY As Boolean = False, Optional Duration As Integer = 1, Optional TitleLabelHeight As Single = 0.0F)
        If PercentageSize.X <= 0.0F OrElse PercentageSize.Y <= 0.0F Then Return

        Dim FinalColor As ColorRGBA
        If Color = Nothing Then
            FinalColor = New ColorRGBA(0, 0, 0, 180)
        Else
            FinalColor = Color
        End If

        Dim BorderColor As ColorRGBA = FinalColor.Darken(1.7F)

        BorderColor.Alpha =
        FinalColor.Alpha +
        ((255 - FinalColor.Alpha) \ 2)

        Dim FinalTitleHeight As Single = 0.0F

        If TitleLabelHeight > 0.0F Then
            FinalTitleHeight =
            Math.Min(TitleLabelHeight, PercentageSize.Y)
        End If

        Dim CenterPosition As Vector2 = PercentagePos

        If Not CenteredX Then CenterPosition.X += PercentageSize.X / 2.0F
        If Not CenteredY Then CenterPosition.Y += PercentageSize.Y / 2.0F

        If Duration < 5 Then
            DrawRectangle(CenterPosition, PercentageSize, FinalColor, Duration, True, True, 0.00165F, BorderColor)

            If FinalTitleHeight > 0.0F Then
                Dim TitleCenterY As Single = CenterPosition.Y - (PercentageSize.Y / 2.0F) + (FinalTitleHeight / 2.0F)
                DrawRectangle(New Vector2(CenterPosition.X, TitleCenterY), New Vector2(PercentageSize.X, FinalTitleHeight), FinalColor, Duration, True, True, 0.00165F, BorderColor)
            End If
        Else
            DrawManager.DrawWindow(CenterPosition, PercentageSize, FinalColor, Duration, BorderColor, 0.00165F, FinalTitleHeight)
        End If
    End Sub
    ''' <summary>
    ''' Draws a curved window with an optional title area.
    ''' The native position represents the top-left corner.
    ''' CenteredX and CenteredY can be enabled independently.
    ''' For durations of at least 5 milliseconds, the window and title
    ''' area share a single DrawManager request and the same lifetime.
    ''' </summary>
    ''' <param name="PercentagePos">
    ''' The normalized position. Each coordinate represents the top-left
    ''' corner by default, or the center when its Centered option is True.
    ''' </param>
    ''' <param name="PercentageSize">The normalized window size.</param>
    ''' <param name="Alpha">The window opacity, from 0 to 255.</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    ''' <param name="CenteredX">If True, X specifies the horizontal center.</param>
    ''' <param name="CenteredY">If True, Y specifies the vertical center.</param>
    ''' <param name="TitleLabelHeight">Optional height of the title area.</param>
    Public Shared Sub DrawWindowCurved(PercentagePos As Vector2, PercentageSize As Vector2, Optional Alpha As Byte = 180, Optional CenteredX As Boolean = False,
                                       Optional CenteredY As Boolean = False, Optional Duration As Integer = 1, Optional TitleLabelHeight As Single = 0.0F)

        If PercentageSize.X <= 0.0F OrElse PercentageSize.Y <= 0.0F Then Return

        Dim TopLeftPosition As Vector2 = PercentagePos
        If CenteredX Then TopLeftPosition.X -= PercentageSize.X / 2.0F
        If CenteredY Then TopLeftPosition.Y -= PercentageSize.Y / 2.0F

        Dim FinalTitleHeight As Single = 0.0F
        If TitleLabelHeight > 0.0F Then FinalTitleHeight = Math.Min(TitleLabelHeight, PercentageSize.Y)

        If Duration < 5 Then
            [Call]("DRAW_CURVED_WINDOW", TopLeftPosition.X, TopLeftPosition.Y, PercentageSize.X, PercentageSize.Y, CInt(Alpha))
            If FinalTitleHeight > 0.0F Then
                [Call]("DRAW_CURVED_WINDOW", TopLeftPosition.X, TopLeftPosition.Y, PercentageSize.X, FinalTitleHeight, CInt(Alpha))
            End If
        Else
            DrawManager.DrawCurvedWindow(TopLeftPosition, PercentageSize, CInt(Alpha), Duration, FinalTitleHeight)
        End If
    End Sub
    ''' <summary>
    ''' Draws a square window with optional borders and title area.
    ''' Size is expressed as a percentage of screen height.
    ''' The width is adjusted using the current screen aspect ratio.
    ''' The position is top-left anchored by default.
    ''' </summary>
    ''' <param name="PercentagePos">The normalized window position.</param>
    ''' <param name="Size">Square size relative to screen height.</param>
    ''' <param name="Color">The window fill color.</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    ''' <param name="CenteredX">If True, the position is the horizontal center.</param>
    ''' <param name="CenteredY">If True, the position is the vertical center.</param>
    ''' <param name="TitleLabelHeight">Optional title area height.</param>
    Public Shared Sub DrawWindowSquare(PercentagePos As Vector2, Size As Single, Optional Color As ColorRGBA = Nothing, Optional CenteredX As Boolean = False,
                                       Optional CenteredY As Boolean = False, Optional Duration As Integer = 1, Optional TitleLabelHeight As Single = 0.0F)

        If Size <= 0.0F Then Return
        Dim Aspect As Single = Tools.ScreenAspectRatio
        If Aspect <= 0.0F Then Return
        Dim SquareSize As New Vector2(Size / Aspect, Size)
        DrawWindow(PercentagePos, SquareSize, Color, Duration, CenteredX, CenteredY, TitleLabelHeight)
    End Sub
    '=========================================================================================================================================================================================
    '''' <summary>
    '''' Returns the native vertical line-box height for the specified native Y scale (DrawText functions).
    '''' </summary>
    '''' <param name="ScaleY">The native vertical text scale.</param>
    '''' <param name="LineHeightMultiplier">The line-height multiplier, clamped to the supported range.</param>
    '''' <returns>The vertical space allocated to one line, in normalized screen coordinates.</returns>
    'Public Shared Function GetLineHeight(ScaleY As Single, Optional LineHeightMultiplier As Single = 1.0F) As Single
    '    If ScaleY <= 0.0F OrElse Single.IsNaN(ScaleY) OrElse Single.IsInfinity(ScaleY) Then
    '        Return 0.0F
    '    End If

    '    Dim FinalMultiplier As Single = ClampFloat(LineHeightMultiplier, 0.7F, 10.0F)
    '    [Call]("SET_TEXT_SCALE", 1.0F, ScaleY)
    '    Dim NativeLineHeight As Single = [Call](Of Single)("GET_LINE_HEIGHT")
    '    Return Math.Max(0.0F, NativeLineHeight) * FinalMultiplier
    'End Function
    ''' <summary>
    ''' Displays up to three lines of custom text at the lower-center
    ''' portion of the screen.
    ''' The default style is white, centered text.
    ''' Repeated calls update the existing request and renew its
    ''' five-second lifetime instead of creating additional requests.
    ''' When calls stop, the last displayed text expires naturally
    ''' after five seconds.
    ''' </summary>
    ''' <param name="Text">The first line.</param>
    ''' <param name="Text2">Optional second line.</param>
    ''' <param name="Text3">Optional third line.</param>
    Public Shared Sub Print(Text As String, Optional Text2 As String = Nothing, Optional Text3 As String = Nothing)
        Dim Lines As New List(Of String)
        Lines.Add(If(Text, String.Empty))
        If Not String.IsNullOrEmpty(Text2) Then
            Lines.Add(Text2)
        End If
        If Not String.IsNullOrEmpty(Text3) Then
            Lines.Add(Text3)
        End If
        Dim ColouredLines(Lines.Count - 1) As String
        For i As Integer = 0 To Lines.Count - 1
            ColouredLines(i) = TextColors.MP_Yellow & Lines(i)
        Next
        Dim ColouredText As String = String.Join("~n~", ColouredLines)
        Dim Style As New TextStyle(Color:=ColorRGBA.White, Size:=12, Justification:=NativeDraws.Justification.Center, BorderColor:=ColorRGBA.Black, Font:=TextStyle.Fonts.Standard, Shadow:=False, WidthScale:=1.0F)
        Dim PositionY As Single = 0.946F
        DrawManager.DrawPersistentText("PrintText", ColouredText, 5000, New Vector2(0.5F, PositionY), Style, TextLines:=ColouredLines, LineSpacingY:=0.015F)
    End Sub
    ''' <summary>
    ''' Displays a custom text using a native PrintBig position and style.
    ''' Each BigPrintType has an independent queue.
    ''' When DisplayNow is False, the text is displayed immediately if
    ''' no custom message of the same type is active; otherwise it is
    ''' queued and displayed when the previous message expires.
    ''' When DisplayNow is True, the active custom message and all
    ''' queued messages of the same type are discarded, and the new
    ''' text is displayed immediately.
    ''' Other BigPrintType values are not affected.
    ''' </summary>
    ''' <param name="Text">The custom text to display.</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    ''' <param name="Type">The native position and display type to use.</param>
    ''' <param name="Color"> Optional custom color. When omitted, the native StringColor associated with the selected type is used.</param>
    ''' <param name="Font">Optional override for the native font.</param>
    ''' <param name="WidthCoef"> Optional horizontal width coefficient. When omitted, the coefficient associated with Type is used.</param>
    ''' <param name="QueueMode">If True, immediately replaces the active and queued custom messages of the same type.
    ''' </param>
    Public Shared Sub PrintBig(Text As String, Optional Duration As Integer = 5000, Optional Type As BigPrintType = BigPrintType.Center_Standard,
                                   Optional Color As ColorRGBA? = Nothing, Optional Font As TextStyle.Fonts? = Nothing, Optional WidthCoef As Single? = Nothing,
                                   Optional QueueMode As QueueMode? = QueueMode.AddToQueue)

        Dim DisplayString As String = If(Text, String.Empty)
        Dim Col As ColorRGBA = ColorRGBA.White
        Dim StringColor As String = String.Empty
        Dim SelectedFont As TextStyle.Fonts
        Dim FinalStyle As TextStyle
        Dim PositionX As Single = 0.5F
        Dim PositionY As Single = 0.5F
        Dim TextSize As Integer = 49
        Dim DefaultWidthCoef As Single = 1.0F
        Dim TextJustification As NativeDraws.Justification = NativeDraws.Justification.Center
        Dim BorderColor As ColorRGBA = Nothing

        Select Case Type
            Case BigPrintType.Center_GtaNumbers
                PositionX = 0.5F
                PositionY = 0.45F
                TextSize = 49
                TextJustification = NativeDraws.Justification.Center
                BorderColor = ColorRGBA.Black
                SelectedFont = TextStyle.Fonts.StandardUppercase_GtaNumbers
                DefaultWidthCoef = 0.86F
                If Color.HasValue Then
                    Col = Color.Value
                    StringColor = String.Empty
                Else
                    Col = ColorRGBA.White
                    StringColor = TextColors.Red
                End If

            Case BigPrintType.Center_Standard
                PositionX = 0.5F
                PositionY = 0.5F
                TextSize = 49
                TextJustification = NativeDraws.Justification.Center
                BorderColor = ColorRGBA.Black
                SelectedFont = TextStyle.Fonts.Standard
                DefaultWidthCoef = 1.37F
                If Color.HasValue Then
                    Col = Color.Value
                    StringColor = String.Empty
                Else
                    Col = ColorRGBA.White
                    StringColor = TextColors.Green
                End If

            Case BigPrintType.BottomRight_Narrow
                PositionX = 0.946F
                PositionY = 0.91F
                TextSize = 36
                TextJustification = NativeDraws.Justification.Left
                BorderColor = ColorRGBA.Black
                SelectedFont = TextStyle.Fonts.Narrow
                DefaultWidthCoef = 0.8F
                If Color.HasValue Then
                    Col = Color.Value
                    StringColor = String.Empty
                Else
                    Col = ColorRGBA.White
                    StringColor = TextColors.Gray
                End If

            Case Else
                Throw New ArgumentOutOfRangeException(NameOf(Type), Type, "The specified BigPrintType is not implemented by PrintBigText.")
        End Select
        '-----
        If Font.HasValue Then
            SelectedFont = Font.Value
        End If
        '--
        Dim FinalWidthCoef As Single = If(WidthCoef.HasValue, Math.Max(0.0F, WidthCoef.Value), DefaultWidthCoef)
        '--
        FinalStyle = New TextStyle(Color:=Col, Size:=TextSize, Justification:=TextJustification, BorderColor:=BorderColor, Font:=SelectedFont, Shadow:=False, WidthScale:=FinalWidthCoef)

        ' GXT.PrintBig("STRING", 2, Type)
        Dim FinalText As String = StringColor & DisplayString
        If Type = BigPrintType.BottomRight_Narrow Then

            TextStyle.SetTextStyle(FinalStyle, (StartPos:=0.0F, EndPos:=1.0F, LineHeightMultiplier:=1.0F))
            Dim StringWidth As Single = [Call](Of Single)("GET_STRING_WIDTH_WITH_STRING", "STRING", FinalText)
            PositionX = Math.Max(0.0F, 0.95F - StringWidth)
        End If

        DrawManager.DrawBigPrintText(
            FinalText,
            Duration,
            Type,
            New Vector2(PositionX, PositionY),
            FinalStyle,
            QueueMode)

    End Sub
    Public Shared Sub PrintHelp(Text As String, Optional Duration As Integer = 7000, Optional NoSound As Boolean = False, Optional DisplayNow As Boolean = False, Optional IgnoreIfQueued As Boolean = True, Optional Color As ColorRGBA? = Nothing, Optional Background As ColorRGBA? = Nothing, Optional QueueMode As QueueMode? = Nothing)

        If String.IsNullOrEmpty(Text) Then Return
        If Duration = 0 OrElse Duration < -1 Then Throw New ArgumentOutOfRangeException(NameOf(Duration), "Duration must be -1 (forever) or positive.")

        Dim Mode As QueueMode
        If QueueMode.HasValue Then
            Mode = QueueMode.Value
        ElseIf DisplayNow Then
            Mode = NativeDraws.QueueMode.DisplayNow
        ElseIf IgnoreIfQueued Then
            Mode = NativeDraws.QueueMode.IgnoreIfBusy
        Else
            Mode = NativeDraws.QueueMode.AddToQueue
        End If

        Dim FinalColor As ColorRGBA = If(Color.HasValue, Color.Value, HelpTextStyle.Color)
        Dim FinalBackground As ColorRGBA = If(Background.HasValue, Background.Value, New ColorRGBA(0, 0, 0, 180))
        Dim Style As TextStyle = HelpTextStyle.WithColor(FinalColor).WithBackground(FinalBackground)

        Dim FinalWrap As (StartPos As Single, EndPos As Single, LineHeightMultiplier As Single) = (StartPos:=HelpTextPosition.X, EndPos:=HelpTextPosition.X + HelpTextWrap.Length, LineHeightMultiplier:=ClampFloat(HelpTextWrap.LineHeightMultiplier, 0.7F, 10.0F))
        Dim NativeWrap As (StartPos As Single?, EndPos As Single?, LineHeightMultiplier As Single?) = (StartPos:=CType(FinalWrap.StartPos, Single?), EndPos:=CType(FinalWrap.EndPos, Single?), LineHeightMultiplier:=CType(FinalWrap.LineHeightMultiplier, Single?))

        Dim SingleLineHeight As Single = TextStyle.SetTextStyle(Style, NativeWrap)

        Dim StringWidth As Single = 0.0F
        Dim NumberOfLines As Integer = 0

        If Text.IndexOf(TextColors.FX_LineBreak, StringComparison.Ordinal) >= 0 Then

            Dim ExplicitLines As String() = SplitTextAtLineBreak(Text)

            For Each Line As String In ExplicitLines

                ' La larghezza complessiva è quella della linea più larga.
                Dim CurrentWidth As Single = GetLiteralTextLineWidth(Line)
                StringWidth = Math.Max(StringWidth, CurrentWidth)

                ' Conta anche le linee vuote e le eventuali righe
                ' aggiuntive dovute al wrapping nativo.
                NumberOfLines += GetLiteralTextLineCount(
            Line,
            FinalWrap.StartPos,
            FinalWrap.EndPos)

            Next

        Else

            ' Mantiene il comportamento originale se non ci sono line break.
            StringWidth =
        [Call](Of Single)(
            "GET_STRING_WIDTH_WITH_STRING",
            "STRING",
            Text)

            If Text.Length > 0 Then

                NumberOfLines =
            Math.Max(
                1,
                [Call](Of Integer)(
                    "GET_NUMBER_LINES_WITH_LITERAL_STRINGS",
                    FinalWrap.StartPos,
                    FinalWrap.EndPos,
                    "STRING",
                    "",
                    Text))

            End If

        End If

        Dim TotalHeight As Single = CSng(NumberOfLines) * SingleLineHeight * FinalWrap.LineHeightMultiplier
        Dim BackgroundSize As New Vector2(0.0F, 0.0F)
        Dim BackgroundPosition As Vector2? = Nothing

        If FinalBackground.Alpha > 0 AndAlso NumberOfLines > 0 Then

            Dim BackgroundWidth As Single

            If NumberOfLines = 1 Then
                BackgroundWidth = Math.Min(HelpTextWrap.Length, Math.Max(0.0F, StringWidth) + HelpTextRightPadding)
            Else
                BackgroundWidth = HelpTextWrap.Length
            End If
            BackgroundWidth += HelpTextPosition.X - HelpTextBoxLeft

            BackgroundSize = New Vector2(BackgroundWidth, TotalHeight + (BackgroundVerticalPadding * 2.0F))
            BackgroundSize = New Vector2(BackgroundWidth, TotalHeight + (BackgroundVerticalPadding * 2.0F))
            If BackgroundSize.X > 0.0F Then
                BackgroundPosition = CalculateTextBackgroundPosition(New Vector2(HelpTextBoxLeft, HelpTextPosition.Y), BackgroundSize, Justification.Left, FinalWrap.StartPos, FinalWrap.EndPos)
            End If
        End If

        DrawManager.DrawHelpText(Text, Duration, HelpTextPosition, Style, BackgroundSize, NativeWrap, BackgroundPosition, Mode, Not NoSound)
    End Sub

    '======================================================================================================================================================================
    Public Structure ColorRGBA
        Private _r As Integer
        Private _g As Integer
        Private _b As Integer
        Private _a As Integer
        Public Shared ReadOnly Property Zero As ColorRGBA = New ColorRGBA(0, 0, 0, 0)

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
        Public Sub New(R As Byte, G As Byte, B As Byte, A As Byte)
            _r = R
            _g = G
            _b = B
            _a = A
        End Sub
        Public Function Invert() As ColorRGBA
            Return New ColorRGBA(255 - _r, 255 - _g, 255 - _b, _a)
        End Function
        Public Function Brighten(Optional Factor As Single = 1.5) As ColorRGBA
            If Factor <= 0.0F OrElse Single.IsNaN(Factor) OrElse Single.IsInfinity(Factor) Then
                Throw New ArgumentOutOfRangeException(NameOf(Factor), "Factor must be a finite value greater than zero.")
            End If
            Return New ColorRGBA(
                ClampInteger(CInt(_r * Factor), 0, 255),
                ClampInteger(CInt(_g * Factor), 0, 255),
                ClampInteger(CInt(_b * Factor), 0, 255),
                _a)
        End Function
        Public Function Darken(Optional Factor As Single = 1.5) As ColorRGBA
            If Factor <= 0.0F OrElse Single.IsNaN(Factor) OrElse Single.IsInfinity(Factor) Then
                Throw New ArgumentOutOfRangeException(NameOf(Factor), "Factor must be a finite value greater than zero.")
            End If
            Return New ColorRGBA(
                ClampInteger(CInt(_r / Factor), 0, 255),
                ClampInteger(CInt(_g / Factor), 0, 255),
                ClampInteger(CInt(_b / Factor), 0, 255),
                _a)
        End Function
        ''' <summary>
        ''' Returns if colors are the same or not.
        ''' </summary>
        Public Overrides Function Equals(ColorRGBA As Object) As Boolean
            If Not TypeOf ColorRGBA Is ColorRGBA Then Return False
            Dim c As ColorRGBA = CType(ColorRGBA, ColorRGBA)
            Return Red = c.Red AndAlso Green = c.Green AndAlso Blue = c.Blue AndAlso Alpha = c.Alpha
        End Function
        Public Function ToARGB() As System.Drawing.Color
            Return Drawing.Color.FromArgb(_a, _r, _g, _b)
        End Function
        Public Function ToHex() As Integer
            Return (_a << 24) Or (_r << 16) Or (_g << 8) Or _b
        End Function
        Public Overrides Function ToString() As String
            Return $"Color: R={Red}, G={Green}, B={Blue}, A={Alpha}"
        End Function
        '===========================================================================================
        'Shared
        '---------
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
        Public Shared Function Invert(Color As ColorRGBA) As ColorRGBA
            Return New ColorRGBA(255 - Color.Red, 255 - Color.Green, 255 - Color.Blue, Color.Alpha)
        End Function
        Public Shared Function Brighten(Color As ColorRGBA, Optional Factor As Single = 1.5) As ColorRGBA
            Return New ColorRGBA(
                ClampInteger(CInt(Color.Red * Factor), 0, 255),
                ClampInteger(CInt(Color.Green * Factor), 0, 255),
                ClampInteger(CInt(Color.Blue * Factor), 0, 255),
                Color.Alpha)
        End Function
        Public Shared Function Darken(Color As ColorRGBA, Optional Factor As Single = 1.5) As ColorRGBA
            Return New ColorRGBA(
                ClampInteger(CInt(Color.Red / Factor), 0, 255),
                ClampInteger(CInt(Color.Green / Factor), 0, 255),
                ClampInteger(CInt(Color.Blue / Factor), 0, 255),
                Color.Alpha)
        End Function
    End Structure
    Public Structure ColorRGB
        Private _r As Integer
        Private _g As Integer
        Private _b As Integer
        Private _a As Integer
        Public Sub New(R As Byte, G As Byte, B As Byte)
            _r = R
            _g = G
            _b = B
            _a = 255
        End Sub
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
        Public Function Invert() As ColorRGB
            Return New ColorRGB(255 - _r, 255 - _g, 255 - _b)
        End Function
        Public Function Brighten(Optional Factor As Single = 1.5) As ColorRGB
            If Factor <= 0.0F OrElse Single.IsNaN(Factor) OrElse Single.IsInfinity(Factor) Then
                Throw New ArgumentOutOfRangeException(NameOf(Factor), "Factor must be a finite value greater than zero.")
            End If
            Return New ColorRGB(
                ClampInteger(CInt(_r * Factor), 0, 255),
                ClampInteger(CInt(_g * Factor), 0, 255),
                ClampInteger(CInt(_b * Factor), 0, 255))
        End Function
        Public Function Darken(Optional Factor As Single = 1.5) As ColorRGB
            If Factor <= 0.0F OrElse Single.IsNaN(Factor) OrElse Single.IsInfinity(Factor) Then
                Throw New ArgumentOutOfRangeException(NameOf(Factor), "Factor must be a finite value greater than zero.")
            End If
            Return New ColorRGB(
                ClampInteger(CInt(_r / Factor), 0, 255),
                ClampInteger(CInt(_g / Factor), 0, 255),
                ClampInteger(CInt(_b / Factor), 0, 255))
        End Function
        ''' <summary>
        ''' Returns if colors are the same or not.
        ''' </summary>
        Public Overrides Function Equals(ColorRGB As Object) As Boolean
            If Not TypeOf ColorRGB Is ColorRGB Then Return False
            Dim c As ColorRGB = CType(ColorRGB, ColorRGB)
            Return Red = c.Red AndAlso Green = c.Green AndAlso Blue = c.Blue
        End Function
        Public Function ToARGB() As System.Drawing.Color
            Return Drawing.Color.FromArgb(_a, _r, _g, _b)
        End Function
        Public Function ToHex() As Integer
            Return (_a << 24) Or (_r << 16) Or (_g << 8) Or _b
        End Function
        Public Overrides Function ToString() As String
            Return $"Color: R={Red}, G={Green}, B={Blue}"
        End Function
        '===========================================================================================
        'Shared
        '---------
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
        Public Shared Function FromHex(ARGB As Integer) As ColorRGB
            Dim a As Byte = CByte((ARGB >> 24) And &HFF)
            Dim r As Byte = CByte((ARGB >> 16) And &HFF)
            Dim g As Byte = CByte((ARGB >> 8) And &HFF)
            Dim b As Byte = CByte(ARGB And &HFF)
            Return New ColorRGB(r, g, b)
        End Function
        Public Shared Operator =(Color1 As ColorRGB, Color2 As ColorRGB) As Boolean
            Return Color1.Red = Color2.Red AndAlso Color1.Green = Color2.Green AndAlso Color1.Blue = Color2.Blue
        End Operator
        Public Shared Operator <>(Color1 As ColorRGB, Color2 As ColorRGB) As Boolean
            Return Not (Color1 = Color2)
        End Operator
        Public Shared Function Invert(Color As ColorRGB) As ColorRGB
            Return New ColorRGB(255 - Color.Red, 255 - Color.Green, 255 - Color.Blue)
        End Function
        Public Shared Function Brighten(Color As ColorRGB, Optional Factor As Single = 1.5) As ColorRGB
            Return New ColorRGB(
                ClampInteger(CInt(Color.Red * Factor), 0, 255),
                ClampInteger(CInt(Color.Green * Factor), 0, 255),
                ClampInteger(CInt(Color.Blue * Factor), 0, 255))
        End Function
        Public Shared Function Darken(Color As ColorRGB, Optional Factor As Single = 1.5) As ColorRGB
            Return New ColorRGB(
                ClampInteger(CInt(Color.Red / Factor), 0, 255),
                ClampInteger(CInt(Color.Green / Factor), 0, 255),
                ClampInteger(CInt(Color.Blue / Factor), 0, 255))
        End Function
    End Structure
    '======================================================================================================================================================================
    ''' <summary>
    ''' Provides manual fade-in/out control without native functions.
    ''' Automatically hooks into TickHelper and supports color and minimum/maximum alpha.
    ''' </summary>
    Public NotInheritable Class CustomFade
        Private Sub New() : End Sub
        ''' <summary>
        ''' Cancels any fade and clears the overlay immediately.
        ''' </summary>
        Public Shared Sub CancelAll()
            fadeState = FadeDirection.None
            currentAlpha = 0
            isHolding = False
            ' If isPauseMenuDisabled Then
            'NativeGeneric.PauseMenu.Disable = False
            ' isPauseMenuDisabled = False
            ' End If
            shouldReenablePauseMenu = False
            UnregisterFromTick()
        End Sub
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
        Public Shared Sub FadeIn(Optional Duration As Integer = 2000, Optional ByVal Color As ColorRGB = Nothing, Optional MinAlpha As Byte = 0, Optional MaxAlpha As Byte = 255)

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


            If fadeDuration <= 0 AndAlso holdDuration <= 0 Then
                currentAlpha = endAlpha
                fadeState = FadeDirection.None

                If currentAlpha > 0 Then
                    RegisterToTick()
                Else
                    UnregisterFromTick()
                    '    If isPauseMenuDisabled Then
                    'NativeGeneric.PauseMenu.Disable = False
                    '    isPauseMenuDisabled = False
                    '   shouldReenablePauseMenu = False
                    'E'nd If
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
        Public Shared Sub FadeOut(Optional Duration As Integer = 2000, Optional ByVal Color As ColorRGB = Nothing, Optional MinAlpha As Byte = 0, Optional MaxAlpha As Byte = 255, Optional DisablePauseMenu As Boolean = True)

            If Color = Nothing Then
                fadeColor = New ColorRGB(0, 0, 0)
            Else
                fadeColor = Color
            End If

            fadeState = FadeDirection.Outward
            fadeStart = DateTime.Now
            fadeDuration = Math.Max(0, Duration)
            startAlpha = MinAlpha
            endAlpha = MaxAlpha
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
                        '  NativeGeneric.PauseMenu.Disable = False
                        isPauseMenuDisabled = False
                    End If
                    UnregisterFromTick()
                End If
                Return
            End If

            RegisterToTick()

            'If DisablePauseMenu AndAlso Not isPauseMenuDisabled Then
            ' NativeGeneric.PauseMenu.Disable = True
            '  isPauseMenuDisabled = True
            ' End If
        End Sub

        '=========================================================================================================================================================================
        ' Internals
        '-------------
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

        Private Shared lastMinAlpha As Integer = 0
        Private Shared lastMaxAlpha As Integer = 255
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

                'If shouldReenablePauseMenu AndAlso finalAlpha <= lastMinAlpha Then
                'NativeGeneric.PauseMenu.Disable = False
                ' isPauseMenuDisabled = False
                ' shouldReenablePauseMenu = False
                ' End If


                If finalAlpha <= 0 Then
                    currentAlpha = 0
                    UnregisterFromTick()
                Else

                End If
            End If
        End Sub
        Private Shared Sub draw()
            If currentAlpha <= 0 Then Return

            Dim c As ColorRGB = fadeColor
            Dim alphaSingle As Single = Tools.ClampInteger(currentAlpha, 0, 255) / 255.0F

            NativeDraws.DrawRectangle(New Vector2(0.5F, 0.5F), New Vector2(1.0F, 1.0F), New ColorRGBA(c.Red, c.Green, c.Blue, CByte(Tools.ClampInteger(currentAlpha, 0, 255))), Duration:=1)
        End Sub
        Private Shared Sub internalTick()
            update()
            draw()
        End Sub
        Private Shared Sub RegisterToTick()
            If registeredInTick Then Return
            TickHelper.Add_Internal(AddressOf internalTick)
            registeredInTick = True
        End Sub
        Private Shared Sub UnregisterFromTick()
            If Not registeredInTick Then Return
            TickHelper.Remove_Internal(AddressOf internalTick)
            registeredInTick = False
        End Sub

        '=========================================================================================================================================================================
        '===============================================================================================================================================================================================================
    End Class

    Public NotInheritable Class GameTexture
        Private _txdHandle As Integer
        Private _textureHandle As Integer
        Private ReadOnly _txd As String
        Private ReadOnly _texture As String
        Private _streamed As Boolean
        Private _isLoaded As Boolean
        Private _streamRequestIssued As Boolean = False
        Private _res As (X As Integer, Y As Integer) = (X:=0, Y:=0)
        Private _asp As Single = 0.0F
        Public Sub New(TXD As String, Texture As String, Optional StreamedTXD As Boolean = False)
            If String.IsNullOrWhiteSpace(TXD) Then
                Throw New ArgumentException("TXD cannot be null or empty.", NameOf(TXD))
            End If
            If String.IsNullOrWhiteSpace(Texture) Then
                Throw New ArgumentException("Texture cannot be null or empty.", NameOf(Texture))
            End If
            _txd = TXD
            _texture = Texture
            _streamed = StreamedTXD
        End Sub

        Public ReadOnly Property Aspect As Single
            Get
                If _res.X <= 0 OrElse _res.Y <= 0 Then
                    Tools.WriteLog("Error: GameTexture ' " & _texture & " ' never loaded before. Cannot return Aspect.")
                    Return Nothing
                    Exit Property
                End If
                Return _asp
            End Get
        End Property
        ''' <summary>
        ''' Userful only for raw native coding.
        ''' </summary>
        ''' <returns>Handle got from GET_TEXTURE or GET_TEXTURE_FROM_STREAMED_TXD</returns>
        Public ReadOnly Property Handle As Integer
            Get
                Return _textureHandle
            End Get
        End Property
        Public ReadOnly Property IsLoaded As Boolean
            Get
                Return _isLoaded
            End Get
        End Property
        Public ReadOnly Property Resolution As (X As Integer, Y As Integer)
            Get
                If _res.X <= 0 OrElse _res.Y <= 0 Then
                    Tools.WriteLog("Error: GameTexture ' " & _texture & " ' never loaded before. Cannot return Resolution.")
                    Return (X:=0, Y:=0)
                    Exit Property
                End If
                Return _res
            End Get
        End Property
        Public ReadOnly Property TXD As String
            Get
                Return _txd
            End Get
        End Property
        Public Function Load() As Boolean
            If _isLoaded Then Return True

            If _streamed Then
                If Not _streamRequestIssued Then
                    [Call]("REQUEST_STREAMED_TXD", _txd, True)
                    _streamRequestIssued = True
                End If
                If Not [Call](Of Boolean)("HAS_STREAMED_TXD_LOADED", _txd) Then Return False
                _textureHandle = [Call](Of Integer)("GET_TEXTURE_FROM_STREAMED_TXD", _txd, _texture)
                If _textureHandle = 0 Then Return False
            Else
                If _txdHandle = 0 Then
                    _txdHandle = [Call](Of Integer)("LOAD_TXD", _txd)
                End If

                If _txdHandle = 0 Then Return False

                _textureHandle = [Call](Of Integer)("GET_TEXTURE", _txdHandle, _texture)
                If _textureHandle = 0 Then Return False
            End If

            If _res.X = 0 AndAlso _res.Y = 0 Then
                Dim p1, p2 As New Native.Pointer(GetType(Integer))
                [Call]("GET_TEXTURE_RESOLUTION", _textureHandle, p1, p2)
                _res.X = p1.Value
                _res.Y = p2.Value
                _asp = _res.X / _res.Y
            End If

            _isLoaded = True
            Return True
        End Function
        ''' <summary>
        ''' Automatically loads TXD and texture if required.
        ''' </summary>
        ''' <param name="Position"></param>
        ''' <param name="Size"></param>
        ''' <param name="Rotation"></param>
        ''' <param name="Color"></param>
        Public Sub Draw(Position As Vector2, Size As Vector2, Rotation As Single, Color As ColorRGBA)
            If Not _isLoaded AndAlso Not Load() Then
                Return
            End If
            [Call]("DRAW_SPRITE", _textureHandle, Position.X, Position.Y, Size.X, Size.Y, Rotation, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        End Sub
        Public Sub Release(Optional ReleaseTXD As Boolean = False)
            If _textureHandle <> 0 Then
                [Call]("RELEASE_TEXTURE", _textureHandle)
                _textureHandle = 0
            End If

            If ReleaseTXD Then
                If _streamed Then
                    [Call]("MARK_STREAMED_TXD_AS_NO_LONGER_NEEDED", _txd)
                    _streamRequestIssued = False
                ElseIf _txdHandle <> 0 Then
                    [Call]("REMOVE_TXD", _txdHandle)
                End If
                _txdHandle = 0
            End If

            _isLoaded = False
        End Sub
        'Public ReadOnly Property Texture As String
        '    Get
        '        Return _Texture
        '    End Get
        'End Property
        'Public ReadOnly Property TXDHandle As Integer
        '    Get
        '        Return _txdHandle
        '    End Get
        'End Property
        'Public ReadOnly Property StreamedTXD As Boolean
        '    Get
        '        Return _streamed
        '    End Get
        'End Property
        Public Overrides Function ToString() As String
            Return _texture
        End Function
    End Class

    Public NotInheritable Class GXT
        '(DOES_TEXT_LABEL_EXIST( sParam1 )
        'GET_LENGTH_OF_STRING_WITH_THIS_TEXT_LABEL( sParam1 )
        'IS_STRING_NULL( sParam1 )
        ''' <summary>
        ''' Clears all big print (printing blank string for all types).
        ''' </summary>
        Public Shared Sub ClearAllBigPrints()
            Dim i As Integer
            For i = 1 To 7
                [Call]("PRINT_BIG", "STRING", 1, i)
            Next
        End Sub
        ''' <summary>
        ''' Clears a specified GXT big print.
        ''' </summary>
        Public Shared Sub ClearBigPrint(GxtEntry As String)
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.ClearThisBigPrint: " & GxtEntry & " is not valid.")
                Return
            End If
            [Call]("CLEAR_THIS_BIG_PRINT", GxtEntry)
        End Sub
        ''' <summary>
        ''' Clears a specified big print type (printing blank string).
        ''' </summary>
        ''' <param name="Type">A native value from BigPrintType enum.</param>
        Public Shared Sub ClearBigPrintType(Type As BigPrintType)
            [Call]("PRINT_BIG", "STRING", 1, Type)
        End Sub
        Public Shared Sub ClearHelp()
            [Call]("CLEAR_HELP")
        End Sub

        Public Shared Sub EnableNonMinigameHelpMessages(Optional Enable As Boolean = True)
            [Call]("DISPLAY_NON_MINIGAME_HELP_MESSAGES", Enable)
        End Sub
        ''' <summary>
        ''' Displays a native GXT text at the specified screen position.
        ''' If Duration is at least 5 milliseconds, the text is managed by
        ''' DrawManager and remains visible for the requested duration.
        ''' Shorter durations draw the text immediately.
        ''' Returns the native text width, single-line height and line count.
        ''' </summary>
        ''' <param name="GxtEntry">The GXT label to display.</param>
        ''' <param name="PercentagePos">The normalized screen position.</param>
        ''' <param name="Style">Optional native text style.</param>
        ''' <param name="Duration">Display duration in milliseconds.</param>
        Public Shared Function Draw(GxtEntry As String, PercentagePos As Vector2, Optional Style As TextStyle = Nothing, Optional Duration As Integer = 1, Optional LineHeightMultiplier As Single = 1.0F) As (Width As Single, Height As Single, LineCount As Integer)
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.Draw: " & GxtEntry & " is not valid.")
                Return (0.0F, 0.0F, 0)
            End If
            Dim FinalStyle As TextStyle = If(Style, TextStyle.Default)

            Dim FinalMultiplier As Single =
    ClampFloat(LineHeightMultiplier, 0.7F, 10.0F)

            Dim NativeWrap As (
    StartPos As Single?,
    EndPos As Single?,
    LineHeightMultiplier As Single?
) = (
    StartPos:=0.0F,
    EndPos:=1.0F,
    LineHeightMultiplier:=FinalMultiplier)

            Dim SingleLineHeight As Single =
    TextStyle.SetTextStyle(FinalStyle, NativeWrap)

            Dim StringWidth As Single =
    [Call](Of Single)("GET_STRING_WIDTH", GxtEntry)

            Dim NumberOfLines As Integer = 0

            If Not String.IsNullOrEmpty(GxtEntry) Then

                NumberOfLines = [Call](Of Integer)(
        "GET_NUMBER_LINES",
        0.0F,
        1.0F,
        GxtEntry)

                NumberOfLines = Math.Max(1, NumberOfLines)

            End If

            Dim TotalHeight As Single =
    CSng(NumberOfLines) *
    SingleLineHeight *
    FinalMultiplier
            Dim BackgroundSize As New Vector2(0.0F, 0.0F)
            Dim BackgroundPosition As Vector2? = Nothing

            If FinalStyle.Background.Alpha > 0 AndAlso NumberOfLines > 0 Then

                Dim BackgroundWidth As Single

                If FinalStyle.Justification = NativeDraws.Justification.Justify Then

                    BackgroundWidth = 1.0F

                Else

                    BackgroundWidth = Math.Min(
            Math.Max(0.0F, StringWidth),
            1.0F)

                End If

                BackgroundSize = New Vector2(
        BackgroundWidth + (BackgroundHorizontalPadding * 2.0F),
        TotalHeight + (BackgroundVerticalPadding * 2.0F))

                If BackgroundSize.X > 0.0F Then

                    BackgroundPosition = CalculateTextBackgroundPosition(
            PercentagePos,
            BackgroundSize,
            FinalStyle.Justification,
            0.0F,
            1.0F,
            HorizontalPadding:=BackgroundHorizontalPadding)

                End If

            End If
            If Duration < 5 Then
                If BackgroundPosition.HasValue Then
                    DrawRectangle(
        BackgroundPosition.Value,
        BackgroundSize,
        FinalStyle.Background,
        Duration:=1)
                End If

                TextStyle.SetTextStyle(FinalStyle, NativeWrap)
                [Call]("DISPLAY_TEXT", PercentagePos.X, PercentagePos.Y, GxtEntry)
            Else

                DrawManager.DrawGxtText(
        GxtEntry,
        Duration,
        PercentagePos,
        FinalStyle,
        BackgroundSize,
        BackgroundPosition,
        FinalMultiplier)

            End If

            Return (Width:=StringWidth, Height:=TotalHeight, LineCount:=NumberOfLines)
        End Function
        ''' <summary>
        ''' Displays a GXT text (subtitle).
        ''' </summary>
        Public Shared Sub Display(GxtEntry As String, Optional Duration As Integer = 3000, Optional BriefDisplay As Boolean = False, Optional DisplayNow As Boolean = True)
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.Display: " & GxtEntry & " is not valid.")
                Return
            End If
            [Call]("ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS", BriefDisplay)
            If Duration < 5 Then Duration = 5
            If DisplayNow Then
                [Call]("PRINT_NOW", GxtEntry, Duration)
            Else
                [Call]("PRINT", GxtEntry, Duration)
            End If
        End Sub
        ''' <summary>
        ''' Displays a GXT text with Input Text (bottom-left corner). Must be used in a tick (cycle).
        ''' You can display two or more lines contemporarily.
        ''' </summary>
        ''' <param name="TextKey">A string from TextKeys structure.</param>
        Public Shared Sub DisplayInputHelper(GxtEntry As String, TextKey As String, OnTheSameLine As Boolean)
            'aggiungere metodo che rimuove "~" da TextKey
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.DisplayInputHelper: " & GxtEntry & " is not valid.")
                Return
            End If
            Dim FinalTextKey As String = If(TextKey, String.Empty).Replace("~", String.Empty)
            [Call]("DRAW_FRONTEND_HELPER_TEXT", GxtEntry, FinalTextKey, Not OnTheSameLine)
        End Sub
        ''' <summary>
        ''' Returns true if GxtEntry exists in any GXT table. Yet added inside every GXT and HelpMessage function.
        ''' </summary>
        Public Shared Function Exists(GxtEntry As String) As Boolean
            Return [Call](Of Boolean)("DOES_TEXT_LABEL_EXIST", GxtEntry)
        End Function
        Public Shared Function GetHelpMessageBoxSize() As GTA.Vector2
            Dim pointer1, pointer2 As Native.Pointer
            pointer1 = New Native.Pointer(GetType(Single))
            pointer2 = New Native.Pointer(GetType(Single))
            [Call]("GET_HELP_MESSAGE_BOX_SIZE", pointer1, pointer2)
            Return New GTA.Vector2(pointer1.Value, pointer2.Value)
        End Function
        Public Shared Function GetString(GxtEntry As String) As String
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.GetString: " & GxtEntry & " is not valid.")
                Return "GxtEntry is not valid!"
            End If
            Return [Call](Of String)("GET_STRING_FROM_TEXT_FILE", GxtEntry)
        End Function
        Public Shared Function GetStringLength(GxtEntry As String) As Integer
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.GetStringLength: " & GxtEntry & " is not valid.")
                Return 0
            End If
            Return [Call](Of Integer)("GET_LENGTH_OF_STRING_WITH_THIS_TEXT_LABEL", GxtEntry)
        End Function
        Public Shared Function GetFirstCharacters(GxtEntry As String, NumberOfCharacters As Integer) As String
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.GetFirstCharacters: " & GxtEntry & " is not valid.")
                Return "GxtEntry is not valid!"
            End If
            Return [Call](Of String)("GET_FIRST_N_CHARACTERS_OF_STRING", GxtEntry, NumberOfCharacters)
        End Function
        Public Shared Sub HideHelpThisFrame()
            [Call]("HIDE_HELP_TEXT_THIS_FRAME")
        End Sub
        ''' <summary>
        ''' Untested. Maybe linked to DisplayInputHelper.w
        ''' </summary>
        Public Shared Sub InitFrontendHelperText()
            [Call]("INIT_FRONTEND_HELPER_TEXT")
        End Sub
        Public Shared Function IsPrintingAnyHelp() As Boolean
            Return [Call](Of Boolean)("IS_HELP_MESSAGE_BEING_DISPLAYED")
        End Function
        'TESTARE AdditionalGxtEntry
        ''' <summary>
        ''' Returns true if GxtEntry (with optional AdditionalGxtEntry) is being printed.
        ''' If AdditionalGxtEntry = nothing, will be checked also help message with "STRING" additional string
        ''' (needed for printing help without sound).
        ''' </summary>
        Public Shared Function IsPrintingHelp(GxtEntry As String, Optional AdditionalGxtEntry As String = Nothing) As Boolean
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.HelpMessage.IsPrintingHelp: " & GxtEntry & " is not valid.")
                Return False
            End If
            'TESTARE
            If AdditionalGxtEntry <> Nothing Then
                If Not Exists(AdditionalGxtEntry) Then
                    Tools.WriteLog("Error in NativeDraws.GXT.HelpMessage.IsPrintingHelp: " & AdditionalGxtEntry & " is not valid.")
                    Return False
                End If
                Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_WITH_STRING_BEING_DISPLAYED", GxtEntry, AdditionalGxtEntry)
            End If
            Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_BEING_DISPLAYED", GxtEntry) OrElse
                       [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_WITH_STRING_BEING_DISPLAYED", GxtEntry, "STRING") 'check for printing with no sound and without additional string
        End Function
        'TESTARE
        Public Shared Function IsPrintingHelpWithNumber(GxtEntry As String, Number As Integer) As Boolean
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.HelpMessage.IsPrintingHelpWithNumber: " & GxtEntry & " is not valid.")
                Return False
            End If
            Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_WITH_NUMBER_BEING_DISPLAYED", GxtEntry, Number)
        End Function
        ''' <summary>
        ''' Displays a native GXT message at a fixed PrintBig position.
        ''' By default, cancels any active or queued custom PrintBigText
        ''' messages of the same type before displaying the native message.
        ''' </summary>
        ''' <param name="GxtEntry">The native GXT label.</param>
        ''' <param name="Duration">Native display duration in milliseconds.</param>
        ''' <param name="Type">The native PrintBig type.</param>
        ''' <param name="DisplayNow">If True, removes custom PrintBigText messages of the same type before displaying the native message.
        ''' </param>
        Public Shared Sub PrintBig(GxtEntry As String, Optional Duration As Integer = 5000, Optional Type As BigPrintType = BigPrintType.Center_Standard, Optional DisplayNow As Boolean = True)
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.PrintBig: " & GxtEntry & " is not valid.")
                Return
            End If
            If DisplayNow Then
                DrawManager.ClearBigPrintText(Type)
            End If
            [Call]("PRINT_BIG", GxtEntry, Duration, Type)
        End Sub
        ''' <summary>
        ''' Displays a number using a native PrintBig position.
        ''' By default, cancels active or queued custom PrintBigText
        ''' messages of the same type before displaying the native message.
        ''' </summary>
        ''' <param name="Number">The number to display.</param>
        ''' <param name="Duration">Native display duration in milliseconds.</param>
        ''' <param name="Type">The native PrintBig type.</param>
        ''' <param name="DisplayNow">If True, removes custom PrintBigText messages of the same type before displaying the native message.
        ''' </param>
        Public Shared Sub PrintBigNumber(Number As Integer, Optional Duration As Integer = 5000, Optional Type As BigPrintType = BigPrintType.Center_Standard, Optional DisplayNow As Boolean = True)
            If DisplayNow Then
                DrawManager.ClearBigPrintText(Type)
            End If
            [Call]("PRINT_WITH_NUMBER_BIG", "NUMBER", Number, Duration, Type)
        End Sub
        ''' <summary>
        ''' Print GXT help message with optional flags and additional string.
        ''' </summary>
        ''' <param name="GxtEntry"></param>
        ''' <param name="PrintForever"></param>
        ''' <param name="NoSound">If true it will be used "STRING" as additional string due to missing native. 
        ''' It will be checked in IsPrintingHelpe so it won't cause any issue.</param>
        ''' <param name="AdditionalGxtEntry">Note: not every GxtEntry support additional string (it will be displayed only the main one).</param>
        Public Shared Sub PrintHelp(GxtEntry As String, Optional PrintForever As Boolean = False, Optional NoSound As Boolean = False, Optional BriefDisplay As Boolean = True, Optional AdditionalGxtEntry As String = Nothing)
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.HelpMessage.Print: " & GxtEntry & " is not valid.")
                Return
            End If
            If AdditionalGxtEntry <> Nothing Then
                If AdditionalGxtEntry <> "STRING" AndAlso Not Exists(AdditionalGxtEntry) Then
                    Tools.WriteLog("Error in NativeDraws.GXT.HelpMessage.Print: " & AdditionalGxtEntry & " is not valid.")
                    Return
                End If
            Else
                If AdditionalGxtEntry = Nothing Then AdditionalGxtEntry = "STRING"
            End If

            [Call]("ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS", BriefDisplay)
            If Not PrintForever Then
                If Not NoSound Then
                    If AdditionalGxtEntry = "STRING" Then
                        [Call]("PRINT_HELP", GxtEntry)
                    Else
                        [Call]("PRINT_HELP_WITH_STRING", GxtEntry, AdditionalGxtEntry)
                    End If
                Else
                    [Call]("PRINT_HELP_WITH_STRING_NO_SOUND", GxtEntry, AdditionalGxtEntry)
                End If
            Else
                If Not NoSound Then
                    If AdditionalGxtEntry = "STRING" Then
                        [Call]("PRINT_HELP_FOREVER", GxtEntry)
                    Else
                        [Call]("PRINT_HELP_FOREVER_WITH_STRING", GxtEntry, AdditionalGxtEntry)
                    End If
                Else
                    [Call]("PRINT_HELP_FOREVER_WITH_STRING_NO_SOUND", GxtEntry, AdditionalGxtEntry)
                End If
            End If
        End Sub
        ''' <param name="GxtEntry">Use "NUMBER" to display only the number.</param>
        ''' <param name="Number">Note: not every GxtEntry support additional number (it won't be displayed anything).</param>
        Public Shared Sub PrintHelpWithNumber(GxtEntry As String, Number As Integer, Optional PrintForever As Boolean = False, Optional BriefDisplay As Boolean = True)
            If Not Exists(GxtEntry) Then
                Tools.WriteLog("Error in NativeDraws.GXT.HelpMessage.PrintWithNumber: " & GxtEntry & " is not valid.")
                Return
            End If

            [Call]("ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS", BriefDisplay)
            If Not PrintForever Then
                [Call]("PRINT_HELP_WITH_NUMBER", GxtEntry, Number)
            Else
                [Call]("PRINT_HELP_FOREVER_WITH_NUMBER", GxtEntry, Number)
            End If
        End Sub
        'Public Shared Sub SetMessageBoxSize(Size As Single)
        '    [Call]("SET_HELP_MESSAGE_BOX_SIZE", value unknown type)
        'End Sub
        ''' <summary>
        ''' Loads a GXT table. Not required for many GXT entries, like "GO" and "NET_LOADING".
        ''' </summary>
        ''' <param name="Unknown">SCO scripts use 0, 3 and 6.</param>
        Public Shared Sub Load(GxtTable As String, Optional Unknown As Integer = 1)
            [Call]("LOAD_ADDITIONAL_TEXT", GxtTable, Unknown)
        End Sub
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
    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public NotInheritable Class Movie
        Public Structure MoviesList
            Public Shared Weazel As String = "" '"weazel""CNT""static""burgershot_nosound""static_nosound" 
        End Structure
        Private Sub New()
            End Sub
            ' Private Shared _currMovie As String
            Public Shared Sub Draw(PercentagePos As Vector2, PercentageSize As Vector2, Rotation As Single, Color As ColorRGBA)
                [Call]("DRAW_MOVIE", PercentagePos.X, PercentagePos.Y, PercentageSize.X, PercentageSize.Y, Rotation, Color.Red, Color.Green, Color.Blue, Color.Alpha)
            End Sub
            Public Shared Sub Play()
                [Call]("PLAY_MOVIE")
            End Sub
            Public Shared Sub Release()
                [Call]("RELEASE_MOVIE")
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
            Public Shared Sub [Stop](Optional Release As Boolean = False)
                [Call]("STOP_MOVIE")
                If Release Then [Call]("Release_MOVIE")
            End Sub
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
        Public Const FX_LineBreak As String = "~n~"
        Public Const FX_StopColorFxHere As String = "~s~"
        Public Const Fx_Highlighted As String = "~h~"
        Public Const Fx_String As String = "~a~"
        Public Const Fx_Unknown3 As String = "~1~"
        Public Const Fx_Subtitle As String = "~z~"
        ''' <summary>Used after FX_Subtitle</summary>
        Public Const FX_Unknown1 As String = "~x~"
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

    Public NotInheritable Class TextStyle
        Private _Col As ColorRGBA
        Private _justify As Justification
        Private _font As Fonts
        Private _proport As Boolean
        Private _nativeScale As Vector2
        Private _sizeValue As Integer = 12
        Private _widthScale As Single = 1.0F
        Private _Background As ColorRGBA

        Private Shared ReadOnly _FontBaseXCoef As New System.Collections.Generic.Dictionary(Of Fonts, Single) From {{Fonts.Standard, 0.56F},
                                                                                                                    {Fonts.Helvetica, 0.56F},
                                                                                                                    {Fonts.StandardUppercase_GtaNumbers, 0.56F},
                                                                                                                    {Fonts.Dotted, 0.585F},
                                                                                                                    {Fonts.Dotted_HandwriteNumbers, 0.585F},
                                                                                                                    {Fonts.Narrow, 0.65F},
                                                                                                                    {Fonts.HelveticaBold, 0.53F}}
        '--------------------
        Private _ShadowEnabled As Boolean
        Private _Underscores As Boolean
        Private _BorderCol As ColorRGBA
        Private Const NativeLineHeightPerScaleY As Single = 0.066F
        Public Shared Property [Default] As New TextStyle(ColorRGBA.White, Size:=12)
        Public Enum Fonts
            'None = -1
            Standard = 0
            ''' <summary>
            ''' Only uppercase
            ''' </summary>
            Dotted = 1
            StandardUppercase_GtaNumbers = 4
            ''' <summary>
            ''' Only uppercase
            ''' </summary>
            Dotted_HandwriteNumbers = 5
            ''' <summary>
            ''' Only uppercase
            ''' </summary>
            Narrow = 6
            ''' <summary>This font will automatically call LOAD_TEXT_FONT</summary>
            HelveticaBold = 7
            ''' <summary>
            ''' Only uppercase
            ''' </summary>
            Helvetica = 8
        End Enum
        ''' <summary>
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
        Public Property Justification As Justification
            Get
                Return _justify
            End Get
            Set(value As Justification)
                _justify = value
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
        Public Property Shadow As Boolean
            Get
                Return _ShadowEnabled
            End Get
            Set(value As Boolean)
                _ShadowEnabled = value
            End Set
        End Property
        Private Property Proportional As Boolean
            Get
                Return _proport
            End Get
            Set(value As Boolean)
                _proport = value
            End Set
        End Property
        Public ReadOnly Property Scale As Vector2
            Get
                Return _nativeScale
            End Get
        End Property
        Public Property Size As Integer
            Get
                Return _sizeValue
            End Get
            Set(value As Integer)
                If value < 1 Then value = 1
                _sizeValue = value
                UpdateNativeScale()
            End Set
        End Property
        Public Property WidthScale As Single
            Get
                Return _widthScale
            End Get
            Set(value As Single)
                If value < 0.0F Then value = 0.0F
                _widthScale = value
                UpdateNativeScale()
            End Set
        End Property
        Public Property Font As Fonts
            Get
                Return _font
            End Get
            Set(value As Fonts)
                _font = value
                UpdateNativeScale()
            End Set
        End Property
        Public Property Background As ColorRGBA
            Get
                Return _Background
            End Get
            Set(value As ColorRGBA)
                _Background = value
            End Set
        End Property
        Public Property ShowUnderscores As Boolean
            Get
                Return _Underscores
            End Get
            Set(value As Boolean)
                _Underscores = value
            End Set
        End Property
        Private Sub New() : End Sub
        ''' <param name="Justification">You need to set proper wrap length to use justify or right justification.</param>
        Public Sub New(Color As ColorRGBA, Optional Size As Integer = 12, Optional Justification As Justification = Justification.Left, Optional BorderColor As ColorRGBA = Nothing, Optional Font As Fonts = Fonts.Standard,
                       Optional Shadow As Boolean = False, Optional WidthScale As Single = 1.0F, Optional Background As ColorRGBA = Nothing, Optional Proportional As Boolean = False, Optional ShowUnderscores As Boolean = True)

            If Color = Nothing Then _Col = New ColorRGBA(255, 255, 255, 255) Else _Col = Color
            _font = Font
            _justify = Justification
            _BorderCol = BorderColor
            _proport = Proportional
            _ShadowEnabled = Shadow
            If Size < 1 Then Size = 1
            If WidthScale < 0.0F Then WidthScale = 0.0F
            _sizeValue = Size
            _widthScale = WidthScale
            _Background = Background
            _Underscores = ShowUnderscores
            UpdateNativeScale()
        End Sub
        ''' <summary>
        ''' Returns single line height (native).
        ''' </summary>
        ''' <returns></returns>
        Friend Shared Function SetTextStyle(Style As TextStyle, Optional Wrap As (StartPos As Single?, EndPos As Single?, LineHeightMultiplier As Single?) = Nothing, Optional ViewportID As Integer? = Nothing, Optional RenderID As Integer? = Nothing) As Single
            SyncLock _lockSettingStyle
                Dim FinalStart As Single = If(Wrap.StartPos.HasValue, Wrap.StartPos.Value, 0.0F)
                Dim FinalEnd As Single = If(Wrap.EndPos.HasValue, Wrap.EndPos.Value, 1.0F)
                Dim FinalLineHeight As Single = If(Wrap.LineHeightMultiplier.HasValue, ClampFloat(Wrap.LineHeightMultiplier.Value, 0.7F, 10.0F), 1.0F)
                '-----
                If ViewportID.HasValue Then [Call]("SET_TEXT_VIEWPORT_ID", ViewportID.Value)
                If RenderID.HasValue Then [Call]("SET_TEXT_RENDER_ID", RenderID.Value)
                '-----
                If Style.Font = Fonts.HelveticaBold AndAlso Not [Call](Of Boolean)("IS_FONT_LOADED", Style.Font) Then [Call]("LOAD_TEXT_FONT", Style.Font)
                [Call]("SET_TEXT_FONT", Style.Font)
                '-----
                [Call]("SET_TEXT_SCALE", Style._nativeScale.X, Style._nativeScale.Y)
                Dim SingleLineHeight As Single = Style._nativeScale.Y * NativeLineHeightPerScaleY
                '-----
                Dim RightJustify As Boolean
                Dim Center As Boolean
                Dim Justify As Boolean
                Select Case Style._justify
                    Case Justification.Left
                        RightJustify = False
                        Center = False
                        Justify = False
                    Case Justification.Center
                        RightJustify = False
                        Center = True
                        Justify = False
                    Case Justification.Right
                        RightJustify = True
                        Center = False
                        Justify = False
                    Case Justification.Justify
                        RightJustify = False
                        Center = False
                        Justify = True
                    Case Else
                        RightJustify = False
                        Center = False
                        Justify = False
                End Select
                [Call]("SET_TEXT_RIGHT_JUSTIFY", RightJustify)
                [Call]("SET_TEXT_CENTRE", Center)
                [Call]("SET_TEXT_JUSTIFY", Justify)
                '-----
                [Call]("SET_TEXT_COLOUR", Style._Col.Red, Style._Col.Green, Style._Col.Blue, Style._Col.Alpha)
                '-----
                [Call]("SET_TEXT_DROPSHADOW", Style._ShadowEnabled, 0, 0, 0, 255)
                '-----
                Dim BorderEnabled As Boolean = (Style._BorderCol <> Nothing)
                [Call]("SET_TEXT_EDGE", BorderEnabled, Style._BorderCol.Red, Style._BorderCol.Green, Style._BorderCol.Blue, Style._BorderCol.Alpha)
                '-----
                [Call]("SET_TEXT_PROPORTIONAL", Not Style._proport)
                '-----
                [Call]("SET_TEXT_USE_UNDERSCORE", Style._Underscores)
                '-----
                [Call]("SET_TEXT_WRAP", FinalStart, FinalEnd)
                [Call]("SET_TEXT_LINE_HEIGHT_MULT", FinalLineHeight) 'Rimuovere per facilitare calcolo altezza linee?
                Return SingleLineHeight
            End SyncLock
        End Function
        Friend Function WithColor(NewColor As ColorRGBA) As TextStyle
            If NewColor = Me.Color Then Return Me
            Return New TextStyle(Color:=NewColor, Size:=Me.Size, Justification:=Me.Justification, BorderColor:=Me.Borders, Font:=Me.Font, Shadow:=Me.Shadow, WidthScale:=Me.WidthScale, Background:=Me.Background, Proportional:=Me._proport, ShowUnderscores:=Me._Underscores)
        End Function
        Friend Function WithFont(NewFont As Fonts) As TextStyle

            If NewFont = Me.Font Then
                Return Me
            End If

            Return New TextStyle(
        Color:=Me.Color,
        Size:=Me.Size,
        Justification:=Me.Justification,
        BorderColor:=Me.Borders,
        Font:=NewFont,
        Shadow:=Me.Shadow,
        WidthScale:=Me.WidthScale,
        Background:=Me.Background,
        Proportional:=Me._proport,
        ShowUnderscores:=Me._Underscores)

        End Function
        Friend Function WithBackground(NewBackground As ColorRGBA) As TextStyle

            If NewBackground = Me.Background Then Return Me

            Return New TextStyle(
        Color:=Me.Color,
        Size:=Me.Size,
        Justification:=Me.Justification,
        BorderColor:=Me.Borders,
        Font:=Me.Font,
        Shadow:=Me.Shadow,
        WidthScale:=Me.WidthScale,
        Background:=NewBackground,
        Proportional:=Me._proport,
        ShowUnderscores:=Me._Underscores)

        End Function
        Friend Function Clone() As TextStyle

            Return New TextStyle(
        Color:=Me.Color,
        Size:=Me.Size,
        Justification:=Me.Justification,
        BorderColor:=Me.Borders,
        Font:=Me.Font,
        Shadow:=Me.Shadow,
        WidthScale:=Me.WidthScale,
        Background:=Me.Background,
        Proportional:=Me._proport,
        ShowUnderscores:=Me._Underscores)

        End Function
        Private Shared Function GetFontBaseXCoef(Font As Fonts) As Single
            Dim Coef As Single
            If _FontBaseXCoef.TryGetValue(Font, Coef) AndAlso Coef > 0.0F Then Return Coef
            Return 1.0F
        End Function
        Private Sub UpdateNativeScale()
            _nativeScale.Y = _sizeValue / 50.0F
            _nativeScale.X = _nativeScale.Y * GetFontBaseXCoef(_font) * _widthScale
        End Sub
        Public Shared Operator =(Style1 As TextStyle, Style2 As TextStyle) As Boolean
            If ReferenceEquals(Style1, Style2) Then Return True
            If Style1 Is Nothing OrElse Style2 Is Nothing Then Return False

            Return ((Style1._BorderCol = Nothing AndAlso Style2._BorderCol = Nothing) OrElse
                   (Style1._BorderCol <> Nothing AndAlso Style1._BorderCol.Equals(Style2._BorderCol))) AndAlso
                   Style1._justify = Style2._justify AndAlso Style1._Col.Equals(Style2._Col) AndAlso
                   Style1._proport = Style2._proport AndAlso Style1._nativeScale.Equals(Style2._nativeScale) AndAlso
                   Style1._font = Style2._font AndAlso Style1._ShadowEnabled = Style2._ShadowEnabled AndAlso
                   Style1._Background.Equals(Style2._Background) AndAlso Style1._sizeValue = Style2._sizeValue AndAlso
                   Style1._widthScale.Equals(Style2._widthScale) AndAlso Style1._Underscores = Style2._Underscores
        End Operator
        Public Shared Operator <>(Style1 As TextStyle, Style2 As TextStyle) As Boolean
            Return Not (Style1 = Style2)
        End Operator
        Public Overrides Function Equals(Style As Object) As Boolean
            If ReferenceEquals(Me, Style) Then Return True
            If Style Is Nothing OrElse Style.GetType() IsNot Me.GetType() Then Return False

            Dim other As TextStyle = CType(Style, TextStyle)

            Return ((_BorderCol = Nothing AndAlso other._BorderCol = Nothing) OrElse
                   (_BorderCol <> Nothing AndAlso _BorderCol.Equals(other._BorderCol))) AndAlso
                   _justify = other._justify AndAlso _Col.Equals(other._Col) AndAlso
                   _proport = other._proport AndAlso _nativeScale.Equals(other._nativeScale) AndAlso
                   _font = other._font AndAlso _ShadowEnabled = other._ShadowEnabled AndAlso _Background.Equals(other._Background) AndAlso
                   _sizeValue = other._sizeValue AndAlso _widthScale.Equals(other._widthScale) AndAlso _Underscores = other._Underscores
        End Function
        Public Overrides Function GetHashCode() As Integer
            Return _BorderCol.GetHashCode() Xor _justify.GetHashCode() Xor _Col.GetHashCode() Xor _proport.GetHashCode() Xor _nativeScale.GetHashCode() Xor _sizeValue.GetHashCode() Xor _widthScale.GetHashCode() Xor _font.GetHashCode() Xor _ShadowEnabled.GetHashCode() Xor _Underscores.GetHashCode() Xor _Background.GetHashCode()
        End Function
        'SET_TEXT_DRAW_BEFORE_FADE
        'SET_TEXT_INPUT_ACTIVE
        'SET_TEXT_LINE_DISPLAY
    End Class



    Friend NotInheritable Class DrawManager
        Private Shared ReadOnly ActiveDraws As New List(Of DrawRequest)
        Private Shared ReadOnly ActiveDraws3D As New List(Of DrawRequest3D)
        Private Shared ReadOnly ActiveBigPrints As New Dictionary(Of BigPrintType, BigPrintRequest)
        Private Shared ReadOnly QueuedBigPrints As New Dictionary(Of BigPrintType, Queue(Of BigPrintRequest))
        Private Shared ActiveHelpRequest As DrawRequest = Nothing
        Private Shared ReadOnly QueuedHelpRequests As New Queue(Of DrawRequest)
        Private Shared isRunning As Boolean = False
        Private Shared Sub EnsureRunning()
            If isRunning Then Return
            isRunning = True
            TickHelper.Add_Internal(AddressOf Tick)
        End Sub
        Public Shared Sub DrawText(Text As String, Duration As Integer, Position As Vector2, Style As TextStyle, Optional BackgroundSize As Vector2 = Nothing,
                                   Optional Wrap As (StartPos As Single?, EndPos As Single?, LineHeightMultiplier As Single?) = Nothing,
                                   Optional ViewportID As Integer? = Nothing, Optional RenderID As Integer? = Nothing, Optional BackgroundPosition As Vector2? = Nothing)

            If Duration < 1 Then Return

            Dim Request As New DrawRequest(
    Text,
    Duration,
    Position,
    Style,
    BackgroundSize,
    Wrap:=Wrap,
    ViewportID:=ViewportID,
    RenderID:=RenderID,
    BackgroundPosition:=BackgroundPosition)

            ActiveDraws.Add(Request)
            EnsureRunning()
        End Sub
        Public Shared Sub DrawGxtText(
        GxtEntry As String,
        Duration As Integer,
        Position As Vector2,
        Style As TextStyle,
        Optional BackgroundSize As Vector2 = Nothing,
Optional BackgroundPosition As Vector2? = Nothing,
Optional LineHeightMultiplier As Single = 1.0F)

            If Duration < 1 Then Return

            Dim Request As New DrawRequest(
    GxtEntry,
    Duration,
    Position,
    Style,
    True,
    BackgroundSize,
    BackgroundPosition,
    LineHeightMultiplier)

            ActiveDraws.Add(Request)
            EnsureRunning()

        End Sub
        Public Shared Sub DrawPersistentText(Key As String, Text As String, Duration As Integer, Position As Vector2, Style As TextStyle, Optional TextLines As String() = Nothing, Optional LineSpacingY As Single = 0.0F)

            If String.IsNullOrEmpty(Key) Then
                Throw New ArgumentException(
                "A persistent draw request requires a key.",
                NameOf(Key))
            End If

            If Duration < 1 Then Return
            Dim FinalLines As String() = Nothing
            If TextLines IsNot Nothing Then FinalLines = CType(TextLines.Clone(), String())
            For Each Request As DrawRequest In ActiveDraws

                If Request.PersistentKey = Key Then

                    Request.Refresh(
                    Text,
                    Duration,
                    Position,
                    Style)
                    Request.TextLines = FinalLines
                    Request.LineSpacingY = If(FinalLines IsNot Nothing, Math.Max(0.0F, LineSpacingY), 0.0F)

                    EnsureRunning()
                    Return

                End If

            Next

            Dim NewRequest As New DrawRequest(
            Text,
            Duration,
            Position,
            Style,
            PersistentKey:=Key)
            NewRequest.TextLines = FinalLines
            NewRequest.LineSpacingY = If(FinalLines IsNot Nothing, Math.Max(0.0F, LineSpacingY), 0.0F)
            ActiveDraws.Add(NewRequest)

            EnsureRunning()

        End Sub
        Public Shared Sub DrawRect(
        Position As Vector2,
        Size As Vector2,
        Color As ColorRGBA,
        Duration As Integer,
        Optional BordersThickness As Single = 0.0F,
        Optional BordersColor As ColorRGBA = Nothing)

            If Duration < 1 Then Return

            ActiveDraws.Add(
            New DrawRequest(
                Position,
                Size,
                Duration,
                Color,
                BordersColor,
                BordersThickness))

            EnsureRunning()

        End Sub
        Public Shared Sub DrawGameTexture(
        Texture As GameTexture,
        Position As Vector2,
        Size As Vector2,
        Rotation As Single,
        Color As ColorRGBA,
        Duration As Integer)

            If Texture Is Nothing OrElse Duration < 1 Then Return

            ActiveDraws.Add(
            New DrawRequest(
                Texture,
                Position,
                Size,
                Rotation,
                Color,
                Duration))

            EnsureRunning()

        End Sub
        ''' <summary>
        ''' Registers a curved window and its optional title area as a single
        ''' draw request. Both share the same duration and lifecycle.
        ''' The supplied position represents the top-left corner.
        ''' </summary>
        ''' <param name="Position">The normalized top-left position.</param>
        ''' <param name="Size">The normalized window size.</param>
        ''' <param name="Alpha">The window opacity, from 0 to 255.</param>
        ''' <param name="Duration">Display duration in milliseconds.</param>
        ''' <param name="TitleLabelHeight">The optional title area height.</param>
        Public Shared Sub DrawCurvedWindow(Position As Vector2, Size As Vector2, Alpha As Integer, Duration As Integer, Optional TitleLabelHeight As Single = 0.0F)
            If Duration < 1 Then Return
            Dim Request As New DrawRequest(Position, Size, Duration, Alpha, TitleLabelHeight)
            ActiveDraws.Add(Request)
            EnsureRunning()
        End Sub
        Public Shared Sub DrawWindow(Position As Vector2, Size As Vector2, Color As ColorRGBA, Duration As Integer, BordersColor As ColorRGBA, BordersThickness As Single, TitleLabelHeight As Single)
            If Duration < 1 Then Return
            Dim Request As New DrawRequest(Position, Size, Duration, Color, BordersColor, BordersThickness, TitleLabelHeight:=TitleLabelHeight, IsWindow:=True)
            ActiveDraws.Add(Request)
            EnsureRunning()
        End Sub
        Public Shared Sub DrawFrontBuff(
        Position As Vector2,
        Size As Vector2,
        Rotation As Single,
        Color As ColorRGBA,
        Duration As Integer)

            If Duration < 1 Then Return

            ActiveDraws.Add(
            New DrawRequest(
                Position,
                Size,
                Duration,
                Rotation,
                Color))

            EnsureRunning()

        End Sub
        Public Shared Sub DrawBigPrintText(Text As String, Duration As Integer, Type As BigPrintType, Position As Vector2, Style As TextStyle, Mode As QueueMode)

            If Duration < 1 Then Return

            Dim Request As New BigPrintRequest(
            Text,
            Duration,
            Type,
            Position,
            Style)

            Select Case Mode
                Case QueueMode.DisplayNow
                    ClearBigPrintText(Type)
                    StartBigPrint(Request)
                    'RenderBigPrint(Request)

                Case QueueMode.AddToQueue
                    If ActiveBigPrints.ContainsKey(Type) Then
                        Dim Pending As Queue(Of BigPrintRequest) = Nothing
                        If Not QueuedBigPrints.TryGetValue(Type, Pending) Then
                            Pending = New Queue(Of BigPrintRequest)
                            QueuedBigPrints.Add(Type, Pending)
                        End If
                        Pending.Enqueue(Request)
                    Else
                        StartBigPrint(Request)
                        'RenderBigPrint(Request)
                    End If

                Case QueueMode.IgnoreIfBusy
                    Dim Pending As Queue(Of BigPrintRequest) = Nothing
                    If ActiveBigPrints.ContainsKey(Type) OrElse (QueuedBigPrints.TryGetValue(Type, Pending) AndAlso Pending.Count > 0) Then Return
                    StartBigPrint(Request)
                    'RenderBigPrint(Request)

                Case Else
                    Throw New ArgumentOutOfRangeException(NameOf(Mode), Mode, "Unknown message queue mode.")
            End Select

            EnsureRunning()
        End Sub
        Public Shared Sub DrawHelpText(Text As String, Duration As Integer, Position As Vector2, Style As TextStyle, BackgroundSize As Vector2, Wrap As (StartPos As Single?, EndPos As Single?, LineHeightMultiplier As Single?), BackgroundPosition As Vector2?, Mode As QueueMode, PlaySound As Boolean)
            If Duration = 0 OrElse Duration < -1 Then Throw New ArgumentOutOfRangeException(NameOf(Duration), "Duration must be -1 (forever) or positive.")
            Dim Request As New DrawRequest(Text, Duration, Position, Style, BackgroundSize, Wrap:=Wrap, BackgroundPosition:=BackgroundPosition)
            Request.PlaySoundOnStart = PlaySound
            Select Case Mode
                Case QueueMode.DisplayNow
                    ActiveHelpRequest = Nothing
                    QueuedHelpRequests.Clear()
                    StartHelpRequest(Request)
                Case QueueMode.AddToQueue
                    If ActiveHelpRequest Is Nothing Then StartHelpRequest(Request) Else QueuedHelpRequests.Enqueue(Request)
                Case QueueMode.IgnoreIfBusy
                    If ActiveHelpRequest IsNot Nothing OrElse QueuedHelpRequests.Count > 0 Then Return
                    StartHelpRequest(Request)
                Case Else
                    Throw New ArgumentOutOfRangeException(NameOf(Mode), Mode, "Unknown message queue mode.")
            End Select
            EnsureRunning()
        End Sub
        '--------------
        Private Shared Sub StartHelpRequest(Request As DrawRequest)
            ActiveHelpRequest = Request
            Request.StartTime = NativeTimer.TotalGameTime
            Request.HasRendered = False
            [Call]("CLEAR_HELP")
            If Request.PlaySoundOnStart Then NativeAudio.Trigger(NativeAudio.GameSounds.HelpMessage)
        End Sub
        Private Shared Sub ProcessHelpRequests()
            If ActiveHelpRequest IsNot Nothing AndAlso ActiveHelpRequest.IsExpired Then ActiveHelpRequest = Nothing
            If ActiveHelpRequest Is Nothing AndAlso QueuedHelpRequests.Count > 0 Then StartHelpRequest(QueuedHelpRequests.Dequeue())
        End Sub
        Public Shared Sub ClearHelpRequests()
            ActiveHelpRequest = Nothing
            QueuedHelpRequests.Clear()
        End Sub
        ' ============================================================
        ' RICHIESTE 3D
        ' ============================================================
        Public Shared Sub DrawCorona(
        Position As GTA.Vector3,
        Radius As Single,
        Color As ColorRGB,
        Intensity As Single,
        Duration As Integer)

            If Duration < 1 OrElse Radius <= 0.0F OrElse Intensity <= 0.0F Then
                Return
            End If

            ActiveDraws3D.Add(
            New DrawRequest3D(
                Position,
                Duration,
                Color,
                Radius,
                Intensity))

            EnsureRunning()

        End Sub
        Public Shared Sub DrawLight(
        Position As GTA.Vector3,
        Color As ColorRGB,
        Intensity As Single,
        Range As Single,
        CoronaRadius As Single,
        CoronaIntensity As Single,
        Duration As Integer)

            If Duration < 1 Then Return

            ActiveDraws3D.Add(
            New DrawRequest3D(
                Position,
                Duration,
                Color,
                Intensity,
                Range,
                CoronaRadius,
                CoronaIntensity))

            EnsureRunning()

        End Sub
        Public Shared Sub DrawArrow(
        Position As GTA.Vector3,
        Color As ColorRGBA,
        Unknown01 As Single,
        Unknown02 As Single,
        Duration As Integer)

            If Duration < 1 Then Return

            ActiveDraws3D.Add(
            New DrawRequest3D(
                Position,
                Duration,
                Color,
                Unknown01,
                Unknown02))

            EnsureRunning()

        End Sub
        Public Shared Sub DrawCheckpoint(Position As GTA.Vector3, Radius As Single, Color As ColorRGBA, UseSphere As Boolean, Duration As Integer)
            If Duration < 1 Then Return
            ActiveDraws3D.Add(New DrawRequest3D(Position, Duration, Radius, Color, UseSphere))
            EnsureRunning()
        End Sub
        ' ============================================================
        Private Shared Sub StartBigPrint(Request As BigPrintRequest)

            Request.StartTime = NativeTimer.TotalGameTime

            ' Nasconde l'eventuale testo nativo presente
            ' nella stessa posizione.

            [Call]("PRINT_BIG", "STRING", 2, CInt(Request.Type))

            ActiveBigPrints(Request.Type) = Request

        End Sub
        Private Shared Sub RenderBigPrint(Request As BigPrintRequest)

            TextStyle.SetTextStyle(
            Request.Style,
            (StartPos:=0.0F,
             EndPos:=1.0F,
             LineHeightMultiplier:=1.0F))

            [Call](
            "DISPLAY_TEXT_WITH_LITERAL_STRING",
            Request.Position.X,
            Request.Position.Y,
            "STRING",
            Request.Text)

        End Sub
        Private Shared Sub ProcessBigPrints()

            Dim ExpiredTypes As New List(Of BigPrintType)

            For Each Pair As KeyValuePair(Of BigPrintType, BigPrintRequest) _
            In ActiveBigPrints

                If Pair.Value.IsExpired Then
                    ExpiredTypes.Add(Pair.Key)
                End If

            Next

            For Each Type As BigPrintType In ExpiredTypes

                ActiveBigPrints.Remove(Type)

                Dim Pending As Queue(Of BigPrintRequest) = Nothing

                If QueuedBigPrints.TryGetValue(Type, Pending) Then

                    If Pending.Count > 0 Then

                        ' Il successivo messaggio inizia solo quando
                        ' il precedente è effettivamente terminato.

                        Dim NextRequest As BigPrintRequest = Pending.Dequeue()

                        StartBigPrint(NextRequest)

                    End If

                    If Pending.Count = 0 Then
                        QueuedBigPrints.Remove(Type)
                    End If

                End If

            Next

        End Sub
        Public Shared Sub ClearBigPrintText(Type As BigPrintType)

            ActiveBigPrints.Remove(Type)
            QueuedBigPrints.Remove(Type)

        End Sub
        Public Shared Sub ClearAllBigPrintTexts()
            ActiveBigPrints.Clear()
            QueuedBigPrints.Clear()
        End Sub
        Private Shared Sub RenderRectangle(
    Position As Vector2,
    Size As Vector2,
    Color As ColorRGBA,
    BordersThickness As Single,
    BordersColor As ColorRGBA)

            ' Fill.
            [Call](
        "DRAW_RECT",
        Position.X,
        Position.Y,
        Size.X,
        Size.Y,
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Alpha)

            If BordersThickness <= 0.0F Then Return

            Dim FinalBorderColor As ColorRGBA = BordersColor

            If FinalBorderColor = Nothing Then
                FinalBorderColor = ColorRGBA.Black
            End If

            Dim Aspect As Single = Tools.ScreenAspectRatio

            Dim ThicknessX As Single = BordersThickness
            Dim ThicknessY As Single = ThicknessX * Aspect

            Dim HalfX As Single = Size.X / 2.0F
            Dim HalfY As Single = Size.Y / 2.0F

            Dim HalfThicknessX As Single = ThicknessX / 2.0F
            Dim HalfThicknessY As Single = ThicknessY / 2.0F

            Dim HorizontalWidth As Single =
        Size.X + ThicknessX * 2.0F

            Dim TopCenterY As Single =
        Position.Y - HalfY - HalfThicknessY

            Dim BottomCenterY As Single =
        Position.Y + HalfY + HalfThicknessY

            ' Top border.
            [Call](
        "DRAW_RECT",
        Position.X,
        TopCenterY,
        HorizontalWidth,
        ThicknessY,
        FinalBorderColor.Red,
        FinalBorderColor.Green,
        FinalBorderColor.Blue,
        FinalBorderColor.Alpha)

            ' Bottom border.
            [Call](
        "DRAW_RECT",
        Position.X,
        BottomCenterY,
        HorizontalWidth,
        ThicknessY,
        FinalBorderColor.Red,
        FinalBorderColor.Green,
        FinalBorderColor.Blue,
        FinalBorderColor.Alpha)

            Dim LeftCenterX As Single =
        Position.X - HalfX - HalfThicknessX

            Dim RightCenterX As Single =
        Position.X + HalfX + HalfThicknessX

            ' Left border.
            [Call](
        "DRAW_RECT",
        LeftCenterX,
        Position.Y,
        ThicknessX,
        Size.Y,
        FinalBorderColor.Red,
        FinalBorderColor.Green,
        FinalBorderColor.Blue,
        FinalBorderColor.Alpha)

            ' Right border.
            [Call](
        "DRAW_RECT",
        RightCenterX,
        Position.Y,
        ThicknessX,
        Size.Y,
        FinalBorderColor.Red,
        FinalBorderColor.Green,
        FinalBorderColor.Blue,
        FinalBorderColor.Alpha)

        End Sub
        Private Shared Sub RenderTextRequest(Request As DrawRequest)
            RenderTextBackground(Request)
            If Request.TextLines IsNot Nothing Then
                For i As Integer = 0 To Request.TextLines.Length - 1
                    TextStyle.SetTextStyle(Request.Style, Request.Wrap, Request.ViewportID, Request.RenderID)

                    [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING",
                   Request.Position.X,
                   Request.Position.Y + (i * Request.LineSpacingY),
                   "STRING",
                   Request.TextLines(i))
                Next
            ElseIf Request.Text IsNot Nothing AndAlso Request.Text.IndexOf(TextColors.FX_LineBreak, StringComparison.Ordinal) >= 0 Then
                Dim ExplicitLines As String() = NativeDraws.SplitTextAtLineBreak(Request.Text)
                NativeDraws.RenderLiteralTextLines(ExplicitLines, Request.Position, Request.Style, Request.Wrap, Request.ViewportID, Request.RenderID)
            Else
                TextStyle.SetTextStyle(Request.Style, Request.Wrap, Request.ViewportID, Request.RenderID)
                [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", Request.Position.X, Request.Position.Y, "STRING", Request.Text)
            End If
        End Sub
        Private Shared Sub RenderTextBackground(Request As DrawRequest)

            If Request.Style Is Nothing Then Return

            If Request.Style.Background.Alpha = 0 Then Return

            If Request.BackgroundSize.X <= 0.0F OrElse
       Request.BackgroundSize.Y <= 0.0F Then Return

            NativeDraws.DrawRectangle(
        Request.BackgroundPosition,
        Request.BackgroundSize,
        Request.Style.Background,
        Duration:=1)

        End Sub
        Private Shared Sub RenderHelpTextRequest(Request As DrawRequest)
            RenderTextBackground(Request)

            If Request.Text IsNot Nothing AndAlso Request.Text.IndexOf(TextColors.FX_LineBreak, StringComparison.Ordinal) >= 0 Then
                Dim ExplicitLines As String() = NativeDraws.SplitTextAtLineBreak(Request.Text)
                NativeDraws.RenderLiteralTextLines(ExplicitLines, Request.Position, Request.Style, Request.Wrap, Request.ViewportID, Request.RenderID)
            Else
                TextStyle.SetTextStyle(Request.Style, Request.Wrap, Request.ViewportID, Request.RenderID)
                [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", Request.Position.X, Request.Position.Y, "STRING", Request.Text)
            End If
        End Sub
        Private Shared Sub Tick()
            ActiveDraws.RemoveAll(Function(Request) Request.IsExpired)
            ActiveDraws3D.RemoveAll(Function(Request) Request.IsExpired)
            ProcessBigPrints()
            ProcessHelpRequests()
            If ActiveDraws.Count = 0 AndAlso ActiveDraws3D.Count = 0 AndAlso ActiveBigPrints.Count = 0 AndAlso
                ActiveHelpRequest Is Nothing AndAlso QueuedHelpRequests.Count = 0 AndAlso QueuedBigPrints.Count = 0 Then
                isRunning = False
                TickHelper.Remove_Internal(AddressOf Tick)
                Return
            End If

            For Each Request As DrawRequest In ActiveDraws
                Select Case Request.Type
                    Case DrawRequest.DrawType.TextAdvanced
                        RenderTextRequest(Request)

                    Case DrawRequest.DrawType.GxtText
                        RenderTextBackground(Request)
                        If Request.Style IsNot Nothing Then
                            TextStyle.SetTextStyle(Request.Style, Request.Wrap, Request.ViewportID, Request.RenderID)
                        End If
                        [Call]("DISPLAY_TEXT", Request.Position.X, Request.Position.Y, Request.Text)

                    Case DrawRequest.DrawType.Rectangle
                        RenderRectangle(Request.Position, Request.Size, Request.Color_NoText, Request.BordersThickness, Request.BordersColor)

                    Case DrawRequest.DrawType.Window
                        RenderRectangle(Request.Position, Request.Size, Request.Color_NoText, Request.BordersThickness, Request.BordersColor)
                        If Request.TitleLabelHeight > 0.0F Then
                            Dim FinalTitleHeight As Single = Math.Min(Request.TitleLabelHeight, Request.Size.Y)
                            Dim TitleCenterY As Single = Request.Position.Y - (Request.Size.Y / 2.0F) + (FinalTitleHeight / 2.0F)
                            Dim TitlePosition As New Vector2(Request.Position.X, TitleCenterY)
                            Dim TitleSize As New Vector2(Request.Size.X, FinalTitleHeight)

                            RenderRectangle(TitlePosition, TitleSize, Request.Color_NoText, Request.BordersThickness, Request.BordersColor)
                        End If


                    Case DrawRequest.DrawType.CurvedWindow
                        [Call]("DRAW_CURVED_WINDOW", Request.Position.X, Request.Position.Y, Request.Size.X, Request.Size.Y, Request.CurvedWindowAlpha)
                        If Request.TitleLabelHeight > 0.0F Then
                            Dim FinalTitleHeight As Single = Math.Min(Request.TitleLabelHeight, Request.Size.Y)

                            [Call]("DRAW_CURVED_WINDOW", Request.Position.X, Request.Position.Y, Request.Size.X, FinalTitleHeight, Request.CurvedWindowAlpha)
                        End If

                    Case DrawRequest.DrawType.GameTexture
                        If Request.GameTexture Is Nothing OrElse Not Request.GameTexture.Load() Then Continue For
                        [Call]("DRAW_SPRITE", Request.GameTexture.Handle, Request.Position.X, Request.Position.Y, Request.Size.X, Request.Size.Y, Request.Rotation,
                               Request.Color_NoText.Red, Request.Color_NoText.Green, Request.Color_NoText.Blue, Request.Color_NoText.Alpha)


                    Case DrawRequest.DrawType.FrontBuff
                        [Call]("DRAW_SPRITE_FRONT_BUFF", Request.Position.X, Request.Position.Y, Request.Size.X, Request.Size.Y, Request.Rotation,
                               Request.Color_NoText.Red, Request.Color_NoText.Green, Request.Color_NoText.Blue, Request.Color_NoText.Alpha)
                End Select
                Request.MarkRendered()
            Next

            For Each Request As DrawRequest3D In ActiveDraws3D
                Request.Render()
                Request.MarkRendered()
            Next

            For Each Pair As KeyValuePair(Of BigPrintType, BigPrintRequest) _
            In ActiveBigPrints
                RenderBigPrint(Pair.Value)
            Next
            If ActiveHelpRequest IsNot Nothing Then
                RenderHelpTextRequest(ActiveHelpRequest)
                ActiveHelpRequest.MarkRendered()
            End If
        End Sub

        Private Class DrawRequest

            Public Enum DrawType
                TextAdvanced = 0
                Rectangle = 1
                CurvedWindow = 2
                FrontBuff = 3
                GameTexture = 4
                GxtText = 5
                Window = 6
            End Enum

            Public Text As String
            Public TextLines As String()
            Public LineSpacingY As Single
            Public PlaySoundOnStart As Boolean
            Public Position As Vector2
            Public Style As TextStyle

            Public Duration As Integer
            Public StartTime As Integer
            Public HasRendered As Boolean = False

            Public Type As DrawType

            Public Size As Vector2
            Public Color_NoText As ColorRGBA

            Public CurvedWindowAlpha As Integer
            Public ReadOnly TitleLabelHeight As Single

            Public GameTexture As GameTexture
            Public Rotation As Single

            Public BordersColor As ColorRGBA
            Public BordersThickness As Single
            Public ReadOnly Wrap As (StartPos As Single?, EndPos As Single?, LineHeightMultiplier As Single?)
            Public ReadOnly ViewportID As Integer?
            Public ReadOnly RenderID As Integer?

            Public BackgroundSize As Vector2
            Public BackgroundPosition As Vector2

            Public PersistentKey As String

            'Custom Text (DrawText)
            Public Sub New(Text As String, Duration As Integer, Position As Vector2, Style As TextStyle, Optional BackgroundSize As Vector2 = Nothing, Optional PersistentKey As String = Nothing,
                           Optional Wrap As (StartPos As Single?, EndPos As Single?, LineHeightMultiplier As Single?) = Nothing, Optional ViewportID As Integer? = Nothing, Optional RenderID As Integer? = Nothing,
                           Optional BackgroundPosition As Vector2? = Nothing)

                Me.Text = If(Text, String.Empty)
                Me.Position = Position
                Me.Style = Style
                Me.Duration = Duration
                Me.StartTime = NativeTimer.TotalGameTime

                Me.BackgroundSize = BackgroundSize
                Me.BackgroundPosition = If(BackgroundPosition.HasValue, BackgroundPosition.Value, Position)

                Me.PersistentKey = PersistentKey

                Me.Wrap = Wrap
                Me.ViewportID = ViewportID
                Me.RenderID = RenderID

                Type = DrawType.TextAdvanced
            End Sub

            'GXT Texts (GXT.Draw)
            Public Sub New(
    GxtEntry As String,
    Duration As Integer,
    Position As Vector2,
    Style As TextStyle,
    IsGxtText As Boolean,
    Optional BackgroundSize As Vector2 = Nothing,
    Optional BackgroundPosition As Vector2? = Nothing,
    Optional LineHeightMultiplier As Single = 1.0F)

                Me.Text = GxtEntry
                Me.Position = Position
                Me.Style = Style
                Me.Duration = Duration
                Me.StartTime = NativeTimer.TotalGameTime

                Me.BackgroundSize = BackgroundSize
                Me.BackgroundPosition =
        If(BackgroundPosition.HasValue, BackgroundPosition.Value, Position)

                Me.Wrap = (
        StartPos:=CType(0.0F, Single?),
        EndPos:=CType(1.0F, Single?),
        LineHeightMultiplier:=CType(
            ClampFloat(LineHeightMultiplier, 0.7F, 10.0F),
            Single?))

                Type = DrawType.GxtText

            End Sub

            'Rectangle / Window
            Public Sub New(RectPos As Vector2, RectSize As Vector2, Duration As Integer, Color As ColorRGBA, Optional BordersColor As ColorRGBA = Nothing,
                           Optional BordersThickness As Single = 0.0F, Optional TitleLabelHeight As Single = 0.0F, Optional IsWindow As Boolean = False)

                Position = RectPos
                Size = RectSize
                Me.Duration = Duration
                Me.Color_NoText = Color
                Me.BordersColor = BordersColor
                Me.BordersThickness = BordersThickness
                Me.TitleLabelHeight = Math.Max(0.0F, TitleLabelHeight)
                StartTime = NativeTimer.TotalGameTime
                If IsWindow Then Type = DrawType.Window Else Type = DrawType.Rectangle
            End Sub

            'Front Buff
            Public Sub New(Position As Vector2, Size As Vector2, Duration As Integer, Rotation As Single, Color As ColorRGBA)
                Me.Position = Position
                Me.Size = Size
                Me.Duration = Duration
                Me.Rotation = Rotation
                Me.Color_NoText = Color
                Me.StartTime = NativeTimer.TotalGameTime
                Type = DrawType.FrontBuff
            End Sub

            'Game Texture
            Public Sub New(Texture As GameTexture, Position As Vector2, Size As Vector2, Rotation As Single, Color As ColorRGBA, Duration As Integer)
                Me.GameTexture = Texture
                Me.Position = Position
                Me.Size = Size
                Me.Rotation = Rotation
                Me.Color_NoText = Color
                Me.Duration = Duration
                Me.StartTime = NativeTimer.TotalGameTime
                Type = DrawType.GameTexture
            End Sub

            'Curved Window
            Public Sub New(CurvedWindowPos As Vector2, CurvedWindowSize As Vector2, Duration As Integer, Alpha As Integer, Optional TitleLabelHeight As Single = 0.0F)
                Position = CurvedWindowPos
                Size = CurvedWindowSize
                Me.Duration = Duration
                CurvedWindowAlpha = Alpha
                Me.TitleLabelHeight = Math.Max(0.0F, TitleLabelHeight)
                StartTime = NativeTimer.TotalGameTime
                Type = DrawType.CurvedWindow
            End Sub

            Public Sub Refresh(Text As String, Duration As Integer, Position As Vector2, Style As TextStyle)
                Me.Text = If(Text, String.Empty)
                Me.Duration = Duration
                Me.Position = Position
                Me.Style = Style
                Me.StartTime = NativeTimer.TotalGameTime
                Me.HasRendered = False
                Me.TextLines = Nothing
                Me.LineSpacingY = 0.0F
                Me.PlaySoundOnStart = False
            End Sub
            Public Sub MarkRendered()
                If HasRendered Then Return

                StartTime = NativeTimer.TotalGameTime
                HasRendered = True
            End Sub
            Public ReadOnly Property IsExpired As Boolean
                Get
                    Return Duration >= 0 AndAlso HasRendered AndAlso
                        (NativeTimer.TotalGameTime - StartTime >= Duration)
                End Get
            End Property
        End Class

        Private Class DrawRequest3D

            Public Enum Draw3DType
                Corona
                Light
                Arrow
                Checkpoint
            End Enum

            Public ReadOnly Type As Draw3DType

            Public ReadOnly Position As GTA.Vector3
            Public ReadOnly Duration As Integer
            Public StartTime As Integer
            Public HasRendered As Boolean = False

            Public ReadOnly Red As Integer
            Public ReadOnly Green As Integer
            Public ReadOnly Blue As Integer
            Public ReadOnly Alpha As Integer

            Public ReadOnly Radius As Single
            Public ReadOnly Intensity As Single
            Public ReadOnly LightIntensity As Single
            Public ReadOnly Range As Single

            Public ReadOnly CoronaRadius As Single
            Public ReadOnly CoronaIntensity As Single

            Public ReadOnly Unknown01 As Single
            Public ReadOnly Unknown02 As Single

            Public ReadOnly Color As ColorRGBA
            Public ReadOnly UseSphere As Boolean


            Public Sub New(
            Position As GTA.Vector3,
            Duration As Integer,
            Color As ColorRGB,
            Radius As Single,
            Intensity As Single)

                Type = Draw3DType.Corona

                Me.Position = Position
                Me.Duration = Duration
                StartTime = NativeTimer.TotalGameTime

                Red = Color.Red
                Green = Color.Green
                Blue = Color.Blue

                Me.Radius = Radius
                Me.Intensity = Math.Max(0.0F, Math.Min(100.0F, Intensity))

            End Sub


            Public Sub New(
            Position As GTA.Vector3,
            Duration As Integer,
            Color As ColorRGB,
            Intensity As Single,
            Range As Single,
            CoronaRadius As Single,
            CoronaIntensity As Single)

                Type = Draw3DType.Light

                Me.Position = Position
                Me.Duration = Duration
                StartTime = NativeTimer.TotalGameTime

                Red = Color.Red
                Green = Color.Green
                Blue = Color.Blue

                LightIntensity = Intensity
                Me.Range = Range

                Me.CoronaRadius = CoronaRadius
                Me.CoronaIntensity =
                Math.Max(0.0F, Math.Min(100.0F, CoronaIntensity))

            End Sub


            Public Sub New(
            Position As GTA.Vector3,
            Duration As Integer,
            Color As ColorRGBA,
            Unknown01 As Single,
            Unknown02 As Single)

                Type = Draw3DType.Arrow

                Me.Position = Position
                Me.Duration = Duration
                StartTime = NativeTimer.TotalGameTime

                Me.Color = Color

                Me.Unknown01 = Unknown01
                Me.Unknown02 = Unknown02

            End Sub


            Public Sub New(
            Position As GTA.Vector3,
            Duration As Integer,
            Radius As Single,
            Color As ColorRGBA,
            UseSphere As Boolean)

                Type = Draw3DType.Checkpoint

                Me.Position = Position
                Me.Duration = Duration
                StartTime = NativeTimer.TotalGameTime

                Me.Radius = Radius
                Me.Color = Color
                Me.UseSphere = UseSphere

            End Sub


            Public Sub Render()

                Select Case Type

                    Case Draw3DType.Corona

                        Dim Factor As Single = Intensity / 100.0F

                        Dim R As Integer = CInt(Red * Factor)
                        Dim G As Integer = CInt(Green * Factor)
                        Dim B As Integer = CInt(Blue * Factor)

                        [Call](
                        "DRAW_CORONA",
                        Position.X,
                        Position.Y,
                        Position.Z,
                        Radius,
                        0,
                        0.0F,
                        R, G, B)


                    Case Draw3DType.Light

                        [Call](
                        "DRAW_LIGHT_WITH_RANGE",
                        Position.X,
                        Position.Y,
                        Position.Z,
                        Red, Green, Blue,
                        LightIntensity,
                        Range)

                        If CoronaRadius > 0.0F AndAlso
                       CoronaIntensity > 0.0F Then

                            Dim Factor As Single =
                            CoronaIntensity / 100.0F

                            [Call](
                            "DRAW_CORONA",
                            Position.X,
                            Position.Y,
                            Position.Z,
                            CoronaRadius,
                            0,
                            0.0F,
                            CInt(Red * Factor),
                            CInt(Green * Factor),
                            CInt(Blue * Factor))

                        End If


                    Case Draw3DType.Arrow

                        [Call](
                        "DRAW_COLOURED_CYLINDER",
                        Position.X,
                        Position.Y,
                        Position.Z,
                        Unknown01,
                        Unknown02,
                        Color.Red,
                        Color.Green,
                        Color.Blue,
                        Color.Alpha)


                    Case Draw3DType.Checkpoint

                        If UseSphere Then

                            [Call](
                            "DRAW_SPHERE",
                            Position.X,
                            Position.Y,
                            Position.Z,
                            Radius)

                        Else

                            [Call](
                            "DRAW_CHECKPOINT_WITH_ALPHA",
                            Position.X,
                            Position.Y,
                            Position.Z,
                            Radius,
                            Color.Red,
                            Color.Green,
                            Color.Blue,
                            Color.Alpha)

                        End If
                End Select

            End Sub
            Public Sub MarkRendered()
                If HasRendered Then Return

                StartTime = NativeTimer.TotalGameTime
                HasRendered = True
            End Sub

            Public ReadOnly Property IsExpired As Boolean
                Get
                    Return HasRendered AndAlso (NativeTimer.TotalGameTime - StartTime >= Duration)
                End Get
            End Property

        End Class

        Private Class BigPrintRequest

            Public ReadOnly Text As String
            Public ReadOnly Duration As Integer
            Public ReadOnly Type As BigPrintType

            Public ReadOnly Position As Vector2
            Public ReadOnly Style As TextStyle

            Public StartTime As Integer


            Public Sub New(
            Text As String,
            Duration As Integer,
            Type As BigPrintType,
            Position As Vector2,
            Style As TextStyle)

                Me.Text = If(Text, String.Empty)
                Me.Duration = Duration
                Me.Type = Type
                Me.Position = Position
                Me.Style = Style

                StartTime = NativeTimer.TotalGameTime

            End Sub


            Public ReadOnly Property IsExpired As Boolean
                Get
                    Return NativeTimer.TotalGameTime - StartTime >= Duration
                End Get
            End Property

        End Class

    End Class
    Private Shared Function CalculateTextBackgroundPosition(
    Position As Vector2,
    BackgroundSize As Vector2,
    StyleJustification As Justification,
    WrapStart As Single,
    WrapEnd As Single,
    Optional HorizontalPadding As Single = 0.0F) As Vector2

        Dim Result As Vector2 = Position
        Select Case StyleJustification
            Case Justification.Center
            Case Justification.Right
                Result.X -= BackgroundSize.X / 2.0F
                Result.X += HorizontalPadding
            Case Justification.Justify
                Result.X = (WrapStart + WrapEnd) / 2.0F
            Case Else
                Result.X += BackgroundSize.X / 2.0F
                Result.X -= HorizontalPadding
        End Select
        Result.Y += BackgroundSize.Y / 2.0F - BackgroundVerticalPadding
        Return Result
    End Function

    Private Shared Function SplitTextAtLineBreak(Text As String) As String()
        Return If(Text, String.Empty).Split(
        New String() {TextColors.FX_LineBreak},
        StringSplitOptions.None)
    End Function
    Private Shared Function GetLiteralTextLineWidth(TextLine As String) As Single
        If String.IsNullOrEmpty(TextLine) Then Return 0.0F

        Return [Call](Of Single)(
        "GET_STRING_WIDTH_WITH_STRING",
        "STRING",
        TextLine)
    End Function
    Private Shared Function GetLiteralTextLineCount(
    TextLine As String,
    StartPos As Single,
    EndPos As Single) As Integer

        ' Anche una linea vuota occupa una riga verticale.
        If String.IsNullOrEmpty(TextLine) Then Return 1

        Return Math.Max(
        1,
        [Call](Of Integer)(
            "GET_NUMBER_LINES_WITH_LITERAL_STRINGS",
            StartPos,
            EndPos,
            "STRING",
            "",
            TextLine))
    End Function
    Private Shared Sub RenderLiteralTextLines(
    TextLines As String(),
    Position As Vector2,
    Style As TextStyle,
    Wrap As (StartPos As Single?, EndPos As Single?, LineHeightMultiplier As Single?),
    Optional ViewportID As Integer? = Nothing,
    Optional RenderID As Integer? = Nothing,
    Optional PrecalculatedLineCounts As Integer() = Nothing)

        If TextLines Is Nothing OrElse TextLines.Length = 0 Then Return

        Dim StartPos As Single =
        If(Wrap.StartPos.HasValue, Wrap.StartPos.Value, 0.0F)

        Dim EndPos As Single =
        If(Wrap.EndPos.HasValue, Wrap.EndPos.Value, 1.0F)

        Dim LineHeightMultiplier As Single =
        If(
            Wrap.LineHeightMultiplier.HasValue,
            ClampFloat(Wrap.LineHeightMultiplier.Value, 0.7F, 10.0F),
            1.0F)

        Dim CurrentY As Single = Position.Y

        For i As Integer = 0 To TextLines.Length - 1

            ' Reimposta lo stile prima di ogni chiamata nativa.
            Dim SingleLineHeight As Single =
            TextStyle.SetTextStyle(
                Style,
                Wrap,
                ViewportID,
                RenderID)

            Dim CurrentLineCount As Integer

            If PrecalculatedLineCounts IsNot Nothing AndAlso
           i < PrecalculatedLineCounts.Length Then

                CurrentLineCount = PrecalculatedLineCounts(i)

            Else

                CurrentLineCount = GetLiteralTextLineCount(
                TextLines(i),
                StartPos,
                EndPos)

            End If

            [Call](
            "DISPLAY_TEXT_WITH_LITERAL_STRING",
            Position.X,
            CurrentY,
            "STRING",
            TextLines(i))

            ' Avanza anche di più righe se il segmento va a capo
            ' automaticamente all'interno del Wrap.
            CurrentY +=
            SingleLineHeight *
            LineHeightMultiplier *
            CurrentLineCount

        Next

    End Sub
    '=============================================================================================================================
    ' Debug Grid
    '--------------------
    Private Shared _drawDebugGrid As Boolean = False
    Private Shared _gridCenter As New Vector2(0.5F, 0.5F)
    Friend Shared Property GridCenter As Vector2
        Get
            Return _gridCenter
        End Get
        Set(value As Vector2)
            _gridCenter = ClampGridPosition(value)
        End Set
    End Property

    Private Shared ReadOnly GridColor As New ColorRGBA(0, 255, 0, 90)
    Private Const MainLineLength As Single = 0.2F
    Private Const MainLineThickness As Single = 0.0025F

    Private Shared ReadOnly GridCenterMarkerColor As New ColorRGBA(255, 0, 0, 255)
    Private Shared ReadOnly GridMediumStepMarkerColor As New ColorRGBA(255, 130, 0, 180)
    Private Shared ReadOnly GridMajorStepMarkerColor As New ColorRGBA(255, 220, 0, 180)
    Private Const GridCenterMarkerSize As Single = MainLineThickness / 2

    Private Const MinorStep As Single = 0.01F
    Private Const MinorThickness As Single = 0.0009F
    Private Const MinorLength As Single = 0.015F
    Private Const MediumStep As Single = 0.05F
    Private Const MediumThickness As Single = 0.0018F
    Private Const MediumLength As Single = 0.028F
    Private Const MajorStep As Single = 0.1F
    Private Const MajorThickness As Single = 0.002F
    Private Const MajorLength As Single = 0.04F '+ (MajorThickness / 2)

    Private Const MovementSpeed As Single = 0.2F
    Private Const SlowMovementSpeed As Single = 0.04F

    Private Shared _lastUpdate As DateTime = DateTime.UtcNow
    Private Shared _tickRegistered As Boolean = False
    Private Shared TextStyleLabels As New TextStyle(New ColorRGBA(0, 230, 0, 255), 9, Justification.Center, ColorRGBA.Black, TextStyle.Fonts.Dotted)
    Private Shared TextStyleCoords As New TextStyle(New ColorRGBA(0, 230, 0, 255), 12, Justification.Center, ColorRGBA.Black, TextStyle.Fonts.Narrow,, 1.4)
    Private Shared Sub StartDebugGrid()
        If _tickRegistered Then Return

        _lastUpdate = DateTime.UtcNow
        TickHelper.Add_Internal(AddressOf DebugGridTick)
        _tickRegistered = True
    End Sub
    Private Shared Sub StopDebugGrid()
        If Not _tickRegistered Then Return

        TickHelper.Remove_Internal(AddressOf DebugGridTick)
        _tickRegistered = False
    End Sub
    Private Shared Sub DebugGridTick()
        If Not _drawDebugGrid Then Return

        Dim now As DateTime = DateTime.UtcNow
        Dim deltaTime As Single = CSng((now - _lastUpdate).TotalSeconds)
        _lastUpdate = now

        If deltaTime < 0.0F Then
            deltaTime = 0.0F
        ElseIf deltaTime > 0.1F Then
            deltaTime = 0.1F
        End If

        UpdateGridPosition(deltaTime)
        DrawGrid()
    End Sub
    Private Shared Sub UpdateGridPosition(deltaTime As Single)
        Dim speed As Single = MovementSpeed
        If NativeControls.IsKeyPressed(Windows.Forms.Keys.LShiftKey, False) Then
            speed = SlowMovementSpeed
        End If

        Dim amount As Single = speed * deltaTime

        Dim x As Single = _gridCenter.X
        Dim y As Single = _gridCenter.Y

        If NativeControls.IsKeyPressed(Windows.Forms.Keys.Up, False) OrElse NativeControls.IsKeyPressed(Windows.Forms.Keys.NumPad8, False) Then
            y -= amount
        End If

        If NativeControls.IsKeyPressed(Windows.Forms.Keys.Down, False) OrElse NativeControls.IsKeyPressed(Windows.Forms.Keys.NumPad2, False) Then
            y += amount
        End If

        If NativeControls.IsKeyPressed(Windows.Forms.Keys.Left, False) OrElse NativeControls.IsKeyPressed(Windows.Forms.Keys.NumPad4, False) Then
            x -= amount
        End If

        If NativeControls.IsKeyPressed(Windows.Forms.Keys.Right, False) OrElse NativeControls.IsKeyPressed(Windows.Forms.Keys.NumPad6, False) Then
            x += amount
        End If

        _gridCenter = ClampGridPosition(New Vector2(x, y))
    End Sub
    Private Shared Sub ResetGridPosition()
        _gridCenter = New Vector2(0.5F, 0.5F)
    End Sub
    Private Shared Function ClampGridPosition(position As Vector2) As Vector2
        Dim x As Single = position.X
        Dim y As Single = position.Y

        If x < 0.0F Then
            x = 0.0F
        ElseIf x > 1.0F Then
            x = 1.0F
        End If

        If y < 0.0F Then
            y = 0.0F
        ElseIf y > 1.0F Then
            y = 1.0F
        End If

        Return New Vector2(x, y)
    End Function
    '============================================================
    'DRAW
    '=========================
    Private Shared Sub DrawGrid()
        Dim centerX As Single = _gridCenter.X
        Dim centerY As Single = _gridCenter.Y

        Dim gridMinX As Single = centerX - (MainLineLength / 2.0F)
        Dim gridMaxX As Single = centerX + (MainLineLength / 2.0F)
        Dim gridMinY As Single = centerY - (MainLineLength / 2.0F)
        Dim gridMaxY As Single = centerY + (MainLineLength / 2.0F)

        Dim i As Integer
        For i = -10 To 10
            Dim offset As Single = i * MinorStep
            Dim x As Single = centerX + offset

            If x < 0.0F OrElse x > 1.0F Then
                Continue For
            End If

            If i = 0 Then
                DrawLine(New Vector2(x, centerY), MainLineLength, MainLineThickness, GridColor, Vertical:=True, Center:=True, Duration:=1)
                Continue For
            End If
            Dim absoluteIndex As Integer = Math.Abs(i)

            If absoluteIndex Mod 10 = 0 Then '0.1
                DrawGridVertical(x, centerY, MajorLength, MajorThickness)
            ElseIf absoluteIndex Mod 5 = 0 Then '0.5
                DrawGridVertical(x, centerY, MediumLength, MediumThickness)
            Else '0.01
                DrawGridVertical(x, centerY, MinorLength, MinorThickness)
            End If
        Next

        For i = -10 To 10
            Dim offset As Single = i * MinorStep
            Dim y As Single = centerY + offset

            If y < 0.0F OrElse y > 1.0F Then
                Continue For
            End If

            If i = 0 Then
                DrawLine(New Vector2(centerX, y), MainLineLength, MainLineThickness, GridColor, Vertical:=False, Center:=True, Duration:=1)
                Continue For
            End If
            Dim absoluteIndex As Integer = Math.Abs(i)

            If absoluteIndex Mod 10 = 0 Then '0.1
                DrawGridHorizontal(centerX, y, MajorLength, MajorThickness)
            ElseIf absoluteIndex Mod 5 = 0 Then '0.5
                DrawGridHorizontal(centerX, y, MediumLength, MediumThickness)
            Else '0.01
                DrawGridHorizontal(centerX, y, MinorLength, MinorThickness)
            End If
        Next

        'Center marker
        DrawSquare(New Vector2(centerX, centerY), MajorThickness * 1.5, GridCenterMarkerColor, Duration:=1)
        'Medium step markers
        DrawSquare(New Vector2(centerX - 0.05, centerY), MajorThickness, GridMediumStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX + 0.05, centerY), MajorThickness, GridMediumStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX, centerY + 0.05), MajorThickness, GridMediumStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX, centerY - 0.05), MajorThickness, GridMediumStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX - 0.05, centerY - 0.05), MajorThickness, GridMediumStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX + 0.05, centerY - 0.05), MajorThickness, GridMediumStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX - 0.05, centerY + 0.05), MediumThickness, GridMediumStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX + 0.05, centerY + 0.05), MediumThickness, GridMediumStepMarkerColor, Duration:=1)
        'Major step markers
        DrawSquare(New Vector2(centerX - 0.1, centerY), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX + 0.1, centerY), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX, centerY + 0.1), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX, centerY - 0.1), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX - 0.1, centerY - 0.1), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX + 0.1, centerY - 0.1), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX - 0.1, centerY + 0.1), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)
        DrawSquare(New Vector2(centerX + 0.1, centerY + 0.1), MediumThickness * 1.5, GridMajorStepMarkerColor, Duration:=1)

        DrawXLabel(centerX - 0.0935F, centerY, -0.1F)
        DrawXLabel(centerX + 0.0935F, centerY, 0.1F)
        DrawYLabel(centerX, centerY - 0.1F, -0.1F)
        DrawYLabel(centerX, centerY + 0.09F, 0.1F)

        Dim centerText As String = String.Format("X: {0:0.000}    Y: {1:0.000}", centerX, centerY)
        DrawText(centerText, Duration:=1, PositionPercent:=New Vector2(0.5F, 0.977F), Style:=TextStyleCoords)
    End Sub
    Private Shared Sub DrawGridVertical(
        x As Single,
        centerY As Single,
        length As Single,
        thickness As Single)

        DrawLine(
            New Vector2(x, centerY),
            length,
            thickness,
            GridColor,
            Vertical:=True,
            Center:=True,
            Duration:=1)

    End Sub
    Private Shared Sub DrawGridHorizontal(
        centerX As Single,
        y As Single,
        length As Single,
        thickness As Single)

        DrawLine(
            New Vector2(centerX, y),
            length,
            thickness,
            GridColor,
            Vertical:=False,
            Center:=True,
            Duration:=1)

    End Sub
    Private Shared Sub DrawXLabel(x As Single, y As Single, value As Single)
        If x < -0.0F OrElse x > 1.0F Then Return

        Dim label As String
        If value > 0.0F Then label = "+" & value.ToString("0.0") Else label = value.ToString("0.0")

        Dim textY As Single = y + 0.012F
        If textY > 1.0F Then textY = y - 0.012F
        DrawText(label, Duration:=1, PositionPercent:=New Vector2(x, textY), Style:=TextStyleLabels)
    End Sub
    Private Shared Sub DrawYLabel(x As Single, y As Single, value As Single)
        If y < 0.0F OrElse y > 1.0F Then Return

        Dim label As String
        If value > 0.0F Then label = "+" & value.ToString("0.00") Else label = value.ToString("0.00")

        Dim textX As Single = x + 0.015F
        If textX > 1.0F Then textX = x - 0.015F

        DrawText(label, Duration:=1, PositionPercent:=New Vector2(textX, y), Style:=TextStyleLabels)
    End Sub
    '============================================================
    '=============================================================================================================================
    Friend Shared Function DrawText_Internal(Text As String, Optional Duration As Integer = 3000, Optional PositionPercent As Vector2 = Nothing, Optional Style As TextStyle = Nothing,
                                 Optional Wrap As (Length As Single?, LineHeightMultiplier As Single?) = Nothing, Optional ViewportID As Integer? = Nothing,
                                 Optional RenderID As Integer? = Nothing, Optional MeasureOnly As Boolean = False) As (Width As Single, Height As Single, LineCount As Integer)

        Dim FinalText As String = If(Text, String.Empty)
        Dim FinalStyle As TextStyle
        If Style Is Nothing Then FinalStyle = TextStyle.Default Else FinalStyle = Style

        If PositionPercent = Nothing Then PositionPercent = New Vector2(0.5F, 0.9F)

        Dim X As Single = PositionPercent.X
        Dim FinalWrap As (StartPos As Single, EndPos As Single, LineHeightMultiplier As Single)

        If Wrap.LineHeightMultiplier.HasValue Then
            FinalWrap.LineHeightMultiplier =
         ClampFloat(Wrap.LineHeightMultiplier.Value, 0.7F, 10.0F)
        Else
            FinalWrap.LineHeightMultiplier = 1.0F
        End If

        If Wrap.Length.HasValue Then
            Dim RequestedLength As Single = Math.Max(0.0F, Wrap.Length.Value)

            Select Case FinalStyle.Justification
                Case Justification.Center
                    FinalWrap.StartPos = X - (RequestedLength / 2.0F)
                    FinalWrap.EndPos = X + (RequestedLength / 2.0F)

                Case Justification.Right
                    FinalWrap.StartPos = X - RequestedLength
                    FinalWrap.EndPos = X

                Case Justification.Justify
                    FinalWrap.StartPos = X
                    FinalWrap.EndPos = X + RequestedLength

                Case Else
                    FinalWrap.StartPos = X
                    FinalWrap.EndPos = X + RequestedLength
            End Select
        Else
            FinalWrap.StartPos = 0.0F
            FinalWrap.EndPos = 1.0F
        End If

        Dim SingleLineHeight As Single = 0.0F
        SingleLineHeight = TextStyle.SetTextStyle(FinalStyle, FinalWrap, ViewportID, RenderID)

        Dim ExplicitLines As String() = Nothing
        Dim ExplicitLineCounts As Integer() = Nothing

        Dim StringWidth As Single = 0.0F
        Dim NumberOfLines As Integer = 0

        If FinalText.IndexOf(TextColors.FX_LineBreak, StringComparison.Ordinal) >= 0 Then

            ExplicitLines = SplitTextAtLineBreak(FinalText)

            ReDim ExplicitLineCounts(ExplicitLines.Length - 1)

            For i As Integer = 0 To ExplicitLines.Length - 1

                Dim CurrentWidth As Single =
            GetLiteralTextLineWidth(ExplicitLines(i))

                StringWidth = Math.Max(StringWidth, CurrentWidth)

                ExplicitLineCounts(i) = GetLiteralTextLineCount(
            ExplicitLines(i),
            FinalWrap.StartPos,
            FinalWrap.EndPos)

                NumberOfLines += ExplicitLineCounts(i)

            Next

        Else

            ' Mantiene invariato il comportamento per il testo
            ' che non contiene line break espliciti.
            StringWidth =
        [Call](Of Single)(
            "GET_STRING_WIDTH_WITH_STRING",
            "STRING",
            FinalText)

            If FinalText.Length > 0 Then

                NumberOfLines =
            [Call](Of Integer)(
                "GET_NUMBER_LINES_WITH_LITERAL_STRINGS",
                FinalWrap.StartPos,
                FinalWrap.EndPos,
                "STRING",
                "",
                FinalText)

                NumberOfLines = Math.Max(1, NumberOfLines)

            End If

        End If

        Dim TotalHeight As Single = CSng(NumberOfLines) * (SingleLineHeight * FinalWrap.LineHeightMultiplier)
        Dim BackgroundSize As New Vector2(0.0F, 0.0F)
        Dim BackgroundPosition As Vector2? = Nothing

        If FinalStyle.Background.Alpha > 0 AndAlso NumberOfLines > 0 Then

            Dim BackgroundWidth As Single

            If FinalStyle.Justification = NativeDraws.Justification.Justify Then

                ' L'intero wrap, non soltanto la larghezza del testo.
                BackgroundWidth = Math.Max(
         0.0F,
         FinalWrap.EndPos - FinalWrap.StartPos)

            Else

                BackgroundWidth = Math.Max(0.0F, StringWidth)

                If Wrap.Length.HasValue AndAlso Wrap.Length.Value > 0.0F Then
                    BackgroundWidth = Math.Min(
             BackgroundWidth,
             Wrap.Length.Value)
                End If

            End If

            BackgroundSize = New Vector2(
     BackgroundWidth + (BackgroundHorizontalPadding * 2.0F),
     TotalHeight + (BackgroundVerticalPadding * 2.0F))

            If BackgroundSize.X > 0.0F Then

                BackgroundPosition = CalculateTextBackgroundPosition(
         PositionPercent,
         BackgroundSize,
         FinalStyle.Justification,
         FinalWrap.StartPos,
         FinalWrap.EndPos,
         HorizontalPadding:=BackgroundHorizontalPadding)

            End If

        End If

        If Not MeasureOnly Then
            If Duration < 5 Then
                If BackgroundPosition.HasValue Then DrawRectangle(BackgroundPosition.Value, BackgroundSize, FinalStyle.Background, Duration:=1)

                If ExplicitLines IsNot Nothing Then

                    RenderLiteralTextLines(
        ExplicitLines,
        PositionPercent,
        FinalStyle,
        FinalWrap,
        ViewportID,
        RenderID,
        ExplicitLineCounts)

                Else

                    TextStyle.SetTextStyle(
        FinalStyle,
        FinalWrap,
        ViewportID,
        RenderID)

                    [Call](
        "DISPLAY_TEXT_WITH_LITERAL_STRING",
        PositionPercent.X,
        PositionPercent.Y,
        "STRING",
        FinalText)

                End If
            Else
                DrawManager.DrawText(FinalText, Duration, PositionPercent, FinalStyle, BackgroundSize, FinalWrap, ViewportID, RenderID, BackgroundPosition)
            End If
        End If
        Return (Width:=StringWidth, Height:=TotalHeight, LineCount:=NumberOfLines)
    End Function

End Class