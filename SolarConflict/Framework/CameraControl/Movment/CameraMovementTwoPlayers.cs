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
    class CameraMovementTwoPlayers : CameraBaseMovment
    {
        public CameraMovementTwoPlayers(float changeFactor, float changeSpeed = 0) : base(changeFactor, changeSpeed)
        {
        }

        public override void Update(Camera camera, ref Vector2 targetPosition, GameObject mainTarget = null, GameObject secondaryTarget = null, GameEngine gameEngine = null, InputState inputState = null)
        {
            if (mainTarget == null || inputState == InputState.EmptyState)
                return;
            //targetPosition = mainTarget.Position;
            //if (secondaryTarget != null)
            //    targetPosition = (mainTarget.Position + secondaryTarget.Position) * 0.5f;
            targetPosition = mainTarget.Position + (inputState.Cursor.Position - ActivityManager.ScreenCenter) * 2;
            base.Update(camera, ref targetPosition, mainTarget, secondaryTarget, gameEngine, inputState);
         
        }
    }
}
