using QwertyMod.Content.Items.Tool.Mining.TheDevourer;
using QwertyMod.Content.Items.Weapon.Magic.BlackHole;
using QwertyMod.Content.Items.Weapon.Magic.Plasma;
using QwertyMod.Content.Items.Weapon.Minion.UrQuan;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.B4Bow;
using QwertyMod.Content.NPCs.Bosses.OLORD;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Equipment.Accessories.Expert;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class BattleshipBag : ModItem
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
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("WeakPoint");
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Coins(1000000, true));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderPlating>(), 1, 100, 200));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderIDCard>(), 1, 1, 1));
            //itemLoot.Add(ItemDropRule.FewFromOptions(1, 1, ItemType<BlackHoleStaff>(), ItemType<ExplosivePierce>(), ItemType<DreadnoughtStaff>(), ItemType<B4Bow>()));
        }
    }
}