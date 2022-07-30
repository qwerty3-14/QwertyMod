using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

namespace QwertyMod.Content.Items.Weapon.Magic.EnlightenedConfusion
{
    public class EnlightenedConfusion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enlightened Confusion");
            Tooltip.SetDefault("Doesd not actually inflict confusion, confusing.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.crit = 773;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 1;
            Item.value = 50000;
            Item.rare = 3;
            Item.width = 26;
            Item.height = 18;
            Item.useStyle = 5;
            Item.shootSpeed = 10f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 30 : 15;
            Item.shoot = ProjectileType<EnlightenedConfusionP>();
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<SacredDaze.SacredDaze>())
            .AddIngredient(ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

    }
    public class EnlightenedConfusionP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sacred Daze");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
            Projectile.extraUpdates = 2;
        }
        bool exploded = false;
        void explode()
        {
            if (!exploded)
            {
                exploded = true;
                Projectile.timeLeft = 5;
                Projectile.position -= Vector2.One * 31;
                Projectile.width = 80;
                Projectile.height = 80;
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 60; i++)
                {
                    float rot = (float)Math.PI * 2f * ((float)i / 30f);
                    Dust.NewDustPerfect(Projectile.Center, DustType<CaeliteDust>(), QwertyMethods.PolarVector(6f, rot));
                }
            }
        }
        
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft > 480 - 30)
            {
                return false;
            }
            return null;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            explode();
            if (!target.boss)
            {
                target.AddBuff(BuffType<Stunned>(), 60);
            }
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            explode();
            return false;
        }
        bool runOnce = true;
        public override void AI()
        {
            if (!exploded)
            {
                if (Projectile.timeLeft < 5)
                {
                    explode();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (exploded)
            {
                return false;
            }
            return true;
        }
    }
}
