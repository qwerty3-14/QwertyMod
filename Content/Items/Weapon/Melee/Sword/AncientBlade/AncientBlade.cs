using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.AncientBlade
{
    public class AncientBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Blade");
            //Tooltip.SetDefault("Launches a spread of orbs");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = 150000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;

            Item.width = 70;
            Item.height = 70;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Melee/Sword/AncientBlade/AncientBlade_Glow").Value;
            }

            Item.autoReuse = true;
            Item.shoot = ProjectileType<AncientOrb>();
            Item.shootSpeed = 9;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 8 + Main.rand.Next(6);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15)); // 30 degree spread.
                                                                                             // If you want to randomize the speed to stagger the projectiles
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI, 35 + Main.rand.Next(10));
            }
            return false; // return false because we don't want tmodloader to shoot projectile
        }
    }

    public class AncientOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            //DisplayName,SetDefault("Ancient Orb");
        }


        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 40;
            Projectile.alpha = 255;
        }

        public int dustTimer;

        public override void AI()
        {
            if (Projectile.ai[0] != 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[0];
                Projectile.ai[0] = 0;
            }
            if (Projectile.alpha > 0)
            {
                //Projectile.alpha -= (int)(255f / 180f);
                Projectile.alpha -= 12;
            }
            else
            {
                Projectile.alpha = 0;
            }
            Projectile.scale = .5f + (.5f * 1 - (Projectile.alpha / 255f));
            for (int d = 0; d < Projectile.alpha / 30; d++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(25, theta), DustType<AncientGlow>(), QwertyMethods.PolarVector(-6, theta) + Projectile.velocity);
                dust.scale = .5f;
                dust.alpha = 255;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 10)
            {
                if (Projectile.frame == 1)
                {
                    Projectile.frame = 0;
                }
                else
                {
                    Projectile.frame = 1;
                }
                Projectile.frameCounter = 0;
            }
            dustTimer++;
            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<AncientGlow>(), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }
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
            return false;
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(0, 0, 0, 0), (float)Projectile.alpha / 255f), Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}