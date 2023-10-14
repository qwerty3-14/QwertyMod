using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Shaman
{
    public class SpiritCallCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
    }
}