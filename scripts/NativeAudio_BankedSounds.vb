Partial Public Structure NativeAudio
    Friend Structure AmbientBanks
        Private Sub New()
        End Sub

        Public Const Television As String = "SCRIPT_AMBIENT/TELEVISION"
        Public Const Wardrobe As String = "SCRIPT_AMBIENT/WARDROBE"
        Public Const CarWash As String = "SCRIPT_AMBIENT/CAR_WASH"
        Public Const Darts As String = "SCRIPT_AMBIENT/DARTS"
        Public Const EndCreditsSpeech As String = "SCRIPT_AMBIENT/END_CREDITS_SPEECH"
        Public Const PoolTable As String = "SCRIPT_AMBIENT/POOL_TABLE"
        Public Const CashRegister As String = "SCRIPT_AMBIENT/CASH_REGISTER"
        Public Const BowlingAlley As String = "SCRIPT_AMBIENT/BOWLING_ALLEY"
        Public Const Multiplayer As String = "SCRIPT_AMBIENT/MULTIPLAYER_SOUNDS"
        Public Const Binoculars As String = "SCRIPT_AMBIENT/BINOCULARS"
        Public Const TollBooth As String = "SCRIPT_AMBIENT/TOLLBOOTH"
    End Structure

    ' No mission-bank names were present in the supplied source.
    ' Add verified bank-path constants here as you catalogue them.
    Friend Structure AmbientBanks
        Private Sub New()
        End Sub
    End Structure

    ''' <summary>
    ''' Shared implementation for sounds that require one globally loaded audio bank.
    ''' The bank itself is managed by the corresponding AmbientSound or MissionSound class.
    ''' </summary>
    Public MustInherit Class BankedSound
        Implements IDisposable

        Public Enum State
            NotPlayed = 0
            Playing = 1
            Ended = 2
            Stopped = 3
        End Enum

        Private Enum PlaybackMode
            Frontend
            FromObject
            FromPed
            FromPosition
            FromVehicle
        End Enum

        Private _AttachedPed As GTA.Ped
        Private _AttachedVehicle As GTA.Vehicle
        Private _AttachedObject As GTA.Object
        Private _Position As GTA.Vector3
        Private _ID As Integer
        Private _PlaybackState As State = State.NotPlayed
        Private _ForceLoop As Boolean
        Private _HasTriggered As Boolean
        Private _PlaybackMode As PlaybackMode = PlaybackMode.Frontend
        Private _Disposed As Boolean
        Private ReadOnly _TickAction As Action
        Private ReadOnly _Name As String
        Private ReadOnly _BankName As String

        Protected Sub New(SoundName As String, BankName As String)
            If String.IsNullOrWhiteSpace(SoundName) Then
                Throw New ArgumentException("The sound name cannot be null or empty.", NameOf(SoundName))
            End If
            If String.IsNullOrWhiteSpace(BankName) Then
                Throw New ArgumentException("The audio bank name cannot be null or empty.", NameOf(BankName))
            End If

            _Name = SoundName
            _BankName = BankName
            _Position = GTA.Vector3.Zero
            _TickAction = AddressOf UpdatePlayback
        End Sub

        ''' <summary>Gets the internal sound name.</summary>
        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        ''' <summary>Gets the bank path associated with this sound.</summary>
        Public ReadOnly Property Bank As String
            Get
                Return _BankName
            End Get
        End Property

        ''' <summary>Gets the native sound ID, or 0 when no ID is allocated.</summary>
        Public ReadOnly Property Handle As Integer
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>Gets the current playback state, independently of the sound ID.</summary>
        Public ReadOnly Property PlaybackState As State
            Get
                Return _PlaybackState
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether playback should be forcibly restarted through TickHelper.
        ''' Set to False to let the current playback finish without restarting it.
        ''' </summary>
        Public Property ForceLoop As Boolean
            Get
                Return _ForceLoop
            End Get
            Set(value As Boolean)
                ThrowIfDisposed()
                _ForceLoop = value
            End Set
        End Property

        ''' <summary>Indicates whether a Trigger method succeeded since the last successful Play.</summary>
        Public ReadOnly Property HasTriggered As Boolean
            Get
                Return _HasTriggered
            End Get
        End Property

        ''' <summary>Gets the current sound position, following a valid attached entity when applicable.</summary>
        Public ReadOnly Property Position3D As GTA.Vector3
            Get
                If _AttachedVehicle IsNot Nothing Then
                    If _AttachedVehicle.Exists Then
                        _Position = _AttachedVehicle.Position
                    Else
                        _AttachedVehicle = Nothing
                    End If
                ElseIf _AttachedObject IsNot Nothing Then
                    If _AttachedObject.Exists Then
                        _Position = _AttachedObject.Position
                    Else
                        _AttachedObject = Nothing
                    End If
                ElseIf _AttachedPed IsNot Nothing Then
                    If _AttachedPed.Exists Then
                        _Position = _AttachedPed.Position
                    Else
                        _AttachedPed = Nothing
                    End If
                End If

                Return _Position
            End Get
        End Property

        Protected MustOverride Function PrepareBank(OverrideLoadedBank As Boolean) As Boolean
        Protected MustOverride Function IsAssociatedBankLoaded() As Boolean

        ''' <summary>Plays the sound as frontend audio.</summary>
        ''' <param name="ForceLoop">Forcibly restarts playback through TickHelper when it finishes.</param>
        ''' <param name="OverrideLoadedBank">Releases a different loaded bank and requests this sound's bank.</param>
        Public Sub Play(Optional ForceLoop As Boolean = False, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            ResetAttachments()
            _Position = GTA.Vector3.Zero
            _PlaybackMode = PlaybackMode.Frontend
            BeginPlayback(ForceLoop, OverrideLoadedBank)
        End Sub

        ''' <summary>Plays the sound attached to an object.</summary>
        ''' <param name="Object">The object to which the sound is attached.</param>
        ''' <param name="ForceLoop">Forcibly restarts playback through TickHelper when it finishes.</param>
        ''' <param name="OverrideLoadedBank">Releases a different loaded bank and requests this sound's bank.</param>
        Public Sub Play([Object] As GTA.Object, Optional ForceLoop As Boolean = False, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            If [Object] Is Nothing OrElse Not [Object].Exists Then
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Play: invalid object.")
                Return
            End If

            ResetAttachments()
            _AttachedObject = [Object]
            _Position = [Object].Position
            _PlaybackMode = PlaybackMode.FromObject
            BeginPlayback(ForceLoop, OverrideLoadedBank)
        End Sub

        ''' <summary>Plays the sound attached to a ped.</summary>
        ''' <param name="Ped">The ped to which the sound is attached.</param>
        ''' <param name="ForceLoop">Forcibly restarts playback through TickHelper when it finishes.</param>
        ''' <param name="OverrideLoadedBank">Releases a different loaded bank and requests this sound's bank.</param>
        Public Sub Play(Ped As GTA.Ped, Optional ForceLoop As Boolean = False, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            If Ped Is Nothing OrElse Not Ped.Exists Then
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Play: invalid ped.")
                Return
            End If

            ResetAttachments()
            _AttachedPed = Ped
            _Position = Ped.Position
            _PlaybackMode = PlaybackMode.FromPed
            BeginPlayback(ForceLoop, OverrideLoadedBank)
        End Sub

        ''' <summary>Plays the sound at a fixed world position.</summary>
        ''' <param name="Position">The world position at which to play the sound.</param>
        ''' <param name="ForceLoop">Forcibly restarts playback through TickHelper when it finishes.</param>
        ''' <param name="OverrideLoadedBank">Releases a different loaded bank and requests this sound's bank.</param>
        Public Sub Play(Position As GTA.Vector3, Optional ForceLoop As Boolean = False, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            ResetAttachments()
            _Position = Position
            _PlaybackMode = PlaybackMode.FromPosition
            BeginPlayback(ForceLoop, OverrideLoadedBank)
        End Sub

        ''' <summary>Plays the sound attached to a vehicle.</summary>
        ''' <param name="Vehicle">The vehicle to which the sound is attached.</param>
        ''' <param name="ForceLoop">Forcibly restarts playback through TickHelper when it finishes.</param>
        ''' <param name="OverrideLoadedBank">Releases a different loaded bank and requests this sound's bank.</param>
        Public Sub Play(Vehicle As GTA.Vehicle, Optional ForceLoop As Boolean = False, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            If Vehicle Is Nothing OrElse Not Vehicle.Exists Then
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Play: invalid vehicle.")
                Return
            End If

            ResetAttachments()
            _AttachedVehicle = Vehicle
            _Position = Vehicle.Position
            _PlaybackMode = PlaybackMode.FromVehicle
            BeginPlayback(ForceLoop, OverrideLoadedBank)
        End Sub

        ''' <summary>Triggers an audio event without allocating a managed sound ID.</summary>
        ''' <param name="OverrideLoadedBank">Releases a different loaded bank and requests this sound's bank.</param>
        Public Sub Trigger(Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            If Not PrepareBank(OverrideLoadedBank) Then Return

            Try
                [Call]("PLAY_AUDIO_EVENT", Name)
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Trigger.", ex)
            End Try
        End Sub

        ''' <summary>Triggers an audio event from an object without allocating a managed sound ID.</summary>
        Public Sub Trigger([Object] As GTA.Object, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            If [Object] Is Nothing OrElse Not [Object].Exists Then
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Trigger: invalid object.")
                Return
            End If
            If Not PrepareBank(OverrideLoadedBank) Then Return

            Try
                [Call]("PLAY_AUDIO_EVENT_FROM_OBJECT", Name, [Object])
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Trigger from object.", ex)
            End Try
        End Sub

        ''' <summary>Triggers an audio event from a ped without allocating a managed sound ID.</summary>
        Public Sub Trigger(Ped As GTA.Ped, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            If Ped Is Nothing OrElse Not Ped.Exists Then
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Trigger: invalid ped.")
                Return
            End If
            If Not PrepareBank(OverrideLoadedBank) Then Return

            Try
                [Call]("PLAY_AUDIO_EVENT_FROM_PED", Name, Ped)
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Trigger from ped.", ex)
            End Try
        End Sub

        ''' <summary>Triggers an audio event from a vehicle without allocating a managed sound ID.</summary>
        Public Sub Trigger(Vehicle As GTA.Vehicle, Optional OverrideLoadedBank As Boolean = False)
            ThrowIfDisposed()
            If Vehicle Is Nothing OrElse Not Vehicle.Exists Then
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Trigger: invalid vehicle.")
                Return
            End If
            If Not PrepareBank(OverrideLoadedBank) Then Return

            Try
                [Call]("PLAY_AUDIO_EVENT_FROM_VEHICLE", Name, Vehicle)
                _HasTriggered = True
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Trigger from vehicle.", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Stops managed playback and disables forced looping.
        ''' Optionally releases the native sound ID. The shared audio bank is not released here.
        ''' </summary>
        Public Sub [Stop](Optional Release As Boolean = False)
            If _Disposed Then Return
            _ForceLoop = False
            TickHelper.Remove_Internal(_TickAction)

            If _ID <> 0 Then
                Try
                    [Call]("STOP_SOUND", _ID)
                Catch ex As Exception
                    Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Stop.", ex)
                End Try
            End If

            _PlaybackState = State.Stopped
            If Release Then ReleaseCurrentSoundID()
        End Sub

        ''' <summary>
        ''' Stops playback if necessary and releases this instance's native sound ID.
        ''' The shared audio bank remains loaded until explicitly released or overridden.
        ''' </summary>
        Public Sub Release()
            If _Disposed Then Return
            _ForceLoop = False
            TickHelper.Remove_Internal(_TickAction)

            If _ID <> 0 AndAlso _PlaybackState = State.Playing Then
                Try
                    [Call]("STOP_SOUND", _ID)
                Catch ex As Exception
                    Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".Release while stopping sound.", ex)
                End Try
                _PlaybackState = State.Stopped
            End If

            ReleaseCurrentSoundID()
        End Sub

        ''' <summary>Releases the managed sound ID and unregisters this instance from TickHelper.</summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            If _Disposed Then Return
            [Stop](True)
            If _ID <> 0 Then Return
            _Disposed = True
            GC.SuppressFinalize(Me)
        End Sub

        Private Sub BeginPlayback(ForceLoop As Boolean, OverrideLoadedBank As Boolean)
            _ForceLoop = ForceLoop

            If _ID <> 0 Then
                If Not StopAndReleaseCurrentPlayback() Then Return
            End If

            If Not PrepareBank(OverrideLoadedBank) Then
                _ForceLoop = False
                Return
            End If

            If Not StartPlayback() Then
                _ForceLoop = False
            End If
        End Sub

        Private Function StartPlayback() As Boolean
            If _ID <> 0 Then
                If Not StopAndReleaseCurrentPlayback() Then Return False
            End If

            ' A loop must not silently override a bank that another instance has selected.
            If Not IsAssociatedBankLoaded() Then Return False

            If Not CanReplayCurrentMode() Then
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".StartPlayback: attached entity no longer exists.")
                _PlaybackState = State.Stopped
                Return False
            End If

            Try
                Dim NewID As Integer = [Call](Of Integer)("GET_SOUND_ID")
                If NewID = 0 Then
                    Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".StartPlayback: GET_SOUND_ID returned 0.")
                    Return False
                End If

                _ID = NewID
                Select Case _PlaybackMode
                    Case PlaybackMode.Frontend
                        [Call]("PLAY_SOUND_FRONTEND", _ID, Name)
                    Case PlaybackMode.FromObject
                        [Call]("PLAY_SOUND_FROM_OBJECT", _ID, Name, _AttachedObject)
                    Case PlaybackMode.FromPed
                        [Call]("PLAY_SOUND_FROM_PED", _ID, Name, _AttachedPed)
                    Case PlaybackMode.FromPosition
                        [Call]("PLAY_SOUND_FROM_POSITION", _ID, Name, _Position.X, _Position.Y, _Position.Z)
                    Case PlaybackMode.FromVehicle
                        [Call]("PLAY_SOUND_FROM_VEHICLE", _ID, Name, _AttachedVehicle)
                End Select

                _PlaybackState = State.Playing
                _HasTriggered = False
                TickHelper.Remove_Internal(_TickAction)
                TickHelper.Add_Internal(_TickAction)
                Return True
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".StartPlayback.", ex)
                If _ID <> 0 Then ReleaseCurrentSoundID()
                _PlaybackState = State.Stopped
                Return False
            End Try
        End Function

        Private Function StopAndReleaseCurrentPlayback() As Boolean
            If _ID = 0 Then Return True
            Dim CurrentID As Integer = _ID

            Try
                [Call]("STOP_SOUND", CurrentID)
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".StopAndReleaseCurrentPlayback while stopping previous playback.", ex)
            End Try

            _PlaybackState = State.Stopped
            TickHelper.Remove_Internal(_TickAction)

            Try
                [Call]("RELEASE_SOUND_ID", CurrentID)
                _ID = 0
                Return True
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".StopAndReleaseCurrentPlayback while releasing previous sound ID.", ex)
                Return False
            End Try
        End Function

        Private Function ReleaseCurrentSoundID() As Boolean
            If _ID = 0 Then Return True
            Try
                [Call]("RELEASE_SOUND_ID", _ID)
                _ID = 0
                Return True
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".ReleaseCurrentSoundID.", ex)
                Return False
            End Try
        End Function

        Private Sub UpdatePlayback()
            If _ID = 0 OrElse _PlaybackState <> State.Playing Then
                TickHelper.Remove_Internal(_TickAction)
                Return
            End If

            Try
                If _ForceLoop AndAlso Not CanReplayCurrentMode() Then
                    Tools.WriteLog("Info in " & Me.GetType().Name & " " & Name & ".UpdatePlayback: attached entity no longer exists; stopping loop.")
                    [Stop](True)
                    Return
                End If

                If Not [Call](Of Boolean)("HAS_SOUND_FINISHED", _ID) Then Return
                If Not ReleaseCurrentSoundID() Then Return

                _PlaybackState = State.Ended
                If Not _ForceLoop Then
                    TickHelper.Remove_Internal(_TickAction)
                    Return
                End If

                If Not CanReplayCurrentMode() OrElse Not IsAssociatedBankLoaded() Then
                    _ForceLoop = False
                    TickHelper.Remove_Internal(_TickAction)
                    Return
                End If

                If Not StartPlayback() Then
                    _ForceLoop = False
                    _PlaybackState = State.Ended
                    TickHelper.Remove_Internal(_TickAction)
                End If
            Catch ex As Exception
                Tools.WriteLog("Error in " & Me.GetType().Name & " " & Name & ".UpdatePlayback.", ex)
            End Try
        End Sub

        Private Function CanReplayCurrentMode() As Boolean
            Select Case _PlaybackMode
                Case PlaybackMode.Frontend, PlaybackMode.FromPosition
                    Return True
                Case PlaybackMode.FromObject
                    Return _AttachedObject IsNot Nothing AndAlso _AttachedObject.Exists
                Case PlaybackMode.FromPed
                    Return _AttachedPed IsNot Nothing AndAlso _AttachedPed.Exists
                Case PlaybackMode.FromVehicle
                    Return _AttachedVehicle IsNot Nothing AndAlso _AttachedVehicle.Exists
                Case Else
                    Return False
            End Select
        End Function

        Private Sub ResetAttachments()
            _AttachedPed = Nothing
            _AttachedVehicle = Nothing
            _AttachedObject = Nothing
        End Sub

        Private Sub ThrowIfDisposed()
            If _Disposed Then Throw New ObjectDisposedException(Me.GetType().FullName)
        End Sub
    End Class

    ''' <summary>
    ''' Plays or triggers sounds that require an ambient audio bank.
    ''' Only one ambient bank is tracked as loaded by this class at a time.
    ''' </summary>
    Public NotInheritable Class AmbientSound
        Inherits BankedSoundBase

        Private Shared _LoadedBank As String

        ''' <summary>Catalog of ambient sound names known by this library.</summary>
        Public NotInheritable Class Sounds
            Private Sub New()
            End Sub

            Public Const TelevisionTurnOn As String = "TELEVISION_TURN_ON"
            Public Const TelevisionTurnOff As String = "TELEVISION_TURN_OFF"

            ' Add verified sound constants here, then add their bank mapping in ResolveBankRequired.
        End Class

        ''' <summary>Creates an ambient sound from the built-in sound catalog.</summary>
        Public Sub New(SoundName As String)
            MyBase.New(SoundName, ResolveBankRequired(SoundName))
        End Sub

        ''' <summary>Creates an ambient sound with an explicit bank path for custom or not-yet-catalogued sounds.</summary>
        Public Sub New(SoundName As String, AmbientBank As String)
            MyBase.New(SoundName, AmbientBank)
        End Sub

        Protected Overrides Function PrepareBank(OverrideLoadedBank As Boolean) As Boolean
            Return EnsureAmbientBank(Bank, OverrideLoadedBank)
        End Function

        Protected Overrides Function IsAssociatedBankLoaded() As Boolean
            Return String.Equals(_LoadedBank, Bank, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>
        ''' Releases the currently tracked ambient bank. Active sounds using that bank may be affected.
        ''' </summary>
        Public Shared Sub ReleaseLoadedBank()
            ReleaseLoadedBankInternal()
        End Sub

        Private Shared Function EnsureAmbientBank(BankName As String, OverrideLoadedBank As Boolean) As Boolean
            If String.Equals(_LoadedBank, BankName, StringComparison.OrdinalIgnoreCase) Then Return True

            If Not String.IsNullOrEmpty(_LoadedBank) Then
                If Not OverrideLoadedBank Then
                    Tools.WriteLog("Error in AmbientSound: bank " & BankName & " is not loaded. Current bank is " & _LoadedBank & "; set OverrideLoadedBank=True to replace it.")
                    Return False
                End If

                If Not ReleaseLoadedBankInternal() Then Return False
            End If

            If Not NativeAudio.RequestAmbientBank(BankName) Then
                Tools.WriteLog("Error in AmbientSound: failed to request ambient bank " & BankName & ".")
                Return False
            End If

            _LoadedBank = BankName
            Return True
        End Function

        Private Shared Function ReleaseLoadedBankInternal() As Boolean
            If String.IsNullOrEmpty(_LoadedBank) Then Return True
            If Not NativeAudio.ReleaseAmbientBank() Then Return False
            _LoadedBank = Nothing
            Return True
        End Function

        Private Shared Function ResolveBankRequired(SoundName As String) As String
            If String.IsNullOrWhiteSpace(SoundName) Then
                Throw New ArgumentException("The sound name cannot be null or empty.", NameOf(SoundName))
            End If

            Select Case SoundName
                Case Sounds.TelevisionTurnOn, Sounds.TelevisionTurnOff
                    Return NativeAudio.AmbientBanks.Television
                Case Else
                    Throw New ArgumentException("The ambient sound is not in the catalog. Add a bank mapping or use the constructor that accepts an explicit bank path.", NameOf(SoundName))
            End Select
        End Function
    End Class

    ''' <summary>
    ''' Plays or triggers sounds that require a mission audio bank.
    ''' Only one mission bank is tracked as loaded by this class at a time.
    ''' </summary>
    Public NotInheritable Class MissionSound
        Inherits BankedSoundBase

        Private Shared _LoadedBank As String

        ''' <summary>Catalog of mission sound names known by this library.</summary>
        Public NotInheritable Class Sounds
            Private Sub New()
            End Sub

            ' No mission sound identifiers were present in the supplied source.
            ' Add verified sound constants here, then add their bank mapping in ResolveBankRequired.
        End Class

        ''' <summary>Creates a mission sound from the built-in sound catalog.</summary>
        Public Sub New(SoundName As String)
            MyBase.New(SoundName, ResolveBankRequired(SoundName))
        End Sub

        ''' <summary>Creates a mission sound with an explicit bank path for custom or not-yet-catalogued sounds.</summary>
        Public Sub New(SoundName As String, MissionBank As String)
            MyBase.New(SoundName, MissionBank)
        End Sub

        Protected Overrides Function PrepareBank(OverrideLoadedBank As Boolean) As Boolean
            Return EnsureMissionBank(Bank, OverrideLoadedBank)
        End Function

        Protected Overrides Function IsAssociatedBankLoaded() As Boolean
            Return String.Equals(_LoadedBank, Bank, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>
        ''' Releases the currently tracked mission bank. Active sounds using that bank may be affected.
        ''' </summary>
        Public Shared Sub ReleaseLoadedBank()
            ReleaseLoadedBankInternal()
        End Sub

        Private Shared Function EnsureMissionBank(BankName As String, OverrideLoadedBank As Boolean) As Boolean
            If String.Equals(_LoadedBank, BankName, StringComparison.OrdinalIgnoreCase) Then Return True

            If Not String.IsNullOrEmpty(_LoadedBank) Then
                If Not OverrideLoadedBank Then
                    Tools.WriteLog("Error in MissionSound: bank " & BankName & " is not loaded. Current bank is " & _LoadedBank & "; set OverrideLoadedBank=True to replace it.")
                    Return False
                End If

                If Not ReleaseLoadedBankInternal() Then Return False
            End If

            If Not NativeAudio.RequestMissionAudioBank(BankName) Then
                Tools.WriteLog("Error in MissionSound: failed to request mission bank " & BankName & ".")
                Return False
            End If

            _LoadedBank = BankName
            Return True
        End Function

        Private Shared Function ReleaseLoadedBankInternal() As Boolean
            If String.IsNullOrEmpty(_LoadedBank) Then Return True
            If Not NativeAudio.ReleaseMissionAudioBank() Then Return False
            _LoadedBank = Nothing
            Return True
        End Function

        Private Shared Function ResolveBankRequired(SoundName As String) As String
            If String.IsNullOrWhiteSpace(SoundName) Then
                Throw New ArgumentException("The sound name cannot be null or empty.", NameOf(SoundName))
            End If

            ' Add Case entries here when the mission sound catalog and bank paths have been verified.
            Select Case SoundName
                Case Else
                    Throw New ArgumentException("The mission sound is not in the catalog. Add a bank mapping or use the constructor that accepts an explicit bank path.", NameOf(SoundName))
            End Select
        End Function
    End Class

End Class
