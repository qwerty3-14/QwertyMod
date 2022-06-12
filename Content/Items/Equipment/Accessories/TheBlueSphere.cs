using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class TheBlueSphere : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Blue Sphere");
            Tooltip.SetDefault("Magic attacks pierce 2 extra enemies\n Projectiles that normally don't pierce will use local immunity\n10% reduced magic damage\nExtra pierces will do reduced damage when hitting the same target multiple times");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagicPierePlayer>().pierceBoost += 2;
            player.GetDamage(DamageClass.Magic) -= .1f;
        }
    }

    public class MagicPierePlayer : ModPlayer
    {
        public int pierceBoost = 0;

        public override void ResetEffects()
        {
            pierceBoost = 0;
        }

    }

    public class SpherePierce : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private bool gotBoost = false;
        private int[] hitCounts = new int[200];

        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().pierceBoost > 0 && !gotBoost && projectile.friendly && projectile.CountsAsClass(DamageClass.Magic))
            {
                gotBoost = true;
                if (projectile.penetrate > 0)
                {
                    if (!projectile.usesLocalNPCImmunity && projectile.penetrate == 1)
                    {
                        projectile.localNPCHitCooldown = -10;
                        projectile.usesLocalNPCImmunity = true;
                    }
                    projectile.penetrate += Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().pierceBoost;
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (gotBoost && projectile.penetrate > 0)
            {
                if (projectile.penetrate <= Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().pierceBoost)
                {
                    damage = (int)((float)damage / (float)Math.Pow(2, hitCounts[target.whoAmI]));
                }
                if (projectile.penetrate <= Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().pierceBoost || hitCounts[target.whoAmI] < 1)
                {
                    hitCounts[target.whoAmI]++;
                }
            }
        }
    }

    public class ShpereDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.DarkCaster)
            {
                npcLoot.Add(ItemDropRule.Common(ItemType<TheBlueSphere>(), 50, 1, 1));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}

