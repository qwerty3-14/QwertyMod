using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Morphs.HydraBarrage
{
    public class HydraBarrage : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape shift: Hydra Barrage");
            Tooltip.SetDefault("Launches a barrage of hydra breath");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 142;
            Item.knockBack = 0;
            Item.DamageType = DamageClass.Summon;
            Item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 24;
            Item.noMelee = true;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = 5;

            Item.value = 250000;
            Item.rare = 5;
            Item.noUseGraphic = true;
            Item.width = 18;
            Item.height = 32;
            Item.shoot = ProjectileType<HydraBarrageBase>();
            Item.shootSpeed = 0f;
            Item.channel = true;
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if ((Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot) || player.HasBuff(BuffType<MorphCooldown>()))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class HydraBarrageBase : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HydraBarrage Barrage Base");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;
        }

        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<HydraBarrageHead>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f, 2);
                }
                runOnce = false;
            }

            Player player = Main.player[Projectile.owner];
            player.Center = Projectile.Center;
            player.immune = true;
            player.immuneTime = 2;
            player.statDefense = 0;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }

    public class HydraBarrageHead : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 10;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Summon;
        }

        private bool runOnce = true;
        private Vector2 offset;

        public override void AI()
        {
            if (runOnce)
            {
                offset = QwertyMethods.PolarVector(100, Main.rand.NextFloat() * (float)Math.PI * 2f);
                runOnce = false;
            }
            Projectile.scale = Projectile.ai[0];
            Player player = Main.player[Projectile.owner];
            Vector2 diff = ((player.Center + offset) - Projectile.Center);
            Projectile.velocity = diff;
            if (Projectile.velocity.Length() > 16f)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 16f;
            }
            Projectile.rotation = (QwertyMod.GetLocalCursor(player.whoAmI) - Projectile.Center).ToRotation();
            if (Projectile.timeLeft == 10)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + QwertyMethods.PolarVector(57 * Projectile.scale, Projectile.rotation), QwertyMethods.PolarVector(10, Projectile.rotation), ProjectileType<HydraBarrageBreath>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0], 0f);
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[1] > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<HydraBarrageHead>(), (int)(Projectile.damage * .8f), Projectile.knockBack * .8f, Projectile.owner, Projectile.ai[0] * .8f, Projectile.ai[1] - 1);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D neck = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Morphs/HydraBarrage/HydraBarrageNeck").Value;
            Texture2D neckBase = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Morphs/HydraBarrage/HydraBarrageBase").Value;
            for (float f = 0; f < (Projectile.Center - Main.player[Projectile.owner].Center).Length(); f += neck.Height * Projectile.scale)
            {
                Main.EntitySpriteDraw(f == 0 ? neckBase : neck, Main.player[Projectile.owner].Center - Main.screenPosition + QwertyMethods.PolarVector(f, (Projectile.Center - Main.player[Projectile.owner].Center).ToRotation()), null, lightColor, (Projectile.Center - Main.player[Projectile.owner].Center).ToRotation() + (float)Math.PI / 2, neck.Size() * .5f, Vector2.One * Projectile.scale, 0, 0);
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, Vector2.One * 36f, Vector2.One * Projectile.scale, 0, 0);
            return false;
        }
    }

    public class HydraBarrageBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrage Breath");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 14;
            Projectile.height = 8;

            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.scale = Projectile.ai[0];
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBreathGlow>());
        }
    }
}