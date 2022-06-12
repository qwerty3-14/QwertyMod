using QwertyMod.Content.Items.MiscMaterials;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Equipment.Armor.Hydra
{
    [AutoloadEquip(EquipType.Body)]
    public class HydraScalemail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Scalemail");
            Tooltip.SetDefault("+0.5 life/sec regen rate" + "\n+1 max minions");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 5;

            Item.width = 30;
            Item.height = 20;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 1;
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<HydraScale>(), 24)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

}