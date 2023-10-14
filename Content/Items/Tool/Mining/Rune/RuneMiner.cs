using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.GameContent.Creative;


namespace QwertyMod.Content.Items.Tool.Mining.Rune
{
    public class RuneMiner : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 12;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = 500000;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.width = 66;
            Item.height = 66;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.pick = 200;
            Item.tileBoost = 4;
            Item.GetGlobalItem<AoePick>().miningRadius = 2;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Tool/Mining/Rune/RuneMiner_Glow").Value;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CraftingRune>(), 15)
                .AddIngredient(ModContent.ItemType<Ancient.AncientMiner>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
