using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.CameraControl
{
    interface ICameraControl
    {
        void Update(Camera camera, GameObject mainTarget, GameObject secondaryTarget, GameEngine gameEngine, InputState inputState);
    }
}
