using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.Framework.Scenes.HudEngine.Components;
using SolarConflict.Framework.Scenes.DialogEngine;
using Microsoft.Xna.Framework.Input;

namespace SolarConflict.Framework.Scenes
{
    
    public class SkirmishBattle : Scene
    {        
        float _spawnRangeFromCenter = 5000f;        
        int _exitTimer = 0;
        bool _gameOver;
        int RespwanTime = 60 * 3;
        Dictionary<FactionType, List<Agent>> _agentsLists;        
        DummyObject _centerTarget;
        int endLevelTimer;
               

        public SkirmishBattle(string parameters = null) : base(parameters, passFactions: false) {
            
                       
            SceneComponentSelector.AddComponent(SceneComponentType.Inventory);
            _centerTarget = new DummyObject();            
            //CameraManager.ZoomType = CameraZoomType.Auto;
            this.GameEngine.AmbientColor = Vector3.One * 0.6f;
            this.MetaWorld = null;

            IsShipSwitchable = true;
            UpdatePlayerShip = false;
            // Sow hatred
            GameEngine.GetFaction(FactionType.Player).SetRelationToFaction(GameEngine, FactionType.Federation, -1f);
            GameEngine.GetFaction(FactionType.Federation).SetRelationToFaction(GameEngine, FactionType.Player, -1f);
            GameEngine.GetFaction(FactionType.Player).Name = "Player";
            GameEngine.GetFaction(FactionType.Pirates1).Name = "Enemy";
            _agentsLists = new Dictionary<FactionType, List<Agent>>();
            

        }        

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
           // CameraManager.ZoomType = CameraZoomType.Auto;
            // CameraManager.MovmentType = CameraMovmentType.TwoPlayers;
            HudManager.AddComponent(new TargetIndicator());
            HudManager.AddComponent(new AnnouncerEffects());
        }

        public override void UpdateScript(InputState inpuState)
        {
            if (DebugUtils.Mode == ModeType.Debug)
            {
                var player = PlayerAgent;
                for (int i = 0; i < DebugUtils.DebugKeys.Length; i++)
                {
                    if (inpuState.IsKeyPressed(DebugUtils.DebugKeys[i]))
                    {
                        if (!inpuState.IsKeyDown(Keys.LeftShift))
                        {
                            ContentBank.Get(player.ID + "_sg").Emit(GameEngine, null, FactionType.Player, player.Position + Vector2.UnitX * 300, Vector2.Zero, 0, param: (i + 1));
                            ActivityManager.Inst.AddToast("Added Ship Level:" + (i + 1).ToString(), 100);
                        }
                        else
                        {
                            ContentBank.Get(player.ID + "_sg").Emit(GameEngine, null, FactionType.Pirates1, player.Position + Vector2.UnitX * 3000, Vector2.Zero, 0, param: (i + 1));
                            ActivityManager.Inst.AddToast("Added Enemy Ship Level:" + (i + 1).ToString(), 100);
                        }
                    }
                }
            }

            if (this.SceneFrameCounter == 3)
                DialogManager.AddDialog(new Dialog("Battle the enemy!", "help")); ;
            foreach (var factionListPair in _agentsLists)
            {
                FactionType factionType = factionListPair.Key;
                int index = 0;
                foreach (var agent in factionListPair.Value)
                {
                    if(agent.IsNotActive)
                    {
                        if (agent.GetMeterValue(MeterType.RespawnTimer) >= RespwanTime)
                        {
                            agent.Revive();
                            float rotation = MathHelper.Pi;
                            if (factionType == FactionType.Player)
                                rotation = 0;
                            Vector2 position = FMath.RotateVector(-_spawnRangeFromCenter * Vector2.UnitX + FMath.GetFormationPosition(index) * 600, rotation);
                            agent.Position = position;
                            agent.AddMeterValue(MeterType.Lives, -1);
                            agent.SetMeterValue(MeterType.RespawnTimer, 0);
                            GameEngine.AddList.Add(agent); //todo: add warp in effect
                            AddGameObject("HyperSpaceJumpFx", FactionType.Neutral, position);
                        }
                        else
                        {
                            agent.AddMeterValue(MeterType.RespawnTimer, 1);
                        }
                    }
                   
                    index++;
                }
            }
            int numOfKills = 19;
            if(GameEngine.GetFaction(FactionType.Player).GetMeter(MeterType.FactionKills).Value> numOfKills || GameEngine.GetFaction(FactionType.Pirates1).GetMeter(MeterType.FactionKills).Value > numOfKills)
            {
                if (endLevelTimer == 0)
                {
                    CameraManager.TargetPosition = Vector2.Zero;
                    if(GameEngine.GetFaction(FactionType.Player).GetMeter(MeterType.FactionKills).Value > numOfKills)
                    {
                      //  GameEngine.AddGameObject("VictoryImage", Framework.FactionType.Alliance, Vector2.Zero);// camera.GetWorldPos(delarsHP.Position));
                        GameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Camera.Position + Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                        GameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Camera.Position + Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                        GameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Camera.Position + Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                        
                    }
                    else
                    {
                        //GameEngine.AddGameObject("DefeatImage", Framework.FactionType.Alliance, Vector2.Zero);// camera.GetWorldPos(delarsHP.Position));
                        //fadeAlpha = endLevelTimer / 200f;                        
                    }

                }
                endLevelTimer++;
            }
            
            if(endLevelTimer == 200)
            {
                ActivityManager.Inst.Back();
            }
        }



        public override void DrawScript(SpriteBatch sb)
        {
            sb.Begin();
            int index = 0;
            foreach (var item in _agentsLists)
            {
                Faction faction = GameEngine.GetFaction(item.Key);
                Color color = Color.White;
                string text = faction.Name + " Kills: " + faction.GetMeter(MeterType.FactionKills).Value.ToString();
                Vector2 textSize = Game1.font.MeasureString(text);
                Vector2 position = new Vector2(ActivityManager.ScreenSize.X * 0.5f + (index * 2 - 1) * ActivityManager.ScreenSize.X * 0.25f, 20) - textSize * 0.5f;
                sb.DrawString(Game1.font, text, position, color);
                index++;
            }
            sb.End();
        }

        public void AddLoadout(AgentLoadout loadout, FactionType factionType, bool isFirstPlayer)
        {
            AgentControlType controlType = AgentControlType.AI;            
            if(!_agentsLists.ContainsKey(factionType))
            {
                _agentsLists.Add(factionType, new List<Agent>());
                if (factionType == FactionType.Player)
                    controlType = AgentControlType.Player;
                else
                {
                    if(isFirstPlayer)
                        controlType = AgentControlType.CoPlayerOrAI1;
                }
            }
            int index = _agentsLists[factionType].Count;
            float rotation = MathHelper.Pi;
            if (factionType == FactionType.Player)
                rotation = 0;

            Vector2 position = FMath.RotateVector(-_spawnRangeFromCenter* Vector2.UnitX + FMath.GetFormationPosition(index) * 600, rotation);
            Agent agent = loadout.Emit(GameEngine, null, factionType, position, Vector2.Zero, rotation) as Agent;            
            agent.SetControlType(controlType);
            agent.SetTarget(_centerTarget, TargetType.Goal);
            agent.targetSelector.SetAggroRange(_spawnRangeFromCenter * 2, _spawnRangeFromCenter * 3, TargetType.Enemy);
            _agentsLists[factionType].Add(agent);
                        
        }        
                
    }
}
