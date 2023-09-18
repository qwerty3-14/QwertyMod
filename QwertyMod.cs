using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using QwertyMod.Common;
using QwertyMod.Common.Playerlayers;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content;
using QwertyMod.Content.Items;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Consumable.BossSummon;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.Consumable.Tiles.MusicBoxes;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Ancient;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Blade;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.FortressBoss;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Hydra;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Polar;
using QwertyMod.Content.Items.Equipment.Accessories;
using QwertyMod.Content.Items.Equipment.Accessories.Expert;
using QwertyMod.Content.Items.Equipment.Accessories.Expert.Doppleganger;
using QwertyMod.Content.Items.Equipment.Accessories.Expert.HyperRunestone;
using QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath;
using QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Equipment.Armor.Bionic;
using QwertyMod.Content.Items.Equipment.Armor.Caelite;
using QwertyMod.Content.Items.Equipment.Armor.Gale;
using QwertyMod.Content.Items.Equipment.Armor.Hydra;
using QwertyMod.Content.Items.Equipment.Armor.Invader;
using QwertyMod.Content.Items.Equipment.Armor.Lune;
using QwertyMod.Content.Items.Equipment.Armor.Rhuthinium;
using QwertyMod.Content.Items.Equipment.Armor.Shaman;
using QwertyMod.Content.Items.Equipment.Armor.Vitallum;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Equipment.Vanity.SilkDress;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Pet;
using QwertyMod.Content.Items.Tool.FishingRod;
using QwertyMod.Content.Items.Tool.Mining;
using QwertyMod.Content.Items.Tool.Mining.Ancient;
using QwertyMod.Content.Items.Tool.Mining.TheDevourer;
using QwertyMod.Content.Items.Weapon.Magic.AncientMissile;
using QwertyMod.Content.Items.Weapon.Magic.AncientWave;
using QwertyMod.Content.Items.Weapon.Magic.BlackHole;
using QwertyMod.Content.Items.Weapon.Magic.ExtinctionGun;
using QwertyMod.Content.Items.Weapon.Magic.HydraBeam;
using QwertyMod.Content.Items.Weapon.Magic.HydraMissile;
using QwertyMod.Content.Items.Weapon.Magic.PenguinWhistle;
using QwertyMod.Content.Items.Weapon.Magic.Plasma;
using QwertyMod.Content.Items.Weapon.Magic.RestlessSun;
using QwertyMod.Content.Items.Weapon.Magic.Swordpocalypse;
using QwertyMod.Content.Items.Weapon.Melee.Flail.Ankylosaurus;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Hydra;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Imperium;
using QwertyMod.Content.Items.Weapon.Melee.Misc;
using QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrent;
using QwertyMod.Content.Items.Weapon.Melee.Sword;
using QwertyMod.Content.Items.Weapon.Melee.Sword.AncientBlade;
using QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV;
using QwertyMod.Content.Items.Weapon.Melee.Top.Cyclone;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.AncientThrow;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.Arsenal;
using QwertyMod.Content.Items.Weapon.Minion.AncientMinion;
using QwertyMod.Content.Items.Weapon.Minion.HydraHead;
using QwertyMod.Content.Items.Weapon.Minion.Longsword;
using QwertyMod.Content.Items.Weapon.Minion.Priest;
using QwertyMod.Content.Items.Weapon.Minion.UrQuan;
using QwertyMod.Content.Items.Weapon.Morphs.AncientNuke;
using QwertyMod.Content.Items.Weapon.Morphs.HydraBarrage;
using QwertyMod.Content.Items.Weapon.Morphs.Swordquake;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.Ancient;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.B4Bow;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.HolyExiler;
using QwertyMod.Content.Items.Weapon.Ranged.DartLauncher.Whirpool;
using QwertyMod.Content.Items.Weapon.Ranged.Gun;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.DinoVulcan;
using QwertyMod.Content.Items.Weapon.Ranged.SpecialAmmo;
using QwertyMod.Content.Items.Weapon.Sentry.BubbleBrewer;
using QwertyMod.Content.Items.Weapon.Whip.Discipline;
using QwertyMod.Content.NPCs.Bosses.AncientMachine;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using QwertyMod.Content.NPCs.Bosses.CloakedDarkBoss;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using QwertyMod.Content.NPCs.Bosses.Hydra;
using QwertyMod.Content.NPCs.Bosses.OLORD;
using QwertyMod.Content.NPCs.Bosses.RuneGhost;
using QwertyMod.Content.NPCs.Bosses.TundraBoss;
using QwertyMod.Content.NPCs.DinoMilitia;
using QwertyMod.Content.NPCs.Bosses.InvaderBattleship;
using QwertyMod.Content.Items.Consumable.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Equipment.Vanity.RunicRobe;
using QwertyMod.Content.Items.Equipment.Armor.Hero;
using QwertyMod.Content.Items.Equipment.VanityAccessories.Corset;
using QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using QwertyMod.Content.Items.Equipment.Vanity.PurpleDress;
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.AngelicTracker;
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.Lune;
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.Rhuthinium;
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.SeraphimPredator;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Rhuthinium;
using QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrospear;
using QwertyMod.Content.Items.Weapon.Melee.Sword.EtimsSword;
using QwertyMod.Content.Items.Weapon.Melee.Sword.Overkill;
using QwertyMod.Content.Items.Weapon.Melee.Sword.RuneBlade;
using QwertyMod.Content.Items.Weapon.Melee.Top.Lune;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SuperquantumRifle;
using System.Linq;
using QwertyMod.Content.Dusts;


namespace QwertyMod
{
    public class QwertyMod : Mod
    {

        public static Texture2D debugCross;
        public static QwertyMod Instance;
        private static Vector2[] LocalCursor = new Vector2[Main.player.Length];

        //public const string HydraHead1 = "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead1";
        //public const string HydraHead2 = "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead2";
        //public const string HydraHead3 = "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead3";
        public static ModKeybind YetAnotherSpecialAbility;

        public static int hydraLegMale = 0;
        public static int hydraLegFemale = 0;
        public static int shamaLegMale = 0;
        public static int shamanLegFemale = 0;
        public static int LuneLegMale = 0;
        public static int LuneLegFemale = 0;
        public static int RhuthiniumLegMale = 0;
        public static int RhuthiniumLegFemale = 0;
        public static int CaeliteLegMale = 0;
        public static int CaeliteLegFemale = 0;
        public static int GaleLegMale = 0;
        public static int GaleLegFemale = 0;
        public static int VitLegMale = 0;
        public static int VitLegFemale = 0;
        public static int BionicLegMale = 0;
        public static int BionicLegFemale = 0;
        public static int invaderLanderMale = 0;
        public static int invaderLanderFemale = 0;
        public static int RuneLegMale = 0;
        public static int RuneLegFemale = 0;
        public static int HeroShieldHandOn = 0;
        public static int HeroShieldShield = 0;
        public static int HeroShieldShieldUp = 0;
        public static int HeroPantsMale = 0;
        public static int HeroPantsFemale = 0;
        public static int CorsetMale = 0;
        public static int BallGownSkirt = 0;
        public static int BallGownSkirtAlt = 0;
        public static int SilkSkirt = 0;
        public static int PurpleSkirt = 0;
        public static int PurpleSkirtAlt = 0;
        public static SoundStyle FortressBlocks;
        public override void Load()
        {

            Instance = this;
            YetAnotherSpecialAbility = KeybindLoader.RegisterKeybind(this, "Yet Another Special Ability Key", Keys.R);

            //AddBossHeadTexture(HydraHead1);
            //AddBossHeadTexture(HydraHead2);
            //AddBossHeadTexture(HydraHead3);
            FortressBlocks = SoundID.Tink;
            if (!Main.dedServ)
            {
                hydraLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_Legs", EquipType.Legs, GetModItem(ItemType<HydraLeggings>()));
                hydraLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_FemaleLegs", EquipType.Legs, GetModItem(ItemType<HydraLeggings>()));
                shamaLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Shaman/ShamanLegs_Legs", EquipType.Legs, GetModItem(ItemType<ShamanLegs>()));
                shamanLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Shaman/ShamanLegs_FemaleLegs", EquipType.Legs, GetModItem(ItemType<ShamanLegs>()));
                LuneLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Lune/LuneLeggings_Legs", EquipType.Legs, GetModItem(ItemType<LuneLeggings>()));
                LuneLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Lune/LuneLeggings_FemaleLegs", EquipType.Legs, GetModItem(ItemType<LuneLeggings>()));
                RhuthiniumLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Rhuthinium/RhuthiniumGreaves_Legs", EquipType.Legs, GetModItem(ItemType<RhuthiniumGreaves>()));
                RhuthiniumLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Rhuthinium/RhuthiniumGreaves_FemaleLegs", EquipType.Legs, GetModItem(ItemType<RhuthiniumGreaves>()));
                CaeliteLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Caelite/CaeliteGreaves_Legs", EquipType.Legs, GetModItem(ItemType<CaeliteGreaves>()));
                CaeliteLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Caelite/CaeliteGreaves_FemaleLegs", EquipType.Legs, GetModItem(ItemType<CaeliteGreaves>()));
                GaleLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Gale/GaleSwiftRobes_Legs", EquipType.Legs, GetModItem(ItemType<GaleSwiftRobes>()));
                GaleLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Gale/GaleSwiftRobes_FemaleLegs", EquipType.Legs, GetModItem(ItemType<GaleSwiftRobes>()));
                VitLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_Legs", EquipType.Legs, GetModItem(ItemType<VitallumJeans>()));
                VitLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_FemaleLegs", EquipType.Legs, GetModItem(ItemType<VitallumJeans>()));
                BionicLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicLimbs_Legs", EquipType.Legs, GetModItem(ItemType<BionicLimbs>()));
                BionicLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicLimbs_FemaleLegs", EquipType.Legs, GetModItem(ItemType<BionicLimbs>()));
                invaderLanderMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderLanders_Legs", EquipType.Legs, GetModItem(ItemType<InvaderLanders>()));
                invaderLanderFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderLanders_FemaleLegs", EquipType.Legs, GetModItem(ItemType<InvaderLanders>()));
                RuneLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/RunicRobe/RunicRobe_Legs", EquipType.Legs, GetModItem(ItemType<RunicRobe>()));
                RuneLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/RunicRobe/RunicRobe_FemaleLegs", EquipType.Legs, GetModItem(ItemType<RunicRobe>()));

                HeroShieldHandOn = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroShield_HandsOn", EquipType.HandsOn, GetModItem(ItemType<HeroShield>()));
                HeroShieldShield = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroShield_Shield", EquipType.Shield, GetModItem(ItemType<HeroShield>()));
                HeroShieldShieldUp = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroShield_ShieldUp", EquipType.Shield, name: "ShieldUp");

                HeroPantsMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroPants_Legs", EquipType.Legs, GetModItem(ItemType<HeroPants>()));
                HeroPantsFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroPants_FemaleLegs", EquipType.Legs, GetModItem(ItemType<HeroPants>()));

                CorsetMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/VanityAccessories/Corset/Corset_WaistMale", EquipType.Waist, name: "CorsetMale");

                BallGownSkirt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/ScarletBallGown/ScarletBallGown_Legs", EquipType.Legs, GetModItem(ItemType<ScarletBallGown>()));
                BallGownSkirtAlt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/ScarletBallGown/ScarletBallGown_LegsAlt", EquipType.Legs, name: "BallGownAlt");
                SilkSkirt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/SilkDress/SilkDress_Legs", EquipType.Legs, GetModItem(ItemType<SilkDress>()));
                PurpleSkirt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/PurpleDress/PurpleDress_Legs", EquipType.Legs, GetModItem(ItemType<PurpleDress>()));
                PurpleSkirtAlt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/PurpleDress/PurpleDress_AltLegs", EquipType.Legs, name: "PurpleDressAlt");
                Main.QueueMainThreadAction(() =>
                {
                    RuneSprites.BuildRunes();
                    NoehtnapAnimations.BuildAnims();
                    Turret.LoadTextures();
                });

            }

        }
        public override void PostSetupContent()
        {
            if (!Main.dedServ)
            {
                Main.QueueMainThreadAction(() =>
                {
                    debugCross = Request<Texture2D>("QwertyMod/DebugCross", AssetRequestMode.ImmediateLoad).Value;

                    OnHeadDraw.RegisterHeads();
                    OnLegDraw.RegisterLegs();
                    OnBodyDraw.ReigsterBodies();
                });
            }
            
            if (ModLoader.HasMod("TRAEProject"))
            {
                Mod TRAE = ModLoader.GetMod("TRAEProject");
                if(TRAE.Version.Major >= 1 || TRAE.Version.Minor >= 6)
                {
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<CaeliteBoomerang>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<LuneBoomerang>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<RhuthiniumBoomerang>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<SeraphimPredator>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<DinoFlail>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<HydraJavelin>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<Imperium>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<RhuthiniumJavelin>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<BlessedMonsoonKnife>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<CaeliteRainKnife>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<Cyclone>());
                    TRAE.Call("BoomerangPrefix", ModContent.ItemType<LuneTop>());
                }
                if(TRAE.Version.Major >= 1 || TRAE.Version.Minor >= 6)
                {
                    TRAE.Call("RegisterSidearm", ModContent.ItemType<Content.Items.Weapon.Magic.Lune.LuneStaff>(), ModContent.ProjectileType<Content.Items.Weapon.Magic.Lune.LuneCrest>(), 20, 20, 0);
                }

            }
            if (ModLoader.HasMod("BossChecklist"))
            {
                Mod bossChecklist = ModLoader.GetMod("BossChecklist");
                if (bossChecklist != null)
                {
                    
                    bossChecklist.Call("AddBoss", 5.5f, NPCType<AncientMachine>(), this, "Ancient Machine", (Func<bool>)(() => DownedBossSystem.downedAncient), ItemType<AncientEmblem>(),
                        new List<int> { ItemType<AncientMachineTrophy>(), ItemType<MusicBoxBuiltToDestroy>() },
                        new List<int> { ItemType<AncientMachineBag>(), ItemType<AncientBlade>(), ItemType<AncientThrow>(), ItemType<AncientLongbow>(), ItemType<AncientSniper>(), ItemType<AncientMissileStaff>(), ItemType<AncientWave>(), ItemType<AncientMinionStaff>(), ItemType<AncientNuke>(), ItemType<AncientMiner>(), ItemID.HealingPotion },
                        "Look into the [i:" + ItemType<AncientEmblem>() + "]", null, "QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMachine_Checklist");

                    bossChecklist.Call("AddBoss", 7.000001f, NPCType<Hydra>(), this, "The Hydra", (Func<bool>)(() => DownedBossSystem.downedHydra), ItemType<HydraSummon>(),
                        new List<int> { ItemType<HydraTrophy>(), ItemType<MusicBoxBeastOfThreeHeads>() },
                        new List<int> { ItemType<HydraBag>(), ItemType<Hydrent>(), ItemType<HydraJavelin>(), ItemType<HydraCannon>(), ItemType<HydraBeam>(), ItemType<HydraMissileStaff>(), ItemType<HydraHeadStaff>(), ItemType<HydraBarrage>(), ItemType<Hydrill>(), ItemType<Hydrator>(), ItemType<HydraScale>(), ItemID.GreaterHealingPotion },
                        "Tempt with [i:" + ItemType<HydraSummon>() + "]", null, "QwertyMod/Content/NPCs/Bosses/Hydra/Hydra_Checklist", "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead1");

                    bossChecklist.Call("AddBoss", 14.2f, NPCType<RuneGhost>(), this, "Rune Ghost", (Func<bool>)(() => DownedBossSystem.downedRuneGhost), ItemType<SummoningRune>(),
                        new List<int> { ItemType<RuneGhostMask>(), ItemType<MusicBoxTheConjurer>() },
                        new List<int> { ItemType<RuneGhostBag>(), ItemType<HyperRunestone>(), ItemType<AggroScroll>(), ItemType<IceScroll>(), ItemType<LeechScroll>(), ItemType<PursuitScroll>(), ItemType<CraftingRune>(), ItemID.GreaterHealingPotion },
                        "Use a [i:" + ItemType<SummoningRune>() + "] to challenge its power. [i:" + ItemType<SummoningRune>() + "] drops from the dungeon's spirits");

                    bossChecklist.Call("AddBoss", 15.8f, NPCType<OLORDv2>(), this, "Oversized Laser-emitting Obliteration Radiation-emitting Destroyer", (Func<bool>)(() => DownedBossSystem.downedOLORD), ItemType<B4Summon>(),
                        new List<int> { ItemType<MusicBoxEPIC>() },
                        new List<int> { ItemType<B4Bag>(), ItemType<B4ExpertItem>(), ItemType<B4Bow>(), ItemType<BlackHoleStaff>(), ItemType<ExplosivePierce>(), ItemType<DreadnoughtStaff>(), ItemType<TheDevourer>(), ItemID.GreaterHealingPotion },
                        "Use a [i:" + ItemType<B4Summon>() + "]", null, "QwertyMod/Content/NPCs/Bosses/OLORD/BackGround");

                    bossChecklist.Call("AddBoss", 4.2f, NPCType<FortressBoss>(), this, "The Divine Light", (Func<bool>)(() => DownedBossSystem.downedDivineLight), ItemType<FortressBossSummon>(),
                        new List<int> { ItemType<FortressBossTrophy>(), ItemType<DivineLightMask>(), ItemType<MusicBoxHigherBeing>() },
                        new List<int> { ItemType<FortressBossBag>(), ItemType<CaeliteRainKnife>(), ItemType<HolyExiler>(), ItemType<CaeliteMagicWeapon>(), ItemType<PriestStaff>(), ItemType<Lightling>(), ItemType<SkywardHilt>(), ItemType<CaeliteBar>(), ItemType<CaeliteCore>(), ItemID.LesserManaPotion },
                        "Use a [i:" + ItemType<FortressBossSummon>() + "]" + " at the altar in the sky fortress", null);

                    bossChecklist.Call("AddBoss", .7f, NPCType<PolarBear>(), this, "Polar Exterminator", (Func<bool>)(() => DownedBossSystem.downedBear), null,
                        new List<int> { ItemType<PolarTrophy>(), ItemType<PolarMask>() },
                        new List<int> { ItemType<TundraBossBag>(), ItemType<PenguinGenerator>(), ItemType<PenguinClub>(), ItemType<PenguinLauncher>(), ItemType<PenguinWhistle>(), ItemID.Penguin, ItemID.LesserHealingPotion },
                        "Hibernates in the underground tundra. After defeating it it will respawn the next day.", null);

                    bossChecklist.Call("AddBoss", 11.4f, NPCType<Imperious>(), this, "Imperious", (Func<bool>)(() => DownedBossSystem.downedBlade), null,
                        new List<int> { ItemType<BladeBossTrophy>(), ItemType<MusicBoxBladeOfAGod>() },
                        new List<int> { ItemType<BladeBossBag>(), ItemType<ImperiousSheath>(), ItemType<ImperiousTheIV>(), ItemType<Discipline>(), ItemType<Arsenal>(), ItemType<BladedArrowShaft>(), ItemType<SwordStormStaff>(), ItemType<SwordMinionStaff>(), ItemType<Imperium>(), ItemType<Swordquake>(), ItemType<SwordsmanBadge>(), ItemID.GreaterHealingPotion },
                        "Use the [i:" + ItemType<BladeBossSummon>() + "], and accept its challenge", null);

                    bossChecklist.Call("AddBoss", 5.7f, NPCType<CloakedDarkBoss>(), this, "Noehtnap", (Func<bool>)(() => DownedBossSystem.downedNoehtnap), ItemType<RitualInterupter>(),
                        new List<int> { ItemType<MusicBoxTheGodsBleed>() },
                        new List<int> { ItemType<NoehtnapBag>(), ItemType<Doppleganger>(), ItemType<Etims>(), ItemID.HealingPotion },
                        "Just use the [i:" + ItemType<RitualInterupter>() + "] mortal!", null, "QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/CloakedDarkBoss_Checklist");

                    bossChecklist.Call("AddEvent", 11.001f,
                        new List<int> { NPCType<Utah>(), NPCType<Triceratank>(), NPCType<AntiAir>(), NPCType<Velocichopper>(), NPCType<TheGreatTyrannosaurus>() },
                        this, "Dino Militia", (Func<bool>)(() => DownedBossSystem.downedDinos), ItemType<DinoEgg>(),
                        ItemType<MusicBoxOldDinosNewGuns>(),
                        new List<int> { ItemType<DinoFlail>(), ItemType<DinoVulcan>(), ItemType<TheTyrantsExtinctionGun>(), ItemType<Tricerashield>(), ItemType<DinoTooth>() },
                        "Use a [i:" + ItemType<DinoEgg>() + "] and they'll come to drive you extinct!", null, "QwertyMod/Content/NPCs/DinoMilitia/TheGreatTyrannosaurus_Bestiary", "QwertyMod/Content/Items/Consumable/BossSummons/DinoEgg");


                    bossChecklist.Call("AddToBossLoot", "Terraria", "DukeFishron", new List<int> { ItemType<Cyclone>(), ItemType<Whirlpool>(), ItemType<BubbleBrewerBaton>() });
                    
                }
            }
        }
        public static Vector2 GetLocalCursor(int id)
        {
            LocalCursor[id] = Main.MouseWorld;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Instance.GetPacket();
                packet.Write((byte)ModMessageType.UpdateLocalCursor); // Message type, you would need to create an enum for this
                packet.Write((byte)id);
                packet.WriteVector2(LocalCursor[id]);
                packet.Send();
            }
            return LocalCursor[id];
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModMessageType msgType = (ModMessageType)reader.ReadByte();
            switch (msgType)
            {
                case ModMessageType.UpdateLocalCursor:
                    byte playerIndex = reader.ReadByte();
                    Vector2 Cursor = reader.ReadVector2();

                    LocalCursor[playerIndex] = Cursor;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.UpdateLocalCursor); // Message type, you would need to create an enum for this
                        packet.Write(playerIndex);
                        packet.WriteVector2(Cursor);
                        packet.Send();
                    }
                    break;
                case ModMessageType.DivineCall:
                    Vector2 summonAt = reader.ReadVector2();
                    int playerID = reader.ReadInt32();
                    int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(playerID), (int)summonAt.X, (int)summonAt.Y, NPCType<FortressBoss>());
                    break;
                case ModMessageType.StartDinoEvent:
                    DinoEvent.EventActive = true;
                    DinoEvent.DinoKillCount = 0;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    }

                    break;
                case ModMessageType.SpawnBear:
                    Vector2 summonAt2 = reader.ReadVector2();
                    int playerID2 = reader.ReadInt32();
                    int npcID2 = NPC.NewNPC(NPC.GetBossSpawnSource(playerID2), (int)summonAt2.X, (int)summonAt2.Y, NPCType<Sleeping>());
                    break;
                case ModMessageType.SummonBattleship:
                    Vector2 summonAt3 = reader.ReadVector2();
                    int playerID3 = reader.ReadInt32();
                    int npcID3 = NPC.NewNPC(NPC.GetBossSpawnSource(playerID3), (int)summonAt3.X, (int)summonAt3.Y, NPCType<InvaderBattleship>());
                    break;
                case ModMessageType.AutoDesummon:
                    int playerID4 = reader.ReadInt32();
                    for(int i = 0; i < 1000; i++)
                    {
                        if(Main.projectile[i].active && Main.projectile[i].owner == playerID4 && Main.projectile[i].minionSlots > 0)
                        {
                            Main.projectile[i].Kill();
                        }
                    }
                    break;
                case ModMessageType.AmmoEnchantEtims:
                    int projectileID = reader.ReadInt32();
                    Main.projectile.FirstOrDefault(x => x.identity == projectileID).GetGlobalProjectile<EtimsProjectile>().effect = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.AmmoEnchantEtims);
                        packet.Write(projectileID);
                        packet.Send();
                    }
                    break;
                case ModMessageType.AmmoEnchantArrowWarping:
                    int projectileID2 = reader.ReadInt32();
                    Main.projectile.FirstOrDefault(x => x.identity == projectileID2).GetGlobalProjectile<ArrowWarping>().warpedArrow = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.AmmoEnchantArrowWarping);
                        packet.Write(projectileID2);
                        packet.Send();
                    }
                    break;
                case ModMessageType.AmmoEnchantArrowHoming:
                    int projectileID3 = reader.ReadInt32();
                    Main.projectile.FirstOrDefault(x => x.identity == projectileID3).GetGlobalProjectile<arrowHoming>().B4HomingArrow = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.AmmoEnchantArrowHoming);
                        packet.Write(projectileID3);
                        packet.Send();
                    }
                    break;
                case ModMessageType.AmmoEnchantQuantum:
                    int projectileID4 = reader.ReadInt32();
                    Main.projectile.FirstOrDefault(x => x.identity == projectileID4).GetGlobalProjectile<QuantumProjectile>().isQuantum = true;
                    Vector2 qC = reader.ReadVector2();
                    Main.projectile.FirstOrDefault(x => x.identity == projectileID4).GetGlobalProjectile<QuantumProjectile>().quantumCenter = qC;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.AmmoEnchantQuantum);
                        packet.Write(projectileID4);
                        packet.WriteVector2(qC);
                        packet.Send();
                    }
                    break;
                case ModMessageType.SpawnQuantumRing:
                    Vector2 ringCenter = reader.ReadVector2();
                    if(Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)ModMessageType.SpawnQuantumRing);
                        packet.WriteVector2(ringCenter);
                        packet.Send();
                    }
                    else
                    {
                        for(int d = 0; d < 40; d++)
                        {
                            float dRot = ((float)d / 40f) * MathF.PI * 2f;
                            Dust.NewDustPerfect(ringCenter, ModContent.DustType<DarknessDust>(), QwertyMethods.PolarVector(1, dRot));
                        }
                    }
                    break;
            }
        }
    }
    internal enum ModMessageType : byte
    {

        DivineCall,
        UpdateLocalCursor,
        StartDinoEvent,
        SpawnBear,
        SummonBattleship,
        AutoDesummon,
        AmmoEnchantEtims,
        AmmoEnchantArrowWarping,
        AmmoEnchantArrowHoming,
        AmmoEnchantQuantum,
        SpawnQuantumRing
    }
}