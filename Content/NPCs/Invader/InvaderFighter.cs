using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Invader
{
    public class InvaderFighter : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Fighter");
        }
        public override void SetDefaults()
        {
            NPC.width = NPC.height = 44;
            NPC.aiStyle = -1;
            NPC.damage = 100;
            NPC.defense = 12;
            NPC.lifeMax = 720;
            NPC.value = 5000;
            //NPC.alpha = 100;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
        }
        int combatTimer = 0;
        bool firedPlasma = false;
        public override void AI()
        {
            NPC.damage = 0;
            Entity target = InvaderNPCGeneral.FindTarget(NPC, false);
            if (combatTimer < 0)
            {
                combatTimer++;
                Dust.NewDustPerfect(NPC.Center + QwertyMethods.PolarVector(-21, NPC.rotation), ModContent.DustType<InvaderGlow>(), Vector2.Zero);
            }
            else if (combatTimer <= 240)
            {
                NPC.velocity = Vector2.Zero;
                combatTimer++;
                if (combatTimer % 60 == 30)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        SoundEngine.PlaySound(SoundID.Item95, NPC.Center);
                        Vector2 shootFrom = NPC.Center + QwertyMethods.PolarVector(9, NPC.rotation) + QwertyMethods.PolarVector(9 * (combatTimer % 120 == 30 ? -1 : 1), NPC.rotation + (float)Math.PI / 2f);
                        Projectile.NewProjectile(new EntitySource_Misc(""), shootFrom, (shootFrom - NPC.Center).SafeNormalize(-Vector2.UnitY) * 8, ModContent.ProjectileType<InvaderMicroMissile>(), 30, 0, 0);
                    }
                }
                if (target != null)
                {
                    NPC.rotation.SlowRotation((target.Center - NPC.Center).ToRotation(), (float)Math.PI / 30f);
                }
            }
            else
            {
                NPC.velocity = Vector2.Zero;
                if (firedPlasma)
                {
                    combatTimer++;
                    if (combatTimer > 360 && target != null)
                    {
                        combatTimer = -60;
                        firedPlasma = false;
                        NPC.velocity = QwertyMethods.PolarVector(20, NPC.rotation);
                    }
                }
                else if (target != null)
                {
                    float shotOffset = 28;
                    float shotSpeed = 8;
                    float aimAt = QwertyMethods.PredictiveAimWithOffset(NPC.Center, shotSpeed * 3, target.Center, target.velocity, shotOffset);
                    if (!float.IsNaN(aimAt))
                    {
                        float rotOld = NPC.rotation;
                        NPC.rotation.SlowRotation(aimAt, (float)Math.PI / 30f);
                        if (QwertyMethods.AngularDifference(rotOld, NPC.rotation) < (float)Math.PI / 30f)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
                                Vector2 shootFrom = NPC.Center + QwertyMethods.PolarVector(8, NPC.rotation) + QwertyMethods.PolarVector(8 * (combatTimer % 120 == 30 ? -1 : 1), NPC.rotation + (float)Math.PI / 2f);
                                Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center + QwertyMethods.PolarVector(shotOffset, NPC.rotation), QwertyMethods.PolarVector(shotSpeed, NPC.rotation), ModContent.ProjectileType<InvaderFighterBlast>(), 30, 0, 0);
                            }
                            firedPlasma = true;
                        }
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
            NPC.frame, drawColor, NPC.rotation,
            new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, TextureAssets.Npc[NPC.type].Value.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderFighter_Glow").Value, NPC.Center - screenPos,
            NPC.frame, drawColor, NPC.rotation,
            new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, TextureAssets.Npc[NPC.type].Value.Height * 0.5f), 1f, SpriteEffects.None, 0f);
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

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<FortressBiome>()) && NPC.downedGolemBoss)
            {
                return 90f;
            }
            return 0f;
        }
    }
    public class InvaderMicroMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Fighter");
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 240;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.GetGlobalProjectile<InvaderProjectile>().isInvaderProjectile = true;
        }
        bool exploded = false;
        void explode()
        {
            if (!exploded)
            {
                exploded = true;
                Projectile.timeLeft = 5;
                Projectile.width = 15;
                Projectile.height = 15;
                Projectile.position -= Vector2.One * 12;
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 30; i++)
                {
                    float rot = (float)Math.PI * 2f * ((float)i / 30f);
                    Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<InvaderGlow>(), QwertyMethods.PolarVector(3f, rot));
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            explode();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            explode();
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
            explode();
        }
        bool runOnce = true;
        public override void AI()
        {
            if (!exploded)
            {
                if (runOnce)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    runOnce = false;
                }
                Entity target = InvaderProjectile.FindTarget(Projectile, 600);
                if (target != null)
                {
                    Projectile.rotation.SlowRotation((target.Center - Projectile.Center).ToRotation(), (float)Math.PI / 120f);
                }
                Projectile.velocity = QwertyMethods.PolarVector(6, Projectile.rotation);
                Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-4, Projectile.rotation), ModContent.DustType<InvaderGlow>(), Vector2.Zero, Scale: 0.2f);
                if (Projectile.timeLeft < 5)
                {
                    explode();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (exploded)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        Projectile.Size * 0.5f, 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderMicroMissile_Glow").Value, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), Color.White, Projectile.rotation,
                        Projectile.Size * 0.5f, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
    public class InvaderFighterBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Fighter");
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 75;
            Projectile.extraUpdates = 74;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.GetGlobalProjectile<InvaderProjectile>().isInvaderProjectile = true;
            Projectile.extraUpdates = 2;
        }
        bool runOnce = true;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }
    }
}