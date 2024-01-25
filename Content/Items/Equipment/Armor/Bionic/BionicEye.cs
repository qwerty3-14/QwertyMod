using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.Weapon.Morphs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Armor.Bionic
{
    [AutoloadEquip(EquipType.Head)]
    public class BionicEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 7;
            Item.width = 16;
            Item.height = 18;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicEye_Glow").Value;
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= 0.7f;
            player.nightVision = true;
            player.dangerSense = true;
            player.detectCreature = true;
            player.findTreasure = true;
            player.GetCritChance(DamageClass.Generic) += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SoulofMight, 8)
                .AddIngredient(ItemID.HallowedBar, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

    }
}
