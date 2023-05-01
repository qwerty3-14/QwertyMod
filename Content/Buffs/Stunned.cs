using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    class Stunned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            ////DisplayName,SetDefault("Stunned");
            ////Description.SetDefault("If you can read this you're hacking!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }

}
