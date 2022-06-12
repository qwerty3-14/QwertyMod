using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Head)]
    public class RhuthiniumCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Cap");
            Tooltip.SetDefault("10% increased ranged critical chance\n10% chance not to consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 3;

            Item.width = 26;
            Item.height = 16;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetModPlayer<CommonStats>().ammoReduction *= .9f;
        }


        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<RhuthiniumChestplate>() && legs.type == ItemType<RhuthiniumGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<RhuthiniumArmorEfffects>().rangedSet = true;
            player.setBonus = "Avoiding damage long enough will increase ranged damage by 10%";
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<RhuthiniumBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}