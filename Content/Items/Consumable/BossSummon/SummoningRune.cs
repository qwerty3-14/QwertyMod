using QwertyMod.Content.NPCs.Bosses.RuneGhost;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class SummoningRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Summoning Rune");
            Tooltip.SetDefault("Summons The Rune Ghost");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.maxStack = 20;
            Item.rare = 3;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, NPCType<RuneGhost>());
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            Item.stack--;
            return true;
        }
    }
    public class DropFromWisps : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if(npc.type == NPCID.DungeonSpirit)
            {
                npcLoot.Add(ItemDropRule.Common(ItemType<SummoningRune>(), 4));
            }
        }
    }
}