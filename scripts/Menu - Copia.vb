Imports System.Windows.Forms
Imports GTA
Imports NativeExtended
Imports NativeExtended.NativeDraws


Public Class NativeMenu
    Private _isActive As Boolean = False
    Private _activeSubMenu As NativeMenu = Nothing
    Friend _parentMenu As NativeMenu = Nothing
    Friend _applyItem As MenuItem = Nothing
    ' Private Shared _applyAction As Action
    '---------------------------------
    Private _holdStartTime As Integer = 0
    Private _lastRepeatTime As Integer = 0
    Private _repeatCount As Integer = 0
    Private _holdingDirection As Integer = 0
    Private _holdingItem As MenuItem = Nothing
    '---------------------------------
    Private _hasPendingChanges As Boolean = False
    Public Property CurrentActiveMenu As NativeMenu = Me
    '--------------------------------------
    '----------- EVENTS ----------- 
    ''' <summary>
    ''' Fired when any item value changes. Provides the typed value automatically.
    ''' </summary>
    Public Event ValueChanged(Item As MenuItem, NewValue As Object)
    '----
    Private Event ItemValueChanged(Item As MenuItem)
    '--------------------------------------
    Friend Property OptionsBinding As Dictionary(Of String, Object)
    '--------------------------------------
    'string editing
    Private _isEditingText As Boolean = False
    Private _editingTextItem As MenuItem = Nothing
    Private _textBuffer As String = ""
    Private _previousText As String = ""
    ''' <returns>True if is editing any key or custom string</returns>
    Public ReadOnly Property IsInputLocked As Boolean
        Get
            Return _isEditingText OrElse IsEditingKey
        End Get
    End Property

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

    Private Function GetActiveMenuRecursive(menu As NativeMenu) As Boolean
        If menu Is Nothing Then Return False
        If menu._isActive Then Return True

        For Each sm In menu.SubMenusList
            If GetActiveMenuRecursive(sm) Then Return True
        Next

        Return False
    End Function

    Public Shared Function GetItemByName(Name As String, Menu As NativeMenu) As MenuItem
        If Menu Is Nothing OrElse String.IsNullOrEmpty(Name) Then Return Nothing

        ' Cerca nell'elenco principale degli items
        Dim found = Menu.Items.FirstOrDefault(Function(it) it.Name = Name)
        If found IsNot Nothing Then Return found

        ' Cerca ricorsivamente nei sottomenu
        For Each sm In Menu.SubMenusList
            Dim subFound = GetItemByName(Name, sm)
            If subFound IsNot Nothing Then Return subFound
        Next

        ' Non trovato
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets/sets selected item index.
    ''' </summary>
    Public Property SelectedIndex As Integer
        Get
            Return _SelectIndex
        End Get
        Set(value As Integer)
            _SelectIndex = value
            UpdateVisible()
        End Set
    End Property
    ''' <summary>
    ''' Gets/sets the selected item by name.
    ''' If the name is not found, selection is not changed.
    ''' </summary>
    Public Property SelectedItem As String
        Get
            If Items Is Nothing OrElse Items.Count = 0 Then Return ""
            Return Items(_SelectIndex).Name
        End Get
        Set(value As String)
            If String.IsNullOrWhiteSpace(value) Then Return
            Dim idx As Integer = Items.FindIndex(Function(i) i.Name = value)
            If idx >= 0 Then
                _SelectIndex = idx
                UpdateVisible()
            End If
        End Set
    End Property
    '========================
    Public Property Title As String
    Public Property Items As New List(Of MenuItem)
    Public Property Appearance As New MenuAppearance
    Public Property MenuKeys As New MenuKeys
    Friend Property RootMenu As NativeMenu
    Private _SelectIndex As Integer = 0
    Private FirstVisible As Integer = 0
    Public Property SubMenusList As New List(Of NativeMenu)
    ' Friend Shared CurrentEditingItem As MenuItem
    'Scrollbar state
    Private ScrollbarAlpha As Integer = 0
    Private ScrollbarVisible As Boolean = False
    Private LastScrollChange As Integer = 0
    ''' <summary>
    ''' Creates new menu.
    ''' </summary>
    Public Sub New(MenuTitle As String)
        Title = MenuTitle
        Appearance = New MenuAppearance
        RootMenu = Me
        OptionsBinding = New Dictionary(Of String, Object)
    End Sub
    Public Property IsRedefineKeyMenu As Boolean = False
    ''' <summary>
    ''' Adds a category item (not selectable) or a button linked to a SubMenu.
    ''' </summary>
    Public Sub AddItem(Name As String, Optional Description As String = "", Optional SubMenuLinked As NativeMenu = Nothing,
                       Optional ActionToTrigger As Action = Nothing, Optional SelectedColor As ColorRGBA = Nothing, Optional DescriptionColor As ColorRGBA = Nothing)
        Dim subMenu As NativeMenu = Nothing

        If SubMenuLinked IsNot Nothing Then
            subMenu = Me.SubMenu(SubMenuLinked.Title)
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
        item.IsSubsection = Not item.HasValue AndAlso item.SubMenuLinked Is Nothing AndAlso item.Action Is Nothing

        Items.Add(item)
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

        Dim entries = IniFileReader.ReadSection(FilePath, Section)

        If entries.Count = 0 Then
            Tools.WriteLog("Error in NativeMenu.AddItems_FromFileSection of " & Title & ": section " & Section & " is empty or missing.")
            Return
        End If

        For Each kvp In entries

            Dim keyName As String = kvp.Key
            Dim rawValue As String = kvp.Value

            If Items.Any(Function(i) i.Name = keyName) Then Continue For

            Dim parsedValue As Object = ParseValueWithClamp(rawValue, ClampValue)

            AddItem_WithValue(Name:=keyName, Value:=parsedValue, SelectedColor:=SelectedColor, DescriptionColor:=DescriptionColor, Clamp:=ClampValue)
        Next
    End Sub
    ''' <summary>
    ''' Adds an in-game editable item with fixed ValueType.
    ''' The value is entered manually by the player.
    ''' </summary>
    Public Sub AddItem_InGame(Name As String, ValueType As MenuItem.MenuValueType, Optional Description As String = "", Optional SelectedColor As ColorRGBA = Nothing,
                          Optional DescriptionColor As ColorRGBA = Nothing, Optional Clamp As (Min As Single, Max As Single)? = Nothing)

        Dim creator As New MenuItem With {
        .Name = Name,
        .Description = Description,
        .HasValue = True,
        .ValueType = MenuItem.MenuValueType.Text,
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
    End Sub
    ''' <summary>
    ''' Adds a selectable string list value.
    ''' </summary>
    Public Sub AddItem_WithTextList(Name As String, Values As IEnumerable(Of String), Optional Description As String = "", Optional DefaultValue As String = "",
                                    Optional SelectedColor As ColorRGBA = Nothing, Optional DescriptionColor As ColorRGBA = Nothing)

        If Values Is Nothing Then Exit Sub

        Dim list As List(Of String) = Values.ToList()
        If list.Count = 0 Then Exit Sub

        ' Avoid duplicates
        If Items.Any(Function(i) i.Name = Name) Then Exit Sub

        ' Resolve default index safely
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
        .ValueType = MenuItem.MenuValueType.TextList,
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
            If Items.Any(Function(i) i.Name = Name) Then Exit Sub
        End If
        ' --- Menu di riconfigurazione: solo Keys
        If Me.IsRedefineKeyMenu Then
            If TypeOf Value IsNot Keys Then
                '  Throw New InvalidOperationException($"Cannot add a non-key value '{Name}' in Redefine Key menu '{Me.Title}'.")
            End If

            Dim keyItem As New KeyBindItem(Name, Description, DirectCast(Value, Keys)) With {
            .HasValue = True, .Color_SelectedRect = If(SelectedColor = Nothing, Appearance.Item_SelectedColor, SelectedColor),
            .Color_DescriptionRect = If(DescriptionColor = Nothing, Appearance.Description_BoxColor, DescriptionColor),
            .ValueType = MenuItem.MenuValueType.KeyBind, .AssignedKey = Value, .ParentMenu = Me}

            ' Aggiorna automaticamente il dictionary dei bindings
            OptionsBinding(Name) = CType(keyItem.AssignedKey, Object)

            ' AddHandler keyItem.KeyChanged, AddressOf OnKeyItemChanged

            '  AddHandler keyItem.KeyChanged, Sub(sender As KeyBindItem, k As Keys)
            'OptionsBinding(sender.Name) = k
            '   End Sub
            Items.Add(keyItem)


            Return
        End If

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
                item.ValueType = MenuItem.MenuValueType.Bool
                item.Value_Boolean = CBool(Value)
                ResizeValuePrecision = 0

                OptionsBinding(Name) = item.GetTypedValue
            Case TypeOf Value Is Integer
                item.ValueType = MenuItem.MenuValueType.Int

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
                item.ValueType = MenuItem.MenuValueType.Float

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
                item.ValueType = MenuItem.MenuValueType.Text
                item.Value = CStr(Value)
                OptionsBinding(Name) = item.GetTypedValue
            Case Else
                ' Throw New ArgumentException("Unsupported value type. Only Boolean, Integer, Single, String or Keys are allowed.")
        End Select

        AddHandler Me.ItemValueChanged,
        Sub(changedItem)
            If changedItem.Name = Name Then
                Select Case changedItem.ValueType
                    Case MenuItem.MenuValueType.Bool : OptionsBinding(Name) = changedItem.GetTypedValue
                    Case MenuItem.MenuValueType.Int : OptionsBinding(Name) = changedItem.GetTypedValue
                    Case MenuItem.MenuValueType.Float : OptionsBinding(Name) = changedItem.GetTypedValue
                    Case MenuItem.MenuValueType.Text : OptionsBinding(Name) = changedItem.GetTypedValue
                End Select
            End If
        End Sub

        Items.Add(item)
    End Sub
    ''' <summary>
    ''' Finds and returns the menu or submenu that contains
    ''' an item with the specified name.
    ''' </summary>
    ''' <param name="ItemName">Name of the menu item.</param>
    ''' <param name="RootMenu">Root menu to search from.</param>
    Public Shared Function GetMenuFromItem(ItemName As String, RootMenu As NativeMenu) As NativeMenu

        If RootMenu Is Nothing OrElse String.IsNullOrWhiteSpace(ItemName) Then
            Return Nothing
        End If

        ' check items in this menu
        If RootMenu.Items.Any(Function(it) it.Name = ItemName) Then
            Return RootMenu
        End If

        ' search recursively in submenus
        For Each sm In RootMenu.SubMenusList
            Dim found = GetMenuFromItem(ItemName, sm)
            If found IsNot Nothing Then Return found
        Next

        Return Nothing
    End Function
    ''' <summary>
    ''' Removes the first menu item that matches the provided name.
    ''' Safely updates selection index and scroll controls.
    ''' </summary>
    ''' <param name="Name">The display name of the item to remove.</param>
    ''' <returns>True if an item was found and removed, otherwise False.</returns>
    Public Function RemoveItem(Name As String) As Boolean

        If String.IsNullOrWhiteSpace(Name) Then Return False

        ' find item by name
        Dim index As Integer = Items.FindIndex(Function(it) it.Name = Name)
        If index = -1 Then Return False ' no item found

        ' remove
        Items.RemoveAt(index)

        ' selection fixes
        If _SelectIndex >= Items.Count Then
            _SelectIndex = Math.Max(Items.Count - 1, 0)
        End If
        Dim firstVisible As Integer = Math.Max(0, _SelectIndex - Appearance.MaxVisibleItems + 1)
        If firstVisible > _SelectIndex Then
            firstVisible = _SelectIndex
        End If

        '' reset scrollbar fade visibility
        'scrollFade = 0F
        'showScrollbar = False
        'lastScrollTime = 0

        Return True
    End Function
    ''' <summary>
    ''' Marks an item as locked (greyed out and skipped during navigation).
    ''' </summary>
    Public Sub LockItem(Name As String, Optional Lock As Boolean = True)
        Dim it = Items.FirstOrDefault(Function(x) x.Name = Name)
        If it IsNot Nothing Then
            it.IsLocked = Lock
        End If
    End Sub
    Private Sub Show(Optional ShowMenu As Boolean = True)
        EnsureInitialized()
        If ShowMenu Then
            If _activeSubMenu IsNot Nothing Then
                _activeSubMenu.Show(False)
                _activeSubMenu = Nothing
            End If

            If Not _isActive Then
                ResetMenuState()
                _isActive = True
                CurrentActiveMenu = Me

                TickHelper.Add_Internal(AddressOf ProcessMenu)
                TriggerScrollbar()
            End If

        Else
            If _isActive Then
                _isActive = False
                'ResetMenuState()
                If _isEditingText Then
                    _editingTextItem.Value = _previousText
                    ExitTextEdit()
                End If

                TickHelper.Remove_Internal(AddressOf ProcessMenu)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Retrieves or creates a submenu by name.  
    ''' If "Redefine Keys" is requested, automatically creates the special redefine SubMenu.
    ''' </summary>
    Public Function SubMenu(Name As String) As NativeMenu

        Dim sm As NativeMenu = SubMenusList.FirstOrDefault(Function(s) s.Title = Name)

        If sm IsNot Nothing Then Return sm

        sm = New NativeMenu(Name)
        sm._parentMenu = Me
        sm.RootMenu = RootMenu
        sm.OptionsBinding = OptionsBinding
        If sm.Title = "Redefine Keys" Then sm.IsRedefineKeyMenu = True
        SubMenusList.Add(sm)
        'AddHandler sm.ValueChanged,
        '    Sub(it As MenuItem, newValue As Object)
        '        ' Rilancia lo stesso evento anche su questo menu
        '        RaiseEvent ValueChanged(it, newValue)
        '    End Sub
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
                If _isActive Then Show(False)
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
    ''' <param name="OnValueChange">The function to trigger to update parameters inside main script.</param>
    Public Sub BindValueChange(OnValueChange As Action(Of MenuItem, Object))
        AddHandler RootMenu.ValueChanged, Sub(item, newvalue)
                                              OnValueChange(item, newvalue)
                                          End Sub
    End Sub
    '---------------------------------
    '============================================================================================================================
    'Controls 
    '-----------
    Private _inputBlockedUntil As Integer = 0
    Private _navHoldStart As Integer = 0
    Private _navLastRepeat As Integer = 0
    Private _navHoldingDir As Integer = 0
    Private Sub HandleControls()
        'multiple input avoider
        Dim now As Integer = NativeTimer.TotalGameTime
        Dim blockHardInput As Boolean = now < _inputBlockedUntil

        'editing text item
        If _isEditingText Then
            HandleTextInput()
            Return
        End If

        'submenu: back to parent menu
        If _activeSubMenu IsNot Nothing Then

            If NativeControls.isKeyPressed(MenuKeys.Back) AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil Then
                BackToParent()
                _inputBlockedUntil = NativeTimer.TotalGameTime + 50
                IsEditingKey = False
                _isEditingText = False
                Return
            End If

            _activeSubMenu.HandleControls()
            Return
        End If

        'main menu: close
        If NativeControls.isKeyPressed(MenuKeys.Back) AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil AndAlso _parentMenu Is Nothing Then
            Me.Show(False)
            IsEditingKey = False
            Return
        End If

        ' back inside submenu
        If _parentMenu IsNot Nothing Then
            If NativeControls.isKeyPressed(MenuKeys.Back) AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil Then
                BackToParent()
                _inputBlockedUntil = NativeTimer.TotalGameTime + 50
                Return
            End If
        End If

        Dim it = Items(_SelectIndex)

        'key binding
        If TypeOf it Is KeyBindItem Then
            Dim keyItem As KeyBindItem = DirectCast(it, KeyBindItem)

            If keyItem.WaitingForKey AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil Then
                ' If  Then
                Dim k = NativeControls.GetKeyPressed()
                If k.HasValue Then
                    If k <> Keys.LWin AndAlso k <> Keys.RWin AndAlso k <> Keys.Enter AndAlso k <> Keys.Escape AndAlso k <> Keys.Return AndAlso
                        k <> Keys.Back AndAlso k <> Keys.NumLock AndAlso k <> Keys.Pause AndAlso k <> Keys.Print Then
                        IsEditingKey = False
                        keyItem.SetKey(k)
                        'RaiseEvent ValueChanged(keyItem, k)
                        OnKeyItemChanged(keyItem, k)
                        _hasPendingChanges = True
                        _currentEditingItem = Nothing
                        _isEditingText = False
                        '  _applyAction.Invoke
                    Else
                        ' k = Nothing
                        DisplayText(TextColors.MP_Red & "Key not valid!", 1000)
                    End If
                End If
                Return

                ' End If

                'If pressed.HasValue Then
                '    keyItem.AssignedKey = pressed.Value
                '    keyItem.WaitingForKey = False

                '    ' Aggiorna visualizzazione del valore
                '    it.Value = pressed.Value.ToString()

                '    ' Notifica esterno
                '    RaiseEvent KeyReassigned(keyItem, pressed.Value)

                '    Return
                'End If

                '' se nessun tasto premuto → ignora tutto il resto
                'Return
            End If
        End If
        '-----
        'nav keys
        HandleNavHold()

        ' da qui in poi, rispetti il blocco
        If blockHardInput Then Return
        ' ----
        ' ---------------
        ' ---- ENTER ----
        ' ---------------
        'submenu
        If Not it.IsLocked AndAlso it.SubMenuLinked IsNot Nothing Then
            If NativeControls.isKeyPressed(MenuKeys.Enter) Then
                EnterItem()
            End If
        End If

        If NativeControls.isKeyPressed(Keys.Enter) Then
            If it.SubMenuLinked Is Nothing And Not it.IsLocked Then
                If it.Action IsNot Nothing Then
                    it.Action.Invoke()
                    ' DisplayText("Trying to invoke action")
                Else
                    ' DisplayText("no action")
                End If
            End If
        End If

        'key binding: waiting
        If NativeControls.isKeyPressed(MenuKeys.Enter) AndAlso IsEditingKey = False Then
            If TypeOf it Is KeyBindItem AndAlso Not it.IsLocked Then
                _inputBlockedUntil = NativeTimer.TotalGameTime + 50
                Dim keyItem As KeyBindItem = DirectCast(it, KeyBindItem)
                keyItem.WaitingForKey = True
                IsEditingKey = True
                _currentEditingItem = it
                it.Value = "Press a key..."
                it.Description = "Press a key to change " & it.Name & " key..."
                ' DisplayText("")
                Return
            End If
        End If
        ' ----
        ' --------------------
        ' ---- VALUE EDIT ----
        ' --------------------
        'left/right values edit

        'If it.ValueType = MenuValueType.Text AndAlso Not it.IsLocked Then
        If it.HasValue AndAlso it.ValueType = MenuItem.MenuValueType.Text AndAlso Not it.IsLocked Then
            If NativeControls.isKeyPressed(MenuKeys.Enter) AndAlso Not _isEditingText Then
                _isEditingText = True
                _editingTextItem = it
                _previousText = it.Value
                _textBuffer = ""
                it.Value = ""
                _currentEditingItem = it
                Return
            End If
        End If
        If it.HasValue AndAlso Not it.IsLocked Then ' AndAlso it.ValueType <> MenuValueType.Bool AndAlso it.ValueType <> MenuValueType.KeyBind Then
            If it.ValueType = MenuItem.MenuValueType.Bool Then
                If NativeControls.isKeyPressed(Keys.Left) Then
                    ModifyValue(it, -1)
                ElseIf NativeControls.isKeyPressed(Keys.Right) Then
                    ModifyValue(it, +1)
                End If
            ElseIf it.ValueType = MenuItem.MenuValueType.Float OrElse it.ValueType = MenuItem.MenuValueType.Int OrElse it.ValueType = MenuItem.MenuValueType.TextList Then
                HandleValueHold(it)
            End If
        End If
        'If it.HasValue AndAlso Not it.IsLocked Then
        '    If NativeControls.isKeyPressed(MenuKeys.DecreaseValue) Then
        '        ModifyValue(it, -1)
        '    End If
        '    If NativeControls.isKeyPressed(MenuKeys.IncreaseValue) Then
        '        ModifyValue(it, +1)
        '    End If
        'End If

    End Sub
    Private Sub HandleValueHold(it As MenuItem)

        Dim now As Integer = NativeTimer.TotalGameTime

        Dim dir As Integer = 0
        If NativeControls.isKeyPressed(Keys.Left, False) Then dir = -1
        If NativeControls.isKeyPressed(Keys.Right, False) Then dir = 1

        If dir = 0 Then
            _holdingItem = Nothing
            _repeatCount = 0
            Return
        End If

        If _holdingItem IsNot it OrElse _holdingDirection <> dir Then
            ' Primo click
            _holdingItem = it
            _holdingDirection = dir
            _holdStartTime = now
            _lastRepeatTime = now
            _repeatCount = 0

            ModifyValue(it, dir)
            ' HighlightValueArrows(it)
            Return
        End If

        Dim elapsed As Integer = now - _holdStartTime

        If elapsed < 250 Then Return

        Dim interval As Integer = If(_repeatCount < 9, 150, 60)
        If now - _lastRepeatTime < interval Then Return

        _lastRepeatTime = now
        _repeatCount += 1

        Dim stepmultiplier As Integer = 1
        If it.ValueType = MenuItem.MenuValueType.Int OrElse it.ValueType = MenuItem.MenuValueType.Float Then
            stepmultiplier = If(_repeatCount >= 9, 10, 1)
        End If

        ModifyValue(it, dir * stepmultiplier)
        ' HighlightValueArrows(it)
        ' it.ValueJustChangedDirection = dir
    End Sub
    Private Sub HandleTextInput()

        Dim key = NativeControls.GetKeyPressed()
        If key = Keys.Delete Then
            _editingTextItem.Value = _previousText
            ExitTextEdit()
            Return
        End If

        If Not key.HasValue Then Return

        If key = Keys.Back Then
            If _textBuffer.Length > 0 Then
                _textBuffer = _textBuffer.Substring(0, _textBuffer.Length - 1)
            End If
        ElseIf key = Keys.Enter Then

            If Not IsValidString(_textBuffer) Then
                DisplayText(TextColors.MP_Red & "String not valid!", 2000)

                '  _editingTextItem.Value = _previousText
                '  ExitTextEdit()
                Return
            End If

            If _editingTextItem.IsInGameCreator Then

                Dim newName As String = _textBuffer

                If Items.Any(Function(x) x.Name = newName) Then
                    DisplayText(TextColors.MP_Red & "Name already exists!", 2000)
                    Return
                End If

                Dim insertIndex As Integer = Items.IndexOf(_editingTextItem)

                Dim clampTuple As (Min As Single, Max As Single)? = Nothing

                If _editingTextItem.CreatorMinValue.HasValue AndAlso _editingTextItem.CreatorMaxValue.HasValue Then

                    clampTuple = (_editingTextItem.CreatorMinValue.Value,
                  _editingTextItem.CreatorMaxValue.Value)
                End If

                Select Case _editingTextItem.InGameCreatorValueType
                    Case MenuItem.MenuValueType.Int
                        AddItem_WithValue(newName, 1, Clamp:=clampTuple)

                    Case MenuItem.MenuValueType.Float
                        AddItem_WithValue(newName, 1.0F, Clamp:=clampTuple)

                    Case MenuItem.MenuValueType.Text
                        AddItem_WithValue(newName, "")

                    Case MenuItem.MenuValueType.Bool
                        AddItem_WithValue(newName, False)

                    Case Else
                        AddItem_WithValue(newName, 1, Clamp:=clampTuple)
                End Select

                Dim created As MenuItem = Items(Items.Count - 1)

                If _editingTextItem.CreatorMinValue.HasValue Then
                    created.MinValue = _editingTextItem.CreatorMinValue
                End If
                If _editingTextItem.CreatorMaxValue.HasValue Then
                    created.MaxValue = _editingTextItem.CreatorMaxValue
                End If

                Items.RemoveAt(Items.Count - 1)
                Items.Insert(insertIndex, created)

                _editingTextItem.Value = ""
                _SelectIndex = insertIndex
                'created.GetRootMenu().NotifyValueChanged(created, created.GetTypedValue())
                ' Applica clamp manuale al valore iniziale (in-game)
                Select Case created.ValueType
                    Case MenuItem.MenuValueType.Int
                        If created.MinValue.HasValue AndAlso created.MaxValue.HasValue Then
                            created.Value_Integer =
                Tools.ClampInteger(created.Value_Integer,
                                   CInt(created.MinValue.Value),
                                   CInt(created.MaxValue.Value))
                        End If

                    Case MenuItem.MenuValueType.Float
                        If created.MinValue.HasValue AndAlso created.MaxValue.HasValue Then
                            created.Value_Float =
                Tools.ClampFloat(created.Value_Float,
                                 created.MinValue.Value,
                                 created.MaxValue.Value)
                        End If
                End Select

                created.GetRootMenu().NotifyValueChanged(created, created.GetTypedValue())

                UpdateVisible()
                ExitTextEdit()
                Return

            End If

            _editingTextItem.Value = _textBuffer
            RootMenu.NotifyValueChanged(_editingTextItem, _textBuffer)
            ExitTextEdit()
            Return
        Else
            Dim ch As Char? = ConvertKeyToChar(key.Value)
            If ch.HasValue Then
                _textBuffer &= ch.Value
            End If
        End If

        _editingTextItem.Value = _textBuffer
    End Sub
    Private Sub ExitTextEdit()
        _isEditingText = False
        _currentEditingItem = Nothing
        _editingTextItem = Nothing
        _textBuffer = ""
        _previousText = ""
        _holdingItem = Nothing
        _holdingDirection = 0
        _repeatCount = 0
    End Sub
    Private Sub MoveUp()
        _SelectIndex -= 1
        If _SelectIndex < 0 Then _SelectIndex = Items.Count - 1

        ' Skip subsection
        If Items(_SelectIndex).IsSubsection Then
            _SelectIndex -= 1
            If _SelectIndex < 0 Then _SelectIndex = Items.Count - 1
        End If

        UpdateVisible()
        TriggerScrollbar()
    End Sub
    Private Sub MoveDown()
        _SelectIndex += 1
        If _SelectIndex >= Items.Count Then
            _SelectIndex = 0
            FirstVisible = 0
        End If

        ' Skip subsection
        If Items(_SelectIndex).IsSubsection Then
            _SelectIndex += 1
            If _SelectIndex >= Items.Count Then _SelectIndex = 0
        End If

        UpdateVisible()
        TriggerScrollbar()
    End Sub
    Private Sub HandleNavHold()

        Dim now As Integer = NativeTimer.TotalGameTime

        Dim dir As Integer = 0
        If NativeControls.isKeyPressed(MenuKeys.NavUp, False) Then dir = -1
        If NativeControls.isKeyPressed(MenuKeys.NavDown, False) Then dir = 1

        ' nessun tasto premuto
        If dir = 0 Then
            _navHoldingDir = 0
            Return
        End If

        ' primo frame (edge)
        If _navHoldingDir = 0 Then
            _navHoldingDir = dir
            _navHoldStart = now
            _navLastRepeat = now

            If dir = -1 Then MoveUp() Else MoveDown()
            Return
        End If

        ' cambio direzione
        If _navHoldingDir <> dir Then
            _navHoldingDir = dir
            _navHoldStart = now
            _navLastRepeat = now

            If dir = -1 Then MoveUp() Else MoveDown()
            Return
        End If

        ' hold delay
        If now - _navHoldStart < 160 Then Return

        ' repeat rate
        If now - _navLastRepeat < 60 Then Return

        _navLastRepeat = now
        If dir = -1 Then MoveUp() Else MoveDown()

    End Sub
    Friend Sub ModifyValue(item As MenuItem, Direction As Integer)
        If item.IsLocked Then Return

        Select Case item.ValueType

            Case MenuItem.MenuValueType.Bool
                item.Value_Boolean = Not item.Value_Boolean

            Case MenuItem.MenuValueType.Int
                Dim prec As Integer = Tools.ClampInteger(item._valueResizePrecision, 0, 7)
                Dim stepValue As Integer = CInt(Math.Pow(10, prec))

                item.Value_Integer += Direction * stepValue

                If item.MinValue.HasValue AndAlso item.MaxValue.HasValue Then
                    item.Value_Integer =
                    Tools.ClampInteger(item.Value_Integer, CInt(item.MinValue.Value), CInt(item.MaxValue.Value))
                End If

            Case MenuItem.MenuValueType.Float
                Dim prec As Integer = Tools.ClampInteger(item._valueResizePrecision, 0, 5)
                Dim stepValue As Single = CSng(Math.Pow(10, -prec))

                item.Value_Float += Direction * stepValue

                If item.MinValue.HasValue AndAlso item.MaxValue.HasValue Then
                    item.Value_Float =
                    Tools.ClampFloat(item.Value_Float, CSng(item.MinValue.Value), CSng(item.MaxValue.Value))
                End If

            Case MenuItem.MenuValueType.TextList
                If item.Value_TextList Is Nothing OrElse item.Value_TextList.Count = 0 Then Return

                item.TextListIndex += Direction

                ' Clamp index (non wrap invisibile)
                item.TextListIndex =
                Tools.ClampInteger(item.TextListIndex, 0, item.Value_TextList.Count - 1)

                item.Value = item.Value_TextList(item.TextListIndex)

        End Select

        item.ValueJustChanged = True
        item.ValueChangeTime = NativeTimer.TotalGameTime
        item.ValueJustChangedDirection = Math.Sign(Direction)

        item.GetRootMenu().NotifyValueChanged(item, item.GetTypedValue())
    End Sub
    Private Sub UpdateVisible()
        If _SelectIndex < FirstVisible Then
            FirstVisible = _SelectIndex
        ElseIf _SelectIndex >= FirstVisible + Appearance.MaxVisibleItems Then
            FirstVisible = _SelectIndex - Appearance.MaxVisibleItems + 1
        End If
    End Sub
    Private Sub EnterItem()

        Dim it = Items(_SelectIndex)

        If it Is Nothing OrElse it.SubMenuLinked Is Nothing Then
            Return
        End If

        Me.Show(False)

        it.SubMenuLinked.ResetMenuState()
        'If it.SubMenuLinked.Title = "Redefine Keys" Then
        '    it.SubMenuLinked.EnsureMenuKeysItems()
        'End If

        it.SubMenuLinked.Show(True)
        _activeSubMenu = it.SubMenuLinked
        _activeSubMenu._inputBlockedUntil = NativeTimer.TotalGameTime + 50
        _CurrentActiveMenu = _activeSubMenu
    End Sub
    Public Sub BackToParent()

        Dim now As Integer = NativeTimer.TotalGameTime

        If _parentMenu Is Nothing Then
            Me.Show(False)
            Return
        End If

        'chiudi questo submenu
        Me.Show(False)

        'reset stati "hold" + edit sul submenu
        _holdingItem = Nothing
        _repeatCount = 0
        _holdingDirection = 0
        _isEditingText = False
        IsEditingKey = False
        _currentEditingItem = Nothing

        'prima pulisci il parent
        _parentMenu._activeSubMenu = Nothing
        _parentMenu._inputBlockedUntil = now + 120 'evita stuck input al ritorno

        'poi riapri parent
        _parentMenu.Show(True)
        _parentMenu.CurrentActiveMenu = _parentMenu

    End Sub
    Private Sub ResetMenuState()
        ScrollbarAlpha = 0
        FirstVisible = 0
        If Items(FirstVisible).IsSubsection Then
            If Items(FirstVisible + 1).IsSubsection Then _SelectIndex = 2 Else _SelectIndex = 1
        Else
            _SelectIndex = 0
        End If
        _activeSubMenu = Nothing
    End Sub
    Private Sub ProcessMenu()
        If Not _isActive OrElse Items.Count = 0 Then Return

        ' Gestione dei controlli (anche per sottomenu)
        HandleControls()

        ' Disegno menu o sottomenu attivo
        If _activeSubMenu IsNot Nothing Then
            _activeSubMenu.ProcessMenu() ' delega al submenu
        Else
            DrawMenu()
            DrawDescription()
            ScrollbarUpdateFade()
        End If
    End Sub
    Private Function IsValidString(s As String) As Boolean
        If String.IsNullOrWhiteSpace(s) Then Return False
        Return s.All(Function(c) Char.IsLetterOrDigit(c) OrElse c = "_"c OrElse c = " "c)
    End Function
    ''' <summary>
    ''' Converts a string value to Boolean, Integer, Single or String.
    ''' </summary>
    Private Function ParseValue(value As String) As Object

        Dim b As Boolean
        If Boolean.TryParse(value, b) Then Return b

        Dim i As Integer
        If Integer.TryParse(value, i) Then Return i

        Dim f As Single
        If Single.TryParse(value, Globalization.NumberStyles.Float,
                       Globalization.CultureInfo.InvariantCulture, f) Then
            Return f
        End If

        ' fallback string
        Return value
    End Function
    ''' <summary>
    ''' Parses a string value and applies numeric clamp if requested.
    ''' Supports Integer and Single values stored as strings.
    ''' </summary>
    Private Function ParseValueWithClamp(rawValue As String,
                                     Optional Clamp As (Min As Single, Max As Single)? = Nothing) As Object

        ' Integer
        Dim i As Integer
        If Integer.TryParse(rawValue, i) Then
            If Clamp.HasValue Then
                i = Tools.ClampInteger(i, CInt(Clamp.Value.Min), CInt(Clamp.Value.Max))
            End If
            Return i
        End If

        ' Float (Single)
        Dim f As Single
        If Single.TryParse(rawValue.Replace(",", "."), Globalization.NumberStyles.Float,
                       Globalization.CultureInfo.InvariantCulture, f) Then
            If Clamp.HasValue Then
                f = Tools.ClampFloat(f, Clamp.Value.Min, Clamp.Value.Max)
            End If
            Return f
        End If

        ' Fallback: string
        Return rawValue
    End Function
    '============================================================================================================================
    'SCROLLBAR
    '-----------
    Private _cachedSelectableItems As List(Of MenuItem)
    Private _cachedSelectableCount As Integer = -1
    Private _cacheVersion As Integer = -1
    '  Private _menuKeysInjected As Boolean = False
    Friend Property IsEditingKey As Boolean
    Private Sub ScrollbarUpdateFade()
        If Not ScrollbarVisible Then Return

        Dim elapsed As Integer = NativeTimer.TotalGameTime - LastScrollChange
        Dim maxAlpha As Integer = Appearance.Scrollbar_Colors.Thumb.Alpha

        'Fade-in
        If ScrollbarAlpha < maxAlpha AndAlso elapsed <= Appearance.Scrollbar_AutoHide.Time Then
            ScrollbarAlpha += CInt(Appearance.Scrollbar_AutoHide.FadeSpeed * Tools.GetCoefFPS(30) * maxAlpha)
            If ScrollbarAlpha > maxAlpha Then ScrollbarAlpha = maxAlpha
        End If

        'Fade-out
        If elapsed > Appearance.Scrollbar_AutoHide.Time Then
            ScrollbarAlpha -= CInt(Appearance.Scrollbar_AutoHide.FadeSpeed * Tools.GetCoefFPS(30) * maxAlpha)
            If ScrollbarAlpha <= 0 Then
                ScrollbarAlpha = 0
                ScrollbarVisible = False
            End If
        End If
    End Sub
    Private Sub UpdateSelectableCache()

        If _cacheVersion <> Items.Count Then

            _cachedSelectableItems = Items.
            Where(Function(i) Not i.IsSubsection).
            ToList()

            _cachedSelectableCount = _cachedSelectableItems.Count

            _cacheVersion = Items.Count
        End If

    End Sub
    Private Sub TriggerScrollbar()
        ScrollbarVisible = True
        LastScrollChange = NativeTimer.TotalGameTime
        ' ScrollbarAlpha = 0
    End Sub
    '===========================================
    Private Sub DrawMenu()
        Dim visibleCount As Integer = Math.Min(Items.Count, Appearance.MaxVisibleItems)
        Dim dynamicHeight As Single = Appearance.MenuSize.Y + (visibleCount * Appearance.Item_Height)

        Dim size As New Vector2(Appearance.MenuSize.X, dynamicHeight)

        'Menu
        DrawCurvedWindow(New Vector2(Appearance.MenuPosition.X, Appearance.MenuPosition.Y), size, CInt(Appearance.WindowAlpha))

        'Title
        Dim titleHeight As Single = Appearance.Title_Height
        Dim Pos As (X As Single, Y As Single) = (Appearance.MenuPosition.X, Appearance.MenuPosition.Y)
        DrawCurvedWindow(New Vector2(Appearance.MenuPosition.X, Appearance.MenuPosition.Y), New Vector2(size.X, titleHeight), CInt(Appearance.WindowAlpha))
        Appearance.ApplyCustomFontIfNeeded(MenuAppearance.MenuTextContext.Title)
        DisplayTextAdvanced(Title, 1, New Vector2(Appearance.MenuPosition.X + Appearance.Title_TextOffsets.x, Appearance.MenuPosition.Y + +Appearance.Title_TextOffsets.Y), Appearance.TextStyle_Title)
        '----------
        Dim startY As Single = Appearance.MenuPosition.Y + titleHeight + Appearance.Items_StartOffsetY
        For i = FirstVisible To Math.Min(Items.Count - 1, FirstVisible + Appearance.MaxVisibleItems - 1) 'For i = FirstVisible To FirstVisible + visibleCount - 1

            Dim it = Items(i)
            Dim y As Single = startY + (i - FirstVisible) * Appearance.Item_Height

            'If i = SelectedIndex Then
            '    DrawRectangle(New Vector2((Appearance.MenuPosition.X + size.X / 2) - 0.004F, y + 0.008F),
            '                  New Vector2(size.X - 0.02F, Appearance.PerItemHeight - 0.004F), it.Color_SelectedRect)
            'End If
            Dim rectColor As ColorRGBA = it.Color_SelectedRect

            If it Is _currentEditingItem Then
                rectColor = ColorRGBA.InvertColor(rectColor)
            End If

            If i = _SelectIndex Then
                DrawRectangle(New Vector2((Appearance.MenuPosition.X + size.X / 2) - Appearance.Item_SelectedOffsets.X, y + Appearance.Item_SelectedOffsets.Y),
                              New Vector2(size.X - Appearance.Item_SelectedPadding.X, Appearance.Item_Height - Appearance.Item_SelectedPadding.Y), rectColor)
            End If
            '---------------------------
            'Option name
            '-------------
            Dim ts As TextStyle
            Dim color As ColorRGBA
            If Items(i).IsSubsection Then
                ts = Appearance.TextStyle_Subsection
                color = ts.Color
            Else
                ts = Appearance.TextStyle_Item
                If it.IsLocked Then
                    color = New ColorRGBA(100, 100, 100, 255)
                Else
                    color = ts.Color
                End If
            End If

            Appearance.ApplyCustomFontIfNeeded(If(it.IsSubsection, MenuAppearance.MenuTextContext.Subsection, MenuAppearance.MenuTextContext.Item))
            DisplayTextAdvanced(it.Name, 1, New Vector2(Appearance.MenuPosition.X + Appearance.Item_TextOffsetX, y),
                                New TextStyle(color, ts.Scale, ts.Center, ts.Borders, ts.Background, ts.Proportional))
            '---------------------------
            'Value 
            '-------
            If it.HasValue Then
                ts = Appearance.TextStyle_ItemValue
                If it.IsLocked Then
                    color = New ColorRGBA(100, 100, 100, 255)
                Else
                    color = ts.Color
                End If
                Dim StringColored As String
                If it.GetValueString = "ON" Then
                    StringColored = TextColors.MP_LightGreen & "ON" & TextColors.FX_StopColorFxHere
                ElseIf it.GetValueString = "OFF" Then
                    StringColored = TextColors.MP_Red & "OFF" & TextColors.FX_StopColorFxHere
                Else
                    StringColored = TextColors.FX_StopColorFxHere & it.GetValueString
                End If
                'DisplayTextAdvanced("< " & StringColored & " >", 1, New Vector2(Appearance.MenuPosition.X + size.X * 0.755F, y),
                '                    New TextStyle(color, ts.Scale, ts.Center, ts.Borders, ts.Background, ts.Proportional))
                Dim canLeft As Boolean = True
                Dim canRight As Boolean = True

                Select Case it.ValueType
                    Case MenuItem.MenuValueType.Int
                        If it.MinValue.HasValue AndAlso it.Value_Integer <= it.MinValue.Value Then
                            canLeft = False
                        End If
                        If it.MaxValue.HasValue AndAlso it.Value_Integer >= it.MaxValue.Value Then
                            canRight = False
                        End If

                    Case MenuItem.MenuValueType.Float
                        If it.MinValue.HasValue AndAlso it.Value_Float <= it.MinValue.Value Then
                            canLeft = False
                        End If
                        If it.MaxValue.HasValue AndAlso it.Value_Float >= it.MaxValue.Value Then
                            canRight = False
                        End If

                    Case MenuItem.MenuValueType.TextList
                        canLeft = it.TextListIndex > 0
                        canRight = it.TextListIndex < it.Value_TextList.Count - 1
                End Select

                Dim RESET As String = TextColors.FX_StopColorFxHere
                Dim gray As String = Appearance.Item_DisabledArrowColor

                Dim arrowLeft As String =
                    If(canLeft, "< ", gray & "< " & RESET)

                Dim arrowRight As String =
                    If(canRight, " >", gray & " >" & RESET)

                ' Evidenzia animazione
                If it.ValueJustChanged AndAlso NativeTimer.TotalGameTime - it.ValueChangeTime < Appearance.Item_ValueChangeHighlightTime Then
                    If it.ValueJustChangedDirection = -1 AndAlso canLeft Then
                        arrowLeft = Appearance.Item_HighlightArrowColor & "< " & RESET
                    ElseIf it.ValueJustChangedDirection = 1 AndAlso canRight Then
                        arrowRight = Appearance.Item_HighlightArrowColor & " >" & RESET
                    End If
                Else
                    it.ValueJustChanged = False
                    it.ValueJustChangedDirection = 0
                End If
                Appearance.ApplyCustomFontIfNeeded(MenuAppearance.MenuTextContext.ItemValue)
                'NativeDraws.NativeFonts.Change(NativeFonts.Fonts.Dotted)
                DisplayTextAdvanced(arrowLeft & StringColored & arrowRight, 1, New Vector2(Appearance.MenuPosition.X + size.X * Appearance.Item_ValueOffsetX, y),
                                    New TextStyle(color, ts.Scale, ts.Center, ts.Borders, ts.Background, ts.Proportional))

            End If
        Next
        '---------------------------
        'ScrollBar
        '----------
        DrawScrollbar(New Vector2(Appearance.MenuPosition.X, Appearance.MenuPosition.Y), size)
    End Sub
    Private Sub DrawDescription()
        Dim it = Items(_SelectIndex)
        If String.IsNullOrWhiteSpace(it.Description) Then Return

        Dim maxTextWidth As Single = Appearance.Description_TextWrapMaxWidth
        Dim wrappedLines As List(Of String) = WrapTextManual(it.Description, maxTextWidth)
        Dim lineCount As Integer = wrappedLines.Count

        Dim descHeight As Single = (Appearance.Description_PerLineHeight * lineCount) + Appearance.Description_BoxHeightAdjust
        Dim posY As Single = Appearance.Description_BaseOffsetY + (Appearance.MenuSize.Y + (Math.Min(Items.Count, Appearance.MaxVisibleItems) * Appearance.Item_Height)) + Appearance.Description_PositionY
        Dim pos As New Vector2(Appearance.MenuPosition.X, posY)
        Dim size As New Vector2(Appearance.MenuSize.X, descHeight)

        ' ======
        DrawRectangle(New Vector2(pos.X + size.X / 2, pos.Y + size.Y / 2), New Vector2(size.X, size.Y), it.Color_DescriptionRect)
        ' ======
        For i As Integer = 0 To wrappedLines.Count - 1
            Dim lineY As Single = pos.Y + Appearance.Description_TextPadding.Y + i * Appearance.Description_PerLineHeight

            'Native.Function.Call("SET_TEXT_WRAP", pos.X + 0.01, maxTextWidth)
            'Native.Function.Call("SET_TEXT_LINE_DISPLAY", 2.0, 101)
            'Native.Function.Call("SET_TEXT_LINE_HEIGHT_MULT", 1.06)

            DisplayTextAdvanced(wrappedLines(i), 1, New Vector2(pos.X + Appearance.Description_TextPadding.X, lineY), Appearance.TextStyle_Description)
        Next
    End Sub
    Private Sub DrawScrollbar(menuPos As Vector2, menuSize As Vector2)
        UpdateSelectableCache()

        Dim selectable = _cachedSelectableItems
        Dim totalSelectable As Integer = _cachedSelectableCount

        If totalSelectable <= Appearance.MaxVisibleItems Then Return
        If ScrollbarAlpha <= 0 Then Return

        Dim currentSelectableIndex As Integer = selectable.IndexOf(Items(_SelectIndex))

        If currentSelectableIndex < 0 Then currentSelectableIndex = 0

        Dim visibleCount As Integer = Math.Min(totalSelectable, Appearance.MaxVisibleItems)
        Dim listHeight As Single = visibleCount * Appearance.Item_Height

        Dim baseX As Single = menuPos.X + menuSize.X - Appearance.Scrollbar_Offsets.X
        Dim baseY As Single = menuPos.Y + Appearance.Title_Height + Appearance.Scrollbar_Offsets.Y
        Dim pad As Single = Appearance.Scrollbar_VerticalPadding

        ' --- Background
        Dim colorBackgr As ColorRGBA = Appearance.Scrollbar_Colors.Background
        DrawRectangle(New Vector2(baseX, (baseY + listHeight / 2) - (pad / 2)), New Vector2(Appearance.Scrollbar_Width.Background, listHeight - pad),
                      New ColorRGBA(colorBackgr.Red, colorBackgr.Green, colorBackgr.Blue, ScrollbarAlpha))

        ' ------
        Dim thumbHeight As Single = listHeight * (visibleCount / Math.Max(1, totalSelectable))
        Dim scrollPos As Single = CSng(currentSelectableIndex / Math.Max(1, totalSelectable - 1)) * (listHeight - thumbHeight)

        Dim colorThumb As ColorRGBA = Appearance.Scrollbar_Colors.Thumb

        DrawRectangle(New Vector2(baseX, baseY + ((thumbHeight / 2) + scrollPos) - (pad / 2)), New Vector2(Appearance.Scrollbar_Width.Thumb, thumbHeight - pad),
                      New ColorRGBA(colorThumb.Red, colorThumb.Green, colorThumb.Blue, ScrollbarAlpha))
    End Sub
    ' ===== Modalità modifica tasto =====
    Friend Shared _currentEditingItem As MenuItem = Nothing
    '   Private _editBackgroundColor As ColorRGBA = New ColorRGBA(255, 255, 0, 255) ' default giallo
    ''' <summary>
    ''' Converts a Keys value into a printable character.
    ''' Returns Nothing if the key is not a valid text character.
    ''' </summary>
    Friend Function ConvertKeyToChar(k As Keys) As Char?

        Dim shift As Boolean =
        NativeControls.isKeyPressed(Keys.ShiftKey, False) OrElse
        NativeControls.isKeyPressed(Keys.LShiftKey, False) OrElse
        NativeControls.isKeyPressed(Keys.RShiftKey, False)

        Dim caps As Boolean = Control.IsKeyLocked(Keys.CapsLock)

        ' -------- Letters A-Z
        If k >= Keys.A AndAlso k <= Keys.Z Then
            Dim c As Char = ChrW(Asc("A"c) + (k - Keys.A))
            If shift Xor caps Then
                Return c
            Else
                Return Char.ToLower(c)
            End If
        End If

        ' -------- Numbers 0-9
        If k >= Keys.D0 AndAlso k <= Keys.D9 Then
            Dim normal() As Char = "0123456789".ToCharArray()
            Dim shifted() As Char = ")!@#$%^&*(".ToCharArray()
            Dim index As Integer = k - Keys.D0
            Return If(shift, shifted(index), normal(index))
        End If

        ' -------- Numpad numbers
        If k >= Keys.NumPad0 AndAlso k <= Keys.NumPad9 Then
            Return ChrW(Asc("0"c) + (k - Keys.NumPad0))
        End If

        ' -------- Space
        If k = Keys.Space Then Return " "c

        ' -------- Not supported
        Return Nothing

    End Function
    ''' <summary>
    ''' Estimates normalized text width for wrap calculations.
    ''' Uses customizable character widths configuration.
    ''' </summary>
    Private Function EstimateTextWidth(text As String) As Single
        If String.IsNullOrEmpty(text) Then Return 0F

        Dim w = Appearance.TextWrap_CharWidths
        Dim width As Single = 0F

        For Each ch As Char In text
            Select Case ch
                Case "W"c, "M"c
                    width += w.Wide

                Case "w"c, "m"c
                    width += w.Medium

                Case "I"c, "i"c, "l"c, "."c, ","c, "'"c, "`"c
                    width += w.Narrow

                Case " "c
                    width += w.Space

                Case Else
                    width += w.DefaultChar
            End Select
        Next

        Dim baseScale As Single =
        Appearance.TextStyle_Description.Scale.X *
        Appearance.Description_TextWrapCoef

        Return width * baseScale
    End Function
    ''' <summary>
    ''' Splits text into wrapped lines based on visual width estimation.
    ''' </summary>
    Private Function WrapTextManual(text As String, maxWidth As Single) As List(Of String)

        Dim words() As String = text.Split(" "c)
        Dim lines As New List(Of String)
        Dim current As String = ""

        For Each w In words
            Dim test = If(current = "", w, current & " " & w)

            If EstimateTextWidth(test) > maxWidth Then
                lines.Add(current)
                current = w
            Else
                current = test
            End If
        Next

        If current <> "" Then lines.Add(current)
        Return lines
    End Function
    Friend Sub OnKeyItemChanged(item As KeyBindItem, newKey As Keys)
        item.Value = newKey.ToString
        OptionsBinding(item.Name) = newKey
        RootMenu.NotifyValueChanged(item, item.GetTypedValue())
    End Sub
    ''' <summary>
    ''' Raises ValueChanged on the RootMenu, so every submenu change is routed to the same single event stream.
    ''' </summary>
    Friend Sub NotifyValueChanged(item As MenuItem, newValue As Object)

        item.ValueJustChanged = True
        item.ValueChangeTime = NativeTimer.TotalGameTime

        ' Funnel everything to RootMenu (single event source)
        Dim root As NativeMenu = Me.RootMenu
        If root IsNot Nothing AndAlso root IsNot Me Then
            root.NotifyValueChanged(item, newValue)
            Return
        End If

        RaiseEvent ValueChanged(item, newValue)
    End Sub
    Private _isInitialized As Boolean = False
    Private Sub EnsureInitialized()
        Try
            If _isInitialized Then
                Return
            Else
            End If
            _isInitialized = True
        Catch ex As Exception

        End Try
    End Sub
    Friend NotInheritable Class IniFileReader

        ''' <summary>
        ''' Reads all key/value pairs from a specific INI section.
        ''' </summary>
        Public Shared Function ReadSection(filePath As String, section As String) As Dictionary(Of String, String)

            Dim result As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

            Dim currentSection As String = ""
            Dim targetSection As String = section.Trim()

            For Each rawLine In IO.File.ReadAllLines(filePath)

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

            Return result
        End Function

    End Class
End Class
Public Class MenuItem
    Friend _valueResizePrecision As Integer
    Friend Property ValueJustChanged As Boolean = False
    Friend Property ValueJustChangedDirection As Integer = 0
    Friend Property ValueChangeTime As Integer = 0
    Public Property MinValue As Single? = Nothing
    Public Property MaxValue As Single? = Nothing
    Friend Property CreatorMinValue As Single?
    Friend Property CreatorMaxValue As Single?
    Public Property Value_TextList As List(Of String)
    Public Property TextListIndex As Integer = 0
    Public Property ParentMenu As NativeMenu
    '---------------------------------
    'new
    Friend Property IsInGameCreator As Boolean = False
    Friend Property InGameCreatorValueType As MenuItem.MenuValueType = MenuItem.MenuValueType.Int
    '-----------------------------------------
    Friend ReadOnly Property IsSelectable As Boolean
        Get
            Return Not IsLocked AndAlso (HasValue OrElse SubMenuLinked IsNot Nothing)
        End Get
    End Property
    ''' <summary>The displayed name of the menu item.</summary>
    Public Property Name As String
    ''' <summary>Optional description shown at the bottom.</summary>
    Public Property Description As String
    ''' <summary>If true, left/right actions are active.</summary>
    Public Property HasValue As Boolean

    ''' <summary>Visible text representing the current value.</summary>
    Public Property Value As String

    ''' <summary>Optional submenu associated with this item.</summary>
    Public Property SubMenuLinked As NativeMenu

    ''' <summary>The highlight color when this item is selected.</summary>
    Public Property Color_SelectedRect As ColorRGBA

    ''' <summary>The background color for the additional description.</summary>
    Public Property Color_DescriptionRect As ColorRGBA
    ''' <returns>True if the item is locked and can't be selected.</returns>
    Public Property IsLocked As Boolean = False
    ''' <returns>True if the item is a section element and can't be selected.</returns>
    Public Property IsSubsection As Boolean = False
    Public Property Action As Action
    Public Enum MenuValueType
        Text
        Bool
        Int
        Float
        KeyBind
        TextList
    End Enum
    Public ValueType As MenuValueType

    Public Value_Boolean As Boolean
    Public Value_Integer As Integer
    Public Value_Float As Single
    'Public Value_Key As Keys
    Public Overridable Function GetValueString() As String
        Select Case ValueType
            Case MenuValueType.Bool
                Return If(Value_Boolean, "ON", "OFF")
            Case MenuValueType.Int
                Return Value_Integer.ToString()
            Case MenuValueType.Float
                Dim prec As Integer = Tools.ClampInteger(_valueResizePrecision, 0, 7)
                Return Tools.FloatToString(Value_Float, prec)
            Case MenuValueType.Text
                Return Value
            Case MenuValueType.KeyBind
                Return CType(Me, KeyBindItem).AssignedKey.ToString
            Case MenuValueType.TextList
                Return Value
            Case Else
                Return "Error: type " & Me.GetType().ToString & " not valid!"
        End Select
    End Function
    ''' <summary>
    ''' Returns the underlying typed value of this item.
    ''' </summary>
    Friend Overridable Function GetTypedValue() As Object
        Select Case ValueType
            Case MenuValueType.Bool
                Return Value_Boolean
            Case MenuValueType.Int
                Return Value_Integer
            Case MenuValueType.Float
                Return Value_Float
            Case MenuValueType.Text
                Return Value
            Case MenuValueType.TextList
                Return Value
            Case Else
                'If TypeOf Me Is KeyBindItem Then
                '    Return DirectCast(Me, KeyBindItem).AssignedKey '.ToString
                'End If
                Tools.WriteLog("Error in NativeMenu...GetTypedValue: type " & Me.GetType().ToString & " not valid!")
                Tools.WriteLog("   in item of name " & Me.Name)
                Return Nothing
        End Select
    End Function
    '''' <summary>
    '''' Returns the menu or submenu that owns this item.
    '''' </summary>
    'Public Function GetParentMenu() As NativeMenu
    '    Return ParentMenu
    'End Function
    Friend Function GetRootMenu() As NativeMenu
        Dim m As NativeMenu = Me.ParentMenu
        While m IsNot Nothing AndAlso m._parentMenu IsNot Nothing
            m = m._parentMenu
        End While
        Return m
    End Function
    '===================================================================================
End Class
''' <summary>
''' Contains all visual and behavioral customization options for a NativeMenu.
''' </summary>
Public Class MenuAppearance
    '  Private ReadOnly _textTitleStyle As Color

    '---- Colors
    Public Property Title_Brighten As Single = 1.35
    Public Property Subsection_Brighten As Single = 1.55
    Public Property WindowAlpha As Byte = 180
    '---- Dimesions
    Public Property MenuSize As (X As Single, Y As Single) = (0.2, 0.083)
    Public Property MenuPosition As (X As Single, Y As Single) = (0.02F, 0.04F)
    Public Property Title_Height As Single = 0.0605
    Public Property MaxVisibleItems As Integer = 10
    '-----
    Friend Property CustomFont As (Font As NativeFonts.Fonts, Title As Boolean, Subsection As Boolean, Item As Boolean, ItemValue As Boolean,
                                           Description As Boolean) = (NativeFonts.Fonts.Narrow, True, True, False, False, False)
    '-----
    Public Property Description_BoxColor As ColorRGBA = New ColorRGBA(0, 162, 232, 160)
    Public Property Description_PerLineHeight As Single = 0.0195
    Public Property Description_PositionY As Single = 0.012
    Public Property Description_TextWrapMaxWidth As Single = 0.214
    Public Property Description_TextWrapCoef As Single = 0.0375
    Public Property Description_BaseOffsetY As Single = 0.032F
    Public Property Description_BoxHeightAdjust As Single = 0.008F
    Public Property Description_TextPadding As (X As Single, Y As Single) = (X:=0.005, Y:=0.005)
    Public Property TextWrap_CharWidths As (Wide As Single,Medium As Single,Narrow As Single, Space As Single, DefaultChar As Single) = 
                                           (Wide:=1.75F, Medium:=1.25F, Narrow:=0.4F, Space:=0.6F, DefaultChar:=1.0F)
    '-----
    Public Property Items_StartOffsetY As Single = 0.018F
    Public Property Item_SelectedOffsets As (X As Single, Y As Single) = (X:=0.004F, Y:=0.008F)
    Public Property Item_SelectedPadding As (X As Single, Y As Single) = (X:=0.02F, Y:=0.004F)
    Public Property Item_TextOffsetX As Single = 0.012F
    Public Property Item_SelectedColor As ColorRGBA = New ColorRGBA(0, 162, 232, 180)
    Public Property Item_ValueOffsetX As Single = 0.755F
    Public Property Item_ValueChangeHighlightTime As Integer = 100
    Public Property Item_Height As Single = 0.033
    ''' <summary>Use NativeDraws.TextColors</summary>
    Public Property Item_DisabledArrowColor As String = TextColors.Black
    ''' <summary>Use NativeDraws.TextColors</summary>
    Public Property Item_HighlightArrowColor As String = TextColors.Fx_Highlighted & TextColors.MP_Orange
    '---- TextStyles
    Public Property TextStyle_Title As TextStyle
        Get
            If _customTextStyleTitle IsNot Nothing Then
                Return _customTextStyleTitle
            End If

            Return New TextStyle(ColorRGBA.Brighten(Item_SelectedColor, Title_Brighten), New Vector2(0.255, 0.5), False, ColorRGBA.Black)
        End Get
        Set(value As TextStyle)
            _customTextStyleTitle = value
        End Set
    End Property
    Public Property TextStyle_Subsection As TextStyle
        Get
            If _customTextStyleSubsection IsNot Nothing Then
                Return _customTextStyleSubsection
            End If

            Return New TextStyle(ColorRGBA.Brighten(Item_SelectedColor, Subsection_Brighten), New Vector2(0.17, 0.33))
        End Get
        Set(value As TextStyle)
            _customTextStyleSubsection = value
        End Set
    End Property
    Public Property TextStyle_Item As New TextStyle(ColorRGBA.White, New Vector2(0.14, 0.25))
    Public Property TextStyle_ItemValue As New TextStyle(ColorRGBA.White, New Vector2(0.14, 0.25), True)
    Public Property TextStyle_Description As New TextStyle(ColorRGBA.White, New Vector2(0.14, 0.25))

    Private _customTextStyleTitle As TextStyle = Nothing
    Private _customTextStyleSubsection As TextStyle = Nothing

    '---- SCROLLBAR SETTINGS
    Public Property Scrollbar_Enable As Boolean = True
    ''' <summary>Time in Milliseconds</summary>
    Public Property Scrollbar_AutoHide As (Time As Integer, FadeSpeed As Single) = (1800, 0.045)
    Public Property Scrollbar_Width As (Thumb As Single, Background As Single) = (0.0045, 0.0015)
    Public Property Scrollbar_Colors As (Thumb As ColorRGBA, Background As ColorRGBA) = (New ColorRGBA(200, 200, 200, 200), New ColorRGBA(80, 80, 80, 200))
    Public Property Scrollbar_Offsets As (X As Single, Y As Single) = (0.0067, 0.018)
    Public Property Scrollbar_VerticalPadding As Single = 0.01
    '-----
    Public Property Title_TextOffsets As (x As Single, Y As Single) = (0.01, 0.015)
    ' Public Property Title_TextOffsetX As Single = 0.01
    ' Public Property Title_TextOffsetY As Single = 0.015
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
    ''' Sets all main color-related properties of the menu.
    ''' </summary>
    Public Sub SetColors(Optional ItemSelected As ColorRGBA = Nothing, Optional DescriptionBox As ColorRGBA = Nothing,
                         Optional ScrollbarThumb As ColorRGBA = Nothing, Optional ScrollbarBackground As ColorRGBA = Nothing)

        If ItemSelected = Nothing Then
            Me.Item_SelectedColor = ItemSelected
        End If

        If DescriptionBox = Nothing Then
            Me.Description_BoxColor = DescriptionBox
        End If

        Dim thumb = Me.Scrollbar_Colors.Thumb
        Dim back = Me.Scrollbar_Colors.Background

        If ScrollbarThumb = Nothing Then thumb = ScrollbarThumb
        If ScrollbarBackground = Nothing Then back = ScrollbarBackground

        Me.Scrollbar_Colors = (thumb, back)
    End Sub
    ''' <summary>
    ''' Configures description box, text wrapping and layout.
    ''' </summary>
    Public Sub SetDescription(Optional BoxColor As ColorRGBA = Nothing, Optional PositionY As Single? = Nothing, Optional BaseOffsetY As Single? = Nothing,
                              Optional BoxHeightAdjust As Single? = Nothing, Optional PerLineHeight As Single? = Nothing, Optional TextPadding As (X As Single, Y As Single)? = Nothing,
                              Optional WrapMaxWidth As Single? = Nothing, Optional WrapCoef As Single? = Nothing,
                              Optional CharWidths As (Wide As Single, Medium As Single, Narrow As Single, Space As Single, DefaultChar As Single)? = Nothing,
                              Optional TextStyle As TextStyle = Nothing)

        If BoxColor = Nothing Then Me.Description_BoxColor = BoxColor
        If PositionY.HasValue Then Me.Description_PositionY = PositionY.Value
        If BaseOffsetY.HasValue Then Me.Description_BaseOffsetY = BaseOffsetY.Value
        If BoxHeightAdjust.HasValue Then Me.Description_BoxHeightAdjust = BoxHeightAdjust.Value
        If PerLineHeight.HasValue Then Me.Description_PerLineHeight = PerLineHeight.Value
        If TextPadding.HasValue Then Me.Description_TextPadding = TextPadding.Value
        If WrapMaxWidth.HasValue Then Me.Description_TextWrapMaxWidth = WrapMaxWidth.Value
        If WrapCoef.HasValue Then Me.Description_TextWrapCoef = WrapCoef.Value
        If CharWidths.HasValue Then Me.TextWrap_CharWidths = CharWidths.Value

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
    Public Sub SetTitle(Optional Brighten As Single? = Nothing, Optional TextOffsets As (X As Single, Y As Single)? = Nothing,
                        Optional CustomTextStyle As TextStyle = Nothing)

        If Brighten.HasValue Then Me.Title_Brighten = Brighten.Value
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
                            Optional Offsets As (X As Single, Y As Single)? = Nothing, Optional VerticalPadding As Single? = Nothing)

        If Enabled.HasValue Then Me.Scrollbar_Enable = Enabled.Value

        If AutoHideTimeMs.HasValue OrElse FadeSpeed.HasValue Then
            Me.Scrollbar_AutoHide = (If(AutoHideTimeMs, Me.Scrollbar_AutoHide.Time), If(FadeSpeed, Me.Scrollbar_AutoHide.FadeSpeed))
        End If

        If Widths.HasValue Then Me.Scrollbar_Width = Widths.Value
        If Colors.HasValue Then Me.Scrollbar_Colors = Colors.Value
        If Offsets.HasValue Then Me.Scrollbar_Offsets = Offsets.Value
        If VerticalPadding.HasValue Then Me.Scrollbar_VerticalPadding = VerticalPadding.Value
    End Sub
    ''' <summary>
    ''' Sets a custom font and specifies where it should be applied.
    ''' </summary>
    Public Sub SetCustomFont(Font As NativeFonts.Fonts, Optional Title As Boolean = False, Optional Subsection As Boolean = False,
                             Optional Item As Boolean = False, Optional ItemValue As Boolean = False, Optional Description As Boolean = False)

        Me.CustomFont = (Font, Title, Subsection, Item, ItemValue, Description)
    End Sub
    Friend Sub ApplyCustomFontIfNeeded(context As MenuTextContext)

        ' nessun font custom
        If CustomFont.Font = NativeFonts.Fonts.None Then Return

        Dim apply As Boolean = False

        Select Case context
            Case MenuTextContext.Title
                apply = CustomFont.Title

            Case MenuTextContext.Subsection
                apply = CustomFont.Subsection

            Case MenuTextContext.Item
                apply = CustomFont.Item

            Case MenuTextContext.ItemValue
                apply = CustomFont.ItemValue

            Case MenuTextContext.Description
                apply = CustomFont.Description
        End Select

        If apply Then
            NativeDraws.NativeFonts.Change(CustomFont.Font)
        End If

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
''' <summary>
''' Contains all keyboard shortcuts used by the menu.
''' These values can be customized per-menu instance.
''' </summary>
Public Class MenuKeys

    Public Property ToggleMenu As Keys = Keys.F8
    Public Property NavUp As Keys = Keys.Up
    Public Property NavDown As Keys = Keys.Down
    Public Property DecreaseValue As Keys = Keys.Left
    Public Property IncreaseValue As Keys = Keys.Right
    Public Property Enter As Keys = Keys.Enter
    Public Property Back As Keys = Keys.Back

End Class
Public Class KeyBindItem
    Inherits MenuItem
    Public Property AssignedKey As Keys
    Public Property WaitingForKey As Boolean = False
    '---------------------------------
    Friend _keyChanged As Boolean = False
    Friend _originalDescription As String
    '''' <summary>
    '''' Event fired when the key has been changed
    '''' </summary>
    'friend Event KeyChanged(item As KeyBindItem, keys As Object)
    ''' <summary>
    ''' Private property to track if the key was modified
    ''' </summary>
    Friend Property KeyChangedState As Boolean
        Get
            Return _keyChanged
        End Get
        Set(value As Boolean)
            _keyChanged = value
            If value Then
                'Tools.WriteLog($"[NativeMenu] Raising KeyChanged for '{Name}' = {AssignedKey}") ' debug
                'RaiseEvent KeyChanged(Me, AssignedKey)
            End If
        End Set
    End Property
    Friend Sub SetKey(k As Keys)
        AssignedKey = k
        KeyChangedState = True
        WaitingForKey = False
        Description = _originalDescription
        Value = AssignedKey.ToString()
        'Tools.WriteLog($"[NativeMenu] SetKey called on item '{Name}' -> {AssignedKey}") ' debug
        'DisplayText("SetKey = " & k, 1000, DisplayNow:=False)
    End Sub
    Public Sub New(name As String, description As String, defaultKey As Keys)
        MyBase.New()
        Me.Name = name
        Me.Description = description
        Me.AssignedKey = defaultKey
        Me.ValueType = MenuValueType.KeyBind
        Me.HasValue = True
        Me.Value = defaultKey.ToString()
        _originalDescription = description
    End Sub
    Public Overrides Function GetValueString() As String
        If ParentMenu.IsEditingKey AndAlso Me Is NativeMenu._currentEditingItem Then
            Return "Press a key..."
        End If

        Return AssignedKey.ToString()
    End Function
    Friend Overrides Function GetTypedValue() As Object
        Return AssignedKey
    End Function
End Class