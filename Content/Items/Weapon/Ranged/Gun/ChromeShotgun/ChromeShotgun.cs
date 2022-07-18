using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.ChromeShotgun
{
    public class ChromeShotgunDefault : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chrome Shotgun");
            Tooltip.SetDefault("Right click to switch between 4 modes");
        }

        private Vector2 DefaultHoldOffset = new Vector2(-8, 0);
        private Vector2 DefaultMuzzleOffset = new Vector2(42, -7);
        private Vector2 ReverseHoldOffset = new Vector2(-12, 0);
        private Vector2 ReverseMuzzleOffset = new Vector2(10, -7);
        private Vector2 TightHoldOffset = new Vector2(-10, -4);
        private Vector2 TightMuzzleOffset = new Vector2(50, -5);
        private Vector2 MinionHoldOffset = new Vector2(-6, 0);
        private Vector2 MinionMuzzleOffset = new Vector2(50, -5);

        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.knockBack = 1;
            Item.value = 500000;
            Item.rare = 7;

            Item.noUseGraphic = true;
            Item.width = 54;
            Item.height = 22;

            Item.shoot = 97;
            Item.useAmmo = 97;
            Item.shootSpeed = 9f;
            Item.noMelee = true;
            Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)DefaultHoldOffset.X;
            Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)DefaultHoldOffset.Y;
            Item.autoReuse = true;
        }


        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddIngredient(ItemID.SoulofNight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            return base.CanConsumeAmmo(ammo, player);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            switch (Item.GetGlobalItem<ChromeGunToggle>().mode)
            {
                case 0:
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)DefaultHoldOffset.X;
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)DefaultHoldOffset.Y;
                    break;

                case 1:
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)ReverseHoldOffset.X;
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)ReverseHoldOffset.Y;
                    break;

                case 2:
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)TightHoldOffset.X;
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)TightHoldOffset.Y;
                    break;

                case 3:
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)MinionHoldOffset.X;
                    Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)MinionHoldOffset.Y;
                    break;
            }
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = 103;
            }
            else
            {
                Item.UseSound = SoundID.Item11;
                Item.useStyle = 5;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Item.useStyle == 5)
            {
                float direction = velocity.ToRotation();
                float horizontalShift = DefaultMuzzleOffset.X;
                float verticalShift = DefaultMuzzleOffset.Y;
                switch (Item.GetGlobalItem<ChromeGunToggle>().mode)
                {
                    case 0:
                        horizontalShift = DefaultMuzzleOffset.X;
                        verticalShift = DefaultMuzzleOffset.Y;
                        break;

                    case 1:
                        horizontalShift = ReverseMuzzleOffset.X;
                        verticalShift = ReverseMuzzleOffset.Y;
                        break;

                    case 2:
                        horizontalShift = TightMuzzleOffset.X;
                        verticalShift = TightMuzzleOffset.Y;
                        break;

                    case 3:
                        horizontalShift = MinionMuzzleOffset.X;
                        verticalShift = MinionMuzzleOffset.Y;
                        break;
                }
                position += QwertyMethods.PolarVector(horizontalShift, direction) + QwertyMethods.PolarVector(verticalShift * player.direction, direction + (float)Math.PI / 2);
                switch (Item.GetGlobalItem<ChromeGunToggle>().mode)
                {
                    case 0:
                        for (int p = 0; p < 3; p++)
                        {
                            Projectile.NewProjectile(source, position, velocity.RotatedBy((((float)(p + 1) / 4f) * (float)Math.PI / 16f) - (float)Math.PI / 32f), type, damage, knockback, player.whoAmI);
                        }
                        break;

                    case 1:
                        for (int p = 0; p < 4; p++)
                        {
                            Projectile.NewProjectile(source, position, velocity.RotatedBy(Math.PI).RotatedBy((((float)(p + 1) / 5f) * (float)Math.PI / 8f) - (float)Math.PI / 16f), type, damage, knockback, player.whoAmI);
                        }
                        break;

                    case 2:
                        for (int p = 0; p < 2; p++)
                        {
                            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(p * 2, velocity.ToRotation() + (float)Math.PI / 2), velocity, type, damage, knockback, player.whoAmI);
                        }
                        break;

                    case 3:
                        return true;
                }
            }
            return false;
        }
    }

    public class ShotgunMinion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 2;
        }

        private NPC target;
        private bool runOnce = true;
        private Vector2[] GunPositions = new Vector2[2];
        private float[] GunRotations = new float[2];
        private int shotCounter = 0;

        public override void AI()
        {
            if (runOnce)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();

                runOnce = false;
            }
            Player player = Main.player[Projectile.owner];
            shotCounter++;
            if (player.HeldItem.GetGlobalItem<ChromeGunToggle>().mode == 3)
            {
                Projectile.timeLeft = 300;
            }
            else if (player.HeldItem.type != ModContent.ItemType<ChromeShotgunDefault>())
            {
                Projectile.Kill();
            }
            Vector2 spot = player.Center;
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, player.MinionAttackTargetNPC))
            {
                spot = target.Center;
            }
            Vector2 Difference = (spot - Projectile.Center);
            Projectile.rotation = QwertyMethods.SlowRotation(Projectile.rotation, Difference.ToRotation(), 5);
            if (Difference.Length() > 100)
            {
                Projectile.velocity = Difference *= .02f;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }
            for (int i = 0; i < 2; i++)
            {
                NPC gunTarget = new NPC();
                if (QwertyMethods.ClosestNPC(ref gunTarget, 450, GunPositions[i], false, player.MinionAttackTargetNPC))
                {
                    GunRotations[i] = QwertyMethods.SlowRotation(GunRotations[i], (gunTarget.Center - GunPositions[i]).ToRotation() + (float)Math.PI / 2 * (i == 1 ? 1 : -1) - Projectile.rotation, 6);
                    if (shotCounter % 30 == i * 15)
                    {
                        int bullet = ProjectileID.Bullet;
                        bool canShoot = true;
                        float speedB = 14f;
                        int weaponDamage = Projectile.damage;
                        float kb = Projectile.knockBack;
                        player.PickAmmo(player.HeldItem, out bullet, out speedB, out weaponDamage, out kb, out _, false);
                        if (canShoot)
                        {
                            Projectile.NewProjectile(new EntitySource_Misc(""), GunPositions[i], QwertyMethods.PolarVector(16, GunRotations[i] + Projectile.rotation + (float)Math.PI / 2 * (i == 0 ? 1 : -1)), bullet, weaponDamage, kb, Projectile.owner);
                        }
                    }
                }
                else
                {
                    GunRotations[i] = QwertyMethods.SlowRotation(GunRotations[i], 0f, 6);
                }
            }
            GunPositions[0] = Projectile.Center + QwertyMethods.PolarVector(13, Projectile.rotation) + QwertyMethods.PolarVector(14, Projectile.rotation + (float)Math.PI / 2);
            GunPositions[1] = Projectile.Center + QwertyMethods.PolarVector(13, Projectile.rotation) + QwertyMethods.PolarVector(14, Projectile.rotation - (float)Math.PI / 2);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D core = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(core, Projectile.Center - Main.screenPosition, core.Frame(), lightColor, Projectile.rotation - (float)Math.PI / 2, core.Size() * .5f, 1f, SpriteEffects.None, 0);

            Texture2D rightGun = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/ChromeShotgun/ShotgunMinionRightGun").Value;
            Main.EntitySpriteDraw(rightGun, GunPositions[1] - Main.screenPosition, rightGun.Frame(), lightColor, Projectile.rotation + GunRotations[1] - (float)Math.PI / 2, new Vector2(8, 8), 1f, SpriteEffects.None, 0);

            Texture2D leftGun = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/ChromeShotgun/ShotgunMinionLeftGun").Value;
            Main.EntitySpriteDraw(leftGun, GunPositions[0] - Main.screenPosition, leftGun.Frame(), lightColor, Projectile.rotation + GunRotations[0] - (float)Math.PI / 2, new Vector2(leftGun.Width - 8, 8), 1f, SpriteEffects.None, 0);
            return false;
        }
    }

    public class ChromeGunToggle : GlobalItem
    {
        public int mode = 0;
        //public override bool CloneNewInstances => true;
        public override bool InstancePerEntity => true;
    }

    public class ChromeGunUseDraw : ModPlayer
    {
        public override void PostItemCheck()
        {
            if (!Player.inventory[Player.selectedItem].IsAir)
            {
                Item item = Player.inventory[Player.selectedItem];
                if (item.useStyle == 103 && Player.itemAnimation > 0)
                {
                    Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
                    Player.itemRotation = (float)(-Math.PI * 2f * Player.direction) * ((float)Player.itemAnimation / Player.itemAnimationMax);
                    Player.itemLocation.X = Player.position.X + (float)Player.width * 0.5f - (float)TextureAssets.Item[item.type].Value.Width * 0.5f - (float)(Player.direction * 2);
                    Player.itemLocation.Y = Player.MountedCenter.Y - (float)TextureAssets.Item[item.type].Value.Height * 0.5f;
                    if (Player.itemAnimation == Player.itemAnimationMax / 2)
                    {
                        item.GetGlobalItem<ChromeGunToggle>().mode++;
                        if (item.GetGlobalItem<ChromeGunToggle>().mode >= 4)
                        {
                            item.GetGlobalItem<ChromeGunToggle>().mode = 0;
                        }
                    }
                }
                if (Player.HeldItem.type == ModContent.ItemType<ChromeShotgunDefault>() && Player.HeldItem.GetGlobalItem<ChromeGunToggle>().mode == 3 && Player.ownedProjectileCounts[ModContent.ProjectileType<ShotgunMinion>()] < 1)
                {
                    Projectile.NewProjectile(new EntitySource_Misc(""), Player.Center, Vector2.Zero, ModContent.ProjectileType<ShotgunMinion>(), Player.HeldItem.damage, Player.HeldItem.knockBack, Player.whoAmI);
                }
            }
        }
    }

    class ChromeGunLayer : PlayerDrawLayer
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
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player drawPlayer = drawInfo.drawPlayer;
            if (!drawPlayer.HeldItem.IsAir)
            {
                if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == ModContent.ItemType<ChromeShotgunDefault>())
                {
                    Item item = drawPlayer.HeldItem;
                    Color color37 = Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16.0));
                    Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/ChromeShotgun/ChromeShotgunDefault").Value;
                    switch (item.GetGlobalItem<ChromeGunToggle>().mode)
                    {
                        case 0:
                            texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/ChromeShotgun/ChromeShotgunDefault").Value;
                            break;

                        case 1:
                            texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/ChromeShotgun/ChromeShotgunReverseFire").Value;
                            break;

                        case 2:
                            texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/ChromeShotgun/ChromeShotgunTight").Value;
                            break;

                        case 3:
                            texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/ChromeShotgun/ChromeShotgunMinionMode").Value;
                            break;
                    }
                    Vector2 zero2 = Vector2.Zero;

                    if (texture != null && drawPlayer.itemAnimation > 0)
                    {
                        Vector2 vector10 = new Vector2((float)(TextureAssets.Item[item.type].Value.Width / 2), (float)(TextureAssets.Item[item.type].Value.Height / 2));
                        Vector2 vector11 = new Vector2(10, texture.Height / 2);
                        if (item.GetGlobalItem<ItemUseGlow>().glowOffsetX != 0)
                        {
                            vector11.X = item.GetGlobalItem<ItemUseGlow>().glowOffsetX;
                        }
                        vector11.Y += item.GetGlobalItem<ItemUseGlow>().glowOffsetY * drawPlayer.gravDir;
                        int num107 = (int)vector11.X;
                        vector10.Y = vector11.Y;
                        Color chromeGunColor = drawPlayer.inventory[drawPlayer.selectedItem].GetAlpha(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16.0)));
                        Vector2 value2 = drawInfo.ItemLocation;

                        Vector2 origin5 = new Vector2((float)(-(float)num107), (float)(TextureAssets.Item[item.type].Value.Height / 2));
                        if (drawPlayer.direction == -1)
                        {
                            origin5 = new Vector2((float)(TextureAssets.Item[item.type].Value.Width + num107), (float)(TextureAssets.Item[item.type].Value.Height / 2));
                        }

                        DrawData value = new DrawData(texture,
                            new Vector2((float)((int)(value2.X - Main.screenPosition.X + vector10.X)), (float)((int)(value2.Y - Main.screenPosition.Y + vector10.Y))),
                            new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, TextureAssets.Item[item.type].Value.Width, TextureAssets.Item[item.type].Value.Height)),
                            chromeGunColor,
                            drawPlayer.itemRotation,
                            origin5,
                            item.scale,
                            drawInfo.itemEffect, 0);
                        drawInfo.DrawDataCache.Add(value);
                    }
                }
            }
        }
    }

}