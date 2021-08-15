using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace QwertyMod.Common
{
    public class CommonStats : ModPlayer
    {
        public float ammoReduction = 1f;
        public int genericCounter = 0;
        public int dodgeChance = 0;
        public bool dodgeDamageBoost = false;
        public bool damageBoostFromDodge = false;
        public float hookRange = 1f;
        public float hookSpeed = 1f;
        public override void ResetEffects()
        {
            ammoReduction = 1f;
            dodgeChance = 0;
            dodgeDamageBoost = false;
            hookRange = 1f;
            hookSpeed = 1f;
        }
        public override void PreUpdate()
        {
            genericCounter++;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            int dodgeRng = Main.rand.Next(100);
            if (dodgeRng < dodgeChance && dodgeRng < 80)
            {
                Player.immune = true;
                Player.immuneTime = 40;
                if (Player.longInvince)
                {
                    Player.immuneTime += 80;
                }
                if (dodgeDamageBoost)
                {
                    damageBoostFromDodge = true;
                }
                for (int i = 0; i < Player.hurtCooldowns.Length; i++)
                {
                    Player.hurtCooldowns[i] = Player.immuneTime;
                }
                #region dust and gore
                for (int j = 0; j < 100; j++)
                {
                    int num = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Dust expr_A4_cp_0 = Main.dust[num];
                    expr_A4_cp_0.position.X = expr_A4_cp_0.position.X + (float)Main.rand.Next(-20, 21);
                    Dust expr_CB_cp_0 = Main.dust[num];
                    expr_CB_cp_0.position.Y = expr_CB_cp_0.position.Y + (float)Main.rand.Next(-20, 21);
                    Main.dust[num].velocity *= 0.4f;
                    Main.dust[num].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(Player.cWaist, Player);
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.dust[num].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                        Main.dust[num].noGravity = true;
                    }
                }
                int num2 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = 1.5f + (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                num2 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num2].scale = 1.5f;
                Main.gore[num2].velocity.X = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity.Y = -1.5f - (float)Main.rand.Next(-50, 51) * 0.01f;
                Main.gore[num2].velocity *= 0.4f;
                #endregion
                if (Player.whoAmI == Main.myPlayer)
                {
                    NetMessage.SendData(62, -1, -1, null, Player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
                }
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }
        public override void PostUpdateEquips()
        {
            if (damageBoostFromDodge)
            {
                if (Player.immuneTime > 0)
                {
                    Player.GetDamage(DamageClass.Generic) += .25f;
                }
                else
                {
                    damageBoostFromDodge = false;
                }
            }
        }
    }
}
