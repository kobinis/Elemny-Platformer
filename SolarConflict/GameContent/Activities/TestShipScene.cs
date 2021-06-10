using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities
{
    /// <summary>
    /// Test starting ship scene
    /// </summary>
    class TestShipScene:Scene
    {
        private string targetActivity;
        private string loadoutID;
        //private GuiManager _gui;
        ActivityParameters activityParameters;

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            this.activityParameters = activityParameters;
            IsShipSwitchable = true;
            loadoutID = "PrologShip2";
            if (activityParameters != null && activityParameters.ParamDictionary.ContainsKey("loadout"))
            {
                loadoutID = activityParameters.ParamDictionary["loadout"];
            }

            targetActivity = activityParameters.GetParam("target_activity", "back");
            //Help
            var sun = AddGameObject("Sun", -Vector2.One* 100000);
            this.AddObjectRandomlyInCircle("Asteroid1", 80, 50000);
            this.AddObjectRandomlyInCircle("SmallAsteroid1", 50, 60000);
           
            //List<string> ships = activityParameters.GetList("loadouts");
            //for (int i = 0; i < ships.Count; i++)
            //{
            //    AgentControlType controlType = AgentControlType.AI;
            //    if (ships[i] == loadoutID)
            //        controlType = AgentControlType.Player;
            //    AddGameObject(ships[i], new Vector2((i/(float)ships.Count - 0.5f) * 1000, 0), 0, FactionType.Player, controlType);
            //}
            AddGameObject(loadoutID, new Vector2(0, 0), 0, FactionType.Player, AgentControlType.Player);
            gui = new GuiManager();
            gui.AddControl(MakeGui());
        }

        public override void UpdateScript(InputState inputState)
        {
            base.UpdateScript(inputState);
            //if(PlayerAgent != null)
            //    loadoutID = this.PlayerAgent.ID;            
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new TestShipScene();
        }

        private GuiControl MakeGui()
        {
            HorizontalLayout layout = new HorizontalLayout(new Vector2(ActivityManager.ScreenSize.X * 0.5f, ActivityManager.ScreenSize.Y * 0.9f));
            RichTextControl playControl = new RichTextControl("Play!", Game1.menuFont, true);
            playControl.Action += SwitchActivity;
            layout.AddChild(playControl);
            GuiControl backControl = new RichTextControl("Back", Game1.menuFont, true);
            backControl.Action += BackButton.Exit_Action;
            layout.AddChild(backControl);

            return layout;
        }

        private void SwitchActivity(GuiControl source, CursorInfo cursorLocation)
        {
            //activityParameters.ParamDictionary["loadout"] = loadoutID;
            ActivityManager.Inst.SwitchActivity(targetActivity, activityParameters, false);
        }

    }
}
