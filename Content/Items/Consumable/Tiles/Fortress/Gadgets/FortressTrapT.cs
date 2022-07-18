using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class FortressTrapT : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSolid[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.AnchorLeft = default(AnchorData);
            TileObjectData.newTile.AnchorRight = default(AnchorData);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);

            DustType = DustType<FortressDust>();
            HitSound = QwertyMod.FortressBlocks;
            MinPick = 50;
            AddMapEntry(new Color(162, 184, 185));
            MineResist = 1;
            ItemDrop = ItemType<FortressTrap>();
        }

        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i + 1, j].HasTile || Main.tile[i - 1, j].HasTile || Main.tile[i, j + 1].HasTile || Main.tile[i, j - 1].HasTile;
        }

        public override bool Slope(int i, int j)
        {
            int num248 = 0;

            switch (Main.tile[i, j].TileFrameX / 18)
            {
                case 0:
                    num248 = 2;
                    break;

                case 1:
                    num248 = 3;
                    break;

                case 2:
                    num248 = 4;
                    break;

                case 3:
                    num248 = 5;
                    break;

                case 4:
                    num248 = 1;
                    break;

                case 5:
                    num248 = 0;
                    break;
            }

            Main.tile[i, j].TileFrameX = (short)(num248 * 18);
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1, TileChangeType.None);
            }
            return false;
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.player[Main.myPlayer].dangerSense)
            {
                Color drawColor = drawData.colorTint;
                if (drawColor.R < 255)
                {
                    drawColor.R = 255;
                }
                if (drawColor.G < 50)
                {
                    drawColor.G = 50;
                }
                if (drawColor.B < 50)
                {
                    drawColor.B = 50;
                }
                drawColor.A = Main.mouseTextColor;
                if (!Main.gamePaused && Main.rand.Next(30) == 0)
                {
                    int num51 = Dust.NewDust(new Vector2((float)(j * 16), (float)(i * 16)), 16, 16, 60, 0f, 0f, 100, default(Microsoft.Xna.Framework.Color), 0.3f);
                    Main.dust[num51].fadeIn = 1f;
                    Main.dust[num51].velocity *= 0.1f;
                    Main.dust[num51].noLight = true;
                    Main.dust[num51].noGravity = true;
                }
            }
        }

        public override void HitWire(int i, int j)
        {
            if (Wiring.CheckMech(i, j, 60))
            {
                Vector2 velocity = Vector2.Zero;
                if (Main.tile[i, j].TileFrameX < 18)
                {
                    velocity = new Vector2(-.001f, 0);
                }
                else if (Main.tile[i, j].TileFrameX < 36)
                {
                    velocity = new Vector2(.001f, 0);
                }
                else if (Main.tile[i, j].TileFrameX < 72)
                {
                    velocity = new Vector2(0, -.001f);
                }
                else if (Main.tile[i, j].TileFrameX < 108)
                {
                    velocity = new Vector2(0, .001f);
                }
                Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), new Vector2(i, j) * 16 + new Vector2(8, 8) + velocity.SafeNormalize(-Vector2.UnitY) * 16, velocity, ProjectileType<FortressTrapP>(), 18, .5f, Main.myPlayer, 0f);
                Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), new Vector2(i, j) * 16 + new Vector2(8, 8) + velocity.SafeNormalize(-Vector2.UnitY) * 16, velocity, ProjectileType<FortressTrapP>(), 18, .5f, Main.myPlayer, 20f);
            }
        }
    }

    public class FortressTrapP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caleite Pulse");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.light = .6f;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
        }

        private int timer = 0;

        public override void AI()
        {
            if (timer == (int)Projectile.ai[0])
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 8f;
                Projectile.alpha = 0;
            }
            else if (timer > (int)Projectile.ai[0])
            {
                if (Main.rand.Next(2) == 0)
                {
                    Dust dust2 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
                    dust2.scale = .5f;
                }
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.frameCounter++;
                if (Projectile.frameCounter % 10 == 0)
                {
                    Projectile.frame++;
                    if (Projectile.frame > 1)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            timer++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<PowerDown>(), 1200);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<PowerDown>(), 1200);
        }
    }
}