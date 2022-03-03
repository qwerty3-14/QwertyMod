using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.Longsword
{
    public class SwordMinion : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Longsword");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
        }

        private float yetAnotherTrigCounter;
        private NPC target;
        private bool returningToPlayer = false;
        private float turnOffset = 3 * (float)Math.PI / 4;
        private int counter = 0;
        private float bladeLength = 10;
        float? toward = null;
        public override void AI()
        {
            bool spinAttack = false;
            bladeLength = 24 + 16 + 14 * Projectile.minionSlots;
            counter++;
            if (counter % Projectile.localNPCHitCooldown == 0)
            {
                turnOffset *= -1;
            }
            yetAnotherTrigCounter += (float)Math.PI / 120;
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().SwordMinion)
            {
                Projectile.timeLeft = 2;
            }
            if ((player.Center - Projectile.Center).Length() > 1000)
            {
                returningToPlayer = true;
            }
            if ((player.Center - Projectile.Center).Length() < 300)
            {
                returningToPlayer = false;
            }
            Vector2 flyTo = player.Center + new Vector2(-50 * player.direction, -50 - 14 * Projectile.minionSlots) + Vector2.UnitY * (float)Math.Sin(yetAnotherTrigCounter) * 20;
            float turnTo = (float)Math.PI / 2;
            float speed = 12f;
            if (returningToPlayer)
            {
                speed = (player.Center - Projectile.Center).Length() / 30f;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, player.MinionAttackTargetNPC) && !returningToPlayer)
            {
                Vector2 difference2 = Projectile.Center - target.Center;
                flyTo = target.Center + QwertyMethods.PolarVector(bladeLength / 2, difference2.ToRotation());
                toward = turnTo = (target.Center - Projectile.Center).ToRotation();
                int nerabyEnemies = 0;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.chaseable && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && !npc.immortal && (npc.Center - Projectile.Center).Length() < bladeLength)
                    {
                        nerabyEnemies++;
                    }
                }
                if (nerabyEnemies > 2)
                {
                    spinAttack = true;
                }
                if (difference2.Length() < bladeLength)
                {
                    turnTo += turnOffset;
                }
            }
            else
            {
                toward = null;
            }

            if (spinAttack)
            {
                Projectile.rotation += ((float)Math.PI * 2) / Projectile.localNPCHitCooldown;
            }
            else
            {
                if (toward == null)
                {
                    Projectile.rotation.SlowRotation(turnTo, ((float)Math.PI * 2) / Projectile.localNPCHitCooldown);
                }
                else
                {
                    Projectile.rotation.SlowRotWhileAvoid(turnTo, ((float)Math.PI * 2) / Projectile.localNPCHitCooldown, (float)toward + (float)Math.PI);
                }
            }
            Vector2 difference = flyTo - Projectile.Center;
            if (difference.Length() < speed)
            {
                Projectile.Center = flyTo;
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.velocity = difference.SafeNormalize(Vector2.UnitY) * speed;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(bladeLength, Projectile.rotation), 14f, ref point) || Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projHitbox.TopLeft(), projHitbox.Size());
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
            target.immune[Projectile.owner] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                       new Rectangle(0, 0, 31, 21), lightColor, Projectile.rotation,
                       new Vector2(9f, 11f), Projectile.scale, SpriteEffects.None, 0);
            for (int b = 0; b < Projectile.minionSlots; b++)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + QwertyMethods.PolarVector(22 + b * 14, Projectile.rotation),
                       new Rectangle(34, 0, 14, 21), lightColor, Projectile.rotation,
                       new Vector2(0, 11f), Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + QwertyMethods.PolarVector(22 + Projectile.minionSlots * 14, Projectile.rotation),
                       new Rectangle(50, 0, 16, 21), lightColor, Projectile.rotation,
                       new Vector2(0, 11f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)Projectile.minionSlots * damage;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().SwordMinion)
            {
                Projectile p = Main.projectile[player.SpawnMinionOnCursor(Projectile.InheritSource(Projectile), Projectile.owner, Projectile.type, Projectile.originalDamage, Projectile.knockBack)];
                p.minionSlots += player.maxMinions - player.slotsMinions - 1;
                p.rotation = Projectile.rotation;
            }
        }
    }
}
