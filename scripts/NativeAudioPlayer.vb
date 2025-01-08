Imports GTA
Imports GTA.Native.Function

Public Class NativeAudioPlayer
    'Inherits Script
    Public Enum MissionComplete
        Sound1 = 6
        Sound2 = 7
        Sound3 = 10
        Sound4 = 11
        Sound5 = 15
        Sound6 = 18
        Sound7 = 24
        Sound8 = 25
        Sound9 = 27
        Sound10 = 28
        Sound11 = 33
        Sound12 = 35
        Sound13 = 42
        Sound14 = 43
        Sound15 = 51
        Sound16 = 52
        Sound17 = 53
        Sound18 = 54
        Sound19 = 55
        Sound20 = 56
        Sound21 = 57
        Sound22 = 58
        Sound23 = 59
        Sound24 = 60
        Sound25 = 61
        Sound26 = 62
        Sound27 = 63
        Sound28 = 64
        Sound29 = 65
        Sound30 = 66
        Sound31 = 67
        Sound32 = 68
        Sound33 = 69
        Sound34 = 71
    End Enum

    'Public Shared SoundList() As Int64
    Public SoundID As Integer
    Public SoundString As String

    '================================================================================================================
    Sub New(GameSound As String) 'GameSound As String
        'Me.Interval = 10

        SoundID = [Call](Of Integer)("GET_SOUND_ID")
        SoundString = GameSound
    End Sub
    Shared Sub PlayAudio(Sound As NativeAudioPlayer) 'As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        [Call]("PLAY_SOUND_FRONTED", Sound.SoundID, Sound.SoundString)
    End Sub
    Shared Sub PlayAudioFromObject(Sound As NativeAudioPlayer, Obj As GTA.Object) 'As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        [Call]("PLAY_SOUND_FROM_OBJECT", Sound.SoundID, Sound.SoundString, Obj)
    End Sub
    Shared Sub PlayAudioFromPed(Sound As NativeAudioPlayer, Ped As GTA.Ped) ' As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        [Call]("PLAY_SOUND_FROM_PED", Sound.SoundID, Sound.SoundString, Ped)
    End Sub
    Shared Sub PlayAudioFromVehicle(Sound As NativeAudioPlayer, Car As GTA.Vehicle) 'As Integer
        'SoundList(SoundID) = Native.Function.Call(Of Int64)("GET_SOUND_ID")
        [Call]("PLAY_SOUND_FROM_VEHICLE", Sound.SoundID, Sound.SoundString, Car)
    End Sub

    '------------------------------------------------------------------
    'Stop & Relase ID
    Shared Sub StopSound(Sound As NativeAudioPlayer, Optional RelaseSoundID As Boolean = False)
        [Call]("STOP_SOUND", Sound.SoundID)
        If RelaseSoundID Then [Call]("RELEASE_SOUND_ID", Sound.SoundID)
    End Sub
    Shared Sub RelaseSoundID(Sound As NativeAudioPlayer)
        [Call]("RELEASE_SOUND_ID", Sound.SoundID)
    End Sub
    '================================================================================================================
    'PLAY_AUDIO_EVENT - not stoppable, doesn't require SoundID
    Shared Sub TriggerAudio(GameSound As String)
        [Call]("PLAY_AUDIO_EVENT", GameSound)
    End Sub
    Shared Sub TriggerAudioFromObject(GameSound As String, Obj As GTA.Object)
        [Call]("PLAY_AUDIO_EVENT_FROM_OBJECT", GameSound, Obj)
    End Sub
    Shared Sub TriggerAudioFromPed(GameSound As String, Ped As GTA.Ped)
        [Call]("PLAY_AUDIO_EVENT_FROM_PED", GameSound, Ped)
    End Sub
    Shared Sub TriggerAudioFromVehicle(GameSound As String, Car As GTA.Vehicle)
        [Call]("PLAY_AUDIO_EVENT_FROM_VEHICLE", GameSound, Car)
    End Sub
    '================================================================================================================
    'Other Audio Native Calls
    Shared Sub TriggerMissionCompleteAudio(AudioID As MissionComplete)
        [Call]("TRIGGER_MISSION_COMPLETE_AUDIO", AudioID)
    End Sub
    '================================================================================================================
End Class
'Public Class GameSounds
'    Public Shared Test = New NativeAudioPlayer("TEST")
'End Class