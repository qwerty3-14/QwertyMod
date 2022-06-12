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

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Mythril
{
    public class MythrilArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Arrow");
            Tooltip.SetDefault("Pierces enemies and blocks, short range");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 5;
            Item.rare = 3;
            Item.width = 22;
            Item.height = 32;

            Item.shootSpeed = 15;

            Item.consumable = true;
            Item.shoot = ProjectileType<MythrilArrowP>();
            Item.ammo = 40;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.MythrilBar)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
