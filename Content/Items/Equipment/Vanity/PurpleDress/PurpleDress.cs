using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QwertyMod.Content.Items.Equipment.Vanity.PurpleDress
{
    [AutoloadEquip(EquipType.Body)]
    internal class PurpleDress : ModItem
    {

        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            //ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 60);
        }
        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;
            equipSlot = QwertyMod.PurpleSkirt;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 20)
				.AddIngredient(ItemID.ShadowScale, 6)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
    public class DressHoldUp : ModPlayer
    {
        public override void PostUpdate()
        {
            /*
            if(Player.legs == QwertyMod.PurpleSkirt && (Player.mount.Active || Player.sitting.isSitting))
            {
                Player.legs = QwertyMod.BallGownSkirtAlt;
            }
            */
            //Dust.NewDustPerfect(new Vector2(Player.Bottom.ToTileCoordinates().X, Player.Bottom.ToTileCoordinates().Y) * 16 + Vector2.One * 8, DustID.Torch, Vector2.Zero);
            //MathF.Abs(Player.velocity.X) > 6f || dirtyBlocks.Contains(Main.tile[Player.Bottom.ToTileCoordinates().X, Player.Bottom.ToTileCoordinates().Y].TileType)
            if(Player.legs == QwertyMod.PurpleSkirt && !Player.mount.Active && !Player.compositeBackArm.enabled && !Player.compositeFrontArm.enabled && Player.velocity.Y == 0 && (Player.controlUp) && Player.itemAnimation == 0)
            {
                Player.legs = QwertyMod.PurpleSkirtAlt;
                Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathF.PI / -16f * Player.direction);
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 16f * Player.direction);                
                //Player.bodyFrame.Y = 5 * Player.bodyFrame.Height;
            }
            else
            {
                //Player.bodyFrame.Y = 0;
            }
            
        }
    }
}