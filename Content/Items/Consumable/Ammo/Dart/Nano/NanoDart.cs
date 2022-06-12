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

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Nano
{
    public class NanoDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nano Dart");
            Tooltip.SetDefault("Breaks into a cluster of homing nanites");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.width = 10;
            Item.height = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Dart;
            Item.shoot = ProjectileType<NanoDartP>();
            Item.shootSpeed = 3;
            Item.knockBack = 1;
            Item.rare = 3;
            Item.consumable = true;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.Nanites)
                .Register();
        }
    }
}
