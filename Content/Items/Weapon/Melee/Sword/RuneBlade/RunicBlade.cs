using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.RuneBlade
{
    public class RunicBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Blade");
            Tooltip.SetDefault("Launches a spread Mini Ice Runes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.value = 500000;
            Item.rare = 9;
            Item.UseSound = SoundID.Item1;

            Item.width = 70;
            Item.height = 70;

            Item.autoReuse = true;
            Item.shoot = ProjectileType<MiniIceRune>();
            Item.shootSpeed = 9;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Melee/Sword/RuneBlade/RunicBlade_Glow").Value;
            }
        }



        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 15)
                .AddIngredient(ItemType<AncientBlade.AncientBlade>())
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 15 + Main.rand.Next(6);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15)); // 30 degree spread.
                                                                                             // If you want to randomize the speed to stagger the projectiles
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false; // return false because we don't want tmodloader to shoot projectile
        }
    }
    public class MiniIceRune : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            DisplayName.SetDefault("Mini Ice Rune");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 40;
        }

        public int dustTimer;

        public override void AI()
        {
            //Projectile.rotation += (float)((2 * Math.PI) / (Math.PI * 20));
            dustTimer++;
            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<IceRuneDeath>(), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 10; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<IceRuneDeath>());
            }
        }


        public override bool OnTileCollide(Vector2 velocityChange)
        {
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
            target.AddBuff(BuffID.Frostburn, 1200);
        }

    }
}
