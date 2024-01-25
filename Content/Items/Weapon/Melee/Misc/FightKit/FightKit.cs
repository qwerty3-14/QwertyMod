using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Weapon.Melee.Sword.EtimsSword;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Melee.Misc.FightKit
{
    public class KitJump : ExtraJump
	{
		public override Position GetDefaultPosition() => new Before(SandstormInABottle);
		/*
		public override IEnumerable<Position> GetModdedConstraints() 
		{
			// By default, modded extra jumps set to be between two vanilla extra jumps (via After and Before) are ordered in load order.
			// This hook allows you to organize where this extra jump is located relative to other modded extra jumps that are also
			// placed between the same two vanila extra jumps.
			yield return new Before(ModContent.GetInstance<MultipleUseExtraJump>());
		}
		*/

		public override float GetDurationMultiplier(Player player) 
		{
			// Use this hook to set the duration of the extra jump
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			return 1f;
		}

		public override void UpdateHorizontalSpeeds(Player player) 
		{
			// Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			player.moveSpeed *= 1.5f;
		}

		public override void OnStarted(Player player, ref bool playSound) 
		{
			// Use this hook to trigger effects that should appear at the start of the extra jump
		}

		public override void ShowVisuals(Player player) 
		{
			// Use this hook to trigger effects that should appear throughout the duration of the extra jump
			// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
		}
	}
    public class FightKit : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.knockBack = 0;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Orange;
            Item.width = 18;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 12f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.shoot = ModContent.ProjectileType<FightKitP>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Knockback") //this checks if it's the line we're interested in
                {
                    line.Text = Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipCKB"));//change tooltip
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            return player.itemAnimation == 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, (byte)ChooseAtk(player));
            return false;
        }
        public static KitAttacks ChooseAtk(Player player)
        {
            KitAttacks atk = KitAttacks.Jab;
            if (player.altFunctionUse == 2)
            {
                if(player.controlUp)
                {
                    atk = KitAttacks.USpecial;
                }
                else if(player.controlDown)
                {
                    atk = KitAttacks.DSpecial;
                }
                else if(player.controlLeft || player.controlRight)
                {
                    atk = KitAttacks.FSpecial;
                }
                else
                {
                    atk = KitAttacks.NSpecial;
                }
            }
            else if(player.velocity.Y != 0)
            {
                if(player.controlUp)
                {
                    atk = KitAttacks.UpAir;
                }
                else if(player.controlDown)
                {
                    atk = KitAttacks.Dair;
                }
                else if(player.controlLeft || player.controlRight)
                {
                    atk = KitAttacks.Fair;
                }
                else
                {
                    atk = KitAttacks.Nair;
                }
            }
            else
            {
                if(player.controlUp)
                {
                    atk = KitAttacks.UTilt;
                }
                else if(player.controlDown)
                {
                    atk = KitAttacks.DTilt;
                }
                else if(player.controlLeft || player.controlRight)
                {
                    atk = KitAttacks.FTilt;
                }
                else
                {
                    atk = KitAttacks.Jab;
                }
            }
            return atk;
        }
    }
    public enum KitAttacks : byte
    {
        Jab,
        Jab2,
        Jab3,
        FTilt,
        DTilt,
        UTilt,
        Nair,
        Fair,
        Dair,
        UpAir,
        Bair,
        NSpecial,
        FSpecial,
        USpecial,
        DSpecial
    }
    public class FightKitP : ModProjectile
    {
        bool runOnce = false;
        KitAttacks atk = KitAttacks.Jab;
        int startUp = -1;
        int endLag = -1;
        int attackTime = -1;
        bool etimsActive = false;
        bool invaActive = false;
        public override void SetDefaults()
        {
            Projectile.timeLeft = 18;
            Projectile.width = Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
        }

		public override void AI()
        {
            if(!runOnce)
            {
                runOnce = true;
                atk = (KitAttacks)Projectile.ai[0];
                switch(atk)
                {
                    case KitAttacks.Jab:
                        startUp = 4;
                        endLag = 9;
                        attackTime = 14;
                    break;
                    case KitAttacks.Jab2:
                        startUp = 2;
                        endLag = 12;
                        attackTime = 18;
                    break;
                    case KitAttacks.Jab3:
                        startUp = 4;
                        endLag = 8;
                        attackTime = 9;
                    break;
                    case KitAttacks.FTilt:
                        startUp = 12;
                        endLag = 20;
                        attackTime = 30;
                    break;
                    case KitAttacks.DTilt:
                        startUp = 12;
                        endLag = 26;
                        attackTime = 30;
                    break;
                    case KitAttacks.UTilt:
                        startUp = 14;
                        endLag = 26;
                        attackTime = 40;
                    break;
                    case KitAttacks.Nair:
                        startUp = 4;
                        endLag = 10;
                        attackTime = 15;
                    break;
                    case KitAttacks.Fair:
                        startUp = 4;
                        endLag = 18;
                        attackTime = 26;
                    break;
                    case KitAttacks.UpAir:
                        startUp = 5;
                        endLag = 10;
                        attackTime = 16;
                    break;
                    case KitAttacks.Dair:
                        startUp = 5;
                        endLag = 10;
                        attackTime = 16;
                    break;
                    case KitAttacks.FSpecial:
                        startUp = 10;
                        endLag = 20;
                        attackTime = 60;
                    break;
                    case KitAttacks.USpecial:
                        startUp = 10;
                        endLag = 12;
                        attackTime = 20;
                    break;
                    case KitAttacks.DSpecial:
                        startUp = 2;
                        endLag = 8;
                        attackTime = 20;
                    break;
                }

                for(int i = 0 ; i < Projectile.localNPCImmunity.Length; i++)
                {
                    Projectile.localNPCImmunity[i] = 0;
                }
                Projectile.timeLeft = attackTime;
                
            }
            etimsActive = false;
            invaActive = false;
            int progress = attackTime - Projectile.timeLeft;
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            player.itemTime = player.itemAnimation = Projectile.timeLeft;
            player.itemAnimationMax = player.itemTimeMax = attackTime;
            FightMode FightPlayer = player.GetModPlayer<FightMode>();
            FightPlayer.InvaKuniExtend = 0;
            FightPlayer.EtimsKuniScale = 1f;
            FightPlayer.CanParry = false;
            switch(atk)
            {
                case KitAttacks.Jab:
                    player.velocity = Vector2.Zero;
                    if(progress <= startUp)
                    {
                        float ratio = (float)progress / startUp;
                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, ((-1f * MathF.PI / 8f + ratio * 2f * MathF.PI / 8f) * player.direction) );
                        FightPlayer.EtimsKuniRot = (-1f * MathF.PI / 8f * player.direction + MathF.PI / 2f * -player.direction) + MathF.PI / 2f;
                    }
                    else 
                    {
                        float ratio = MathF.Min((float)(progress - startUp) / (endLag - startUp - 4), 1f);
                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, ((1f * MathF.PI / 8f + ratio * -4f * MathF.PI / 8f) * player.direction) );
                        FightPlayer.EtimsKuniRot = (-1f * MathF.PI / 8f * player.direction * (1f - ratio) + MathF.PI / 2f * -player.direction) + MathF.PI / 2f;
                        if(progress < endLag)
                        {
                            etimsActive = true;
                        }
                    }
                    if(Projectile.timeLeft == 2 && Main.mouseLeft && Projectile.owner == Main.myPlayer)
                    {
                        Projectile.netUpdate = true;
                        KitAttacks next = FightKit.ChooseAtk(player);
                        if(next == KitAttacks.Jab)
                        {
                            next = KitAttacks.Jab2;
                        }
                        Projectile.ai[0] = (float)next;
                        runOnce = false;
                    }
                break;
                case KitAttacks.Jab2:
                    player.velocity = Vector2.Zero;
                    if(progress > startUp)
                    {
                        float ratio = MathF.Min((float)(progress - startUp) / (endLag - startUp - 4), 1f);
                        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (MathF.PI / 8f + ratio * 11f * MathF.PI / 8f) * player.direction);
                        FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * player.direction  * (1f - ratio) + MathF.PI / 2f;
                        if(progress < endLag)
                        {
                            invaActive = true;
                        }
                    }
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, ((-3f * MathF.PI / 8f) * player.direction) );
                    FightPlayer.EtimsKuniRot = (MathF.PI / 2f * -player.direction) + MathF.PI / 2f;


                    if(Projectile.timeLeft == 2 && Main.mouseLeft && Projectile.owner == Main.myPlayer)
                    {
                        Projectile.netUpdate = true;
                        KitAttacks next = FightKit.ChooseAtk(player);
                        if(next == KitAttacks.Jab)
                        {
                            next = KitAttacks.Jab3;
                        }
                        Projectile.ai[0] = (float)next;
                        runOnce = false;
                    }
                break;

                case KitAttacks.Jab3:
                {
                    player.velocity = Vector2.Zero;
                    Player.CompositeArmStretchAmount amt = Player.CompositeArmStretchAmount.Quarter;
                    if(progress > startUp)
                    {
                        amt = Player.CompositeArmStretchAmount.Full;
                        etimsActive = true;
                        invaActive = true;
                    }
                    player.SetCompositeArmBack(true, amt, ((-3f * MathF.PI / 8f) * player.direction));
                    FightPlayer.EtimsKuniRot = (MathF.PI / 2f * -player.direction) + MathF.PI / 2f;

                    player.SetCompositeArmFront(true, amt, (12f * MathF.PI / 8f) * player.direction);
                    FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f;

                    if(Projectile.timeLeft == 2 && Main.mouseLeft && Projectile.owner == Main.myPlayer)
                    {
                        Projectile.netUpdate = true;
                        KitAttacks next = FightKit.ChooseAtk(player);
                        if(next == KitAttacks.Jab)
                        {
                            next = KitAttacks.Jab3;
                        }
                        Projectile.ai[0] = (float)next;
                        runOnce = false;
                    }
                }
                break;

                case KitAttacks.FTilt:
                    if(progress <= startUp)
                    {
                        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, -3 * MathF.PI / 4f * player.direction);
                        FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;

                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, -3 * MathF.PI / 4f * player.direction);
                        FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;
                    }
                    else
                    {
                        float ratio = MathF.Min((float)(progress - startUp) / (endLag - startUp - 4), 1f);

                        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (-3 * MathF.PI / 4f + ratio * MathF.PI) * player.direction);
                        FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;

                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (-3 * MathF.PI / 4f + ratio * MathF.PI) * player.direction);
                        FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;

                        

                        if(progress < endLag)
                        {
                            invaActive = true;
                            etimsActive = true;
                        }
                    }
                    
                break;
                case KitAttacks.DTilt:
                {
                    player.velocity = Vector2.Zero;
                    float ratio = 0f;
                    if(progress > startUp)
                    {
                        ratio = MathF.Min((float)(progress - startUp) / (endLag - startUp), 1f);
                        if(progress < endLag)
                        {
                            invaActive = true;
                            etimsActive = true;
                        }
                    }
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 4f * player.direction);
                    FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * player.direction + MathF.PI / 2f + player.direction * MathF.PI * -2f * ratio;

                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, -MathF.PI / 4f * player.direction);
                    FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + -MathF.PI / 2f * player.direction + MathF.PI / 2f + player.direction * MathF.PI * 2f * ratio;
                    
                }
                break;
                case KitAttacks.UTilt:
                    player.velocity = Vector2.Zero;
                    FightPlayer.InvaKuniRot = -MathF.PI / 2f;
                    FightPlayer.EtimsKuniRot = -MathF.PI / 2f;
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 8f * -player.direction);
                    if(progress <= startUp)
                    {
                        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, -MathF.PI / 4f * player.direction);
                    }
                    else
                    {
                        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, -3f * MathF.PI / 4f * player.direction);
                        FightPlayer.InvaKuniExtend = 1;
                        if(progress > startUp + 4 && progress < endLag - 3)
                        {
                            FightPlayer.InvaKuniExtend = 2;
                        }
                        if(progress < endLag)
                        {
                            invaActive = true;
                        }
                        else
                        {
                            FightPlayer.InvaKuniExtend = 0;
                        }
                    }
                break;
                case KitAttacks.Nair:
                {
                    float ratio = 0f;
                    if(progress > startUp)
                    {
                        ratio = MathF.Min((float)(progress - startUp) / (endLag - startUp), 1f);
                        if(progress < endLag)
                        {
                            invaActive = true;
                            etimsActive = true;
                        }
                    }
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, 3f * MathF.PI / 4f * player.direction + -ratio * MathF.PI / 2f * player.direction);
                    FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f;

                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, -3f * MathF.PI / 4f * player.direction + ratio * MathF.PI / 2f * player.direction);
                    FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + MathF.PI / 2f;
                    
                }
                break;
                case KitAttacks.Fair:
                    if(progress <= startUp)
                    {
                        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, -4 * MathF.PI / 4f * player.direction);
                        FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;

                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Quarter, (-4 * MathF.PI / 4f + MathF.PI) * player.direction);
                        FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f + MathF.PI;
                    }
                    else
                    {
                        float ratio = MathF.Min((float)(progress - startUp) / (endLag - startUp - 6), 1f);

                        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (-4 * MathF.PI / 4f + ratio * MathF.PI) * player.direction);
                        FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;

                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Quarter, (-4 * MathF.PI / 4f + (1f - ratio) * MathF.PI) * player.direction);
                        FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f + MathF.PI;

                        

                        if(progress < endLag)
                        {
                            invaActive = true;
                            etimsActive = true;
                        }
                    }
                break;
                case KitAttacks.UpAir:
                {
                    FightPlayer.InvaKuniRot = -MathF.PI / 2f;
                    FightPlayer.EtimsKuniRot = -MathF.PI / 2f;
                    Player.CompositeArmStretchAmount amt = Player.CompositeArmStretchAmount.Quarter;
                    if(progress > startUp && progress < endLag)
                    {
                        amt = Player.CompositeArmStretchAmount.Full;
                        etimsActive = true;
                        invaActive = true;
                    }
                    player.SetCompositeArmBack(true, amt, MathF.PI * -player.direction);
                    player.SetCompositeArmFront(true, amt, MathF.PI * -player.direction);
                }
                break;
                case KitAttacks.Dair:
                {
                    float ratio = 0f;
                    if(progress > startUp)
                    {
                        ratio = MathF.Min((float)(progress - startUp) / (endLag - (startUp + 2)), 1f);
                        if(progress < endLag)
                        {
                            invaActive = true;
                            etimsActive = true;
                        }
                    }
                    if(progress > endLag - 2 && Main.mouseLeft && Projectile.owner == Main.myPlayer && player.velocity.Y != 0)
                    {
                        progress = endLag - 2;
                        Projectile.netUpdate = true;
                        Projectile.timeLeft = attackTime - progress;
                    }
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, 3f * MathF.PI / 4f * player.direction + -ratio * MathF.PI / 2f * player.direction);
                    FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + (MathF.PI / 4f * -player.direction * ratio) + MathF.PI / 2f;

                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, -3f * MathF.PI / 4f * player.direction + ratio * MathF.PI / 2f * player.direction);
                    FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + (MathF.PI / 4f * player.direction * ratio) + MathF.PI / 2f;
                }
                break;
                case KitAttacks.FSpecial:
                {
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 8f * player.direction);
                    FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * player.direction + MathF.PI / 2f;
                    if(progress < endLag)
                    {
                        float ratio = 0f;
                        if(progress > startUp)
                        {
                            ratio = MathF.Min(1f, (float)(progress - startUp) / (endLag - startUp));
                        }
                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (9f * MathF.PI / 8f  - MathF.PI * ratio) * -player.direction);
                        FightPlayer.EtimsKuniRot = player.compositeBackArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;
                        if(progress == endLag - 1)
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.MountedCenter, Vector2.UnitX * player.direction * 12f, ModContent.ProjectileType<ThrowEtimsKuni>(), Projectile.damage * 2, 1f, Projectile.owner);
                            FightPlayer.EtimsKuniScale = 0f;
                        }
                    }
                    else
                    {
                        float ratio = MathF.Min(1f, (float)(progress - endLag) / (attackTime - endLag));
                        FightPlayer.EtimsKuniScale = ratio;
                        player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, -3f * MathF.PI / 8f  * player.direction);
                        FightPlayer.EtimsKuniRot = (MathF.PI / 2f + ratio * -4f * MathF.PI) * -player.direction + MathF.PI / 2f;
                    }
                }
                break;
                case KitAttacks.USpecial:
                {
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 8f * player.direction);
                    FightPlayer.InvaKuniRot = -MathF.PI / 2f;

                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 8f * -player.direction);
                    FightPlayer.EtimsKuniRot = -MathF.PI / 2f;
                    player.velocity *= 0;

                    if(progress == 18)
                    {
                        Vector2 dirVect = -Vector2.UnitY;
                        if(player.controlLeft)
                        {
                            if(player.controlUp)
                            {
                                dirVect = QwertyMethods.PolarVector(1f, 5f * MathF.PI / 4f);
                            }
                            else if(player.controlDown)
                            {
                                dirVect = QwertyMethods.PolarVector(1f, 3f * MathF.PI / 4f);
                            }
                            else
                            {
                                dirVect = -Vector2.UnitX;
                            }
                        }
                        else if(player.controlRight)
                        {
                            if(player.controlUp)
                            {
                                dirVect = QwertyMethods.PolarVector(1f, 7f * MathF.PI / 4f);
                            }
                            else if(player.controlDown)
                            {
                                dirVect = QwertyMethods.PolarVector(1f, 1f * MathF.PI / 4f);
                            }
                            else
                            {
                                dirVect = Vector2.UnitX;
                            }
                        }
                        else
                        {
                            if(player.controlDown)
                            {
                                dirVect = Vector2.UnitY;
                            }
                        }
                        for(int l = 400; l > 0; l--)
                        {
                            if(!Collision.SolidCollision((player.Center + dirVect * l) / 16f, player.width, player.height))
                            {
                                player.Center += dirVect * l;
                                SoundEngine.PlaySound(SoundID.Item8, player.Center);
                                int dustCount = 80;
                                for (int i = 0; i < dustCount; i++)
                                {
                                    float rot = MathF.PI * 2f * ((float)i / dustCount);
                                    Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                                    Dust d = Dust.NewDustPerfect(player.Top + new Vector2(unitVector.X * player.width, unitVector.Y * player.width / 2f), ModContent.DustType<InvaderGlow>(), Vector2.UnitY * player.height * 0.2f);
                                    d.noGravity = true;
                                    d.frame.Y = 0;
                                    d.scale *= 2;
                                }
                                break;
                            }
                        }
                    }
                }
                break;
                case KitAttacks.DSpecial:
                {
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, 3f * MathF.PI / 4f * player.direction);
                    FightPlayer.InvaKuniRot = player.compositeFrontArm.rotation + MathF.PI / 2f * -player.direction + MathF.PI / 2f;

                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 4f * -player.direction);
                    FightPlayer.EtimsKuniRot = -MathF.PI / 2f;
                    if(progress > startUp && progress <= endLag)
                    {
                        FightPlayer.CanParry = true;
                    }
                }
                break;
            }
            FightPlayer.InvaKuniPos = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation);
            FightPlayer.EtimsKuniPos = player.GetBackHandPosition(player.compositeBackArm.stretch, player.compositeBackArm.rotation);
            
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float useless = 0;
            Player player = Main.player[Projectile.owner];
            FightMode FightPlayer = player.GetModPlayer<FightMode>();
            if(etimsActive)
            {
                Vector2 start = FightPlayer.EtimsKuniPos + QwertyMethods.PolarVector(15, FightPlayer.EtimsKuniRot);
                if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, FightPlayer.EtimsKuniPos + QwertyMethods.PolarVector(41, FightPlayer.EtimsKuniRot), 6, ref useless) || targetHitbox.Contains((int)start.X, (int)start.Y))
                {
                    return true;
                }
            }
            if(invaActive)
            {
                float length = 41;
                if(FightPlayer.InvaKuniExtend == 1)
                {
                    length = FightMode.MidExtendLength;
                }
                if(FightPlayer.InvaKuniExtend == 2)
                {
                    length = FightMode.HighExtendLegth;;
                }
                Vector2 start = FightPlayer.InvaKuniPos + QwertyMethods.PolarVector(15, FightPlayer.InvaKuniRot);
                if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, FightPlayer.InvaKuniPos + QwertyMethods.PolarVector(length, FightPlayer.InvaKuniRot), 6, ref useless) || targetHitbox.Contains((int)start.X, (int)start.Y))
                {
                    return true;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(runOnce);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            runOnce = reader.ReadBoolean();
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            if(atk == KitAttacks.Dair)
            {
			    Projectile.localNPCImmunity[target.whoAmI] = 20;
                Main.player[Projectile.owner].velocity.Y = -Player.jumpSpeed;
                Main.player[Projectile.owner].RefreshExtraJumps();
            }
            int hitStunTime = attackTime * 2;
            target.AddBuff(ModContent.BuffType<Hitstun>(), hitStunTime);

            float kB = 1f;
            Vector2 knockBackDir = Vector2.UnitX * Main.player[Projectile.owner].direction;
            knockBackDir.Y += -0.5f;
            knockBackDir.Normalize();
            float knockBackResist = target.knockBackResist;
            switch(atk)
            {
                case KitAttacks.Jab:
                case KitAttacks.Jab2:
                case KitAttacks.Jab3:
                kB = 0.1f;
                break;
                case KitAttacks.FTilt:
                kB = 2f;
                break;
                case KitAttacks.DTilt:
                kB = 5f;
                break;
                case KitAttacks.UTilt:
                knockBackDir = -Vector2.UnitY;
                kB = 4f;
                break;
                case KitAttacks.Nair:
                kB = 1f;
                break;
                case KitAttacks.Dair:
                knockBackDir = Vector2.UnitY;
                kB = 2f;
                break;
                case KitAttacks.Fair:
                knockBackDir = Vector2.UnitX * Main.player[Projectile.owner].direction;
                kB = 1.5f;
                break;
                case KitAttacks.UpAir:
                knockBackDir = -Vector2.UnitY;
                kB = 1.5f;
                break;
            }
            kB *= 10;
            if (knockBackResist > 0f) 
            {
					float modifiedKnockback = kB * (0.9f + 0.2f * knockBackResist);
					if (target.onFire2)
						modifiedKnockback *= 1.1f;

					if (hit.Crit)
						modifiedKnockback *= 1.4f;
                    
                    target.velocity = knockBackDir * modifiedKnockback;
					
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            switch(atk)
            {
                case KitAttacks.Jab:
                    modifiers.FinalDamage *= 1.5f;
                break;
                case KitAttacks.Jab2:
                    modifiers.FinalDamage *= 2f;
                break;
                case KitAttacks.Jab3:
                break;
                case KitAttacks.FTilt:
                    modifiers.FinalDamage *= 3f;
                break;
                case KitAttacks.DTilt:
                    modifiers.FinalDamage *= 4f;
                break;
                case KitAttacks.UTilt:
                    modifiers.FinalDamage *= 5f;
                break;
                case KitAttacks.Nair:
                break;
                case KitAttacks.Dair:
                    modifiers.FinalDamage *= 2f;
                break;
                case KitAttacks.Fair:
                    modifiers.FinalDamage *= 2f;
                break;
                case KitAttacks.UpAir:
                    modifiers.FinalDamage *= 3f;
                break;
            }
        }
    }
    public class ThrowEtimsKuni : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 1;
        }

        public bool runOnce = true;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.frameCounter++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        null, Color.White, Projectile.rotation,
                        new Vector2(50 - 7, 7), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
    public class FightMode : ModPlayer
    {
        public const int KuniSpriteWidth = 14;
        public const int KuniHoldAt = 9;
        public const int KuniTotalLength = 52;
        public const int MidExtendLength = 160;
        public const int HighExtendLegth = 240;
        public Vector2 EtimsKuniPos = Vector2.Zero;
        public Vector2 InvaKuniPos = Vector2.Zero;
        public float EtimsKuniRot = 0;
        public float InvaKuniRot = 0;
        public int InvaKuniExtend = 0;
        public float EtimsKuniScale = 1f;
        public bool CanParry = false;

		public override void PostItemCheck()
        {
            if(Player.HeldItem.type == ModContent.ItemType<FightKit>())
            {
                if(Player.velocity.Y < 0)
                {
                    Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, 3f * MathF.PI / 4f * Player.direction);
                    InvaKuniRot = Player.compositeFrontArm.rotation + MathF.PI / 2f * -Player.direction;

                    Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 3f * MathF.PI / 4f * -Player.direction);
                    EtimsKuniRot = Player.compositeBackArm.rotation + MathF.PI / 2f * Player.direction;
                    
                }
                else if(Player.velocity.Y == 0 && Player.velocity.X != 0)
                {
                    Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, -MathF.PI / 4f * Player.direction);
                    InvaKuniRot = Player.compositeFrontArm.rotation + MathF.PI / 2f * -Player.direction;

                    Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, -MathF.PI / 4f * Player.direction);
                    EtimsKuniRot = Player.compositeBackArm.rotation + MathF.PI / 2f * -Player.direction;
                }
                else
                {
                    Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 8f * Player.direction);
                    InvaKuniRot = Player.compositeFrontArm.rotation + MathF.PI / 2f * Player.direction;

                    Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathF.PI / 8f * -Player.direction);
                    EtimsKuniRot = Player.compositeBackArm.rotation + MathF.PI / 2f * -Player.direction;
                }

                InvaKuniPos = Player.GetFrontHandPosition(Player.compositeFrontArm.stretch, Player.compositeFrontArm.rotation);
                EtimsKuniPos = Player.GetBackHandPosition(Player.compositeBackArm.stretch, Player.compositeBackArm.rotation);
                int fHeight = 56;
                if (Player.bodyFrame.Y == 7 * fHeight || Player.bodyFrame.Y == 8 * fHeight || Player.bodyFrame.Y == 9 * fHeight || Player.bodyFrame.Y == 14 * fHeight || Player.bodyFrame.Y == 15 * fHeight || Player.bodyFrame.Y == 16 * fHeight)
                {
                    if (Player.gravDir == -1)
                    {
                        InvaKuniPos.Y += 2;
                        EtimsKuniPos.Y += 2;
                    }
                    else
                    {
                        InvaKuniPos.Y -= 2;
                        EtimsKuniPos.Y -= 2;
                    }
                }
                InvaKuniRot += MathF.PI / 2f;
                EtimsKuniRot += MathF.PI / 2f;
            }
        }

        public override void PostUpdateEquips()
        {
            if(Player.HeldItem.type == ModContent.ItemType<FightKit>())
            {
                Player.GetJumpState<KitJump>().Enable();
                Player.noFallDmg = true;
                if(ModLoader.HasMod("TRAEProject"))
                {
                    Player.moveSpeed *= 1.5f;
                    if(!Player.controlDown)
                    {
                        Player.jumpSpeedBoost += (0.5f * 5) * 1.28f;
                    }
                }
                else
                {
                    Player.moveSpeed *= 1.5f * 1.33f;
                    if(!Player.controlDown)
                    {
                        Player.jumpSpeedBoost += 2.4f;
                    }
                }
                Player.runAcceleration *= 4f;
                if(Player.velocity.Y == 0)
                {
                    Player.runSlowdown *= 2f;
                }
                else
                {
                    Player.runSlowdown /= 4f;
                }
            }
        }

		public override void OnHurt(Player.HurtInfo info)
		{
			base.OnHurt(info);
            if(Player.HeldItem.type == ModContent.ItemType<FightKit>())
            {
                Player.AddBuff(ModContent.BuffType<Hitstun>(), 60);
            }
		}
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if(CanParry)
            {
                for(int d = 0; d < 200; d++)
                {
                    float dRot = ((float)d / 40f) * MathF.PI * 2f;
                    Dust.NewDustPerfect(Player.Center, ModContent.DustType<DarknessDust>(), QwertyMethods.PolarVector(4, dRot));
                }
                Player.SetImmuneTimeForAllTypes(120);
                Player.AddBuff(ModContent.BuffType<AntiProjectile>(), 5 * 60);
                if(info.DamageSource.SourceNPCIndex != -1)
                {
                    Player.Center = Main.npc[info.DamageSource.SourceNPCIndex].Center;
                    NPC.HitInfo hit = new NPC.HitInfo();
                    hit.Damage = Player.GetWeaponDamage(Player.HeldItem) * 10;
                    hit.DamageType = DamageClass.Melee;
                    Main.npc[info.DamageSource.SourceNPCIndex].StrikeNPC(hit);
                }
                if(info.DamageSource.SourceProjectileLocalIndex != -1)
                {
                    NPC nPC = Main.projectile[info.DamageSource.SourceProjectileLocalIndex].GetGlobalProjectile<QwertyGlobalProjectile>().npcOwner;
                    if(nPC != null && nPC.active)
                    {
                        Player.Center = nPC.Center;
                        NPC.HitInfo hit = new NPC.HitInfo();
                        hit.Damage = Player.GetWeaponDamage(Player.HeldItem) * 10;
                        hit.DamageType = DamageClass.Melee;
                        nPC.StrikeNPC(hit);
                    }
                }
                if(info.DamageSource.SourceNPCIndex != -1 || info.DamageSource.SourceProjectileLocalIndex != -1)
                {
                    SoundEngine.PlaySound(SoundID.Item8, Player.Center);
                    int dustCount = 80;
                    for (int i = 0; i < dustCount; i++)
                    {
                        float rot = MathF.PI * 2f * ((float)i / dustCount);
                        Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                        Dust d = Dust.NewDustPerfect(Player.Top + new Vector2(unitVector.X * Player.width, unitVector.Y * Player.width / 2f), ModContent.DustType<InvaderGlow>(), Vector2.UnitY * Player.height * 0.2f);
                        d.noGravity = true;
                        d.frame.Y = 0;
                        d.scale *= 2;
                    }

                }
                return true;
            }
            return false;
        }
	}
    public class InvaKuniDraw : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.JustDroppedAnItem)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            if(drawPlayer.TryGetModPlayer(out FightMode fightPlayer))
            {
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == ModContent.ItemType<FightKit>())
                {
                    Item item = drawPlayer.HeldItem;
                    Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Melee/Misc/FightKit/FightKit").Value;
                    if(fightPlayer.InvaKuniExtend > 0)
                    {
                        texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Melee/Misc/FightKit/ExtendedInvaKuni").Value;
                        DrawData value = new DrawData(texture, fightPlayer.InvaKuniPos - Main.screenPosition, new Rectangle(0, fightPlayer.InvaKuniExtend == 2 ? FightMode.KuniSpriteWidth : 0, FightMode.HighExtendLegth, FightMode.KuniSpriteWidth), color12, fightPlayer.InvaKuniRot, new Vector2(FightMode.KuniHoldAt, FightMode.KuniSpriteWidth / 2), item.scale, SpriteEffects.None, 0);
                        drawInfo.DrawDataCache.Add(value);
                    }
                    else
                    {
                        DrawData value = new DrawData(texture, fightPlayer.InvaKuniPos - Main.screenPosition, new Rectangle(0, 0, FightMode.KuniTotalLength, FightMode.KuniSpriteWidth), color12, fightPlayer.InvaKuniRot, new Vector2(FightMode.KuniHoldAt, FightMode.KuniSpriteWidth / 2), item.scale, SpriteEffects.None, 0);
                        drawInfo.DrawDataCache.Add(value);
                    }
                    
                }
            }
        }
    }
    public class EtimsKuniDraw : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.JustDroppedAnItem)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            if(drawPlayer.TryGetModPlayer(out FightMode fightPlayer))
            {
                Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
                if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == ModContent.ItemType<FightKit>())
                {
                    Item item = drawPlayer.HeldItem;
                    Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Melee/Misc/FightKit/FightKit").Value;
                    DrawData value = new DrawData(texture, fightPlayer.EtimsKuniPos - Main.screenPosition, new Rectangle(0, FightMode.KuniSpriteWidth, FightMode.KuniTotalLength, FightMode.KuniSpriteWidth), color12, fightPlayer.EtimsKuniRot, new Vector2(FightMode.KuniHoldAt, FightMode.KuniSpriteWidth / 2), item.scale * fightPlayer.EtimsKuniScale, SpriteEffects.None, 0);
                    drawInfo.DrawDataCache.Add(value);
                }
            }
        }
    }
}