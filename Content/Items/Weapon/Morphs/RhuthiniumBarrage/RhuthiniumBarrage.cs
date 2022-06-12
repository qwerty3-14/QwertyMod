using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Morphs.RhuthiniumBarrage
{
    internal class RhuthiniumBarrage : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape shift: Rhuthinium Barrage");
            Tooltip.SetDefault("Launches a HUGE barrage of darts dealing massive damage!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public const int dmg = 26;
        public const int crt = 5;
        public const float kb = 0f;
        public const int def = -1;

        public override void SetDefaults()
        {
            Item.damage = dmg;
            Item.crit = crt;
            Item.knockBack = kb;
            Item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 40;
            Item.noMelee = true;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = 5;

            Item.value = 25000;
            Item.rare = 3;
            Item.crit = 5;
            Item.noUseGraphic = true;
            Item.width = 18;
            Item.height = 32;

            //Item.autoReuse = true;
            Item.shoot = ProjectileType<RhuthiniumBarrageLauncher>();
            Item.shootSpeed = 0f;
            Item.channel = true;
            Item.DamageType = DamageClass.Summon;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<RhuthiniumBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if ((Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot) || player.HasBuff(BuffType<MorphCooldown>()))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class RhuthiniumBarrageLauncher : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Barrage Launcher");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
        }

        private Deck<Projectile> Darts = new Deck<Projectile>();
        private bool runOnce = true;
        private int indexCounter = 0;

        public override void AI()
        {
            if (runOnce)
            {
                for (int d = 0; d < 60; d++)
                {
                    Darts.Add(Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center, Vector2.Zero, ProjectileType<RhuthiniumBarrageDart>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.Next(-14, 15), 0f)]);
                }
                runOnce = false;
            }

            Player player = Main.player[Projectile.owner];
            player.Center = Projectile.Center;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.statDefense = 0;
            player.immune = true;
            player.immuneTime = 2;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            Projectile.rotation = (QwertyMod.GetLocalCursor(Projectile.owner) - Projectile.Center).ToRotation();

            foreach (Projectile dart in Darts)
            {
                if (dart.ai[1] == 0 && dart.type == ProjectileType<RhuthiniumBarrageDart>())
                {
                    dart.Center = Projectile.Center + QwertyMethods.PolarVector(25, Projectile.rotation) + QwertyMethods.PolarVector(dart.ai[0], Projectile.rotation + (float)Math.PI / 2);
                    dart.rotation = Projectile.rotation;
                }
            }
            if (Projectile.timeLeft < Darts.Count + 30)
            {
                if (indexCounter < Darts.Count)
                {
                    Darts[indexCounter].ai[1] = 1f;
                    indexCounter++;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawDart = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Morphs/RhuthiniumBarrage/RhuthiniumBarrageDart").Value;
            foreach (Projectile dart in Darts)
            {
                if (dart != null && dart.active && dart.type == ProjectileType<RhuthiniumBarrageDart>())
                {
                    Main.EntitySpriteDraw(drawDart, dart.Center - Main.screenPosition,
                       drawDart.Frame(), Lighting.GetColor((int)dart.Center.X / 16, (int)dart.Center.Y / 16), dart.rotation,
                       new Vector2(drawDart.Width, drawDart.Height * .5f), 1f, 0, 0);
                }
            }
            Texture2D Launcher = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(Launcher, Projectile.Center - Main.screenPosition,
                       Launcher.Frame(), lightColor, Projectile.rotation,
                       new Vector2(0, Launcher.Height * .5f), 1f, 0, 0);
            return false;
        }
    }

    public class RhuthiniumBarrageDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Barrage");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<RhuthiniumDust>());
                d.frame.Y = Main.rand.Next(2) == 0 ? 0 : 10;
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 1f)
            {
                Projectile.friendly = true;
                Projectile.tileCollide = true;
                Projectile.extraUpdates = 2;
                Projectile.velocity = QwertyMethods.PolarVector(8, Projectile.rotation);
            }
        }
    }
}