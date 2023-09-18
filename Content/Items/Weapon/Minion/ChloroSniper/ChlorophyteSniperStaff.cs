using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Common;

namespace QwertyMod.Content.Items.Weapon.Minion.ChloroSniper
{
    class ChlorophyteSniperStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Chlorophyte Sniper Staff");
            //Tooltip.SetDefault("Summons a Chlorophyte Sniper to execute your foes! +\nBurst damage minion");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 160;
            Item.mana = 20;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 4, 52, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<ChlorophyteSniper>();
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<ChlorophyteSniperB>(), 3600);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
