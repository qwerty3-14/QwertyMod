using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Tool.Mining
{
    public class Hydrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mines a 3x3 area");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 72;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 12;
            Item.useTime = 6;
            Item.useAnimation = 25;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.pick = 195;
            Item.tileBoost++;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 250000;
            Item.rare = 5;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<HydrillP>();
            Item.shootSpeed = 40f;
            Item.tileBoost = -2;
            Item.GetGlobalItem<AoePick>().miningRadius = 1;
        }
    }

    public class HydrillP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 1.9f);
            Main.dust[dust].noGravity = true;
        }
    }
}