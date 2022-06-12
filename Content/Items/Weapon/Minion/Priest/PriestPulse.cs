using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.Priest
{
    public class PriestPulse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Preist Pulse");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.light = 1f;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter % 40 > 20 ? 1 : 0;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
                dust.velocity *= 3f;
            }
        }
    }
}
