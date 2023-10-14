﻿using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Magic.RhuthiniumScepter
{
    public class RhuthiniumScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.damage = 15;
            Item.crit = 5;
            Item.useTime = 6;
            Item.useAnimation = 24;
            Item.useTurn = false;
            Item.noMelee = true; //can't hit as item since they have custom animation
            Item.useStyle = 20; //Vanilla doesn't use this for anything but it still starts the useAnimation timer, good for custom animations
            Item.mana = ModLoader.HasMod("TRAEProject") ? 24 : 16;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<RhuthiniumBolt>();
            Item.UseSound = SoundID.Item39;
            Item.knockBack = 1.2f;
            Item.DamageType = DamageClass.Magic;
            Item.value = 25000;
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<RhuthiniumBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        private Vector2 staveHoldOffset = new Vector2(0, -10);
        private float staveHoldRotation = MathF.PI / 8;
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 3; //force the player to a specific frame
            player.itemRotation = -1 * staveHoldRotation * player.direction;
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
            float trueRotation = MathF.PI / 2 - player.itemRotation + MathF.PI;
            player.itemLocation += new Vector2(MathF.Cos(trueRotation), MathF.Sin(trueRotation)) * staveHoldOffset.Y;
            player.itemLocation += new Vector2(MathF.Cos(trueRotation + MathF.PI / 2), MathF.Sin(trueRotation + MathF.PI / 2)) * staveHoldOffset.X * player.direction;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = new Vector2(player.Center.X + (float)Main.rand.Next(-100, 101), player.Center.Y + (float)Main.rand.Next(-100, 101) - 600);
            velocity = QwertyMethods.PolarVector(Item.shootSpeed, (Main.MouseWorld - position).ToRotation() + MathF.PI / 16 - MathF.PI / 8 * Main.rand.NextFloat());
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    public class RhuthiniumBolt : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.extraUpdates = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
        }

        private NPC target;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                Projectile.ai[0] = Main.rand.Next(2);
            }
            if (true)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<RhuthiniumDust>(), Vector2.Zero);
                d.frame.Y = (int)(Projectile.ai[0] * 10);
                d.noGravity = true;
                d.velocity = Vector2.Zero;
            }
            if (QwertyMethods.ClosestNPC(ref target, 250, Projectile.Center))
            {
                float rot = (Projectile.velocity.ToRotation());
                rot.SlowRotation((target.Center - Projectile.Center).ToRotation(), MathF.PI / 60);
                Projectile.velocity = QwertyMethods.PolarVector(10, rot);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}