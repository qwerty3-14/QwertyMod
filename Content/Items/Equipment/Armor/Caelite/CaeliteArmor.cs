using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;


namespace QwertyMod.Content.Items.Equipment.Armor.Caelite
{
    [AutoloadEquip(EquipType.Body)]
    public class CaeliteArmor : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 30000;
            Item.rare = ItemRarityID.Orange;

            Item.width = 32;
            Item.height = 24;
            Item.defense = 8;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CaeliteBar>(), 16)
                .AddIngredient(ModContent.ItemType<CaeliteCore>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateEquip(Player player)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && !Main.projectile[i].GetGlobalProjectile<CaeliteArmorEffect>().g)
                {
                    Main.projectile[i].GetGlobalProjectile<CaeliteArmorEffect>().g = true;
                }
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<CaeliteHelm>() && legs.type == ModContent.ItemType<CaeliteGreaves>();
        }

        public override void ArmorSetShadows(Player player)
        {
            //Main.NewText("active set effect");

            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Effects granted by this armor are 25% more effective!";
            player.GetModPlayer<CaeliteSetBonus>().setBonus = true;
        }
    }

    public class CaeliteSetBonus : ModPlayer
    {
        public bool setBonus;

        public override void ResetEffects()
        {
            setBonus = false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Player.GetModPlayer<CaeliteHelmEffect>().hasEffect && damageDone > target.life && (hit.DamageType.CountsAsClass(DamageClass.Magic) || hit.DamageType.CountsAsClass(DamageClass.Melee)))
            {
                target.value = (int)(target.value * 1.25f);
            }
		}
    }

    public class CaeliteArmorEffect : GlobalProjectile
    {
        public bool g;

        public override bool InstancePerEntity => true;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (g)
            {
                Point origin = target.Bottom.ToTileCoordinates();
                Point point;
                if (projectile.CountsAsClass(DamageClass.Magic) && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(4), new GenCondition[]
                                            {
                                            new Conditions.IsSolid()
                                            }), out point))
                {
                    if (Main.player[projectile.owner].GetModPlayer<CaeliteSetBonus>().setBonus)
                    {
                        modifiers.FinalDamage *= 1.25f;
                    }
                    else
                    {
                        modifiers.FinalDamage *= 1.25f;
                    }
                }
                if (projectile.CountsAsClass(DamageClass.Melee) && WorldUtils.Find(origin, Searches.Chain(new Searches.Down(4), new GenCondition[]
                                            {
                                            new Conditions.IsSolid()
                                            }), out point))
                {
                    if (Main.player[projectile.owner].GetModPlayer<CaeliteSetBonus>().setBonus)
                    {
                        modifiers.FinalDamage *= 1.25f;
                    }
                    else
                    {
                        modifiers.FinalDamage *= 1.25f;
                    }
                }
            }
        }
    }
}