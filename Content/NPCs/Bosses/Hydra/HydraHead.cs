using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.Hydra
{
    internal class HydraHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Hydra Head");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 72;
            NPC.height = 72;

            NPC.damage = 50;
            NPC.defense = 18;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.dontTakeDamage = false;
            NPC.noTileCollide = true;
            NPC.rotation = MathF.PI / 2;
            NPC.lifeMax = 2000;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/BeastOfThreeHeads");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
				new FlavorTextBestiaryInfoElement("It's a Hydra!")
            });
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = 2000;
            NPC.damage = (int)(NPC.damage * .7f);
        }

        private NPC Body = null;
        private float headSpread = 3f * MathF.PI / 4f;
        private bool runOnce = true;
        private float rotateTo;
        private Vector2 flyTo;
        private int attackTimer = 0;
        private bool attacking = false;
        private int projDamge;
        private bool beamAttack;
        private Projectile laser;
        private int shotWarming = 60;
        private int beamTime = 300;

        public override void AI()
        {
            if (Main.expertMode)
            {
                projDamge = NPC.damage / 4;
            }
            else
            {
                projDamge = NPC.damage / 2;
            }
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (runOnce)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[0] = -1;
                }

                runOnce = false;
                NPC.lifeMax = 2000;
                NPC.life = NPC.lifeMax;
            }

            if (NPC.ai[0] == -1)
            {
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].type == NPCType<Hydra>() && Main.npc[n].active)
                    {
                        NPC.ai[0] = n;
                        break;
                    }
                }
            }

            if (NPC.ai[0] != -1 || Main.npc[(int)NPC.ai[0]].type != NPCType<Hydra>())
            {
                Body = Main.npc[(int)NPC.ai[0]];
                int headCount = 0;
                int whichHeadAmI = 0;
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].type == NPCType<HydraHead>() && Main.npc[n].active && Main.npc[n].ai[0] == NPC.ai[0])
                    {
                        if (Main.npc[n].Center.X < NPC.Center.X || (Main.npc[n].Center.X == NPC.Center.X && n < NPC.whoAmI))
                        {
                            whichHeadAmI++;
                        }
                        headCount++;
                    }
                }

                float rotationOffset = (headSpread * (((float)whichHeadAmI + 1) / ((float)headCount + 1))) - headSpread / 2f;
                Vector2 offSet = QwertyMethods.PolarVector(400, -MathF.PI / 2 + rotationOffset);
                offSet.X *= 1.5f;

                flyTo = Body.Center + offSet;
                NPC.velocity = (flyTo - NPC.Center) * .1f;

                if (attacking && attackTimer == 0)
                {
                    if (beamAttack)
                    {
                        laser.Kill();
                        laser = null;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (!beamAttack)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(50, NPC.rotation), QwertyMethods.PolarVector(5, NPC.rotation), ProjectileType<HydraBreath>(), projDamge, 0f, Main.myPlayer);
                        }
                    }
                    attacking = false;
                    beamAttack = false;
                }
                if (attackTimer < 60)
                {
                    attackTimer++;
                }
                else
                {
                    if (Main.rand.NextBool(20) && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        attacking = true;
                        if (Main.rand.NextFloat(10) < Math.Abs(player.velocity.X) && ((NPC.Center.X > player.Center.X && player.velocity.X > 0) || (NPC.Center.X < player.Center.X && player.velocity.X < 0)))
                        {
                            beamAttack = true;
                            attackTimer = -beamTime;
                            rotateTo = MathF.PI / 2;
                        }
                        else
                        {
                            rotateTo = (player.Center - NPC.Center).ToRotation() + (Main.rand.NextFloat(1, -1) * MathF.PI / 8);
                            attackTimer = -shotWarming;
                        }
                        NPC.netUpdate = true;
                    }
                }
                if (beamAttack)
                {
                    if (attackTimer < -beamTime / 2)
                    {
                        float dir = NPC.rotation + Main.rand.NextFloat(-1, 1) * MathF.PI / 4;
                        Dust.NewDustPerfect(NPC.Center + QwertyMethods.PolarVector(50, NPC.rotation), DustType<HydraBeamGlow>(), QwertyMethods.PolarVector(6, dir));
                    }
                    else if (attackTimer == -beamTime / 2)
                    {
                        laser = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ProjectileType<HydraBeamT>(), (int)(projDamge * 1.5f), 3f, Main.myPlayer, NPC.whoAmI, 420)];
                    }
                    else
                    {
                        NPC.velocity = Vector2.Zero;
                    }
                }
                if (!attacking)
                {
                    rotateTo = (player.Center - NPC.Center).ToRotation();
                    if (laser != null)
                    {
                        if (laser.active)
                        {
                            laser.Kill();
                            laser = null;
                        }
                    }
                }

                NPC.rotation = QwertyMethods.SlowRotation(NPC.rotation, rotateTo, 4);
                if (!Body.active || Body.type != NPCType<Hydra>())
                {
                    NPC.life = 0;
                    NPC.checkDead();
                }
                if (Body.dontTakeDamage)
                {
                    attacking = false;
                    beamAttack = false;
                }
            }
            else
            {
                NPC.life = 0;
                NPC.checkDead();
            }
        }
        public override bool PreKill()
        {
            if (laser != null)
            {
                if (laser.active)
                {
                    laser.Kill();
                    laser = null;
                }
            }
            if (NPC.ai[0] != -1)
            {
                Body = Main.npc[(int)NPC.ai[0]];
                if (Body.life > 1)
                {
                    Body.life--;
                }
                else
                {
                    //Body.dontTakeDamage = true;
                    Body.ai[3]++;
                }

                for (int h = 0; h < 2; h++)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient && Body.active && Body.type == NPCType<Hydra>())
                    {
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<HydraHead>(), ai0: Body.whoAmI, ai1: NPC.ai[1]);
                    }
                }
            }
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (attacking)
            {
                NPC.frame.Y = (int)NPC.ai[1] * 2 * frameHeight + frameHeight;
            }
            else
            {
                NPC.frame.Y = (int)NPC.ai[1] * 2 * frameHeight;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[0] != -1)
            {
                Body = Main.npc[(int)NPC.ai[0]];
                int headCount = 0;
                int whichHeadAmI = 0;
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].type == NPCType<HydraHead>() && Main.npc[n].active && Main.npc[n].ai[0] == NPC.ai[0])
                    {
                        if (n < NPC.whoAmI)
                        {
                            whichHeadAmI++;
                        }
                        headCount++;
                    }
                }
                if (whichHeadAmI == headCount - 1 && NPC.ai[0] != -1)
                {
                    Body = Main.npc[(int)NPC.ai[0]];
                    spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/Hydra").Value, Body.position - screenPos,
                            Body.frame, Lighting.GetColor((int)Body.Center.X / 16, (int)Body.Center.Y / 16), Body.rotation,
                            new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/Hydra_Glow").Value, Body.position - screenPos,
                                Body.frame, Color.White, Body.rotation,
                                new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                }
                Vector2 neckOrigin = new Vector2(Body.Center.X, Body.Center.Y - 50);
                Vector2 center = NPC.Center;
                Vector2 distToProj = neckOrigin - NPC.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                while (distance > 30f && !float.IsNaN(distance))
                {
                    distToProj.Normalize();                 //get unit vector
                    distToProj *= 30f;                      //speed = 30
                    center += distToProj;                   //update draw position
                    distToProj = neckOrigin - center;    //update distance
                    distance = distToProj.Length();

                    //Draw chain
                    spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/HydraNeck").Value, center - screenPos,
                        new Rectangle(0, 0, 52, 30), Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), projRotation,
                        new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);
                }
                spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/HydraNeckBase").Value, neckOrigin - screenPos,
                            new Rectangle(0, 0, 52, 30), Lighting.GetColor((int)neckOrigin.X / 16, (int)neckOrigin.Y / 16), projRotation,
                            new Vector2(52 * 0.5f, 30 * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
                        NPC.frame, drawColor, NPC.rotation,
                        new Vector2(72 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/HydraHead_Glow").Value, NPC.Center - screenPos,
                        NPC.frame, Color.White, NPC.rotation,
                        new Vector2(72 * 0.5f, 72 * 0.5f), 1f, SpriteEffects.None, 0f);
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }

        public override void OnKill()
        {

        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attacking);
            writer.Write(rotateTo);
            writer.Write(beamAttack);
            writer.Write(attackTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attacking = reader.ReadBoolean();
            rotateTo = reader.ReadSingle();
            beamAttack = reader.ReadBoolean();
            attackTimer = reader.ReadInt32();
        }

        public override void BossHeadSlot(ref int index)
        {
            switch ((int)NPC.ai[1])
            {
                case 0:
                    index = NPCHeadLoader.GetBossHeadSlot(QwertyMod.HydraHead1);
                    break;

                case 1:
                    index = NPCHeadLoader.GetBossHeadSlot(QwertyMod.HydraHead2);
                    break;

                case 2:
                    index = NPCHeadLoader.GetBossHeadSlot(QwertyMod.HydraHead3);
                    break;
            }
        }
    }
}