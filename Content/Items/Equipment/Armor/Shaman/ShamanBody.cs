using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Shaman
{
    [AutoloadEquip(EquipType.Body)]
    public class ShamanBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Shaman Warplate");
            //Tooltip.SetDefault("+1 max minions \n14% increased melee speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;

            Item.width = 22;
            Item.height = 12;
            Item.defense = 6;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.JungleSpores, 8)
                .AddIngredient(ItemID.Bone, 30)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.GetAttackSpeed(DamageClass.Melee) += .14f;
        }

    }
}