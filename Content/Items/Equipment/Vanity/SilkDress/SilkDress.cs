using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using System;
using Terraria.DataStructures;

namespace QwertyMod.Content.Items.Equipment.Vanity.SilkDress
{
    [AutoloadEquip(EquipType.Body)]
    internal class SilkDress : ModItem
    {

        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 10);
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;
            equipSlot = QwertyMod.SilkSkirt;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 30)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
    public class RobesRegularSit : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if( drawPlayer.body == EquipLoader.GetEquipSlot(Mod, "SilkDress", EquipType.Body) )
            {
                drawInfo.cLegs = drawInfo.cBody;
                drawPlayer.wearsRobe = false;
            }
        }
    }
}