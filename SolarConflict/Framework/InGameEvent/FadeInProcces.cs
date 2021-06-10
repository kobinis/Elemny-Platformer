using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework.InGameEvent
{
    [Serializable]
    class FadeInProcces : GameProcess
    {
        public float FadeDurationInSec = 3;

        public override void Update(GameEngine gameEngine)
        {
            if(gameEngine.Scene != null)
            {
                float delta = 1f / FadeDurationInSec / 60f;
                gameEngine.Scene.fadeAlpha -= delta;
                if(gameEngine.Scene.fadeAlpha <= 0)
                {
                    gameEngine.Scene.fadeAlpha = 0;
                    this.Finished = true;
                }

            }
        }

        public override GameProcess GetWorkingCopy()
        {
            return this;
        }
    }
}
