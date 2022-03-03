using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Magic.RestlessSun
{
    public class CaeliteMagicWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Restless Sun");
            Tooltip.SetDefault("Blessed by higher beings this weapon excels in large open areas!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 1;
            Item.value = 50000;
            Item.rare = 3;
            Item.width = 24;
            Item.height = 28;
            Item.useStyle = 5;
            Item.shootSpeed = 12f;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.mana = 11;
            Item.shoot = ProjectileType<CaeliteMagicProjectile>();
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item21;
            Item.autoReuse = true;
        }

        private float direction;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int rng = Main.rand.Next(100);
            if (rng == 0)
            {
                int numberOfProjectiles = 10;
                float spread = (float)Math.PI / 2;
                float speed = velocity.Length();
                for (int p = 0; p < numberOfProjectiles; p++)
                {
                    direction = (velocity.ToRotation() - (spread / 2)) + (spread * ((float)p / (float)numberOfProjectiles));
                    Projectile.NewProjectile(source, position, QwertyMethods.PolarVector(speed, direction), type, damage, knockback, player.whoAmI);
                }
            }
            else if (rng < 10)
            {
                float speed = velocity.Length();
                direction = (velocity.ToRotation() - (float)Math.PI / 6);
                Projectile.NewProjectile(source, position, QwertyMethods.PolarVector(speed, direction), type, damage, knockback, player.whoAmI);
                direction = (velocity.ToRotation() + (float)Math.PI / 6);
                Projectile.NewProjectile(source, position, QwertyMethods.PolarVector(speed, direction), type, damage, knockback, player.whoAmI);
            }
            else
            {
                return true;
            }
            return false;
        }

        

        public class CaeliteMagicProjectile : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Restless Sun");
            }

            public override void SetDefaults()
            {
                Projectile.aiStyle = 0;
                //aiType = ProjectileID.Bullet;
                Projectile.width = 44;
                Projectile.height = 44;
                Projectile.friendly = true;
                Projectile.penetrate = 3;
                Projectile.DamageType = DamageClass.Magic;
                Projectile.timeLeft = 180;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 30;
                Projectile.tileCollide = true;
                Projectile.light = 1f;
            }
            

            public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            {
                if (Main.rand.Next(10) == 0)
                {
                    target.AddBuff(BuffType<PowerDown>(), 120);
                }
                Projectile.localNPCImmunity[target.whoAmI] = -1;
                //target.immune[Projectile.owner] = 0;
            }

            private NPC target;
            private NPC possibleTarget;
            private bool foundTarget;
            private float maxDistance = 10000f;
            private float distance;
            private int timer;
            private float speed = 24;
            private bool runOnce = true;
            private float direction;

            public override void AI()
            {
                if (runOnce)
                {
                    direction = Projectile.velocity.ToRotation();

                    runOnce = false;
                }
                Player player = Main.player[Projectile.owner];
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    if (!Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, possibleTarget.position, possibleTarget.Size))
                    {
                        Projectile.localNPCImmunity[k] = 0;
                    }
                }
                if (QwertyMethods.ClosestNPC(ref target, maxDistance, Projectile.Center))
                {
                    direction = QwertyMethods.SlowRotation(direction, (target.Center - Projectile.Center).ToRotation(), 10f);
                }
                Projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
                foundTarget = false;
                maxDistance = 10000f;
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>());
                Projectile.rotation += (float)Math.PI / 7.5f;
            }

            public override void Kill(int timeLeft)
            {
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
                }
            }
        }
    }
}