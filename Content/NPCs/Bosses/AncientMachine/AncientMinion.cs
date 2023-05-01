using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.AncientMachine
{
    public class AncientMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Minion");
            Main.npcFrameCount[NPC.type] = 1;
        }


        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 56;
            NPC.damage = 40;
            NPC.defense = 8;

            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;

            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            //NPC.aiStyle = 10;
            //aiType = 10;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 70;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("Ancient Machine doesn't just destroy...")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        /*
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Vector2 pos = NPC.Center + QwertyMethods.PolarVector(-16, NPC.rotation + MathF.PI / 2);
                Gore gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/MiniDebris_1" + (ModContent.GetInstance<SpriteSettings>().ClassicAncient ? "_Old" : "")), 1f)];
                gore.rotation = NPC.rotation;

                pos = NPC.Center + QwertyMethods.PolarVector(14, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/MiniDebris_2" + (ModContent.GetInstance<SpriteSettings>().ClassicAncient ? "_Old" : "")), 1f)];
                gore.rotation = NPC.rotation;
                for (int i = 0; i < 180; i++)
                {
                    float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                    Dust dust = Dust.NewDustPerfect(NPC.Center, mod.DustType("AncientGlow"), QwertyMethods.PolarVector(Main.rand.Next(1, 9), theta));
                    dust.noGravity = true;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                int dustType = 148;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }
        */
        public const int minionRingRadius = 50;
        public const int minionRingDustQty = 50;
        private const int AI_Timer_Slot = 1;

        public int timer;
        public int Pos = 1;
        public int damage = 30;
        public int switchTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int attackType = 1;
        public bool charging;
        public NPC parent;
        private int waitTime = 120;
        private int chargeTime = 120;
        private Vector2 moveTo;
        private bool justTeleported;
        private float chargeSpeed = 12;
        private bool runOnce = true;

        public override void AI()
        {
            if (justTeleported)
            {
                justTeleported = false;
            }
            if (runOnce)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[2] = NPC.Center.X;
                    NPC.ai[3] = NPC.Center.Y;
                    NPC.netUpdate = true;
                }
                runOnce = false;
            }
            Player player = Main.player[NPC.target];
            //parent = Main.npc[(int)NPC.ai[0]];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 1)
                    {
                        NPC.timeLeft = 1;
                    }
                    return;
                }
            }
            timer++;
            if (timer > waitTime + chargeTime)
            {
                for (int i = 0; i < minionRingDustQty; i++)
                {
                    float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);

                    Dust dust = Dust.NewDustPerfect(NPC.Center + QwertyMethods.PolarVector(minionRingRadius, theta), DustType<AncientGlow>(), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                    dust.noGravity = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[1] = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                    NPC.netUpdate = true;
                }
                moveTo = new Vector2(player.Center.X + MathF.Cos(NPC.ai[1]) * 600, player.Center.Y + MathF.Sin(NPC.ai[1]) * 350);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[2] = moveTo.X;
                    NPC.ai[3] = moveTo.Y;
                    NPC.netUpdate = true;
                }
                justTeleported = true;
                timer = 0;
            }
            else if (timer > waitTime)
            {
                charging = true;
            }
            else
            {
                if (timer == 2)
                {
                    SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                        Dust dust = Dust.NewDustPerfect(NPC.Center, DustType<AncientGlow>(), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                }
                charging = false;
            }
            if (charging)
            {
                NPC.velocity = new Vector2(MathF.Cos(NPC.rotation) * chargeSpeed, MathF.Sin(NPC.rotation) * chargeSpeed);
            }
            else
            {
                NPC.Center = new Vector2(NPC.ai[2], NPC.ai[3]);
                NPC.velocity = new Vector2(0, 0);
                float targetAngle = new Vector2(player.Center.X - NPC.Center.X, player.Center.Y - NPC.Center.Y).ToRotation();
                NPC.rotation = targetAngle;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
                        NPC.frame, drawColor, NPC.rotation,
                        new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMinion_Glow").Value, new Vector2(NPC.Center.X - screenPos.X, NPC.Center.Y - screenPos.Y),
                        NPC.frame, Color.White, NPC.rotation,
                        new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}