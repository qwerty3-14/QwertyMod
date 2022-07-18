using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Invader
{
    [AutoloadEquip(EquipType.Head)]
    public class InvaderPercepticals : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Percepticals");
            Tooltip.SetDefault("10% increased damage and critical strike chance/nIncreases maximum mana by 80/nReduces mana useage by 15%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Head.Sets.DrawFullHair[Item.headSlot] = true;
        }


        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 5;

            Item.width = 28;
            Item.height = 22;
            Item.defense = 8;

        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetCritChance(DamageClass.Generic) += 10;
            player.statManaMax2 += 80;
            player.manaCost -= 0.85f;
        }


        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<InvaderProtector>() && legs.type == ItemType<InvaderLanders>();
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }

}