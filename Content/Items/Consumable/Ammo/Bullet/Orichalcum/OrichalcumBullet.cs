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

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Orichalcum
{
    public class OrichalcumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum Bullet");
            Tooltip.SetDefault("Upon hitting an enemy flies toward another enemy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 1;
            Item.rare = 3;
            Item.width = 12;
            Item.height = 18;

            Item.shootSpeed = 14;

            Item.consumable = true;
            Item.shoot = ProjectileType<OrichalcumBulletP>();
            Item.ammo = 97;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.OrichalcumBar)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
