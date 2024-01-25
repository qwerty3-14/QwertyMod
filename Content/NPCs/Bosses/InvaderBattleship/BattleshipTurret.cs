using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;



namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class BattleshipTurret : Turret
    {
        public BattleshipTurret(NPC parent, Vector2 anchorAt, float homeRotation = 0) : base(parent, anchorAt, homeRotation)
        {
            origin = new Vector2(18f, 18f);
            if(!Main.dedServ)
            {
                texture = turretTextures[0];
            }
            turretLength = 38f;
            rotSpeed = (float)Math.PI / 90f;
        }
        
        public override void Fire()
        {  
            SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_turret"), AbsoluteShootPosition());
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(parent.GetSource_FromAI(), AbsoluteShootPosition() + QwertyMethods.PolarVector(11, AbsoluteRotation() + MathF.PI / 2f), QwertyMethods.PolarVector(4.5f, AbsoluteRotation()), ModContent.ProjectileType<InvaderRay>(), Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage, 0);
                Projectile.NewProjectile(parent.GetSource_FromAI(), AbsoluteShootPosition() + QwertyMethods.PolarVector(11, AbsoluteRotation() - MathF.PI / 2f), QwertyMethods.PolarVector(4.5f, AbsoluteRotation()), ModContent.ProjectileType<InvaderRay>(), Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage, 0);
            }
            gunOffset = 16;
            lightCount = 0;
        }
        float gunOffset = 0;
        public override void Update()
        {
            if(gunOffset > 0)
            {
                gunOffset-= 0.5f;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            spriteBatch.Draw(Turret.turretTextures[0], AbsolutePosition() - screenPos, null, drawColor, AbsoluteRotation(), origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Turret.turretTextures[1], AbsolutePosition() - screenPos, new Rectangle(0, 0, 4 + lightCount * 6, 36), Color.White, AbsoluteRotation(), origin, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(Turret.turretTextures[2], AbsolutePosition() + QwertyMethods.PolarVector(-gunOffset, AbsoluteRotation()) - screenPos, null, drawColor, AbsoluteRotation(), origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Turret.turretTextures[3], AbsolutePosition() + QwertyMethods.PolarVector(-gunOffset, AbsoluteRotation()) -screenPos, null, Color.White, AbsoluteRotation(), origin, 1f, SpriteEffects.None, 0f);
        }
        int lightCount = 0;
        public void SetLights(int count)
        {
            lightCount = count;
        }
    }
    public class BattleshipMissileLauncher : Turret
    {
        public BattleshipMissileLauncher(NPC parent, Vector2 anchorAt, float homeRotation = 0) : base(parent, anchorAt, homeRotation)
        {
            origin = new Vector2(7f, 7f);
            if(!Main.dedServ)
            {
                texture = turretTextures[4];
            }
            turretLength = 36f;
            rotSpeed = (float)Math.PI / 180f;
        }
        int frame = 0;
        public override void Draw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            spriteBatch.Draw(Turret.turretTextures[4], AbsolutePosition() - screenPos, new Rectangle(0, 16 * frame, 68, 16), drawColor, AbsoluteRotation() + (parent.direction == 1 ? 0 : MathF.PI), (parent.direction == 1 ? origin : new Vector2(Turret.turretTextures[4].Width - origin.X, origin.Y)), 1f, parent.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(Turret.turretTextures[5], AbsolutePosition() - screenPos, new Rectangle(0, 16 * frame, 68, 16), Color.White, AbsoluteRotation() + (parent.direction == 1 ? 0 : MathF.PI), (parent.direction == 1 ? origin : new Vector2(Turret.turretTextures[5].Width - origin.X, origin.Y)), 1f, parent.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        public override void Fire()
        {

            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_missile"), AbsoluteShootPosition());
                Projectile.NewProjectile(parent.GetSource_FromAI(), AbsoluteShootPosition(), QwertyMethods.PolarVector(4, AbsoluteRotation()), ModContent.ProjectileType<BattleshipMissile>(), Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage, 0);
            }
            frame = 1;
        }
        public bool HasFired()
        {
            return frame == 1;
        }
        public void Reload()
        {
            frame = 0;
        }
    }
}