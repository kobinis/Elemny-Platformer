
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using SolarConflict.Session.World.MissionManagment;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework.Scenes.Activitys;
using Microsoft.Xna.Framework;
using XnaUtils.Input;
using XnaUtils.Graphics;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    public class CodexTmpActivity : SceneActivity
    {
        private Sprite selectedSprite;
        private Sprite unselectedSprite;
        protected GuiManager _gui;
        protected VerticalLayout _layout;

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            _gui = new GuiManager();
            _layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
            _gui.AddControl(_layout);
            _layout.ShowFrame = true;
            selectedSprite = Sprite.Get("selected");
            unselectedSprite = Sprite.Get("unselected");
            InitGui();
        }

        private void InitGui()
        {
            RichTextControl title = new RichTextControl("Codex (work in progress):", Game1.menuFont);
            _layout.AddChild(title);
            ScrollableGrid grid = new ScrollableGrid(1, 6, new Vector2(ActivityManager.ScreenSize.X - 200, 100 * GuiManager.Scale));
            _layout.AddChild(grid);
            var missions = MetaWorld.Inst.CodexManager.GetPastMissionList();
            for (int i = missions.Count -1; i >= 0; i--)
            {
                var mission = missions[i];
                if (!mission.IsHidden || DebugUtils.Mode == ModeType.Test)
                    grid.AddChild(MakeMissionControl(mission, _scene));
            }
           
            //AddHelp(TextBank.Inst.GetString("HelpMissionLog"));
        }

        public GuiControl MakeMissionControl(Mission mission, Scene scene)
        {
            var result = new RelativeLayout();
            result.Spacing = 13;
            result.HalfSize = new Vector2(ActivityManager.ScreenSize.X - 200, 100 * GuiManager.Scale);
            result.TooltipText = mission.GetTooltipText();
            result.CursorOn += _gui.ToolTipHandler;
            result.ShowFrame = true;
         

            var textControl = new RichTextControl(mission.Title);
            textControl.TextColor = mission.Color;
            textControl.IsConsumingInput = false;
            textControl.IsShowFrame = false;
            textControl.PressedControlColor = new Color(0, 0, 255);
            result.AddChild(textControl, HorizontalAlignment.Center, VerticalAlignment.Center);
            //var chain = new List<GuiControl>();
            //chain.Add(text);
            //result.AddChain(Alignment.LeftToRight, new Vector2(0.5f, 0.5f), 100f, 0f, text);


            HorizontalLayout horizontalLayout = new HorizontalLayout(Vector2.Zero);

            if (mission.Icon != null)
            {
                ImageControl image = new ImageControl(mission.Icon, Vector2.Zero, Vector2.One * 60 * GuiManager.Scale);
                image.IsConsumingInput = false;

                horizontalLayout.AddChild(image);
                //chain.Insert(0, image);
                //result.AddChain(Alignment.LeftToRight, image);
            }

            horizontalLayout.SetHorizontalPositions();
            horizontalLayout.RefreshSize();
            result.AddChild(horizontalLayout, HorizontalAlignment.Right, VerticalAlignment.Center);
            result.Update(InputState.EmptyState);
            return result;
        }



        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            _gui.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);
            _gui.Draw();
            base.Draw(sb);
        }


        public static Activity ActivityProvider(string parameters)
        {
            return new CodexTmpActivity();
        }
    }
}

