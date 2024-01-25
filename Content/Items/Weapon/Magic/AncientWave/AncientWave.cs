using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;



namespace QwertyMod.Content.Items.Weapon.Magic.AncientWave
{
    public class AncientWave : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8;
            Item.value = 150000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.width = 28;
            Item.height = 30;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/AncientWave/AncientWave_Glow").Value;
            }
            Item.mana = ModLoader.HasMod("TRAEProject") ? 32 : 12;
            Item.shoot = ModContent.ProjectileType<AncientWaveP>();
            Item.shootSpeed = 9;
            Item.noMelee = true;
        }


    }

    public class AncientWaveP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60 * 2;
        }

        public int dustTimer;

        public override void AI()
        {
            dustTimer++;
            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AncientGlow>(), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }
        }

        public override bool PreDraw(ref Color drawColor)
        {
            drawColor = Color.White;
            return true;
        }
    }
}