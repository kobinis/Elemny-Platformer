using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Generation
{
    class GenerationUtils
    {
       

        public static Agent AddPortal(GameEngine gameEngine, Vector2 position, string activityName, bool isPersistent = true, string text = "Enter")
        {
            Agent portal = ContentBank.Inst.GetEmitter("Portal").Emit(gameEngine, null, Framework.FactionType.Neutral, position, Vector2.Zero, 0) as Agent;
            ActivitySwitcherSystem instituteSystem = new ActivitySwitcherSystem(activityName); 
            if(text != null)
            {
                instituteSystem.InteractionText = text;
            }
            portal.AddSystem(instituteSystem);
            return portal;
        }

        public static Agent AddArena(GameEngine gameEngine, Vector2 position, string activityName, bool isPersistent = true)
        {
            //Agent portal = ContentBank.Inst.GetEmitter("Portal").Emit(gameEngine, null, Framework.FactionType.Neutral, position, Vector2.Zero, 0) as Agent;
            //InstituteSystem instituteSystem = new InstituteSystem();
            //instituteSystem.ActivityName = activityName;
            //portal.AddSystem(instituteSystem);
            //return portal;
            return null;
        }

        public static Item MakePortalItem(string id, string activityName, string activityParams, int? sceneId, Color color, string name, string text, Activity activity = null)
        {
            ItemProfile profile = ItemQuickStart.Profile(name, text, 0, "portalItem", null);
            profile.Id = id;
            profile.SlotType = SlotType.None;
            profile.ItemSize = SizeType.Small;            
            profile.IsActivatable = true;
            profile.TextureColor = color;
            profile.BuyPrice = 50;
            profile.SellPrice = 50;
            profile.MaxStack = 1;
            Item item = new Item(profile);
            ActivitySwitcherSystem system;
            
            if (activity != null)
            {
                system = new ActivitySwitcherSystem(activity);
            }
            else
            {
                system = new ActivitySwitcherSystem(activityName);
            }
            system.SceneIndex = sceneId;

            system.ActivityParams = activityParams;
            
            system.Persistent = false;
            item.System = system;
            return item;
        }

        public static Item MakeCompassItem(GameObject goal, int sceneId, Color color, string name, string text)
        {
            ItemProfile profile = ItemQuickStart.Profile(name, text, 0, "comp", null);
            profile.Id = "GoalTo" + goal.GetHashCode().ToString(); //TODO cahnge, not good???
            profile.SlotType = SlotType.None;
            profile.ItemSize = SizeType.Small;
            profile.IsWorkingInInventory = true;
            profile.IsActivatable = true;
            profile.TextureColor = color;
            profile.BuyPrice = 50;
            profile.SellPrice = 50;
            profile.MaxStack = 999;
            Item item = new Item(profile);
            // GoalChanger gc =new GoalChanger(goal, sceneId);
            //  gc.arrowColor = color;
            //     item.MainSystem = gc;
            return item;
        }

        //public static Agent MakeStarport(List<string> loadouts)
        //{

        //}


    }
}
