using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public class CameraZoomBase
    {

        /// <summary>
        /// The target zoom that the camera zoom will move towards
        /// </summary>
        public float TargetZoom { set; get; }

        /// <summary>
        /// Determines the speed that the camera zoom moves to target zoom 
        /// </summary>
        public float ZoomChangeFactor { set; get; }

        /// <summary>
        /// Determines the speed that the camera zoom moves to target zoom 
        /// </summary>
        public float ZoomChangeSpeed { set; get; }

        public CameraZoomBase(float zoomChangeFactor, float zoomChangeSpeed = 0)
        {
            ZoomChangeSpeed = zoomChangeSpeed;
            ZoomChangeFactor = zoomChangeFactor;
            TargetZoom = CameraManager.STARTING_ZOOM;
        }

        public virtual void Update(Camera camera, GameObject mainTarget = null, GameObject secondaryTarget = null, GameEngine gameEngine = null, InputState inputState = null)
        {
            if (Math.Abs(TargetZoom - camera.Zoom) <= ZoomChangeSpeed)
            {
                camera.Zoom = TargetZoom;
            }
            else
            {
                camera.Zoom += Math.Sign(TargetZoom - camera.Zoom) * ZoomChangeSpeed;
            }
            camera.Zoom = camera.Zoom * (1 - ZoomChangeFactor) + TargetZoom * ZoomChangeFactor;
        }

    }
}
