using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework;
using XnaUtils.Graphics;
using System.Runtime.Serialization;
using System.Linq;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.PlayersManagement;
using SolarConflict.NodeGeneration;

namespace SolarConflict
{
    /// <summary>
    /// 
    /// </summary>
    public enum Relations {
        /// <summary> </summary>
        VeryHostile,
        Hostile,
        Netural,
        Friendly,
        Allied
    }

    //public static class RelationsExtensions
    //{
    //    public static string ToFriendlyString(this Relations rel)
    //    {
    //        switch (rel)
    //        {
    //            case Relations.None:
    //                return "Everything is OK";
    //            case Relations.Low:
    //                return "SNAFU, if you know what I mean.";
    //            case Relations.High:
    //                return "Reaching TARFU levels";
    //            case Relations.SoylentGreen:
    //                return "ITS PEOPLE!!!!";
    //            default:
    //                return "Get your damn dirty hands off me you FILTHY APE!";
    //        }
    //    }
    //}

    [Serializable]
    public class Faction //Refactor: split to Faction - that holds things relevant for this session and MetaFaction
    {


        public const int WARP_COOLDOWNTIME = 60;
        public int HomeWorldIndex = 0;
        public int WarpCooldown = 0;

        public FleetCommandType FleetCommand;

        public Agent Mothership;
        public FleetSystem MothershipHanger
        {
            get { return Mothership?.GetSystem<FleetSystem>(); }
        }
        public FactionType FactionType { get; set; }        
        /// <summary>
        /// Hull parts counter, if bigger the Consts.NeededHullPart then faction can build the hull
        /// </summary>
        public Dictionary<string, int> hullPartsCounter { get; private set; }                                   
        public List<GameObject> _factionShips; //All Ships in current game engine that belong to the faction
        private Dictionary<MeterType, Meter> meters; //Faction meters, like Money, ControlPoints, Reputation...
        public FactionGenerationData GenerationData; //Someday:move out to meta world        
        public bool ReflectRelations { get; set; }
        public bool OnTheSafeSide { get; set; }
        public float DefaultAttitude { get; set; }
        private Dictionary<FactionType, float> _alliances;
        public Dictionary<FactionType, float> Alliances { get { return _alliances; } }
        
        public String Name { get; set; }
        public String Motto { get; set; }
        public string Description { get; set; }
        public Sprite LogoSprite { get; set; }
        public string LogoTextureID { get { return LogoSprite.ID; } set { LogoSprite = Sprite.Get(value); } }
        public Color Color { get; set; }

        public string ToTag()
        {
            return LogoSprite.ToTag() + " " + Color.ToTag(Name);
        }

        public string ToTag(string massage)
        {
            return LogoSprite.ToTag() + " " + Color.ToTag(massage);
        }
               
        private Sprite _bigGlow; //??

        public string GetInfoText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            sb.AppendLine("\"" + Motto + "\"");
            if (!string.IsNullOrWhiteSpace(Description))
            {
                sb.AppendLine(Description);
            }
            sb.AppendLine("Relations: " + RelationsString(GetRelationToFaction(FactionType.Player)) + " : " + GetRelationToFaction(FactionType.Player).ToString());
            return sb.ToString(); ;
        }

        public string GetPlayerRelationsString()
        {
            float relations = GetRelationToFaction(FactionType.Player);
            int intRelations = (int) (relations * 1000);
            return RelationsString(relations) + " (" + intRelations + ")";
        }
    
        public static string RelationsString(float relation) //TODO: change this function to return an enum
        {
            //Very hostile, hostile, natural, Friendly, Very Friendly
            if (relation < -0.1f)
                return Color.Red.ToTag("Very hostile");
            if (relation < 0)
                return Color.Red.ToTag("Hostile");
            if (relation < 0.01f)
                return Color.Yellow.ToTag("Neutral");
            if (relation < 0.5f)
                return Color.Green.ToTag("Friendly");
            return Color.Green.ToTag("Very Friendly"); //Will not attack 
        }

        public Faction(FactionType faction)
        {
            GenerationData = new FactionGenerationData(); //Temp
            hullPartsCounter = new Dictionary<string, int>();            
            _bigGlow = Sprite.Get("smoke2");
            DefaultAttitude = -0.5f;
            FactionType = faction;
            _alliances = new Dictionary<FactionType, float>();
            _alliances[0] = 0;
            _alliances[FactionType] = 1f;

            meters = new Dictionary<MeterType, Meter>();
            meters.Add(MeterType.MaxControlPoints, new Meter(20, 100));
            meters.Add(MeterType.ControlPoints, new Meter(0, 200));



            _factionShips = new List<GameObject>();
            //shipsToRespawn = new List<GameObject>();
            Color =  FactionColorIndicator.FactionToColor(faction); //MetaWorld.GetFactionColor(FactionIndex);            
        }

        public void AddLoadout(string loadoutID) //TODO: maybe add roles: miner, ...
        {
            GenerationData.loadouts.Add(ContentBank.Inst.GetLoadout(loadoutID));
        }
                
        public void ChangeRelationToFaction(GameEngine gameEngine, FactionType faction, float delta)
        {
            _alliances[faction] = MathHelper.Clamp(GetRelationToFaction(faction) + delta, -1, 1);
            if (delta > 0)
            {
                ClearTargets(gameEngine, faction);
            }
        }

        public void ClearTargets(GameEngine gameEngine, FactionType targetFaction) 
        {
            foreach (var ship in gameEngine._collideAllCheckList) 
            {
                GameObject target = ship.GetTarget(gameEngine, TargetType.Enemy);
                if (ship.GetFactionType() == FactionType && target != null && target.GetFactionType() == targetFaction)
                {
                    ship.SetTarget(null, TargetType.Enemy);
                }
                //ship.SetTarget(null, TargetType.Enemy);
            }
        }

        public void SetRelationToFaction(GameEngine gameEngine, FactionType faction, float relation)
        {
            if (relation >= 0 && relation > GetRelationToFaction(faction))
                ClearTargets(gameEngine, faction);
            _alliances[faction] = relation;
        }

        /// <remarks>We don't enforce mutuality in faction relations, so faction A could be hostile to B, but not vice versa. Most of the time,
        /// you'd be interested in whether the two factions would fight on contact, not in which faction would initiate hostilities,
        /// hence this method, which given A and B, returns the min of the relationships A->B and B->A</remarks>        
        public float GetMinRelationBetweenFactions(FactionType faction) {
            return Math.Min(GetRelationToFaction(faction), MetaWorld.Inst.GetFaction(faction).GetRelationToFaction(FactionType));
        }

        public float GetRelationToFaction(FactionType faction)
        {
            float value;
            if (!_alliances.TryGetValue(faction, out value))
            {
                value = DefaultAttitude;
            }
            return value;
        }

        public bool IsFriendly(FactionType faction)
        {
            return !IsHostile(faction);
        }

        public bool IsHostile(FactionType faction)
        {
            return GetRelationToFaction(faction) < 0f;
        }




        public void AddValueToMeter(MeterType type, float value)
        {
            if (meters.ContainsKey(type))
            {
                meters[type].AddValue(value);
            }
            else
            {
                meters.Add(type, new Meter(value, float.MaxValue));
            }
        }

        public Meter GetMeter(MeterType meterType)
        {
            if (meters.ContainsKey(meterType))
            {
                return meters[meterType];
            }
            else
            {
                Meter meter = new Meter();
                meters[meterType] = meter;
                return meter;
            }
        }

        public void Clear()
        {
            _factionShips.Clear();
            // ActiveShipsCount = 0;
            //TotalHitpoints = 0;            
        }

        public void Update(GameEngine gameEngine)
        {
            //REFACTOR: this is dependent of the order of the faction updates
            if (ReflectRelations) 
            {
                Faction[] factions = gameEngine.Factions;
                for (int i = 0; i < factions.Length; i++)
                {
                    if(factions[i] != null)
                        SetRelationToFaction(gameEngine, factions[i].FactionType, factions[i].GetRelationToFaction(FactionType));
                    if (factions[i] != null)
                        SetRelationToFaction(gameEngine, factions[i].FactionType, factions[i].GetRelationToFaction(FactionType));

                }
            }

            if (OnTheSafeSide)
            {
                Faction[] factions = gameEngine.Factions;
                for (int i = 0; i < factions.Length; i++)
                {
                    if (factions[i] != null)
                        SetRelationToFaction(gameEngine, factions[i].FactionType, Math.Max(Math.Sign(GetRelationToFaction(factions[i].FactionType)),0));
                }
            }



            //Forgive and forget
            if (gameEngine.FrameCounter % 60 == 0)
            {
                float epsilon = 0.1f / 10; //10 Sec per small ship
                var keys = _alliances.Keys.ToArray();
                foreach (var key in keys)
                {
                    if (_alliances[key] > -0.5f && _alliances[key] < 0)
                    {
                        _alliances[key] += epsilon;
                        if (Math.Abs(_alliances[key]) <= epsilon)
                        {
                            _alliances[key] = 0;
                        }
                    }
                }
            }
            Meter controlPointMeter = GetMeter(MeterType.ControlPoints);          
            controlPointMeter.Value = GetMeter(MeterType.MaxControlPoints).Value - _factionShips.Count;
            WarpCooldown--;                               
        }


        public void CallForHelp(GameEngine gameEngine, GameObject callerTarget, GameObject callingObject, float range = 10000)
        {
            for (int i = 0; i < _factionShips.Count; i++)
            {
                if (_factionShips[i].GetTarget(gameEngine, TargetType.Enemy) == null)
                {
                    if ((_factionShips[i].Position - callingObject.Position).LengthSquared() < range * range)
                    {
                        _factionShips[i].SetTarget(callerTarget, TargetType.Enemy);
                    }
                }
            }
        }

        private int FindIndex(AgentControlType controlType)
        {
            for (int i = 0; i < _factionShips.Count; i++)
            {
                if (_factionShips[i].GetControlType() == controlType)
                    return i;
            }
            return 0;
        }

        public void SwitchShip(AgentControlType controlType, int direction) //change
        {
            direction = Math.Sign(direction);
            int playerIndex = FindIndex(controlType);
            List<GameObject> shipList = _factionShips; //TODO: maybe fleetships
            if (_factionShips.Count > 0) //maybe >1
            {
                for (int i = 1; i < shipList.Count; i++)
                {
                    int nextIndex = FMath.Mod(playerIndex + i * direction, shipList.Count);

                    if (shipList[nextIndex].IsControllable() && //TODO: think about it, maybe GetControlType() != controlType or maybe just is controlable
                        (shipList[nextIndex].GetControlType() == AgentControlType.AI || shipList[nextIndex].GetControlType() == AgentControlType.None)
                        ) //is ship controlable
                    {
                        shipList[playerIndex].SetControlType(AgentControlType.AI);
                        shipList[nextIndex].SetControlType(controlType);
                        break;
                    }
                }
            }
        }

        public List<GameObject> AddFleetToNode(GameEngine gameEngine, Vector2 startingPosition)
        {
            if (Mothership == null)
                return new List<GameObject>();

            var result = new List<GameObject>() { Mothership };

            Mothership.Position = startingPosition;
            gameEngine.AddList.Add(Mothership);

            var increment = MathHelper.TwoPi / MothershipHanger.FleetSlots.Count();
            var angle = 0f;
            foreach (var slot in MothershipHanger.FleetSlots)
            {
                angle += increment;
                var ship = slot.Agent;
                if (ship == null)
                    continue;
                ship.Position = Mothership.Position + FMath.ToCartesian(Mothership.Size + ship.Size * 2f, angle);
                ship.FactionType = FactionType;
                gameEngine.AddList.Add(ship);
                result.Add(ship);                
            }

            return result;
        }

        public void RemoveFleet(GameEngine gameEngine)
        {
            if (MothershipHanger != null)
            {
                foreach (var ship in MothershipHanger.FleetShips)
                {
                    ship.ResetSystems(); // to avoid serializing unnecessary objects
                    gameEngine.RemoveGameObject(ship);
                    ship.targetSelector?.ClearAllTargets();
                }
            }           
            if (Mothership != null)
            {
                Mothership.ResetSystems();
                gameEngine.RemoveGameObject(Mothership);
                Mothership.targetSelector?.ClearAllTargets();
            }
            if (gameEngine.Scene != null)
                gameEngine.Scene.PlayerAgent = null;
        }



        public void DrawLogo(Camera camera, Vector2 position, float scale)
        {
            camera.CameraDraw(_bigGlow, position, 0, 6.5f * scale, Color);
            if (LogoSprite != null)
                camera.CameraDraw(LogoSprite, position, 0, scale, Color.White);
            SpriteFont font = Game1.font;
            Vector2 fontSize = font.MeasureString(Name);
            camera.SpriteBatch.DrawString(font, Name, camera.GetScreenPos(position + Vector2.UnitY * 60) - fontSize * 0.5f, Color.White);
        }

       

        public bool CheckIfHullIsKnown(string id)
        {
            int hullParts;
            hullPartsCounter.TryGetValue(id, out hullParts);
            return hullParts >= Consts.NeededHullPart;
        }

        public void AddHullParts(string id, int parts = 1)
        {
            int hullParts;
            hullPartsCounter.TryGetValue(id, out hullParts);
            hullPartsCounter[id] = hullParts + parts;
        }

        public void AddHull(string id)
        {
            AddHullParts(id, Consts.NeededHullPart);
        }

        //public List<ChallengeInfo> GetChallenges(int level) //Add level
        //{
        //    List<ChallengeInfo> challengeList = new List<ChallengeInfo>();
        //    foreach (var loadout in GenerationData.loadouts)
        //    {
        //        //loadout.
        //        ChallengeInfo challenge = new ChallengeInfo()
        //        {
        //            Name = "Duel",
        //            Description = "Fight a friendly simulated battle against another ship",
        //            Icon = ContentBank.Inst.GetLoadout(loadout.ID).GetSprite()
        //        };
        //        // "Duel", "Fight a friendly simulated battle against another ship", loadout.ID, (int)(loadout.Cost * 0.1f));
        //        //challenge.ActivityParameters = new ActivityParameters();
        //        challenge.ActivityParameters["enemy"] = loadout.ID;
        //        challenge.ActivityID = "DuelChallengeScene";
        //        challenge.IconText = loadout.FullDescription;
        //        challenge.Icon = loadout.GetSprite();
        //        challenge.Level = level;
        //        var recipe = ContentBank.Inst.GetAllRecipes().Where(i => i.CraftedItem.Level == level).Choice(FMath.Rand);
        //        //GenerationData.AddItem
        //        Reward reward = new Reward()
        //        {
        //            Money = (int)(loadout.Cost * 0.1f),
        //            ReputationDelta = level / 10f                  
        //        };
        //        reward.Items.Add(new Tuple<string, int>( recipe.CraftedItem.ID, 1));
        //        challengeList.Add(challenge);
        //    }
        //    return challengeList;
        //}

            //    //return challengeList;
            //    return null;
            //}


            //public float GetFactionWorth()
            //{
            //    float worth = 0;
            //    foreach (var item in _factionShips)
            //    {
            //        Agent agent = item as Agent;
            //        if (agent != null)
            //        {
            //            worth += agent.GetCost();
            //        }
            //    }
            //    Agent ms = Mothership as Agent;
            //    if (ms != null)
            //    {
            //        worth += ms.GetCost();
            //    }
            //    return worth;
            //}

            //public List<string> GetAllKnownHulls()
            //{
            //    List<string> knownHulls = new List<string>();
            //    foreach (var pair in hullPartsCounter)
            //    {
            //        if(CheckIfHullIsKnown(pair.Key))
            //        {
            //            knownHulls.Add(pair.Key);
            //        }
            //    }
            //    return knownHulls;
            //}

            //Maybe: change to agent procces //private List<IAgentStatelessSystem> _agentSystems; //a list of systems that act on the faction ships(agents)        

            //SOMEDAY: split in to MetaFaction and EngineFaction
        }
}
