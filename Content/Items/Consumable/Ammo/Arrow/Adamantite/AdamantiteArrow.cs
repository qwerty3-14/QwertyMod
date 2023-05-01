﻿using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Adamantite
{
    public class AdamantiteArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 30f;
            Item.value = 5;
            Item.rare = ItemRarityID.Orange;
            Item.width = 14;
            Item.height = 32;

            Item.shootSpeed = 40f;

            Item.consumable = true;
            Item.shoot = ProjectileType<AdamantiteArrowP>();
            Item.ammo = 40;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.AdamantiteBar)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
