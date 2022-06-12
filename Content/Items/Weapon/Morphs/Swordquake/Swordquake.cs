using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Morphs.Swordquake
{
    public class Swordquake : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Swordquake");
            Tooltip.SetDefault("Turn into a sword that causes a deadly swordquake upon striking the ground!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public const int dmg = 900;
        public const int crt = 0;
        public const float kb = 9f;
        public const int def = -1;

        public override void SetDefaults()
        {
            Item.damage = dmg;
            Item.crit = crt;
            Item.knockBack = kb;
            Item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 32;
            Item.noMelee = true;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = 5;

            Item.rare = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.noUseGraphic = true;
            Item.width = 38;
            Item.height = 38;

            //Item.autoReuse = true;
            Item.shoot = ProjectileType<SwordquakeP>();
            Item.shootSpeed = 1f;
            Item.channel = true;
        }
    }

    public class SwordquakeP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordquake");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 1;
        }

        private bool runOnce = true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.Center = Projectile.Center;
            player.immune = true;
            player.immuneTime = 2;
            player.statDefense = 0;
            Projectile.velocity = Vector2.Zero;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            if (runOnce)
            {
                Projectile.rotation = player.direction == 1 ? (float)Math.PI : 0;
                runOnce = false;
            }

            if (Projectile.ai[0] == 2)
            {
                player.GetModPlayer<SwordQuakeShake>().shake = true;
            }
            else
            {
                if (Projectile.timeLeft < 30)
                {
                    Projectile.rotation += player.direction * (float)Math.PI / 15;
                    if (!Collision.CanHit(Projectile.Center, 0, 0, Projectile.Center + QwertyMethods.PolarVector(180, Projectile.rotation), 0, 0))
                    {
                        Projectile.timeLeft = 30;
                        Projectile.ai[0] = 2;
                        Vector2 start = Projectile.Center + QwertyMethods.PolarVector(180, Projectile.rotation) + player.direction * 14 * Vector2.UnitX;

                        Point point;
                        while (WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Up(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
                        {
                            start += -Vector2.UnitY;
                        }
                        while (!WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
                        {
                            start += Vector2.UnitY;
                        }
                        start += Vector2.UnitY * 20;
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), start, Vector2.Zero, ProjectileType<SwordlagmitePlayer>(), Projectile.damage, Projectile.knockBack, Projectile.owner, player.direction, 40);
                    }
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float CP = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(-12, Projectile.rotation), Projectile.Center + QwertyMethods.PolarVector(194 - 12, Projectile.rotation), 34, ref CP) && Projectile.timeLeft < 30;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(12, texture.Height / 2), 1f, 0, 0);
            return false;
        }
    }

    public class SwordlagmitePlayer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordlagmite");
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 2;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 61;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.usesIDStaticNPCImmunity = true;
            int immutime = 300;
            Projectile.perIDStaticNPCImmunity[Projectile.type][target.whoAmI] = (uint)(Main.GameUpdateCount + immutime);

        }
        private const int lingerTime = 60;
        private const int extendSpeed = 30;
        private int heightMax = 150;
        private Projectile next = null;

        public override void AI()
        {
            if (Projectile.timeLeft == lingerTime)
            {
                Projectile.height += extendSpeed;
                Projectile.position.Y -= extendSpeed;

                if (Projectile.height < heightMax)
                {
                    Projectile.timeLeft++;
                }
            }
            if (Projectile.timeLeft == lingerTime - 1)
            {
                SoundEngine.PlaySound(SoundID.Item69, Projectile.Center);
                if (Projectile.ai[1] > 0)
                {
                    Vector2 start = Projectile.Bottom + Projectile.ai[0] * 28 * Vector2.UnitX;
                    Point point;
                    while (WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Up(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
                    {
                        start += -Vector2.UnitY;
                    }
                    while (!WorldUtils.Find(start.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[] { new Conditions.IsSolid() }), out point))
                    {
                        start += Vector2.UnitY;
                    }
                    start += Vector2.UnitY * 20;
                    next = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), start, Vector2.Zero, ProjectileType<SwordlagmitePlayer>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0], Projectile.ai[1] - 1)];
                }
            }
            if (Projectile.timeLeft == 1)
            {
                Projectile.height -= extendSpeed;
                Projectile.position.Y += extendSpeed;
                if (Projectile.height > extendSpeed)
                {
                    Projectile.timeLeft++;
                }
            }
        }

        

        public override bool PreDraw(ref Color lightColor)
        {
            int tipHeight = 14;
            int segmentHeight = 12;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int k = 0;
            Main.EntitySpriteDraw(texture, Projectile.position + Vector2.UnitY * (k * segmentHeight) - Main.screenPosition, new Rectangle(0, 0, Projectile.width, 82), Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.position.Y + tipHeight / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);

            for (; k < ((Projectile.height - tipHeight) / segmentHeight) - 1; k++)
            {
                Main.EntitySpriteDraw(texture, Projectile.position + Vector2.UnitY * (k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, Projectile.width, segmentHeight), Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.position.Y + (k * segmentHeight) + tipHeight + segmentHeight / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.position + Vector2.UnitY * (k * segmentHeight + tipHeight) - Main.screenPosition, new Rectangle(0, tipHeight, Projectile.width, (Projectile.height - tipHeight) % segmentHeight), Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.position.Y + (k * segmentHeight) + tipHeight + ((Projectile.height - tipHeight) % segmentHeight) / 2) / 16), 0, Vector2.Zero, 1f, 0, 0);
            return false;
        }
    }
}