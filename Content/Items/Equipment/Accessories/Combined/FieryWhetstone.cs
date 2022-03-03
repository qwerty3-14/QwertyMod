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

namespace QwertyMod.Content.Items.Equipment.Accessories.Combined
{
    class FieryWhetstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fiery Whetstone");
            Tooltip.SetDefault("Melee attacks ingnite enemies\nMelee attacks do extra magic damage against enemies vulnerable to fire");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FieryWhetStoneEffect>().effect += .3f;
            player.magmaStone = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<EnchantedWhetstone>(), 1)
                .AddIngredient(ItemID.MagmaStone, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class FieryMagicBonusOnProj : GlobalProjectile
    {
        public int magicBoost = 0;
        public bool whetStoned = false;
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<FieryWhetStoneEffect>().effect > 0f && (!whetStoned && projectile.CountsAsClass(DamageClass.Melee)))
            {
                magicBoost += (int)(Main.player[projectile.owner].GetModPlayer<FieryWhetStoneEffect>().effect * Main.player[projectile.owner].HeldItem.damage);
                whetStoned = true;
            }
        }
    }

    public class FieryWhetStoneEffect : ModPlayer
    {
        public float effect = 0f;

        public override void ResetEffects()
        {
            effect = 0f;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (effect != 0f && !target.buffImmune[BuffID.OnFire]  && proj.CountsAsClass(DamageClass.Melee))
            {
                QwertyMethods.PokeNPC(Player, target, Projectile.InheritSource(proj), proj.GetGlobalProjectile<MagicBonusOnProj>().magicBoost * Player.GetDamage(DamageClass.Magic).Multiplicative, DamageClass.Magic);
            }
        }

        public override void ModifyHitNPC(Terraria.Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (effect != 0f &&  !target.buffImmune[BuffID.OnFire] && item.CountsAsClass(DamageClass.Melee))
            {
                QwertyMethods.PokeNPC(Player, target, Player.GetProjectileSource_Item(item), damage * effect * Player.GetDamage(DamageClass.Magic).Multiplicative, DamageClass.Magic);
            }
        }
    }

    public class FieryWhetstoneTooltips : GlobalItem
    {
        public override void ModifyTooltips(Terraria.Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<FieryWhetStoneEffect>().effect > 0f && item.CountsAsClass(DamageClass.Melee))
            {
                int TLIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("CritChance"));
                TooltipLine line = new TooltipLine(Mod, "MagicBoost", (int)(item.damage * player.GetModPlayer<FieryWhetStoneEffect>().effect * player.GetDamage(DamageClass.Magic).Multiplicative) + " magic damage");
                {
                    line.overrideColor = Color.OrangeRed;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 1, line);
                }

                line = new TooltipLine(Mod, "MagicBoostCrit", (player.GetCritChance(DamageClass.Magic) + 4) + "% critical strike chance");
                {
                    line.overrideColor = Color.OrangeRed;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 2, line);
                }
            }
        }
    }
}
