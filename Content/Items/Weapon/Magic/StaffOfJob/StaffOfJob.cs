using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.StaffOfJob
{
    public class StaffOfJob : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.width = Item.height = 48;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 10;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 30 : 10;
            Item.shootSpeed = 1f;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.Orange;
            Item.value = 120000;
        }



        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            NPC target = new NPC();
            if (QwertyMethods.ClosestNPC(ref target, 100, Main.MouseWorld, true))
            {
                target.GetGlobalNPC<GraveMisery>().MiseryIntensity = (damage);
                target.GetGlobalNPC<GraveMisery>().MiseryTime = ModLoader.HasMod("TRAEProject") ? 60 * 60 : 4 * 60;
                target.GetGlobalNPC<GraveMisery>().MiseryCauser = player.whoAmI;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<Etims>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class GraveMisery : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int MiseryIntensity = 0;
        public int MiseryTime = 0;
        public int MiseryCauser = 0;
        int miseryCounter = 0;

        private float trigCounter = 0f;

        public override void AI(NPC npc)
        {
            if (MiseryTime > 0)
            {
                MiseryTime--;
                trigCounter += 250 * MathF.PI / (60f * 240f);
                miseryCounter++;
                if(ModLoader.HasMod("TRAEProject") && (int)(Main.player[MiseryCauser].manaCost * 4) > Main.player[MiseryCauser].statMana)
                {
                    MiseryTime = 0;
                    return;
                }
                if (miseryCounter % 6 == 0)
                {
                    if(ModLoader.HasMod("TRAEProject"))
                    {
                        Main.player[MiseryCauser].statMana -= (int)(Main.player[MiseryCauser].manaCost * 4);
                    }
                    QwertyMethods.PokeNPC(Main.player[MiseryCauser], npc, Main.player[MiseryCauser].GetSource_ItemUse(Main.player[MiseryCauser].HeldItem), MiseryIntensity, DamageClass.Magic, 0, 20);
                }
            }
            else
            {
                MiseryTime = 0;
                MiseryIntensity = 0;
                MiseryCauser = 0;
            }
        }

        public Texture2D DrawCurve(int width, int height, float shift, bool increasing)
        {
            if (Math.Sin(trigCounter + shift) < 0)
            {
                increasing = !increasing;
            }
            if (Math.Cos(trigCounter + shift) < 0)
            {
                increasing = !increasing;
            }
            height = (int)(height * Math.Abs(Math.Sin(trigCounter + shift)));
            width /= 2;
            height /= 2;
            if (width % 2 == 0)
            {
                width++;
            }
            if (height % 2 == 0)
            {
                height++;
            }

            int major = Math.Max(height, width);
            int minor = Math.Min(height, width);
            int semiMajor = (major - 1) / 2;
            int semiMinor = (minor - 1) / 2;
            if (major != 0 && minor != 0 && semiMajor != 0 && semiMinor != 0)
            {
                Texture2D curve = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                Color[] dataColors = new Color[width * height];
                for (int x = 0; x < width; x++)
                {
                    int y = (int)(((float)semiMinor / semiMajor) * Math.Sqrt((semiMajor * semiMajor) - ((x - width / 2) * (x - width / 2))));
                    dataColors[x + (height / 2 + y * (increasing ? 1 : -1)) * width] = new Color(122, 24, 24);
                    // dataColors[x + (height / 2 + y) * width] = new Color(122, 24, 24);
                }
                curve.SetData(0, null, dataColors, 0, width * height);
                return curve;
            }
            return new Texture2D(Main.graphics.GraphicsDevice, 1, 1); ;
        }

        private int painRings = 3;

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (MiseryIntensity > 0)
            {
                for (int i = 0; i < painRings; i++)
                {
                    Texture2D curve = DrawCurve(npc.width + 50, npc.height + 50, i * (MathF.PI) / painRings, false);
                    spriteBatch.Draw(curve, npc.Center - screenPos,
                           curve.Frame(), Color.White, 0f,
                           curve.Size() * .5f, 2f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (MiseryIntensity > 0)
            {
                for (int i = 0; i < painRings; i++)
                {
                    Texture2D curve = DrawCurve(npc.width + 50, npc.height + 50, i * (MathF.PI) / painRings, true);
                    spriteBatch.Draw(curve, npc.Center - screenPos,
                           curve.Frame(), Color.White, 0f,
                           curve.Size() * .5f, 2f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}