using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Minion.InvaderCaster;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.Invader
{
    public class InvaderCaster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 32;
            NPC.aiStyle = -1;
            NPC.damage = 100;
            NPC.defense = 20;
            NPC.lifeMax = 600;
            NPC.value = 5000;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/invcrawler_hurt1");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/invcrawler_death");
            NPC.knockBackResist = 0f;
            NPC.noGravity = false;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            shotCounter = 60;
        }
        int preTimer = 120;
        int shotCounter = 60;
        public override void AI()
        {
            NPC.damage = 0;
            if (preTimer > 0)
            {
                if (preTimer == 120)
                {
                    InvaderNPCGeneral.SpawnIn(NPC);
                    NPC.dontTakeDamage = true;
                }
                if (preTimer % 20 == 0)
                {
                    InvaderNPCGeneral.SpawnAnimation(NPC);
                }
                preTimer--;
            }
            else
            {
                if(Main.netMode != NetmodeID.Server && Main.rand.NextBool(12000))
                {
                    if(Main.rand.NextBool(2))
                    {
                        SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invcrawler_idle1"), NPC.Center);
                    }
                    else
                    {
                        SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invcrawler_idle2"), NPC.Center);
                    }
                }
                NPC.dontTakeDamage = false;
                shotCounter--;
                Entity target = InvaderNPCGeneral.FindTarget(NPC, true);
                Vector2 shootFrom = NPC.Top + Vector2.UnitY * 8;
                if (target != null && Collision.CanHitLine(shootFrom, 0, 0, target.Center, 0, 0) && (target.Center - NPC.Center).Length() < 600)
                {
                    float dist = Math.Abs(target.Center.X - NPC.Center.X);
                    if (dist < 100)
                    {
                        NPC.velocity.X = 0;
                        if (dist < 60)
                        {
                            NPC.direction *= -1;
                            InvaderNPCGeneral.WalkerWalk(NPC, 3);
                        }
                    }
                    else
                    {
                        InvaderNPCGeneral.WalkerWalk(NPC, 3);
                    }
                    if ((target.Center - shootFrom).Length() < 150 && shotCounter <= 0)
                    {
                        shotCounter = 20;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Item157, shootFrom);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), shootFrom, QwertyMethods.PolarVector(3, (target.Center - shootFrom).ToRotation()), ModContent.ProjectileType<InvaderZap>(), 20, 0, Main.myPlayer);
                        }
                    }
                    else if (shotCounter <= 0 && NPC.velocity.X == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Item157, shootFrom);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), shootFrom, QwertyMethods.PolarVector(5, (target.Center - shootFrom).ToRotation()), ModContent.ProjectileType<InvaderSphere>(), 20, 0, Main.myPlayer);
                        }
                        shotCounter = 180;
                    }
                }
                else
                {
                    NPC.velocity.X = 0;
                    InvaderNPCGeneral.WalkerIdle(NPC, 2);
                }

            }
            NPC.spriteDirection = NPC.direction;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return preTimer <= 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X != 0)
            {
                NPC.frameCounter++;
                if (NPC.frame.Y < frameHeight)
                {
                    NPC.frame.Y = frameHeight;
                }
                if (NPC.frameCounter % 6 == 0)
                {
                    NPC.frame.Y += frameHeight;
                    if (NPC.frame.Y >= frameHeight * 4)
                    {
                        NPC.frame.Y = 0;
                    }
                }
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

		int pulseCounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (preTimer > 0)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
            NPC.frame, drawColor, NPC.rotation,
            new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, NPC.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);


			pulseCounter++;

            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderCaster_Glow").Value, NPC.Center - screenPos,
            new Rectangle(0, ((pulseCounter % 40) / 10) * texture.Height / 4, texture.Width, texture.Height / 4), Color.White, NPC.rotation,
            new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, NPC.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The invaders use orbs to casts spells, they attach them to some feet to move them around.")
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderPlating>(), 1, 2, 4));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderCasterStaff>(), 20, 1, 1));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<FortressBiome>()) && SkyFortress.beingInvaded)
            {
                return 140f;
            }
            return 0f;
        }
    }
    public class InvaderZap : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 75;
            Projectile.extraUpdates = 74;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.GetGlobalProjectile<InvaderProjectile>().isInvaderProjectile = true;
            Projectile.GetGlobalProjectile<InvaderProjectile>().EvEMultiplier = 3f;
        }

        private bool decaying = false;
        private bool cantHit = false;

        private void StartDecay()
        {
            if (!decaying)
            {
                Projectile.extraUpdates = 0;
                Projectile.timeLeft = 10;
                Projectile.velocity = Vector2.Zero;
                decaying = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private Vector2 start;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                start = Projectile.Center;
            }
            if (Projectile.timeLeft == 2 && !decaying)
            {
                StartDecay();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!runOnce)
            {
                float rot = (Projectile.Center - (Vector2)start).ToRotation();
                int c = decaying ? (int)(255f * Projectile.timeLeft / 10f) : 255;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, start - Main.screenPosition, null, new Color(c, c, c, c), rot, Vector2.UnitY * 1, new Vector2((Projectile.Center - start).Length() / 2f, 1), SpriteEffects.None, 0);
            }
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (cantHit)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.immuneTime = 18;
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }
    }
    public class InvaderSphere : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.MagnetSphereBall);
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
        }
        int counter;
        public override void AI()
        {
            counter++;
            if (counter % 20 == 0)
            {
                Entity target = InvaderProjectile.FindTarget(Projectile, 150);
                if (target != null)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, QwertyMethods.PolarVector(3, (target.Center - Projectile.Center).ToRotation()), ModContent.ProjectileType<InvaderZap>(), 20, 0, Main.myPlayer);
                }
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 4 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.rotation += MathF.PI / 240f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
}