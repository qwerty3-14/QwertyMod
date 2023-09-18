using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Tool.Mining.Ancient
{
    public class AncientMiner : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Miner");
            //Tooltip.SetDefault("Mines a 3x3 area");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 29;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 40;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = 25000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.width = 66;
            Item.height = 66;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.pick = 95;
            Item.tileBoost = 1;
            Item.GetGlobalItem<AoePick>().miningRadius = 1;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Tool/Mining/Ancient/AncientMiner_Glow").Value;
            }
        }


    }
}