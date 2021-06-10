//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Utils;
//using SolarConflict.Generation;
//using SolarConflict.Session.World.MissionManagment;
//using SolarConflict.Session.World.MissionManagment.Objectives;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.NodeGeneration.NodeProcesess {

//    /// <remarks>TODO: ensure can't cheat by switching ships (make it an actual delivery mission; item is dropped on death)
//    /// 
//    /// Note that we presently assume both the mission giver and target are stationary, which is bad</remarks>
//    class GenerateDeliveryMissionProcess : GameProcess {

//        readonly string[] PREFIXES = new string[] { "Deliver this widget to ", "Send a message to ", "Meet up with ", "You have just GOT to talk to " };

//        /// <summary>TEMP KLUDGE, we should get this from the mission giver, a specific Agent associated with this mission</summary>
//        public FactionType MissionGiverFaction;

//        /// <summary>TEMP KLUDGE, we should get this from the mission giver, a specific Agent associated with this mission</summary>
//        public GameObject MissionGiver;

//        /// <summary>We won't generate missions for which the estimated travel time is more than this</summary>
//        /// <remarks>In seconds</remarks>
//        public float MaxEstimatedTime;

//        /// <summary>We won't generate missions for which the estimated travel time is less than this</summary>
//        /// <remarks>In seconds</remarks>
//        public float MinEstimatedTime;

//        /// <summary>Mission will fail after estimatedTimeToReachTarget * TimeAllowanceRatio</summary>
//        /// <remarks>Estimated time to reach target is based on typical ship speeds for the mission tier, not on the player's ship</remarks>
//        public float TimeLimitRatio;

//        public GenerateDeliveryMissionProcess(FactionType missionGiverFaction, GameObject missionGiver, float minEstimatedTime = 20f, float maxEstimatedTime = float.PositiveInfinity,
//            float timeLimitRatio = 1.3f) {
//            MissionGiverFaction = missionGiverFaction;
//            MissionGiver = missionGiver;
//            MinEstimatedTime = minEstimatedTime;
//            TimeLimitRatio = timeLimitRatio;
//        }

//        /// <returns>true on failure</returns>
//        bool CreateMission(Scene scene) {
//            try
//            {

//            }
//            catch (Exception)
//            {

//                throw;
//            }
//            var faction = scene.GameEngine.GetFaction(MissionGiverFaction);

//            // Get all legal targets
//            //var hostileFactions = scene.GameEngine.Factions.Where(f => f != null && faction.IsHostile(f.FactionType));
//            //var hostileFleets = hostileFactions.Select(f => f._factionShips).Where(f => f != null);
//            //var legalTargets = hostileFleets.SelectMany(f => f).Where(o => o is Agent).Cast<Agent>()
//            //    .Where(a => IsLegalTarget(a, scene.GameEngine.Level));
//            var legalTargets = scene.GameEngine.PotentialTargets.Where(t => IsLegalTarget(t, faction, scene.GameEngine.Level));

//            if (legalTargets.FirstOrDefault() == null)
//                // No legal targets
//                return true;

//            // Choose one
//            var target = legalTargets.Choice(scene.GameEngine.Rand) as Agent; // TEMP, might not be an Agent

//            // Generate mission
//            var mission = new Mission($"Rendezvous with {target.Name}");

//            var timeLimit = EstimatedTravelTime(target, scene.GameEngine.Level) * TimeLimitRatio;

//            mission.Faction = MissionGiverFaction;
//            mission.Description = $"{GetPrefix(scene.GameEngine.Rand)}{target.Name}";

//            var objective = new ObjectiveGroup();
//            objective.AddObjective(new TimeObjective(timeLimit));
//            objective.AddObjective(new GoToTargetObjective(target, withSameShip: true));

//            mission.AddObjective(objective);

//            mission.IsDismissable = true;

//            scene.AddMissionGenerator(mission);

//            return false;
//        }

//        /// <remarks>In seconds</remarks>
//        float EstimatedTravelTime(GameObject target, int level) {

//            return ((target.Position - MissionGiver.Position).Length() / ScalingUtils.ScaleEngineSpeed(level)) / Utility.FramesPerSecond;
//        }

//        string GetPrefix(Random random) {
//            return PREFIXES.Choice(random);
//        }        

//        public override GameProcess GetWorkingCopy() {
//            return MemberwiseClone() as GameProcess;
//        }

//        bool IsLegalTarget(GameObject obj, Faction missionGiverFaction, int level) {
//            if (!(obj is Agent))
//                return false;
//            var agent = obj as Agent;

//            if (!(missionGiverFaction.IsFriendly(agent.GetFactionType())
//                // KLUDGE: we only select relatively pricy ships, because they're less likely to get destroyed mid-mission
//                && agent.GetCost() >= ScalingUtils.ScaleCost(level) * 6))
//                return false;

//            var estimatedTime = EstimatedTravelTime(agent, level);

//            return (estimatedTime >= MinEstimatedTime && estimatedTime <= MaxEstimatedTime);
//        }

//        public override void Update(GameEngine gameEngine) {
//            CreateMission(gameEngine.Scene);

//            Finished = true;
//        }
//    }
//}
