using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using QwertyMod.Common.RuneBuilder;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Weapon.Morphs.HydraBarrage;
using QwertyMod.Content.Items.Weapon.Magic.HydraBeam;
using QwertyMod.Content.Items.Weapon.Ranged.Gun;
using QwertyMod.Content.Items.Weapon.Minion.HydraHead;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Hydra;
using QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrent;
using QwertyMod.Content.Items.Tool.Mining;
using QwertyMod.Content.Items.Weapon.Magic.HydraMissile;
using QwertyMod.Content.Items.Weapon.Melee.Sword.AncientBlade;
using QwertyMod.Content.Items.Weapon.Minion.AncientMinion;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.Ancient;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.AncientThrow;
using QwertyMod.Content.Items.Weapon.Magic.AncientMissile;
using QwertyMod.Content.Items.Weapon.Morphs.AncientNuke;
using QwertyMod.Content.Items.Weapon.Magic.AncientWave;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient;
using Microsoft.Xna.Framework.Input;
using QwertyMod.Content.Items.Weapon.Magic.Swordpocalypse;
using QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV;
using QwertyMod.Content.Items.Weapon.Whip.Discipline;
using QwertyMod.Content.Items.Weapon.Minion.Longsword;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.Arsenal;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Imperium;
using QwertyMod.Content.Items.Weapon.Morphs.Swordquake;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using QwertyMod.Common.Playerlayers;
using QwertyMod.Content.Items.Equipment.Armor.Hydra;
using QwertyMod.Content.Items.Equipment.Armor.Shaman;
using QwertyMod.Content.Items.Equipment.Armor.Rhuthinium;
using QwertyMod.Content.Items.Equipment.Armor.Lune;
using QwertyMod.Content.Items.Equipment.Armor.Caelite;
using QwertyMod.Content.Items.Equipment.Armor.Gale;
using QwertyMod.Content.Items.Equipment.Armor.Vitallum;
using QwertyMod.Content;

namespace QwertyMod
{
	public class QwertyMod : Mod
	{
        public static QwertyMod Instance;
        private static Vector2[] LocalCursor = new Vector2[Main.player.Length];

        public static Deck<int> AMLoot;
        public static Deck<int> ImperiousLoot;
        public static Deck<int> HydraLoot;

        public const string HydraHead1 = "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead1";
        public const string HydraHead2 = "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead1";
        public const string HydraHead3 = "QwertyMod/Content/NPCs/Bosses/Hydra/MapHead1";
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
        public override void Load()
        {
            AMLoot = new Deck<int>();
            AMLoot.Add(ItemType<AncientBlade>());
            AMLoot.Add(ItemType<AncientSniper>());
            AMLoot.Add(ItemType<AncientWave>());
            AMLoot.Add(ItemType<AncientThrow>());
            AMLoot.Add(ItemType<AncientMinionStaff>());
            AMLoot.Add(ItemType<AncientMissileStaff>());
            AMLoot.Add(ItemType<AncientLongbow>());
            AMLoot.Add(ItemType<AncientNuke>());

            ImperiousLoot = new Deck<int>();
            ImperiousLoot.Add(ItemType<SwordStormStaff>());
            ImperiousLoot.Add(ItemType<ImperiousTheIV>());
            ImperiousLoot.Add(ItemType<Discipline>());
            ImperiousLoot.Add(ItemType<SwordMinionStaff>());
            ImperiousLoot.Add(ItemType<Arsenal>());
            ImperiousLoot.Add(ItemType<SwordsmanBadge>());
            ImperiousLoot.Add(ItemType<Imperium>());
            ImperiousLoot.Add(ItemType<Swordquake>());

            HydraLoot = new Deck<int>();
            HydraLoot.Add(ItemType<HydraBarrage>());
            HydraLoot.Add(ItemType<HydraBeam>());
            HydraLoot.Add(ItemType<HydraCannon>());
            HydraLoot.Add(ItemType<HydraHeadStaff>());
            HydraLoot.Add(ItemType<HydraJavelin>());
            HydraLoot.Add(ItemType<Hydrent>());
            HydraLoot.Add(ItemType<Hydrill>());
            HydraLoot.Add(ItemType<HydraMissileStaff>());

            Instance = this;
            YetAnotherSpecialAbility = KeybindLoader.RegisterKeybind(this, "Yet Another Special Ability Key", Keys.R);

            AddBossHeadTexture(HydraHead1);
            AddBossHeadTexture(HydraHead2);
            AddBossHeadTexture(HydraHead3);

            if (!Main.dedServ)
            {
                hydraLegMale = AddEquipTexture(GetModItem(ItemType<HydraLeggings>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_Legs");
                hydraLegFemale = AddEquipTexture(GetModItem(ItemType<HydraLeggings>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_FemaleLegs");
                shamaLegMale = AddEquipTexture(GetModItem(ItemType<ShamanLegs>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Shaman/ShamanLegs_Legs");
                shamanLegFemale = AddEquipTexture(GetModItem(ItemType<ShamanLegs>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Shaman/ShamanLegs_FemaleLegs");
                LuneLegMale = AddEquipTexture(GetModItem(ItemType<LuneLeggings>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Lune/LuneLeggings_Legs");
                LuneLegFemale = AddEquipTexture(GetModItem(ItemType<LuneLeggings>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Lune/LuneLeggings_FemaleLegs");
                RhuthiniumLegMale = AddEquipTexture(GetModItem(ItemType<RhuthiniumGreaves>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Rhuthinium/RhuthiniumGreaves_Legs");
                RhuthiniumLegFemale = AddEquipTexture(GetModItem(ItemType<RhuthiniumGreaves>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Rhuthinium/RhuthiniumGreaves_FemaleLegs");
                CaeliteLegMale = AddEquipTexture(GetModItem(ItemType<CaeliteGreaves>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Caelite/CaeliteGreaves_Legs");
                CaeliteLegFemale = AddEquipTexture(GetModItem(ItemType<CaeliteGreaves>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Caelite/CaeliteGreaves_FemaleLegs");
                GaleLegMale = AddEquipTexture(GetModItem(ItemType<GaleSwiftRobes>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Gale/GaleSwiftRobes_Legs");
                GaleLegFemale = AddEquipTexture(GetModItem(ItemType<GaleSwiftRobes>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Gale/GaleSwiftRobes_FemaleLegs");
                VitLegMale = AddEquipTexture(GetModItem(ItemType<VitallumJeans>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_Legs");
                VitLegFemale = AddEquipTexture(GetModItem(ItemType<VitallumJeans>()), EquipType.Legs, "QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_FemaleLegs");
                Main.QueueMainThreadAction(() =>
                {
                    RuneSprites.BuildRunes();
                });

            }

        }
        public override void PostSetupContent()
        {
            if (!Main.dedServ)
            {
                Main.QueueMainThreadAction(() =>
                {
                    OnHeadDraw.RegisterHeads();
                    OnLegDraw.RegisterLegs();
                    OnBodyDraw.ReigsterBodies();
                });
            }
        }
        public static Vector2 GetLocalCursor(int id)
        {
            LocalCursor[id] = Main.MouseWorld;

            if (Main.netMode == 1)
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
                    int npcID = NPC.NewNPC((int)summonAt.X, (int)summonAt.Y, NPCType<FortressBoss>());
                    break;
                case ModMessageType.StartDinoEvent:
                    DinoEvent.EventActive = true;
                    DinoEvent.DinoKillCount = 0;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    }

                    break;
            }
        }
    }
    internal enum ModMessageType : byte
    {

        DivineCall,
        UpdateLocalCursor,
        StartDinoEvent
    }
}