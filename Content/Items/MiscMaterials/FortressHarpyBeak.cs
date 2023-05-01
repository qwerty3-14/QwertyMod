using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class FortressHarpyBeak : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 2500;
        }
    }
}