using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Top.Cyclone
{
    public class Cyclone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyclone");
            Tooltip.SetDefault("<3");
        }
        public override void SetDefaults()
        {
            Item.damage = 240;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = 8;
            Item.width = 34;
            Item.height = 38;
            Item.useStyle = 1;
            Item.shootSpeed = 3f;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.shoot = ProjectileType<CycloneP>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
    }
    public class CycloneP : Top
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyclone");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;

            Projectile.width = 34;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.tileCollide = true;
            friction = .001f;
            enemyFriction = 0f;
            frameDelay = 4;
        }
        int maxSegments = 48;
        int segments = 0;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (hitGround && Projectile.friendly)
            {
                projHitbox.Height += (segments * 8);
                projHitbox.Y -= (segments * 8);
                return Collision.CheckAABBvAABBCollision(projHitbox.TopLeft(), projHitbox.Size(), targetHitbox.TopLeft(), targetHitbox.Size());
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        float trigCounter = 0;
        public override void ExtraTopNonesense()
        {
            if(hitGround && Projectile.friendly)
            {
                if(Projectile.frameCounter % 2 == 0 && segments < maxSegments)
                {
                    segments++;
                }
                trigCounter += (float)Math.PI / 30f;
            }
        }
        public override void TopHit(NPC target)
        {
            target.velocity.Y -= 1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (hitGround && Projectile.friendly)
            {
                Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Melee/Top/Cyclone/CycloneSpout").Value;
                int height = texture.Height / 6;
                float spoutRadius = 10f;
                for (int i = 0; i < segments; i++)
                {
                    int frame = ((Projectile.frameCounter/5) + i) % 6;
                    Vector2 pos = Projectile.Center - Vector2.UnitY * i * height - Vector2.UnitY + Vector2.UnitX * (spoutRadius * (float)Math.Sin(trigCounter + ((float)Math.PI/3f * i)));
                    Main.EntitySpriteDraw(texture, pos - Main.screenPosition, new Rectangle(0, height * frame, texture.Width, height), lightColor, 0, new Vector2(texture.Width, height) * .5f, 1, 0, 0);
                }
            }
            return base.PreDraw(ref lightColor);
        }
    }
}
