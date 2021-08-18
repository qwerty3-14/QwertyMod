using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.DinoVulcan
{
    public class DinoVulcan : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Builds up in speed while used" + "\n66% chance not to consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = 5;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 20f;
            Item.knockBack = 2f;
            Item.width = 50;
            Item.height = 18;
            Item.damage = 20;

            Item.shoot = ProjectileType<DinoVulcanP>();
            Item.rare = 6;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useAmmo = AmmoID.Bullet;

            Item.autoReuse = true;
        }

        public override void HoldItem(Player player)
        {
            player.accRunSpeed *= .5f;
            player.maxRunSpeed *= .5f;
            player.jumpSpeedBoost *= .5f;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ProjectileType<DinoVulcanP>();
            position = player.Center;
            for (int l = 0; l < Main.projectile.Length; l++)
            {                                                                  //this make so you can only spawn one of this projectile at the time,
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
                {
                    return false;
                }
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return  Main.rand.Next(3)>0;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -8);
        }
    }

    public class DinoVulcanP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }

        public int timer = 0;
        public int reloadTime;
        public float direction;
        public float VarA;
        public float SVarA;
        public float Radd;
        public bool runOnce = true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                reloadTime = player.inventory[player.selectedItem].useTime;
                runOnce = false;
            }
            Projectile.timeLeft = 10;
            timer++;

            bool firing = player.channel && player.HasAmmo(player.HeldItem, true) && !player.noItems && !player.CCed;

            int Ammo = 14;
            float speed = 14f;

            int weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            direction = (Main.MouseWorld - player.Center).ToRotation();
            float weaponKnockback = player.inventory[player.selectedItem].knockBack;
            if (firing)
            {
                ///////////////////////////////////// copied from vanilla drill/chainsaw AI
                Vector2 vector24 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
                if (Main.myPlayer == Projectile.owner)
                {
                    if (Main.player[Projectile.owner].channel)
                    {
                        float num264 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].shootSpeed * Projectile.scale;
                        Vector2 vector25 = vector24;
                        float num265 = (float)Main.mouseX + Main.screenPosition.X - vector25.X;
                        float num266 = (float)Main.mouseY + Main.screenPosition.Y - vector25.Y;
                        if (Main.player[Projectile.owner].gravDir == -1f)
                        {
                            num266 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector25.Y;
                        }
                        float num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
                        num267 = (float)Math.Sqrt((double)(num265 * num265 + num266 * num266));
                        num267 = num264 / num267;
                        num265 *= num267;
                        num266 *= num267;
                        if (num265 != Projectile.velocity.X || num266 != Projectile.velocity.Y)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.velocity.X = num265;
                        Projectile.velocity.Y = num266;
                    }
                    else
                    {
                        Projectile.Kill();
                    }
                }
                if (Projectile.velocity.X > 0f)
                {
                    Main.player[Projectile.owner].ChangeDir(1);
                }
                else if (Projectile.velocity.X < 0f)
                {
                    Main.player[Projectile.owner].ChangeDir(-1);
                }
                Projectile.spriteDirection = Projectile.direction;
                Main.player[Projectile.owner].ChangeDir(Projectile.direction);
                Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                Main.player[Projectile.owner].itemTime = 2;
                Main.player[Projectile.owner].itemAnimation = 2;
                Projectile.position.X = vector24.X - (float)(Projectile.width / 2);
                Projectile.position.Y = vector24.Y - (float)(Projectile.height / 2);
                Projectile.rotation = (float)(Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.5700000524520874);
                if (Main.player[Projectile.owner].direction == 1)
                {
                    Main.player[Projectile.owner].itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
                }
                else
                {
                    Main.player[Projectile.owner].itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
                }
                Projectile.velocity.X = Projectile.velocity.X * (1f + (float)Main.rand.Next(-3, 4) * 0.01f);
                if (Main.rand.Next(6) == 0)
                {
                    int num268 = Dust.NewDust(Projectile.position + Projectile.velocity * (float)Main.rand.Next(6, 10) * 0.1f, Projectile.width, Projectile.height, 31, 0f, 0f, 80, default(Color), 1.4f);
                    Dust dust51 = Main.dust[num268];
                    dust51.position.X = dust51.position.X - 4f;
                    Main.dust[num268].noGravity = true;
                    Dust dust3 = Main.dust[num268];
                    dust3.velocity *= 0.2f;
                    Main.dust[num268].velocity.Y = -(float)Main.rand.Next(7, 13) * 0.15f;
                    return;
                }
                ///////////////////////////////


                if (timer >= reloadTime)
                {
                    VarA = direction + MathHelper.ToRadians(Main.rand.Next(-100, 101) / 10);

                    float shellShift = MathHelper.ToRadians(-50);
                    SVarA = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    float SspeedA = .05f * Main.rand.Next(15, 41);

                    SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
                    player.PickAmmo(player.HeldItem, ref Ammo, ref speed, ref firing, ref weaponDamage, ref weaponKnockback, out _);
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)Math.Cos(VarA) * speed, (float)Math.Sin(VarA) * speed, Ammo, weaponDamage, weaponKnockback, Main.myPlayer);
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)Math.Cos(SVarA) * SspeedA * -player.direction, (float)Math.Sin(SVarA) * SspeedA, ProjectileType<Shell>(), 0, 0, Main.myPlayer);
                        
                    }

                    if (reloadTime > 3)
                    {
                        reloadTime -= 1;
                    }
                    timer = 0;
                }
            }
            else
            {
                reloadTime = player.inventory[player.selectedItem].useTime;
                Projectile.Kill();
            }
        }
    }

    public class Shell : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shell");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6; //Set the hitbox width
            Projectile.height = 10;   //Set the hitbox height
            Projectile.hostile = false;    //tells the game if is hostile or not.
            Projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Projectile.aiStyle = 1;

            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            Projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain

            Projectile.timeLeft = 90;
        }

        public bool runOnce = true;
        public float rotationSpeed;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                rotationSpeed = player.direction * Main.rand.Next(0, 241);
                runOnce = false;
            }

            Projectile.rotation += MathHelper.ToRadians(rotationSpeed);
            if (Projectile.timeLeft <= 20)
            {
                Projectile.alpha = (int)(255f - ((float)Projectile.timeLeft / 20f) * 255f);
            }
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            Projectile.velocity.X /= 2;
            return false;
        }
    }
}