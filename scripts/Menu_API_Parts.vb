Imports System.Windows.Forms
Imports GTA
Imports NativeExtended
Imports NativeExtended.NativeDraws
''' <summary>
''' Contains all visual and behavioral customization options for a NativeMenu.
''' </summary>
Public Class MenuAppearance
    ''' <summary>
    ''' Default: 1.55
    ''' </summary>
    Public Property Subsection_Brighten As Single = 1.55
    ''' <summary>
    ''' Default: 180
    ''' </summary>
    Public Property WindowAlpha As Byte = 180
    ''' <summary>
    ''' Default: (0.2F, 0.083F)
    ''' </summary>  
    Public Property MenuSize As (X As Single, Y As Single) = (0.2F, 0.083F)
    ''' <summary>
    ''' Default: (0.02F, 0.04F)
    ''' </summary>
    Public Property MenuPosition As (X As Single, Y As Single) = (0.02F, 0.04F)
    ''' <summary>
    ''' Default: 10
    ''' </summary>
    Public Property MaxVisibleItems As Integer = 10
    '-----
    ''' <summary>
    ''' Default: (0, 162, 232, 160)
    ''' </summary>
    Public Property Description_BoxColor As ColorRGBA = New ColorRGBA(0, 162, 232, 160)
    '''' <summary>
    '''' Default: 0.0195
    '''' </summary>
    'Public Property Description_PerLineHeight As Single = 0.0195F
    ''' <summary>
    ''' Default: 0.012
    ''' </summary>
    Public Property Description_PositionY As Single = 0.012F
    '''' <summary>
    '''' Default: 0.0294
    '''' </summary>
    'Public Property Description_TextWrapCoef As Single = 0.0294F
    '''' <summary>
    '''' Default: 0.032F
    '''' </summary>
    'Public Property Description_BaseOffsetY As Single = 0.032F
    ''' <summary>
    ''' Default: 0.008F
    ''' </summary>
    Public Property Description_BoxHeightAdjust As Single = 0.008F
    ''' <summary>
    ''' Default: (X:=0.005, Y:=0.005)
    ''' </summary>
    Public Property Description_TextPadding As (X As Single, Y As Single) = (X:=0.005F, Y:=0.005F)
    ''' <summary>
    ''' Default: 1.0F
    ''' </summary>
    Public Property Description_LineHeightMultiplier As Single = 1.0F
    '''' <summary>
    '''' Default: (Wide:=1.75F, Medium:=1.25F, Narrow:=0.4F, Space:=0.6F, DefaultChar:=1.0F)
    '''' </summary>
    'Public Property TextWrap_CharWidths As (Wide As Single, Medium As Single, Narrow As Single, Space As Single, DefaultChar As Single) =
    '                                       (Wide:=1.75F, Medium:=1.25F, Narrow:=0.4F, Space:=0.6F, DefaultChar:=1.0F)
    '-----
    ''' <summary>
    ''' Default: 0.018F
    ''' </summary>
    Public Property Items_StartOffsetY As Single = 0.018F
    ''' <summary>
    ''' Default: (X:=0.004F, Y:=0.008F)
    ''' </summary>
    Public Property Item_SelectedOffsets As (X As Single, Y As Single) = (X:=0.004F, Y:=0.008F)
    ''' <summary>
    ''' Default: (X:=0.02F, Y:=0.004F)
    ''' </summary>
    Public Property Item_SelectedPadding As (X As Single, Y As Single) = (X:=0.02F, Y:=0.004F)
    ''' <summary>
    ''' Default: 0.012F
    ''' </summary>
    Public Property Item_TextOffsetX As Single = 0.012F
    ''' <summary>
    ''' Default: (0, 162, 232, 180)
    ''' </summary>
    Public Property Item_SelectedColor As ColorRGBA = New ColorRGBA(0, 162, 232, 180)
    ''' <summary>
    ''' Default: 0.755F
    ''' </summary>
    Public Property Item_ValueOffsetX As Single = 0.755F
    ''' <summary>
    ''' Default: 100
    ''' </summary>
    Public Property Item_ValueChangeHighlightTime As Integer = 100
    ''' <summary>
    ''' Default: 0.033
    ''' </summary>
    Public Property Item_Height As Single = 0.033
    ''' <summary>
    ''' Use NativeDraws.TextColors. Default: Black
    ''' </summary>
    Public Property Item_DisabledArrowColor As String = TextColors.Black
    ''' <summary>
    ''' Use NativeDraws.TextColors. Default: Fx_Highlighted + MP_Orange
    ''' </summary>
    Public Property Item_HighlightArrowColor As String = TextColors.Fx_Highlighted & TextColors.MP_Orange
    '---- TextStyles
    ''' <summary>
    ''' Default: (Item_SelectedColor, 25, False, ColorRGBA.Black, TextStyle.Fonts.Narrow)
    ''' </summary>
    Public Property TextStyle_Title As TextStyle
        Get
            If _customTextStyleTitle IsNot Nothing Then
                Return _customTextStyleTitle
            End If

            ' Return New TextStyle(ColorRGBA.Brighten(Item_SelectedColor, Title_Brighten), New Vector2(0.255, 0.5), False, ColorRGBA.Black, TextStyle.Fonts.Narrow)
            Return New TextStyle(Item_SelectedColor, 25, False, ColorRGBA.Black, TextStyle.Fonts.Narrow)
        End Get
        Set(value As TextStyle)
            _customTextStyleTitle = value
        End Set
    End Property
    ''' <summary>
    ''' Default: (ColorRGBA.Brighten(Item_SelectedColor, Subsection_Brighten), 17)
    ''' </summary>
    Public Property TextStyle_Subsection As TextStyle
        Get
            If _customTextStyleSubsection IsNot Nothing Then
                Return _customTextStyleSubsection
            End If

            Return New TextStyle(ColorRGBA.Brighten(Item_SelectedColor, Subsection_Brighten), 17)
        End Get
        Set(value As TextStyle)
            _customTextStyleSubsection = value
        End Set
    End Property
    Public Property TextStyle_Item As New TextStyle(ColorRGBA.White, 13)
    ''' <summary>
    ''' Default: (ColorRGBA.White, 13, True)
    ''' </summary>
    Public Property TextStyle_ItemValue As New TextStyle(ColorRGBA.White, 13, Justification.Center)
    ''' <summary>
    ''' Default: (Color.RGBA.white, 13)
    ''' </summary>
    Public Property TextStyle_Description As New TextStyle(ColorRGBA.White, 13)
    Friend ReadOnly Property Description_ContentWidth As Single
        Get
            Dim width As Single = MenuSize.X

            ' Left padding (text start)
            width -= Description_TextPadding.X

            ' Right padding (same as left for symmetry)
            width -= Description_TextPadding.X

            ' Scrollbar space (if enabled)
            If Scrollbar_Enable Then
                width -= (Scrollbar_Offsets.X + Scrollbar_Width.Background)
            End If

            Return Math.Max(0.0F, width)
        End Get
    End Property
    Friend ReadOnly Property Description_TextWrapMaxWidth As Single
        Get
            Return Description_ContentWidth
        End Get
    End Property
    Private _customTextStyleTitle As TextStyle = Nothing
    Private _customTextStyleSubsection As TextStyle = Nothing

    '---- SCROLLBAR SETTINGS
    Public Property Scrollbar_Enable As Boolean = True
    ''' <summary>
    ''' Time in Milliseconds. Default: (1800, 0.045)
    ''' </summary>
    Public Property Scrollbar_AutoHide As (Time As Integer, FadeSpeed As Single) = (1800, 0.045)
    ''' <summary>
    ''' Default: (0.0045, 0.0015)
    ''' </summary>
    Public Property Scrollbar_Width As (Thumb As Single, Background As Single) = (0.0045, 0.0015)
    ''' <summary>
    ''' Default: (200, 200, 200, 200), (80, 80, 80, 200))
    ''' </summary>
    Public Property Scrollbar_Colors As (Thumb As ColorRGBA, Background As ColorRGBA) = (New ColorRGBA(200, 200, 200, 200), New ColorRGBA(80, 80, 80, 200))
    ''' <summary>
    ''' Default: (0.0067, 0.018)
    ''' </summary>
    Public Property Scrollbar_Offsets As (X As Single, Y As Single) = (0.0067, 0.018)
    ''' <summary>
    ''' Scrollbar smooth movement speed.
    ''' Values between 0.0 - 1.0 | Default: 0.4
    ''' </summary>
    Public Property Scrollbar_SmoothSpeed As Single = 0.4
    ''' <summary>
    ''' Default: 0.01
    ''' </summary>
    Public Property Scrollbar_VerticalPadding As Single = 0.01
    '-----
    Public Property Title_TextOffsets As (x As Single, Y As Single) = (0.01, 0.015)
    ''' <summary>
    ''' Default: same as WindowAlpha
    ''' </summary>
    Public Property Title_BackgroundAlpha As Byte? = WindowAlpha
    ' Public Property Title_Brighten As Single = 1.35
    Public Property Title_Height As Single = 0.0605
    ''' <summary>
    ''' Use NativeDraws.TextColors. Default: MP_Red
    ''' </summary>
    Public Property ValueBoolFalse_Color As String = TextColors.MP_Red
    ''' <summary>
    ''' Use NativeDraws.TextColors. Default: MP_LightGreen
    ''' </summary>
    Public Property ValueBoolTrue_Color As String = TextColors.MP_LightGreen
    ''' <summary>
    ''' Text shown for a False bool value. Default: "OFF"
    ''' </summary>
    ''' <returns></returns>
    Public Property ValueBoolFalse_Text As String = "OFF"
    ''' <summary>
    ''' Text shown for a True bool value. Default: "ON"
    ''' </summary>
    ''' <returns></returns>
    Public Property ValueBoolTrue_Text As String = "ON"
    ''' <summary>
    ''' Default: (100, 100, 100, 255)
    ''' </summary>
    Public Property ValueLocked_Color As ColorRGBA = New ColorRGBA(100, 100, 100, 255)
    ''' <summary>
    ''' Default: "None"
    ''' </summary>
    Public Property ValueKeyNone_Text As String = "None"
    ''' <summary>
    ''' Use NativeDraws.TextColors. Default: LightGray
    ''' </summary>
    Public Property ValueKeyNone_Color As String = TextColors.LightGray
    '---------------------
    '----- Separator -----
    ''' <summary>Separator layout.
    ''' Length = 1.0: full available width inside the selected-rect width. Default: (Length:=1.0F, Thickness:=0.0035F)
    ''' </summary>
    Public Property Separator_Layout As (Length As Single, Thickness As Single) = (Length:=1.0F, Thickness:=0.0035F)
    Private _autoScrollbarColor As Boolean = True
    '------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Initializes a new menu appearance configuration.
    ''' All parameters are optional and fallback to default values.
    ''' </summary>
    Public Sub New(Optional SelectedColor As ColorRGBA = Nothing, Optional DescriptionColor As ColorRGBA = Nothing, Optional WindowAlpha As Byte = 180,
                   Optional MenuPosition As (X As Single, Y As Single)? = Nothing, Optional MenuSize As (X As Single, Y As Single)? = Nothing)

        If SelectedColor <> Nothing Then
            Me.Item_SelectedColor = SelectedColor
        End If

        If DescriptionColor <> Nothing Then
            Me.Description_BoxColor = DescriptionColor
        End If

        Me.WindowAlpha = WindowAlpha

        If MenuPosition.HasValue Then
            Me.MenuPosition = MenuPosition.Value
        End If

        If MenuSize.HasValue Then
            Me.MenuSize = MenuSize.Value
        End If
    End Sub
    ''' <summary>
    ''' Applies custom fonts to the different menu text styles.
    ''' Only the Font property is changed, you may need to adjust size manually.
    ''' </summary>
    Public Sub ApplyFonts(Optional Title As TextStyle.Fonts = TextStyle.Fonts.Narrow, Optional Subsection As TextStyle.Fonts = TextStyle.Fonts.Narrow,
                          Optional Item As TextStyle.Fonts = TextStyle.Fonts.Standard, Optional ItemValue As TextStyle.Fonts = TextStyle.Fonts.Standard,
                          Optional Description As TextStyle.Fonts = TextStyle.Fonts.Standard)

        Me.TextStyle_Title = _cloneWithFont(Me.TextStyle_Title, Title)
        Me.TextStyle_Subsection = _cloneWithFont(Me.TextStyle_Subsection, Subsection)
        Me.TextStyle_Description = _cloneWithFont(Me.TextStyle_Description, Description)
        Me.TextStyle_Item = _cloneWithFont(Me.TextStyle_Item, Item)
        Me.TextStyle_ItemValue = _cloneWithFont(Me.TextStyle_ItemValue, ItemValue)
    End Sub
    Private Shared Function _cloneWithFont(style As TextStyle, font As TextStyle.Fonts) As TextStyle
        If style Is Nothing Then Return Nothing
        Return style.WithFont(font)
    End Function
    ''' <summary>
    ''' Copies all appearance settings from another menu.
    ''' </summary>
    Public Sub CopyFrom(Menu As NativeMenu)
        If Menu Is Nothing OrElse Menu.Appearance Is Nothing Then Return

        Dim src = Menu.Appearance
        'Me.Title_Brighten = src.Title_Brighten
        Me.Subsection_Brighten = src.Subsection_Brighten
        Me.WindowAlpha = src.WindowAlpha
        Me.MenuSize = src.MenuSize
        Me.MenuPosition = src.MenuPosition
        Me.Title_Height = src.Title_Height
        Me.Title_TextOffsets = src.Title_TextOffsets
        Me.Title_BackgroundAlpha = src.Title_BackgroundAlpha
        Me.MaxVisibleItems = src.MaxVisibleItems

        Me.Description_BoxColor = src.Description_BoxColor
        Me.Description_PositionY = src.Description_PositionY
        Me.Description_BoxHeightAdjust = src.Description_BoxHeightAdjust
        Me.Description_TextPadding = src.Description_TextPadding
        Me.Description_BoxColor = src.Description_BoxColor
        Me.Description_LineHeightMultiplier = src.Description_LineHeightMultiplier
        Me.Description_PositionY = src.Description_PositionY
        Me.Description_BoxHeightAdjust = src.Description_BoxHeightAdjust
        Me.Description_TextPadding = src.Description_TextPadding

        Me.Items_StartOffsetY = src.Items_StartOffsetY
        Me.Item_SelectedOffsets = src.Item_SelectedOffsets
        Me.Item_SelectedPadding = src.Item_SelectedPadding
        Me.Item_TextOffsetX = src.Item_TextOffsetX
        Me.Item_SelectedColor = src.Item_SelectedColor
        Me.Item_ValueOffsetX = src.Item_ValueOffsetX
        Me.Item_ValueChangeHighlightTime = src.Item_ValueChangeHighlightTime
        Me.Item_Height = src.Item_Height
        Me.Item_DisabledArrowColor = src.Item_DisabledArrowColor
        Me.Item_HighlightArrowColor = src.Item_HighlightArrowColor

        Me.TextStyle_Title = If(src.TextStyle_Title Is Nothing, Nothing, src.TextStyle_Title.Clone())
        Me.TextStyle_Subsection = If(src.TextStyle_Subsection Is Nothing, Nothing, src.TextStyle_Subsection.Clone())
        Me.TextStyle_Item = If(src.TextStyle_Item Is Nothing, Nothing, src.TextStyle_Item.Clone())
        Me.TextStyle_ItemValue = If(src.TextStyle_ItemValue Is Nothing, Nothing, src.TextStyle_ItemValue.Clone())
        Me.TextStyle_Description = If(src.TextStyle_Description Is Nothing, Nothing, src.TextStyle_Description.Clone())

        Me.Scrollbar_Enable = src.Scrollbar_Enable
        Me.Scrollbar_AutoHide = src.Scrollbar_AutoHide
        Me.Scrollbar_Width = src.Scrollbar_Width
        Me.Scrollbar_Colors = src.Scrollbar_Colors
        Me.Scrollbar_Offsets = src.Scrollbar_Offsets
        Me.Scrollbar_VerticalPadding = src.Scrollbar_VerticalPadding
        Me.Scrollbar_SmoothSpeed = src.Scrollbar_SmoothSpeed
        Me._autoScrollbarColor = src._autoScrollbarColor

        Me.ValueBoolFalse_Color = src.ValueBoolFalse_Color
        Me.ValueBoolTrue_Color = src.ValueBoolTrue_Color
        Me.ValueBoolFalse_Text = src.ValueBoolFalse_Text
        Me.ValueBoolTrue_Text = src.ValueBoolTrue_Text
        Me.ValueLocked_Color = src.ValueLocked_Color
        Me.ValueKeyNone_Color = src.ValueKeyNone_Color
        Me.ValueKeyNone_Text = src.ValueKeyNone_Text
        Me.Separator_Layout = src.Separator_Layout
    End Sub
    ''' <summary>
    ''' Sets all main color-related properties of the menu.
    ''' All parameters are optional and applied only if provided.
    ''' </summary>
    Public Sub SetColors(Optional ItemSelected As ColorRGBA? = Nothing, Optional DescriptionBox As ColorRGBA? = Nothing,
                         Optional ScrollbarThumb As ColorRGBA? = Nothing, Optional ScrollbarBackground As ColorRGBA? = Nothing)

        If ItemSelected.HasValue Then
            Me.Item_SelectedColor = ItemSelected.Value
            If _customTextStyleTitle IsNot Nothing Then _customTextStyleTitle = _customTextStyleTitle.WithColor(ItemSelected.Value)
            If _customTextStyleSubsection IsNot Nothing Then _customTextStyleSubsection = _customTextStyleSubsection.WithColor(ColorRGBA.Brighten(ItemSelected.Value, Subsection_Brighten))
            UpdateScrollbarFromSelectedColor()
        End If

        If ScrollbarThumb.HasValue OrElse ScrollbarBackground.HasValue Then _autoScrollbarColor = False
        If DescriptionBox.HasValue Then Me.Description_BoxColor = DescriptionBox.Value

        Dim thumb As ColorRGBA = Me.Scrollbar_Colors.Thumb
        Dim back As ColorRGBA = Me.Scrollbar_Colors.Background

        If ScrollbarThumb.HasValue Then thumb = ScrollbarThumb.Value
        If ScrollbarBackground.HasValue Then back = ScrollbarBackground.Value

        Me.Scrollbar_Colors = (thumb, back)
    End Sub
    ''' <summary>
    ''' Configures the description box, text position, padding and native line spacing.
    ''' </summary>
    Public Sub SetDescription(Optional BoxColor As ColorRGBA? = Nothing, Optional PositionY As Single? = Nothing, Optional BoxHeightAdjust As Single? = Nothing,
                              Optional TextPadding As (X As Single, Y As Single)? = Nothing, Optional LineHeightMultiplier As Single? = Nothing, Optional TextStyle As TextStyle = Nothing)

        If BoxColor.HasValue Then Me.Description_BoxColor = BoxColor.Value
        If PositionY.HasValue Then Me.Description_PositionY = PositionY.Value
        If BoxHeightAdjust.HasValue Then Me.Description_BoxHeightAdjust = BoxHeightAdjust.Value
        If TextPadding.HasValue Then Me.Description_TextPadding = TextPadding.Value
        If LineHeightMultiplier.HasValue Then Me.Description_LineHeightMultiplier = Tools.ClampFloat(LineHeightMultiplier.Value, 0.7F, 10.0F)
        If TextStyle IsNot Nothing Then Me.TextStyle_Description = TextStyle
    End Sub
    ''' <summary>
    ''' Configures item and subsection appearance.
    ''' </summary>
    Public Sub SetItems(Optional StartOffsetY As Single? = Nothing, Optional TextOffsetX As Single? = Nothing, Optional SelectedOffsets As (X As Single, Y As Single)? = Nothing,
                        Optional SelectedPadding As (X As Single, Y As Single)? = Nothing, Optional SubsectionBrighten As Single? = Nothing,
                        Optional ItemTextStyle As TextStyle = Nothing, Optional SubsectionTextStyle As TextStyle = Nothing)

        If StartOffsetY.HasValue Then Me.Items_StartOffsetY = StartOffsetY.Value
        If TextOffsetX.HasValue Then Me.Item_TextOffsetX = TextOffsetX.Value
        If SelectedOffsets.HasValue Then Me.Item_SelectedOffsets = SelectedOffsets.Value
        If SelectedPadding.HasValue Then Me.Item_SelectedPadding = SelectedPadding.Value
        If SubsectionBrighten.HasValue Then Me.Subsection_Brighten = SubsectionBrighten.Value

        If ItemTextStyle IsNot Nothing Then Me.TextStyle_Item = ItemTextStyle
        If SubsectionTextStyle IsNot Nothing Then Me.TextStyle_Subsection = SubsectionTextStyle
    End Sub
    ''' <summary>
    ''' Configures value display and interaction visuals.
    ''' </summary>
    Public Sub SetItemValues(Optional ValueOffsetX As Single? = Nothing, Optional HighlightTimeMs As Integer? = Nothing, Optional DisabledArrowColor As String = Nothing,
                             Optional HighlightArrowColor As String = Nothing, Optional ValueTextStyle As TextStyle = Nothing)

        If ValueOffsetX.HasValue Then Me.Item_ValueOffsetX = ValueOffsetX.Value
        If HighlightTimeMs.HasValue Then Me.Item_ValueChangeHighlightTime = HighlightTimeMs.Value
        If DisabledArrowColor IsNot Nothing Then Me.Item_DisabledArrowColor = DisabledArrowColor
        If HighlightArrowColor IsNot Nothing Then Me.Item_HighlightArrowColor = HighlightArrowColor

        If ValueTextStyle IsNot Nothing Then Me.TextStyle_ItemValue = ValueTextStyle
    End Sub
    ''' <summary>
    ''' Sets global layout and sizing parameters of the menu.
    ''' </summary>
    Public Sub SetLayout(Optional Position As Vector2? = Nothing, Optional MenuSize As Vector2? = Nothing, Optional TitleHeight As Single? = Nothing,
                         Optional ItemHeight As Single? = Nothing, Optional MaxVisibleItems As Integer? = Nothing)

        If Position.HasValue Then Me.MenuPosition = (Position.Value.X, Position.Value.Y)
        If MenuSize.HasValue Then Me.MenuSize = (MenuSize.Value.X, MenuSize.Value.Y)
        If TitleHeight.HasValue Then Me.Title_Height = TitleHeight.Value
        If ItemHeight.HasValue Then Me.Item_Height = ItemHeight.Value
        If MaxVisibleItems.HasValue Then Me.MaxVisibleItems = MaxVisibleItems.Value

    End Sub
    ''' <summary>
    ''' Configures title appearance and positioning.
    ''' </summary>
    Public Sub SetTitle(Optional TextOffsets As (X As Single, Y As Single)? = Nothing, Optional CustomTextStyle As TextStyle = Nothing) ',Optional Brighten As Single? = Nothing, 

        ' If Brighten.HasValue Then Me.Title_Brighten = Brighten.Value
        If TextOffsets.HasValue Then Me.Title_TextOffsets = TextOffsets.Value
        If CustomTextStyle IsNot Nothing Then Me.TextStyle_Title = CustomTextStyle
    End Sub
    ''' <summary>
    ''' Sets default text styles used by the menu.
    ''' </summary>
    Public Sub SetTextStyles(Optional Title As TextStyle = Nothing, Optional Subsection As TextStyle = Nothing, Optional ItemText As TextStyle = Nothing,
                             Optional ItemValue As TextStyle = Nothing, Optional DescriptionText As TextStyle = Nothing)

        If Title IsNot Nothing Then Me.TextStyle_Title = Title
        If Subsection IsNot Nothing Then Me.TextStyle_Subsection = Subsection
        If ItemText IsNot Nothing Then Me.TextStyle_Item = ItemText
        If ItemValue IsNot Nothing Then Me.TextStyle_ItemValue = ItemValue
        If DescriptionText IsNot Nothing Then Me.TextStyle_Description = DescriptionText
    End Sub
    ''' <summary>
    ''' Configures scrollbar visibility, timing, size and appearance.
    ''' </summary>
    Public Sub SetScrollbar(Optional Enabled As Boolean? = Nothing, Optional AutoHideTimeMs As Integer? = Nothing, Optional FadeSpeed As Single? = Nothing,
                            Optional Widths As (Thumb As Single, Background As Single)? = Nothing, Optional Colors As (Thumb As ColorRGBA, Background As ColorRGBA)? = Nothing,
                            Optional Offsets As (X As Single, Y As Single)? = Nothing, Optional VerticalPadding As Single? = Nothing, Optional ScrollSmooth As Single? = Nothing)

        If Enabled.HasValue Then Me.Scrollbar_Enable = Enabled.Value

        If AutoHideTimeMs.HasValue OrElse FadeSpeed.HasValue Then
            Me.Scrollbar_AutoHide = (If(AutoHideTimeMs.HasValue, AutoHideTimeMs.Value, Me.Scrollbar_AutoHide.Time), If(FadeSpeed.HasValue, FadeSpeed.Value, Me.Scrollbar_AutoHide.FadeSpeed))
        End If

        If Widths.HasValue Then Me.Scrollbar_Width = Widths.Value
        If Colors.HasValue Then Me.Scrollbar_Colors = Colors.Value
        If Offsets.HasValue Then Me.Scrollbar_Offsets = Offsets.Value
        If VerticalPadding.HasValue Then Me.Scrollbar_VerticalPadding = VerticalPadding.Value
        If ScrollSmooth.HasValue Then
            Me.Scrollbar_SmoothSpeed = Tools.ClampFloat(ScrollSmooth.Value, 0.0, 1.0)
        End If
    End Sub
    Public Sub SetWindow(Optional Alpha As Byte? = Nothing, Optional TitleBackgroundAlpha As Byte? = Nothing)
        If Alpha.HasValue Then Me.WindowAlpha = Alpha.Value
        If TitleBackgroundAlpha.HasValue Then Me.Title_BackgroundAlpha = TitleBackgroundAlpha.Value
    End Sub
    Public Overrides Function Equals(obj As Object) As Boolean
        If ReferenceEquals(Me, obj) Then Return True
        If obj Is Nothing OrElse obj.GetType() IsNot GetType(MenuAppearance) Then Return False
        Dim o = DirectCast(obj, MenuAppearance)
        Return MenuSize.Equals(o.MenuSize) AndAlso MenuPosition.Equals(o.MenuPosition) AndAlso
               MaxVisibleItems = o.MaxVisibleItems AndAlso Item_Height = o.Item_Height AndAlso
               WindowAlpha = o.WindowAlpha
    End Function
    Private Sub UpdateScrollbarFromSelectedColor()
        If Not _autoScrollbarColor Then Return
        Dim thumbAlpha As Byte = Scrollbar_Colors.Thumb.Alpha
        Dim backAlpha As Byte = Scrollbar_Colors.Background.Alpha
        Dim base As ColorRGBA = Item_SelectedColor
        Scrollbar_Colors = (New ColorRGBA(base.Red, base.Green, base.Blue, thumbAlpha), New ColorRGBA(base.Red, base.Green, base.Blue, backAlpha))
    End Sub
    Friend Enum MenuTextContext
        Title
        Subsection
        Item
        ItemValue
        Description
    End Enum
    '===============================================================================================================================================
End Class
Public Class MenuKeys
    Public Property ToggleMenu As Keys = Keys.F8
    Public Property NavUp As Keys = Keys.Up
    Public Property NavDown As Keys = Keys.Down
    Public Property DecreaseValue As Keys = Keys.Left
    Public Property IncreaseValue As Keys = Keys.Right
    Public Property Enter As Keys = Keys.Enter
    Public Property Back As Keys = Keys.Back

End Class
'=============================================================================================================================================================