Imports GTA.Native.Function
Imports GTA
Public Class NativeScripts
    Public Shared Sub ActivateScriptPopulationZone()
        [Call]("ACTIVATE_SCRIPT_POPULATION_ZONE")
    End Sub
    Public Shared Function GetNumberOfInstancesOfScript(ScriptName As String) As Integer
        Return [Call](Of Integer)("GET_NUMBER_OF_INSTANCES_OF_STREAMED_SCRIPT", ScriptName)
    End Function
    Public Shared Function HasScriptLoaded(ScriptName As String) As Boolean
        Return [Call](Of Boolean)("HAS_SCRIPT_LOADED", ScriptName)
    End Function
    Public Shared Sub Relase(ScriptName As String)
        [Call]("MARK_SCRIPT_AS_NO_LONGER_NEEDED", ScriptName)
    End Sub
    Public Shared Sub Request(ScriptName As String)
        [Call]("REQUEST_SCRIPT", ScriptName)
    End Sub
    Public Shared Sub Start(ScriptName As String, Optional Unknown As Integer = 1024)
        [Call]("START_NEW_SCRIPT", ScriptName, Unknown)
    End Sub
    Public Shared Sub TerminateAllScriptWithThisName(ScriptName As String)
        [Call]("TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME", ScriptName)
    End Sub
End Class
