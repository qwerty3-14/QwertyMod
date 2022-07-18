using QwertyMod.Content.Items.MiscMaterials;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Invader
{
    [AutoloadEquip(EquipType.Legs)]
    public class InvaderLanders : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Landers");
            Tooltip.SetDefault("13% increased damage\n15% increased melee and movement speed\nGrants immunity to fall damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 5;
            Item.width = 22;
            Item.height = 18;
            Item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.13f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.moveSpeed += 0.15f;
            player.noFallDmg = true;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot =  QwertyMod.invaderLanderMale;
            if (!male) equipSlot = QwertyMod.invaderLanderFemale;
        }
    }

}