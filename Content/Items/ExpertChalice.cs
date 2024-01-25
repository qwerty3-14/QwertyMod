using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items
{
    public class ExpertChalice : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;
            Item.healLife = 100;
            Item.useStyle = ItemUseStyleID.DrinkLiquid; 
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.width = 38;
            Item.height = 34;
            Item.rare = ItemRarityID.Orange;
            Item.potion = true;
            Item.value = 50000;
            Item.expert = true;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {
            player.immune = true;
            player.immuneTime = 120;
            return true;
        }
    }
}