using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QwertyMod.Common;
using QwertyMod.Common.Playerlayers;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content;
using QwertyMod.Content.Dusts;
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
using QwertyMod.Content.Items.Equipment.Armor.Hero;
using QwertyMod.Content.Items.Equipment.Armor.Hydra;
using QwertyMod.Content.Items.Equipment.Armor.Invader;
using QwertyMod.Content.Items.Equipment.Armor.Lune;
using QwertyMod.Content.Items.Equipment.Armor.Rhuthinium;
using QwertyMod.Content.Items.Equipment.Armor.Shaman;
using QwertyMod.Content.Items.Equipment.Armor.Vitallum;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Equipment.Vanity.PurpleDress;
using QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown;
using QwertyMod.Content.Items.Equipment.Vanity.RunicRobe;
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
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.AngelicTracker;
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.Lune;
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.Rhuthinium;
using QwertyMod.Content.Items.Weapon.Melee.Boomerang.SeraphimPredator;
using QwertyMod.Content.Items.Weapon.Melee.Flail.Ankylosaurus;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Hydra;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Imperium;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Rhuthinium;
using QwertyMod.Content.Items.Weapon.Melee.Misc;
using QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrent;
using QwertyMod.Content.Items.Weapon.Melee.Sword;
using QwertyMod.Content.Items.Weapon.Melee.Sword.AncientBlade;
using QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV;
using QwertyMod.Content.Items.Weapon.Melee.Top.Cyclone;
using QwertyMod.Content.Items.Weapon.Melee.Top.Lune;
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
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SuperquantumRifle;
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
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;



namespace QwertyMod
{
    public class QwertyMod : Mod
    {

        public static Texture2D debugCross;
        public static QwertyMod Instance;
        private static Vector2[] LocalCursor = new Vector2[Main.player.Length];
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

            
            FortressBlocks = SoundID.Tink;
            if (!Main.dedServ)
            {
                hydraLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<HydraLeggings>()));
                hydraLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<HydraLeggings>()));
                shamaLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Shaman/ShamanLegs_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<ShamanLegs>()));
                shamanLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Shaman/ShamanLegs_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<ShamanLegs>()));
                LuneLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Lune/LuneLeggings_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<LuneLeggings>()));
                LuneLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Lune/LuneLeggings_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<LuneLeggings>()));
                RhuthiniumLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Rhuthinium/RhuthiniumGreaves_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<RhuthiniumGreaves>()));
                RhuthiniumLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Rhuthinium/RhuthiniumGreaves_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<RhuthiniumGreaves>()));
                CaeliteLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Caelite/CaeliteGreaves_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<CaeliteGreaves>()));
                CaeliteLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Caelite/CaeliteGreaves_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<CaeliteGreaves>()));
                GaleLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Gale/GaleSwiftRobes_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<GaleSwiftRobes>()));
                GaleLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Gale/GaleSwiftRobes_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<GaleSwiftRobes>()));
                VitLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<VitallumJeans>()));
                VitLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<VitallumJeans>()));
                BionicLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicLimbs_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<BionicLimbs>()));
                BionicLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicLimbs_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<BionicLimbs>()));
                invaderLanderMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderLanders_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<InvaderLanders>()));
                invaderLanderFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderLanders_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<InvaderLanders>()));
                RuneLegMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/RunicRobe/RunicRobe_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<RunicRobe>()));
                RuneLegFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/RunicRobe/RunicRobe_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<RunicRobe>()));

                HeroShieldHandOn = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroShield_HandsOn", EquipType.HandsOn, ModContent.GetModItem(ModContent.ItemType<HeroShield>()));
                HeroShieldShield = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroShield_Shield", EquipType.Shield, ModContent.GetModItem(ModContent.ItemType<HeroShield>()));
                HeroShieldShieldUp = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroShield_ShieldUp", EquipType.Shield, name: "ShieldUp");

                HeroPantsMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroPants_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<HeroPants>()));
                HeroPantsFemale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Armor/Hero/HeroPants_FemaleLegs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<HeroPants>()));

                CorsetMale = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/VanityAccessories/Corset/Corset_WaistMale", EquipType.Waist, name: "CorsetMale");

                BallGownSkirt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/ScarletBallGown/ScarletBallGown_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<ScarletBallGown>()));
                BallGownSkirtAlt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/ScarletBallGown/ScarletBallGown_LegsAlt", EquipType.Legs, name: "BallGownAlt");
                SilkSkirt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/SilkDress/SilkDress_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<SilkDress>()));
                PurpleSkirt = EquipLoader.AddEquipTexture(this, "QwertyMod/Content/Items/Equipment/Vanity/PurpleDress/PurpleDress_Legs", EquipType.Legs, ModContent.GetModItem(ModContent.ItemType<PurpleDress>()));
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
                    debugCross = ModContent.Request<Texture2D>("QwertyMod/DebugCross", AssetRequestMode.ImmediateLoad).Value;

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
                    
                    bossChecklist.Call("AddBoss", 5.5f, ModContent.NPCType<AncientMachine>(), this, "Ancient Machine", (Func<bool>)(() => DownedBossSystem.downedAncient), ModContent.ItemType<AncientEmblem>(),
                        new List<int> { ModContent.ItemType<AncientMachineTrophy>(), ModContent.ItemType<MusicBoxBuiltToDestroy>() },
                        new List<int> { ModContent.ItemType<AncientMachineBag>(), ModContent.ItemType<AncientBlade>(), ModContent.ItemType<AncientThrow>(), ModContent.ItemType<AncientLongbow>(), ModContent.ItemType<AncientSniper>(), ModContent.ItemType<AncientMissileStaff>(), ModContent.ItemType<AncientWave>(), ModContent.ItemType<AncientMinionStaff>(), ModContent.ItemType<AncientNuke>(), ModContent.ItemType<AncientMiner>(), ItemID.HealingPotion },
                        "Look into the [i:" + ModContent.ItemType<AncientEmblem>() + "]", null, "QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMachine_Checklist");

                    bossChecklist.Call("AddBoss", 7.000001f, ModContent.NPCType<Hydra>(), this, "The Hydra", (Func<bool>)(() => DownedBossSystem.downedHydra), ModContent.ItemType<HydraSummon>(),
                        new List<int> { ModContent.ItemType<HydraTrophy>(), ModContent.ItemType<MusicBoxBeastOfThreeHeads>() },
                        new List<int> { ModContent.ItemType<HydraBag>(), ModContent.ItemType<Hydrent>(), ModContent.ItemType<HydraJavelin>(), ModContent.ItemType<HydraCannon>(), ModContent.ItemType<HydraBeam>(), ModContent.ItemType<HydraMissileStaff>(), ModContent.ItemType<HydraHeadStaff>(), ModContent.ItemType<HydraBarrage>(), ModContent.ItemType<Hydrill>(), ModContent.ItemType<Hydrator>(), ModContent.ItemType<HydraScale>(), ItemID.GreaterHealingPotion },
                        "Tempt with [i:" + ModContent.ItemType<HydraSummon>() + "]", null, "QwertyMod/Content/NPCs/Bosses/Hydra/Hydra_Checklist", "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead1");

                    bossChecklist.Call("AddBoss", 14.2f, ModContent.NPCType<RuneGhost>(), this, "Rune Ghost", (Func<bool>)(() => DownedBossSystem.downedRuneGhost), ModContent.ItemType<SummoningRune>(),
                        new List<int> { ModContent.ItemType<RuneGhostMask>(), ModContent.ItemType<MusicBoxTheConjurer>() },
                        new List<int> { ModContent.ItemType<RuneGhostBag>(), ModContent.ItemType<HyperRunestone>(), ModContent.ItemType<AggroScroll>(), ModContent.ItemType<IceScroll>(), ModContent.ItemType<LeechScroll>(), ModContent.ItemType<PursuitScroll>(), ModContent.ItemType<CraftingRune>(), ItemID.GreaterHealingPotion },
                        "Use a [i:" + ModContent.ItemType<SummoningRune>() + "] to challenge its power. [i:" + ModContent.ItemType<SummoningRune>() + "] drops from the dungeon's spirits");

                    bossChecklist.Call("AddBoss", 15.8f, ModContent.NPCType<OLORDv2>(), this, "Oversized Laser-emitting Obliteration Radiation-emitting Destroyer", (Func<bool>)(() => DownedBossSystem.downedOLORD), ModContent.ItemType<B4Summon>(),
                        new List<int> { ModContent.ItemType<MusicBoxEPIC>() },
                        new List<int> { ModContent.ItemType<B4Bag>(), ModContent.ItemType<B4ExpertItem>(), ModContent.ItemType<B4Bow>(), ModContent.ItemType<BlackHoleStaff>(), ModContent.ItemType<ExplosivePierce>(), ModContent.ItemType<DreadnoughtStaff>(), ModContent.ItemType<TheDevourer>(), ItemID.GreaterHealingPotion },
                        "Use a [i:" + ModContent.ItemType<B4Summon>() + "]", null, "QwertyMod/Content/NPCs/Bosses/OLORD/BackGround");

                    bossChecklist.Call("AddBoss", 4.2f, ModContent.NPCType<FortressBoss>(), this, "The Divine Light", (Func<bool>)(() => DownedBossSystem.downedDivineLight), ModContent.ItemType<FortressBossSummon>(),
                        new List<int> { ModContent.ItemType<FortressBossTrophy>(), ModContent.ItemType<DivineLightMask>(), ModContent.ItemType<MusicBoxHigherBeing>() },
                        new List<int> { ModContent.ItemType<FortressBossBag>(), ModContent.ItemType<CaeliteRainKnife>(), ModContent.ItemType<HolyExiler>(), ModContent.ItemType<CaeliteMagicWeapon>(), ModContent.ItemType<PriestStaff>(), ModContent.ItemType<Lightling>(), ModContent.ItemType<SkywardHilt>(), ModContent.ItemType<CaeliteBar>(), ModContent.ItemType<CaeliteCore>(), ItemID.LesserManaPotion },
                        "Use a [i:" + ModContent.ItemType<FortressBossSummon>() + "]" + " at the altar in the sky fortress", null);

                    bossChecklist.Call("AddBoss", .7f, ModContent.NPCType<PolarBear>(), this, "Polar Exterminator", (Func<bool>)(() => DownedBossSystem.downedBear), null,
                        new List<int> { ModContent.ItemType<PolarTrophy>(), ModContent.ItemType<PolarMask>() },
                        new List<int> { ModContent.ItemType<TundraBossBag>(), ModContent.ItemType<PenguinGenerator>(), ModContent.ItemType<PenguinClub>(), ModContent.ItemType<PenguinLauncher>(), ModContent.ItemType<PenguinWhistle>(), ItemID.Penguin, ItemID.LesserHealingPotion },
                        "Hibernates in the underground tundra. After defeating it it will respawn the next day.", null);

                    bossChecklist.Call("AddBoss", 11.4f, ModContent.NPCType<Imperious>(), this, "Imperious", (Func<bool>)(() => DownedBossSystem.downedBlade), null,
                        new List<int> { ModContent.ItemType<BladeBossTrophy>(), ModContent.ItemType<MusicBoxBladeOfAGod>() },
                        new List<int> { ModContent.ItemType<BladeBossBag>(), ModContent.ItemType<ImperiousSheath>(), ModContent.ItemType<ImperiousTheIV>(), ModContent.ItemType<Discipline>(), ModContent.ItemType<Arsenal>(), ModContent.ItemType<BladedArrowShaft>(), ModContent.ItemType<SwordStormStaff>(), ModContent.ItemType<SwordMinionStaff>(), ModContent.ItemType<Imperium>(), ModContent.ItemType<Swordquake>(), ModContent.ItemType<SwordsmanBadge>(), ItemID.GreaterHealingPotion },
                        "Use the [i:" + ModContent.ItemType<BladeBossSummon>() + "], and accept its challenge", null);

                    bossChecklist.Call("AddBoss", 5.7f, ModContent.NPCType<CloakedDarkBoss>(), this, "Noehtnap", (Func<bool>)(() => DownedBossSystem.downedNoehtnap), ModContent.ItemType<RitualInterupter>(),
                        new List<int> { ModContent.ItemType<MusicBoxTheGodsBleed>() },
                        new List<int> { ModContent.ItemType<NoehtnapBag>(), ModContent.ItemType<Doppleganger>(), ModContent.ItemType<Etims>(), ItemID.HealingPotion },
                        "Just use the [i:" + ModContent.ItemType<RitualInterupter>() + "] mortal!", null, "QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/CloakedDarkBoss_Checklist");

                    bossChecklist.Call("AddEvent", 11.001f,
                        new List<int> { ModContent.NPCType<Utah>(), ModContent.NPCType<Triceratank>(), ModContent.NPCType<AntiAir>(), ModContent.NPCType<Velocichopper>(), ModContent.NPCType<TheGreatTyrannosaurus>() },
                        this, "Dino Militia", (Func<bool>)(() => DownedBossSystem.downedDinos), ModContent.ItemType<DinoEgg>(),
                        ModContent.ItemType<MusicBoxOldDinosNewGuns>(),
                        new List<int> { ModContent.ItemType<DinoFlail>(), ModContent.ItemType<DinoVulcan>(), ModContent.ItemType<TheTyrantsExtinctionGun>(), ModContent.ItemType<Tricerashield>(), ModContent.ItemType<DinoTooth>() },
                        "Use a [i:" + ModContent.ItemType<DinoEgg>() + "] and they'll come to drive you extinct!", null, "QwertyMod/Content/NPCs/DinoMilitia/TheGreatTyrannosaurus_Bestiary", "QwertyMod/Content/Items/Consumable/BossSummons/DinoEgg");


                    bossChecklist.Call("AddToBossLoot", "Terraria", "DukeFishron", new List<int> { ModContent.ItemType<Cyclone>(), ModContent.ItemType<Whirlpool>(), ModContent.ItemType<BubbleBrewerBaton>() });
                    
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
                    int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(playerID), (int)summonAt.X, (int)summonAt.Y, ModContent.NPCType<FortressBoss>());
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
                    int npcID2 = NPC.NewNPC(NPC.GetBossSpawnSource(playerID2), (int)summonAt2.X, (int)summonAt2.Y, ModContent.NPCType<Sleeping>());
                    break;
                case ModMessageType.SummonBattleship:
                    Vector2 summonAt3 = reader.ReadVector2();
                    int playerID3 = reader.ReadInt32();
                    int npcID3 = NPC.NewNPC(NPC.GetBossSpawnSource(playerID3), (int)summonAt3.X, (int)summonAt3.Y, ModContent.NPCType<InvaderBattleship>());
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