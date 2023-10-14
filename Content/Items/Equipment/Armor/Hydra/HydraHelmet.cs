using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Minion.Longsword;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Armor.Hydra
{
    [AutoloadEquip(EquipType.Head)]
    public class HydraHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }


        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = ItemRarityID.Pink;
            Item.width = 30;
            Item.height = 26;
            Item.defense = 13;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraHelmet_Glow").Value;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 1;
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }


        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HydraScalemail>() && legs.type == ModContent.ItemType<HydraLeggings>();
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
            CreateRecipe(1).AddIngredient(ModContent.ItemType<HydraScale>(), 12)
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
                            if (projectile.type == ModContent.ProjectileType<SwordMinion>() && spareSlots >= 1f)
                            {
                                projectile.minionSlots += (int)spareSlots;
                                break;
                            }
                            else
                            {
                                for (float i = 0; i < spareSlots; i += projectile.minionSlots)
                                {
                                    Player.SpawnMinionOnCursor(projectile.GetSource_FromThis(), projectile.owner, projectile.type, projectile.originalDamage, projectile.knockBack);
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