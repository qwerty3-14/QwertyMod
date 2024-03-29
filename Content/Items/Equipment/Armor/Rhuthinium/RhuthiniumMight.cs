using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    public class RhuthiniumMight : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) += .2f;
        }
    }
}