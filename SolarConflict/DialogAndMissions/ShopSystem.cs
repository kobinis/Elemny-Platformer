using System;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.GameContent;
using SolarConflict.Framework.Scenes.Activitys.Shop;
using XnaUtils.Graphics;
using SolarConflict.GameContent.Activities.SceneActivitys;

namespace SolarConflict.Framework.Agents.Systems
{
    /// <summary>
    /// A system to allow agents to buy and sell items
    /// </summary>
    [Serializable]
    public class ShopSystem : AgentSystem, IInteractionSystem
    {
                        
        public ShopData shopData;
        private int _randomSeedOffset;
        private float _range;

        public ShopSystem()
        {
            //shopActivityProvider = new ShopActivityProvider();
            _randomSeedOffset = 0;
            _range = 50;            
            shopData = new ShopData();
            shopData.Inventory = new Inventory(9 * 4);
            shopData.Portrait = Sprite.Get(Consts.AI_HELPER_TEXTURE_ID);
        }

        public void AddItem(string itemID, float probability = 1)
        {
            shopData.Inventory.AddItem(itemID);//, probability);  //Fix this shit          
        }

        public void AddItems(string itemsAndProb)
        {
            var pairs = StringUtils.ParseStringFloatPair(itemsAndProb);
            foreach (var item in pairs)
            {
                AddItem(item.Item1, item.Item2);
            }       
                      
        }

        public void AddItemsFromAsset(string assetID)
        {
            AddItems(TextBank.Inst.GetString(assetID));
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if (_randomSeedOffset == 0)
            {
                _randomSeedOffset = gameEngine.Rand.Next(1000000) + 1;
            }

            return DialogUpdate(agent, gameEngine);            
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }


        private bool DialogUpdate(Agent agent, GameEngine gameEngine)
        {
            bool wasActivated = false;
            GameObject target = gameEngine.Scene.FindPlayer();
            if (target != null && target.GetControlType() == AgentControlType.Player && GameObject.DistanceFromEdge(target, agent) < _range + target.Size) //change
            {
                //parse massage
               // _messageID = gameEngine.Scene.DialogManager.AddDialogBox("Press " + Color.Yellow.ToTag("F") + " to dock", SellerImageID, boxID: _messageID, maxLifetime: 10, isFixedSize:false);
                if (gameEngine.Scene.PlayersManager.players[0].IsCommandClicked(PlayerCommand.Use)) //Fix Docking
                {                    
                    if (target.GetControlType() == AgentControlType.Player)
                    {
                        Interact(agent, gameEngine, null);
                        wasActivated = true;
                    }
                }
            }
            return wasActivated;
        }

        private Activity GenerateShopActivity(Agent agent, GameEngine gameEngine)
        {
            return new ShopActivity(shopData, gameEngine.Scene); //shopActivityProvider.ActivityProvider(agent.GetHashCode().ToString());
        }

        public string GetInteractionText(Agent agent, GameEngine gameEngine, Agent playerAgent)
        {
            return "Enter Shop";
        }

        public bool Interact(Agent agent, GameEngine gameEngine, Agent playerAgent)
        {
            ActivityParameters activityParams = new ActivityParameters();
            activityParams.DataParams.Add("Scene", gameEngine.Scene);
            var shopActivity = GenerateShopActivity(agent, gameEngine);
            ActivityManager.Inst.SwitchActivity(shopActivity);
            return true;
        }
    }
}
