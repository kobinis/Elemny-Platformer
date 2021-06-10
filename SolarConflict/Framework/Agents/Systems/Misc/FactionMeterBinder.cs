using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    /// <summary>
    /// FactionMeterBinder -Binds meter of the agent with the global meter of the faction(used for supply points, money, kill count...) 
    /// </summary>
    [Serializable]
    public class FactionMeterBinder : AgentSystem
    {

        public MeterType meterToBind;
        
        public FactionMeterBinder(MeterType meterToBind)
        {
            this.meterToBind = meterToBind;
        }

        public FactionMeterBinder():this(MeterType.Money)
        {
           
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {            
            Faction faction = gameEngine.GetFaction(agent.FactionType);
            agent.SetMeter(meterToBind,faction.GetMeter(meterToBind));
            return false;
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return (FactionMeterBinder)MemberwiseClone();
        }
    }
}
