using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.RuneBuilder;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls
{
    public class AggroScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aggro Scroll");
            Tooltip.SetDefault("An aggro rune occasionally fires");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 500000;
            Item.rare = 9;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 500;

            Item.width = 54;
            Item.height = 56;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ScrollEffects>().aggro = true;
        }
    }

    internal class AggroRuneFriendly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int timer;
        bool runOnce = true;
        Vector2 relativeVelocity = Vector2.Zero;
        Vector2 relativePosition = Vector2.Zero;
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<ScrollEffects>().aggro)
            {
                Projectile.timeLeft = 2;
            }
            timer++;
            if (runOnce)
            {
                relativePosition = QwertyMethods.PolarVector(50, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
                runOnce = false;
            }
            if (timer % 120 == 29)
            {
                relativeVelocity = Vector2.Zero;
            }
            if (timer % 120 == 90 && Main.netMode != 1)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), player.Center, QwertyMethods.PolarVector(1, Projectile.rotation), ProjectileType<AggroStrikeFriendly>(), Projectile.damage, 0, Projectile.owner);
            }
            if (timer % 120 == 119)
            {
                Vector2 goTo = QwertyMethods.PolarVector(50, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
                relativeVelocity = (goTo - relativePosition) / 30f;
            }
            relativePosition += relativeVelocity;
            Projectile.Center = player.Center + relativePosition;
            Projectile.rotation = (Projectile.Center - player.Center).ToRotation();

        }
        public override bool PreDraw(ref Color lightColor)
        {
            float c = (timer / 60f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = timer / 3;
            if (frame > 19)
            {
                frame = 19;
            }
            Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.Aggro][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(15.5f, 15.5f), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            if (timer % 120 > 30 && timer % 120 < 90)
            {
                Texture2D texture = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/RuneGhost/AggroLaser").Value;
                Main.EntitySpriteDraw(texture, Main.player[Projectile.owner].Center - Main.screenPosition, null, Color.White, Projectile.rotation, Vector2.UnitY, new Vector2(1500, 1), 0, 0);
            }
        }
    }
    public class AggroStrikeFriendly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
        bool runOnce = true;
        int timer;
        public override void AI()
        {
            timer++;
            if (runOnce)
            {
                runOnce = false;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < 5)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(1000, Projectile.rotation));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            int frame = timer / 2;
            if (timer > 22)
            {
                frame = (30 - timer) / 2;
            }
            if (frame > 3)
            {
                frame = 3;
            }
            float c = (float)frame / 3f;
            for (int i = 0; i < 3000; i += 8)
            {
                Main.EntitySpriteDraw(RuneSprites.aggroStrike[frame], Projectile.Center + QwertyMethods.PolarVector(i, Projectile.rotation) - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(0, 3), Vector2.One * 2, 0, 0);
            }

            return false;
        }
    }
}