using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.AncientMinion
{
    public class AncientMinionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Minion Staff");
            //Tooltip.SetDefault("Summons an ancient minion to fight for you!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }


        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.mana = 20;
            Item.width = 72;
            Item.height = 72;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 150000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<AncientMinionFreindly>();
            Item.DamageType = DamageClass.Summon;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/AncientMinion/AncientMinionStaff_Glow").Value;
            }

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<AncientMinionB>(), 3600);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

    }
}
