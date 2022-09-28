using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Buffs
{
    public class Darkness : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkness");
            Description.SetDefault("You knew this was coming...");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<CommonStats>().Darkened = true;
            Projectile.NewProjectile(new EntitySource_Buff(player, Type, buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<DarknessBorder>(), 0, 0);
        }
    }
    public class DarknessBorder : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.timeLeft = 2;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void AI()
        {
            if(Projectile.timeLeft > 2)
            {
                Projectile.timeLeft = 2;
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D TheShadow = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(TheShadow, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(Main.screenWidth / 1000f, Main.screenHeight / 1000f), SpriteEffects.None, 0);

            return false;
        }
    }
}