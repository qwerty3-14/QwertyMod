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
                Main.QueueMainThreadAction(() =>
                {
                    RuneSprites.BuildRunes();
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
            }
        }
    }
    internal enum ModMessageType : byte
    {

        DivineCall,
        UpdateLocalCursor
    }
}