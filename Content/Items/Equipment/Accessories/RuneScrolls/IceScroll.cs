using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using QwertyMod.Common.RuneBuilder;

using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Dusts;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls
{
    public class IceScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Scroll");
            Tooltip.SetDefault("Summons two ice runes to orbit you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 500000;
            Item.rare = 9;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 300;

            Item.width = 48;
            Item.height = 60;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ScrollEffects>().ice = true;
        }
    }

    internal class IceRuneFreindly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = 30;
            target.immune[Projectile.owner] = 0;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < 2; i++)
            {
                if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(dist, Projectile.rotation + i * (float)Math.PI) + new Vector2(-18, -18), new Vector2(36, 36)))
                {
                    return true;
                }
            }
            return false;
        }
        float dist = 50;
        int timer;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<ScrollEffects>().ice)
            {
                Projectile.timeLeft = 2;
            }
            timer++;
            Projectile.rotation += (float)Math.PI / 30f;
            Projectile.Center = player.Center;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 pos = Projectile.Center + QwertyMethods.PolarVector(dist, Projectile.rotation + i * (float)Math.PI) + new Vector2(-18, -18);
                for (int d = 0; d <= 40; d++)
                {
                    Dust.NewDust(pos, 36, 36, DustType<IceRuneDeath>());
                }
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 2; i++)
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
                Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.IceRune][frame], Projectile.Center + QwertyMethods.PolarVector(dist, Projectile.rotation + i * (float)Math.PI) - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(9, 9), Vector2.One * 2, 0, 0);
            }

            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 120);
        }
    }
}