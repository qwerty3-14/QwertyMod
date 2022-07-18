using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.AncientMissile
{
    public class AncientMissileStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Missile Staff");
            Tooltip.SetDefault("Fires explosive Ancient Missiles!");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.DamageType = DamageClass.Magic;

            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = 5;
            Item.knockBack = 2;
            Item.value = 150000;
            Item.rare = 3;
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.width = 72;
            Item.height = 72;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/AncientMissile/AncientMissileStaff_Glow").Value;
            }
            Item.mana = ModLoader.HasMod("TRAEProject") ? 14 : 7;
            Item.shoot = ProjectileType<AncientMissileP>();
            Item.shootSpeed = 9;
            Item.noMelee = true;
            //Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("Items/AncientItems/AncientWave_Glow");
        }

        public override void UseAnimation(Player player)
        {
            if (player.statMana > Item.mana)
            {
                SoundEngine.PlaySound(SoundID.MaxMana, player.position);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 70f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return true;
        }

    }

    public class AncientMissileP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Missile");

            Main.projFrames[Projectile.type] = 2;
        }


        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 20;
            Projectile.height = Projectile.width;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public int dustTimer;
        private float missileAcceleration = .5f;
        private float topSpeed = 10f;
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
                if (QwertyMethods.ClosestNPC(ref target, 10000f, Projectile.Center))
                {
                    Projectile.velocity += QwertyMethods.PolarVector(missileAcceleration, (target.Center - Projectile.Center).ToRotation());
                    if (Projectile.velocity.Length() > topSpeed)
                    {
                        Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * topSpeed;
                    }
                }
            }
            Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(26, Projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(Main.rand.Next(-6, 6), Projectile.rotation), DustType<AncientGlow>());
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<AncientBlastFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<AncientBlastFriendly>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            return true;
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, Projectile.frame * texture.Height / 2, texture.Width, texture.Height / 2), drawColor, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/AncientMissile/AncientMissileP_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, Projectile.frame * texture.Height / 2, texture.Width, texture.Height / 2), Color.White, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }

    public class AncientBlastFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Blast");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;

        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.FriendlyFire();

            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

            for (int i = 0; i < 400; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<AncientGlow>(), QwertyMethods.PolarVector(Main.rand.Next(2, 20), theta));
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}