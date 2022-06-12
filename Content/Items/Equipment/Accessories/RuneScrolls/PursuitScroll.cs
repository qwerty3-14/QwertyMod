using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Dusts;
using Terraria.DataStructures;

namespace QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls
{
    public class PursuitScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pursuit Scroll");
            Tooltip.SetDefault("Minions ocasionaly shoot pursuit runes");
            
        }

        public override void SetDefaults()
        {
            Item.value = 500000;
            Item.rare = 9;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 40;

            Item.width = 54;
            Item.height = 56;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ScrollEffects>().pursuit = true;
        }
    }

    internal class PursuitRuneFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 20;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 720;
            Projectile.DamageType = DamageClass.Summon;
        }

        public int runeTimer;
        public NPC target;

        public float runeSpeed = 10;
        public float runeDirection;
        public float runeTargetDirection;
        public bool runOnce = true;
        public int f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                Projectile.rotation = (Projectile.velocity).ToRotation();
                runOnce = false;
            }
            if (Projectile.alpha > 0)
                Projectile.alpha -= 5;
            else
                Projectile.alpha = 0;
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center))
            {
                Projectile.rotation.SlowRotation((target.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(1));
            }
            Projectile.velocity = new Vector2((float)(Math.Cos(Projectile.rotation) * runeSpeed), (float)(Math.Sin(Projectile.rotation) * runeSpeed));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 1200);
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<PursuitRuneDeath>());
            }
        }
    }

    public class MinionPursuit : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int runeCounter;
        public float runeSpeed = 10;
        NPC target;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            ScrollEffects modPlayer = player.GetModPlayer<ScrollEffects>();
            if ((projectile.minion && projectile.minionSlots > 0 || projectile.sentry) && modPlayer.pursuit)
            {
                runeCounter++;
                if (runeCounter >= 120 / projectile.minionSlots || (runeCounter >= 120 && projectile.sentry))
                {
                    if(QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC ))
                    {
                        Projectile.NewProjectile(new EntitySource_Misc(""), projectile.Center, (target.Center - projectile.Center).SafeNormalize(Vector2.UnitY) * runeSpeed, ProjectileType<PursuitRuneFreindly>(), (int)(40f * player.GetDamage(DamageClass.Summon).Multiplicative), projectile.knockBack, projectile.owner);
                        runeCounter = 0;
                    }
                    
                }
            }
        }
    }
}