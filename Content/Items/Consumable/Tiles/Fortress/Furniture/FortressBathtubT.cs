using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData; 
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Dusts;
using Terraria.DataStructures;

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
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Fortress Bathtub");
            AddMapEntry(new Color(162, 184, 185), name);
            DustType = DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks; 
           
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ItemType<FortressBathtub>());
        }
    }
}