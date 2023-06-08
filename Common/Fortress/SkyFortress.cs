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
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Net;
using Terraria.Localization;
using Terraria.ID;

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

                if (NPC.AnyNPCs(NPCType<FortressBoss>()) || NPC.AnyNPCs(NPCType<InvaderBattleship>()) || BattleshipSpawnIn.spawnTimer > -1)
                {
                    spawnRate = 0;
                    maxSpawns = 0;
                }
                else
                {
                    if (SkyFortress.beingInvaded)
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
        public static bool beingInvaded = false;
        public static bool initalInvasion = false;
        public override void OnWorldLoad()
        {
            beingInvaded = false;
            initalInvasion = false;
        }

        public override void OnWorldUnload()
        {
            beingInvaded = false;
            initalInvasion = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if(beingInvaded)
            {
                tag["beingInvaded"] = true;
            }
            if(initalInvasion)
            {
                tag["initalInvasion"] = true;
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            beingInvaded = tag.ContainsKey("beingInvaded");
            initalInvasion = tag.ContainsKey("initalInvasion");
        }

        public override void NetSend(BinaryWriter writer)
        {
            //Order of operations is important and has to match that of NetReceive
            var flags = new BitsByte();
            flags[0] = beingInvaded;
            flags[1] = initalInvasion;
        }

        public override void NetReceive(BinaryReader reader)
        {
            //Order of operations is important and has to match that of NetSend
            BitsByte flags = reader.ReadByte();
            beingInvaded = flags[0];
            initalInvasion = flags[1];
        }

        public override void ResetNearbyTileEffects()
        {
            fortressBrick = 0;
        }
        public override void PreUpdateWorld()
        {
            if(!initalInvasion && NPC.downedPlantBoss)
            {
                beingInvaded = true;
                initalInvasion = true;
                
                string key = "The sky fortress is being invaded!";
                Color messageColor = Color.Green;
                if (Main.netMode == NetmodeID.Server) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
            }
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