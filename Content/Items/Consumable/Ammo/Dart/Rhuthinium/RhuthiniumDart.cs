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

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Rhuthinium
{
    public class RhuthiniumDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Dart");
            Tooltip.SetDefault("Flies around you and fires 3 beams before breaking");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Dart;
            Item.shoot = ProjectileType<RhuthiniumDartP>();
            Item.shootSpeed = 3;
            Item.knockBack = 0;
            Item.rare = 3;
            Item.consumable = true;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.DirtBlock)
                .Register();
        }
    }
}
