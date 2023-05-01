using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameInput;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace QwertyMod.Content.Items.Equipment.Armor.Hero
{
    //[AutoloadEquip(EquipType.HandsOn, EquipType.Shield)]
    public class HeroShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Blue;
            Item.width = 28;
            Item.height = 22;
            Item.defense = 6;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Melee) += 8;
            player.GetModPlayer<HeroAbilities>().shieldEquiped = true;
        }
    }
    public class HeroAbilities : ModPlayer
    {
        public bool setBonus = false;
        public Projectile shieldThrown = null;
        public bool shieldEquiped = false;
        bool useAltHandOn = false;
        int throwShieldTimer = 0;
        public int strikeDown = 0;
        bool[] hitList = new bool[Main.npc.Length];
        public override void ResetEffects()
        {
            setBonus = false;
            //shieldThrown = false;
            shieldEquiped = false;
            useAltHandOn = false;
        }
        /*
        public override bool? CanHitNPCWithItem(Item item, NPC target)
        {
            if(strikeDown>0)
            {
                return false;
            }
            return null;
        }
        */
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers hitModifiers)
        {
            if(strikeDown>0)
            {
                hitModifiers.FinalDamage *= 10;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus && shieldThrown == null && Player.itemAnimation == 0 && ValidWeapon(Player.HeldItem))
                {
                    throwShieldTimer = 30;
                    shieldThrown = Main.projectile[Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ThrowShield>(), 0, 0, Player.whoAmI)];
                }
            }
        }
        public override bool CanUseItem(Item item)
        {
            if(shieldThrown != null)
            {
                return false;
            }
            return true;
        }
        public override void PostItemCheck()
        {
            if(strikeDown > 0 && ValidWeapon(Player.HeldItem))
            {
                Player.armorEffectDrawShadow = true;
                Player.itemAnimationMax = 30;
                Player.itemAnimation = 2;
                if(strikeDown > 1 && Player.HeldItem.noMelee)
                {
                    float swordLength = Player.HeldItem.Size.Length() * Player.GetAdjustedItemScale(Player.HeldItem) * 2;
                    //Player.itemRotation += -MathF.PI / 8f * Player.direction;
                    //Dust.NewDustPerfect(Player.itemLocation, DustID.Torch, Vector2.Zero);
                    //Dust.NewDustPerfect(Player.itemLocation + QwertyMethods.PolarVector(swordLength, Player.itemRotation + (Player.direction == 1 ? -MathF.PI / 4f : 5f * MathF.PI / 4f)), DustID.Torch, Vector2.Zero);
                    for (int n = 0; n < Main.npc.Length; n++)
                    {
                        if (!hitList[n] && Main.npc[n].active && !Main.npc[n].dontTakeDamage && (!Main.npc[n].friendly || (Main.npc[n].type == NPCID.Guide && Player.killGuide) || (Main.npc[n].type == NPCID.Clothier && Player.killClothier)) && Player.itemAnimation > 0 && Collision.CheckAABBvLineCollision(Main.npc[n].position, Main.npc[n].Size, Player.itemLocation, Player.itemLocation + QwertyMethods.PolarVector(swordLength, Player.itemRotation + (Player.direction == 1 ? -MathF.PI / 4f : 5f * MathF.PI / 4f))))
                        {
                            hitList[n] = true;
                            Projectile p = QwertyMethods.PokeNPC(Player, Main.npc[n], Player.GetSource_ItemUse(Player.HeldItem), Player.GetWeaponDamage(Player.HeldItem) * 10, DamageClass.Melee, Player.GetWeaponKnockback(Player.HeldItem));
                        }
                    }
                }
                else
                {
                    strikeDown++;
                }
            }
            else
            {
                strikeDown = 0;
                for (int n = 0; n < Main.npc.Length; n++)
                {
                    hitList[n] = false;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if(shieldEquiped && Player.armor[1].type == ModContent.ItemType<HeroPlate>() && Player.armor[2].type == ModContent.ItemType<HeroPants>())
            {
                setBonus = true;
                string s = "Please go to conrols and bind the 'Yet another special ability key'";
                foreach (string key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
                {
                    s = "Press " + key + " to perform 'The Hero Crit'.";
                }
                Player.setBonus = s;
            }
            if((Player.velocity.Y == 0 || shieldThrown == null || Player.velocity.Y < 0) && strikeDown > 0)
            {
                if(shieldThrown != null)
                {
                    shieldThrown.timeLeft = ThrowShield.endAnimation;
                }
                strikeDown = 0;
                for (int n = 0; n < Main.npc.Length; n++)
                {
                    hitList[n] = false;
                }
            }
        }
        public override void PostUpdate()
        {
            throwShieldTimer--;
            if(Player.shield == QwertyMod.HeroShieldShield && ((Player.bodyFrame.Y <= 0 || Player.bodyFrame.Y > 4 * Player.bodyFrame.Height)))
            {
                Player.shield = -1;
                if(Player.shieldRaised)
                {
                    Player.shield = QwertyMod.HeroShieldShieldUp;
                }
            }
            if(Player.handon == QwertyMod.HeroShieldHandOn && (Player.shieldRaised || (Player.bodyFrame.Y > 0 && Player.bodyFrame.Y < 4 * Player.bodyFrame.Height)))
            {
                Player.handon = -1;
                useAltHandOn = true;
            }
            if(shieldThrown != null)
            {
                if(!shieldThrown.active)
                {
                    shieldThrown = null;
                }
            }
            if(shieldThrown != null)
            {
                if(!ValidWeapon(Player.HeldItem) && shieldThrown.timeLeft > ThrowShield.endAnimation)
                {
                    shieldThrown.timeLeft = ThrowShield.endAnimation;
                }
                if(shieldThrown.velocity.Y > 0 && strikeDown == 0 && shieldThrown.timeLeft > ThrowShield.endAnimation)
                {
                    Player.armorEffectDrawShadow = true;
                    Player.mount.Dismount(Player);
                    Player.velocity = (shieldThrown.Center - Player.Center).SafeNormalize(Vector2.UnitY) * 20f;
                    Player.direction = Math.Sign(Player.velocity.X);
                    Player.legFrame.Y = 14 * Player.legFrame.Height;
                    if((Player.Center - shieldThrown.Center).Length() < 20f)
                    {
                        shieldThrown.velocity = Vector2.UnitX * MathF.Sign(shieldThrown.velocity.X) * 0.1f;
                        strikeDown = 1;
                        Player.velocity = Vector2.UnitY * 20;
                    }
                }
                else if(strikeDown == 0)
                {
                    Player.direction = MathF.Sign((throwShieldTimer > -1 ? Main.MouseWorld.X : shieldThrown.Center.X) - Player.Center.X);
                    if(throwShieldTimer > 20)
                    {
                        int time = throwShieldTimer - 20;
                        float backAmt = 1f - (float)time / 10f;
                        Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, backAmt * MathF.PI / 8f * Player.direction);
                    }
                    else
                    {
                        int locTimer = throwShieldTimer;
                        if(locTimer < 0)
                        {
                            locTimer = 0;
                        }
                        float backAmt = 1f - (float)locTimer / 20f;
                        Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, backAmt * -5f * MathF.PI / 8f * Player.direction);
                    }
                }
                if(Player.shield == QwertyMod.HeroShieldShield)
                {
                    Player.shield = -1;
                }
                if(Player.handon == QwertyMod.HeroShieldHandOn)
                {
                    Player.handon = -1;
                    useAltHandOn = true;
                }
                
            }
            if(useAltHandOn)
            {
                for(int i = 3; i < 10; i++)
                {
                    if(!Player.hideVisibleAccessory[i] && Player.armor[i].type != ModContent.ItemType<HeroShield>() && Player.armor[i].handOnSlot != -1)
                    {
                        Player.handon = Player.armor[i].handOnSlot;
                        Player.cHandOn = Player.dye[i].dye;
                    }
                }
                for(int i = 13; i < 20; i++)
                {
                    if(Player.armor[i].type != ModContent.ItemType<HeroShield>() && Player.armor[i].handOnSlot != -1)
                    {
                        Player.handon = Player.armor[i].handOnSlot;
                        Player.cHandOn = Player.dye[i-10].dye;
                    }
                }
            }
        }
        bool ValidWeapon(Item heldItem)
        {
            return heldItem.useStyle == ItemUseStyleID.Swing && heldItem.damage > 0 && !heldItem.noUseGraphic;
        }
    }
    public class ThrowShield : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 22;
            Projectile.friendly = false;
        }

        private int shader;
        int thrownCount = 30;
        public const int endAnimation = 5;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            thrownCount--;
            if(Projectile.timeLeft < endAnimation)
            {
                Projectile.rotation = (player.Center - Projectile.Center).ToRotation();
                if(player.Center.X < Projectile.Center.X)
                {
                    Projectile.rotation -= MathF.PI;
                }
                return;
            }
            if(thrownCount > 0)
            {
                shader = player.cShield;
                Projectile.Center = player.RotatedRelativePoint(player.MountedCenter + GetCompositeOffset_FrontArm(player));
                Projectile.rotation = player.compositeFrontArm.rotation;
                Projectile.spriteDirection = player.direction;
            }
            else if(thrownCount == 0)
            {
                Projectile.velocity = new Vector2(4f * player.direction, -8f);
            }
            else
            {
                Projectile.velocity.Y += 0.2f;
                Projectile.rotation -= MathF.PI / 15f * MathF.Sign(Projectile.velocity.X);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.timeLeft = endAnimation;
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = (player.Center - Projectile.Center).ToRotation();
            if(player.Center.X < Projectile.Center.X)
            {
                Projectile.rotation -= MathF.PI;
            }
            return false;
        }
        public static Vector2 GetCompositeOffset_FrontArm(Player player)
        {
            return new Vector2(-5 * player.direction, 0f) + QwertyMethods.PolarVector(6f, player.compositeFrontArm.rotation + MathF.PI / 2f);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public override void PostDraw(Color drawColor)
        {
            // As mentioned above, be sure not to forget this step.
            Player player = Main.player[Projectile.owner];
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if(Projectile.timeLeft > endAnimation && player.GetModPlayer<HeroAbilities>().strikeDown <= 0)
            {
                Item weapon = player.HeldItem;
                Texture2D weaponTexture = TextureAssets.Item[weapon.type].Value;
                Main.EntitySpriteDraw(weaponTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + 0.75f * MathF.PI * -Projectile.spriteDirection, weaponTexture.Size() * 0.5f, 1f, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            if(Projectile.timeLeft < endAnimation - 1)
            {
                Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Hero/ThrowShieldStretch").Value;
                float dist = (player.MountedCenter + GetCompositeOffset_FrontArm(player) - Projectile.Center).Length();
                float rot = (Projectile.Center - player.Center).ToRotation();
                if(Projectile.timeLeft < endAnimation / 2)
                {
                    dist /= 2;
                }
                float stretch = (float)dist / texture.Width;
                Main.EntitySpriteDraw(texture, player.MountedCenter + GetCompositeOffset_FrontArm(player) + QwertyMethods.PolarVector(dist / 2f, rot) - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, new Vector2(stretch, 1f), Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                return false;
            }
            return true;
        }
    }
}