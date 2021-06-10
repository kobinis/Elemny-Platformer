using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict {
    [Serializable]
    class DestroyMissionFactory : IMissionFactory
    {

        readonly string[] PREFIXES = new string[] { "Destroy", "Eliminate", "Annihilate", "Obliterate" };

        readonly string[] CAUSES = new string[] { "they are a threat to our interests in this sector", "because we don't like them", "kay?" };

        public int MinimalCost = 0;
        
        public DestroyMissionFactory()
        {
        }

        public int NumOfMissionsToGenerate { get; set; }

        /// <returns>true on failure</returns>
        public int CrateMissionGenerator(Scene scene, Agent agent, List<GameObject> targetList, List<IMissionGenerator> newMissionsList, int numOfMissionsToAdd = 0)
        {
            if (numOfMissionsToAdd == 0)
                numOfMissionsToAdd = NumOfMissionsToGenerate;
            var faction = scene.GameEngine.GetFaction(agent.GetFactionType());
            
            var legalTargets = scene.GameEngine._collideAllCheckList.Where(t => IsLegalTarget(t, faction, scene.GameEngine.Level));

            if (legalTargets.FirstOrDefault() == null)            
                return 0;    // No legal targets

            // Choose one
            var shuffledTargets = legalTargets.Shuffled(scene.GameEngine.Rand).ToArray();
            if (numOfMissionsToAdd == 0)
                numOfMissionsToAdd = shuffledTargets.Length;
            int counter = 0;
            for (int i = 0; i < Math.Min(numOfMissionsToAdd, shuffledTargets.Length); i++)
            {
                var mission = CreateDestroyTargetMission(scene, agent, shuffledTargets[i]);
                if(mission != null)
                {
                    counter++;
                    newMissionsList.Add(mission);
                }
            }
            //var target = legalTargets.Choice(scene.GameEngine.Rand) as Agent; // IsLegalTarget checks that all targets in lists are Agents
            return counter;
        }

        public Mission CreateDestroyTargetMission(Scene scene, Agent giver, GameObject target)
        {
            Agent agentTarget = target as Agent;
            if (agentTarget == null)
                return null;

            var ch = giver.GetCharacter(scene);
            // Generate mission
            var mission = new Mission($"{GetPrefix(scene.GameEngine.Rand)} {agentTarget.Name}");
            mission.Icon = Sprite.Get(ch.SpriteID);
            mission.Faction = giver.GetFactionType();
            mission.Color = Color.Red;
            mission.Description = new TextAsset($"Hunt down and eliminate {agentTarget.FactionType}'s {agentTarget.Name}, {GetCause(agentTarget, scene.GameEngine.Rand)}");
            int cost = (int)agentTarget.GetCost() / 2;

            mission.Reward = new Reward(Math.Max(cost / 2, 100));
            var rewardList = ContentBank.Inst.GetAllItems().Where(i => i.Profile.Level  == agentTarget.Level + 1).Where(i => (i.Profile.Category & (ItemCategory.CraftingMaterial | ItemCategory.EnergyConsumingWeapon)) > 0).ToList(); //Save
            if (rewardList.Count > 0)
            {
                Item rewardItem = rewardList[FMath.Rand.Next(rewardList.Count)];
                int amout = 1;
                if (rewardItem.Profile.BuyPrice > 0)
                {
                    amout = Math.Max((int)Math.Ceiling(mission.Reward.Money / rewardItem.Profile.BuyPrice), 1);
                }
                mission.Reward.Items.Add(new Tuple<string, int>(rewardItem.ID, amout));
            }

            if (mission.Faction != FactionType.Neutral)
                mission.Reward.ReputationDelta = 0.1f;

            mission.Objective = new DestroyByPlayerFactionObjective(agentTarget);
            
            mission.IsDismissable = true;
            mission.DialogOnComplete = new Dialog("Mission completed\n#line{}\n" + mission.Reward.GetTag());
            mission.ID = "_d" + agentTarget.ID;
            return mission;
        }

        private string GetPrefix(Random random)
        {
            return PREFIXES.Choice(random);
        }

        private string GetCause(Agent target, Random random)
        {
            return CAUSES.Choice(random);
        }


        bool IsLegalTarget(GameObject obj, Faction missionGiverFaction, int level) {
            if (!(obj is Agent))
                return false;
            var agent = obj as Agent;
            return missionGiverFaction.IsHostile(agent.GetFactionType()) && (MinimalCost == 0 || agent.GetCost() >= MinimalCost);
        }        
    }
}
