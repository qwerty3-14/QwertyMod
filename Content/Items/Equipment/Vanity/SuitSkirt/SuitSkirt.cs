using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace QwertyMod.Content.Items.Equipment.Vanity.SuitSkirt
{
    [AutoloadEquip(EquipType.Legs)]
    internal class SuitSkirt : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 10);
        }
		public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 20)
				.AddIngredient(ItemID.BlackThread, 3)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
	public class HeelOffset : PlayerDrawLayer
	{
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HairBack);

        protected override void Draw(ref PlayerDrawSet drawinfo)
		{
            Player drawPlayer = drawinfo.drawPlayer;
			if ((drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, "SuitSkirt", EquipType.Legs) || drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, "CocktailDressSkirt", EquipType.Legs) || drawPlayer.legs == QwertyMod.PurpleSkirt || drawPlayer.legs == QwertyMod.PurpleSkirtAlt) && !drawPlayer.mount.Active && !drawinfo.hidesBottomSkin && !HeelLegs.IsBottomOverridden(ref drawinfo)) 
			{
				drawinfo.Position.Y -= 2;
			}
		}
		
	}
    public class HeelLegs : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            Player drawPlayer = drawinfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            if (!drawinfo.hidesBottomSkin && (drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, "SuitSkirt", EquipType.Legs) || drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, "CocktailDressSkirt", EquipType.Legs) || drawPlayer.legs == QwertyMod.PurpleSkirt || drawPlayer.legs == QwertyMod.PurpleSkirtAlt) && !drawinfo.hidesBottomSkin && !IsBottomOverridden(ref drawinfo)) 
            {
                drawinfo.hidesBottomSkin = true;
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/SuitSkirt/HeelLegs").Value;
				if(drawinfo.skinVar > 9)
                {
                    texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/SuitSkirt/MannequinHeelLegs").Value;
                }
				if (drawinfo.isSitting) 
                {
					DrawSittingLegs(ref drawinfo, texture, drawinfo.colorLegs);
					return;
				}
				DrawData item = new DrawData(texture, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0);
				drawinfo.DrawDataCache.Add(item);
			}
        }

		static void DrawSittingLegs(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false) {
			Vector2 legsOffset = drawinfo.legsOffset;
			Vector2 value = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
			Rectangle legFrame = drawinfo.drawPlayer.legFrame;
			value.Y -= 2f;
			value.Y += drawinfo.seatYOffset;
			value += legsOffset;
			int num = 2;
			int num2 = 42;
			int num3 = 2;
			int num4 = 2;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			bool flag = drawinfo.drawPlayer.legs == 101 || drawinfo.drawPlayer.legs == 102 || drawinfo.drawPlayer.legs == 118 || drawinfo.drawPlayer.legs == 99;
			if (drawinfo.drawPlayer.wearsRobe && !flag) {
				num = 0;
				num4 = 0;
				num2 = 6;
				value.Y += 4f;
				legFrame.Y = legFrame.Height * 5;
			}

			switch (drawinfo.drawPlayer.legs) {
				case 214:
				case 215:
				case 216:
					num = -6;
					num4 = 2;
					num5 = 2;
					num3 = 4;
					num2 = 6;
					legFrame = drawinfo.drawPlayer.legFrame;
					value.Y += 2f;
					break;
				case 106:
				case 143:
				case 226:
					num = 0;
					num4 = 0;
					num2 = 6;
					value.Y += 4f;
					legFrame.Y = legFrame.Height * 5;
					break;
				case 132:
					num = -2;
					num7 = 2;
					break;
				case 193:
				case 194:
					if (drawinfo.drawPlayer.body == 218) {
						num = -2;
						num7 = 2;
						value.Y += 2f;
					}
					break;
				case 210:
					if (glowmask) {
						Vector2 vector = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
						value += vector;
					}
					break;
			}

			for (int num8 = num3; num8 >= 0; num8--) {
				Vector2 position = value + new Vector2(num, 2f) * new Vector2(drawinfo.drawPlayer.direction, 1f);
				Rectangle value2 = legFrame;
				value2.Y += num8 * 2;
				value2.Y += num2;
				value2.Height -= num2;
				value2.Height -= num8 * 2;
				if (num8 != num3)
					value2.Height = 2;

				position.X += drawinfo.drawPlayer.direction * num4 * num8 + num6 * drawinfo.drawPlayer.direction;
				if (num8 != 0)
					position.X += num7 * drawinfo.drawPlayer.direction;

				position.Y += num2;
				position.Y += num5;
				DrawData item = new DrawData(textureToDraw, position, value2, matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0);
				item.shader = shaderIndex;
				drawinfo.DrawDataCache.Add(item);
			}
        }
        public static bool IsBottomOverridden(ref PlayerDrawSet drawinfo) 
        {
			if (ShouldOverrideLegs_CheckPants(ref drawinfo))
				return true;

			if (ShouldOverrideLegs_CheckShoes(ref drawinfo))
				return true;

			return false;
		}
        public static bool ShouldOverrideLegs_CheckPants(ref PlayerDrawSet drawinfo) 
        {
			switch (drawinfo.drawPlayer.legs) 
            {
				case 67:
				case 106:
				case 138:
				case 140:
				case 143:
				case 217:
				case 222:
				case 226:
				case 228:
					return true;
				default:
					return false;
			}
		}

		public static bool ShouldOverrideLegs_CheckShoes(ref PlayerDrawSet drawinfo) 
        {
			if (drawinfo.drawPlayer.shoe == 15)
				return true;

			return false;
		}
    }

}