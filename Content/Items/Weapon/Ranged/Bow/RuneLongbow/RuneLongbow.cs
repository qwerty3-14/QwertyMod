using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.Ancient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Bow.RuneLongbow
{
    public class RuneLongbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hold to charge up" + "\nFires 3 arrows at max charge" + "\nWooden arrows become aggro rune strikes");
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
            Item.damage = 250;
            //Item.reuseDelay = 30;
            Item.shoot = ProjectileType<RuneLongbowP>();
            Item.value = 500000;
            Item.rare = 9;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useAmmo = AmmoID.Arrow;

            Item.autoReuse = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Bow/RuneLongbow/RuneLongbow_Glow").Value;
            }
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center;
            for (int l = 0; l < Main.projectile.Length; l++)
            {
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
                {
                    return false;
                }
            }
            Projectile.NewProjectile(source, position, velocity, ProjectileType<RuneLongbowP>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 15)
                .AddIngredient(ItemType<AncientLongbow>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class RuneLongbowP : ModProjectile
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
                        Ammo = ProjectileType<RuneArrow>();
                    }
                    if (Main.netMode != 2)
                    {
                        arrow = Main.projectile[Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, Ammo, weaponDamage, weaponKnockback, Projectile.owner)];
                    }
                }
                arrow.velocity = QwertyMethods.PolarVector(speed, Projectile.rotation - (float)Math.PI / 2);
                arrow.Center = Projectile.Center + QwertyMethods.PolarVector(40 - 2 * speed, Projectile.rotation - (float)Math.PI / 2);
                arrow.friendly = false;
                arrow.rotation = Projectile.rotation;
                arrow.timeLeft += arrow.extraUpdates + 1;
                arrow.alpha = 1 - (int)(((float)timer / maxTime) * 255f);
                speed = (8f * (float)timer / maxTime) + 7f;
                arrow.ai[0] = 0;
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
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), arrow.Center, QwertyMethods.PolarVector(arrow.velocity.Length(), arrow.velocity.ToRotation() + (float)Math.PI / 64f) , arrow.type, arrow.damage, arrow.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), arrow.Center, QwertyMethods.PolarVector(arrow.velocity.Length(), arrow.velocity.ToRotation() - (float)Math.PI / 64f), arrow.type, arrow.damage, arrow.knockBack, Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), drawColor, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Bow/RuneLongbow/RuneLongbowP_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 50, 34), Color.White, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
    public class RuneArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
        bool runOnce = true;
        int timer;
        public override void AI()
        {
            timer = 30 - Projectile.timeLeft;
            if (timer > 3 && runOnce)
            {
                runOnce = false;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < 5)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(1000, Projectile.rotation));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (timer > 3)
            {
                int frame = timer / 2;
                if (timer > 22)
                {
                    frame = (30 - timer) / 2;
                }
                if (frame > 3)
                {
                    frame = 3;
                }
                float c = (float)frame / 3f;
                for (int i = 0; i < 3000; i += 8)
                {
                    Main.EntitySpriteDraw(RuneSprites.aggroStrike[frame], Projectile.Center + QwertyMethods.PolarVector(i, Projectile.rotation) - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(0, 3), Vector2.One * 2, 0, 0);
                }
            }
            else
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector( 8, Projectile.rotation - (float)Math.PI/2f) - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Size * .5f, 1f, 0, 0);
            }
            return false;
        }
    }
    
}
