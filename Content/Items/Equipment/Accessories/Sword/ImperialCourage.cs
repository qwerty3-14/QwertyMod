using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class ImperialCourage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 20;
        }
    }
}