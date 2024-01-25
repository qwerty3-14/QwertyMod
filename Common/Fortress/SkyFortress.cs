using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using QwertyMod.Content.NPCs.Bosses.InvaderBattleship;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

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
            if (player.InModBiome(ModContent.GetInstance<FortressBiome>()))
            {

                if (NPC.AnyNPCs(ModContent.NPCType<FortressBoss>()) || NPC.AnyNPCs(ModContent.NPCType<InvaderBattleship>()) || BattleshipSpawnIn.spawnTimer > -1)
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
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<FortressBiome>()))
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
            writer.Write(flags);
            //QwertyMethods.ServerClientCheck("NetSend");
        }

        public override void NetReceive(BinaryReader reader)
        {
            //Order of operations is important and has to match that of NetSend
            BitsByte flags = reader.ReadByte();
            beingInvaded = flags[0];
            initalInvasion = flags[1];
            //QwertyMethods.ServerClientCheck("NetReceive");
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
                //QwertyMethods.ServerClientCheck("Invaded?: " + beingInvaded);
                string key = Language.GetTextValue(Mod.GetLocalizationKey("FortressInvasion"));
                Color messageColor = Color.Green;
                if (Main.netMode == NetmodeID.Server) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                NetMessage.SendData(MessageID.WorldData);
                if(Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)ModMessageType.SetFortressInvasionStatus);
                    packet.Write(beingInvaded);
                    packet.Write(initalInvasion);
                    packet.Send();
                }
            }
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            fortressBrick = tileCounts[ModContent.TileType<FortressBrickT>()];
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Add(new PassLegacy(Language.GetTextValue(Mod.GetLocalizationKey("WorldgenSkyFortress")), delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                FortressBuilder.BuildFortress();
            }));
        }
    }
}