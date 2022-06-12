using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;

namespace QwertyMod.Content.Items.Equipment.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class DivineLightMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Light Mask");
            Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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