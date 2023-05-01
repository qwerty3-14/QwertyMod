using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using QwertyMod.Content.NPCs.Bosses.InvaderBattleship;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Common.Fortress
{
    public class FortressBiome : ModBiome
    {
        public bool TheFortress = false;
        // Select Music
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/HeavenlyFortress");
        public override bool IsBiomeActive(Player player)
        {
            return (SkyFortress.fortressBrick > 100) && (((Main.maxTilesX < 6000) && (player.Center.Y / 16) < 160) || ((Main.maxTilesX < 8000 && Main.maxTilesX > 6000) && (player.Center.Y / 16) < 250) || ((Main.maxTilesX > 8000) && (player.Center.Y / 16) < 350));
        }
    }

    public class FortressSpawn : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            int defaultSpawnRate = 600;
            int defaultmaxSpawn = 5;
            if (player.InModBiome(GetInstance<FortressBiome>()))
            {

                if (NPC.AnyNPCs(NPCType<FortressBoss>()) || NPC.AnyNPCs(NPCType<InvaderBattleship>()))
                {
                    spawnRate = 0;
                    maxSpawns = 0;
                }
                else
                {
                    if (NPC.downedGolemBoss)
                    {
                        if (Main.dayTime)
                        {
                            spawnRate = (int)((spawnRate * 15f) / defaultSpawnRate);
                            maxSpawns = (int)((maxSpawns * 24f) / defaultmaxSpawn);
                        }
                        else
                        {
                            spawnRate = (int)((spawnRate * 17f) / defaultSpawnRate);
                            maxSpawns = (int)((maxSpawns * 20f) / defaultmaxSpawn);
                        }
                    }
                    else if (Main.hardMode)
                    {
                        if (Main.dayTime)
                        {
                            spawnRate = (int)((spawnRate * 30f) / defaultSpawnRate);
                            maxSpawns = (int)((maxSpawns * 12f) / defaultmaxSpawn);
                        }
                        else
                        {
                            spawnRate = (int)((spawnRate * 34f) / defaultSpawnRate);
                            maxSpawns = (int)((maxSpawns * 10f) / defaultmaxSpawn);
                        }
                    }
                    else
                    {
                        if (Main.dayTime)
                        {
                            spawnRate = (int)((spawnRate * 34f) / defaultSpawnRate);
                            maxSpawns = (int)((maxSpawns * 14f) / defaultmaxSpawn);
                        }
                        else
                        {
                            spawnRate = (int)((spawnRate * 38f) / defaultSpawnRate);
                            maxSpawns = (int)((maxSpawns * 12f) / defaultmaxSpawn);
                        }
                    }
                }
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(GetInstance<FortressBiome>()))
            {
                pool[0] = 0f;
            }
        }
    }

    public class SkyFortress : ModSystem
    {
        public static int fortressBrick;

        public override void ResetNearbyTileEffects()
        {
            fortressBrick = 0;
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            fortressBrick = tileCounts[TileType<FortressBrickT>()];
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Add(new PassLegacy("Fortifying the sky!", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                FortressBuilder.BuildFortress();
            }));
        }
    }
}