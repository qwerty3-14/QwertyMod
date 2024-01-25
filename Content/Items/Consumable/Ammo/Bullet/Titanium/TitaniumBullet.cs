using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Titanium
{
    public class TitaniumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 1;
            Item.rare = ItemRarityID.Orange;
            Item.width = 18;
            Item.height = 18;

            Item.shootSpeed = 1;

            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<TitaniumBulletP>();
            Item.ammo = 97;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.TitaniumBar)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
