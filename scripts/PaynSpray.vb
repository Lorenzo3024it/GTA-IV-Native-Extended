Imports GTA
Imports GTA.Native.Function
Public Class NativePaynSpray
    Public Shared WriteOnly Property Enable As Boolean
        Set(value As Boolean)
            [Call]("SET_NO_RESPRAYS", Not value)
        End Set
    End Property
    Public Shared WriteOnly Property FreeResprays As Boolean
        Set(value As Boolean)
            [Call]("SET_FREE_RESPRAYS", value)
        End Set
    End Property
    Public Shared ReadOnly Property isAnyPaynSprayActive As Boolean
        Get
            Return [Call](Of Boolean)("IS_PAY_N_SPRAY_ACTIVE")
        End Get
    End Property

    Public Shared Function AllowCarToBeResprayed(Car As Vehicle, Optional Allow As Boolean = True) As Boolean
        Return [Call](Of Boolean)("SET_CAN_RESPRAY_CAR", Car, Allow)
    End Function
    Public Shared Function HasCarBeenResprayed(Car As Vehicle) As Boolean
        Return [Call](Of Boolean)("HAS_CAR_BEEN_RESPRAYED")
    End Function
    Public Shared Function HasResprayHappened() As Boolean
        Return [Call](Of Boolean)("HAS_RESPRAY_HAPPENED")
    End Function
End Class

'Public Shared Sub EnablePaynSprays(Optional Enable As Boolean = True)
'    [Call]("SET_NO_RESPRAYS", Not Enable)
'End Sub
'Public Shared Function isPaynSprayActive() As Boolean
'    Return [Call](Of Boolean)("IS_PAY_N_SPRAY_ACTIVE")
'End Function
'Public Shared Sub SetFreeResprays(Optional Free As Boolean = True)
'    [Call]("SET_FREE_RESPRAYS", Free)
'End Sub