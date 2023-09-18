using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Lune
{
    [AutoloadEquip(EquipType.Body)]
    public class LuneCrestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 30000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 26;
            Item.height = 18;
            Item.defense = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<LuneBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].CountsAsClass(DamageClass.Ranged) && Main.projectile[i].owner == player.whoAmI && !Main.projectile[i].GetGlobalProjectile<LunePierce>().lunePierce)
                {
                    Main.projectile[i].GetGlobalProjectile<LunePierce>().lunePierce = true;
                }
            }
        }

        public override bool IsVanitySet(int head, int body, int legs)
        {
            return base.IsVanitySet(head, body, legs);
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ItemType<LuneHat>() && legs.type == ItemType<LuneLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            //Main.NewText("active set effect");
            if (!Main.dayTime)
            {
                float radius = Main.rand.NextFloat(200);
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(player.Center + QwertyMethods.PolarVector(radius, theta), DustType<LuneDust>());
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<crestSet>().setBonus = true;
            player.setBonus = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (String key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                player.setBonus = "Set Bonus: Shoot the moon!" + "\nPress the " + key + " key to summon a moon" + "\nRanged attacks shot through the moon will be boosted";
            }
        }

    }

    public class LunePierce : GlobalProjectile
    {
        public bool lunePierce;
        public bool runOnce = true;
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (lunePierce && projectile.CountsAsClass(DamageClass.Ranged))
            {
                if (runOnce)
                {
                    if (projectile.penetrate > 0 && projectile.type != ProjectileID.ChlorophyteBullet)
                    {
                        if (!projectile.usesLocalNPCImmunity && projectile.penetrate == 1)
                        {
                            projectile.localNPCHitCooldown = -10;
                            projectile.usesLocalNPCImmunity = true;
                        }
                        projectile.penetrate += 1;
                    }

                    runOnce = false;
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.localNPCHitCooldown == -10)
            {
                projectile.localNPCImmunity[target.whoAmI] = 30;
                target.immune[projectile.owner] = 0;
            }
        }
    }

    public class crestSet : ModPlayer
    {
        public bool setBonus = false;

        public override void ResetEffects()
        {
            setBonus = false;
        }

        public bool justSummonedMoon;

        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            justSummonedMoon = false;
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus)
                {
                    if (Player.HasBuff(BuffType<MoonCooldown>()))
                    {
                    }
                    else
                    {
                        Projectile.NewProjectile(new EntitySource_Misc("SetBouns_Lune"), Main.MouseWorld, new Vector2(0, 0), ProjectileType<MoonTarget>(), 0, 0, Player.whoAmI, 0, 0);
                        Player.AddBuff(BuffType<MoonCooldown>(), 3 * 60);
                        justSummonedMoon = true;
                    }
                }
            }
        }
    }

    public class MoonTarget : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 60 * 15;
            Projectile.light = 1;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
            behindProjectiles.Add(index);
        }

        private int timer;

        private int shader;
        private bool runOnce = true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                shader = player.ArmorSetDye();
                //Main.NewText(shader);
                runOnce = false;
            }
            //Main.NewText(Projectile.timeLeft + ", " + player.ArmorSetDye());
            //Projectile.rotation += MathF.PI / 30;
            timer++;
            if ((timer > 10 && player.GetModPlayer<crestSet>().justSummonedMoon) || !player.GetModPlayer<crestSet>().setBonus)
            {
                Projectile.Kill();
            }
            Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<LuneDust>())];

            dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            for (int i = 0; i < 200; i++)
            {
                if (Main.projectile[i].CountsAsClass(DamageClass.Ranged) && Main.projectile[i].owner == Projectile.owner && Collision.CheckAABBvAABBCollision(Main.projectile[i].position, new Vector2(Main.projectile[i].width, Main.projectile[i].height), Projectile.position, new Vector2(Projectile.width, Projectile.height)))
                {
                    Main.projectile[i].GetGlobalProjectile<moonBoost>().boosted = true;
                }
            }
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

            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            return true;
        }
    }

    public class moonBoost : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool boosted;
        private bool runOnce = true;

        public override void AI(Projectile projectile)
        {
            if (boosted)
            {
                if (runOnce)
                {
                    //Main.NewText("Boost!");
                    projectile.damage = (int)(projectile.damage * 1.2f);
                    //Projectile.velocity *= 2;
                    runOnce = false;
                }
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<LuneDust>())];
                Player player = Main.player[projectile.owner];
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (boosted)
            {
                target.AddBuff(BuffType<LuneCurse>(), 60 * 3);
            }
        }
    }
}