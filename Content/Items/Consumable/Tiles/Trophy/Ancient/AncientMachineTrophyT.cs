using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace QwertyMod.Content.Items.Consumable.Tiles.Trophy.Ancient
{
    public class AncientMachineTrophyT : ModTile
    {


        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Terraria.Tile tile = Main.tile[i, j];

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Consumable/Tiles/Trophy/Ancient/AncientMachineTrophyT_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 54, 52), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}