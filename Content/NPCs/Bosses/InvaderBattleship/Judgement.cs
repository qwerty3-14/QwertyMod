using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.MiscMaterials;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class Judgement : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public const int loadInTime = 10;
        public const int punchTime = 10;
        public const int fadeOut = 10;
        public const float spawnDist = 100;
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 48;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = punchTime + loadInTime + fadeOut;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }
        bool runOnce = true;
        int timer = 0;
        public override void AI()
        {
            if(runOnce)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
                runOnce = false;
            }
            if(Projectile.timeLeft > punchTime + fadeOut)
            {
                Projectile.alpha = 255 - (int)(255 * (1f - ((float)(Projectile.timeLeft - (punchTime + fadeOut)) / loadInTime)));
            }
            else if (Projectile.timeLeft > fadeOut)
            {
                Projectile.velocity = QwertyMethods.PolarVector((float)spawnDist / punchTime,  Projectile.rotation);
            }
            else if(Projectile.timeLeft == fadeOut)
            {
                Projectile.velocity = Vector2.Zero;
                SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/PUNCH"), Projectile.Center);
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.alpha = 255 - (int)(255 * ((float)(Projectile.timeLeft) / fadeOut));
            }
        }
    }
    public class FinalJudgement : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public const int loadInTime = 10;
        public const int punchTime = 10;
        public const int fadeOut = 60;
        public const float spawnDist = 200;
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 96;
            Projectile.height = 80;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = punchTime + loadInTime + fadeOut;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }
        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 30; i++)
            {
                Item.NewItem(Projectile.GetSource_FromAI(), Projectile.position, Projectile.Size, ModContent.ItemType<SoulOfHeight>(), 1);
            }
        }
        bool runOnce = true;
        int timer = 0;
        public override void AI()
        {
            if(runOnce)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
                runOnce = false;
            }
            if(Projectile.timeLeft > punchTime + fadeOut)
            {
                Projectile.alpha = 255 - (int)(255 * (1f - ((float)(Projectile.timeLeft - (punchTime + fadeOut)) / loadInTime)));
            }
            else if (Projectile.timeLeft > fadeOut)
            {
                Projectile.velocity = QwertyMethods.PolarVector((float)spawnDist / punchTime,  Projectile.rotation);
            }
            else if(Projectile.timeLeft == fadeOut)
            {
                Projectile.velocity = Vector2.Zero;
                SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/PUNCH"), Projectile.Center);
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.alpha = 255 - (int)(255 * ((float)(Projectile.timeLeft) / fadeOut));
            }
        }
    }
}