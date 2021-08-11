using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class RuneGhostMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Ghost Mask");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = 1;

            Item.vanity = true;
            Item.width = 20;
            Item.height = 20;
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}