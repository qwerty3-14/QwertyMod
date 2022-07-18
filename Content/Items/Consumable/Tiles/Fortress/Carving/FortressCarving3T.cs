using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Carving
{
    public class FortressCarving3T : ModTile
    {

        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.addTile(Type);
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            DustType = DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Fortress Carving");
            AddMapEntry(new Color(162, 184, 185));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ItemType<FortressCarving3>());
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}