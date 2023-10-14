using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Tiles.Relics
{
	public class AncientMachineRelic : ModItem
	{
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<AncientMachineRelicT>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}
    public class AncientMachineRelicT : Relic
	{
		public override string RelicTextureName => "QwertyMod/Content/Items/Consumable/Tiles/Relics/AncientMachineRelicT";
	}
}