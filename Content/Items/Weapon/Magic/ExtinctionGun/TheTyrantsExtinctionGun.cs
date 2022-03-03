using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.ExtinctionGun
{
    public class TheTyrantsExtinctionGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Tyrant's Extinction Gun");
            Tooltip.SetDefault("Left click shoots climate change!" + "\nRight click shoots disease!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 1;
            Item.rare = 6;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.width = 66;
            Item.height = 24;
            Item.useStyle = 5;
            Item.shootSpeed = 10f;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.mana = 5;
            Item.shoot = ProjectileType<SnowFlakeF>();
            Item.noUseGraphic = false;
            Item.noMelee = true;
            //Item.UseSound = 16
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ProjectileType<MosquittoF>();
            }
            else
            {
                Item.shoot = ProjectileType<SnowFlakeF>();
            }
            SoundEngine.PlaySound(16, player.Center, 0);
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 28f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false ;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -4);
        }
    }

    public class MosquittoF : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mosquitto");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 36;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            AIType = ProjectileID.Bee;
            //animationType = ProjectileID.Bee;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<DinoPox>(), 480);
        }
    }

    public class SnowFlakeF : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Flake");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 0;

            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Projectile.rotation += 1.5f;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            for (int k = 0; k < 200; k++)
            {
                Projectile.localNPCImmunity[k] = 0;
            }
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
    }
}