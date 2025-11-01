Imports GTA

Public Enum EntityType
    Ped
    Vehicle
    [Object]
    Camera
End Enum

Public Class NativeEntity
    ' Tipo dell'entità
    Public ReadOnly Property Type As EntityType

    ' Handle originale (Ped, Vehicle, Object, Camera)
    Public ReadOnly Property Handle As Object

    ' Costruttore
    Public Sub New(handle As Object, type As EntityType)
        Me.Handle = handle
        Me.Type = type
    End Sub

    ' Verifica se l'entità esiste ancora
    Public Function Exists() As Boolean
        Try
            Select Case Type
                Case EntityType.Ped
                    Return CType(Handle, Ped).Exists()
                Case EntityType.Vehicle
                    Return CType(Handle, Vehicle).Exists()
                Case EntityType.Object
                    Return CType(Handle, GTA.Object).Exists()
                Case EntityType.Camera
                    Return CType(Handle, Camera) IsNot Nothing
                Case Else
                    Return False
            End Select
        Catch
            Return False
        End Try
    End Function
    Public Sub Delete()
        Select Case Type
            Case EntityType.Ped
                CType(Handle, Ped).Delete()
            Case EntityType.Vehicle
                CType(Handle, Vehicle).Delete()
            Case EntityType.Object
                CType(Handle, GTA.Object).Delete()
            Case EntityType.Camera
        End Select
    End Sub
End Class

