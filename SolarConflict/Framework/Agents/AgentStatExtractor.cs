using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject
{
    //NOW: fix case agent is null
    public enum Stats
    {
        Hitpoints, Mass, Size, AngularMass, ShieldCapacity, ShieldGen,
        EnergyCapacity, EnergyGen, MaxSpeed, RotationSpeed, DPS, DPE, EnergyNeed,
    }

    public class AgentStatExtractor //TODO: change the class
    {
        GameEngine _tempEngine;
        private Dictionary<Stats, float?> _statistics;

        public AgentStatExtractor()
        {
            _tempEngine = new GameEngine(new Camera());
            _statistics = new Dictionary<Stats, float?>();
        }


        public float? GetStatValue(Stats stat)
        {
            float? value;
            _statistics.TryGetValue(stat, out value);
            return value;
        }

        //TODO: a system will return a list of the stats it effects, this will combine all
        //public float? Cal(Stats stat, Agent agent)
        //{
        //    switch (stat)
        //    {
        //        case Stats.Size:
        //            return agent.Size;
        //        case Stats.Hitpoints:
        //            return CalcHitpoints(agent);                    
        //        case Stats.Mass:
        //            return CalcMass(agent);                    
        //        case Stats.AngularMass:
        //            return CalcAngularMass(agent);
        //        case Stats.ShieldCapacity:
        //            return 0;                    
        //        case Stats.ShieldGen:
        //            return 0;                    
        //        case Stats.EnergyCapacity:
        //            break;
        //        case Stats.EnergyGen:
        //            break;
        //        case Stats.MaxSpeed:
        //            break;
        //        case Stats.RotationSpeed:
        //            break;
        //        default:
        //            break;
        //    }
        //    return null;
        //}

        public float? CalcHitpoints(Agent agent)
        {
            return agent.MaxHitpoints;
        }

        public float? CalcMass(Agent agent)
        {
            return agent.Mass;
        }

        public float? CalcAngularMass(Agent agent)
        {
            return agent.RotationMass;
        }

    }
}
