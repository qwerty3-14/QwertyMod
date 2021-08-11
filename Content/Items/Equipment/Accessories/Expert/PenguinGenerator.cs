using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.Expert
{
    public class PenguinGenerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penguin Generator");
            Tooltip.SetDefault("Attacks have a 10% chance to release penguins");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 100000;
            Item.rare = 1;
            Item.expert = true;

            Item.width = 28;
            Item.height = 32;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PenguinEffect>().effect = true;
        }
    }

    public class PenguinLimit : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool realeasedPenguin = false;
    }

    public class PenguinEffect : ModPlayer
    {
        public bool effect;
        public bool noSound;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0 && effect && !target.immortal)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile penguin = Main.projectile[Projectile.NewProjectile(Player.GetProjectileSource_Item(item), Player.Center, new Vector2(6 - 12 * i, 0), ProjectileType<SlidingPenguinGeneric>(), damage, 0, Player.whoAmI)];
                    penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.Next(10) == 0 && effect && !target.immortal)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile penguin = Main.projectile[Projectile.NewProjectile(new ProjectileSource_ProjectileParent(proj), Player.Center, new Vector2(6 - 12 * i, 0), ProjectileType<SlidingPenguinGeneric>(), damage, 0, Player.whoAmI)];
                    penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                }
            }
        }
    }
}
