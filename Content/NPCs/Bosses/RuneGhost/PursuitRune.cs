﻿using Microsoft.Xna.Framework;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.Bosses.RuneGhost
{
    public class PursuitRune : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
        }
        private int timer;
        private float closest = 10000;
        bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            timer++;
            if (timer > 119 && timer % 120 == 0)
            {
                Projectile.extraUpdates++;
                for (int d = 0; d <= 40; d++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PursuitRuneDeath>());
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && (Projectile.Center - Main.player[i].Center).Length() < closest)
                    {
                        closest = (Projectile.Center - Main.player[i].Center).Length();
                        Projectile.ai[0] = (Main.player[i].Center - Projectile.Center).ToRotation();
                        Projectile.netUpdate = true;
                    }
                }
            }
            Projectile.rotation.SlowRotation(Projectile.ai[0], MathF.PI / 240f);
            Projectile.velocity = QwertyMethods.PolarVector(12, Projectile.rotation);
            closest = 10000;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float c = (timer / 40f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = timer / 2;
            if (frame > 19)
            {
                frame = 19;
            }
            Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.Pursuit][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(10, 5), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int d = 0; d <= 40; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PursuitRuneDeath>());
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Venom, 180);
        }
    }
}
