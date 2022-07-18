using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossSummon;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Common.Fortress
{
    public class FortressAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);
            TileObjectData.addTile(Type);
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            DustType = DustType<CaeliteDust>();
            HitSound = QwertyMod.FortressBlocks;
            MinPick = 10000;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Altar");
            AddMapEntry(new Color(162, 184, 185), name);
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            //QwertyMethods.ServerClientCheck();
            if (!NPC.AnyNPCs(NPCType<FortressBoss>()))
            {
                for (int b = 0; b < 58; b++) // this searches every invintory slot
                {
                    if (player.inventory[b].type == ItemType<FortressBossSummon>() && player.inventory[b].stack > 0) //this checks if the slot has the valid item
                    {
                        if (Main.netMode == 0)
                        {
                            //QwertyWorld.FortressBossQuotes();
                            int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), i * 16, j * 16 - 200, NPCType<FortressBoss>());
                        }
                        else
                        {
                            ModPacket packet = Mod.GetPacket();
                            packet.Write((byte)ModMessageType.DivineCall);
                            packet.WriteVector2(new Vector2(i * 16 + 400, j * 16));
                            packet.Write(player.whoAmI);
                            packet.Send();
                        }

                        player.inventory[b].stack--;
                        break;
                    }
                }
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemType<FortressBossSummon>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}