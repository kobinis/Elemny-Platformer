using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using SolarConflict.Framework.GUI;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    class HangarActivity : SceneActivity
    {
        private GuiManager gui;

        protected override void Init(ActivityParameters parameters)
        {           
            base.Init(parameters);            
            gui = new GuiManager();
            VerticalLayout layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
            gui.Root = layout;
            var parts = _scene.GetPlayerFaction().hullPartsCounter;
            layout.AddChild(GuiControlFactory.MakeHangerControl(gui, _scene.GetPlayerFaction(), _scene.GameEngine));
            VerticalLayout hullsLayout = new VerticalLayout(Vector2.Zero, 8, true);
            hullsLayout.AddChild(new RichTextControl("Available Hulls", Game1.menuFont));
            hullsLayout.AddChild(GuiControlFactory.MakeKnownHullControl(gui, _scene.GetPlayerFaction()));
            layout.AddChild(hullsLayout);
            AddHelp(TextBank.Inst.GetString("HelpHangar"));
        }


        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            gui.Update(inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            gui.Draw();
            base.Draw(spriteBatch);
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new HangarActivity();
        }
    }
}
