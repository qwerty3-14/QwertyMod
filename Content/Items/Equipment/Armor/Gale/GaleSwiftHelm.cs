using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tile.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Gale
{
    [AutoloadEquip(EquipType.Head)]
    public class GaleSwiftHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gale Swift Helm");
            Tooltip.SetDefault("+6% chance to dodge an attack" + "\n+12% critical strike chance" + "\nGreatly increased damage after dodging");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = 4;
            Item.defense = 1;
            //Item.vanity = true;
            Item.width = 20;
            Item.height = 20;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CommonStats>().dodgeChance += 6;
            player.GetCritChance(DamageClass.Generic) += 12;
            player.GetModPlayer<CommonStats>().dodgeDamageBoost = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CaeliteBar>(), 6)
                .AddIngredient(ItemType<FortressHarpyBeak>(), 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}