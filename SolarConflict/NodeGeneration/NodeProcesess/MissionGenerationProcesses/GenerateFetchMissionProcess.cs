using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.NodeGeneration.NodeProcesess
{
    [Serializable]
    class GenerateFetchMissionProcess : GameProcess
    {
        public GameObject MissionGiver;

        public GenerateFetchMissionProcess(GameObject missionGiver)
        {
            MissionGiver = missionGiver;
        }

        /// <returns>true on failure</returns>
        bool CreateMission(Scene scene)
        {
            // Roughly how valuable should the request item/s be?
            var level = Math.Max( scene.GameEngine.Level,1);
            var value = ScalingUtils.ScaleCost(level);

            // Select random affordable item in the scene's tier
            var validItems = ContentBank.Inst.GetAllItems().Where(i => i.Profile.Level == level);
            if (validItems.Count() == 0)
                // No such items
                return true;

            var item = validItems.Choice(scene.GameEngine.Rand);


            // Pick amount
            var amount = (int)Math.Round(((float)value) / item.Profile.BuyPrice);
            if ((item.Category & ItemCategory.Material) == 0)
                amount = 1;


            // Generate mission
            var cargoDescription = amount == 1 ? item.IconTag + item.Tag : $"{amount} {item.IconTag}{item.Tag}s";
            var cargoPronoun = amount == 1 ? "it" : "them";
            var mission = new Mission($"Acquire {item.IconTag}");

            mission.Faction = MissionGiver.GetFactionType();
            mission.Description = new TextAsset($"Acquire {cargoDescription} and deliver to {MissionGiver.Name}");
            mission.Icon = MissionGiver.GetSprite();


            mission.Reward = new Reward((int)(item.Profile.BuyPrice * amount));
            var rewardList = ContentBank.Inst.GetAllItems().Where(i => i.Profile.Level == level + 1).Where(i => (i.Profile.Category & ItemCategory.CraftingMaterial) > 0).ToList(); //Save
            if (rewardList.Count > 0)
            {
                Item rewardItem = rewardList[FMath.Rand.Next(rewardList.Count)];
                int amout = 1;
                if (rewardItem.Profile.BuyPrice > 0)
                {
                    amout = Math.Max((int)Math.Ceiling(mission.Reward.Money / 2 / rewardItem.Profile.BuyPrice), 1);
                }
                if (mission.Faction != FactionType.Neutral)
                    mission.Reward.ReputationDelta = 0.1f;
                mission.Reward.Items.Add(new Tuple<string, int>(rewardItem.ID, amout));
            }
            mission.DialogOnComplete = new Dialog("Thank you for the "+ item.Tag + item.IconTag+ "\n#line{}\n" + mission.Reward.GetTag(), MissionGiver.GetSprite().ID);
            

            mission.Objective = new ObjectiveGroup();

            mission.AddObjective(new HoldInInventoryObjective(item.Profile.Id, amount));
            mission.AddObjective(new GoToTargetObjective(MissionGiver));
            //mission.AddObjective(new GoToPositionObjective(MissionGiverPosition));zz

            mission.IsDismissable = true;

            scene.AddMissionGenerator(mission);

            // KLUDGE: redundantly store mission state in data field, because ObjectiveGroup is apparently designed to strongly discourage inspecting
            // its subobjectives
            mission.Data = new string[] { item.Profile.Id, amount.ToString() };
            mission.Color = Color.Purple;
            mission.OnMissionCompletion += (msn, scn) =>
            {
                try
                {
                    var data = msn.Data as string[];
                    var itemId = data[0];
                    var removed = scn.PlayerAgent.Inventory.RemoveItem(data[0], int.Parse(data[1]));
                   // Debug.Assert(removed, "Completed fetch quest, but fetched items not found in player inventory");
                }
                catch (Exception)
                {


                }

            };

            return false;
        }

        public override GameProcess GetWorkingCopy()
        {
            return MemberwiseClone() as GameProcess;
        }

        public override void Update(GameEngine gameEngine)
        {
            CreateMission(gameEngine.Scene);

            Finished = true;
        }
    }
}
