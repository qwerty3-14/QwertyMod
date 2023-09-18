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
    [AutoloadBossHead]
    internal class HydraHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 72;
            NPC.height = 72;

            NPC.damage = 60;
            NPC.defense = 18;
            //NPC.boss = true;
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
		public override void BossHeadRotation(ref float rotation)
		{
			rotation = NPC.rotation;
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
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossAdjustment);
            NPC.damage = (int)(NPC.damage * 0.75f);
        }
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return fireTimer > 0;
		}
        float shotSpeed = 11;
        NPC body = null;
        bool runOnce = true;
        Vector2 relativePosition;
        float relativeRotation;
        bool aimAtPlayer = false;
        float rotSpeed = MathF.PI / 15f;
        int fireTimer = 0;
        int fireType = 0;
        
        Vector2 NeckPos()
        {
            return body.Center + QwertyMethods.PolarVector(50, MathF.PI * -0.5f + body.rotation);
        }
		public override void AI()
		{
            //QwertyMethods.ServerClientCheck("" + NPC.Center);
            if(runOnce)
            {
                for(int i = 0; i < 200; i++)
                {
                    if(Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<Hydra>())
                    {
                        body = Main.npc[i];
                    }
                }
                runOnce = false;
            }
            if(body == null)
            {
                return;
            }
            if(!body.active || body.type != ModContent.NPCType<Hydra>())
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.StrikeInstantKill();
                }
            }
            if(fireTimer > 0)
            {
                if(fireTimer == 1)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if(fireType == 1)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(58, NPC.rotation), QwertyMethods.PolarVector(shotSpeed, NPC.rotation), ModContent.ProjectileType<HydraBreath>(), 25, 0);
                        }
                        if(fireType == 4)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(58, NPC.rotation), QwertyMethods.PolarVector(shotSpeed, NPC.rotation), ModContent.ProjectileType<LargeHydraBreath>(), 30, 0);
                        }
                    }
                }
                if(fireType == 2 || fireType == 3)
                {
                    relativePosition = relativePosition.RotatedBy((fireType == 2 ? 1 : -1) * (MathF.PI * 2f) / Hydra.swingHeadTime);
                    relativeRotation = relativeRotation += (fireType == 2 ? 1 : -1) * (MathF.PI * 2f) / Hydra.swingHeadTime;
                }
                fireTimer--;
            }
            Vector2 moveTo = NeckPos() + relativePosition.RotatedBy(body.rotation);
            //QwertyMethods.ServerClientCheck(moveTo + "");
            NPC.velocity = (moveTo - NPC.Center) * 0.13f;
            if(aimAtPlayer)
            {
                NPC.TargetClosest(false);
                float rotTo = QwertyMethods.PredictiveAimWithOffset(NPC.Center, shotSpeed, Main.player[NPC.target].Center, Main.player[NPC.target].velocity, 58);
                if(!float.IsNaN(rotTo))
                {
                    relativeRotation = rotTo;
                }
                else
                {  
                    relativeRotation = (Main.player[NPC.target].Center - NPC.Center).ToRotation();
                }
            }
            NPC.rotation.SlowRotation(relativeRotation + body.rotation, rotSpeed);
            if(Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[3] != -1)
            {
                switch((int)NPC.localAI[3])
                {
                    case 0:
                    //set orientation
                    if(NPC.localAI[0] != -1)
                    {
                        relativePosition = new Vector2(NPC.localAI[0], NPC.localAI[1]);
                        NPC.localAI[0] = -1;
                        NPC.localAI[1] = -1;
                    }
                    if(NPC.localAI[2] != -1)
                    {
                        if(NPC.localAI[2] == -2)
                        {
                            aimAtPlayer = true;
                        }
                        else
                        {
                            aimAtPlayer = false;
                            relativeRotation = NPC.localAI[2];
                        }
                        NPC.localAI[2] = -1;
                    }
                    break;
                    case 1:
                    //set fire
                    if(NPC.localAI[0] != -1)
                    {
                        fireType = (int)NPC.localAI[0];
                        NPC.localAI[0] = -1;
                    }
                    if(NPC.localAI[1] != -1)
                    {
                        fireTimer = (int)NPC.localAI[1];
                        NPC.localAI[1] = -1;
                    }
                    break;
                }
                NPC.localAI[3] = -1;
                NPC.netUpdate = true;
            }
		}
        public override void FindFrame(int frameHeight)
        {
            int f = 0;
            if(fireType >= 2)
            {
                f += 2;
            }
            if(fireType == 4)
            {
                f += 2;
            }

            if(fireTimer > 0)
            {
                f++;
            }
            NPC.frame.Y = frameHeight * f;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(relativePosition);
            writer.Write(relativeRotation);
            writer.Write(aimAtPlayer);
            writer.Write(fireType);
            writer.Write(fireTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            relativePosition = reader.ReadVector2();
            relativeRotation = reader.ReadSingle();
            aimAtPlayer = reader.ReadBoolean();
            fireType = reader.ReadInt32();
            fireTimer = reader.ReadInt32();
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            if(body != null)
            {
                
                Texture2D head = TextureAssets.Npc[NPC.type].Value;
                int width = head.Width;
                int height = head.Height / Main.npcFrameCount[NPC.type];
                spriteBatch.Draw(head, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(width, height) * 0.5f, NPC.scale, SpriteEffects.None, 0);
                spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/HydraHead_Glow").Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, new Vector2(width, height) * 0.5f, NPC.scale, SpriteEffects.None, 0);
            }

			return false;
		}
    }
}