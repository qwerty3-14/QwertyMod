using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Ranged.Bow.B4Bow
{
    public class B4Bow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possesing Bow");
            Tooltip.SetDefault("Arrows fired from this will chase your enemies!");
        }
        public override void SetDefaults()
        {
            Item.damage = 233;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 4;
            Item.useAnimation = 12;
            Item.reuseDelay = 20;
            Item.useStyle = 5;
            Item.knockBack = 2f;
            Item.value = 750000;
            Item.rare = 10;
            Item.UseSound = SoundID.Item5;

            Item.width = 32;
            Item.height = 62;

            Item.shoot = 40;
            Item.useAmmo = 40;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

        public override bool ConsumeAmmo(Player player)
        {
            return !(player.itemAnimation < Item.useAnimation - 2);
        }

        public Projectile arrow;

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 trueSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15));
            arrow = Main.projectile[Projectile.NewProjectile(source, position.X, position.Y, trueSpeed.X, trueSpeed.Y, type, damage, knockback, player.whoAmI)];
            arrow.GetGlobalProjectile<arrowHoming>().B4HomingArrow = true;
            return false;
        }
    }

    public class arrowHoming : GlobalProjectile
    {
        public bool B4HomingArrow;

        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (B4HomingArrow)
            {
                projectile.netUpdate = true;
                NPC prey = null;
                if (QwertyMethods.ClosestNPC(ref prey, 1000, projectile.Center))
                {
                    float direction = projectile.velocity.ToRotation();
                    direction.SlowRotation((prey.Center - projectile.Center).ToRotation(), MathHelper.ToRadians(4));
                    projectile.velocity = QwertyMethods.PolarVector(projectile.velocity.Length(), direction);
                }
            }
        }
    }
}