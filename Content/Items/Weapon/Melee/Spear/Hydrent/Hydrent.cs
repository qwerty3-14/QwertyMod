using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrent
{
    public class Hydrent : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Fire Hydrent");
            //Tooltip.SetDefault("Shoots hydra breath");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Spears[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.shootSpeed = 3.7f;
            Item.knockBack = 6.5f;
            Item.width = 104;
            Item.height = 104;
            Item.scale = 1f;
            Item.value = 250000;
            Item.rare = ItemRarityID.Pink;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // Important because the spear is actually a projectile instead of an Item. This prevents the melee hitbox of this Item.
            Item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this Item.
            Item.channel = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<HydrentP>();
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
    public class HydrentP : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 190;
            stabStart = 190f - 43f;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
        }
        private int streamCounter = 0;
        public override void Channeling()
        {
            Player player = Main.player[Projectile.owner];
            if (Collision.CanHit(player.Center, 0, 0, Projectile.Center, 0, 0))
            {
                streamCounter++;
                if (streamCounter % 6 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                     Projectile.Center + QwertyMethods.PolarVector(-7f + (7f * ((streamCounter / 6) % 3)), aimDirection - (MathF.PI / 2)), 
                     QwertyMethods.PolarVector(16f, aimDirection), 
                     ProjectileType<HydrentBreath>(), 
                     (int)(Projectile.damage * .8f), 
                     Projectile.knockBack, 
                     Projectile.owner);
                }
            }
        }
    }

    public class HydrentBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 14;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBreathGlow>());
        }
    }
}