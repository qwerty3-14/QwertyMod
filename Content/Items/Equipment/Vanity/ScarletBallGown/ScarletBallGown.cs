using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown
{
    [AutoloadEquip(EquipType.Body)]
    internal class ScarletBallGown : ModItem
    {

        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[QwertyMod.BallGownSkirt] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[QwertyMod.BallGownSkirtAlt] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 60);
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;
            equipSlot = QwertyMod.BallGownSkirt;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 20)
				.AddIngredient(ItemID.TissueSample, 6)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
    public class GownHoldUp : ModPlayer
    {
        public override void PostUpdate()
        {
            if(Player.legs == QwertyMod.BallGownSkirt && (Player.mount.Active || Player.sitting.isSitting))
            {
                Player.legs = QwertyMod.BallGownSkirtAlt;
            }
            /*
            if(Player.legs == QwertyMod.BallGownSkirt && !Player.compositeBackArm.enabled && !Player.compositeFrontArm.enabled && Player.velocity.Y == 0 && Player.velocity.X != 0 && Player.itemAnimation == 0)
            {
                Player.legs = QwertyMod.BallGownSkirtAlt;
                Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathF.PI / -16f * Player.direction);
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 16f * Player.direction);
            }
            */
        }
    }
}