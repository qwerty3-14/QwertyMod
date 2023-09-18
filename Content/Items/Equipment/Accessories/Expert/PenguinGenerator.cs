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
        public int buildUp;
        public int cooldown;

        public override void ResetEffects()
        {
            effect = false;
        }
        public override void PreUpdate()
        {
            if(cooldown > 0)
            {
                cooldown--;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(12) && effect && !target.immortal && cooldown <= 0)
            {
                if(cooldown <= 0)
                {
                    cooldown = 60;
                    for (int i = 0; i < 2; i++)
                    {
                        Projectile penguin = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc("Accesory_PenguinGenerator"), Player.Center, new Vector2(6 - 12 * i, 0), ProjectileType<SlidingPenguinGeneric>(), damageDone + buildUp, 0, Player.whoAmI)];
                        penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                        buildUp = 0;
                    }
                }
                else
                {
                    buildUp += damageDone;
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            if (Main.rand.NextBool(12) && effect && !target.immortal && cooldown <= 0)
            {
                if(cooldown <= 0)
                {
                    cooldown = 60;
                    for (int i = 0; i < 2; i++)
                    {
                        Projectile penguin = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(proj), Player.Center, new Vector2(6 - 12 * i, 0), ProjectileType<SlidingPenguinGeneric>(), damageDone + buildUp, 0, Player.whoAmI)];
                        penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                        buildUp = 0;
                    }
                }
                else
                {
                    buildUp += damageDone;
                }
            }
        }
    }
}
