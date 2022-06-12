using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class AmuletOfPatience : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amulet Of Patience");
            Tooltip.SetDefault("Deal more damage if you do haven't dealt damage in a while");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = 2;

            Item.width = 14;
            Item.height = 18;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AmuletOfPatienceEffect>().effect = true;
            if (!hideVisual && player.GetModPlayer<AmuletOfPatienceEffect>().patienceCount == 180 && Main.rand.Next(6) == 0)
            {
                Dust d = Dust.NewDustPerfect(player.Center + new Vector2((2 * player.direction) + (player.direction == -1 ? -1 : 0), 0), 172);
                d.noGravity = true;
                d.velocity *= .1f;
                d.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
        }
    }

    public class AmuletOfPatienceEffect : ModPlayer
    {
        public bool effect = false;
        public int patienceCount;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void PreUpdate()
        {
            if (effect && patienceCount < 180)
            {
                patienceCount++;
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (patienceCount > 60)
            {
                damage += (int)((float)damage * (((float)patienceCount - 60) / 60f));
            }
            patienceCount = 0;
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (patienceCount > 60)
            {
                damage += (int)((float)damage * (((float)patienceCount - 60) / 60f));
            }
            patienceCount = 0;
        }
    }
}