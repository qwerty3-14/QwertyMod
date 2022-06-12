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
            DisplayName.SetDefault("Chalice of the Light");
            Tooltip.SetDefault("Reusable" + "\nGives you 2 seconds of invinsibility after drinking");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;
            Item.healLife = 100;
            Item.useStyle = 2;
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.width = 14;
            Item.height = 24;
            Item.rare = 3;
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