Imports GTA
Imports GTA.Native.Function
'Imports System.IO
'Imports System.Windows.Media.Animation
'Imports GTA.base
'Imports System
'Imports System.Windows.Forms



'Public Class PlayerMoods
'    Public Shared Normal As String = "plyr_mood_normal"
'    Public Shared Happy As String = "plyr_mood_happy"
'    Public Shared Angry As String = "plyr_mood_angry"
'    Public Shared Angry2 As String = "gest_angry_loop"
'    Public Shared PoliceChase As String = "police_chase"
'    Public Shared Thinking As String = "gest_think_loop"
'    Public Shared Suprised As String = "gest_surprised_loop"
'    Public Shared Chewing As String = "chew"
'End Class
'Public Class PedFacials
'    Public Shared Think As String = "gest_think_loop"
'    Public Shared Suprised As String = "gest_surprised_loop"
'    Public Shared Chew As String = "chew"
'    Public Shared GunAim As String = "gun_aim"
'    Public Shared CueAim As String = "aim_cue"
'    Public Shared Angry1 As String = "gest_angry_loop"
'    Public Shared Angry2 As String = "angry_a"
'    Public Shared Angry3 As String = "angry_b"
'    Public Shared Angry4 As String = "angry_c"
'    Public Shared Blow As String = "blow"
'    Public Shared HeavyBreath As String = "heavybreath"
'    Public Shared LookAround As String = "lookaround"
'    Public Shared LookDown As String = "look_down"
'    Public Shared LookLeft As String = "look_left"
'    Public Shared LookRight As String = "look_right"
'    Public Shared Pain1 As String = "pain_a"
'    Public Shared Pain2 As String = "pain_b"
'    Public Shared Pain3 As String = "pain_c"
'    Public Shared Shocked As String = "shocked"
'    Public Shared Whatever As String = "whatever"
'    Public Shared Dead1 As String = "dead_a"
'    Public Shared Dead2 As String = "dead_b"
'    Public Shared Dead3 As String = "die_a"
'End Class

'Public Class Gestures
'    Public ReadOnly AnimSet As String
'    Public ReadOnly AnimName As String
'    Public Class GestureAnim
'        Public Sub New(AnimSet As String, AnimName As String)
'            Me.AnimSet = AnimSet
'            Me.AnimName = AnimName
'        End Sub
'        Public Sub Play(Ped As Ped, Optional Speed As Double = 1.0)
'            [Call]("REQUEST_ANIMS", AnimSet)
'            Do
'                Game.WaitInCurrentScript(1)
'            Loop Until [Call](Of Boolean)("HAS_ANIMS_LOADED")
'            If [Call](Of Boolean)("IS_CHAR_SITTING_IN_ANY_CAR", Ped) Then
'                [Call]("TASK_PLAY_ANIM_SECONDARY_IN_CAR", Ped, AnimName, AnimSet, Speed, False, False, False, False)
'            Else
'                [Call]("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY", Ped, AnimName, AnimSet, Speed, False, False, False, False)
'            End If
'        End Sub
'    End Class

'    Public Class HandMale
'        Public Const AnimSet As String = "gestures@mp_male"

'        Public Shared ReadOnly Finger As New GestureAnim(AnimSet, "finger")
'        Public Shared ReadOnly Rock As New GestureAnim(AnimSet, "rock")
'        Public Shared ReadOnly Hello As New GestureAnim(AnimSet, "wave")
'        Public Shared ReadOnly Salute As New GestureAnim(AnimSet, "salute")
'    End Class
'    Public Class HandFemale
'        Public Const AnimSet As String = "gestures@mp_female"

'        Public Shared ReadOnly Finger As New GestureAnim(AnimSet, "finger")
'        Public Shared ReadOnly Rock As New GestureAnim(AnimSet, "rock")
'        Public Shared ReadOnly Hello As New GestureAnim(AnimSet, "wave")
'        Public Shared ReadOnly Salute As New GestureAnim(AnimSet, "salute")
'    End Class
'    Public Class Male
'        Public Const AnimSet As String = "gestures@male"

'        Public Shared ReadOnly Absolutely As New GestureAnim(AnimSet, "absolutely")
'        Public Shared ReadOnly Agree As New GestureAnim(AnimSet, "agree")
'        Public Shared ReadOnly Amazing As New GestureAnim(AnimSet, "amazing")
'        Public Shared ReadOnly AngerA As New GestureAnim(AnimSet, "anger_a")
'        Public Shared ReadOnly AreYouIn As New GestureAnim(AnimSet, "are_you_in")
'        Public Shared ReadOnly BringItOn As New GestureAnim(AnimSet, "bring_it_on")
'        Public Shared ReadOnly BringItToMe As New GestureAnim(AnimSet, "bring_it_to_me")
'        Public Shared ReadOnly ButWhy As New GestureAnim(AnimSet, "but_why")
'        Public Shared ReadOnly ComeHere As New GestureAnim(AnimSet, "come_here")
'        Public Shared ReadOnly ComeOn As New GestureAnim(AnimSet, "come_on")
'        Public Shared ReadOnly Damn As New GestureAnim(AnimSet, "damn")
'        Public Shared ReadOnly Despair As New GestureAnim(AnimSet, "despair")
'        Public Shared ReadOnly Disbelief As New GestureAnim(AnimSet, "disbelief")
'        Public Shared ReadOnly DontHitMe As New GestureAnim(AnimSet, "dont_hit_me")
'        Public Shared ReadOnly DontKnow As New GestureAnim(AnimSet, "dont_know")
'        Public Shared ReadOnly DoIt As New GestureAnim(AnimSet, "do_it")
'        Public Shared ReadOnly EasyNow As New GestureAnim(AnimSet, "easy_now")
'        Public Shared ReadOnly Enough As New GestureAnim(AnimSet, "enough")
'        Public Shared ReadOnly Exactly As New GestureAnim(AnimSet, "exactly")
'        Public Shared ReadOnly FoldArmsOhYeah As New GestureAnim(AnimSet, "fold_arms_oh_yeah")
'        Public Shared ReadOnly ForgetIt As New GestureAnim(AnimSet, "forget_it")
'        Public Shared ReadOnly GiveMeABreak As New GestureAnim(AnimSet, "give_me_a_break")
'        Public Shared ReadOnly Goddamn As New GestureAnim(AnimSet, "goddamn")
'        Public Shared ReadOnly Good As New GestureAnim(AnimSet, "good")
'        Public Shared ReadOnly GoAway As New GestureAnim(AnimSet, "go_away")
'        Public Shared ReadOnly Hello As New GestureAnim(AnimSet, "hello")
'        Public Shared ReadOnly Hey As New GestureAnim(AnimSet, "hey")
'        Public Shared ReadOnly How As New GestureAnim(AnimSet, "how")
'        Public Shared ReadOnly HowCouldYou As New GestureAnim(AnimSet, "how_could_you")
'        Public Shared ReadOnly HowMuch As New GestureAnim(AnimSet, "how_much")
'        Public Shared ReadOnly IfUSaySo As New GestureAnim(AnimSet, "if_u_say_so")
'        Public Shared ReadOnly IllDoIt As New GestureAnim(AnimSet, "ill_do_it")
'        Public Shared ReadOnly ImTalking2You As New GestureAnim(AnimSet, "im talking_2_you")
'        Public Shared ReadOnly ImBeggingYou As New GestureAnim(AnimSet, "im_begging_you")
'        Public Shared ReadOnly ImNotSure As New GestureAnim(AnimSet, "im_not_sure")
'        Public Shared ReadOnly ImSorry As New GestureAnim(AnimSet, "im_sorry")
'        Public Shared ReadOnly ImTellingYou As New GestureAnim(AnimSet, "im_telling_you")
'        Public Shared ReadOnly IndicateBack As New GestureAnim(AnimSet, "indicate_back")
'        Public Shared ReadOnly IndicateLeft As New GestureAnim(AnimSet, "indicate_left")
'        Public Shared ReadOnly IndicateListener As New GestureAnim(AnimSet, "indicate_listener")
'        Public Shared ReadOnly IndicateRightB As New GestureAnim(AnimSet, "indicate_right_b")
'        Public Shared ReadOnly IndicateRightC As New GestureAnim(AnimSet, "indicate_right_c")
'        Public Shared ReadOnly IsThisIt As New GestureAnim(AnimSet, "is_this_it")
'        Public Shared ReadOnly ItsDone As New GestureAnim(AnimSet, "its_done")
'        Public Shared ReadOnly ItsMine As New GestureAnim(AnimSet, "its_mine")
'        Public Shared ReadOnly ItsOk As New GestureAnim(AnimSet, "its_ok")
'        Public Shared ReadOnly IveForgot As New GestureAnim(AnimSet, "ive_forgot")
'        Public Shared ReadOnly ICantSay As New GestureAnim(AnimSet, "i_cant_say")
'        Public Shared ReadOnly ICouldnt As New GestureAnim(AnimSet, "i_couldnt")
'        Public Shared ReadOnly IDontHave As New GestureAnim(AnimSet, "i_dont_have")
'        Public Shared ReadOnly IDontThinkSo As New GestureAnim(AnimSet, "i_dont_think_so")
'        Public Shared ReadOnly IGetIt As New GestureAnim(AnimSet, "i_get_it")
'        Public Shared ReadOnly IGiveUp As New GestureAnim(AnimSet, "i_give_up")
'        Public Shared ReadOnly ISaidNo As New GestureAnim(AnimSet, "i_said_no")
'        Public Shared ReadOnly IWill As New GestureAnim(AnimSet, "i_will")
'        Public Shared ReadOnly KissMyAss As New GestureAnim(AnimSet, "kiss_my_ass")
'        Public Shared ReadOnly Later As New GestureAnim(AnimSet, "later")
'        Public Shared ReadOnly LeaveIt2Me As New GestureAnim(AnimSet, "leave_it_2_me")
'        Public Shared ReadOnly LetMeThink As New GestureAnim(AnimSet, "let_me_think")
'        Public Shared ReadOnly LikeThis As New GestureAnim(AnimSet, "like_this")
'        Public Shared ReadOnly MeGest As New GestureAnim(AnimSet, "me")
'        Public Shared ReadOnly Natuarally As New GestureAnim(AnimSet, "natuarally")
'        Public Shared ReadOnly Negative As New GestureAnim(AnimSet, "negative")
'        Public Shared ReadOnly NodNo As New GestureAnim(AnimSet, "nod_no")
'        Public Shared ReadOnly NodYes As New GestureAnim(AnimSet, "nod_yes")
'        Public Shared ReadOnly NotMe As New GestureAnim(AnimSet, "not_me")
'        Public Shared ReadOnly NotSure As New GestureAnim(AnimSet, "not_sure")
'        Public Shared ReadOnly NoChance As New GestureAnim(AnimSet, "no_chance")
'        Public Shared ReadOnly NoReally As New GestureAnim(AnimSet, "no_really")
'        Public Shared ReadOnly Numbnuts As New GestureAnim(AnimSet, "numbnuts")
'        Public Shared ReadOnly OfCourse As New GestureAnim(AnimSet, "of_course")
'        Public Shared ReadOnly OhShit As New GestureAnim(AnimSet, "oh_shit")
'        Public Shared ReadOnly Ok As New GestureAnim(AnimSet, "ok")
'        Public Shared ReadOnly OkOk As New GestureAnim(AnimSet, "ok_ok")
'        Public Shared ReadOnly OverThere As New GestureAnim(AnimSet, "over_there")
'        Public Shared ReadOnly PissOff As New GestureAnim(AnimSet, "piss_off")
'        Public Shared ReadOnly Please As New GestureAnim(AnimSet, "please")
'        Public Shared ReadOnly PointFwd As New GestureAnim(AnimSet, "point_fwd")
'        Public Shared ReadOnly PointRight As New GestureAnim(AnimSet, "point_right")
'        Public Shared ReadOnly Positive As New GestureAnim(AnimSet, "positive")
'        Public Shared ReadOnly RaiseHands As New GestureAnim(AnimSet, "raise_hands")
'        Public Shared ReadOnly SayAgain As New GestureAnim(AnimSet, "say_again")
'        Public Shared ReadOnly ScrewYou As New GestureAnim(AnimSet, "screw_you")
'        Public Shared ReadOnly Shit As New GestureAnim(AnimSet, "shit")
'        Public Shared ReadOnly Shock As New GestureAnim(AnimSet, "shock")
'        Public Shared ReadOnly ShutUp As New GestureAnim(AnimSet, "shut_up")
'        Public Shared ReadOnly StopIt As New GestureAnim(AnimSet, "stop")
'        Public Shared ReadOnly Sure As New GestureAnim(AnimSet, "sure")
'        Public Shared ReadOnly TellMeAboutIt As New GestureAnim(AnimSet, "tell_me_about_it")
'        Public Shared ReadOnly Test As New GestureAnim(AnimSet, "test")
'        Public Shared ReadOnly That As New GestureAnim(AnimSet, "that")
'        Public Shared ReadOnly ThatWay As New GestureAnim(AnimSet, "that_way")
'        Public Shared ReadOnly ThisAndThat As New GestureAnim(AnimSet, "this_and_that")
'        Public Shared ReadOnly ThisBig As New GestureAnim(AnimSet, "this_big")
'        Public Shared ReadOnly Threaten As New GestureAnim(AnimSet, "threaten")
'        Public Shared ReadOnly Time As New GestureAnim(AnimSet, "time")
'        Public Shared ReadOnly Tosser As New GestureAnim(AnimSet, "tosser")
'        Public Shared ReadOnly TouchFace As New GestureAnim(AnimSet, "touch_face")
'        Public Shared ReadOnly ToHellWithIt As New GestureAnim(AnimSet, "to_hell_with_it")
'        Public Shared ReadOnly Unbelievable As New GestureAnim(AnimSet, "unbelievable")
'        Public Shared ReadOnly Uptight As New GestureAnim(AnimSet, "uptight")
'        Public Shared ReadOnly UCantDoThat As New GestureAnim(AnimSet, "u_cant_do_that")
'        Public Shared ReadOnly USerious As New GestureAnim(AnimSet, "u_serious")
'        Public Shared ReadOnly UTalkin2Me As New GestureAnim(AnimSet, "u_talkin_2_me")
'        Public Shared ReadOnly UThinImStupid As New GestureAnim(AnimSet, "u_thin_i'm_stupid")
'        Public Shared ReadOnly UUnderstand As New GestureAnim(AnimSet, "u_understand")
'        Public Shared ReadOnly WantSomeOfThis As New GestureAnim(AnimSet, "want_some_of_this")
'        Public Shared ReadOnly We As New GestureAnim(AnimSet, "we")
'        Public Shared ReadOnly Well As New GestureAnim(AnimSet, "well")
'        Public Shared ReadOnly WellAlright As New GestureAnim(AnimSet, "well_alright")
'        Public Shared ReadOnly WeCanDoIt As New GestureAnim(AnimSet, "we_can_do_it")
'        Public Shared ReadOnly What As New GestureAnim(AnimSet, "what")
'        Public Shared ReadOnly Whatever As New GestureAnim(AnimSet, "whatever")
'        Public Shared ReadOnly WhateverC As New GestureAnim(AnimSet, "whatever_c")
'        Public Shared ReadOnly Why As New GestureAnim(AnimSet, "why")
'        Public Shared ReadOnly WotTheFuck As New GestureAnim(AnimSet, "wot_the_fuck")
'        Public Shared ReadOnly YeahIGotIt As New GestureAnim(AnimSet, "yeah_i_got_it")
'        Public Shared ReadOnly Yes As New GestureAnim(AnimSet, "yes")
'        Public Shared ReadOnly YoureRight As New GestureAnim(AnimSet, "youre_right")
'        Public Shared ReadOnly YouDig As New GestureAnim(AnimSet, "you_dig")
'        Public Shared ReadOnly YouWillLovethis As New GestureAnim(AnimSet, "you_will_love this")
'    End Class
'    Public Class Female
'        Public Const AnimSet As String = "gestures@female"

'        Public Shared ReadOnly Absolutely As New GestureAnim(AnimSet, "absolutely")
'        Public Shared ReadOnly Agree As New GestureAnim(AnimSet, "agree")
'        Public Shared ReadOnly AllGone As New GestureAnim(AnimSet, "all_gone")
'        Public Shared ReadOnly CantBe As New GestureAnim(AnimSet, "cant_be")
'        Public Shared ReadOnly Clap As New GestureAnim(AnimSet, "clap")
'        Public Shared ReadOnly DefinitelyNot As New GestureAnim(AnimSet, "definitely_not")
'        Public Shared ReadOnly Dont As New GestureAnim(AnimSet, "dont")
'        Public Shared ReadOnly DontEven As New GestureAnim(AnimSet, "dont_even")
'        Public Shared ReadOnly DontKnow As New GestureAnim(AnimSet, "dont_know")
'        Public Shared ReadOnly DontUDare As New GestureAnim(AnimSet, "dont_u_dare")
'        Public Shared ReadOnly GetLost As New GestureAnim(AnimSet, "get_lost")
'        Public Shared ReadOnly GetThisStraight As New GestureAnim(AnimSet, "get_this_straight")
'        Public Shared ReadOnly Gimme As New GestureAnim(AnimSet, "gimme")
'        Public Shared ReadOnly GoOver As New GestureAnim(AnimSet, "go_over")
'        Public Shared ReadOnly HiYa As New GestureAnim(AnimSet, "hi_ya")
'        Public Shared ReadOnly IndicateBwd As New GestureAnim(AnimSet, "indicate_bwd")
'        Public Shared ReadOnly IndicateFwdA As New GestureAnim(AnimSet, "indicate_fwd_a")
'        Public Shared ReadOnly IndicateFwdB As New GestureAnim(AnimSet, "indicate_fwd_b")
'        Public Shared ReadOnly IndicateLeft As New GestureAnim(AnimSet, "indicate_left")
'        Public Shared ReadOnly IndicateRight As New GestureAnim(AnimSet, "indicate_right")
'        Public Shared ReadOnly ItsSoLike As New GestureAnim(AnimSet, "its_so_like")
'        Public Shared ReadOnly ICant As New GestureAnim(AnimSet, "i_cant")
'        Public Shared ReadOnly IDontCare As New GestureAnim(AnimSet, "i_dont_care")
'        Public Shared ReadOnly IDontThinkSo As New GestureAnim(AnimSet, "i_dont_think_so")
'        Public Shared ReadOnly IOughta As New GestureAnim(AnimSet, "i_oughta")
'        Public Shared ReadOnly JustGo As New GestureAnim(AnimSet, "just_go")
'        Public Shared ReadOnly LeaveIt As New GestureAnim(AnimSet, "leave_it")
'        Public Shared ReadOnly LetMeTellU As New GestureAnim(AnimSet, "let_me_tell_u")
'        Public Shared ReadOnly LookAtMe As New GestureAnim(AnimSet, "look_at_me")
'        Public Shared ReadOnly MaybeU As New GestureAnim(AnimSet, "maybe_u")
'        Public Shared ReadOnly MeGest As New GestureAnim(AnimSet, "me")
'        Public Shared ReadOnly NeverOnYourLife As New GestureAnim(AnimSet, "never_on_your_life")
'        Public Shared ReadOnly [No] As New GestureAnim(AnimSet, "no")
'        Public Shared ReadOnly NoThanks As New GestureAnim(AnimSet, "no_thanks")
'        Public Shared ReadOnly NoUCant As New GestureAnim(AnimSet, "no_u_cant")
'        Public Shared ReadOnly NoWay As New GestureAnim(AnimSet, "no_way")
'        Public Shared ReadOnly NoYourWrong As New GestureAnim(AnimSet, "no_your_wrong")
'        Public Shared ReadOnly OfCoarse As New GestureAnim(AnimSet, "of_coarse")
'        Public Shared ReadOnly OhComeOn As New GestureAnim(AnimSet, "oh_come_on")
'        Public Shared ReadOnly OhMyGod As New GestureAnim(AnimSet, "oh_my_god")
'        Public Shared ReadOnly OhNo As New GestureAnim(AnimSet, "oh_no")
'        Public Shared ReadOnly Ok As New GestureAnim(AnimSet, "ok")
'        Public Shared ReadOnly OverThere As New GestureAnim(AnimSet, "over_there")
'        Public Shared ReadOnly Possibly As New GestureAnim(AnimSet, "possibly")
'        Public Shared ReadOnly Shocked As New GestureAnim(AnimSet, "shocked")
'        Public Shared ReadOnly SoTough As New GestureAnim(AnimSet, "so_tough")
'        Public Shared ReadOnly Sure As New GestureAnim(AnimSet, "sure")
'        Public Shared ReadOnly TakeIt As New GestureAnim(AnimSet, "take_it")
'        Public Shared ReadOnly ThankU As New GestureAnim(AnimSet, "thank_u")
'        Public Shared ReadOnly TheirSoFat As New GestureAnim(AnimSet, "their_so_fat")
'        Public Shared ReadOnly ThisBig As New GestureAnim(AnimSet, "this_big")
'        Public Shared ReadOnly ThisHigh As New GestureAnim(AnimSet, "this_high")
'        Public Shared ReadOnly Threaten As New GestureAnim(AnimSet, "threaten")
'        Public Shared ReadOnly Upset As New GestureAnim(AnimSet, "upset")
'        Public Shared ReadOnly UpYours As New GestureAnim(AnimSet, "up_yours")
'        Public Shared ReadOnly UJustWatch As New GestureAnim(AnimSet, "u_just_watch")
'        Public Shared ReadOnly UShould As New GestureAnim(AnimSet, "u_should")
'        Public Shared ReadOnly WhatUWish As New GestureAnim(AnimSet, "what_u_wish")
'        Public Shared ReadOnly WhyNot As New GestureAnim(AnimSet, "why_not")
'        Public Shared ReadOnly YeahOk As New GestureAnim(AnimSet, "yeah_ok")
'        Public Shared ReadOnly YeahSure As New GestureAnim(AnimSet, "yeah_sure")
'        Public Shared ReadOnly YourRight As New GestureAnim(AnimSet, "your_right")
'    End Class
'    Public Class CarMale
'        Public Const AnimSet As String = "gestures@car"

'        Public Shared ReadOnly AreYouIn As New GestureAnim(AnimSet, "are_you_in")
'        Public Shared ReadOnly ButWhy As New GestureAnim(AnimSet, "but_why")
'        Public Shared ReadOnly ComeOn As New GestureAnim(AnimSet, "come_on")
'        Public Shared ReadOnly Despair As New GestureAnim(AnimSet, "despair")
'        Public Shared ReadOnly Disbelief As New GestureAnim(AnimSet, "disbelief")
'        Public Shared ReadOnly DontHitMe As New GestureAnim(AnimSet, "dont_hit_me")
'        Public Shared ReadOnly DontKnow As New GestureAnim(AnimSet, "dont_know")
'        Public Shared ReadOnly EasyNow As New GestureAnim(AnimSet, "easy_now")
'        Public Shared ReadOnly ForgetIt As New GestureAnim(AnimSet, "forget_it")
'        Public Shared ReadOnly Goddamn As New GestureAnim(AnimSet, "goddamn")
'        Public Shared ReadOnly Good As New GestureAnim(AnimSet, "good")
'        Public Shared ReadOnly HowCouldYou As New GestureAnim(AnimSet, "how_could_you")
'        Public Shared ReadOnly Imtalking2You As New GestureAnim(AnimSet, "im talking_2_you")
'        Public Shared ReadOnly ImNotSure As New GestureAnim(AnimSet, "im_not_sure")
'        Public Shared ReadOnly ImTellingYou As New GestureAnim(AnimSet, "im_telling_you")
'        Public Shared ReadOnly IndicateListener As New GestureAnim(AnimSet, "indicate_listener")
'        Public Shared ReadOnly ItsDone As New GestureAnim(AnimSet, "its_done")
'        Public Shared ReadOnly IGetIt As New GestureAnim(AnimSet, "i_get_it")
'        Public Shared ReadOnly ISaidNo As New GestureAnim(AnimSet, "i_said_no")
'        Public Shared ReadOnly IWill As New GestureAnim(AnimSet, "i_will")
'        Public Shared ReadOnly LetMeThink As New GestureAnim(AnimSet, "let_me_think")
'        Public Shared ReadOnly NodNo As New GestureAnim(AnimSet, "nod_no")
'        Public Shared ReadOnly NodYes As New GestureAnim(AnimSet, "nod_yes")
'        Public Shared ReadOnly OfCourse As New GestureAnim(AnimSet, "of_course")
'        Public Shared ReadOnly OhShit As New GestureAnim(AnimSet, "oh_shit")
'        Public Shared ReadOnly OkOk As New GestureAnim(AnimSet, "ok_ok")
'        Public Shared ReadOnly Please As New GestureAnim(AnimSet, "please")
'        Public Shared ReadOnly Shit As New GestureAnim(AnimSet, "shit")
'        Public Shared ReadOnly Shock As New GestureAnim(AnimSet, "shock")
'        Public Shared ReadOnly ShutUp As New GestureAnim(AnimSet, "shut_up")
'        Public Shared ReadOnly Smoke As New GestureAnim(AnimSet, "smoke")
'        Public Shared ReadOnly StopIt As New GestureAnim(AnimSet, "stop")
'        Public Shared ReadOnly Test As New GestureAnim(AnimSet, "Test")
'        Public Shared ReadOnly Uptight As New GestureAnim(AnimSet, "uptight")
'        Public Shared ReadOnly UThinImStupid As New GestureAnim(AnimSet, "u_thin_i'm_stupid")
'        Public Shared ReadOnly Whatever As New GestureAnim(AnimSet, "whatever")
'        Public Shared ReadOnly YeahIGotIt As New GestureAnim(AnimSet, "yeah_i_got_it")
'        Public Shared ReadOnly Yes As New GestureAnim(AnimSet, "yes")
'        Public Shared ReadOnly YoureRight As New GestureAnim(AnimSet, "youre_right")
'        Public Shared ReadOnly YouDig As New GestureAnim(AnimSet, "you_dig")
'        Public Shared ReadOnly YouWillLovethis As New GestureAnim(AnimSet, "you_will_love this")
'    End Class
'    Public Class CarFemale
'        Public Const AnimSet As String = "gestures@car_f"

'        Public Shared ReadOnly Absolutely As New GestureAnim(AnimSet, "absolutely")
'        Public Shared ReadOnly AllGone As New GestureAnim(AnimSet, "all_gone")
'        Public Shared ReadOnly CantBe As New GestureAnim(AnimSet, "cant_be")
'        Public Shared ReadOnly Dont As New GestureAnim(AnimSet, "dont")
'        Public Shared ReadOnly DontUDare As New GestureAnim(AnimSet, "dont_u_dare")
'        Public Shared ReadOnly GetThisStraight As New GestureAnim(AnimSet, "get_this_straight")
'        Public Shared ReadOnly Gimme As New GestureAnim(AnimSet, "gimme")
'        Public Shared ReadOnly ItsSoLike As New GestureAnim(AnimSet, "its_so_like")
'        Public Shared ReadOnly ICant As New GestureAnim(AnimSet, "i_cant")
'        Public Shared ReadOnly IDontCare As New GestureAnim(AnimSet, "i_dont_care")
'        Public Shared ReadOnly IOughta As New GestureAnim(AnimSet, "i_oughta")
'        Public Shared ReadOnly JustGo As New GestureAnim(AnimSet, "just_go")
'        Public Shared ReadOnly LeaveIt As New GestureAnim(AnimSet, "leave_it")
'        Public Shared ReadOnly LookAtMe As New GestureAnim(AnimSet, "look_at_me")
'        Public Shared ReadOnly No As New GestureAnim(AnimSet, "no")
'        Public Shared ReadOnly NoThanks As New GestureAnim(AnimSet, "no_thanks")
'        Public Shared ReadOnly NoWay As New GestureAnim(AnimSet, "no_way")
'        Public Shared ReadOnly OfCoarse As New GestureAnim(AnimSet, "of_coarse")
'        Public Shared ReadOnly OhComeOn As New GestureAnim(AnimSet, "oh_come_on")
'        Public Shared ReadOnly OhMyGod As New GestureAnim(AnimSet, "oh_my_god")
'        Public Shared ReadOnly OhNo As New GestureAnim(AnimSet, "oh_no")
'        Public Shared ReadOnly Ok As New GestureAnim(AnimSet, "ok")
'        Public Shared ReadOnly Possibly As New GestureAnim(AnimSet, "possibly")
'        Public Shared ReadOnly Shocked As New GestureAnim(AnimSet, "shocked")
'        Public Shared ReadOnly SoTough As New GestureAnim(AnimSet, "so_tough")
'        Public Shared ReadOnly Test As New GestureAnim(AnimSet, "Test")
'        Public Shared ReadOnly ThisHigh As New GestureAnim(AnimSet, "this_high")
'        Public Shared ReadOnly Threaten As New GestureAnim(AnimSet, "threaten")
'        Public Shared ReadOnly Upset As New GestureAnim(AnimSet, "upset")
'        Public Shared ReadOnly UShould As New GestureAnim(AnimSet, "u_should")
'        Public Shared ReadOnly WhatUWish As New GestureAnim(AnimSet, "what_u_wish")
'        Public Shared ReadOnly YeahSure As New GestureAnim(AnimSet, "yeah_sure")
'        Public Shared ReadOnly YourRight As New GestureAnim(AnimSet, "your_right")
'    End Class
'    Public Class Seated
'        Public Const AnimSet As String = "gestures@m_seated"

'        Public Shared ReadOnly Absolutely As New GestureAnim(AnimSet, "absolutely")
'        Public Shared ReadOnly Agree As New GestureAnim(AnimSet, "agree")
'        Public Shared ReadOnly Amazing As New GestureAnim(AnimSet, "amazing")
'        Public Shared ReadOnly AngerA As New GestureAnim(AnimSet, "anger_a")
'        Public Shared ReadOnly AreYouIn As New GestureAnim(AnimSet, "are_you_in")
'        Public Shared ReadOnly BringItOn As New GestureAnim(AnimSet, "bring_it_on")
'        Public Shared ReadOnly BringItToMe As New GestureAnim(AnimSet, "bring_it_to_me")
'        Public Shared ReadOnly ButWhy As New GestureAnim(AnimSet, "but_why")
'        Public Shared ReadOnly ComeHere As New GestureAnim(AnimSet, "come_here")
'        Public Shared ReadOnly ComeOn As New GestureAnim(AnimSet, "come_on")
'        Public Shared ReadOnly Damn As New GestureAnim(AnimSet, "damn")
'        Public Shared ReadOnly Despair As New GestureAnim(AnimSet, "despair")
'        Public Shared ReadOnly Disbelief As New GestureAnim(AnimSet, "disbelief")
'        Public Shared ReadOnly DontHitMe As New GestureAnim(AnimSet, "dont_hit_me")
'        Public Shared ReadOnly DontKnow As New GestureAnim(AnimSet, "dont_know")
'        Public Shared ReadOnly DoIt As New GestureAnim(AnimSet, "do_it")
'        Public Shared ReadOnly EasyNow As New GestureAnim(AnimSet, "easy_now")
'        Public Shared ReadOnly Enough As New GestureAnim(AnimSet, "enough")
'        Public Shared ReadOnly Exactly As New GestureAnim(AnimSet, "exactly")
'        Public Shared ReadOnly FoldArmsOhYeah As New GestureAnim(AnimSet, "fold_arms_oh_yeah")
'        Public Shared ReadOnly ForgetIt As New GestureAnim(AnimSet, "forget_it")
'        Public Shared ReadOnly Goddamn As New GestureAnim(AnimSet, "goddamn")
'        Public Shared ReadOnly Good As New GestureAnim(AnimSet, "good")
'        Public Shared ReadOnly GoAway As New GestureAnim(AnimSet, "go_away")
'        Public Shared ReadOnly HowCouldYou As New GestureAnim(AnimSet, "how_could_you")
'        Public Shared ReadOnly HowMuch As New GestureAnim(AnimSet, "how_much")
'        Public Shared ReadOnly ImTellingYou As New GestureAnim(AnimSet, "im_telling_you")
'        Public Shared ReadOnly IndicateListener As New GestureAnim(AnimSet, "indicate_listener")
'        Public Shared ReadOnly ItsOk As New GestureAnim(AnimSet, "its_ok")
'        Public Shared ReadOnly ICantSay As New GestureAnim(AnimSet, "i_cant_say")
'        Public Shared ReadOnly ICouldnt As New GestureAnim(AnimSet, "i_couldnt")
'        Public Shared ReadOnly IGiveUp As New GestureAnim(AnimSet, "i_give_up")
'        Public Shared ReadOnly ISaidNo As New GestureAnim(AnimSet, "i_said_no")
'        Public Shared ReadOnly IWill As New GestureAnim(AnimSet, "i_will")
'        Public Shared ReadOnly LetMeThink As New GestureAnim(AnimSet, "let_me_think")
'        Public Shared ReadOnly Negative As New GestureAnim(AnimSet, "negative")
'        Public Shared ReadOnly NodNo As New GestureAnim(AnimSet, "nod_no")
'        Public Shared ReadOnly NotMe As New GestureAnim(AnimSet, "not_me")
'        Public Shared ReadOnly NoChance As New GestureAnim(AnimSet, "no_chance")
'        Public Shared ReadOnly OhShit As New GestureAnim(AnimSet, "oh_shit")
'        Public Shared ReadOnly Ok As New GestureAnim(AnimSet, "ok")
'        Public Shared ReadOnly Please As New GestureAnim(AnimSet, "please")
'        Public Shared ReadOnly RaiseHands As New GestureAnim(AnimSet, "raise_hands")
'        Public Shared ReadOnly Shit As New GestureAnim(AnimSet, "shit")
'        Public Shared ReadOnly Shock As New GestureAnim(AnimSet, "shock")
'        Public Shared ReadOnly StopIt As New GestureAnim(AnimSet, "stop")
'        Public Shared ReadOnly Test As New GestureAnim(AnimSet, "Test")
'        Public Shared ReadOnly ThatWay As New GestureAnim(AnimSet, "that_way")
'        Public Shared ReadOnly ThisAndThat As New GestureAnim(AnimSet, "this_and_that")
'        Public Shared ReadOnly Unbelievable As New GestureAnim(AnimSet, "unbelievable")
'        Public Shared ReadOnly USerious As New GestureAnim(AnimSet, "u_serious")
'        Public Shared ReadOnly UThinImStupid As New GestureAnim(AnimSet, "u_thin_i'm_stupid")
'        Public Shared ReadOnly UUnderstand As New GestureAnim(AnimSet, "u_understand")
'        Public Shared ReadOnly We As New GestureAnim(AnimSet, "we")
'        Public Shared ReadOnly WeCanDoIt As New GestureAnim(AnimSet, "we_can_do_it")
'        Public Shared ReadOnly Whatever As New GestureAnim(AnimSet, "whatever")
'        Public Shared ReadOnly YeahIGotIt As New GestureAnim(AnimSet, "yeah_i_got_it")
'        Public Shared ReadOnly Yes As New GestureAnim(AnimSet, "yes")
'        Public Shared ReadOnly YoureRight As New GestureAnim(AnimSet, "youre_right")
'        Public Shared ReadOnly YouDig As New GestureAnim(AnimSet, "you_dig")
'        Public Shared ReadOnly YouWillLovethis As New GestureAnim(AnimSet, "you_will_love this")
'    End Class
'    Public Class Niko
'        Public Const AnimSet As String = "gestures@niko"

'        Public Shared ReadOnly Absolutely As New GestureAnim(AnimSet, "absolutely")
'        Public Shared ReadOnly Agree As New GestureAnim(AnimSet, "agree")
'        Public Shared ReadOnly Amazing As New GestureAnim(AnimSet, "amazing")
'        Public Shared ReadOnly AngerA As New GestureAnim(AnimSet, "anger_a")
'        Public Shared ReadOnly AreYouIn As New GestureAnim(AnimSet, "are_you_in")
'        Public Shared ReadOnly BringItOn As New GestureAnim(AnimSet, "bring_it_on")
'        Public Shared ReadOnly BringItToMe As New GestureAnim(AnimSet, "bring_it_to_me")
'        Public Shared ReadOnly ButWhy As New GestureAnim(AnimSet, "but_why")
'        Public Shared ReadOnly ComeHere As New GestureAnim(AnimSet, "come_here")
'        Public Shared ReadOnly ComeOn As New GestureAnim(AnimSet, "come_on")
'        Public Shared ReadOnly Damn As New GestureAnim(AnimSet, "damn")
'        Public Shared ReadOnly Despair As New GestureAnim(AnimSet, "despair")
'        Public Shared ReadOnly Disbelief As New GestureAnim(AnimSet, "disbelief")
'        Public Shared ReadOnly DontHitMe As New GestureAnim(AnimSet, "dont_hit_me")
'        Public Shared ReadOnly DontKnow As New GestureAnim(AnimSet, "dont_know")
'        Public Shared ReadOnly DoIt As New GestureAnim(AnimSet, "do_it")
'        Public Shared ReadOnly EasyNow As New GestureAnim(AnimSet, "easy_now")
'        Public Shared ReadOnly Enough As New GestureAnim(AnimSet, "enough")
'        Public Shared ReadOnly Exactly As New GestureAnim(AnimSet, "exactly")
'        Public Shared ReadOnly FoldArmsOhYeah As New GestureAnim(AnimSet, "fold_arms_oh_yeah")
'        Public Shared ReadOnly ForgetIt As New GestureAnim(AnimSet, "forget_it")
'        Public Shared ReadOnly GiveMeABreak As New GestureAnim(AnimSet, "give_me_a_break")
'        Public Shared ReadOnly Goddamn As New GestureAnim(AnimSet, "goddamn")
'        Public Shared ReadOnly Good As New GestureAnim(AnimSet, "good")
'        Public Shared ReadOnly GoAway As New GestureAnim(AnimSet, "go_away")
'        Public Shared ReadOnly Hello As New GestureAnim(AnimSet, "hello")
'        Public Shared ReadOnly Hey As New GestureAnim(AnimSet, "hey")
'        Public Shared ReadOnly HoldsUpFingers As New GestureAnim(AnimSet, "holds_up_fingers")
'        Public Shared ReadOnly How As New GestureAnim(AnimSet, "how")
'        Public Shared ReadOnly HowCouldYou As New GestureAnim(AnimSet, "how_could_you")
'        Public Shared ReadOnly HowMuch As New GestureAnim(AnimSet, "how_much")
'        Public Shared ReadOnly IfUSaySo As New GestureAnim(AnimSet, "if_u_say_so")
'        Public Shared ReadOnly IllDoIt As New GestureAnim(AnimSet, "ill_do_it")
'        Public Shared ReadOnly Imtalking2You As New GestureAnim(AnimSet, "im talking_2_you")
'        Public Shared ReadOnly ImBeggingYou As New GestureAnim(AnimSet, "im_begging_you")
'        Public Shared ReadOnly ImNotSure As New GestureAnim(AnimSet, "im_not_sure")
'        Public Shared ReadOnly ImSorry As New GestureAnim(AnimSet, "im_sorry")
'        Public Shared ReadOnly ImTellingYou As New GestureAnim(AnimSet, "im_telling_you")
'        Public Shared ReadOnly IndicateBack As New GestureAnim(AnimSet, "indicate_back")
'        Public Shared ReadOnly IndicateLeft As New GestureAnim(AnimSet, "indicate_left")
'        Public Shared ReadOnly IndicateListener As New GestureAnim(AnimSet, "indicate_listener")
'        Public Shared ReadOnly IndicateRightB As New GestureAnim(AnimSet, "indicate_right_b")
'        Public Shared ReadOnly IndicateRightC As New GestureAnim(AnimSet, "indicate_right_c")
'        Public Shared ReadOnly IsThisIt As New GestureAnim(AnimSet, "is_this_it")
'        Public Shared ReadOnly ItsDone As New GestureAnim(AnimSet, "its_done")
'        Public Shared ReadOnly ItsMine As New GestureAnim(AnimSet, "its_mine")
'        Public Shared ReadOnly ItsOk As New GestureAnim(AnimSet, "its_ok")
'        Public Shared ReadOnly IveForgot As New GestureAnim(AnimSet, "ive_forgot")
'        Public Shared ReadOnly ICantSay As New GestureAnim(AnimSet, "i_cant_say")
'        Public Shared ReadOnly ICouldnt As New GestureAnim(AnimSet, "i_couldnt")
'        Public Shared ReadOnly IDontHave As New GestureAnim(AnimSet, "i_dont_have")
'        Public Shared ReadOnly IDontThinkSo As New GestureAnim(AnimSet, "i_dont_think_so")
'        Public Shared ReadOnly IGetIt As New GestureAnim(AnimSet, "i_get_it")
'        Public Shared ReadOnly IGiveUp As New GestureAnim(AnimSet, "i_give_up")
'        Public Shared ReadOnly ISaidNo As New GestureAnim(AnimSet, "i_said_no")
'        Public Shared ReadOnly IWill As New GestureAnim(AnimSet, "i_will")
'        Public Shared ReadOnly KissMyAss As New GestureAnim(AnimSet, "kiss_my_ass")
'        Public Shared ReadOnly Later As New GestureAnim(AnimSet, "later")
'        Public Shared ReadOnly LeaveIt2Me As New GestureAnim(AnimSet, "leave_it_2_me")
'        Public Shared ReadOnly LetMeThink As New GestureAnim(AnimSet, "let_me_think")
'        Public Shared ReadOnly LikeThis As New GestureAnim(AnimSet, "like_this")
'        Public Shared ReadOnly MeGest As New GestureAnim(AnimSet, "me")
'        Public Shared ReadOnly Natuarally As New GestureAnim(AnimSet, "natuarally")
'        Public Shared ReadOnly Negative As New GestureAnim(AnimSet, "negative")
'        Public Shared ReadOnly NodNo As New GestureAnim(AnimSet, "nod_no")
'        Public Shared ReadOnly NodYes As New GestureAnim(AnimSet, "nod_yes")
'        Public Shared ReadOnly NotMe As New GestureAnim(AnimSet, "not_me")
'        Public Shared ReadOnly NotSure As New GestureAnim(AnimSet, "not_sure")
'        Public Shared ReadOnly NoChance As New GestureAnim(AnimSet, "no_chance")
'        Public Shared ReadOnly NoReally As New GestureAnim(AnimSet, "no_really")
'        Public Shared ReadOnly Numbnuts As New GestureAnim(AnimSet, "numbnuts")
'        Public Shared ReadOnly OfCourse As New GestureAnim(AnimSet, "of_course")
'        Public Shared ReadOnly OhShit As New GestureAnim(AnimSet, "oh_shit")
'        Public Shared ReadOnly Ok As New GestureAnim(AnimSet, "ok")
'        Public Shared ReadOnly OkOk As New GestureAnim(AnimSet, "ok_ok")
'        Public Shared ReadOnly OverThere As New GestureAnim(AnimSet, "over_there")
'        Public Shared ReadOnly PissOff As New GestureAnim(AnimSet, "piss_off")
'        Public Shared ReadOnly Please As New GestureAnim(AnimSet, "please")
'        Public Shared ReadOnly PointFwd As New GestureAnim(AnimSet, "point_fwd")
'        Public Shared ReadOnly PointRight As New GestureAnim(AnimSet, "point_right")
'        Public Shared ReadOnly Positive As New GestureAnim(AnimSet, "positive")
'        Public Shared ReadOnly RaiseHands As New GestureAnim(AnimSet, "raise_hands")
'        Public Shared ReadOnly SayAgain As New GestureAnim(AnimSet, "say_again")
'        Public Shared ReadOnly ScrewYou As New GestureAnim(AnimSet, "screw_you")
'        Public Shared ReadOnly Shit As New GestureAnim(AnimSet, "shit")
'        Public Shared ReadOnly Shock As New GestureAnim(AnimSet, "shock")
'        Public Shared ReadOnly ShutUp As New GestureAnim(AnimSet, "shut_up")
'        Public Shared ReadOnly StopIt As New GestureAnim(AnimSet, "stop")
'        Public Shared ReadOnly Sure As New GestureAnim(AnimSet, "sure")
'        Public Shared ReadOnly TellMeAboutIt As New GestureAnim(AnimSet, "tell_me_about_it")
'        Public Shared ReadOnly Test As New GestureAnim(AnimSet, "Test")
'        Public Shared ReadOnly That As New GestureAnim(AnimSet, "that")
'        Public Shared ReadOnly ThatWay As New GestureAnim(AnimSet, "that_way")
'        Public Shared ReadOnly ThisAndThat As New GestureAnim(AnimSet, "this_and_that")
'        Public Shared ReadOnly ThisBig As New GestureAnim(AnimSet, "this_big")
'        Public Shared ReadOnly Threaten As New GestureAnim(AnimSet, "threaten")
'        Public Shared ReadOnly Time As New GestureAnim(AnimSet, "time")
'        Public Shared ReadOnly Tosser As New GestureAnim(AnimSet, "tosser")
'        Public Shared ReadOnly TouchFace As New GestureAnim(AnimSet, "touch_face")
'        Public Shared ReadOnly ToHellWithIt As New GestureAnim(AnimSet, "to_hell_with_it")
'        Public Shared ReadOnly Unbelievable As New GestureAnim(AnimSet, "unbelievable")
'        Public Shared ReadOnly Uptight As New GestureAnim(AnimSet, "uptight")
'        Public Shared ReadOnly UCantDoThat As New GestureAnim(AnimSet, "u_cant_do_that")
'        Public Shared ReadOnly USerious As New GestureAnim(AnimSet, "u_serious")
'        Public Shared ReadOnly UTalkin2Me As New GestureAnim(AnimSet, "u_talkin_2_me")
'        Public Shared ReadOnly UThinImStupid As New GestureAnim(AnimSet, "u_thin_i'm_stupid")
'        Public Shared ReadOnly UUnderstand As New GestureAnim(AnimSet, "u_understand")
'        Public Shared ReadOnly WantSomeOfThis As New GestureAnim(AnimSet, "want_some_of_this")
'        Public Shared ReadOnly We As New GestureAnim(AnimSet, "we")
'        Public Shared ReadOnly Well As New GestureAnim(AnimSet, "well")
'        Public Shared ReadOnly WellAlright As New GestureAnim(AnimSet, "well_alright")
'        Public Shared ReadOnly WeCanDoIt As New GestureAnim(AnimSet, "we_can_do_it")
'        Public Shared ReadOnly What As New GestureAnim(AnimSet, "what")
'        Public Shared ReadOnly Whatever As New GestureAnim(AnimSet, "whatever")
'        Public Shared ReadOnly WhateverC As New GestureAnim(AnimSet, "whatever_c")
'        Public Shared ReadOnly Why As New GestureAnim(AnimSet, "why")
'        Public Shared ReadOnly WotTheFuck As New GestureAnim(AnimSet, "wot_the_fuck")
'        Public Shared ReadOnly YeahIGotIt As New GestureAnim(AnimSet, "yeah_i_got_it")
'        Public Shared ReadOnly Yes As New GestureAnim(AnimSet, "yes")
'        Public Shared ReadOnly YoureRight As New GestureAnim(AnimSet, "youre_right")
'        Public Shared ReadOnly YouDig As New GestureAnim(AnimSet, "you_dig")
'        Public Shared ReadOnly YouWillLovethis As New GestureAnim(AnimSet, "you_will_love this")
'    End Class
'End Class
'Public Shared ReadOnly Pickup As String = "~INPUT_PICKUP~"
'Public Shared ReadOnly Enter As String = "~INPUT_ENTER~"
'Public Shared ReadOnly Jump As String = "~INPUT_JUMP~"
'Public Shared ReadOnly Reload As String = "~INPUT_RELOAD~"
'Public Shared ReadOnly MoveUp As String = "~INPUT_MOVE_UP~"
'Public Shared ReadOnly MoveLeft As String = "~INPUT_MOVE_LEFT~"
'Public Shared ReadOnly MoveDown As String = "~INPUT_MOVE_DOWN~"
'Public Shared ReadOnly MoveRight As String = "~INPUT_MOVE_RIGHT~"
'Public Shared ReadOnly TextChatUniversal As String = "~INPUT_TEXT_CHAT_UNIVERSAL~"
'Public Shared ReadOnly TextChatResultsToggle As String = "~INPUT_TEXT_CHAT_RESULTS_TOGGLE~"
'Public Shared ReadOnly TextChatY As String = "~INPUT_TEXT_CHAT_Y~"
'Public Shared ReadOnly NextTrack As String = "~INPUT_NEXT_TRACK~"
'Public Shared ReadOnly PrevTrack As String = "~INPUT_PREV_TRACK~"
'Public Shared ReadOnly FrontendRefresh As String = "~INPUT_FRONTEND_REFRESH~"
'Public Shared ReadOnly FrontendLockLobby As String = "~INPUT_FRONTEND_LOCK_LOBBY~"
'Public Shared ReadOnly FrontendAccept As String = "~INPUT_FRONTEND_ACCEPT~"
'Public Shared ReadOnly FrontendCancel As String = "~INPUT_FRONTEND_CANCEL~"
'Public Shared ReadOnly FrontendLt As String = "~INPUT_FRONTEND_LT~"
'Public Shared ReadOnly FrontendRt As String = "~INPUT_FRONTEND_RT~"
'Public Shared ReadOnly FrontendLb As String = "~INPUT_FRONTEND_LB~"
'Public Shared ReadOnly FrontendRb As String = "~INPUT_FRONTEND_RB~"
'Public Shared ReadOnly FrontendPause As String = "~INPUT_FRONTEND_PAUSE~"
'Public Shared ReadOnly FrontendModelScreen As String = "~INPUT_FRONTEND_MODEL_SCREEN~"
'Public Shared ReadOnly FrontendLegend As String = "~INPUT_FRONTEND_LEGEND~"
'Public Shared ReadOnly FrontendReplayScreenshot As String = "~INPUT_FRONTEND_REPLAY_SCREENSHOT~"
'Public Shared ReadOnly FrontendDeleteFilter As String = "~INPUT_FRONTEND_DELETE_FILTER~"
'Public Shared ReadOnly FrontendApply As String = "~INPUT_FRONTEND_APPLY~"
'Public Shared ReadOnly FrontendVehicleToggle As String = "~INPUT_FRONTEND_VEHICLE_TOGGLE~"
'Public Shared ReadOnly ReplaySaveToHdd As String = "~INPUT_REPLAY_SAVE_TO_HDD~"
'Public Shared ReadOnly MouseUd As String = "~INPUT_MOUSE_UD~"
'Public Shared ReadOnly FreeAim As String = "~INPUT_FREE_AIM~"
'Public Shared ReadOnly VehicleHorn As String = "~INPUT_VEH_HORN~"
'Public Shared ReadOnly KbUp As String = "~INPUT_KB_UP~"
'Public Shared ReadOnly KbDown As String = "~INPUT_KB_DOWN~"
'Public Shared ReadOnly KbLeft As String = "~INPUT_KB_LEFT~"
'Public Shared ReadOnly KbRight As String = "~INPUT_KB_RIGHT~"
'Public Shared ReadOnly Sprint As String = "~INPUT_SPRINT~"
'Public Shared ReadOnly VehicleExit As String = "~INPUT_VEH_EXIT~"
'Public Shared ReadOnly PhoneAccept As String = "~INPUT_PHONE_ACCEPT~"
'Public Shared ReadOnly Attack As String = "~INPUT_ATTACK~"
'Public Shared ReadOnly DropWeapon As String = "~INPUT_DROP_WEAPON~"
'Public Shared ReadOnly VehicleHeadlight As String = "~INPUT_VEH_HEADLIGHT~"
'Public Shared ReadOnly VehicleHandbrake As String = "~INPUT_VEH_HANDBRAKE~"
'Public Shared ReadOnly MouseRmb As String = "~INPUT_MOUSE_RMB~"
'Public Shared ReadOnly Aim As String = "~INPUT_AIM~"
'Public Shared ReadOnly Cover As String = "~INPUT_COVER~"
'Public Shared ReadOnly Duck As String = "~INPUT_DUCK~"
'Public Shared ReadOnly FeReplayPreview As String = "~INPUT_FE_REPLAY_PREVIEW~"
'Public Shared ReadOnly FeReplayToggletime As String = "~INPUT_FE_REPLAY_TOGGLETIME~"
'Public Shared ReadOnly FeReplayToggletips As String = "~INPUT_FE_REPLAY_TOGGLETIPS~"
'Public Shared ReadOnly FrontendAxisBack As String = "~INPUT_FRONTEND_AXIS_BACK~"
'Public Shared ReadOnly FrontendAxisFwd As String = "~INPUT_FRONTEND_AXIS_FWD~"
'Public Shared ReadOnly FrontendAxisLeft As String = "~INPUT_FRONTEND_AXIS_LEFT~"
'Public Shared ReadOnly FrontendAxisRight As String = "~INPUT_FRONTEND_AXIS_RIGHT~"
'Public Shared ReadOnly FrontendCopy As String = "~INPUT_FRONTEND_COPY~"
'Public Shared ReadOnly FrontendCut As String = "~INPUT_FRONTEND_CUT~"
'Public Shared ReadOnly FrontendF6 As String = "~INPUT_FRONTEND_F6~"
'Public Shared ReadOnly FrontendMarkerDelete As String = "~INPUT_FRONTEND_MARKER_DELETE~"
'Public Shared ReadOnly FrontendMinigame1 As String = "~INPUT_FRONTEND_MINIGAME_1~"
'Public Shared ReadOnly FrontendMinigame2 As String = "~INPUT_FRONTEND_MINIGAME_2~"
'Public Shared ReadOnly FrontendMinigame3 As String = "~INPUT_FRONTEND_MINIGAME_3~"
'Public Shared ReadOnly FrontendMinigame4 As String = "~INPUT_FRONTEND_MINIGAME_4~"
'Public Shared ReadOnly FrontendPaste As String = "~INPUT_FRONTEND_PASTE~"
'Public Shared ReadOnly FrontendPlayerList As String = "~INPUT_FRONTEND_PLAYER_LIST~"
'Public Shared ReadOnly FrontendReplayAdvance As String = "~INPUT_FRONTEND_REPLAY_ADVANCE~"
'Public Shared ReadOnly FrontendReplayBack As String = "~INPUT_FRONTEND_REPLAY_BACK~"
'Public Shared ReadOnly FrontendReplayCyclemarkerleft As String = "~INPUT_FRONTEND_REPLAY_CYCLEMARKERLEFT~"
'Public Shared ReadOnly FrontendReplayCyclemarkerright As String = "~INPUT_FRONTEND_REPLAY_CYCLEMARKERRIGHT~"
'Public Shared ReadOnly FrontendReplayFfwd As String = "~INPUT_FRONTEND_REPLAY_FFWD~"
'Public Shared ReadOnly FrontendReplayHidehud As String = "~INPUT_FRONTEND_REPLAY_HIDEHUD~"
'Public Shared ReadOnly FrontendReplayNewmarker As String = "~INPUT_FRONTEND_REPLAY_NEWMARKER~"
'Public Shared ReadOnly FrontendReplayPause As String = "~INPUT_FRONTEND_REPLAY_PAUSE~"
'Public Shared ReadOnly FrontendReplayRestart As String = "~INPUT_FRONTEND_REPLAY_RESTART~"
'Public Shared ReadOnly FrontendReplayRewind As String = "~INPUT_FRONTEND_REPLAY_REWIND~"
'Public Shared ReadOnly FrontendReplayShowhotkey As String = "~INPUT_FRONTEND_REPLAY_SHOWHOTKEY~"
'Public Shared ReadOnly FrontendReplayTools As String = "~INPUT_FRONTEND_REPLAY_TOOLS~"
'Public Shared ReadOnly FrontendX As String = "~INPUT_FRONTEND_X~"
'Public Shared ReadOnly FrontendY As String = "~INPUT_FRONTEND_Y~"
'Public Shared ReadOnly MeleeAttack1 As String = "~INPUT_MELEE_ATTACK1~"
'Public Shared ReadOnly MeleeAttack2 As String = "~INPUT_MELEE_ATTACK2~"
'Public Shared ReadOnly MeleeAttack3 As String = "~INPUT_MELEE_ATTACK3~"
'Public Shared ReadOnly MeleeBlock As String = "~INPUT_MELEE_BLOCK~"
'Public Shared ReadOnly MeleeKick As String = "~INPUT_MELEE_KICK~"
'Public Shared ReadOnly MouseLmb As String = "~INPUT_MOUSE_LMB~"
'Public Shared ReadOnly MouseWheelDown As String = "~INPUT_MOUSE_WHEEL_DOWN~"
'Public Shared ReadOnly MouseWheelUp As String = "~INPUT_MOUSE_WHEEL_UP~"
'Public Shared ReadOnly NextCamera As String = "~INPUT_NEXT_CAMERA~"
'Public Shared ReadOnly NextWeapon As String = "~INPUT_NEXT_WEAPON~"
'Public Shared ReadOnly PhoneCancel As String = "~INPUT_PHONE_CANCEL~"
'Public Shared ReadOnly PhonePutAway As String = "~INPUT_PHONE_PUT_AWAY~"
'Public Shared ReadOnly PhoneTakeOut As String = "~INPUT_PHONE_TAKE_OUT~"
'Public Shared ReadOnly PrevWeapon As String = "~INPUT_PREV_WEAPON~"
'Public Shared ReadOnly SniperZoomInAlternate As String = "~INPUT_SNIPER_ZOOM_IN_ALTERNATE~"
'Public Shared ReadOnly SniperZoomIn As String = "~INPUT_SNIPER_ZOOM_IN~"
'Public Shared ReadOnly SniperZoomOutAlternate As String = "~INPUT_SNIPER_ZOOM_OUT_ALTERNATE~"
'Public Shared ReadOnly SniperZoomOut As String = "~INPUT_SNIPER_ZOOM_OUT~"
'Public Shared ReadOnly TurnOffRadio As String = "~INPUT_TURN_OFF_RADIO~"
'Public Shared ReadOnly VehicleAccelerate As String = "~INPUT_VEH_ACCELERATE~"
'Public Shared ReadOnly VehicleAttack2 As String = "~INPUT_VEH_ATTACK2~"
'Public Shared ReadOnly VehicleAttack As String = "~INPUT_VEH_ATTACK~"
'Public Shared ReadOnly VehicleBrake As String = "~INPUT_VEH_BRAKE~"
'Public Shared ReadOnly VehicleCinCam As String = "~INPUT_VEH_CIN_CAM~"
'Public Shared ReadOnly VehicleFlyPitchDown As String = "~INPUT_VEH_FLY_PITCH_DOWN~"
'Public Shared ReadOnly VehicleFlyPitchUp As String = "~INPUT_VEH_FLY_PITCH_UP~"
'Public Shared ReadOnly VehicleFlyRollLeft As String = "~INPUT_VEH_FLY_ROLL_LEFT~"
'Public Shared ReadOnly VehicleFlyRollRight As String = "~INPUT_VEH_FLY_ROLL_RIGHT~"
'Public Shared ReadOnly VehicleFlyThrottleDown As String = "~INPUT_VEH_FLY_THROTTLE_DOWN~"
'Public Shared ReadOnly VehicleFlyThrottleUp As String = "~INPUT_VEH_FLY_THROTTLE_UP~"
'Public Shared ReadOnly VehicleFlyYawLeft As String = "~INPUT_VEH_FLY_YAW_LEFT~"
'Public Shared ReadOnly VehicleFlyYawRight As String = "~INPUT_VEH_FLY_YAW_RIGHT~"
'Public Shared ReadOnly VehicleHotwireLeft As String = "~INPUT_VEH_HOTWIRE_LEFT~"
'Public Shared ReadOnly VehicleHotwireRight As String = "~INPUT_VEH_HOTWIRE_RIGHT~"
'Public Shared ReadOnly VehicleKeyUd As String = "~INPUT_VEH_KEY_UD~"
'Public Shared ReadOnly VehicleLookBehind As String = "~INPUT_VEH_LOOK_BEHIND~"
'Public Shared ReadOnly VehicleLookLeft As String = "~INPUT_VEH_LOOK_LEFT~"
'Public Shared ReadOnly VehicleLookRight As String = "~INPUT_VEH_LOOK_RIGHT~"
'Public Shared ReadOnly VehicleNextRadio As String = "~INPUT_VEH_NEXT_RADIO~"
'Public Shared ReadOnly VehicleNextWeapon As String = "~INPUT_VEH_NEXT_WEAPON~"
'Public Shared ReadOnly VehiclePrevRadio As String = "~INPUT_VEH_PREV_RADIO~"
'Public Shared ReadOnly ZoomRadar As String = "~INPUT_ZOOM_RADAR~"
'Public Shared ReadOnly MouseWheel As String = "~MOUSE_WHEEL~"
'Public Shared ReadOnly Mouse As String = "~MOUSE~"
'' Joypad
'Public Shared ReadOnly JoypadA As String = "~PAD_A~"
'Public Shared ReadOnly JoypadB As String = "~PAD_B~"
'Public Shared ReadOnly JoypadDown As String = "~PAD_DOWN~"
'Public Shared ReadOnly JoypadDpadAll As String = "~PAD_DPAD_ALL~"
'Public Shared ReadOnly JoypadDpadLeftright As String = "~PAD_DPAD_LEFTRIGHT~"
'Public Shared ReadOnly JoypadDpadLeft As String = "~PAD_DPAD_LEFT~"
'Public Shared ReadOnly JoypadDpadNone As String = "~PAD_DPAD_NONE~"
'Public Shared ReadOnly JoypadDpadRight As String = "~PAD_DPAD_RIGHT~"
'Public Shared ReadOnly JoypadDpadUpdown As String = "~PAD_DPAD_UPDOWN~"
'Public Shared ReadOnly JoypadLb As String = "~PAD_LB~"
'Public Shared ReadOnly JoypadLeft As String = "~PAD_LEFT~"
'Public Shared ReadOnly JoypadLshock As String = "~PAD_LSHOCK~"
'Public Shared ReadOnly JoypadLstickAll As String = "~PAD_LSTICK_ALL~"
'Public Shared ReadOnly JoypadLstickDown As String = "~PAD_LSTICK_DOWN~"
'Public Shared ReadOnly JoypadLstickLeftrightVehicle As String = "~PAD_LSTICK_LEFTRIGHT_VEH~"
'Public Shared ReadOnly JoypadLstickLeftright As String = "~PAD_LSTICK_LEFTRIGHT~"
'Public Shared ReadOnly JoypadLstickLeft As String = "~PAD_LSTICK_LEFT~"
'Public Shared ReadOnly JoypadLstickNone As String = "~PAD_LSTICK_NONE~"
'Public Shared ReadOnly JoypadLstickRight As String = "~PAD_LSTICK_RIGHT~"
'Public Shared ReadOnly JoypadLstickUpdown As String = "~PAD_LSTICK_UPDOWN~"
'Public Shared ReadOnly JoypadLstickUp As String = "~PAD_LSTICK_UP~"
'Public Shared ReadOnly JoypadLt As String = "~PAD_LT~"
'Public Shared ReadOnly JoypadRb As String = "~PAD_RB~"
'Public Shared ReadOnly JoypadRight As String = "~PAD_RIGHT~"
'Public Shared ReadOnly JoypadRshock As String = "~PAD_RSHOCK~"
'Public Shared ReadOnly JoypadRstickAll As String = "~PAD_RSTICK_ALL~"
'Public Shared ReadOnly JoypadRstickDown As String = "~PAD_RSTICK_DOWN~"
'Public Shared ReadOnly JoypadRstickLeftright As String = "~PAD_RSTICK_LEFTRIGHT~"
'Public Shared ReadOnly JoypadRstickLeft As String = "~PAD_RSTICK_LEFT~"
'Public Shared ReadOnly JoypadRstickNone As String = "~PAD_RSTICK_NONE~"
'Public Shared ReadOnly JoypadRstickUpdownOnly As String = "~PAD_RSTICK_UPDOWN_ONLY~"
'Public Shared ReadOnly JoypadRstickUpdown As String = "~PAD_RSTICK_UPDOWN~"
'Public Shared ReadOnly JoypadRstickUp As String = "~PAD_RSTICK_UP~"
'Public Shared ReadOnly JoypadRt As String = "~PAD_RT~"
'Public Shared ReadOnly JoypadSelect As String = "~PAD_SELECT~"
'Public Shared ReadOnly JoypadSixaxisDrive As String = "~PAD_SIXAXIS_DRIVE~"
'Public Shared ReadOnly JoypadSixaxisPitch As String = "~PAD_SIXAXIS_PITCH~"
'Public Shared ReadOnly JoypadSixaxisReload As String = "~PAD_SIXAXIS_RELOAD~"
'Public Shared ReadOnly JoypadSixaxisRoll As String = "~PAD_SIXAXIS_ROLL~"
'Public Shared ReadOnly JoypadStart As String = "~PAD_START~"
'Public Shared ReadOnly JoypadUp As String = "~PAD_UP~"
'Public Shared ReadOnly JoypadX As String = "~PAD_X~"
'Public Shared ReadOnly JoypadY As String = "~PAD_Y~"
'Public Shared ReadOnly JoypadDpadUp As String = "~PAD_DPAD_UP~"
'Public Shared ReadOnly JoypadDpadDown As String = "~PAD_DPAD_DOWN~"
'Public Shared ReadOnly JoypadBack As String = "~PAD_BACK~"

'Public Shared Sub ControlTrafficAndPedDensity(CarDensity As Single, PedDensity As Single, SwitchRoadsOFF As Boolean) ', Planes As Boolean, PoliceHelis As Boolean, Trains As Boolean, Boats As Boolean, GarbageTrucks As Boolean, MadDrivers As Boolean)
'    [Call]("SET_CAR_DENSITY_MULTIPLIER", CarDensity)
'    [Call]("SET_PARKED_CAR_DENSITY_MULTIPLIER", CarDensity)
'    [Call]("SET_RANDOM_CAR_DENSITY_MULTIPLIER", CarDensity)
'    [Call]("SET_PED_DENSITY_MULTIPLIER", PedDensity)
'    'ControlAmbientVehicles(Planes, PoliceHelis, Trains, Boats, GarbageTrucks, MadDrivers)
'    'If SwitchRoadsOFF Then NativeGeneric.SwitchRoadsOFF(New Vector3(0, 0, 0), New Vector3(5000, 5000, 200)) Else NativeGeneric.SwitchRoadsON(New Vector3(0, 0, 0), New Vector3(5000, 5000, 200))
'    NativeGeneric.SwitchRoadsOnOFF(Not SwitchRoadsOFF, New Vector3(0, 0, 0), New Vector3(5000, 5000, 200))
'    'If DisableRadio = True Then EnableFrontedRadio(False)
'End Sub

Public Class TEST

End Class

'If (g_U481._fU160 == 0)
'    {
'        If (g_U91._fU40 == 2)
'        {
'            StrCopy(ref cVar2, "phone3D_HQ", 16);
'        }
'        REQUEST_STREAMED_TXD(ref cVar2, 1);
'        sub_1814("\n requested streamed txd - ");
'        sub_1814("mainTXD");
'        bVar14 = True;
'        sub_8345(1, ref g_U91._fU8 );
'        sub_8975();
'        CREATE_MOBILE_PHONE(2);
'        If (Not IS_NETWORK_SESSION())
'        {
'            REQUEST_STREAMED_TXD(ref cVar6, 1);
'            bVar14 = False;
'        }
'        While ((Not bVar14) || (Not (HAS_STREAMED_TXD_LOADED( ref cVar2 ))))
'        {
'            WAIT(0);
'            If (Not bVar14)
'            {
'                If (HAS_STREAMED_TXD_LOADED(ref cVar6 ))
'                {
'                    bVar14 = True;
'                }
'            }
'        }
'        g_U481._fU0[0] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "menuArrow" );
'        g_U481._fU0[1] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad1" );
'        g_U481._fU0[2] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad2" );
'        g_U481._fU0[3] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad3" );
'        g_U481._fU0[4] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad4" );
'        g_U481._fU0[5] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad5" );
'        g_U481._fU0[6] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad6" );
'        g_U481._fU0[7] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad7" );
'        g_U481._fU0[8] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad8" );
'        g_U481._fU0[9] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad9" );
'        g_U481._fU0[10] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypadStar" );
'        g_U481._fU0[11] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypad0" );
'        g_U481._fU0[12] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypadHash" );
'        g_U481._fU0[13] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypadPhoneLeft" );
'        g_U481._fU0[14] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "keypadPhoneRight" );
'        g_U481._fU0[15] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "alertMsg" );
'        g_U481._fU0[16] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "alertBell" );
'        g_U481._fU0[17] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "alertInvite" );
'        g_U481._fU0[18] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "inGTA" );
'        If (g_U91._fU40 == 2)
'        {
'            g_U481._fU0[19] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "envelope1" );
'            g_U481._fU0[20] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "envelope2" );
'            g_U481._fU160 = 21;
'            sub_9783(0);
'        }
'        Else
'        {
'            g_U481._fU0[19] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar2, "background" );
'            g_U481._fU160 = 20;
'        }
'        If ((Not IS_NETWORK_SESSION()) And (Not g_U481._fU164))
'        {
'            g_U481._fU88[0] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad1" );
'            g_U481._fU88[1] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad2" );
'            g_U481._fU88[2] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad3" );
'            g_U481._fU88[3] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad4" );
'            g_U481._fU88[4] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad5" );
'            g_U481._fU88[5] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad6" );
'            g_U481._fU88[6] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad7" );
'            g_U481._fU88[7] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad8" );
'            g_U481._fU88[8] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad9" );
'            g_U481._fU88[9] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypadStar" );
'            g_U481._fU88[10] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypad0" );
'            g_U481._fU88[11] = GET_TEXTURE_FROM_STREAMED_TXD( ref cVar6, "dialkeypadHash" );
'            g_U481._fU140 = GET_TXD("buttons");
'            If ((# -NULL-0xc27bfa()) || (# -NULL-0xc27c84()))
'            {
'                g_U481._fU144[0] = GET_TEXTURE( g_U481._fU140, "A_butt" );
'                g_U481._fU144[1] = GET_TEXTURE( g_U481._fU140, "X_butt" );
'                g_U481._fU144[2] = GET_TEXTURE( g_U481._fU140, "B_butt" );
'            }
'            Else
'            {
'                g_U481._fU144[0] = GET_TEXTURE( g_U481._fU140, "p_Cross" );
'                g_U481._fU144[1] = GET_TEXTURE( g_U481._fU140, "p_Square" );
'                g_U481._fU144[2] = GET_TEXTURE( g_U481._fU140, "p_Circle" );
'            }
'            g_U481._fU164 = 1;
'        }
'        sub_1814("\n streamed txd's loaded");
'    }
'    Return;
'}

'void sub_247115()
'{
'    unknown uVar2;
'    unknown uVar3;
'    unknown uVar4;
'    unknown uVar5;
'    unknown uVar6;
'    Char[32] cVar7;
'    unknown uVar15;
'    float fVar16;
'    float fVar17;
'    float fVar18;
'    unknown uVar19;

'    SET_WIDESCREEN_FORMAT(3);
'    GET_MOUSE_POSITION(ref uVar15, ref fVar16 );
'    fVar17 = 0.00000000;
'    fVar18 = sub_246579();
'    If ((fVar18 < 1.26) And (fVar18 > 1.24))
'    {
'        fVar17 = l_U7394;
'    }
'    DRAW_SPRITE(l_U6251[0], (sub_124158() - (l_U213 / 2)) + fVar17, l_U7390 - (l_U214 / 2), l_U213, l_U214, 0.00000000, l_U81, l_U81, l_U81, 255);
'    If ((fVar16 > (l_U7393 + (l_U214 / 2))) And (# -NULL-0xc27bfa()))
'    {
'        sub_141432((sub_124158() - (l_U213 / 2)) + fVar17, l_U7390 - (l_U214 / 2), 0);
'    }
'    SET_TEXT_SCALE(l_U164, l_U165);
'    sub_50369(0, 0.00000000, 1.0, 0, 0, 0, 0, 0, 255);
'    If (Not (l_U7367 == 2))
'    {
'        StrCopy(ref cVar7, sub_247394(), 32);
'    }
'    Else
'    {
'        String(ref cVar7, sub_247462(l_U7381), 32);
'        ConcatString(ref cVar7, sub_247583(sub_247462(l_U7381), 0), 32);
'        ConcatString(ref cVar7, " : ", 32);
'        ConcatString(ref cVar7, sub_247394(), 32);
'    }
'    SET_TEXT_USE_UNDERSCORE(1);
'    uVar19 = GET_STRING_WIDTH_WITH_STRING("STRING", ref cVar7 );
'    sub_248027(ref uVar2, ref uVar3, ref uVar4 );
'    SET_TEXT_COLOUR(uVar2, uVar3, uVar4, 255);
'    sub_50644((sub_124158() - l_U213) - l_U7392, l_U7390 + l_U7391, l_U164, l_U165, uVar2, uVar3, uVar4, 255, 3, ref cVar7, 0);
'    SET_TEXT_USE_UNDERSCORE(0);
'    DRAW_SPRITE(l_U6251[0], (((sub_124158() - (l_U213 * 1.5)) - uVar19) - (l_U7392 * 2)) + fVar17, l_U7390 - (l_U214 / 2), l_U213 * -1, l_U214, 0.00000000, l_U81, l_U81, l_U81, 255);
'    If ((fVar16 > (l_U7393 + (l_U214 / 2))) And (# -NULL-0xc27bfa()))
'    {
'        sub_141432((((sub_124158() - (l_U213 * 1.5)) - uVar19) - (l_U7392 * 2)) + fVar17, l_U7390 - (l_U214 / 2), 1);
'    }
'    Return;
'}



'GET_CAR_SPEED_VECTOR(uParam0, ref vVar3, 1);

'NITRO (SCOCL)

void sub_70371(unknown uParam0, unknown uParam1, int iParam2, unknown uParam3, unknown uParam4)
{
    int I;
    int iVar8;
    Boolean bVar9;
    vector vVar10;
    vector vVar13;
    int iVar16;
    int iVar17;
    unknown uVar18;

    I = 0;
    iVar8 = -1;
    If ((uParam3) || (iParam2->_fU8))
    {
        iVar16 = sub_66043();
        If (iVar16 > l_U137[uParam0]._fU52._fU20)
        {
            GENERATE_RANDOM_INT_IN_RANGE(20, 400, ref iVar17 );
            l_U137[uParam0]._fU52._fU20 = iVar16 + iVar17;
            sub_67248(100, iVar17 / 4);
            sub_66437(uParam1, 0.25, 60);
            uVar18 = GET_SOUND_ID();
            PLAY_SOUND_FROM_VEHICLE(uVar18, "MP_NITROUS_POPS", uParam1);
            RELEASE_SOUND_ID(uVar18);
            For (I = 0; I < 4; I++ )
            {
                bVar9 = GET_VEHICLE_COMPONENT_INFO(uParam1, sub_69599(I), ref vVar10, ref vVar13, ref iVar8, 1);
                If (bVar9)
                {
                    If (sub_69803(uParam1))
                    {
                        TRIGGER_PTFX_ON_VEH("nitro_splutter_dub", uParam1, vVar10, vVar13, 1.0);
                    }
                    ElseIf (sub_69928(uParam1))
                    {
                        TRIGGER_PTFX_ON_VEH("nitro_splutter_dukes", uParam1, vVar10, vVar13, 1.0);
                    }
                    Else
                    {
                        TRIGGER_PTFX_ON_VEH("nitro_splutter", uParam1, vVar10, vVar13, 1.0);
                    }
                    GET_VEHICLE_COMPONENT_INFO(uParam1, sub_69599(I), ref vVar10, ref vVar13, ref iVar8, 0);
                    GET_CAR_FORWARD_VECTOR(uParam1, ref vVar13 );
                    vVar10 = {vVar10 + ((vVar13 * (vector(0.45, 0.45, 0.00000000))) * -1.0)};
                    If (Not (sub_70833(uParam1)))
                    {
                        vVar10.z += 0.1;
                    }
                    sub_70895(vVar10);
                }
            }
        }
    }
    Return;
}


void sub_65094()
{
    vector vVar2;
    float fVar5;
    unknown uVar6;
    int iVar7;

    vVar2 = {0.00000000, -1.0, 0.00000000};
    fVar5 = 0.00000000;
    iVar7 = nil;
    l_U57 = 0;
    If ((IS_CONTROL_PRESSED(0, 41)) || ((IS_CONTROL_PRESSED( 0, 45 )) || (IS_CONTROL_PRESSED( 0, 44 ))))
    {
        If (IS_PLAYER_SCRIPT_CONTROL_ON(sub_28067()))
        {
            If (Not (IS_CHAR_DEAD(sub_2475())))
            {
                If (IS_CHAR_IN_ANY_CAR(sub_2475()))
                {
                    GET_CAR_CHAR_IS_USING(sub_2475(), ref uVar6 );
                    GET_DRIVER_OF_CAR(uVar6, ref iVar7 );
                    If ((sub_2475() == iVar7) And ((IS_VEH_DRIVEABLE(uVar6)) And (DOES_VEHICLE_EXIST(uVar6))))
                    {
                        If ((IS_VEHICLE_ON_ALL_WHEELS(uVar6)) And ((Not (sub_65299(uVar6))) And (Not (IS_CAR_IN_AIR_PROPER(uVar6)))))
                        {
                            GET_CAR_SPEED(uVar6, ref fVar5 );
                            If ((fVar5 > 12.5) And (fVar5 < 55.0))
                            {
                                l_U57 = 1;
                                vVar2 = {vVar2 * (fVar5 * 0.6)};
                                APPLY_FORCE_TO_CAR(uVar6, 0, vVar2, 0.00000000, 0.00000000, 0.00000000, 0, 1, 1, 1);
                            }
                        }
                    }
                }
            }
        }
    }
    Return l_U57;
}
