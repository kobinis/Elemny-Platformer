using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.GameObjects
{
    class BeamProjectile : GameObject, IEmitter
    {
        public Vector2 Origin;
        public override float Mass { get => 1; set { } }
        public override string Name { get => string.Empty; set { } }

        public override string Tag => string.Empty;

        public override CollisionSpec CollisionInfo { get; set; }
        public string ID { get; set; }

        public Sprite Sprite;
        private int lifetime;
        private int BeamWidth;

        public Color color;

        public override void Draw(Camera camera)
        {      
            Vector2 destenation = Position;   //projectile.Position;
            Vector2 diff = destenation - Origin;
            float length = diff.Length();
            Camera.NormalMapEffect.Parameters["BeamMaxLength"].SetValue(length);
            Camera.NormalMapEffect.Parameters["BeamLength"].SetValue(length);
            Camera.NormalMapEffect.Parameters["BeamLifetime"].SetValue((float)lifetime * 0.045f);
            float rotation = (float)Math.Atan2(diff.Y, diff.X);
            float beamWidth = BeamWidth * 0.5f; //TODO add *size
            //Rectangle source = new Rectangle((int)projectile.Parent.Lifetime * -10, 0, projectile.profile.Sprite.Width, projectile.profile.Sprite.Height);
            camera.SpriteBatch.Draw(Sprite, new Rectangle((int)Origin.X, (int)Origin.Y, (int)length, (int)beamWidth), null, color, rotation, new Vector2(0, Sprite.Height / 2f), SpriteEffects.None, 1);
        }

        public override void ApplyCollision(GameObject collidingObject, GameEngine gameEngine)
        {          
        }

        public override void ApplyForce(Vector2 force, float speedLimit)
        {
            
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Origin = refPosition;
            return null;
        }

        public override GameObject GetAgentAncestor()
        {
            throw new NotImplementedException();
        }

        public override string GetId()
        {
            throw new NotImplementedException();
        }

        public override float GetMeterValue(MeterType type)
        {
            throw new NotImplementedException();
        }

        public override GameObjectType GetObjectType()
        {
            throw new NotImplementedException();
        }

        public override Sprite GetSprite()
        {
            throw new NotImplementedException();
        }

        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType)
        {
            throw new NotImplementedException();
        }

        public override void SetMeterValue(MeterType type, float value)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameEngine gameEngine)
        {
            throw new NotImplementedException();
        }
    }
}
