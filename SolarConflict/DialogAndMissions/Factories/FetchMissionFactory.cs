//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.Generation;
//using SolarConflict.Session.World.MissionManagment;
//using SolarConflict.Session.World.MissionManagment.Objectives;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using XnaUtils;

//namespace SolarConflict.NodeGeneration.NodeProcesess
//{
//    [Serializable]
//    class FetchMissionFactory : IMissionFactory
//    {
//        private Agent _missionGiver;

//        public int NumOfMissionsToGenerate { get; set; }

//        public FetchMissionFactory(Agent missionGiver)
//        {
//            _missionGiver = missionGiver;
//        }

//        public int CrateMissionGenerator(Scene scene, Agent agent, List<GameObject> targetList, List<IMissionGenerator> newMissionsList, int numOfMissionsToAdd = 0)
//        {
//            throw new NotImplementedException();
//        }

//        /// <returns>true on failure</returns>
//        private IMissionGenerator CreateMission(Scene scene, Agent aggiverAgentnt, List<GameObject> targetList)
//        {
//            // Roughly how valuable should the request item/s be?
//            var level = scene.GameEngine.Level;
//            var value = ScalingUtils.ScaleCost(level);

//            // Select random affordable item in the scene's tier
//            var validItems = ContentBank.Inst.GetAllItems().Where(i => i.Profile.Level == level);
//            if (validItems.Count() == 0)
//                // No such items
//                return null;

//            var item = validItems.Choice(scene.GameEngine.Rand);


//            // Pick amount
//            var amount = (int)Math.Round(((float)value) / item.Profile.BuyPrice);
//            if ((item.Category & ItemCategory.Material) == 0)
//                amount = 1;


//            // Generate mission
//            var cargoDescription = amount == 1 ? item.IconTag + item.Tag : $"{amount} {item.IconTag}{item.Tag}s";
//            var cargoPronoun = amount == 1 ? "it" : "them";
//            var mission = new Mission($"Acquire {item.IconTag}");

//            mission.Faction = MissionGiverFaction;
//            mission.Description = $"Acquire {cargoDescription}"// and deliver {cargoPronoun} to {MissionGiverPosition * 0.01f}\n"
//                + $"\nReward: {item.Profile.BuyPrice * amount}" + "#image{coin}";
//            mission.MoneyReward = (int)(item.Profile.BuyPrice * amount);

//            mission.Objective = new ObjectiveGroup();

//            mission.AddObjective(new AcquireItemObjective(item.Profile.Id, amount));
//            //mission.AddObjective(new GoToPositionObjective(MissionGiverPosition));zz

//            mission.IsDismissable = true;

//            scene.AddMissionGenerator(mission);

//            // KLUDGE: redundantly store mission state in data field, because ObjectiveGroup is apparently designed to strongly discourage inspecting
//            // its subobjectives
//            mission.Data = new string[] { item.Profile.Id, amount.ToString() };
//            mission.Color = Color.Purple;
//            mission.OnMissionCompletion += (msn, scn) =>
//            {
//                try
//                {
//                    var data = msn.Data as string[];
//                    var itemId = data[0];
//                    var removed = scn.PlayerAgent.Inventory.RemoveItem(data[0], int.Parse(data[1]));
//                    Debug.Assert(removed, "Completed fetch quest, but fetched items not found in player inventory");
//                }
//                catch (Exception)
//                {


//                }

//            };

//            return false;
//        }
//    }
//}
