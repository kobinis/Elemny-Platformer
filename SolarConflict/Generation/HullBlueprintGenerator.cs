using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Generation
{
    class HullBlueprintGenerator
    {
        private static float blueprintCostMultiplier = 0.1f;        

        public static void Generate()
        {
            var agents = ContentBank.Inst.GetAllAgents();
            foreach (var agent in agents)
            {
                if(agent.gameObjectType.HasFlag(GameObjectType.Ship))
                {
                    ContentBank.Inst.AddContent(MakeHullPrint(agent));
                }
            }
        }

        public static Item MakeHullPrint(Agent agent)
        {
            ItemProfile profile = ItemQuickStart.Profile(agent.GetSprite().ToTag() + agent.ID + " Blueprint Part", "Collect parts to unlock the hull", 6, "blueprintpart");
            profile.IconSecondarySprite = agent.GetSprite();
            profile.BuyPrice = agent.HullCost * blueprintCostMultiplier;
            profile.SellPrice = 10;
            profile.Id = agent.ID + "BlueprintPart";
            profile.MaxStack = 999;
            profile.IsActivatable = false;
            profile.IsWorkingInInventory = true;
            profile.IsConsumed = true;
            profile.Category = ItemCategory.Blueprint;

            profile.Category = ItemCategory.None;
            Item item = new Item(profile);
            item.System = new BlueprintConsumeSystem(agent.ID);
            return item;
        }
    }
}
