using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun
{
    public class HydraCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Cannon");
            Tooltip.SetDefault("Killing enemies releases a powerful wave of destruction");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = DamageClass.Ranged;

            Item.useAnimation = 12;
            Item.useTime = 6;
            Item.reuseDelay = 14;

            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.value = 250000;
            Item.rare = 5;
            Item.UseSound = SoundID.Item11;

            Item.width = 54;
            Item.height = 64;

            Item.shoot = 97;
            Item.useAmmo = 97;
            Item.shootSpeed = 36;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(8, 4);
        }

        public override void HoldItem(Player player)
        {
            player.GetModPlayer<HydraCannonPlayer>().hydraCannon = true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            // We can get the Clockwork Assault Riffle Effect by not consuming ammo when itemAnimation is lower than the first shot.
            return !(player.itemAnimation < Item.useAnimation - 2);
        }
    }

    public class DoomBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doom Breath");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 96;
            Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        private int frameCounter;

        public override void AI()
        {
            frameCounter++;
            if (frameCounter > 20)
            {
                frameCounter = 0;
            }
            else if (frameCounter > 10)
            {
                Projectile.frame = 1;
            }
            else
            {
                Projectile.frame = 0;
            }
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBreathGlow>());
        }
    }
    public class HydraCannonPlayer : ModPlayer
    {
        public bool hydraCannon = false;
        public override void ResetEffects()
        {
            hydraCannon = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (hydraCannon && !target.immortal && target.life <= 0 && proj.CountsAsClass(DamageClass.Ranged) && proj.type != ProjectileType<DoomBreath>() && !target.SpawnedFromStatue)
            {
                SoundEngine.PlaySound(SoundID.Roar, Player.position);

                Projectile.NewProjectile(Projectile.InheritSource(proj), Player.Center, (target.Center - Player.Center).SafeNormalize(Vector2.UnitY) * 24f, ProjectileType<DoomBreath>(), damage * 5, knockback * 3, Player.whoAmI);

                Main.rand.NextFloat(Player.width);
            }
        }
    }
}