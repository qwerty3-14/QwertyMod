using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.NPCs.Bosses.CloakedDarkBoss;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class RitualInterupter : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }


        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<CloakedDarkBoss>());
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.Center);
                QwertyMethods.NPCSpawnOnPlayer(player, ModContent.NPCType<CloakedDarkBoss>());
                return true;
            }

            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LuneBar>(), 2)
                .AddIngredient(ModContent.ItemType<CaeliteCore>(), 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}