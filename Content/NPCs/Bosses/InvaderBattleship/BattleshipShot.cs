using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class BattleshipShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Battleship");
        }


        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }
        bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {

                SoundEngine.PlaySound(SoundID.Item157, Projectile.Center);
                runOnce = false;
            }
            Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<InvaderGlow>(), Vector2.Zero);
            d.noGravity = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
    public class InvaderRay : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Battleship");
        }


        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }
        bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                SoundEngine.PlaySound(SoundID.Item157, Projectile.Center);
                runOnce = false;
            }
            Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<InvaderGlow>(), Vector2.Zero);
            d.noGravity = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
}