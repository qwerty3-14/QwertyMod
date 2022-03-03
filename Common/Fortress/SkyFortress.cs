using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
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
            if (player.InModBiome(GetInstance<FortressBiome>()))
            {

                if (NPC.AnyNPCs(NPCType<FortressBoss>()))
                {
                    spawnRate = 0;
                    maxSpawns = 0;
                }
                else
                {

                    if (Main.hardMode)
                    {
                        if (Main.dayTime)
                        {
                            spawnRate = 30;
                            maxSpawns = 12;
                        }
                        else
                        {
                            spawnRate = 34;
                            maxSpawns = 10;
                        }
                    }
                    else
                    {
                        if (Main.dayTime)
                        {
                            spawnRate = 34;
                            maxSpawns = 10;
                        }
                        else
                        {
                            spawnRate = 38;
                            maxSpawns = 8;
                        }
                    }
                }
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.InModBiome(GetInstance<FortressBiome>()))
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
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            tasks.Add(new PassLegacy("Fortifying the sky!", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                FortressBuilder.BuildFortress();
            }));
        }
    }
}