using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Sentry.AntiAir
{
    public class AntiAirWrench : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air Sentry Wrench");
            Tooltip.SetDefault("Summons a stationary anti air sentry");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.mana = 20;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 10f;
            Item.value = 25000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<AntiAirSentry>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

    }

    public class AntiAirSentry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air Sentry");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.sentry = true;
            Projectile.width = 60;
            Projectile.height = 94;
            Projectile.hostile = false;    //tells the game if is hostile or not.
            Projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.knockBack = 10f;
            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            Projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
            Projectile.sentry = true; //tells the game that this is a sentry
        }

        public int frameType = 0;
        public int ReloadTime = 30;

        public int secondShot = 1;
        public float minimumHeight = 400;
        public float maxDistanceX = 600f;
        public float distanceX;
        public float distanceY;
        public NPC validTarget;
        public bool foundTarget;
        public int timer;
        public bool playAttackFrame;
        public int attackFrameCounter;
        public int attackFrameTime = 5;
        public int rocketDirection = 1;

        public override void AI()
        {
            Projectile.frame = 1;
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Projectile.velocity.Y = 5;

            timer++;
            if (QwertyMethods.ClosestNPC(ref validTarget, 2000, Projectile.Center, false, Main.player[Projectile.owner].MinionAttackTargetNPC, delegate (NPC possibleTarget) { Point origin = possibleTarget.Center.ToTileCoordinates(); Point point; return !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(12), new GenCondition[] { new Conditions.IsSolid() }), out point) && Math.Abs(possibleTarget.Center.X - Projectile.Center.X) < maxDistanceX && Projectile.Center.Y - possibleTarget.Center.Y > minimumHeight; }) && timer > ReloadTime)
            {
                playAttackFrame = true;
                if (Main.netMode != 1)
                    Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X + (16 * secondShot), Projectile.Center.Y - 30, 0, -5f, ProjectileType<SentryAntiAir>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, validTarget.Center.Y, rocketDirection);

                secondShot *= -1;

                timer = 0;
            }

            if (playAttackFrame)
            {
                attackFrameCounter++;
                if (secondShot == 1)
                {
                    Projectile.frame = 0;
                }
                else
                {
                    Projectile.frame = 2;
                }

                if (attackFrameCounter > attackFrameTime)
                {
                    playAttackFrame = false;
                }
            }
            else
            {
                attackFrameCounter = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }

    public class SentryAntiAir : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air Rocket");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.alpha = 255;
        }

        public int dustTimer;
        private float missileAcceleration = 2f;
        private float topSpeed = 14f;
        private int timer;

        private NPC target;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 30 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= 2)
                {
                    Projectile.frame = 0;
                }
            }
            timer++;
            if (timer > 30)
            {
                if (QwertyMethods.ClosestNPC(ref target, 2000, Projectile.Center, false, Main.player[Projectile.owner].MinionAttackTargetNPC, delegate (NPC possibleTarget) { Point origin = possibleTarget.Center.ToTileCoordinates(); Point point; return !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(12), new GenCondition[] { new Conditions.IsSolid() }), out point); }))
                {
                    Projectile.velocity += QwertyMethods.PolarVector(missileAcceleration, (target.Center - Projectile.Center).ToRotation());
                    if (Projectile.velocity.Length() > topSpeed)
                    {
                        Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * topSpeed;
                    }
                }
            }
            Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(26, Projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(Main.rand.Next(-6, 6), Projectile.rotation), 6);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }



        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= .6f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 2f;
                dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1f;
            }
        }
    }
}