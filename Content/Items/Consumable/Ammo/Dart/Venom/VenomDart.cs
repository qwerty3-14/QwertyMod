﻿using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Venom
{
    public class VenomDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
            AmmoID.Sets.IsSpecialist[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.width = 10;
            Item.height = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Dart;
            Item.shoot = ModContent.ProjectileType<VenomDartP>();
            Item.shootSpeed = 3;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.VialofVenom)
                .Register();
        }
    }
}
