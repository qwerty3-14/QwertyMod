using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Rhuthinium
{
    public class RhuthiniumDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
            AmmoID.Sets.IsSpecialist[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Dart;
            Item.shoot = ModContent.ProjectileType<RhuthiniumDartP>();
            Item.shootSpeed = 3;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ModContent.ItemType<RhuthiniumBar>())
                .Register();
        }
    }
}
