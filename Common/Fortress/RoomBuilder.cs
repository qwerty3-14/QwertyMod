using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QwertyMod.Common.Fortress
{
    public static class RoomBuilder
    {
        public static void BreakTiles(int i, int j, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    WorldGen.KillTile(i + x, j + y, false, false, true);
                    WorldGen.KillWall(i + x, j + y, false);
                    Main.tile[i + x, j + y].LiquidAmount = 0;
                }
            }
        }
        public static void BuildRoom(int i, int j, List<int[]>[] RoomTileTypes, int[,,,] Rooms, int type = -1)
        {
            BreakTiles(i, j, Rooms.GetLength(3), Rooms.GetLength(2));
            if (type == -1)
            {
                type = Main.rand.Next(Rooms.GetLength(0));
            }
            for (int y = Rooms.GetLength(2) - 1; y >= 0; y--) // built from bottom to top
            {
                for (int x = 0; x < Rooms.GetLength(3); x++) //built from left to right
                {
                    for (int k = 0; k < RoomTileTypes[0][type].Length; k++) //tiles
                    {
                        if (Rooms[type, 0, y, x] == k && Rooms[type, 0, y, x] != 0)
                        {
                            WorldGen.PlaceTile(i + x, j + y, RoomTileTypes[0][type][k], false, false);
                        }
                    }

                    for (int k = 0; k < RoomTileTypes[1][type].Length; k++) //walls
                    {
                        if (Rooms[type, 1, y, x] == k && Rooms[type, 1, y, x] != 0)
                        {
                            WorldGen.PlaceWall(i + x, j + y, RoomTileTypes[1][type][k]);
                        }
                    }
                }
            }

            for (int y = Rooms.GetLength(2) - 1; y >= 0; y--) // built from bottom to top
            {
                for (int x = 0; x < Rooms.GetLength(3); x++) //built from left to right
                {
                    //redo the tile placement for any tiles that failed to place the first time (like hanging tiles)
                    for (int k = 0; k < RoomTileTypes[0][type].Length; k++) //tiles
                    {
                        if (Rooms[type, 0, y, x] == k && Rooms[type, 0, y, x] != 0 && !Main.tile[i + x, j + y].HasTile)
                        {
                            WorldGen.PlaceTile(i + x, j + y, RoomTileTypes[0][type][k], false, true);
                        }
                    }
                    Main.tile[i + x, j + y].TileFrameX = (short)Rooms[type, 5, y, x];
                    Main.tile[i + x, j + y].TileFrameY = (short)Rooms[type, 6, y, x];
                    WorldGen.SlopeTile(i + x, j + y, Rooms[type, 2, y, x] % 100);
                    if (Rooms[type, 2, y, x] >= 100)
                    {
                        //Main.tile[i + x, j + y].HalfBrick = true;
                        WorldGen.PoundTile(i + x, j + y);
                    }

                    if (Rooms[type, 3, y, x] % 10 == 1)
                    {
                        WorldGen.PlaceWire(i + x, j + y);
                    }
                    if (Rooms[type, 3, y, x] % 100 >= 10)
                    {
                        WorldGen.PlaceWire2(i + x, j + y);
                    }
                    if (Rooms[type, 3, y, x] % 1000 >= 100)
                    {
                        WorldGen.PlaceWire3(i + x, j + y);
                    }
                    if (Rooms[type, 3, y, x] % 10000 >= 1000)
                    {
                        WorldGen.PlaceWire4(i + x, j + y);
                    }
                    if (Rooms[type, 3, y, x] % 100000 >= 10000)
                    {
                        WorldGen.PlaceActuator(i + x, j + y);
                    }

                    Main.tile[i + x, j + y].LiquidAmount = (byte)Rooms[type, 4, y, x];
                }
            }
        }
    }
}
