using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.Consumable.BossBag;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Weapon.Magic.Swordpocalypse;
using QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV;
using QwertyMod.Content.Items.Weapon.Whip.Discipline;
using QwertyMod.Content.Items.Weapon.Minion.Longsword;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.Arsenal;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Imperium;
using QwertyMod.Content.Items.Weapon.Morphs.Swordquake;
using Terraria.GameContent.ItemDropRules;
using QwertyMod.Content.Items.Consumable.Tile.Trophy.Blade;
using Terraria.GameContent.Bestiary;
using QwertyMod.Common;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace QwertyMod.Content.NPCs.Bosses.BladeBoss
{
    [AutoloadBossHead]
    public class Imperious : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "QwertyMod/Content/NPCs/Bosses/BladeBoss/Imperious_Bestiary",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            //Specify the debuffs it is immune to
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    BuffID.Ichor
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.width = 10;
            NPC.height = 10;
            NPC.damage = 65;
            NPC.defense = 42;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 150000f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BladeOfAGod");
            NPC.lifeMax = 25000;
            BossBag = ItemType<BladeBossBag>();
            NPC.chaseable = false;
            NPC.immortal = true;
            NPC.alpha = 255;
            NPC.behindTiles = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("The day the great conquorer fell his sword took on a life of its own...")
            });
            /*
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMissile_Checklist",
                PortraitScale = 0.6f, //Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionYOverride = 0f,
            };
            */
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            if (Main.rand.Next(100) == 0)
            {
                potionType = ItemID.CopperShortsword;
            }
            else
            {
                potionType = ItemID.GreaterHealingPotion;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(16000 * bossLifeScale);
            NPC.damage = 50;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        private bool activeCheck = false;

        public override bool CheckActive()
        {
            return activeCheck;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(BossBag)); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<SwordsmanBadge>(), ItemType<ImperiousTheIV>(), ItemType<Imperium>(), ItemType<SwordStormStaff>(), ItemType<Arsenal>(), ItemType<Discipline>(), ItemType<SwordMinionStaff>(), ItemType<Swordquake>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);

            //Boss masks are spawned with 1/7 chance
            //notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            //notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<PolarMask>(), 7));
            //npcLoot.Add(notExpertRule);



            //Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ItemType<BladeBossTrophy>(), 1));


            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            for (int h = 0; h < hitBoxSegmentIds.Length; h++)
            {
                if (hitBoxSegmentIds[h] != -1)
                {
                    Main.npc[hitBoxSegmentIds[h]].life = 0;
                    Main.npc[hitBoxSegmentIds[h]].HitEffect(0, 10.0);
                    Main.npc[hitBoxSegmentIds[h]].active = false;
                }
            }
            ClearPhantoms();

            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedBlade, -1);
        }

        private List<Vector3> nextPositions = new List<Vector3>();
        private float maxSpeed = 30f;
        private float maxSwingSpeed = (float)Math.PI / 80;
        private int[] hitBoxSegmentIds = { -1, -1, -1, -1, -1, -1, -1, -1 };
        private int totalLength = 398;
        private int bladeLength = 308;
        private int bladeWidth = 82;
        private bool debug = false;
        private float swingTargetDistance = 200;

        private int SpecialAttack
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private const int none = 0;
        private const int swordquake = 1;
        private const int starRage = 2;
        private const int phantomCircle = 3; //this attack is disabled in multiplayer since it would probably feel cheap for the player not being targeted

        private int SpecialAttackTimer
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private int swordlagmiteCount = 24;
        private int swordlagmiteStart = 120;
        private int followupSwordlagmiteDelay = 10;
        private int followupSwordlagmiteDelay2 = 120;
        private bool secondPhase = false;
        private List<int> PhantomBladeIds = new List<int>();
        private Vector3[] trailingEffect = new Vector3[10];
        private bool runOnce = true;

        private void CreatePhantom()
        {
            if (secondPhase)
            {
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                if (Main.netMode != 1)
                {
                    PhantomBladeIds.Add(Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center, Vector2.Zero, ProjectileType<PhantomBlade>(), (int)(NPC.damage * (Main.expertMode ? .25f : .5f)), 0, ai0: NPC.rotation));
                    Main.projectile[PhantomBladeIds[PhantomBladeIds.Count - 1]].rotation = Main.projectile[PhantomBladeIds[PhantomBladeIds.Count - 1]].ai[0] = NPC.rotation;
                    Main.projectile[PhantomBladeIds[PhantomBladeIds.Count - 1]].netUpdate = true;
                    NPC.netUpdate = true;
                }
            }
        }

        private void ClearPhantoms()
        {
            foreach (int p in PhantomBladeIds)
            {
                Main.projectile[p].Kill();
            }
            PhantomBladeIds.Clear();
        }

        private void FindMaxSpeed()
        {
            if (Main.netMode != 1)
            {
                Vector2 diff = nextPositions[0].to2() - NPC.Center;
                maxSpeed = diff.Length() / (QwertyMethods.AngularDifference(NPC.rotation, nextPositions[0].Z) / maxSwingSpeed);
                if (maxSpeed > 24f)
                {
                    maxSpeed = 24f;
                }
                NPC.netUpdate = true;
            }
        }

        private void AddPosition(Vector3 orientation)
        {
            if (Main.netMode != 1)
            {
                nextPositions.Add(orientation);
                if (nextPositions.Count == 1)
                {
                    FindMaxSpeed();
                }
                NPC.netUpdate = true;
            }
        }

        public override void AI()
        {
            NPC.defense = (secondPhase ? 34 : 42);
            followupSwordlagmiteDelay2 = secondPhase ? 90 : 120;
            NPC.frameCounter++;
            if (NPC.frameCounter % 2 == 0)
            {
                for (int v = trailingEffect.Length - 1; v > 0; v--)
                {
                    trailingEffect[v] = trailingEffect[v - 1];
                }
            }

            trailingEffect[0] = new Vector3(NPC.Center.X, NPC.Center.Y, NPC.rotation);
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (runOnce)
            {
                NPC.rotation = (NPC.Center - player.Center).ToRotation();
                runOnce = false;
            }
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, -30f);
                    activeCheck = true;
                    NPC.timeLeft--;
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                }
            }
            else
            {
                if (!secondPhase && Main.expertMode && (float)NPC.life / NPC.lifeMax < .5f)
                {
                    NPC.dontTakeDamage = true;
                    NPC.ai[2]++;
                    NPC.velocity = Vector2.Zero;
                    SpecialAttack = none;
                    SpecialAttackTimer = 0;
                    if (NPC.ai[2] > 120)
                    {
                        SpecialAttackTimer += 10;
                        NPC.dontTakeDamage = false;
                        secondPhase = true;
                    }
                }
                else
                {
                    //debug area
                    if (debug)
                    {
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            AddPosition(new Vector3(Main.MouseWorld.X, Main.MouseWorld.Y, (Main.MouseWorld - player.Center).ToRotation()));
                        }
                    }

                    //move to planned location
                    if (nextPositions.Count > 0)
                    {
                        Vector2 diff = nextPositions[0].to2() - NPC.Center;
                        NPC.velocity = diff;
                        if (NPC.velocity.Length() > maxSpeed)
                        {
                            NPC.velocity = NPC.velocity.SafeNormalize(Vector2.UnitY) * maxSpeed;
                        }
                        NPC.rotation.SlowRotation(nextPositions[0].Z, maxSwingSpeed);
                        if (diff.Length() < .01f && QwertyMethods.AngularDifference(nextPositions[0].Z, NPC.rotation) < .01f)
                        {
                            if (Main.netMode != 1)
                            {
                                nextPositions.RemoveAt(0);
                                if (nextPositions.Count > 0)
                                {
                                    FindMaxSpeed();
                                }
                                NPC.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        NPC.velocity = Vector2.Zero;
                    }

                    //plan where to go if there is no where planned
                    if (nextPositions.Count == 0)
                    {
                        switch ((int)SpecialAttack)
                        {
                            case none:
                                CreatePhantom();
                                SpecialAttackTimer++;
                                float towardMe = (NPC.Center - player.Center).ToRotation();
                                if (((player.Center - NPC.Center).Length() < totalLength && QwertyMethods.AngularDifference(towardMe + (float)Math.PI, NPC.rotation) < (float)Math.PI / 2) || SpecialAttackTimer >= 10)
                                {
                                    Vector2 pos = player.Center + QwertyMethods.PolarVector(500, towardMe);
                                    AddPosition(new Vector3(pos.X, pos.Y, towardMe));
                                    if (SpecialAttackTimer >= 10)
                                    {
                                        ClearPhantoms();
                                        SpecialAttackTimer = 0;
                                        NPC.netUpdate = true;
                                        if (Main.netMode != 1)
                                        {
                                            switch ((secondPhase && Main.netMode == 0) ? 2 : Main.rand.Next(2))
                                            {
                                                case 0:
                                                    AddPosition(new Vector3(pos.X, pos.Y, (float)Math.PI / 2));
                                                    AddPosition(new Vector3(pos.X, pos.Y - 40, (float)Math.PI / 2));
                                                    SpecialAttack = swordquake;
                                                    break;

                                                case 1:
                                                    AddPosition(new Vector3(pos.X, pos.Y, -(float)Math.PI / 2));
                                                    SpecialAttack = starRage;
                                                    break;

                                                case 2:
                                                    AddPosition(new Vector3(pos.X, pos.Y, -(float)Math.PI / 2));
                                                    AddPosition(new Vector3(pos.X, pos.Y + 40, -(float)Math.PI / 2));
                                                    AddPosition(new Vector3(pos.X, pos.Y - 400, -(float)Math.PI / 2));
                                                    SpecialAttack = phantomCircle;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Vector2 pos = player.Center + QwertyMethods.PolarVector(swingTargetDistance, towardMe);
                                    AddPosition(new Vector3(pos.X, pos.Y, towardMe + (float)Math.PI));
                                }
                                break;

                            case swordquake:
                                if (Collision.CanHit(NPC.Center, 0, 0, NPC.Center + Vector2.UnitY * bladeLength, 0, 0) && NPC.Center.Y - player.Center.Y < 3000 && SpecialAttackTimer ==0)
                                {
                                    NPC.velocity = Vector2.UnitY * 24f;
                                }
                                else
                                {
                                    NPC.velocity = Vector2.Zero;
                                    SpecialAttackTimer++;
                                    Vector2 spawnPos = NPC.Center + Vector2.UnitY * bladeLength;
                                    if (SpecialAttackTimer >= swordlagmiteStart + followupSwordlagmiteDelay2 + swordlagmiteCount * followupSwordlagmiteDelay)
                                    {
                                        SpecialAttackTimer = 0;
                                        SpecialAttack = none;
                                    }
                                    else
                                    {
                                        if (SpecialAttackTimer >= swordlagmiteStart + followupSwordlagmiteDelay2 && SpecialAttackTimer < swordlagmiteStart + followupSwordlagmiteDelay2 + swordlagmiteCount * followupSwordlagmiteDelay)
                                        {
                                            if (SpecialAttackTimer % followupSwordlagmiteDelay == 0)
                                            {
                                                int wave = (SpecialAttackTimer - (swordlagmiteStart + followupSwordlagmiteDelay2)) / followupSwordlagmiteDelay;
                                                if (Main.netMode != 1)
                                                {
                                                    SpawnSwordlagmite(spawnPos + Vector2.UnitX * (2 * wave * bladeWidth));

                                                    if (SpecialAttackTimer != 300)
                                                    {
                                                        SpawnSwordlagmite(spawnPos + Vector2.UnitX * (-2 * wave * bladeWidth));
                                                    }
                                                }
                                            }
                                        }
                                        if (SpecialAttackTimer >= swordlagmiteStart && SpecialAttackTimer < swordlagmiteStart + swordlagmiteCount * followupSwordlagmiteDelay)
                                        {
                                            if (SpecialAttackTimer % followupSwordlagmiteDelay == 0)
                                            {
                                                int wave = (SpecialAttackTimer - swordlagmiteStart) / followupSwordlagmiteDelay;
                                                if (Main.netMode != 1)
                                                {
                                                    SpawnSwordlagmite(spawnPos + Vector2.UnitX * (2 * wave * bladeWidth + bladeWidth));
                                                    SpawnSwordlagmite(spawnPos + Vector2.UnitX * (-2 * wave * bladeWidth - bladeWidth));
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                            case starRage:
                                SpecialAttackTimer++;
                                if (SpecialAttackTimer > 600)
                                {
                                    SpecialAttackTimer = 0;
                                    SpecialAttack = none;
                                }
                                else if (SpecialAttackTimer > 60)
                                {
                                    if (SpecialAttackTimer > 480 && QwertyMethods.AngularDifference(NPC.rotation, (float)-Math.PI / 2) < (float)Math.PI / 30)
                                    {
                                        NPC.rotation.SlowRotation((float)-Math.PI / 2, maxSwingSpeed);
                                    }
                                    else
                                    {
                                        if (SpecialAttackTimer % 16 == 0)
                                        {
                                            SoundEngine.PlaySound(SoundID.Item105, NPC.Center);
                                        }
                                        int starDirection = (player.Center.X - NPC.Center.X > 0 ? 1 : -1);
                                        NPC.rotation += (float)Math.PI / 30 * starDirection;
                                        NPC.velocity = (player.Center - NPC.Center).SafeNormalize(-Vector2.UnitY) * (Main.expertMode ? 4f : 2f);
                                        int impactZoneWidth = 1000;
                                        int startAwayAmount = 800;
                                        float shootSpeed = 10f;
                                        if (SpecialAttackTimer % 16 == 0)
                                        {
                                            if (Main.netMode != 1)
                                            {
                                                Vector2 fallToHere = new Vector2((player.Center.X + (player.velocity.X * startAwayAmount / shootSpeed)) - impactZoneWidth / 2 + Main.rand.Next(impactZoneWidth), player.Bottom.Y);
                                                Vector2 positionOffset = QwertyMethods.PolarVector(startAwayAmount, (float)Math.PI + (float)Math.PI / 2 - (float)Math.PI / 4 + Main.rand.NextFloat() * (float)Math.PI / 8);
                                                positionOffset.X *= starDirection;
                                                Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), fallToHere + positionOffset, QwertyMethods.PolarVector(shootSpeed, (positionOffset).ToRotation() + (float)Math.PI), ProjectileType<Swordpocalypse>(), (int)(NPC.damage * (Main.expertMode ? .2f : .4f)), 0);
                                            }
                                        }
                                    }
                                }
                                break;

                            case phantomCircle:

                                NPC.rotation.SlowRotation((float)Math.PI / 2, maxSwingSpeed);
                                SpecialAttackTimer++;
                                int circleDelay = 90;
                                int circleSpeed = 14;
                                int cicrleRange = 1400;
                                float swordCount = 7f;
                                int waveCount = 4;
                                if (SpecialAttackTimer >= circleDelay * waveCount + (cicrleRange - bladeLength + 18) / circleSpeed)
                                {
                                    SpecialAttackTimer = 0;
                                    SpecialAttack = none;
                                }
                                else
                                {
                                    if (SpecialAttackTimer <= circleDelay * waveCount && SpecialAttackTimer % circleDelay == 0)
                                    {
                                        SoundEngine.PlaySound(SoundID.Item8, player.Center);
                                        if (Main.netMode != 1)
                                        {
                                            float offset = Main.rand.NextFloat() * 2 * (float)Math.PI;
                                            if (SpecialAttackTimer == circleDelay * waveCount)
                                            {
                                                offset = -(float)Math.PI / 2;
                                            }
                                            for (int i = (SpecialAttackTimer == circleDelay * waveCount ? 1 : 0); i < swordCount; i++)
                                            {
                                                Projectile p = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), player.Center + QwertyMethods.PolarVector(cicrleRange, (i / swordCount) * 2 * (float)Math.PI + offset), QwertyMethods.PolarVector(-circleSpeed, (i / swordCount) * 2 * (float)Math.PI + offset), ProjectileType<PhantomBlade>(), (int)(NPC.damage * (Main.expertMode ? .25f : .5f)), 0, ai0: NPC.rotation)];
                                                p.timeLeft = (cicrleRange - bladeLength + 18) / circleSpeed;
                                                p.rotation = p.ai[0] = (i / swordCount) * 2 * (float)Math.PI + offset + (float)Math.PI;
                                                p.netUpdate = true;
                                            }
                                        }
                                    }
                                    if (SpecialAttackTimer >= circleDelay * waveCount)
                                    {
                                        NPC.velocity = Vector2.UnitY * circleSpeed;
                                    }
                                    else
                                    {
                                        Vector2 goTo = player.Center - Vector2.UnitY * cicrleRange;
                                        if ((goTo - NPC.Center).Length() < 30f)
                                        {
                                            NPC.Center = goTo;
                                            NPC.velocity = Vector2.Zero;
                                        }
                                        else
                                        {
                                            NPC.velocity = (goTo - NPC.Center).SafeNormalize(-Vector2.UnitY) * 30f;
                                        }
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            //position hitbox segments
            NPC.realLife = NPC.whoAmI;
            for (int h = 0; h < hitBoxSegmentIds.Length; h++)
            {
                Vector2 spot = NPC.Center + NPC.velocity + QwertyMethods.PolarVector((totalLength - bladeLength - 18) + h * (bladeLength / (hitBoxSegmentIds.Length + 1)) + bladeWidth / 2, NPC.rotation);
                if (hitBoxSegmentIds[h] == -1)
                {
                    if (Main.netMode != 1)
                    {
                        hitBoxSegmentIds[h] = NPC.NewNPC((int)spot.X, (int)spot.Y, NPCType<BladeHitbox>());
                        Main.npc[hitBoxSegmentIds[h]].realLife = NPC.realLife;
                        NPC.netUpdate = true;
                    }
                }
                else
                {
                    Main.npc[hitBoxSegmentIds[h]].Center = spot;
                    Main.npc[hitBoxSegmentIds[h]].timeLeft = 10;
                    Main.npc[hitBoxSegmentIds[h]].dontTakeDamage = NPC.dontTakeDamage;
                    Main.npc[hitBoxSegmentIds[h]].defense = NPC.defense;
                    Lighting.AddLight(spot, 1f, 1f, 1f);
                    Lighting.AddLight(spot + QwertyMethods.PolarVector(bladeWidth / 2, NPC.rotation), 1f, 1f, 1f);
                }
            }
        }

        private void SpawnSwordlagmite(Vector2 pos)
        {
            while (Collision.CanHit(pos, 0, 0, pos + Vector2.UnitY * 4, 0, 0))
            {
                pos.Y++;
            }
            pos.Y += bladeLength;
            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos, Vector2.Zero, ProjectileType<Swordlacmite>(), (int)(NPC.damage * (Main.expertMode ? .25f : .5f)), 0f);
        }

        public override void DrawEffects(ref Color drawColor)
        {
            drawColor.R = (byte)(drawColor.R * (1f - .4f * NPC.ai[2] / 120f));
            drawColor.G = (byte)(drawColor.G * (1f - .4f * NPC.ai[2] / 120f));
            drawColor.B = (byte)(drawColor.B * (1f - .4f * NPC.ai[2] / 120f));
        }

        private int phsaeChangeAnimationSpeed = 40;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (secondPhase)
            {
                GameShaders.Armor.Apply(1050, NPC);
            }
            if (debug)
            {
                foreach (Vector3 pos in nextPositions)
                {
                    Texture2D texture2 = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/BladeBoss/DebugBlade").Value;
                    spriteBatch.Draw(texture2, pos.to2() - screenPos, null, drawColor, pos.Z, new Vector2(18, texture2.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
                }
            }

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            if (!secondPhase && Main.expertMode && (float)NPC.life / NPC.lifeMax < .5f)
            {
                int alpha = (int)(120f * (((int)NPC.ai[2] % phsaeChangeAnimationSpeed) / (float)phsaeChangeAnimationSpeed));
                spriteBatch.Draw(texture, NPC.Center + QwertyMethods.PolarVector(totalLength / 2, NPC.rotation) - screenPos, null, new Color(alpha, alpha, alpha, alpha), NPC.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(1f, 1f) * (1 + (1 * (phsaeChangeAnimationSpeed - (int)NPC.ai[2] % phsaeChangeAnimationSpeed) / (float)phsaeChangeAnimationSpeed)), SpriteEffects.None, 0f);
            }
            if (secondPhase)
            {
                for (int v = 0; v < trailingEffect.Length; v++)
                {
                    if (trailingEffect[v] != null)
                    {
                        spriteBatch.Draw(texture, trailingEffect[v].to2() - screenPos, null, new Color(v * 3, v * 3, v * 3, v * 3), trailingEffect[v].Z, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    }
                }
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);

            if (debug)
            {
                texture = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/BladeBoss/DebugBladeOutline").Value;
                spriteBatch.Draw(texture, Main.MouseWorld - screenPos, null, drawColor, (Main.MouseWorld - Main.LocalPlayer.Center).ToRotation(), new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(nextPositions.Count);
            for (int i = 0; i < nextPositions.Count; i++)
            {
                writer.WriteVector2(nextPositions[i].to2());
                writer.Write(nextPositions[i].Z);
            }
            writer.Write(maxSpeed);
            writer.WriteVector2(NPC.position);
            for (int i = 0; i < hitBoxSegmentIds.Length; i++)
            {
                writer.Write(hitBoxSegmentIds[i]);
            }
            writer.Write(PhantomBladeIds.Count);
            for (int i = 0; i < PhantomBladeIds.Count; i++)
            {
                writer.Write(PhantomBladeIds[i]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            nextPositions.Clear();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Vector2 pos = reader.ReadVector2();
                float rot = reader.ReadSingle();
                nextPositions.Add(new Vector3(pos.X, pos.Y, rot));
            }
            maxSpeed = reader.ReadSingle();
            NPC.position = reader.ReadVector2();
            for (int i = 0; i < hitBoxSegmentIds.Length; i++)
            {
                hitBoxSegmentIds[i] = reader.ReadInt32();
            }
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                PhantomBladeIds[i] = reader.ReadInt32();
            }
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
    }

    public class BladeHitbox : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious");
            //Specify the debuffs it is immune to
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    BuffID.Ichor
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.width = 82;
            NPC.height = 82;
            NPC.damage = 65;
            NPC.defense = 42;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lifeMax = 25000;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BladeOfAGod");
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        private bool activeCheck = false;

        public override bool CheckActive()
        {
            return !NPC.AnyNPCs(NPCType<Imperious>());
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(32000 * bossLifeScale);
            NPC.damage = 50;
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(NPCType<Imperious>()))
            {
                NPC.position.Y -= 100;
            }
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    activeCheck = true;
                    NPC.timeLeft--;
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                }
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            List<int> hitboxIds = new List<int>();
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].type == NPCType<BladeHitbox>())
                {
                    hitboxIds.Add(i);
                }
            }
            if (projectile.usesLocalNPCImmunity || projectile.localNPCImmunity[NPC.whoAmI] != 0)
            {
                foreach (int who in hitboxIds)
                {
                    projectile.localNPCImmunity[who] = projectile.localNPCImmunity[NPC.whoAmI];
                    Main.npc[who].immune[projectile.owner] = NPC.immune[projectile.owner];
                }
            }
            else if(projectile.usesIDStaticNPCImmunity)
            {
                foreach (int who in hitboxIds)
                {
                    Projectile.perIDStaticNPCImmunity[projectile.type][who] = Projectile.perIDStaticNPCImmunity[projectile.type][NPC.whoAmI];
                }
            }
            else
            {
                foreach (int who in hitboxIds)
                {
                    Main.npc[who].immune[projectile.owner] = NPC.immune[projectile.owner];
                }
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            List<int> hitboxIds = new List<int>();
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].type == NPCType<BladeHitbox>())
                {
                    hitboxIds.Add(i);
                }
            }
            foreach (int who in hitboxIds)
            {
                Main.npc[who].immune[player.whoAmI] = NPC.immune[player.whoAmI];
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            for (int b = 0; b < NPC.buffType.Length; b++)
            {
                if (NPC.buffType[b] != 0)
                {
                    target.AddBuff(NPC.buffType[b], NPC.buffTime[b]);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }

    
    public class Swordlacmite : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordlagmite");
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
        }

        public override void SetDefaults()
        {
            Projectile.width = 82;
            Projectile.height = 2;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 190;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        private const int lingerTime = 60;
        private const int extendSpeed = 30;

        public override void AI()
        {
            if (Projectile.timeLeft == lingerTime)
            {
                Projectile.height += extendSpeed;
                Projectile.position.Y -= extendSpeed;

                Player player = null;
                float dist = -1f;
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if (Main.player[i].active && (Math.Abs(Main.player[i].Center.X - Projectile.Center.X) < dist || dist == -1f))
                    {
                        player = Main.player[i];
                        dist = Math.Abs(Main.player[i].Center.X - Projectile.Center.X);
                    }
                }
                if (player != null)
                {
                    if (!(Projectile.position.Y < player.Center.Y - 100))
                    {
                        Projectile.timeLeft++;
                    }
                }
            }
            if (Projectile.timeLeft == lingerTime - 1)
            {
                SoundEngine.PlaySound(SoundID.Item69, Projectile.Center);
            }
            if (Projectile.timeLeft == 1)
            {
                Projectile.height -= extendSpeed;
                Projectile.position.Y += extendSpeed;
                if (Projectile.height > extendSpeed)
                {
                    Projectile.timeLeft++;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 130)
            {
                Texture2D line = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/BladeBoss/WarningLaser").Value;
                Main.EntitySpriteDraw(line, new Vector2(Projectile.Center.X - Main.screenPosition.X, Main.screenHeight), null, ((Projectile.timeLeft % 10 == 0) ? Color.White : Color.Red), 0f, new Vector2(1, 6), new Vector2(1f, Main.screenHeight / 6), 0, 0);
            }
            int tipHeight = 60;
            int segmentHeight = 40;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int k = 0;
            Main.EntitySpriteDraw(texture, Projectile.position + Vector2.UnitY * (k * segmentHeight) - Main.screenPosition, new Rectangle(0, 0, Projectile.width, 82), Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.position.Y + tipHeight / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);

            for (; k < ((Projectile.height - tipHeight) / segmentHeight) - 1; k++)
            {
                Main.EntitySpriteDraw(texture, Projectile.position + Vector2.UnitY * (k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, Projectile.width, segmentHeight), Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.position.Y + (k * segmentHeight) + tipHeight + segmentHeight / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.position + Vector2.UnitY * (k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, Projectile.width, (Projectile.height - tipHeight) % segmentHeight), Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.position.Y + (k * segmentHeight) + tipHeight + ((Projectile.height - tipHeight) % segmentHeight) / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);
            return false;
        }
    }

    public class Swordpocalypse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordpocalypse");
        }

        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.width = Projectile.height = 34;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
        }

        private int bladeLength = 124;
        private int bladeStart = 56;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 40; d++)
            {
                int lengthOffset = (Projectile.width / 2 - bladeLength) + Main.rand.Next(bladeLength);
                int widthOffset = +Main.rand.Next(Projectile.width) - Projectile.width / 2;
                Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(lengthOffset, Projectile.rotation) + QwertyMethods.PolarVector(widthOffset, Projectile.width + (float)Math.PI / 2), 15);
                dust.noGravity = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float CP = 0;
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-bladeLength + Projectile.width / 2 + bladeStart, Projectile.rotation), 15);
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(Projectile.width / 2, Projectile.rotation), 15);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(-bladeLength + Projectile.width / 2 + bladeStart, Projectile.rotation), Projectile.Center + QwertyMethods.PolarVector(Projectile.width / 2, Projectile.rotation), Projectile.width, ref CP);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width - Projectile.width / 2, texture.Height / 2), 1f, 0, 0);
            return false;
        }
    }
}