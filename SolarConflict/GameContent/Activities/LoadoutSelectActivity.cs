using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    public class LoadoutSelectActivity : Activity
    {
        const int SIZE = 500;
        GuiManager _gui;
        ScrollableGrid _agentGrid;
        string _targetActivity;
        string _targetActivityParams;
        string _selectedAgentID;
        AgentLoadout _selectedAgent;
        

        public LoadoutSelectActivity(string param) // public static readonly string Empty; const class
        {
            string[] paramaetres = param.Split(',');
            _targetActivity = "back";
            _targetActivityParams = string.Empty;
            if (paramaetres.Length > 0)
                _targetActivity = paramaetres[0];
            if (paramaetres.Length > 1)
                _targetActivityParams = paramaetres[1];

            _gui = new GuiManager();
            _agentGrid = new ScrollableGrid(9, 7, Vector2.One * SIZE); //TODO:  calculate number of lines and grid size according to res
            var ships = ContentBank.Inst.GetAllLoadout();



            foreach (var ship in ships)
            {
                LoadoutControl control = new LoadoutControl(ship, Vector2.One * 150, Vector2.One * SIZE);
                control.CursorOn += _gui.ToolTipHandler;
                _agentGrid.AddChild(control);
                control.Action += EditShipSlots;
            }

            _gui.Root = new GuiLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 10));
            _gui.Root.AddChild(_agentGrid);

            _gui.Update(InputState.EmptyState);
        }

        //TODO: understand and complete
        private void EditShipSlots(GuiControl source, CursorInfo cursorLocation)
        {
            LoadoutControl shipControl = (LoadoutControl)source;
            _selectedAgentID = shipControl.Loadout.ID;
            _selectedAgent = shipControl.Loadout;
            ActivityManager.Inst.SwitchActivity(_targetActivity, _targetActivityParams);
            //ActivityManager.SwitchActivity(new ShipEditActivity(shipControl.Ship));
        }

        public override ActivityParameters OnLeave()
        {
            ActivityParameters activityParameters = new ActivityParameters();
            activityParameters.ParamDictionary.Add("LoadoutID", _selectedAgentID);
            activityParameters.DataParams.Add("Loadout", _selectedAgent);
            return activityParameters;
        }

        #region Update/Draw
        public override void Update(InputState inputState)
        {
            _gui.Update(inputState);
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();            
        }

        public override void Draw(SpriteBatch sb)
        {
            ActivityManager.GraphicsDevice.Clear(Color.Black);
            _gui.Draw();
        }
        #endregion

        public static Activity ActivityProvider(string param)
        {
            return new LoadoutSelectActivity(param);
        }
    }
}
