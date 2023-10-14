using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Magic.EnlightenedConfusion
{
    public class EnlightenedConfusion : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.crit = 773;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 1;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Orange;
            Item.width = 28;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 10f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 30 : 15;
            Item.shoot = ModContent.ProjectileType<EnlightenedConfusionP>();
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<SacredDaze.SacredDaze>())
            .AddIngredient(ModContent.ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

    }
    public class EnlightenedConfusionP : ModProjectile
    {

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
                    float rot = MathF.PI * 2f * ((float)i / 30f);
                    Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<CaeliteDust>(), QwertyMethods.PolarVector(6f, rot));
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            explode();
            if (!target.boss)
            {
                target.AddBuff(ModContent.BuffType<Stunned>(), 60);
            }
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(ModContent.BuffType<PowerDown>(), 120);
            }
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.CursedInferno, 240);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            explode();
            return false;
        }
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
