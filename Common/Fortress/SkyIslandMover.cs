using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace QwertyMod.Common.Fortress
{
    public class SkyIslandMover : ModSystem
    {
        private static int numIslandHouses = 0;
        private static int[] floatingIslandStyle = new int[30];
        private static int houseCount = 0;
        private static bool[] skyLake = new bool[30];
        private static int[] fihX = new int[30];

        private static int[] fihY = new int[30];
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            #region
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Islands"));
            if (ShiniesIndex != -1)
            {
                tasks.RemoveAt(ShiniesIndex);
            }
            ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Island Houses"));
            if (ShiniesIndex != -1)
            {
                tasks.RemoveAt(ShiniesIndex);
            }
            int skyLakes = 1;
            if (Main.maxTilesX > 8000)
            {
                int skyLakes2 = skyLakes;
                skyLakes = skyLakes2 + 1;
            }
            if (Main.maxTilesX > 6000)
            {
                int skyLakes2 = skyLakes;
                skyLakes = skyLakes2 + 1;
            }
            ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Giving Sky Fortress space", delegate (GenerationProgress progress, GameConfiguration configuration)
                {
					numIslandHouses = 0;
					houseCount = 0;
					progress.Message = Lang.gen[12].Value;
					int num814 = (int)((double)Main.maxTilesX * 0.0008);
					int num815 = 0;
					float num816 = num814 + skyLakes;
					for (int num817 = 0; (float)num817 < num816; num817++)
					{
						progress.Set((float)num817 / num816);
						int num818 = Main.maxTilesX;
						while (--num818 > 0)
						{
							bool flag54 = true;
							//Modify where islands can spawn to give the sky fortress space
							double maxLeft = 0.1;
							double maxRight = 0.9;
							if (WorldGen.dungeonX < Main.maxTilesX * .5f)
                            {
								maxRight = 0.7;
                            }
							else
                            {
								maxLeft = 0.3;
                            }

							int num819 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * maxLeft), (int)((double)Main.maxTilesX * maxRight));
							while (num819 > Main.maxTilesX / 2 - 150 && num819 < Main.maxTilesX / 2 + 150)
							{
								num819 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * maxLeft), (int)((double)Main.maxTilesX * maxRight));
							}
							for (int num820 = 0; num820 < numIslandHouses; num820++)
							{
								if (num819 > fihX[num820] - 180 && num819 < fihX[num820] + 180)
								{
									flag54 = false;
									break;
								}
							}
							if (flag54)
							{
								flag54 = false;
								int num821 = 0;
								for (int num822 = 200; (double)num822 < Main.worldSurface; num822++)
								{
									if (Main.tile[num819, num822].HasTile)
									{
										num821 = num822;
										flag54 = true;
										break;
									}
								}
								if (flag54)
								{
									int num823 = 0;
									num818 = -1;
									int val = WorldGen.genRand.Next(90, num821 - 100);
									val = Math.Min(val, (int)WorldGen.worldSurfaceLow - 50);
									if (num815 >= num814)
									{
										skyLake[numIslandHouses] = true;
										WorldGen.CloudLake(num819, val);
									}
									else
									{
										skyLake[numIslandHouses] = false;
										if (WorldGen.drunkWorldGen)
										{
											if (WorldGen.genRand.Next(2) == 0)
											{
												num823 = 3;
												WorldGen.SnowCloudIsland(num819, val);
											}
											else
											{
												num823 = 1;
												WorldGen.DesertCloudIsland(num819, val);
											}
										}
										else
										{
											if (WorldGen.getGoodWorldGen)
											{
												num823 = ((!WorldGen.crimson) ? 4 : 5);
											}
											WorldGen.CloudIsland(num819, val);
										}
									}
									fihX[numIslandHouses] = num819;
									fihY[numIslandHouses] = val;
									floatingIslandStyle[numIslandHouses] = num823;
									numIslandHouses++;
									num815++;
								}
							}
						}
					}
				}));
                ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle Trees"));
                if (ShiniesIndex != -1)
                {
                    #region

                    tasks.Insert(ShiniesIndex + 1, new PassLegacy("Giving Sky Fortress space", delegate (GenerationProgress progress, GameConfiguration configuration)
                    {
                        for (int k = 0; k < numIslandHouses; k++)
                        {
                            if (!skyLake[k])
                            {
                                WorldGen.IslandHouse(fihX[k], fihY[k], floatingIslandStyle[k]);
                            }
                        }
                    }));

                    #endregion
                }
            }
            #endregion
        }

    }
}
