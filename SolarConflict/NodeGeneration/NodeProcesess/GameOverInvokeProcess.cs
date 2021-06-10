using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.NodeGeneration.NodeProcesess
{
    [Serializable]
    class GameOverInvokeProcess: GameProcess
    {
        int timer;

        public override void Update(GameEngine gameEngine)
        {
            var mothership = gameEngine.GetFaction(FactionType.Player).Mothership;
            if (mothership != null && mothership.IsNotActive)
            {
                timer++;
            }
            else
            {
                timer = 0;
            }

            if(timer > 60)
            {
                GameOver(gameEngine);
            }

        }

        private void GameOver(GameEngine gameEngine)
        {
            var parameters = new ActivityParameters();
            parameters.DataParams.Add("Scene", gameEngine.Scene);
            parameters.ParamDictionary.Add("title", "Game Over");
            parameters.ParamDictionary.Add("Activity", "MainMenu");
            parameters.ParamDictionary.Add("ActivityParams", string.Empty);
            ActivityManager.Inst.SwitchActivity("GameOverActivity", parameters, false);
        }

        public override GameProcess GetWorkingCopy()
        {
            return this;
        }
    }
}
