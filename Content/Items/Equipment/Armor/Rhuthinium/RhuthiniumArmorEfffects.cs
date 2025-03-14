﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Rhuthinium
{
    public class RhuthiniumArmorEfffects : ModPlayer
    {
        public bool meleeSet = false;
        public bool magicSet = false;
        public bool rangedSet = false;
        public bool summonSet = false;
        private int rangedCounter = 0;
        private int summonCounter = 0;
        private int manaLimiter = 60;

        public override void ResetEffects()
        {
            meleeSet = magicSet = rangedSet = summonSet = false;
        }

        private void RhuthimisWraith(Vector2 position)
        {
            if (summonSet && summonCounter > 60)
            {
                summonCounter = 0;
                float rot = Main.rand.NextFloat() * 2f * MathF.PI;
                Projectile.NewProjectile(new EntitySource_Misc("SetBonus_Rhuthinium"), position + QwertyMethods.PolarVector(240, rot), QwertyMethods.PolarVector(4f, rot + MathF.PI), ModContent.ProjectileType<RhuthimisWraith>(), (int)(30 * Player.GetDamage(DamageClass.Summon).Multiplicative), (int)(5f + Player.GetKnockback(DamageClass.Summon).Multiplicative), Player.whoAmI);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.DamageType == DamageClass.Melee && meleeSet && target.life < damageDone)
            {
                Player.AddBuff(ModContent.BuffType<RhuthiniumMight>(), 300);
            }
            if (hit.DamageType.CountsAsClass(DamageClass.Magic) && magicSet && hit.Crit)
            {
                if(manaLimiter > 0)
                {
                    Player.statMana += damageDone / 2;
                    Player.ManaEffect(damageDone / 2);
                    if(ModLoader.HasMod("TRAEProject"))
                    { 
                        manaLimiter -= damageDone * 6;
                    }
                }
                Player.AddBuff(ModContent.BuffType<RhuthiniumMagic>(), 300);
                for (int num71 = 0; num71 < 5; num71++)
                {
                    int num72 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ManaRegeneration, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                    Main.dust[num72].noLight = true;
                    Main.dust[num72].noGravity = true;
                    Main.dust[num72].velocity *= 0.5f;
                }
            }
            RhuthimisWraith(target.Center);
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            rangedCounter = 0;
        }

        public override void PreUpdate()
        {
            if(manaLimiter < 60)
            {
                manaLimiter++;
            }
            summonCounter++;
            rangedCounter++;
            if (rangedCounter > 300 && rangedSet)
            {
                Player.AddBuff(ModContent.BuffType<RhuthiniumFocus>(), 2);
            }
        }

        public override void PostUpdateEquips()
        {
            if (Player.HasBuff(ModContent.BuffType<RhuthiniumMight>()))
            {
                Player.accRunSpeed *= 1.2f;
            }
        }
    }

    public class RhuthimisWraith : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.width = Projectile.height = 14;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathF.PI / 2;
            if (Projectile.timeLeft > 80)
            {
                Projectile.alpha = (int)(255f * (40f - (120f - Projectile.timeLeft)) / 40f);
            }
            if (Projectile.timeLeft < 40)
            {
                Projectile.alpha = (int)(255f * (40f - Projectile.timeLeft) / 40f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha), Projectile.rotation, new Vector2(21, 7), Vector2.One, SpriteEffects.None, 0);
            return false;
        }
    }
}