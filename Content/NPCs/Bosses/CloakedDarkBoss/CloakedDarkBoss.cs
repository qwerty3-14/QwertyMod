using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.CloakedDarkBoss
{
    public class CloakedDarkBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noehtnap");
            Main.npcFrameCount[NPC.type] = 5;
        }


        public override void SetDefaults()
        {
            NPC.width = 166;
            NPC.height = 128;
            NPC.damage = 80;
            NPC.defense = 12;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TheGodsBleed");
            NPC.lifeMax = 6500;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheGodsBleed");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
				new FlavorTextBestiaryInfoElement("A being who's every action is in opposition to the Gods. How is such a being even able to exist? Maybye the Gods aren't so powerful after all...")
            });
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        private bool canDespawn = false;

        public override bool CheckActive()
        {
            return canDespawn;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return NPC.chaseable;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax *  0.75f * bossLifeScale);
            NPC.damage = 60;
        }

        private static float randomRotation()
        {
            return Main.rand.NextFloat(-1, 1) * (float)Math.PI;
        }

        public Projectile cloak;
        private int timer = -360;
        private float playerviewRadius = 80;
        private float orbitalVelocity = 7f;
        private float orbitDistance = 350;
        private float retreatApproachSpeed = 4f;
        private float pupilDirection = 0f;
        private float greaterPupilRadius = 18;
        private float lesserPupilRadius = 6;
        private float pupilStareOutAmount = .2f;
        private float blinkCounter = 60;
        private int frame = 0;
        private float pulseCounter = 0f;
        private int attackType = -1;
        private Projectile myWall = null;
        private Vector2 lastMoved = Vector2.UnitX;

        private float defaultOrbitalSpeed()
        {
            if (orbitalVelocity == 0)
            {
                NPC.netUpdate = true;
                return 7f * (Main.rand.Next(2) == 0 ? 1f : -1f);
            }
            return 7f * (orbitalVelocity > 0 ? 1f : -1f);
        }

        public override void AI()
        {
            //QwertyMethods.ServerClientCheck(timer);

            Player player = Main.player[NPC.target];
            NPC.chaseable = false;
            if (NPC.ai[3] > 10)
            {
                NPC.chaseable = true;
            }
            else
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if ((Main.player[i].active && (QwertyMod.GetLocalCursor(i) - NPC.Center).Length() < 180) || (Main.player[i].Center - NPC.Center).Length() < orbitDistance)
                    {
                        NPC.chaseable = true;
                        break;
                    }
                }
            }
            NPC.TargetClosest(false);
            pupilDirection = (player.Center - NPC.Center).ToRotation();
            
            pupilStareOutAmount = (player.Center - NPC.Center).Length() / 300f;
            if (pupilStareOutAmount > 1f)
            {
                pupilStareOutAmount = 1f;
            }

            blinkCounter--;
            if (blinkCounter < 0 && attackType != 1)
            {
                if (blinkCounter % 10 == 0)
                {
                    if (frame == 7)
                    {
                        blinkCounter = Main.rand.Next(180, 240);
                    }
                    else
                    {
                        frame++;
                    }
                }
            }
            else
            {
                frame = 0;
            }
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    canDespawn = true;
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            else
            {
                canDespawn = false;

                if ((cloak == null || cloak.type != ProjectileType<Cloak>() || !cloak.active) && Main.netMode != 1)
                {
                    cloak = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center, Vector2.Zero, ProjectileType<Cloak>(), 0, 0, Main.myPlayer, NPC.whoAmI)];
                }

                if (playerviewRadius > 80)
                {
                    playerviewRadius -= 10;
                }
                if (Main.netMode != 1)
                {
                    cloak.ai[1] = playerviewRadius;
                    cloak.timeLeft = 30;
                }

                if (myWall != null)
                {
                    myWall.timeLeft = 2;
                }

                if (NPC.ai[3] > 0)
                {
                    NPC.ai[3]--;
                }
                if (attackType != 0)
                {
                    timer++;
                    if (player.velocity.Length() > 0f)
                    {
                        lastMoved = player.velocity;
                    }
                }
                if (Main.expertMode && (float)NPC.life / NPC.lifeMax < .1f)
                {
                    attackType = 2;
                    orbitalVelocity = defaultOrbitalSpeed() * 2f;
                }
                switch (attackType)
                {
                    case -1:

                        if (timer > 120 * (Main.expertMode ? .2f + .8f * ((float)NPC.life / NPC.lifeMax) : 1f) && (player.Center - NPC.Center).Length() < 1000f)
                        {
                            if (Main.netMode != 1)
                            {
                                attackType = Main.rand.Next(7);
                                switch (attackType)
                                {
                                    default:
                                        orbitalVelocity = 14f;
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            orbitalVelocity *= -1;
                                        }
                                        break;

                                    case 0:
                                        orbitalVelocity = defaultOrbitalSpeed() * 4f;
                                        break;

                                    case 1:
                                        Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center, Vector2.Zero, ProjectileType<Warning>(), 0, 0f, Main.myPlayer, 0, 0);
                                        if (!Main.dedServ)
                                        {
                                            SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                                            //SoundEngine.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Notice").WithVolume(3f).WithPitchVariance(.5f), NPC.Center);
                                        }
                                        NPC.velocity = Vector2.Zero;
                                        break;
                                }

                                NPC.netUpdate = true;
                            }
                            timer = 0;
                        }
                        break;

                    default:
                        if (timer >= 60 && !(Main.expertMode && (float)NPC.life / NPC.lifeMax < .1f))
                        {
                            orbitalVelocity = defaultOrbitalSpeed();
                            timer = 0;
                            attackType = -1;
                        }
                        else if (timer % 15 == 0 && Main.netMode != 1)
                        {
                            Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center, Vector2.Zero, ProjectileType<EtimsicCannon>(), Main.expertMode ? 18 : 24, 0f, Main.myPlayer, (player.Center - NPC.Center).ToRotation());
                            Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center, Vector2.Zero, ProjectileType<Warning>(), 0, 0f, Main.myPlayer, 1, (player.Center - NPC.Center).ToRotation());
                            if (!Main.dedServ)
                            {
                                SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                                // SoundEngine.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Notice").WithVolume(3f).WithPitchVariance(.5f), NPC.Center);
                            }
                        }
                        break;

                    case 0:
                        if (timer == 0 && QwertyMethods.AngularDifference(lastMoved.ToRotation(), (NPC.Center - player.Center).ToRotation()) < (float)Math.PI / 30)
                        {
                            if (Main.netMode != 1)
                            {
                                myWall = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center, Vector2.Zero, ProjectileType<EtimsicWall>(), Main.expertMode ? 24 : 36, 0f, Main.myPlayer, (player.Center - NPC.Center).ToRotation() + (float)Math.PI / 2)];
                                Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center, Vector2.Zero, ProjectileType<Warning>(), 0, 0f, Main.myPlayer, 2, (player.Center - NPC.Center).ToRotation() + (float)Math.PI / 2);
                            }

                            if (!Main.dedServ)
                            {
                                SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                                //  SoundEngine.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Notice").WithVolume(3f).WithPitchVariance(.5f), NPC.Center);
                            }

                            timer = 1;
                            NPC.netUpdate = true;
                        }
                        if (timer > 0)
                        {
                            timer++;
                        }
                        if (timer >= 60)
                        {
                            orbitalVelocity = defaultOrbitalSpeed();
                            NPC.netUpdate = true;
                            timer = 0;
                            attackType = -1;
                        }
                        break;

                    case 1:
                        if (timer > 60)
                        {
                            NPC.ai[3] = 80;
                            if (timer < 180 && timer % 15 == 0)
                            {
                                NPC.velocity = Vector2.Zero;
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center + new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * NPC.scale, QwertyMethods.PolarVector(10, pupilDirection), ProjectileType<EtimsicRay>(), Main.expertMode ? 18 : 24, 0f, Main.myPlayer);
                                }
                                if (!Main.dedServ)
                                {
                                    SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                                    //SoundEngine.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PewPew").WithVolume(3f).WithPitchVariance(.5f), NPC.Center);
                                }
                            }
                            if (timer == 179 && Main.netMode != 1)
                            {
                                NPC.netUpdate = true;
                            }
                            if (timer == 180)
                            {
                                NPC.velocity = QwertyMethods.PolarVector(15, (player.Center - NPC.Center).ToRotation());
                                NPC.netUpdate = true;
                            }
                            if (timer > 240)
                            {
                                NPC.netUpdate = true;
                                orbitalVelocity = defaultOrbitalSpeed();
                                timer = 0;
                                attackType = -1;
                            }
                        }
                        break;
                }

                //movement
                if (attackType == 1 && (((float)NPC.life / NPC.lifeMax > .5f) || timer >= 180 || timer < 60))
                {
                }
                else
                { 
                    NPC.velocity = QwertyMethods.PolarVector(orbitalVelocity, (player.Center - NPC.Center).ToRotation() + (float)Math.PI / 2);

                    if ((player.Center - NPC.Center).Length() < orbitDistance - 50)
                    {
                        NPC.velocity += QwertyMethods.PolarVector(-retreatApproachSpeed, (player.Center - NPC.Center).ToRotation());
                    }
                    else if ((player.Center - NPC.Center).Length() > orbitDistance + 50)
                    {
                        NPC.velocity += QwertyMethods.PolarVector(retreatApproachSpeed, (player.Center - NPC.Center).ToRotation());
                    }
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(orbitalVelocity);
            writer.Write(attackType);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
            orbitalVelocity = reader.ReadSingle();
            attackType = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            pulseCounter += (float)Math.PI / 30;
            NPC.scale = 1f + .05f * (float)Math.Sin(pulseCounter);
            if (frame > 4)
            {
                NPC.frame.Y = (8 - frame) * frameHeight;
            }
            else
            {
                NPC.frame.Y = frame * frameHeight;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos,
                       NPC.frame, drawColor, NPC.rotation,
                       new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), NPC.scale, SpriteEffects.None, 0f);
            Texture2D Pupil = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/Pupil").Value;
            spriteBatch.Draw(Pupil, NPC.Center - screenPos + new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * NPC.scale,
                       Pupil.Frame(), drawColor, NPC.rotation,
                       Pupil.Size() * .5f, NPC.scale, SpriteEffects.None, 0f);
            Texture2D Eyelid = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/Eyelid").Value;
            spriteBatch.Draw(Eyelid, NPC.Center - screenPos,
                       NPC.frame, drawColor, NPC.rotation,
                       new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ItemType<NoehtnapBag>())); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<Etims>(), 1, 12, 24));
            npcLoot.Add(notExpertRule);

            //Trophies are spawned with 1/10 chance
            //npcLoot.Add(ItemDropRule.Common(ItemType<HydraTrophy>(), 10));


            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedNoehtnap, -1);
        }
    }

    public class Cloak : ModProjectile
    {
        public bool cloak;

        public override void SetStaticDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.timeLeft = 2;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        { 
            overWiresUI.Add(index);
        }

        private List<Vector3> lightSpots = new List<Vector3>();

        private int fadeInTimer = 0;

        public override void AI()
        {
            if (fadeInTimer < 255)
            {
                fadeInTimer++;
            }
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
            // QwertyMethods.ServerClientCheck((int)Projectile.ai[1]);
            if (Main.netMode != 1)
            {
                Projectile.netUpdate = true;
            }
        }

        private int screenWidthOld = 0;
        private int screenHeightOld = 0;
        private int height = 0;
        private int width = 0;
        private Color[] defaultdataColors = null;
        private Color trans = new Color(0, 0, 0, 0);
        private float scale = 8;

        private void lightsUpdate(Color color)
        {
            foreach (Vector3 spot in lightSpots)
            {
                Vector2 spotCoords = new Vector2(spot.X, spot.Y);
                for (int localX = 0; localX < (int)spot.Z / scale; localX++)
                {
                    for (int localY = 0; localY < (int)spot.Z / scale; localY++)
                    {
                        if (new Vector2(localX, localY).Length() < spot.Z / scale)
                        {
                            int x = (int)spotCoords.X + localX;
                            int y = (int)spotCoords.Y + localY;
                            int loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                defaultdataColors[loc] = color;
                            }
                            x = (int)spotCoords.X - localX;
                            y = (int)spotCoords.Y + localY;
                            loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                defaultdataColors[loc] = color;
                            }
                            x = (int)spotCoords.X - localX;
                            y = (int)spotCoords.Y - localY;
                            loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                defaultdataColors[loc] = color;
                            }
                            x = (int)spotCoords.X + localX;
                            y = (int)spotCoords.Y - localY;
                            loc = x + y * width;
                            if (loc < defaultdataColors.Length && x < width && x > 0 && loc >= 0)
                            {
                                defaultdataColors[loc] = color;
                            }
                        }
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player drawPlayer = Main.LocalPlayer;

            if (Main.screenWidth != screenWidthOld || Main.screenHeight != screenHeightOld)
            {
                height = (int)(Main.screenHeight / scale);
                width = (int)(Main.screenWidth / scale);
                height++;
                defaultdataColors = new Color[width * height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        defaultdataColors[x + y * width] = Color.Black;
                    }
                }
            }

            lightsUpdate(Color.Black); //reset color

            lightSpots = new List<Vector3>();
            lightSpots.Add(new Vector3((drawPlayer.Center.X - Main.screenPosition.X) / scale, (drawPlayer.Center.Y - Main.screenPosition.Y) / scale, Projectile.ai[1]));
            if (!Main.gamePaused)
            {
                lightSpots.Add(new Vector3(Main.mouseX / scale, Main.mouseY / scale, 80));
            }

            NPC master = Main.npc[(int)Projectile.ai[0]];
            if (master.ai[3] > 0)
            {
                lightSpots.Add(new Vector3((master.Center.X - Main.screenPosition.X) / scale, (master.Center.Y - Main.screenPosition.Y) / scale, master.ai[3]));
            }
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].active && (Main.projectile[i].type == ProjectileType<EtimsicCannon>() || Main.projectile[i].type == ProjectileType<EtimsicWall>()) && Main.projectile[i].ai[1] == 1)
                {
                    lightSpots.Add(new Vector3((Main.projectile[i].Center.X - Main.screenPosition.X) / scale, (Main.projectile[i].Center.Y - Main.screenPosition.Y) / scale, 40));
                }
            }

            lightsUpdate(trans); //now that we have lights make them transparent
            Texture2D TheShadow = new Texture2D(Main.graphics.GraphicsDevice, width, height);
            TheShadow.SetData(0, null, defaultdataColors, 0, width * height);
            Main.EntitySpriteDraw(TheShadow, Vector2.Zero, null, new Color(fadeInTimer, fadeInTimer, fadeInTimer, fadeInTimer), 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            screenWidthOld = Main.screenWidth;
            screenHeightOld = Main.screenHeight;

            return false;
        }
    }
}