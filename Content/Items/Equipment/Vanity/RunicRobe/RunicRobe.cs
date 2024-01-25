using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Vanity.RunicRobe
{
    [AutoloadEquip(EquipType.Body)]
    internal class RunicRobe : ModItem
    {

        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;
            if (male) equipSlot = QwertyMod.RuneLegMale;
            if (!male) equipSlot = QwertyMod.RuneLegFemale;
        }
    }

    public class DressLegs : EquipTexture
    {
    }

    public class DressLegsFemale : EquipTexture
    {
    }
}