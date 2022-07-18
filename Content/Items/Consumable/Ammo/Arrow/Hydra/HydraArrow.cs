using QwertyMod.Content.Items.MiscMaterials;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Hydra
{
    public class HydraArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Arrow");
            Tooltip.SetDefault("Splits into 3 arrows");
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 5;
            Item.rare = 5;
            Item.width = 14;
            Item.height = 32;

            Item.shootSpeed = 6;

            Item.consumable = true;
            Item.shoot = ProjectileType<HydraArrowP>();
            Item.ammo = 40;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(111).AddIngredient(ItemType<HydraScale>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
