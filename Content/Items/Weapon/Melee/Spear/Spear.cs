using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Spear
{
    public abstract class Spear : ModProjectile
    {
        /// <summary> How long the spear is from corner to corner, check the sprite the spear uses to find the correct value. </summary>
        public float spearLength = 76f;
        /// <summary> Where along the spear's sprite the animation starts at, bottom right corner of the sprite is 0 and top left should be spearLength, also used for collision. This should e where the handle meets the tip of the spear </summary>
        public float stabStart = 54f;
        /// <summary> Where along the spear's sprite the animation ends at, bottom right corner of the sprite is 0 and top left should be spearLength </summary>
        public float stabEnd = 0;
        /// <summary> How much the spear swings when it stabs, this value is in radians </summary>
        public float swingAmount = 0;
        /// <summary> Draws colored dots on the spear, green dots should be at the ends of the spear, red dot is spear startm blue dot is spear end, and the whit dot is the player's center</summary>
        public bool debug = false;
        /// <summary> Can be used by specific spears to interupt the standard animation </summary>
        public int interupting = 0;

        /// <summary> Use this instead of SetDefaults(). </summary>
        public virtual void SpearDefaults()
        {

        }
        /// <summary> Called every frame the spear is being used, useful for stuff like dusts </summary>
        public virtual void SpearActive()
        {

        }
        /// <summary> Use this instead of OnHitNPC() </summary>
        public virtual void SpearHitNPCMelee(NPC target, NPC.HitInfo hit)
        {

        }
        /// <summary> Use this instead of ModifyHitNPC <summary> 
        public virtual void SpearModfiyHitNPCMelee(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        /// <summary> Called the moment the spear reaches its max range </summary>
        public virtual void OnMaxReach(float direction)
        {

        }
        /// <summary> If the item has channel the spear will be held out when at max reach and this method will be called </summary>
        public virtual void Channeling()
        {

        }
        /// <summary> Called when interupting > 0 </summary>
        public virtual void InteruptedAnimation()
        {

        }
        /// <summary> Called when the spear is created </summary>
        public virtual void OnStart()
        {

        }
        public static void SpearPrefixScaleing(Item item, Player player, Projectile projectile)
        {
            if (!item.IsAir)
            {
                float bonusSize = 1f;
                switch (item.prefix)
                {
                    case PrefixID.Large:
                        bonusSize = (1.18f / 1.15f);
                        break;
                    case PrefixID.Massive:
                        bonusSize = (1.25f / 1.18f);
                        break;
                    case PrefixID.Dangerous:
                        bonusSize = (1.12f / 1.5f);
                        break;
                    case PrefixID.Bulky:
                        bonusSize = (1.2f / 1.1f);
                        break;
                }
                projectile.scale = bonusSize * item.scale * (player.meleeScaleGlove ? 1.1f : 1f);
            }
        }

        bool runOnce = true;
        float stabDirection = 0;
        public float aimDirection = 0;
        SpriteEffects effects = SpriteEffects.None;
        bool calledMaxReach = false;
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = Projectile.height = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            //Projectile.aiStyle = 19; //vanilla spear AI is turned off by PreAI, this just helps other mods know this is a spear
            SpearDefaults();
        }
        protected float outAmount = 0f;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            if (runOnce)
            {
                aimDirection = Projectile.velocity.ToRotation();
                swingAmount *= player.direction * -1;
                stabDirection = aimDirection * swingAmount;
                runOnce = false;
                Projectile.velocity = Vector2.Zero;
                OnStart();
            }
            //Projectile.scale = player.HeldItem.scale * (player.meleeScaleGlove ? 1.1f : 1f) * player.GetModPlayer<MeleeStats>().weaponSize;
            SpearPrefixScaleing(player.HeldItem, player, Projectile);
            player.itemTime = player.itemAnimation;
            int switchStabTime = (int)(2f * (float)player.itemAnimationMax / 3f);
            int stabTime = player.itemAnimationMax - switchStabTime;
            int swivelDir = 1;
            if (player.itemAnimation > switchStabTime)
            {
                outAmount = 1f - ((float)(player.itemAnimation - switchStabTime) / (float)stabTime);
            }
            else
            {

                swivelDir = -1;
                outAmount = 1f - ((float)(switchStabTime - player.itemAnimation) / (float)switchStabTime);

                if (player.channel)
                {
                    player.itemAnimation = switchStabTime;
                    Channeling();
                }
                else if (interupting > 0)
                {
                    player.itemAnimation = switchStabTime;
                    InteruptedAnimation();
                }
                else
                {
                }

            }

            stabDirection = aimDirection + swingAmount * (Math.Abs(MathF.Sin(MathF.PI * outAmount))) * swivelDir;
            Projectile.Center = ownerMountedCenter + PolarVector((outAmount * (stabStart - stabEnd) + (spearLength - stabStart)) * Projectile.scale, stabDirection);


            AnimatePlayer();
            if (player.direction == 1)
            {
                effects = SpriteEffects.FlipVertically;
                Projectile.rotation = stabDirection + 5f * MathF.PI / 4f;
            }
            else
            {
                effects = SpriteEffects.None;
                Projectile.rotation = stabDirection + 3f * MathF.PI / 4f;
            }
            if (player.itemAnimation == 0 || player.itemAnimation == 1)
            {
                Projectile.Kill();
            }
            if (!calledMaxReach && player.itemAnimation <= switchStabTime)
            {
                OnMaxReach(stabDirection);
                calledMaxReach = true;
            }
            SpearActive();
        }
        void AnimatePlayer()
        {
            Player player = Main.player[Projectile.owner];
            player.bodyFrame.Y = player.bodyFrame.Height * 1;

            Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            float num2 = (float)Projectile.Center.X - pointPoisition.X;
            float num3 = (float)Projectile.Center.Y - pointPoisition.Y;
            float itemRotation = MathF.Atan2(num3 * (float)player.direction, num2 * (float)player.direction) - player.fullRotation;

            Vector2 SpearStabPos = Projectile.Center + PolarVector((spearLength - stabStart) * Projectile.scale, stabDirection + MathF.PI);
            float distance = (SpearStabPos - pointPoisition).Length();

            Player.CompositeArmStretchAmount stretch = Player.CompositeArmStretchAmount.Quarter;
            if (distance > 24f)
            {
                stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
            }
            if (distance > 48f)
            {
                stretch = Player.CompositeArmStretchAmount.Full;
            }
            float rotation = itemRotation - MathF.PI / 2f * (float)player.direction;
            player.SetCompositeArmFront(enabled: true, stretch, rotation);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + PolarVector((spearLength - stabStart) * Projectile.scale, stabDirection + MathF.PI), Projectile.Center, spearLength - stabStart, ref point);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            int direction = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
            modifiers.HitDirectionOverride = direction;
            SpearModfiyHitNPCMelee(target, ref modifiers);
        }
        public int[] hitCount = new int[Main.npc.Length];
        public int pierceLimit = 0;
        public int MaxPierce = 6;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount[target.whoAmI]++;
            if(hitCount[target.whoAmI] >= 2)
            {
                Projectile.localNPCImmunity[target.whoAmI] = -1;
            }
            else
            {
                Projectile.localNPCImmunity[target.whoAmI] = Main.player[Projectile.owner].itemAnimationMax / 3;
            }
            target.immune[Projectile.owner] = 0;
            pierceLimit++;
            if (pierceLimit == MaxPierce)
            {
                Projectile.damage = 0;
            }
            SpearHitNPCMelee(target, hit);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, effects == SpriteEffects.None ? Vector2.Zero : Vector2.UnitY * texture.Width, Projectile.scale, effects, 0);
            
            if (debug)
            {
                //spearLength
                Main.EntitySpriteDraw(QwertyMod.debugCross, Projectile.Center + PolarVector(spearLength * Projectile.scale, stabDirection + MathF.PI) - Main.screenPosition, null, Color.Green, 0, QwertyMod.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(QwertyMod.debugCross, Projectile.Center - Main.screenPosition, null, Color.Green, 0, QwertyMod.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);

                //stabStart
                Main.EntitySpriteDraw(QwertyMod.debugCross, Projectile.Center + PolarVector((spearLength - stabStart) * Projectile.scale, stabDirection + MathF.PI) - Main.screenPosition, null, Color.Red, 0, QwertyMod.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);

                //stabEnd
                Main.EntitySpriteDraw(QwertyMod.debugCross, Projectile.Center + PolarVector((spearLength - stabEnd) * Projectile.scale, stabDirection + MathF.PI) - Main.screenPosition, null, Color.Blue, 0, QwertyMod.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);


                Player player = Main.player[Projectile.owner];
                Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
                //mounted center
                Main.EntitySpriteDraw(QwertyMod.debugCross, ownerMountedCenter - Main.screenPosition, null, Color.White, 0, QwertyMod.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            return false;
        }

        public static Vector2 PolarVector(float radius, float theta)
        {
            return new Vector2(MathF.Cos(theta), MathF.Sin(theta)) * radius;
        }
    }
}