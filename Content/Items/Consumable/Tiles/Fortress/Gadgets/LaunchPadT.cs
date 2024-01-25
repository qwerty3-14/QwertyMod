using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class LaunchPadT : ModTile
    {
        public override void SetStaticDefaults()
        {

            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.addTile(Type);
            
            DustType = ModContent.DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks;
            MinPick = 1;

            //ModTranslation name = CreateMapEntryName();
            //name.SetDefault("Launchpad");
            AddMapEntry(new Color(162, 184, 185));
        }

        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i, j + 1].HasTile;
        }

        public override void FloorVisuals(Player player)
        {
            player.velocity.Y = -20;
        }
    }
}