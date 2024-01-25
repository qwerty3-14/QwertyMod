using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Furniture
{
    public class FortressClockT : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Clock[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new[]
            {
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(162, 184, 185));
            DustType = ModContent.DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks;
            AdjTiles = new int[] { TileID.GrandfatherClocks };
        }

        public override bool RightClick(int x, int y)
        {
            {
                Main.NewText("Get a watch kid!!", 255, 240, 20);
            }
            return true;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}