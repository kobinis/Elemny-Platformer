using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    class EscapeMenuActivity: SceneActivity
    {
        protected GuiManager _gui;
        private MenuData _menuData;

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            _gui = new GuiManager();
            _back.ActivationKey = Microsoft.Xna.Framework.Input.Keys.None;
            _menuData = new MenuData(null);
            //Add Stats
            _menuData.AddEntry("Help", "HelpActivity", "HelpMain");
            _menuData.AddEntry("Settings", "PopupMenu", "InGameSettings.xml");
            _menuData.AddEntry("Close Menu  ", "back");
            if (_scene.SaveOnExit)
                _menuData.AddEntry("Save", "SaveGame");
            if (_scene.SaveOnExit && GameplaySettings.AutoSave)
            {
              
                _menuData.AddEntry("Save & Exit", "popback", "", false, Microsoft.Xna.Framework.Input.Keys.Enter);
            }
            else
                _menuData.AddEntry("Exit", "popback", "", false, Microsoft.Xna.Framework.Input.Keys.Enter);
            
            _gui.Root =  _menuData.MakeGui(_gui);
            AddHelp(TextBank.Inst.GetString("HelpMain"));
        }

       

        //public RichTextControl MakeOptionControl("sw")

        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            _gui.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);
            //_scene.background.Draw(_scene.Camera);
            base.Draw(sb);
            _gui.Draw();
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new EscapeMenuActivity();
        }
    }
}
