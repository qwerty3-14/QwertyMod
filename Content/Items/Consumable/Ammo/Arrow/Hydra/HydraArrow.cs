﻿using QwertyMod.Content.Items.MiscMaterials;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Hydra
{
    public class HydraArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 5;
            Item.rare = ItemRarityID.Pink;
            Item.width = 14;
            Item.height = 32;

            Item.shootSpeed = 6;

            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<HydraArrowP>();
            Item.ammo = 40;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(111).AddIngredient(ModContent.ItemType<HydraScale>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
