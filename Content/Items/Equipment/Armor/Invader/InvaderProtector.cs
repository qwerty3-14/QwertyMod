using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Armor.Invader
{
    [AutoloadEquip(EquipType.Body)]
    public class InvaderProtector : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = GearStats.InvaderGearValue;
            Item.rare = ItemRarityID.Yellow;

            Item.width = 18;
            Item.height = 14;
            Item.defense = 27;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderProtector_Glow").Value;
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.ammoCost80 = true;
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

}