using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion.DVR
{
    public class DVR : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 10;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        float useless = 0;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(timer < 0)
            {
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(4000, Projectile.rotation), 28f, ref useless);
            }
            return false;
        }
        float armOutRotation = 0;
        int reloadTime = 12 * 60;
        int timer = 0;
        float trigCounter = 0;
        int lightsLit = -1;
        const float firingPosition = 0.6f;
        float chainOutAmount = firingPosition;
        float portalScale = 0;
        bool loaded = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            trigCounter += MathF.PI / 30;
            if (player.GetModPlayer<MinionManager>().DVRMinion)
            {
                Projectile.timeLeft = 2;
            }
            if(timer < 0)
            {
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.velocity = (player.Center + new Vector2(player.direction * -1 * (70 + (MinionManager.GetIdentity(Projectile) * 100)), -70) - Projectile.Center) * 0.1f;
            }
            if(timer < reloadTime)
            {
                timer++;
                if(timer < 0)
                {

                }
                else
                {
                    float animProgress = (float)timer / (float)reloadTime;
                    SetAnimation(animProgress);
                    Projectile.rotation.SlowRotation(MathF.PI / -2f, MathF.PI / 120);
                }
                
            }
            else
            {
                List<NPC> targets = new List<NPC>();
                if(QwertyMethods.NPCsInRange(ref targets, 4000, Projectile.Center, true))
                {
                    int mostHealth = 0;
                    int healthiest = -1;
                    for(int i = 0; i < targets.Count; i++)
                    {
                        if(targets[i].life > mostHealth)
                        {
                            mostHealth = targets[i].life;
                            healthiest = i;
                        }
                    }
                    if(healthiest != -1)
                    {
                        if(Projectile.rotation.SlowRotation((targets[healthiest].Center - Projectile.Center).ToRotation(), MathF.PI / 120))
                        {
                            timer = -30;
                            loaded = false;
                            lightsLit = -1;
                            SoundEngine.PlaySound(SoundID.Item88 with {Pitch = 0.4f}, player.Center);
                        }
                    }
                }
                else
                {
                    Projectile.rotation.SlowRotation(MathF.PI / -2f, MathF.PI / 120);
                }
            }
        }
        void SetAnimation(float progress)
        {
            portalScale = 0;
            if(progress <= 0.1f)
            {
                OpenArms(progress * 10f);
            }
            else if(progress > 0.1f && progress <= 0.5f)
            {
                LightBulbs((progress - 0.1f) / 0.4f);
            }
            else if(progress > 0.5f && progress <= 0.8f)
            {
                lightsLit = 6;
                MoveArmIntoPortal((progress - 0.5f) / 0.3f);
            }
            else if(progress > 0.8f && progress <= 0.9f)
            {
                MoveArmOutOfPortal((progress - 0.8f) / 0.1f);
            }
            else if(progress > 0.9f)
            {
                CloseArms((progress - 0.9f) * 10f);
            }
        }
        void OpenArms(float progress)
        {
            armOutRotation = progress * MathF.PI / 3f;
            chainOutAmount = firingPosition * (1f - progress);
        }
        void CloseArms(float progress)
        {
            armOutRotation = (1f - progress) * MathF.PI / 3f;
        }
        void LightBulbs(float progress)
        {
            lightsLit = (int)(progress * 6);
        }
        void MoveArmIntoPortal(float progress)
        {
            portalScale = MathF.Min(progress * 3, 1f);
            chainOutAmount = progress;
        }
        void MoveArmOutOfPortal(float progress)
        {
            portalScale = (1f - progress);
            loaded = true;
            chainOutAmount = firingPosition + (1f - firingPosition) * (1f - progress);
        }
        void BulbOrientation(int index, out Vector2 location, out float rotation)
        {
            rotation = 0;
            location = Projectile.Center;
            if(index % 2 == 0)
            {
                rotation = Projectile.rotation + armOutRotation;
            }
            else
            {
                rotation = (Projectile.rotation - armOutRotation) + MathF.PI;
            }
            switch(index)
            {
                case 0:
                    location = Projectile.Center + new Vector2(78, 10).RotatedBy(Projectile.rotation + armOutRotation);
                break;
                case 1:
                    location = Projectile.Center + new Vector2(78, -10).RotatedBy(Projectile.rotation - armOutRotation);
                break;
                case 2:
                    location = Projectile.Center + new Vector2(58, 10).RotatedBy(Projectile.rotation + armOutRotation);
                break;
                case 3:
                    location = Projectile.Center + new Vector2(58, -10).RotatedBy(Projectile.rotation - armOutRotation);
                break;
                case 4:
                    location = Projectile.Center + new Vector2(38, 10).RotatedBy(Projectile.rotation + armOutRotation);
                break;
                case 5:
                    location = Projectile.Center + new Vector2(38, -10).RotatedBy(Projectile.rotation - armOutRotation);
                break;
            }
        }
        int chargerCyclerCounter = 0;
        int beamCycletCounter = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            float linkLength = (Projectile.Center - player.Center).Length();
            float rot = (Projectile.Center - player.Center).ToRotation();
            for(int l = 0; l < linkLength; l += 4)
            {
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/PlayerAttach").Value, player.Center + QwertyMethods.PolarVector(l, rot) - Main.screenPosition, null, lightColor, rot, new Vector2(0, 6), 1f, SpriteEffects.None);
            }

            Texture2D leftArmTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/LeftArm").Value;
            Main.EntitySpriteDraw(leftArmTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - armOutRotation, Vector2.One * 24f, 1f, SpriteEffects.None);

            Texture2D rightArmTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/RightArm").Value;
            Main.EntitySpriteDraw(rightArmTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + armOutRotation, Vector2.One * 24f, 1f, SpriteEffects.None);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, Vector2.One * 24f, 1f, SpriteEffects.None);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DVR_Glow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Vector2.One * 24f, 1f, SpriteEffects.None);
            
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/ShadowPortal").Value, Projectile.Center + QwertyMethods.PolarVector(79, Projectile.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(103, 24), portalScale, SpriteEffects.None);

            int chainWidth = (int)(88 * chainOutAmount);
            int cropAt = 65;
            int cutOff = 0;
            if(chainWidth > cropAt)
            {
                cutOff = chainWidth - cropAt;
            }
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/GrabChain").Value, 
            Projectile.Center + QwertyMethods.PolarVector(14, Projectile.rotation) - Main.screenPosition, new Rectangle(88 - chainWidth, (loaded ? 16 : 0), chainWidth - cutOff, 16), lightColor, 
            Projectile.rotation, new Vector2(0, 8), 1f, SpriteEffects.None);
            
            Texture2D bulb = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/Bulb").Value;
            for(int i =0; i < lightsLit; i++)
            {
                BulbOrientation(i, out Vector2 bulbLoc, out float bulbRot);
                Main.EntitySpriteDraw(bulb, bulbLoc - Main.screenPosition, null, Color.White, bulbRot, new Vector2(4, 4), 1.2f + 0.2f * MathF.Cos(trigCounter), SpriteEffects.None);
            }
            if(lightsLit >= 0 && lightsLit < 6)
            {
                chargerCyclerCounter++;
                if(chargerCyclerCounter > 16)
                {
                    chargerCyclerCounter = 0;
                }
                BulbOrientation(lightsLit, out Vector2 bulbLoc, out _);
                float length = (bulbLoc - Projectile.Center).Length();
                float towardsBulb = (bulbLoc - Projectile.Center).ToRotation();
                float centerBeamLength = (length - 8) - chargerCyclerCounter;
                Texture2D charger = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/BulbCharger").Value;
                Main.EntitySpriteDraw(charger, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 10, 12), Color.White, towardsBulb, new Vector2(6, 6), new Vector2(1f, 1.2f + 0.2f * MathF.Cos(trigCounter)), SpriteEffects.None);
                
                Main.EntitySpriteDraw(charger, Projectile.Center + QwertyMethods.PolarVector(4, towardsBulb) - Main.screenPosition, new Rectangle(28 - chargerCyclerCounter, 0, chargerCyclerCounter, 12), Color.White, towardsBulb, new Vector2(0, 6), new Vector2(1f, 1.2f + 0.2f * MathF.Cos(trigCounter)), SpriteEffects.None);
                for(int l = 0; l < centerBeamLength; l += 16)
                {
                    int bW = 16;
                    if(l + 16 > centerBeamLength + 1)
                    {
                        bW = (int)(centerBeamLength + 1) - l;
                    }
                    Main.EntitySpriteDraw(charger, Projectile.Center + QwertyMethods.PolarVector(4 + chargerCyclerCounter + l, towardsBulb) - Main.screenPosition, new Rectangle(12, 0, bW, 12), Color.White, towardsBulb, new Vector2(0, 6), new Vector2(1f, 1.2f + 0.2f * MathF.Cos(trigCounter)), SpriteEffects.None);
                }
                Main.EntitySpriteDraw(charger, Projectile.Center + QwertyMethods.PolarVector(length - 4, towardsBulb) - Main.screenPosition, new Rectangle(30, 0, 4, 12), Color.White, towardsBulb, new Vector2(0, 6), new Vector2(1f, 1.2f + 0.2f * MathF.Cos(trigCounter)), SpriteEffects.None);
            }
            if(timer < 0)
            {
                beamCycletCounter += 4;
                if(beamCycletCounter > 42)
                {
                    beamCycletCounter = 0;
                }

                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DarkBeamBase").Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, Vector2.One * 24f, 1f, SpriteEffects.None);
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DarkBeamBase_Glow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Vector2.One * 24f, 1f, SpriteEffects.None);

                for(float l = 0; l < 4000; l += 42)
                {
                    Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DarkBeamSegment").Value, Projectile.Center + QwertyMethods.PolarVector(l + 91 + beamCycletCounter, Projectile.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(0, 16), 1f, SpriteEffects.None);
                    Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DarkBeamSegment_Glow").Value, Projectile.Center + QwertyMethods.PolarVector(l + 91 + beamCycletCounter, Projectile.rotation) - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(0, 16), 1f, SpriteEffects.None);
                }
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DarkBeamSegment").Value, Projectile.Center + QwertyMethods.PolarVector(91, Projectile.rotation) - Main.screenPosition, new Rectangle(42 - beamCycletCounter, 0, beamCycletCounter, 32), lightColor, Projectile.rotation, new Vector2(0, 16), 1f, SpriteEffects.None);
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DarkBeamSegment_Glow").Value, Projectile.Center + QwertyMethods.PolarVector(91, Projectile.rotation) - Main.screenPosition, new Rectangle(42 - beamCycletCounter, 0, beamCycletCounter, 32), Color.White, Projectile.rotation, new Vector2(0, 16), 1f, SpriteEffects.None);
            }
            



            return false;
        }
    }
}