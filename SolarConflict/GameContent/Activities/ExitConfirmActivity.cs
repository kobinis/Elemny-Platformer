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
using XnaUtils.Graphics;
using Microsoft.Xna.Framework;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    class ExitConfirmActivity : Activity
    {
        protected GuiManager _gui;
        private MenuData _menuData;

        public ExitConfirmActivity()
        {
            IsPopup = true;
            IsDrawBackgroundActivity = true;
            //IsBlocking = false;
        }

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            _gui = new GuiManager();           
            _menuData = new MenuData(null);
            _menuData.Title = "Do you want to exit?";
            _menuData.AddEntry("Cancel", "back","", false, Microsoft.Xna.Framework.Input.Keys.Escape);
            _menuData.AddEntry("Exit", "popback", "", false, Microsoft.Xna.Framework.Input.Keys.Enter);
            Color color = Palette.GuiBody.Multiply(1.1f, false);
            color.A = 255;
            _gui.Root = _menuData.MakeGui(_gui, false, null, color);
        }



        //public RichTextControl MakeOptionControl("sw")

        public override void Update(InputState inputState)
        {
            _gui.Update(inputState);            
        }

        public override void Draw(SpriteBatch sb)
        {
            _gui.Draw();
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new ExitConfirmActivity();
        }
    }
}

