using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class FortressHarpyBeak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Harpy Beak");
            Tooltip.SetDefault("Lightweight and sturdy, goes well with Caelite when making weapons");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.maxStack = 999;
            Item.rare = 4;
            Item.value = 2500;
        }
    }
}