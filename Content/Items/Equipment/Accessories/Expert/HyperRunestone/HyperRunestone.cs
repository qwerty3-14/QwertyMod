using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories.Expert.HyperRunestone
{
    public class HyperRunestone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyper Runestone");
            Tooltip.SetDefault("Makes other dashes more powerful or can be used to grant a dash on its own" + "\nMakes you invincible when dashing" + "\nThis effect needs four seconds to recharge");
        }

        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 76;
            Item.maxStack = 1;
            Item.value = 500000;
            Item.rare = 3;

            Item.rare = 9;
            Item.expert = true;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Dash>().SetDash(5);
            player.GetModPlayer<Dash>().Bonus += 5;
            player.GetModPlayer<Dash>().hyperRune = true;
        }
    }


}