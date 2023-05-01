using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.RuneWave
{
    public class RunicWave : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Runic Wave");
            //Tooltip.SetDefault("Cast a wave that draws ice runes in flight");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 180;
            Item.DamageType = DamageClass.Magic;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 100;
            Item.value = 500000;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.width = 28;
            Item.height = 30;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/RuneWave/RunicWave_Glow").Value;
            }
            Item.mana = ModLoader.HasMod("TRAEProject") ? 32 : 12;
            Item.shoot = ProjectileType<RunicWaveP>();
            Item.shootSpeed = 9;
            Item.noMelee = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 15)
                .AddIngredient(ItemType<AncientWave.AncientWave>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class RunicWaveP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Runic Wave");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60 * 3;
        }

        public int dustTimer;
        public int timer;
        private bool runOnce = true;
        private float iceRuneSpeed = 10;
        private Projectile ice1;
        private Projectile ice2;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.frame = (int)Projectile.ai[1];
            dustTimer++;
            timer++;

            if (runOnce)
            {
                float startDistance = 100;

                ice1 = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + MathF.Cos(0) * startDistance, Projectile.Center.Y + MathF.Sin(0) * startDistance, 0, 0, ProjectileType<IceRuneTome>(), Projectile.damage, 3f, Main.myPlayer)];
                ice2 = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + MathF.Cos(MathF.PI) * startDistance, player.Center.Y + MathF.Sin(MathF.PI) * startDistance, 0, 0, ProjectileType<IceRuneTome>(), Projectile.damage, 3f, Main.myPlayer)];
                runOnce = false;
            }
            ice1.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / iceRuneSpeed));
            ice1.velocity.X = iceRuneSpeed * MathF.Cos(ice1.rotation) + Projectile.velocity.X;
            ice1.velocity.Y = iceRuneSpeed * MathF.Sin(ice1.rotation) + Projectile.velocity.Y;

            ice2.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / iceRuneSpeed));
            ice2.velocity.X = iceRuneSpeed * MathF.Cos(ice2.rotation) + Projectile.velocity.X;
            ice2.velocity.Y = iceRuneSpeed * MathF.Sin(ice2.rotation) + Projectile.velocity.Y;

            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<IceRuneDeath>(), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }
    }

    internal class IceRuneTome : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60 * 3;
            Projectile.DamageType = DamageClass.Magic;
        }
        public float startDistance = 200f;
        public float runeSpeed = 10;
        public bool runOnce = true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                Projectile.rotation = (player.Center - Projectile.Center).ToRotation() - MathF.PI / 2;
                runOnce = false;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 1200);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
}