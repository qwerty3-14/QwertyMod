﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Melee.Yoyo.Arsenal
{
    public class Arsenal : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 30;
            Item.height = 26;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.shootSpeed = 16f;
            Item.knockBack = 2.5f;
            Item.damage = 27;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Lime;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<ArsenalP>();
        }

        private Projectile yoyo;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int n = 0; n < 4; n++)
            {
                yoyo = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
                yoyo.localAI[1] = n;
            }

            return false;
        }
    }

    public class ArsenalP : QwertyYoyo
    {
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 160f; 
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 11f;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
            Projectile.localNPCHitCooldown = 30;
            Projectile.usesLocalNPCImmunity = true;
            yoyoCount = 4;
            time = 2f;
            range = ProjectileID.Sets.YoyosMaximumRange[Projectile.type] ;
            speed = ProjectileID.Sets.YoyosTopSpeed[Projectile.type];
            spread = MathF.PI / 4;
            //counterWeightId = mod.ProjectileType("SpiderCounterweight");
        }

        public override void YoyoHit(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
            target.immune[Projectile.owner] = 0;
        }

        public override void PostYoyoAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 30 == 0)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(4f + Main.rand.NextFloat(2f), MathF.PI * 2f * Main.rand.NextFloat()), ModContent.ProjectileType<ArsenalSword>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }

    public class ArsenalSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        private bool redirect = false;
        private NPC target;

        public override void AI()
        {
            if (!redirect)
            {
                Projectile.velocity *= .823f;
                Projectile.rotation += (Projectile.velocity.Length() * MathF.PI * .4f + MathF.PI / 60) * Math.Sign(Projectile.velocity.X);
                if (QwertyMethods.ClosestNPC(ref target, 300, Projectile.Center))
                {
                    redirect = true;
                    Projectile.velocity = QwertyMethods.PolarVector(6f, (target.Center - Projectile.Center).ToRotation());
                    Projectile.extraUpdates++;
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            if (Projectile.velocity.Length() < .1f || redirect)
            {
                if (Projectile.timeLeft > 60)
                {
                    Projectile.timeLeft = 60;
                }
                Projectile.alpha = (int)(255f * (1f - (Projectile.timeLeft / 60f)));
            }
        }
    }
}