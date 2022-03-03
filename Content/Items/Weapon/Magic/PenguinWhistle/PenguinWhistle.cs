using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.PenguinWhistle
{
    public class PenguinWhistle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penguin Whistle");
            Tooltip.SetDefault("Calls flying penguins!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.mana = 10;
            Item.width = 100;
            Item.height = 100;
            Item.useTime = 39;
            Item.useAnimation = 39;
            //Item.reuseDelay = 60;
            Item.useStyle = 3;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 100000;
            Item.rare = 1;

            Item.autoReuse = true;
            Item.shoot = ProjectileType<PenguinFall>();
            Item.DamageType = DamageClass.Magic;
            Item.shootSpeed = 0;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberOfProjectiles = 5;
            for (int i = -2; i < numberOfProjectiles - 2; i++)
            {
                Vector2 positionb = new Vector2(Main.MouseWorld.X + 80 * i, position.Y - 600);
                Projectile penguin = Main.projectile[Projectile.NewProjectile(source, positionb, new Vector2(0, 10), type, damage, knockback, player.whoAmI)];
                if (positionb.X > player.Center.X)
                {
                    penguin.spriteDirection = -1;
                }
            }
            return false;
        }
    }
}
