using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Bow.Ancient
{
    public class AncientLongbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hold to charge up" + "\nFires 3 arrows at max charge" + "\nWooden arrows become ancient arrows");
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
            Item.damage = 60;
            Item.shoot = ProjectileType<AncientLongbowP>();
            Item.value = 150000;
            Item.rare = 3;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useAmmo = AmmoID.Arrow;

            Item.autoReuse = true;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Bow/Ancient/AncientLongbow_Glow").Value;
            }
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<AncientLongbowP>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }
    }

    public class AncientLongbowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }


        public override void SetDefaults()
        {
            Projectile.width = 50;
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

        public float Radd;
        public bool runOnce = true;
        private Projectile arrow = null;
        private float speed = 15f;
        private int maxTime = 120;
        private int weaponDamage = 10;
        private int Ammo = 0;
        private float weaponKnockback = 0;
        private bool giveTileCollision = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                runOnce = false;
            }
            Projectile.timeLeft = 2;

            //var modPlayer = player.GetModPlayer<QwertyPlayer>();
            bool firing = (player.channel || timer < 30) && player.HasAmmo(player.HeldItem, true) && !player.noItems && !player.CCed;
            Ammo = AmmoID.Arrow;

            weaponDamage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            direction = (Main.MouseWorld - player.Center).ToRotation();
            weaponKnockback = player.inventory[player.selectedItem].knockBack;

            if (firing)
            {
                
                #region drill ai

                ///////////////////////////////////// copied from vanilla drill/chainsaw AI
                Vector2 vector24 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
                if (Main.myPlayer == Projectile.owner)
                {
                    if (Main.player[Projectile.owner].channel || timer < 30)
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

                ///////////////////////////////

                #endregion drill ai

                if (timer == 0)
                {
                   
                    player.PickAmmo(player.HeldItem, ref Ammo, ref speed, ref firing, ref weaponDamage, ref weaponKnockback, out _);

                    if (Ammo == ProjectileID.WoodenArrowFriendly)
                    {
                        Ammo = ProjectileType<AncientArrow>();
                    }
                    if (Main.netMode != 2)
                    {
                        arrow = Main.projectile[Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), Projectile.Center, Vector2.Zero, Ammo, weaponDamage, weaponKnockback, Projectile.owner)];
                    }
                }
                arrow.velocity = QwertyMethods.PolarVector(speed, Projectile.rotation - (float)Math.PI / 2);
                arrow.Center = Projectile.Center + QwertyMethods.PolarVector(40 - 2 * speed, Projectile.rotation - (float)Math.PI / 2);
                arrow.friendly = false;
                arrow.rotation = Projectile.rotation;
                arrow.timeLeft += arrow.extraUpdates + 1;
                arrow.alpha = 1 - (int)(((float)timer / maxTime) * 255f);
                arrow.ai[0] = 0;
                speed = (8f * (float)timer / maxTime) + 7f;
                //Main.NewText(arrow.damage);
                // Main.NewText("AI0: " + arrow.ai[0] + ", AI1: " + arrow.ai[1] + ", LocalAI0: " + arrow.localAI[0] + ", LocalAI1: " + arrow.localAI[1]);
                if (arrow.tileCollide)
                {
                    giveTileCollision = true;
                    arrow.tileCollide = false;
                }
                if (timer < maxTime)
                {
                    timer++;
                    /*
                    for (int d = 0; d < 3; d++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Dust dust = Dust.NewDustPerfect(arrow.Center + QwertyMethods.PolarVector(40, theta), DustType<AncientGlow>(), QwertyMethods.PolarVector(-8, theta));
                        dust.scale = .5f;
                        dust.alpha = 255;
                    }
                    */
                    if (timer == maxTime)
                    {
                        SoundEngine.PlaySound(25, player.position, 0);
                    }
                }
            }
            else
            {
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item5, Projectile.position);
            arrow.velocity = QwertyMethods.PolarVector(speed, Projectile.rotation - (float)Math.PI / 2);
            arrow.friendly = true;
            if (arrow != null && giveTileCollision)
            {
                arrow.tileCollide = true;
            }
            if (timer >= maxTime)
            {
                Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), arrow.Center, arrow.velocity * .9f, arrow.type, arrow.damage, arrow.knockBack, Projectile.owner);
                Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), arrow.Center, arrow.velocity * 1.1f, arrow.type, arrow.damage, arrow.knockBack, Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Bow/Ancient/AncientLongbowP").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), drawColor, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Bow/Ancient/AncientLongbowP_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), Color.White, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }

    public class AncientArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Arrow");
        }


        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        private void drawArrowCore(Color drawColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 18, 36), drawColor, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Bow/AncientLongbow/AncientArrow_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 18, 36), Color.White, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
        }

        private void drawOrbital(Color drawColor, Vector2 Loc)
        {
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Bow/AncientLongbow/AncientArrow_Orbital").Value, Loc - Main.screenPosition,
                        new Rectangle(0, 0, 6, 10), drawColor, Projectile.rotation,
                        new Vector2(3, 5), 1f, SpriteEffects.None, 0);
        }

        private float orbitalCounter = 0;
        private float lengthDown = 10;
        private float orbitRadius = 11;

        public override bool PreDraw(ref Color drawColor)
        {
            orbitalCounter += (float)Math.PI / 60;

            if (Math.Cos(orbitalCounter) > 0)
            {
                Vector2 orbitalLocation = Projectile.Center + QwertyMethods.PolarVector(lengthDown, Projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), Projectile.rotation);
                drawOrbital(drawColor, orbitalLocation);
                drawArrowCore(drawColor);
                orbitalLocation = Projectile.Center + QwertyMethods.PolarVector(lengthDown, Projectile.rotation + (float)Math.PI / 2) - QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), Projectile.rotation);
                drawOrbital(drawColor, orbitalLocation);
            }
            else
            {
                Vector2 orbitalLocation = Projectile.Center + QwertyMethods.PolarVector(lengthDown, Projectile.rotation + (float)Math.PI / 2) - QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), Projectile.rotation);
                drawOrbital(drawColor, orbitalLocation);
                drawArrowCore(drawColor);
                orbitalLocation = Projectile.Center + QwertyMethods.PolarVector(lengthDown, Projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(orbitRadius * (float)Math.Sin(orbitalCounter), Projectile.rotation);
                drawOrbital(drawColor, orbitalLocation);
            }

            return false;
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}