using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
            if(frame > 19)
            {
                frame = 19;
            }
            if(timer > 240f)
            {
                atackTimer++;
                switch ((int)Projectile.ai[0])
                {
                    case (int)Runes.Aggro:
                        if (!madeRunes)
                        {
                            madeRunes = true;
                            if (Main.netMode != 1)
                            {
                                for(int i = 0; i < 8; i++)
                                {
                                    Projectile p = Main.projectile[ Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center, Vector2.Zero, ProjectileType<AggroRune>(), (int)(Projectile.damage * 1.3f), 0, Main.myPlayer)];
                                    p.rotation = (i / 8f) * (float)Math.PI * 2;
                                }
                            }
                        }
                        break;
                    case (int)Runes.Leech:
                        if(atackTimer % 120 == 1)
                        {
                            if (Main.netMode != 1)
                            {
                                float closest = 100000;
                                Player closestPlayer = null;
                                for (int i = 0; i < 255; i++)
                                {
                                    if (Main.player[i].active && (Projectile.Center - Main.player[i].Center).Length() < closest)
                                    {
                                        closest = (Projectile.Center - Main.player[i].Center).Length();
                                        closestPlayer = Main.player[i];
                                    }
                                }
                                if (closestPlayer != null)
                                {
                                    float r = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                                    Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), closestPlayer.Center + QwertyMethods.PolarVector(160f, r), Vector2.Zero, ProjectileType<LeechRune>(), (int)(Projectile.damage), 0, Main.myPlayer)];
                                }
                            }
                        }
                        break;
                    case (int)Runes.IceRune:
                        if(!madeRunes)
                        {
                            madeRunes = true;
                            if(Main.netMode != 2)
                            {
                                Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), Main.LocalPlayer.Center, Vector2.Zero, ProjectileType<IceRune>(), (int)(Projectile.damage * 1.6f), 0, Main.myPlayer)];
                                p.rotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                            }
                            Projectile.Kill();
                        }
                        break;
                    case (int)Runes.Pursuit:
                        if (!madeRunes)
                        {
                            madeRunes = true;
                            if (Main.netMode != 1)
                            {
                                for(int i =0; i < 4; i++)
                                {
                                    Projectile.NewProjectile(new EntitySource_Misc(""),  Projectile.Center, QwertyMethods.PolarVector(20f, (i /4f) * (float)Math.PI * 2f), ProjectileType<PursuitRune>(), (int)(Projectile.damage * 1f), 0, Main.myPlayer);
                                }
                            }
                            Projectile.Kill();
                        }
                        break;
                }
                if(atackTimer >=720)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float c = (timer / 240f);
            if(c > 1f)
            {
                c = 1f;
            }
            Main.EntitySpriteDraw(RuneSprites.bigRuneTransition[(int)Projectile.ai[0]][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(50, 50), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            int dustType = 0;

            switch ((int)Projectile.ai[0])
            {
                case 0:
                    dustType = DustType<AggroRuneLash>();
                    break;
                case 1:
                    dustType = DustType<LeechRuneDeath>();
                    break;
                case 2:
                    dustType = DustType<IceRuneDeath>();
                    break;
                case 3:
                    dustType = DustType<PursuitRuneDeath>();
                    break;
            }
            for(int d = 0; d < 300; d++)
            {
                //Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, mod.DustType(dustName));
                Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(Main.rand.Next(100), Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)),dustType);
            }
        }
    }
}
