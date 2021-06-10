using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.CameraControl.Zoom
{
    [Serializable]
    public class ManualZoom : CameraZoomBase //TODO: 
    {
        public float ManualTargetZoom = 0;
        public float ManualZoomChangeFactor = 0.05f;
        public float MinZoom;
        public float MaxZoom;
      //  public static float _zoom = 0;
        public ManualZoom(float zoomChangeFactor = 0.05f, float zoomChangeSpeed = 0.01f)
            : base(zoomChangeFactor, zoomChangeSpeed)
        {
            MinZoom = Consts.CAMERA_MIN_ZOOM;
            MaxZoom = Consts.CAMERA_MAX_ZOOM;
        }
       
        public override void Update(Camera camera, GameObject mainTarget, GameObject secondaryTarget, GameEngine gameEngine, InputState inputState)
        {
            if (ManualTargetZoom == 0)
            {
                ManualTargetZoom = TargetZoom;
            }

            if(inputState.IsKeyDown(Keys.PageUp))
            {
                ManualTargetZoom = Math.Max( camera.Zoom * 1.1f, ManualTargetZoom);             
                ManualTargetZoom = MathHelper.Clamp(ManualTargetZoom, MinZoom, MaxZoom);
            }

            if (inputState.IsKeyDown(Keys.PageDown))
            {
                ManualTargetZoom = Math.Min(camera.Zoom * 0.9f, ManualTargetZoom);
                ManualTargetZoom = MathHelper.Clamp(ManualTargetZoom, MinZoom, MaxZoom);
            }

            if (MouseUtils.Inst.GetDScroolWheel() != 0)
            {
                // float dScrool = 
                ManualTargetZoom = camera.Zoom * MathHelper.Clamp(((MouseUtils.Inst.GetDScroolWheel()) / 100f), 0.8f, 1.4f); //KOBI: change it to input state             
                ManualTargetZoom = MathHelper.Clamp(ManualTargetZoom, MinZoom, MaxZoom);
            }

            if (Math.Abs(ManualTargetZoom - camera.Zoom) <= ZoomChangeSpeed)
            {
                camera.Zoom = ManualTargetZoom;
            }
            else
            {
                camera.Zoom += Math.Sign(ManualTargetZoom - camera.Zoom) * ZoomChangeSpeed;
            }
            camera.Zoom = camera.Zoom * (1 - ManualZoomChangeFactor) + ManualTargetZoom * ManualZoomChangeFactor;
        }
    }
 }

