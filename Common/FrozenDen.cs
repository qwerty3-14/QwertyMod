using Microsoft.Xna.Framework;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.NPCs.Bosses.TundraBoss;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Common
{
    public class FrozenDen : ModSystem
    {
        public static int denLength = 101;
        public static int denLowerHeight = 7;
        public static int denUpperHeight = 40;
        public static Vector2 BearSpawn = new Vector2(-1, -1);
        public static bool activeSleeper = false;
        static int entryWidth = 11;
        static int enntryWallWidth = 9;
        /*
        public override void OnWorldLoad()
        {
            BearSpawn = new Vector2(-1, -1);
        }
        */
        public static int[,,,] iglooBlueprints = new int[,,,]
        {
            { { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0},{0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},{0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 5, 0, 4, 0, 0, 1, 0, 0, 0},{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 6, 6, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0} }, { {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1},{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1},{1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1},{1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1},{1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1},{1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1},{1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1},{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1} }, { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 1, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 0, 0, 0},{0, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 1, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0} }, { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0} }, { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0} }, { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 72, 18, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 144, 144, 126, 54, 54, 36, 108, 144, 144, 18, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 108, 54, 0, 0, 0, 0, 18, 36, 0, 0, 0, 36, 144, 18, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 36, 54, 0, 0, 0, 0, 0, 0, 18, 36, 0, 0, 0, 0, 0, 36, 18, 0, 0, 0, 0},{0, 72, 54, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 18, 36, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0},{0, 72, 36, 144, 144, 144, 108, 108, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 36, 18, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 1296, 1314, 1332, 0, 0, 0, 90, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 1296, 1314, 1332, 0, 0, 0, 90, 0, 0, 0},{0, 0, 18, 18, 18, 54, 36, 54, 36, 108, 126, 126, 216, 54, 0, 0, 72, 162, 144, 126, 36, 36, 18, 18, 36, 72, 0, 0, 0} }, { {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 54, 0, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 54, 72, 72, 72, 36, 36, 36, 72, 72, 72, 54, 0, 0, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 54, 72, 72, 0, 0, 0, 594, 594, 594, 0, 0, 0, 72, 72, 54, 0, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 54, 72, 0, 0, 0, 0, 0, 612, 612, 612, 0, 0, 0, 0, 0, 72, 54, 0, 0, 0, 0},{0, 54, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 630, 630, 630, 0, 0, 0, 0, 0, 0, 36, 0, 0, 0, 0},{0, 72, 36, 72, 72, 72, 72, 72, 72, 0, 198, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 198, 0, 18, 0, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 72, 54, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1120, 0, 0, 0, 1120, 0, 0, 36, 0, 0, 0},{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1138, 18, 18, 18, 1138, 0, 0, 36, 0, 0, 0},{0, 54, 0, 0, 0, 0, 0, 0, 0, 72, 72, 72, 0, 630, 630, 630, 630, 0, 72, 72, 0, 0, 0, 0, 0, 36, 0, 0, 0} } }
        };
        public static List<int[]>[] IglooTileTypes = new List<int[]>[]
            {
                //Tiles
                new List<int[]>()
                {
                  new int[] {-1, 148, 34, 4, 15, 14, 19}
                },
                //walls
                new List<int[]>()
                {
                  new int[] {-1, 0, 31}
                }
            };

        //public override TagCompound Save()
        public override void SaveWorldData(TagCompound tag)
        {
            tag["BearSpawnX"] = BearSpawn.X;
            tag["BearSpawnY"] = BearSpawn.Y;
            tag["activeSleeper"] = activeSleeper;
        }

        //public override void Load(TagCompound tag)
        public override void LoadWorldData(TagCompound tag)
        {
            BearSpawn.X = tag.GetFloat("BearSpawnX");
            BearSpawn.Y = tag.GetFloat("BearSpawnY");
            activeSleeper = tag.GetBool("activeSleeper");
        }

        public static void GenerateDen(int x, int y)
        {
            //QwertyMethods.BreakTiles(x - (denLength-1)/2, y, denLength, denLowerHeight);

            for (int l = 0; l < denLength; l++)
            {
                for (int h = 0; h < denUpperHeight; h++)
                {
                    if (!Main.tile[(x - ((denLength - 1) / 2)) + l, y - h].HasTile)
                    {
                        WorldGen.PlaceTile((x - ((denLength - 1) / 2)) + l, y - h, TileID.IceBlock);
                    }
                    Main.tile[x - ((denLength - 1) / 2) + l, y - h].LiquidAmount = 0;
                }
                int ceilingHeight = (int)((float)Math.Sin(((float)l / (float)denLength) * (float)Math.PI) * (float)denUpperHeight);
                for (int h = 0; h < ceilingHeight; h++)
                {
                    WorldGen.KillTile(x - ((denLength - 1) / 2) + l, y - h, false, false, true);
                    WorldGen.KillWall(x - ((denLength - 1) / 2) + l, y - h, false);

                    WorldGen.PlaceWall(x - ((denLength - 1) / 2) + l, y - h, WallID.IceUnsafe);
                    if (l % 4 == 0 && h == ceilingHeight - 5)
                    {
                        WorldGen.PlaceTile(x - ((denLength - 1) / 2) + l, y - h, TileID.Torches, false, false, -1, 9);
                    }
                    if (Math.Abs((x - ((denLength - 1) / 2) + l) - x) >= 10 && Math.Abs((x - ((denLength - 1) / 2) + l) - x) <= 19 && h == 15)
                    {
                        WorldGen.PlaceTile(x - ((denLength - 1) / 2) + l, y - h, TileID.Platforms, style: 35);
                    }
                }
                for (int h = 0; h < denLowerHeight; h++)
                {
                    WorldGen.PlaceTile(x - ((denLength - 1) / 2) + l, y + h, TileID.IceBlock);
                }
            }
            for (int l = 0; l < denLength; l++)
            {
                int ceilingHeight = (int)((float)Math.Sin(((float)l / (float)denLength) * (float)Math.PI) * (float)denUpperHeight);
                for (int h = 0; h < ceilingHeight; h++)
                {
                    if (l == 35 && h == 16)
                    {
                        Chest chest = Main.chest[WorldGen.PlaceChest(x - ((denLength - 1) / 2) + l, y - h, style: 11)];
                        int slot = 0;

                        chest.item[slot].SetDefaults(ItemID.SnowballCannon, false);
                        slot++;
                        chest.item[slot].SetDefaults(ItemID.Snowball, false);
                        chest.item[slot].stack = Main.rand.Next(500, 1000);
                        slot++;

                        chest.item[slot].SetDefaults(ItemID.IceTorch, false);
                        chest.item[slot].stack = Main.rand.Next(20, 100);
                        slot++;
                        chest.item[slot].SetDefaults(ItemID.LesserHealingPotion, false);
                        chest.item[slot].stack = Main.rand.Next(4, 11);
                        slot++;
                        if (Main.rand.Next(5) == 0)
                        {
                            chest.item[slot].SetDefaults(ItemID.IceMirror, false);
                            slot++;
                        }
                        slot++;
                    }
                    if (l == 64 && h == 16)
                    {
                        Chest chest = Main.chest[WorldGen.PlaceChest(x - ((denLength - 1) / 2) + l, y - h, style: 11)];

                        int slot = 0;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                chest.item[slot].SetDefaults(ItemID.IceSkates, false);
                                slot++;
                                break;

                            case 1:
                                chest.item[slot].SetDefaults(ItemID.FlurryBoots, false);
                                slot++;
                                break;

                            case 2:
                                chest.item[slot].SetDefaults(ItemID.BlizzardinaBottle, false);
                                slot++;
                                break;
                        }
                        chest.item[slot].SetDefaults(ItemID.IceTorch, false);
                        chest.item[slot].stack = Main.rand.Next(20, 100);
                        slot++;
                        chest.item[slot].SetDefaults(ItemID.LesserHealingPotion, false);
                        chest.item[slot].stack = Main.rand.Next(4, 11);
                        slot++;
                        if (Main.rand.Next(5) == 0)
                        {
                            chest.item[slot].SetDefaults(ItemID.IceMirror, false);
                            slot++;
                        }
                    }
                }
            }

            for (int l = ((denLength - 1) / 2) - (entryWidth - 1) / 2; l < ((denLength - 1) / 2) + (entryWidth + 1) / 2; l++)
            {
                for (int h = 0; h < 10; h++)
                {
                    WorldGen.KillTile(x - ((denLength - 1) / 2) + l, y - denUpperHeight + h, false, false, true);
                }
            }

            int m = ((denLength - 1) / 2) - (entryWidth - 1) / 2;
            GenerateTunnel(x - ((denLength - 1) / 2) + m - enntryWallWidth, y - denUpperHeight);
        }
        public static void GenerateTunnel(int x, int y)
        {
            int layer = 0;
            int xOffset = 0;
            int xDir = WorldGen.genRand.Next(2) == 0 ? -1 : 1;
            for (; true; layer++)
            {
                bool doubleBreak = false;


                for (int i = 0; i < enntryWallWidth * 2 + entryWidth; i++)
                {
                    //kill some trees!
                    for (int j = 0; j < 20; j++)
                    {
                        if (y - layer - j < Main.tile.Height && Main.tile[x + i + xOffset, y - layer - j].HasTile && Main.tile[x + i + xOffset, y - layer - j].TileType == TileID.Trees)
                        {
                            WorldGen.KillTile(x + i + xOffset, y - layer - j, false, false, true);
                        }
                    }

                    for (int j = 0; j < 20; j++)
                    {
                        if (y - layer - j < Main.tile.Height && Main.tile[x + i + xOffset, y - layer - j].HasTile)
                        {
                            doubleBreak = true;
                            break;
                        }
                    }
                    if (doubleBreak)
                    {
                        break;
                    }

                }
                if (!doubleBreak)
                {
                    int iglooX = x + xOffset;
                    int iglooY = y - layer - 9;
                    RoomBuilder.BuildRoom(iglooX, iglooY, IglooTileTypes, iglooBlueprints);
                    WorldGen.PlaceTile(iglooX + 16, iglooY + 2, TileID.Chandeliers, style: 11);
                    WorldGen.PlaceTile(iglooX + 20, iglooY + 8, TileID.Tables, style: 24);
                    //WorldGen.PlaceTile(iglooX + 18, iglooY + 8, TileID.Chairs, style: 1);
                    WorldGen.PlaceTile(iglooX + 22, iglooY + 8, TileID.Chairs, style: 28);
                    return;
                }
                else
                {
                    for (int k = 0; k < enntryWallWidth * 2 + entryWidth; k++)
                    {
                        if (k < enntryWallWidth || k >= enntryWallWidth + entryWidth)
                        {
                            WorldGen.PlaceTile(x + k + xOffset, y - layer, TileID.SnowBlock);
                        }
                        else
                        {
                            WorldGen.KillTile(x + k + xOffset, y - layer, false, false, true);
                        }
                        if (k >= enntryWallWidth - 1 || k < enntryWallWidth + entryWidth + 1)
                        {
                            WorldGen.PlaceWall(x + k + xOffset, y - layer, WallID.SnowWallUnsafe);
                        }
                        WorldGen.SlopeTile(x + enntryWallWidth - 1 + xOffset, y - layer, xDir == 1 ? 3 : 1);
                        WorldGen.SlopeTile(x + enntryWallWidth + entryWidth + xOffset, y - layer, xDir == 1 ? 2 : 5);

                    }
                }

                xOffset += xDir;
                if (WorldGen.genRand.Next(25) == 0)
                {
                    xDir *= -1;
                }
                if (xOffset < -50)
                {
                    xDir = 1;
                }
                if (xOffset > 50)
                {
                    xDir = -1;
                }
            }
        }


        public static void RunDenGenerator()
        {
            int leftOfSnow = 0;
            int rightOfSnow = Main.maxTilesX - 1;
            bool doubleBreak = false;
            for (; leftOfSnow < Main.maxTilesX; leftOfSnow++)
            {
                for (int y = 0; y < Main.worldSurface; y++)
                {
                    if (Main.tile[leftOfSnow, y].TileType == TileID.SnowBlock || Main.tile[leftOfSnow, y].TileType == TileID.IceBlock)
                    {
                        doubleBreak = true;
                        break;
                    }
                }
                if (doubleBreak)
                {
                    break;
                }
            }
            doubleBreak = false;
            for (; rightOfSnow > 0; rightOfSnow--)
            {
                for (int y = 0; y < Main.worldSurface; y++)
                {
                    if (Main.tile[rightOfSnow, y].TileType == TileID.SnowBlock || Main.tile[rightOfSnow, y].TileType == TileID.IceBlock)
                    {
                        doubleBreak = true;
                        break;
                    }
                }
                if (doubleBreak)
                {
                    break;
                }
            }

            int snowWidth = rightOfSnow - leftOfSnow;
            float portion = WorldGen.genRand.NextFloat() * .5f + .25f;
            int denX = (int)(leftOfSnow + snowWidth * portion);
            int denY = (int)(Main.worldSurface + 80 + WorldGen.genRand.Next(50));
            for (int i = 0; i < 100; i++)
            {
                if (!SafeDenLocation(denX, denY))
                {
                    portion = WorldGen.genRand.NextFloat() * .5f + .25f;
                    denX = (int)(leftOfSnow + snowWidth * portion);
                    denY = (int)(Main.worldSurface + 80 + WorldGen.genRand.Next(50));
                }
                else
                {
                    break;
                }
            }
            if (SafeDenLocation(denX, denY))
            {
                BearSpawn = new Vector2(denX * 16, (denY - 2) * 16);
                activeSleeper = true;
                if (Main.netMode == NetmodeID.Server && !WorldGen.gen)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                GenerateDen(denX, denY);
            }

        }
        static bool SafeDenLocation(int x, int y)
        {
            for(int i = x -60; i < 60; i++)
            {
                for(int j = (int)Main.worldSurface; j < y+10; j++)
                {
                    if(Main.tile[i,j].TileType == TileID.Containers || Main.tile[i, j].TileType == TileID.Containers2 || Main.tile[i, j].TileType == TileID.FakeContainers || Main.tile[i, j].TileType == TileID.FakeContainers2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            tasks.Add(new PassLegacy("Carving out a beast's den!", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                RunDenGenerator();
            }));
        }
        public override void PreUpdateWorld()
        {
            //QwertyMethods.ServerClientCheck("Time: " + Main.time + ", SpawnAt: " + BearSpawn);
            if (!NPC.AnyNPCs(NPCType<PolarBear>()) && !NPC.AnyNPCs(NPCType<Sleeping>()) && Main.time < 100 && BearSpawn.X != -1 && BearSpawn.Y != -1)
            {
                activeSleeper = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                if (Main.netMode == 0)
                {
                    int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)BearSpawn.X, (int)BearSpawn.Y, NPCType<Sleeping>());
                }
                else
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)ModMessageType.SpawnBear);
                    packet.WriteVector2(new Vector2((int)BearSpawn.X, (int)BearSpawn.Y));
                    packet.Write(Main.myPlayer);
                    packet.Send();
                }
                
            }
            else if (activeSleeper && !NPC.AnyNPCs(NPCType<PolarBear>()) && !NPC.AnyNPCs(NPCType<Sleeping>()) && BearSpawn.X != -1 && BearSpawn.Y != -1)
            {
                activeSleeper = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                if (Main.netMode == 0)
                {
                    int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)BearSpawn.X, (int)BearSpawn.Y, NPCType<Sleeping>());
                }
                else
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)ModMessageType.SpawnBear);
                    packet.WriteVector2(new Vector2((int)BearSpawn.X, (int)BearSpawn.Y));
                    packet.Write(Main.myPlayer);
                    packet.Send();
                }
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WritePackedVector2(BearSpawn);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BearSpawn = reader.ReadPackedVector2();
        }
    }

    internal class CreateDen : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "createDen"; }
        }

        public override string Description
        {
            get { return "Create's a den in the underground tundra and places the polar exterminator in it"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            FrozenDen.RunDenGenerator();
        }
    }
}
