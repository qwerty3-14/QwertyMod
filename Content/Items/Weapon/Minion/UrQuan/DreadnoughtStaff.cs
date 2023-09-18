using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Minion.UrQuan
{
    public class DreadnoughtStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Rod of Command");
            //Tooltip.SetDefault("Used by Ur-Quan lords to issue commands");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.mana = 20;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 750000;
            Item.rare = ItemRarityID.LightRed;
            //Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<Dreadnought>();
            Item.DamageType = DamageClass.Summon;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<UrQuanB>(), 3600);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }
}
