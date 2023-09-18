using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent;

namespace QwertyMod.Content.Items.Weapon.Magic.Plasma
{
    public class ExplosivePierce : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Plasma Cannon");
            //Tooltip.SetDefault("Fires projectiles that pierce through enemies exploding every time they hit something!");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 175;
            Item.DamageType = DamageClass.Magic;

            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = 750000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
            Item.width = 66;
            Item.height = 28;
            Item.crit = 20;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 8 : 7;
            Item.shoot = ProjectileType<EPShot>();
            Item.shootSpeed = 27;
            Item.noMelee = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/Plasma/ExplosivePierce_Glowmask").Value;
            }
            Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -15;
            Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -2;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, -2);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += velocity.SafeNormalize(-Vector2.UnitY).RotatedBy(MathF.PI / 2) * -4 * player.direction + velocity.SafeNormalize(-Vector2.UnitY) * 40;
        }

    }

    public class EPShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 18;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.light = 1f;
        }

        public bool runOnce = true;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.frameCounter++;
            int scaleUpTime = 8;
            if(Projectile.timeLeft > 600 - scaleUpTime)
            {
                Projectile.scale = 1f * (((scaleUpTime - (Projectile.timeLeft - (600 - scaleUpTime))) / (float)scaleUpTime));
            }
            if (Projectile.frameCounter % 1 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            for (int i = 0; i < 1; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType<EPDust>(), 0f, 0f, 100, default(Color), 1f)];
            }
        }

        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<EPexplosion>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<EPexplosion>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * 26, texture.Width, 26), Color.White, Projectile.rotation,
                        new Vector2(44 - 13, 13), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }

    public class EPexplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("EP explosion");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.width = 100;
            Projectile.height = 100;

            SoundEngine.PlaySound(SoundID.Item91, Projectile.position);
            int dustCount = 80;
            int petals = 8;
            for(int i =0; i < dustCount; i++)
            {
                int portion = (dustCount / petals);
                int d = i % portion;
                if(d > portion / 2)
                {
                    d = portion - d;
                }
                float shiftModifer = 2 * MathF.PI * (1f / petals) * (Math.Abs((portion / 2) - d) / (float)(portion / 2));
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<EPDust>(), QwertyMethods.PolarVector(d * 0.4f, Projectile.velocity.ToRotation() + 2 * MathF.PI * ((float)i / (float)dustCount) + shiftModifer));
                dust.noGravity = true;
                dust.velocity *= 5f;
            }
            /*
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType<EPDust>(), 0f, 0f, 100, default(Color), 1f)];
                dust.noGravity = true;
                dust.velocity *= 5f;
                float distFromCenter = Main.rand.NextFloat(0, 1f);
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);

                dust.position = Projectile.Center + new Vector2(MathF.Cos(theta) * distFromCenter * Projectile.width / 2, MathF.Sin(theta) * distFromCenter * Projectile.height / 2);
            }
            */
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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