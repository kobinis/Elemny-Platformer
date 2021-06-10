using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Agents.Systems.Misc
{
    [Serializable]
    class LightSystem : AgentSystem
    {
        public LightObject LightObject;
        ControlSignals signals;

        public LightSystem()
        {
            LightObject = new LightObject(new Vector3(1f, 0.5f, 0.2f), 500, 0f, 0f);
            LightObject.Light.Intensity = 1;
            signals = ControlSignals.AlwaysOn;
        }
        
        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if((signals & agent.ControlSignal) > 0 || tryActivate)
            {
                LightObject.Position = initPosition;
                if (gameEngine.Camera.IsOnScreen(LightObject))
                    gameEngine.LightsPerFrame.Add(LightObject); //Only if on screen
                //LightObject.Light.Intensity = Math.Min(LightObject.Light.Intensity + 0.1f, 5f);

                return true;
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            var engineSystem = (LightSystem)MemberwiseClone();
            engineSystem.LightObject = this.LightObject.GetWorkingCopy();
            engineSystem.LightObject.Light = this.LightObject.Light.GetWorkingCopy();
            return engineSystem; ;
        }
    }
}
