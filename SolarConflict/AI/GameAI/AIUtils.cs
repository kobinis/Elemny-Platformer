using SolarConflict.AI.GameAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI
{
    class AIUtils
    {
        /// <summary>
        /// Generates AI for all loadouts!
        /// </summary>
        public static void GenerateAIForLoadouts()
        {
            var loadouts = ContentBank.Inst.GetAllLoadout();
            int counter = 5;
            foreach (var loadout in loadouts)
            {
                GameEngine gameEngine = new GameEngine(null);
                var agent = loadout.MakeGameObject(gameEngine) as Agent;
                ParameterControl control = ParameterControl.MakeAIFromAgent(agent);
                counter++;
                control.ID = counter;
                AIBank.Inst.AddControl(control);
                loadout.AiKey = counter;
            }
        }
    }
}
