Imports System.Windows.Forms
Imports GTA
Imports Microsoft.VisualBasic.Logging
Imports NativeExtended
Imports NativeExtended.NativeDraws

'--------------- API - Interface ---------------
Partial Public Class NativeMenu

    ''' <summary>
    ''' Clears both native and custom help messages whenever this menu is shown.
    ''' The setting is inherited by existing and newly created submenus.
    ''' </summary>
    Public Property ClearHelpMessages As Boolean
        Get
            Return _clearHelpMessages
        End Get
        Set(value As Boolean)

            _clearHelpMessages = value

            For Each submenu As NativeMenu In SubMenusList
                If submenu IsNot Nothing Then
                    submenu.ClearHelpMessages = value
                End If
            Next

        End Set
    End Property
    ''' <summary>
    ''' Creates new menu. MainColor will affect title, subsection, separator and selected item colors
    ''' </summary>
    Public Sub New(MenuTitle As String, Optional MainColor As ColorRGBA = Nothing, Optional SettingsFile As Tools.SettingsFile = Nothing, Optional LogFile As Tools.LogFile = Nothing)

        Title = MenuTitle
        Appearance = New MenuAppearance
        RootMenu = Me
        If SettingsFile IsNot Nothing Then Me.SettingsFile = SettingsFile
        If LogFile IsNot Nothing Then Me.LogFile = LogFile

        OptionsBinding = New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
        AddHandler Me.ItemValueChanged,
        Sub(changedItem As MenuItem)
            If changedItem Is Nothing OrElse changedItem.ParentMenu Is Nothing Then Return
            changedItem.ParentMenu.NotifyValueChanged(changedItem, changedItem.GetTypedValue())
        End Sub
        '----
        If MainColor <> Nothing Then
            Appearance.SetColors(ItemSelected:=MainColor, DescriptionBox:=New ColorRGBA(MainColor.Red, MainColor.Green, MainColor.Blue, Appearance.Description_BoxColor.Alpha))
        End If
        '----
    End Sub
    ' Public Property EnableAppearanceDebug As Boolean = False
    '----
    ''' <returns>True if is editing any key or custom string</returns>
    Public ReadOnly Property IsInputLocked As Boolean
        Get
            Return _isEditingText OrElse IsEditingKey
        End Get
    End Property
    ''' <returns>If True will disable all menu keys</returns>
    Public Property DisableAllKeys As Boolean = False
    '--------------------------------------
    ''' <returns>True if menu or submenu is currently active</returns>
    Public ReadOnly Property IsActive As Boolean
        Get
            Return _isActive
        End Get
    End Property
    ''' <summary>
    ''' Returns True if the root menu or any of its submenus is currently active.
    ''' </summary>
    Public ReadOnly Property IsAnyMenuActive As Boolean
        Get
            Return GetActiveMenuRecursive(Me.RootMenu)
        End Get
    End Property
    ''' <summary>
    ''' Gets/sets the selected item by name.
    ''' If the name is not found, selection won't changed.
    ''' </summary>
    Public Property SelectedItem As String
        Get
            Dim it = GetSelectedItemInternal()
            Return If(it IsNot Nothing, it.Name, "")
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then Return

            Dim it = MenuItem.GetItemByName(value, Me)
            If it Is Nothing OrElse it.IsSubsection Then Return

            _SelectIndex = Items.IndexOf(it)
            UpdateVisible()
        End Set
    End Property
    '========================
    Public Property Title As String
    Public Property Appearance As New MenuAppearance
    Public Property MenuKeys As New MenuKeys
    Public Property IsRedefineKeyMenu As Boolean = False
    ''' <summary>
    ''' Adds a subsection item (not selectable) or a button linked to a SubMenu or action.
    ''' </summary>
    Public Sub AddItem(Name As String, Optional Description As String = "", Optional SubMenuLinked As NativeMenu = Nothing, Optional ActionToTrigger As Action = Nothing,
                       Optional SelectedColor As ColorRGBA = Nothing, Optional DescriptionColor As ColorRGBA = Nothing)

        If Items.Any(Function(i) i.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)) Then Exit Sub

        Dim subMenu As NativeMenu = Nothing
        If SubMenuLinked IsNot Nothing Then
            subMenu = Me.SubMenu(SubMenuLinked.Title)
        End If

        Dim willBeSubsection As Boolean = SubMenuLinked Is Nothing AndAlso ActionToTrigger Is Nothing
        If willBeSubsection AndAlso Items.Count > 0 Then
            Dim lastItem As MenuItem = Items.Last()
            If lastItem IsNot Nothing AndAlso lastItem.IsSubsection Then
                Tools.WriteLog("   INFO: Cannot add consecutive subsection item in '" & Me.Title & "' menu.")
                Exit Sub
            End If
        End If

        Dim item As New MenuItem With {
        .Name = Name,
        .Description = Description,
        .SubMenuLinked = subMenu,
        .Action = ActionToTrigger,
        .HasValue = False,
        .Value = "",
        .Color_SelectedRect = If(SelectedColor = Nothing, Appearance.Item_SelectedColor, SelectedColor),
        .Color_DescriptionRect = If(DescriptionColor = Nothing, Appearance.Description_BoxColor, DescriptionColor),
        .ParentMenu = Me}

        item.IsSubsection = willBeSubsection

        Items.Add(item)
        _layoutDirty = True
    End Sub
    ''' <summary>
    ''' Loads all parameter/value pairs from a file section and automatically
    ''' adds them as value items to this menu or submenu.
    ''' Suggested only for parameters linked to AddItem_InGame items.
    ''' </summary>
    ''' <param name="FilePath">Full path to the config file.</param>
    ''' <param name="Section">Section name without brackets.</param>
    Public Sub AddItems_FromFileSection(FilePath As String, Section As String, Optional SelectedColor As ColorRGBA = Nothing,
                                        Optional DescriptionColor As ColorRGBA = Nothing, Optional ClampValue As (Min As Single, Max As Single)? = Nothing)
        If Not IO.File.Exists(FilePath) Then
            ' DisplayText(TextColors.MP_Red & "Config file not found!", 1500)
            Tools.WriteLog("Error in NativeMenu.AddItems_FromFileSection of " & Title & ": file not found.")
            Tools.WriteLog("  Path: " & FilePath)
            Return
        End If

        If Not IO.File.Exists(FilePath) Then
            Tools.WriteLog("Error in NativeMenu.AddItems_FromFileSection of " & Title & ": file not found.")
            Tools.WriteLog("  Path: " & FilePath)
            Return
        End If

        Dim sf As New Tools.SettingsFile(FilePath)
        Dim entries As Dictionary(Of String, String) = sf.GetValues_FromSection(Section)

        If entries.Count = 0 Then
            LogMenuError("   INFO: " & Section & " is empty or missing.")
            Return
        End If

        For Each kvp In entries

            Dim keyName As String = kvp.Key
            Dim rawValue As String = kvp.Value

            If Items.Any(Function(i) i.Name.Equals(keyName, StringComparison.OrdinalIgnoreCase)) Then Continue For

            Dim parsedValue As Object = ParseValueWithClamp(rawValue, ClampValue)

            AddItem_WithValue(Name:=keyName, Value:=parsedValue, SelectedColor:=SelectedColor, DescriptionColor:=DescriptionColor, Clamp:=ClampValue)
            _layoutDirty = True
        Next
    End Sub
    ''' <summary>
    ''' Adds an in-game item adder with fixed ValueType.
    ''' The item name is entered manually by the player.
    ''' </summary>
    Public Sub AddItem_InGame(Name As String, ValueType As ValueType, Optional Description As String = "", Optional SelectedColor As ColorRGBA = Nothing,
                          Optional DescriptionColor As ColorRGBA = Nothing, Optional Clamp As (Min As Single, Max As Single)? = Nothing)

        If Items.Any(Function(i) i.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)) Then Exit Sub

        Dim creator As New MenuItem With {
        .Name = Name,
        .Description = Description,
        .HasValue = True,
        .ValueType = ValueType.Text,
        .Value = "",
        .IsLocked = False,
        .Color_SelectedRect = If(SelectedColor = Nothing, Appearance.Item_SelectedColor, SelectedColor),
        .Color_DescriptionRect = If(DescriptionColor = Nothing, Appearance.Description_BoxColor, DescriptionColor),
        .IsInGameCreator = True,
        .InGameCreatorValueType = ValueType,
        .ParentMenu = Me}

        If Clamp.HasValue Then
            creator.MinValue = Clamp.Value.Min
            creator.MaxValue = Clamp.Value.Max

            creator.CreatorMinValue = Clamp.Value.Min
            creator.CreatorMaxValue = Clamp.Value.Max
        End If

        Items.Add(creator)
        _layoutDirty = True
    End Sub
    ''' <summary>
    ''' Adds a selectable string list value.
    ''' </summary>
    Public Sub AddItem_WithTextList(Name As String, Values As IEnumerable(Of String), Optional Description As String = "", Optional DefaultValue As String = "",
                                    Optional SelectedColor As ColorRGBA = Nothing, Optional DescriptionColor As ColorRGBA = Nothing)

        If Values Is Nothing Then Exit Sub

        Dim list As List(Of String) = Values.ToList()
        If list.Count = 0 Then Exit Sub

        If Items.Any(Function(i) i.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)) Then Exit Sub

        Dim index As Integer
        If Not String.IsNullOrEmpty(DefaultValue) AndAlso list.Contains(DefaultValue) Then
            index = list.IndexOf(DefaultValue)
        Else
            index = 0
        End If

        Dim item As New MenuItem With {
        .Name = Name,
        .Description = Description,
        .HasValue = True,
        .IsLocked = False,
        .ValueType = ValueType.TextList,
        .Value_TextList = list,
        .TextListIndex = index,
        .Value = list(index),
        .Color_SelectedRect = If(SelectedColor = Nothing, Appearance.Item_SelectedColor, SelectedColor),
        .Color_DescriptionRect = If(DescriptionColor = Nothing, Appearance.Description_BoxColor, DescriptionColor),
        .ParentMenu = Me}

        ' Bind value like any other item
        OptionsBinding(Name) = item.Value

        AddHandler Me.ItemValueChanged,
        Sub(changedItem)
            If changedItem Is item Then
                OptionsBinding(Name) = changedItem.Value
            End If
        End Sub

        Items.Add(item)
        _layoutDirty = True
    End Sub
    ''' <summary>
    ''' Adds an item with a value. Automatically creates a KeyBindItem if the value is a Keys type.
    ''' ResizeValuePrecision allows you to multiply integer by 10 * value or approximate floats. 
    ''' If -1 will be applied 0 for integers and 2 for foats.
    ''' Clamp allows you to resize a int/float between a minimum and maximum value.
    ''' </summary>
    ''' <param name="ResizeValuePrecision">Integers: 0 = 1 / 1 = 10 / 2 = 100 (Max 7) | Floats 0 = 1.0 / 1 = 0.1 / 2 = 0.01 (Max 5)</param>
    Public Sub AddItem_WithValue(Name As String, Value As Object, Optional Description As String = "", Optional SelectedColor As ColorRGBA = Nothing,
                                 Optional DescriptionColor As ColorRGBA = Nothing, Optional ResizeValuePrecision As Integer = -1, Optional Clamp As (Min As Single, Max As Single)? = Nothing)
        If Me.IsRedefineKeyMenu Then
            If Items.Any(Function(i) i.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)) Then Exit Sub
        End If
        If String.IsNullOrWhiteSpace(Name) Then
            Throw New ArgumentException("Item name cannot be empty.", NameOf(Name))
        End If

        If Items.Any(Function(i) String.Equals(i.Name, Name, StringComparison.OrdinalIgnoreCase)) Then
            Return
        End If

        ' --- KeyBindItem SOLO per Keys
        If TypeOf Value Is Keys Then
            If Items.Any(Function(i) i.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)) Then Exit Sub

            Dim k As Keys = DirectCast(Value, Keys)

            Dim keyItem As New KeyBindItem(Name, Description, k) With {
              .HasValue = True,
              .Color_SelectedRect = If(SelectedColor = Nothing, Appearance.Item_SelectedColor, SelectedColor),
              .Color_DescriptionRect = If(DescriptionColor = Nothing, Appearance.Description_BoxColor, DescriptionColor),
              .ValueType = ValueType.KeyBind,
              .AssignedKey = k,
              .ParentMenu = Me}

            OptionsBinding(Name) = CType(keyItem.AssignedKey, Object)
            Items.Add(keyItem)
            _layoutDirty = True
            Return
        End If
        '-------------
        '--- old
        'If Me.IsRedefineKeyMenu Then
        '    If TypeOf Value IsNot Keys Then
        '        '  Throw New InvalidOperationException($"Cannot add a non-key value '{Name}' in Redefine Key menu '{Me.Title}'.")
        '    End If

        '    Dim keyItem As New KeyBindItem(Name, Description, DirectCast(Value, Keys)) With {
        '    .HasValue = True, .Color_SelectedRect = If(SelectedColor = Nothing, Appearance.Item_SelectedColor, SelectedColor),
        '    .Color_DescriptionRect = If(DescriptionColor = Nothing, Appearance.Description_BoxColor, DescriptionColor),
        '    .ValueType = ValueType.KeyBind, .AssignedKey = Value, .ParentMenu = Me}

        '    ' Aggiorna automaticamente il dictionary dei bindings
        '    OptionsBinding(Name) = CType(keyItem.AssignedKey, Object)

        '    ' AddHandler keyItem.KeyChanged, AddressOf OnKeyItemChanged

        '    '  AddHandler keyItem.KeyChanged, Sub(sender As KeyBindItem, k As Keys)
        '    'OptionsBinding(sender.Name) = k
        '    '   End Sub
        '    Items.Add(keyItem)


        '    Return
        'End If

        ' --- Normal value item ---
        Dim item As New MenuItem With {
        .Name = Name,
        .Description = Description,
        .HasValue = True,
        .Color_SelectedRect = If(SelectedColor = Nothing, Appearance.Item_SelectedColor, SelectedColor),
        .Color_DescriptionRect = If(DescriptionColor = Nothing, Appearance.Description_BoxColor, DescriptionColor),
        .ParentMenu = Me}

        Select Case True
            Case TypeOf Value Is Boolean
                item.ValueType = ValueType.Bool
                item.Value_Boolean = CBool(Value)
                ResizeValuePrecision = 0

                OptionsBinding(Name) = item.GetTypedValue
            Case TypeOf Value Is Integer
                item.ValueType = ValueType.Int

                Dim v As Integer = CInt(Value)
                If Clamp.HasValue Then
                    v = Tools.ClampInteger(v, CInt(Clamp.Value.Min), CInt(Clamp.Value.Max))
                    item.MinValue = CInt(Clamp.Value.Min)
                    item.MaxValue = CInt(Clamp.Value.Max)
                End If

                item.Value_Integer = v
                item._valueResizePrecision = If(ResizeValuePrecision = -1, 0, Tools.ClampInteger(ResizeValuePrecision, 0, 7))

                OptionsBinding(Name) = item.GetTypedValue
            Case TypeOf Value Is Single
                item.ValueType = ValueType.Float

                Dim v As Single = CSng(Value)

                If Clamp.HasValue Then
                    v = Tools.ClampFloat(v, Clamp.Value.Min, Clamp.Value.Max)
                    item.MinValue = Clamp.Value.Min
                    item.MaxValue = Clamp.Value.Max
                End If

                item.Value_Float = v

                item._valueResizePrecision = If(ResizeValuePrecision = -1, 2, Tools.ClampInteger(ResizeValuePrecision, 0, 5))

                OptionsBinding(Name) = item.GetTypedValue
            Case TypeOf Value Is String
                item.ValueType = ValueType.Text
                item.Value = CStr(Value)
                OptionsBinding(Name) = item.GetTypedValue
            Case Else
                ' Throw New ArgumentException("Unsupported value type. Only Boolean, Integer, Single, String or Keys are allowed.")
        End Select

        AddHandler Me.ItemValueChanged,
        Sub(changedItem)
            If changedItem.Name = Name Then
                Select Case changedItem.ValueType
                    Case ValueType.Bool : OptionsBinding(Name) = changedItem.GetTypedValue
                    Case ValueType.Int : OptionsBinding(Name) = changedItem.GetTypedValue
                    Case ValueType.Float : OptionsBinding(Name) = changedItem.GetTypedValue
                    Case ValueType.Text : OptionsBinding(Name) = changedItem.GetTypedValue
                End Select
            End If
        End Sub

        Items.Add(item)
        _layoutDirty = True
    End Sub
    ''' <summary>
    ''' Adds a visual separator line to the menu.
    ''' Length is a 0..1 multiplier of the available width. Thickness is screen units.
    ''' The separator occupies the same vertical slot as a normal item.
    ''' </summary>
    Public Sub AddSeparator(Optional Color As ColorRGBA = Nothing,
                        Optional Style As (Length As Single, Thickness As Single)? = Nothing)

        If Items.Count > 0 AndAlso Items.Last().IsSeparator Then
            Tools.WriteLog("   INFO: Cannot add consecutive separators in " & Me.Title)
            Return
        End If

        Dim sepColor As ColorRGBA = If(Color = Nothing, Appearance.TextStyle_Title.Color, Color)

        ' Clamp style if provided
        Dim ov As (Length As Single, Thickness As Single)? = Nothing
        If Style.HasValue Then
            Dim len As Single = Style.Value.Length
            Dim thick As Single = Style.Value.Thickness

            If len < 0.0F Then len = 0.0F
            If len > 1.0F Then len = 1.0F
            If thick < 0.0001F Then thick = 0.0001F

            ov = (len, thick)
        End If

        Dim item As New MenuItem With {
        .Name = String.Empty,
        .IsSeparator = True,
        .IsSubsection = True,
        .HasValue = False,
        .IsLocked = True,
        .SeparatorColor = sepColor,
        .SeparatorOverride = ov,
        .ParentMenu = Me
        }

        Items.Add(item)
        _layoutDirty = True
    End Sub
    ''' <summary>
    ''' Binds a callback method that is invoked when item is removed inside menu or submenu deleting also from SettingsFile.
    ''' </summary>
    Public Sub BindItemRemoved(handler As Action(Of NativeMenu, String, String))
        If handler Is Nothing Then Return

        AddHandler Me.RootMenu.ItemRemoved, Sub(e As MenuItemRemovedEventArgs)
                                                handler(e.Menu, e.ItemName, e.Category)
                                            End Sub
    End Sub
    ''' <summary>
    ''' Binds a callback method that is invoked when item value changes inside specific menu.
    ''' </summary>
    Public Sub BindValueChange(handler As Action(Of NativeMenu, String, Object))

        If handler Is Nothing Then Return

        AddHandler Me.RootMenu.ValueChanged,
        Sub(e As MenuValueChangedEventArgs)
            handler(e.Menu, e.ItemName, e.Value)
        End Sub

    End Sub
    ''' <summary>
    ''' Returns the value of an item by name, searching only inside
    ''' the current menu instance.
    ''' </summary>
    Public Function GetItemValue(Name As String) As Object
        If String.IsNullOrWhiteSpace(Name) Then
            Return Nothing
        End If

        Dim it As MenuItem = MenuItem.GetItemByName(Name, Me)
        If it Is Nothing Then Return Nothing

        Return it.GetTypedValue()
    End Function
    ''' <summary>
    ''' Locks or unlocks a menu item by name.
    ''' Locked items can't be modified.
    ''' </summary>
    Public Sub LockItem(Name As String, Optional Locked As Boolean = True)
        If String.IsNullOrWhiteSpace(Name) Then Return

        Dim it As MenuItem = MenuItem.GetItemByName(Name, Me)
        If it Is Nothing Then Return

        it.IsLocked = Locked
        _layoutDirty = True
        'vecchio
        'Dim it = Items.FirstOrDefault(Function(x) x.Name = Name)
        'If it IsNot Nothing Then
        '    it.IsLocked = Lock()
        'End If
    End Sub
    ''' <summary>
    ''' Removes the first menu item that matches the provided name.
    ''' Safely updates selection index and scroll controls.
    ''' </summary>
    ''' <param name="Name">The display name of the item to remove.</param>
    ''' <returns>True if an item was found and removed, otherwise False.</returns>
    Public Function RemoveItem(Name As String) As Boolean

        If String.IsNullOrWhiteSpace(Name) Then
            Return False
        End If

        Dim index As Integer = Items.FindIndex(
        Function(it) String.Equals(
            it.Name,
            Name,
            StringComparison.OrdinalIgnoreCase))

        If index = -1 Then
            Return False
        End If

        Dim removedItem As MenuItem = Items(index)

        Items.RemoveAt(index)

        If Items.Count = 0 Then

            _SelectIndex = 0
            FirstVisible = 0

        Else

            If index < _SelectIndex Then
                _SelectIndex -= 1
            End If

            _SelectIndex = Math.Max(
            0,
            Math.Min(_SelectIndex, Items.Count - 1))

            If Items(_SelectIndex).IsSubsection Then
                GetSelectedItemInternal(autoFix:=True)
            End If

            FirstVisible = Math.Max(
            0,
            Math.Min(
                FirstVisible,
                Math.Max(0, Items.Count - Appearance.MaxVisibleItems)))

            UpdateVisible()

        End If

        _layoutDirty = True

        RootMenu.RaiseItemRemoved(Me, removedItem.Name, Me.Title)

        Return True

    End Function
    ''' <summary>
    ''' Sets the value of a menu item by name.
    ''' Automatically applies type conversion.
    ''' </summary>
    Public Sub SetItemValue(Name As String, Value As Object)

        If String.IsNullOrWhiteSpace(Name) Then Return

        Dim it As MenuItem = MenuItem.GetItemByName(Name, Me)
        If it Is Nothing OrElse it.IsLocked Then Return

        Try
            Select Case it.ValueType

                Case ValueType.Bool
                    it.Value_Boolean = CBool(Value)

                Case ValueType.Int
                    it.Value_Integer = CInt(Value)

                    If it.MinValue.HasValue AndAlso it.MaxValue.HasValue Then
                        it.Value_Integer =
                        Tools.ClampInteger(it.Value_Integer,
                                           CInt(it.MinValue.Value),
                                           CInt(it.MaxValue.Value))
                    End If

                Case ValueType.Float
                    it.Value_Float = CSng(Value)

                    If it.MinValue.HasValue AndAlso it.MaxValue.HasValue Then
                        it.Value_Float =
                        Tools.ClampFloat(it.Value_Float,
                                         it.MinValue.Value,
                                         it.MaxValue.Value)
                    End If

                Case ValueType.Text
                    it.Value = CStr(Value)

                Case ValueType.TextList
                    Dim strValue As String = CStr(Value)
                    If it.Value_TextList IsNot Nothing AndAlso
                    it.Value_TextList.Any(Function(v) v.Equals(strValue, StringComparison.OrdinalIgnoreCase)) Then

                        ' it.TextListIndex = it.Value_TextList.IndexOf(strValue)
                        it.TextListIndex = it.Value_TextList.FindIndex(Function(v) v.Equals(strValue, StringComparison.OrdinalIgnoreCase))
                        it.Value = strValue
                    End If

                Case Else
                    Return
            End Select
            RaiseEvent ItemValueChanged(it)
            'it.ParentMenu.NotifyValueChanged(it, it.GetTypedValue()) 'Rimosso durante l'ultima modifica (aggiunta wrap text nativo)
            _layoutDirty = True
        Catch ex As Exception
            LogMenuError("Error in SetItemValue for " & Name & "'.", ex)
        End Try

    End Sub
    ''' <summary>
    ''' Retrieves or creates a submenu by name.  
    ''' If "Redefine Keys" is requested, automatically creates the special redefine SubMenu.
    ''' </summary>
    Public Function SubMenu(Name As String) As NativeMenu

        Dim sm As NativeMenu = SubMenusList.FirstOrDefault(Function(s) s.Title.Equals(Name, StringComparison.OrdinalIgnoreCase))

        If sm IsNot Nothing Then Return sm

        sm = New NativeMenu(Name, Nothing)
        sm._parentMenu = Me
        sm.RootMenu = RootMenu
        sm.OptionsBinding = OptionsBinding
        sm.Appearance.CopyFrom(Me)
        sm.ClearHelpMessages = Me.ClearHelpMessages
        If sm.Title = "Redefine Keys" Then sm.IsRedefineKeyMenu = True
        SubMenusList.Add(sm)
        'AddHandler sm.ItemValueChanged, Sub(changedItem As MenuItem)
        '                                    RaiseEvent ItemValueChanged(changedItem)
        '                                End Sub 'Rimosso durante l'ultima modifica (aggiunta wrap text nativo)
        Return sm
    End Function
    ''' <summary>
    ''' Toggles the menu visibility. 
    ''' If 'ForceState' is provided, the menu will be set ON or OFF accordingly. 
    ''' Otherwise, it toggles the current state.
    ''' </summary>
    ''' <param name="ForceState">Optional Boolean to force menu ON (True) or OFF (False)</param>
    Public Sub Toggle(Optional ForceState As Boolean? = Nothing)
        EnsureInitialized()
        ' Se viene passato un valore, usalo come stato
        If ForceState.HasValue Then
            If ForceState.Value Then
                If Not _isActive Then Show(True)
            Else
                If _isActive Then Show(False, fullClose:=(_parentMenu Is Nothing))
            End If
        Else
            If _isActive Then
                Show(False)
                If _isEditingText Then
                    _editingTextItem.Value = _previousText
                    ExitTextEdit()
                End If
            Else
                Show(True)
            End If
        End If
    End Sub
    Public Enum ValueType
        Text
        Bool
        Int
        Float
        KeyBind
        TextList
    End Enum
End Class

