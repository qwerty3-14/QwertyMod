using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using QwertyMod.Content.Dusts;
using Terraria.ModLoader;
using ReLogic.Content;

namespace QwertyMod
{
    public abstract class Turret
    {
        protected float rotation;
        protected float homeRotation;
        Vector2 anchorAt;
        protected Vector2 relativePosition;
        protected float rotSpeed = (float)Math.PI / 30f;
        protected NPC parent;
        protected float turretLength;
        protected Texture2D texture;
        protected Vector2 origin;
        public Turret(NPC parent, Vector2 anchorAt, float homeRotation = 0)
        {
            this.parent = parent;
            this.anchorAt = anchorAt;
            this.homeRotation = homeRotation;
        }
        public bool AimHome()
        {
            float old = rotation;
            rotation.SlowRotation(homeRotation, rotSpeed);
            return rotation == old;
        }

        public bool AimRelative(float here)
        {
            float old = rotation;
            rotation.SlowRotation(here, rotSpeed);
            return rotation == old;
        }
        public bool AimAt(Vector2 here)
        {
            return AimAt((here - AbsolutePosition()).ToRotation());
        }
        public bool AimAt(float here)
        {
            float aimtoward = here - parent.rotation;
            if(parent.direction == -1)
            {
                aimtoward = MathF.PI - aimtoward;
            }
            rotation.SlowRotation(aimtoward, rotSpeed);
            return QwertyMethods.AngularDifference(rotation, here - parent.rotation) < rotSpeed * 2;
        }
        public virtual void UpdateRelativePosition(Vector2? move = null)
        {

            if (move != null)
            {
                anchorAt = (Vector2)move;
            }
            relativePosition = QwertyMethods.PolarVector(anchorAt.X, parent.rotation) + QwertyMethods.PolarVector(anchorAt.Y, parent.rotation + (float)Math.PI / 2);
            if(parent.direction == -1)
            {
                relativePosition.X *= -1;
            }
            Update();
        }
        public float AbsoluteRotation()
        {
            float rot = rotation + parent.rotation;
            if(parent.direction == -1)
            {
                rot = MathF.PI - rot;
            }
            return rot;
        }
        public Vector2 AbsolutePosition()
        {
            return relativePosition + parent.Center;
        }
        public virtual Vector2 AbsoluteShootPosition()
        {
            return AbsolutePosition() + QwertyMethods.PolarVector(turretLength, AbsoluteRotation());
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            spriteBatch.Draw(texture, AbsolutePosition() - screenPos,
            null, drawColor, AbsoluteRotation(),
            origin, 1f, 
            SpriteEffects.None, 0f);
        }
        public virtual void Fire()
        {

        }
        public virtual void Update()
        {

        }
        public static Texture2D[] turretTextures;
        public static void LoadTextures()
        {
            turretTextures = new Texture2D[6];

            var immediate = AssetRequestMode.ImmediateLoad;
            turretTextures[0] = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipTurret", immediate).Value;
            turretTextures[1] = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipTurret_Glow", immediate).Value;
            turretTextures[2] = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipTurretGuns", immediate).Value;
            turretTextures[3] = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipTurretGuns_Glow", immediate).Value;
            turretTextures[4] = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipMissileLauncher", immediate).Value;
            turretTextures[5] = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipMissileLauncher_Glow", immediate).Value;
        }
    }
}