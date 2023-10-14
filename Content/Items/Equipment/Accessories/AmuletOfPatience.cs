using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class AmuletOfPatience : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Green;
            Item.width = 14;
            Item.height = 18;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AmuletOfPatienceEffect>().effect = true;
            if (!hideVisual && player.GetModPlayer<AmuletOfPatienceEffect>().patienceCount == 180 && Main.rand.NextBool(6))
            {
                Dust d = Dust.NewDustPerfect(player.Center + new Vector2((2 * player.direction) + (player.direction == -1 ? -1 : 0), 0), DustID.DungeonWater);
                d.noGravity = true;
                d.velocity *= .1f;
                d.shader = GameShaders.Armor.GetSecondaryShader(player.cNeck, player);
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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (patienceCount > 60)
            {
                modifiers.FinalDamage *= 1 + (((float)patienceCount - 60) / 60f);
            }
            patienceCount = 0;
        }
    }
}