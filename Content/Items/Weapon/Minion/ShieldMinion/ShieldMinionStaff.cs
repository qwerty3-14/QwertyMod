using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Weapon.Minion.ShieldMinion
{
    public class ShieldMinionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Slams intruders that get too close to you! +\nBurst damage minion");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.mana = 20;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.rare = ItemRarityID.Orange;
            Item.value = 120000;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<ShieldMinion>();
            Item.DamageType = DamageClass.Summon;
            Item.buffType = BuffType<ShieldMinionB>();
            Item.buffTime = 3600;
        }
        
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<Etims>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
