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
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using QwertyMod.Content.Buffs;

namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class InvaderBattleship : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Battleship");
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 100000;
            NPC.width = 176;
            NPC.height = 66;
            NPC.value = 100000;
            NPC.damage = 200;
            NPC.noGravity = true;
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/BuiltToDestroy");
            }
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
        }
        bool OpenDoor = false;
        bool ArmMissiles = false;
        bool missileReady = false;
        float garageDropAmount = 1f;
        float garageDoorDropAcc = 0.01f;
        float garageDoorVelocity = 0f;
        float lowerDoorShutAmount = 1f;
        float lowerDoorShutAcc = 0.01f;
        float lowerDoorVelocity = 0f;
        float missileOutAmount = 0f;
        public const int normalDamage = 48;
        public const int expertDamage = 35;
        void ProcessDoorLogic()
        {
            if(!OpenDoor)
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

            if(!ArmMissiles)
            {

                missileCooldown = 60;
                if(missileOutAmount > 0)
                {
                    missileOutAmount += -2f / 60;
                }
                else
                {
                    lowerDoorVelocity += lowerDoorShutAcc;
                }
            }
            else
            {
                if(lowerDoorShutAmount <= 0)
                {
                    lowerDoorVelocity = 0;
                    missileOutAmount += 2f / 60;
                }
                else
                {
                    lowerDoorVelocity = -2f / 60;
                }
            }
            lowerDoorShutAmount += lowerDoorVelocity;
            if(lowerDoorShutAmount > 1f)
            {
                lowerDoorShutAmount = 1f;
                lowerDoorVelocity *= -0.4f;
            }
            if(lowerDoorShutAmount < 0f)
            {
                lowerDoorShutAmount = 0;
            }
            if(missileOutAmount >= 1)
            {
                missileOutAmount = 1;
                missileReady = true;
            }
            if(missileOutAmount < 0)
            {
                missileOutAmount = 0;
            }
        }
        int missileCooldown = 0;
        void FireMissiles()
        {
            ArmMissiles = true;
            Vector2 shootFrom = new Vector2( NPC.direction == 1 ? NPC.Left.X + 117 : NPC.Right.X - 117, NPC.Top.Y + 73);
            if(missileReady && missileCooldown <= 0 && MissileSiteOpen(shootFrom))
            {
                missileCooldown = 60;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), shootFrom, Vector2.UnitX * 4 * NPC.direction, ModContent.ProjectileType<BattleshipMissile>(),  NPC.GetAttackDamage_ForProjectiles(normalDamage, expertDamage), 0);
                }
            }
            else
            {
                missileCooldown--;
            }
            AimFly(8, shootFrom);
        }
        bool MissileSiteOpen(Vector2 shootFrom)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 checkSpot = shootFrom;
                switch (i)
                {
                    case 0:
                        checkSpot = shootFrom + Vector2.UnitX * 40;
                        break;
                    case 1:
                        checkSpot = shootFrom + Vector2.UnitX * -40;
                        break;
                    case 2:
                        checkSpot = shootFrom + Vector2.UnitY * 10;
                        break;
                    case 3:
                        checkSpot = shootFrom + Vector2.UnitY * -10;
                        break;
                }
                Point coords = checkSpot.ToTileCoordinates();
                if (Main.tile[coords.X, coords.Y].HasUnactuatedTile)
                {
                    return false;
                }
            }
            return true;
        }
        float secondGunRotation = (float)Math.PI;
        bool OpenGun = false;
        Vector2 gunPos;
        Vector2[] gunTips = new Vector2[4];
        int gunShootCounter = 0;
        void PositionGuns()
        {
            gunPos =  new Vector2(NPC.direction == 1 ? NPC.Left.X + 150 : NPC.Right.X - 150, NPC.Top.Y + 44);
            gunTips[0] = gunPos + new Vector2(20 * NPC.direction, 4);
            gunTips[1] = gunPos + new Vector2(26 * NPC.direction, 14);
            gunTips[2] = gunPos + new Vector2(26 * NPC.direction, 22);
            gunTips[3] = gunPos + new Vector2(32 * NPC.direction, 32);
        }
        void ProcessGunLogic(bool tryToShoot = false)
        {
            PositionGuns();
            if(OpenGun)
            {
                if(secondGunRotation > 0)
                {
                    secondGunRotation -= (float)Math.PI / 30;
                }
                else
                {
                    secondGunRotation = 0;
                    if(tryToShoot)
                    {
                        gunShootCounter++;
                        if(gunShootCounter % 45 == 0)
                        {
                            for(int gunIndex = 0; gunIndex < 4; gunIndex++)
                            {
                                if(Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), gunTips[gunIndex], Vector2.UnitX * 6 * NPC.direction, ModContent.ProjectileType<BattleshipShot>(), NPC.GetAttackDamage_ForProjectiles(normalDamage, expertDamage), 0);
                                }
                            }
                        }
                        AimFly(18, gunTips[1]);
                    }
                }
            }
            else
            {
                if(secondGunRotation < (float)Math.PI)
                {
                    secondGunRotation += (float)Math.PI / 30;
                }
                else
                {
                    secondGunRotation = (float)Math.PI;
                }
            }
        }
        float distressOutAmount = 0;
        bool callForHelp = false;
        int distresFrameCounter = 0;
        void ProcessDistressLogic()
        {
            if(callForHelp)
            {
                if(distressOutAmount < 1f)
                {
                    distressOutAmount += 1/30f;
                }
                else
                {
                    distressOutAmount = 1f;
                }
                if(distressOutAmount >= 1f)
                {
                    if(distresFrameCounter < 0)
                    {
                        distresFrameCounter = 0;
                    }
                    distresFrameCounter++;
                    if(distresFrameCounter == 45)
                    {
                        SoundEngine.PlaySound(SoundID.Item6, NPC.Center);
                    }
                    int droneCount = 120;
                    
                    if(distresFrameCounter >= 45 && distresFrameCounter <= 45 * 1 * droneCount && distresFrameCounter % 1 == 0)
                    {
                        Player player = Main.player[NPC.target];
                        int i = (distresFrameCounter - 45) / 1;
                        Vector2 here = player.Center - Vector2.UnitY * BattleshipStrikeDrone.heightAbove + Vector2.UnitX * (i * 60 - 60 * droneCount / 2) - new Vector2(BattleshipStrikeDrone.offFlyX, BattleshipStrikeDrone.offFlyY);
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)here.X, (int)here.Y, ModContent.NPCType<BattleshipStrikeDrone>());
                        
                    }
                }
            }
            else
            {
                if(distresFrameCounter > 30)
                {
                    distresFrameCounter = 30;
                }
                distresFrameCounter--;
                if(distresFrameCounter <= 0)
                {
                    if(distressOutAmount > 0f)
                    {
                        distressOutAmount -= 1/30f;
                    }
                    else
                    {
                        distressOutAmount = 0f;
                    }
                }
            
            }
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(0.75f * NPC.lifeMax * bossLifeScale);
            NPC.damage = NPC.damage / 2;
        }
        float pupilStareOutAmount = 1f;
        float pupilDirection = 0;
        void ProcessPassenger()
        {
            //pupilDirection = (Main.player[NPC.target].Center - NPC.Center).ToRotation();
        }
        bool AimFly(float shootSpeed, Vector2 shootFrom, float speed = 10)
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            float height = QwertyMethods.PredictiveVerticalAim(shootFrom.X, shootSpeed, player.Center, player.velocity);
            NPC.velocity.X = ((player.Center.X - rangedLingerDistance * NPC.direction)  - NPC.Center.X) * 0.02f;
            if(!float.IsNaN(height))
            {
                NPC.velocity.Y = Math.Sign(height - shootFrom.Y) * speed;
            }
            else
            {
                NPC.velocity.Y = Math.Sign(player.Center.Y - shootFrom.Y) * speed;
            }
            if(Math.Abs(shootFrom.Y - player.Center.Y) < Math.Abs(NPC.velocity.Y))
            {
                NPC.velocity.Y = player.Center.Y - shootFrom.Y;
                return !float.IsNaN(height);
            }
            return false;
        }
        int aiTimer = 0;
        int attack = 0;
        int rangedLingerDistance = 500;
        int[] rangedAttacks = new int[] {2, 3, 4};
        int attackCounter = 0;
        float chargeSpeed = 24;
        int spellTimer = 0;
        int spellTimeUp = 60 * 15;
        int activeSpellCountdown = 0;
        int spellIndex = 2;
        int distruptiveSpellCounter = 0;
        int attackSpellCounter = 0;
        Spell[] spells = new Spell[] {Spell.DistruptGravity, Spell.DistruptVision, Spell.DistruptCamera, Spell.Beam, Spell.AimedShots, Spell.ShadowMissiles};
        int beamIndex = -1;
        bool UseDistruptSpell()
        {
            Player player = Main.player[NPC.target];
            return !(player.HasBuff(ModContent.BuffType<GravityFlipped>()) || player.HasBuff(ModContent.BuffType<Darkness>()) || player.HasBuff(ModContent.BuffType<CameraIssues>()));
        }
        public override void AI()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player allPlayer = Main.player[i];
                if (allPlayer.active && !allPlayer.dead)
                {
                    allPlayer.AddBuff(ModContent.BuffType<NormalGravity>(), 2);
                }
            }
            aiTimer++;
            spellTimer++;
            if((float)NPC.life / NPC.lifeMax < 0.8f)
            {
                int amt = NPC.life - (int)(NPC.lifeMax * 0.2f);
                if(amt < 0)
                {
                    amt = 0;
                }
                float ratio = ((float)amt / (int)(NPC.lifeMax * 0.6f));
                spellTimeUp = (int)(ratio * 14 + 1) * 60;
            }
            OpenDoor = false;
            if(spellTimer > spellTimeUp)
            {
                OpenDoor = true;
                if(garageDropAmount == 0)
                {
                    if(activeSpellCountdown == 0)
                    {
                        if(UseDistruptSpell())
                        {
                            spellIndex = distruptiveSpellCounter;
                            distruptiveSpellCounter++;
                            if(distruptiveSpellCounter > 2)
                            {
                                distruptiveSpellCounter = 0;
                            }
                        }
                        else
                        {
                            spellIndex = attackSpellCounter + 3;
                            attackSpellCounter++;
                            if(attackSpellCounter > 1)
                            {
                                attackSpellCounter = 0;
                            }
                        }
                        activeSpellCountdown = NoehtnapSpells.Start(spells[spellIndex]);
                        if(spellIndex == 3)
                        {
                            beamIndex = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One, ModContent.ProjectileType<SpellBeam>(), NPC.GetAttackDamage_ForProjectiles(normalDamage, expertDamage), 0);
                        }
                    }
                }
                if(activeSpellCountdown > 0)
                {
                    NoehtnapSpells.UpdateSpell(spells[spellIndex], NPC.position + new Vector2(NPC.direction == 1 ? 91 : 176 - 91, 31), activeSpellCountdown, out pupilDirection, out pupilStareOutAmount);
                    if(beamIndex != -1)
                    {
                        if(Main.projectile[beamIndex].type != ModContent.ProjectileType<SpellBeam>() || !Main.projectile[beamIndex].active)
                        {
                            beamIndex = -1;
                        }
                        else
                        {
                            Main.projectile[beamIndex].rotation = pupilDirection;
                            Main.projectile[beamIndex].Center = NPC.position + new Vector2(NPC.direction == 1 ? 91 : 176 - 91, 31) + NoehtnapSpells.PupilPosition(pupilDirection, pupilStareOutAmount); 
                        }
                    }
                    activeSpellCountdown--;
                    if(activeSpellCountdown <= 0)
                    {
                        activeSpellCountdown = 0;
                        spellTimer = 0;
                    }
                }
                else
                {
                    beamIndex = -1;
                }
            }
            ArmMissiles = false;
            NPC.velocity = Vector2.Zero;
            OpenGun = false;
            callForHelp = false;
            switch(attack)
            {
                case 0:
                    if(AimFly(chargeSpeed, NPC.Center, 18) && aiTimer > 120)
                    {
                        attack = 1;
                    }
                    break;
                case 1:
                    NPC.velocity = Vector2.UnitX * NPC.direction * chargeSpeed;
                    NPC.TargetClosest(false);
                    Player player = Main.player[NPC.target];
                    if((NPC.Center.X - player.Center.X) * NPC.direction > rangedLingerDistance)
                    {
                        attack = rangedAttacks[attackCounter];
                        attackCounter++;
                        if(attackCounter > 2)
                        {
                            attackCounter = 0;
                        }
                        aiTimer = 0;
                        if(attack == 4)
                        {
                            aiTimer = 50 * 5;
                        }
                        if(Main.netMode != 1)
                        {
                            NPC.netUpdate = true;
                        }
                    }
                    break;
                case 2:
                    OpenGun = true;
                    break;
                case 3:
                    FireMissiles();
                    break;
                case 4:
                    callForHelp = true;
                    AimFly(chargeSpeed, NPC.Center);
                    break;

            }
            if(aiTimer > 60 * 8 && attack != 1 && beamIndex == -1)
            {
                attack = 0;
            }
            ProcessDoorLogic();
            ProcessGunLogic(OpenGun);
            ProcessDistressLogic();
            ProcessPassenger();
            
        }
        
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(aiTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            aiTimer = reader.ReadInt32();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            PositionGuns();
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D interior = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Interior").Value;
            Texture2D garage = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_GarageDoor").Value;
            Texture2D lowerDoor = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_LowerDoor").Value;
            Texture2D missileLauncher = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_MissileLauncher").Value;
            Texture2D gun = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Guns").Value;
            Texture2D gunCover = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_GunsCover").Value;
            Texture2D distress = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Distress").Value;

            Texture2D textureGlow = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Glow").Value;
            Texture2D missileLauncherGlow = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_MissileLauncher_Glow").Value;
            Texture2D gunGlow = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Guns_Glow").Value;
            Texture2D gunCoverGlow = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_GunsCover_Glow").Value;
            Texture2D distressGlow = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Distress_Glow").Value;

            Vector2 origin = texture.Size() * 0.5f;
            int garageTop = 10;
            int garageCenter = 91;
            int garageHeight = garage.Height;
            int lowerDoorCenter = 99;
            int lowerDoorTop = 62;
            int lowerDoorOpenLimit = 33;
            int lowerDoorPos = (int)(lowerDoorOpenLimit - (float)lowerDoorOpenLimit * lowerDoorShutAmount);
            int launcherHieght = 152 / 4;
            int missileFrame = 0;
            if(missileCooldown > 52)
            {
                missileFrame = 3;
            }
            else if(missileCooldown > 46)
            {
                missileFrame = 2;
            }
            else if(missileCooldown > 38)
            {
                missileFrame = 1;
            }
            
            Vector2 gunOrigin = Vector2.UnitX * (NPC.direction == 1 ?  24 : 50-24);
            Vector2 secondGunOffset = new Vector2(6 * NPC.direction, 18);
            int distressCenter = 26;
            int distressHeight = 40;
            int distressFrame = 0;
            if(distresFrameCounter > 30)
            {
                int mod = (distresFrameCounter - 30) % 21;
                distressFrame = 2;
                if(mod > 14)
                {
                    distressFrame = 4;
                }
                else if(mod > 7)
                {
                    distressFrame = 3;
                }
            }
            else if(distresFrameCounter > 15)
            {
                distressFrame = 1;
            }
            
            

            spriteBatch.Draw(missileLauncher, new Vector2(NPC.direction == 1 ? NPC.Left.X + lowerDoorCenter : NPC.Right.X - lowerDoorCenter, NPC.Top.Y + lowerDoorTop - (launcherHieght - missileOutAmount * launcherHieght)) - screenPos, new Rectangle(0, launcherHieght * missileFrame, missileLauncher.Width, launcherHieght), drawColor, NPC.rotation, Vector2.UnitX * (NPC.direction == 1 ?  21 : 29), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(missileLauncherGlow, new Vector2(NPC.direction == 1 ? NPC.Left.X + lowerDoorCenter : NPC.Right.X - lowerDoorCenter, NPC.Top.Y + lowerDoorTop - (launcherHieght - missileOutAmount * launcherHieght)) - screenPos, new Rectangle(0, launcherHieght * missileFrame, missileLauncher.Width, launcherHieght), Color.White, NPC.rotation, Vector2.UnitX * (NPC.direction == 1 ?  21 : 29), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(interior, NPC.Center - screenPos, null, drawColor, NPC.rotation, origin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            NoehtnapSpells.DrawBody(spriteBatch, NPC.position + new Vector2(NPC.direction == 1 ? 91 : 176 - 91, 31) - screenPos, drawColor, pupilDirection, pupilStareOutAmount, true);
            spriteBatch.Draw(distress, new Vector2(NPC.direction == 1 ? NPC.Left.X + distressCenter : NPC.Right.X - distressCenter, NPC.Top.Y + 2 + (distressHeight - distressOutAmount * distressHeight)) - screenPos, new Rectangle(0, distressHeight * distressFrame, distress.Width, distressHeight), drawColor, NPC.rotation, new Vector2(NPC.direction == 1 ? 17 : 28 - 17, 40), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(distressGlow, new Vector2(NPC.direction == 1 ? NPC.Left.X + distressCenter : NPC.Right.X - distressCenter, NPC.Top.Y + 2 + (distressHeight - distressOutAmount * distressHeight)) - screenPos, new Rectangle(0, distressHeight * distressFrame, distress.Width, distressHeight), Color.White, NPC.rotation, new Vector2(NPC.direction == 1 ? 17 : 28 - 17, 40), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(garage, new Vector2(NPC.direction == 1 ? NPC.Left.X + garageCenter : NPC.Right.X - garageCenter, NPC.Top.Y + garageTop) - screenPos, new Rectangle(0, garageHeight - (int)((float)garageHeight * garageDropAmount), garage.Width, (int)((float)garageHeight * garageDropAmount)), drawColor, NPC.rotation, Vector2.UnitX * garage.Width * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(lowerDoor, new Vector2(NPC.direction == 1 ? NPC.Left.X + (lowerDoorCenter - lowerDoorPos) : NPC.Right.X - (lowerDoorCenter + lowerDoorPos), NPC.Top.Y + lowerDoorTop) - screenPos, null, drawColor, NPC.rotation, Vector2.UnitX * lowerDoor.Width, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(lowerDoor, new Vector2(NPC.direction == 1 ? NPC.Left.X + (lowerDoorCenter + lowerDoorPos) : NPC.Right.X - (lowerDoorCenter - lowerDoorPos), NPC.Top.Y + lowerDoorTop) - screenPos, null, drawColor, NPC.rotation, Vector2.Zero, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, origin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(textureGlow, NPC.Center - screenPos, null, Color.White, NPC.rotation, origin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(gun, gunPos - screenPos, null, drawColor, NPC.rotation, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(gun, gunPos + secondGunOffset - screenPos, null, drawColor, NPC.rotation + secondGunRotation * -NPC.direction, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(gunGlow, gunPos - screenPos, null, Color.White, NPC.rotation, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(gunGlow, gunPos + secondGunOffset - screenPos, null, Color.White, NPC.rotation + secondGunRotation * -NPC.direction, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(gunCover, gunPos - screenPos, null, drawColor, NPC.rotation, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(gunCover, gunPos + secondGunOffset - screenPos, null, drawColor, NPC.rotation + secondGunRotation * -NPC.direction, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(gunCoverGlow, gunPos - screenPos, null, Color.White, NPC.rotation, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(gunCoverGlow, gunPos + secondGunOffset - screenPos, null, Color.White, NPC.rotation + secondGunRotation * -NPC.direction, gunOrigin, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }
}