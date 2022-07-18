using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Ores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

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
                        WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 200), // Y Coord of the tile
                        (double)WorldGen.genRand.Next(18, 28), // Strength (High = more)
                        WorldGen.genRand.Next(5, 6), // Steps
                        (ushort)TileType<RhuthiniumOreT>() // The tile type that will be spawned
                       );
                }
                string key = "Rhuthimis has blessed your world with Rhuthinium!";
                Color messageColor = Color.Cyan;
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                hasGeneratedRhuthinium = true;
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            tasks.Add( new PassLegacy("Placing ore in space!", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                PlaceOreInIslands();
            }));
        }
        public static void PlaceOreInIslands()
        {
            for(int i =0; i < Main.maxTilesX; i++)
            {
                for(int j =0; j < Main.maxTilesY / 4; j++) 
                {
                    if(Main.tile[i, j].HasTile && Main.tile[i, j].TileType == TileID.Dirt && Main.tile[i, j+1].TileType == TileID.Cloud)
                    {
                        int amt = WorldGen.genRand.Next(2, 5);
                        for(int k =0; k < amt; k++)
                        {
                            WorldGen.PlaceTile(i, j + 1 + k, TileType<LuneOreT>(), true, true);
                        }
                    }
                }
            }
        }
        public static void PlaceMoons()
        {
            int amount = 7;
            if (Main.maxTilesX > 6000)
            {
                amount += 4;
            }
            if (Main.maxTilesX > 8000)
            {
                amount += 2;
            }
            double maxLeft = 0.1;
            double maxRight = 0.9;
            if (WorldGen.dungeonX < Main.maxTilesX * .5f)
            {
                maxLeft = 0.3;
            }
            else
            {
                maxRight = 0.7;
            }
            int count = 0;
            for (int i = 0; i < 100 && count < amount; i++)
            {
                int x = WorldGen.genRand.Next((int)(maxLeft * Main.maxTilesX), (int)(maxRight * Main.maxTilesX));
                int y = WorldGen.genRand.Next(40, 90);
                if (PlaceMoon(x, y))
                {
                    count++;
                }
            }
        }
        static bool PlaceMoon(int x, int y)
        {
            int size = WorldGen.genRand.Next(30, 51);
            for(int i=0; i < size; i++)
            {
                for(int j =0; j < size; j++)
                {
                    if(Main.tile[x+i, y+j].HasTile)
                    {
                        return false;
                    }
                }
            }
            float offset = WorldGen.genRand.Next(10, 21) * (WorldGen.genRand.NextBool() ? 1 : -1);
            //Main.NewText("Offset: " + offset);
            float radius = size / 2f;
            //Main.NewText("Radius: " + radius);
            Vector2 center = new Vector2(x + radius, y + radius);
            Vector2 offsetPos = center + (offset * Vector2.UnitX);
            //Main.NewText("Center: " + center);
            //Main.NewText("Offset Position: " + offsetPos);
            float offsetRadius = radius - WorldGen.genRand.NextFloat(8);
            //Main.NewText("Offset Radius: " + offsetRadius);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Vector2 pos = new Vector2(x + i, y + j);
                    if ((center - pos).Length() < radius && (offsetPos - pos).Length() > offsetRadius)
                    {
                        WorldGen.PlaceTile(i + x, j + y, TileType<LuneOreT>());
                    }
                }
            }
            return true;
        }
    }
    internal class Moons : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "moons"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            OreGeneration.PlaceMoons();
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

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            OreGeneration.PlaceOreInIslands();
        }
    }
}
