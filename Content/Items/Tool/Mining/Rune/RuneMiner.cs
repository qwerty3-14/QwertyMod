using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Tool.Mining.Rune
{
    public class RuneMiner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune Miner");
            Tooltip.SetDefault("Mines a 5x5 area");
        }


        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 12;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = 500000;
            Item.rare = 9;
            Item.UseSound = SoundID.Item1;
            //Item.prefix = 0;
            Item.width = 16;
            Item.height = 16;
            //Item.crit = 5;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.pick = 200;
            Item.tileBoost = 4;
            Item.GetGlobalItem<AoePick>().miningRadius = 2;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Tool/Mining/Rune/RuneMiner_Glow").Value;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 15)
                .AddIngredient(ItemType<Ancient.AncientMiner>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
