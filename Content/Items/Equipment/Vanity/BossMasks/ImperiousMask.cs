using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class ImperiousMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.width = 20;
            Item.height = 24;
        }
    }
}
