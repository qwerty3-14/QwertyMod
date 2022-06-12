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

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Venom
{
    public class VenomDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venom Dart");
            Tooltip.SetDefault("Creates venom clouds when hitting enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.width = 10;
            Item.height = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Dart;
            Item.shoot = ProjectileType<VenomDartP>();
            Item.shootSpeed = 3;
            Item.knockBack = 1;
            Item.rare = 3;
            Item.consumable = true;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.VialofVenom)
                .Register();
        }
    }
}
