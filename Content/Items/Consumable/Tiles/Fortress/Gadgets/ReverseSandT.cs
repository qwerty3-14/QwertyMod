using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class ReverseSandT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            DustType = ModContent.DustType<DnasDust>();

            //ModTranslation name = CreateMapEntryName();
            //name.SetDefault("Dnas");
            AddMapEntry(Color.Blue);

            //ItemDrop = ModContent.ItemType<ReverseSand>();
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j - 1].HasTile)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<ReverseSandBall>(), 50, 0f, Main.myPlayer);
            }
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (!Main.tile[i, j - 1].HasTile)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<ReverseSandBall>(), 50, 0f, Main.myPlayer);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Vector2 entityCoord = new Vector2(i, j) * 16 + new Vector2(8, 8);
            if (!Main.tile[i, j - 1].HasTile)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new EntitySource_TileBreak(i, j), entityCoord, Vector2.Zero, ModContent.ProjectileType<ReverseSandBall>(), 50, 0f, Main.myPlayer);
            }

            //if(Main.LocalPlayer.Top.Y- entityCoord.Y <16 && Main.LocalPlayer.Top.Y - entityCoord.Y >0 && Math.Abs(Main.LocalPlayer.Top.X-entityCoord.X)<16)
            {
                //Main.LocalPlayer.GetModPlayer<QwertyPlayer>().forcedAntiGravity = true;
                // Main.LocalPlayer.gravDir = -1f;
                //Main.LocalPlayer.gravControl2 = true;
            }
        }
    }

    public class ReverseSandBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.knockBack = 6f;
            Projectile.width = 14;
            Projectile.height = 14;
            //Projectile.aiStyle = 10;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            // Main.NewText(Projectile.width);
            Projectile.width = 14;
            Projectile.height = 14;
            if (Main.rand.NextBool(2))
            {
                int num129 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<DnasDust>(), 0f, Projectile.velocity.Y / 2f, 0, default(Color), 1f);
                Dust dust = Main.dust[num129];
                dust.velocity.X = dust.velocity.X * 0.4f;
            }

            Projectile.tileCollide = true;
            Projectile.localAI[1] = 0f;

            Projectile.velocity.Y = Projectile.velocity.Y - 0.41f;

            Projectile.rotation -= 0.1f;

            if (Projectile.velocity.Y < -10f)
            {
                Projectile.velocity.Y = -10f;
            }
            //Projectile.velocity = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, false, false, 1);
        }

        public override void OnKill(int timeLeft)
        {
            int i = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
            int j = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
            int tileToPlace = 0;


            tileToPlace = ModContent.TileType<ReverseSandT>();
            if (!Main.tile[i, j].HasTile && tileToPlace >= 0)
            {
                WorldGen.PlaceTile(i, j, tileToPlace, false, true, -1, 0);
            }
        }
    }

    public class ReverseGravity : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            int xPos = (int)(Player.Top.X) / 16;
            int yPos = (int)(Player.Top.Y) / 16;
            int yUpper = (int)(Player.Top.Y) / 16 - 1;
            if (xPos < Main.tile.Width && yPos < Main.tile.Height && yUpper < Main.tile.Height && xPos > 0 && yPos > 0 && yUpper > 0) //hopefully this prevents index outside bounds of array error
            {
                if (Main.tile[xPos, yUpper].TileType == ModContent.TileType<ReverseSandT>() || Main.tile[xPos, yPos].TileType == ModContent.TileType<ReverseSandT>() ||
                Main.tile[xPos, yUpper].TileType == ModContent.TileType<DnasBrickT>() || Main.tile[xPos, yPos].TileType == ModContent.TileType<DnasBrickT>())
                {
                    //player.gravDir = -1f;
                    //player.gravControl2 = true;
                    if (Player.GetModPlayer<AntiGravity>().forcedAntiGravity == 0)
                    {
                        Player.velocity.Y = 0;
                    }
                    Player.GetModPlayer<AntiGravity>().forcedAntiGravity = 10;
                }
            }
        }
    }
    public class AntiGravity : ModPlayer
    {
        public int forcedAntiGravity = 0;
        public override void PostUpdateEquips()
        {
            if (forcedAntiGravity > 0)
            {
                Player.gravDir = -1f;
                Player.gravControl2 = true;
                forcedAntiGravity--;

                if (forcedAntiGravity == 1)
                {
                    Player.velocity.Y = 0;
                }
            }
            if (forcedAntiGravity < 0)
            {
                forcedAntiGravity = 0;
            }
        }
    }
}