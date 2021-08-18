using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Hook
{
    internal class LaunchingHook : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Launching Hook");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AmethystHook);
            Item.shootSpeed = 18f; // how quickly the hook is shot.
            Item.shoot = ProjectileType<LaunchingHookP>();
            Item.value = 50000;
            Item.rare = 5;
        }
    }

    internal class LaunchingHookP : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            Projectile.width = 18;
            Projectile.height = 20;
        }

        // Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            for (int l = 0; l < 1000; l++)
            {
                if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type)
                {
                    hooksOut++;
                }
            }
            if (hooksOut > 0)
            {
                return false;
            }
            return true;
        }

        // Amethyst Hook is 300, Static Hook is 600
        public override float GrappleRange()
        {
            return 160f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 20f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 24f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if ((Projectile.Center - player.Center).Length() < 100 && player.grappling[0] >= 0)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            float directionToHook = (Projectile.Center - player.Center).ToRotation();
            float distanceToHook = (Projectile.Center - player.Center).Length();
            Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/Hook/LHChain").Value;
            for (int d = 0; d < distanceToHook; d += texture.Height)
            {
                Main.EntitySpriteDraw(texture, player.Center + QwertyMethods.PolarVector(d, directionToHook) - Main.screenPosition,
                       new Rectangle(0, 0, texture.Width, texture.Height), lightColor, directionToHook + (float)Math.PI / 2,
                       new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}