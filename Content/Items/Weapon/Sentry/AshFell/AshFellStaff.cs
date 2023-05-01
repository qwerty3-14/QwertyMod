using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Sentry.AshFell
{
    public class AshFellStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ash  Fell Staff");
            //Tooltip.SetDefault("Thi Sentry suffocates your foes with ash missiles!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 54000;
            Item.rare = ItemRarityID.Orange;
            Item.damage = 22;
            Item.knockBack = 3f;
            Item.width = Item.height = 44;
            Item.mana = 20;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item44;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.shoot = ModContent.ProjectileType<AshFell>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }
    public class AshFell : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ash fell");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16; //Set the hitbox width
            Projectile.height = 26;   //Set the hitbox heinght
            Projectile.hostile = false;    //tells the game if is hostile or not.
            Projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;    //Tells the game whether or not Projectile will be affected by water
            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            Projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
            Projectile.sentry = true; //tells the game that this is a sentry
            Projectile.timeLeft = Projectile.SentryLifeTime; //allows for the sentry to automaticly be replaced when new sentries are summoned
        }

        private NPC target;
        private int[] missileCounters = new int[2];
        private int missileTime = 60;
        private float missileLoadPosition = 5;

        public override void AI()
        {
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < missileCounters.Length; i++)
            {
                if (missileCounters[i] < missileTime)
                {
                    missileCounters[i]++;
                    break;
                }
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, player.MinionAttackTargetNPC))
            {
                for (int i = 0; i < missileCounters.Length; i++)
                {
                    Projectile.rotation = (target.Center - Projectile.Center).ToRotation();
                    if (missileCounters[i] == missileTime)
                    {
                        //shoot
                        missileCounters[i] = 0;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + QwertyMethods.PolarVector(2f, Projectile.rotation) + QwertyMethods.PolarVector(missileLoadPosition * (i == 0 ? 1 : -1), Projectile.rotation + MathF.PI / 2),
                            QwertyMethods.PolarVector(2f, Projectile.rotation), ModContent.ProjectileType<AshMissile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D missile = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/AshFell/AshMissile").Value;
            for (int i = 0; i < missileCounters.Length; i++)
            {
                Main.EntitySpriteDraw(missile,
                    Projectile.Center + QwertyMethods.PolarVector(2f, Projectile.rotation) + QwertyMethods.PolarVector(missileLoadPosition * (float)missileCounters[i] / missileTime * (i == 0 ? 1 : -1), Projectile.rotation + MathF.PI / 2) - Main.screenPosition,
                    missile.Frame(), lightColor, Projectile.rotation, missile.Size() * .5f, 1f, SpriteEffects.None, 0);
            }

            return true;
        }
    }

    public class AshMissile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6; //Set the hitbox width
            Projectile.height = 6;   //Set the hitbox heinght
            Projectile.hostile = false;    //tells the game if is hostile or not.
            Projectile.friendly = true;   //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;    //Tells the game whether or not Projectile will be affected by water
            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 600;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > finalTime)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                           texture.Frame(), lightColor, Projectile.rotation,
                           new Vector2(texture.Width - 2, texture.Height * .5f), 1f, 0, 0);
            }
            return false;
        }

        private NPC target;
        private int finalTime = 120;
        private int blastSize = 30;

        public override void AI()
        {
            if (Projectile.timeLeft == 600)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            //int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + (float)Projectile.height - 2f), Projectile.width, 6, 36, 0f, 0f, 50, default, 1f);
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft <= finalTime)
            {
                if (Projectile.width != blastSize)
                {
                    Vector2 oldCenter = Projectile.Center;
                    Projectile.width = Projectile.height = blastSize;
                    Projectile.Center = oldCenter;
                }
                Projectile.rotation += MathF.PI / 10;
                for (int d = 0; d < 5; d++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, 36, QwertyMethods.PolarVector(4, Projectile.rotation + (MathF.PI * 2 * d) / 5), Scale: .5f);
                    dust.noGravity = true;
                }
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, player.MinionAttackTargetNPC))
                {
                    Projectile.rotation = QwertyMethods.SlowRotation(Projectile.rotation, (target.Center - Projectile.Center).ToRotation(), 2f);
                }
                Projectile.velocity = QwertyMethods.PolarVector(8f, Projectile.rotation);
                Dust dust = Dust.NewDustPerfect(Projectile.Center - QwertyMethods.PolarVector(6, Projectile.rotation), 36, Vector2.Zero, Scale: .5f);
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.timeLeft > finalTime)
            {
                Projectile.timeLeft = finalTime;
            }

            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
            target.immune[Projectile.owner] = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > finalTime)
            {
                Projectile.timeLeft = finalTime;
            }
            return false;
        }
    }
    class AshFellLoot : ModSystem
    {
        public override void PostWorldGen()
        {
            AddAshFell();
        }

        public static void AddAshFell()
        {
            for (int c = 0; c < Main.chest.Length; c++)
            {
                if (Main.chest[c] != null)
                {
                    if (Main.chest[c].item[0].type == ItemID.DarkLance || Main.chest[c].item[0].type == ItemID.Flamelash || Main.chest[c].item[0].type == ItemID.FlowerofFire || Main.chest[c].item[0].type == ItemID.Sunfury || Main.chest[c].item[0].type == ItemID.HellwingBow)
                    {
                        if (Main.rand.NextBool(4))
                        {
                            for (int i = 0; i < Main.chest[c].item.Length; i++)
                            {
                                if (Main.chest[c].item[i].IsAir)
                                {
                                    Main.chest[c].item[i].SetDefaults(ModContent.ItemType<AshFellStaff>(), false);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}