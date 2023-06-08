using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks
{
    public class FortressBrickT : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            //Main.tileSpelunker[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlendAll[Type] = false;

            DustType = DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks;
            MinPick = 50;
            AddMapEntry(new Color(162, 184, 185));
            MineResist = 1;
            //ItemDrop = ItemType<FortressBrick>();
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