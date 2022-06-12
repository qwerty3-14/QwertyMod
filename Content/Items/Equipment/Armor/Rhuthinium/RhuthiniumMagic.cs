using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    public class RhuthiniumMagic : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Magic");
            Description.SetDefault("10% increased magic damage");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Magic) += .1f;
        }
    }
}