Imports GTA
Imports GTA.Native.Function
Public Class NativePhone
    Public Enum PhoneHudItems
        TextMessage = 1
        NewContact = 2
        AppointmentRing = 3
        NewAppointment = 4
        MissionReplay = 5
        PhotoSent = 6
        PhoneDisabled = 7
        RespectGained = 8
        RespectLost = 9
        InviteReceived = 10
    End Enum
    'Shared Sub CreateMobilePhone()
    '    [Call]("CREATE_MOBILE_PHONE")
    'End Sub 'testare parametri aggiuntivi
    Shared Sub DestroyMobilePhone()
        [Call]("DESTROY_MOBILE_PHONE")
    End Sub
    Shared Sub SetPhoneHudItem(Icon As PhoneHudItems, TextFromGXT As String, NumberInFrontOfString As Integer)
        [Call]("SET_PHONE_HUD_ITEM", CInt(Icon), TextFromGXT, NumberInFrontOfString)
    End Sub
    Shared Sub StartCustomMobilePhoneRinging(RingtoneID As Integer)
        [Call]("START_CUSTOM_MOBILE_PHONE_RINGING", RingtoneID)
    End Sub
    'Shared Sub StartMobilePhoneCall(Ped As Ped)
    '    [Call]("START_MOBILE_PHONE_CALL")
    'End Sub 'testare con parametri
    'Shared Sub StartMobilePhoneCalling(Ped As Ped)
    '    [Call]("START_MOBILE_PHONE_CALLING")
    'End Sub 'testare con parametri
    Shared Sub StartMobilePhoneRinging()
        [Call]("START_MOBILE_PHONE_RINGING")
    End Sub
    Shared Sub StopMobilePhoneRinging()
        [Call]("STOP_MOBILE_PHONE_RINGING")
    End Sub
    Shared Sub AddLineToMobilePhoneCall(p1 As Integer, p2 As Integer, p3 As Integer) 'p2 and p3 unknown
        [Call]("ADD_LINE_TO_MOBILE_PHONE_CALL", p1, p2, p3)
    End Sub
    Shared Sub UseMobilePhone(Ped As Ped, Duration As Integer) 'testare altri parametri
        [Call]("TASK_USE_MOBILE_PHONE_TIMED", Ped, Duration)
    End Sub
End Class