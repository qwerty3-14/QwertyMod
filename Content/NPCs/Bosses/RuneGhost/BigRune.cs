using Microsoft.Xna.Framework;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;

namespace QwertyMod.Content.NPCs.Bosses.RuneGhost
{
    public class BigRune : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = Projectile.height = 200;
            Projectile.timeLeft = 1400;
        }
        int frame = 0;
        float timer = 0;
        int atackTimer = 0;
        bool madeRunes = false;
        public override void AI()
        {
            Projectile.netUpdate = true;
            timer += Projectile.ai[1];
            frame = (int)(20f * (timer / 240f));
            if (frame > 19)
            {
                frame = 19;
            }
            if (timer > 240f)
            {
                atackTimer++;
                switch ((int)Projectile.ai[0])
                {
                    case (int)Runes.Aggro:
                        if (!madeRunes)
                        {
                            madeRunes = true;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AggroRune>(), (int)(Projectile.damage * 1.3f), 0, Main.myPlayer)];
                                    p.rotation = (i / 8f) * MathF.PI * 2;
                                }
                            }
                        }
                        break;
                    case (int)Runes.Leech:
                        if (atackTimer % 120 == 1)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                float closest = 100000;
                                Player closestPlayer = null;
                                for (int i = 0; i < Main.maxPlayers; i++)
                                {
                                    if (Main.player[i].active && (Projectile.Center - Main.player[i].Center).Length() < closest)
                                    {
                                        closest = (Projectile.Center - Main.player[i].Center).Length();
                                        closestPlayer = Main.player[i];
                                    }
                                }
                                if (closestPlayer != null)
                                {
                                    float r = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                                    Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), closestPlayer.Center + QwertyMethods.PolarVector(160f, r), Vector2.Zero, ModContent.ProjectileType<LeechRune>(), (int)(Projectile.damage), 0, Main.myPlayer)];
                                }
                            }
                        }
                        break;
                    case (int)Runes.IceRune:
                        if (!madeRunes)
                        {
                            madeRunes = true;
                            if (Main.netMode != NetmodeID.Server)
                            {
                                Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.LocalPlayer.Center, Vector2.Zero, ModContent.ProjectileType<IceRune>(), (int)(Projectile.damage * 1.6f), 0, Main.myPlayer)];
                                p.rotation = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                            }
                            Projectile.Kill();
                        }
                        break;
                    case (int)Runes.Pursuit:
                        if (!madeRunes)
                        {
                            madeRunes = true;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, QwertyMethods.PolarVector(20f, (i / 4f) * MathF.PI * 2f), ModContent.ProjectileType<PursuitRune>(), (int)(Projectile.damage * 1f), 0, Main.myPlayer);
                                }
                            }
                            Projectile.Kill();
                        }
                        break;
                }
                if (atackTimer >= 720)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float c = (timer / 240f);
            if (c > 1f)
            {
                c = 1f;
            }
            Main.EntitySpriteDraw(RuneSprites.bigRuneTransition[(int)Projectile.ai[0]][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(50, 50), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            int dustType = 0;

            switch ((int)Projectile.ai[0])
            {
                case 0:
                    dustType = ModContent.DustType<AggroRuneLash>();
                    break;
                case 1:
                    dustType = ModContent.DustType<LeechRuneDeath>();
                    break;
                case 2:
                    dustType = ModContent.DustType<IceRuneDeath>();
                    break;
                case 3:
                    dustType = ModContent.DustType<PursuitRuneDeath>();
                    break;
            }
            for (int d = 0; d < 300; d++)
            {
                //Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, mod.DustType(dustName));
                Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(Main.rand.Next(100), Main.rand.NextFloat(-MathF.PI, MathF.PI)), dustType);
            }
        }
    }
}
