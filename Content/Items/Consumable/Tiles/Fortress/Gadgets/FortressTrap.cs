using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class FortressTrap : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }


        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
            Item.createTile = ModContent.TileType<FortressTrapT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
        
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if(player.itemAnimation == player.itemAnimationMax - 1 && Main.tile[Player.tileTargetX, Player.tileTargetY].TileType == ModContent.TileType<FortressTrapT>())
            {
                if (player.direction == 1)
                {
                    Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameX = 18;
                }
                if (Main.netMode == 1)
                {
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY);
                }
            }
        }
        public override void UseAnimation(Player player)
        {
            
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ModContent.ItemType<FortressBrick>(), 2)
                .AddIngredient(ModContent.ItemType<CaeliteBar>(), 1)
                .AddIngredient(ModContent.ItemType<CaeliteCore>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}