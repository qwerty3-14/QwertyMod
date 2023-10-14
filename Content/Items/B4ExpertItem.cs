using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

using Terraria.ID;

namespace QwertyMod.Content.Items
{
    public class B4ExpertItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
            Item.value = 500;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.autoReuse = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                //QwertyMethods.ServerClientCheck("UseItem");
                float direction = (Main.MouseWorld - player.Center).ToRotation();
                float distance = (Main.MouseWorld - player.Center).Length();
                player.armorEffectDrawShadow = true;
                player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                player.velocity = new Vector2(MathF.Cos(direction), MathF.Sin(direction)) * distance / 10;
                NetMessage.SendData(MessageID.PlayerControls, number: player.whoAmI);
            }
            player.noFallDmg = true;
            int dust = Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<B4PDust>(), 0, 0);

            return true;
        }
    }
}
