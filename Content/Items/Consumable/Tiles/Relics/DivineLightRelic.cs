using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Consumable.Tiles.Relics
{
	public class DivineLightRelic : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Divine Light Relic");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<DivineLightRelicT>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}
    public class DivineLightRelicT : Relic
	{
		public override string RelicTextureName => "QwertyMod/Content/Items/Consumable/Tiles/Relics/DivineLightRelicT";
		public override void KillMultiTile(int i, int j, int frameX, int frameY) 
        {
			Item.NewItem(new EntitySource_Misc(""), i * 16, j * 16, 32, 32, ModContent.ItemType<DivineLightRelic>());
		}
	}
}