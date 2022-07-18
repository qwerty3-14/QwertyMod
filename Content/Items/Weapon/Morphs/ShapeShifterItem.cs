using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Morphs
{
    class ShapeShifterItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public int morphCooldown = -1;
        float PrefixorphCooldownModifier = 1f;
        public override bool CanUseItem(Item item, Player player)
        {
            if (morphCooldown != -1 && !item.IsAir)
            {
                if (player.HasBuff(BuffType<MorphCooldown>()))
                {
                    return false;
                }
            }
            return base.CanUseItem(item, player);
        }

        public override void UseAnimation(Item item, Player player)
        {
            if (morphCooldown != -1 && !item.IsAir)
            {
                player.AddBuff(BuffType<MorphCooldown>(), (int)((morphCooldown * PrefixorphCooldownModifier * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) * 60f));
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (morphCooldown != -1)
            {
                int KBIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("Knockback"));
                TooltipLine line = new TooltipLine(Mod, "MorphCool", (morphCooldown * PrefixorphCooldownModifier * Main.LocalPlayer.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) + " second cooldown");
                {
                    line.OverrideColor = Color.Orange;
                    int insertIndex = (int)MathHelper.Min(tooltips.Count, KBIndex + 3);
                    tooltips.Insert(insertIndex, line);
                }
                //line.Text = (item.GetGlobalItem<ShapeShifterItem>().morphCooldown * PrefixorphCooldownModifier * Main.LocalPlayer.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) + Language.GetTextValue("Mods.QwertysRandomContent.Morphcooldown");
            }
        }
    }
}
