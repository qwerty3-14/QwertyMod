using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion.InvaderCaster
{
    public class InvaderCasterB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Invader Caster");
            //Description.SetDefault("The Invader Caster will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<InvaderCasterMinion>()] > 0)
            {
                modPlayer.CasterMinion = true;
            }
            if (!modPlayer.CasterMinion)
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