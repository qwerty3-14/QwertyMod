using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class Hitstun : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.cursed = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
        public class HitstunEffect : GlobalNPC
        {
			public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
			{
                if(npc.HasBuff(ModContent.BuffType<Hitstun>()))
                {
                    return false;
                }
				return base.CanHitPlayer(npc, target, ref cooldownSlot);
			}
			public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
			{
                if(npc.HasBuff(ModContent.BuffType<Hitstun>()))
                {
                    npc.color = Color.LimeGreen;
                    if(npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<Hitstun>())] <=2)
                    {
                        NPC fake = new NPC();
                        fake.SetDefaults(npc.type);
                        npc.color = fake.color;
                    }
                }
				return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
			}
		}
    }
}