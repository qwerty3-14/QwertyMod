using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Gun
{
    public class GunArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 5;
            Item.rare = ItemRarityID.LightRed;
            Item.width = 14;
            Item.height = 32;

            Item.shootSpeed = 6;
            Item.useAmmo = AmmoID.Bullet;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<GunArrowP>();
            Item.ammo = 40;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.HallowedBar)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
