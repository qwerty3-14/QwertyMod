using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Weapon.Sentry.ShroomiteTurret
{
    public class ShroomiteTurretStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Turret Staff");
            Tooltip.SetDefault("Fire bullets from your inventory.\n50% chance not to consume ammo.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.rare = 8;
            Item.useStyle = 1;
            Item.value = Item.sellPrice(gold: 1);
            Item.width = Item.height = 28;
            Item.sentry = true;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.useTime = Item.useAnimation = 25;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ShroomiteTurretBase>();
            Item.UseSound = SoundID.Item44;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.ShroomiteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }

    public class ShroomiteTurretBase : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 18;
            Projectile.tileCollide = true;
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private float gunRotation = 0;
        private float aimRotation = 0;
        private bool runOnce = true;
        private NPC target;
        private Vector2 gunRotationOrigionOffset = Vector2.UnitY * -5;
        private int shotCooldown = 8;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.UpdateMaxTurrets();
            if (runOnce)
            {
                if (player.direction == -1)
                {
                    gunRotation = (float)Math.PI;
                }
                runOnce = false;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, player.MinionAttackTargetNPC))
            {
                aimRotation = (target.Center - Projectile.Center).ToRotation();
                shotCooldown++;
                if (shotCooldown >= 12)
                {
                    if (QwertyMethods.AngularDifference(aimRotation, gunRotation) < (float)Math.PI / 8)
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                aimRotation = player.direction == 1 ? 0 : (float)Math.PI;
            }
            gunRotation = QwertyMethods.SlowRotation(gunRotation, aimRotation, 5);
            Projectile.velocity.Y = 10;
        }

        public int bullet = 1;
        public bool canShoot = true;
        public float speedB = 14f;

        private void Shoot()
        {
            int weaponDamage = Projectile.damage;
            float weaponKnockback = Projectile.knockBack;
            if (Projectile.UseAmmo(AmmoID.Bullet, ref bullet, ref speedB, ref weaponDamage, ref weaponKnockback, Main.rand.Next(2) == 0))
            {
                Projectile bul = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center + QwertyMethods.PolarVector(29, gunRotation), QwertyMethods.PolarVector(10, gunRotation), bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
                
                shotCooldown = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/ShroomiteTurret/ShroomiteTurretGun").Value;
            Main.EntitySpriteDraw(texture, Projectile.Center + gunRotationOrigionOffset - Main.screenPosition, null, lightColor, gunRotation, new Vector2(13, 11), Projectile.scale, SpriteEffects.None, 0);
            return true;
        }
    }
}