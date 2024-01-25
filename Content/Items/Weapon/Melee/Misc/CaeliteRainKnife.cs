using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;


namespace QwertyMod.Content.Items.Weapon.Melee.Misc
{
    public class CaeliteRainKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.knockBack = 1;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.width = 18;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 12f;
            Item.useTime = 5;
            Item.useAnimation = 15;
            Item.shoot = ModContent.ProjectileType<CaeliteRainKnifeP>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = new Vector2((Main.MouseWorld.X + player.Center.X) / 2f + Main.rand.Next(-100, 100), position.Y - 600);
            float trueSpeed = velocity.Length();
            int shift = Main.rand.Next(-50, 50);
            velocity.X = MathF.Cos((new Vector2(Main.MouseWorld.X + shift, Main.MouseWorld.Y) - position).ToRotation()) * trueSpeed;
            velocity.Y = MathF.Sin((new Vector2(Main.MouseWorld.X + shift, Main.MouseWorld.Y) - position).ToRotation()) * trueSpeed;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public class CaeliteRainKnifeP : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                //DisplayName,SetDefault("Caelite Rain Knife");
            }

            public override void SetDefaults()
            {
                Projectile.aiStyle = 1;
                //aiType = ProjectileID.Bullet;
                Projectile.width = 18;
                Projectile.height = 18;
                Projectile.friendly = true;
                Projectile.penetrate = 1;
                Projectile.DamageType = DamageClass.Melee;
                Projectile.tileCollide = false;
            }

            private bool runOnce = true;
            private float outOfPhaseHeight;
            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (Main.rand.NextBool(10))
                {
                    target.AddBuff(ModContent.BuffType<PowerDown>(), 120);
                }
            }

            public override void AI()
            {
                if (Main.rand.NextBool(10))
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<CaeliteDust>(), Vector2.Zero);
                    dust.frame.Y = 0;
                }
                if (runOnce)
                {
                    outOfPhaseHeight = Main.MouseWorld.Y;
                    runOnce = false;
                }

                if (Projectile.Center.Y > outOfPhaseHeight)
                {
                    Projectile.tileCollide = true;
                }
                //Main.NewText(outOfPhaseHeight);
            }

            public override void OnKill(int timeLeft)
            {
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CaeliteDust>())];
                }
            }
        }
    }
}