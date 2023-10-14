using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Cobalt
{
    public class CobaltArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 5;
            Item.rare = ItemRarityID.Orange;
            Item.width = 14;
            Item.height = 32;

            Item.shootSpeed = 40;

            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<CobaltArrowP>();
            Item.ammo = 40;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.CobaltBar)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
