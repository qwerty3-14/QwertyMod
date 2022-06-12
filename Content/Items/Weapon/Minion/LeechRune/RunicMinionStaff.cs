using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Minion.AncientMinion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.LeechRune
{
    public class RunicMinionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Rune Staff");
            Tooltip.SetDefault("Summons an leech rune to fight for you!" + "\nchance to steal life");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 76; 
            Item.mana = 20;     
            Item.width = 72;    
            Item.height = 72;     
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1f;  
            Item.value = 500000;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ProjectileType<RunicMinionFreindly>();   
            Item.DamageType = DamageClass.Summon;
            Item.buffType = BuffType<RunicMinionB>();
            Item.buffTime = 3600;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 15)
                .AddIngredient(ItemType<AncientMinion.AncientMinionStaff>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
