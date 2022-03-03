using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.RhuthiniumVaporizer
{
    public class RhuthiniumVaporizer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Vaporizer");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.width = 54;
            Item.height = 22;
            Item.mana = 8;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shootSpeed = 1f;
            Item.shoot = ProjectileType<RhuthiniumVaporizerP>();
            Item.DamageType = DamageClass.Magic;
            Item.channel = true;
            Item.autoReuse = true;
            Item.value = 25000;
            Item.rare = 3;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.crit = 5;
            Item.knockBack = .5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<RhuthiniumBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            player.itemLocation = player.position + vector24;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.ownedProjectileCounts[type] == 0;
        }

        /*
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24, -6);
        }*/
    }

    public class RhuthiniumVaporizerP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Vaporizer");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = Projectile.height = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hide = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = 5;
            target.immune[Projectile.owner] = 0;
        }

        public int beamLength
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public float chargeUp
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (chargeUp < 30)
            {
                chargeUp++;
            }
            for (int i = 0; i < 100; i++)
            {
                beamLength = i;
                if (!Collision.CanHit(Projectile.Center, 0, 0, Projectile.Center + QwertyMethods.PolarVector(i, Projectile.rotation), 0, 0))
                {
                    break;
                }
            }
            Player player = Main.player[Projectile.owner];
            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            Projectile.rotation = player.itemRotation + (player.direction == 1 ? 0 : (float)Math.PI);
            Projectile.Center = player.position + vector24 + QwertyMethods.PolarVector(22, Projectile.rotation) + QwertyMethods.PolarVector(-10 * (player.direction == 1 ? 1 : -1), Projectile.rotation + (float)Math.PI / 2);

            if (player.channel && player.itemAnimation > 0)
            {
                Projectile.damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                Projectile.timeLeft = 2;
            }
            if (Main.rand.Next(200) < beamLength)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(Main.rand.Next(beamLength), Projectile.rotation), DustType<RhuthiniumDust>());
                d.velocity *= 2;
                d.noGravity = true;
                d.frame.Y = 0;
            }
            if (beamLength < 99)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(beamLength, Projectile.rotation), DustType<RhuthiniumDust>());
                    d.velocity *= 2;
                    d.noGravity = true;
                    d.frame.Y = 0;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * (chargeUp / 30f));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(beamLength, Projectile.rotation), 12, ref point);
        }
    }
    
    public class DrawVaporizer : PlayerDrawLayer
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
            Color lightColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == ItemType<RhuthiniumVaporizer>() && drawPlayer.itemAnimation > 0)
            {
                Projectile Projectile = null;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == ProjectileType<RhuthiniumVaporizerP>() && Main.projectile[i].owner == drawPlayer.whoAmI)
                    {
                        Projectile = Main.projectile[i];
                        break;
                    }
                }
                if (Projectile != null)
                {
                    Texture2D gun = TextureAssets.Item[ItemType<RhuthiniumVaporizer>()].Value;
                    DrawData d = new DrawData(gun, Projectile.Center - Main.screenPosition + (Main.player[Projectile.owner].direction == 1 ? Vector2.Zero : QwertyMethods.PolarVector(-10, Projectile.rotation + (float)Math.PI / 2)), null, lightColor, Projectile.rotation, new Vector2(54, 6), Vector2.One, (Main.player[Projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                    drawInfo.DrawDataCache.Add(d);
                    Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                    d = new DrawData(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, (int)Projectile.ai[1], 12), new Color((int)((Projectile.ai[0] / 30f) * 100), (int)((Projectile.ai[0] / 30f) * 100), (int)((Projectile.ai[0] / 30f) * 100), (int)((Projectile.ai[0] / 30f) * 100)), Projectile.rotation, new Vector2(0, 6), new Vector2(1f, Projectile.ai[0] / 30f), SpriteEffects.None, 0);
                    drawInfo.DrawDataCache.Add(d);
                }
            }
            
        }
    }
}