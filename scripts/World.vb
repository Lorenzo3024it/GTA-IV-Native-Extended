Imports System.Security.Policy
Imports GTA
Imports GTA.base
'Imports GTA.Math
Imports GTA.Native.Function
'Imports NativeExtended.NativeGarage
Public Class NativeWorld
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub AddHospitalRestart(Coordinate As GTA.Vector3, FacingAngle As Double, HospitalID As Integer)
        [Call]("ADD_HOSPITAL_RESTART", Coordinate.X, Coordinate.Y, Coordinate.Z, FacingAngle, HospitalID)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub AddPoliceRestart(Coordinate As GTA.Vector3, FacingAngle As Double, PoliceStationID As Integer)
        [Call]("ADD_POLICE_RESTART", Coordinate.X, Coordinate.Y, Coordinate.Z, FacingAngle, PoliceStationID)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    ''' <param name="Coordinate"></param>
    ''' <param name="Range"></param>
    ''' Not sure about this, but it should be the radius of area
    Public Shared Sub AddSpawnBlockingArea(Coordinate As GTA.Vector3, Range As Single)
        [Call]("ADD_SPAWN_BLOCKING_AREA", Coordinate.X, Coordinate.Y, Coordinate.Z, Range)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    ''' <param name="MinStartPosition"></param>
    ''' <param name="MaxStartPosition"></param>
    ''' <param name="MinEndPosition"></param>
    ''' <param name="MaxEndPosition"></param>
    ''' <param name="CameraPosition"></param>
    ''' <param name="CashReward"></param>
    Public Shared Sub AddStuntJump(MinStartPosition As Vector3, MaxStartPosition As Vector3, MinEndPosition As Vector3, MaxEndPosition As Vector3, CameraPosition As Vector3, CashReward As Integer)
        [Call]("ADD_STUNT_JUMP", MinStartPosition.X, MinStartPosition.Y, MinStartPosition.Z, MaxStartPosition.X, MaxStartPosition.Y, MaxStartPosition.Z, MinEndPosition.X, MinEndPosition.Y, MinEndPosition.Z, MaxEndPosition.X, MaxEndPosition.Y, MaxEndPosition.Z, CameraPosition.X, CameraPosition.Y, CameraPosition.Z, CashReward)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub AllowEmergencyServices(Optional Allow As Boolean = True)
        [Call]("ALLOW_EMERGENCY_SERVICES", Allow)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    Public Shared Sub AllowStuntJumps(Optional AllowToTrigger As Boolean = True)
        [Call]("ALLOW_STUNT_JUMPS_TO_TRIGGER", AllowToTrigger)
    End Sub
    Public Shared Sub ClearArea(Coordinate As GTA.Vector3, Radius As Single, UnknownFlag As Boolean, Optional ClearPeds As Boolean = True, Optional ClearCars As Boolean = True, Optional ClearCops As Boolean = True, Optional ClearObjects As Boolean = True)
        If ClearPeds = True AndAlso ClearCars = True AndAlso ClearCops = True Then
            [Call]("CLEAR_AREA", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius, UnknownFlag)
        Else
            If ClearPeds Then [Call]("CLEAR_AREA_OF_CHARS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
            If ClearCars Then [Call]("CLEAR_AREA_OF_CARS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
            If ClearCops Then [Call]("CLEAR_AREA_OF_COPS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
            If ClearObjects Then [Call]("CLEAR_AREA_OF_OBJECTS", Coordinate.X, Coordinate.Y, Coordinate.Z, Radius)
        End If
    End Sub
    Public Shared Sub ClearTimecycleModifier()
        [Call]("CLEAR_TIMECYCLE_MODIFIER")
    End Sub
    Public Shared Sub ControlAmbientVehicles(Planes As Boolean, Optional PoliceHelis As Boolean = True, Optional Boats As Boolean = True,
                                             Optional GarbageTrucks As Boolean = True, Optional MadDrivers As Boolean = True, Optional EmergencyServices As Boolean = True, Optional Trains As Boolean = True, Optional DeleteAllTrains As Boolean = False)
        [Call]("SWITCH_AMBIENT_PLANES", Planes)
        [Call]("SWITCH_POLICE_HELIS", PoliceHelis)
        [Call]("SWITCH_RANDOM_BOATS", Boats)
        [Call]("SWITCH_RANDOM_TRAINS", Trains)
        [Call]("SWITCH_GARBAGE_TRUCKS", GarbageTrucks)
        [Call]("SWITCH_MAD_DRIVERS", MadDrivers)
        If Trains = False Then [Call]("SWITCH_RANDOM_TRAINS", Trains)
        If DeleteAllTrains Then [Call]("DELETE_ALL_TRAINS")
    End Sub
    Public Shared Sub ControlPedDensity(PedDensity As Single)
        [Call]("SET_PED_DENSITY_MULTIPLIER", PedDensity)
    End Sub
    ''' <summary>
    ''' Change traffic and parked cars density, with the options to switch Roads off for entire map.
    ''' It combines SET_CAR_DENSITY_MULTIPLIER with SET_RANDOM_CAR_DENSITY_MULTIPLIER, and also SET_PARKED_CAR_DENSITY_MULTIPLIER.
    ''' </summary>
    ''' <param name="CarDensity">Values from 0.0 to 10.0</param>
    ''' <param name="ParkedCarDensity">Values from 0.0 to 10.0. If set to -1 the parked car density won't be affected.</param>
    ''' <param name="RandomCarDensity">Values from 0.0 to 10.0. If set to -1 it will the same as CarDensity, if set to -2 it won't be affected.</param>
    Public Shared Sub ControlTrafficDensity(CarDensity As Single, Optional SwitchRoadsOFF As Boolean = False, Optional ParkedCarDensity As Single = -1, Optional RandomCarDensity As Single = -1)
        [Call]("SET_CAR_DENSITY_MULTIPLIER", CarDensity)
        If RandomCarDensity = -1 Then
            [Call]("SET_RANDOM_CAR_DENSITY_MULTIPLIER", CarDensity)
        ElseIf RandomCarDensity > -1 Then
            [Call]("SET_RANDOM_CAR_DENSITY_MULTIPLIER", RandomCarDensity)
        End If
        If ParkedCarDensity >= 0 Then [Call]("SET_PARKED_CAR_DENSITY_MULTIPLIER", ParkedCarDensity)
        SwitchRoads(Not SwitchRoadsOFF, New Vector3(0, 0, 0), New Vector3(5000, 5000, 200))
    End Sub
    Public Shared Sub CreateExplosion(Position As GTA.Vector3, ExplosionType As Integer, Radius As Single, Audio As Boolean, Visible As Boolean, CameraShake As Single)
        [Call]("ADD_EXPLOSION", Position.X, Position.Y, Position.Z, ExplosionType, Radius, Audio, Not Visible, CameraShake)
    End Sub
    Public Shared Sub EnableFancyWater(Optional Enable As Boolean = True)
        [Call]("ENABLE_FANCY_WATER", Enable)
    End Sub
    Public Shared Sub EnableRomansTaxiGarageRadio(Optional Enable As Boolean = True)
        [Call]("SET_TAXI_GARAGE_RADIO_STATE", Enable)
    End Sub
    Public Shared Sub EnableSaveHouse(SavehouseID As Integer, Optional Enable As Boolean = True)
        [Call]("ENABLE_SAVE_HOUSE", SavehouseID, Enable)
    End Sub
    Public Shared Sub ForceParkedCarTooCloseToOthers(Enable As Boolean)
        [Call]("FORCE_GENERATE_PARKED_CARS_TOO_CLOSE_TO_OTHERS", Enable)
    End Sub
    ''' <summary>
    ''' Untested
    ''' </summary>
    ''' <param name="Unknown"></param>
    ''' Values from -1.0 to 1.0
    Public Shared Sub ForceWind(Unknown As Single)
        [Call]("FORCE_WIND", Unknown)
    End Sub
    Public Shared Function GetClosestCarNode_WithHeading(Coordinates As GTA.Vector3) As (Vector3, Single)
        Dim pointPos1, pointPos2, pointPos3, pointHeading As Native.Pointer
        pointPos1 = New Native.Pointer(GetType(Single))
        pointPos2 = New Native.Pointer(GetType(Single))
        pointPos3 = New Native.Pointer(GetType(Single))
        pointHeading = New Native.Pointer(GetType(Single))
        [Call]("GET_CLOSEST_CAR_NODE_WITH_HEADING", Coordinates.X, Coordinates.Y, Coordinates.Z, pointPos1, pointPos2, pointPos3, pointHeading)
        Return (New Vector3(pointPos1.Value, pointPos2.Value, pointPos3.Value), pointHeading.Value)
    End Function
    Public Shared Function GetCurrentWeather() As Weather
        Dim point As Native.Pointer = New Native.Pointer(GetType(Weather))
        [Call](Of Weather)("GET_CURRENT_WEATHER", point)
        Return point.Value
    End Function
    Public Shared Function GetDistance(Position1 As Vector3, Position2 As Vector3) As Single
        Dim Pointer As Native.Pointer = New Native.Pointer(GetType(Single))
        Return [Call](Of Single)("GET_DISTANCE_BETWEEN_COORDS_3D", Position1.X, Position1.Y, Position1.Z, Position2.X, Position2.Y, Position2.Z, Pointer)
        Return Pointer.Value
    End Function
    Public Shared Function GetInteriorFromPed(Ped As Ped) As Integer
        Return [Call](Of Integer)("GET_INTERIOR_FROM_CHAR", Ped)
    End Function
    Public Shared Function GetInteriorFromCar(Car As Vehicle) As Integer
        Return [Call](Of Integer)("GET_INTERIOR_FROM_CAR", Car)
    End Function
    Public Shared Function GetNameOfZone(Position As Vector3, Optional InfoZone As Boolean = False) As String
        If InfoZone Then
            Return [Call](Of String)("GET_NAME_OF_INFO_ZONE", Position.X, Position.Y, Position.Z)
        Else
            Return [Call](Of String)("GET_NAME_OF_ZONE", Position.X, Position.Y, Position.Z)
        End If
    End Function
    Public Shared Function GetWaterHeight(Position As Vector3, Optional NoWaves As Boolean = False) As Single
        Dim tFloat As Native.Pointer = New Native.Pointer(GetType(Single))
        If NoWaves Then [Call]("GET_WATER_HEIGHT_NO_WAVES", Position.X, Position.Y, Position.Z, tFloat) Else [Call]("GET_WATER_HEIGHT", Position.X, Position.Y, Position.Z, tFloat)
        Return tFloat.Value
    End Function
    Public Shared Sub LoadPathNodes(LoadAll As Boolean, Optional Unknown1 As Single = 0, Optional Unknown2 As Single = 0, Optional Unknown3 As Single = 0, Optional Unknown4 As Single = 0)
        If LoadAll Then
            [Call]("LOAD_ALL_PATH_NODES", LoadAll)
        Else
            [Call]("LOAD_PATH_NODES_IN_AREA", Unknown1, Unknown2, Unknown3, Unknown4)
        End If
    End Sub
    Public Shared Sub MuteWorldAudio(Optional Mute As Boolean = True, Optional MuteForTV As Boolean = False)
        If MuteForTV Then
            [Call]("MUTE_GAMEWORLD_AND_POSITIONED_RADIO_FOR_TV", MuteForTV)
        Else
            [Call]("MUTE_GAMEWORLD_AUDIO", Mute)
        End If
    End Sub
    Public Shared Sub RelasePathNodes()
        [Call]("RELEASE_PATH_NODES")
    End Sub
    Public Shared Sub LoadIPL(IPL As String)
        [Call]("REQUEST_IPL", IPL)
        [Call]("LOAD_ALL_OBJECTS_NOW")
    End Sub
    Public Shared Sub RemoveIPL(IPL As String, Discreetly As Boolean) 'testare
        If Discreetly Then [Call]("REMOVE_IPL_DISCREETLY", IPL) Else [Call]("REMOVE_IPL", IPL)
    End Sub
    'Public Shared Sub RequestInteriorModels() 'testare
    ' [Call]("REQUEST_INTERIOR_MODELS", hash as integer, string as object name?)
    ' End Sub
    Public Shared Sub SetFreeHealthCare(Free As Boolean)
        [Call]("SET_FREE_HEALTH_CARE", nativeplayer.index, Free)
    End Sub
    Public Shared Sub SetTimeOfDay(Hour As Integer, Minute As Integer, Optional ForceTimeDay As Boolean = False)
        If ForceTimeDay = False Then [Call]("SET_TIME_OF_DAY", Hour, Minute) Else [Call]("FORCE_TIME_OF_DAY", Hour, Minute)
    End Sub
    Public Shared Sub SetZonePopulationType(Zone As String, PopulationType As Integer)
        [Call]("SET_ZONE_POPULATION_TYPE", Zone, PopulationType)
    End Sub
    Public Shared Sub SetZoneScumminess(Zone As String, Unknown As Integer)
        [Call]("SET_ZONE_SCUMMINESS", Zone, Unknown)
    End Sub
    Public Shared Sub SetTimecycleModifier(Modifier As String)
        [Call]("SET_TIMECYCLE_MODIFIER", Modifier)
    End Sub
    Public Shared Sub SwapNearestBuildingModel(Position As Vector3, Unknown1 As Single, Model_old As String, Model_new As String)
        [Call]("SWAP_NEAREST_BUILDING_MODEL", Position.X, Position.Y, Position.Z, Unknown1, NativeModels.getHashKey(Model_old), NativeModels.getHashKey(Model_new))
    End Sub
    ''' <summary>
    ''' Disables boats, but not parked cars. Easy way to disable roads without affect traffic density.
    ''' </summary>
    ''' <param name="RoadsON">False to disable roads, true to switch roads back to normal.</param>
    ''' <param name="MinCoordinates">If set to "nothing" (default value), it will be set to Vector3.Zero</param>
    ''' <param name="MaxCoordinates">If set to "nothing" (default value), it will be set to New Vector3(5000, 5000, 200), which should cover the entire map.</param>
    Public Shared Sub SwitchRoads(RoadsON As Boolean, Optional MinCoordinates As GTA.Vector3 = Nothing, Optional MaxCoordinates As GTA.Vector3 = Nothing)
        Dim MaxCoords, MinCoords As Vector3
        If MinCoordinates = Nothing Then
            MinCoords = New Vector3(0, 0, 0)
        Else
            MinCoords = MaxCoordinates
        End If
        If MinCoordinates = Nothing Then
            MaxCoords = New Vector3(5000, 5000, 200)
        Else
            MaxCoords = MaxCoordinates
        End If
        If RoadsON Then
            [Call]("SWITCH_ROADS_BACK_TO_ORIGINAL", MinCoordinates.X, MinCoordinates.Y, MinCoordinates.Z, MaxCoordinates.X, MaxCoordinates.Y, MaxCoordinates.Z)
        Else
            [Call]("SWITCH_ROADS_OFF", MinCoordinates.X, MinCoordinates.Y, MinCoordinates.Z, MaxCoordinates.X, MaxCoordinates.Y, MaxCoordinates.Z)
        End If
    End Sub
    Public Shared Sub WeatherControl(RandomWeather As Boolean, InstantChange As Boolean, Optional Weather As GTA.Weather = 0, Optional KeepForced As Boolean = False)
        Dim WeatherSet As Weather
        If RandomWeather Then
            Dim RandomNum As New Random
            WeatherSet = RandomNum.Next(9)
            If InstantChange Then [Call]("FORCE_WEATHER_NOW", WeatherSet) Else [Call]("FORCE_WEATHER", WeatherSet)
            If KeepForced = False Then [Call]("RELEASE_WEATHER")
        Else
            If InstantChange Then [Call]("FORCE_WEATHER_NOW", Weather) Else [Call]("FORCE_WEATHER", Weather)
            If KeepForced = False Then [Call]("RELEASE_WEATHER")
        End If
    End Sub
    Public Structure TimecycleModifiers
        Public Const NoAmbient As String = "noambient"
        Public Const NoAmbientmult As String = "NoAmbientmult"
        Public Const Qwnomoon As String = "qwnomoon"
        Public Const Qw2nomoon As String = "qw2nomoon"
        Public Const Brook_S2_TC As String = "Brook_S2_TC"
        Public Const MH_NOMOON As String = "MH_NOMOON"
        Public Const KsS1nomoon1 As String = "KsS1nomoon1"
        Public Const KsS1nomoon2 As String = "KsS1nomoon2"
        Public Const KsS1nomoon3 As String = "KsS1nomoon3"
        Public Const Brook_N_gden As String = "Brook_N_gden"
        Public Const Buildsite_MH1 As String = "Buildsite_MH1"
        Public Const MH3carpark As String = "MH3carpark"
        Public Const Bkn2_Nomoon1 As String = "bkn2_Nomoon1"
        Public Const Bkn2_Nomoon2 As String = "bkn2_Nomoon2"
        Public Const Bks3norain As String = "bks3norain"
        Public Const MHNoMoon As String = "MHNoMoon"
        Public Const Erosware As String = "erosware"
        Public Const QM_Nomoon As String = "QM_Nomoon"
        Public Const NJ2nomoon As String = "NJ2nomoon"
        Public Const Bxwnomoon As String = "bxwnomoon"
        Public Const Star_junc As String = "star_junc"
        Public Const Raytest As String = "raytest"
        Public Const Raytest2 As String = "raytest2"
        Public Const NJ02TUNNEL As String = "NJ02TUNNEL"
        Public Const Internaldim As String = "Internaldim"
        Public Const SoosTunnel As String = "SoosTunnel"
        Public Const Nikwarehouse As String = "Nikwarehouse"
        Public Const Clam As String = "clam"
        Public Const Vlads As String = "vlads"
        Public Const Generic As String = "generic"
        Public Const Jamcafe As String = "jamcafe"
        Public Const Ten_str As String = "ten_str"
        Public Const Playboyx As String = "playboyx"
        Public Const Browner As String = "browner"
        Public Const Limo As String = "limo"
        Public Const Stevetunnel As String = "STEVETUNNEL"
        Public Const Subway As String = "SUBWAY"
        Public Const Subway_Station As String = "SUBWAY_STATION"
        Public Const Carpark As String = "CARPARK"
        Public Const RomansFl As String = "RomansFl"
        Public Const Rscafe As String = "rscafe"
        Public Const Bernies As String = "bernies"
        Public Const Factorytest As String = "Factorytest"
        Public Const Factory As String = "Factory"
        Public Const Bens2 As String = "bens2"
        Public Const Hospital As String = "Hospital"
        Public Const Museum2 As String = "Museum2"
        Public Const Badamine As String = "Badamine"
        Public Const DrugDen As String = "DrugDen"
        Public Const Bank3 As String = "Bank3"
        Public Const Chase As String = "Chase"
        Public Const Sexshop As String = "sexshop"
        Public Const Diner As String = "Diner"
        Public Const Hospitallobby As String = "hospitallobby"
        Public Const Trespass As String = "Trespass"
        Public Const Ritz As String = "ritz"
        Public Const Ritzf3 As String = "ritzf3"
        Public Const Ritzpen As String = "ritzpen"
        Public Const Intcafe As String = "intcafe"
        Public Const Firedept As String = "firedept"
        Public Const Deal As String = "deal"
        Public Const Korrest As String = "korrest"
        Public Const Korbar As String = "korbar"
        Public Const Korkitch As String = "korkitch"
        Public Const Apart As String = "apart"
        Public Const Parktoilet As String = "parktoilet"
        Public Const HarlemProjects As String = "HarlemProjects"
        Public Const HarlemDrug As String = "HarlemDrug"
        Public Const JerSave As String = "JerSave"
        Public Const Burgershot As String = "burgershot"
        Public Const Burgershotold As String = "burgershotold"
        Public Const HarlemTopFloor As String = "HarlemTopFloor"
        Public Const Irishbar As String = "Irishbar"
        Public Const Boatcabs As String = "boatcabs"
        Public Const Corplobby As String = "corplobby"
        Public Const Binco As String = "binco"
        Public Const Gazwarehouse As String = "gazwarehouse"
        Public Const Bruciechopshop As String = "bruciechopshop"
        Public Const Playboyxlobby As String = "playboyxlobby"
        Public Const Statuestair As String = "statuestair"
        Public Const Waste As String = "waste"
        Public Const Bowl As String = "Bowl"
        Public Const GunShop As String = "GunShop"
        Public Const Chinagun As String = "chinagun"
        Public Const Harlem_ten As String = "harlem_ten"
        Public Const Sw_har_decor As String = "sw_har_decor"
        Public Const Sw_har_psh As String = "sw_har_psh"
        Public Const Ten_standard As String = "ten_standard"
        Public Const Ten_ornate As String = "ten_ornate"
        Public Const Ten_modern As String = "ten_modern"
        Public Const Cluckinbell As String = "cluckinbell"
        Public Const Casino As String = "Casino"
        Public Const Limooffice As String = "limooffice"
        Public Const DrugDenStair As String = "DrugDenStair"
        Public Const Project As String = "project"
        Public Const DwayneApart As String = "DwayneApart"
        Public Const Stair1 As String = "stair1"
        Public Const Stair2 As String = "stair2"
        Public Const MH8_carpark As String = "MH8_carpark"
        Public Const MH8_Savehouse As String = "MH8_Savehouse"
        Public Const MH8_Showroom As String = "MH8_Showroom"
        Public Const Studio_Apart01 As String = "STUDIO_APART01"
        Public Const ProjectStair As String = "projectStair"
        Public Const Lightning As String = "lightning"
        Public Const Playersettings As String = "playersettings"
        Public Const Playersettings2 As String = "playersettings2"
        Public Const Sniper As String = "sniper"
        Public Const Sniper_ini As String = "sniper_ini"
        Public Const Binocular As String = "binocular"
        Public Const Injured As String = "injured"
        Public Const Fast As String = "fast"
        Public Const Death As String = "death"
        Public Const Death2 As String = "death2"
        Public Const Death3 As String = "death3"
        Public Const Train_int As String = "train_int"
        Public Const Busted As String = "busted"
        Public Const Cabaret As String = "cabaret"
        Public Const Lobby2office As String = "lobby2office"
        Public Const Lobby2 As String = "lobby2"
        Public Const Police As String = "Police"
        Public Const SUBWAYSERV As String = "SUBWAYSERV"
        Public Const SUBWAYENT As String = "SUBWAYENT"
        Public Const SUBWAY_N As String = "SUBWAY_N"
        Public Const SUBWAY_E As String = "SUBWAY_E"
        Public Const SUBWAY_S As String = "SUBWAY_S"
        Public Const SUBWAY_W As String = "SUBWAY_W"
        Public Const NIGHTSHADE As String = "NIGHTSHADE"
        Public Const PIZZAREST As String = "PIZZAREST"
        Public Const PIZZAREST2 As String = "PIZZAREST2"
        Public Const BRUCIE_STUDIO As String = "BRUCIE_STUDIO"
        Public Const Church As String = "church"
        Public Const Faustins As String = "Faustins"
        Public Const Faustinsbase As String = "Faustinsbase"
        Public Const LittleJacobs As String = "LittleJacobs"
        Public Const Prison As String = "Prison"
        Public Const BernieCrane As String = "BernieCrane"
        Public Const McRearyHouse As String = "McRearyHouse"
        Public Const CopshopOffice As String = "CopshopOffice"
        Public Const Michelles As String = "Michelles"
        Public Const Sopranos As String = "sopranos"
        Public Const CIAoffice As String = "CIAoffice"
        Public Const Portacabin As String = "portacabin"
        Public Const Comclub As String = "comclub"
        Public Const Elizabetas As String = "elizabetas"
        Public Const Bada As String = "Bada"
        Public Const Fau3_a As String = "fau3_a"
        Public Const Imbhst As String = "imbhst"
        Public Const Em_4b As String = "em_4b"
        Public Const G_1 As String = "g_1"
        Public Const G_2 As String = "g_2"
        Public Const G_3 As String = "g_3"
        Public Const Em_4 As String = "em_4"
        Public Const Df_2 As String = "df_2"
        Public Const Df_3 As String = "df_3"
        Public Const Lilj1_a As String = "lilj1_a"
        Public Const Imfau6 As String = "imfau6"
        Public Const Imfau2 As String = "imfau2"
        Public Const Wedint As String = "wedint"
        Public Const Gm_2 As String = "gm_2"
        Public Const Br_1 As String = "br_1"
        Public Const Br_4 As String = "br_4"
        Public Const Px_2 As String = "px_2"
        Public Const Pxdf As String = "pxdf"
        Public Const Rb_4b As String = "rb_4b"
        Public Const Vla1_a As String = "vla1_a"
        Public Const Vla2_a As String = "vla2_a"
        Public Const Vla4_a As String = "vla4_a"
        Public Const Rom8_b As String = "rom8_b"
        Public Const Pm_3 As String = "pm_3"
        Public Const Em_1 As String = "em_1"
        Public Const Em_2 As String = "em_2"
        Public Const Em_3 As String = "em_3"
        Public Const Em_5 As String = "em_5"
        Public Const Em_7 As String = "em_7"
        Public Const Fau4_a As String = "fau4_a"
        Public Const Show_1 As String = "show_1"
        Public Const Show_2 As String = "show_2"
        Public Const Show_3 As String = "show_3"
        Public Const Show_4 As String = "show_4"
        Public Const Show_5 As String = "show_5"
        Public Const Show_6 As String = "show_6"
        Public Const Show_7 As String = "show_7"
        Public Const Show_8 As String = "show_8"
        Public Const Rb_4 As String = "rb_4"
        Public Const J_1 As String = "j_1"
        Public Const Rp_13 As String = "rp_13"
        Public Const Rom2_a As String = "rom2_a"
        Public Const Rom3_a As String = "rom3_a"
        Public Const Rom5_a As String = "rom5_a"
        Public Const Rom6_a As String = "rom6_a"
        Public Const Rom8_a As String = "rom8_a"
        Public Const R_9 As String = "r_9"
        Public Const Classic As String = "Classic"
        Public Const Tweaked As String = "Tweaked"
        Public Const Cinema As String = "Cinema"
        Public Const Verte As String = "Verte"
        Public Const Hot As String = "Hot"
        Public Const Steel As String = "Steel"
        Public Const Psyche As String = "Psyche"
        Public Const Romantic As String = "Romantic"
        Public Const Sepia As String = "Sepia"
        Public Const Muddy As String = "Muddy"
        Public Const Neon As String = "Neon"
        Public Const Rouge As String = "Rouge"
        Public Const Bronze As String = "Bronze"
        Public Const Ulraviolet As String = "Ulraviolet"
        Public Const Eclipse As String = "Eclipse"
        Public Const Noire As String = "Noire"
        Public Const Colors As String = "colors"
        Public Const Vintage As String = "Vintage"
        Public Const Fire As String = "Fire"
        Public Const Sketch As String = "Sketch"
    End Structure

    'REQUEST_INTERIOR_MODELS
    'CLEAR_PED_NON_REMOVAL_AREA();
    'SET_PED_NON_REMOVAL_AREA
    ''GET_CLOSEST_CAR_NODE
    ' FIND_PRIMARY_POPULATION_ZONE_GROUP( ref iVar2, ref uVar3 );
    'SWAP_NEAREST_BUILDING_MODEL( 619.01590000, 1478.10600000, 11.29510000, 50.00000000, 737085764, 630869279 )

    Public Class Interiors
        ''' <summary>
        ''' Untested
        ''' </summary>
        Public Shared Sub Activate(InteriorID As Integer, Optional Enable As Boolean = True)
            'REQUEST_INTERIOR_MODELS
            [Call]("ACTIVATE_INTERIOR", InteriorID, Enable)
        End Sub
        'Public Shared Function GetDoorState(Model As String, Position As Vector3) As (Boolean, Single)
        '    Dim point1 As Native.Pointer = New Native.Pointer(GetType(Boolean))
        '    Dim point2 As Native.Pointer = New Native.Pointer(GetType(Single))
        '    [Call]("GET_STATE_OF_CLOSEST_DOOR_OF_TYPE", NativeModels.getHashKey(Model), Position.X, Position.Y, Position.Z, point1, point2)
        '    Return (point1.Value, point2.Value)
        'End Function
        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="Model">String from "Doors" structure</param>
        '''' <param name="Position">Door's coordinates</param>
        '''' <param name="LockedInPosition">If false the door can't move, if true can move.</param>
        '''' <param name="DoorAngle">Value from -1.0 and 1.0 (open)</param>
        'Public Shared Sub SetDoorState(Model As String, Position As Vector3, LockedInPosition As Boolean, DoorAngle As Single)
        '    [Call]("SET_STATE_OF_CLOSEST_DOOR_OF_TYPE", NativeModels.getHashKey(Model), Position.X, Position.Y, Position.Z, LockedInPosition, DoorAngle)
        'End Sub
        'Public Shared Sub TerminateNativeDoorLockScripts(Optional JimsLocks As Boolean = True, Optional Gerry3DoorLock As Boolean = True)
        '    [Call]("TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME", "ambjimslocks")
        '    [Call]("TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME", "ambgerry3doorlock")
        'End Sub
        '''' <summary>
        '''' Contains all known door model names in GTA IV (not EFLC).
        '''' Each field is a ReadOnly string constant representing a door model name.
        '''' </summary>
        'Public Structure Doors
        '    ' Lawyer office
        '    Public Shared ReadOnly BM_lawyerDoor As String = "BM_lawyerDoor"
        '    Public Shared ReadOnly BM_lawyerDoor_2a As String = "BM_lawyerDoor_2a"
        '    Public Shared ReadOnly BM_lawyerDoor_2b As String = "BM_lawyerDoor_2b"

        '    ' Bank of Liberty
        '    Public Shared ReadOnly BankOfLibertyFrontdoorL As String = "cj_bank_door_L"
        '    Public Shared ReadOnly BankOfLibertyFrontdoorR As String = "cj_bank_door_R"

        '    ' Cargo Ship
        '    Public Shared ReadOnly CargoshipDoor As String = "cj_boat_door"

        '    ' Business Shop
        '    Public Shared ReadOnly BusinessShopDoorL As String = "cj_BS_door_L"
        '    Public Shared ReadOnly BusinessShopDoorR As String = "cj_BS_door_R"

        '    ' Misc Doors
        '    Public Shared ReadOnly cj_DB_MH3_door1 As String = "cj_DB_MH3_door1"
        '    Public Shared ReadOnly VladsBarDoor As String = "cj_int_door_6"
        '    Public Shared ReadOnly RomansTaxiOfficeDoor As String = "cj_int_door_7"
        '    Public Shared ReadOnly BlueDoor As String = "cj_int_door_10"
        '    Public Shared ReadOnly cj_int_door_12_H As String = "cj_int_door_12_H"
        '    Public Shared ReadOnly cj_int_door_22 As String = "cj_int_door_22"
        '    Public Shared ReadOnly cj_int_door_24 As String = "cj_int_door_24"
        '    Public Shared ReadOnly cj_int_door_27L As String = "cj_int_door_27L"
        '    Public Shared ReadOnly cj_int_door_27R As String = "cj_int_door_27R"
        '    Public Shared ReadOnly cj_int_door_29 As String = "cj_int_door_29"
        '    Public Shared ReadOnly cj_int_door_30 As String = "cj_int_door_30"
        '    Public Shared ReadOnly cj_int_door_3L As String = "cj_int_door_3L"

        '    ' Lawyer Front Door
        '    Public Shared ReadOnly LawyerFrontdoorL As String = "cj_law_frontdoor_L"
        '    Public Shared ReadOnly LawyerFrontdoorR As String = "cj_law_frontdoor_R"

        '    ' Misc Interiors
        '    Public Shared ReadOnly cj_MC_door_1 As String = "cj_MC_door_1"
        '    Public Shared ReadOnly DwainesAptDoor As String = "cj_mision_door_1"
        '    Public Shared ReadOnly BohanFactoryCageDoor As String = "cj_MP_fact_door_2"

        '    ' China Shop
        '    Public Shared ReadOnly ChinaShopFrontdoorL As String = "cj_new_china_door_L"
        '    Public Shared ReadOnly ChinaShopFrontdoorR As String = "cj_new_china_door_R"

        '    ' Perseus Clothes Shop
        '    Public Shared ReadOnly PerseusFrontdoorL As String = "cj_PER_door_L"
        '    Public Shared ReadOnly PerseusFrontdoorR As String = "cj_PER_door_R"

        '    ' Russian Clothes Shop
        '    Public Shared ReadOnly RussianShopFrontdoorL As String = "cj_RUS_door_1"
        '    Public Shared ReadOnly RussianShopFrontdoorR As String = "cj_RUS_door_2"

        '    ' Bowling Alley
        '    Public Shared ReadOnly BowlingAlleyDoorL As String = "ec_ML_door_L"
        '    Public Shared ReadOnly BowlingAlleyDoorR As String = "ec_ML_door_R"

        '    Public Shared ReadOnly FaustinsFrontdoor As String = "faustinsfrontdoor"

        '    ' Central Park Bathroom
        '    Public Shared ReadOnly ToiletDoor As String = "cj_shoot_t_door"
        '    Public Shared ReadOnly ToiletDoorBRK As String = "cj_t_door_BRK"
        '    Public Shared ReadOnly ToiletDoorENG As String = "cj_t_door_ENG"
        '    Public Shared ReadOnly ToiletDoorVAC As String = "cj_t_door_VAC"

        '    ' Church
        '    Public Shared ReadOnly ChurchDoorL As String = "cj_church_door_L"
        '    Public Shared ReadOnly ChurchDoorR As String = "cj_church_door_R"

        '    ' Gunshop
        '    Public Shared ReadOnly GunshopFrontdoor As String = "cj_ext_door_11"

        '    ' Triangle Club
        '    Public Shared ReadOnly TriangleClubFrontdoor As String = "cj_ext_door_CM"

        '    ' Gracie Kidnapping Safehouse
        '    Public Shared ReadOnly GracieSafehouseDoor As String = "cj_ext_door_1"

        '    ' General Exteriors
        '    Public Shared ReadOnly cj_ext_door_10 As String = "cj_ext_door_10"
        '    Public Shared ReadOnly cj_ext_door_15b As String = "cj_ext_door_15b"
        '    Public Shared ReadOnly cj_ext_door_16 As String = "cj_ext_door_16"
        '    Public Shared ReadOnly cj_ext_door_17 As String = "cj_ext_door_17"
        '    Public Shared ReadOnly cj_ext_door_18 As String = "cj_ext_door_18"
        '    Public Shared ReadOnly cj_ext_door_19_L As String = "cj_ext_door_19_L"
        '    Public Shared ReadOnly cj_ext_door_19_R As String = "cj_ext_door_19_R"
        '    Public Shared ReadOnly cj_ext_door_22 As String = "cj_ext_door_22"
        '    Public Shared ReadOnly cj_ext_door_6 As String = "cj_ext_door_6"
        '    Public Shared ReadOnly cj_ext_door_9 As String = "cj_ext_door_9"

        '    ' Clubs
        '    Public Shared ReadOnly CabaretDoorL As String = "cabaret_door_L"
        '    Public Shared ReadOnly CabaretDoorR As String = "cabaret_door_R"

        '    ' Vault
        '    Public Shared ReadOnly VaultDoor As String = "CJ_VAULT_DOOR"
        '    Public Shared ReadOnly VaultDoorDam As String = "CJ_VAULT_DOOR_DAM"
        '    Public Shared ReadOnly VaultGate As String = "CJ_VAULT_GATE"
        '    Public Shared ReadOnly SafeDoor As String = "GB_safe02_door"

        '    ' Cluck’n Bell
        '    Public Shared ReadOnly CluckBellDoor As String = "KM_CBDoor"
        '    Public Shared ReadOnly CluckBellMensroom As String = "KM_CBDoorTM"
        '    Public Shared ReadOnly CluckBellWomensroom As String = "KM_CBDoorTW"
        '    Public Shared ReadOnly CluckBellFrontdoorL As String = "KM_CBFrontDrL"
        '    Public Shared ReadOnly CluckBellFrontdoorR As String = "KM_CBFrontDrR"

        '    ' Majestic Hotel
        '    Public Shared ReadOnly RitzIntDoorA As String = "ab_RitzIntDoorA"
        '    Public Shared ReadOnly RitzIntDoorB As String = "ab_RitzIntDoorB"
        '    Public Shared ReadOnly RitzMainDoor As String = "ab_ritzMaindoor"
        '    Public Shared ReadOnly RitzAptDoor As String = "ab_ritz_aptdoor"
        '    Public Shared ReadOnly RitzGlassDoor As String = "ab_ritzglassdoor"

        '    ' Miscellaneous
        '    Public Shared ReadOnly PortacabinDoor As String = "KM_PortacabinDoor"
        '    Public Shared ReadOnly PrisonSecurityDoor As String = "KM_PrisSecDoor"
        '    Public Shared ReadOnly PrisonVisitorDoor As String = "KM_PrisVisDoor"
        '    Public Shared ReadOnly NorthwoodProjectsDoor As String = "proj_doorRM1"
        '    Public Shared ReadOnly NorthwoodAptDoor As String = "proj_doorRM1d"
        '    Public Shared ReadOnly proj_doorRM2 As String = "proj_doorRM2"
        '    Public Shared ReadOnly proj_doorRM4 As String = "proj_doorRM4"
        '    Public Shared ReadOnly JamFrontdoor As String = "jamfrontdoor"
        '    Public Shared ReadOnly HospitalMainDoors As String = "nhospmaindoors"
        '    Public Shared ReadOnly HospitalMainDoors01 As String = "nhospmaindoors01"

        '    ' Diner
        '    Public Shared ReadOnly DinerDoor1 As String = "ndinerdoor1"
        '    Public Shared ReadOnly DinerDoor2 As String = "ndinerdoor2"

        '    ' Crackhouse
        '    Public Shared ReadOnly CrackhouseDoor As String = "XJ_TenDoor"

        '    ' Search and Delete
        '    Public Shared ReadOnly SearchAndDeleteDoorA As String = "Deal_doorA"
        '    Public Shared ReadOnly SearchAndDeleteDoorB As String = "Deal_doorB"

        '    ' Chinese Restaurant
        '    Public Shared ReadOnly ChineseFrontdoor As String = "KM_KorBlacDoor"
        '    Public Shared ReadOnly ChineseInteriorDoor As String = "KM_KorTDoor"

        '    ' Playboy
        '    Public Shared ReadOnly PlayboyFrontdoor As String = "playboyfrontdoor"

        '    ' CIA Lobby
        '    Public Shared ReadOnly CIALobbyDoor As String = "Blk3_LobbyDoor"
        '    Public Shared ReadOnly CIALobbyOfficeDoor As String = "corpdoor"

        '    ' Turismo Showroom
        '    Public Shared ReadOnly TurismoShowroomL As String = "LD_show_door_L"
        '    Public Shared ReadOnly TurismoShowroomR As String = "LD_show_door_R"

        '    ' Hospital Corridors
        '    Public Shared ReadOnly HospCorridorDoor1 As String = "hospdoorcorri1"
        '    Public Shared ReadOnly HospCorridorDoor3 As String = "hospdoorcorri3"
        '    Public Shared ReadOnly HospCorridorDoor4 As String = "hospdoorcorri4"
        '    Public Shared ReadOnly HospCorridorDoor5 As String = "hospdoorcorri5"

        '    ' Projects
        '    Public Shared ReadOnly ProjectsBottomDoor As String = "ab_projDoor2"

        '    ' Museum Garage Door
        '    Public Shared ReadOnly MuseumGarageDoor As String = "bs3missiongardoor"
        '    Public Shared ReadOnly ResprayDoor As String = "bs3respraydoor"

        '    ' Elevator Doors
        '    Public Shared ReadOnly LiftLeftDoor As String = "CJ_LIFT_L_DOOR"
        '    Public Shared ReadOnly LiftLeftDoor2 As String = "CJ_LIFT_L_DOOR_2"
        '    Public Shared ReadOnly LiftLeftDoorOut As String = "CJ_LIFT_L_DOOR_OUT"
        '    Public Shared ReadOnly LiftLeftDoorOut2 As String = "CJ_LIFT_L_DOOR_OUT_2"
        '    Public Shared ReadOnly LiftRightDoor As String = "CJ_LIFT_R_DOOR"
        '    Public Shared ReadOnly LiftRightDoor2 As String = "CJ_LIFT_R_DOOR_2"
        '    Public Shared ReadOnly LiftRightDoorOut As String = "CJ_LIFT_R_DOOR_OUT"
        '    Public Shared ReadOnly LiftRightDoorOut2 As String = "CJ_LIFT_R_DOOR_OUT_2"
        'End Structure
    End Class
End Class


