using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.NPCs.Fortress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.ShieldMinion
{
    public class ShieldMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield Minion");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = 28;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
        }

        private Vector2 flyTo;
        private int identity = 0;
        private int ShieldCount = 0;
        private Vector2 eyeOffset;
        private NPC target;
        private float horizontalEyeMultiploer = 3;
        private float verticalEyeMultiplier = 2;

        private const int guarding = 0;
        private const int charging = 1;
        private const int cooling = 2;
        private int chargeTimer = 0;
        private Vector2 LatestValidVelocity;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().ShieldMinion)
            {
                Projectile.timeLeft = 2;
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == ProjectileType<ShieldMinion>() && Main.projectile[p].active && Main.projectile[p].owner == Projectile.owner && Main.projectile[p].ai[1] == Projectile.ai[1])
                {
                    ShieldCount++;
                }
            }

            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == ProjectileType<ShieldMinion>() && Main.projectile[p].active && Main.projectile[p].owner == Projectile.owner && Main.projectile[p].ai[1] == Projectile.ai[1])
                {
                    if (p == Projectile.whoAmI)
                    {
                        break;
                    }
                    else
                    {
                        identity++;
                    }
                }
            }
            Projectile.friendly = (Projectile.ai[1] == charging);
            if (Projectile.ai[1] != charging)
            {
                if (player.velocity.Length() > .1f)
                {
                    LatestValidVelocity = player.velocity;
                }

                float myOffset = (((float)Math.PI / 2) * (float)(identity + 1)) / (ShieldCount + 1) - (float)Math.PI / 4;
                flyTo = player.Center + QwertyMethods.PolarVector(Projectile.ai[1] == guarding ? 120 : -50, (QwertyMod.GetLocalCursor(Projectile.owner) - player.Center).ToRotation() + myOffset);

                if (flyTo != null && flyTo != Vector2.Zero)
                {
                    Projectile.velocity = (flyTo - Projectile.Center) * .1f;
                }
            }
            switch ((int)Projectile.ai[1])
            {
                case guarding:
                    Projectile.frame = 0;

                    if (QwertyMethods.ClosestNPC(ref target, 1000, player.Center, true, player.MinionAttackTargetNPC))
                    {
                        eyeOffset = (target.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY);
                        eyeOffset.X *= horizontalEyeMultiploer;
                        eyeOffset.Y *= verticalEyeMultiplier;
                        if ((target.Center - Projectile.Center).Length() < 120)
                        {
                            Projectile.velocity = QwertyMethods.PolarVector(24, (target.Center - Projectile.Center).ToRotation());
                            Projectile.ai[1] = charging;
                            chargeTimer = 10;
                            break;
                        }
                    }
                    else
                    {
                        eyeOffset = Vector2.Zero;
                    }

                    break;

                case charging:
                    Projectile.frame = 0;
                    chargeTimer--;
                    if (chargeTimer <= 0)
                    {
                        Projectile.ai[1] = cooling;
                        chargeTimer = -120;
                    }
                    break;

                case cooling:
                    Projectile.frame = 1;
                    chargeTimer++;
                    if (chargeTimer >= 0)
                    {
                        Projectile.ai[1] = guarding;
                    }
                    break;
            }

            identity = 0;
            ShieldCount = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[Projectile.owner] = 0;
            if (Main.rand.Next(10) == 0 && !target.boss)
            {
                target.AddBuff(BuffType<Stunned>(), 120);
            }
        }
        
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<FortressNPCGeneral>().fortressNPC)
            {
                for (int i = 0; i < damage / 3; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<BloodforceDust>());
                    d.velocity *= 5f;
                }
                damage *= 2;
            }
        }
        
        public override void PostDraw(Color lightColor)
        {
            if (Projectile.frame == 0)
            {
                Texture2D eye = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/ShieldMinion/ShieldMinionEye").Value;
                Main.EntitySpriteDraw(eye, (Projectile.position + new Vector2(14, 13)) + eyeOffset - Main.screenPosition,
                           eye.Frame(), lightColor, Projectile.rotation,
                           eye.Size() * .5f, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
