Imports System.Runtime.Remoting.Metadata.W3cXsd2001
Imports GTA
Imports GTA.Native.Function
Imports NativeExtended.NativeDraws
Imports NativeExtended.Tools
Imports System.Windows

Public Class NativeDraws
    Public Structure ColorRGBA
        Private _r As Integer
        Private _g As Integer
        Private _b As Integer
        Private _a As Integer
        Public Property Red As Byte
            Get
                Return CByte(ClampInteger(_r, 0, 255))
            End Get
            Set(value As Byte)
                _r = value
            End Set
        End Property
        Public Property Green As Byte
            Get
                Return CByte(ClampInteger(_g, 0, 255))
            End Get
            Set(value As Byte)
                _g = value
            End Set
        End Property
        Public Property Blue As Byte
            Get
                Return CByte(ClampInteger(_b, 0, 255))
            End Get
            Set(value As Byte)
                _b = value
            End Set
        End Property
        Public Property Alpha As Byte
            Get
                Return CByte(ClampInteger(_a, 0, 255))
            End Get
            Set(value As Byte)
                _a = value
            End Set
        End Property
        Public Shared ReadOnly Property DefaultColor As ColorRGBA
            Get
                Return New ColorRGBA(0, 0, 0, 120)
            End Get
        End Property
        Public Shared ReadOnly Property White As ColorRGBA
            Get
                Return New ColorRGBA(255, 255, 255, 255)
            End Get
        End Property
        Public Sub New(R As Byte, G As Byte, B As Byte, A As Byte)
            _r = R
            _g = G
            _b = B
            _a = A
        End Sub
        Public Overrides Function ToString() As String
            Return $"Color RGBA: R={Red}, G={Green}, B={Blue}, A={Alpha}"
        End Function
        Public Function ToHex() As Integer
            Return (_a << 24) Or (_r << 16) Or (_g << 8) Or _b
        End Function
        Public Shared Function FromHex(ARGB As Integer) As ColorRGBA
            Dim a As Byte = CByte((ARGB >> 24) And &HFF)
            Dim r As Byte = CByte((ARGB >> 16) And &HFF)
            Dim g As Byte = CByte((ARGB >> 8) And &HFF)
            Dim b As Byte = CByte(ARGB And &HFF)
            Return New ColorRGBA(r, g, b, a)
        End Function
    End Structure
    Public Structure ColorRGB
        Private _r As Integer
        Private _g As Integer
        Private _b As Integer
        Private _a As Integer
        Public Property Red As Byte
            Get
                Return CByte(ClampInteger(_r, 0, 255))
            End Get
            Set(value As Byte)
                _r = value
            End Set
        End Property
        Public Property Green As Byte
            Get
                Return CByte(ClampInteger(_g, 0, 255))
            End Get
            Set(value As Byte)
                _g = value
            End Set
        End Property
        Public Property Blue As Byte
            Get
                Return CByte(ClampInteger(_b, 0, 255))
            End Get
            Set(value As Byte)
                _b = value
            End Set
        End Property
        Public Shared ReadOnly Property DefaultColor As ColorRGB
            Get
                Return New ColorRGB(0, 0, 0)
            End Get
        End Property
        Public Shared ReadOnly Property White As ColorRGB
            Get
                Return New ColorRGB(255, 255, 255)
            End Get
        End Property
        Public Sub New(R As Byte, G As Byte, B As Byte)
            _r = R
            _g = G
            _b = B
            _a = 255
        End Sub
        Public Overrides Function ToString() As String
            Return $"Color RGBA: R={Red}, G={Green}, B={Blue}"
        End Function
        Public Function ToHex() As Integer
            Return (_a << 24) Or (_r << 16) Or (_g << 8) Or _b
        End Function
        Public Shared Function FromHext(ARGB As Integer) As ColorRGBA
            Dim a As Byte = CByte((ARGB >> 24) And &HFF)
            Dim r As Byte = CByte((ARGB >> 16) And &HFF)
            Dim g As Byte = CByte((ARGB >> 8) And &HFF)
            Dim b As Byte = CByte(ARGB And &HFF)
            Return New ColorRGBA(r, g, b, a)
        End Function
    End Structure
    Public Structure TextBlips
        Public Const Destination1 As String = "~BLIP_0~"
        Public Const Destination2 As String = "~BLIP_1~"
        Public Const Destination3 As String = "~BLIP_2~"
        Public Const Objective1 As String = "~BLIP_3~"
        Public Const Objective2 As String = "~BLIP_4~"
        Public Const Objective3 As String = "~BLIP_5~"
        Public Const Player As String = "~BLIP_6~"
        Public Const North As String = "~BLIP_77~"
        Public Const Waypoint As String = "~BLIP_8~"
        Public Const Weapon_Pistol As String = "~BLIP_9~"
        Public Const Weapon_Shotgun As String = "~BLIP_10~"
        Public Const Weapon_SMG As String = "~BLIP_11~"
        Public Const Weapon_Rifle As String = "~BLIP_~12"
        Public Const Weapon_RocketLauncher As String = "~BLIP_13~"
        Public Const Weapon_Grenade As String = "~BLIP_14~"
        Public Const Weapon_Molotov As String = "~BLIP_15~"
        Public Const Weapon_Sniper As String = "~BLIP_16~"
        Public Const Weapon_BaseballBat As String = "~BLIP_17~"
        Public Const Weapon_Knife As String = "~BLIP_18~"
        Public Const Weapon_Health As String = "~BLIP_19~"
        Public Const Weapon_Armor As String = "~BLIP_20~"
        Public Const BurgerShot As String = "~BLIP_21~"
        Public Const CluckinBell As String = "~BLIP_22~"
        Public Const Vlad As String = "~BLIP_23~"
        Public Const Internet As String = "~BLIP_24~"
        Public Const Manny As String = "~BLIP_25~"
        Public Const LittleJacob As String = "~BLIP_26~"
        Public Const Roman As String = "~BLIP_27~"
        Public Const Faustin As String = "~BLIP_28~"
        Public Const Safehouse As String = "~BLIP_29~"
        Public Const Taxi As String = "~BLIP_30~"
        Public Const Bernie As String = "~BLIP_31~"
        Public Const Brucie As String = "~BLIP_32~"
        Public Const UnknownContact As String = "~BLIP_33~"
        Public Const Dwayne As String = "~BLIP_34~"
        Public Const Elizabeta As String = "~BLIP_35~"
        Public Const Gambetti As String = "~BLIP_36~"
        Public Const JimmyPegorino As String = "~BLIP_37~"
        Public Const Derrick As String = "~BLIP_38~"
        Public Const Francis As String = "~BLIP_39~"
        Public Const Gerry As String = "~BLIP_40~"
        Public Const Kate As String = "~BLIP_41~"
        Public Const Packie As String = "~BLIP_42~"
        Public Const PhilBell As String = "~BLIP_43~"
        Public Const PlayboyX As String = "~BLIP_44~"
        Public Const RayBoccino As String = "~BLIP_45~"
        Public Const EightPool As String = "~BLIP_46~"
        Public Const Bar As String = "~BLIP_47~"
        Public Const BoatTour As String = "~BLIP_48~"
        Public Const Bowling As String = "~BLIP_49~"
        Public Const ClothesShop As String = "~BLIP_50~"
        Public Const Club As String = "~BLIP_51~"
        Public Const Darts As String = "~BLIP_52~"
        Public Const DwayneRed As String = "~BLIP_53~"
        Public Const DateGirl As String = "~BLIP_54~"
        Public Const PlayboyXRed As String = "~BLIP_55~"
        Public Const Helitour As String = "~BLIP_56~"
        Public Const Restaurant As String = "~BLIP_57~"
        Public Const TrainStation As String = "~BLIP_58~"
        Public Const WeaponShop As String = "~BLIP_59~"
        Public Const PoliceStation As String = "~BLIP_60~"
        Public Const FireStation As String = "~BLIP_61~"
        Public Const Hospital As String = "~BLIP_62~"
        Public Const FriendMale As String = "~BLIP_63~"
        Public Const FriendFemale As String = "~BLIP_64~"
        Public Const RaceFinishLine As String = "~BLIP_65~"
        Public Const StripClub As String = "~BLIP_66~"
        Public Const ConsoleGame As String = "~BLIP_67~"
        Public Const CopCar As String = "~BLIP_68~"
        Public Const Dimitri As String = "~BLIP_69~"
        Public Const ComedyClub As String = "~BLIP_70~"
        Public Const CabaretClub As String = "~BLIP_71~"
        Public Const Ransom As String = "~BLIP_72~"
        Public Const CopHeli As String = "~BLIP_73~"
        Public Const Michelle As String = "~BLIP_74~"
        Public Const PayNSpray As String = "~BLIP_75~"
        Public Const Assassin As String = "~BLIP_76~"
        Public Const Revenge As String = "~BLIP_77~"
        Public Const Deal As String = "~BLIP_78~"
        Public Const Garage As String = "~BLIP_79~"
        Public Const Lawyer As String = "~BLIP_80~"
        Public Const Trophy As String = "~BLIP_81~"
        Public Const MultiplayerTutorial As String = "~BLIP_82~"
        Public Const Station3 As String = "~BLIP_83~"
        Public Const Station8 As String = "~BLIP_84~"
        Public Const StationA As String = "~BLIP_85~"
        Public Const StationB As String = "~BLIP_86~"
        Public Const StationC As String = "~BLIP_87~"
        Public Const StationE As String = "~BLIP_88~"
        Public Const StationJ As String = "~BLIP_89~"
        Public Const StationK As String = "~BLIP_90~"
        Public Const CarWash As String = "~BLIP_91~"
        Public Const UnitedLibertyPaper As String = "~BLIP_92~"
        Public Const ArrowAbove As String = "~BLIP_93~"
        Public Const ArrowBelow As String = "~BLIP_94~"
    End Structure
    Public Structure TextColors
        Public Const Red As String = "~r~"
        Public Const Green As String = "~g~"
        Public Const Blue As String = "~b~"
        Public Const Yellow As String = "~y~"
        Public Const Orange As String = "~o~"
        Public Const Purple As String = "~p~"
        Public Const White As String = "~w~"
        Public Const Gray As String = "~m~"
        Public Const Black As String = "~l~"
        Public Const LightGray As String = "~c~"
        'Public Const LightBlue As String = "~e~"
        Public Const LightPink As String = "~s~"

        ' Multiplayer
        Public Const MP_Orange As String = "~COL_NET_1~"
        Public Const MP_Purple As String = "~COL_NET_2~"
        Public Const MP_LightGreen As String = "~COL_NET_3~"
        Public Const MP_Red As String = "~COL_NET_4~"
        Public Const MP_Pink As String = "~COL_NET_5~"
        Public Const MP_Brown As String = "~COL_NET_6~"
        Public Const MP_Agua As String = "~COL_NET_7~"
        Public Const MP_VeryLightBlue As String = "~COL_NET_8~"
        Public Const MP_Yellow As String = "~COL_NET_9~"
        Public Const MP_AguaDark As String = "~COL_NET_10~"
        Public Const MP_Violet As String = "~COL_NET_11~"
        Public Const MP_Pink2 As String = "~COL_NET_12~"
        Public Const MP_LightOrange As String = "~COL_NET_13~"
        Public Const MP_Yellow2 As String = "~COL_NET_14~"
        Public Const MP_Yellow3 As String = "~COL_NET_15~"
        Public Const MP_Purple2 As String = "~COL_NET_16~"

        ' FX colors / special
        Public Const FX_Unknown02 As String = "~n~"
        Public Const FX_StopColorFxHere As String = "~s~"
        Public Const Fx_Highlighted As String = "~h~"
        Public Const Fx_String As String = "~a~"
        Public Const Fx_Number As String = "~1~"
        Public Const Fx_Subtitle As String = "~z~"
        Public Const RockstarLogo As String = "¬"
        '  Public Shared FivePointStar As String = "^"
        'Public Shared FivePointStarSmall As String = "*"
    End Structure
    Public Structure TextKeys
        Public Const Action As String = "~INPUT_PICKUP~"
        Public Const Pickup As String = "~INPUT_PICKUP~"
        Public Const Enter As String = "~INPUT_ENTER~"
        Public Const Jump As String = "~INPUT_JUMP~"
        Public Const Reload As String = "~INPUT_RELOAD~"
        Public Const MoveUp As String = "~INPUT_MOVE_UP~"
        Public Const MoveLeft As String = "~INPUT_MOVE_LEFT~"
        Public Const MoveDown As String = "~INPUT_MOVE_DOWN~"
        Public Const MoveRight As String = "~INPUT_MOVE_RIGHT~"
        Public Const TextChatUniversal As String = "~INPUT_TEXT_CHAT_UNIVERSAL~"
        Public Const TextChatResultsToggle As String = "~INPUT_TEXT_CHAT_RESULTS_TOGGLE~"
        Public Const TextChatY As String = "~INPUT_TEXT_CHAT_Y~"
        Public Const NextTrack As String = "~INPUT_NEXT_TRACK~"
        Public Const PrevTrack As String = "~INPUT_PREV_TRACK~"
        Public Const FrontendRefresh As String = "~INPUT_FRONTEND_REFRESH~"
        Public Const FrontendLockLobby As String = "~INPUT_FRONTEND_LOCK_LOBBY~"
        Public Const FrontendAccept As String = "~INPUT_FRONTEND_ACCEPT~"
        Public Const FrontendCancel As String = "~INPUT_FRONTEND_CANCEL~"
        Public Const FrontendLT As String = "~INPUT_FRONTEND_LT~"
        Public Const FrontendRT As String = "~INPUT_FRONTEND_RT~"
        Public Const FrontendLB As String = "~INPUT_FRONTEND_LB~"
        Public Const FrontendRB As String = "~INPUT_FRONTEND_RB~"
        Public Const FrontendPause As String = "~INPUT_FRONTEND_PAUSE~"
        Public Const FrontendModelScreen As String = "~INPUT_FRONTEND_MODEL_SCREEN~"
        Public Const FrontendLegend As String = "~INPUT_FRONTEND_LEGEND~"
        Public Const FrontendReplayScreenshot As String = "~INPUT_FRONTEND_REPLAY_SCREENSHOT~"
        Public Const FrontendDeleteFilter As String = "~INPUT_FRONTEND_DELETE_FILTER~"
        Public Const FrontendApply As String = "~INPUT_FRONTEND_APPLY~"
        Public Const FrontendVehicleToggle As String = "~INPUT_FRONTEND_VEHICLE_TOGGLE~"
        Public Const ReplaySaveToHdd As String = "~INPUT_REPLAY_SAVE_TO_HDD~"
        Public Const MouseUd As String = "~INPUT_MOUSE_UD~"
        Public Const FreeAim As String = "~INPUT_FREE_AIM~"
        Public Const VehicleHorn As String = "~INPUT_VEH_HORN~"
        Public Const KbUp As String = "~INPUT_KB_UP~"
        Public Const KbDown As String = "~INPUT_KB_DOWN~"
        Public Const KbLeft As String = "~INPUT_KB_LEFT~"
        Public Const KbRight As String = "~INPUT_KB_RIGHT~"
        Public Const Sprint As String = "~INPUT_SPRINT~"
        Public Const VehicleExit As String = "~INPUT_VEH_EXIT~"
        Public Const PhoneAccept As String = "~INPUT_PHONE_ACCEPT~"
        Public Const Attack As String = "~INPUT_ATTACK~"
        Public Const DropWeapon As String = "~INPUT_DROP_WEAPON~"
        Public Const VehicleHandbrake As String = "~INPUT_VEH_HANDBRAKE~"
        Public Const VehicleHeadlight As String = "~INPUT_VEH_HEADLIGHT~"
        Public Const MouseRmb As String = "~INPUT_MOUSE_RMB~"
        Public Const Aim As String = "~INPUT_AIM~"
        Public Const Cover As String = "~INPUT_COVER~"
        Public Const Duck As String = "~INPUT_DUCK~"
        Public Const FeReplayPreview As String = "~INPUT_FE_REPLAY_PREVIEW~"
        Public Const FeReplayToggletime As String = "~INPUT_FE_REPLAY_TOGGLETIME~"
        Public Const FeReplayToggletips As String = "~INPUT_FE_REPLAY_TOGGLETIPS~"
        Public Const FrontendAxisBack As String = "~INPUT_FRONTEND_AXIS_BACK~"
        Public Const FrontendAxisFwd As String = "~INPUT_FRONTEND_AXIS_FWD~"
        Public Const FrontendAxisLeft As String = "~INPUT_FRONTEND_AXIS_LEFT~"
        Public Const FrontendAxisRight As String = "~INPUT_FRONTEND_AXIS_RIGHT~"
        Public Const FrontendCopy As String = "~INPUT_FRONTEND_COPY~"
        Public Const FrontendCut As String = "~INPUT_FRONTEND_CUT~"
        Public Const FrontendF6 As String = "~INPUT_FRONTEND_F6~"
        Public Const FrontendMarkerDelete As String = "~INPUT_FRONTEND_MARKER_DELETE~"
        Public Const FrontendMinigame1 As String = "~INPUT_FRONTEND_MINIGAME_1~"
        Public Const FrontendMinigame2 As String = "~INPUT_FRONTEND_MINIGAME_2~"
        Public Const FrontendMinigame3 As String = "~INPUT_FRONTEND_MINIGAME_3~"
        Public Const FrontendMinigame4 As String = "~INPUT_FRONTEND_MINIGAME_4~"
        Public Const FrontendPaste As String = "~INPUT_FRONTEND_PASTE~"
        Public Const FrontendPlayerList As String = "~INPUT_FRONTEND_PLAYER_LIST~"
        Public Const FrontendReplayAdvance As String = "~INPUT_FRONTEND_REPLAY_ADVANCE~"
        Public Const FrontendReplayBack As String = "~INPUT_FRONTEND_REPLAY_BACK~"
        Public Const FrontendReplayCyclemarkerleft As String = "~INPUT_FRONTEND_REPLAY_CYCLEMARKERLEFT~"
        Public Const FrontendReplayCyclemarkerright As String = "~INPUT_FRONTEND_REPLAY_CYCLEMARKERRIGHT~"
        Public Const FrontendReplayFfwd As String = "~INPUT_FRONTEND_REPLAY_FFWD~"
        Public Const FrontendReplayHidehud As String = "~INPUT_FRONTEND_REPLAY_HIDEHUD~"
        Public Const FrontendReplayNewmarker As String = "~INPUT_FRONTEND_REPLAY_NEWMARKER~"
        Public Const FrontendReplayPause As String = "~INPUT_FRONTEND_REPLAY_PAUSE~"
        Public Const FrontendReplayRestart As String = "~INPUT_FRONTEND_REPLAY_RESTART~"
        Public Const FrontendReplayRewind As String = "~INPUT_FRONTEND_REPLAY_REWIND~"
        Public Const FrontendReplayShowhotkey As String = "~INPUT_FRONTEND_REPLAY_SHOWHOTKEY~"
        Public Const FrontendReplayTools As String = "~INPUT_FRONTEND_REPLAY_TOOLS~"
        Public Const FrontendX As String = "~INPUT_FRONTEND_X~"
        Public Const FrontendY As String = "~INPUT_FRONTEND_Y~"
        Public Const MeleeAttack1 As String = "~INPUT_MELEE_ATTACK1~"
        Public Const MeleeAttack2 As String = "~INPUT_MELEE_ATTACK2~"
        Public Const MeleeAttack3 As String = "~INPUT_MELEE_ATTACK3~"
        Public Const MeleeBlock As String = "~INPUT_MELEE_BLOCK~"
        Public Const MeleeKick As String = "~INPUT_MELEE_KICK~"
        Public Const MouseLmb As String = "~INPUT_MOUSE_LMB~"
        Public Const MouseWheelDown As String = "~INPUT_MOUSE_WHEEL_DOWN~"
        Public Const MouseWheelUp As String = "~INPUT_MOUSE_WHEEL_UP~"
        Public Const NextCamera As String = "~INPUT_NEXT_CAMERA~"
        Public Const NextWeapon As String = "~INPUT_NEXT_WEAPON~"
        Public Const PhoneCancel As String = "~INPUT_PHONE_CANCEL~"
        Public Const PhonePutAway As String = "~INPUT_PHONE_PUT_AWAY~"
        Public Const PhoneTakeOut As String = "~INPUT_PHONE_TAKE_OUT~"
        Public Const PrevWeapon As String = "~INPUT_PREV_WEAPON~"
        Public Const SniperZoomInAlternate As String = "~INPUT_SNIPER_ZOOM_IN_ALTERNATE~"
        Public Const SniperZoomIn As String = "~INPUT_SNIPER_ZOOM_IN~"
        Public Const SniperZoomOutAlternate As String = "~INPUT_SNIPER_ZOOM_OUT_ALTERNATE~"
        Public Const SniperZoomOut As String = "~INPUT_SNIPER_ZOOM_OUT~"
        Public Const TurnOffRadio As String = "~INPUT_TURN_OFF_RADIO~"
        Public Const VehicleAccelerate As String = "~INPUT_VEH_ACCELERATE~"
        Public Const VehicleAttack2 As String = "~INPUT_VEH_ATTACK2~"
        Public Const VehicleAttack As String = "~INPUT_VEH_ATTACK~"
        Public Const VehicleBrake As String = "~INPUT_VEH_BRAKE~"
        Public Const VehicleCinCam As String = "~INPUT_VEH_CIN_CAM~"
        Public Const VehicleFlyPitchDown As String = "~INPUT_VEH_FLY_PITCH_DOWN~"
        Public Const VehicleFlyPitchUp As String = "~INPUT_VEH_FLY_PITCH_UP~"
        Public Const VehicleFlyRollLeft As String = "~INPUT_VEH_FLY_ROLL_LEFT~"
        Public Const VehicleFlyRollRight As String = "~INPUT_VEH_FLY_ROLL_RIGHT~"
        Public Const VehicleFlyThrottleDown As String = "~INPUT_VEH_FLY_THROTTLE_DOWN~"
        Public Const VehicleFlyThrottleUp As String = "~INPUT_VEH_FLY_THROTTLE_UP~"
        Public Const VehicleFlyYawLeft As String = "~INPUT_VEH_FLY_YAW_LEFT~"
        Public Const VehicleFlyYawRight As String = "~INPUT_VEH_FLY_YAW_RIGHT~"
        Public Const VehicleHotwire1 As String = "~INPUT_VEH_HOTWIRE_LEFT~"
        Public Const VehicleHotwire2 As String = "~INPUT_VEH_HOTWIRE_RIGHT~"
        Public Const VehicleKeyUd As String = "~INPUT_VEH_KEY_UD~"
        Public Const VehicleLookBehind As String = "~INPUT_VEH_LOOK_BEHIND~"
        Public Const VehicleLookLeft As String = "~INPUT_VEH_LOOK_LEFT~"
        Public Const VehicleLookRight As String = "~INPUT_VEH_LOOK_RIGHT~"
        Public Const VehicleNextRadio As String = "~INPUT_VEH_NEXT_RADIO~"
        Public Const VehicleNextWeapon As String = "~INPUT_VEH_NEXT_WEAPON~"
        Public Const VehiclePrevRadio As String = "~INPUT_VEH_PREV_RADIO~"
        Public Const ZoomRadar As String = "~INPUT_ZOOM_RADAR~"
        Public Const MouseWheel As String = "~MOUSE_WHEEL~"
        Public Const Mouse As String = "~MOUSE~"
        Public Const Joypad_A As String = "~PAD_A~"
        Public Const Joypad_B As String = "~PAD_B~"
        Public Const Joypad_Down As String = "~PAD_DOWN~"
        Public Const Joypad_DpadAll As String = "~PAD_DPAD_ALL~"
        Public Const Joypad_DpadLeftright As String = "~PAD_DPAD_LEFTRIGHT~"
        Public Const Joypad_DpadLeft As String = "~PAD_DPAD_LEFT~"
        Public Const Joypad_DpadNone As String = "~PAD_DPAD_NONE~"
        Public Const Joypad_DpadRight As String = "~PAD_DPAD_RIGHT~"
        Public Const Joypad_DpadUpdown As String = "~PAD_DPAD_UPDOWN~"
        Public Const Joypad_Lb As String = "~PAD_LB~"
        Public Const Joypad_Left As String = "~PAD_LEFT~"
        Public Const Joypad_Lshock As String = "~PAD_LSHOCK~"
        Public Const Joypad_LstickAll As String = "~PAD_LSTICK_ALL~"
        Public Const Joypad_LstickDown As String = "~PAD_LSTICK_DOWN~"
        Public Const Joypad_LstickLeftrightVehicle As String = "~PAD_LSTICK_LEFTRIGHT_VEH~"
        Public Const Joypad_LstickLeftright As String = "~PAD_LSTICK_LEFTRIGHT~"
        Public Const Joypad_LstickLeft As String = "~PAD_LSTICK_LEFT~"
        Public Const Joypad_LstickNone As String = "~PAD_LSTICK_NONE~"
        Public Const Joypad_LstickRight As String = "~PAD_LSTICK_RIGHT~"
        Public Const Joypad_LstickUpdown As String = "~PAD_LSTICK_UPDOWN~"
        Public Const Joypad_LstickUp As String = "~PAD_LSTICK_UP~"
        Public Const Joypad_LT As String = "~PAD_LT~"
        Public Const Joypad_RB As String = "~PAD_RB~"
        Public Const Joypad_Right As String = "~PAD_RIGHT~"
        Public Const Joypad_Rshock As String = "~PAD_RSHOCK~"
        Public Const Joypad_RstickAll As String = "~PAD_RSTICK_ALL~"
        Public Const Joypad_RstickDown As String = "~PAD_RSTICK_DOWN~"
        Public Const Joypad_RstickLeftright As String = "~PAD_RSTICK_LEFTRIGHT~"
        Public Const Joypad_RstickLeft As String = "~PAD_RSTICK_LEFT~"
        Public Const Joypad_RstickNone As String = "~PAD_RSTICK_NONE~"
        Public Const Joypad_RstickUpdownOnly As String = "~PAD_RSTICK_UPDOWN_ONLY~"
        Public Const Joypad_RstickUpdown As String = "~PAD_RSTICK_UPDOWN~"
        Public Const Joypad_RstickUp As String = "~PAD_RSTICK_UP~"
        Public Const Joypad_RT As String = "~PAD_RT~"
        Public Const Joypad_Select As String = "~PAD_SELECT~"
        Public Const Joypad_SixaxisDrive As String = "~PAD_SIXAXIS_DRIVE~"
        Public Const Joypad_SixaxisPitch As String = "~PAD_SIXAXIS_PITCH~"
        Public Const Joypad_SixaxisReload As String = "~PAD_SIXAXIS_RELOAD~"
        Public Const Joypad_SixaxisRoll As String = "~PAD_SIXAXIS_ROLL~"
        Public Const Joypad_Start As String = "~PAD_START~"
        Public Const Joypad_Up As String = "~PAD_UP~"
        Public Const Joypad_X As String = "~PAD_X~"
        Public Const Joypad_Y As String = "~PAD_Y~"
        Public Const Joypad_DpadUp As String = "~PAD_DPAD_UP~"
        Public Const Joypad_DpadDown As String = "~PAD_DPAD_DOWN~"
        Public Const Joypad_Back As String = "~PAD_BACK~"
    End Structure
    '--------------------------------------------------------------------------------------------------------
    'Brief / News Scrollbar
    '-----------------------
    Public Shared Sub AddTextToBrief(Text As String, Optional Underscore As Boolean = False)
        If Underscore = False Then [Call]("ADD_TO_PREVIOUS_BRIEF", Text) Else [Call]("ADD_TO_PREVIOUS_BRIEF_WITH_UNDERSCORE", Text)
    End Sub
    Public Shared Sub AddTextToNewsScrollbar(Text As String)
        [Call]("ADD_STRING_TO_NEWS_SCROLLBAR", Text)
    End Sub
    Public Shared Sub ClearBrief()
        [Call]("CLEAR_BRIEF")
    End Sub
    Public Shared Sub ClearNewsScrollbar()
        [Call]("CLEAR_NEWS_SCROLLBAR")
    End Sub
    '--------------------------------------------------------------------------------------------------------
    'Custom Texts
    '-------------
    Public Shared Sub DisplayText(Text As String, Optional Duration As Int32 = 3000, Optional BriefDisplay As Boolean = False, Optional TopLeftCorner As Boolean = False)
        If TopLeftCorner Then
            Game.DisplayText(Text, Duration)
        Else
            [Call]("ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS", BriefDisplay)
            [Call]("PRINT_STRING_WiTH_LITERAL_STRING_NOW", "STRING", Text, Duration, 1)
        End If
    End Sub
    Public Shared Sub DisplayText_ThisFrame(Text As String, BackgroundColor As NativeDraws.ColorRGBA, Optional BriefDisplay As Boolean = False) 'Optional BackgroundColor_Red As Int16 = 0, Optional BackgroundColor_Green As Int16 = 0, Optional BackgroundColor_Blue As Int16 = 0, Optional Background_Alpha As Int16 = 120)
        Dim maxWidth As Single = 0.47F
        Dim baseCharCount As Integer = 49
        Dim rectHeight As Single = 0.04
        Dim TextLength As Single = Text.Length / baseCharCount
        Dim rectWidth As Single = (TextLength * maxWidth) + 0.007F
        If rectWidth > maxWidth Then
            rectWidth = maxWidth
            rectHeight = 0.08
            [Call]("DRAW_RECT", 0.5, 0.9105, rectWidth, rectHeight, BackgroundColor.Red, BackgroundColor.Green, BackgroundColor.Blue, BackgroundColor.Alpha)
            DisplayText(Text, 10, BriefDisplay)
        Else
            [Call]("DRAW_RECT", 0.5, 0.93, rectWidth, rectHeight, BackgroundColor.Red, BackgroundColor.Green, BackgroundColor.Blue, BackgroundColor.Alpha)
            DisplayText(Text, 10, BriefDisplay)
        End If
    End Sub
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Untested
    '''</summary>
    Public Shared Sub DrawCylinder(Coordinates As GTA.Vector3, Unknown01 As Single, Unknown02 As Single, Color As ColorRGBA)
        [Call]("DRAW_COLOURED_CYLINDER", Coordinates.X, Coordinates.Y, Coordinates.Z, Unknown01, Unknown02, Color.Red, Color.Green, Color.Blue, Color.Alpha)
    End Sub
    ''' <summary>
    '''Draws a checkpoint. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawCheckpoint(Coordinates As GTA.Vector3, Radius As Single, Color As ColorRGBA)
        If Color.Alpha = 255 Then
            [Call]("DRAW_CHECKPOINT", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Color.Red, Color.Green, Color.Blue)
        Else
            [Call]("DRAW_CHECKPOINT_WITH_ALPHA", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        End If
    End Sub
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    Public Shared Sub DrawCurvedWindow(PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, Alpha As Byte)
        [Call]("DRAW_CURVED_WINDOW", PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, CInt(Alpha))
    End Sub

    Public Shared Sub DrawRectangle(PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, Color As ColorRGBA)
        [Call]("DRAW_RECT", PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, Color.Red, Color.Green, Color.Blue, Color.Alpha)
    End Sub
    ''' <summary>
    ''' Draws the game screen
    '''</summary>
    Public Shared Sub DrawScreen(PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, RotationInDegrees As Single, Color As ColorRGBA)
        [Call]("DRAW_SPRITE_FRONT_BUFF", PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, RotationInDegrees, Color.Red, Color.Green, Color.Blue, Color.Alpha)
    End Sub
    '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Lights and Coronas
    '-------------------
    ''' <summary>
    '''Draws a corona fx. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawCorona(Position As GTA.Vector3, Radius As Single, Color As ColorRGB, Optional Intensity_0_To_100 As Integer = 10)
        Dim R, G, B As Integer
        R = Color.Red * (Intensity_0_To_100 / 100)
        G = Color.Green * (Intensity_0_To_100 / 100)
        B = Color.Blue * (Intensity_0_To_100 / 100)
        [Call]("DRAW_CORONA", Position.X, Position.Y, Position.Z, Radius, 0, 0.0F, R, G, B)
    End Sub
    ''' <summary>
    '''Draws a light with range. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawLight(Position As GTA.Vector3, Color As ColorRGB, Intensity As Single, Range As Single)
        [Call](Of Integer)("DRAW_LIGHT_WITH_RANGE", Position.X, Position.Y, Position.Z, Color.Red, Color.Green, Color.Blue, Intensity, Range)
    End Sub
    ''' <summary>
    ''' Two combined functions to easily draw a corona with proper light. Must be use in a tick (cycle)
    '''</summary>
    Public Shared Sub DrawLightWithCorona(Position As GTA.Vector3, Color As ColorRGB, LightIntensity As Single, LightRange As Single, CoronaRadius As Single, Optional CoronaIntensity_0_To_100 As Integer = 10)
        Dim R, G, B As Integer
        R = Color.Red * (CoronaIntensity_0_To_100 / 100)
        G = Color.Green * (CoronaIntensity_0_To_100 / 100)
        B = Color.Blue * (CoronaIntensity_0_To_100 / 100)
        [Call]("DRAW_CORONA", Position.X, Position.Y, Position.Z, CoronaRadius, 0, 0.0F, R, G, B)
        [Call](Of Integer)("DRAW_LIGHT_WITH_RANGE", Position.X, Position.Y, Position.Z, Color.Red, Color.Green, Color.Blue, LightIntensity, LightRange)
    End Sub
    '--------------------------------------------------------------------------------------------------------
    Private Class Cutscenes

    End Class
    Public Class GXT
        Public Enum Fonts
            Standard = 0
            Dotted = 1
            StandardAndGTAfont = 3
            StandardAndDartHandwrite = 4
            Narrow = 5
        End Enum
        Public Shared Sub ClearFont()
            [Call]("USE_PREVIOUS_FONT_SETTINGS")
        End Sub
        Public Shared Function isFontLoaded(FontID As Fonts)
            Return [Call](Of Boolean)("IS_FONT_LOADED", FontID)
        End Function
        Public Shared Sub LoadFont(FontID As Fonts)
            [Call]("LOAD_TEXT_FONT", FontID)
        End Sub
        Public Shared Sub Print(PercentagePosX As Single, PercentagePosY As Single, GXT_Table As String, GXT_Entry As String, Optional Unknown As Integer = 1)
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, Unknown)
            [Call]("DISPLAY_TEXT", PercentagePosX, PercentagePosY, GXT_Entry)
        End Sub
        Public Shared Sub Print_InputHelperText(GXT_Table As String, GXT_Entry As String, GameInput As String, OnTheSameLine As Boolean, Optional Unknown As UInteger = 1)
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, Unknown)
            [Call]("DRAW_FRONTEND_HELPER_TEXT", GXT_Entry, GameInput, Not OnTheSameLine)
        End Sub
        Public Shared Sub Print_WithAdditionalFloat(PercentagePosX As Single, PercentagePosY As Single, Float As Single, DecimalToShow As Integer, Optional GXT_Entry As String = "NUMBER", Optional GXT_Table As String = "")
            If GXT_Table <> "" AndAlso GXT_Entry <> "NUMBER" Then [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
            [Call]("DISPLAY_TEXT_WITH_FLOAT", PercentagePosX, PercentagePosY, GXT_Entry, Float, DecimalToShow)
        End Sub
        Public Shared Sub Print_WithAdditionalNumbers(PercentagePosX As Single, PercentagePosY As Single, GXT_Entry As String, NumbersCount_1_to_3 As Integer, Number1 As Integer, Optional Number2 As Integer = 0, Optional Number3 As Integer = 0)
            Select Case NumbersCount_1_to_3
                Case 1
                    [Call]("DISPLAY_TEXT_WITH_NUMBER", PercentagePosX, PercentagePosY, GXT_Entry, Number1)
                Case 2
                    [Call]("DISPLAY_TEXT_WITH_2_NUMBERS", PercentagePosX, PercentagePosY, GXT_Entry, Number1, Number2)
                Case 3
                    [Call]("DISPLAY_TEXT_WITH_3_NUMBERS", PercentagePosX, PercentagePosY, GXT_Entry, Number1, Number2, Number2)
                Case Else
                    Exit Sub
            End Select
        End Sub
        Public Shared Sub Print_WithAdditionalStrings(PercentagePosX As Single, PercentagePosY As Single, GXT_Table As String, GXT_Entry As String, LiteralStrings As Boolean, String1 As String, Optional String2 As String = "")
            [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
            If String2 = "" Then
                If LiteralStrings Then
                    [Call]("DISPLAY_TEXT_WITH_LITERAL_STRING", PercentagePosX, PercentagePosY, GXT_Entry, String1)
                Else
                    [Call]("DISPLAY_TEXT_WITH_STRING", PercentagePosX, PercentagePosY, GXT_Entry, String1)
                End If
            Else
                If LiteralStrings Then
                    [Call]("DISPLAY_TEXT_WITH_TWO_STRING", PercentagePosX, PercentagePosY, GXT_Entry, String1, String2)
                Else
                    [Call]("DISPLAY_TEXT_WITH_TWO_LITERAL_STRING", PercentagePosX, PercentagePosY, GXT_Entry, String1, String2)
                End If
            End If
        End Sub
        Public Shared Sub SetFont(FontID As Fonts)
            [Call]("LOAD_TEXT_FONT", FontID)
            [Call]("SET_TEXT_FONT", FontID)
        End Sub
        Public Class HelpMessage
            Public Shared Sub Clear()
                [Call]("CLEAR_HELP")
            End Sub
            Public Shared Sub EnableNonMinigameHelpMessages(Optional Enable As Boolean = True)
                [Call]("DISPLAY_NON_MINIGAME_HELP_MESSAGES", Enable)
            End Sub
            Public Shared Function getMessageBoxSize() As GTA.Vector2
                Dim pointer1, pointer2 As Native.Pointer
                pointer1 = New Native.Pointer(GetType(Single))
                pointer2 = New Native.Pointer(GetType(Single))
                [Call]("GET_HELP_MESSAGE_BOX_SIZE", pointer1, pointer2)
                Return New GTA.Vector2(pointer1.Value, pointer2.Value)
            End Function
            Public Shared Sub Hide_ThisFrame()
                [Call]("HIDE_HELP_TEXT_THIS_FRAME")
            End Sub
            Public Shared Sub InitFrontedHelperText()
                [Call]("INIT_FRONTEND_HELPER_TEXT")
            End Sub
            Public Shared Function isPrintingAnyHelp() As Boolean
                Return [Call](Of Boolean)("IS_HELP_MESSAGE_BEING_DISPLAYED")
            End Function
            Public Shared Function isPrintingHelp(GXT_Entry As String) As Boolean
                Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_BEING_DISPLAYED", GXT_Entry)
            End Function
            Public Shared Function isPrintingHelp_WithNumber(GXT_Entry As String, Number As Integer) As Boolean
                Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_WITH_NUMBER_BEING_DISPLAYED", GXT_Entry, Number)
            End Function
            Public Shared Function isPrintingHelp_WithString(GXT_Entry As String, AdditionalString As String) As Boolean
                Return [Call](Of Boolean)("IS_THIS_HELP_MESSAGE_WITH_STRING_BEING_DISPLAYED", GXT_Entry, AdditionalString)
            End Function
            Public Shared Sub Print(GXT_Table As String, GXT_Entry As String, Optional PrintForever As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                If Not PrintForever Then
                    [Call]("PRINT_HELP", GXT_Entry)
                Else
                    [Call]("PRINT_HELP_FOREVER", GXT_Entry)
                End If
            End Sub
            Public Shared Sub Print_ThisFrame(GXT_Table As String, GXT_Entry As String, Optional Unknown As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                [Call]("DISPLAY_HELP_TEXT_THIS_FRAME", GXT_Entry, Unknown)
            End Sub
            Public Shared Sub Print_WithAdditionalNumber(GXT_Table As String, GXT_Entry As String, AdditionalNumber As Integer, Optional PrintForever As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                If Not PrintForever Then
                    [Call]("PRINT_HELP_WITH_NUMBER", GXT_Entry, AdditionalNumber)
                Else
                    [Call]("PRINT_HELP_FOREVER_WITH_NUMBER", GXT_Entry, AdditionalNumber)
                End If
            End Sub
            Public Shared Sub Print_WithAdditionalString(GXT_Table As String, GXT_Entry As String, AdditionalString_GXT As String, Optional PrintForever As Boolean = False, Optional NoSound As Boolean = False)
                [Call]("LOAD_ADDITIONAL_TEXT", GXT_Table, 1)
                If Not PrintForever Then
                    [Call]("PRINT_HELP_WITH_STRING", GXT_Entry, AdditionalString_GXT)
                Else
                    If Not NoSound Then
                        [Call]("PRINT_HELP_FOREVER_WITH_STRING", GXT_Entry, AdditionalString_GXT)
                    Else
                        [Call]("PRINT_HELP_FOREVER_WITH_STRING_NO_SOUND", GXT_Entry, AdditionalString_GXT)
                    End If
                End If
            End Sub
            'Public Shared Sub SetMessageBoxSize(Size As Single)
            '    [Call]("SET_HELP_MESSAGE_BOX_SIZE", value unknown type)
            'End Sub
        End Class
    End Class
    Public Class Movies
        ' Private Shared _currMovie As String
        Public Shared Sub Draw(PercentPosX As Single, PercentPosY As Single, PercentSizeX As Single, PercentSizeY As Single, Rotation As Single, Color As ColorRGBA)
            [Call]("DRAW_MOVIE", PercentSizeX, PercentPosY, PercentSizeX, PercentSizeY, Rotation, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        End Sub
        Public Shared Sub Play()
            [Call]("PLAY_MOVIE")
        End Sub
        Public Shared Sub Relase()
            [Call]("RELASE_MOVIE")
        End Sub
        Public Shared WriteOnly Property CurrentMovie As String
            'Get
            '    Return _currMovie
            'End Get
            Set(value As String)
                [Call]("SET_CURRENT_MOVIE", value)
                '_currMovie = value
            End Set
        End Property
        Public Shared WriteOnly Property Time As Single
            Set(value As Single)
                [Call]("SET_MOVIE_TIME", value)
            End Set
        End Property
        Public Shared WriteOnly Property Volume As Single
            Set(value As Single)
                [Call]("SET_MOVIE_VOLUME", value)
            End Set
        End Property
        Public Shared Sub [Stop](Optional Relase As Boolean = False)
            [Call]("STOP_MOVIE")
            If Relase Then [Call]("RELASE_MOVIE")
        End Sub
    End Class
    Public Class Textures
        ''' <summary>
        ''' Draws game texture
        ''' </summary>
        Public Shared Sub Draw(Texture As Integer, PercentagePosX As Single, PercentagePosY As Single, PercentageSizeX As Single, PercentageSizeY As Single, RotationInDegrees As Single, Color As ColorRGBA)
            [Call]("DRAW_SPRITE", Texture, PercentagePosX, PercentagePosY, PercentageSizeX, PercentageSizeY, RotationInDegrees, Color.Red, Color.Green, Color.Blue, Color.Alpha)
        End Sub
        Public Shared Function Request(TXD As String, Texture As String, Optional StreamedTXD As Boolean = False) As Integer
            Dim txdint As Integer
            If StreamedTXD Then
                txdint = [Call](Of Integer)("REQUEST_STREAMED_TXD", TXD)
                Return [Call](Of Integer)("GET_TEXTURE_FROM_STREAMED_TXD", txdint, Texture)
            Else
                txdint = [Call](Of Integer)("LOAD_TXD", TXD)
                Return [Call](Of Integer)("GET_TEXTURE", txdint, Texture)
            End If
        End Function
        Public Shared Sub Relase(Texture As Integer)
            [Call]("RELEASE_TEXTURE", Texture)
        End Sub
    End Class
End Class

'Public Shared Function AddSphere(Coordinates As GTA.Vector3, Radius As Single, Unknown As Integer) As Integer
'    Return [Call](Of Integer)("ADD_SPHERE", Coordinates.X, Coordinates.Y, Coordinates.Z, Radius, Unknown)
'End Function
'Public Shared Sub DrawSphere(X As Single, Y As Single, Z As Single, Radius As Single)
'    [Call]("DRAW_SPHERE", X, Y, Z, Radius)
'End Sub