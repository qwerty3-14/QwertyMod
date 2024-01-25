using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    class Stunned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }

}
