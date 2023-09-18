using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.EtimsSword
{
    public class EtimsSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("The Massacre");
            //Tooltip.SetDefault("Right click on the ground for an uppercut" + "\nRight click in the air to slam down!" + "\nKilling an enemy grants immunity to hostile projectiles for a short time.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.useStyle = 101; //custom use style
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.damage = 50;
            Item.knockBack = 7f;
            Item.width = 36;
            Item.height = 34;
            Item.noUseGraphic = true;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.rare = ItemRarityID.Orange;
            Item.value = 120000;
            Item.useTurn = true;
            Item.scale = 1.8f;
            Item.shootsEveryUse = true;
            Item.shoot = ModContent.ProjectileType<EtimsSwordP>();
        }


        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<Etims>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            return player.itemAnimation == 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, 2);
                return false;
            }
            return true;
        }
    }
    public class EtimsSwordP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 18;
            Projectile.width = Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.penetrate = -1;
        }
        private bool uppercut = false;
        private bool slam = false;
        private bool checkedRightClick = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.MountedCenter;
            player.heldProj = Projectile.whoAmI;
            Point origin = player.Bottom.ToTileCoordinates();
            Point point;
            Item item = player.HeldItem;
            Projectile.localNPCHitCooldown = item.useAnimation / 3;
            if (Projectile.ai[0] == 2 && !checkedRightClick)
            {
                if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                                    {
                                    new Conditions.IsSolid()
                                    }), out point))
                {
                    player.itemAnimation = player.itemAnimationMax;
                    player.velocity.Y = -10 - player.jumpSpeedBoost;
                    uppercut = true;
                    slam = false;
                }
                else
                {
                    player.velocity.Y = 10;
                    slam = true;
                    uppercut = false;
                }
            }
            checkedRightClick = true;
            float shift = 0f;
            if (player.itemAnimation > 0 && uppercut || slam)
            {
                if (slam)
                {
                    //Main.NewText("Slamming");
                    player.bodyFrame.Y = player.bodyFrame.Height * 4;
                    shift = MathF.PI / 2;
                    if (player.velocity.Y != 0)
                    {
                        player.itemAnimation = 3;
                    }
                    else
                    {
                        player.itemAnimation = 0;
                        slam = false;
                    }
                    player.velocity.Y = 10;
                }
                else if (uppercut)
                {
                    shift = MathF.PI / 2 * ((float)player.itemAnimation / (float)player.itemAnimationMax) - MathF.PI / 4;

                    if (player.itemAnimation < player.itemAnimationMax * .5f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .25f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    }
                    else
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 4;
                    }
                    if (player.itemAnimation < 2)
                    {
                        player.itemAnimation = 2;
                    }
                    if (player.velocity.Y >= 0)
                    {
                        player.itemAnimation = 0;
                        uppercut = false;
                    }
                }
            }
            else
            {
                if (player.itemAnimation < player.itemAnimationMax * .25f)
                {
                    shift = MathF.PI / -4 * ((player.itemAnimation) / (player.itemAnimationMax * .25f));
                }
                else if (player.itemAnimation < player.itemAnimationMax * .75f)
                {
                    shift = MathF.PI / -2 * (1 - (player.itemAnimation - (player.itemAnimationMax * .25f)) / (player.itemAnimationMax * .5f)) + MathF.PI / 4;
                }
                else
                {
                    shift = MathF.PI / 4 * (1 - (player.itemAnimation - (player.itemAnimationMax * .75f)) / (player.itemAnimationMax * .25f));
                }
                if (player.itemAnimation < player.itemAnimationMax * .15f)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 3;
                }
                else if (player.itemAnimation < player.itemAnimationMax * .35f)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 2;
                }
                else if (player.itemAnimation < player.itemAnimationMax * .65f)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 3;
                }
                else if (player.itemAnimation < player.itemAnimationMax * .85f)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 4;
                }
                else
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 3;
                }
            }
            /*
            if (Main.mouseRight && !slam && !uppercut)
            {
                player.itemAnimation = 0;
            }
            */

            player.itemRotation = MathF.PI / -4 + player.direction * (MathF.PI / 2 + shift);
            //Main.NewText(MathHelper.ToDegrees(Player.itemRotation));

            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = (float)player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = (float)player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
            player.itemLocation = player.position + vector24;

            player.itemTime = player.itemAnimation;
            Projectile.timeLeft = player.itemAnimation;
            
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            Item item = player.HeldItem;
            float swordLength = new Vector2((TextureAssets.Item[item.type].Value).Width, (TextureAssets.Item[item.type].Value).Height).Length();
            swordLength *= item.scale;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.itemLocation, player.itemLocation + QwertyMethods.PolarVector(swordLength, player.itemRotation - MathF.PI / 4));
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (slam || uppercut)
            {
                modifiers.FinalDamage *= 2;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (slam)
            {
                player.immune = true;
                if (player.immuneTime < 60)
                {
                    player.immuneTime = 60;
                }
            }
            if (damageDone > target.life && !target.SpawnedFromStatue)
            {
                Main.player[Projectile.owner].AddBuff(BuffType<AntiProjectile>(), 360);
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
    public class AltSword : ModPlayer
    {
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (Player.HasBuff(BuffType<AntiProjectile>()))
            {
                return false;
            }
            return base.CanBeHitByProjectile(proj);
        }
    }
    public class WeirdSword : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.JustDroppedAnItem)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == ItemType<EtimsSword>() && drawPlayer.itemAnimation > 0)
            {
                Item item = drawPlayer.HeldItem;
                Texture2D texture = TextureAssets.Item[item.type].Value;
                Vector2 origin = new Vector2(0, texture.Height);
                DrawData value = new DrawData(texture, drawPlayer.itemLocation - Main.screenPosition, texture.Frame(), color12, drawPlayer.itemRotation, origin, item.scale, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(value);
            }
        }
    }
}