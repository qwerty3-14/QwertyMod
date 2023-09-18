using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Hydra;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Tool.FishingRod;
using QwertyMod.Content.Items.Tool.Mining;
using QwertyMod.Content.Items.Weapon.Magic.HydraBeam;
using QwertyMod.Content.Items.Weapon.Magic.HydraMissile;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Hydra;
using QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrent;
using QwertyMod.Content.Items.Weapon.Minion.HydraHead;
using QwertyMod.Content.Items.Weapon.Morphs.HydraBarrage;
using QwertyMod.Content.Items.Weapon.Ranged.Gun;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using System.IO;
using Terraria.Audio;

namespace QwertyMod.Content.NPCs.Bosses.Hydra
{
    public class Hydra : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.MPAllowedEnemies[NPC.type] = true; //For allowing use of SpawnOnPlayer in multiplayer
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 10;
            NPC.defense = 18;
            NPC.boss = true;
            NPC.value = 60f;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lifeMax = 12;
            NPC.dontTakeDamage = true;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.hide = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/BeastOfThreeHeads");
            }
        }
        
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossAdjustment);
            NPC.damage = (int)(NPC.damage * 0.75f);
        }
        

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
				new FlavorTextBestiaryInfoElement("It's a Hydra!")
            });
        }
        

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter < 5)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
            else if (NPC.frameCounter < 10)
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else if (NPC.frameCounter < 15)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.frameCounter < 20)
            {
                NPC.frame.Y = 3 * frameHeight;
            }
            else if (NPC.frameCounter < 25)
            {
                NPC.frame.Y = 4 * frameHeight;
            }
            else
            {
                NPC.frameCounter = 0;
            }
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ItemType<HydraBag>())); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<HydraBarrage>(), ItemType<HydraBeam>(), ItemType<HydraCannon>(), ItemType<HydraHeadStaff>(), ItemType<HydraJavelin>(), ItemType<Hydrent>(), ItemType<Hydrill>(), ItemType<HydraMissileStaff>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);

            //Boss masks are spawned with 1/7 chance
            //notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            //notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<PolarMask>(), 7));
            //npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<Hydrator>(), 7));
            npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<HydraScale>(), 1, 20, 30));
            npcLoot.Add(notExpertRule);

            //Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ItemType<HydraTrophy>(), 10));

            

			// ItemDropRule.MasterModeCommonDrop for the relic
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Consumable.Tiles.Relics.HydraRelic>()));

            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedHydra, -1);
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            Texture2D body = TextureAssets.Npc[NPC.type].Value;
            int width = body.Width;
            int height = body.Height / Main.npcFrameCount[NPC.type];
            spriteBatch.Draw(body, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(width, height) * 0.5f, NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/Hydra_Glow").Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, new Vector2(width, height) * 0.5f, NPC.scale, SpriteEffects.None, 0);
            if(attackCounter >= 3)
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/HydraHeart").Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, new Vector2(width, height) * 0.5f, NPC.scale, SpriteEffects.None, 0);
            }
            for(int i =0; i < 200; i++)
            {
                if(Main.npc[i].type == ModContent.NPCType<HydraHead>())
                {
                    NPC head = Main.npc[i];
                    if(head.active)
                    {
                        float neckDir = ((head.Center + head.netOffset) - NeckPos()).ToRotation();
                        Texture2D neckSeg = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/HydraNeck").Value;
                        width = neckSeg.Width;
                        height = neckSeg.Height;
                        for(float l =6; l < ((head.Center + head.netOffset) - NeckPos()).Length(); l += height)
                        {
                            spriteBatch.Draw(neckSeg, NeckPos() + QwertyMethods.PolarVector(l, neckDir) - screenPos, null, drawColor, neckDir + MathF.PI / 2f, new Vector2(width, height) * 0.5f, NPC.scale, SpriteEffects.None, 0);
                        }
                        Texture2D neckBase = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/HydraNeckBase").Value;
                        width = neckBase.Width;
                        height = neckBase.Height;
                        spriteBatch.Draw(neckBase, NeckPos() - screenPos, null, drawColor, neckDir + MathF.PI / 2f, new Vector2(width, height) * 0.5f, NPC.scale, SpriteEffects.None, 0);
                    }
                }
            }
			return false;
		}
        Vector2 NeckPos()
        {
            return NPC.Center + QwertyMethods.PolarVector(50, MathF.PI * -0.5f + NPC.rotation);
        }
        void SetHeadPos(NPC npc, Vector2 offset, float rotation)
        {
            npc.localAI[0] = offset.X;
            npc.localAI[1] = offset.Y;
            npc.localAI[2] = rotation;
            npc.localAI[3] = 0;
        }
        void SetFire(NPC npc, int type, int cooldown)
        {
            npc.localAI[0] = type;
            npc.localAI[1] = cooldown;
            npc.localAI[3] = 1;
        }
		public override void DrawBehind(int index)
		{
            Main.instance.DrawCacheNPCsBehindNonSolidTiles.Add(index);
		}
        bool runOnce = true;
        List<NPC> activeHeads = new List<NPC>();
        int timer = 0;
        int attackCounter = 0;
        Vector2 moveTo;
        const int barrageTimeFinal = 60 * 5 + barrageStart;
        const int barrageStart = 270;
        const int headReturnAfterTime = 45;
        int barrageEnd = barrageTimeFinal - headReturnAfterTime - 10;
        const int swingHeadStart = 60 * 3;
        public const int swingHeadTime = 180;
        const int fireWaveStart = 60;
        const int fireWaveTime = 30;
        const int chargeAndExtendStart = 60;
        const int switchFromChargeToExtend = 180;
        const int chargeAndExtendFinalTime = chargeAndExtendStart + switchFromChargeToExtend + 120;
        int headKillCount = 0;
        int maxHeads = 30;
        public override void AI()
        {
            if(runOnce)
            {
                activeHeads = new List<NPC>();
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for(int i = 0; i < 3; i++)
                    {
                        activeHeads.Add(Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HydraHead>())]);
                    }
                }
                runOnce = false;
            }
            NPC.dontTakeDamage = attackCounter < 3;
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, -20f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            timer++;
            if(timer == 1)
            {
                int headsToAdd = 0;
                for(int i =0; i < activeHeads.Count; i++)
                {
                    if(!activeHeads[i].active)
                    {
                        headsToAdd += 2;
                        headKillCount++;
                        activeHeads.RemoveAt(i);
                        i--;
                    }
                }
                headsToAdd = Math.Min(headsToAdd, maxHeads - activeHeads.Count);
                for(int i = 0; i < headsToAdd; i++)
                {
                    activeHeads.Add(Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HydraHead>())]);
                }
                for(int i =0; i < activeHeads.Count; i++)
                {
                    SetHeadPos(activeHeads[i],  QwertyMethods.PolarVector(200, ((float)i / (activeHeads.Count - 1)) * MathF.PI * 0.75f - MathF.PI * 0.75f * 0.5f + MathF.PI * -0.5f), MathF.PI / 2f);
                }
                if(headKillCount >= 18 && attackCounter < 3)
                {
                    attackCounter = 3;
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.netUpdate = true;
                    }
                }
            }
            switch(attackCounter)
            {
                case 0:
                case 3:
                    if(timer < barrageStart - 60)
                    {
                        moveTo = player.Center + new Vector2(0, -200);
                        if(attackCounter == 3)
                        {
                            moveTo = player.Center + new Vector2(MathF.Sign(player.Center.X - NPC.Center.X) * -600, 400);
                        }
                    }
                    else if(timer < barrageStart)
                    {
                        
                    }
                    else if (timer < barrageTimeFinal)
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if(timer == barrageStart)
                            {
                                NPC.netUpdate = true;
                            }
                            int barrageDuration = barrageEnd - barrageStart;
                            int interval = barrageDuration / activeHeads.Count;
                            if((timer - barrageStart) % interval == 0)
                            {
                                int index = (timer - barrageStart) / interval;
                                if(index < activeHeads.Count)
                                {
                                    SetHeadPos(activeHeads[index], (player.Center - NeckPos()).SafeNormalize(-Vector2.UnitY) * 300, -2);
                                }
                            }
                            if((timer - barrageStart) % interval == 1)
                            {
                                int index = (timer - barrageStart) / interval;
                                if(index < activeHeads.Count)
                                {
                                    SetFire(activeHeads[index], 1, 20);
                                }
                            }
                            
                            if((timer - barrageStart - headReturnAfterTime) >= 0 && (timer - barrageStart - headReturnAfterTime) % interval == 0)
                            {
                                int index = (timer - barrageStart - headReturnAfterTime) / interval;
                                if(index < activeHeads.Count)
                                {
                                    SetHeadPos(activeHeads[index],  QwertyMethods.PolarVector(200, ((float)index / (activeHeads.Count - 1)) * MathF.PI * 0.75f - MathF.PI * 0.75f * 0.5f + MathF.PI * -0.5f), MathF.PI / 2f);
                                }
                            }
                        }
                    }
                    if(timer > barrageTimeFinal)
                    {
                        timer = 0;
                        attackCounter++;
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.netUpdate = true;
                        }
                    }
                break;
                case 1:
                case 4:
                    if(timer < swingHeadStart)
                    {
                        moveTo = player.Center + new Vector2(MathF.Sign(player.Center.X - NPC.Center.X) * -300, 40);
                    }
                    if(timer == swingHeadStart - 61 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.netUpdate = true;
                    }
                    if(timer == swingHeadStart - 60)
                    {
                        float playerDist = (player.Center - NeckPos()).Length();
                        float headSpacing = 110;
                        float startDist = Math.Max(playerDist - (headSpacing * (activeHeads.Count - 1) / 2f), 0);
                        for(int i = 0; i < activeHeads.Count; i++)
                        {
                            float rot = (NeckPos() - player.Center).ToRotation() + (i % 2 == 0 ? 1 : -1) * MathF.PI / 120f * i;
                            SetHeadPos(activeHeads[i], QwertyMethods.PolarVector(startDist + i * headSpacing, rot), rot + (i % 2 == 0 ? 1 : -1) * MathF.PI / 2f);
                        }
                    }

                    if(timer == swingHeadStart - 1 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.netUpdate = true;
                    }

                    if(timer == swingHeadStart)
                    {
                        for(int i = 0; i < activeHeads.Count; i++)
                        {
                            SetFire(activeHeads[i], (i % 2 == 0 ? 2 : 3), swingHeadTime * (attackCounter == 4 ? 2 : 1));
                        }
                    }
                    if((timer > swingHeadStart + swingHeadTime * 2  && attackCounter == 4) || (timer > swingHeadStart + swingHeadTime && attackCounter == 1))
                    {
                        timer = 0;
                        attackCounter++;
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.netUpdate = true;
                        }
                    }
                break;
                case 2:
                case 5:
                if(timer == 4 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.netUpdate = true;
                }
                if(timer == 5)
                {
                    float headSpacing = (attackCounter == 5 ? 40 : 180);
                    float headStart = ((activeHeads.Count - 1) * headSpacing) / -2f;
                    for(int i = 0; i < activeHeads.Count; i++)
                    {
                        if(attackCounter == 5)
                        {
                            SetHeadPos(activeHeads[i], new Vector2(MathF.Sign(player.Center.X - NPC.Center.X) * 200, headStart + i * headSpacing), MathF.PI / 2f + MathF.Sign(player.Center.X - NPC.Center.X) * MathF.PI / -2f);
                        }
                        else
                        {
                            SetHeadPos(activeHeads[i], new Vector2(headStart + i * headSpacing, 100), MathF.PI / 2f);
                        }
                    }
                    
                }
                if(timer < fireWaveStart)
                {
                    
                    if(attackCounter == 5)
                    {
                        moveTo = player.Center + new Vector2(MathF.Sign(player.Center.X - NPC.Center.X) * -900, 40);
                    }
                    else
                    {
                        moveTo = player.Center + new Vector2(0, -500);
                    }
                }
                if(timer == fireWaveStart)
                {
                    for(int i = 0; i < activeHeads.Count; i++)
                    {
                        SetFire(activeHeads[i], 4, fireWaveTime);
                    }

                }
                if(timer > fireWaveStart + fireWaveTime)
                {
                    timer = 0;
                    attackCounter = (attackCounter == 5 ? 3 : 0);
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.netUpdate = true;
                    }
                }
                break;
            }
            
            NPC.damage = 0;
            NPC.velocity = (moveTo - NPC.Center) * 0.06f;
            NPC.rotation = NPC.velocity.X * 0.04f;
            if(NPC.rotation > MathF.PI / 4f)
            {
                NPC.rotation = MathF.PI / 4f;
            }
            if(NPC.rotation < -MathF.PI / 4f)
            {
                NPC.rotation = -MathF.PI / 4f;
            }
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                bool reset = true;
                for(int i =0; i < activeHeads.Count; i++)
                {
                    if(activeHeads[i].active)
                    {
                        reset = false;
                        break;
                    }
                }
                if(reset)
                {
                    timer = 0;
                    NPC.netUpdate = true;
                }
            }
        }
        
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(attackCounter);
            writer.Write(headKillCount);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
            attackCounter = reader.ReadInt32();
            headKillCount = reader.ReadInt32();
        }

    }
}