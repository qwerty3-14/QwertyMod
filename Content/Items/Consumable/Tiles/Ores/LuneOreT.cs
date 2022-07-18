using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Ores
{
    public class LuneOreT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileBlockLight[Type] = true;

            DustType = DustType<LuneDust>();
            HitSound = SoundID.Tink; 

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Lune Ore");
            AddMapEntry(new Color(102, 143, 204), name);

            ItemDrop = ItemType<LuneOre>();

            MinPick = 65;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }

        public override bool CanExplode(int i, int j)
        {
            if (!NPC.downedBoss3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}