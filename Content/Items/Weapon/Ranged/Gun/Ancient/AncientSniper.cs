using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient
{
    public class AncientSniper : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Sniper");
            //Tooltip.SetDefault("Harness the ancient power of sniping" + "\nRight click to zoom");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = 150000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item11;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/Ancient/AncientSniper_Glow").Value;
            }
            Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -26;
            Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -2;
            Item.width = 92;
            Item.height = 30;
            Item.crit = 25;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 36;
            Item.noMelee = true;
            //Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("Items/AncientItems/AncientSniper_Glow");
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(Item.GetGlobalItem<ItemUseGlow>().glowOffsetX, Item.GetGlobalItem<ItemUseGlow>().glowOffsetY);
        }

        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
    }
}