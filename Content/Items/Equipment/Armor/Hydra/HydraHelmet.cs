using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.Playerlayers;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Minion.Longsword;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Hydra
{
    [AutoloadEquip(EquipType.Head)]
    public class HydraHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Helmet");
            Tooltip.SetDefault("+0.5 life/sec regen rate" + "\n+10% summon damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = 5;

            Item.width = 28;
            Item.height = 22;
            Item.defense = 13;
            
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 1;
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }
        
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HydraScalemail>() && legs.type == ItemType<HydraLeggings>();
        }
        
        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<MinionDuper>().DupeMinions = true;
            player.setBonus = "Increases minion slots as your health depletes \nAutomaticly summons more minions to fill empty slots";
            if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .01f)
            {
                player.maxMinions += 20;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .2f)
            {
                player.maxMinions += 4;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .4f)
            {
                player.maxMinions += 3;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .6f)
            {
                player.maxMinions += 2;
            }
            else if (((player.statLife * 1.0f) / (player.statLifeMax2 * 1.0f)) < .8f)
            {
                player.maxMinions += 1;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<HydraScale>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class MinionDuper : ModPlayer
    {
        public bool DupeMinions = false;
        public override void ResetEffects()
        {
            DupeMinions = false;
        }
        public override void PostUpdateEquips()
        {
            if (DupeMinions)
            {
                float spareSlots = Player.maxMinions - Player.slotsMinions;
                if (spareSlots > 0)
                {
                    for (int p = 0; p < 1000; p++)
                    {
                        Projectile projectile = Main.projectile[p];
                        if (projectile.owner == Player.whoAmI && projectile.active && projectile.minionSlots > 0 && (projectile.type < ProjectileID.StardustDragon1 || projectile.type > ProjectileID.StardustDragon4))
                        {
                            if (projectile.type == ProjectileType<SwordMinion>() && spareSlots >= 1f)
                            {
                                projectile.minionSlots += (int)spareSlots;
                                break;
                            }
                            else
                            {
                                for (float i = 0; i < spareSlots; i += projectile.minionSlots)
                                {
                                    Player.SpawnMinionOnCursor(projectile.GetProjectileSource_FromThis(), projectile.owner, projectile.type, projectile.originalDamage, projectile.knockBack);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

}