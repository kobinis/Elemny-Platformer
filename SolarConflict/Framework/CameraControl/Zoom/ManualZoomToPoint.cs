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
    public class ManualZoomToPoint : CameraZoomBase //TODO: 
    {
        public float ManualTargetZoom = 0;
        public float ManualZoomChangeFactor = 0.4f;
        public float MinZoom;
        public float MaxZoom;

        public ManualZoomToPoint(float zoomChangeFactor = 0.05f, float zoomChangeSpeed = 0.01f)
            : base(zoomChangeFactor, zoomChangeSpeed)
        {
            MinZoom = Consts.CAMERA_MIN_ZOOM;
            MaxZoom = Consts.CAMERA_MAX_ZOOM;
        }

        public override void Update(Camera camera, GameObject mainTarget, GameObject secondaryTarget, GameEngine gameEngine, InputState inputState)
        {
            Vector2 targetPoint = Vector2.Zero;
            if(inputState.Cursor.IsActive)
                targetPoint = inputState.Cursor.Position - ActivityManager.ScreenCenter;
            if (ManualTargetZoom == 0)
            {
                ManualTargetZoom = camera.Zoom;
            }

            float oldZoom = camera.Zoom;
            Vector2 oldPos = camera.Position;
            float dm = MouseUtils.Inst.GetDScroolWheel();
            if (dm != 0)
            {
                // float dScrool = 
                ManualTargetZoom = camera.Zoom * MathHelper.Clamp(1+ dm/50f, 0.5f, 2f); //KOBI: change it to input state             
                ManualTargetZoom = MathHelper.Clamp(ManualTargetZoom, MinZoom, MaxZoom);
            }

            if (inputState.IsKeyDown(Keys.PageUp))
            {
                ManualTargetZoom = Math.Max(camera.Zoom * 1.1f, ManualTargetZoom);
                ManualTargetZoom = MathHelper.Clamp(ManualTargetZoom, MinZoom, MaxZoom);
            }

            if (inputState.IsKeyDown(Keys.PageDown))
            {
                ManualTargetZoom = Math.Min(camera.Zoom * 0.9f, ManualTargetZoom);
                ManualTargetZoom = MathHelper.Clamp(ManualTargetZoom, MinZoom, MaxZoom);
            }

            camera.Zoom = camera.Zoom * (1 - ManualZoomChangeFactor) + ManualTargetZoom * ManualZoomChangeFactor;
            
            camera.Position = targetPoint/ oldZoom - targetPoint/ camera.Zoom + oldPos;
            //
        }
    }
}

