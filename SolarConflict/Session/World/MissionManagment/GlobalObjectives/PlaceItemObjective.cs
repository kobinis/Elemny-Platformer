using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Activities;
using SolarConflict.GameContent.Activities.SceneActivitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    /// <summary>
    /// Place item in a spesific inventory slot
    /// </summary>
    [Serializable]
    class PlaceItemObjective : MissionObjective
    {
        public string ItemID { set { item = ContentBank.Inst.GetItem(value, false); } }
       // public int Amount = 1;
        public int DestIndex;

        private Item item;
        public ObjectiveStatus StatusOnNotAcquired;

        public PlaceItemObjective(string itemID, int destIndex, ObjectiveStatus statusOnNotAcquired = ObjectiveStatus.Ongoing)
        {
            ItemID = itemID;
         //   Amount = amount;
            DestIndex = destIndex;
            StatusOnNotAcquired = statusOnNotAcquired;
        }

        //public override string GetObjectiveText()
        //{
        //    return "Place " + GetStatusTag() + (NeedsToBeEquipped ? "Equip " : "Acquire ") + Amount + " x " + item.IconTag + item.Tag;
        //}

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
            var player = mission.Agent; //??S
            if (player != null && player.IsActive)
            {
                if (player.Inventory != null)
                {
                    if (player.Inventory.GetItem(0)?.ID == item.ID)
                    {
                        Status = ObjectiveStatus.Completed;
                        return Status;
                    }
                }
            }

            return Status;
        }

    }
}
