using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Potion.CaeliteFlask
{
    public class CaeliteImbune : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weapon Imbue: Caelite Wrath");
            Description.SetDefault("Melee Attacks reduce the damage enemies deal");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            //longerExpertDebuff = false;
            Main.meleeBuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            /*
            if(player.meleeEnchant >0)
            {
                player.buffTime[player.FindBuffIndex(mod.BuffType("CaeliteImbune"))] = 0;
            }
            */
        }
    }

    public class InflictCaelite : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (player.HasBuff(BuffType<CaeliteImbune>()))
            {
                npc.AddBuff(BuffType<PowerDown>(), Main.rand.Next(10, 20));
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (Main.player[projectile.owner].HasBuff(BuffType<CaeliteImbune>()) && (projectile.CountsAsClass(DamageClass.Melee) || ProjectileID.Sets.IsAWhip[projectile.type]))
            {
                npc.AddBuff(BuffType<PowerDown>(), Main.rand.Next(10, 20));
            }
        }
    }

    public class CaeliteImbunedProjectile : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].HasBuff(BuffType<CaeliteImbune>()) && projectile.CountsAsClass(DamageClass.Melee))
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<CaeliteDust>());
            }
        }
    }

    public class CaeliteImbunedItem : GlobalItem
    {
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (player.HasBuff(BuffType<CaeliteImbune>()))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<CaeliteDust>());
            }
        }
    }
}