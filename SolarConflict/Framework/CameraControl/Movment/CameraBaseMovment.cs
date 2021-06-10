using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.CameraControl.Movment
{
    [Serializable]
    class CameraBaseMovment
    {
        
        /// <summary>
        /// Determens the speed that the camera zoom moves to target zoom 
        /// </summary>
        public float MovmentFactor { set; get; }

        /// <summary>
        /// Determens the speed that the camera zoom moves to target zoom 
        /// </summary>
        public float MovmentSpeed { set; get; }

        /// <summary>
        /// The MovmentSpeed Acceleration
        /// </summary>
        public float MovmentAcceleration { set; get; }

        public CameraBaseMovment(float changeFactor, float changeSpeed = 0)
        {
            MovmentSpeed = changeSpeed;
            MovmentFactor = changeFactor;            
        }

        public virtual void Update(Camera camera, ref Vector2 targetPosition,  GameObject mainTarget = null, GameObject secondaryTarget = null, GameEngine gameEngine = null, InputState inputState = null)
        {
            if (MovmentSpeed > 0)
            {
                float length= (targetPosition - camera.Position).Length();
                if (length <= MovmentSpeed)
                {
                    camera.Position = targetPosition;
                }
                else
                {
                    camera.Position += (targetPosition - camera.Position)* (MovmentSpeed/ length);
                }
            }
            MovmentSpeed += MovmentAcceleration;
            camera.Position = camera.Position * (1 - MovmentFactor) + targetPosition * MovmentFactor;
        }
    }
}
