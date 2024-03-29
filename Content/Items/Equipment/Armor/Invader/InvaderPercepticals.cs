using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Armor.Invader
{
    [AutoloadEquip(EquipType.Head)]
    public class InvaderPercepticals : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.value = GearStats.InvaderGearValue;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 18;
            Item.height = 18;
            Item.defense = 8;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderPercepticals_Glow").Value;
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetCritChance(DamageClass.Generic) += 10;
            player.statManaMax2 += 80;
            player.manaCost -= 0.15f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}