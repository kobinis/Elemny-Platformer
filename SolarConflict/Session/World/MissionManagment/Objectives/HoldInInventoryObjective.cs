
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
    class HoldInInventoryObjective : MissionObjective
    {
        public string ItemID { set { item = ContentBank.Inst.GetItem(value, false); } }
        public int Amount = 1;

        public TutorialGoal TutorialGoal { get; set; }

        private Item item;
        public ObjectiveStatus StatusOnNotAcquired;

        public HoldInInventoryObjective(string itemID, int amount = 1, TutorialGoal tutorialGoal = null, ObjectiveStatus statusOnNotAcquired = ObjectiveStatus.Ongoing)
        {
            ItemID = itemID;
            Amount = amount;
            TutorialGoal = tutorialGoal;
            StatusOnNotAcquired = statusOnNotAcquired;
        }

        public override string GetObjectiveText()
        {
            return GetStatusTag() + "Acquire " + Amount + " x " + item.IconTag + item.Tag;
        }

        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return 0;
        }


        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = StatusOnNotAcquired;
            int count = 0;
            var player = mission.Agent;
            if (player != null && player.IsActive && player.Inventory != null)
            {
                if (player.Inventory.CheckForItem(item.ID, Amount - count))
                {
                    Status = ObjectiveStatus.Completed;
                    return Status;
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
