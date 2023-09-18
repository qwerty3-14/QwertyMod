using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.MiniTank
{
    public class MiniTankStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Mini Tank Staff");
            //Tooltip.SetDefault("Summons a Mini Tank!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.mana = 20;
            Item.width = 66;
            Item.height = 66;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = 200000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<MiniTank>();
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<MiniTankB>(), 3600);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

    }
}
