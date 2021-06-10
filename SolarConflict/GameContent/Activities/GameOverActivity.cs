using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using XnaUtils.Input;
using SolarConflict.Session;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Activities
{
    public class GameOverActivity : Activity
    {
        private Scene _scene;
        private Dictionary<string, string> _content;
        private List<string> _lines;
        private GuiManager _gui;
        private ActivityParameters activityParameters;

        public GameOverActivity()
        {
            IsPopup = true;
            IsDrawBackgroundActivity = true;
            _gui = new GuiManager();
            _content = new Dictionary<string, string>();
            _lines = new List<string>();
        }

        protected override void Init(ActivityParameters parameters)
        {
            activityParameters = new ActivityParameters();
            activityParameters.Parameter = parameters.Parameter;
            foreach (var param in parameters.ParamDictionary) //IF param dont start with %
            {
                activityParameters.ParamDictionary.Add(param.Key, param.Value);
            }

            _scene = parameters.DataParams["Scene"] as Scene;
            _content = parameters.ParamDictionary.Clone();
            ExtractStats(parameters);
            _gui.Root = MakeGui();
        }

        public override void Update(InputState inputState)
        {
            _gui.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            _gui.Draw();
        }

        private void ExtractStats(ActivityParameters parameters)
        {
            if (parameters.ParamDictionary.ContainsKey("text"))
            {
                _lines.Add(parameters.ParamDictionary["text"]);
            }
            int timeMin = (_scene.GameEngine.FrameCounter / 60) / 60;
            int timeSec = (_scene.GameEngine.FrameCounter / 60) % 60;
            _lines.Add("Time: " + timeMin.ToString() + ":" + timeSec.ToString());
            int shipsKilled = (int)_scene.GetPlayerFaction().GetMeter(MeterType.FactionKills).Value;
            _lines.Add("Ship kills: " + shipsKilled.ToString());
        }

        private GuiControl MakeGui()
        {
            VerticalLayout control = new VerticalLayout(ActivityManager.ScreenSize * 0.5f, 10, true);
            string title;
            if (!_content.TryGetValue("title", out title))
                title = "Level Summary";
            control.AddChild(new RichTextControl(title, Game1.menuFont));
            //Image
            foreach (var line in _lines)
            {
                RichTextControl lineControl = new RichTextControl(line);
                control.AddChild(lineControl);
            }
            var continueControl = new TextControl(" Continue ", Game1.font);
            continueControl.ActivationKey = Microsoft.Xna.Framework.Input.Keys.Space;
            continueControl.IsShowFrame = true;
            continueControl.Action += Continue;
            continueControl.SecondaryActivationKey = Microsoft.Xna.Framework.Input.Keys.Escape;
            control.AddChild(continueControl);

            return control;
        }

        private void Continue(GuiControl source, CursorInfo cursorLocation)
        {
            //ActivityManager.Inst.Back();
            PersistenceManager.Inst.Continue();
            //if (_content.ContainsKey("Activity"))
            //{
            //    string activityParams = activityParameters;

            //    if (_content.ContainsKey("ActivityParams"))
            //    {
            //        activityParams = _content["ActivityParams"];
            //    }

            //    ActivityManager.Inst.SwitchActivity(_content["Activity"], activityParams, false); //Fix
            //}
            //else
            //{
            //    GameSession.Inst.Continue(); //Not good; //Start screen countine 
            //}
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new GameOverActivity();
        }

    }
}
