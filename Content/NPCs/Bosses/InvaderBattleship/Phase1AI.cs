using Microsoft.Xna.Framework;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Buffs;
using System;
using System.Linq; 
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;



namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public partial class InvaderBattleship : ModNPC
    {
        public bool doTransition = false;
        int strafeTimer = 0;
        int engineTimer = -1;
        float gunTimer = 0;
        float playerXDistanceThreshold = 900;
        float combatSpeed = 1f;
        BattleshipTurret[] turrets = new BattleshipTurret[2];
        BattleshipMissileLauncher launcher;
        int baseShotCooldown = 240;
        bool useBeams = false;
        bool useMissile = false;
        int centerEyeX = 137;
        float beamEmitterOutAmount = 0;
        bool openDoor = false;
        float garageDropAmount = 1f;
        float garageDoorDropAcc = 0.01f;
        float garageDoorVelocity = 0f;
        Spell spell = Spell.DistruptGravity;
        int activeSpellCountdown = 0;
        float pupilDirection;
        float pupilStareOutAmount;
        bool didSpell = false;
        bool desperation = false;
        void Phase1AI()
        {
            if(doTransition)
            {
                NPC.velocity *= 0.92f;
                strafeTimer++;
                UpdateLauncher(0);
                turrets[0].UpdateRelativePosition();
                turrets[1].UpdateRelativePosition();
                if(strafeTimer > 180)
                {
                    ClearStatusEffects();
                    Explode();
                }
            }
            else
            {
                if(runOnce)
                {
                    //QwertyMethods.ServerClientCheck("" + strafeTimer);
                    IntroLogc();
                }
                else if(turrets[0] != null && turrets[1] != null)
                {
                    turrets[0].UpdateRelativePosition();
                    turrets[1].UpdateRelativePosition();
                    NPC.TargetClosest(false);
                    if(strafeTimer == 4)
                    {
                        Warp();
                    }
                    strafeTimer++;
                    float relXPos = (NPC.Center.X - Main.player[NPC.target].Center.X) * NPC.direction;
                    combatSpeed = 2f - ((float)NPC.life / NPC.lifeMax);
                    float combatSpeedDistModifer = 1f + (((Main.player[NPC.target].Center - NPC.Center).Length() - 400) / 800f);
                    if(combatSpeedDistModifer < 0.9f)
                    {
                        combatSpeedDistModifer = 0.9f;
                    }
                    combatSpeed *= combatSpeedDistModifer;
                    if(relXPos > playerXDistanceThreshold || strafeTimer > 12 * 60)
                    {
                        StartEngine();
                    }
                    if(engineTimer != -1)
                    {
                        ChargeEngine();
                        turrets[0].AimHome();
                        turrets[1].AimHome();
                    }
                    else
                    {
                        NPC.velocity = NPC.direction * 5 * combatSpeed * Vector2.UnitX;
                        gunTimer += combatSpeed;
                        for(int i = 0; i < 2; i++)
                        {
                            Player aimAt = Main.player[NPC.target];
                            if(Main.netMode != NetmodeID.SinglePlayer && i == 1)
                            {
                                float maxDist = 2000;
                                for(int p = 0; p < Main.player.Length; p++)
                                {
                                    if(p != NPC.target && Main.player[p].active)
                                    {
                                        float dist = (Main.player[p].Center - NPC.Center).Length() - Main.player[p].aggro;
                                        if(dist < maxDist)
                                        {
                                            maxDist = dist;
                                            aimAt = Main.player[p];
                                        }
                                    }
                                }
                            }
                            float shootAngle = QwertyMethods.PredictiveAimWithOffset(turrets[i].AbsolutePosition(), 4.5f * 3, aimAt.Center, aimAt.velocity, 9);
                            if(float.NaN != shootAngle)
                            {
                                turrets[i].AimAt(shootAngle);
                            }
                            else
                            {
                                turrets[i].AimHome();
                            }
                            int turretLightCount = (int)gunTimer / (baseShotCooldown / 6);
                            turrets[i].SetLights(turretLightCount);
                            if(gunTimer > baseShotCooldown)
                            {
                                turrets[i].Fire();
                            }
                        }
                        if(gunTimer > baseShotCooldown)
                        {
                            gunTimer -= baseShotCooldown;
                        }
                    }
                    if(relXPos > playerXDistanceThreshold * 2 && strafeTimer >  60)
                    {
                        CheckPhaseProgress();
                    }
                    UpdateLauncher(relXPos);
                }
                
            }
            UpdateBeams();
            UpdateDistress();
            ProcessDoorLogic();
            ManageSpells();
        }
        void ManageSpells()
        {
            if(garageDropAmount == 0 && !didSpell)
            {
                activeSpellCountdown = NoehtnapSpells.Start(spell);
                switch(spell)
                {
                    case Spell.DistruptCamera:
                        spell = Spell.DistruptVision;
                    break;
                    case Spell.DistruptVision:
                        spell = Spell.DistruptGravity;
                    break;
                    case Spell.DistruptGravity:
                        spell = Spell.DistruptCamera;
                    break;
                }
                didSpell = true;
            }
            if(activeSpellCountdown > 0)
            {
                NoehtnapSpells.UpdateSpell(NPC.GetSource_FromAI(), spell, NPC.position + new Vector2(NPC.direction == 1 ? centerEyeX : NPC.width - centerEyeX, 55), activeSpellCountdown, out pupilDirection, out pupilStareOutAmount);
                activeSpellCountdown--;
                if(activeSpellCountdown <= 0)
                {
                    activeSpellCountdown = 0;
                    openDoor = false;
                }
            }
        }
        public static void ClearStatusEffects()
        {

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player allPlayer = Main.player[i];
                if (allPlayer.active && !allPlayer.dead)
                {
                    allPlayer.ClearBuff(ModContent.BuffType<CameraIssues>());
                    allPlayer.ClearBuff(ModContent.BuffType<Darkness>());
                    allPlayer.ClearBuff(ModContent.BuffType<GravityFlipped>());
                }
            }
        }
        void ProcessDoorLogic()
        {
            if(!openDoor)
            {
                garageDoorVelocity += garageDoorDropAcc;
            }
            else
            {
                garageDoorVelocity = -2f / 60;
            }
            garageDropAmount += garageDoorVelocity;
            if(garageDropAmount > 1f)
            {
                garageDropAmount = 1f;
                garageDoorVelocity *= -0.4f;
            }
            if(garageDropAmount < 0f)
            {
                garageDropAmount = 0;
            }
        }

        Vector2[] ExhaustOffsets = new Vector2[] 
        {
            new Vector2(1, 13),
            new Vector2(5, 34),
            new Vector2(9, 54),
            new Vector2(13, 74)
        };
        int[] beamIndexes = new int[] {-1, -1};
        void UpdateBeams()
        {
            if(useBeams)
            {
                if(beamEmitterOutAmount < 1f)
                {
                    beamEmitterOutAmount += 1f / 30f;
                }
                else
                {
                    
                    for(int i = 0; i < 2; i++)
                    {
                        
                        float emmiterX =  NPC.position.X + (NPC.direction == 1 ? centerEyeX : (NPC.width - centerEyeX));
                        Vector2 beamPosition = new Vector2(emmiterX, i == 0 ? (NPC.position.Y - 18) : (NPC.Bottom.Y + 18)) + NPC.velocity;
                        if(beamIndexes[i] == -1 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            beamIndexes[i] = Projectile.NewProjectile(NPC.GetSource_FromAI(), beamPosition, Vector2.UnitY * (i == 0 ? -1 : 1), ModContent.ProjectileType<FlybyBeam>(), 2 * (Main.expertMode ? expertDamage : normalDamage), 0);
                            beamIndexes[i] = Main.projectile[beamIndexes[i]].identity;
                        }
                        if(beamIndexes[i] != -1)
                        {
                            Projectile beam = Main.projectile.FirstOrDefault(x => x.identity == beamIndexes[i]);
                            if(beam != null && beam.timeLeft <= FlybyBeam.openTime)
                            {
                                beam.timeLeft = FlybyBeam.openTime;
                            }
                        }
                        
                        
                    }
                    
                }
            }
            else
            {
                if(beamEmitterOutAmount > 0f && beamIndexes[0] == -1 && beamIndexes[1] == -1)
                {
                    beamEmitterOutAmount -= 1f / 30f;
                }
            }
            for(int i = 0; i < 2; i++)
            {
                
                float emmiterX =  NPC.position.X + (NPC.direction == 1 ? centerEyeX : (NPC.width - centerEyeX));
                Vector2 beamPosition = new Vector2(emmiterX, i == 0 ? (NPC.position.Y - 18) : (NPC.Bottom.Y + 18)) + NPC.velocity;
                
                if(beamIndexes[i] != -1)
                {
                    Projectile beam = Main.projectile.FirstOrDefault(x => x.identity == beamIndexes[i]);
                    if(beam != null)
                    {
                        if((!beam.active || beam.type != ModContent.ProjectileType<FlybyBeam>()))
                        {
                            beamIndexes[i] = -1;
                        }
                        else
                        {
                            beam.Center = beamPosition;
                        }
                    }
                }
                
            }

        }
        int distressFrame = 0;
        int distressCounter = 0;
        float distressRotation = -MathF.PI / 2f;
        bool doDistress = false;
        void UpdateDistress()
        {
            if(doDistress && distressCounter < 120)
            {
                if(distressRotation < 0f)
                {
                    distressRotation += MathF.PI / 90f;
                }
                else
                {
                    distressCounter++;
                    if(distressCounter == 1)
                    {
                        SoundEngine.PlaySound(SoundID.Item6, NPC.Center);
                        for(int i =0; i < 2; i++)
                        {
                            Vector2 spawnHere = Main.player[NPC.target].Center + QwertyMethods.PolarVector(Main.rand.NextFloat() * 700, Main.rand.NextFloat() * MathF.PI * 2f);
                            NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnHere.X, (int)spawnHere.Y, ModContent.NPCType<Invader.InvaderCaster>());
                        }
                    }
                    distressFrame = ((distressCounter / 6) % 4) + 1;

                }
            }
            else
            {
                distressFrame = 0;
                if(distressRotation > -MathF.PI / 2f)
                {
                    distressRotation -= MathF.PI / 90f;
                }
            }
        }
        void UpdateLauncher(float relXPos)
        {
            if(launcher != null)
            {
                launcher.UpdateRelativePosition();
                if(useMissile && !launcher.HasFired())
                {
                    if(launcher.AimRelative(MathF.PI / 4f) && ((Collision.CanHitLine(launcher.AbsoluteShootPosition(), 1, 1, Main.player[NPC.target].Center, 1, 1) && relXPos > -800) || engineTimer != -1))
                    {
                        launcher.Fire();
                    }
                }
                else
                {
                    if(launcher.AimHome() && useMissile)
                    {
                        launcher.Reload();
                    }
                }
            }
        }
        void CheckPhaseProgress()
        {
            NPC.TargetClosest(false);
            if (!Main.player[NPC.target].InModBiome(ModContent.GetInstance<FortressBiome>()) || !Main.player[NPC.target].active || Main.player[NPC.target].dead)
            {
                NPC.active = false;
                return;
            }
            desperation = false;
            if((float)NPC.life / NPC.lifeMax  < 0.9f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                useBeams = true;
            }
            if((float)NPC.life / NPC.lifeMax  < 0.7f)
            {
                if(NPC.direction == 1)
                {
                    doDistress = true;
                    useMissile = false;
                }
                else
                {
                    doDistress = false;
                    useMissile = true;
                }
            }
            if((float)NPC.life / NPC.lifeMax  < 0.5f)
            {
                openDoor = true;
            }
            if((float)NPC.life / NPC.lifeMax  < 0.3f)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if(Main.rand.NextBool(2))
                    {
                        useBeams = false;
                        desperation = true;
                        doDistress = true;
                        useMissile = false;
                    }
                    NPC.netUpdate = true;
                }
            }
            strafeTimer = 0;
            engineTimer = -1;
            distressCounter = 0;
            didSpell = false;
            if(launcher != null)
            {
                launcher.Reload();
            }
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.netUpdate = true;
            }
            ClearStatusEffects();
            NPC.direction *= -1;
        }
        void Warp()
        {
            if(desperation)
            {
                StartEngine();
            }
            SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_warp"), NPC.Center);
            NPC.TargetClosest(false);
            Vector2 warpTo = Main.player[NPC.target].Center + new Vector2(NPC.direction * -1500, desperation ? 0 : -300f * NPC.direction);
            NPC.Center = warpTo;
            NPC.velocity = NPC.direction * 5 * combatSpeed * Vector2.UnitX;
            
        }
        void StartEngine()
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                useBeams = false;
                NPC.netUpdate = true;
            }
            if(engineTimer == -1)
            {
                engineTimer = 0;
            }
            openDoor = false;
        }
        void ChargeEngine()
        {
            engineTimer++;
            if(engineTimer % 5 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for(int i =0; i < 4; i++)
                {
                    Vector2 offset = ExhaustOffsets[i];
                    if(NPC.direction == -1)
                    {
                        offset.X = NPC.width - offset.X;
                    }
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + offset, Vector2.Zero, ModContent.ProjectileType<InvaderExhaust>(), 0, 0);
                }
            }
            if(engineTimer > 120)
            {
                NPC.velocity = NPC.direction * 40 * Vector2.UnitX;
            }
        }
        bool runOnce = true;
        void IntroLogc()
        {
            NPC.direction = -1;
            runOnce = false;
            turrets[0] = new BattleshipTurret(NPC, new Vector2(60, 39) - NPC.Size / 2f);
            turrets[1] = new BattleshipTurret(NPC, new Vector2(221, 39) - NPC.Size / 2f);
            launcher = new BattleshipMissileLauncher(NPC, new Vector2(79, 86) - NPC.Size / 2f);
            Warp();
        }
        void Explode()
        {
            doTransition = false;
            phaseTwo = true;
            SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                float shotDir = -7f * MathF.PI/8f;
                if(NPC.direction == -1)
                {
                    shotDir = MathF.PI - shotDir;
                }
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2( NPC.direction == 1 ? 41 : (NPC.width - 41), 36), QwertyMethods.PolarVector(6f, shotDir), ModContent.ProjectileType<BattleshipDebris_Engine>(), 0, 0);

                shotDir = -3f * MathF.PI/4f;
                if(NPC.direction == -1)
                {
                    shotDir = MathF.PI - shotDir;
                }
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2( NPC.direction == 1 ? 41 : (NPC.width - 41), 36), QwertyMethods.PolarVector(6f, shotDir), ModContent.ProjectileType<BattleshipDebris_BackWithGun>(), 0, 0);

                shotDir = 17f * MathF.PI/32f;
                if(NPC.direction == -1)
                {
                    shotDir = MathF.PI - shotDir;
                }
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2( NPC.direction == 1 ? 136 : (NPC.width - 136), 53), QwertyMethods.PolarVector(4f, shotDir), ModContent.ProjectileType<BattleshipDebris_Center>(), 0, 0);

                shotDir = 15f * MathF.PI/32f;
                if(NPC.direction == -1)
                {
                    shotDir = MathF.PI - shotDir;
                }
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2( NPC.direction == 1 ? 73 : (NPC.width - 73), 82), QwertyMethods.PolarVector(4f, shotDir), ModContent.ProjectileType<BattleshipDebris_Launcher>(), 0, 0);

                shotDir = 0f;
                if(NPC.direction == -1)
                {
                    shotDir = MathF.PI - shotDir;
                }
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2( NPC.direction == 1 ? 223 : (NPC.width - 223), 40), QwertyMethods.PolarVector(5f, shotDir), ModContent.ProjectileType<BattleshipDebris_FrontWithGun>(), 0, 0);
            }
        }
    }
}