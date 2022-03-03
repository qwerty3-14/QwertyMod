using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Gale
{
    [AutoloadEquip(EquipType.Body)]
    public class GaleSwiftplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gale Swiftplate");
            Tooltip.SetDefault("+10% chance to dodge an attack" + "\n+10% critical strike chance" + "\nAllows you to cling to walls" + "\nDamage increased by 20% while clinging to walls");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = 4;
            Item.defense = 2;
            //Item.vanity = true;
            Item.width = 20;
            Item.height = 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ItemType<GaleSwiftHelm>() && legs.type == ItemType<GaleSwiftRobes>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.GaleSet");
            player.GetModPlayer<GaleSetBonus>().setBonus = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CommonStats>().dodgeChance += 10;
            player.GetCritChance(DamageClass.Generic) += 10;
            player.spikedBoots = 2;
            if (player.sliding)
            {
                player.GetDamage(DamageClass.Generic) += .2f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CaeliteBar>(), 10)
                .AddIngredient(ItemType<FortressHarpyBeak>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class GaleKnife : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gale Knife");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Generic;
        }

        public int dustTimer;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
            target.immune[Projectile.owner] = 0;
        }
    }

    public class GaleSetBonus : ModPlayer
    {
        public bool setBonus = false;
        public Vector3[,] orb = new Vector3[10, Main.player.Length];
        private bool runOnce = true;

        public override void Initialize()
        {
        }

        public override void ResetEffects()
        {
            setBonus = false;
        }

        public float counter = 0;
        public int timer;

        public override void UpdateEquips()
        {
            for (int b = 0; b < 10; b++)
            {
                if (orb[b, Player.whoAmI].X != 0)
                {
                    Player.GetModPlayer<CommonStats>().dodgeChance++;
                }
            }
        }

        public int rightclickTimer;
        private bool resetRTimer;
        private int orbFrameCounter;

        public override void PreUpdate()
        {
            if (runOnce)
            {
                for (int b = 0; b < 10; b++)
                {
                    orb[b, Player.whoAmI] = new Vector3(0, 2 * (float)Math.PI * (b / (float)10), 0);
                }
                runOnce = false;
            }

            orbFrameCounter++;
            if (orbFrameCounter >= 40)
            {
                orbFrameCounter = 0;
            }
            orb[orbFrameCounter % 10, Player.whoAmI].Z = (orbFrameCounter - (orbFrameCounter % 10)) / 10;

            if (!setBonus)
            {
                for (int b = 0; b < 10; b++)
                {
                    orb[b, Player.whoAmI].X = 0;
                }
            }
            else
            {
                if (Main.mouseRight && Main.mouseRightRelease)
                {
                    if (rightclickTimer > 0)
                    {
                        for (int b = 0; b < 10; b++)
                        {
                            if (orb[b, Player.whoAmI].X != 0)
                            {
                                Vector2 Position = Player.Center;
                                Position.X += (float)Math.Sin(orb[b, Player.whoAmI].Y) * 50;
                                Position.Y += (float)Math.Sin(orb[b, Player.whoAmI].Y) * 50 * (float)Math.Sin(counter);
                                float speed = 10;
                                Projectile.NewProjectile(Player.GetProjectileSource_SetBonus(1), Position, (Main.MouseWorld - Position).SafeNormalize(-Vector2.UnitY) * speed, ProjectileType<GaleKnife>(), (int)(75f * Player.GetDamage(DamageClass.Generic)), 3f, Player.whoAmI);
                                orb[b, Player.whoAmI].X = 0;
                            }
                        }
                        //Main.NewText("Double tap!");
                        rightclickTimer = 0;
                    }
                }
                if (rightclickTimer > 0)
                {
                    rightclickTimer--;
                    //Main.NewText(rightclickTimer);
                }
                if (Main.mouseRight && resetRTimer)
                {
                    //Main.NewText("Double tap!");

                    rightclickTimer = 15;

                    resetRTimer = false;
                }
                else if (!Main.mouseRight)
                {
                    resetRTimer = true;
                }
            }
            timer++;
            if (timer > 120)
            {
                for (int b = 0; b < 10; b++)
                {
                    if (setBonus && orb[b, Player.whoAmI].X == 0)
                    {
                        orb[b, Player.whoAmI].X = 1;
                        break;
                    }
                }
                timer = 0;
            }
            counter += (float)Math.PI / 120;
            for (int b = 0; b < 10; b++)
            {
                /*
                if (Main.netMode == 1)
                {
                    Main.NewText("client: " + "Hello");
                }

                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + "Hello"), Color.White);
                }
                */
                orb[b, Player.whoAmI].Y += (float)Math.PI / 60 * Player.direction;
                //orb[b].Z = orb[b].Y ;
                if (orb[b, Player.whoAmI].X != 0)
                {
                    if ((float)Math.Cos(orb[b, Player.whoAmI].Y) < 0)
                    {
                        orb[b, Player.whoAmI].X = 1;
                    }
                    else
                    {
                        orb[b, Player.whoAmI].X = 2;
                    }
                }
            }
        }
    }
    public class DrawOrbsFront : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.JimsCloak);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            GaleSetBonus modPlayer = drawPlayer.GetModPlayer<GaleSetBonus>();
            Mod mod = ModLoader.GetMod("QwertyMod");

            Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Gale/GaleOrb").Value;

            for (int b = 0; b < 10; b++)
            {
                if (modPlayer.orb[b, drawPlayer.whoAmI].X == 2)
                {
                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawPlayer.Center;
                    Position.X += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50;
                    Position.Y += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50 * (float)Math.Sin(modPlayer.counter);
                    //Main.NewText(b + ", " + (float)Math.Sin(modPlayer.orb[b].Y) * 50);
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    pos.Y -= drawPlayer.mount.PlayerOffset;
                    DrawData data = new DrawData(texture, pos, new Rectangle(0, (int)modPlayer.orb[b, drawPlayer.whoAmI].Z * texture.Height / 4, texture.Width, texture.Height / 4), Color.White, 0, origin, 1f, drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    data.shader = drawInfo.cBody;
                    drawInfo.DrawDataCache.Add(data);
                }
            }
        }
    }
    public class DrawOrbsBack : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BeetleBuff);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            GaleSetBonus modPlayer = drawPlayer.GetModPlayer<GaleSetBonus>();
            Mod mod = ModLoader.GetMod("QwertyMod");

            Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Gale/GaleOrb").Value;

            for (int b = 0; b < 10; b++)
            {
                if (modPlayer.orb[b, drawPlayer.whoAmI].X == 1)
                {
                    int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                    int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                    Vector2 Position = drawPlayer.Center;
                    Position.X += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50;
                    Position.Y += (float)Math.Sin(modPlayer.orb[b, drawPlayer.whoAmI].Y) * 50 * (float)Math.Sin(modPlayer.counter);
                    //Main.NewText(b + ", " + (float)Math.Sin(modPlayer.orb[b].Y) * 50);
                    Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                    Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                    pos.Y -= drawPlayer.mount.PlayerOffset;
                    DrawData data = new DrawData(texture, pos, new Rectangle(0, (int)modPlayer.orb[b, drawPlayer.whoAmI].Z * texture.Height / 4, texture.Width, texture.Height / 4), Color.White, 0, origin, 1f, drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    data.shader = drawInfo.cBody;
                    drawInfo.DrawDataCache.Add(data);
                }
            }
        }
    }
}