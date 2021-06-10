using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.CameraControl.Zoom;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.CameraControl
{
    [Serializable]
    public class AutoZoom : CameraZoomBase
    {
        public int AutoZoomCooldownTime = 60 * 10;
        public float ManualZoomChangeFactor = 0.9f;
        private enum State { Transition, ManualZoom }
       // private State CurrentState;
        private int _cooldownTimer;
        private float minZoom;
        private ManualZoom _manualZoom;

        public AutoZoom(float zoomChangeFactor) : base(zoomChangeFactor)
        {
          //  CurrentState = State.ManualZoom;
            TargetZoom = CameraManager.STARTING_ZOOM;
            minZoom = Consts.CAMERA_MIN_ZOOM;
            _manualZoom = new ManualZoom();
        }

        public override void Update(Camera camera, GameObject mainTarget, GameObject secondaryTarget, GameEngine gameEngine, InputState inputState)
        {
            //Find the closest target
            // gameEngine.coll
            Agent agent = mainTarget as Agent;
            if (agent == null)
                return;
            secondaryTarget = TargetSelector.FindClosestEnemy(gameEngine, agent, 3000);
            if (secondaryTarget != null && mainTarget != null && _cooldownTimer <= 0)
            {
                float distance = (secondaryTarget.Position - mainTarget.Position).Length() * 1.2f;
                TargetZoom =  MathHelper.Clamp(ActivityManager.ScreenHeight / distance * 0.5f, minZoom, 1f); //Maximal zoom is one
                base.Update(camera);                
                if (Math.Abs(MouseUtils.Inst.GetDScroolWheel()) > 0.01f)
                {
                    _cooldownTimer = AutoZoomCooldownTime;
                }
                _manualZoom.TargetZoom = TargetZoom;
            }
            else
            {
                _cooldownTimer--;
                _manualZoom.Update(camera, mainTarget, secondaryTarget, gameEngine, inputState);
            }

        }

    }
}
