using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using QwertyMod.Content.Items.Weapon.Whip.Fork;
using Terraria.ID;

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
        public float weaponSize = 1f;
        public int normalGravity = 0;
        
        public override void ResetEffects()
        {
            ammoReduction = 1f;
            dodgeChance = 0;
            dodgeDamageBoost = false;
            hookRange = 1f;
            hookSpeed = 1f;
            weaponSize = 1f;
            normalGravity--;
        }
        public override void PreUpdate()
        {
            genericCounter++;
        }
        public override void PreUpdateBuffs()
        {
            if(normalGravity > 0)
            {
                Player.gravity = Player.defaultGravity;
            }
        }
        public override void PostUpdate()
        {
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            int dodgeRng = Main.rand.Next(100);
            if (dodgeRng < dodgeChance && dodgeRng < 80)
            {
                Player.NinjaDodge();
                modifiers.FinalDamage *= 0;
            }
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
        public int negativeCritChance = 0;
        bool criticalFailure = false;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            ProcessCritChanceNegatable(ref modifiers);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ProcessCriticalFailure();
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
                ProcessCritChanceNegatable(ref modifiers);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ProcessCriticalFailure();
            if(target.HasBuff<ForkTag>() && proj.owner == Player.whoAmI && (proj.minion || ProjectileID.Sets.MinionShot[proj.type]))
            {
                Projectile.NewProjectile(proj.GetSource_FromThis(), target.Center, (target.Center - Player.Center).SafeNormalize(-Vector2.UnitY) * 8, ModContent.ProjectileType<TagMissile>(), (int)(damageDone * 0.5f), 0, Player.whoAmI);
            }
        }
        void ProcessCritChanceNegatable(ref NPC.HitModifiers modifiers)
        {
            if (modifiers.DamageType != DamageClass.Summon && Player.GetTotalCritChance(modifiers.DamageType) < 0)
            {
                float chanceToDoHalf = Player.GetTotalCritChance(modifiers.DamageType) * -1;
                //Main.NewText(Player.GetTotalCritChance(modifiers.DamageType));
                if (Main.rand.Next(100) < chanceToDoHalf)
                {
                    modifiers.FinalDamage = modifiers.FinalDamage / 2;
                    criticalFailure = true;
                }
                modifiers.DisableCrit();
            }
        }
        void ProcessCriticalFailure()
        {
            if (criticalFailure)
            {
                int recent = -1;
                for (int i = 99; i >= 0; i--)
                {
                    CombatText ctToCheck = Main.combatText[i];
                    if (ctToCheck.lifeTime == 60 || ctToCheck.lifeTime == 120 || (ctToCheck.dot && ctToCheck.lifeTime == 40))
                    {
                        if (ctToCheck.alpha == 1f)
                        {
                            if ((ctToCheck.color == CombatText.DamagedHostile || ctToCheck.color == CombatText.DamagedHostileCrit))
                            {
                                recent = i;
                                break;
                            }
                        }
                    }
                }
                if (recent == -1)
                {
                    criticalFailure = false;
                    return;
                }
                else
                {

                    Main.combatText[recent].color = Color.Gray;
                    Main.combatText[recent].dot = true;
                    Main.combatText[recent].velocity.Y *= 0.6f;
                    Main.combatText[recent].lifeTime = 30;
                }
                criticalFailure = false;
            }
        }
    }
}
