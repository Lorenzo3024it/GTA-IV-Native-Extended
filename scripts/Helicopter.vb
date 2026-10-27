Imports GTA
Imports GTA.Native.Function

Public NotInheritable Class NativeHelicopter
    Inherits NativeVehicle
    Private Sub New() : End Sub
    Public Shared Sub ForceBladesFullSpeed(Heli As Vehicle)
        Try
            If Heli IsNot Nothing AndAlso Heli.Exists Then
                [Call]("SET_HELI_BLADES_FULL_SPEED", Heli)
            Else
                Tools.WriteLog("Error in NativeHelicopters.ForceBladesFullSpeed: heli does't exists or isn't valid.")
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeHelicopters.ForceBladesFullSpeed.", ex)
        End Try
    End Sub
    Public Shared Sub ForceEngineON_TBoGT(Heli As Vehicle, Optional Enabled As Boolean = True)
        Try
            If Heli IsNot Nothing AndAlso Heli.Exists Then
                [Call]("SET_HELI_FORCE_ENGINE_ON", Heli, Enabled)
            Else
                Tools.WriteLog("Error in NativeHelicopters.ForceEngineON: heli does't exists or isn't valid.")
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeHelicopters.ForceEngineON.", ex)
        End Try
    End Sub
    Public Shared Function IsPartBroken(Heli As Vehicle, Part As HeliParts, Optional IncludeDetached As Boolean = True, Optional IncludeDamaged As Boolean = True) As Boolean
        Try
            If Heli IsNot Nothing AndAlso Heli.Exists Then
                Return [Call](Of Boolean)("IS_HELI_PART_BROKEN", Heli, Part, IncludeDetached, IncludeDamaged)
            Else
                Tools.WriteLog("Error in NativeHelicopters.IsPartBroken: heli does't exists or isn't valid.")
                Return False
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeHelicopters.IsPartBroken.", ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Untested. Used only in TBoGT SCO scripts.
    ''' </summary>
    Public Shared Sub SetStabilizer(Heli As Vehicle, Optional Enabled As Boolean = True)
        Try
            If Heli IsNot Nothing AndAlso Heli.Exists Then
                [Call]("SET_HELI_STABILISER", Heli, Enabled)
            Else
                Tools.WriteLog("Error in NativeHelicopters.SetStabilizer: heli does't exists or isn't valid.")
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeHelicopters.SetStabilizer.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Untested. Used only in TBoGT SCO scripts.
    ''' </summary>
    Public Shared Sub SkipStartupAudio(Heli As Vehicle, Optional Enabled As Boolean = True)
        Try
            If Heli IsNot Nothing AndAlso Heli.Exists Then
                [Call]("HELI_AUDIO_SHOULD_SKIP_STARTUP", Heli, Enabled)
            Else
                Tools.WriteLog("Error in NativeHelicopters.SkipStartupAudio: heli does't exists or isn't valid.")
            End If
        Catch ex As Exception
            Tools.WriteLog("Error in NativeHelicopters.SkipStartupAudio.", ex)
        End Try
    End Sub
    Public Enum HeliParts
        MainRotor = 1
        TailRotor = 0
    End Enum
End Class
