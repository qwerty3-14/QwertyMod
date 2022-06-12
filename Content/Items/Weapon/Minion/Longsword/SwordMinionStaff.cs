using Microsoft.Xna.Framework;
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

namespace QwertyMod.Content.Items.Weapon.Minion.Longsword
{
    public class SwordMinionStaff : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Longsword Staff");
            Tooltip.SetDefault("Who needs a horde of minions when you have a giant longsword?");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.mana = 4;
            Item.damage = 19;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.knockBack = 2f;
            Item.useAnimation = Item.useTime = 8;
            Item.shootSpeed = 24f;
            Item.width = Item.height = 44;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileType<SwordMinion>();
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.buffType = BuffType<SwordMinionBuff>();
            Item.buffTime = 3600;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float minionCount = 0;
            //Main.NewText(minionCount + ", " + player.maxMinions);
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.owner == player.whoAmI)
                {
                    minionCount += projectile.minionSlots;
                }
            }
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.type == type && projectile.owner == player.whoAmI)
                {
                    if (player.maxMinions - minionCount >= 1)
                    {
                        projectile.minionSlots++;
                    }
                    return false;
                }
            }
            player.AddBuff(BuffType<SwordMinionBuff>(), 3600); //Idk why but the item.buffType didn't work for this
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

    }
}
