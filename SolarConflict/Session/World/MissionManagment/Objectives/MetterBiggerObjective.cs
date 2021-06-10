using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{   
    [Serializable] 
    class MetterBiggerObjective:MissionObjective
    {
        public GameObject Target;
        public MeterType Type;
        public float Value;

        public MetterBiggerObjective(MeterType type, float value, GameObject target)
        {
            Text = type.ToString() + " " + value.ToString();
            Type = type;
            Value = value;
            Target = target;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if(Target.GetMeterValue(Type) >= Value)
            {
                Status = ObjectiveStatus.Completed;
            }
            return Status;
        }

        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return 0;
        }
    }
}
