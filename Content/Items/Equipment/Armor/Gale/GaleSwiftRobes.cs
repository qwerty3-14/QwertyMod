using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Armor.Gale
{
    [AutoloadEquip(EquipType.Legs)]
    public class GaleSwiftRobes : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.GaleLegMale;
            if (!male) equipSlot = QwertyMod.GaleLegFemale;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 1;
            Item.width = 22;
            Item.height = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CommonStats>().dodgeChance += 9;
            player.GetCritChance(DamageClass.Generic) += 8;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CaeliteBar>(), 8)
                .AddIngredient(ModContent.ItemType<FortressHarpyBeak>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}