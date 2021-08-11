using QwertyMod.Common.Fortress;
using QwertyMod.Content.Items.Consumable.Tile.Banners;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class FortressFlier : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Harpy");
            Main.npcFrameCount[NPC.type] = 4;//number of frames, frames will be cut from your nps's png evenly vertically
        }

        public override void SetDefaults()
        {
            NPC.width = 56; //should be the same as your npc's frame width
            NPC.height = 48;//should be the same as your npc's frame height
            NPC.aiStyle = -1; // -1 is blank (we will write our own)
            NPC.damage = 50; // damage the enemy does on contact automaticly doubled in expert
            NPC.defense = 28; // defense of enemy
            NPC.lifeMax = 330; //maximum life doubled automaticly in expert
            NPC.value = 100; // how much $$ it drops
            NPC.HitSound = SoundID.NPCHit1; //sfx when hit
            NPC.DeathSound = SoundID.NPCDeath1; // sfx when killed
            NPC.knockBackResist = 0f; //knockback reducion 0 means it takes no knockback
            NPC.noGravity = true; // recommended for flying enemies
            Banner = NPC.type;
            BannerItem = ItemType<FortressFlierBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Most harpies are forbidden from living in the fortress. However a few harpies have managed to adapt and become a part of the fortress's defenses.")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.InModBiome(GetInstance<FortressBiome>()) && Main.hardMode)
            {
                return 80f;
            }
            return 0f;
        }
        public override void HitEffect(int hitDirection, double damage)//run whenever enemy is hit should be used for visuals like gore
        {
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FortressHarpyBeak>(), 1));
        }

        //here I set variables that the AI uses
        private float flySpeed = 6f;

        private bool clinged = false;
        private int timer;
        private float playerDistance;
        private bool runOnce = true;
        private int faceDirection = 1;
        private int frame;
        private int frameTimer;
        private int damage = 24;
        private float verticalSpeed = 3;
        private float verticalFlightTimer;
        private int attackTimer;

        public override void AI() //this will run every frame
        {
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (runOnce)
            {
                //I put stuff here I want to only run once
                runOnce = false;
            }
            Player player = Main.player[NPC.target]; // sets the variable player needed to locate the player
            NPC.TargetClosest(true); // give the npc a target

            playerDistance = (player.Center - NPC.Center).Length(); // finds the distance between this enemy and player

            timer++;
            if (clinged) //run when the enemy is clinged to the wall
            {
                NPC.velocity.X = 0; // set velocity to 0 (no movement)
                NPC.velocity.Y = 0;// set velocity to 0 (no movement)
                if (timer > 10 && playerDistance < 200 && Collision.CanHit(NPC.Center, 0, 0, player.Center, 0, 0)) // this checks if the player is too close and not behind tiles, the timer is so it doesn't immediatly stick to a wall it jumps off
                {
                    clinged = false; // stop sticking to the wall
                    timer = 0; //reset timer
                }
                else if (Collision.CanHit(NPC.Center, 0, 0, player.Center, 0, 0) && playerDistance < 600) // this checks if the player is close but not too close and not behind tiles
                {
                    attackTimer++; // this timer is used so the attack isn't every frame
                    if (attackTimer >= 60) //this will be true when the timer is above 60 frames (1 second)
                    {
                        float shootDirection = (player.Center - NPC.Center).ToRotation(); // find the direction the player is in
                        for (int p = -1; p < 2; p++) //this will repeat 3 times for 3 projectiles
                        {
                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center, QwertyMethods.PolarVector(6, shootDirection + ((float)Math.PI / 8 * p)), ProjectileType<FortressHarpyProjectile>(), damage, player.whoAmI); // shoots a projectile
                        }
                        attackTimer = 0; // resets attackTimer needer for the once per second effect
                    }
                    else if (attackTimer > 45)  //this will be true when the timer is above 45 frames (.75 seconds)
                    {
                        frame = 3; // change the frame to signal it's about to attack
                    }
                    else
                    {
                        frame = 2; // change the frame to normal cling
                    }
                }
                else // player too far away or can't be seen
                {
                    attackTimer = 0;
                    frame = 2;
                }
            }
            else
            {
                //this is for flying animaion it cycles through 2 frames
                frameTimer++;
                if (frameTimer % 10 == 0) // true every 10th frame the % gives the remainder of the division problem frameTimer/10
                {
                    if (frame == 0)
                    {
                        frame = 1;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
                //////////////

                verticalFlightTimer += (float)Math.PI / 60; //add this amount to the flight timer every frame, it is used as a radian value so this means it will go 180 degrees every second
                NPC.velocity.Y = (float)Math.Cos(verticalFlightTimer) * verticalSpeed; //the up and down flying motion uses a cosine function,
                                                                                       //It is based on a sine wave on a graph as x continually increases Y goes from 1 - -1
                                                                                       //Cosine is the derivitive of Sine the harpy flies in a sine wave pattern
                                                                                       //Vertical speed increases the speed it flies up and down othersie it'll just range from 1 - -1
                NPC.velocity.X = flySpeed * faceDirection; // much simpler than the vertical movement this simply moves in the direction of face direction at the flySpeed
                if (NPC.collideX && timer > 10)
                {
                    faceDirection *= -1; //flips the direction it faces
                    clinged = true; //start clinging to the wall
                    timer = 0; // reset pattern
                }
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) // this is run whenever the npc is hit by a projectile
        {
            if (playerDistance > 600) //this checks the distance, it will make it fly away if it's getting 'sniped
            {
                clinged = false;
                timer = 0;
            }
            else if (Main.rand.Next(5) == 0) // if the player is in 'valid' range it will randomly fly away
            {
                clinged = false;
                timer = 0;
            }
        }

        public override void FindFrame(int frameHeight) // this part takes care of animations
        {
            NPC.spriteDirection = faceDirection;
            NPC.frame.Y = frameHeight * frame;
        }
    }

    public class FortressHarpyProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Harpy Projectile");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
        }

        public int dustTimer;

        public override void AI()
        {
        }
    }
}