using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories.Expert
{
    public class PenguinGenerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Penguin Generator");
            //Tooltip.SetDefault("Attacks have a 10% chance to release penguins");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 100000;
            Item.rare = ItemRarityID.Blue;
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10) && effect && !target.immortal)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile penguin = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc("Accesory_PenguinGenerator"), Player.Center, new Vector2(6 - 12 * i, 0), ProjectileType<SlidingPenguinGeneric>(), damageDone, 0, Player.whoAmI)];
                    penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10) && effect && !target.immortal)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile penguin = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(proj), Player.Center, new Vector2(6 - 12 * i, 0), ProjectileType<SlidingPenguinGeneric>(), damageDone, 0, Player.whoAmI)];
                    penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                }
            }
        }
    }
}
