using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Tool.Mining
{
    public class AoePick : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public int miningRadius = 0;
    }

    public class SpecialPick : ModPlayer
    {
        public override void PostItemCheck()
        {
            if (!Player.inventory[Player.selectedItem].IsAir)
            {
                Item item = Player.inventory[Player.selectedItem];
                bool flag18 = Player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (Player.position.X + (float)Player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && Player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (Player.position.Y + (float)Player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY;
                if (Player.noBuilding)
                {
                    flag18 = false;
                }
                if (flag18)
                {
                    if (item.GetGlobalItem<AoePick>().miningRadius > 0)
                    {
                        if ((item.pick > 0 && !Main.tileAxe[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && !Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]) || (item.axe > 0 && Main.tileAxe[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]) || (item.hammer > 0 && Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]))
                        {
                        }
                        if (Player.toolTime == 0 && Player.itemAnimation > 0 && Player.controlUseItem)
                        {
                            int tileId = Player.hitTile.HitObject(Player.tileTargetX, Player.tileTargetY, 1);

                            if (item.pick > 0)
                            {
                                for (int i = -item.GetGlobalItem<AoePick>().miningRadius; i <= item.GetGlobalItem<AoePick>().miningRadius; i++)
                                {
                                    for (int j = -item.GetGlobalItem<AoePick>().miningRadius; j <= item.GetGlobalItem<AoePick>().miningRadius; j++)
                                    {
                                        if ((i != 0 || j != 0) && !Main.tileAxe[(int)Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].type] && !Main.tileHammer[(int)Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].type])
                                        {
                                            Player.PickTile(Player.tileTargetX + i, Player.tileTargetY + j, item.pick);
                                        }
                                    }
                                }

                                Player.itemTime = (int)((float)item.useTime * Player.pickSpeed);
                            }

                            {
                                Player.poundRelease = false;
                            }
                        }

                        if (Player.releaseUseItem)
                        {
                            Player.poundRelease = true;
                        }
                        int num263 = Player.tileTargetX;
                        int num264 = Player.tileTargetY;
                        bool flag24 = true;
                        if (Main.tile[num263, num264].wall > 0)
                        {
                            if (!Main.wallHouse[(int)Main.tile[num263, num264].wall])
                            {
                                for (int num265 = num263 - 1; num265 < num263 + 2; num265++)
                                {
                                    for (int num266 = num264 - 1; num266 < num264 + 2; num266++)
                                    {
                                        if (Main.tile[num265, num266].wall != Main.tile[num263, num264].wall)
                                        {
                                            flag24 = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                flag24 = false;
                            }
                        }
                        if (flag24 && !Main.tile[num263, num264].IsActive)
                        {
                            int num267 = -1;
                            if ((double)(((float)Main.mouseX + Main.screenPosition.X) / 16f) < Math.Round((double)(((float)Main.mouseX + Main.screenPosition.X) / 16f)))
                            {
                                num267 = 0;
                            }
                            int num268 = -1;
                            if ((double)(((float)Main.mouseY + Main.screenPosition.Y) / 16f) < Math.Round((double)(((float)Main.mouseY + Main.screenPosition.Y) / 16f)))
                            {
                                num268 = 0;
                            }
                            for (int num269 = Player.tileTargetX + num267; num269 <= Player.tileTargetX + num267 + 1; num269++)
                            {
                                for (int num270 = Player.tileTargetY + num268; num270 <= Player.tileTargetY + num268 + 1; num270++)
                                {
                                    if (flag24)
                                    {
                                        num263 = num269;
                                        num264 = num270;
                                        if (Main.tile[num263, num264].wall > 0)
                                        {
                                            if (!Main.wallHouse[(int)Main.tile[num263, num264].wall])
                                            {
                                                for (int num271 = num263 - 1; num271 < num263 + 2; num271++)
                                                {
                                                    for (int num272 = num264 - 1; num272 < num264 + 2; num272++)
                                                    {
                                                        if (Main.tile[num271, num272].wall != Main.tile[num263, num264].wall)
                                                        {
                                                            flag24 = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                flag24 = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
