using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.CameraControl.Movment
{
    [Serializable]
    class ManualMovement
    {
        Vector2 _mousePos;
        Vector2 _cameraSpeed = new Vector2();

        public void Update(Camera camera, GameObject mainTarget, GameObject secondaryTarget, GameEngine gameEngine, InputState inputState)
        {
            //float targetZoom = camera.zoom;
            //float zoomValue = targetZoom * MathHelper.Clamp(((50f + MouseUtils.Inst.GetDScroolWheel()) / 50f), 0.9f, 1.1f); //KOBI: change it to input state
            //targetZoom = MathHelper.Clamp(zoomValue, 0.17f, 8.5f);
            //camera.zoom = targetZoom;

            _mousePos = camera.GetWorldPos(inputState.Cursor.Position);
            Vector2 diff = Vector2.Zero;
            if(inputState.Cursor.IsActive)
                diff = inputState.Cursor.PreviousPosition - inputState.Cursor.Position;
            var gamepadState = GamePad.GetState(PlayerIndex.One);
           
            if (inputState.Cursor.OnPressLeft || inputState.Cursor.OnPressRight)
            {
                diff = Vector2.Zero;
            }

            Vector2 xboxDiff = gamepadState.ThumbSticks.Left * 10f;
            xboxDiff.Y = -xboxDiff.Y;
            if(xboxDiff.LengthSquared() > 0.3f)
            _cameraSpeed = xboxDiff;
            
            if (inputState.Cursor.IsPressedLeft  || inputState.Cursor.IsPressedRight)
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

