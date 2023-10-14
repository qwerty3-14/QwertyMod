using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Ores;
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


namespace QwertyMod.Common
{
    class OreGeneration : ModSystem
    {

        public bool hasGeneratedRhuthinium;
        public override void OnWorldLoad()
        {
            hasGeneratedRhuthinium = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["genned"] = hasGeneratedRhuthinium;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            hasGeneratedRhuthinium = tag.GetBool("genned");
        }
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = hasGeneratedRhuthinium;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            hasGeneratedRhuthinium = flags[0];
        }
        public override void PreUpdateWorld()
        {
            if (!hasGeneratedRhuthinium && NPC.downedBoss3)
            {
                for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-06); i++)
                {
                    WorldGen.OreRunner(
                        WorldGen.genRand.Next(0, Main.maxTilesX), // X Coord of the tile
                        WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200), // Y Coord of the tile
                        (double)WorldGen.genRand.Next(18, 28), // Strength (High = more)
                        WorldGen.genRand.Next(5, 6), // Steps
                        (ushort)ModContent.TileType<RhuthiniumOreT>() // The tile type that will be spawned
                       );
                }
                string key = Language.GetTextValue(Mod.GetLocalizationKey("RhuthiniumGeneration"));
                Color messageColor = Color.Cyan;
                if (Main.netMode == NetmodeID.Server) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                hasGeneratedRhuthinium = true;
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Add(new PassLegacy(Language.GetTextValue(Mod.GetLocalizationKey("WorldgenLuneOre")), delegate (GenerationProgress progress, GameConfiguration configuration)
           {
               PlaceOreInIslands();
           }));
        }
        public static void PlaceOreInIslands()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY / 4; j++)
                {
                    if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == TileID.Dirt && Main.tile[i, j + 1].TileType == TileID.Cloud)
                    {
                        int amt = WorldGen.genRand.Next(2, 5);
                        for (int k = 0; k < amt; k++)
                        {
                            WorldGen.PlaceTile(i, j + 1 + k, ModContent.TileType<LuneOreT>(), true, true);
                        }
                    }
                }
            }
        }
    }
    internal class LuneOre : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "luneOre"; }
        }
        public override string Description
        {
            get { return Language.GetTextValue(Mod.GetLocalizationKey("CommandDescriptionLuneGen")); }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            OreGeneration.PlaceOreInIslands();
        }
    }
    
    internal class RhuthiniumGeneration : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "createRhuthinium"; }
        }
        public override string Description
        {
            get { return Language.GetTextValue(Mod.GetLocalizationKey("CommandDescriptionRhuthiniumGen")); }
        }
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-06); i++)
            {
                WorldGen.OreRunner(
                    WorldGen.genRand.Next(0, Main.maxTilesX), // X Coord of the tile
                    WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200), // Y Coord of the tile
                    (double)WorldGen.genRand.Next(18, 28), // Strength (High = more)
                    WorldGen.genRand.Next(5, 6), // Steps
                    (ushort)ModContent.TileType<RhuthiniumOreT>() // The tile type that will be spawned
                    );
            }
            string key = Language.GetTextValue(Mod.GetLocalizationKey("RhuthiniumGeneration"));
            Color messageColor = Color.Cyan;
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
    internal class RhuthiniumScan : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "scanRhuthinium"; }
        }
        public override string Description
        {
            get { return Language.GetTextValue(Mod.GetLocalizationKey("CommandDescriptionRhuthiniumScan")); }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            int count = 0;
            for(int i = 0; i < Main.tile.Width; i++)
            {
                for(int j = 0; j < Main.tile.Height; j++)
                {
                    if(Main.tile[i, j].HasTile && Main.tile[i, j].TileType == ModContent.TileType<RhuthiniumOreT>())
                    {
                        count++;
                    }
                }
            }
            if(count <= 0)
            {
                if(NPC.downedBoss3)
                {
                    Main.NewText(Language.GetTextValue(Mod.GetLocalizationKey("RhuthiniumScanFailure")));
                }
                else
                {
                    Main.NewText(Language.GetTextValue(Mod.GetLocalizationKey("RhuthiniumScanNoSkeletron")));
                }
                return;
            }
            Main.NewText(count + Language.GetTextValue(Mod.GetLocalizationKey("RhuthiniumScanSuccess")));
        }
    }
}
