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

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class EnchantedWhetstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Whetstone");
            Tooltip.SetDefault("Melee attacks inflict a bit of extra magic damage");
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
            player.GetModPlayer<WhetStoneEffect>().effect += .2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.FallenStar, 3)
                .AddIngredient(ItemID.StoneBlock, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class MagicBonusOnProj : GlobalProjectile
    {
        public int magicBoost = 0;
        public bool whetStoned = false;
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<WhetStoneEffect>().effect > 0f && (!whetStoned && projectile.CountsAsClass(DamageClass.Melee)))
            {
                magicBoost += (int)(Main.player[projectile.owner].GetModPlayer<WhetStoneEffect>().effect * Main.player[projectile.owner].HeldItem.damage);
                whetStoned = true;
            }
        }
    }

    public class WhetStoneEffect : ModPlayer
    {
        public float effect = 0f;

        public override void ResetEffects()
        {
            effect = 0f;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (effect != 0f && proj.CountsAsClass(DamageClass.Melee))
            {
                QwertyMethods.PokeNPC(Player, target, Projectile.InheritSource(proj),  proj.GetGlobalProjectile<MagicBonusOnProj>().magicBoost * Player.GetDamage(DamageClass.Magic).Multiplicative, DamageClass.Magic);
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (effect != 0f && item.CountsAsClass(DamageClass.Melee))
            {
                QwertyMethods.PokeNPC(Player, target, Player.GetProjectileSource_Item(item),  damage * effect * Player.GetDamage(DamageClass.Magic).Multiplicative, DamageClass.Magic);
            }
        }
    }

    public class WhetstoneTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<WhetStoneEffect>().effect > 0f && item.CountsAsClass(DamageClass.Melee))
            {
                int TLIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("CritChance"));
                TooltipLine line = new TooltipLine(Mod, "MagicBoost", (int)(item.damage * player.GetModPlayer<WhetStoneEffect>().effect * player.GetDamage(DamageClass.Magic).Multiplicative) + " magic damage");
                {
                    line.overrideColor = Color.SkyBlue;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 1, line);
                }

                line = new TooltipLine(Mod, "MagicBoostCrit", (player.GetCritChance(DamageClass.Magic) + 4) + "% critical strike chance");
                {
                    line.overrideColor = Color.SkyBlue;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 2, line);
                }
            }
        }
    }
}
