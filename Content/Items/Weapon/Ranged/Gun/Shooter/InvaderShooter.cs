using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using System;
using Terraria.GameContent;
using System.Collections.Generic;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.Shooter
{
    public class InvaderShooter : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = QwertyMod.InvaderGearValue;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item11;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/Shooter/InvaderShooter_Glow").Value;
            }
            Item.width = 56;
            Item.height = 32;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 5;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float vel = velocity.Length();
            velocity.Normalize();
            position = player.MountedCenter + velocity * 30 + velocity.RotatedBy(-MathF.PI / 2f) * player.direction * 4;
            velocity *= 4f;
            //type = ModContent.ProjectileType<ShooterBeam>();
            Projectile projectile = Main.projectile[Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ShooterBeam>(), damage, knockback, player.whoAmI, type, vel)];
            
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }
    }
    
    public class ShooterBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 399;
            Projectile.timeLeft = 400;
            Projectile.friendly = true;
        }
        bool decaying = false;
        bool cantHit = false;
        Vector2 end;
        private void StartDecay()
        {
            if (!decaying)
            {
                Projectile.extraUpdates = 0;
                Projectile.timeLeft = 21;
                Projectile.velocity = Vector2.Zero;
                decaying = true;
                end = Projectile.Center;
                Projectile.Center = start;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 0;
            modifiers.HideCombatText();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Projectile.ai[1] * Vector2.UnitX.RotatedByRandom(MathF.PI * 2f) * 0.25f, (int)Projectile.ai[0], Projectile.damage - 1, Projectile.knockBack, Projectile.owner);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
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
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                for (int d = 0; d < (end - start).Length(); d += texture.Width)
                {
                    float rot = (end - (Vector2)start).ToRotation();
                    int frame = (21 - Projectile.timeLeft) / 3;
                    Main.EntitySpriteDraw(texture, start + QwertyMethods.PolarVector(d, rot) - Main.screenPosition, new Rectangle(0, frame * texture.Height / 7, texture.Width, texture.Height / 7), Color.White, rot, Vector2.UnitY * 8f, 1f, SpriteEffects.None, 0);
                }
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