using System;
using Terraria;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class BattleshipDebris_BackWithGun : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 84;
            Projectile.height = 72;
        }
        public override void AI()
        {
            Projectile.rotation -= Math.Sign(Projectile.velocity.X) * MathF.PI / 100f;
            Projectile.velocity.Y += 0.3f;
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
        }
    } 
    public class BattleshipDebris_Engine : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 127;
            Projectile.height = 88;
        }
        public override void AI()
        {
            Projectile.rotation -= Math.Sign(Projectile.velocity.X) * MathF.PI / 300f;
            Projectile.velocity.Y += 0.3f;
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
        }
    } 
    public class BattleshipDebris_Launcher : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 61;
            Projectile.height = 22;
        }
        public override void AI()
        {
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathF.PI / 60f;
            Projectile.velocity.Y += 0.3f;
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
        }
    } 
    public class BattleshipDebris_Center : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 90;
            Projectile.height = 92;
        }
        public override void AI()
        {
            Projectile.rotation += -Math.Sign(Projectile.velocity.X) * MathF.PI / 60f;
            Projectile.velocity.Y += 0.3f;
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
        }
    } 
    public class BattleshipDebris_FrontWithGun: ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.width = 98;
            Projectile.height = 92;
        }
        public override void AI()
        {
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathF.PI / 600f;
            Projectile.velocity.Y += 0.3f;
            Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
        }
    } 
}