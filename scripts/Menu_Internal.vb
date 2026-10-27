Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports GTA
Imports NativeExtended
Imports NativeExtended.NativeDraws
Imports NativeExtended.Tools

'---------------------- Internal Funcs ----------------------
Partial Public Class NativeMenu
    Private ScrollbarAlpha As Integer = 0
    Private ScrollbarVisible As Boolean = False
    Private LastScrollChange As Integer = 0
    Private _SelectIndex As Integer = 0
    Private FirstVisible As Integer = 0
    Friend Property RootMenu As NativeMenu
    Private _isActive As Boolean = False
    Private _activeSubMenu As NativeMenu = Nothing
    Friend _parentMenu As NativeMenu = Nothing
    Friend _applyItem As MenuItem = Nothing
    Private _holdStartTime As Integer = 0
    Private _lastRepeatTime As Integer = 0
    Private _repeatCount As Integer = 0
    Private _holdingDirection As Integer = 0
    Private _holdingItem As MenuItem = Nothing
    Private Event ItemValueChanged(Item As MenuItem)
    Friend Property Items As New List(Of MenuItem)
    Friend Property OptionsBinding As Dictionary(Of String, Object)
    Friend Event ValueChanged(e As MenuValueChangedEventArgs)
    Friend Event ItemRemoved(e As MenuItemRemovedEventArgs)
    Private _isEditingText As Boolean = False
    Private _editingTextItem As MenuItem = Nothing
    Private _textBuffer As String = ""
    Private _previousText As String = ""
    Private _hasPendingChanges As Boolean = False
    Private _cachedSelectableItems As List(Of MenuItem)
    Private _cachedSelectableCount As Integer = -1
    Private _cacheVersion As Integer = -1
    Private _scrollbarCurrentPos As Single = 0.0F
    Friend Property IsEditingKey As Boolean
    Private ReadOnly _subMenus As New List(Of NativeMenu)
    Friend Property CurrentActiveMenu As NativeMenu = Me
    Private _resetOnNextOpen As Boolean = True
    Private _localSettings As Tools.SettingsFile
    Private _localLog As Tools.LogFile
    Public Property SettingsFile As Tools.SettingsFile
        Get
            If _localSettings IsNot Nothing Then
                Return _localSettings
            End If

            If _parentMenu IsNot Nothing Then
                Return _parentMenu.SettingsFile
            End If

            Return Nothing
        End Get
        Set(value As Tools.SettingsFile)
            _localSettings = value
        End Set
    End Property
    Friend Property LogFile As Tools.LogFile
        Get
            If _localLog IsNot Nothing Then
                Return _localLog
            End If

            If _parentMenu IsNot Nothing Then
                Return _parentMenu.LogFile
            End If

            Return Nothing
        End Get
        Set(value As Tools.LogFile)
            _localLog = value
        End Set
    End Property
    Friend ReadOnly Property SubMenusList As List(Of NativeMenu)
        Get
            Return _subMenus
        End Get
    End Property
    Friend Property SelectedIndex As Integer
        Get
            Return _SelectIndex
        End Get
        Set(value As Integer)
            If Items Is Nothing OrElse Items.Count = 0 Then Return
            If value < 0 OrElse value >= Items.Count Then Return
            If Items(value).IsSubsection Then Return

            _SelectIndex = value
            UpdateVisible()
        End Set
    End Property
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
    Private Sub Show(Optional ShowMenu As Boolean = True, Optional fullClose As Boolean = False)
        EnsureInitialized()

        If ShowMenu Then
            If ClearHelpMessages Then
                NativeDraws.ClearHelp(ClearNative:=True)
            End If
            If _activeSubMenu IsNot Nothing Then
                _activeSubMenu.Show(False)
                _activeSubMenu = Nothing
            End If

            If Not _isActive Then

                If _parentMenu Is Nothing AndAlso _resetOnNextOpen Then
                    ResetMenuStateRecursive()
                    _resetOnNextOpen = False
                End If

                _isActive = True
                CurrentActiveMenu = Me
                InitializeScrollbarOnOpen()

                TickHelper.Add_Internal(AddressOf ProcessMenu)
                TriggerScrollbar()
            End If

        Else
            If _isActive Then
                _isActive = False

                If _isEditingText Then
                    _editingTextItem.Value = _previousText
                    ExitTextEdit()
                End If

                TickHelper.Remove_Internal(AddressOf ProcessMenu)
            End If

            ' ✅ If user fully closes the ROOT, mark reset for next open
            If fullClose AndAlso _parentMenu Is Nothing Then
                _resetOnNextOpen = True
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
    Private Function GetActiveMenuRecursive(menu As NativeMenu) As Boolean
        If menu Is Nothing Then Return False
        If menu._isActive Then Return True

        For Each sm In menu.SubMenusList
            If GetActiveMenuRecursive(sm) Then Return True
        Next

        Return False
    End Function
    '============================================================================================================================
    '--------------------------------
    'Operators & Override Functions
    '--------------------------------
    Public Shared Operator =(a As NativeMenu, b As NativeMenu) As Boolean
        If ReferenceEquals(a, b) Then Return True
        If a Is Nothing OrElse b Is Nothing Then Return False
        Return a.Equals(b)
    End Operator

    Public Shared Operator <>(a As NativeMenu, b As NativeMenu) As Boolean
        Return Not (a = b)
    End Operator
    Public Overrides Function Equals(obj As Object) As Boolean
        If ReferenceEquals(Me, obj) Then Return True
        If obj Is Nothing OrElse obj.GetType() IsNot GetType(NativeMenu) Then Return False
        Dim other = DirectCast(obj, NativeMenu)
        Return String.Equals(Me.Title, other.Title, StringComparison.OrdinalIgnoreCase) AndAlso
           ReferenceEquals(Me.RootMenu, other.RootMenu)
    End Function
    Public Overrides Function GetHashCode() As Integer
        Dim h1 = If(Title Is Nothing, 0, Title.ToLowerInvariant().GetHashCode())
        Dim h2 = If(RootMenu Is Nothing, 0, RuntimeHelpers.GetHashCode(RootMenu))
        Return h1 Xor h2
    End Function
    '============================================================================================================================
#Region "Controls"
    Private _inputBlockedUntil As Integer = 0
    Private _navHoldStart As Integer = 0
    Private _navLastRepeat As Integer = 0
    Private _navHoldingDir As Integer = 0
    Private Sub HandleControls()
        If Not DisableAllKeys Then
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

                If NativeControls.IsKeyPressed(MenuKeys.Back) AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil Then
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
            If NativeControls.IsKeyPressed(MenuKeys.Back) AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil AndAlso _parentMenu Is Nothing Then
                Me.Show(False, fullClose:=True)
                IsEditingKey = False
                Return
            End If

            ' back inside submenu
            If _parentMenu IsNot Nothing Then
                If NativeControls.IsKeyPressed(MenuKeys.Back) AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil Then
                    BackToParent()
                    _inputBlockedUntil = NativeTimer.TotalGameTime + 50
                    Return
                End If
            End If

            ' Dim it = Items(_SelectIndex)
            Dim it As MenuItem = GetSelectedItemInternal(autoFix:=True)
            If it Is Nothing Then Return

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
                End If
            End If
            '-----
            'nav keys
            HandleNavHold()
            If blockHardInput Then Return
            ' --- DELETE handling (non-editing) ---
            If NativeControls.IsKeyPressed(Keys.Delete) AndAlso NativeTimer.TotalGameTime > _inputBlockedUntil Then

                If it.HasValue AndAlso Not it.IsLocked Then
                    Select Case it.ValueType

                        Case ValueType.KeyBind
                            Dim kb = TryCast(it, KeyBindItem)
                            If kb IsNot Nothing Then
                                kb.AssignedKey = Keys.None
                                kb.Value = Keys.None.ToString()
                                OptionsBinding(it.Name) = Keys.None
                                _hasPendingChanges = True
                                RaiseEvent ItemValueChanged(it)
                                ' it.ParentMenu.NotifyValueChanged(it, it.GetTypedValue())
                                _inputBlockedUntil = NativeTimer.TotalGameTime + 120
                                Return
                            End If
                        Case ValueType.Text
                            ' rimuovi item dalla lista
                            Dim removedName = it.Name
                            Dim removedCategory = Me.Title ' oppure sourceMenu.Title se sei in submenu
                            Dim ok = Me.RemoveItem(removedName)

                            If ok Then
                                ' RaiseEvent ItemRemoved(New MenuItemRemovedEventArgs(Me, removedName, removedCategory))
                                _layoutDirty = True
                                _inputBlockedUntil = NativeTimer.TotalGameTime + 120
                                Return
                            End If
                    End Select
                End If

            End If
            ' ---------------
            ' ---- ENTER ----
            ' ---------------
            'submenu
            If Not it.IsLocked AndAlso it.SubMenuLinked IsNot Nothing Then
                If NativeControls.IsKeyPressed(MenuKeys.Enter) Then
                    EnterItem()
                End If
            End If

            If NativeControls.IsKeyPressed(Keys.Enter) Then
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
            If NativeControls.IsKeyPressed(MenuKeys.Enter) AndAlso IsEditingKey = False Then
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
            ' --------------------
            ' ---- VALUE EDIT ----
            ' --------------------
            'left/right values edit

            'If it.ValueType = ValueType.Text AndAlso Not it.IsLocked Then
            If it.HasValue AndAlso it.ValueType = NativeMenu.ValueType.Text AndAlso Not it.IsLocked Then
                If NativeControls.IsKeyPressed(MenuKeys.Enter) AndAlso Not _isEditingText Then
                    _isEditingText = True
                    _editingTextItem = it
                    _previousText = it.Value
                    _textBuffer = ""
                    it.Value = ""
                    _currentEditingItem = it
                    Return
                End If
            End If
            If it.HasValue AndAlso Not it.IsLocked Then ' AndAlso it.ValueType <> ValueType.Bool AndAlso it.ValueType <> ValueType.KeyBind Then
                If it.ValueType = NativeMenu.ValueType.Bool Then
                    If NativeControls.IsKeyPressed(Keys.Left) Then
                        ModifyValue(it, -1)
                    ElseIf NativeControls.IsKeyPressed(Keys.Right) Then
                        ModifyValue(it, +1)
                    End If
                ElseIf it.ValueType = NativeMenu.ValueType.Float OrElse it.ValueType = NativeMenu.ValueType.Int OrElse it.ValueType = NativeMenu.ValueType.TextList Then
                    HandleValueHold(it)
                End If
            End If
        End If

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
            _holdingItem = it
            _holdingDirection = dir
            _holdStartTime = now
            _lastRepeatTime = now
            _repeatCount = 0

            ModifyValue(it, dir)
            Return
        End If

        Dim elapsed As Integer = now - _holdStartTime

        If elapsed < 250 Then Return

        Dim interval As Integer = If(_repeatCount < 9, 150, 60)
        If now - _lastRepeatTime < interval Then Return

        _lastRepeatTime = now
        _repeatCount += 1

        Dim stepmultiplier As Integer = 1
        If it.ValueType = NativeMenu.ValueType.Int OrElse it.ValueType = NativeMenu.ValueType.Float Then
            stepmultiplier = If(_repeatCount >= 9, 10, 1)
        End If

        ModifyValue(it, dir * stepmultiplier)
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
                    Case NativeMenu.ValueType.Int
                        AddItem_WithValue(newName, 1, Clamp:=clampTuple)

                    Case NativeMenu.ValueType.Float
                        AddItem_WithValue(newName, 1.0F, Clamp:=clampTuple)

                    Case NativeMenu.ValueType.Text
                        AddItem_WithValue(newName, "")

                    Case NativeMenu.ValueType.Bool
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

                'clamp
                Select Case created.ValueType
                    Case ValueType.Int
                        If created.MinValue.HasValue AndAlso created.MaxValue.HasValue Then
                            created.Value_Integer =
                Tools.ClampInteger(created.Value_Integer,
                                   CInt(created.MinValue.Value),
                                   CInt(created.MaxValue.Value))
                        End If

                    Case ValueType.Float
                        If created.MinValue.HasValue AndAlso created.MaxValue.HasValue Then
                            created.Value_Float =
                Tools.ClampFloat(created.Value_Float,
                                 created.MinValue.Value,
                                 created.MaxValue.Value)
                        End If
                End Select

                '  created.ParentMenu.NotifyValueChanged(created, created.GetTypedValue()) 'sostituito durante ultima modifica (aggiunta wrap nativo)
                created.ParentMenu.RaiseItemValueChanged(created)
                UpdateVisible()
                ExitTextEdit()
                Return

            End If

            _editingTextItem.RawValue = _textBuffer
            _editingTextItem.Value = _textBuffer
            ' _editingTextItem.ParentMenu.NotifyValueChanged(_editingTextItem, _textBuffer) 'sostituito durante ultima modifica (aggiunta wrap nativo)
            _editingTextItem.ParentMenu.RaiseItemValueChanged(_editingTextItem)
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
    Private Sub MoveSelection(Direction As Integer)

        If Items.Count = 0 Then
            Return
        End If

        For attempt As Integer = 1 To Items.Count

            _SelectIndex =
            (_SelectIndex + Direction + Items.Count) Mod Items.Count

            If Not Items(_SelectIndex).IsSubsection Then
                UpdateVisible()
                TriggerScrollbar()
                Return
            End If

        Next

    End Sub
    Private Sub MoveUp()
        MoveSelection(-1)
    End Sub
    Private Sub MoveDown()
        MoveSelection(1)
    End Sub
    Private Sub HandleNavHold()
        Dim now As Integer = NativeTimer.TotalGameTime

        Dim dir As Integer = 0
        If NativeControls.isKeyPressed(MenuKeys.NavUp, False) Then dir = -1
        If NativeControls.isKeyPressed(MenuKeys.NavDown, False) Then dir = 1

        If dir = 0 Then
            _navHoldingDir = 0
            Return
        End If

        If _navHoldingDir = 0 Then
            _navHoldingDir = dir
            _navHoldStart = now
            _navLastRepeat = now

            If dir = -1 Then MoveUp() Else MoveDown()
            Return
        End If

        If _navHoldingDir <> dir Then
            _navHoldingDir = dir
            _navHoldStart = now
            _navLastRepeat = now

            If dir = -1 Then MoveUp() Else MoveDown()
            Return
        End If

        If now - _navHoldStart < 160 Then Return

        If now - _navLastRepeat < 60 Then Return

        _navLastRepeat = now
        If dir = -1 Then MoveUp() Else MoveDown()
    End Sub
    Friend Sub ModifyValue(item As MenuItem, Direction As Integer)
        If item.IsLocked Then Return

        Select Case item.ValueType

            Case ValueType.Bool
                item.Value_Boolean = Not item.Value_Boolean

            Case ValueType.Int
                Dim prec As Integer = Tools.ClampInteger(item._valueResizePrecision, 0, 7)
                Dim stepValue As Integer = CInt(Math.Pow(10, prec))

                item.Value_Integer += Direction * stepValue

                If item.MinValue.HasValue AndAlso item.MaxValue.HasValue Then
                    item.Value_Integer = Tools.ClampInteger(item.Value_Integer, CInt(item.MinValue.Value), CInt(item.MaxValue.Value))
                End If

            Case ValueType.Float
                Dim prec As Integer = Tools.ClampInteger(item._valueResizePrecision, 0, 5)
                Dim stepValue As Single = CSng(Math.Pow(10, -prec))

                item.Value_Float += Direction * stepValue

                If item.MinValue.HasValue AndAlso item.MaxValue.HasValue Then
                    item.Value_Float = Tools.ClampFloat(item.Value_Float, CSng(item.MinValue.Value), CSng(item.MaxValue.Value))
                End If

            Case ValueType.TextList
                If item.Value_TextList Is Nothing OrElse item.Value_TextList.Count = 0 Then Return

                item.TextListIndex += Direction

                item.TextListIndex = Tools.ClampInteger(item.TextListIndex, 0, item.Value_TextList.Count - 1)

                item.Value = item.Value_TextList(item.TextListIndex)

        End Select

        item.ValueJustChanged = True
        item.ValueChangeTime = NativeTimer.TotalGameTime
        item.ValueJustChangedDirection = Math.Sign(Direction)

        RaiseEvent ItemValueChanged(item)
        'item.ParentMenu.NotifyValueChanged(item, item.GetTypedValue())
    End Sub
    Private Sub EnterItemOLD()

        Dim it = Items(_SelectIndex)

        If it Is Nothing OrElse it.SubMenuLinked Is Nothing Then
            Return
        End If

        Me.Show(False)
        it.SubMenuLinked.ResetMenuState()

        it.SubMenuLinked.Show(True)
        _activeSubMenu = it.SubMenuLinked
        _activeSubMenu._inputBlockedUntil = NativeTimer.TotalGameTime + 50
        _CurrentActiveMenu = _activeSubMenu
    End Sub
    Private Sub EnterItem()

        Dim it = Items(_SelectIndex)
        If it Is Nothing OrElse it.SubMenuLinked Is Nothing Then Return

        Me.Show(False)
        it.SubMenuLinked.Show(True)

        _activeSubMenu = it.SubMenuLinked
        _activeSubMenu._inputBlockedUntil = NativeTimer.TotalGameTime + 50
        _CurrentActiveMenu = _activeSubMenu
    End Sub
    Private Sub BackToParent()
        Dim now As Integer = NativeTimer.TotalGameTime

        If _parentMenu Is Nothing Then
            Me.Show(False, fullClose:=True)
            Return
        End If
        Me.Show(False)

        _holdingItem = Nothing
        _repeatCount = 0
        _holdingDirection = 0
        _isEditingText = False
        IsEditingKey = False
        _currentEditingItem = Nothing

        _parentMenu._activeSubMenu = Nothing
        _parentMenu._inputBlockedUntil = now + 120
        _parentMenu.Show(True)
        _parentMenu.CurrentActiveMenu = _parentMenu
    End Sub
#End Region

    Private Sub ResetMenuState()
        ScrollbarAlpha = 0
        _scrollbarCurrentPos = 0.0F
        FirstVisible = 0
        _activeSubMenu = Nothing
        _layoutDirty = True
        If Items Is Nothing OrElse Items.Count = 0 Then
            _SelectIndex = 0
            Return
        End If
        For i As Integer = 0 To Items.Count - 1
            If Not Items(i).IsSubsection Then
                _SelectIndex = i
                Return
            End If
        Next
        _SelectIndex = 0
    End Sub
    Private Sub ResetMenuStateRecursive()
        ResetMenuState()
        For Each sm In SubMenusList
            If sm IsNot Nothing Then
                sm.ResetMenuStateRecursive()
            End If
        Next
    End Sub
    Private Sub ProcessMenu()
        If Not _isActive OrElse Items.Count = 0 Then Return
        HandleControls()
        If _activeSubMenu IsNot Nothing Then
            _activeSubMenu.ProcessMenu()
        Else
            DrawMenu()
            DrawDescription()
            ScrollbarUpdateFade()
        End If
    End Sub
    Friend Function GetSelectedItemInternal(Optional autoFix As Boolean = False) As MenuItem
        If Items Is Nothing OrElse Items.Count = 0 Then Return Nothing

        If _SelectIndex < 0 OrElse _SelectIndex >= Items.Count Then
            _SelectIndex = Math.Max(0, Math.Min(_SelectIndex, Items.Count - 1))
        End If

        Dim it = Items(_SelectIndex)

        If autoFix AndAlso it IsNot Nothing AndAlso it.IsSubsection Then
            For i As Integer = 0 To Items.Count - 1
                Dim idx = (_SelectIndex + i) Mod Items.Count
                If Not Items(idx).IsSubsection Then
                    _SelectIndex = idx
                    Return Items(idx)
                End If
            Next
        End If

        If it IsNot Nothing AndAlso it.IsSubsection Then Return Nothing
        Return it
    End Function
    Private Sub UpdateVisible()
        If _SelectIndex < FirstVisible Then
            FirstVisible = _SelectIndex
        ElseIf _SelectIndex >= FirstVisible + Appearance.MaxVisibleItems Then
            FirstVisible = _SelectIndex - Appearance.MaxVisibleItems + 1
        End If
        _layoutDirty = True
    End Sub
    Private Function GetRowHeight(it As MenuItem) As Single
        Return Appearance.Item_Height
    End Function
    Private Function GetVisibleContentHeight() As Single
        Dim h As Single = 0.0F

        For Each it In Items.Skip(FirstVisible).Take(Appearance.MaxVisibleItems)
            h += GetRowHeight(it)
        Next

        Return h
    End Function
    '======================================================================================
    Private Sub DrawMenu()
        GXT.HideHelpThisFrame()
        RecalculateLayout()
        Dim size As Vector2 = _layout.MenuSize
        Dim titleHeight As Single = Appearance.Title_Height
        '========================================
        '---
        DrawWindowCurved(New Vector2(Appearance.MenuPosition.X, Appearance.MenuPosition.Y), size, CInt(Appearance.WindowAlpha))

        If Appearance.Title_BackgroundAlpha.HasValue Then
            DrawWindowCurved(New Vector2(Appearance.MenuPosition.X, Appearance.MenuPosition.Y), New Vector2(size.X, titleHeight), CInt(Appearance.Title_BackgroundAlpha.Value))
        End If
        '---
        DrawText(Title, New Vector2(Appearance.MenuPosition.X + Appearance.Title_TextOffsets.x, Appearance.MenuPosition.Y + Appearance.Title_TextOffsets.Y), 1, Appearance.TextStyle_Title.WithColor(Appearance.TextStyle_Title.Color))

        '========================================
        Dim startIndex As Integer = FirstVisible
        Dim currentY As Single = _layout.ItemsStartY
        Dim selectedCenterX As Single = (Appearance.MenuPosition.X + size.X / 2.0F) - Appearance.Item_SelectedOffsets.X
        Dim selectedRectW As Single = size.X - Appearance.Item_SelectedPadding.X
        Dim selectedRectH As Single = Appearance.Item_Height - Appearance.Item_SelectedPadding.Y
        Dim itemTextX As Single = Appearance.MenuPosition.X + Appearance.Item_TextOffsetX
        Dim valueTextX As Single = Appearance.MenuPosition.X + (size.X * Appearance.Item_ValueOffsetX)
        Dim halfItemH As Single = Appearance.Item_Height / 2.0F
        Dim RESET As String = TextColors.FX_StopColorFxHere
        Dim gray As String = Appearance.Item_DisabledArrowColor

        '========================================
        For localIndex As Integer = 0 To _layout.VisibleItems.Count - 1

            Dim it As MenuItem = _layout.VisibleItems(localIndex)
            Dim idx As Integer = startIndex + localIndex
            Dim y As Single = currentY

            '--------------------
            If it.IsSeparator Then

                Dim st = If(it.SeparatorOverride.HasValue, it.SeparatorOverride.Value, Appearance.Separator_Layout)
                Dim lineWidth As Single = selectedRectW * st.Length
                Dim lineY As Single = y + halfItemH
                DrawLine(Position:=New Vector2(selectedCenterX, lineY), Length:=lineWidth, Thickness:=st.Thickness, Color:=it.SeparatorColor, Vertical:=False, Center:=True)
                currentY += Appearance.Item_Height
                Continue For
            End If

            '--------------------
            Dim rectColor As ColorRGBA = it.Color_SelectedRect
            If it Is _currentEditingItem Then rectColor = ColorRGBA.Invert(rectColor)

            If idx = _SelectIndex Then DrawRectangle(New Vector2(selectedCenterX, y + Appearance.Item_SelectedOffsets.Y), New Vector2(selectedRectW, selectedRectH), rectColor)

            '--------------------
            Dim ts As TextStyle
            If it.IsSubsection Then
                ts = Appearance.TextStyle_Subsection
            Else
                ts = Appearance.TextStyle_Item
            End If

            Dim itemTextColor As ColorRGBA = ts.Color
            If it.IsLocked AndAlso Not it.IsSubsection Then
                itemTextColor = Appearance.ValueLocked_Color
            End If

            '--------------------
            DrawText(it.Name, New Vector2(itemTextX, y), 1, ts.WithColor(itemTextColor))

            '--------------------
            If it.HasValue Then
                Dim vts As TextStyle = Appearance.TextStyle_ItemValue
                Dim vColor As ColorRGBA = If(it.IsLocked, Appearance.ValueLocked_Color, vts.Color)
                Dim valueStr As String = it.GetValueString()
                If it.ValueType = ValueType.KeyBind Then
                    Dim kb = TryCast(it, KeyBindItem)
                    If kb IsNot Nothing AndAlso kb.AssignedKey = Keys.None Then
                        valueStr = Appearance.ValueKeyNone_Color & Appearance.ValueKeyNone_Text & RESET
                    End If
                End If
                Dim stringColored As String

                If it.ValueType = ValueType.KeyBind Then
                    Dim kb = TryCast(it, KeyBindItem)
                    If kb IsNot Nothing AndAlso kb.AssignedKey = Keys.None Then
                        stringColored = valueStr
                    Else
                        stringColored = RESET & valueStr
                    End If
                ElseIf valueStr = "True" Then
                    stringColored = Appearance.ValueBoolTrue_Color & Appearance.ValueBoolTrue_Text & RESET
                ElseIf valueStr = "False" Then
                    stringColored = Appearance.ValueBoolFalse_Color & Appearance.ValueBoolFalse_Text & RESET
                Else
                    stringColored = RESET & valueStr
                End If

                Dim canLeft As Boolean = True
                Dim canRight As Boolean = True

                Select Case it.ValueType
                    Case ValueType.Int
                        If it.MinValue.HasValue AndAlso it.Value_Integer <= it.MinValue.Value Then canLeft = False
                        If it.MaxValue.HasValue AndAlso it.Value_Integer >= it.MaxValue.Value Then canRight = False

                    Case ValueType.Float
                        If it.MinValue.HasValue AndAlso it.Value_Float <= it.MinValue.Value Then canLeft = False
                        If it.MaxValue.HasValue AndAlso it.Value_Float >= it.MaxValue.Value Then canRight = False

                    Case ValueType.TextList
                        canLeft = it.TextListIndex > 0
                        canRight = it.TextListIndex < it.Value_TextList.Count - 1
                End Select

                Dim arrowLeft As String = If(canLeft, "< ", gray & "< " & RESET)
                Dim arrowRight As String = If(canRight, " >", gray & " >" & RESET)

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

                DrawText(arrowLeft & stringColored & arrowRight, New Vector2(valueTextX, y), 1, vts.WithColor(vColor))
            End If
            currentY += Appearance.Item_Height
        Next

        DrawScrollbar(New Vector2(Appearance.MenuPosition.X, Appearance.MenuPosition.Y), size)
    End Sub
    Private Sub DrawDescription()

        Dim it As MenuItem = GetSelectedItemInternal(autoFix:=True)

        If it Is Nothing OrElse String.IsNullOrWhiteSpace(it.Description) Then
            Return
        End If

        If _layoutDirty Then
            RecalculateLayout()
        End If

        Dim app As MenuAppearance = Appearance
        Dim wrapWidth As Single = app.Description_ContentWidth

        If wrapWidth <= 0.0F Then
            Return
        End If

        Dim boxTopLeft As New Vector2(
        app.MenuPosition.X,
        _layout.DescriptionTopY)

        Dim textStyle As TextStyle = app.TextStyle_Description

        ' Il wrap di DrawText viene calcolato rispetto alla giustificazione.
        ' Adeguiamo quindi l'ancora affinché il testo resti dentro il box.
        Dim textX As Single =
        boxTopLeft.X + app.Description_TextPadding.X

        Select Case textStyle.Justification

            Case NativeDraws.Justification.Center
                textX += wrapWidth / 2.0F

            Case NativeDraws.Justification.Right
                textX += wrapWidth

            Case NativeDraws.Justification.Left,
             NativeDraws.Justification.Justify
                ' L'ancora coincide con il bordo sinistro dell'area di testo.

        End Select

        Dim textPosition As New Vector2(
        textX,
        boxTopLeft.Y + app.Description_TextPadding.Y)

        Dim wrap As (Length As Single?, LineHeightMultiplier As Single?) =
        (
            Length:=CType(wrapWidth, Single?),
            LineHeightMultiplier:=CType(
                app.Description_LineHeightMultiplier,
                Single?)
        )

        ' Prima misuriamo il testo senza disegnarlo.
        Dim metrics = DrawText_Internal(
        it.Description,
        Duration:=1,
        PositionPercent:=textPosition,
        Style:=textStyle,
        Wrap:=wrap,
        MeasureOnly:=True)

        If metrics.LineCount <= 0 Then
            Return
        End If

        ' L'altezza restituita include tutte le righe e il moltiplicatore
        ' di line-height, ma non il padding esterno del box.
        Dim height As Single =
        metrics.Height + app.Description_BoxHeightAdjust

        Dim boxSize As New Vector2(
        app.MenuSize.X,
        height)

        DrawRectangle(
        New Vector2(
            boxTopLeft.X + boxSize.X / 2.0F,
            boxTopLeft.Y + boxSize.Y / 2.0F),
        boxSize,
        it.Color_DescriptionRect)

        ' Disegniamo la descrizione completa con lo stesso wrap usato
        ' per la misurazione.
        DrawText(
        it.Description,
        Duration:=1,
        PositionPercent:=textPosition,
        Style:=textStyle,
        Wrap:=wrap)

    End Sub
    Private Sub DrawScrollbar(menuPos As Vector2, menuSize As Vector2)
        If Not Appearance.Scrollbar_Enable Then Return
        If ScrollbarAlpha <= 0 Then Return

        UpdateSelectableCache()
        If _cachedSelectableCount <= Appearance.MaxVisibleItems Then Return

        If _layoutDirty Then RecalculateLayout()
        Dim currentIndex As Integer =
        _cachedSelectableItems.IndexOf(Items(_SelectIndex))
        If currentIndex < 0 Then currentIndex = 0
        Dim listHeight As Single = _layout.ScrollbarListHeight
        Dim baseX As Single = menuPos.X + menuSize.X - Appearance.Scrollbar_Offsets.X

        Dim baseY As Single = menuPos.Y + Appearance.Title_Height + Appearance.Scrollbar_Offsets.Y
        Dim pad As Single = Appearance.Scrollbar_VerticalPadding

        ' Background
        DrawRectangle(New Vector2(baseX, baseY + listHeight / 2 - pad / 2), New Vector2(Appearance.Scrollbar_Width.Background, listHeight - pad), New ColorRGBA(
                      Appearance.Scrollbar_Colors.Background.Red, Appearance.Scrollbar_Colors.Background.Green, Appearance.Scrollbar_Colors.Background.Blue, ScrollbarAlpha))

        ' Thumb
        Dim thumbHeight As Single = listHeight * (Appearance.MaxVisibleItems / _cachedSelectableCount)
        Dim targetPos As Single = CSng(currentIndex / Math.Max(1, _cachedSelectableCount - 1)) * (listHeight - thumbHeight)
        If Appearance.Scrollbar_SmoothSpeed <= 0.0F OrElse Appearance.Scrollbar_SmoothSpeed >= 1.0 Then
            _scrollbarCurrentPos = targetPos
        Else
            Dim speed As Single = Tools.ClampFloat(Appearance.Scrollbar_SmoothSpeed, 0.0, 1.0) * Tools.GetCoefFPS(30)
            _scrollbarCurrentPos += (targetPos - _scrollbarCurrentPos) * speed
        End If


        DrawRectangle(New Vector2(baseX, baseY + (thumbHeight / 2) + _scrollbarCurrentPos - pad / 2), New Vector2(Appearance.Scrollbar_Width.Thumb, thumbHeight - pad),
                      New ColorRGBA(Appearance.Scrollbar_Colors.Thumb.Red, Appearance.Scrollbar_Colors.Thumb.Green, Appearance.Scrollbar_Colors.Thumb.Blue, ScrollbarAlpha))
    End Sub
    '======================================================================================
    Private Sub TriggerScrollbar()
        ScrollbarVisible = True
        LastScrollChange = NativeTimer.TotalGameTime
        ' ScrollbarAlpha = 0
    End Sub
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
    Private Sub InitializeScrollbarOnOpen()
        UpdateSelectableCache()

        _layoutDirty = True
        RecalculateLayout()

        If Not Appearance.Scrollbar_Enable Then
            _scrollbarCurrentPos = 0.0F
            Return
        End If

        Dim selectableCount As Integer = Math.Max(0, _cachedSelectableCount)
        If selectableCount <= Appearance.MaxVisibleItems Then
            _scrollbarCurrentPos = 0.0F
            Return
        End If

        Dim currentIndex As Integer = 0
        If _cachedSelectableItems IsNot Nothing AndAlso Items.Count > 0 Then
            currentIndex = _cachedSelectableItems.IndexOf(Items(_SelectIndex))
            If currentIndex < 0 Then currentIndex = 0
        End If

        Dim listHeight As Single = _layout.ScrollbarListHeight
        Dim thumbHeight As Single = listHeight * (Appearance.MaxVisibleItems / CSng(selectableCount))

        Dim targetPos As Single =
        (currentIndex / CSng(Math.Max(1, selectableCount - 1))) * (listHeight - thumbHeight)

        _scrollbarCurrentPos = targetPos
    End Sub
    Friend Sub OnKeyItemChanged(item As KeyBindItem, newKey As Keys)
        item.Value = newKey.ToString
        OptionsBinding(item.Name) = newKey
        '  RootMenu.NotifyValueChanged(item, item.GetTypedValue())
        RaiseEvent ItemValueChanged(item)
        'item.ParentMenu.NotifyValueChanged(item, item.GetTypedValue())
        _layoutDirty = True
    End Sub
    Friend Class MenuItemRemovedEventArgs
        Public ReadOnly Property Menu As NativeMenu
        Public ReadOnly Property ItemName As String
        Public ReadOnly Property Category As String

        Friend Sub New(menu As NativeMenu, itemName As String, category As String)
            Me.Menu = menu
            Me.ItemName = itemName
            Me.Category = category
        End Sub
    End Class
    ''===========================================
    Friend Sub NotifyValueChanged(item As MenuItem, newValue As Object)
        If item Is Nothing Then Return

        If item.ParentMenu IsNot Me Then
            Throw New InvalidOperationException("NotifyValueChanged must be called from the item's ParentMenu.")
        End If

        NotifyValueChanged_Internal(Me, item, newValue)
    End Sub
    Friend Sub NotifyValueChanged_Internal(sourceMenu As NativeMenu, item As MenuItem, newValue As Object)
        item.ValueJustChanged = True
        item.ValueChangeTime = NativeTimer.TotalGameTime

        Dim root As NativeMenu = Me.RootMenu
        If root IsNot Nothing AndAlso root IsNot Me Then
            root.NotifyValueChanged_Internal(sourceMenu, item, newValue)
            Return
        End If

        If root.SettingsFile IsNot Nothing Then
            Try
                Dim category As String = sourceMenu.Title
                Dim name As String = item.Name

                Select Case item.ValueType

                    Case NativeMenu.ValueType.Bool
                        SettingsFile.SetValue(name, category, item.Value_Boolean)

                    Case NativeMenu.ValueType.Int
                        SettingsFile.SetValue(name, category, item.Value_Integer)

                    Case NativeMenu.ValueType.Float
                        SettingsFile.SetValue(name, category, item.Value_Float)

                    Case NativeMenu.ValueType.KeyBind
                        Dim kb = TryCast(item, KeyBindItem)
                        Dim keyToSave As Keys = If(kb IsNot Nothing, kb.AssignedKey, Keys.None)
                        SettingsFile.SetValue(name, category, CInt(keyToSave))

                    Case NativeMenu.ValueType.Text
                        SettingsFile.SetValue(name, category, item.RawValue)

                    Case NativeMenu.ValueType.TextList
                        SettingsFile.SetValue(name, category, item.Value)

                End Select

            Catch ex As Exception
                LogMenuError("Failed to write menu value to settings file.", ex)
            End Try
        End If

        RaiseEvent ValueChanged(New MenuValueChangedEventArgs(sourceMenu, item.Name, newValue))
        _layoutDirty = True
    End Sub
    Private _layoutDirty As Boolean = True
    Private _layout As MenuLayoutCache
    Private Structure MenuLayoutCache
        Public VisibleItems As List(Of MenuItem)
        Public MenuSize As Vector2
        Public ItemsStartY As Single
        Public ContentHeight As Single
        Public DescriptionTopY As Single
        ' Scrollbar list height (only items)
        Public ScrollbarListHeight As Single
    End Structure
    Private Sub RaiseItemRemoved(
    sourceMenu As NativeMenu,
    itemName As String,
    category As String)

        RaiseEvent ItemRemoved(
        New MenuItemRemovedEventArgs(sourceMenu, itemName, category))

    End Sub
    Private Sub RecalculateLayoutOLD()
        _layoutDirty = False
        Dim visible = Items.Skip(FirstVisible).Take(Appearance.MaxVisibleItems).ToList()
        _layout.VisibleItems = visible
        Dim contentHeight As Single = 0.0F
        For Each it In visible
            contentHeight += GetRowHeight(it)
        Next

        _layout.ContentHeight = contentHeight
        _layout.MenuSize = New Vector2(Appearance.MenuSize.X, Appearance.MenuSize.Y + contentHeight)
        _layout.ItemsStartY = Appearance.MenuPosition.Y + Appearance.Title_Height + Appearance.Items_StartOffsetY
        _layout.DescriptionTopY = Appearance.MenuPosition.Y + Appearance.Title_Height + Appearance.Items_StartOffsetY + contentHeight + Appearance.Description_PositionY
        Dim visibleSelectable As Integer = Math.Min(_cachedSelectableCount, Appearance.MaxVisibleItems)
        _layout.ScrollbarListHeight = visibleSelectable * Appearance.Item_Height
    End Sub
    Private Sub RecalculateLayout()
        _layoutDirty = False
        UpdateSelectableCache()
        Dim visible = Items.Skip(FirstVisible).Take(Appearance.MaxVisibleItems).ToList()
        _layout.VisibleItems = visible
        Dim contentHeight As Single = 0.0F
        For Each it In visible
            contentHeight += GetRowHeight(it)
        Next

        _layout.ContentHeight = contentHeight
        _layout.MenuSize = New Vector2(Appearance.MenuSize.X, Appearance.MenuSize.Y + contentHeight)
        _layout.ItemsStartY = Appearance.MenuPosition.Y + Appearance.Title_Height + Appearance.Items_StartOffsetY
        _layout.DescriptionTopY = Appearance.MenuPosition.Y + Appearance.Title_Height + Appearance.Items_StartOffsetY + contentHeight + Appearance.Description_PositionY
        Dim selectableCount As Integer = Math.Max(0, _cachedSelectableCount)
        Dim visibleSelectable As Integer = Math.Min(selectableCount, Appearance.MaxVisibleItems)
        _layout.ScrollbarListHeight = visibleSelectable * Appearance.Item_Height
    End Sub
End Class

'---------------------- Internal Elements ----------------------
Friend Class MenuItem
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
    Friend Property RawValue As String
    '---------------------------------
    'new
    Friend Property IsInGameCreator As Boolean = False
    Friend Property IsSeparator As Boolean = False
    Friend Property SeparatorColor As ColorRGBA = Nothing
    Friend Property InGameCreatorValueType As NativeMenu.ValueType = NativeMenu.ValueType.Int
    Friend Property SeparatorOverride As (Length As Single, Thickness As Single)?
    Friend ReadOnly Property NormalizedName As String
        Get
            Return Name.ToLowerInvariant()
        End Get
    End Property
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
    Public Shared Function GetItemByName(Name As String, Menu As NativeMenu) As MenuItem
        If Menu Is Nothing OrElse String.IsNullOrEmpty(Name) Then Return Nothing

        Dim found = Menu.Items.FirstOrDefault(Function(it) it.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
        If found IsNot Nothing Then Return found

        'subemnu search
        For Each sm In Menu.SubMenusList
            Dim subFound = GetItemByName(Name, sm)
            If subFound IsNot Nothing Then Return subFound
        Next

        Return Nothing
    End Function
    Public ValueType As NativeMenu.ValueType

    Public Value_Boolean As Boolean
    Public Value_Integer As Integer
    Public Value_Float As Single
    'Public Value_Key As Keys
    Public Overridable Function GetValueString() As String
        Select Case ValueType
            Case NativeMenu.ValueType.Bool
                Return Value_Boolean.ToString
            Case NativeMenu.ValueType.Int
                Return Value_Integer.ToString()
            Case NativeMenu.ValueType.Float
                Dim prec As Integer = Tools.ClampInteger(_valueResizePrecision, 0, 7)
                Return Tools.FloatToString(Value_Float, prec)
            Case NativeMenu.ValueType.Text
                Return Value
            Case NativeMenu.ValueType.KeyBind
                Return CType(Me, KeyBindItem).AssignedKey.ToString
            Case NativeMenu.ValueType.TextList
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
            Case NativeMenu.ValueType.Bool
                Return If(Value_Boolean, "True", "False")
            Case NativeMenu.ValueType.Int
                Return Value_Integer
            Case NativeMenu.ValueType.Float
                Return Value_Float
            Case NativeMenu.ValueType.Text
                Return Value
            Case NativeMenu.ValueType.TextList
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
    Friend Function GetRootMenu() As NativeMenu
        Dim m As NativeMenu = Me.ParentMenu
        While m IsNot Nothing AndAlso m._parentMenu IsNot Nothing
            m = m._parentMenu
        End While
        Return m
    End Function
    '===================================================================================
End Class
Friend Class KeyBindItem
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
        Me.ValueType = NativeMenu.ValueType.KeyBind
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
'==================================================================================
'---------------------- Helpers ----------------------
Partial Public Class NativeMenu
    Friend Sub RaiseItemValueChanged(item As MenuItem)

        If item Is Nothing Then
            Return
        End If

        RaiseEvent ItemValueChanged(item)

    End Sub

    Friend Function FindMenuByName(menuName As String) As NativeMenu

        If Me.Title.Equals(menuName, StringComparison.OrdinalIgnoreCase) Then
            Return Me
        End If

        For Each sm In Me.SubMenusList
            Dim found = sm.FindMenuByName(menuName)
            If found IsNot Nothing Then Return found
        Next

        Return Nothing

    End Function
    Friend Shared Function FindMenuFromItem(ItemName As String, RootMenu As NativeMenu) As NativeMenu

        If RootMenu Is Nothing OrElse String.IsNullOrWhiteSpace(ItemName) Then
            Return Nothing
        End If

        If RootMenu.Items.Any(Function(it) it.Name.Equals(ItemName, StringComparison.OrdinalIgnoreCase)) Then
            Return RootMenu
        End If

        For Each sm In RootMenu.SubMenusList
            Dim found = FindMenuFromItem(ItemName, sm)
            If found IsNot Nothing Then Return found
        Next

        Return Nothing
    End Function
    Private Function IsValidString(s As String) As Boolean
        If String.IsNullOrWhiteSpace(s) Then Return False
        Return s.All(Function(c) Char.IsLetterOrDigit(c) OrElse c = "_"c OrElse c = " "c)
    End Function
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
    Private Function ParseValueWithClamp(rawValue As String, Optional Clamp As (Min As Single, Max As Single)? = Nothing) As Object
        Dim b As Boolean
        If Boolean.TryParse(rawValue, b) Then
            Return b
        End If

        Dim i As Integer
        If Integer.TryParse(rawValue, i) Then
            If Clamp.HasValue Then
                i = Tools.ClampInteger(i, CInt(Clamp.Value.Min), CInt(Clamp.Value.Max))
            End If
            Return i
        End If

        Dim f As Single
        If Single.TryParse(rawValue.Replace(",", "."),
                       Globalization.NumberStyles.Float,
                       Globalization.CultureInfo.InvariantCulture, f) Then
            If Clamp.HasValue Then
                f = Tools.ClampFloat(f, Clamp.Value.Min, Clamp.Value.Max)
            End If
            Return f
        End If

        Return rawValue
    End Function

    ' ===== Modalità modifica tasto =====
    Friend Shared _currentEditingItem As MenuItem = Nothing
    Private _clearHelpMessages As Boolean = True
    Friend Function ConvertKeyToChar(k As Keys) As Char?

        Dim shift As Boolean =
        NativeControls.IsKeyPressed(Keys.ShiftKey, False) OrElse
        NativeControls.IsKeyPressed(Keys.LShiftKey, False) OrElse
        NativeControls.IsKeyPressed(Keys.RShiftKey, False)

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
    Private Sub LogMenuError(message As String, Optional ex As Exception = Nothing)
        If LogFile IsNot Nothing Then
            If ex IsNot Nothing Then
                LogFile.WriteLog("[NativeMenu] " & message, ex)
            Else
                LogFile.WriteLog("[NativeMenu] " & message, True)
            End If
        Else
            Tools.WriteLog("[NativeMenu] " & message)
        End If
    End Sub

    Friend Class MenuValueChangedEventArgs
        Public ReadOnly Property Menu As NativeMenu
        Public ReadOnly Property ItemName As String
        Public ReadOnly Property Value As Object

        Friend Sub New(menu As NativeMenu, itemName As String, value As Object)
            Me.Menu = menu
            Me.ItemName = itemName
            Me.Value = value
        End Sub
    End Class
End Class