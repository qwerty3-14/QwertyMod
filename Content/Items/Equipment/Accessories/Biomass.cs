using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class Biomass : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Biomass");
            Tooltip.SetDefault("Stacks of ammo will slowly grow in size" + "\nGrowth rate is lower for higher value ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BiomassEffect>().effect += 1;
        }
    }

    public class BiomassEffect : ModPlayer
    {
        public float effect;

        public override void ResetEffects()
        {
            effect = 00;
        }

        public override void PreUpdate()
        {
            if (effect > 0)
            {
                Item item = new Item();
                List<int> possibleStacks = new List<int>();
                bool attemptGrowth = false;
                for (int i = 0; i < 58; i++)
                {
                    if (!Player.inventory[i].IsAir)
                    {
                        if ((Player.inventory[i].ammo != AmmoID.None) && Player.inventory[i].stack < Player.inventory[i].maxStack)
                        {
                            possibleStacks.Add(i);
                            attemptGrowth = true;
                        }
                    }
                }
                if (attemptGrowth)
                {
                    item = Player.inventory[possibleStacks[Main.rand.Next(possibleStacks.Count)]];
                    int valueStat = 5;
                    if (item.value > valueStat)
                    {
                        valueStat = item.value;
                        valueStat = (int)(valueStat / effect);
                    }
                    if (Main.rand.Next(valueStat * 4) == 0)
                    {
                        item.stack++;
                    }
                }
                attemptGrowth = false;
            }
        }
    }
    public class BiomassGrowth : ModSystem
    {
        public override void PostWorldGen()
        {
            for (int c = 0; c < Main.chest.Length; c++)
            {
                if (Main.chest[c] != null)
                {
                    if (Main.chest[c].item[0].type == ItemID.LivingWoodWand || Main.chest[c].item[0].type == ItemID.LeafWand)
                    {
                        for (int i = 0; i < Main.chest[c].item.Length; i++)
                        {
                            if (Main.chest[c].item[i].type == ItemID.LivingLoom)
                            {
                                break;
                            }
                            if (Main.chest[c].item[i].type == 0)
                            {
                                Main.chest[c].item[i].SetDefaults(ItemType<Biomass>(), false);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
