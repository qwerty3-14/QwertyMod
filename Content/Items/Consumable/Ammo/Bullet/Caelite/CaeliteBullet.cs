using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Caelite
{
    public class CaeliteBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 1;
            Item.rare = ItemRarityID.Orange;
            Item.width = 12;
            Item.height = 18;

            Item.shootSpeed = 16;

            Item.consumable = true;
            Item.shoot = ProjectileType<CaeliteBulletP>();
            Item.ammo = 97;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemType<CaeliteBar>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
