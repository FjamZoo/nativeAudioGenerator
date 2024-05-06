using System.Reflection.Metadata;
using System.Windows;
using System.Xml;
namespace NativeAudioGen.Types
{

    /*
    public static string[] datCategories = {
            "ambience",
            "ambience_birds",
            "ambience_city_noise",
            "ambience_collectables",
            "ambience_general",
            "ambience_general_traffic",
            "ambience_industrial",
            "ambience_insects",
            "ambience_insects_flies",
            "ambience_music",
            "ambience_oneshot_vehicles",
            "ambience_speech",
            "ambience_weather",
            "animals_footsteps",
            "animals_vocals",
            "base",
            "collisions",
            "collisions_cloth",
            "collisions_dynamic_upped",
            "collisions_glass",
            "collisions_louder",
            "collisions_scripted",
            "collisions_vehicles_glass",
            "cutscenes",
            "cutscenes_reverb",
            "doors",
            "doors_loud",
            "fire",
            "fire_louder",
            "frontend_game",
            "frontend_game_loud",
            "frontend_game_nofade",
            "frontend_game_player_switch",
            "frontend_game_police_scanner",
            "frontend_game_slow_mo",
            "frontend_menu",
            "frontend_menu_loud",
            "frontend_menu_pauseable",
            "frontend_radio",
            "game_world",
            "hash_05403EE1",
            "hash_0607FDB8",
            "hash_0D3C9D38",
            "hash_0E4CF672",
            "hash_25E385A7",
            "hash_266E012E",
            "hash_291E7FD7",
            "hash_2F34D6FC",
            "hash_3874EB6C",
            "hash_3A52AFA3",
            "hash_3C496EED",
            "hash_3F6594E4",
            "hash_414231B5",
            "hash_4FFD9CA5",
            "hash_639A44A4",
            "hash_6805AAC2",
            "hash_80722AAA",
            "hash_85B8BFD4",
            "hash_85DBC375",
            "hash_8A91FE75",
            "hash_94821026",
            "hash_9748F077",
            "hash_9C6A4771",
            "hash_A6A84701",
            "hash_A6DA13DC",
            "hash_B2681B31",
            "hash_B4C14B9C",
            "hash_B9CB44C7",
            "hash_BC72B5EB",
            "hash_BCE6F3E0",
            "hash_BF162C33",
            "hash_C7D71D61",
            "hash_CB2382B4",
            "hash_CD3365DE",
            "hash_CFF0C1C2",
            "hash_DA38F55D",
            "hash_E3FAF7D3",
            "hash_EB0865AB",
            "hash_F0E66096",
            "hash_F3196F77",
            "hash_F4FABC2A",
            "hash_F841C9F9",
            "hash_F846B110",
            "hud",
            "interactive_music",
            "interactive_music_1",
            "interactive_music_2",
            "interactive_music_3",
            "interactive_music_4",
            "interactive_music_5",
            "interactive_music_6",
            "interactive_music_7",
            "interactive_music_8",
            "melee",
            "music",
            "music_loading",
            "music_oneshot",
            "music_slider",
            "ped_vehicle_convertible_roof",
            "ped_vehicle_offsets",
            "ped_vehicles_loud",
            "ped_vehicles_normal",
            "ped_vehicles_quiet",
            "ped_vehicles_very_loud",
            "ped_vehicles_very_quiet",
            "peds",
            "peds_clothing",
            "peds_collisions",
            "peds_collisions_loud",
            "peds_collisions_loud_dynamic",
            "peds_footsteps",
            "peds_footsteps_boostable",
            "peds_footsteps_leather",
            "peds_footsteps_rubber_hard",
            "peds_footsteps_rubber_soft",
            "peds_wind",
            "player_vehicle_convertible_roof",
            "player_vehicle_offsets",
            "player_vehicles_loud",
            "player_vehicles_normal",
            "player_vehicles_quiet",
            "player_vehicles_very_loud",
            "player_vehicles_very_quiet",
            "positioned_radio",
            "radio",
            "radio_dj_front_end",
            "radio_dj_positioned",
            "radio_front_end",
            "radio_positioned",
            "score",
            "scripted",
            "scripted_alarms",
            "scripted_louder",
            "scripted_mini_games",
            "scripted_override_fade",
            "scripted_override_pitch",
            "scripted_tv",
            "sfx_slider",
            "speech",
            "speech_ambient",
            "speech_breathing",
            "speech_pain",
            "speech_pain_damage",
            "speech_scripted",
            "underwater",
            "underwater_loud",
            "underwater_muted",
            "underwater_swimming",
            "vehicles",
            "vehicles_bicycles",
            "vehicles_bicycles_mechanical",
            "vehicles_boats",
            "vehicles_boats_engines",
            "vehicles_boats_water",
            "vehicles_bodies",
            "vehicles_brakes",
            "vehicles_car_by",
            "vehicles_chassis_rattle",
            "vehicles_doors",
            "vehicles_engines",
            "vehicles_engines_cooling",
            "vehicles_engines_damage",
            "vehicles_engines_ignition",
            "vehicles_engines_intake",
            "vehicles_engines_loud",
            "vehicles_engines_pops",
            "vehicles_engines_reflections",
            "vehicles_engines_reflections_loud",
            "vehicles_engines_startup",
            "vehicles_engines_transmission",
            "vehicles_extras_loud",
            "vehicles_helis",
            "vehicles_helis_distant",
            "vehicles_helis_rotor",
            "vehicles_horns",
            "vehicles_horns_loud",
            "vehicles_offsets",
            "vehicles_planes",
            "vehicles_planes_close",
            "vehicles_planes_distant",
            "vehicles_planes_extras",
            "vehicles_planes_extras_loud",
            "vehicles_planes_jet",
            "vehicles_planes_prop",
            "vehicles_sirens",
            "vehicles_suspension",
            "vehicles_train",
            "vehicles_train_brakes",
            "vehicles_train_carriage",
            "vehicles_train_clack",
            "vehicles_train_rumble_approach",
            "vehicles_wheels",
            "vehicles_wheels_blow_out",
            "vehicles_wheels_loud",
            "vehicles_wheels_road_noise",
            "vehicles_wheels_skids",
            "video_editor",
            "video_editor_collisions_louder",
            "video_editor_explosions",
            "video_editor_gunfire",
            "video_editor_gunfire_action",
            "video_editor_gunfire_npc",
            "video_editor_gunfire_npc_action",
            "video_editor_gunfire_npc_lazer",
            "video_editor_melee",
            "water",
            "water_loud",
            "water_ocean",
            "water_river",
            "water_swimming",
            "weapons",
            "weapons_explosions",
            "weapons_explosions_distant",
            "weapons_explosions_loud",
            "weapons_explosions_tails",
            "weapons_guns",
            "weapons_guns_bullet_bys",
            "weapons_guns_bullet_impacts",
            "weapons_guns_bullet_impacts_body",
            "weapons_guns_bullet_impacts_ricco",
            "weather",
            "weather_rain",
            "weather_rain_heavy",
            "weather_rain_props",
            "weather_thunder",
            "weather_wind",
            "weather_wind_foliage"
    };*/

    enum DatFlags : uint
    {
        Flags2 = 0x00000001,
        Unk01 = 0x00000002,
        Volume = 0x00000004,
        VolumeVariance = 0x00000008,
        Pitch = 0x00000010,
        PitchVariance = 0x00000020,
        Pan = 0x00000040,
        PanVariance = 0x00000080,
        PreDelay = 0x00000100,
        PreDelayVariance = 0x00000200,
        StartOffset = 0x00000400,
        StartOffsetVariance = 0x00000800,
        AttackTime = 0x00001000,
        ReleaseTime = 0x00002000,
        DopplerFactor = 0x00004000,
        Category = 0x00008000,
        LPFCutOff = 0x00010000,
        LPFCutOffVariance = 0X00020000,
        HPFCutOff = 0x00040000,
        HPFCutOffVariance = 0X00080000,
        UnkHash3 = 0x00100000,
        DistanceAttentuation = 0x00200000,
        Unk19 = 0x00400000,
        Unk20 = 0x00800000,
        Unk21 = 0x01000000,
        UnkHash4 = 0x02000000,
        UnkHash5 = 0x04000000,
        Unk22 = 0x08000000,
        Unk23 = 0x10000000,
        Unk24 = 0x20000000,
        Unk25 = 0x40000000,
        Unk26 = 0x80000000,
    }
    public struct DatHeader {
        public int Flags;
        public int? Flags2;
        public ushort Unk01;
        public short? Volume;
        public ushort? VolumeVariance;
        public short? Pitch;
        public short? PitchVariance;
        public ushort? PreDelay;
        public ushort? PreDelayVariance;
        public int? StartOffset;
        public int? StartOffsetVariance;
        public ushort? AttackTime;
        public ushort? ReleaseTime;
        public ushort? DopplerFactor;
        //TODO: Enum?
        public string Category;
        public ushort? LPFCutOff;
        public ushort? LPFCutOffVariance;
        public ushort? HPFCutOff;
        public ushort? HPFCutOffVariance;
        //TODO: Findout all options
        public string? VolumeCurve;
        public ushort? VolumeCurveScale;
        public byte? VolumeCurvePlateau;
        //Virtualise As group - Stereo panning L-R
        public byte? Unk20;
        public byte? Unk21;
        //TODO: Find all options;
        public string? PreDelayParameter;
        public string? StartOffsetParameter;
        public ushort? Unk22;
        public ushort? Unk23;
        public ushort? Unk24;
    }

    public abstract class SoundEntry
    {
        public string Name;
        public DatHeader Header;

        public abstract XmlNode GenerateXML();
    }

    public class LoopingSound : SoundEntry
    {
        public ushort LoopCount;
        public ushort LoopCountVariance;
        public ushort LoopPoint;
        public string ChildSound;
        public string LoopCountVariable;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }


    public class SimpleSound : SoundEntry
    {
        public string Name;
        public DatHeader Header;
        public string ContainerName;
        public string FileName;
        public byte WaveSlotNum = 0;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }
    public class EnvelopeSound : SoundEntry
    {
        public short Attack = 0;
        public short AttackVariance = 0;
        public short Decay = 0;
        public short DecayVariance = 0;
        public byte Sustain = 100;
        public byte SustainceVariance = 0;
        public int Hold = 0;
        public short HoldVariance = 0;
        public int Release = 0;
        public int ReleaseVariance = 0;
        public string AttackCurve = "linear_rise";
        public string DecayCurve = "linear_fall";
        public string ReleaseCurve = "default_release_curve";
        public string? AttackVariable;
        public string? DecayVariable;
        public string? SustainVariable;
        public string? HoldVariable;
        public string? ReleaseVariable;
        public string? ChildSound;
        
        //0 - Volume
        //1 - Pitch
        //2 - Pan
        //3 - LPF
        //4 - HPF
        //5 - Variable (uses OutputVariable Field)
        //6 - Logarithmic LPF
        //7 - Logarithmic HPF
        public int Mode = 0;
        public string OutputVariable;
        public float OuputRangeMin = -100;
        public float OuputRangeMax = 0;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class TwinLoopSound : SoundEntry
    {
        public ushort MinSwapTime;
        public ushort MaxSwapTime;
        public ushort MinCrossfadeTime;
        public ushort MaxCrossfadeTime;
        public string CrossfadeCurve;
        public string? MinSwapTimeVariable;
        public string? MaxSwapTimeVariable;
        public string? MinCrossfadeTimeVariable;
        public string? MaxCrossfadeTimeVariable;
        public string[] ChildSounds;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SpeechSound : SoundEntry
    {
        public int LastVariation;
        public int SpeechUnkInt1;
        public string VoiceName;
        public string ContextName;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }


    public class StopSound : SoundEntry
    {
        public string ChildName;
        public string StopName;
        public string? FinishedSound;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class WrapperSound : SoundEntry
    {
        public string ChildSound;
        public int LastPlayTime;
        public string FallBackSound;
        public string MinRepeatTime; //Apprently FrameTimeInterval
        //public string? Variables; - Unknown, represented as <Variables />
        protected string[] ChildSounds; //Should Always contain ChildSound and FallBackSound

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SequentialSound : SoundEntry
    {
        public string[] ChildSounds;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class StreamingSound : SoundEntry
    {
        public string[] ChildSounds;
        public int Duration;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class RetriggeredOverlappedSound : SoundEntry
    {
        public ushort LoopCount;
        public ushort LoopCountVariance;
        public ushort DelayTime;
        public ushort DelayTimeVariance;
        public string? LoopCountVariable;
        public string? DelayTimeVariable;
        public string? StartSound;
        public string RetriggerSound;
        public string? StopSound;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }


    public class CrossfadeSound : SoundEntry
    {
        public string NearSound; //FadeOutSound
        public string FarSound; //FadeInSound

        //0 = Mode
        //1 = Linear
        //2 = Equal Power
        public byte Mode = 0; //UnkByte

        public float MinDistance = 20; // UnkFloat0
        public float MaxDistance = 100; // UnkFloat 1

        public int Hysterersis = 0; //UnkInt
        public string CrossfadeCurve = "crossfade_curve"; //UnkCurveHash
        public string DistanceVariable; //ParameterHash0
        public string MinDistanceVariable; //ParameterHash1
        public string MaxDistanceVariable; //ParameterHash2
        public string HysteresisVariable; //ParameterHash3
        public string CrossfadeVariable; //ParameterHash4

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }


    public class MultitrackSound : SoundEntry
    {
        public string[] ChildSounds;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public struct RandomizedVariations
    {
        public string key;
        public float value;
    }

    public class RandomizedSound : SoundEntry
    {
        public int HistoryIndex = 0; // 0 - no limit, 2 - First two variations only, 3 - Firstthree variations only etc
        public RandomizedVariations[] Variations;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class EnvironmentSound : SoundEntry
    {
        /// <summary>
        /// Defines left or right channel output:
        /// 0 - Left Channel,
        /// 1 - Right Channel,
        /// 2 - 2nd side left channel,
        /// 3 - 2nd side right channel,
        /// </summary>
        public byte ChannelID;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SequentialOverlapSound : SoundEntry
    {

        /// <summary>
        /// The delay of each sound in the sequence.  If this value is set to 0, all sounds will be played on top of each other, creating amplitude. This will affect all sounds in the sequence.
        /// </summary>
        public ushort DelayTime;

        public string? DelayTimeVariable;

        public string? SequenceDierection;

        public string[] ChildSounds;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DirectionalSound : SoundEntry
    {

        public string ChildSound;
        public float InnerAngle = 30;
        public float OuterAngle = 90;
        public float RearAttenuation = 12; 
        public float YawAngle = 90;
        public float PitchAngle = 0;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }


    //Monky's audio research contains nothing about this aside from referencing it as an type in 'Dat54Sound types'
    public class SwitchSound : SoundEntry
    {
        public string Variable = "variation";
        public string[] ChildSounds;

        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }
    public struct SoundSetEntry
    {
        public string Name;
        public string Child;
    }

    public class SoundSet : SoundEntry
    {
        public SoundSetEntry[] Items;
        public override XmlNode GenerateXML()
        {
            throw new System.NotImplementedException();
        }
    }


    public class Dat54
    {

    }
}