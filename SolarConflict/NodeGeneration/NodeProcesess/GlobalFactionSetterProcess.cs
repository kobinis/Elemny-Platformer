using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NodeGeneration.NodeProcesess
{
    [Serializable]
    class GlobalFactionSetterProcess : GameProcess
    {
        public override void Update(GameEngine gameEngine)
        {
            foreach (var faction in MetaWorld.Inst.Factions.Values)
            {
                gameEngine.Factions[(int)faction.FactionType] = faction;
            }
        }

        public override GameProcess GetWorkingCopy()
        {
            return this;
        }
    }
}
