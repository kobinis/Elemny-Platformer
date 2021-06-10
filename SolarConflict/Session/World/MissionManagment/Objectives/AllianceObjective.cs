using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    /// <summary>
    /// AllianceObjective - An objective to aille or be hostile to a spesific faction
    /// </summary>
    [Serializable]
    public class AllianceObjective: MissionObjective
    {
        public FactionType FactionType;
        public float Threshold;
        public ObjectiveStatus StatusOnLow;
        public ObjectiveStatus StatusOnHigh;

        public AllianceObjective(FactionType factionType)
        {
            FactionType = factionType;
            StatusOnHigh = ObjectiveStatus.Completed;
            StatusOnLow = ObjectiveStatus.Failed;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Faction faction = scene.GameEngine.GetFaction(FactionType);
            Status = faction.GetRelationToFaction(FactionType.Player) < Threshold ? StatusOnLow : StatusOnHigh;
            return Status;
        }

        public override string GetObjectiveText()
        {
            // Faction faction = scene.GameEngine.GetFaction(FactionType);
            return GetStatusTag();// + " Relation with faction"
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
