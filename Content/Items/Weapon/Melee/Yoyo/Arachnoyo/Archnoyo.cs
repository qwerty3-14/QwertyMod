using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Melee.Yoyo.Arachnoyo
{
    public class Arachnoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aracnoyo");
            Tooltip.SetDefault("Throws 8 yoyos at once!");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = 5;
            Item.width = 30;
            Item.height = 26;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.shootSpeed = 16f;
            Item.knockBack = 2.5f;
            Item.damage = 38;
            Item.value = 50000;
            Item.rare = 4;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<ArachnoyoP>();
        }

        private Projectile yoyo;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int n = 0; n < 8; n++)
            {
                yoyo = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
                yoyo.localAI[1] = n;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SpiderFang, 16)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class ArachnoyoP : QwertyYoyo
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            //Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;

            yoyoCount = 8;
            time = -1f;
            range = 120;
            speed = 16f;
            counterWeightId = ModContent.ProjectileType<SpiderCounterweight>();
        }

        public override void YoyoHit(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 360);
        }
    }

    public class SpiderCounterweight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // The following sets are only applicable to yoyo that use aiStyle 99.
            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player.
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            //ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            //ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 120f;
            // YoyosTopSpeed is top speed of the yoyo Projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            //ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 10;
            Projectile.height = 10;
            // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            //Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
        }

        // notes for aiStyle 99:
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.
        private float range = 120;

        private float speed = 13f;
        private float time = -1f;
        private Vector2 modifiedMousePosition;
        private int frameTimer;

        public override void AI()
        {
            frameTimer++;
            if (frameTimer > 40)
            {
                frameTimer = 0;
                Projectile.frame = 0;
            }
            else if (frameTimer > 30)
            {
                Projectile.frame = 2;
            }
            else if (frameTimer > 20)
            {
                Projectile.frame = 0;
            }
            else if (frameTimer > 10)
            {
                Projectile.frame = 1;
            }
            else
            {
                Projectile.frame = 0;
            }
            Projectile.timeLeft = 6;
            bool flag = true;
            float num = 250f;
            float scaleFactor = 0.1f;
            float num2 = 15f;
            float num3 = 12f;
            num *= 0.5f;
            num2 *= 0.8f;
            num3 *= 1.5f;
            if (Projectile.owner == Main.myPlayer)
            {
                bool flag2 = false;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == ModContent.ProjectileType<ArachnoyoP>())
                    {
                        flag2 = true;
                    }
                }
                if (!flag2)
                {
                    Projectile.ai[0] = -1f;
                    Projectile.netUpdate = true;
                }
            }
            if (Main.player[Projectile.owner].yoyoString)
            {
                num += num * 0.25f + 10f;
            }
            //Projectile.rotation += 0.5f;
            if (Main.player[Projectile.owner].dead)
            {
                Projectile.Kill();
                return;
            }
            if (!flag)
            {
                Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                Main.player[Projectile.owner].itemAnimation = 2;
                Main.player[Projectile.owner].itemTime = 2;
                if (Projectile.position.X + (float)(Projectile.width / 2) > Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2))
                {
                    Main.player[Projectile.owner].ChangeDir(1);
                    Projectile.direction = 1;
                }
                else
                {
                    Main.player[Projectile.owner].ChangeDir(-1);
                    Projectile.direction = -1;
                }
            }
            if (Projectile.ai[0] >= 0f)
            {
                num *= Projectile.ai[0];
                num3 *= 0.5f;
                bool flag3 = false;
                Vector2 vector = Main.player[Projectile.owner].Center - Projectile.Center;
                if ((double)vector.Length() > (double)num * 0.9)
                {
                    flag3 = true;
                }
                if (vector.Length() > num)
                {
                    float num4 = vector.Length() - num;
                    Vector2 vector2;
                    vector2.X = vector.Y;
                    vector2.Y = vector.X;
                    vector.Normalize();
                    vector *= num;
                    Projectile.position = Main.player[Projectile.owner].Center - vector;
                    Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
                    Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
                    float num5 = Projectile.velocity.Length();
                    Projectile.velocity.Normalize();
                    if (num4 > num5 - 1f)
                    {
                        num4 = num5 - 1f;
                    }
                    Projectile.velocity *= num5 - num4;
                    num5 = Projectile.velocity.Length();
                    Vector2 vector3 = new Vector2(Projectile.Center.X, Projectile.Center.Y);
                    Vector2 vector4 = new Vector2(Main.player[Projectile.owner].Center.X, Main.player[Projectile.owner].Center.Y);
                    if (vector3.Y < vector4.Y)
                    {
                        vector2.Y = Math.Abs(vector2.Y);
                    }
                    else if (vector3.Y > vector4.Y)
                    {
                        vector2.Y = -Math.Abs(vector2.Y);
                    }
                    if (vector3.X < vector4.X)
                    {
                        vector2.X = Math.Abs(vector2.X);
                    }
                    else if (vector3.X > vector4.X)
                    {
                        vector2.X = -Math.Abs(vector2.X);
                    }
                    vector2.Normalize();
                    vector2 *= Projectile.velocity.Length();
                    new Vector2(vector2.X, vector2.Y);
                    if (Math.Abs(Projectile.velocity.X) > Math.Abs(Projectile.velocity.Y))
                    {
                        Vector2 vector5 = Projectile.velocity;
                        vector5.Y += vector2.Y;
                        vector5.Normalize();
                        vector5 *= Projectile.velocity.Length();
                        if ((double)Math.Abs(vector2.X) < 0.1 || (double)Math.Abs(vector2.Y) < 0.1)
                        {
                            Projectile.velocity = vector5;
                        }
                        else
                        {
                            Projectile.velocity = (vector5 + Projectile.velocity * 2f) / 3f;
                        }
                    }
                    else
                    {
                        Vector2 vector6 = Projectile.velocity;
                        vector6.X += vector2.X;
                        vector6.Normalize();
                        vector6 *= Projectile.velocity.Length();
                        if ((double)Math.Abs(vector2.X) < 0.2 || (double)Math.Abs(vector2.Y) < 0.2)
                        {
                            Projectile.velocity = vector6;
                        }
                        else
                        {
                            Projectile.velocity = (vector6 + Projectile.velocity * 2f) / 3f;
                        }
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    if (Main.player[Projectile.owner].channel)
                    {
                        Vector2 vector7 = new Vector2((float)(Main.mouseX - Main.lastMouseX), (float)(Main.mouseY - Main.lastMouseY));
                        if (Projectile.velocity.X != 0f || Projectile.velocity.Y != 0f)
                        {
                            if (flag)
                            {
                                vector7 *= -1f;
                            }
                            if (flag3)
                            {
                                if (Projectile.Center.X < Main.player[Projectile.owner].Center.X && vector7.X < 0f)
                                {
                                    vector7.X = 0f;
                                }
                                if (Projectile.Center.X > Main.player[Projectile.owner].Center.X && vector7.X > 0f)
                                {
                                    vector7.X = 0f;
                                }
                                if (Projectile.Center.Y < Main.player[Projectile.owner].Center.Y && vector7.Y < 0f)
                                {
                                    vector7.Y = 0f;
                                }
                                if (Projectile.Center.Y > Main.player[Projectile.owner].Center.Y && vector7.Y > 0f)
                                {
                                    vector7.Y = 0f;
                                }
                            }
                            Projectile.velocity += vector7 * scaleFactor;
                            Projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        Projectile.ai[0] = 10f;
                        Projectile.netUpdate = true;
                    }
                }
                if (flag)
                {
                    float num6 = 800f;
                    Vector2 vector8 = default(Vector2);
                    bool flag4 = false;

                    for (int j = 0; j < 200; j++)
                    {
                        if (Main.npc[j].CanBeChasedBy(Projectile, false))
                        {
                            float num7 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                            float num8 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                            float num9 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num7) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num8);
                            if (num9 < num6 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height) && (double)(Main.npc[j].Center - Main.player[Projectile.owner].Center).Length() < (double)num * 0.9)
                            {
                                num6 = num9;
                                vector8.X = num7;
                                vector8.Y = num8;
                                flag4 = true;
                            }
                        }
                    }
                    if (flag4)
                    {
                        vector8 -= Projectile.Center;
                        vector8.Normalize();

                        vector8 *= 6f;
                        Projectile.velocity = (Projectile.velocity * 7f + vector8) / 8f;
                    }
                }
                if (Projectile.velocity.Length() > num2)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= num2;
                }
                if (Projectile.velocity.Length() < num3)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= num3;
                    return;
                }
            }
            else
            {
                Projectile.tileCollide = false;
                Vector2 vector9 = Main.player[Projectile.owner].Center - Projectile.Center;
                if (vector9.Length() < 40f || vector9.HasNaNs())
                {
                    Projectile.Kill();
                    return;
                }
                float num10 = num2 * 1.5f;

                float num11 = 12f;
                vector9.Normalize();
                vector9 *= num10;
                Projectile.velocity = (Projectile.velocity * (num11 - 1f) + vector9) / num11;
            }
            Projectile.rotation = (Projectile.Center - Main.player[Projectile.owner].Center).ToRotation() + (float)Math.PI / 2;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool notMain10 = false;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                notMain10 = true;
                Projectile.velocity.X = oldVelocity.X * -1f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                notMain10 = true;
                Projectile.velocity.Y = oldVelocity.Y * -1f;
            }
            if (notMain10)
            {
                Vector2 vector10 = Main.player[Projectile.owner].Center - Projectile.Center;
                vector10.Normalize();
                vector10 *= Projectile.velocity.Length();
                vector10 *= 0.25f;
                Projectile.velocity *= 0.75f;
                Projectile.velocity += vector10;
                if (Projectile.velocity.Length() > 6f)
                {
                    Projectile.velocity *= 0.5f;
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 vector = mountedCenter;
            vector.Y += Main.player[Projectile.owner].gfxOffY;
            float num3 = Projectile.Center.X - vector.X;
            float num4 = Projectile.Center.Y - vector.Y;
            Math.Sqrt((double)(num3 * num3 + num4 * num4));
            float rotation = (float)Math.Atan2((double)num4, (double)num3) - 1.57f;
            if (!Projectile.counterweight)
            {
                int num5 = -1;
                if (Projectile.position.X + (float)(Projectile.width / 2) < Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2))
                {
                    num5 = 1;
                }
                num5 *= -1;
                Main.player[Projectile.owner].itemRotation = (float)Math.Atan2((double)(num4 * (float)num5), (double)(num3 * (float)num5));
            }
            bool notMain = true;
            if (num3 == 0f && num4 == 0f)
            {
                notMain = false;
            }
            else
            {
                float num6 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
                num6 = 12f / num6;
                num3 *= num6;
                num4 *= num6;
                vector.X -= num3 * 0.1f;
                vector.Y -= num4 * 0.1f;
                num3 = Projectile.position.X + (float)Projectile.width * 0.5f - vector.X;
                num4 = Projectile.position.Y + (float)Projectile.height * 0.5f - vector.Y;
            }
            while (notMain)
            {
                float num7 = 12f;
                float num8 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
                float num9 = num8;
                if (float.IsNaN(num8) || float.IsNaN(num9))
                {
                    notMain = false;
                }
                else
                {
                    if (num8 < 20f)
                    {
                        num7 = num8 - 8f;
                        notMain = false;
                    }
                    num8 = 12f / num8;
                    num3 *= num8;
                    num4 *= num8;
                    vector.X += num3;
                    vector.Y += num4;
                    num3 = Projectile.position.X + (float)Projectile.width * 0.5f - vector.X;
                    num4 = Projectile.position.Y + (float)Projectile.height * 0.1f - vector.Y;
                    if (num9 > 12f)
                    {
                        float num10 = 0.3f;
                        float num11 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
                        if (num11 > 16f)
                        {
                            num11 = 16f;
                        }
                        num11 = 1f - num11 / 16f;
                        num10 *= num11;
                        num11 = num9 / 80f;
                        if (num11 > 1f)
                        {
                            num11 = 1f;
                        }
                        num10 *= num11;
                        if (num10 < 0f)
                        {
                            num10 = 0f;
                        }
                        num10 *= num11;
                        num10 *= 0.5f;
                        if (num4 > 0f)
                        {
                            num4 *= 1f + num10;
                            num3 *= 1f - num10;
                        }
                        else
                        {
                            num11 = Math.Abs(Projectile.velocity.X) / 3f;
                            if (num11 > 1f)
                            {
                                num11 = 1f;
                            }
                            num11 -= 0.5f;
                            num10 *= num11;
                            if (num10 > 0f)
                            {
                                num10 *= 2f;
                            }
                            num4 *= 1f + num10;
                            num3 *= 1f - num10;
                        }
                    }
                    rotation = (float)Math.Atan2((double)num4, (double)num3) - 1.57f;
                    int stringColor = Main.player[Projectile.owner].stringColor;
                    Microsoft.Xna.Framework.Color color = WorldGen.paintColor(stringColor);
                    if (color.R < 75)
                    {
                        color.R = 75;
                    }
                    if (color.G < 75)
                    {
                        color.G = 75;
                    }
                    if (color.B < 75)
                    {
                        color.B = 75;
                    }
                    if (stringColor == 13)
                    {
                        color = new Microsoft.Xna.Framework.Color(20, 20, 20);
                    }
                    else if (stringColor == 14 || stringColor == 0)
                    {
                        color = new Microsoft.Xna.Framework.Color(200, 200, 200);
                    }
                    else if (stringColor == 28)
                    {
                        color = new Microsoft.Xna.Framework.Color(163, 116, 91);
                    }
                    else if (stringColor == 27)
                    {
                        color = new Microsoft.Xna.Framework.Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                    }
                    color.A = (byte)((float)color.A * 0.4f);
                    float num12 = 0.5f;
                    color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
                    color = new Microsoft.Xna.Framework.Color((int)((byte)((float)color.R * num12)), (int)((byte)((float)color.G * num12)), (int)((byte)((float)color.B * num12)), (int)((byte)((float)color.A * num12)));
                    Main.spriteBatch.Draw(TextureAssets.FishingLine.Value, new Vector2(vector.X - Main.screenPosition.X + (float)TextureAssets.FishingLine.Value.Width * 0.5f, vector.Y - Main.screenPosition.Y + (float)TextureAssets.FishingLine.Value.Height * 0.5f) - new Vector2(6f, 0f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.FishingLine.Value.Width, (int)num7)), color, rotation, new Vector2((float)TextureAssets.FishingLine.Value.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 360);
            int num8 = Main.DamageVar((float)Projectile.damage);
            //Main.player[Projectile.owner].GetModPlayer<CustomYoyoPlayer>().Counterweight(target.Center, Projectile.damage, Projectile.knockBack, projectile, this);
            if (target.Center.X < Main.player[Projectile.owner].Center.X)
            {
                Projectile.direction = -1;
            }
            else
            {
                Projectile.direction = 1;
            }

            if (Projectile.ai[0] >= 0f)
            {
                Vector2 value2 = Projectile.Center - target.Center;
                value2.Normalize();
                float scaleFactor = 16f;
                Projectile.velocity *= -0.5f;
                Projectile.velocity += value2 * scaleFactor;
                Projectile.netUpdate = true;
                Projectile.localAI[0] += 20f;
                if (!Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.player[Projectile.owner].position, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height))
                {
                    Projectile.localAI[0] += 40f;
                    num8 = (int)((double)num8 * 0.75);
                }
            }
        }
    }
}