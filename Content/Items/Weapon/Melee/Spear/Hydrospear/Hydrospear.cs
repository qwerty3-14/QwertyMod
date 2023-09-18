using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrospear
{
    public class Hydrospear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Hydrospear");
            //Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Spears[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.shootSpeed = 37f;
            Item.knockBack = 6f;
            Item.width = 50;
            Item.height = 46;
            Item.scale = 1f;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // Important because the spear is actually a projectile instead of an Item. This prevents the melee hitbox of this Item.
            Item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this Item.
            //Item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()
            Item.channel = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<HydrospearP>();
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
    public class HydrospearP : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 153;
            stabStart = 153f - 26f;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
        }
        private bool noDust = false;
        private int streamCounter = 0;
        public override void SpearActive()
        {
            if(noDust)
            {
                noDust = false;
                return;
            }
            Dust k = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-12, aimDirection), DustID.DungeonWater);
            k.velocity = Vector2.Zero;
        }
        public override void Channeling()
        {
            Player player = Main.player[Projectile.owner];
            if (Collision.CanHit(player.Center, 0, 0, Projectile.Center, 0, 0))
            {
                noDust = true;
                streamCounter++;
                if (streamCounter % 16 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + QwertyMethods.PolarVector(-12, aimDirection), QwertyMethods.PolarVector(1, aimDirection), ProjectileType<HydrospearStream>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
    public class HydrospearStream : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 99;
            Projectile.timeLeft = 1200;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
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
    }
}