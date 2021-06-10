using Microsoft.Xna.Framework;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.World.MetaGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Session.World.MissionManagment.Objectives {

    /// <summary>Objective to go to the nearest node that satisfies a bunch of constraints</summary>
    [Serializable]
    class GoToNearestNodeObjective : MissionObjective {

        int _currentTarget;                
        public NodeType? Type;


        public GoToNearestNodeObjective(NodeType? type = null) {
            Type = type;
            _currentTarget = -1;                      
        }

        public override string GetObjectiveText() {        
            return $"Visit a {Type.Value} node";
        }

        public override Vector2? GetPosition() {
            return null;
        }

        public override float GetRadius() {
            return 0;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene) {
            if (_currentTarget >= 0)
                // We already know where we're going, haven't changed nodes since
                return ObjectiveStatus.Ongoing;

            // Just assigned this objective or changed nodes. Are we good?
            // Get current node's type, kludgily, by looking it up by index in the galaxy map
            var index = GalaxyMap.Inst.CurrentNodeIndex;
            if (index < 0 || index >= GalaxyMap.Inst.Nodes.Count)
                // Guess we're in a special node?
                return ObjectiveStatus.Ongoing;
                // WARNING: since we don't know where we are on the map, we'll keep running this check each step, which... shouldn't really ding performance,
                // but is a bit messy

            var info = GalaxyMap.Inst.Nodes[index];

            if (SatisfiesConditions(info))
                // We're good
                return ObjectiveStatus.Completed;

            // Mission ongoing, point us at the nearest node that satisfies our conditions
            var candidates = GalaxyMap.Inst.Nodes.Enumerate().Where(t => SatisfiesConditions(t.Item2));
            if (candidates.Count() == 0)
                // No such node in the known galaxy
                return ObjectiveStatus.Failed;

            _currentTarget = candidates.Minimal(n => (n.Item2.Position - info.Position).LengthSquared()).Item1;

            return ObjectiveStatus.Ongoing;
        }

        public override List<int> GetTargetNodeIndices() {
            return _currentTarget >= 0 ? new List<int> { _currentTarget } : null;
        }

        public override void OnEnterNode() {
            // Reset target, recompute (or check if succeeded) next CheckStatus() call
            _currentTarget = -1;
        }

        bool SatisfiesConditions(NodeInfo nodeInfo) {
            return (Type ?? nodeInfo.Type) == nodeInfo.Type;
        }

    }

}
