using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Lune
{
    public class MoonCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon Cooldown");
            Description.SetDefault("Can't shoot another moon yet!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
    }
}