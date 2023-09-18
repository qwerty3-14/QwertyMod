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
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60 * 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public int dustTimer;
        private bool runOnce = true;
        private float iceRuneSpeed = 10;
        float iceRuneRotCounter = 0;
        float runeDist = 100;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.frame = (int)Projectile.ai[1];
            dustTimer++;
            iceRuneRotCounter += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / iceRuneSpeed));

                

            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<IceRuneDeath>(), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            for(int i = 0; i < 4; i++)
            {
                float rot = iceRuneRotCounter + (MathF.PI * 2f * (i / 4f));
                Vector2 runeCenter = Projectile.Center + QwertyMethods.PolarVector(runeDist, rot);
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/RuneWave/IceRuneTome").Value;
                Main.EntitySpriteDraw(texture, runeCenter - Main.screenPosition, null, Color.White, iceRuneRotCounter, new Vector2(18, 18), 1f, SpriteEffects.None, 0);
            }
            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for(int i = 0; i < 4; i++)
            {
                float rot = iceRuneRotCounter + (MathF.PI * 2f * (i / 4f));
                Vector2 runeCenter = Projectile.Center + QwertyMethods.PolarVector(runeDist, rot);
                if(Collision.CheckAABBvAABBCollision(runeCenter + new Vector2(-18, -18), new Vector2(36, 36), targetHitbox.TopLeft(), targetHitbox.Size()))
                {
                    return true;
                }
            }
            return null;
        }
    }
}