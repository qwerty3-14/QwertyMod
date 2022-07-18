using QwertyMod.Content.Items.MiscMaterials;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Equipment.Armor.Invader
{
    [AutoloadEquip(EquipType.Body)]
    public class InvaderProtector : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Protector");
            Tooltip.SetDefault("12% increased damage/n20% chance not to consume ammo/n+1 max minions");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 5;

            Item.width = 30;
            Item.height = 20;
            Item.defense = 27;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.ammoCost80 = true;
            player.maxMinions++;
        }
    }

}