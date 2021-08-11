using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Tile.Fortress.BuildingBlocks
{
    public class FortressPillarT : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);

            TileObjectData.newTile.Height = 1;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(162, 184, 185));

            DustType = DustType<FortressDust>();
            ItemDrop = ItemType<FortressPillar>();
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
        }

        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i + 1, j].IsActive || Main.tile[i - 1, j].IsActive || Main.tile[i, j + 1].IsActive || Main.tile[i, j - 1].IsActive; ;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            if (Main.tile[i, j + 1].type == TileType<FortressPillarT>())
            {
                if (Main.tile[i, j - 1].type == TileType<FortressPillarT>())
                {
                    Main.tile[i, j].frameY = 36;
                    Main.tile[i, j].frameX = 0;
                    //middle
                }
                else
                {
                    Main.tile[i, j].frameY = 18;
                    //top
                    if (Main.tile[i, j].frameX == 0)
                    {
                    }
                }
            }
            else if (Main.tile[i, j - 1].type == TileType<FortressPillarT>())
            {
                Main.tile[i, j].frameY = 54;
                Main.tile[i, j].frameX = 0;
                //bottom
            }
            else
            {
                Main.tile[i, j].frameY = 0;
                //solo
            }
        }
    }
}