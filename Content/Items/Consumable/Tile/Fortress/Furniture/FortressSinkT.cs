using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData; using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Dusts;

namespace QwertyMod.Content.Items.Consumable.Tile.Fortress.Furniture
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
            //AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Fortress Sink");
            AddMapEntry(new Color(162, 184, 185), name);
            DustType = DustType<FortressDust>();
            // disableSmartCursor = true;
            AdjTiles = new int[] { TileID.Sinks };
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ItemType<FortressSink>());
        }
    }
}