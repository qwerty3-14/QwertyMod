using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;

namespace QwertyMod.Content.Items.Equipment.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class RuneGhostMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Ghost Mask");
            Tooltip.SetDefault("");
            Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = 1;

            Item.vanity = true;
            Item.width = 20;
            Item.height = 20;
        }
    }
}