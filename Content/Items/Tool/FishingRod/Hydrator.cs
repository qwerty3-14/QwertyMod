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


namespace QwertyMod.Content.Items.Tool.FishingRod
{
    public class Hydrator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hydrator");
            Tooltip.SetDefault("Three bobbers are better than one!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenFishingRod);  //This defines the fishing pole you want to clone

            Item.fishingPole = 10; //this defines the fishing pole fishing power

            Item.value = 25000;
            Item.rare = 3;    //The color the title of your item when hovering over it ingame .
            Item.shoot = ProjectileType<HydraBobber>();  //This defines what type of projectile this item will shot
            Item.shootSpeed = 9f; //this defines the the projectile speed when shot. for fishing pole also increases the fishing line length/range
        }

        // The code below makes the pole shoot two additional bobbers
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 1.2f, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity * 0.8f, type, damage, knockback, player.whoAmI);
            return true;
        }
    }

    public class HydraBobber : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Bobber");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BobberGolden);   //so we are going to clone a bobber from vanilla terraria
        }
        public override bool PreDrawExtras()   //this draws the fishing line correctly
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.bobber && Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].holdStyle > 0)
            {
                float pPosX = player.MountedCenter.X;
                float pPosY = player.MountedCenter.Y;
                pPosY += Main.player[Projectile.owner].gfxOffY;
                int type = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].type;
                float gravDir = Main.player[Projectile.owner].gravDir;

                if (type == ItemType<Hydrator>())
                {
                    pPosX += (float)(50 * Main.player[Projectile.owner].direction);
                    if (Main.player[Projectile.owner].direction < 0)
                    {
                        pPosX -= 13f;
                    }
                    pPosY -= 30f * gravDir;
                }

                if (gravDir == -1f)
                {
                    pPosY -= 12f;
                }
                Vector2 value = new Vector2(pPosX, pPosY);
                value = Main.player[Projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
                float projPosX = Projectile.position.X + (float)Projectile.width * 0.5f - value.X;
                float projPosY = Projectile.position.Y + (float)Projectile.height * 0.5f - value.Y;
                Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                bool flag2 = true;
                if (projPosX == 0f && projPosY == 0f)
                {
                    flag2 = false;
                }
                else
                {
                    float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    projPosXY = 12f / projPosXY;
                    projPosX *= projPosXY;
                    projPosY *= projPosXY;
                    value.X -= projPosX;
                    value.Y -= projPosY;
                    projPosX = Projectile.position.X + (float)Projectile.width * 0.5f - value.X;
                    projPosY = Projectile.position.Y + (float)Projectile.height * 0.5f - value.Y;
                }
                while (flag2)
                {
                    float num = 12f;
                    float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
                    float num3 = num2;
                    if (float.IsNaN(num2) || float.IsNaN(num3))
                    {
                        flag2 = false;
                    }
                    else
                    {
                        if (num2 < 20f)
                        {
                            num = num2 - 8f;
                            flag2 = false;
                        }
                        num2 = 12f / num2;
                        projPosX *= num2;
                        projPosY *= num2;
                        value.X += projPosX;
                        value.Y += projPosY;
                        projPosX = Projectile.position.X + (float)Projectile.width * 0.5f - value.X;
                        projPosY = Projectile.position.Y + (float)Projectile.height * 0.1f - value.Y;
                        if (num3 > 12f)
                        {
                            float num4 = 0.3f;
                            float num5 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
                            if (num5 > 16f)
                            {
                                num5 = 16f;
                            }
                            num5 = 1f - num5 / 16f;
                            num4 *= num5;
                            num5 = num3 / 80f;
                            if (num5 > 1f)
                            {
                                num5 = 1f;
                            }
                            num4 *= num5;
                            if (num4 < 0f)
                            {
                                num4 = 0f;
                            }
                            num5 = 1f - Projectile.localAI[0] / 100f;
                            num4 *= num5;
                            if (projPosY > 0f)
                            {
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                            else
                            {
                                num5 = Math.Abs(Projectile.velocity.X) / 3f;
                                if (num5 > 1f)
                                {
                                    num5 = 1f;
                                }
                                num5 -= 0.5f;
                                num4 *= num5;
                                if (num4 > 0f)
                                {
                                    num4 *= 2f;
                                }
                                projPosY *= 1f + num4;
                                projPosX *= 1f - num4;
                            }
                        }
                        rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
                        Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), new Microsoft.Xna.Framework.Color(228, 29, 249));    //this is the fishing line color in RGB, 200 is red, 12 is green, 50 blue

                        Main.spriteBatch.Draw(TextureAssets.FishingLine.Value, new Vector2(value.X - Main.screenPosition.X + (float)TextureAssets.FishingLine.Value.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)TextureAssets.FishingLine.Value.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.FishingLine.Value.Width, (int)num)), color2, rotation2, new Vector2((float)TextureAssets.FishingLine.Value.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            return false;
        }
    }
}