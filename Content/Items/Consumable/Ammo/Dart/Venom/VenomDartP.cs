using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Venom
{
    public class VenomDartP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 10;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 7;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            int num67 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 171, 0f, 0f, 100);
            Main.dust[num67].scale = (float)Main.rand.Next(1, 10) * 0.1f;
            Main.dust[num67].noGravity = true;
            Main.dust[num67].fadeIn = 1.5f;
            Main.dust[num67].velocity *= 0.25f;
            Main.dust[num67].velocity += Projectile.velocity * 0.25f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            target.AddBuff(BuffID.Venom, 60 * 30);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(Main.rand.NextFloat(), Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)), ProjectileType<VenomCloud>(), (int)(.5f * Projectile.damage), Projectile.knockBack, Projectile.owner);
        }
    }
}
