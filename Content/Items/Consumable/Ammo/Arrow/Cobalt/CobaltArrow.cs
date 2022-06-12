using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Cobalt
{
    public class CobaltArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cobalt Arrow");
            Tooltip.SetDefault("Remains stationarry until you right click, which sends it flying at your cursor");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 5;
            Item.rare = 3;
            Item.width = 14;
            Item.height = 32;

            Item.shootSpeed = 40;

            Item.consumable = true;
            Item.shoot = ProjectileType<CobaltArrowP>();
            Item.ammo = 40;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.CobaltBar)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
