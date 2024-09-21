using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.NPCs.Invader;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public partial class InvaderNoehtnap : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;

            NPCID.Sets.MPAllowedEnemies[NPC.type] = true; //For allowing use of SpawnOnPlayer in multiplayer
            
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
            
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("This wasn't the first time noehtnap teamed with 'mortals' to bully a God")
            });
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 28000;
            NPC.width = 150;
            NPC.height = 100;
            NPC.value = 100000;
            NPC.damage = 200;
            NPC.noGravity = true;
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/UngodlyDivision");
            }
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(0.75f * NPC.lifeMax * bossAdjustment);
            NPC.damage = NPC.damage / 2;
        }
        float pupilDirection = 0;
        float pupilStareOutAmount = 0;
        int teleportframe = -1;
        int timer = 0;
        Spell NDS = Spell.DistruptGravity;
        Spell currentSpell;
        int activeSpellCountdown = 0;
        int timeToTele = 30;
        int teleCounter = 0;
        bool didSpell = false;
        int splitTimer = 0;
        Vector2 teleHere;
        int finalPhaseAttackCounter = 0;
        int deathCounter = 0;
        const int startPunchin = 60;
        const int finalPunch = 60 * 4;
        const int knockoutTime = finalPunch + FinalJudgement.loadInTime + FinalJudgement.punchTime;
        
        
        bool UseDistruptSpell()
        {
            Player player = Main.player[NPC.target];
            return !(player.HasBuff(ModContent.BuffType<GravityFlipped>()) || player.HasBuff(ModContent.BuffType<Darkness>()) || player.HasBuff(ModContent.BuffType<CameraIssues>())) || NPC.ai[1] == 2;
        }
        public override bool CheckDead()
        {

            if(deathCounter > knockoutTime)
            {
                return true;
            }
            NPC.life = 1;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;
            NPC.ai[1] = 3;
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.netUpdate = true;
            }
            for(int i = 0; i < clones.Length; i++)
            {
                if(clones[i] != null)
                {
                    clones[i].ai[1] = 1;
                    clones[i] = null;
                }
            }
            SpawnGore();
            return false;
        }
        void SpawnGore()
        {
            if (Main.netMode == NetmodeID.Server) 
            {
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}
            // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
            int monocol = Mod.Find<ModGore>("InvaderNoehtnap_Monocol").Type;
            int leftGore = Mod.Find<ModGore>("InvaderNoehtnap_LeftGore").Type;
            int rightGore = Mod.Find<ModGore>("InvaderNoehtnap_RightGore").Type;
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, monocol);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center + Vector2.UnitX * -0.25f * NPC.width, -Vector2.UnitX, leftGore);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center + Vector2.UnitX * 0.25f * NPC.width, Vector2.UnitX, rightGore);
        }
        void SpawnJudgement()
        {
            if(Main.netMode == NetmodeID.MultiplayerClient) return;
            float rot = Main.rand.NextFloat() * 2f * MathF.PI;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(Judgement.spawnDist, rot), QwertyMethods.PolarVector(1, rot) * -1, ModContent.ProjectileType<Judgement>(), 0, 0);
        }
        
        public override void AI()
        {
            if(NPC.ai[1] == 3)
            {
                armBeamAttackCounter = -1;
                deathCounter++;
                NPC.velocity = Vector2.Zero;
                if(deathCounter > startPunchin && deathCounter < finalPunch)
                {
                    if(deathCounter % 30 == 0)
                    {
                        SpawnJudgement();
                    }
                    if(deathCounter % 20 == 5 && deathCounter < 2 * 60)
                    {
                        SpawnJudgement();
                    }
                    if(deathCounter % 10 == 3 && deathCounter < 3 * 60)
                    {
                        SpawnJudgement();
                    }
                }
                else if(deathCounter == finalPunch)
                {
                    if(Main.netMode == NetmodeID.MultiplayerClient) return;
                    float rot = 0;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(FinalJudgement.spawnDist, rot), QwertyMethods.PolarVector(1, rot) * -1, ModContent.ProjectileType<FinalJudgement>(), 0, 0);
                }
                if(deathCounter > knockoutTime)
                {
                    teleportframe++;
                    NPC.velocity = Vector2.UnitX * -((float)FinalJudgement.spawnDist / FinalJudgement.punchTime);
                    if(teleportframe >= 20)
                    {
                        NPC.immortal = false;
                        NPC.dontTakeDamage = false;
                        NPC.StrikeInstantKill();
                    }
                }
                return;
            }
            if((float)NPC.life / NPC.lifeMax < 0.5f && NPC.ai[1] == 0)
            {
                NPC.ai[1] = 1;
            }
            /*
            if(!NPC.AnyNPCs(ModContent.NPCType<InvaderBattleship>()) && NPC.ai[1] < 2)
            {
                NPC.ai[1] = 2;
            }
            */
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            pupilDirection = (player.Center - NPC.Center).ToRotation();
            pupilStareOutAmount = MathF.Min(1f, (player.Center - NPC.Center).Length() / 1000f);
            NPC.dontTakeDamage = false;
            timeToTele = 30;
            if(teleCounter % 3 == 2)
            {
                timeToTele = 60;
                NPC.velocity = Vector2.Zero;
            }
            finalPhaseAttackCounter = (teleCounter / 3) % 4;
            if(NPC.ai[1] == 2)
            {
                for(int i = 0; i < Main.player.Length; i++)
                {
                    Player allPlayer = Main.player[i];
                    if (allPlayer.active && !allPlayer.dead)
                    {
                        if(allPlayer.HasBuff(ModContent.BuffType<GravityFlipped>()))
                        {
                            if(allPlayer.buffTime[allPlayer.FindBuffIndex(ModContent.BuffType<GravityFlipped>())] < 2)
                            {
                                allPlayer.buffTime[allPlayer.FindBuffIndex(ModContent.BuffType<GravityFlipped>())] = 2;
                            }
                        }
                        if(allPlayer.HasBuff(ModContent.BuffType<CameraIssues>()))
                        {
                            if(allPlayer.buffTime[allPlayer.FindBuffIndex(ModContent.BuffType<CameraIssues>())] < 2)
                            {
                                allPlayer.buffTime[allPlayer.FindBuffIndex(ModContent.BuffType<CameraIssues>())] = 2;
                            }
                        }
                        if(allPlayer.HasBuff(ModContent.BuffType<Darkness>()))
                        {
                            if(allPlayer.buffTime[allPlayer.FindBuffIndex(ModContent.BuffType<Darkness>())] < 2)
                            {
                                allPlayer.buffTime[allPlayer.FindBuffIndex(ModContent.BuffType<Darkness>())] = 2;
                            }
                        }
                    }
                }
            }
            //finalPhaseAttackCounter = 2;
            if(timer > timeToTele)
            {
                NPC.dontTakeDamage = true;
                if(NPC.ai[1] == 2 && splitTimer > 0)
                {
                    splitTimer--;
                    teleCounter = 0;
                }
                else if(NPC.ai[1] == 1)
                {
                    InvaderBattleship.ClearStatusEffects();
                    teleportframe = -1;
                    NDS = Spell.DistruptGravity;
                    if(splitTimer < 140)
                    {
                        splitTimer++;
                    }
                    else
                    {
                        
                        for(int i =0; i < Main.npc.Length; i++)
                        {
                            if(Main.npc[i].active && Main.npc[i].GetGlobalNPC<InvaderNPCGeneral>().invaderNPC)
                            {
                                Main.npc[i].ai[3] = 3;
                                Main.npc[i].velocity = (NPC.Center - Main.npc[i].Center).SafeNormalize(Vector2.UnitY) * 30f;
                                Main.npc[i].damage = 0;
                                if((NPC.Center - Main.npc[i].Center).Length() < 30 || !NPC.AnyNPCs(ModContent.NPCType<InvaderBattleship>()))
                                {
                                    NPC.life += Main.npc[i].life;
                                    NPC.lifeMax += Main.npc[i].life;
                                    NPC.HealEffect(Main.npc[i].life, true);
                                    Main.npc[i].StrikeInstantKill();
                                }
                            }
                        }
                        if(!NPC.AnyNPCs(ModContent.NPCType<InvaderBattleship>()))
                        {
                            NPC.active = false;
                            return;
                        }
                    }
                }
                else if(teleportframe < 20)
                {
                    teleportframe++;
                    if(teleportframe == 16 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        encirclementCenter = player.Center;
                        Vector2 unitV = player.velocity.SafeNormalize(-Vector2.UnitY);
                        unitV.X *= 2;
                        if(teleCounter % 3 == 1 && finalPhaseAttackCounter == 2)
                        {
                            unitV.Y *= 2;
                        }
                        teleHere = player.Center + unitV * 400;
                        NPC.netUpdate = true;
                    }
                }
                else
                {
                    
                    if(!NPC.AnyNPCs(ModContent.NPCType<InvaderBattleship>()))
                    {
                        NPC.active = false;
                        return;
                    }
                    
                    
                    
                    NPC.Center = teleHere;
                    //if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.netUpdate = true;
                        if(teleCounter % 3 == 1 && finalPhaseAttackCounter == 2 && NPC.ai[1] == 2)
                        {
                            for(int i = 0; i < clones.Length; i++)
                            {
                                float rot = MathF.PI * 2f * ((i + 1) / 6f) + (NPC.Center - encirclementCenter ).ToRotation();
                                Point spawnHere = (QwertyMethods.PolarVector(800, rot)+ encirclementCenter).ToPoint();
                                clones[i] = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), spawnHere.X, spawnHere.Y, ModContent.NPCType<NoehtnapClone>())];
                                clones[i].realLife = NPC.whoAmI;
                            }
                        }
                    }
                    timer = 0;
                    teleCounter++;
                    beamSweepCount = 0;
                    armBeamAttackCounter = -1;
                    didSpell = false;
                }
            }
            else if(teleportframe > -1)
            {
                teleportframe--;
                NPC.dontTakeDamage = true;
            }
            else
            {
                if(timer > 40)
                {
                    if(NPC.ai[1] == 2 && finalPhaseAttackCounter != 3)
                    {
                        switch(finalPhaseAttackCounter)
                        {
                            case 0:
                                BeamLogic(player);
                            break;
                            case 1:
                                DualSphereLogic();
                            break;
                            case 2:
                                EncirclementLogic();
                            break;
                        }
                        
                    }
                    else
                    {
                        if(!didSpell)
                        {
                            if(UseDistruptSpell())
                            {
                                currentSpell = NDS;
                                switch(NDS)
                                {
                                    case Spell.DistruptCamera:
                                        NDS = Spell.DistruptVision;
                                    break;
                                    case Spell.DistruptVision:
                                        NDS = Spell.DistruptGravity;
                                    break;
                                    case Spell.DistruptGravity:
                                        NDS = Spell.DistruptCamera;
                                    break;
                                }
                            }
                            else
                            {
                                currentSpell = Spell.AimedShot;
                            }
                            activeSpellCountdown = NoehtnapSpells.Start(currentSpell);
                            didSpell = true;
                        }
                        if(activeSpellCountdown > 0)
                        {
                            NoehtnapSpells.UpdateSpell(NPC.GetSource_FromAI(), currentSpell, NPC.Center, activeSpellCountdown, out pupilDirection, out pupilStareOutAmount);
                            activeSpellCountdown--;
                            if(activeSpellCountdown <= 0)
                            {
                                activeSpellCountdown = 0;
                            }
                        }
                        else
                        {
                            timer++;
                        }
                    }
                }
                else
                {
                    timer++;
                }
            }
            NPC.damage = NPC.dontTakeDamage ? 0 : 200;
        }
        void DrawSplit(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, int splitModifer)
        {
            Texture2D leftSide = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_LeftHalf").Value;
            Texture2D rightSide = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_RightHalf").Value;
            int splitVert = (int)MathF.Min(10, splitModifer);
            int spiltHori = 0;
            if(splitModifer > timeToSplit && splitModifer <= 99)
            {
                leftSide = NoehtnapAnimations.leftMorphAnimation[99 - splitModifer];
                rightSide = NoehtnapAnimations.rightMorphAnimaion[99 - splitModifer];
            }
            if(splitModifer > 30)
            {
                spiltHori = 2 * (Math.Min(timeToSplit, splitModifer) - 30);
            }
            spriteBatch.Draw(leftSide, NPC.Center + new Vector2(-spiltHori, splitVert) - screenPos,
                    null, drawColor, 0,
                    leftSide.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(rightSide, NPC.Center + new Vector2(spiltHori, -splitVert) - screenPos,
                    null, drawColor, 0,
                    rightSide.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(NPC.ai[1] == 3)
            {
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap").Value;
                spriteBatch.Draw(texture, NPC.Center - screenPos,
                    new Rectangle(0, 0, texture.Width, texture.Height / 5), drawColor, 0,
                    new Vector2(texture.Width, texture.Height / 5) * 0.5f, NoehtnapSpells.scale, SpriteEffects.None, 0f);

                Vector2 pupilOffset = NoehtnapSpells.PupilPosition(pupilDirection, pupilStareOutAmount);
                Texture2D Pupil = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/Pupil").Value;
                spriteBatch.Draw(Pupil, NPC.Center - screenPos + pupilOffset,
                        Pupil.Frame(), drawColor, 0,
                        Pupil.Size() * .5f, NoehtnapSpells.scale, SpriteEffects.None, 0f);
                return false;
            }
            if(splitTimer > 0)
            {
                int splitModifer = (int)MathF.Min(70, splitTimer);
                DrawSplit(spriteBatch, screenPos, drawColor, splitModifer);
                if(splitTimer > 80)
                {

                    Texture2D cloak = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_MiniCloak").Value;
                    int voidModifier = (int)MathF.Min(60, splitTimer - 80) * 2;
                    float scale = voidModifier / 120f;
                    spriteBatch.Draw(cloak, NPC.Center - screenPos,
                        null, drawColor, 0,
                        cloak.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                }
            }
            else if(dualSphereTimer > 0 )
            {
                if(dualSphereTimer <= timeToSplit + timeToMorph)
                {
                    int splitModifer = (int)MathF.Min(timeToSplit + timeToMorph, dualSphereTimer);
                    DrawSplit(spriteBatch, screenPos, drawColor, splitModifer);
                }
            }
            else if(teleportframe != -1)
            {
                if(teleportframe < 20)
                {
                    spriteBatch.Draw(NoehtnapAnimations.teleportAnimaion[19 - teleportframe], NPC.Center - screenPos,
                        null, drawColor, 0,
                        NoehtnapAnimations.teleportAnimaion[19 - teleportframe].Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                }
            }
            else
            {
                if(armBeamAttackCounter > -1)
                {
                    Texture2D armTip = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BeamArm").Value;
                    Texture2D armSeg = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BeamArm_Segment").Value;
                    for(int i = 0; i < 4; i++)
                    {
                        float rot = (armPos[i] - NPC.Center).ToRotation();
                        for(float j = 0; j < armOutAmount; j += armSeg.Width)
                        {
                            spriteBatch.Draw(armSeg, (armPos[i] + QwertyMethods.PolarVector(-j, rot)) - screenPos,
                                null, drawColor, rot,
                                Vector2.UnitY * armSeg.Height * 0.5f, 1f, SpriteEffects.None, 0f);
                        }
                        spriteBatch.Draw(armTip, armPos[i] - screenPos,
                        null, drawColor, rot,
                        Vector2.UnitY * armTip.Height * 0.5f, 1f, SpriteEffects.None, 0f);
                    }
                }
                NoehtnapSpells.DrawBody(spriteBatch, NPC.Center - screenPos, drawColor, pupilDirection, pupilStareOutAmount, false);
            }
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(teleHere.X);
            writer.Write(teleHere.Y);
            writer.Write(timer);
            writer.Write(encirclementCenter.X);
            writer.Write(encirclementCenter.Y);
            writer.Write(activeSpellCountdown);
            writer.Write(dualSphereTimer);
            writer.Write(teleportframe);
            for(int i = 0; i < 4; i++)
            {
                writer.Write(beamIndexes[i]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            teleHere.X = reader.ReadSingle();
            teleHere.Y = reader.ReadSingle();
            timer = reader.ReadInt32();
            encirclementCenter.X = reader.ReadSingle();
            encirclementCenter.Y = reader.ReadSingle();
            activeSpellCountdown = reader.ReadInt32();
            dualSphereTimer = reader.ReadInt32();
            teleportframe = reader.ReadInt32();
            for(int i = 0; i < 4; i++)
            {
                beamIndexes[i] = reader.ReadInt32();
            }
        }
    }

}