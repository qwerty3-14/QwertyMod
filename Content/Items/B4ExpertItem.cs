using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items
{
    public class B4ExpertItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Rod");
            Tooltip.SetDefault("Moves you toward your cursor when used");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = 10;
            Item.value = 500;
            Item.rare = 9;
            Item.expert = true;
            Item.useStyle = 5;
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.autoReuse = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                float direction = (Main.MouseWorld - player.Center).ToRotation();
                float distance = (Main.MouseWorld - player.Center).Length();
                player.armorEffectDrawShadow = true;
                player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;

                player.velocity = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction)) * distance / 10;

                int dust = Dust.NewDust(player.position, player.width, player.height, DustType<B4PDust>(), 0, 0);
                player.noFallDmg = true;
            }

            return true;
        }
    }
}
