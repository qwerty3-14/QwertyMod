using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System.Linq;
using QwertyMod.Content.Items.Consumable.BossSummon;

namespace QwertyMod.Common
{
    public class ChestLoot : ModSystem
    {
        public override void PostWorldGen()
        {
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null)
                {
                    if (WorldGen.genRand.NextBool(4) && 
                    (Main.tile[chest.x, chest.y].TileType == TileID.Containers && 
                    (Main.tile[chest.x, chest.y].TileFrameX == 8 * 36 || 
                    Main.tile[chest.x, chest.y].TileFrameX == 10 * 36)) || (Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == 10 * 36))
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<AncientEmblem>());
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}




