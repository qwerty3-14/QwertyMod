using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Ranged.SpecialAmmo
{
    class SlidingPenguinRanged : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sliding Penguin");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            //aiType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.usesIDStaticNPCImmunity = true;
        }

        private bool runOnce = true;
        private float initVel;
        private bool hitGround;
        private int timer;

        public override void AI()
        {
            Projectile.spriteDirection = -(int)(Projectile.velocity.X * Math.Abs(1f / Projectile.velocity.X));
            if (runOnce)
            {
                initVel = (float)Math.Abs(Projectile.velocity.Length());

                runOnce = false;
            }
            if (hitGround)
            {
                timer++;
                if (timer > 120)
                {
                    initVel -= .3f;
                    if (Math.Abs(Projectile.velocity.X) < 1f)
                    {
                        Projectile.friendly = false;
                        NPC Penguin = Main.npc[NPC.NewNPC(Projectile.GetNoneSource(), (int)Projectile.Top.X, (int)Projectile.Top.Y, NPCID.Penguin)];
                        if (Projectile.ai[1] == 1)
                        {
                            Penguin.SpawnedFromStatue = true;
                        }
                        Projectile.Kill();
                    }
                }
                if (Projectile.velocity.X < 0)
                {
                    Projectile.velocity.X = -initVel;
                }
                else
                {
                    Projectile.velocity.X = initVel;
                }
            }
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].type == NPCID.Penguin || Main.npc[n].type == NPCID.PenguinBlack)
                {
                    Main.npc[n].immune[Projectile.owner] = 0;
                    Projectile.localNPCImmunity[n] = 10;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            hitGround = true;
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }

            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.perIDStaticNPCImmunity[Projectile.type][target.whoAmI] = (uint)(Main.GameUpdateCount + 10);
        }
    }
}
