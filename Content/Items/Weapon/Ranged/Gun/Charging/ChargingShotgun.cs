using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.Charging
{
    public class ChargingShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Charging Shotgun");
            Tooltip.SetDefault("Right click to add an extra bullet to your next fire");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.value = 500000;
            Item.rare = 9;
            Item.UseSound = SoundID.Item11;

            Item.width = 56;
            Item.height = 34;

            Item.shoot = 97;
            Item.useAmmo = 97;
            Item.shootSpeed = 6f;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public int numberProjectiles = 1;
        public float colorProgress = .02f;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = 0;
                Item.useAmmo = 0;
                Item.useTime = 12;
                Item.useAnimation = 12;
                Item.UseSound = new SoundStyle("QwertyMod/Assets/Sounds/click", SoundType.Sound);
                numberProjectiles++;
                if (numberProjectiles > 50)
                {
                    numberProjectiles = 50;
                    CombatText.NewText(player.getRect(), new Color(colorProgress, colorProgress, colorProgress), "MAX!", true, false);
                }
                else
                {
                    colorProgress += .02f;
                    CombatText.NewText(player.getRect(), new Color(colorProgress, colorProgress, colorProgress), numberProjectiles, true, false);
                }
            }
            else
            {
                Item.shoot = 97;
                Item.useAmmo = 97;

                Item.useTime = 12;
                Item.useAnimation = 12;
                if (numberProjectiles > 1)
                {
                    Item.UseSound = SoundID.Item38;
                }
                else
                {
                    Item.UseSound = SoundID.Item11;
                }
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (player.altFunctionUse == 2)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 trueSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                    float scale = Main.rand.NextFloat(.8f, 1.6f);
                    trueSpeed *= scale;

                    float shellShift = MathHelper.ToRadians(-50);
                    float SVar = shellShift + MathHelper.ToRadians(Main.rand.Next(-100, 301) / 10);
                    float Sspeed = .05f * Main.rand.Next(15, 41);
                    Projectile.NewProjectile(source, position, new Vector2((float)Math.Cos(SVar) * Sspeed * -player.direction, (float)Math.Sin(SVar) * Sspeed), ModContent.ProjectileType<DinoVulcan.Shell>(), 0, 0, Main.myPlayer);
                    Projectile.NewProjectile(source, position, trueSpeed, type, damage, knockback, player.whoAmI);
                }
                colorProgress = .02f;
                numberProjectiles = 1;
                return false;
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, -0);
        }
    }
}