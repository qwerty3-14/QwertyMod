using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Palladium
{
    public class PalladiumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palladium Bullet");
            Tooltip.SetDefault("Right click to redirect in flight!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 1;
            Item.rare = 3;
            Item.width = 16;
            Item.height = 22;

            Item.shootSpeed = 16;

            Item.consumable = true;
            Item.shoot = ProjectileType<PalladiumBulletP>();
            Item.ammo = 97;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.PalladiumBar)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
