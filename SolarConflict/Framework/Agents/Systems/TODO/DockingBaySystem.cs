//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using XnaUtils;
//using SolarConflict.Framework.Utils;

//namespace SolarConflict
//{
//    /// <summary>
//    /// DockingBaySystem - emits a loadout from a list, if the resulting game object is killed re constructs it
//    /// </summary>
//    [Serializable]
//    public class DockingBaySystem : AgentSystem //TODO: add money cost
//    {
//        [Serializable]
//        private class LoadoutGameObjectPair //TODO: no need for it/ maybe remove
//        {
//            public AgentLoadout Loadout;
//            public GameObject GameObject;

//            public LoadoutGameObjectPair(AgentLoadout loadout)
//            {
//                Loadout = loadout;
//                GameObject = null;
//            }
//        }

        

//        public ActivationCheck ActivationCheck;
//        public bool IsUsingFactionLoadouts;
//        public int CooldownTime;
//        public int MaxShipsToProduce = 20;
//        /// <remarks>Might wanna make this a mission-changing property, make it a per-squad parameter, and/or make it a versatile Position type (absolute or relative coordinates)</remarks>
//        public Vector2 MusterPoint;
//        public float PriceRatio = 0.1f;
       
              
//        private FactionType _prevFaction;
//        private List<LoadoutGameObjectPair> _exsistingPairs;        
//        /// <summary>Probably redundant, could just check if mission is mustering</summary>
//        HashSet<string> _squadronsToRebuild = new HashSet<string>();
//        private int _cooldown;
//        int _squadTimer = Utility.Frames(2f); // TEMP, mostly pointless, doesn't belong here
//        //ADD construction time

//        public DockingBaySystem()
//        {
//            _prevFaction = FactionType.Neutral;
//            ActivationCheck = new ActivationCheck();
//            _exsistingPairs = new List<LoadoutGameObjectPair>();               
//        }                 

//        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
//        {           
//            ResetLoadouts(agent, gameEngine);
//            bool wasActive = false;            
            
//            // Rebuilding
//            if (_cooldown <= 0 && (ActivationCheck == null || ActivationCheck.Check(agent, tryActivate))) {                
//                // Rebuild destroyed units in existingPairs, if able
//                for (int i = 0; i < Math.Min(_exsistingPairs.Count, MaxShipsToProduce) && _cooldown <= 0; i++) 
//                    if (!TryBuildUnit(agent, gameEngine, initPosition, initRotation, _exsistingPairs[i].Loadout, ref _exsistingPairs[i].GameObject))
//                        wasActive = true;

//                // Ditto for squad units          
//                (gameEngine.GetFaction(agent.faction).SquadManager as AI.SimpleSquadronManager).SquadsToBuild(agent).Do(squad => {
//                    foreach (var sNl in squad.ShipsAndLoadOuts) {
//                        GameObject gameObject = sNl.Item1;
//                        if (!TryBuildUnit(agent, gameEngine, initPosition, initRotation, sNl.Item2, ref gameObject))
//                            wasActive = true;
//                        sNl.Item1 = (Agent)gameObject;
//                    }
//                });                
//            }            

//            _cooldown--;
//            return wasActive;
//        }

//        private void ResetLoadouts(Agent agent, GameEngine gameEngine)
//        {
//            // Load entire faction roster. Note that each of a faction's stations will have its own copy of the roster (so multiple bases will each construct and maintain their own copy of
//            // each unit on the roster)
//            if (_prevFaction != agent.faction && IsUsingFactionLoadouts)
//            {
//                _exsistingPairs.Clear();
//                var loadouts = gameEngine.GetFaction(agent.faction).FactionLoadouts;
//                foreach (var loadout in loadouts)
//                {
//                    _exsistingPairs.Add(new LoadoutGameObjectPair(loadout));
//                }
//                _prevFaction = agent.faction;
//            }            
//        }                

//        public override AgentSystem GetWorkingCopy()
//        {
//            DockingBaySystem system = MemberwiseClone() as DockingBaySystem;
//            system._exsistingPairs = new List<LoadoutGameObjectPair>();
//            system._prevFaction = FactionType.Neutral;
//            return system;           
//        }                                

//        /// <returns>True on failure.</returns>
//        public bool TryBuildUnit(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, AgentLoadout loadout, ref GameObject gameObject) {
//            // Does the unit already exist?
//            if (gameObject?.IsActive == true)            
//                // Yeah. Can't rebuild it, then
//                return true;
            
//            // Do we have our construction cooldown? Can we afford the build?
//            if (loadout.Cost * PriceRatio > agent.GetMeterValue(MeterType.Money) || _cooldown > 0)
//                // Nope
//                return true;

//            //TODO: add cost, add option to release one ship at a time                                                                                                
//            gameObject = loadout.Emit(gameEngine, agent, agent.faction, initPosition, agent.Velocity, initRotation);

//            if (gameObject == null)
//                return true;

//            //Debug.WriteLine($"Built ship, Loadout: {(gameObject as Agent)?.LoadoutID}, Controller: {(gameObject as Agent)?.control.controlAi.GetType()}");
            
//            float price = loadout.Cost * PriceRatio;
//            agent.AddMeterValue(MeterType.Money, -price);
//            ActivationCheck.DrainCost(agent);
//            _cooldown = CooldownTime;

//            return false;
//        }        
//    }
//}
