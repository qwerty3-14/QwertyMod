using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Equipment.Vanity.RunicRobe;
using QwertyMod.Content.Items.MiscMaterials;
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

namespace QwertyMod.Content.NPCs.Bosses.RuneGhost
{
    [AutoloadBossHead]
    public class RuneGhost : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Ghost");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 90;
            NPC.damage = 0;
            NPC.defense = 42;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.damage = 10;

            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.alpha = 0;
            NPC.lifeMax = 60000;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheConjurer");
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
				new FlavorTextBestiaryInfoElement("An ancient powerful sorcerer sought Godhood. Although he remains a powerful being long after death he is still tied to the mortal realm. Was this a failure? or had the sourcerer changed his mind on Godhood?")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
        }
        public override bool CheckActive()
        {
            return despawn;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        bool casting = false;
        float castingSpeed = 1f;
        int frame = 0;
        float castTime = 0f;
        bool drawRune = true;
        Vector2 goTo;
        bool runOnce = true;
        float goToYOffset = -150;
        float flightTime = 0;
        float flightTimeMax = 60;
        int phase = 0;
        bool despawn = false;
        int lastRune = 5;
        void StartMoving()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            phase = 0;
            if (Main.expertMode)
            {
                castingSpeed = 1f + 1f - ((float)NPC.life / (float)NPC.lifeMax);
                if ((((float)NPC.life / NPC.lifeMax) < .5f))
                {
                    phase++;
                }
            }
            if (Main.netMode != 1)
            {
                goTo = player.Center + Vector2.UnitY * goToYOffset + Vector2.UnitX * Main.rand.NextFloat(-400, 400);
                NPC.velocity = castingSpeed * ((goTo - NPC.Center) / flightTimeMax);
                NPC.netUpdate = true;
            }

            flightTime = flightTimeMax;
            goToYOffset *= -1;

        }
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead || despawn)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];

                if (!player.active || player.dead || despawn)
                {
                    despawn = true;
                    casting = false;
                    NPC.velocity = Vector2.UnitX * 10f;
                    if (flightTime > 90)
                    {
                        flightTime--;
                    }
                    else
                    {
                        NPC.position = new Vector2(100000, 0);
                    }
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            else
            {
                if (runOnce)
                {
                    StartMoving();
                    runOnce = false;
                }
                NPC.dontTakeDamage = !casting;
                if (casting)
                {
                    NPC.velocity = Vector2.Zero;
                    castTime += castingSpeed;
                    #region calculate frame
                    frame = 0;
                    if (castTime >= 15)
                    {
                        frame++;
                    }
                    if (castTime >= 30)
                    {
                        frame++;
                    }
                    if (castTime >= 45)
                    {
                        frame++;
                    }
                    if (castTime >= 60)
                    {
                        frame++;

                        frame += (((int)castTime - 60) / 15) % 4;
                    }
                    #endregion

                    if (castTime >= 60 && drawRune)
                    {
                        drawRune = false;
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < phase + 1; i++)
                            {
                                Projectile rune = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Top + QwertyMethods.PolarVector(-120, (float)Math.PI * ((float)(i + 1) / (phase + 2))), Vector2.Zero, ProjectileType<BigRune>(), Main.expertMode ? 30 : 40, 0, Main.myPlayer)];

                                int newRune = lastRune == 5 ? Main.rand.Next(4) : Main.rand.Next(3);
                                if (newRune >= lastRune)
                                {
                                    newRune++;
                                }
                                lastRune = newRune;
                                rune.ai[0] = lastRune;
                                rune.ai[1] = castingSpeed;
                                rune.netUpdate = true;
                            }
                        }

                    }
                    if (castTime >= 300)
                    {
                        casting = false;
                        StartMoving();
                        frame = 0;
                        castTime = 0;
                    }
                }
                else
                {
                    flightTime -= castingSpeed;
                    if (flightTime < 0)
                    {

                        drawRune = true;
                        casting = true;
                        castTime = 0;
                        if (Main.netMode != 1)
                        {
                            NPC.netUpdate = true;
                        }
                    }
                }
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (casting)
            {
                Texture2D texture = TextureAssets.Npc[NPC.type].Value;
                spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle(0, 90 * frame, 82, 90), Color.White, NPC.rotation, new Vector2(41, 45), Vector2.One, 0, 0);
            }
            else
            {
                Texture2D texture = RuneSprites.runeGhostMoving;
                int phaseTime = (int)(flightTimeMax / 3f);
                int phaseFrame = (int)((float)RuneSprites.runeGhostPhaseIn.Length * (float)((int)flightTime % phaseTime) / phaseTime);
                if (phaseFrame > RuneSprites.runeGhostPhaseIn.Length - 1)
                {
                    phaseFrame = RuneSprites.runeGhostPhaseIn.Length - 1;
                }
                if (flightTime < phaseTime)
                {
                    float c = 1f - (flightTime / (float)phaseTime);
                    spriteBatch.Draw(RuneSprites.runeGhostPhaseIn[19 - phaseFrame], NPC.Center - screenPos, null, new Color(c, c, c, c), NPC.rotation, texture.Size() * .5f, Vector2.One * 2, NPC.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
                if (flightTime > flightTimeMax - phaseTime)
                {
                    float c = (flightTime / (float)phaseTime);
                    spriteBatch.Draw(RuneSprites.runeGhostPhaseIn[phaseFrame], NPC.Center - screenPos, null, new Color(c, c, c, c), NPC.rotation, texture.Size() * .5f, Vector2.One * 2, NPC.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ItemType<RuneGhostBag>())); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<CraftingRune>(), 1, 25, 36));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);

            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<AggroScroll>(), ItemType<PursuitScroll>(), ItemType<IceScroll>(), ItemType<LeechScroll>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);


            //Boss masks are spawned with 1/7 chance
            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<RuneGhostMask>(), 7));
            npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<RunicRobe>(), 7));
            npcLoot.Add(notExpertRule);

            

			// ItemDropRule.MasterModeCommonDrop for the relic
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Consumable.Tiles.Relics.RuneGhostRelic>()));

            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedRuneGhost, -1);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(NPC.velocity);
            writer.WriteVector2(NPC.position);
            writer.Write(flightTime);
            writer.Write(castTime);
            writer.Write(castingSpeed);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {

            NPC.velocity = reader.ReadVector2();
            NPC.position = reader.ReadVector2();
            flightTime = reader.ReadSingle();
            castTime = reader.ReadSingle();
            castingSpeed = reader.ReadSingle();
        }
    }
}
