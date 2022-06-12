using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
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

namespace QwertyMod.Content.Items.Weapon.Minion.LuneArcherMinion
{
    class LuneArcherStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Archer Staff");
            Tooltip.SetDefault("Summons a lune archer to shoot arrows from your inventory at enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;  
            Item.mana = 20;     
            Item.width = 32;   
            Item.height = 32;    
            Item.useTime = 25;  
            Item.useAnimation = 25;   
            Item.useStyle = 1;  
            Item.noMelee = true; 
            Item.knockBack = 1f;  
            Item.value = 20000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44;   
            Item.shoot = ProjectileType<LuneArcher>();   
            Item.DamageType = DamageClass.Summon;
            Item.buffType = BuffType<LuneArcherB>(); 
            Item.buffTime = 3600;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<LuneBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
