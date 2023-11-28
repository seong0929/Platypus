using System.Collections.Generic;
using System.Linq;
using Assets.PixelHeroes.Scripts.CollectionScripts;
using Assets.PixelHeroes.Scripts.Utils;
using UnityEngine;


namespace Assets.PixelHeroes.Scripts.CharacterScripts
{
    public class CharacterBuilder : MonoBehaviour
    {
        public SpriteCollection SpriteCollection;
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
        public UnityEngine.U2D.Animation.SpriteLibrary SpriteLibrary;

        public CharacterAppearance CharacterAppearance;

        public Texture2D Texture { get; private set; }
        private Dictionary<string, Sprite> _sprites;
        #region Options
//        private List<string> _headOptions = new List<string>
//        {
//            "DarkElf",
//            "Demigod",
//            "Demon",
//            "Elf",
//            "FireLizard",
//            "Furry",
//            "Goblin",
//            "Human",
//            "HumanShadow",
//            "Lizard",
//            "Merman",
//            "Orc",
//            "Skeleton",
//            "Vampire",
//            "Werewolf",
//            "ZombieA",
//            "ZombieB"
//        };
//        private List<string> _ArmorOptions = new List<string>
//        {
//            "ArcherTunic",
//            "BanditTunic",
//            "BlueKnight",
//            "BlueWizardTunic",
//            "CaptainArmor",
//            "Chief",
//            "ChumDoctorTunic",
//            "ClericRobe",
//            "DeathRobe",
//            "DeerCostume",
//            "DemigodArmour",
//            "DruidRobe",
//            "ElfTunic",
//            "Executioner",
//            "FarmerClothes",
//            "FemaleOutfit",
//            "FemaleSwimmingSuit",
//            "FireWizardRobe",
//            "GuardianTunic",
//            "HeavyKnightArmor",
//            "HolowKnight",
//            "HornsKnight",
//            "IronKnight",
//            "LegionaryArmor",
//            "MilitiamanArmor",
//            "MinerArmour",
//            "MonkRobe",
//            "MusketeerTunic",
//            "NecromancerRobe",
//            "NinjaTunic",
//            "Overalls",
//            "PilotArmor",
//            "PirateCostume",
//            "PirateRobe",
//            "PumpkinOveralls",
//            "Samurai",
//            "SantaHelperTunic",
//            "SantaTunic",
//            "ThiefTunic",
//            "TravelerTunic"
//        };
//        private List<string> _BackOptions = new List<string>
//        {
//            "BackSword",
//            "LargeBackpack",
//            "LeatherQuiver",
//            "SmallBackpack"
//        };
//        private List<string> _EarsOptions = new List<string>
//{
//    "DarkElf",
//    "Demigod",
//    "Demon",
//    "Elf",
//    "FireLizard",
//    "Furry",
//    "Goblin",
//    "Human",
//    "Lizard",
//    "Merman",
//    "Orc",
//    "Skeleton",
//    "Vampire",
//    "Werewolf",
//    "ZombieA",
//    "ZombieB"
//};
//        private List<string> _EyesOptions = new List<string>
//        {
//            "DarkElf",
//            "Demigod",
//            "Demon",
//            "Elf",
//            "FireLizard",
//            "Furry",
//            "Goblin",
//            "Human",
//            "Lizard",
//            "Merman",
//            "Orc",
//            "Skeleton",
//            "Vampire",
//            "Werewolf",
//            "ZombieA",
//            "ZombieB"
//        };
//        private List<string> _BodyOptions = new List<string>
//        {
//            "DarkElf",
//            "Demigod",
//            "Demon",
//            "Elf",
//            "FireLizard",
//            "Furry",
//            "Goblin",
//            "Human",
//            "Lizard",
//            "Merman",
//            "Orc",
//            "Skeleton",
//            "Vampire",
//            "Werewolf",
//            "ZombieA",
//            "ZombieB"
//        };
//        private List<string> _HairOptions = new List<string>
//        {
//            "",
//            "Hair1",
//            "Hair10",
//            "Hair11",
//            "Hair12",
//            "Hair13",
//            "Hair14",
//            "Hair15",
//            "Hair2",
//            "Hair3",
//            "Hair4",
//            "Hair5",
//            "Hair6",
//            "Hair7",
//            "Hair8",
//            "Hair9"
//        };
//        private List<string> _HelmetOptions = new List<string>
//        {
//            "",
//            "ArcherHood",
//            "BanditBandana",
//            "BanditPatch [ShowEars]",
//            "BlueKnightHelmet",
//            "BlueWizzardHat",
//            "CaptainHelmet",
//            "ChiefHat [ShowEars]",
//            "ChumDoctorHelmet",
//            "ClericHood",
//            "DeathHood",
//            "DeerHorns [ShowEars]",
//            "DruidHelmet",
//            "ExecutionerHood",
//            "FireWizardHood",
//            "GuardianHelmet",
//            "HeavyKnightHelmet",
//            "HolowKnightHelmet",
//            "HornsHelmet",
//            "HornsKnightHelmet",
//            "IronKnightHelmet",
//            "LegionaryHelmet",
//            "MilitiamanHelmet",
//            "MinerHelment",
//            "MusketeerHat [ShowEars]",
//            "NecromancerHood",
//            "NinjaMask",
//            "PilotHelment",
//            "PirateBandana [ShowEars]",
//            "PirateHelmet [ShowEars]",
//            "PumpkinHead",
//            "SamuraiHelmet",
//            "SantaHelperCap [ShowEars]",
//            "SantaHood",
//            "SantaHoodBeard",
//            "ThiefHood",
//            "VikingHelmet"
//        };
//        private List<string> _WeaponOptions = new List<string>
//        {
//                    "",
//            "AmurWand",
//            "ArchStaff",
//            "AssaultSword",
//            "Axe",
//            "BastardSword",
//            "BattleAxe",
//            "BattleBow",
//            "BattleHammer",
//            "Bident",
//            "BishopStaff",
//            "BlacksmithHammer",
//            "Blade",
//            "BlueStick",
//            "BlueWand",
//            "Bow",
//            "Branch",
//            "Broadsword",
//            "Butcher",
//            "Crusher",
//            "CrystalWand",
//            "CurveBranch",
//            "CurvedBow",
//            "Cutlass",
//            "Dagger",
//            "DeathScythe",
//            "ElderStaff",
//            "Epee",
//            "Executioner",
//            "FireWand",
//            "FlameStaff",
//            "Fork",
//            "GiantBlade",
//            "GiantSword",
//            "GoldenSkepter",
//            "GoldenSkullWand",
//            "Greataxe",
//            "GreatHammer",
//            "Greatsword",
//            "GreenWand",
//            "GuardianHalberd",
//            "Halberd",
//            "Hammer",
//            "HermitStaff",
//            "HunterKnife",
//            "IronSword",
//            "Katana",
//            "KitchenAxe",
//            "Knife",
//            "Lance",
//            "LargePickaxe",
//            "LargeScythe",
//            "LongBow",
//            "LongKatana",
//            "Longsword",
//            "Mace",
//            "MagicWand",
//            "MarderDagger",
//            "MasterGreataxe",
//            "MasterWand",
//            "Morgenstern",
//            "NatureWand",
//            "NecromancerStaff",
//            "Pan",
//            "Pickaxe",
//            "Pitchfork",
//            "PriestWand",
//            "Rake",
//            "RedKatana",
//            "RedStick",
//            "RedWand",
//            "RoundMace",
//            "RoyalLongsword",
//            "RustedHammer",
//            "RustedPickaxe",
//            "RustedShortSword",
//            "RustedShovel",
//            "Saber",
//            "Scythe",
//            "Shestoper",
//            "ShitOnStick",
//            "ShortBow",
//            "ShortDagger",
//            "Sickle",
//            "SkullWand",
//            "SmallAxe",
//            "SmallPitchfork",
//            "SpikedClub",
//            "Stick",
//            "StormStaff",
//            "Sword",
//            "Tanto",
//            "WaterWand",
//            "WideDagger",
//            "WoodcutterAxe"
//        };
//        private List<string> _ShieldOptions = new List<string>
//        {
//            "",
//            "AncientGreatShield",
//            "BlueShield",
//            "BrassRoundShield",
//            "CrusaderShield",
//            "Dreadnought",
//            "GoldenEagle",
//            "GuardianShield",
//            "IronBuckler",
//            "KnightShield",
//            "LegionShield",
//            "RoyalGreatShield",
//            "SteelShield",
//            "TowerShield",
//            "WoodenBuckler"
//        };
//        private List<string> _CapeOptions = new List<string>
//        { "","Cape"};
//        private List<string> _HornOptions = new List<string>
//        { "","Horns"};
//        private List<string> _maskOptions = new List<string>
//        {"",
//            "AnimeMask",
//            "BanditMask",
//            "BlackMask",
//            "ChumMask",
//            "DarkMask",
//            "IronMask",
//            "Shadow",
//            "SuperMask"
//        };
        #endregion

        // Start is called before the first frame update
//        void Start()
//        {
//            Rebuild();
////            MakeRandomCharacter();
//        }
        public void SetByCharacterAppearance()
        {
            Head = CharacterAppearance.Head;
            Ears = CharacterAppearance.Ears;
            Eyes = CharacterAppearance.Eyes;
            Body = CharacterAppearance.Body;
            Hair = CharacterAppearance.Hair;
            Armor = CharacterAppearance.Armor;
            Helmet = CharacterAppearance.Helmet;
            Weapon = CharacterAppearance.Weapon;
            Shield = CharacterAppearance.Shield;
            Cape = CharacterAppearance.Cape;
            Back = CharacterAppearance.Back;
            Mask = CharacterAppearance.Mask;
            Horns = CharacterAppearance.Horns;
            Rebuild();
        }
        //public void MakeRandomCharacter()
        //{
        //    Head = _headOptions[Random.Range(0, _headOptions.Count)];
        //    Ears = Head;
        //    Eyes = Head;
        //    Body = Head;
        //    Hair = _HairOptions[Random.Range(0, _HairOptions.Count)];
        //    Armor = _ArmorOptions[Random.Range(0, _ArmorOptions.Count)];
        //    Helmet = _HelmetOptions[Random.Range(0, _HelmetOptions.Count)];
        //    Weapon = _WeaponOptions[Random.Range(0, _WeaponOptions.Count)];
        //    Shield = _ShieldOptions[Random.Range(0, _ShieldOptions.Count)];
        //    Cape = _CapeOptions[Random.Range(0, _CapeOptions.Count)];
        //    Back = _BackOptions[Random.Range(0, _BackOptions.Count)];
        //    Mask = _maskOptions[Random.Range(0, _maskOptions.Count)];
        //    Rebuild();
        //}
        public void Rebuild(string changed = null)
        {
            //MakeRandomCharacter();
            var width = SpriteCollection.Layers[0].Textures[0].width;
            var height = SpriteCollection.Layers[0].Textures[0].height;
            var dict = SpriteCollection.Layers.ToDictionary(i => i.Name, i => i);
            var layers = new Dictionary<string, Color32[]>();

            if (Head.Contains("Lizard")) Hair = Helmet = Mask = "";
             
            if (Back != "") layers.Add("Back", dict["Back"].GetPixels(Back, null, changed));
            if (Shield != "") layers.Add("Shield", dict["Shield"].GetPixels(Shield, null, changed));
            
            if (Body != "")
            {
                layers.Add("Body", dict["Body"].GetPixels(Body, null, changed));
                layers.Add("Arms", dict["Arms"].GetPixels(Body, null, changed == "Body" ? "Arms" : changed));
            }

            if (Head != "") layers.Add("Head", dict["Head"].GetPixels(Head, null, changed));
            if (Ears != "" && (Helmet == "" || Helmet.Contains("[ShowEars]"))) layers.Add("Ears", dict["Ears"].GetPixels(Ears, null, changed));

            if (Armor != "")
            {
                layers.Add("Armor", dict["Armor"].GetPixels(Armor, null, changed));
                layers.Add("Bracers", dict["Bracers"].GetPixels(Armor, null, changed == "Armor" ? "Bracers" : changed));
            }

            if (Eyes != "") layers.Add("Eyes", dict["Eyes"].GetPixels(Eyes, null, changed));
            if (Hair != "") layers.Add("Hair", dict["Hair"].GetPixels(Hair, Helmet == "" ? null : layers["Head"], changed));
            if (Cape != "") layers.Add("Cape", dict["Cape"].GetPixels(Cape, null, changed));
            if (Helmet != "") layers.Add("Helmet", dict["Helmet"].GetPixels(Helmet, null, changed));
            if (Weapon != "") layers.Add("Weapon", dict["Weapon"].GetPixels(Weapon, null, changed));
            if (Mask != "") layers.Add("Mask", dict["Mask"].GetPixels(Mask, null, changed));
            if (Horns != "" && Helmet == "") layers.Add("Horns", dict["Horns"].GetPixels(Horns, null, changed));

            var order = SpriteCollection.Layers.Select(i => i.Name).ToList();

            layers = layers.Where(i => i.Value != null).OrderBy(i => order.IndexOf(i.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (Texture == null) Texture = new Texture2D(width, height) { filterMode = FilterMode.Point };

            if (Shield != "")
            {
                var shield = layers["Shield"];
                var last = layers.Last(i => i.Key != "Weapon");
                var copy = last.Value.ToArray();

                for (var i = 64 * 256; i < 2 * 64 * 256; i++)
                {
                    if (shield[i].a > 0) copy[i] = shield[i];
                }

                layers[last.Key] = copy;
            }
            
            Texture = TextureHelper.MergeLayers(Texture, layers.Values.ToArray());
            Texture.SetPixels(0, 912 - 16, 16, 16, new Color[16 * 16]);

            if (Cape != "") CapeOverlay(layers["Cape"]);

            if (_sprites == null)
            {
                var clipNames = new List<string> { "Idle", "Ready", "Run", "Crawl", "Climb", "Jump", "Push", "Jab", "Slash", "Shot", "Fire1H", "Fire2H", "Block", "Death" };

                clipNames.Reverse();

                _sprites = new Dictionary<string, Sprite>();

                for (var i = 0; i < clipNames.Count; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        var key = clipNames[i] + "_" + j;

                        _sprites.Add(key, Sprite.Create(Texture, new Rect(j * 64, i * 64, 64, 64), new Vector2(0.5f, 0.125f), 16, 0, SpriteMeshType.FullRect));
                    }
                }
            }

            var spriteLibraryAsset = ScriptableObject.CreateInstance<UnityEngine.U2D.Animation.SpriteLibraryAsset>();

            foreach (var sprite in _sprites)
            {
                var split = sprite.Key.Split('_');

                spriteLibraryAsset.AddCategoryLabel(sprite.Value, split[0], split[1]);
            }

            SpriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
        }

        private void CapeOverlay(Color32[] cape)
        {
            if (Cape == "") return;
            
            var pixels = Texture.GetPixels32();
            var width = Texture.width;
            var height = Texture.height;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (x >= 0 && x < 2 * 64 && y >= 9 * 64 && y < 10 * 64 // "Climb_0", "Climb_1"
                        || x >= 64 && x < 64 + 2 * 64 && y >= 6 * 64 && y < 7 * 64 // "Jab_1", "Jab_2"
                        || x >= 128 && x < 128 + 2 * 64 && y >= 5 * 64 && y < 6 * 64 // "Slash_2", "Slash_3"
                        || x >= 0 && x < 4 * 64 && y >= 4 * 64 && y < 5 * 64) // "Shot_0", "Shot_1", "Shot_2", "Shot_3"
                    {
                        var i = x + y * width;

                        if (cape[i].a > 0) pixels[i] = cape[i];
                    }
                }
            }

            Texture.SetPixels32(pixels);
            Texture.Apply();
        }
    }
}