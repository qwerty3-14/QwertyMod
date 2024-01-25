using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SkywardHilt : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 25000;
            Item.rare = ItemRarityID.Orange;

            Item.width = 38;
            Item.height = 24;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SkywardHiltEffect>().effect = true;
        }
    }
    public class SkywardHiltEffect : ModPlayer
    {
        public bool effect;

        public override void ResetEffects()
        {
            effect = false;
        }
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
		{
			Point origin = Player.Bottom.ToTileCoordinates();
            Point point;
            if (effect && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                                        {
                                            new Conditions.IsSolid()
                                        }), out point) && Player.grappling[0] == -1)
            {
                modifiers.FinalDamage *= 1.2f;
            }
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
		{
			if(proj.aiStyle == 190)
            {
                Point origin = Player.Bottom.ToTileCoordinates();
                Point point;
                if (effect && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                                            {
                                                new Conditions.IsSolid()
                                            }), out point) && Player.grappling[0] == -1)
                {
                    modifiers.FinalDamage *= 1.2f;
                }
            }
		}

		
		
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            
        }
    }
}