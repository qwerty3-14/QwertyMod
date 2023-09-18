using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.WorldBuilding;
using QwertyMod.Content.Dusts;
using Terraria.Audio;
using Terraria.GameContent;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Common.PlayerLayers;

namespace QwertyMod.Content.Items.Weapon.Minion.DVR
{
    public class DVRStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Invader Caster Staff");
            //Tooltip.SetDefault("Summons an invader caster to fight for you.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 20000;
            Item.mana = 20;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<DVR>();
            Item.DamageType = DamageClass.Summon;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/DVR/DVRStaff_Glow").Value;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<DVRB>(), 3600);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }
}