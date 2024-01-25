using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class Overrdrive : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
    }
    public class OverrdriveCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
    }
}
