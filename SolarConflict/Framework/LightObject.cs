using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;

namespace SolarConflict.Framework
{
    [Serializable]
    public class LightObject : GameObject
    {
        public override float Mass { get { return 1; } set { } }
        public override string Name { get { return "light"; } set { } }

        public override string Tag { get;}

        public override CollisionSpec CollisionInfo { get; set; }

        public override PointLight Light { get; set; }


        public LightObject(Vector3 baseColor, float attenuation, float intensity, float hotspot = 0)
        {
           Light = new PointLight(baseColor, attenuation, intensity, hotspot);
        }

        public override void ApplyCollision(GameObject collidingObject, GameEngine gameEngine)
        {
            
        }

        public override void ApplyForce(Vector2 force, float speedLimit)
        {
            
        }

        public override GameObject GetAgentAncestor()
        {
            return null;
        }

        public override string GetId()
        {
            return "light";
        }

        public override float GetMeterValue(MeterType type)
        {
            return 0;
        }

        public override GameObjectType GetObjectType()
        {
            return GameObjectType.Light;
        }

        public override Sprite GetSprite()
        {
            return null;
        }

        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType)
        {
            return null;
        }

        public override void SetMeterValue(MeterType type, float value)
        {
        }

        public override void Update(GameEngine gameEngine)
        {
            
        }

        public LightObject GetWorkingCopy()
        {
            return (LightObject)MemberwiseClone();
        }
    }
}
