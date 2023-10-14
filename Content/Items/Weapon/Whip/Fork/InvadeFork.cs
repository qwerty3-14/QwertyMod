using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Whip.Fork
{
    public class InvaderFork : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {

            Item.DefaultToWhip(ModContent.ProjectileType<InvaderForkP>(), 111, 3, 4, 30);
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.rare = ItemRarityID.Yellow;
            Item.value = GearStats.InvaderGearValue;
            Item.width = 38;
            Item.height = 36;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Whip/Fork/InvaderFork_Glow").Value;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class InvaderForkP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void WhipDefaults()
        {
            Projectile.WhipSettings.Segments = 28;
            Projectile.WhipSettings.RangeMultiplier = 3f;

            originalColor = new Color(98, 119, 140);
            fallOff = 0.25f;
            tag = ModContent.BuffType<ForkTag>();
        }
    }
    public class ForkTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
    public class TagMissile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 480;
            Projectile.friendly = true;
        }
        bool exploded = false;
        void explode()
        {
            if (!exploded)
            {
                exploded = true;
                Projectile.timeLeft = 5;
                Projectile.width = 15;
                Projectile.height = 15;
                Projectile.position -= Vector2.One * 12;
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 30; i++)
                {
                    float rot = MathF.PI * 2f * ((float)i / 30f);
                    Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<InvaderGlow>(), QwertyMethods.PolarVector(3f, rot));
                }
            }
        }
        
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft > 480 - 30)
            {
                return false;
            }
            return null;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            explode();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            explode();
            return false;
        }
        bool runOnce = true;
        public override void AI()
        {
            if (!exploded)
            {
                if (runOnce)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    runOnce = false;
                }
                NPC target = null;
                if(QwertyMethods.ClosestNPC(ref target, 1200, Projectile.Center, false, Main.player[Projectile.owner].MinionAttackTargetNPC))
                {
                    Projectile.rotation.SlowRotation((target.Center - Projectile.Center).ToRotation(), MathF.PI / 120f);
                }
                Projectile.velocity = QwertyMethods.PolarVector(6, Projectile.rotation);
                Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-4, Projectile.rotation), ModContent.DustType<InvaderGlow>(), Vector2.Zero, Scale: 0.2f);
                if (Projectile.timeLeft < 5)
                {
                    explode();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (exploded)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        Projectile.Size * 0.5f, 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderMicroMissile_Glow").Value, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), Color.White, Projectile.rotation,
                        Projectile.Size * 0.5f, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}
