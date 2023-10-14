using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

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
                if (player.HasBuff(ModContent.BuffType<MorphCooldown>()))
                {
                    if(player.GetModPlayer<ShapeShifterPlayer>().ruthlessMorhphing > 0)
                    {
                        int ouchAmt = (int)(3f * player.GetModPlayer<ShapeShifterPlayer>().morphCooldownTime / 60f);
                        CombatText.NewText(player.getRect(), CombatText.DamagedFriendly, ouchAmt, false);
                        player.statLife -= ouchAmt;
                        if(player.statLife < 0)
                        {
                            player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " morphed into nothing!"), ouchAmt, 0);
                        }
                        return true;
                    }
                    return false;
                }
            }
            return base.CanUseItem(item, player);
        }

        public override void UseAnimation(Item item, Player player)
        {
            if (morphCooldown != -1 && !item.IsAir)
            {
                player.AddBuff(ModContent.BuffType<MorphCooldown>(), (int)((morphCooldown * PrefixorphCooldownModifier * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) * 60f));
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (morphCooldown != -1)
            {
                int KBIndex = tooltips.FindIndex(TooltipLine => TooltipLine.Name.Equals("Knockback"));
                TooltipLine line = new TooltipLine(Mod, "MorphCool", (morphCooldown * PrefixorphCooldownModifier * Main.LocalPlayer.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) + Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipMorphCooldown")));
                {
                    line.OverrideColor = Color.Orange;
                    int insertIndex = (int)MathHelper.Min(tooltips.Count, KBIndex + 3);
                    tooltips.Insert(insertIndex, line);
                }
                //line.Text = (item.GetGlobalItem<ShapeShifterItem>().morphCooldown * PrefixorphCooldownModifier * Main.LocalPlayer.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) + Language.GetTextValue("Mods.QwertysRandomContent.Morphcooldown");
            }
        }
        
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if(morphCooldown != -1)
            {
                int num = 0;
                int num10 = rand.Next(14);
                if (num10 == 0)
                {
                    num = 36;
                }
                if (num10 == 1)
                {
                    num = 37;
                }
                if (num10 == 2)
                {
                    num = 38;
                }
                if (num10 == 3)
                {
                    num = 53;
                }
                if (num10 == 4)
                {
                    num = 54;
                }
                if (num10 == 5)
                {
                    num = 55;
                }
                if (num10 == 6)
                {
                    num = 39;
                }
                if (num10 == 7)
                {
                    num = 40;
                }
                if (num10 == 8)
                {
                    num = 56;
                }
                if (num10 == 9)
                {
                    num = 41;
                }
                if (num10 == 10)
                {
                    num = 57;
                }
                if (num10 == 11)
                {
                    num = 59;
                }
                if (num10 == 12)
                {
                    num = 60;
                }
                if (num10 == 13)
                {
                    num = 61;
                }
                return num;
            }
            return base.ChoosePrefix(item, rand);
        }
    }
}
