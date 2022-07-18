using Microsoft.Xna.Framework;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.Items.Weapon.Minion.TileMinion;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class GuardTile : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guard Tile");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 64;
            NPC.aiStyle = -1;
            NPC.damage = 120;
            NPC.defense = 30;
            NPC.lifeMax = 480;
            NPC.value = 100;

            if (NPC.downedGolemBoss)
            {
                NPC.lifeMax = 960;
                NPC.damage = 160;
            }
            //NPC.alpha = 100;
            //NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            //NPC.dontTakeDamage = true;
            //NPC.scale = 1.2f;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[24] = true;
            Banner = NPC.type;
            BannerItem = ItemType<GuardTileBanner>();
            NPC.buffImmune[BuffID.Confused] = false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Enchanted tiles may fuse together if they think the threat to thier fortress is great enough.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = 180;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 90; i++)
                {
                    int dustType = DustType<FortressDust>();
                    int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                int dustType = DustType<FortressDust>(); ;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FortressBrick>(), 1, 7, 20));
            npcLoot.Add(ItemDropRule.Common(ItemType<TileStaff>(), 6, 1, 1));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(GetInstance<FortressBiome>()) && Main.hardMode)
            {
                return 40f;
            }
            return 0f;
        }

        private int direction = 0;
        private float speed = 10f;
        private float acceleration = 10f / 60f;
        private float maxAwareDistance = 1000f;
        private int timer;
        private int rushCooldown = 60;
        private int frame;
        private int frameTimer;

        public override void AI()
        {
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            NPC.GetGlobalNPC<FortressNPCGeneral>().contactDamageToInvaders = true;
            Entity player = FortressNPCGeneral.FindTarget(NPC, true);
            timer++;
            switch (direction)
            {
                case 0:
                    NPC.velocity = new Vector2(0, 0);
                    float point = 0f;
                    if (timer > rushCooldown)
                    {
                        NPC.dontTakeDamage = true;
                        frame = 6;
                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, NPC.Center, new Vector2(NPC.Center.X + maxAwareDistance, NPC.Center.Y), NPC.height, ref point) && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
                            direction = 1;
                            timer = 0;
                        }

                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, NPC.Center, new Vector2(NPC.Center.X - maxAwareDistance, NPC.Center.Y), NPC.height, ref point) && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
                            direction = 2;
                            timer = 0;
                        }
                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, NPC.Center, new Vector2(NPC.Center.X, NPC.Center.Y - maxAwareDistance), NPC.width, ref point) && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
                            direction = 3;
                            timer = 0;
                        }
                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, NPC.Center, new Vector2(NPC.Center.X, NPC.Center.Y + maxAwareDistance), NPC.width, ref point) && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                        {
                            direction = 4;
                            timer = 0;
                        }
                    }
                    break;

                case 1:
                    frame = 4;
                    NPC.dontTakeDamage = false;
                    NPC.velocity += new Vector2(acceleration * (NPC.confused ? -1 : 1), 0);
                    if (NPC.velocity.Length() > speed)
                    {
                        NPC.velocity = new Vector2(speed * (NPC.confused ? -1 : 1), 0);
                    }
                    if (timer > rushCooldown && (NPC.collideX || NPC.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;

                case 2:
                    frame = 2;
                    NPC.dontTakeDamage = false;
                    NPC.velocity += new Vector2(-acceleration * (NPC.confused ? -1 : 1), 0);
                    if (NPC.velocity.Length() > speed)
                    {
                        NPC.velocity = new Vector2(-speed * (NPC.confused ? -1 : 1), 0);
                    }
                    if (timer > rushCooldown && (NPC.collideX || NPC.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;

                case 3:
                    frame = 1;
                    NPC.dontTakeDamage = false;
                    NPC.velocity += new Vector2(0, -acceleration * (NPC.confused ? -1 : 1));
                    if (NPC.velocity.Length() > speed)
                    {
                        NPC.velocity = new Vector2(0, -speed * (NPC.confused ? -1 : 1));
                    }
                    if (timer > rushCooldown && (NPC.collideX || NPC.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;

                case 4:
                    frame = 3;
                    NPC.dontTakeDamage = false;
                    NPC.velocity += new Vector2(0, acceleration * (NPC.confused ? -1 : 1));
                    if (NPC.velocity.Length() > speed)
                    {
                        NPC.velocity = new Vector2(0, speed * (NPC.confused ? -1 : 1));
                    }
                    if (timer > rushCooldown && (NPC.collideX || NPC.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight * frame;
        }
    }
}