using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Rhuthinium
{
    public class RhuthiniumArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.shootSpeed = 3f;
            Item.shoot = ProjectileType<RhuthiniumArrowP>();
            Item.damage = 12;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.ammo = AmmoID.Arrow;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Orange;
            Item.value = 5;
            Item.DamageType = DamageClass.Ranged;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemType<RhuthiniumBar>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
