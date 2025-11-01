Imports GTA
Imports GTA.Native.Function
Public Class NativePhone
    Public Shared WriteOnly Property SleepMode As Boolean
        Set(value As Boolean)
            [Call]("SET_SLEEP_MODE_ACTIVE", value)
        End Set
    End Property
    Public Shared ReadOnly Property isPhoneCallOnGoing As Boolean
        Get
            Return [Call](Of Boolean)("IS_MOBILE_PHONE_CALL_ONGOING")
        End Get
    End Property


    Public Shared Function GetMobilePhoneID() As Integer
        Dim pointer As Native.Pointer = New Native.Pointer(GetType(Integer))
        [Call]("GET_MOBILE_PHONE_RENDER_ID", pointer)
        Return pointer.Value
    End Function

    Public Shared Sub StartCustomMobilePhoneRinging(RingtoneID As Ringtones)
        [Call]("START_CUSTOM_MOBILE_PHONE_RINGING", CInt(RingtoneID))
    End Sub
    'Public Shared Sub DestroyMobilePhone()
    '    [Call]("DESTROY_MOBILE_PHONE")
    'End Sub
    'public public shared Sub CreateMobilePhone()
    '    [Call]("CREATE_MOBILE_PHONE")
    'End Sub 'testare parametri aggiuntivi
    'Public Shared Sub ActivateSleepMode(Optional Enable As Boolean = True)
    '    [Call]("SET_SLEEP_MODE_ACTIVE", Enable)
    'End Sub
    'public public shared Sub StartMobilePhoneCall(Ped As Ped)
    '    [Call]("START_MOBILE_PHONE_CALL")
    'End Sub 'testare con parametri
    'public public shared Sub StartMobilePhoneCalling(Ped As Ped)
    '    [Call]("START_MOBILE_PHONE_CALLING")
    'End Sub 'testare con parametri
    'Public Shared Sub StartRinging()
    '    [Call]("START_MOBILE_PHONE_RINGING")
    'End Sub
    'Public Shared Sub StopRinging()
    '    [Call]("STOP_MOBILE_PHONE_RINGING")
    'End Sub
    Public Shared Sub AddLineToMobilePhoneCall(p1 As Integer, p2 As Integer, p3 As Integer) 'p2 and p3 unknown
        [Call]("ADD_LINE_TO_MOBILE_PHONE_CALL", p1, p2, p3)
    End Sub
    Public Shared Sub UseMobilePhone(Ped As Ped, Optional Duration As Integer = -1)
        [Call]("TASK_USE_MOBILE_PHONE_TIMED", Ped, Duration)
    End Sub
    ''' <summary>
    ''' All available phone ringtones.
    ''' </summary>
    Public Enum Ringtones
        StandardRing = 0
        Ringing1 = 1
        Ringing2 = 2
        Countryside = 3
        Fox = 4
        HighSeas = 5
        Malfunction = 6
        Pager = 7
        Spy = 8
        Teeker = 9
        Text = 10
        CoolRoom = 11
        KatjasWaltz = 12
        Laidback = 13
        Solo = 14
        SwingIt = 15
        _109 = 16
        Bassmatic = 17
        CreditCheck = 18
        Drive = 19
        FunkInTime = 20
        GetDown = 21
        IntoSomething = 22
        MineUntilMonday = 23
        TakeThePain = 24
        TheOneForMe = 25
        Tonight = 26
        Beheading = 27
        Champagne = 28
        Diamonds = 29
        FlatLine = 30
        Hooker = 31
        Jet = 32
        Lesbians = 33
        MoneyCounter = 34
        OldBitch = 35
        StThomas = 37
        DragonBrain = 60
        ScienceOfCrime = 61
    End Enum

    'SET_MOBILE_RING_TYPE
    'SET_MOBILE_PHONE_SCALE
    'SET_MOBILE_PHONE_ROTATION
    'SET_MOBILE_PHONE_POSITION
    'CAN_PHONE_BE_SEEN_ON_SCREEN
    'CAN_RENDER_RADIOHUD_SPRITE_IN_MOBILE_PHONE
    'IS_MOBILE_PHONE_CALL_ONGOING
    'NEW_MOBILE_PHONE_CALL
    'START_MOBILE_PHONE_CALL
    'CHANGE_PLAYER_PHONE_MODEL (player index, integer --> model phone) Testato, non funziona?
End Class