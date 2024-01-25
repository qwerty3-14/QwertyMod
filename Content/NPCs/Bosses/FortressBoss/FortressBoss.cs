using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.FortressBoss;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Magic.RestlessSun;
using QwertyMod.Content.Items.Weapon.Melee.Misc;
using QwertyMod.Content.Items.Weapon.Minion.Priest;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.HolyExiler;
using QwertyMod.Content.NPCs.Fortress;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;

namespace QwertyMod.Content.NPCs.Bosses.FortressBoss
{
    [AutoloadBossHead]
    public class FortressBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "QwertyMod/Content/NPCs/Bosses/FortressBoss/FortressBoss_Bestiary",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 156;
            NPC.height = 128;
            NPC.damage = 10;
            NPC.defense = 8;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.value = 100000f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lifeMax = 3200;
            NPC.buffImmune[20] = true;
            NPC.npcSlots = 200;
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            //NPC.alpha = 0;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/HigherBeing");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The manifestation of Caelin in the mortal realm")
            });
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.LesserManaPotion;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(0.75f * NPC.lifeMax * bossAdjustment);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedDivineLight, -1);
        }

        public override bool CheckActive()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!player.InModBiome(ModContent.GetInstance<FortressBiome>()) || !player.active || player.dead)
            {
                return true;
            }
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<FortressBossBag>())); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<CaeliteMagicWeapon>(), ModContent.ItemType<HolyExiler>(), ModContent.ItemType<CaeliteRainKnife>(), ModContent.ItemType<PriestStaff>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);

            //Boss masks are spawned with 1/7 chance
            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DivineLightMask>(), 7));
            npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CaeliteBar>(), 1, 12, 20));
            npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CaeliteCore>(), 1, 6, 10));
            npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkywardHilt>(), 6));
            npcLoot.Add(notExpertRule);

            

            //Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FortressBossTrophy>(), 10));


			// ItemDropRule.MasterModeCommonDrop for the relic
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Consumable.Tiles.Relics.DivineLightRelic>()));

            base.ModifyNPCLoot(npcLoot);
        }

        private float upperLimit = 80 * 16;
        private float lowerLimit;
        private int fortressCenter;
        private int maxDistanceFromCenter;
        void SetupBoundries()
        {
            if (Main.dungeonX < Main.maxTilesX * .5f)
            {
                fortressCenter = (int)((double)Main.maxTilesX * 0.8) * 16;
            }
            else
            {
                fortressCenter = (int)((double)Main.maxTilesX * 0.2) * 16;
            }
            if (Main.maxTilesX > 8000)
            {
                lowerLimit = 280 * 16;
                maxDistanceFromCenter = 750 * 16;
            }
            else if (Main.maxTilesX > 6000)
            {
                lowerLimit = 230 * 16;
                maxDistanceFromCenter = 550 * 16;
            }
            else
            {
                lowerLimit = 130 * 16;
                maxDistanceFromCenter = 320 * 16;
            }
        }
        int[] armFrames = new int[] { 4, 4, 0, 0 };
        //Vector2[] spellPositions = new Vector2[] { new Vector2(20, 48), new Vector2(135, 48), new Vector2(18, 76), new Vector2(137, 76) };
        Vector2[] spellPositions = new Vector2[] { new Vector2(23, 36), new Vector2(132, 36), new Vector2(23, 68), new Vector2(132, 68) };
        bool[] drawSpell = new bool[] { false, false, false, false };
        float orbRotatior = 0f;
        float timer = 0f;
        Vector2 scale = new Vector2(0.5f, 2f);
        int spawnInTime = 300;
        int stretchTime = 90;
        int attackCounter = 0;
        Vector2 campSpot;
        int campDirection = 0;
        float campDistance = 2500f;
        float flySpeed = 20f;
        int[] attackOrder = new int[4];
        bool[] armAttacked;
        bool attackStarter = true;
        bool useBarrier = false;
        bool addSaw = false;
        int sawTimer = 0;
        int damage = 16;
        public override void AI()
        {
            
            orbRotatior += MathF.PI / 15f;
            for (int i = 0; i < 4; i++)
            {
                drawSpell[i] = false;
            }

            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead || !player.InModBiome(ModContent.GetInstance<FortressBiome>()))
            {
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                spawnInTime++;
                if(spawnInTime > 300)
                {
                    NPC.position.Y -= 10000;
                }
                if (stretchTime > 300 - spawnInTime)
                {
                    scale.X = 0.5f + 0.5f * ((300f - (float)spawnInTime) / stretchTime);
                    scale.Y = 2f - 1f * ((300f - (float)spawnInTime) / stretchTime);
                    NPC.alpha = (int)(255f * ((300f - (float)spawnInTime) / stretchTime));
                }
                else
                {
                    NPC.alpha = 255;
                    scale = Vector2.One;
                    ArmsVibing();
                }

                return;
            }
            if (spawnInTime > 0)
            {
                SpawnInAnimation();
            }
            else
            {
                NPC.alpha = 255;
                if (attackCounter == 0)
                {
                    FlyToNewCampSpot();
                }
                else
                {
                    if (useBarrier)
                    {
                        if (attackCounter == 1)
                        {
                            SetupBarrier();
                        }
                        else if (attackCounter == 2 || attackCounter == 5)
                        {
                            BarrierSpreadAttack();
                        }
                        else
                        {
                            BoltAttack();
                        }
                    }
                    else
                    {
                        if (attackCounter == 1 || attackCounter == 4)
                        {
                            BarrierSpreadAttack();
                        }
                        else
                        {
                            BoltAttack();
                        }
                    }
                    sawTimer++;
                    if (addSaw)
                    {
                        if (sawTimer % 150 == 0 || sawTimer % 180 == 0)
                        {
                            NPC.TargetClosest(false);
                            player = Main.player[NPC.target];
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, QwertyMethods.PolarVector(8f, (player.Center - NPC.Center).ToRotation()), ModContent.ProjectileType<CaeliteSaw>(), damage, 0);
                            }
                        }
                    }
                }
                timer += 2f - 1f * ((float)NPC.life / (float)NPC.lifeMax);
            }
            Lighting.AddLight(NPC.Center, new Vector3(1.2f, 1.2f, 1.2f));
            NPC.dontTakeDamage = NPC.alpha != 255;
            DrawDust();

        }
        void SetupBarrier()
        {
            if (attackStarter)
            {
                SpawnBarrier();
                attackStarter = false;
            }
            if (timer > 300)
            {
                ResetTimer();
            }
            else if (timer > 180)
            {
                for (int i = 0; i < 4; i++)
                {
                    SetArmToHoldPosition(i);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    SetArmToAttackPosition(i);
                }
            }
        }
        void SpawnBarrier()
        {
            for (int i = 0; i < 200; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<Deflect>())
                {
                    Main.projectile[i].Kill();
                }
            }
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 endPos = NPC.Center + QwertyMethods.PolarVector(130, (player.Center - NPC.Center).ToRotation() + ((float)i / 11f) * MathF.PI - MathF.PI / 2f);
                    Vector2 startPos = NPC.position + spellPositions[i / 3];
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), startPos, Vector2.Zero, ModContent.ProjectileType<Deflect>(), damage, 0, 255, endPos.X, endPos.Y)];
                    projectile.ai[0] = endPos.X;
                    projectile.ai[1] = endPos.Y;
                    projectile.timeLeft = (60 * 30) + (i % 3) * 30 + 60;
                }
            }
        }
        void BoltAttack()
        {
            if (attackStarter)
            {
                PlanAttackOrder();
                attackStarter = false;
            }
            int holdTime = 30;
            int attackSpeed = 45;
            int afterTime = 30;
            if (timer > holdTime)
            {
                for (int i = 0; i < 4; i++)
                {
                    int arm = attackOrder[i];
                    SetArmToHoldPosition(arm);
                    if (!armAttacked[arm] && timer >= holdTime + attackSpeed * i)
                    {
                        armAttacked[arm] = true;
                        ShootBolt(NPC.position + spellPositions[arm]);
                    }
                    else if (!armAttacked[arm] && timer >= holdTime + attackSpeed * i - 30)
                    {
                        SetArmToAttackPosition(arm);
                    }
                }
            }
            if (timer > holdTime + attackSpeed * 3 + afterTime)
            {
                ResetTimer();
            }
        }
        void ShootBolt(Vector2 position)
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            float vel = 12f;
            float angle = QwertyMethods.PredictiveAim(position, vel, player.Center, player.velocity);
            if (!float.IsNaN(angle))
            {
                SoundEngine.PlaySound(SoundID.Item43, position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), position, QwertyMethods.PolarVector(vel, angle), ModContent.ProjectileType<DivineBolt>(), damage, 0);
                }
            }
        }
        void BarrierSpreadAttack()
        {
            if (attackStarter)
            {
                PlanAttackOrder();
                attackStarter = false;
            }
            int holdTime = 90;
            int attackSpeed = 90;
            int afterTime = 0;
            if (timer > holdTime)
            {
                for (int i = 0; i < 4; i++)
                {
                    int arm = attackOrder[i];
                    if (!armAttacked[arm] && timer >= holdTime + attackSpeed * i)
                    {
                        armAttacked[arm] = true;
                        ShootSpread(NPC.position + spellPositions[arm]);
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (armAttacked[i])
                {
                    SetArmToHoldPosition(i);
                }
                else
                {
                    HoldingSpell(i);
                }
            }
            if (timer > holdTime + attackSpeed * 3 + afterTime)
            {
                ResetTimer();
            }
        }
        void ShootSpread(Vector2 position)
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                QwertyMethods.ProjectileSpread(NPC.GetSource_FromAI(), position, 3, 6f, ModContent.ProjectileType<BarrierSpread>(), damage, 0, 255, NPC.whoAmI, rotation: (player.Center - position).ToRotation(), spread: MathF.PI / 6);
            }
        }
        void PlanAttackOrder()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            float[] armDistances = new float[4];
            attackOrder = new int[] { -1, -1, -1, -1 };
            float max = 0;
            float min = 100000;
            for (int i = 0; i < 4; i++)
            {
                armDistances[i] = (player.Center - (NPC.position + spellPositions[i])).Length();
                if (armDistances[i] > max)
                {
                    max = armDistances[i];
                    attackOrder[3] = i;
                }
                if (armDistances[i] < min)
                {
                    min = armDistances[i];
                    attackOrder[0] = i;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (armDistances[i] != max && armDistances[i] != min)
                {
                    if (attackOrder[2] == -1)
                    {
                        attackOrder[2] = i;
                    }
                    else
                    {
                        if (armDistances[i] > attackOrder[2])
                        {
                            attackOrder[1] = attackOrder[2];
                            attackOrder[2] = i;
                        }
                        else
                        {
                            attackOrder[1] = i;
                        }
                    }
                }
            }
            armAttacked = new bool[] { false, false, false, false };
        }
        void ResetTimer()
        {
            attackCounter++;
            timer = 0;
            attackStarter = true;
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.netUpdate = true;
            }
            if (attackCounter > 6 + (useBarrier ? 1 : 0))
            {
                attackCounter = 0;
                if (Main.expertMode && (float)NPC.life / NPC.lifeMax < 0.6f)
                {
                    useBarrier = true;
                }
                if (Main.expertMode && (float)NPC.life / NPC.lifeMax < 0.3f)
                {
                    addSaw = true;
                }
            }
        }
        void FlyToNewCampSpot()
        {
            //QwertyMethods.ServerClientCheck("" + NPC.Center);
            if (attackStarter)
            {
                FindCampSpot();
                attackStarter = false;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.netUpdate = true;
                }
            }

            NPC.alpha = 150;
            for (int i = 0; i < 4; i++)
            {
                SetArmToHoldPosition(i);
            }
            NPC.velocity = QwertyMethods.PolarVector(flySpeed, (campSpot - NPC.Center).ToRotation());
            if (Math.Abs(campSpot.Y - NPC.Center.Y) < Math.Abs(NPC.velocity.Y))
            {
                NPC.velocity.Y = 0;
                NPC.Center = new Vector2(NPC.Center.X, campSpot.Y);
            }
            if (Math.Abs(NPC.velocity.Y) < .01f)
            {
                NPC.velocity.X = flySpeed * campDirection;
                if (Math.Sign(campSpot.X - NPC.Center.X) == campDirection * -1 && CampSpotOpen())
                {
                    ResetTimer();
                    NPC.velocity = Vector2.Zero;
                }
            }
            sawTimer = 0;
        }
        void FindCampSpot()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            campDirection = Math.Sign(player.Center.X - NPC.Center.X);
            campDistance = 1800f - 1000f * ((float)NPC.life / (float)NPC.lifeMax);
            if (player.Center.X + campDirection * campDistance > maxDistanceFromCenter + fortressCenter)
            {
                campDirection = -1;
            }
            if (player.Center.X + campDirection * campDistance < fortressCenter - maxDistanceFromCenter)
            {
                campDirection = 1;
            }
            campSpot = player.Center + (Vector2.UnitX * campDirection * campDistance);
            if (campSpot.Y < upperLimit)
            {
                campSpot.Y = upperLimit + 20 * 16;
            }
            if (campSpot.Y > lowerLimit)
            {
                campSpot.Y = lowerLimit - 20 * 16;
            }
            campDirection = Math.Sign(campSpot.X - NPC.Center.X);
        }
        bool CampSpotOpen()
        {
            for (int i = 0; i < 9; i++)
            {
                Vector2 checkSpot = NPC.position;
                switch (i)
                {
                    case 0:
                        checkSpot = NPC.TopLeft;
                        break;
                    case 1:
                        checkSpot = NPC.Top;
                        break;
                    case 2:
                        checkSpot = NPC.TopRight;
                        break;
                    case 3:
                        checkSpot = NPC.Right;
                        break;
                    case 4:
                        checkSpot = NPC.BottomRight;
                        break;
                    case 5:
                        checkSpot = NPC.Bottom;
                        break;
                    case 6:
                        checkSpot = NPC.BottomLeft;
                        break;
                    case 7:
                        checkSpot = NPC.Left;
                        break;
                    case 8:
                        checkSpot = NPC.Center;
                        break;
                }
                Point coords = checkSpot.ToTileCoordinates();
                if (Main.tile[coords.X, coords.Y].HasTile)
                {
                    return false;
                }
            }
            return true;
        }
        void DrawDust()
        {
            for (int i = 0; i < 4; i++)
            {
                if (drawSpell[i])
                {
                    Dust dust = Main.dust[Dust.NewDust(NPC.position + spellPositions[i] - Vector2.One * 16, 32, 32, ModContent.DustType<CaeliteDust>())];
                    dust.frame.Y = 0;
                    dust.scale = 0.4f;
                }
            }
        }
        void SpawnInAnimation()
        {
            if (spawnInTime == 300)
            {
                SetupBoundries();
            }
            if (stretchTime > 300 - spawnInTime)
            {
                scale.X = 0.5f + 0.5f * ((300f - (float)spawnInTime) / stretchTime);
                scale.Y = 2f - 1f * ((300f - (float)spawnInTime) / stretchTime);
                NPC.alpha = (int)(255f * ((300f - (float)spawnInTime) / stretchTime));
            }
            else
            {
                NPC.alpha = 255;
                scale = Vector2.One;
                ArmsVibing();
            }
            spawnInTime--;
        }
        void HoldingSpell(int i)
        {
            drawSpell[i] = true;
            SetArmToAttackPosition(i);
        }
        void SetArmToHoldPosition(int i)
        {
            armFrames[i] = (int)(NPC.frameCounter) / 10;
        }
        void SetArmToAttackPosition(int i)
        {
            SetArmToHoldPosition(i);
            armFrames[i] += 4;
        }
        //Classic animation
        void ArmsVibing()
        {
            for (int i = 0; i < 4; i++)
            {
                armFrames[i] = (int)(NPC.frameCounter) / 5 + (i >= 2 ? 4 : 0);
                if (armFrames[i] >= 8)
                {
                    armFrames[i] -= 8;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter < 10)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
            else if (NPC.frameCounter < 20)
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else if (NPC.frameCounter < 30)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.frameCounter < 40)
            {
                NPC.frame.Y = 3 * frameHeight;
            }
            else
            {
                NPC.frameCounter = 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            drawColor = new Color(NPC.alpha, NPC.alpha, NPC.alpha, NPC.alpha);
            Texture2D texture = TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, new Vector2(156 * 0.5f, 128 * 0.5f), scale, 0, 0);
            for (int i = 0; i < 4; i++)
            {
                string path = "QwertyMod/Content/NPCs/Bosses/FortressBoss/";
                switch (i)
                {
                    case 0:
                        path += "TopLeftArm";
                        break;
                    case 1:
                        path += "TopRightArm";
                        break;
                    case 2:
                        path += "BottomLeftArm";
                        break;
                    case 3:
                        path += "BottomRightArm";
                        break;
                }
                texture = ModContent.Request<Texture2D>(path).Value;
                spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle(0, armFrames[i] * 128, 156, 128), drawColor, NPC.rotation, new Vector2(156 * 0.5f, 128 * 0.5f), scale, 0, 0);
                if (drawSpell[i])
                {
                    texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/FortressBoss/SpellOrb").Value;
                    spriteBatch.Draw(texture, NPC.position + spellPositions[i] - screenPos, null, drawColor, orbRotatior, texture.Size() * .5f, scale, 0, 0);
                }
            }
            return false;
        }
        
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(attackStarter);
            writer.Write(campSpot.X);
            writer.Write(campSpot.Y);
            writer.Write(attackCounter);
            //writer.Write(NPC.position.X);
            //writer.Write(NPC.position.Y);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadSingle();
            attackStarter = reader.ReadBoolean();
            campSpot.X = reader.ReadSingle();
            campSpot.Y = reader.ReadSingle();
            attackCounter = reader.ReadInt32();
            //NPC.position.X = reader.ReadSingle();
            //NPC.position.Y = reader.ReadSingle();
        }

    }


}