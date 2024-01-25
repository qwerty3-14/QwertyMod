using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Invader;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Tool;
using QwertyMod.Content.Items.Weapon.Melee.Misc.FightKit;
using QwertyMod.Content.Items.Weapon.Minion.DVR;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SuperquantumRifle;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;



namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    //[AutoloadBossHead]
    public partial class InvaderBattleship : ModNPC
    {

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;

            NPCID.Sets.MPAllowedEnemies[NPC.type] = true; //For allowing use of SpawnOnPlayer in multiplayer

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Bestiary",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("An ancient civilization created a super advanced AI. Its first task 'Replace God'")
            });
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedBattleship, -1);
            SkyFortress.beingInvaded = false;
            string key = Language.GetTextValue(Mod.GetLocalizationKey("FortressBanished"));
            Color messageColor = Color.Green;
            if (Main.netMode == NetmodeID.Server) // Server
            {
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
            {
                Main.NewText(Language.GetTextValue(key), messageColor);
            }
            
            if(Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)ModMessageType.SetFortressInvasionStatus);
                packet.Write(SkyFortress.beingInvaded);
                packet.Write(SkyFortress.initalInvasion);
                packet.Send();
            }

        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 49000;
            NPC.width = 262;
            NPC.height = 92;
            NPC.value = 100000;
            NPC.damage = 172;
            NPC.noGravity = true;
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Accomplices");
            }
            NPC.knockBackResist = 0;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
        }
        

        public override bool CheckActive()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!player.InModBiome(ModContent.GetInstance<FortressBiome>()) || !player.active)
            {
                return true;
            }
            return false;
        }
        public override bool CheckDead()
        {
            if(Noehtnap != null && Noehtnap.ai[1] == 3)
            {
                NPC.dontTakeDamage = false;
                NPC.immortal = false;
                return true;
            }
            NPC.damage = 0;
            NPC.life = 1;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;
            doTransition = true;
            useMissile = false;
            useBeams = false;
            openDoor = false;
            strafeTimer = 0;
            return false;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(0.75f * NPC.lifeMax * bossAdjustment);
            NPC.damage = NPC.damage / 2;
            ApplyPieceDifficultyScaling(numPlayers, balance, bossAdjustment);
        }
        public bool phaseTwo = false;
        public const int normalDamage = 41;
        public const int expertDamage = 30;
        
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
            if(phaseTwo)
            {
                Phase2AI();
                return;
            }
            Phase1AI();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(!phaseTwo)
            {
                Vector2 offset  = Vector2.Zero;
                if(doTransition && strafeTimer > 60)
                {
                    offset = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                }
                Texture2D beamEmmitters = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipBeamEmmitters").Value;
                int height = beamEmmitters.Height / 2;
                float emmiterX =  NPC.position.X + (NPC.direction == 1 ? centerEyeX : (NPC.width - centerEyeX));
                spriteBatch.Draw(beamEmmitters, offset + new Vector2(emmiterX, NPC.position.Y + 4f + (1f - beamEmitterOutAmount) * height) - screenPos, new Rectangle(0, 0, beamEmmitters.Width, height), drawColor, NPC.rotation, new Vector2(beamEmmitters.Width / 2, height), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                spriteBatch.Draw(beamEmmitters, offset + new Vector2(emmiterX, NPC.Bottom.Y - (1f - beamEmitterOutAmount) * height) - screenPos, new Rectangle(0, height, beamEmmitters.Width, height), drawColor, NPC.rotation, new Vector2(beamEmmitters.Width / 2, 0), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

                Vector2 distressOffset = new Vector2((NPC.direction == 1 ? 71 : NPC.width - 71), 8);
                Texture2D distress = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Distress").Value;
                spriteBatch.Draw(distress, offset + NPC.position + distressOffset - screenPos, new Rectangle(0, distressFrame * 40, 22, 40), drawColor, NPC.direction == 1 ? distressRotation : (MathF.PI - distressRotation) + MathF.PI, new Vector2(11, 35), 1f, SpriteEffects.None, 0);
                if(launcher != null)
                {
                    launcher.Draw(spriteBatch, screenPos - offset, drawColor);
                }

                NoehtnapSpells.DrawBody(spriteBatch, offset + NPC.position + new Vector2(NPC.direction == 1 ? centerEyeX : NPC.width - centerEyeX, 52) - screenPos, drawColor, pupilDirection, pupilStareOutAmount, true);

                Texture2D garage = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_GarageDoor").Value;
                int garageTop = 30;
                int garageCenter = centerEyeX;
                int garageHeight = garage.Height;
                spriteBatch.Draw(garage, offset + new Vector2(NPC.direction == 1 ? NPC.Left.X + garageCenter : NPC.Right.X - garageCenter, NPC.Top.Y + garageTop) - screenPos, new Rectangle(0, garageHeight - (int)((float)garageHeight * garageDropAmount), garage.Width, (int)((float)garageHeight * garageDropAmount)), drawColor, NPC.rotation, Vector2.UnitX * garage.Width * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

                

                Texture2D texture = TextureAssets.Npc[NPC.type].Value;
                spriteBatch.Draw(texture, offset + NPC.Center - screenPos, null, drawColor, NPC.rotation, NPC.Size * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                Texture2D textureGlow = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Glow").Value;
                spriteBatch.Draw(textureGlow, offset + NPC.Center - screenPos, null, Color.White, NPC.rotation, NPC.Size * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                if(doTransition)
                {
                    Texture2D cracks = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Cracks").Value;
                    float prog = (float)strafeTimer / 180f;
                    if(prog > 1f)
                    {
                        prog = 1f;
                    }
                    int width = (int)((float)cracks.Width * prog);
                    int cHeight = (int)((float)cracks.Height * prog);
                    Rectangle frame = new Rectangle((cracks.Width / 2) - width / 2, (cracks.Height / 2) - cHeight / 2, width, cHeight);
                    spriteBatch.Draw(cracks, offset + NPC.Center - screenPos, frame, Color.White, NPC.rotation, new Vector2(width, cHeight) * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                }
                if(turrets[0] != null && turrets[1] != null)
                {
                    turrets[0].Draw(spriteBatch, screenPos - offset, drawColor);
                    turrets[1].Draw(spriteBatch, screenPos - offset, drawColor);
                }

            }
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(desperation);
            writer.Write(useMissile);
            writer.Write(doDistress);
            writer.Write(useBeams);
            writer.Write(gunTimer);
            writer.Write(strafeTimer);
            
            writer.Write(gunDebrislife[0]);
            writer.Write(gunDebrislife[1]);
            writer.Write(gunDebrislifeMax[0]);
            writer.Write(gunDebrislifeMax[1]);

            writer.Write(beamIndexes[0]);
            writer.Write(beamIndexes[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            desperation = reader.ReadBoolean();
            useMissile = reader.ReadBoolean();
            doDistress = reader.ReadBoolean();
            useBeams = reader.ReadBoolean();
            gunTimer = reader.ReadSingle();
            strafeTimer = reader.ReadInt32();

            gunDebrislife[0] = reader.ReadInt32();
            gunDebrislife[1] = reader.ReadInt32();
            gunDebrislifeMax[0] = reader.ReadInt32();
            gunDebrislifeMax[1] = reader.ReadInt32();

            beamIndexes[0] = reader.ReadInt32();
            beamIndexes[1] = reader.ReadInt32();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BattleshipBag>())); //this requires you to set BossBag in SetDefaults accordingly

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<SuperquantumRifle>(), ModContent.ItemType<DVRStaff>(), ModContent.ItemType<FightKit>()));
            //Finally add the leading rule
            //npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<InvaderPlating>(), 1, 80, 140));
            npcLoot.Add(notExpertRule);

            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<InvasionCaller>(), 1));
            npcLoot.Add(notExpertRule);

            //Boss masks are spawned with 1/7 chance
            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<InvaderMask>(), 7));
            npcLoot.Add(notExpertRule);

            //Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderBossTrophy>(), 10));


			// ItemDropRule.MasterModeCommonDrop for the relic
			//npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Consumable.Tiles.Relics.DivineLightRelic>()));

            base.ModifyNPCLoot(npcLoot);
        }
    }
    
}