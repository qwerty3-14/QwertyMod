using QwertyMod.Common;
using System.Collections.Generic;
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
            DisplayName.SetDefault("Glass Absorber");
            Tooltip.SetDefault("12% chance not to consume ammo\n12% reduced mana usage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 1;

            Item.width = 22;
            Item.height = 12;
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
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 45)
                .AddIngredient(ItemID.TungstenBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

}