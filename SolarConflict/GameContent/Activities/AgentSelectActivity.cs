using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.XnaUtils.SimpleGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Input;
using XnaUtils.SimpleGui;

namespace SolarConflict.GameContent.Activities
{
   
    public class AgentSelectActivity : Activity
    {
        private GuiManager gui;
        private ScrollableGrid _agentGrid;
        string _targetActivity;
        string _targetActivityParams;
        string _selectedAgentID;
        Agent _selectedAgent;
        private string _loadoutPath;

        public AgentSelectActivity(string param) // public static readonly string Empty; const class
        {
            string[] paramaetres = param.Split(',');
            _targetActivity = "back";
            _targetActivityParams = string.Empty;
            if (paramaetres.Length > 0)
                _targetActivity = paramaetres[0];
            if (paramaetres.Length > 1)
            {
                _targetActivityParams = paramaetres[1];
                _loadoutPath = paramaetres[1];
            }
            else
            {
                _loadoutPath = "loadouts";
            }
            
            gui = new GuiManager();
            _agentGrid = new ScrollableGrid(9, 7, Vector2.One * 100); //TODO:  calculate number of lines and grid size according to res
            var ships = ContentBank.Inst.GetAllAgents();
            //ships = ships.Where(ship => ship.ItemSlotsContainer != null).OrderBy(ship => ship.ID).ToList();
            ships = ships.Where(ship => ship.ItemSlotsContainer != null).OrderByDescending(ship => (ship.gameObjectType & GameObjectType.Mothership) ).OrderByDescending(ship => ship.Size).OrderByDescending(s => s.GetFactionType()).ToList();
            foreach (var ship in ships)
            {
                HullControl control = new HullControl(ship, Vector2.One * 150, Vector2.One * 100);
                control.CursorOn += gui.ToolTipHandler;
                _agentGrid.AddChild(control);
                control.Action += OnShipSelected;
            }

            gui.Root = new GuiLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 10));
            gui.Root.AddChild(_agentGrid);

            gui.Update(InputState.EmptyState);
        }



        public override void OnEnter(ActivityParameters parameters)
        {
            //_loadoutPath = parameters.GetParam("LoadoutPath", Consts.AGENTS_SAVE_PATH);
        }


        private void OnShipSelected(GuiControl source, CursorInfo cursorLocation)
        {
            HullControl shipControl = (HullControl)source;
            _selectedAgentID = shipControl.Ship.ID;
            _selectedAgent = shipControl.Ship;
            ActivityParameters activityParameters = new ActivityParameters();
            activityParameters.ParamDictionary.Add("AgentID", _selectedAgentID);
            activityParameters.DataParams.Add("Agent", _selectedAgent.GetWorkingCopy());
            activityParameters.SetParam("LoadoutPath", _loadoutPath);
            ActivityManager.Inst.SwitchActivity(_targetActivity, activityParameters);
        }

        public override ActivityParameters OnLeave()
        {
            ActivityParameters activityParameters = new ActivityParameters();
            activityParameters.ParamDictionary.Add("AgentID", _selectedAgentID);
            activityParameters.DataParams.Add("Agent",_selectedAgent.GetWorkingCopy());
            return activityParameters;
        }

        #region Update/Draw
        public override void Update(InputState inputState)
        {
            gui.Update(inputState);
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();            
        }

        public override void Draw(SpriteBatch sb)
        {
            ActivityManager.GraphicsDevice.Clear(Color.Black);
            gui.Draw();
        }
        #endregion

        public static Activity ActivityProvider(string param)
        {
            return new AgentSelectActivity(param);
        }
    }
}
