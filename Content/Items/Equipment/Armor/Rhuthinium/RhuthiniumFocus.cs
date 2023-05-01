using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    public class RhuthiniumFocus : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Rhuthinium Focus");
            //Description.SetDefault("10% increased ranged damage");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) += .1f;
        }
    }
}