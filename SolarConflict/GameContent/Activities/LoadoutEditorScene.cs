using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SolarConflict.AI.GameAI;
using SolarConflict.Framework;
using SolarConflict.Framework.Scenes.Components.Editors;
using SolarConflict.GameContent.ContentGeneration;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities
{
    public class LoadoutEditorScene: Scene
    {

        private Agent player;
        private Activity _editor;
        private GameObject light;
        private string _loadoutPath;
        public static Keys[] _keys = new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };

        private EquippedAgentGenerator generator;

        public LoadoutEditorScene():base(null, false)
        {
            
        }

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            base.InitScript(parameters, activityParameters);

            //this.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.FactionInfo);
            GameEngine.AddGameObject("Sun", FactionType.Neutral, Vector2.One * -500000);
            GameEngine.PermanentLights.AddRange(GameEngine.AddList);
            GameEngine.AddList.Clear();

            DialogManager.AddDialogBox("Press " + Color.Yellow.ToTag("Space") + " to add target\nPress " + Color.Yellow.ToTag("Enter") + " to add an enemy", null, isSkippable:false);
            GameEngine.AmbientColor = Vector3.One * 0.1f;
            IsConfirmQuitNeeded = true;
            this.gui = new GuiManager();
            TextControl textControl = new TextControl("Edit Loadout", Game1.font);
            textControl.SecondaryActivationKey = Microsoft.Xna.Framework.Input.Keys.I;
            textControl.ActivationLogicOperator = GuiControl.ActivationLogicOperatorType.Or;
            textControl.ActivationKey = Microsoft.Xna.Framework.Input.Keys.F2;
            textControl.IsShowFrame = true;
            gui.Root = textControl;
            textControl.Position =  ActivityManager.ScreenCenter.X * Vector2.UnitX + Vector2.UnitY * 30;
            textControl.Action += OnGui;
            string agentID = null;
            if (activityParameters != null && activityParameters.ParamDictionary.ContainsKey("AgentID"))
            {
                agentID = activityParameters.ParamDictionary["AgentID"];

            }
            else
            {
                agentID = MetaWorld.Inst.PlayerShip.GetId();
            }
            
            player = (Agent)ContentBank.Inst.GetEmitter(agentID).Emit(GameEngine, null, FactionType.Player, Vector2.Zero, Vector2.Zero, 0);
            //this.AddObjectRandomlyInCircle("Asteroid0", 80, 10000);
            //this.AddObjectRandomlyInCircle("SmallAsteroid0", 50, 10000);
            _loadoutPath = activityParameters.GetParam("LoadoutPath", "Loadouts");
            _editor = new LoadoutEditorActivity(this,player as Agent, _loadoutPath);

            //GameEngine.AmbientColor = Vector3.Zero;
            var lightObj = new Agent();
            lightObj.Light = Lights.HugeLight(Color.Yellow);
            light = lightObj;
            generator = new EquippedAgentGenerator(player);

            IsShipSwitchable = true;
            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Imbue);
        }

        public override void OnEnter(ActivityParameters parameters) //Change to init
        {
            base.OnEnter(parameters);
        }

        public override ActivityParameters OnLeave()
        {
            ActivityParameters p = new ActivityParameters();
            p.DataParams["ship"] = player;
            return p;
        }

        private void OnGui(GuiControl source, CursorInfo cursorLocation)
        {
            _editor.OnResume();

            ActivityManager.Inst.SwitchActivity(_editor);
        }

        public override void UpdateScript(InputState inputState)
        {
            if(player != null)
            {
                player.CurrentHitpoints = Math.Max(player.CurrentHitpoints, 200);
            }

            for (int i = 0; i < _keys.Length; i++)
            {
                if (inputState.IsKeyPressed(_keys[i]))
                {
                    if(!inputState.IsKeyDown(Keys.LeftShift))
                    {
                        ContentBank.Get(player.ID + "_sg").Emit(GameEngine, null, FactionType.Player, player.Position + Vector2.UnitX * 300, Vector2.Zero, 0, param: (i + 1));
                    }
                    else
                    {
                        player.ItemSlotsContainer.ClearItems();
                        Agent.EquipAgent(player, i + 1, true);
                        ActivityManager.Inst.AddToast("Equip Ship Level:" + (i + 1).ToString(), 100);
                    }                                                                                
                }
            }

            if (inputState.IsKeyPressed(Keys.Enter))
            {
                var target = this.AddGameObject("SmallTargetShip", FactionType.Pirates1, this.CursorWorldPosition);
               // var meter = target.GetMeter(MeterType.Hitpoints);
               // meter.MaxValue = 4000;
               // meter.Value = meter.MaxValue;
            }

            if (inputState.IsKeyPressed(Keys.Back))
            {
                var target = this.AddGameObject("Asteroid1", FactionType.Neutral, this.CursorWorldPosition);
                // var meter = target.GetMeter(MeterType.Hitpoints);
                // meter.MaxValue = 4000;
                // meter.Value = meter.MaxValue;
            }



            if (inputState.IsKeyPressed(Keys.Space))
            {
                var target = this.AddGameObject("Base2", FactionType.TradingGuild, this.CursorWorldPosition);
                var meter = target.GetMeter(MeterType.Hitpoints);
                meter.MaxValue = 4000;
                meter.Value = meter.MaxValue;
            }

            if (inputState.IsKeyPressed(Keys.F3))
            {
                player.control.controlAi = ParameterControl.MakeAIFromAgent(player);
                ActivityManager.Inst.AddToast("AI generated", 200);
            }

            if (inputState.IsKeyPressed(Keys.F4))
            {
                var target = player.Emit(GameEngine, null, FactionType.Player, this.CursorWorldPosition, Vector2.Zero, player.Rotation) as Agent;
                target.SetControlType(AgentControlType.AI);
                target.control.controlAi = ParameterControl.MakeAIFromAgent(target);
                ActivityManager.Inst.AddToast("AI generated", 200);
            }

            if (inputState.IsKeyPressed(Keys.F5))
            {
                player.SetControlType(AgentControlType.Player);
                ActivityManager.Inst.AddToast("AI generated", 200);
            }

            if (inputState.IsKeyPressed(Keys.F6))
            {
                var id = player.ID;
                var target = this.AddGameObject(id, FactionType.Pirates1, this.CursorWorldPosition);
            }
            light.Position = CursorWorldPosition;
           
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new LoadoutEditorScene();
        }

        public void AddPlayerShip(int level)
        {
            //player.IsNotActive = true;
            //GameEngine.RemoveGameObject(player);      
            //player = generator.Emit(this.GameEngine, null, FactionType.Player, Vector2.Zero, Vector2.Zero, 0) as Agent;
            //player.Revive();
            //player.SetControlType(AgentControlType.Player);
        }

    }
}
