using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class Swarmer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Swarmer");
            Main.npcFrameCount[NPC.type] = 2;//number of frames, frames will be cut from your nps's png evenly vertically
        }

        public override void SetDefaults()
        {
            NPC.width = 28; //should be the same as your npc's frame width
            NPC.height = 28;//should be the same as your npc's frame height
            NPC.aiStyle = -1; // -1 is blank (we will write our own)
            NPC.damage = 20; // damage the enemy does on contact automaticly doubled in expert
            NPC.defense = 3; // defense of enemy
            NPC.lifeMax = 14; //maximum life doubled automaticly in expert
            NPC.value = 3; // how much $$ it drops
            NPC.HitSound = SoundID.NPCHit1; //sfx when hit
            NPC.DeathSound = SoundID.NPCDeath1; // sfx when killed
            NPC.knockBackResist = 0f; //knockback reducion 0 means it takes no knockback
            NPC.noGravity = true; // recommended for flying enemies
            NPC.npcSlots = 0.05f;
            NPC.noTileCollide = true;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.GetGlobalNPC<FortressNPCGeneral>().contactDamageToInvaders = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Fortress swarmers are native to Caelin's realm, despite not being that remarkable of a being on their own. They unkowingly wander into the mortal realm.")
            });
        }
        private float maxSpeed = 4;
        private float maxFriendRepelDistance = 50;
        private int freindCount = 0;
        private int totalCount;

        public override void AI()
        {
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (Main.expertMode)
            {
                NPC.lifeMax = 20;
                NPC.damage = 30;
                if (Main.hardMode)
                {
                    NPC.damage = 70;
                }
            }
            else
            {
                NPC.lifeMax = 14;
                NPC.damage = 15;
                if (Main.hardMode)
                {
                    NPC.damage = 35;
                }
            }
            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }

            NPC.TargetClosest(true);
            freindCount = 0;
            for (int n = 0; n < 200; n++)
            {
                if (n != NPC.whoAmI && Main.npc[n].active && Main.npc[n].type == NPCType<Swarmer>() && (Main.npc[n].Center - NPC.Center).Length() < 200)
                {
                    freindCount++;
                }
            }
            Entity player = FortressNPCGeneral.FindTarget(NPC, true);
            float towardsPlayer = (player.Center - NPC.Center).ToRotation();
            if (freindCount >= 4)
            {
                NPC.velocity = QwertyMethods.PolarVector(maxSpeed, towardsPlayer);
            }
            else
            {
                totalCount = 0;
                for (int n = 0; n < 200; n++)
                {
                    if (n != NPC.whoAmI && Main.npc[n].active && Main.npc[n].type == NPCType<Swarmer>())
                    {
                        totalCount++;
                    }
                }
                if (totalCount >= 4)
                {
                    for (int n = 0; n < 200; n++)
                    {
                        if (n != NPC.whoAmI && Main.npc[n].active && Main.npc[n].type == NPCType<Swarmer>() && (Main.npc[n].Center - NPC.Center).Length() > 150)
                        {
                            totalCount++;
                            NPC.velocity += QwertyMethods.PolarVector(4, (Main.npc[n].Center - NPC.Center).ToRotation());
                        }
                    }
                }
                else
                {
                    NPC.velocity = QwertyMethods.PolarVector(-maxSpeed, towardsPlayer);
                }
            }

            for (int n = 0; n < 200; n++)
            {
                if (n != NPC.whoAmI && Main.npc[n].active && Main.npc[n].type == NPCType<Swarmer>() && (Main.npc[n].Center - NPC.Center).Length() < maxFriendRepelDistance)
                {
                    NPC.velocity += QwertyMethods.PolarVector(-4 * (1 - (Main.npc[n].Center - NPC.Center).Length() / maxFriendRepelDistance), (Main.npc[n].Center - NPC.Center).ToRotation());
                }
            }

            if (NPC.velocity.Length() > maxSpeed)
            {
                NPC.velocity = NPC.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }

            if (NPC.velocity != Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, false, false))
            {
                maxSpeed = 1;
            }
            else
            {
                for (int p = 0; p < 1000; p++)//cycle through every projectile index
                {
                    float CollisionLineLength = Main.projectile[p].velocity.Length() * 60 * (1 + Main.projectile[p].extraUpdates); //this is basicly how far the selected projectile will travel in 1 second
                    float maxProjectileWidth = Main.projectile[p].Size.Length() * 5f; // fly further away from larger projectiles
                    float col = 0f;
                    if (Main.projectile[p].friendly && Main.projectile[p].active && Collision.CheckAABBvLineCollision(NPC.position, NPC.Size, Main.projectile[p].Center + QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2), Main.projectile[p].Center + QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2) + QwertyMethods.PolarVector(CollisionLineLength, Main.projectile[p].velocity.ToRotation()), maxProjectileWidth / 2f, ref col))
                    {
                        NPC.velocity += QwertyMethods.PolarVector(maxSpeed, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2);
                    }
                    else if (Main.projectile[p].friendly && Main.projectile[p].active && Collision.CheckAABBvLineCollision(NPC.position, NPC.Size, Main.projectile[p].Center - QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2), Main.projectile[p].Center - QwertyMethods.PolarVector(maxProjectileWidth / 4f, Main.projectile[p].velocity.ToRotation() + (float)Math.PI / 2) + QwertyMethods.PolarVector(CollisionLineLength, Main.projectile[p].velocity.ToRotation()), maxProjectileWidth / 2f, ref col))
                    {
                        NPC.velocity += QwertyMethods.PolarVector(maxSpeed, Main.projectile[p].velocity.ToRotation() - (float)Math.PI / 2);
                    }
                }
            }

            if (NPC.velocity.Length() > maxSpeed)
            {
                NPC.velocity = NPC.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            NPC.velocity *= (NPC.confused ? -1 : 1);
            NPC.rotation = NPC.velocity.ToRotation() + (float)Math.PI / 2;
            maxSpeed = 4;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) //changes spawn rates must return a float
        {
            return 0f;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) // this is run whenever the npc is hit by a projectile
        {
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight) // this part takes care of animations
        {
            NPC.frameCounter++;
            if (NPC.frameCounter % 4 == 0)
            {
                frame = frame == 0 ? 1 : 0;
            }

            NPC.frame.Y = frameHeight * frame;
        }
    }

    public class Swarm : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swarm");
            Main.npcFrameCount[NPC.type] = 2;//number of frames, frames will be cut from your nps's png evenly vertically
        }

        public override void SetDefaults()
        {
            NPC.width = 28; //should be the same as your npc's frame width
            NPC.height = 28;//should be the same as your npc's frame height
            NPC.aiStyle = -1; // -1 is blank (we will write our own)
            NPC.damage = 20; // damage the enemy does on contact automaticly doubled in expert
            NPC.defense = 3; // defense of enemy
            NPC.lifeMax = 25; //maximum life doubled automaticly in expert
            NPC.value = 3; // how much $$ it drops
            NPC.HitSound = SoundID.NPCHit1; //sfx when hit
            NPC.DeathSound = SoundID.NPCDeath1; // sfx when killed
            NPC.knockBackResist = 0f; //knockback reducion 0 means it takes no knockback
            NPC.noGravity = true; // recommended for flying enemies
            NPC.dontCountMe = true;
            NPC.noTileCollide = true;
            NPC.timeLeft = 2;
            Banner = NPC.type;
            BannerItem = ItemType<SwarmerBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(GetInstance<FortressBiome>()) && spawnInfo.Player.HeldItem.fishingPole <= 0)
            {
                return 5f;
            }
            return 0f;
        }

        private int swarmSize = 0;

        public override void AI()
        {
            if (Main.netMode != 1)
            {
                swarmSize = Main.rand.Next(14, 20);
                if (Main.hardMode)
                {
                    swarmSize *= 2;
                }
                for (int s = 0; s < swarmSize; s++)
                {
                    NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<Swarmer>());
                }
            }
            NPC.active = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
    }
}