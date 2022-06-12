using QwertyMod.Content.Dusts;
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
namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Hydra
{
    public class HydraArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Arrow");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
            if (Projectile.owner == Main.myPlayer && Projectile.ai[1] == 0 && Projectile.timeLeft == 298)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(Projectile.velocity.Length(), Projectile.velocity.ToRotation() + (float)Math.PI / 8), Type, (int)((float)Projectile.damage * .5f), Projectile.knockBack * .5f, Projectile.owner, 1, 1);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(Projectile.velocity.Length(), Projectile.velocity.ToRotation() - (float)Math.PI / 8), Type, (int)((float)Projectile.damage * .5f), Projectile.knockBack * .5f, Projectile.owner, 1, 1);
                Projectile.ai[1] = 1;
            }
        }


    }
}
