using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.PlayersManagement;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.Components.Editors;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.GameContent.Projectiles.Bla;
using SolarConflict.Session;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.XnaUtils;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using static SolarConflict.Framework.Scenes.SceneComponentSelector;

namespace SolarConflict
{


    [Serializable]
    public class Scene : Activity
    {
        [NonSerialized]
        public MetaWorld MetaWorld;

        /// <summary>
        /// Hold Id for scene
        /// </summary>
        public int SceneID;
        
        public bool IsWarpDisabled;
        public bool SaveOnExit = false;
        public Vector2 CursorWorldPosition { get; private set; }
        public GameObject GameObjectUnderCursor { get; private set; }
        public GameEngine GameEngine;
        public CameraManager CameraManager;
        
        public Camera Camera { get { return _camera; } private set { _camera = value; GameEngine.Camera = value; } }
        private Camera _camera;

        public bool IsPlayerInScene { get; private set; }
              
        public SceneComponentSelector SceneComponentSelector { get; protected set; }

        //protected Vector2? LastPlayerDeathPostion { get; set; }
        //public ActionOnPlayerDeathType ActionOnPlayerDeath { get; set; }        
        private MissionManager MissionManager { get; set; }

        public HudManager HudManager;
        
        string text = string.Empty;
        public bool PauseGameEngine = false;

        public bool IsShipSwitchable;

        private string _parameters;

        private int _warpToAtEndOfUpdate;

        public DialogManager DialogManager { get; private set; }


        public InputState InputState
        {
            get { return _inputState; }
        }

        public PlayersManager PlayersManager;

        public Color fadeColor = Color.White;
        public float fadeAlpha = 0;

        public Background background;

        public Boolean IsConfirmQuitNeeded = false;               

        public bool IsGamePaused = false;
        public bool UpdatePlayerShip = false;

        public bool RestorePlayerPosition = true;

        private SceneScript _script;

        private Vector2? playerPosition;
        public int SceneFrameCounter { get; private set; }                
        public Vector2 PlayerStartingPoint { get; set; }               
        private RichTextControl _tooltip;
        [NonSerialized]
        public Agent PlayerAgent;
        [NonSerialized]
        public Agent CoPlayerAgent;                 
        [NonSerialized]
        protected GuiManager gui;
        [NonSerialized]
        private InputState _inputState;
        [NonSerialized]
        public float TargetAlpha;        

        // public Agent CallingAgent;
        public Agent DialogAgent;

        public bool HideRecycleBean;

        public Scene(string parameters = null, bool passFactions = true, int level = 0)
        {            
            this._parameters = parameters;
            DialogManager = new DialogManager(this);
            //LastPlayerDeathPostion = Vector2.Zero;
            background = new Background(1);                        
            

            MissionManager = new MissionManager(false);
            //PointsOfInterest = new List<IPointOfInterest>();
            _tooltip = new RichTextControl("");
            _tooltip.IsShowFrame = true;
            //_tooltip.Update(InputState.EmptyState);            
            InitNonSerialized();
            if (passFactions)
            {
                GameEngine = new GameEngine(Camera, this, MetaWorld.GetFactionsDictionary().Values, level: level);
            }
            else
            {
                GameEngine = new GameEngine(Camera, this, null, level: level);
            }

            SceneComponentSelector = new SceneComponentSelector(this); //According to params

            _warpToAtEndOfUpdate = -1;
         
            Camera = new Camera();
            CameraManager = new CameraManager();
            HudManager = new HudManager();
            
            SceneFrameCounter = 0;
            SceneComponentSelector.AddComponent(SceneComponentType.EscapeMenu);
            //if (SaveOnExit)
            //    this.CallingActivity = null;
        }                

        protected virtual void InitNonSerialized()
        {
            PlayersManager = new PlayersManager();                        
            _warpToAtEndOfUpdate = -1;
            fadeAlpha = 0; //??
            _inputState = new InputState();
            MetaWorld = MetaWorld.Inst; //
        }

        public void SetScript(SceneScript script)
        {
            _script = script;
        }
        
        protected override sealed void Init(ActivityParameters parameters)
        {
            InitScript(_parameters, parameters);
            _script?.InitScript(this);
        }

        

        public override void Update(InputState inputState)
        {

            DebugUpdate(inputState);

            gui?.Update(inputState);
            UpdateScript(inputState);
            _script?.UpdateScript(this);
            SceneComponentSelector.LastComponentSelected = null;
        //    CallingAgent = null;
            SceneComponentSelector.Update(inputState);
            DialogManager.Update();
          
            if (!IsGamePaused)
            {                       
                IsWarpDisabled = false;
               
                this._inputState = inputState;
                // Anything appropriate in range of the cursor?
                CursorWorldPosition = Camera.GetWorldPos(inputState.Cursor.Position);
                GameObjectUnderCursor = GameEngine.FindGameObjectInPosition(CursorWorldPosition, 20, GameObjectType.All); //GameEngine.CollisionManager.FindClosestTarget(CursorWorldPosition, CursorDetectionRadius / Camera.GetZoom(), GameObjectType.Agent, null);


                if (GameObjectUnderCursor != null && GameObjectUnderCursor.Tag  != null)//&& ( (GameObjectUnderCursor.GetObjectType() & GameObjectType.Item) > 0))
                {
                    if (GameObjectUnderCursor.GetFactionType() != FactionType.Neutral)
                    {
                        Faction faction = GameEngine.GetFaction(GameObjectUnderCursor.GetFactionType());
                        _tooltip.Text = StringUtils.JoinNonNullsCustomSeparator(" ", faction.LogoSprite?.ToTag(faction.Color), GameObjectUnderCursor.Tag);
                    }
                    else
                    {
                        _tooltip.Text = GameObjectUnderCursor.Tag;
                    }
                }
                else
                {
                    _tooltip.Text = null;
                }
                UpdateInteraction(GameObjectUnderCursor);
                // Update quest arrows and stuff                                
                MissionManager.Update(this); //maybe after GameEngine Update ???
                             
             

                //if (PlayerAgent != null && PlayerAgent.IsNotActive)
                //{
                //    LastPlayerDeathPostion = PlayerAgent.Position;
                //}


                if (PlayerAgent == null || PlayerAgent.IsNotActive || PlayerAgent.GetControlType() != AgentControlType.Player) 
                {
                    IsPlayerInScene = false;
                    PlayerAgent = FindPlayer() as Agent;
                                     
                }
                else
                {
                    if (PlayerAgent != null)
                        IsPlayerInScene = true;
                }

                if (CoPlayerAgent == null || CoPlayerAgent.IsNotActive || CoPlayerAgent.GetControlType() != AgentControlType.CoPlayerOrAI1)
                {
                    CoPlayerAgent = FindCoPlayer() as Agent;
                }


                if (RestorePlayerPosition && PlayerAgent != null)
                {
                    playerPosition = PlayerAgent.Position;
                }

                // Ensure only one ship is under player control (this is a safeguard against a bug)
                var playerControlledShips = GameEngine.PlayerAgents.Where(a => a.GetControlType() == AgentControlType.Player);
                //if (playerControlledShips.Count() > 1)
                //{
                //    // Multiple player-controlled ship found
                //    Debug.Assert(playerControlledShips.Contains(PlayerAgent), "PlayerAgent has wrong control type");
                //    playerControlledShips.Do(s => s.SetControlType(AgentControlType.AI));
                //    PlayerAgent.SetControlType(AgentControlType.Player);
                //}

                
                if (UpdatePlayerShip)                    
                    MetaWorld.UpdatePlayerShip(PlayerAgent);                 
                
                gui?.Update(inputState);
                PlayersManager.Update(this);

           
                
                if (!PauseGameEngine)
                {
                  
                    GameEngine.Update(inputState);
                    MetaWorld?.UpdateWithGameEngine(GameEngine);   //todo: review                   
                    if (!DebugUtils.HideHud)
                    {
                        //_hud.Update(GameEngine, (Agent)PlayerAgent);
                        HudManager.Update(this, PlayerAgent);
                    }

                    PlayerDeathUpdateLogic();
                    _script?.UpdateWithGameEngine(this);
                    // soleFaction = gameEngine.GetSoleFaction();                                                 
                }
                CameraManager.Update(Camera, this.GameEngine, inputState);
                //Add Swap to all Co Players, change
                //if(inputState.IsKeyDown(Keys.D1))
                //{
                //    GetPlayerFaction().Mothership.SetControlType(AgentControlType.CoPlayerOrAI1);
                //}

                if (IsShipSwitchable)
                {
                    for (int i = 0; i < PlayersManager.players.Length; ++i) {
                        var playerControls = PlayersManager.players[i];

                        if (playerControls is PlayerMouseAndKeys && (GameObjectUnderCursor?.IsSwitchable() == true))
                        {
                            // Player has a cursor, something switchable's under it
                            if (playerControls.IsCommandClicked(PlayerCommand.SwapDown)
                                || playerControls.IsCommandClicked(PlayerCommand.SwapUp))
                                // Switch to it
                                SwitchPlayerShip(GameObjectUnderCursor as Agent, i);
                        }
                        else
                        {
                            if (playerControls?.IsCommandClicked(PlayerCommand.SwapUp) == true)
                                SwitchPlayerShip(playerIndex: i);
                            else if (playerControls?.IsCommandClicked(PlayerCommand.SwapDown) == true)
                                SwitchPlayerShip(true, playerIndex: i);
                        }
                    }   
                }


                //TODO: move to 
                if (MetaWorld != null)
                    MetaWorld.UpdateWithGameEngine(GameEngine);
                //MetaWorld.Inst.UpdateWithGameEngine(GameEngine); //Change it
                // Warp player out, if queued
                //if (_warpToAtEndOfUpdate >= 0) {
                //    var temp = _warpToAtEndOfUpdate;
                //    _warpToAtEndOfUpdate = -1;
                //    GalaxyMap.Inst.WarpToNode(temp);
                //}
            }
            //Out of Pause section
            SceneFrameCounter++;
        }

        public void WarpAtEndOfUpdate(int targetNodeIndex) {
            _warpToAtEndOfUpdate = targetNodeIndex;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!_isInitialized) //??
                return;



            // We will open Render target here if we are allowing postprocessing
            if(GraphicsSettings.IsPostprocessing)
            { 
                ActivityManager.GraphicsDevice.SetRenderTarget(GraphicsSettingsUtils.renderTargetFullA);
            }

            background.Draw(Camera);

          //  sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
          ////  _hud.Faction(sb, Camera, GameEngine);
          //  sb.End();

            GameEngine.Draw(sb);        
            HudManager.Draw(this, PlayerAgent);
            DialogManager.Draw();

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);                                 
            //targetHud.Draw(sb);
            //  text = GetFactionScore(1).ToString();
            if (text != null && text != "")
            {
                Vector2 textLength = Game1.font.MeasureString(text);
                Vector2 textPosition = new Vector2((ActivityManager.ScreenWidth - textLength.X) / 2, 10);
                sb.DrawString(Game1.font, text, textPosition, Color.White);                
            }

            if(!string.IsNullOrWhiteSpace( _tooltip.Text))
            {
                Vector2 pos = _inputState.Cursor.Position + _tooltip.HalfSize + new Vector2(15f, -20f);
                //GraphicsUtils.Draw9Slice(_tooltipTexture ,sb, new Rectangle((int)pos.X, (int)pos.Y, (int)_tooltip.Width, (int)_tooltip.Height));
                _tooltip.IsShowFrame = true;
                _tooltip.Position = pos;
                _tooltip.Draw(sb);
            }

            //if(GameObjectUnderCursor != null && GameObjectUnderCursor.GetId() != null)
            //{
            //    sb.DrawString(Game1.font, GameObjectUnderCursor.GetId()+"_", _inputState.Cursor.Position + new Vector2(15f, -20f), Color.White);
            //}

            //// Somewhat kludgy, display little pseudotooltip if there's something with which we can interface via the inventory screen                
            //var commandKey = PlayerMouseAndKeys.commandBindings[PlayerCommand.Inventory];
            //if (_inputState != null && GameObjectUnderCursor?.GetFactionType() == FactionType.Player && GameEngine.PlayerAgent != null
            //    && GameObjectUnderCursor != GameEngine.PlayerAgent
            //    && ((GameObjectUnderCursor?.GetInventory() != null || GameObjectUnderCursor.GetCraftingStationType() != CraftingStationType.Basic)))
            //    sb.DrawString(Game1.font, $"{commandKey} to dock", _inputState.Cursor.Position + new Vector2(15f, -20f), Color.White);

            sb.End();

            
            DrawScript(sb);
            _script?.Draw(this);

            gui?.Draw(Color.White);
            SceneComponentSelector.Draw(sb);

            if (fadeAlpha > 0)
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                ActivityManager.Inst.FadeDraw(sb,fadeAlpha, fadeColor); ;
                sb.End();
            }
       
        }

        public bool SkipInteraction;
        private void UpdateInteraction(GameObject objectUnderCursor)
        {
            if (PlayersManager.players[0].IsCommandOn(PlayerCommand.CallHelp))
            {

                var ships = GetPlayerFaction().MothershipHanger.FleetShips;
                foreach (var ship in ships)
                {

                    if (objectUnderCursor == PlayerAgent)
                    {
                        ship.SetTarget(PlayerAgent, TargetType.Goal);
                        ship.SetTarget(PlayerAgent, TargetType.Ally);
                        ship.targetSelector.SetTarget(null, TargetType.Enemy, 400);
                        ActivityManager.Inst.AddToast($"Stop Attack, Follow", 100, Color.Blue);
                        GetPlayerFaction().MothershipHanger.SetCommand(FleetCommandType.None);
                    }

                    if (objectUnderCursor == null || (objectUnderCursor.GetObjectType() & GameObjectType.PotentialTarget) == 0)
                    {                      
                        ship.SetTarget(PlayerAgent, TargetType.Goal);
                        ship.SetTarget(PlayerAgent, TargetType.Ally);
                        ActivityManager.Inst.AddToast($"Follow flagship", 100, Color.Green);
                        GetPlayerFaction().MothershipHanger.SetCommand(FleetCommandType.FlagshipTargetAndFlagship);
                    }
                    else
                    {

                        if (objectUnderCursor != PlayerAgent && objectUnderCursor != null)
                        {
                            ship.SetTarget(objectUnderCursor, TargetType.Goal);
                            if (objectUnderCursor.IsHostileToPlayer(GameEngine))
                            {
                                GetPlayerFaction().MothershipHanger.SetCommand(FleetCommandType.None);

                                ship.targetSelector.SetTarget(objectUnderCursor, TargetType.Enemy);
                                ActivityManager.Inst.AddToast($"Attack {objectUnderCursor.Tag}", 100, Color.Red);
                                
                            }
                            else
                            {
                                if(objectUnderCursor == GetPlayerFaction().Mothership)
                                {
                                    GetPlayerFaction().MothershipHanger.SetCommand(FleetCommandType.Mothership);
                                    ActivityManager.Inst.AddToast("Guard Mothership ", 100, Color.Green);
                                }
                                else
                                {
                                    GetPlayerFaction().MothershipHanger.SetCommand(FleetCommandType.None);
                                    ActivityManager.Inst.AddToast($"Go to {objectUnderCursor.Tag}", 100, Color.Green);

                                }
                                
                            }

                        }
                    }
                }
            }

            if (SkipInteraction)
                return;

            if (PlayerAgent != null && PlayerAgent.IsActive)
            {
                var inputTag = PlayersManager.GetCommandString(PlayerCommand.Use);
                var additionalText = objectUnderCursor?.GetInteractionTooltip(GameEngine, PlayerAgent);
                bool isPlayerInteracting = PlayersManager.players[0].IsCommandClicked(PlayerCommand.Use);
                if (isPlayerInteracting && objectUnderCursor != null)
                    objectUnderCursor.Interact(GameEngine, PlayerAgent);               
                if(additionalText != null)
                    _tooltip.Text = StringUtils.JoinNonNulls(_tooltip.Text, Palette.Highlight.ToTag("("+inputTag+") ") + additionalText);
            }


                //try
                //{
                    

                //    if (objectUnderCursor != null && (objectUnderCursor.GetObjectType() &  (GameObjectType.PotentialTarget | GameObjectType.Asteroid)) > 0 )
                //    {
                //        var target = objectUnderCursor.GetTarget(GameEngine, TargetType.Enemy)
                //        if (objectUnderCursor.IsPlayerHostile(GameEngine) ||  )
                //        {
                //            PlayerAgent.SetTarget(objectUnderCursor, TargetType.Enemy);
                             
                //            //GetPlayerFaction().MothershipHanger?.SetCommand(FleetCommandType.FlagshipTargetAndFlagship);
                //            ActivityManager.Inst.AddToast($"Attack {objectUnderCursor.Name}!", 100, Color.Red);
                //        }
                //        else
                //        {
                //            GetPlayerFaction().MothershipHanger?.SetCommand(FleetCommandType.None);
                //            GetPlayerFaction().MothershipHanger?.SetTarget(GameObjectUnderCursor, TargetType.Goal);
                //            if((objectUnderCursor.GetObjectType() & GameObjectType.Asteroid) > 0)
                //                GetPlayerFaction().MothershipHanger?.SetTarget(objectUnderCursor, TargetType.Enemy);
                //            else
                //                GetPlayerFaction().MothershipHanger?.SetTarget(null, TargetType.Enemy);
                //            ActivityManager.Inst.AddToast($"Go to {objectUnderCursor.Name}", 100);
                            
                //        }
                //    }
                //    else
                //    {
                //        // Rally at flagship
                //        GetPlayerFaction().MothershipHanger?.SetCommand(FleetCommandType.FlagshipTargetAndFlagship);
                //        ActivityManager.Inst.AddToast($"Follow flagship", 100);
                //    }

                //    try { 
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //    //TODO: write to log                
                //}
            
           
        }
        
        public GameObject AddGameObject(string id, FactionType faction, Vector2 position, float rotation = 0, AgentControlType controlType = AgentControlType.AI, float param = 0) //add AI index?
        {
            return GameEngine.AddGameObject(id, faction, position, rotation, controlType, param);
        }

        //TODO: move to extensions
        public GameObject AddGameObject(string id, Vector2 position, float rotation = 0, FactionType faction = FactionType.Neutral, AgentControlType controlType = AgentControlType.AI) //add AI index?
        {
            return AddGameObject(id, faction, position, rotation, controlType);
        }

        public Agent AddAgent(string id, FactionType faction, Vector2 position, float rotation = 0, AgentControlType controlType = AgentControlType.AI)  //add AI index?
        {
            var result = AddGameObject(id, faction, position, rotation, controlType);

            if (result is Agent)
            {
                return result as Agent;
            }

            throw new InvalidCastException("Game object - " + id + "isn't an agent");
        }

        public Agent AddAgent(string id, Vector2 position, float rotation = 0, FactionType faction = FactionType.Neutral, AgentControlType controlType = AgentControlType.AI) //add AI index?
        {
            return AddAgent(id, faction, position, rotation, controlType);
        }

        public void SetText(string text)
        {
            this.text = text;
        }


        public GameObject FindPlayer(bool isActiveNeeded = false) //TODO: think about it
        {
            return FindControlType(AgentControlType.Player, isActiveNeeded);
        }

        public GameObject FindCoPlayer(bool isActiveNeeded = false) //TODO: think about it
        {
            return FindControlType(AgentControlType.CoPlayerOrAI1, isActiveNeeded);
        }

        private GameObject FindControlType(AgentControlType type, bool isActiveNeeded = true) //TODO: //Maybe move to GameEngine
        {
            foreach (var gameObject in GameEngine._collideAllCheckList) //TODO: fix
            {

                if (gameObject.GetControlType() == type && (gameObject.IsActive || !IsActive))
                {
                    return gameObject;
                }
            }
            return null;
        }

        public int FindPlayerIndex()
        {
            Faction faction = GetPlayerFaction();
            for (int i = 0; i < faction._factionShips.Count; i++)
            {
                if (faction._factionShips[i].GetControlType() == AgentControlType.Player)
                    return i;
            }
            return 0;
        }

        public void SwitchPlayerShip(GameObject newShip, int playerIndex)
        {
            if (PlayerAgent != null)
                PlayerAgent.SetControlType(AgentControlType.AI);

            PlayerAgent = newShip as Agent;
            newShip.SetControlType(playerIndex == 0 ? AgentControlType.Player : AgentControlType.CoPlayerOrAI1);
        }

        public void SwitchPlayerShip(bool backwards = false, bool ignoreMothership = false, int playerIndex = 0)
        {            
            var ships = GameEngine.PlayerAgents
                .Where(a => (a.IsControllable() &&(  !ignoreMothership) || a != GameEngine.GetFaction(FactionType.Player).Mothership))
                .ToArray();
            if (ships.Count() == 0)
                return;

            var pertinentAgent = playerIndex == 0 ? PlayerAgent : CoPlayerAgent;

            var i = backwards ? 1 : -1;
            if (pertinentAgent != null) {
                pertinentAgent.SetControlType(AgentControlType.AI);
                PlayerAgent = null;
                i = ships.IndexOf(pertinentAgent);                
            }
            
            do {
                if (backwards)
                    --i;
                else
                    ++i;

                i = FMath.Mod(i, ships.Count());
            } while (ships[i].GetControlType() == AgentControlType.Player || ships[i].GetControlType() == AgentControlType.CoPlayerOrAI1);

            SwitchPlayerShip(ships[i], playerIndex);        
        }
        
        public void Exit()
        {
            IsGamePaused = false;
            ActivityManager.Inst.Back();
           // EndScript(this);
        }

        public Faction GetPlayerFaction()
        {
            return GameEngine.GetFaction(FactionType.Player);
        }
                
        public virtual void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {

        }

        public virtual void UpdateScript(InputState inputState)
        {

        }

        public virtual void DrawScript(SpriteBatch sb)
        {
        }


        public override void OnEnter(ActivityParameters parameters) //TODO:Change
        {
            //MusicEngine.Instance.PlayRandomSong();
            MissionManager.OnEnterNode();
            MetaWorld?.OnEnterToNode();
        }

        public override void OnResume(ActivityParameters parameters = null)
        {
            if (RestorePlayerPosition && playerPosition != null && PlayerAgent != null)
            {
                PlayerAgent.Position = playerPosition.Value;
            }
            IsGamePaused = false;
        }

        
      
        public override ActivityParameters OnLeave()
        {          
            OnExit();
            ActivityParameters activityParams = new ActivityParameters();
            activityParams.DataParams.Add("Scene", this);
            return activityParams;
        }

        public override ActivityParameters OnBack()
        {
            OnExit();
            return null;
        }        
                       

        public void OnExit()
        {
            //TODO: Move it
            if (GameplaySettings.AutoSave &&  SaveOnExit && MetaWorld.GetFaction(FactionType.Player).Mothership.IsActive) //TODO: maybe move the auto saving logic to meta world?
            {
                PersistenceManager.Inst.Save();
                ActivityManager.Inst.AddToast("Game Saved", 60 * 2, Color.Green);
            }
            

            DialogManager.StopDialog();
            _script?.OnBack(this);
            
        }

        public void UpdateNodeInfo(NodeInfo nodeInfo)
        {
            Dictionary<FactionType, int> baseCount = new Dictionary<FactionType, int>();
            foreach (var gameObject in GameEngine._collideAllParticles)
            {
                if((gameObject.GetObjectType() & GameObjectType.Mothership) > 0)
                {
                    int count;
                    baseCount.TryGetValue(gameObject.GetFactionType(), out count);
                    baseCount[gameObject.GetFactionType()] = count + 1;
                }
            }
            FactionType controllingFaction = nodeInfo.ControllingFaction;
            int maxCount = 0;
            foreach (var pair in baseCount)
            {
                if(pair.Value > maxCount)
                {
                    controllingFaction = pair.Key;
                    maxCount = pair.Value;
                }
            }
            nodeInfo.ControllingFaction = controllingFaction;
        }

        public void WarpOutPlayerFaction()
        {
            AddGameObject("HyperSpaceJumpFx", FactionType.Neutral, PlayerStartingPoint);
            GetPlayerFaction().RemoveFleet(GameEngine);
        }       

        public void WarpInPlayerFaction(bool showEffect = true)
        {
            GameEngine.Factions[(int)FactionType.Player] = MetaWorld.GetFaction(FactionType.Player);
            if (showEffect)
                AddGameObject("HyperSpaceJumpFx", FactionType.Neutral, PlayerStartingPoint);
            GetPlayerFaction().AddFleetToNode(GameEngine, PlayerStartingPoint);
        }
             
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context) {
            InitNonSerialized();
        }

        public bool AddMission(Mission mission)
        {
            if (mission.IsGlobal && MetaWorld != null)
            {
                if(MetaWorld != null)
                    return MetaWorld.GlobalMissionManager.AddMission(mission);
            }
            else
                return MissionManager.AddMission(mission);
           return false;
        }

        public void RemoveMission(Mission mission)
        {
            if(mission.IsGlobal)
            {
                MetaWorld?.GlobalMissionManager.RemoveMission(mission);
            }
            else
            {
                MissionManager.RemoveMission(mission);
            }            
        }

        public void RemoveMissionGenerator(IMissionGenerator generator)
        {
            if (generator.IsGlobal)
            {
                MetaWorld?.GlobalMissionManager.GeneratorBank.Remove(generator);
            }
            else
            {
                MissionManager.GeneratorBank.Remove(generator);
            }
        }

        public Mission GetMission(string ID)
        {
            Mission mission = MissionManager.GetMission(ID);
            if (mission != null)
                return mission;
            if (MetaWorld != null)
                return MetaWorld.GlobalMissionManager.GetMission(ID);
            return null;
        }

        public List<Mission> GetMissions()
        {
            List<Mission> missions = new List<Mission>(MissionManager.GetMissions());
            if (MetaWorld != null)
                missions.AddRange(MetaWorld.GlobalMissionManager.GetMissions());
            return missions;
        }  

        public void AddMissionGenerator(IMissionGenerator generator)
        {
            if (generator.IsGlobal)
            {                
                MetaWorld?.GlobalMissionManager.GeneratorBank.Add(generator);
            }
            else
                MissionManager.GeneratorBank.Add(generator);
        }

        public void ClearMissions()
        {
            MissionManager.Clear();
        }

        public List<IMissionGenerator> GetMissionGenerators(FactionType factionType, int level = -1)
        {
            List<IMissionGenerator> generators = new List<IMissionGenerator>(MissionManager.GeneratorBank.GetGenerators(this, factionType, level));
            if (MetaWorld != null)
                generators.AddRange(MetaWorld.GlobalMissionManager.GeneratorBank.GetGenerators(this, factionType, level));
            return generators;
        }
        
        public bool ContainsMissionID(string ID)
        {
            if (MissionManager.ContainsMissionID(ID) || MetaWorld.GlobalMissionManager.ContainsMissionID(ID))
                return true;
            return false;
        }

        public string GetNewMissionID() // todo: change
        {
            return MissionManager.GetNewMissionID();
        }

        public List<TutorialGoal> GetTutorialGoals()
        {
            List<TutorialGoal> goals = new List<TutorialGoal>(MissionManager.GetTutorialGoals());
            if (MetaWorld != null)
                goals.AddRange(MetaWorld.GlobalMissionManager.GetTutorialGoals());
            return goals;
        }

        public GameObject FindGameObjectByID(string id, bool lookAtAll = false)
        {
            if (!lookAtAll)
            {
                foreach (var target in GameEngine._collideAllCheckList) //??
                {
                    if (target.GetId() == id)
                        return target;
                }
            }
            else
            {
                foreach (var target in GameEngine._collideParticles1) //??
                {
                    if (target.GetId() == id)
                        return target;
                }
                foreach (var target in GameEngine._collideAllCheckList) //??
                {
                    if (target.GetId() == id)
                        return target;
                }
            }
            return null;
        }

        public GameObject FindClosestByID(string id, Vector2 position, bool lookAtAll = false)
        {
            GameObject res = null;
            float minDis = float.MaxValue;
            if (!lookAtAll)
            {
                foreach (var target in GameEngine._collideAllCheckList) //??
                {
                    if (target.GetId() == id)
                    {
                        float dis = GameObject.DistanceFromEdge(position, target.Position, target.Size);
                        if(dis < minDis)
                        {
                            minDis = dis;
                            res = target;
                        }
                    }
                }
            }
            else
            {
                foreach (var target in GameEngine._collideParticles1) //??
                {
                    if (target.GetId() == id)
                    {
                        float dis = GameObject.DistanceFromEdge(position, target.Position, target.Size);
                        if (dis < minDis)
                        {
                            minDis = dis;
                            res = target;
                        }
                    }
                }
                foreach (var target in GameEngine._collideAllCheckList) //??
                {
                    if (target.GetId() == id)
                    {
                        float dis = GameObject.DistanceFromEdge(position, target.Position, target.Size);
                        if (dis < minDis)
                        {
                            minDis = dis;
                            res = target;
                        }
                    }
                }
            }
            return res;
        }



        private void PlayerDeathUpdateLogic()
        {   //Todo// mark player deth position for one update, an proccess can add this mission
            //if(PlayerAgent != null && PlayerAgent.IsNotActive && IsPlayerInScene)// (PlayerAgent.ControlSignal & ControlSignals.OnDestroyed) >0)
            //{
            //    MissionManager.AddMission(MissionFactory.PlayerDeathLocationMission(PlayerAgent));
            //}

            //TODO, add in a proccess, go back on player death
            //if (ActionOnPlayerDeath == Scene.ActionOnPlayerDeathType.Back &&
            //    PlayerAgent != null && PlayerAgent.IsNotActive)
            //{
            //    if (_backTimer < _respawnTime)
            //    {
            //        // gameEngine.Scene.dialogManager.AddDialogBox("Fail!\nYou will respawn in the world soon", boxID: "back");
            //        _backTimer++;
            //    }
            //    else
            //    {
            //        _backTimer = 0;
            //        ActivityManager.Inst.Back();
            //    }
            //}
        }
        const int BACKGROUND_NUM = 8;
        int _backgroundCounter = 0;
        public void SetBackground(int index)
        {
            _backgroundCounter = (index ) % BACKGROUND_NUM;
            background = new Background(_backgroundCounter, isRandom:true);
        }

        /// <summary>
        /// Switches activity and adds DataParams: Scene, Calling_agent
        /// </summary>
        /// <param name="activityProvider"></param>
        /// <param name="activityParams"></param>
        /// <param name="component"></param>
        /// <param name="callingAgent"></param>
        public Activity SwitchActivity(string activityProvider, ActivityParameters activityParams = null, Component component = null, Agent callingAgent = null)
        {
            if (activityParams == null)
                activityParams = new ActivityParameters();            
            activityParams.DataParams["Calling_agent"] = callingAgent;
            activityParams.ParamDictionary.Add("Level", GameEngine.Level.ToString());
            return SceneComponentSelector.SwitchActivity(activityProvider, activityParams);            
        }

#region DEBUG

        //List<Keys> _keys = new List<Keys> { Keys.D0, Keys.D1, Keys.D2, Keys.D3 , Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        //int loadoutIndex = 10;
        public void DebugUpdate(InputState inputState)
        {

            if (!DebugUtils.UseDebugKeys)
                return;

            if(inputState.IsKeyPressed(Keys.F10))
            {
                GameplaySettings.AutoSave = !GameplaySettings.AutoSave;
                ActivityManager.Inst.AddToast(GameplaySettings.AutoSave ? "Auto Save On!" : "Auto Save Off!", 100);
            }
           
            if(inputState.IsKeyPressed(Keys.F9))
            {
                ActivityManager.Inst.AddToast("Reloaded", 100);
                PersistenceManager.Inst.Continue();                
            }


            if(inputState.IsKeyPressed(Keys.Enter))
            {
                //string path = GameSession._instance.SavePath;
                //GameSession._instance = new GameSession();
                //FactionContent.LoadFactionData();
                //ActivityManager.Inst.AddToast("Clear Meta", 100);
               // GameSession._instance.SavePath = path;

            }      

            if (inputState.IsKeyPressed(Keys.P))
            {
                MetaWorld._tempName = "Pressed";
            }


            if (inputState.IsKeyPressed(Keys.O))
                SceneComponentSelector.SwitchActivity("FactionInfoActivity");// //SwitchActivity("FactionInfoActivity", new ActivityParameters());

            if (inputState.IsKeyPressed(Keys.T) && GameObjectUnderCursor != null)
            {
                GameObjectUnderCursor.SetFactionType(FactionType.Player);             
            }

            if(inputState.IsKeyPressed(Keys.F8))
            {
                if(PlayerAgent != null)
                    ActivityManager.Inst.SwitchActivity( new LoadoutEditorActivity(this, PlayerAgent, null), false);
            }
                                  

            if (inputState.IsKeyPressed(Keys.Y) && PlayerAgent != null)
            {
                PlayerAgent.Position = CursorWorldPosition;
            }        
            
            if(inputState.IsKeyPressed(Keys.K) && GameObjectUnderCursor != null)
            {
                GameObjectUnderCursor.SetMeterValue(MeterType.Hitpoints, 0);
            }

            //if (inputState.IsKeyPressed(Keys.M))
            //{
            //    _backgroundCounter = (_backgroundCounter + 1) % BACKGROUND_NUM;
            //    background = new Background(_backgroundCounter);
            //}
            //if (inputState.IsKeyPressed(Keys.N))
            //    // Galactic speed test
            //    GalacticSpeedTest.Start(Utility.Frames(10f), 120f, 1 / 30f);
            //if (inputState.IsKeyPressed(Keys.OemComma) && GameObjectUnderCursor != null)
            //{
            //    // Damage target
            //    var amount = GameObjectUnderCursor.GetMeter(MeterType.Hitpoints).MaxValue * 0.1f;
            //    GameObjectUnderCursor.GetMeter(MeterType.Damage).AddValue(amount);
            //}

            //if (inputState.IsKeyPressed(Keys.OemPlus))
            //{
            //    loadoutIndex++;
            //    loadoutIndex = loadoutIndex % ContentBank.Inst.GetAllLoadout().Count;
            //    var textGameObject = ProjDamageText.Make().Emit(GameEngine, null, FactionType.Neutral, CursorWorldPosition + Vector2.UnitX * 50, Vector2.Zero,
            //        0, 0, 0, null, null, loadoutIndex) as Projectile;
            //    textGameObject.Param = loadoutIndex;

            //}

            //for (int i = 0; i < _keys.Count; i++)
            //{
            //    if (inputState.IsKeyPressed(_keys[i]))
            //    {

            //        //FactionType factionType = FactionType.Player;
            //        //if (_backgroundCounter % 2 == 0)
            //        var factionType = FactionType.Federation;
            //        var emitterID = ContentBank.Inst.GetAllLoadout()[loadoutIndex].ID + "_Gen";
            //        ContentBank.Inst.GetEmitter(emitterID).Emit(GameEngine, null, factionType, CursorWorldPosition, Vector2.Zero,
            //            0, 0, 0, null, null, i);
            //    }
            //}
            // Add Ships

        }
#endregion

        public Character GetCharacter(string id)
        {
            //Can get it fro metaworld??
            return ContentBank.Inst.CharacterBank.Get(id);
        }

        public Character GetCharacter(int id)
        {
            return ContentBank.Inst.CharacterBank.Get(id);
        }

    }
}
