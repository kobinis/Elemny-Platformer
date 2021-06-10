using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public abstract class AgentDraw
    {
        public abstract void Draw(Agent agent, Camera camera);

    }
}
