using QwertyMod.Content.Items.Equipment.Accessories.Expert;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Weapon.Magic.PenguinWhistle;
using QwertyMod.Content.Items.Weapon.Melee.Sword;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.GameContent.ItemDropRules;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class TundraBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 60;
            Item.height = 34;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }


        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ItemID.Penguin, 1, 40, 80));
            
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PolarMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PenguinGenerator>(), 1));
            itemLoot.Add(ItemDropRule.Coins(40000, true));
            itemLoot.Add(ItemDropRule.FewFromOptions(1, 1, ModContent.ItemType<PenguinClub>(), ModContent.ItemType<PenguinClub>(), ModContent.ItemType<PenguinWhistle>()));
        }
        
    }
}
