using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Hydra
{
    [AutoloadEquip(EquipType.Legs)]
    public class HydraLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Extensions");
            Tooltip.SetDefault("+0.5 life/sec regen rate" + "\n+10% summon damage and movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 5;
            Item.width = 22;
            Item.height = 18;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 1;
            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.moveSpeed += .1f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.hydraLegMale;
            if (!male) equipSlot = QwertyMod.hydraLegFemale;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<HydraScale>(), 18)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

}