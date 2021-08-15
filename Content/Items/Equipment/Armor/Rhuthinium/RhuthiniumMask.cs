using QwertyMod.Content.Items.Consumable.Tile.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Head)]
    public class RhuthiniumMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Mask");
            Tooltip.SetDefault("+1 max minions \n5% increased critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 3;

            Item.width = 22;
            Item.height = 14;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 5;
            player.maxMinions += 1;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<RhuthiniumChestplate>() && legs.type == ItemType<RhuthiniumGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Attacks will call forth Rhuthimis's wraith!";
            player.GetModPlayer<RhuthiniumArmorEfffects>().summonSet = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<RhuthiniumBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}