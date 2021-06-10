//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Activities;
//using SolarConflict.GameContent.Activities.SceneActivitys;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.Session.World.MissionManagment.Objectives
//{
//    [Serializable]
//    class AcquireItemInMothershipObjective : MissionObjective
//    {
//        public string ItemID { set { item = ContentBank.Inst.GetItem(value, false); } }
//        public int Amount = 1;
//        public bool NeedsToBeEquipped = false;

//        public TutorialGoal TutorialGoal { get; set; }

//        private Item item;

//        public AcquireItemInMothershipObjective(string itemID, int amount = 1, bool needsToBeEquipped = false, TutorialGoal tutorialGoal = null)
//        {
//            ItemID = itemID;
//            Amount = amount;
//            NeedsToBeEquipped = needsToBeEquipped;
//            TutorialGoal = tutorialGoal;
//        }

//        public override string GetObjectiveText()
//        {
//            return GetStatusTag() + (NeedsToBeEquipped ? "Equip " : "Acquire ") + Amount + " x " + item.Tag;
//        }

//        public override Vector2? GetPosition()
//        {
//            return null;
//        }

//        public override float GetRadius()
//        {
//            return 0;
//        }


//        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
//        {
//            Status = ObjectiveStatus.Ongoing;
//            int count = 0;
//            var agentToChecl = mission.Agent;
//            if (agentToChecl != null && agentToChecl.IsActive)
//            {
//                if (agentToChecl.ItemSlotsContainer != null)
//                {
//                    foreach (var itemSlot in agentToChecl.ItemSlotsContainer)
//                    {
//                        if (itemSlot.Item != null && itemSlot.Item.ID == item.ID)
//                        {
//                            count += itemSlot.Item.Stack;
//                        }
//                    }
//                }

//                if (count >= Amount)
//                {
//                    Status = ObjectiveStatus.Completed;
//                    return Status;
//                }

//                if (!NeedsToBeEquipped && agentToChecl.Inventory != null)
//                {
//                    if (agentToChecl.Inventory.CheckForItem(item.ID, Amount - count))
//                    {
//                        Status = ObjectiveStatus.Completed;
//                        return Status;
//                    }
//                }
//            }

//            return Status;
//        }

//        public override List<TutorialGoal> GetTutorialGoals()
//        {
//            var list = new List<TutorialGoal>(1);
//            list.Add(TutorialGoal);
//            return list;
//        }

//    }
//}

