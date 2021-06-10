using Microsoft.Xna.Framework;
using SolarConflict.XnaUtils.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.CameraControl
{
    public class ManualCameraLogic : ICameraControl
    {
        Vector2 _mousePos;
        Vector2 _cameraSpeed = new Vector2();
        
        public void Update(Camera camera, GameObject mainTarget, GameObject secondaryTarget, GameEngine gameEngine, InputState inputState)
        {
            float targetZoom = camera.Zoom;
            float zoomValue = targetZoom * MathHelper.Clamp(((50f + MouseUtils.Inst.GetDScroolWheel()) / 50f), 0.9f, 1.1f); //KOBI: change it to input state
            targetZoom = MathHelper.Clamp(zoomValue, 0.17f, 8.5f);
            camera.Zoom = targetZoom;

            _mousePos = camera.GetWorldPos(inputState.Cursor.Position);
            Vector2 diff = inputState.Cursor.PreviousPosition - inputState.Cursor.Position;
            if (inputState.Cursor.OnPressLeft)
            {
                diff = Vector2.Zero;
            }

            if (inputState.Cursor.IsPressedLeft)
            {                
                _cameraSpeed = diff;
            }

            if (_cameraSpeed.Length() > 0.00001f)
            {
                camera.Position += _cameraSpeed / camera.Zoom;
            }

            _cameraSpeed *= 0.91f;
        }
    }
}
