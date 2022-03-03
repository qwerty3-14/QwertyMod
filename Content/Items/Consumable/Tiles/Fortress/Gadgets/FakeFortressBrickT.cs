using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.NPCs.Fortress;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class FakeFortressBrickT : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            //Main.tileSpelunker[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlendAll[Type] = true;

            DustType = DustType<FortressDust>();
            SoundType = 21;
            SoundStyle = 2;
            MinPick = 50;
            AddMapEntry(new Color(162, 184, 185));
            MineResist = 1;
            //drop = mod.ItemType("FortressBrick");
        }
        
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.netMode != 1 && !fail)
            {
                NPC youngTile = Main.npc[NPC.NewNPC(Wiring.GetNPCSource(i, j), i * 16 + 8, j * 16, NPCType<YoungTile>(), ai3: 1)];
                youngTile.velocity = QwertyMethods.PolarVector(2, (float)Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
            }
        }
        
        public override void HitWire(int i, int j)
        {
            WorldGen.KillTile(i, j);
            NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        public override bool CanExplode(int i, int j)
        {
            return true;
        }
    }
}