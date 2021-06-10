using Microsoft.Xna.Framework;
using SolarConflict.Framework.GUI;
using SolarConflict.GameContent.Activities;
using SolarConflict.GameContent.Activities.SceneActivitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    /// <summary>
    /// AcquireItemObjective - Acquire Items
    /// </summary>
    [Serializable]
    class AcquireItemObjective: MissionObjective
    {
        public string ItemID { set { item = ContentBank.Inst.GetItem(value, false); } }
        public int Amount = 1;
        public bool NeedsToBeEquipped = false;
        public Agent MissionAgent;


        public TutorialGoal TutorialGoal { get; set; }

        private Item item;
        public ObjectiveStatus StatusOnNotAcquired;
        private int count;

        public AcquireItemObjective(string itemID, int amount = 1, bool needsToBeEquipped = false, TutorialGoal tutorialGoal =null , ObjectiveStatus statusOnNotAcquired = ObjectiveStatus.Ongoing, Agent agent = null)
        {
            ItemID = itemID;
            Amount = amount;
            NeedsToBeEquipped = needsToBeEquipped;
            TutorialGoal = tutorialGoal;
            StatusOnNotAcquired = statusOnNotAcquired;
            MissionAgent = agent;
        }

        public override string GetActiveText()
        {
            
            return GetStatusTag() + (NeedsToBeEquipped ? "Equip " : Amount > 1 ? (+count+" / "+ Amount + " x "): "Acquire ") + item.IconTag + item.Tag;
        }

        public override string GetObjectiveText()
        {            
            return GetActiveText();
        }

        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return 0;
        }

        public override void Update(Mission mission, Scene scene)
        {
            //base.Update(mission, scene);
            if(scene.GameEngine.FrameCounter % 10 == 0)
            {
                
            }
        }


        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = StatusOnNotAcquired;
            count = 0;            
            var player = mission.Agent;
            if (MissionAgent != null)
                player = MissionAgent;
            if (player != null && player.IsActive)
            {
                if (player.ItemSlotsContainer != null)
                {
                    foreach (var itemSlot in player.ItemSlotsContainer)
                    {
                        if (itemSlot.Item != null && itemSlot.Item.ID == item.ID)
                        {
                            count += itemSlot.Item.Stack;
                        }
                    }
                }

                if(count >= Amount)
                {
                    Status = ObjectiveStatus.Completed;
                    return Status;
                }

                if(!NeedsToBeEquipped && player.Inventory != null)
                {
                    count += player.Inventory.CountItem(item.ID);
                    if (count >= Amount)
                    {
                        Status = ObjectiveStatus.Completed;
                        return Status;
                    }
                }                
            }            

            return Status;
        }

        public override List<TutorialGoal> GetTutorialGoals()
        {
            var list = new List<TutorialGoal>(1);
            list.Add(TutorialGoal);
            return list;
        }

    }
}
