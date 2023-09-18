using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Consumable.Tiles.Relics
{
	public class RuneGhostRelic : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName,SetDefault("Rune Ghost Relic");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<RuneGhostRelicT>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}
    public class RuneGhostRelicT : Relic
	{
		public override string RelicTextureName => "QwertyMod/Content/Items/Consumable/Tiles/Relics/RuneGhostRelicT";
	}
}