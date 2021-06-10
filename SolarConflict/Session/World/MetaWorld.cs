using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.GameContent.Items;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarConflict
{
    /// <summary>
    /// Holds global game world data (factions)
    /// </summary>
    [Serializable]
    public class MetaWorld
    {
        #region Singleton
        public static MetaWorld Inst
        {
            get
            {
                return Session.GameSession.Inst.MetaWorld;
            }
        }
        #endregion

        public MissionManager GlobalMissionManager { get; private set; }
        public NewsManager NewsManager { get; private set; }
        public Dictionary<FactionType, Faction> Factions;
        public Dictionary<string, string> Blackboard;
        public CodexManager CodexManager;

        private HashSet<string> _knownRecipes;

        //  public  CharacterBank CharacterBank { get;}

        private GameObject _playerShip; //???
        public GameObject PlayerShip
        {
            get { return _playerShip; }
            set { _playerShip = value; }
        }

        private StarDate _worldDate;
        public StarDate Stardate { get { return _worldDate; } }

        public string _tempName;

        public MetaWorld()
        {            
            Blackboard = new Dictionary<string, string>();
            GlobalMissionManager = new MissionManager(true);
            //GlobalMissionHolder = new GlobalMissionHolder();
            NewsManager = new NewsManager();
            _worldDate = new StarDate();
            _worldDate.Frames = 0;
            Factions = new Dictionary<FactionType, Faction>();            
            //MissionManager = new MissionManager();
            var faction = new Faction(FactionType.Neutral);
            faction.Alliances[FactionType.Player] = 0;
            faction.Name = "Neutral";
            SetFaction(faction);

            _tempName = "Start";
            CodexManager = new CodexManager();
        }

        //public string GetBlackboardValue()

        public Dictionary<FactionType, Faction> GetFactionsDictionary()
        {
            return Factions;
        }

        public Faction[] GetFactions()
        {
            return Factions.Values.ToArray(); //Filter
        }

        public Meter GetPlayerMoney()
        {
            return GetFaction(Framework.FactionType.Player).GetMeter(MeterType.Money);
        }

        public Faction GetFaction(FactionType factionType)
        {           
            Faction result;
            Factions.TryGetValue(factionType, out result);
            return result;
        }

        public float GetFactionReleations(FactionType faction1, FactionType faction2)
        {
            float value1 = Factions[faction1].GetRelationToFaction(faction2);
            float value2 = Factions[faction2].GetRelationToFaction(faction1);
            return Math.Min(value1, value2);
        }

        public void UpdatePlayerShip(GameObject playerShip) //TODO: think about it
        {
            _playerShip = playerShip;
        }

        public void UpdateWithGameEngine(GameEngine gameEngine)
        {
            GlobalMissionManager.Update(gameEngine.Scene);
            _worldDate.Frames++;
            if (gameEngine.Scene.UpdatePlayerShip)
                _playerShip = gameEngine.PlayerAgent;
        }

        public void OnEnterToNode()
        {
            GlobalMissionManager.OnEnterNode();
        }
        
        public void SetFaction(Faction faction)
        {
            Factions[faction.FactionType] = faction;
        }
        
        /// <summary>
        /// Add messege to blackbaord - messege can is a comma sperated of terms, term can be a single key or a colon: speraded pair of key value
        /// key1&key2:value2&key3:value3
        /// </summary>
        /// <param name="messeage"></param>
        public void AddToBlackboard(string message)
        {
            string[] terms = message.Split('&');
            foreach (var term in terms)
            {
                string[] keyValue = term.Split(':');
                string value = string.Empty;
                if (keyValue.Length > 1)
                    value = keyValue[1];
                Blackboard[keyValue[0].Trim()] = value.Trim();
            }
        }
        /// <summary>
        /// Checks if a collection of terms in in the blackbaord
        /// key1~value&key2~value2|key3&key4~value4
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CheckBlackboard(string message)
        {
            string[] orTerms = message.Split('|');
            foreach (var messege in orTerms)
            {
                bool res = true;
                var andTerm = messege.Split('&');
                foreach (var term in andTerm)
                {                   
                    string[] keyValue = term.Split(':');                    
                    if (keyValue.Length > 1)
                    {
                        string value;
                        Blackboard.TryGetValue(keyValue[0].Trim(), out value);
                        if(keyValue[1].Trim() != value)
                        {
                            res = false;
                            break;
                        }                      
                    }
                    else
                    {
                        if(!Blackboard.ContainsKey(keyValue[0].Trim()))
                        {
                            res = false;
                            break;
                        }
                    }
                }
                if (res)
                    return true; 
            }
            
            return false;
        }
    }
}
