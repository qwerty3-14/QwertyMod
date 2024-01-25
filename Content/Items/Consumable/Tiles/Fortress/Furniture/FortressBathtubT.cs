using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Furniture
{
    public class FortressBathtubT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            //TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); //this style already takes care of direction for us
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(162, 184, 185));
            DustType = ModContent.DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks;

        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }
    }
}