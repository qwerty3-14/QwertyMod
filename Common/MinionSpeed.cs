﻿using QwertyMod.Content.Items.Weapon.Whip.Discipline;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Common
{
    public class MinionSpeedStats : ModPlayer
    {
        public float minionSpeed = 1f;

        public override void ResetEffects()
        {
            minionSpeed = 1f;
        }
        public override void PreUpdate()
        {
            for (int p = 0; p < 200; p++)
            {
                Projectile projectile = Main.projectile[p];
                if (projectile.active && projectile.owner == Player.whoAmI && projectile.minion)
                {

                    projectile.extraUpdates -= projectile.GetGlobalProjectile<MinionSpeedBoost>().bonustUpdates;
                    projectile.GetGlobalProjectile<MinionSpeedBoost>().minionSpeedAccumulator += (minionSpeed - 1f);
                    if (projectile.GetGlobalProjectile<MinionSpeedBoost>().discipline > 0)
                    {
                        projectile.GetGlobalProjectile<MinionSpeedBoost>().minionSpeedAccumulator += 0.25f;
                        projectile.GetGlobalProjectile<MinionSpeedBoost>().discipline--;
                    }
                    projectile.GetGlobalProjectile<MinionSpeedBoost>().bonustUpdates = (int)(projectile.GetGlobalProjectile<MinionSpeedBoost>().minionSpeedAccumulator);
                    projectile.GetGlobalProjectile<MinionSpeedBoost>().minionSpeedAccumulator -= projectile.GetGlobalProjectile<MinionSpeedBoost>().bonustUpdates;
                    projectile.extraUpdates += projectile.GetGlobalProjectile<MinionSpeedBoost>().bonustUpdates;

                    if (projectile.extraUpdates < 0)
                    {
                        projectile.extraUpdates = 0;
                    }
                }
            }
        }
    }
    public class MinionSpeedBoost : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int bonustUpdates = 0;
        public float minionSpeedAccumulator = 0f;
        public int discipline = 0;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.HasBuff(BuffType<DisciplineTag>()))
            {

                if (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type])
                {
                    for (int p = 0; p < 200; p++)
                    {
                        if (Main.projectile[p].active && Main.projectile[p].minion && projectile.owner == Main.projectile[p].owner)
                        {
                            SpeedBuff(Main.projectile[p]);
                            target.RequestBuffRemoval(BuffType<DisciplineTag>());
                        }
                    }
                }
            }
        }
        void SpeedBuff(Projectile projectile)
        {
            if (projectile.GetGlobalProjectile<MinionSpeedBoost>().discipline == 0)
            {
                for (int d = 0; d < 20; d++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, 15, QwertyMethods.PolarVector(10, ((float)d / 20f) * MathF.PI * 2f));
                    dust.noGravity = true;
                }
            }
            projectile.GetGlobalProjectile<MinionSpeedBoost>().discipline = 240;
        }
    }
}
