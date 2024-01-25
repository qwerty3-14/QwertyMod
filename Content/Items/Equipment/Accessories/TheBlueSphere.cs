using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class TheBlueSphere : ModItem
    {
        public override void SetStaticDefaults()
        {
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

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (gotBoost && projectile.penetrate > 0)
            {
                if (projectile.penetrate <= Main.player[projectile.owner].GetModPlayer<MagicPierePlayer>().pierceBoost)
                {
                    modifiers.FinalDamage *= (1f / MathF.Pow(4, hitCounts[target.whoAmI]));
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
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TheBlueSphere>(), 50, 1, 1));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}

