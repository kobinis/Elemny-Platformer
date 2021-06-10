using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    /// <summary>
    /// Checks for a value on the blackboard metaworld
    /// </summary>
    [Serializable]
    class BlackboardObjective : MissionObjective
    {
        bool IsGlobal;
        public string Key;
        public string Value;

        public BlackboardObjective(string key, string value)
        {
            IsGlobal = false;
            Key = key;
            Value = value;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = ObjectiveStatus.Ongoing;
            MetaWorld metaworld = scene.MetaWorld;
            if (IsGlobal)
                metaworld = GameSession.Inst.MetaWorld;
            if (metaworld == null)
                return ObjectiveStatus.Ongoing;
            string metaValue;
            metaworld.Blackboard.TryGetValue(Key, out metaValue);
            if (Value==null && metaValue != null || Value != null && metaValue == Value)
                Status = ObjectiveStatus.Completed;
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
