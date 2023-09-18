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
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.meleeBuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }
    }

    public class InflictCaelite : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (player.HasBuff(BuffType<CaeliteImbune>()) && item.CountsAsClass(DamageClass.Melee))
            {
                npc.AddBuff(BuffType<PowerDown>(), Main.rand.Next(10, 20) * 60);
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].HasBuff(BuffType<CaeliteImbune>()) && (projectile.CountsAsClass(DamageClass.Melee) || ProjectileID.Sets.IsAWhip[projectile.type]))
            {
                npc.AddBuff(BuffType<PowerDown>(), Main.rand.Next(10, 20) * 60);
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