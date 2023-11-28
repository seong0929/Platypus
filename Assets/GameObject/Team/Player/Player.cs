using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Player //: MonoBehaviour
{
    
    public string Name { get; set; } // The name of the Player
    public int Level { get; set; }

    public Team Team { get; set; }

    public List<Enums.EStrategy> Strategies { get; set; }
    public List<Enums.ETrait> Traits;
    public int Attack;
    public int Defense;
    public Dictionary<Enums.ESummon, int> Proficiency { get; set; } // The player's proficiency for each summon

    public CharacterAppearance Appearance;

    public Player(string name = "Unknown", int level = 1, Team team = null)
    {
        Name = name;
        Level = level;
        Team = team;

        SetSkillByLevel(level);

        Appearance = new CharacterAppearance();
        Appearance.SetRandomAppearance();
    }

    private void SetSkillByLevel(int level)
    {
        Attack = level;
        Defense = level;

        Proficiency = new Dictionary<Enums.ESummon, int>();
        Proficiency.Add(Enums.ESummon.SenorZorro, level);
        /*
         * Strategies
         * Traits
         */
    }

}
public class CharacterAppearance
{
    public string Head = "Human";
    public string Ears = "Human";
    public string Eyes = "Human";
    public string Body = "Human";
    public string Hair;
    public string Armor;
    public string Helmet;
    public string Weapon;
    public string Shield;
    public string Cape;
    public string Back;
    public string Mask;
    public string Horns;

    #region Options
    private static List<string> _headOptions = new List<string>
    {
            "DarkElf",
            "Demigod",
            "Demon",
            "Elf",
            "FireLizard",
            "Furry",
            "Goblin",
            "Human",
            "HumanShadow",
            "Lizard",
            "Merman",
            "Orc",
            "Skeleton",
            "Vampire",
            "Werewolf",
            "ZombieA",
            "ZombieB"
    };
    private static List<string> _ArmorOptions = new List<string>
    {
            "ArcherTunic",
            "BanditTunic",
            "BlueKnight",
            "BlueWizardTunic",
            "CaptainArmor",
            "Chief",
            "ChumDoctorTunic",
            "ClericRobe",
            "DeathRobe",
            "DeerCostume",
            "DemigodArmour",
            "DruidRobe",
            "ElfTunic",
            "Executioner",
            "FarmerClothes",
            "FemaleOutfit",
            "FemaleSwimmingSuit",
            "FireWizardRobe",
            "GuardianTunic",
            "HeavyKnightArmor",
            "HolowKnight",
            "HornsKnight",
            "IronKnight",
            "LegionaryArmor",
            "MilitiamanArmor",
            "MinerArmour",
            "MonkRobe",
            "MusketeerTunic",
            "NecromancerRobe",
            "NinjaTunic",
            "Overalls",
            "PilotArmor",
            "PirateCostume",
            "PirateRobe",
            "PumpkinOveralls",
            "Samurai",
            "SantaHelperTunic",
            "SantaTunic",
            "ThiefTunic",
            "TravelerTunic"
    };
    private static List<string> _BackOptions = new List<string>
    {
            "BackSword",
            "LargeBackpack",
            "LeatherQuiver",
            "SmallBackpack"
    };
    private static List<string> _EarsOptions = new List<string>
    {
        "DarkElf",
        "Demigod",
        "Demon",
        "Elf",
        "FireLizard",
        "Furry",
        "Goblin",
        "Human",
        "Lizard",
        "Merman",
        "Orc",
        "Skeleton",
        "Vampire",
        "Werewolf",
        "ZombieA",
        "ZombieB"
    };
    private static List<string> _EyesOptions = new List<string>
    {
            "DarkElf",
            "Demigod",
            "Demon",
            "Elf",
            "FireLizard",
            "Furry",
            "Goblin",
            "Human",
            "Lizard",
            "Merman",
            "Orc",
            "Skeleton",
            "Vampire",
            "Werewolf",
            "ZombieA",
            "ZombieB"
    };
    private static List<string> _BodyOptions = new List<string>
    {
            "DarkElf",
            "Demigod",
            "Demon",
            "Elf",
            "FireLizard",
            "Furry",
            "Goblin",
            "Human",
            "Lizard",
            "Merman",
            "Orc",
            "Skeleton",
            "Vampire",
            "Werewolf",
            "ZombieA",
            "ZombieB"
    };
    private static List<string> _HairOptions = new List<string>
    {
            "",
            "Hair1",
            "Hair10",
            "Hair11",
            "Hair12",
            "Hair13",
            "Hair14",
            "Hair15",
            "Hair2",
            "Hair3",
            "Hair4",
            "Hair5",
            "Hair6",
            "Hair7",
            "Hair8",
            "Hair9"
    };
    private static List<string> _HelmetOptions = new List<string>
    {
            "",
            "ArcherHood",
            "BanditBandana",
            "BanditPatch [ShowEars]",
            "BlueKnightHelmet",
            "BlueWizzardHat",
            "CaptainHelmet",
            "ChiefHat [ShowEars]",
            "ChumDoctorHelmet",
            "ClericHood",
            "DeathHood",
            "DeerHorns [ShowEars]",
            "DruidHelmet",
            "ExecutionerHood",
            "FireWizardHood",
            "GuardianHelmet",
            "HeavyKnightHelmet",
            "HolowKnightHelmet",
            "HornsHelmet",
            "HornsKnightHelmet",
            "IronKnightHelmet",
            "LegionaryHelmet",
            "MilitiamanHelmet",
            "MinerHelment",
            "MusketeerHat [ShowEars]",
            "NecromancerHood",
            "NinjaMask",
            "PilotHelment",
            "PirateBandana [ShowEars]",
            "PirateHelmet [ShowEars]",
            "PumpkinHead",
            "SamuraiHelmet",
            "SantaHelperCap [ShowEars]",
            "SantaHood",
            "SantaHoodBeard",
            "ThiefHood",
            "VikingHelmet"
    };
    private static List<string> _WeaponOptions = new List<string>
    {
                    "",
            "AmurWand",
            "ArchStaff",
            "AssaultSword",
            "Axe",
            "BastardSword",
            "BattleAxe",
            "BattleBow",
            "BattleHammer",
            "Bident",
            "BishopStaff",
            "BlacksmithHammer",
            "Blade",
            "BlueStick",
            "BlueWand",
            "Bow",
            "Branch",
            "Broadsword",
            "Butcher",
            "Crusher",
            "CrystalWand",
            "CurveBranch",
            "CurvedBow",
            "Cutlass",
            "Dagger",
            "DeathScythe",
            "ElderStaff",
            "Epee",
            "Executioner",
            "FireWand",
            "FlameStaff",
            "Fork",
            "GiantBlade",
            "GiantSword",
            "GoldenSkepter",
            "GoldenSkullWand",
            "Greataxe",
            "GreatHammer",
            "Greatsword",
            "GreenWand",
            "GuardianHalberd",
            "Halberd",
            "Hammer",
            "HermitStaff",
            "HunterKnife",
            "IronSword",
            "Katana",
            "KitchenAxe",
            "Knife",
            "Lance",
            "LargePickaxe",
            "LargeScythe",
            "LongBow",
            "LongKatana",
            "Longsword",
            "Mace",
            "MagicWand",
            "MarderDagger",
            "MasterGreataxe",
            "MasterWand",
            "Morgenstern",
            "NatureWand",
            "NecromancerStaff",
            "Pan",
            "Pickaxe",
            "Pitchfork",
            "PriestWand",
            "Rake",
            "RedKatana",
            "RedStick",
            "RedWand",
            "RoundMace",
            "RoyalLongsword",
            "RustedHammer",
            "RustedPickaxe",
            "RustedShortSword",
            "RustedShovel",
            "Saber",
            "Scythe",
            "Shestoper",
            "ShitOnStick",
            "ShortBow",
            "ShortDagger",
            "Sickle",
            "SkullWand",
            "SmallAxe",
            "SmallPitchfork",
            "SpikedClub",
            "Stick",
            "StormStaff",
            "Sword",
            "Tanto",
            "WaterWand",
            "WideDagger",
            "WoodcutterAxe"
    };
    private static List<string> _ShieldOptions = new List<string>
    {
            "",
            "AncientGreatShield",
            "BlueShield",
            "BrassRoundShield",
            "CrusaderShield",
            "Dreadnought",
            "GoldenEagle",
            "GuardianShield",
            "IronBuckler",
            "KnightShield",
            "LegionShield",
            "RoyalGreatShield",
            "SteelShield",
            "TowerShield",
            "WoodenBuckler"
    };
    private static List<string> _CapeOptions = new List<string>
    { "","Cape"};
    private static List<string> _HornOptions = new List<string>
    { "","Horns"};
    private static List<string> _maskOptions = new List<string>
    {"",
            "AnimeMask",
            "BanditMask",
            "BlackMask",
            "ChumMask",
            "DarkMask",
            "IronMask",
            "Shadow",
            "SuperMask"
    };
    #endregion

    public void SetRandomAppearance()
    {
        Head = _headOptions[Random.Range(0, _headOptions.Count)];
        Ears = Head;
        Eyes = Head;
        Body = Head;
        Hair = _HairOptions[Random.Range(0, _HairOptions.Count)];
        Armor = _ArmorOptions[Random.Range(0, _ArmorOptions.Count)];
        Helmet = _HelmetOptions[Random.Range(0, _HelmetOptions.Count)];
        Weapon = _WeaponOptions[Random.Range(0, _WeaponOptions.Count)];
        Shield = _ShieldOptions[Random.Range(0, _ShieldOptions.Count)];
        Cape = _CapeOptions[Random.Range(0, _CapeOptions.Count)];
        Back = _BackOptions[Random.Range(0, _BackOptions.Count)];
        Mask = _maskOptions[Random.Range(0, _maskOptions.Count)];
    }
}