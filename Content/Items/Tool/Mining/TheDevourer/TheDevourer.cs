using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Tool.Mining.TheDevourer
{
    public class TheDevourer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("The Devourer");
            //Tooltip.SetDefault("Mines a 9x9 area!");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 52;
            Item.useAnimation = 52;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = 750000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.scale = 2;
            Item.width = 85;
            Item.height = 82;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.pick = 250;
            Item.tileBoost = 6;
            Item.GetGlobalItem<AoePick>().miningRadius = 4;
        }
    }
}