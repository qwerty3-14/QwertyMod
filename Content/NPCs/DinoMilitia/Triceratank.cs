using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tile.Banners;
using QwertyMod.Content.Items.Equipment.Accessories;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.DinoMilitia
{
    public class Triceratank : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Triceratank");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 195;
            NPC.height = 98;

            NPC.damage = 100;
            NPC.defense = 40;
            NPC.lifeMax = 2000;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 6000f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 3;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/OldDinosNewGuns");
            AIType = 28;
            Banner = NPC.type;
            BannerItem = ItemType<TriceratankBanner>();

            //animationType = 3;
            NPC.buffImmune[BuffID.Confused] = false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (DinoEvent.EventActive)
            {
                return 35f;
            }
            else
            {
                return 0f;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("The Triceatops were at war with the Ankylosauruses for millions of years. There are no longer any Ankylosauruses only Triceratanks!")
            });
        }
        public override void OnKill()
        {
            if (DinoEvent.EventActive)
            {
                DinoEvent.DinoKillCount += 3;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustType = DustType<DinoSkin2>();
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public int AI_Timer = 0;
        public int damage = 30;
        public int walkTime = 300;

        public override void AI()
        {
            AI_Timer++;

            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);

            if (AI_Timer > walkTime)
            {
                //Projectile.NewProjectile(NPC.Center.X+(78f*NPC.direction), NPC.Center.Y-34f, 1f*NPC.direction, 0, 102, damage, 3f, Main.myPlayer);
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center.X + (78f * NPC.direction), NPC.Center.Y - 34f, 10f * NPC.direction, 0, ProjectileType<TankCannonBall>(), damage, 3f, Main.myPlayer);
                }

                AI_Timer = 0;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the NPC.direction.
            NPC.spriteDirection = NPC.direction;
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
            else if (NPC.frameCounter < 50)
            {
                NPC.frame.Y = 4 * frameHeight;
            }
            else if (NPC.frameCounter < 60)
            {
                NPC.frame.Y = 5 * frameHeight;
            }
            else
            {
                NPC.frameCounter = 0;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DinoTooth>(), 100, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemType<Tricerashield>(), 6, 1, 1));
        }
       
    }

    public class TankCannonBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Triceratank's cannon");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
        }

        public bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= .6f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 2f;
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1f;
                }
                runOnce = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.width = 75;
            Projectile.height = 75;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(0, 0), ProjectileType<TankCannonBallExplosion>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
        }
    }

    public class TankCannonBallExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank Cannon Ball Explosion");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 75;
            Projectile.height = 75;
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