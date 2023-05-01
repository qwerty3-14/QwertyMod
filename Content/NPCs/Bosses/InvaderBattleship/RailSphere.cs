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
    public class RailSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 76;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10 * 60;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        int timer = 0;
        Entity target;
        public override void AI()
        {
            if(Projectile.timeLeft < 30)
            {
                Projectile.damage = 0;
                Projectile.scale = Projectile.timeLeft / 30f;
                return;
            }
            target = FindTarget(Projectile);
            if(target != null)
            {
                
                if(timer >= 0)
                {
                    if(Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1))
                    {                
                        timer++;
                        for (int i = 0; i < 2; i++)
                        {
                            float rot = MathF.PI * i + MathF.PI * (float)timer / 10f;
                            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(30, rot), ModContent.DustType<InvaderGlow>(), QwertyMethods.PolarVector(-3f, rot));
                        }
                        if(timer > 120)
                        {
                            timer = -120;
                            SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_turret"), Projectile.Center);
                            if(Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                float rot = (target.Center - Projectile.Center).ToRotation();
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, QwertyMethods.PolarVector(1, rot), ModContent.ProjectileType<FlybyBeam>(), 2 * (Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage), 0);
                            }
                        }
                    }
                    else if(timer > 0)
                    {
                        timer = 0;
                    }
                    Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 4f;
                }
                else
                {
                    Projectile.velocity = Vector2.Zero;
                    timer++;
                }
            }
            else if(timer > 0)
            {
                timer = 0;
            }
            Projectile.frameCounter++;
            if(Projectile.frameCounter % 10 == 0)
            {
                Projectile.frame++;
            }
            if(Projectile.frame > 4)
            {
                Projectile.frame = 0;
            }
        }
        public override void PostDraw( Color lightColor)
        {
            if(target != null)
            {
                if(timer > 0)
                {
                    Texture2D beamWarning = Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderZap").Value;
                    float rot = (target.Center - Projectile.Center).ToRotation();
                    float length = (target.Center - Projectile.Center).Length();
                    Main.EntitySpriteDraw(beamWarning, Projectile.Center - Main.screenPosition, null, Color.White, rot, Vector2.UnitY * 1, new Vector2(length / 2f, 1), SpriteEffects.None, 0);
                }
            }
        }
        public static Entity FindTarget(Projectile projectile)
        {
            Entity target = null;
            float maxRange = 10000;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro < maxRange )
                {
                    target = Main.player[i];
                    maxRange = (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro;
                }
            }
            return target;
        }
    }
}