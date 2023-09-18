using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Furniture
{
    public class FortressSinkT : ModTile
    {
        public override void SetStaticDefaults()
        {
            //Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            //Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                18
            };
            TileObjectData.addTile(Type);
            //AddMapEntry(new Color(162, 184, 185));
            DustType = DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks;
            // disableSmartCursor = true;
            AdjTiles = new int[] { TileID.Sinks };
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}