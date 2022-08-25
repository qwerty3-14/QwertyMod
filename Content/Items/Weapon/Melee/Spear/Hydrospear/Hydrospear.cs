using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrospear
{
    public class Hydrospear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrospear");
            Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Spears[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.useStyle = 5;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.shootSpeed = 37f;
            Item.knockBack = 6f;
            Item.width = 70;
            Item.height = 70;
            Item.scale = 1f;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = 2;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // Important because the spear is actually a projectile instead of an Item. This prevents the melee hitbox of this Item.
            Item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this Item.
            //Item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()
            Item.channel = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<HydrospearP>();
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }

    public class HydrospearP : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrospear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.usesLocalNPCImmunity = true;

            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        // In here the AI uses this example, to make the code more organized and readable
        // Also showcased in ExampleJavelinProjectile.cs
        public float movementFactor // Change this value to alter how fast the spear moves
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }

        public int debugTimer;
        public float movefactSpeed = 1f;
        public float maxDistance = 650;
        public float vel;
        public bool runOnce = true;
        private int streamCounter = 0;

        // It appears that for this AI, only the ai0 field is used!
        private bool noDust = false;

        public override void AI()
        {
            noDust = false;
            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = Main.player[Projectile.owner];
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            vel = maxDistance / projOwner.itemAnimationMax / 2;
            Projectile.velocity = new Vector2((float)Math.Cos(Projectile.velocity.ToRotation()) * vel, (float)Math.Sin(Projectile.velocity.ToRotation()) * vel);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);

            // As long as the player isn't frozen, the spear can move
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movementFactor = 0; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 2) // Somewhere along the item animation, make sure the spear moves back
                {
                    if (projOwner.channel)
                    {
                        Projectile.friendly = false;
                        projOwner.itemAnimation++;
                        if (Collision.CanHit(projOwner.Center, 0, 0, Projectile.Center, 0, 0))
                        {
                            noDust = true;
                            streamCounter++;
                            if (streamCounter % (int)(16f * projOwner.GetAttackSpeed(DamageClass.Melee)) == 0)
                            {
                                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center + QwertyMethods.PolarVector(180, Projectile.rotation - (3 * (float)Math.PI / 4)) + QwertyMethods.PolarVector(5, Projectile.rotation - (1 * (float)Math.PI / 4)), QwertyMethods.PolarVector(1, Projectile.rotation - (3 * (float)Math.PI / 4)), ProjectileType<HydrospearStream>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            }
                        }
                    }
                    else
                    {
                        Projectile.friendly = true;
                        movementFactor -= movefactSpeed;
                    }

                    //Main.NewText("Hi");
                }
                else // Otherwise, increase the movement factor
                {
                    Projectile.friendly = true;
                    movementFactor += movefactSpeed;
                }
            }
            // Change the spear position based off of the velocity and the movementFactor
            Projectile.position += Projectile.velocity * movementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            // Offset by 90 degrees here
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }
            if (!noDust)
            {
                Dust k = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-4, Projectile.rotation - (3 * (float)Math.PI / 4)) + QwertyMethods.PolarVector(4, Projectile.rotation - (1 * (float)Math.PI / 4)), 172);
                k.velocity = Vector2.Zero;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[Projectile.owner] = 0;
        }
    }

    public class HydrospearStream : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 99;
            Projectile.timeLeft = 1200;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            if (Main.rand.Next(8) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, 172)];
                d.velocity *= .1f;
                d.noGravity = true;
                d.position = Projectile.Center;
            }
        }
    }
}