using QwertyMod.Content.Items.Consumable.Tiles.Bars;
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
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemType<RhuthiniumBar>())
                .Register();
        }
    }
}
