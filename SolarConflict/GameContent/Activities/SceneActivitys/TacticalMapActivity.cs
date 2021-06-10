using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework;
using SolarConflict.Framework.CameraControl.Movment;
using SolarConflict.Framework.CameraControl.Zoom;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.Activitys;
using SolarConflict.GameContent.Activities.SceneComponents;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.XnaUtils;
using SolarConflict.XnaUtils.SimpleGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    public class TacticalMapActivity: SceneActivity
    {
        public const string MARKER_MISSION_ID = "_m";
        const float zoomLevel2 = 0.1f;
        private GuiManager _gui;
        private new GuiControl _layout;
        private Camera _newCamera;
        private Camera _oldCamera;
        private ManualMovement _cameraMovmentLogic;
        private ManualZoomToPoint _cameraZoomLogic;
        private Sprite texture;
        // private GameObject _objectUnderCursor;
        private Agent _selectedFriendlyShip;
        private GameObject _selectedObject;
        private Vector2 _cursorWorldPosition;
        private RichTextControl _tooltipControl;
        private InputState _inputState;
        private Mission _markerMission;
       // private Bl

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            _markerMission = _scene.GetMission(MARKER_MISSION_ID);     
            _gui = new GuiManager();
            _layout = new ControlsGroup();
            _gui.AddControl(_layout);
            _layout.AddChild( InitGui(_gui));

            _cameraMovmentLogic = new ManualMovement();
            _cameraZoomLogic = new ManualZoomToPoint();
            _cameraZoomLogic.MinZoom = 0.005f;         
            _newCamera = new Camera();
            _newCamera.Zoom = 0.05f;
            _oldCamera = _scene.GameEngine.Camera;
            _newCamera.Position = _oldCamera.Position;
            _scene.GameEngine.Camera = _newCamera;
            _tooltipControl = new RichTextControl(string.Empty);
            texture = Sprite.Get("hexagon");

            _selectedFriendlyShip = _scene.PlayerAgent;

            TextControl positionDisplay = new TextControl("Position:");
            positionDisplay.IsShowFrame = true;
            positionDisplay.Data = _newCamera;            
            positionDisplay.LogicFunction = (GuiControl control, InputState inputState) => 
            {
                Camera cam = control.Data as Camera;
                Vector2 position = cam.GetWorldPos(inputState.Cursor.Position);
                (control as TextControl).Text = (position * Consts.PixelsToUinits) .ToString(".00");
                control.Position = new Vector2(control.HalfSize.X + 10, ActivityManager.ScreenSize.Y - control.HalfSize.Y - 10);
            };
            _gui.AddControl(positionDisplay);
            AddHelp(TextBank.Inst.GetString("HelpTacticalMap"));
        }

        private GuiControl InitGui(GuiManager gui)
        {
          //  _selectedFriendlyShipTargetsVisualizer = new Framework.Agents.Systems.ShowTargetSystem();

            var playerFaction = _scene.GetPlayerFaction();
            var ships = playerFaction.MothershipHanger.FleetShips;
            ships.Add(playerFaction.Mothership);
            int gridNum = Math.Min(5, ships.Count);
            var agentsGridControl = new ScrollableGrid(1, gridNum, new Vector2(150, 150));            
            RadioSelectionGroup radioGroup = new RadioSelectionGroup();
            GuiControl selectedControl = null;
            int i = 0;
            foreach (var ship in ships)
            {                                    
                var agentControl = new AgentGuiControl(ship);                
                agentControl.IsToggleable = true;
                agentControl.PressedControlColor = Color.Yellow;
                agentControl.CursorOn += (source, cursorLocation) => 
                {
                    source.TooltipText = GetCommandText((source as AgentGuiControl).Agent);
             //       source.TooltipText = playerFaction.Mothership.GetSystem<FleetSystem>().FleetSlots[i].Command.ToString();
                };
                i++;
                agentControl.CursorOn += gui.ToolTipHandler;
                agentControl.TooltipText = GetCommandText(ship);              
                agentsGridControl.AddChild(agentControl);
                agentControl.Action += ShipControlHandler;
                radioGroup.AddControl(agentControl);
                if (ship == _selectedFriendlyShip)
                    selectedControl = agentControl;
            }
            radioGroup.SelectedControl = selectedControl;
            agentsGridControl.Position = new Vector2(agentsGridControl.HalfSize.X, ActivityManager.ScreenSize.Y * 0.5f);            
            return agentsGridControl;
        }

        public static string GetCommandText(Agent agent)
        {
            if (agent.GetControlType() == AgentControlType.Player)
                return "Player";
            StringBuilder sb = new StringBuilder();
            if(agent.targetSelector.GetTarget(TargetType.Enemy) != null)
            {
                sb.AppendLine("Attacking " + agent.targetSelector.GetTarget(TargetType.Enemy).Name+" #image{" +  agent.targetSelector.GetTarget(TargetType.Enemy).GetSprite().ID + "}");
            }
            if (agent.targetSelector.GetTarget(TargetType.Goal) != null)
            {
                sb.AppendLine("Following " + agent.targetSelector.GetTarget(TargetType.Goal).Name + " #image{" + agent.targetSelector.GetTarget(TargetType.Goal).GetSprite().ID + "}");
            }
            if (agent.targetSelector.GetTarget(TargetType.Ally) != null)
            {
                sb.AppendLine("Defending " + agent.targetSelector.GetTarget(TargetType.Ally).Name + " #image{" + agent.targetSelector.GetTarget(TargetType.Ally).GetSprite().ID + "}");
            }
            return sb.ToString();
        }

        private void ShipControlHandler(GuiControl source, CursorInfo cursorLocation)
        {
            AgentGuiControl control = source as AgentGuiControl;
            if(_selectedFriendlyShip == control.Agent)
                _newCamera.Position = control.Agent.Position;
            _selectedFriendlyShip = control.Agent;
        }

        public override void Update(InputState inputState)
        {
            _scene.GameEngine.Camera = _newCamera;
            base.Update(inputState);
            _gui.Update(inputState);
            _inputState = inputState;
            _cameraMovmentLogic.Update(_newCamera, null, null, _scene.GameEngine, inputState);
            _cameraZoomLogic.Update(_newCamera, null, null, _scene.GameEngine, inputState);
            _cursorWorldPosition = _newCamera.GetWorldPos(inputState.Cursor.Position);
            var _objectUnderCursor = _scene.GameEngine.FindGameObjectInPosition(_cursorWorldPosition, 10, GameObjectType.Agent | GameObjectType.Asteroid | GameObjectType.PotentialTarget);
            UpdateTooltip(_objectUnderCursor);
            if(_objectUnderCursor != null && inputState.Cursor.OnPressLeft)
            {
                _selectedObject = _objectUnderCursor;
            }
            _scene.GameEngine.Camera = _oldCamera;
            if(inputState.Cursor.OnPressRight && inputState.Cursor.IsActive)
            {
                var mission = MissionFactory.MapMarker(_cursorWorldPosition);
                _scene.AddMission(mission);
                _markerMission = mission;
            }
            if (inputState.Cursor.OnPressLeft && inputState.Cursor.IsActive && _objectUnderCursor != null)
            {
                if (_selectedFriendlyShip != null)
                {
                    _selectedFriendlyShip.SetTarget(_objectUnderCursor, TargetType.Goal);
                    if (_objectUnderCursor.IsHostileToPlayer(_scene.GameEngine))
                        _selectedFriendlyShip.SetTarget(_objectUnderCursor, TargetType.Enemy);
                }

            }
            if(DebugUtils.Mode == ModeType.Debug || DebugUtils.Mode == ModeType.Test)
            {
                if(inputState.IsKeyDown(Keys.Y) )
                {
                    Vector2 position = _newCamera.GetWorldPos(inputState.Cursor.Position);
                    if (_scene.PlayerAgent != null)
                        _scene.PlayerAgent.Position = position;
                    else
                        _scene.Camera.Position = position;
                    ActivityManager.Inst.AddToast("Debug Warp", 10);

                }
            }
        }

        private void UpdateTooltip(GameObject objectUnderCursor)
        {
            if(objectUnderCursor != null)
            {
                _tooltipControl.Text = objectUnderCursor.Tag;
            }
            else
            {
                _tooltipControl.Text = string.Empty;
            }
        }

        public void DrawZoomedOut(SpriteBatch sb, float startingZoomLevel)
        {
            float alpha = MathHelper.Clamp((zoomLevel2 - _newCamera.Zoom) * 10, 0, 1);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: _newCamera.Transform);

            foreach (var item in _scene.GameEngine._collideAllCheckList) 
            {
                if ((item.GetObjectType() & GameObjectType.PotentialTarget) > 0)
                {
                    Color col = FactionColorIndicator.FactionToColor(item.GetFactionType());
                    col.A = (byte)(alpha * 255);
                    _newCamera.CameraDraw(texture, item.Position, 0, item.Size / 40f, col);
                }
                //sb.DrawString( Game1.font
                //Add Text
            }
            sb.End();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _newCamera.UpdateMatrix();
            _scene.GameEngine.Camera = _newCamera;
            float alpha = MathHelper.Clamp((zoomLevel2 - _newCamera.Zoom) * 10, 0, 1);
            _scene.GameEngine.Camera = _newCamera;
            spriteBatch.GraphicsDevice.Clear(new Color(0,10,30));
            
            if (GraphicsSettings.IsPostprocessing)
            {
                ActivityManager.GraphicsDevice.SetRenderTarget(GraphicsSettingsUtils.renderTargetFullA);
            }
            //_scene.background.Draw(_newCamera, 0.01f, false);
            _scene.GameEngine.Draw(spriteBatch);  
            if(_newCamera.Zoom < zoomLevel2)
                DrawZoomedOut(spriteBatch, zoomLevel2);

            spriteBatch.Begin(transformMatrix: _newCamera.Transform);
            if(_markerMission != null)
                _newCamera.CameraDraw(Sprite.Get("marker"), _markerMission.GetPosition().Value, 0,1, _markerMission.Color);
            

            // Indicate selected ship's targets
            if (_selectedFriendlyShip != null)
            {
                //_selectedFriendlyShipTargetsVisualizer.Draw(_newCamera, _selectedFriendlyShip, _selectedFriendlyShip.Position, _selectedFriendlyShip.Rotation);                                
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: _newCamera.Transform);
            // Indicate selected ship's ship
            if (_selectedFriendlyShip != null)
            {
              // _selectedFriendlyShipTargetsVisualizer.Draw(_newCamera, _selectedFriendlyShip, _selectedFriendlyShip.Position, _selectedFriendlyShip.Rotation);
                var size = Math.Max(_selectedFriendlyShip.Size * 2.4f + 100, 30f / _newCamera.Zoom);
                Sprite sprite = Sprite.Get("glowring");
                var scale = size / sprite.Width;

                _newCamera.CameraDraw(sprite.Texture, _selectedFriendlyShip.Position, 0, scale, _scene.GetPlayerFaction().Color);
            }
            spriteBatch.End();
            spriteBatch.Begin();
            if (!string.IsNullOrWhiteSpace(_tooltipControl.Text))
            {
                Vector2 pos = _inputState.Cursor.Position + _tooltipControl.HalfSize + new Vector2(15f, -20f);
                _tooltipControl.IsShowFrame = true;
                _tooltipControl.Position = pos;
                _tooltipControl.Draw(spriteBatch);
            }
            spriteBatch.End();
            _gui.Draw();
            base.Draw(spriteBatch);
            _scene.GameEngine.Camera = _oldCamera;
        }

        public override ActivityParameters OnBack()
        {
            _scene.GameEngine.Camera = _oldCamera;
            return base.OnBack();
        }

        public override ActivityParameters OnLeave() //TODO: check why it is not beeing called
        {
            _scene.GameEngine.Camera = _oldCamera;
            return base.OnLeave();
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new TacticalMapActivity();
        }
    }
}
