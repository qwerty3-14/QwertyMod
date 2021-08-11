using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Magic.HydraMissile
{
    public class HydraMissileStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Missile Rod");
            Tooltip.SetDefault("Fires a Hydra head that explodes and splits into more hydra heads which explodes and splits into more hydra heads!");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.mana = 10;
            Item.width = 100;
            Item.height = 100;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 250000;
            Item.rare = 5;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<HydraMissileBig>();
            Item.DamageType = DamageClass.Magic;
            Item.shootSpeed = 8;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26, 0);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 131f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    public class HydraMissileBig : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            //Projectile.usesLocalNPCImmunity = true;
        }

        private NPC target;
        private float speed = 15;
        private bool runOnce = true;
        private float direction;

        public override void AI()
        {
            if (runOnce)
            {
                direction = Projectile.velocity.ToRotation();
                Projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            Player player = Main.player[Projectile.owner];
            if (QwertyMethods.ClosestNPC(ref target, 10000, Projectile.Center))
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - Projectile.Center).ToRotation(), 3f);
            }
            Projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
            Projectile.rotation = direction + ((float)Math.PI / 2);
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = -velocityChange.Y;
            }
            direction = Projectile.velocity.ToRotation();
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Projectile.localNPCImmunity[target.whoAmI] = -1;
            //target.immune[Projectile.owner] = 0;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.alpha = 255;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int d = 0; d < 400; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
            }
            //Main.PlaySound(SoundID.Item62, Projectile.position);
            for (int g = 0; g < 2; g++)
            {
                float launchDirection = Main.rand.NextFloat() * (float)Math.PI * 2;
                Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), Projectile.Center, new Vector2((float)Math.Cos(launchDirection) * speed, (float)Math.Sin(launchDirection) * speed), ProjectileType<HydraMissileMedium>(), (int)(Projectile.damage * .6f), Projectile.knockBack * .6f, Projectile.owner);
            }
        }
    }

    public class HydraMissileMedium : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            //Projectile.usesLocalNPCImmunity = true;
        }

        private NPC target;
        private NPC possibleTarget;
        private bool foundTarget;
        private float maxDistance = 10000f;
        private float distance;
        private int timer;
        private float speed = 15;
        private bool runOnce = true;
        private float direction;

        public override void AI()
        {
            if (runOnce)
            {
                direction = Projectile.velocity.ToRotation();
                Projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            Player player = Main.player[Projectile.owner];
            if (QwertyMethods.ClosestNPC(ref target, 10000, Projectile.Center))
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - Projectile.Center).ToRotation(), 3f);
            }
            Projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
            Projectile.rotation = direction + ((float)Math.PI / 2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Projectile.localNPCImmunity[target.whoAmI] = -1;
            //target.immune[Projectile.owner] = 0;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = -velocityChange.Y;
            }
            direction = Projectile.velocity.ToRotation();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.alpha = 255;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int d = 0; d < 200; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
            }
            //Main.PlaySound(SoundID.Item62, Projectile.position);
            for (int g = 0; g < 2; g++)
            {
                float launchDirection = Main.rand.NextFloat() * (float)Math.PI * 2;
                Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), Projectile.Center, new Vector2((float)Math.Cos(launchDirection) * speed, (float)Math.Sin(launchDirection) * speed), ProjectileType<HydraMissileSmall>(), (int)(Projectile.damage * .6f), Projectile.knockBack * .6f, Projectile.owner);
            }
        }
    }

    public class HydraMissileSmall : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            //Projectile.usesLocalNPCImmunity = true;
        }

        private NPC target;
        private NPC possibleTarget;
        private bool foundTarget;
        private float maxDistance = 10000f;
        private float distance;
        private int timer;
        private float speed = 15;
        private bool runOnce = true;
        private float direction;

        public override void AI()
        {
            if (runOnce)
            {
                direction = Projectile.velocity.ToRotation();
                Projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            Player player = Main.player[Projectile.owner];
            if (QwertyMethods.ClosestNPC(ref target, 10000, Projectile.Center))
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - Projectile.Center).ToRotation(), 3f);
            }
            Projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
            Projectile.rotation = direction + ((float)Math.PI / 2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = -velocityChange.Y;
            }
            direction = Projectile.velocity.ToRotation();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.alpha = 255;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int d = 0; d < 100; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
            }
        }
    }
}