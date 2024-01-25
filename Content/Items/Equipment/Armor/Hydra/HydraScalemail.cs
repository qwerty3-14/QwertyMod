using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Hydra
{
    [AutoloadEquip(EquipType.Body)]
    public class HydraScalemail : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = ItemRarityID.Pink;
            Item.width = 30;
            Item.height = 22;
            Item.defense = 18;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraScalemail_Glow").Value;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 1;
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<HydraScale>(), 24)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

}