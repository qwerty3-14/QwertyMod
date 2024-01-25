using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Sentry.Riptide
{
    public class Riptide : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<RiptideP>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Knockback") //this checks if it's the line we're interested in
                {
                    line.Text = Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipAbsNKB"));//change tooltip
                }
            }
        }
    }

    public class RiptideP : ModProjectile
    {

        public override void SetStaticDefaults()
        {
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
            Projectile.DamageType = DamageClass.Summon;
            //Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        private NPC target;
        private float maxDistance = 1000f;
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

                    Vector2 shootFrom = Projectile.Center + QwertyMethods.PolarVector(12, Projectile.rotation) + QwertyMethods.PolarVector(si * 4, Projectile.rotation + MathF.PI / 2);
                    //if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootFrom, QwertyMethods.PolarVector(1, Projectile.rotation), ModContent.ProjectileType<RiptideStream>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
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
            if (Main.rand.NextBool(8))
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonWater)];
                d.velocity *= .1f;
                d.noGravity = true;
                d.position = Projectile.Center;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= 0;
            modifiers.ArmorPenetration += 10;
        }
    }
}