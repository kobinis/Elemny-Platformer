using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    /// <summary>
    /// MeterMaxValueSetter set one metters max value according to another meter value
    /// Used for setting max energy set by generators and batterys, shields and shiled bank. 
    /// </summary> 
    [Serializable]
    public class MeterMaxValueSetter : AgentSystem
    {
        public MeterType MeterTypeToGetValue { get; set; }
        public MeterType MeterTypeToSetMaxValue { get; set; }        

        public MeterMaxValueSetter()
        {
        }

        public MeterMaxValueSetter(MeterType meterTypeToSetMaxValue, MeterType meterTypeToGetValue)
        {
            MeterTypeToSetMaxValue = meterTypeToSetMaxValue;
            MeterTypeToGetValue = meterTypeToGetValue;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            agent.SetMeterMaxValue(MeterTypeToSetMaxValue, agent.GetMeterValue(MeterTypeToGetValue));
            agent.SetMeterValue(MeterTypeToGetValue, 0); //TODO: ?add flag
            return false;
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
