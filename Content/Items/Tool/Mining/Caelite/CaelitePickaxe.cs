using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Tool.Mining.Caelite
{
    public class CaelitePickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Pickaxe");
            Tooltip.SetDefault("Mines a 3x3 area");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 28;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = 25000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.width = 32;
            Item.height = 32;
            //Item.crit = 5;
            Item.autoReuse = true;
            Item.pick = 95;
            Item.tileBoost = 2;
            Item.GetGlobalItem<AoePick>().miningRadius = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<CaeliteBar>(), 16)
                .AddIngredient(ItemType<CaeliteCore>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<CaeliteDust>());
            Lighting.AddLight(hitbox.Center.ToVector2(), new Vector3(.6f, .6f, .6f));
        }
    }
}