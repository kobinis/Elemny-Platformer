using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework
{
    /// <summary>
    /// A process that runs gameEngine
    /// </summary>
    [Serializable]
    public abstract class GameProcess
    {
        public bool Finished { get; set; }
        public virtual void InitProcess(GameEngine gameEngine) {}
        public abstract void Update(GameEngine gameEngine);        
        public abstract GameProcess GetWorkingCopy();
    }
}
