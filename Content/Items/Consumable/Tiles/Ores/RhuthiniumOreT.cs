using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Tiles.Ores
{
    public class RhuthiniumOreT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSpelunker[Type] = true;

            DustType = ModContent.DustType<RhuthiniumDust>();
            HitSound = SoundID.Tink;

            //ModTranslation name = CreateMapEntryName();
            //name.SetDefault("Rhuthinium Ore");
            AddMapEntry(new Color(39, 129, 129));

            //ItemDrop = ModContent.ItemType<RhuthiniumOre>();

            MinPick = 1;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }
    }
}