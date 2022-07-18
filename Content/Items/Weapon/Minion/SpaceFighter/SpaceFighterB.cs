using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Weapon.Minion.SpaceFighter
{
    class SpaceFighterB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Space fighter");
            Description.SetDefault("Breaking news! You're in SPACE!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<SpaceFighter>()] > 0)
            {
                modPlayer.SpaceFighter = true;
            }
            if (!modPlayer.SpaceFighter)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
