Imports GTA
Imports GTA.Native.Function
Imports NativeExtended.NativeDraws

Public NotInheritable Class NativeGeneric
    Private Sub New() : End Sub
    Public Enum RaceCheckpointType
        NormalArrow = 2
        NormalEndRaceGrid = 3
        CircleArrow = 4
        CircleEndRaceGrid = 5
        Arrow = 6
        EndRaceGrid = 7
        Circle = 8
        BaseJump_TBoGT = 9
    End Enum
    '======================================================================================================================================================================
    'Brief / News Scrollbar
    '-----------------------
    ''' <summary>
    ''' Adds a string to previous briefs. Can optionally display a string from GXT entry.
    ''' </summary>
    ''' <param name="Text">The text (or GxtEntry) to add.</param>
    ''' <param name="IsGXT">Set to true to automatically get string from a GXT file.</param>
    ''' <param name="ShowUnderscores">If false, underscores inside string will be displayed as white spaces.</param>
    Public Shared Sub AddTextToBrief(Text As String, Optional IsGXT As Boolean = False, Optional ShowUnderscores As Boolean = True)
        ' Dim FinalString As String
        'If IsGXT Then FinalString = GXT.GetString(Text) Else FinalString = Text
        If IsGXT Then
            [Call]("ADD_STRING_WITH_THIS_TEXT_LABEL_TO_PREVIOUS_BRIEF", Text)
        Else
            If Not ShowUnderscores Then
                [Call]("ADD_TO_PREVIOUS_BRIEF", Text)
            Else
                [Call]("ADD_TO_PREVIOUS_BRIEF_WITH_UNDERSCORE", Text)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Adds a string to the news scrollbar. Can optionally display a string from GXT entry.
    ''' </summary>
    ''' <param name="Text">The text (or GxtEntry) to add.</param>
    ''' <param name="IsGXT">Set to true to automatically get string from a GXT file.</param>
    Public Shared Sub AddTextToNewsScrollbar(Text As String, Optional IsGXT As Boolean = False)
        Dim FinalString As String
        If IsGXT Then FinalString = GXT.GetString(Text) Else FinalString = Text
        [Call]("ADD_STRING_TO_NEWS_SCROLLBAR", FinalString)
    End Sub
    Public Shared Sub ClearBrief()
        [Call]("CLEAR_BRIEF")
    End Sub
    Public Shared Sub ClearNewsScrollbar()
        [Call]("CLEAR_NEWS_SCROLLBAR")
    End Sub
    '======================================================================================================================================================================
    ''' <summary>
    ''' Creates a race checkpoint (native). Returns native handle requested for DeleteRaceCheckpoint void.
    ''' If you create many checkpoints without relase previous one the game will stop rendering new checkpoints.
    ''' </summary>
    ''' <param name="Type">Type from enum RaceCheckpointType.</param>
    ''' <param name="Unknown">Not fully tested, should be concerne arrow direction.</param>
    ''' <returns></returns>
    Public Shared Function CreateRaceCheckpoint(Coordinates As Vector3, Type As RaceCheckpointType, Optional Unknown As Vector3 = Nothing, Optional ScaleMultiplier As Single = 1.0F) As Integer
        If Unknown = Nothing Then Unknown = Vector3.Zero
        Return [Call](Of Integer)("CREATE_CHECKPOINT", Type, Coordinates.X, Coordinates.Y, Coordinates.Z, Unknown.X, Unknown.Y, Unknown.Z, ScaleMultiplier)
    End Function
    ''' <summary>
    ''' Removes a race checkpoint.
    ''' </summary>
    ''' <param name="CheckpointHandle">Native handle returned from CreateRaceCheckpoint void.</param>
    Public Shared Sub DeleteRaceCheckpoint(CheckpointHandle As Integer)
        If CheckpointHandle <= 0 Then
            Tools.WriteLog("Error in NativeGeneric.DeleteRaceCheckpoint: handle is not valid.")
            Return
        End If
        [Call]("DELETE_CHECKPOINT", CheckpointHandle)
    End Sub
    ''' <summary>
    ''' Disables pause menu.. You must call TickHelper.ProcessAll in a tick.
    ''' Return value can be unaccurate.
    ''' </summary>
    Public Shared Property DisablePauseMenu As Boolean
        Get
            Return _isForcingDisabled
        End Get
        Set(value As Boolean)
            If value Then
                TickHelper.Add_Internal(AddressOf DisablePauseMenu_OnTick)
            Else
                TickHelper.Remove_Internal(AddressOf DisablePauseMenu_OnTick)
                _isForcingDisabled = False
            End If
        End Set
    End Property
    ''' <summary>
    ''' Draws a downward-pointing 3D arrow using DRAW_COLOURED_CYLINDER.
    ''' Needs TickHelper to set drawing duration.
    ''' </summary>
    ''' <param name="Coordinates">The world-space position.</param>
    ''' <param name="Color">The RGBA color.</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    ''' <param name="Unknown01">The native's fourth parameter.</param>
    ''' <param name="Unknown02">The native's fifth parameter.</param>
    Public Shared Sub DrawArrow(Coordinates As GTA.Vector3, Color As ColorRGBA, Optional Duration As Integer = 1, Optional Unknown01 As Single = 1.0F, Optional Unknown02 As Single = 1.0F)
        If Coordinates = Vector3.Zero Then
            Tools.WriteLog("Error in DrawArrow: coordinates are zero.")
            Return
        End If

        If Duration < 2 Then
            [Call]("DRAW_COLOURED_CYLINDER", Coordinates.X, Coordinates.Y, Coordinates.Z, Unknown01, Unknown02, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        Else
            DrawManager.DrawArrow(Coordinates, Color, Unknown01, Unknown02, Duration)
        End If
    End Sub
    ''' <summary>
    ''' Draws a 3D checkpoint. Needs TickHelper to set drawing duration.
    ''' If Color is Nothing, the native DRAW_SPHERE is used instead.
    ''' </summary>
    ''' <param name="Coordinates">The world-space position.</param>
    ''' <param name="Radius">The checkpoint radius.</param>
    ''' <param name="Color">If nothing, DRAW_SPHERE native will be used (purple checkpoint Vice City like).</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    Public Shared Sub DrawCheckpoint(Coordinates As GTA.Vector3, Radius As Single, Optional Color As ColorRGBA = Nothing, Optional Duration As Integer = 1)
        If Coordinates = Vector3.Zero Then
            Tools.WriteLog("Error in DrawCheckpoint: coordinates are zero.")
            Return
        End If

        If Radius <= 0.0F Then
            Tools.WriteLog("Info for DrawCheckpoint: radius cannot be zero or negative. It will be set to 0.001F")
            Radius = 0.001F
        End If

        Dim UseSphere As Boolean = (Color = Nothing)
        If Duration < 2 Then
            If UseSphere Then
                [Call]("DRAW_SPHERE", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius)
            Else
                [Call]("DRAW_CHECKPOINT_WITH_ALPHA", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Color.Red, Color.Green, Color.Blue, Color.Alpha)
            End If
        Else
            DrawManager.DrawCheckpoint(Coordinates, Radius, Color, UseSphere, Duration)
        End If
    End Sub
    '======================================================================================================================================================================
    'Lights and Coronas
    '-------------------
    ''' <summary>
    ''' Draws a corona effect at the specified 3D position.
    ''' Needs TickHelper to set drawing duration.
    ''' Intensity scales the RGB channels and ranges from 0 to 100.
    ''' </summary>
    ''' <param name="Position">The world-space position.</param>
    ''' <param name="Radius">The corona radius. Must be positive.</param>
    ''' <param name="Color">The RGB corona color.</param>
    ''' <param name="Intensity">Intensity from 0 to 100. Suggested low values.</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    Public Shared Sub DrawCorona(Position As GTA.Vector3, Radius As Single, Color As ColorRGB, Optional Intensity As Single = 10.0F, Optional Duration As Integer = 1)
        If Radius <= 0.0F OrElse Intensity <= 0.0F Then Return
        Dim FinalIntensity As Single = Math.Max(0.0F, Math.Min(100.0F, Intensity))
        If Duration < 2 Then
            Dim Factor As Single = FinalIntensity / 100.0F
            [Call]("DRAW_CORONA", Position.X, Position.Y, Position.Z, Radius, 0, 0.0F, CInt(Color.Red * Factor), CInt(Color.Green * Factor), CInt(Color.Blue * Factor))
        Else
            DrawManager.DrawCorona(Position, Radius, Color, FinalIntensity, Duration)
        End If
    End Sub
    ''' <summary>
    ''' Draws a light with range and an optional corona.
    ''' Needs TickHelper to set drawing duration.
    ''' CoronaIntensity scales the RGB channels from 0 to 100.
    ''' </summary>
    ''' <param name="Position">The world-space position.</param>
    ''' <param name="Color">The RGB light color.</param>
    ''' <param name="Intensity">The light intensity.</param>
    ''' <param name="Range">The light range.</param>
    ''' <param name="CoronaRadius">Optional corona radius.</param>
    ''' <param name="CoronaIntensity">Optional corona intensity, from 0 to 100. Suggested low values.</param>
    ''' <param name="Duration">Display duration in milliseconds.</param>
    Public Shared Sub DrawLight(Position As GTA.Vector3, Color As ColorRGB, Intensity As Single, Range As Single, Optional CoronaRadius As Single = 0.0F, Optional CoronaIntensity As Single = 10.0F, Optional Duration As Integer = 1)
        If Duration < 2 Then
            [Call]("DRAW_LIGHT_WITH_RANGE", Position.X, Position.Y, Position.Z, Color.Red, Color.Green, Color.Blue, Intensity, Range)
            If CoronaRadius > 0.0F AndAlso CoronaIntensity > 0.0F Then
                Dim FinalIntensity As Single = Tools.ClampFloat(CoronaIntensity, 0.0F, 100.0F)
                Dim Factor As Single = FinalIntensity / 100.0F
                [Call]("DRAW_CORONA", Position.X, Position.Y, Position.Z, CoronaRadius, 0, 0.0F, CInt(Color.Red * Factor), CInt(Color.Green * Factor), CInt(Color.Blue * Factor))
            End If
        Else
            DrawManager.DrawLight(Position, Color, Intensity, Range, CoronaRadius, CoronaIntensity, Duration)
        End If
    End Sub
    '=========================================================================================================================================================================================================================================
    'Random Bool, Float and Int
    '----------------------------
    Public Function GetRandomBoolean(Optional SuccessPercentage As Integer = 50) As Boolean
        Dim Percentage As Integer = Tools.ClampInteger(SuccessPercentage, 0, 100)
        Return GetRandomFloat(0.0F, 1.0F) < (Percentage / 100.0F)
    End Function

    Public Shared Function GetRandomFloat(MinValue As Single, MaxValue As Single) As Single
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Single))
        [Call]("GENERATE_RANDOM_FLOAT_IN_RANGE", CSng(MinValue), CSng(MaxValue), pointer)
        Return pointer.Value
    End Function

    Public Shared Function GetRandomInteger(MinValue As Integer, MaxValue As Integer) As Integer
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call]("GENERATE_RANDOM_INT_IN_RANGE", CInt(MinValue), CInt(MaxValue), pointer)
        Return pointer.Value
    End Function
    '=========================================================================================================================================================================================================================================
    Public Shared ReadOnly Property IsGameInControlOfMusic As Boolean
        Get
            Return [Call](Of Boolean)("IS_GAME_IN_CONTROL_OF_MUSIC")
        End Get
    End Property
    ''' <summary>
    ''' Returns True if saving game help message ("SG_SAVING_PC") is on screen.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property IsSavingGame As Boolean
        Get
            Return GXT.IsPrintingHelp("SG_SAVING_PC")
        End Get
    End Property
    ''' <summary>
    ''' Pauses or unpauses game (not pause menu).
    ''' </summary>
    ''' <param name="Pause"></param>
    Public Shared Sub PauseGame(Optional Pause As Boolean = True)
        If Pause Then [Call]("PAUSE_GAME") Else [Call]("UNPAUSE_GAME")
    End Sub
    ' GENERATE_DIRECTIONS( l_U658, ref l_U805, ref l_U806 );

    Public Shared Sub SaveGame(Optional AutoSave As Boolean = True)
        If AutoSave Then [Call]("DO_AUTO_SAVE") Else [Call]("ACTIVATE_SAVE_MENU")
    End Sub

    Public NotInheritable Class Bullet
        Private Sub New() : End Sub
        Public Shared Sub FireSingleBullet(StartPosition As GTA.Vector3, EndPosition As GTA.Vector3, Damage As Integer)
            [Call]("FIRE_SINGLE_BULLET", StartPosition.X, StartPosition.Y, StartPosition.Z, EndPosition.X, EndPosition.Y, EndPosition.Z, Damage)
        End Sub
        ''' <summary>
        ''' Checks whether any bullet has entered area.
        ''' </summary>
        Public Shared Function IsBulletInArea(Coordsinates As Vector3, Radius As Single, Optional IncludePlayer As Boolean = True) As Boolean
            Try
                Return [Call](Of Boolean)("IS_BULLET_IN_AREA", Coordsinates.X, Coordsinates.Y, Coordsinates.Z, Radius, IncludePlayer)
            Catch
                Return False
            End Try
        End Function
        ''' <summary>
        ''' Checks whether any bullet has entered a 3D box area.
        ''' </summary>
        Public Shared Function IsBulletInBox(MinCoords As Vector3, MaxCoords As Vector3, Optional IncludePlayer As Boolean = True) As Boolean
            Try
                Return [Call](Of Boolean)("IS_BULLET_IN_BOX", MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z, IncludePlayer)
            Catch
                Return False
            End Try
        End Function
        ''' <summary>
        ''' Checks whether any projectile (grenade, rocket, etc.) is inside a 3D area.
        ''' </summary>
        Public Shared Function IsProjectileInArea(MinCoords As Vector3, MaxCoords As Vector3) As Boolean
            Try
                Return [Call](Of Boolean)("IS_PROJECTILE_IN_AREA", MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z)
            Catch
                Return False
            End Try
        End Function
        ''' <summary>
        ''' Checks whether a sniper bullet has passed through a 3D area.
        ''' </summary>
        Public Shared Function IsSniperBulletInArea(MinCoords As Vector3, MaxCoords As Vector3) As Boolean
            Try
                Return [Call](Of Boolean)("IS_SNIPER_BULLET_IN_AREA", MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z)
            Catch
                Return False
            End Try
        End Function
    End Class

    ''' <summary>
    ''' Untested.
    ''' </summary>
    Public NotInheritable Class CarRecording
        Private Sub New()
        End Sub
        Public Shared Function GetVehiclePosition(Car As Vehicle) As Single
            Return [Call](Of Single)("FIND_POSITION_IN_RECORDING", Car)
        End Function
        Public Shared Function GetTimePosition(Car As Vehicle) As Integer
            Return [Call](Of Integer)("FIND_TIME_POSITION_IN_RECORDING", Car)
        End Function
        Public Shared Function GetTotalDuration(Car As Vehicle) As Single
            Return [Call](Of Single)("GET_TOTAL_DURATION_OF_CAR_RECORDING", Car)
        End Function
        Public Shared Function IsLoaded(RRR_Index As Integer) As Boolean
            Return [Call](Of Boolean)("HAS_CAR_RECORDING_BEEN_LOADED", RRR_Index)
        End Function
        Public Shared Sub Request(RRR_Index As Integer)
            [Call]("REQUEST_CAR_RECORDING", RRR_Index)
        End Sub
        Public Shared Sub Remove(RRR_Index As Integer)
            [Call]("REMOVE_CAR_RECORDING", RRR_Index)
        End Sub
        Public Shared Sub SkipTime(Car As Vehicle, Time As Integer)
            [Call]("SKIP_TIME_IN_PLAYBACK_RECORDED_CAR", Car, Time)
        End Sub
        Public Shared Sub SkipToEndAndStop(Car As Vehicle)
            [Call]("SKIP_TO_END_AND_STOP_PLAYBACK_RECORDED_CAR", Car)
        End Sub
        Public Shared Sub Start(RRR_Index As Integer, Car As Vehicle)
            [Call]("START_PLAYBACK_RECORDED_CAR", Car, RRR_Index)
        End Sub
        Public Shared Sub Pause(Car As Vehicle)
            [Call]("PAUSE_PLAYBACK_RECORDED_CAR", Car)
        End Sub
        Public Shared Sub Unpause(Car As Vehicle)
            [Call]("UNPAUSE_PLAYBACK_RECORDED_CAR", Car)
        End Sub
        Public Shared Sub [Stop](Car As Vehicle)
            [Call]("STOP_PLAYBACK_RECORDED_CAR", Car)
        End Sub
    End Class

    Public NotInheritable Class Cheats
        Private Sub New() : End Sub
        ''' <summary>
        ''' Activates a specific cheat, optionally resetting cheat stat.
        ''' </summary>
        ''' <param name="CheatID">Cheat from CheatIndex enum.</param>
        ''' <param name="DontAffectCheatStat">If True cheat stat will be restored to last value.</param>
        ''' <param name="Silent">If True the activation won't trigger help message and sound.</param>
        Public Shared Sub Activate(CheatID As CheatIndex, Optional DontAffectCheatStat As Boolean = False, Optional Silent As Boolean = False)
            [Call]("ACTIVATE_CHEAT", CheatID)
            If Silent Then GXT.ClearHelp()
            If CheatID = CheatIndex.Money Then NativePlayer.Money += 10000
            If DontAffectCheatStat Then NativePlayer.Stats.DecrementInt(NativePlayer.Stats.StatINTs.IV_TimesCheated, 1)
        End Sub
        ''' <summary>
        ''' Tested but I noticed no difference. Used only in elizabeta3 SCO script.
        ''' </summary>
        Public Shared Sub ActivateHeliSpeedCheat(Heli As Vehicle, Optional Activate As Boolean = True)
            [Call]("ACTIVATE_HELI_SPEED_CHEAT", Heli, Activate)
        End Sub
        Public Shared Sub ClearCheatStat()
            NativePlayer.Stats.SetInt(NativePlayer.Stats.StatINTs.IV_TimesCheated, 0)
        End Sub
        Public Enum CheatIndex
            Weapon1 = 0
            Weapon2 = 1
            HealthArmourAndAmmo = 2
            IncreaseWantedLevel = 3
            ClearWantedLevel = 4
            ''' <summary>
            ''' Adds 10.000$. Doesn't work natively, but displays a dedicated GXT help message, so I added a workaround to make it work.
            ''' </summary>
            Money = 5
            ChangeWeather = 6
            Spawn_Annihilator = 7
            Spawn_NRG900 = 8
            Spawn_FibBuffalo = 9
            Spawn_Jetmax010
            Spawn_Comet = 11
            Spawn_Turismo = 12
            Spawn_Cognoscenti = 13
            Spawn_SuperGT = 14
            Spawn_Sanchez = 15
            EFLC_Unknown1
            EFLC_Unknown2
            EFLC_Unknown3
            EFLC_Unknown4
            EFLC_Unknown5
            EFLC_Unknown6
            EFLC_Unknown7
            EFLC_Unknown8
            EFLC_Unknown9
            EFLC_Unknown10
            EFLC_Unknown11
            EFLC_Unknown12
            EFLC_Unknown13
            EFLC_Unknown14
            EFLC_Unknown15
            EFLC_Unknown16
        End Enum
    End Class

    ''' <summary>
    ''' Untested. Provides native functions related to replay.
    ''' </summary>
    Public NotInheritable Class Replay
        Private Sub New() : End Sub
        ''' <summary>
        ''' Forces replay menu (without saving game). Seems that, after exiting replay menu, a new game will be started.
        ''' </summary>
        Public Shared Sub ActivateMenu()
            [Call]("ACTIVATE_REPLAY_MENU")
        End Sub
        ''' <summary>
        ''' Returns if game is saving replay. Combines IS_REPLAY_SAVING and IS_REPLAY_SYSTEM_SAVING native.
        ''' </summary>
        Public Shared ReadOnly Property IsSaving As Boolean
            Get
                Return [Call](Of Boolean)("IS_REPLAY_SAVING") OrElse [Call](Of Boolean)("IS_REPLAY_SYSTEM_SAVING")
            End Get
        End Property

        Public Shared ReadOnly Property IsSystemActive As Boolean
            Get
                Return [Call](Of Boolean)("IS_REPLAY_SYSTEM_ACTIVE")
            End Get
        End Property
    End Class

    '-------------------------------------------------------------------------------------------------------------------------
    Private Shared _isForcingDisabled As Boolean = False
    Friend Shared Sub DisablePauseMenu_OnTick()
        [Call]("DISABLE_PAUSE_MENU", 1)
        _isForcingDisabled = True
    End Sub
End Class
' Add / Remove Sphere
'''' <summary>
'''' Creates a sphere (checkpoint) with fixed position and orange color. Returns native handle requested for DeleteRaceCheckpoint void.
'''' </summary>
'Public Shared Function CreateSphere(Coordinates As Vector3, Radius As Single, Optional Unknown As Integer = 1) As Integer
'    Return [Call](Of Integer)("ADD_SPHERE", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Unknown)
'End Function
'''' <summary>
'''' Removes a sphere (checkpoint).
'''' </summary>
'''' <param name="SphereHandle"></param>
'Public Shared Sub DeleteSphere(SphereHandle As Integer)
'    If SphereHandle <= 0 Then
'        Tools.WriteLog("Error in NativeGeneric.DeleteSphere: handle is not valid.")
'        Return
'    End If
'    [Call]("REMOVE_SPHERE", SphereHandle)
'End Sub

'Public Shared ReadOnly Property IsOpeningAutosaveMenu As Boolean
'    Get
'        Return [Call](Of Boolean)("IS_AUTO_SAVE_IN_PROGRESS")
'    End Get
'End Property
'''' <summary>
'''' Untested. Should work only on online matches (not implemented yet in this library).
'''' </summary>
'Public Shared WriteOnly Property AllowPauseForStreaming As Boolean
'    Set(value As Boolean)
'        [Call]("ALLOW_GAME_TO_PAUSE_FOR_STREAMING", value)
'    End Set
'End Property
'Public Class DeletionList
'    Public Shared DeletionList_NonNative As List(Of NativeEntity) = New List(Of NativeEntity)
'    Public Property List As List(Of NativeEntity)
'    Public ReadOnly Property Count As Integer
'        Get
'            Return List.Count
'        End Get
'    End Property
'    Public Sub New()
'        List = New List(Of NativeEntity)
'    End Sub
'    Public Sub AddPed(Ped As Ped)
'        List.Add(New NativeEntity(Ped, EntityType.Ped))
'    End Sub
'    Public Sub AddCar(Car As Vehicle)
'        List.Add(New NativeEntity(Car, EntityType.Vehicle))
'    End Sub
'    Public Sub AddObject(Obj As GTA.Object)
'        List.Add(New NativeEntity(Obj, EntityType.Object))
'    End Sub
'    Public Sub AddCamera(Camera As GTA.Camera)
'        List.Add(New NativeEntity(Camera, EntityType.Camera))
'    End Sub
'    Public Sub Process(Optional ClearList As Boolean = True)
'        For i As Integer = List.Count - 1 To 0 Step -1
'            Dim entity As NativeEntity = List(i)
'            If entity.Exists Then
'                entity.Delete()
'            End If
'            List.RemoveAt(i)
'        Next
'    End Sub
'    Public Sub ClearList()
'        List.Clear()
'    End Sub
'    Public Sub RemoveEntity(Entity As NativeEntity, Optional RemoveOnlyFromList As Boolean = True)
'        If Entity.Exists Then Entity.Delete()
'        List.Remove(Entity)
'    End Sub
'    Public Sub RemoveEntitiesByType(Type As EntityType)
'        For i As Integer = List.Count - 1 To 0 Step -1
'            Dim entity As NativeEntity = List(i)
'            If entity.Type = Type AndAlso entity.Exists Then
'                entity.Delete()
'                List.RemoveAt(i)
'            End If
'        Next
'    End Sub
'    Public Function AnyExists() As Boolean
'        Return List.Any(Function(e) e.Exists)
'    End Function
'    Public Function getEntitiesByType(Type As EntityType) As List(Of NativeEntity)
'        Return List.Where(Function(e) e.Type = Type).ToList()
'    End Function
'    '------------------------------------------------------------------------------------------------------------------------
'    Public Shared Sub AddPed(Ped As Ped, Optional UseNonNativeMethod As Boolean = True)
'        If UseNonNativeMethod Then
'            DeletionList_NonNative.Add(New NativeEntity(Ped, EntityType.Ped))
'        Else
'            [Call]("ADD_PED_TO_MISSION_DELETION_LIST", Ped, True)
'        End If
'    End Sub
'    Public Shared Sub AddCar(Car As Vehicle, Optional UseNonNativeMethod As Boolean = True)
'        If UseNonNativeMethod Then
'            DeletionList_NonNative.Add(New NativeEntity(Car, EntityType.Vehicle))
'        Else
'            [Call]("ADD_CAR_TO_MISSION_DELETION_LIST", Car)
'        End If
'    End Sub
'    Public Shared Sub AddObject_NonNative(Obj As GTA.Object)
'        DeletionList_NonNative.Add(New NativeEntity(Obj, EntityType.Object))
'    End Sub
'    Public Shared Sub AddCamera_NonNative(Camera As GTA.Camera)
'        DeletionList_NonNative.Add(New NativeEntity(Camera, EntityType.Camera))
'    End Sub
'    Public Shared Sub ProcessDeletionList(Optional NonNative As Boolean = True)
'        [Call]("PROCESS_MISSION_DELETION_LIST")
'        If NonNative Then
'            For i As Integer = DeletionList_NonNative.Count - 1 To 0 Step -1
'                Dim entity As NativeEntity = DeletionList_NonNative(i)
'                If entity.Exists Then
'                    entity.Delete()
'                End If
'                DeletionList_NonNative.RemoveAt(i)
'            Next
'        End If
'    End Sub
'End Class