using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Gun
{
    public class GunArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gun Arrow");
            Tooltip.SetDefault("Shoots 2 bullets from your inventory!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 5;
            Item.rare = 4;
            Item.width = 14;
            Item.height = 32;

            Item.shootSpeed = 6;
            Item.useAmmo = 97;
            Item.consumable = true;
            Item.shoot = ProjectileType<GunArrowP>();
            Item.ammo = 40;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.HallowedBar)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
