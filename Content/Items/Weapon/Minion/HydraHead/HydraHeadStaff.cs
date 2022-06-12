using Microsoft.Xna.Framework;
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

namespace QwertyMod.Content.Items.Weapon.Minion.HydraHead
{
    class HydraHeadStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Head Staff");
            Tooltip.SetDefault("Summons a hydra head to shoot towards your cursor");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 22; 
            Item.mana = 20;     
            Item.width = 80;   
            Item.height = 80;     
            Item.useTime = 25;   
            Item.useAnimation = 25;    
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.noMelee = true; 
            Item.knockBack = 1f;  
            Item.value = 250000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item44; 
            Item.autoReuse = true;   
            Item.shoot = ProjectileType<MinionHead>();
            Item.DamageType = DamageClass.Summon;
            Item.buffType = BuffType<HydraHeadB>();  
            Item.buffTime = 3600;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

    }
}
