using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.PlayersManagement;

namespace SolarConflict.Framework.CameraControl.Movment
{
    [Serializable]
    class CameraOnPlayer : CameraBaseMovment
    {
        public CameraOnPlayer(float changeFactor, float changeSpeed = 0) : base(changeFactor, changeSpeed)
        {
        }

        public override void Update(Camera camera, ref Vector2 targetPosition, GameObject mainTarget = null, GameObject secondaryTarget = null, GameEngine gameEngine = null, InputState inputState = null)
        {
            if (mainTarget == null)
                return;
            targetPosition = mainTarget.Position;
            base.Update(camera, ref targetPosition, mainTarget, secondaryTarget, gameEngine, inputState);

        }
    }
}

