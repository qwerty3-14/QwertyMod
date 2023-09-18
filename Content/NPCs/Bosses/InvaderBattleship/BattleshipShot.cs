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
    public class InvaderRay : ModProjectile
    {
        public override void SetStaticDefaults()
        {
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
                
                runOnce = false;
            }
            Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<InvaderGlow>(), Vector2.Zero);
            d.noGravity = true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
    public class InvaderExhaust : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 28;
            Projectile.timeLeft = 30;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.frame = (30 - Projectile.timeLeft) / 5;
        }
        public override bool PreDraw(ref Color drawColor)
        {
            drawColor = Color.White;
            return true;
        }
    }
    public class FlybyBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 5000;
        }
        public const int openTime = 60;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = openTime * 2;
        }
        float length = 0;
        float beamWidth = 10;
        public override void AI()
        {
            if(Projectile.ai[0] == -1)
            {
                Projectile.Kill();
            }
            //QwertyMethods.ServerClientCheck("" + Projectile.whoAmI);
            if(Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }
            for (length = 10; length < 2000; length++)
            {
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), 1, 1))
                {
                    break;
                }
            }
            if(length > 16)
            {
                for (int num657 = 0; num657 < 2; num657++)
                {
                    int num658 = Dust.NewDust(Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation) + QwertyMethods.PolarVector(beamWidth / 2f, Projectile.rotation) + Vector2.UnitX * -beamWidth/2f + (Projectile.rotation > 0 ? Vector2.UnitY * -16f: Vector2.Zero), (int)beamWidth, 16, ModContent.DustType<InvaderGlow>());
                    Main.dust[num658].noGravity = true;
                    Dust dust2 = Main.dust[num658];
                    dust2.velocity *= 0.5f;
                    //Main.dust[num658].velocity.X -= (float)num657 - nPC6.velocity.X * 2f / 3f;
                    Main.dust[num658].scale = 2.0f;
                }
            }
            if(Projectile.timeLeft > openTime)
            {
                beamWidth = ((float)((openTime * 2) - Projectile.timeLeft) / openTime) * 10f;
            }
            if(Projectile.timeLeft < 10)
            {
                beamWidth = ((float)Projectile.timeLeft / openTime) * 10f;
            }
            
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), beamWidth, ref point);
            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(length > 16)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 26, 22), Color.White, Projectile.rotation - MathF.PI / 2f, new Vector2(13, 11), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
                float subLength = length - (11 + 22);
                int midBeamHieght = 30;
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(11, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 24, 26, midBeamHieght), Color.White, Projectile.rotation - MathF.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, subLength / (float)midBeamHieght), SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(length - 22, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 56, 26, 22), Color.White, Projectile.rotation - MathF.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            }
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }

    }
}