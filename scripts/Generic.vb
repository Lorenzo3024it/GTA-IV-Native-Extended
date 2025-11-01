Imports System
Imports System.Drawing
Imports System.IO
Imports System.Runtime.InteropServices
Imports GTA
Imports GTA.base
Imports GTA.Native.Function

Public Class NativeGeneric
    Public Shared ReadOnly Property isPauseMenuActive As Boolean
        Get
            Return [Call](Of Boolean)("IS_PAUSE_MENU_ACTIVE")
        End Get
    End Property
    Public Shared Sub DisablePauseMenu(Optional Disable As Boolean = True)
        [Call]("DISABLE_PAUSE_MENU", Disable)
    End Sub
    Public Shared Sub FireSingleBullet(StartPosition As GTA.Vector3, EndPosition As GTA.Vector3, Damage As Integer)
        [Call]("FIRE_SINGLE_BULLET", StartPosition.X, StartPosition.Y, StartPosition.Z, EndPosition.X, EndPosition.Y, EndPosition.Z, Damage)
    End Sub
    Public Shared Function GetRandomFloat(MinValue As Single, MaxValue As Single) As Single
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Single))
        [Call]("GENERATE_RANDOM_FLOAT_IN_RANGE", MinValue, MaxValue, pointer)
        Return pointer.Value
    End Function
    Public Shared Function GetRandomInteger(MinValue As Integer, MaxValue As Integer) As Integer
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call]("GENERATE_RANDOM_INT_IN_RANGE", MinValue, MaxValue, pointer)
        Return pointer.Value
    End Function
    Public Shared Sub PauseGame()
        [Call]("PAUSE_GAME")
    End Sub
    ' GENERATE_DIRECTIONS( l_U658, ref l_U805, ref l_U806 );

    Public Shared Sub SaveGame(Optional AutoSave As Boolean = True)
        If AutoSave Then [Call]("DO_AUTO_SAVE") Else [Call]("ACTIVATE_SAVE_MENU")
    End Sub
    Public Class Cheats
        Public Shared Sub Activate(CheatID As CheatIndex, Optional DontAffectCheatStatistic As Boolean = True)
            [Call]("ACTIVATE_CHEAT", CheatID)
            If DontAffectCheatStatistic Then NativePlayer.Stats.DecrementInt(NativePlayer.Stats.StatINTs.IV_TimesCheated, 1)
        End Sub
        ''' <summary>
        ''' Tested but I noticed no difference. Used only in elizabeta3 SCOCL script.
        ''' </summary>
        Public Shared Sub ActivateHeliSpeedCheat(Heli As Vehicle, Optional Activate As Boolean = True)
            [Call]("ACTIVATE_HELI_SPEED_CHEAT", Heli, Activate)
        End Sub
        Public Shared Sub ClearCheatStatistic()
            NativePlayer.Stats.SetInt(NativePlayer.Stats.StatINTs.IV_TimesCheated, 0)
        End Sub
        Public Enum CheatIndex
            Weapon1 = 0
            Weapon2 = 1
            HealthArmourAndAmmo = 2
            IncreaseWantedLevel = 3
            ClearWantedLevel = 4
            Money_NotWorking = 5
            ChangeWeather = 6
            Spawn_Annihilator = 7
            Spawn_NRG900 = 8
            Spawn_FIBBuffalo = 9
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
    Public Class DeletionList
        Public Shared DeletionList_NonNative As List(Of NativeEntity) = New List(Of NativeEntity)
        Public Property List As List(Of NativeEntity)
        Public ReadOnly Property Count As Integer
            Get
                Return List.Count
            End Get
        End Property
        Public Sub New()
            List = New List(Of NativeEntity)
        End Sub
        Public Sub AddPed(Ped As Ped)
            List.Add(New NativeEntity(Ped, EntityType.Ped))
        End Sub
        Public Sub AddCar(Car As Vehicle)
            List.Add(New NativeEntity(Car, EntityType.Vehicle))
        End Sub
        Public Sub AddObject(Obj As GTA.Object)
            List.Add(New NativeEntity(Obj, EntityType.Object))
        End Sub
        Public Sub AddCamera(Camera As GTA.Camera)
            List.Add(New NativeEntity(Camera, EntityType.Camera))
        End Sub
        Public Sub Process(Optional ClearList As Boolean = True)
            For i As Integer = List.Count - 1 To 0 Step -1
                Dim entity As NativeEntity = List(i)
                If entity.Exists Then
                    entity.Delete()
                End If
                List.RemoveAt(i)
            Next
        End Sub
        Public Sub ClearList()
            List.Clear()
        End Sub
        Public Sub RemoveEntity(Entity As NativeEntity, Optional RemoveOnlyFromList As Boolean = True)
            If Entity.Exists Then Entity.Delete()
            List.Remove(Entity)
        End Sub
        Public Sub RemoveEntitiesByType(Type As EntityType)
            For i As Integer = List.Count - 1 To 0 Step -1
                Dim entity As NativeEntity = List(i)
                If entity.Type = Type AndAlso entity.Exists Then
                    entity.Delete()
                    List.RemoveAt(i)
                End If
            Next
        End Sub
        Public Function AnyExists() As Boolean
            Return List.Any(Function(e) e.Exists)
        End Function
        Public Function getEntitiesByType(Type As EntityType) As List(Of NativeEntity)
            Return List.Where(Function(e) e.Type = Type).ToList()
        End Function
        '------------------------------------------------------------------------------------------------------------------------
        Public Shared Sub AddPed(Ped As Ped, Optional UseNonNativeMethod As Boolean = True)
            If UseNonNativeMethod Then
                DeletionList_NonNative.Add(New NativeEntity(Ped, EntityType.Ped))
            Else
                [Call]("ADD_PED_TO_MISSION_DELETION_LIST", Ped, True)
            End If
        End Sub
        Public Shared Sub AddCar(Car As Vehicle, Optional UseNonNativeMethod As Boolean = True)
            If UseNonNativeMethod Then
                DeletionList_NonNative.Add(New NativeEntity(Car, EntityType.Vehicle))
            Else
                [Call]("ADD_CAR_TO_MISSION_DELETION_LIST", Car)
            End If
        End Sub
        Public Shared Sub AddObject_NonNative(Obj As GTA.Object)
            DeletionList_NonNative.Add(New NativeEntity(Obj, EntityType.Object))
        End Sub
        Public Shared Sub AddCamera_NonNative(Camera As GTA.Camera)
            DeletionList_NonNative.Add(New NativeEntity(Camera, EntityType.Camera))
        End Sub
        Public Shared Sub ProcessDeletionList(Optional NonNative As Boolean = True)
            [Call]("PROCESS_MISSION_DELETION_LIST")
            If NonNative Then
                For i As Integer = DeletionList_NonNative.Count - 1 To 0 Step -1
                    Dim entity As NativeEntity = DeletionList_NonNative(i)
                    If entity.Exists Then
                        entity.Delete()
                    End If
                    DeletionList_NonNative.RemoveAt(i)
                Next
            End If
        End Sub
    End Class
End Class