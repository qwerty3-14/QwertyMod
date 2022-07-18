using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Weapon.Minion.GlassSpike
{
    class GlassSpikeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Spike Staff");
            Tooltip.SetDefault("Summon spikes that rest on the ground, damaging enemies that step on them \nWill reposition if you walk away");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }


        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.mana = 20;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<GlassSpike>();
            Item.DamageType = DamageClass.Summon;
            Item.buffType = BuffType<GlassSpikeB>();
            Item.buffTime = 3600;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Glass, 20)
                .AddIngredient(ItemID.SilverBar, 12)
                .AddTile(TileID.GlassKiln)
                .Register();
            CreateRecipe().AddIngredient(ItemID.Glass, 20)
                .AddIngredient(ItemID.TungstenBar, 12)
                .AddTile(TileID.GlassKiln)
                .Register();
        }

    }
}
