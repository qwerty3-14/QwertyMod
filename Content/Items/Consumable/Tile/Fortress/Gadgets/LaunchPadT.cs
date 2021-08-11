using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Tile.Fortress.Gadgets
{
    public class LaunchPadT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            //Main.tileNoAttach[Type] = true;
            //Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);
            TileObjectData.addTile(Type);
            DustType = DustType<FortressDust>();
            SoundType = 21;
            SoundStyle = 2;
            MinPick = 1;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Launchpad");
            AddMapEntry(new Color(162, 184, 185), name);
        }

        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i, j + 1].IsActive;
        }

        public override void FloorVisuals(Player player)
        {
            //Main.NewText("Hi");

            player.velocity.Y = -20;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ItemType<Launchpad>());
        }
    }
}