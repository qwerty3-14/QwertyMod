using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class Overrdrive : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overdrive");
            Description.SetDefault("RWARRR!");
            Main.debuff[Type] = false;
            //longerExpertDebuff = false;
        }
    }
    public class OverrdriveCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overdrive Cooldown");
            Description.SetDefault("For your safety and the safety of others we cannot allow you to use overdrive at this moment");
            Main.debuff[Type] = false;
        }
    }
}
