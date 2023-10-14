using QwertyMod.Common;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Glass
{
    [AutoloadEquip(EquipType.Body)]
    public class GlassAbsorber : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 26;
            Item.height = 18;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CommonStats>().ammoReduction *= .88f;
            player.manaCost *= .88f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Glass, 45)
                .AddIngredient(ItemID.SilverBar, 8)
                .AddIngredient(ItemID.ShadowScale, 4)
                .AddTile(TileID.GlassKiln)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 45)
                .AddIngredient(ItemID.TungstenBar, 8)
                .AddIngredient(ItemID.ShadowScale, 4)
                .AddTile(TileID.GlassKiln)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 45)
                .AddIngredient(ItemID.SilverBar, 8)
                .AddIngredient(ItemID.TissueSample, 4)
                .AddTile(TileID.GlassKiln)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 45)
                .AddIngredient(ItemID.TungstenBar, 8)
                .AddIngredient(ItemID.TissueSample, 4)
                .AddTile(TileID.GlassKiln)
                .Register();
        }
    }

}