using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    public class RhuthiniumMight : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Berzerk");
            Description.SetDefault("20% increased melee damage and max move speed");
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