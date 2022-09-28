using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Lune
{
    [AutoloadEquip(EquipType.Legs)]
    public class LuneLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Runners");
            Tooltip.SetDefault("10% increased movement speed" + "\n8% increased ranged crit");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 25000;
            Item.rare = 1;

            Item.width = 22;
            Item.height = 18;
            Item.defense = 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<LuneBar>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 8;
            player.moveSpeed += .1f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.LuneLegMale;
            if (!male) equipSlot = QwertyMod.LuneLegFemale;
        }
    }
}