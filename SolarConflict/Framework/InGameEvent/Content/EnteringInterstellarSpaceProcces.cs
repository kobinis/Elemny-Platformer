using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.InGameEvent.Content
{
    /// <summary>
    /// 
    /// </summary>
    class EnteringInterstellarSpaceProcces : GameProcess
    {
        private List<IEmitter> Loadouts;

        private List<GameObject> _enemies;
        
        
        public EnteringInterstellarSpaceProcces()
        {
            Loadouts = new List<IEmitter>();
            Loadouts.Add(ContentBank.Inst.GetEmitter("Pirate4"));

            _enemies = new List<GameObject>();
        }

        

        public override void Update(GameEngine gameEngine)
        {

          //  if(gameEngine.PlayerAgent != null &&)
            
        }

        public override GameProcess GetWorkingCopy()
        {
            throw new NotImplementedException();
        }
    }
}
