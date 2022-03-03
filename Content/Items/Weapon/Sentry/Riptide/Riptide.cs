using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Sentry.Riptide
{
    public class Riptide : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Riptide Sentry Staff");
            Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = 2;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<RiptideP>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.mod == "Terraria" && line.Name == "Knockback") //this checks if it's the line we're interested in
                {
                    line.text = "Absolutely no knockback";//change tooltip
                }
            }
        }
    }

    public class RiptideP : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Riptide");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.sentry = true;
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            //Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        private NPC target;

        private float maxDistance = 1000f;
        private float distance;
        private int timer;
        private int reloadTime = 6;
        private int si = 1;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.UpdateMaxTurrets();

            if (QwertyMethods.ClosestNPC(ref target, maxDistance, Projectile.Center, false, player.MinionAttackTargetNPC))
            {
                timer++;
                Projectile.rotation = (target.Center - Projectile.Center).ToRotation();
                if (timer % reloadTime == 0)
                {
                    if (timer % (reloadTime * 2) == 0)
                    {
                        Projectile.frame = 1;
                        si = 1;
                    }
                    else
                    {
                        Projectile.frame = 2;
                        si = -1;
                    }

                    Vector2 shootFrom = Projectile.Center + QwertyMethods.PolarVector(12, Projectile.rotation) + QwertyMethods.PolarVector(si * 4, Projectile.rotation + (float)Math.PI / 2);
                    //if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), shootFrom, QwertyMethods.PolarVector(1, Projectile.rotation), ProjectileType<RiptideStream>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            else
            {
                timer = 0;
                Projectile.frame = 0;
            }
        }
    }

    public class RiptideStream : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 99;
            Projectile.timeLeft = 1200;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            if (Main.rand.Next(8) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, 172)];
                d.velocity *= .1f;
                d.noGravity = true;
                d.position = Projectile.Center;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            knockback = 0;
        }
    }
}