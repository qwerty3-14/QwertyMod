using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Legs)]
    public class RhuthiniumGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Greaves");
            Tooltip.SetDefault("Lets you dash");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 3;

            Item.width = 22;
            Item.height = 18;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Dash>().SetDash(4);
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.RhuthiniumLegMale;
            if (!male) equipSlot = QwertyMod.RhuthiniumLegFemale;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<RhuthiniumBar>(), 14)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}