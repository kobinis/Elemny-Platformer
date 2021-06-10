using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.World.Generation;
using SolarConflict.GameWorld;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SolarConflict.NodeGeneration.Features {
    
    class RaceFeature : GenerationFeature {
        
        float _baseSpeed;
        int _numIntermediateWaypoints;
        //GenerationFeature[] _waypoints;

        public override float Priority => 200f;

        /// <param name="baseSpeed">Speed at which a tier 0 version of this mission expects the player to travel between waypoints, in units/s</param>
        //public RaceFeature(float baseSpeed, params GenerationFeature[] waypoints) {
        public RaceFeature(float baseSpeed, int numIntermediateWaypoints) {
            _baseSpeed = baseSpeed;
            //_waypoints = waypoints;
            _numIntermediateWaypoints = numIntermediateWaypoints;
        }

        //public override GameObject GenerationLogic(Scene scene, SceneGenerator generator) {
        //    var possibleWaypoints = generator.Zones.Where(z => z.Flags.HasFlag(SceneGenerator.ZoneFlags.IsRaceTarget)).Select(z => z.GameObject);
            
        //    var numwaypoints = Math.Min(2 + _numIntermediateWaypoints, possibleWaypoints.Count());

        //    var waypoints = possibleWaypoints.Choices(numwaypoints, scene.GameEngine.Rand);

        //    // Create a chain of missions to visit all the waypoints
        //    var missions = Utility.Range(0, waypoints.Count())
        //        .Select(i => {
        //            var waypointsRemaining = waypoints.Count() - i;
        //            var suffix = waypointsRemaining > 1 ? $" ({waypointsRemaining} more stops)" : "";
        //            return MissionFactory.GoToTargetMission(waypoints[i], $"Deliver the thing{suffix}", $"Y-you gotta deliver the thing to the place");
        //        }).ToArray();
        //    for (int i = 0; i < missions.Length - 1; ++i)
        //        missions[i].NextMissionOnComplete = missions[i + 1];

        //    // Hide the first mission (player becomes aware of the chain when reaching the first objective)
        //    missions[0].IsHidden = true;
        //    scene.AddMission(missions[0]);

        //    // Set time limits
        //    var speed = ScalingUtils.ScaleEngineSpeed(Level);
            
        //    for (int i = 1; i < missions.Length; ++i) {
        //        var distance = (waypoints[i - 1].Position - waypoints[i].Position).Length();
        //        var time = distance / speed;
        //        var group = new ObjectiveGroup();

        //        group.AddObjective(missions[i].Objective);
        //        group.AddObjective(new TimeObjective(time));
        //        group.AddObjective(new SurviveObjective());

        //        missions[i].Objective = group;
        //    }

        //    return null;
        //}
    }
}
