using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using QwertyMod.Content.Items.Equipment.Accessories;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.DinoVulcan;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.DinoMilitia
{
    public class Velocichopper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Velocichopper");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 286;
            NPC.height = 104;

            NPC.damage = 90;
            NPC.defense = 30;
            NPC.lifeMax = 10000;

            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 6000f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            //aiType = 86;
            //animationType = 3;
            NPC.buffImmune[BuffID.Confused] = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/OldDinosNewGuns");
            }
            Banner = NPC.type;
            BannerItem = ItemType<VelocichopperBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("The velociraptor's relations with the pterasaurs have been... mixed, so they've created a machine to put them in the air!")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (DinoEvent.EventActive && !NPC.AnyNPCs(NPCType<Velocichopper>()) && !NPC.AnyNPCs(NPCType<TheGreatTyrannosaurus>()))
            {
                return 7f;
            }
            return 0f;
        }
        public override void OnKill()
        {
            if (DinoEvent.EventActive)
            {
                DinoEvent.DinoKillCount += 10;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }
        public int Pos = 1;
        public int damage = 40;
        public int reloadTime = 2;
        public int moveCount = 0;
        public int fireCount = 0;
        public int attackType = 1;
        public int AI_Timer = 0;
        public int Reload_Timer = 0;
        public int attackTime = 300;
        public int numberOfShots = 0;
        public int rushDirection = 1;
        public int bombTimer;
        public int bombReload = 30;

        public override void AI()
        {
            AI_Timer++;

            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);

            if (AI_Timer > 481)
            {
                bombTimer++;
                NPC.direction = rushDirection;
                NPC.velocity = new Vector2(10 * rushDirection, 0f);
                if ((NPC.Center.X > player.Center.X + 1200 && rushDirection == 1) || (NPC.Center.X < player.Center.X - 1200 && rushDirection == -1) && Main.netMode != 1)
                {
                    AI_Timer = 0;
                    NPC.netUpdate = true;
                }
                if (bombTimer > bombReload && Main.netMode != 1)
                {
                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X, NPC.Center.Y, 0, 0, ProjectileType<DinoBomb>(), damage, 3f, Main.myPlayer);
                    bombTimer = 0;
                }
            }
            else if (AI_Timer > 480)
            {
                if (NPC.Center.X < player.Center.X)
                {
                    rushDirection = 1;
                }
                else
                {
                    rushDirection = -1;
                }
            }
            else if (AI_Timer > 420)
            {
                Vector2 moveTo = new Vector2(player.Center.X - (900f * NPC.direction), player.Center.Y + -300f) - NPC.Center;
                NPC.velocity = (moveTo) * .03f;
            }
            else if (AI_Timer > attackTime)
            {
                NPC.velocity = new Vector2(0, 0f);

                Reload_Timer++;
                if (Reload_Timer > reloadTime && Main.netMode != 1)
                {
                    int Xvar = Main.rand.Next(-50, 50);

                    int Yvar = 50 - Xvar;

                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (100f * NPC.direction), NPC.Center.Y, 5.00f * (1 + Xvar * .01f) * NPC.direction, 5.00f * (1 + Yvar * .01f), 110, damage, 3f, Main.myPlayer);

                    Reload_Timer = 0;
                }
            }
            else if (AI_Timer > attackTime - 120)
            {
                NPC.velocity = new Vector2(0, 0f);
            }
            else
            {
                Vector2 moveTo = new Vector2(player.Center.X - (300f * NPC.direction), player.Center.Y + -300f) - NPC.Center;
                NPC.velocity = (moveTo) * .03f;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(AI_Timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AI_Timer = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the NPC.direction.
            NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (NPC.frameCounter < 1)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
            else if (NPC.frameCounter < 2)
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else if (NPC.frameCounter < 3)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.frameCounter < 4)
            {
                NPC.frame.Y = 3 * frameHeight;
            }
            if (NPC.frameCounter < 5)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.frameCounter < 6)
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else if (NPC.frameCounter < 7)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
            else
            {
                NPC.frameCounter = 0;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DinoTooth>(), 100, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemType<DinoVulcan>(), 6, 1, 1));
        }
        
    }

    public class DinoBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DinoBomb");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;

            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
        }

        public bool runOnce = true;

        public override void AI()
        {
            Projectile.timeLeft = 2;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(0, 0), ProjectileType<DinoBombExplosion>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            for (int i = 0; i < 100; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 160; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
        }
    }

    public class DinoBombExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dino Bomb Explosion");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}