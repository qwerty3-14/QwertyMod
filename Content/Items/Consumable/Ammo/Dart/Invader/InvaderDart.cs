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


namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Invader
{
    public class InvaderDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
            AmmoID.Sets.IsSpecialist[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.width = 10;
            Item.height = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Dart;
            Item.shoot = ModContent.ProjectileType<InvaderDartP>();
            Item.shootSpeed = 3;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
            Item.maxStack = 9999;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Consumable/Ammo/Dart/Invader/InvaderDart_Glow").Value;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ModContent.ItemType<InvaderPlating>())
                .Register();
        }
    }
    public class InvaderDartP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-6, Projectile.rotation + MathF.PI / 2), ModContent.DustType<InvaderGlow>(), Vector2.Zero, 100);
        }
        public override void OnKill(int timeLeft)
        {
            for (int r = 0; r < 1; r++)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DartSphere>(), Projectile.damage, 0, Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        Projectile.Size * 0.5f, 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Consumable/Ammo/Dart/Invader/InvaderDart_Glow").Value, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), Color.White, Projectile.rotation,
                        Projectile.Size * 0.5f, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
    public class DartSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.MagnetSphereBall);
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 50;
        }
        int counter;
        public override void AI()
        {
            counter++;
            if (counter % 10 == 1)
            {
                NPC target = null;
                if (QwertyMethods.ClosestNPC(ref target, 150, Projectile.Center))
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, QwertyMethods.PolarVector(3, (target.Center - Projectile.Center).ToRotation()), ModContent.ProjectileType<FriendlyInvaderZap>(), Projectile.damage, 0, Projectile.owner);
                }
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 4 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.rotation += MathF.PI / 240f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
    public class FriendlyInvaderZap : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 75;
            Projectile.extraUpdates = 74;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        private bool decaying = false;
        private bool cantHit = false;

        private void StartDecay()
        {
            if (!decaying)
            {
                Projectile.extraUpdates = 0;
                Projectile.timeLeft = 10;
                Projectile.velocity = Vector2.Zero;
                decaying = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private Vector2 start;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                start = Projectile.Center;
            }
            if (Projectile.timeLeft == 2 && !decaying)
            {
                StartDecay();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!runOnce)
            {
                float rot = (Projectile.Center - (Vector2)start).ToRotation();
                int c = decaying ? (int)(255f * Projectile.timeLeft / 10f) : 255;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, start - Main.screenPosition, null, new Color(c, c, c, c), rot, Vector2.UnitY * 1, new Vector2((Projectile.Center - start).Length() / 2f, 1), SpriteEffects.None, 0);
            }
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (cantHit)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}