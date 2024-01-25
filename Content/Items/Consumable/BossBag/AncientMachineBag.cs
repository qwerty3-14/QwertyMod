using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.Equipment.Accessories.Expert.AncientGemstone;
using QwertyMod.Content.Items.Tool.Mining.Ancient;
using QwertyMod.Content.Items.Weapon.Magic.AncientMissile;
using QwertyMod.Content.Items.Weapon.Magic.AncientWave;
using QwertyMod.Content.Items.Weapon.Melee.Sword.AncientBlade;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.AncientThrow;
using QwertyMod.Content.Items.Weapon.Minion.AncientMinion;
using QwertyMod.Content.Items.Weapon.Morphs.AncientNuke;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.Ancient;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class AncientMachineBag : ModItem
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
            Item.width = 48;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Consumable/BossBag/AncientMachineBag_Glow").Value;
            }
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.FewFromOptionsNotScalingWithLuck(3, 1, ModContent.ItemType<AncientBlade>(), ModContent.ItemType<AncientSniper>(), ModContent.ItemType<AncientWave>(), ModContent.ItemType<AncientThrow>(), ModContent.ItemType<AncientMinionStaff>(), ModContent.ItemType<AncientMissileStaff>(), ModContent.ItemType<AncientLongbow>(), ModContent.ItemType<AncientNuke>()));

            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientMiner>(), 5));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientGemstone>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientMachineMask>(), 7, 1, 1));
            itemLoot.Add(ItemDropRule.Coins(80000, true));
        }
    }
}