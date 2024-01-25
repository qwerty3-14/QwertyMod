using QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class GravityFlipped : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AntiGravity>().forcedAntiGravity = 10;
            player.mount.Dismount(player);
        }
    }
}