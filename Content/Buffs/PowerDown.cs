using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ModLoader;

using Terraria.DataStructures;

namespace QwertyMod.Content.Buffs
{
    public class PowerDown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        private int timer;

        public override void Update(NPC npc, ref int buffIndex)
        {
            timer++;
            if (timer > 30)
            {
                for (int d = 0; d < 30; d++)
                {
                    Dust dust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<CaeliteDust>())];
                    dust.velocity *= 3;
                }
                timer = 0;
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            timer++;
            if (timer > 30)
            {
                for (int d = 0; d < 30; d++)
                {
                    Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<CaeliteDust>())];
                    dust.velocity *= 3;
                }
                timer = 0;
            }
        }
    }

    public class PowerDownNPC : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<PowerDown>()))
            {
                modifiers.FinalDamage *= 0.8f;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (player.HasBuff(ModContent.BuffType<PowerDown>()))
            {
                modifiers.FinalDamage *= 0.8f;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<PowerDown>()))
            {
                modifiers.FinalDamage *= 0.8f;
            }
        }
    }
    public class PowerDownProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if(source is EntitySource_Parent parent && parent.Entity is NPC npc && npc.HasBuff(ModContent.BuffType<PowerDown>()))
            {
                projectile.damage = (int)(projectile.damage * 0.8f);
                projectile.originalDamage = (int)(projectile.originalDamage * 0.8f);
            }
        }
    }
}