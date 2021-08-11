using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class HydraScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Scale");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = 100;
            Item.rare = 3;
            Item.value = 500;
            Item.rare = 5;
        }
    }
}