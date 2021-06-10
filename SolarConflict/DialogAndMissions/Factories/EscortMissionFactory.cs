//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.Generation;
//using SolarConflict.Session.World.MissionManagment;
//using SolarConflict.Session.World.MissionManagment.Objectives;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [Serializable]
//    class EscortMissionFactory : IMissionFactory
//    {                

//        public EscortMissionFactory()
//        {
//        }

//        public int NumOfMissionsToGenerate { get; set; }

//        /// <returns>true on failure</returns>
//        public int CrateMissionGenerator(Scene scene, Agent agent, List<GameObject> targetList, List<IMissionGenerator> newMissionsList, int numOfMissionsToAdd = 0)
//        {
//            if (numOfMissionsToAdd == 0)
//                numOfMissionsToAdd = NumOfMissionsToGenerate;
//            var faction = scene.GameEngine.GetFaction(agent.GetFactionType());

//            var legalTargets = scene.GameEngine.PotentialTargets.Where(t => IsLegalTarget(t, faction, scene.GameEngine.Level));

//            if (legalTargets.FirstOrDefault() == null)
//                return 0;    // No legal targets

//            // Choose one
//            var shuffledTargets = legalTargets.Shuffled(scene.GameEngine.Rand).ToArray();
//            if (numOfMissionsToAdd == 0)
//                numOfMissionsToAdd = shuffledTargets.Length;
//            int counter = 0;
//            for (int i = 0; i < Math.Min(numOfMissionsToAdd, shuffledTargets.Length); i++)
//            {
//                var mission = CreateDestroyTargetMission(scene, agent, shuffledTargets[i]);
//                if (mission != null)
//                {
//                    counter++;
//                    newMissionsList.Add(mission);
//                }
//            }
//            //var target = legalTargets.Choice(scene.GameEngine.Rand) as Agent; // IsLegalTarget checks that all targets in lists are Agents
//            return counter;
//        }

//        public Mission Create(Scene scene, Agent giver, GameObject target)
//        {
//            Agent agentTarget = target as Agent;
//            if (agentTarget == null)
//                return null;

//            var ch = giver.GetCharacter(scene);
//            // Generate mission
//            var mission = new Mission($"{GetPrefix(scene.GameEngine.Rand)} {agentTarget.Name}");
//            mission.Icon = Sprite.Get(ch.SpriteID);
//            mission.Faction = giver.GetFactionType();
//            mission.Color = Color.Red;
//            mission.Description = $"Hunt down and eliminate {agentTarget.FactionType}'s {agentTarget.Name}, {GetCause(agentTarget, scene.GameEngine.Rand)}";
//            int cost = (int)agentTarget.GetCost() / 2;
//            mission.Reward = new Reward()
//            {
//                Money = cost / 2,
//                ReputationDelta = 0.1f
//            };

//            mission.Objective = new DestroyByPlayerFactionObjective(agentTarget);

//            mission.MoneyReward = (int)agentTarget.GetCost();
//            mission.IsDismissable = true;
//            mission.DialogOnCompleteID = "glSuccess";
//            mission.ID = "_d" + agentTarget.ID;
//            return mission;
//        }
                
//    }
//}
