using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.RuneBuilder;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class CraftingRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune");
            Tooltip.SetDefault("");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            int f = (Main.LocalPlayer.GetModPlayer<ItemFrameCounter>().frameCounter / 4) % 80;
            spriteBatch.Draw
                (
                    RuneSprites.runeCycle[f],
                    position - new Vector2(2f, 2f),
                    null,
                    Color.White,
                    0,
                    origin,
                    scale * 2,
                    SpriteEffects.None,
                    0f
                );
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            int f = (Main.LocalPlayer.GetModPlayer<ItemFrameCounter>().frameCounter / 4) % 80;
            spriteBatch.Draw
                (
                    RuneSprites.runeCycle[f],
                    Item.position - Main.screenPosition + Vector2.One * 54,
                    null,
                    Color.White,
                    0,
                    new Vector2(27, 27),
                    scale * 2,
                    SpriteEffects.None,
                    0f
                );
            return false;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.maxStack = 999;
            Item.value = 100;
            Item.rare = 3;
            Item.value = 500;
            Item.rare = 9;
        }
    }
    public class ItemFrameCounter : ModPlayer
    {
        public int frameCounter = 0;
        public override void PreUpdate()
        {
            frameCounter++;
        }
    }
}