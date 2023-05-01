using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Common
{
    class QwertyGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int age = 0;
        public override bool PreAI(NPC npc)
        {
            age++;
            if (npc.HasBuff(BuffType<Stunned>()))
            {
                npc.velocity = Vector2.Zero;
                return false;
            }
            return base.PreAI(npc);
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            
            if (npc.HasBuff(BuffType<LuneCurse>()))
            {
                modifiers.CritDamage += 0.4f;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(BuffType<LuneCurse>()))
            {
                modifiers.CritDamage += 0.4f;
            }
        }
        private float stunCounter = 0;
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff(BuffType<Stunned>()))
            {
                //float area = npc.width * npc.height;
                float widthForScale = npc.width;
                if (widthForScale < 30)
                {
                    widthForScale = 30;
                }
                if (widthForScale > 300)
                {
                    widthForScale = 300;
                }
                float scale = widthForScale / 100f;
                float stunnedHorizontalMovement = (npc.width / 2) * 1.5f;
                float heightofStunned = (npc.height / 2) * 1.2f;
                stunCounter += MathF.PI / 60;
                Texture2D texture = Request<Texture2D>("QwertyMod/Common/Stun").Value;
                //Main.NewText(MathF.Sin(stunCounter));
                if (MathF.Cos(stunCounter) > 0)
                {
                    Vector2 CenterOfStunned = new Vector2(npc.Center.X + MathF.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);

                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);

                    CenterOfStunned = new Vector2(npc.Center.X - MathF.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);
                }
                else
                {
                    Vector2 CenterOfStunned = new Vector2(npc.Center.X - MathF.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);

                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);

                    CenterOfStunned = new Vector2(npc.Center.X + MathF.Sin(stunCounter) * stunnedHorizontalMovement, npc.Center.Y - heightofStunned);
                    spriteBatch.Draw(texture, new Vector2(CenterOfStunned.X - Main.screenPosition.X, CenterOfStunned.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), drawColor, stunCounter,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
