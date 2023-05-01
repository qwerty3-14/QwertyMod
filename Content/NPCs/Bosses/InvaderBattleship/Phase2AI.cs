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
using Terraria.Audio;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Common.Fortress;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public partial class InvaderBattleship : ModNPC
    {
        int warmupTimer = 0;
        float gunDebrisTimer = -60;
        float launcherDebrisTimer = -120;
        float engineDebrisTimer = -180;
        float distressDebrisTimer = -360;
        float centerDebrisTimer = -240;
        float phase2Speed = 0.75f;
        int[] gunDebrislife = new int[2];
        int[] gunDebrislifeMax = new int[2];
        int launcherDebrislife = 0;
        int launcherDebrislifeMax = 0;
        int engineDebrislife = 0;
        int engineDebrislifeMax = 0;
        int distressDebrislife = 0;
        int distressDebrislifeMax = 0;
        int centerDebrisLife = 0;
        int centerDebrisLifeMax = 0;
        bool gunsOut = false;
        bool launcherOut = false;
        bool engineOut = false;
        bool distressOut = false;
        bool centerOut = false;
        NPC[] gunDebris = new NPC[2];
        NPC launcherDebris;
        NPC engineDebris;
        NPC distressDebris;
        NPC centerDebris;
        NPC Noehtnap;
        int engineSide = 1;
        int launcherSide = -1;
        void Phase2AI()
        {
            NPC.velocity = Vector2.Zero;
            NPC.TargetClosest(false);
            NPC.Center = Main.player[NPC.target].Center + Vector2.UnitY * -1000;


            gunDebrisTimer += phase2Speed;
            launcherDebrisTimer += phase2Speed;
            engineDebrisTimer += phase2Speed;
            distressDebrisTimer += phase2Speed;
            centerDebrisTimer += phase2Speed;
            Player player = Main.player[NPC.target];
            if(player.active && !player.dead)
            {
                if(gunDebrisTimer > 60 && !gunsOut)
                {
                    gunsOut = true;
                    SpawnGuns();
                }
                if(launcherDebrisTimer > 120 && !launcherOut)
                {
                    launcherOut = true;
                    SpawnLauncher();
                }
                if(engineDebrisTimer > 120 && !engineOut)
                {
                    engineOut = true;
                    SpawnEngine();
                }
                if(distressDebrisTimer > 60 && !distressOut)
                {
                    distressOut = true;
                    SpawnDistress();
                }
                if(centerDebrisTimer > 360 && !centerOut)
                {
                    centerOut = true;
                    SpawnCenter();
                }
                int pieceCount = (gunDebrislife[0] <= 0 ? 0 : 1) + (gunDebrislife[1] <= 0 ? 0 : 1) + (engineDebrislife <= 0 ? 0 : 1) + (launcherDebrislife <= 0 ? 0 : 1) + (centerDebrisLife <= 0 ? 0 : 1) + (distressDebrislife <= 0 ? 0 : 1);
                if(Noehtnap != null)
                {
                    if(Noehtnap.ai[1] == 3)
                    {
                        NPC.dontTakeDamage = false;
                        NPC.immortal = false;
                        NPC.Center = Noehtnap.Center;
                        NPC.StrikeInstantKill();
                        return;
                    }
                    else if(pieceCount == 0)
                    {
                        Noehtnap.ai[1] = 2;
                        Noehtnap.netUpdate = true;
                    }
                    else if(pieceCount < 4 && Noehtnap.ai[1] == 0)
                    {
                        Noehtnap.ai[1] = 1;
                        Noehtnap.netUpdate = true;
                    }
                    
                }
                else if(pieceCount < 6 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Noehtnap = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 200, ModContent.NPCType<InvaderNoehtnap>())];
                }
            }
            else if(!gunsOut && !launcherOut && !engineOut && !distressOut && !centerOut)
            {
                NPC.active = false;
            }
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                for(int i =0; i < 2; i++)
                {
                    if(gunDebris[i] != null && (((gunDebris[i].ai[1] == 2 && (Main.player[NPC.target].Center - gunDebris[i].Center).Length() > 1200)) || gunDebris[i].life <= 0 || !gunDebris[i].active))
                    {
                        ClearPiece(ref gunDebris[i], ref gunDebrislife[i]);
                    }
                }

                if(launcherDebris != null && (((launcherDebris.ai[1] == 2)) || launcherDebris.life <= 0 || !launcherDebris.active))
                {
                    ClearPiece(ref launcherDebris, ref launcherDebrislife);
                }
                if(engineDebris != null && (((engineDebris.ai[1] == 2)) || engineDebris.life <= 0 || !engineDebris.active))
                {
                    ClearPiece(ref engineDebris, ref engineDebrislife);
                }
                if(distressDebris != null && (((distressDebris.ai[1] == 2)) || distressDebris.life <= 0 || !distressDebris.active))
                {
                    ClearPiece(ref distressDebris, ref distressDebrislife);
                }
                if(centerDebris != null && (((centerDebris.ai[1] == 2)) || centerDebris.life <= 0 || !centerDebris.active))
                {
                    ClearPiece(ref centerDebris, ref centerDebrisLife);
                }

                if(gunDebris[0] == null && gunDebris[1] == null && gunsOut)
                {
                    gunsOut = false;
                    gunDebrisTimer = 0;
                }

                if(launcherDebris == null && launcherOut)
                {
                    launcherOut = false;
                    launcherDebrisTimer = 0;
                }
                if(engineDebris == null && engineOut)
                {
                    engineOut = false;
                    engineDebrisTimer = 0;
                }
                if(distressDebris == null && distressOut)
                {
                    distressOut = false;
                    distressDebrisTimer = 0;
                }
                if(centerDebris == null && centerOut)
                {
                    centerOut = false;
                    centerDebrisTimer = 0;
                }
            }
            
        }
        void ClearPiece(ref NPC piece, ref int life)
        {
            life = piece.life;
            piece.StrikeInstantKill();
            piece = null;
            if(life < 0)
            {
                life = 0;
            }
            NPC.netUpdate = true;
        }
        void SpawnGuns()
        {
            Player player = Main.player[NPC.target];
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                for(int i = 0; i < 2; i++)
                {
                    Vector2 spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors = player.Center + Vector2.UnitX * 4000 * (i == 1 ? -1 : 1);
                    int spawnType = ModContent.NPCType<FrontGunPiece>();
                    if(i == 1)
                    {
                        spawnType = ModContent.NPCType<BackGunPiece>();
                    }
                    gunDebris[i] = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.X, (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.Y, spawnType)];
                    
                    gunDebris[i].lifeMax = gunDebrislifeMax[i];
                    gunDebris[i].life = gunDebrislife[i];
                    gunDebris[i].netUpdate = true;
                }
            }
        }
        void SpawnLauncher()
        {
            Player player = Main.player[NPC.target];
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors = player.Center + QwertyMethods.PolarVector(1400, (launcherSide == -1 ? 3f : 1f) * MathF.PI / 4f);
                int spawnType = ModContent.NPCType<MissileLauncherPiece>(); 
                launcherDebris = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.X, (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.Y, spawnType)];
                launcherDebris.lifeMax = launcherDebrislifeMax;
                launcherDebris.life = launcherDebrislife;
                launcherSide *= -1;
                launcherDebris.netUpdate = true;
            }
        }
        void SpawnEngine()
        {
            Player player = Main.player[NPC.target];
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors = player.Center + Vector2.UnitX * 1400 * engineSide;
                int spawnType = ModContent.NPCType<EnginePiece>(); 
                engineDebris = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.X, (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.Y, spawnType)];
                engineDebris.lifeMax = engineDebrislifeMax;
                engineDebris.life = engineDebrislife;
                engineSide *= -1;
                engineDebris.netUpdate = true;
            }

        }
        void SpawnDistress()
        {
            Player player = Main.player[NPC.target];
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors = player.Center + Vector2.UnitY * -1000;
                int spawnType = ModContent.NPCType<DistressPiece>(); 
                distressDebris = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.X, (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.Y, spawnType)];
                distressDebris.lifeMax = distressDebrislifeMax;
                distressDebris.life = distressDebrislife;
                distressDebris.netUpdate = true;
            }
        }
        void SpawnCenter()
        {
            Player player = Main.player[NPC.target];
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors = player.Center + Vector2.UnitY * 1000;
                int spawnType = ModContent.NPCType<CenterPiece>(); 
                centerDebris = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.X, (int)spawnLocBecauseStupidNewNPCMethodDoesntTakeVectors.Y, spawnType)];
                centerDebris.lifeMax = centerDebrisLifeMax;
                centerDebris.life = centerDebrisLife;
                centerDebris.netUpdate = true;
            }
        }
        void ApplyPieceDifficultyScaling(int numPlayers, float balance, float bossAdjustment)
        {
            float expertScale = 1.5f;
            float masterScale = 1.5f;
            for(int i = 0; i < 2; i++)
            {
                gunDebrislife[i] = gunDebrislifeMax[i] = 14000;
                if(Main.expertMode)
                {
                    gunDebrislifeMax[i] = (int)(gunDebrislifeMax[i] * expertScale * bossAdjustment);
                    gunDebrislife[i] = (int)(gunDebrislife[i] * expertScale * bossAdjustment);
                }
                if(Main.masterMode)
                {
                    gunDebrislifeMax[i] = (int)(gunDebrislifeMax[i] * masterScale);
                    gunDebrislife[i] = (int)(gunDebrislife[i] * masterScale);
                }
            }
            launcherDebrislife = launcherDebrislifeMax = 8000;
            engineDebrislife = engineDebrislifeMax = 10000;
            distressDebrislife = distressDebrislifeMax = 4000;
            centerDebrisLife = centerDebrisLifeMax = 20000;
            if(Main.expertMode)
            {
                launcherDebrislife = (int)(launcherDebrislife * expertScale * bossAdjustment);
                launcherDebrislifeMax = (int)(launcherDebrislifeMax * expertScale * bossAdjustment);
                engineDebrislife = (int)(engineDebrislife * expertScale * bossAdjustment);
                engineDebrislifeMax = (int)(engineDebrislifeMax * expertScale * bossAdjustment);
                distressDebrislife = (int)(distressDebrislife * expertScale * bossAdjustment);
                distressDebrislifeMax = (int)(distressDebrislifeMax * expertScale * bossAdjustment);
                centerDebrisLife = (int)(centerDebrisLife * expertScale * bossAdjustment);
                centerDebrisLifeMax = (int)(centerDebrisLifeMax * expertScale * bossAdjustment);
            }
            if(Main.masterMode)
            {
                launcherDebrislife = (int)(launcherDebrislife * masterScale);
                launcherDebrislifeMax = (int)(launcherDebrislifeMax * masterScale);
                engineDebrislife = (int)(engineDebrislife * masterScale);
                engineDebrislifeMax = (int)(engineDebrislifeMax * masterScale);
                distressDebrislife = (int)(distressDebrislife * masterScale);
                distressDebrislifeMax = (int)(distressDebrislifeMax * masterScale);
                centerDebrisLife = (int)(centerDebrisLife * masterScale);
                centerDebrisLifeMax = (int)(centerDebrisLifeMax * masterScale);
            }
        }
    }
}