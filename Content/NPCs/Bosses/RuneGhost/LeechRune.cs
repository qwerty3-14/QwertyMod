using Microsoft.Xna.Framework;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace QwertyMod.Content.NPCs.Bosses.RuneGhost
{
    public class LeechRune : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < 60)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        int timer = 0;
        public override void AI()
        {
            timer++;
            if (timer == 60)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float closest = 10000;
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
            }
            if (timer > 60)
            {
                Projectile.velocity = QwertyMethods.PolarVector(10, Projectile.ai[0]);
            }
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathF.PI / 60f;
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 40; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<LeechRuneDeath>());
            }
        }
        public NPC runeGhost;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            foreach (NPC npcSearch in Main.npc)
            {
                if (npcSearch.type == NPCType<RuneGhost>())
                {
                    runeGhost = npcSearch;
                    break;
                }
            }
            if (runeGhost != null && runeGhost.active)
            {
                runeGhost.life += info.Damage * 40;
                runeGhost.HealEffect(info.Damage * 40, true);

            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float c = (timer / 60f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = timer / 3;
            if (frame > 19)
            {
                frame = 19;
            }
            Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.Leech][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(10, 10), Vector2.One * 2, 0, 0);
            return false;
        }
    }
}
