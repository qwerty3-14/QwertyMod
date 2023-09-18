using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Vanity.CocktailDress
{
    [AutoloadEquip(EquipType.Body)]
    internal class CocktailDressTop : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Runic Robe");
            //Tooltip.SetDefault("remeber this?");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 12;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.value = Item.buyPrice(gold: 30);
        }
    }

}