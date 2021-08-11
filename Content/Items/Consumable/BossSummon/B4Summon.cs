using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.NPCs.Bosses.OLORD;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class B4Summon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("O.L.O.R.D. summon");
            Tooltip.SetDefault("Summons the Oversized Laser-emitting Obliteration Radiation-emitting Destroyer");
            Item.staff[Item.type] = true;
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 78;
            Item.height = 78;
            Item.maxStack = 20;
            Item.rare = 3;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 5;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(NPCType<OLORDv2>()))
            {
                NPC.SpawnOnPlayer(player.whoAmI, NPCType<OLORDv2>());
                SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
                Item.stack--;
                return true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 4)
                .AddIngredient(ItemID.MartianConduitPlating, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}