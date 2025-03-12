using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories.Combined
{
    class FieryWhetstone : ModItem
    {
        public override void SetStaticDefaults()
        {
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
            player.GetModPlayer<FieryWhetStoneEffect>().fireOnHit = true;
            //player.magmaStone = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<EnchantedWhetstone>(), 1)
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
        public bool fireOnHit = false;
        public float effect = 0f;
        public int AP = 0;

        public override void ResetEffects()
        {
            fireOnHit = false;
            effect = 0f;
            AP = 0;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (effect != 0f && !target.buffImmune[BuffID.OnFire] && proj.CountsAsClass(DamageClass.Melee))
            {
                QwertyMethods.PokeNPC(Player, target, Projectile.InheritSource(proj), proj.GetGlobalProjectile<MagicBonusOnProj>().magicBoost * Player.GetDamage(DamageClass.Magic).Multiplicative, DamageClass.Magic, 0, AP);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (effect != 0f && !target.buffImmune[BuffID.OnFire] && modifiers.DamageType == DamageClass.Melee)
            {
                QwertyMethods.PokeNPC(Player, target, new EntitySource_Misc(""), modifiers.FinalDamage.Multiplicative * effect * Player.GetDamage(DamageClass.Magic).Multiplicative, DamageClass.Magic, 0, AP);
            }
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            if(fireOnHit && hit.DamageType == DamageClass.Melee)
            {
                target.AddBuff(BuffID.OnFire, 10 * 60);
            }
			base.OnHitNPC(target, hit, damageDone);
		}
    }

    public class FieryWhetstoneTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<FieryWhetStoneEffect>().effect > 0f && item.CountsAsClass(DamageClass.Melee))
            {
                int TLIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("CritChance"));
                TooltipLine line = new TooltipLine(Mod, "MagicBoost", (int)(item.damage * player.GetModPlayer<FieryWhetStoneEffect>().effect * player.GetDamage(DamageClass.Magic).Multiplicative) + Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipWhetDamage")));
                {
                    line.OverrideColor = Color.OrangeRed;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 1, line);
                }

                line = new TooltipLine(Mod, "MagicBoostCrit", player.GetCritChance(DamageClass.Magic) + 4 + Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipWhetCrit")));
                {
                    line.OverrideColor = Color.OrangeRed;
                }
                if (TLIndex != -1)
                {
                    tooltips.Insert(TLIndex + 2, line);
                }
            }
        }
    }
}
