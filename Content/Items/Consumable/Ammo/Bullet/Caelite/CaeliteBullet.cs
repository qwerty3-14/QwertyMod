using QwertyMod.Content.Items.Consumable.Tiles.Bars;
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
namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Caelite
{
    public class CaeliteBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Bullet");
            Tooltip.SetDefault("Inflicts Caelite Wrath reducing damage that enemies deal by 20%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 1;
            Item.rare = 3;
            Item.width = 12;
            Item.height = 18;

            Item.shootSpeed = 32;

            Item.consumable = true;
            Item.shoot = ProjectileType<CaeliteBulletP>();
            Item.ammo = 97;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemType<CaeliteBar>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
