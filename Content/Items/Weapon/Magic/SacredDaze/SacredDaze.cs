using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.SacredDaze
{
    public class SacredDaze : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Sacred Daze");
            //Tooltip.SetDefault("Stuns those who are not worthy!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.crit = 30;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 1;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.width = 26;
            Item.height = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 10f;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 6 : 4;
            Item.shoot = ProjectileType<SacredDazeP>();
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<CaeliteBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
    public class SacredDazeP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Sacred Daze");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.light = 1f;
            Projectile.extraUpdates = 2;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
                dust.velocity *= 3f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss)
            {
                target.AddBuff(BuffType<Stunned>(), 12);
            }
            if (Main.rand.NextBool(10))
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
        }
    }
}
