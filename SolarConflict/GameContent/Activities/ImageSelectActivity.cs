using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using SolarConflict.XnaUtils.Files;
using SolarConflict.XnaUtils.SimpleGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities
{

    public class ImageSelectActivity : Activity
    {
        private GuiManager gui;
        private ScrollableGrid _agentGrid;
        string _targetActivity;
        string _targetActivityParams;
        string _selectedAgentID;
        Agent _selectedAgent;

        public ImageSelectActivity(string param) // public static readonly string Empty; const class
        {
            string[] paramaetres = param.Split(',');
            _targetActivity = "back";
            _targetActivityParams = string.Empty;
            if (paramaetres.Length > 0)
                _targetActivity = paramaetres[0];
            if (paramaetres.Length > 1)
                _targetActivityParams = paramaetres[1];

            var files = FileUtils.GetFiles(Path.Combine(Consts.GAME_DATA_PATH, "Textures\\Ships"), "*.png", SearchOption.TopDirectoryOnly);


            gui = new GuiManager();
            _agentGrid = new ScrollableGrid(9, 7, Vector2.One * 100); //TODO:  calculate number of lines and grid size according to res
            var ships = ContentBank.Inst.GetAllAgents();
            ships = ships.Where(ship => ship.ItemSlotsContainer != null).OrderBy(ship => ship.ID).ToList();
            // ships = ships.Where(ship => ship.ItemSlotsContainer != null).OrderByDescending(ship => (ship.gameObjectType & GameObjectType.Mothership) ).OrderByDescending(ship => ship.Size).OrderByDescending(s => s.GetFactionType()).ToList();
            foreach (var ship in files)
            {
                string sprite = Path.GetFileNameWithoutExtension(ship);
                if (sprite.EndsWith("_normals"))
                    continue;
                ImageControl control = new ImageControl(Sprite.Get(sprite), Vector2.Zero, Vector2.One * 100);
                control.TooltipText = sprite;
                control.CursorOn += gui.ToolTipHandler;
                _agentGrid.AddChild(control);
                control.Action += OnShipSelected;
            }

            gui.Root = new GuiLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 10));
            gui.Root.AddChild(_agentGrid);

            gui.Update(InputState.EmptyState);
        }


        private void OnShipSelected(GuiControl source, CursorInfo cursorLocation)
        {
            string agentID  = source.Sprite.ID + "Hull";
            Agent agent = null;
            try
            {
                if (!ContentBank.Inst.ContainsEmitter(agentID))
                {
                    ImageControl shipControl = (ImageControl)source;
                    ShipData shipData = new ShipData(source.Sprite.ID , 1000);
                    agent = ShipQuickStart.Make(shipData);
                    agent.ID = source.Sprite.ID;
                    ShipQuickStart.AddBasicGearSlots(agent);
                    ShipQuickStart.FinalizeShip(agent);
                    ContentBank.Inst.AddContent(agent, true);
                }
                else
                {
                    agent = ContentBank.Get(agentID) as Agent;
                }
            }
            catch (Exception)
            {

                ActivityManager.Inst.AddToast("Ship was added before", 200);
            }
            
            _selectedAgentID = agent.ID;
            _selectedAgent = agent;
            ActivityParameters activityParameters = new ActivityParameters();
            activityParameters.ParamDictionary.Add("AgentID", _selectedAgentID);
            activityParameters.DataParams.Add("Agent", _selectedAgent.GetWorkingCopy());
            ActivityManager.Inst.SwitchActivity(_targetActivity, activityParameters);
        }

        public override ActivityParameters OnLeave()
        {
            ActivityParameters activityParameters = new ActivityParameters();
            activityParameters.ParamDictionary.Add("AgentID", _selectedAgentID);
            activityParameters.DataParams.Add("Agent", _selectedAgent.GetWorkingCopy());
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
            gui.Draw();
        }
        #endregion

        public static Activity ActivityProvider(string param)
        {
            return new ImageSelectActivity(param);
        }
    }
}
