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
            //DisplayName,SetDefault("Restless Sun");
            //Tooltip.SetDefault("Blessed by higher beings this weapon excels in large open areas!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 1;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.width = 24;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 12f;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 36 : 11;
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
                float spread = MathF.PI / 2;
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
                direction = (velocity.ToRotation() - MathF.PI / 6);
                Projectile.NewProjectile(source, position, QwertyMethods.PolarVector(speed, direction), type, damage, knockback, player.whoAmI);
                direction = (velocity.ToRotation() + MathF.PI / 6);
                Projectile.NewProjectile(source, position, QwertyMethods.PolarVector(speed, direction), type, damage, knockback, player.whoAmI);
            }
            else
            {
                return true;
            }
            return false;
        }
    }
    public class CaeliteMagicProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Restless Sun");
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


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10))
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            //target.immune[Projectile.owner] = 0;
        }

        private NPC target;
        private float maxDistance = 10000f;
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
            if (QwertyMethods.ClosestNPC(ref target, maxDistance, Projectile.Center, specialCondition: delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] == 0;}))
            {
                direction = QwertyMethods.SlowRotation(direction, (target.Center - Projectile.Center).ToRotation(), 10f);
            }
            Projectile.velocity = new Vector2(MathF.Cos(direction) * speed, MathF.Sin(direction) * speed);
            maxDistance = 10000f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>());
            Projectile.rotation += MathF.PI / 7.5f;
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