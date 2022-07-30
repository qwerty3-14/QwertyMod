using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Armor.Invader
{
    [AutoloadEquip(EquipType.Body)]
    public class InvaderProtector : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Protector");
            Tooltip.SetDefault("12% increased damage\n20% chance not to consume ammo\n+1 max minions");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = QwertyMod.InvaderGearValue;
            Item.rare = 8;

            Item.width = 30;
            Item.height = 20;
            Item.defense = 27;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.ammoCost80 = true;
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

}