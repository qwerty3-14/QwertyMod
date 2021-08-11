using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using QwertyMod.Content.Dusts;

namespace QwertyMod.Content.Items.Consumable.Tile.Fortress.Gadgets
{
    public class ReverseSandT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            DustType = DustType<DnasDust>();

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Dnas");
            AddMapEntry(Color.Blue, name);

            ItemDrop = ItemType<ReverseSand>();
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j - 1].IsActive)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new ProjectileSource_TileInteraction(Main.LocalPlayer, i, j),  new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ProjectileType<ReverseSandBall>(), 50, 0f, Main.myPlayer);
            }
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (!Main.tile[i, j - 1].IsActive)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new ProjectileSource_TileInteraction(Main.LocalPlayer, i, j),  new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ProjectileType<ReverseSandBall>(), 50, 0f, Main.myPlayer);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Vector2 entityCoord = new Vector2(i, j) * 16 + new Vector2(8, 8);
            if (!Main.tile[i, j - 1].IsActive)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new ProjectileSource_TileInteraction(Main.LocalPlayer, i, j), entityCoord, Vector2.Zero, ProjectileType<ReverseSandBall>(), 50, 0f, Main.myPlayer);
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
            if (Main.rand.Next(2) == 0)
            {
                int num129 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustType<DnasDust>(), 0f, Projectile.velocity.Y / 2f, 0, default(Color), 1f);
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

        public override void Kill(int timeLeft)
        {
            int i = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
            int j = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
            int tileToPlace = 0;
            int num835 = 2;

            {
                tileToPlace = TileType<ReverseSandT>();
                num835 = 0;
            }
            /*
            if (Main.tile[i, j].halfBrick() && Projectile.velocity.Y > 0f && Math.Abs(Projectile.velocity.Y) > Math.Abs(Projectile.velocity.X))
            {
                j--;
            }*/
            if (!Main.tile[i, j].IsActive && tileToPlace >= 0)
            {
                WorldGen.PlaceTile(i, j, tileToPlace, false, true, -1, 0);

                /*
                if (!flag5 && Main.tile[i, j].active() && (int)Main.tile[i, j].type == tileToPlace)
                {
                    if (Main.tile[i, j + 1].halfBrick() || Main.tile[i, j + 1].slope() != 0)
                    {
                        WorldGen.SlopeTile(i, j + 1, 0);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(17, -1, -1, null, 14, (float)i, (float)(j + 1), 0f, 0, 0, 0);
                        }
                    }
                    if (Main.netMode != 0)
                    {
                        NetMessage.SendData(17, -1, -1, null, 1, (float)i, (float)j, (float)tileToPlace, 0, 0, 0);
                    }
                }*/
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
            if (xPos < Main.tile.GetLength(0) && yPos < Main.tile.GetLength(1) && yUpper < Main.tile.GetLength(1) && xPos > 0 && yPos > 0 && yUpper > 0) //hopefully this prevents index outside bounds of array error
            {
                if (Main.tile[xPos, yUpper].type == TileType<ReverseSandT>() || Main.tile[xPos, yPos].type == TileType<ReverseSandT>() ||
                Main.tile[xPos, yUpper].type == TileType<DnasBrickT>() || Main.tile[xPos, yPos].type == TileType<DnasBrickT>())
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