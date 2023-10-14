using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

using QwertyMod.Content.Items.MiscMaterials;

namespace QwertyMod.Content.Items.Weapon.Minion.HigherPriest
{
    class HigherPriestStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 175;
            Item.mana = 20;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<HigherPriestMinion>();
            Item.DamageType = DamageClass.Summon;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<HigherPriestMinionB>(), 3600);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<Priest.PriestStaff>())
            .AddIngredient(ModContent.ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
