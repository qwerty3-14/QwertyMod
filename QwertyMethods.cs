using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod
{
    public static class QwertyMethods
    {
        public static float SlowRotation(float currentRotation, float targetAngle, float speed)
        {
            int f = 1; //this is used to switch rotation direction
            float actDirection = new Vector2(MathF.Cos(currentRotation), MathF.Sin(currentRotation)).ToRotation();
            targetAngle = new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)).ToRotation();

            //this makes f 1 or -1 to rotate the shorter distance
            if (Math.Abs(actDirection - targetAngle) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }

            if (actDirection <= targetAngle + MathHelper.ToRadians(speed * 2) && actDirection >= targetAngle - MathHelper.ToRadians(speed * 2))
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += MathHelper.ToRadians(speed) * f;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= MathHelper.ToRadians(speed) * f;
            }
            actDirection = new Vector2(MathF.Cos(actDirection), MathF.Sin(actDirection)).ToRotation();

            return actDirection;
        }
        public static Vector2 PolarVector(float radius, float theta)
        {
            return new Vector2(MathF.Cos(theta), MathF.Sin(theta)) * radius;
        }

        public delegate bool SpecialCondition(NPC possibleTarget);

        //used for homing projectile
        public static bool ClosestNPC(ref NPC target, float maxDistance, Vector2 position, bool ignoreTiles = false, int overrideTarget = -1, SpecialCondition specialCondition = null)
        {
            //very advance users can use a delegate to insert special condition into the function like only targetting enemies not currently having local iFrames, but if a special condition isn't added then just return it true
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }
            bool foundTarget = false;
            //If you want to prioritse a certain target this is where it's processed, mostly used by minions that haave a target priority
            if (overrideTarget != -1)
            {
                if ((Main.npc[overrideTarget].Center - position).Length() < maxDistance && !Main.npc[overrideTarget].immortal && (Collision.CanHit(position, 0, 0, Main.npc[overrideTarget].Center, 0, 0) || ignoreTiles) && specialCondition(Main.npc[overrideTarget]))
                {
                    target = Main.npc[overrideTarget];
                    return true;
                }
            }
            //this is the meat of the targetting logic, it loops through every NPC to check if it is valid the miniomum distance and target selected are updated so that the closest valid NPC is selected
            for (int k = 0; k < Main.npc.Length; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();
                if (distance < maxDistance && possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0) || ignoreTiles) && specialCondition(possibleTarget))
                {
                    target = Main.npc[k];
                    foundTarget = true;

                    maxDistance = (target.Center - position).Length();
                }
            }
            return foundTarget;
        }

        public static bool NPCsInRange(ref List<NPC> targets, float maxDistance, Vector2 position, bool ignoreTiles = false, SpecialCondition specialCondition = null)
        {
            //very advance users can use a delegate to insert special condition into the function like only targetting enemies not currently having local iFrames, but if a special condition isn't added then just return it true
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }
            bool foundTarget = false;
            targets = new List<NPC>();
            for (int k = 0; k < Main.npc.Length; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();
                if (distance < maxDistance && possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0) || ignoreTiles) && specialCondition(possibleTarget))
                {
                    targets.Add(possibleTarget);
                    foundTarget = true;
                }
            }
            return foundTarget;
        }


        public static bool SlowRotation(this ref float currentRotation, float targetAngle, float speed)
        {
            int f = 1; //this is used to switch rotation direction
            float actDirection = new Vector2(MathF.Cos(currentRotation), MathF.Sin(currentRotation)).ToRotation();
            targetAngle = new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)).ToRotation();

            //this makes f 1 or -1 to rotate the shorter distance
            if (Math.Abs(actDirection - targetAngle) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }

            if (actDirection <= targetAngle + speed * 2 && actDirection >= targetAngle - speed * 2)
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += speed * f;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= speed * f;
            }
            actDirection = new Vector2(MathF.Cos(actDirection), MathF.Sin(actDirection)).ToRotation();
            currentRotation = actDirection;

            return currentRotation == targetAngle;
        }
        public static void SlowRotWhileAvoid(this ref float currentRotation, float targetAngle, float speed, float avoid)
        {
            float actDirection = new Vector2(MathF.Cos(currentRotation), MathF.Sin(currentRotation)).ToRotation();
            targetAngle = new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)).ToRotation();
            avoid = new Vector2(MathF.Cos(avoid), MathF.Sin(avoid)).ToRotation();


            if (actDirection < 0)
            {
                actDirection += MathF.PI * 2;
            }
            if (targetAngle < 0)
            {
                targetAngle += MathF.PI * 2;
            }

            actDirection -= avoid;
            targetAngle -= avoid;

            actDirection = new Vector2(MathF.Cos(actDirection), MathF.Sin(actDirection)).ToRotation();
            targetAngle = new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)).ToRotation();

            if (actDirection < 0)
            {
                actDirection += MathF.PI * 2;
            }
            if (targetAngle < 0)
            {
                targetAngle += MathF.PI * 2;
            }

            if (actDirection <= targetAngle + speed * 2 && actDirection >= targetAngle - speed * 2)
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += speed;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= speed;
            }

            actDirection += avoid;

            actDirection = new Vector2(MathF.Cos(actDirection), MathF.Sin(actDirection)).ToRotation();
            currentRotation = actDirection;

        }
        public static Vector2 MoveToward(this Vector2 currentPosition, Vector2 here, float speed)
        {
            Vector2 dif = here - currentPosition;
            if (dif.Length() < speed)
            {
                return dif;
            }
            else
            {
                return dif.SafeNormalize(-Vector2.UnitY) * speed;
            }
        }
        //give an angle to shoot at to attempt to hit a moving target, returns NaN when this is impossible
        public static float PredictiveAim(Vector2 shootFrom, float shootSpeed, Vector2 targetPos, Vector2 targetVelocity)
        {
            return PredictiveAimWithOffset(shootFrom, shootSpeed, targetPos, targetVelocity, 0);
        }
        /// <summary>
        /// give an angle to shoot at to attempt to hit a moving target, returns NaN when this is impossible, includes a shoot Offest
        /// </summary>
        public static float PredictiveAimWithOffset(Vector2 shootFrom, float shootSpeed, Vector2 targetPos, Vector2 targetVelocity, float shootOffset)
        {
            float angleToTarget = (targetPos - shootFrom).ToRotation();
            float targetTraj = targetVelocity.ToRotation();
            float targetSpeed = targetVelocity.Length();
            float dist = (targetPos - shootFrom).Length();
            if (dist < shootOffset)
            {
                shootOffset = 0;
            }

            //imagine a tirangle between the shooter, its target and where it think the target will be in the future
            // we need to find an angle in the triangle z this is the angle located at the target's corner
            float z = MathF.PI + (targetTraj - angleToTarget);

            //with this angle z we can now use the law of cosines to find time
            //the side opposite of z is equal to shootSpeed * time
            //the other sides are dist and targetSpeed * time
            //putting these values into law of cosines gets (shootSpeed * time + shootOffset)^2 = (targetSpeed * time)^2 + dist^2 -2*targetSpeed*time*cos(z)
            //we can rearange it to (shootSpeed^2 - targetSpeed^2)time^2 + (2*targetSpeed*dist*cos(z) + 2*shootOffest*shootSpeed)*time + shootOffset^2 - dist^2 = 0, this is a quadratic!

            //here we use the quadratic formula to find time
            float a = shootSpeed * shootSpeed - targetSpeed * targetSpeed;
            float b = 2 * targetSpeed * dist * MathF.Cos(z) + 2 * shootOffset * shootSpeed;
            float c = (shootOffset * shootOffset) - (dist * dist);
            float time = (-b + MathF.Sqrt(b * b - 4 * a * c)) / (2 * a);

            //we now know the time allowing use to find all sides of the tirangle, now we use law of Sines to calculate the angle to shoot at.
            float calculatedShootAngle = angleToTarget - MathF.Asin((targetSpeed * time * MathF.Sin(z)) / (shootSpeed * time + shootOffset));
            return calculatedShootAngle;
        }
        public static float PredictiveVerticalAim(float myX, float shootSpeed, Vector2 targetPos, Vector2 targetVelocity)
        {
            // targetPos.X + targetVelocity.X * time = myX + shootSpeed * time
            shootSpeed *= Math.Sign(targetPos.X - myX);
            float time = (myX - targetPos.X) / (targetVelocity.X - shootSpeed);
            if(time < 0)
            {
                return float.NaN;
            }
            return (targetPos + targetVelocity * time).Y;
        }

        //used for projectiles using ammo, the vanilla PickAmmo had a bunch of clutter we don't need
        public static bool UseAmmo(this Projectile projectile, int ammoID, ref int shoot, ref float speed, ref int Damage, ref float KnockBack, bool dontConsume = false)
        {
            Player player = Main.player[projectile.owner];
            Item item = new Item();
            bool hasFoundAmmo = false;
            for (int i = 54; i < 58; i++)
            {
                if (player.inventory[i].ammo == ammoID && player.inventory[i].stack > 0)
                {
                    item = player.inventory[i];
                    hasFoundAmmo = true;
                    break;
                }
            }
            if (!hasFoundAmmo)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (player.inventory[j].ammo == ammoID && player.inventory[j].stack > 0)
                    {
                        item = player.inventory[j];
                        hasFoundAmmo = true;
                        break;
                    }
                }
            }

            if (hasFoundAmmo)
            {
                shoot = item.shoot;
                if (player.magicQuiver && (ammoID == AmmoID.Arrow || ammoID == AmmoID.Stake))
                {
                    KnockBack = (float)((int)((double)KnockBack * 1.1));
                    speed *= 1.1f;
                }
                speed += item.shootSpeed;
                if (item.CountsAsClass(DamageClass.Ranged))
                {
                    if (item.damage > 0)
                    {
                        Damage += (int)((float)item.damage * player.GetDamage(DamageClass.Ranged).Multiplicative);
                    }
                }
                else
                {
                    Damage += item.damage;
                }
                if (ammoID == AmmoID.Arrow && player.archery)
                {
                    if (speed < 20f)
                    {
                        speed *= 1.2f;
                        if (speed > 20f)
                        {
                            speed = 20f;
                        }
                    }
                    Damage = (int)((double)((float)Damage) * 1.2);
                }
                KnockBack += item.knockBack;
                bool flag2 = dontConsume;

                if (player.magicQuiver && ammoID == AmmoID.Arrow && Main.rand.NextBool(5))
                {
                    flag2 = true;
                }
                if (player.ammoBox && Main.rand.NextBool(5))
                {
                    flag2 = true;
                }
                if (player.ammoPotion && Main.rand.NextBool(5))
                {
                    flag2 = true;
                }

                if (player.ammoCost80 && Main.rand.NextBool(5))
                {
                    flag2 = true;
                }
                if (player.ammoCost75 && Main.rand.NextBool(4))
                {
                    flag2 = true;
                }
                if (!flag2 && item.consumable)
                {
                    item.stack--;
                    if (item.stack <= 0)
                    {
                        item.active = false;
                        item.TurnToAir();
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        public static void FriendlyFire(this Projectile Projectile) //allows friendly projectile to hit player and cause pvp death (like the grenade explosion)
        {
            Rectangle myRect = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            int myPlayer = Projectile.owner;
            if (Main.player[myPlayer].active && !Main.player[myPlayer].dead && !Main.player[myPlayer].immune && (!Projectile.ownerHitCheck || Projectile.CanHitWithOwnBody(Main.player[myPlayer])))
            {
                Rectangle value = new Rectangle((int)Main.player[myPlayer].position.X, (int)Main.player[myPlayer].position.Y, Main.player[myPlayer].width, Main.player[myPlayer].height);
                if (myRect.Intersects(value))
                {
                    if (Main.player[myPlayer].position.X + (float)(Main.player[myPlayer].width / 2) < Projectile.position.X + (float)(Projectile.width / 2))
                    {
                        Projectile.direction = -1;
                    }
                    else
                    {
                        Projectile.direction = 1;
                    }
                    int num4 = Main.DamageVar((float)Projectile.damage);
                    Projectile.StatusPlayer(myPlayer);
                    Main.player[myPlayer].Hurt(PlayerDeathReason.ByProjectile(Projectile.owner, Projectile.whoAmI), num4, Projectile.direction, true, false);
                }
            }
        }
        public static float AngularDifference(float angle1, float angle2)
        {
            angle1 = PolarVector(1f, angle1).ToRotation();
            angle2 = PolarVector(1f, angle2).ToRotation();
            if (Math.Abs(angle1 - angle2) > Math.PI)
            {
                return MathF.PI * 2 - Math.Abs(angle1 - angle2);
            }
            return Math.Abs(angle1 - angle2);
        }
        public static Vector2 to2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }
        public static void BreakTiles(int i, int j, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    WorldGen.KillTile(i + x, j + y, false, false, true);
                    WorldGen.KillWall(i + x, j + y, false);
                    Main.tile[i + x, j + y].LiquidAmount = 0;
                }
            }
        }
        public static List<Projectile> ProjectileSpread(IEntitySource source, Vector2 position, int count, float speed, int type, int damage, float kb, int owner = 255, float ai0 = 0, float ai1 = 0, float rotation = 0f, float spread = MathF.PI * 2)
        {
            List<Projectile> me = new List<Projectile>();
            for (int r = 0; r < count; r++)
            {
                float rot = rotation + r * (spread / count) - (spread / 2) + (spread / (2 * count));
                me.Add(Main.projectile[Projectile.NewProjectile(source, position, PolarVector(speed, rot), type, damage, kb, owner, ai0, ai1)]);
            }
            return me;
        }

        public static Projectile PokeNPC(Player player, NPC npc, IEntitySource source, float damage, DamageClass damageClass, float knockback = 0f, int AP = 0)
        {
            int id = ModContent.ProjectileType<Poke>();
            if(damageClass == DamageClass.Melee)
            {
                id = ModContent.ProjectileType<PokeMelee>();
            }
            if(damageClass == DamageClass.Ranged)
            {
                id = ModContent.ProjectileType<PokeRanged>();
            }
            if(damageClass == DamageClass.Magic)
            {
                id = ModContent.ProjectileType<PokeMagic>();
            }
            if(damageClass == DamageClass.Summon)
            {
                id = ModContent.ProjectileType<PokeSummon>();
            }
            Projectile p = Main.projectile[Projectile.NewProjectile(source, npc.Center, Vector2.Zero, id, (int)damage, knockback, player.whoAmI, ai1: AP)];
            p.ai[0] = npc.whoAmI;
            p.ai[1] = AP;
            //p.DamageType = damageClass;
            return p;
        }
        public static Projectile PokeNPCMinion(Player player, NPC npc, IEntitySource source, float damage, float knockback = 0f)
        {
            Projectile p = Main.projectile[Projectile.NewProjectile(source, npc.Center, Vector2.Zero, ModContent.ProjectileType<MinionPoke>(), (int)damage, knockback, player.whoAmI)];
            p.ai[0] = npc.whoAmI;
            return p;
        }

        public static void ServerClientCheck(string q)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Main.NewText("Client says  " + q, Color.Pink);
            }

            if (Main.netMode == NetmodeID.Server) // Server
            {
                ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says " + q), Color.Green);
            }
        }

        /// <summary>
        /// This should only be called on the local player. Spawns the given NPC in singleplayer, or sends to the server to spawn it there (if the NPC isn't already alive!)
        /// </summary>
        public static void NPCSpawnOnPlayer(Player player, int type)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, type);
            }
            else
            {
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            }
        }
    }
    public class Poke : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += (int)Projectile.ai[1];
            modifiers.HitDirectionOverride = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0] != target.whoAmI)
            {
                return false;
            }
            return null;
        }
    }
    public class PokeMelee : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += (int)Projectile.ai[1];
            modifiers.HitDirectionOverride = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0] != target.whoAmI)
            {
                return false;
            }
            return null;
        }
    }
    public class PokeRanged : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += (int)Projectile.ai[1];
            modifiers.HitDirectionOverride = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0] != target.whoAmI)
            {
                return false;
            }
            return null;
        }
    }
    public class PokeMagic : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += (int)Projectile.ai[1];
            modifiers.HitDirectionOverride = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0] != target.whoAmI)
            {
                return false;
            }
            return null;
        }
    }
    public class PokeSummon : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += (int)Projectile.ai[1];
            modifiers.HitDirectionOverride = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0] != target.whoAmI)
            {
                return false;
            }
            return null;
        }
    }
    public class MinionPoke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[0] != target.whoAmI)
            {
                return false;
            }
            return null;
        }
    }
}
