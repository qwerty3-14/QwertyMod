using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SkywardHilt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyward Hilt");
            Tooltip.SetDefault("Swords deal more damage while airborne");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 25000;
            Item.rare = 3;

            Item.width = Item.height = 20;

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

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            Point origin = Player.Bottom.ToTileCoordinates();
            Point point;
            if (effect && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                                        {
                                            new Conditions.IsSolid()
                                        }), out point) && Player.grappling[0] == -1)
            {
                damage *= 2;
            }
        }
    }
}