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

using System.IO;

namespace QwertyMod.Content.NPCs.Bosses.AncientMachine
{
    public class AncientMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
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
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[1] = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    moveTo = new Vector2(player.Center.X + MathF.Cos(NPC.ai[1]) * 600, player.Center.Y + MathF.Sin(NPC.ai[1]) * 350);
                    NPC.ai[2] = moveTo.X;
                    NPC.ai[3] = moveTo.Y;
                    NPC.netUpdate = true;
                    justTeleported = true;
                    timer = 0;
                }
            }
            else if (timer > waitTime)
            {
                charging = true;
            }
            else
            {
                charging = false;
            }
            if (charging)
            {
                NPC.velocity = new Vector2(MathF.Cos(NPC.rotation) * chargeSpeed, MathF.Sin(NPC.rotation) * chargeSpeed);
            }
            else if(timer > 20)
            {
                if(justTeleported)
                {
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);

                        Dust dust = Dust.NewDustPerfect(NPC.Center + QwertyMethods.PolarVector(minionRingRadius, theta), ModContent.DustType<AncientGlow>(), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    NPC.netOffset *= 0;
                }
                NPC.Center = new Vector2(NPC.ai[2], NPC.ai[3]);
                if(justTeleported)
                {
                    SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                        Dust dust = Dust.NewDustPerfect(NPC.Center, ModContent.DustType<AncientGlow>(), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    justTeleported = false;
                }
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
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMinion_Glow").Value, new Vector2(NPC.Center.X - screenPos.X, NPC.Center.Y - screenPos.Y),
                        NPC.frame, Color.White, NPC.rotation,
                        new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(justTeleported);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
            justTeleported = reader.ReadBoolean();
        }
    }
}