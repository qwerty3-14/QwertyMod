using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace QwertyMod.Content.Items.Consumable.Tiles.Bars
{
    public class LuneBarT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            DustType = ModContent.DustType<LuneDust>();
            HitSound = SoundID.Tink;
            MinPick = 1;
            //AddMapEntry(new Color(102, 143, 204));
            //ItemDrop = ModContent.ItemType<LuneBar>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }
    }
}