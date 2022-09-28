﻿using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Javelin
{
    public abstract class Javelin : ModProjectile
    {
        protected int dropItem = -1;

        protected int maxStickingJavelins = 5; // projectile is the max. amount of javelins being able to attach
        protected float rotationOffset = 0f;
        protected float maxTicks = 45f;

        public virtual void ExtraAI()
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            // For going through platforms and such, javelins use a tad smaller size
            width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Inflate some target hitboxes if they are beyond 8,8 size
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            // Return if the hitboxes intersects, which means the javelin collides or not
            return projHitbox.Intersects(targetHitbox);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Tink, Projectile.position); // Play a death sound
            Vector2 usePos = Projectile.position; // Position to use for dusts
                                                  // Please note the usage of MathHelper, please use projectile! We subtract 90 degrees as radians to the rotation vector to offset the sprite as its default rotation in the sprite isn't aligned properly.
            Vector2 rotVector =
                (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            ExtraKill(timeLeft);
        }

        public virtual void ExtraKill(int timeLeft)
        {
        }

        // Here's an example on how you could make your AI even more readable, by giving AI fields more descriptive names
        // These are not used in AI, but it is good practice to apply some form like projectile to keep things organized

        // Are we sticking to a target?
        public bool isStickingToTarget
        {
            get { return Projectile.ai[0] == 1f; }
            set { Projectile.ai[0] = value ? 1f : 0f; }
        }

        // WhoAmI of the current target
        public float targetWhoAmI
        {
            get { return Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit,
            ref int hitDirection)
        {
            Projectile.timeLeft = 30 * 60;
            // If you'd use the example above, you'd do: isStickingToTarget = 1f;
            // and: targetWhoAmI = (float)target.whoAmI;
            isStickingToTarget = true; // we are sticking to a target
            targetWhoAmI = (float)target.whoAmI; // Set the target whoAmI
            Projectile.velocity =
                (target.Center - Projectile.Center) *
                0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            Projectile.netUpdate = true; // netUpdate projectile javelin
            target.AddBuff(BuffType<Impaled>(), 900); // Adds the Impaled debuff
            Projectile.penetrate = -1;
            Projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

            // The following code handles the javelin sticking to the enemy hit.
            Player player = Main.player[Projectile.owner];
            Point[] stickingJavelins = new Point[(int)(maxStickingJavelins)]; // The point array holding for sticking javelins
            int javelinIndex = 0; // The javelin index
            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != Projectile.whoAmI // Make sure the looped projectile is not the current javelin
                    && currentProjectile.active // Make sure the projectile is active
                    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                    && currentProjectile.type == Projectile.type // Make sure the projectile is of the same type as projectile javelin
                    && currentProjectile.ai[0] == 1f // Make sure ai0 state is set to 1f (set earlier in ModifyHitNPC)
                    && currentProjectile.ai[1] == (float)target.whoAmI
                ) // Make sure ai1 is set to the target whoAmI (set earlier in ModifyHitNPC)
                {
                    stickingJavelins[javelinIndex++] =
                        new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                    if (javelinIndex >= stickingJavelins.Length
                    ) // If the javelin's index is bigger than or equal to the point array's length, break
                    {
                        break;
                    }
                }
            }
            // Here we loop the other javelins if new javelin needs to take an older javelin's place.
            if (javelinIndex >= stickingJavelins.Length)
            {
                int oldJavelinIndex = 0;
                // Loop our point array
                for (int i = 1; i < stickingJavelins.Length; i++)
                {
                    // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                    if (stickingJavelins[i].Y < stickingJavelins[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i; // Remember the index of the removed javelin
                    }
                }
                // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                Main.projectile[stickingJavelins[oldJavelinIndex].X].Kill();
            }
        }

        public virtual void StuckEffects(NPC victim)
        {
        }

        public virtual void NonStickingBehavior()
        {
            targetWhoAmI += 1f;
            // For a little while, the javelin will travel with the same speed, but after projectile, the javelin drops velocity very quickly.
            if (targetWhoAmI >= maxTicks)
            {
                // Change these multiplication factors to alter the javelin's movement change after reaching maxTicks
                float velXmult = 0.98f; // x velocity factor, every AI update the x velocity will be 98% of the original speed
                float velYmult = 0.35f; // y velocity factor, every AI update the y velocity will be be 0.35f bigger of the original speed, causing the javelin to drop to the ground
                targetWhoAmI = maxTicks; // set ai1 to maxTicks continuously
                Projectile.velocity.X = Projectile.velocity.X * velXmult;
                Projectile.velocity.Y = Projectile.velocity.Y + velYmult;
            }
        }

        // Change projectile number if you want to alter how the alpha changes
        private const int alphaReduction = 25;

        public override void AI()
        {
            //Main.NewText(Projectile.owner);
            // Slowly remove alpha as it is present
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= alphaReduction;
            }
            // If alpha gets lower than 0, set it to 0
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            // If ai0 is 0f, run projectile code. projectile is the 'movement' code for the javelin as long as it isn't sticking to a target
            if (!isStickingToTarget)
            {
                NonStickingBehavior();

                // Make sure to set the rotation accordingly to the velocity, and add some to work around the sprite's rotation
                Projectile.rotation =
                    Projectile.velocity.ToRotation() + (float)Math.PI / 2 + rotationOffset;
            }
            // projectile code is ran when the javelin is sticking to a target
            if (isStickingToTarget)
            {
                // These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI

                int aiFactor = 15; // Change projectile factor to change the 'lifetime' of projectile sticking javelin
                bool killProj = false; // if true, kill projectile at the end
                bool hitEffect = false; // if true, perform a hit effect
                Projectile.localAI[0] += 1f;
                // Every 30 ticks, the javelin will perform a hit effect
                hitEffect = Projectile.localAI[0] % 30f == 0f;
                int projTargetIndex = (int)targetWhoAmI;
                if (Projectile.localAI[0] >= (float)(60 * aiFactor)// If it's time for projectile javelin to die, kill it
                    || (projTargetIndex < 0 || projTargetIndex >= 200)) // If the index is past its limits, kill it
                {
                    killProj = true;
                }
                else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage) // If the target is active and can take damage
                {
                    // Set the projectile's position relative to the target's center
                    Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                    if (hitEffect) // Perform a hit effect here
                    {
                        Main.npc[projTargetIndex].HitEffect(0, 1.0);
                    }
                    StuckEffects(Main.npc[projTargetIndex]);
                }
                else // Otherwise, kill the projectile
                {
                    killProj = true;
                }

                if (killProj) // Kill the projectile
                {
                    Projectile.Kill();
                }
            }
            ExtraAI();
        }
    }
    public class JavelinAilments : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int ruthJavs = 0;
        public int hydraJavs = 0;
        public int imperiumJavs = 0;
        public override bool PreAI(NPC npc)
        {
            ruthJavs = 0;
            hydraJavs = 0;
            imperiumJavs = 0;
            return true;
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += ruthJavs;
            knockback += hydraJavs * 1.2f;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            damage += ruthJavs;
            damage = damage + (int)(damage * 0.35f * imperiumJavs);
        }
        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            damage = damage * (int)(1f / (1 + hydraJavs * 0.02));
        }
    }
}