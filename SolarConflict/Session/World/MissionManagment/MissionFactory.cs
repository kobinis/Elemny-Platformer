using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Emitters.SceneRelated;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.InGameEvent.Content;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment
{
    public class MissionFactory
    {
        //baseID: - if present will be used for mission title and icon from portraitID, if not mission will be hidden
        //baseIDInfo: description of the mission
        //baseIDStart1: the dialog in the start of a mission
        //baseIDEnd1: the dialog for the mission completion
        //baseIDFail1: the dialog for mission 
        public static Mission MissionQuickStart(string baseID, bool isGlobal = false)
        {
            //TextBank.Inst.TryLinkTextAssets(baseID + "Start"); 
            //TextBank.Inst.TryLinkTextAssets(baseID + "End");
            //TextBank.Inst.TryLinkTextAssets(baseID + "Fail");
            Dialog titleAsset = TextBank.Inst.TryGetDialogNode(baseID);
            Mission mission = new Mission();
            mission.IsGlobal = isGlobal;
            mission.ID = baseID;
            if (titleAsset != null)
            {
                mission.Title = titleAsset.Text;
                mission.Icon = Sprite.Get(titleAsset.ImageID);
                
            }
            else
                mission.IsHidden = true;

            mission.ActiveTitle = TextBank.Inst.TryGetTextAsset(baseID + "ActiveTitle");
            mission.Description = TextBank.Inst.TryGetTextAsset(baseID+"Info");
            mission.ActiveDescription = TextBank.Inst.TryGetTextAsset(baseID + "ActiveInfo");
            mission.DialogOnStart = TextBank.Inst.TryGetDialogNode(baseID + "Start1");
            if (mission.Description == null && mission.DialogOnStart != null)
                mission.Description = TextBank.Inst.GetTextAsset(baseID + "Start1");
            mission.DialogOnComplete = TextBank.Inst.TryGetDialogNode(baseID + "End1");
            mission.DialogOnFail = TextBank.Inst.TryGetDialogNode(baseID + "Fail1");
            mission.Objective = new EmptyObjective();
            mission.Color = Palette.MainMissionColor;
            return mission;
        }


        //public static Mission 
                        

        public static void ObtainMaterialsForItem(string itemID, Mission mission, string materialId = null, MissionObjective.ObjectiveStatus statusOnNotObtiend = MissionObjective.ObjectiveStatus.Ongoing, bool isNotNeeded = false, int num = 1)
        {
            var recipe = ContentBank.Inst.GetEmitter(itemID + "Recipe") as Recipe;
            var item = ContentBank.Inst.GetItem(itemID, false);
            if (recipe != null) {
                //Mission equipItem = MissionFactory.GenericMission("Obtain need materials for" + item.Name);                
                ObjectiveGroup objective = mission.Objective as ObjectiveGroup;
                if (objective == null)
                    objective = new ObjectiveGroup();

                if (materialId != null) {
                    Debug.Assert(recipe.CraftingCostList.Any(c => c.ItemNeeded.Id == materialId), "Mission to acquire crafting material: recipe doesn't require material");
                    var amount = recipe.CraftingCostList.First(c => c.ItemNeeded.Id == materialId).Amount * num;
                    objective.AddObjective(new AcquireItemObjective(materialId, amount, false, statusOnNotAcquired: statusOnNotObtiend), isNotNeeded);
                } else
                    foreach (var neededMat in recipe.CraftingCostList) {
                        objective.AddObjective(new AcquireItemObjective(neededMat.ItemNeeded.Id, neededMat.Amount * num, false, statusOnNotAcquired: statusOnNotObtiend), isNotNeeded);
                    }
                mission.Objective = objective;
            }
        }


        public static Mission DestroyTargetObjective(GameObject target, string title = null, string description = null)
        {
            Mission mission = new Mission();
            mission.Title = title;
            mission.Description = new TextAsset(description);
            mission.Icon = target.GetSprite(); 
            mission.Objective = new DestroyTargetObjective(target);
            mission.Color = Color.Red;
            return mission;
        }


        public static Mission PlayerDeathLocationMission(Agent player)
        {
            Mission mission = new Mission();
            mission.ID = "pd";
            mission.IsOverrideID = true;
            mission.Title = "Pick up the pieces";
            mission.Description = new TextAsset("Points you to a point your ship got destroyed");
            mission.Icon = player.GetSprite();
            mission.Color = Color.Gray;
            mission.IsDismissable = true;
            mission.MissionCompleteText = null;
            mission.Objective = new GoToPositionObjective(player.Position, 1000);
            return mission;
        }

        public static Mission MapMarker(Vector2 position)
        {
            Mission mission = new Mission();
            mission.ID = TacticalMapActivity.MARKER_MISSION_ID;
            mission.IsOverrideID = true;
            mission.Title = "Map marker";
            mission.Description = new TextAsset("Points you to the a location");
            mission.Icon = Sprite.Get("goalmap");
            mission.Color = Color.Blue;
            mission.IsDismissable = true;
            mission.MissionCompleteText = null;
            mission.Objective = new GoToPositionObjective(position, 400);
            return mission;
        }
        

        public static Mission EnteringInterStllerSpace(float radius)
        {
            Mission mission = new Mission("Enter Interstellar space");
            mission.Objective = new GoOutOfPositionObjective(Vector2.Zero, radius);
            mission.DialogOnComplete = new Dialog(Color.Red.ToTag("You are entering interstellar space, danger of pirates!"), null, false);
            mission.EmitterOnComplete = GameEventFactory.MakeAgentSpawnerProccess("bala",radius);
           // mission.ID = 
            return mission;
        }
    }
}
