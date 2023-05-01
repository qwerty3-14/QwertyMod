using QwertyMod.Content.Items.Weapon.Melee.Top.Cyclone;
using QwertyMod.Content.Items.Weapon.Ranged.DartLauncher.Whirpool;
using QwertyMod.Content.Items.Weapon.Sentry.BubbleBrewer;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace QwertyMod.Common
{
    public class DukeDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.DukeFishron)
            {
                //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
                LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

                //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
                notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<BubbleBrewerBaton>(), ItemType<Cyclone>(), ItemType<Whirlpool>()));
                //Finally add the leading rule
                npcLoot.Add(notExpertRule);
            }
        }
    }
    public class DukeBag : GlobalItem
    {
        
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if(item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<BubbleBrewerBaton>(), ItemType<Cyclone>(), ItemType<Whirlpool>()));
            }
        }
        
    }
}
