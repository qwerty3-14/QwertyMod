using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertyMod.Common.Fortress
{
    internal class BuildFortress : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "buildFortress"; }
        }
        public override string Description
        {
            get { return Language.GetTextValue(Mod.GetLocalizationKey("CommandDescriptionFortressGen")); }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            FortressBuilder.BuildFortress();
        }
    }

    internal class FortressBuilder
    {
        public static void BuildFortress()
        {
            int lowerLimit;
            int upperLimit = 60;
            int CenterX;
            int maxDistanceFromCenter;
            int mediumRoomCount = Main.rand.Next(8, 11);

            if (Main.dungeonX < Main.maxTilesX * .5f)
            {
                CenterX = (int)((double)Main.maxTilesX * 0.8);
            }
            else
            {
                CenterX = (int)((double)Main.maxTilesX * 0.2);
            }
            if (Main.maxTilesX > 8000)
            {
                lowerLimit = 320;
                maxDistanceFromCenter = 750;
                mediumRoomCount *= 6;
            }
            else if (Main.maxTilesX > 6000)
            {
                lowerLimit = 230;
                maxDistanceFromCenter = 550;
                mediumRoomCount *= 3;
            }
            else
            {
                lowerLimit = 130;
                maxDistanceFromCenter = 320;
            }
            int height = lowerLimit - upperLimit;
            Vector2 topLeft = new Vector2(CenterX - maxDistanceFromCenter, upperLimit);
            QwertyMethods.BreakTiles(CenterX - maxDistanceFromCenter, 0, maxDistanceFromCenter * 2, (int)(lowerLimit));
            bool[,] region = new bool[maxDistanceFromCenter * 2, height];
            /*
            for(int i = 0; i < region.GetLength(0); i++)
            {
                for(int j = 0; i < region.GetLength(1); j++)
                {
                    region[i, j] = false;
                }
            }*/
            Vector2 offset = new Vector2(maxDistanceFromCenter - Blueprints.AltarRooms.GetLength(3) / 2, height / 2 - Blueprints.AltarRooms.GetLength(2) / 2);
            RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.AltarRoomTileTypes, Blueprints.AltarRooms);
            OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.AltarRooms.GetLength(3), Blueprints.AltarRooms.GetLength(2), ref region);
            int l = 4;
            if (Main.maxTilesX < 6000)
            {
                //for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l / 2 % 2 == 0) || (b % 2 == 0 && l / 2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.LargeRooms.GetLength(3) / 2, l / 2 * height / l - Blueprints.LargeRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.LargeRoomTileTypes, Blueprints.LargeRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 8;
                // for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l / 2 % 2 == 0) || (b % 2 == 0 && l / 2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.MediumRooms.GetLength(3) / 2, l / 2 * height / l - Blueprints.MediumRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.MediumRoomTileTypes, Blueprints.MediumRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
                l = 16;

                //for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l / 2 % 2 == 0) || (b % 2 == 0 && l / 2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.SmallRooms.GetLength(3) / 2, l / 2 * height / l - Blueprints.SmallRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.SmallRoomTileTypes, Blueprints.SmallRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 32;
                //for (int l/2 = 1; l/2 < l; l/2++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && l / 2 % 2 == 0) || (b % 2 == 0 && l / 2 % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.TinyRooms.GetLength(3) / 2, l / 2 * height / l - Blueprints.TinyRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.TinyRoomTileTypes, Blueprints.TinyRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
            }
            else if (Main.maxTilesX < 8000)
            {
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.LargeRooms.GetLength(3) / 2, q * height / l - Blueprints.LargeRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.LargeRoomTileTypes, Blueprints.LargeRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 6;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.MediumRooms.GetLength(3) / 2, q * height / l - Blueprints.MediumRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.MediumRoomTileTypes, Blueprints.MediumRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
                l = 12;

                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.SmallRooms.GetLength(3) / 2, q * height / l - Blueprints.SmallRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.SmallRoomTileTypes, Blueprints.SmallRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 18;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.TinyRooms.GetLength(3) / 2, q * height / l - Blueprints.TinyRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.TinyRoomTileTypes, Blueprints.TinyRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.LargeRooms.GetLength(3) / 2, q * height / l - Blueprints.LargeRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.LargeRoomTileTypes, Blueprints.LargeRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 8;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.MediumRooms.GetLength(3) / 2, q * height / l - Blueprints.MediumRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.MediumRoomTileTypes, Blueprints.MediumRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
                l = 16;

                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.SmallRooms.GetLength(3) / 2, q * height / l - Blueprints.SmallRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.SmallRoomTileTypes, Blueprints.SmallRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }

                l = 32;
                for (int q = 1; q < l; q++)
                {
                    for (int b = 1; b < l; b++)
                    {
                        if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                        {
                            offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.TinyRooms.GetLength(3) / 2, q * height / l - Blueprints.TinyRooms.GetLength(2) / 2);
                            if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), region))
                            {
                                RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.TinyRoomTileTypes, Blueprints.TinyRooms);
                                OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), ref region);
                            }
                        }
                    }
                }
            }

            /*
            int l = 4;
            for(int q =1; q< l; q++)
            {
                for(int b =1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.LargeRooms.GetLength(3) / 2, q * height / l - Blueprints.LargeRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), region))
                        {
                            RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.LargeRoomTileTypes, Blueprints.LargeRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.LargeRooms.GetLength(3), Blueprints.LargeRooms.GetLength(2), ref region);
                        }
                    }
                }
            }

            l = 8;
            for (int q = 1; q < l; q++)
            {
                for (int b = 1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.MediumRooms.GetLength(3) / 2, q * height / l - Blueprints.MediumRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), region))
                        {
                            RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.MediumRoomTileTypes, Blueprints.MediumRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.MediumRooms.GetLength(3), Blueprints.MediumRooms.GetLength(2), ref region);
                        }
                    }
                }
            }
            l = 16;

            for (int q = 1; q < l; q++)
            {
                for (int b = 1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.SmallRooms.GetLength(3) / 2, q * height / l - Blueprints.SmallRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), region))
                        {
                            RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.SmallRoomTileTypes, Blueprints.SmallRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.SmallRooms.GetLength(3), Blueprints.SmallRooms.GetLength(2), ref region);
                        }
                    }
                }
            }
            l = 32;
            for (int q = 1; q < l; q++)
            {
                for (int b = 1; b < l; b++)
                {
                    if ((b % 2 != 0 && q % 2 == 0) || (b % 2 == 0 && q % 2 != 0))
                    {
                        offset = new Vector2(b * 2 * maxDistanceFromCenter / l - Blueprints.TinyRooms.GetLength(3) / 2, q * height / l - Blueprints.TinyRooms.GetLength(2) / 2);
                        if (CheckRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), region))
                        {
                            RoomBuilder.BuildRoom((int)(topLeft.X + offset.X), (int)(topLeft.Y + offset.Y), Blueprints.TinyRoomTileTypes, Blueprints.TinyRooms);
                            OccupyRegion((int)offset.X, (int)offset.Y, Blueprints.TinyRooms.GetLength(3), Blueprints.TinyRooms.GetLength(2), ref region);
                        }
                    }
                }
            }
            */
            /*
             for (int i = 0; i < mediumRoomCount; i++)
             {
                 AttemptRoomPlace(topLeft, Blueprints.MediumRoomTileTypes, Blueprints.MediumRooms, ref region);
                 for (int k = 0; k < smallRoomsPerMed; k++)
                 {
                     AttemptRoomPlace(topLeft, Blueprints.SmallRoomTileTypes, Blueprints.SmallRooms, ref region);
                 }
                 for (int k = 0; k < tinyRoomsPerMed; k++)
                 {
                     AttemptRoomPlace(topLeft, Blueprints.TinyRoomTileTypes, Blueprints.TinyRooms, ref region);
                 }
             }*/
        }

        public static void OccupyRegion(int x, int y, int width, int height, ref bool[,] region)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    region[i + x, j + y] = true;
                }
            }
        }

        public static bool CheckRegion(int x, int y, int width, int height, bool[,] region)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (region[i + x, j + y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void AttemptRoomPlace(Vector2 TopLeft, List<int[]>[] RoomTileTypes, int[,,,] Rooms, ref bool[,] region, int type = -1)
        {
            for (int i = 0; i < 100; i++)
            {
                int x = Main.rand.Next(region.GetLength(0) - Rooms.GetLength(3));
                int y = Main.rand.Next(region.GetLength(1) - Rooms.GetLength(2));
                if (CheckRegion(x, y, Rooms.GetLength(3), Rooms.GetLength(2), region))
                {
                    RoomBuilder.BuildRoom((int)(TopLeft.X + x), (int)(TopLeft.Y + y), RoomTileTypes, Rooms);
                    OccupyRegion(x, y, Rooms.GetLength(3), Rooms.GetLength(2), ref region);
                    return;
                }
            }
        }
    }
}