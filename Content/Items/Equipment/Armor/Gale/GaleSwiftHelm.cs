using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Gale
{
    [AutoloadEquip(EquipType.Head)]
    public class GaleSwiftHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 1;
            //Item.vanity = true;
            Item.width = 20;
            Item.height = 20;
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