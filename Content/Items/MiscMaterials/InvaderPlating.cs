
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class InvaderPlating : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Plating");
            Tooltip.SetDefault("Leftover invader parts");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] =25;
        }


        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.width = Item.height = 32;
            Item.maxStack = 999;
            Item.rare = 3;
        }
    }
}