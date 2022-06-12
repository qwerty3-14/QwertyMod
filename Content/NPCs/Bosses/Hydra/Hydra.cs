using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Hydra;
using QwertyMod.Content.Items.Tool.FishingRod;
using QwertyMod.Content.Items.Weapon.Morphs.HydraBarrage;
using QwertyMod.Content.Items.Weapon.Magic.HydraBeam;
using QwertyMod.Content.Items.Weapon.Ranged.Gun;
using QwertyMod.Content.Items.Weapon.Minion.HydraHead;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Hydra;
using QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrent;
using QwertyMod.Content.Items.Tool.Mining;
using QwertyMod.Content.Items.Weapon.Magic.HydraMissile;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace QwertyMod.Content.NPCs.Bosses.Hydra
{
    public class Hydra : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 560;
            NPC.height = 250;
            NPC.damage = 0;
            NPC.defense = 18;
            NPC.boss = true;

            NPC.value = 60f;
            NPC.knockBackResist = 40;
            NPC.aiStyle = -1;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lifeMax = 12;
            NPC.immortal = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/BeastOfThreeHeads");
            }
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
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
            }
            for (int i = 0; i < 10; i++)
            {
                int dustType = 148;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public int damage = 30;
        private bool runOnce = true;

        public override bool PreAI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.ai[3] > 0)
            {
                NPC.dontTakeDamage = true;
                NPC.velocity = new Vector2(0, -10);
                if ((player.Center - NPC.Center).Length() > 1000f)
                {
                    NPC.life = 0;
                    NPC.checkDead();
                }
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].type == NPCType<HydraHead>() && Main.npc[n].active && Main.npc[n].ai[0] == NPC.whoAmI)
                    {
                        Main.npc[n].DeathSound = null;
                    }
                }
            }

            return !(NPC.ai[3] > 0);
        }

        public override void AI()
        {
            if (runOnce)
            {
                if (Main.expertMode)
                {
                    for (int p = 0; p < Main.player.Length; p++)
                    {
                        if (Main.player[p].active)
                        {
                            NPC.lifeMax += 4;
                        }
                    }
                    if(Main.masterMode)
                    {
                        NPC.lifeMax += (int)(NPC.lifeMax * 0.5f);
                    }

                    NPC.life = NPC.lifeMax;
                }
                for (int h = 0; h < 3; h++)
                {
                    if (Main.netMode != 1)
                    {
                        NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.Center.X + h, (int)NPC.Center.Y, NPCType<HydraHead>(), ai0: NPC.whoAmI, ai1: h);
                    }
                }
                runOnce = false;
            }

            Player player = Main.player[NPC.target];
            if (Main.netMode != 1)
            {
                player = Main.player[NPC.target];
                NPC.TargetClosest(true);
            }
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }

            Vector2 target = player.Center;
            Vector2 moveTo = target - NPC.Center;

            NPC.velocity = (moveTo) * .04f;
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


            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedHydra, -1);
        }
        
    }
}