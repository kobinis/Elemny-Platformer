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
    public class MissionTestActivity : SceneActivity
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
            RichTextControl title = new RichTextControl("Active Quests:", Game1.menuFont);
            _layout.AddChild(title);
            ScrollableGrid grid = new ScrollableGrid(1, 8, new Vector2(600, 100));
            _layout.AddChild(grid);
            var missions = _scene.GetMissions();
            foreach (var mission in missions)
            {
                if (!mission.IsHidden || DebugUtils.Mode == ModeType.Debug)
                    grid.AddChild(MakeMissionControl(mission, _scene));
            }
            AddHelp(TextBank.Inst.GetString("HelpMissionLog"));
        }

        public GuiControl MakeMissionControl(Mission mission, Scene scene)
        {
            var result = new RelativeLayout();
            result.HalfSize = new Vector2(600, 100);
            result.TooltipText = mission.GetTooltipText();
            result.CursorOn += _gui.ToolTipHandler;
            result.ShowFrame = true;

            var toggle = new ImageControl(unselectedSprite, Vector2.Zero, Vector2.One * 60);
            toggle.PressedSprite = selectedSprite;
            toggle.IsPressed = mission.IsSelected;
            toggle.CursorOn += _gui.ToolTipHandler;
            toggle.TooltipText = "Select/Unselect the mission";
            toggle.IsToggleable = true;
            toggle.Data = mission;
            toggle.LogicFunction = (controlArg, inputState) => { (controlArg.Data as Mission).IsSelected = controlArg.IsPressed; };

            result.AddChild(toggle, HorizontalAlignment.Left, VerticalAlignment.Center);
            //result.AddChain(Alignment.RightToLeft, 0f, 100f, toggle);


            var textControl = new RichTextControl(mission.ID);            
            textControl.TextColor = mission.Color;
            textControl.IsConsumingInput = false;
            textControl.IsShowFrame = false;
            textControl.PressedControlColor = new Color(0, 0, 255);
            result.AddChild(textControl, HorizontalAlignment.Center, VerticalAlignment.Center);
            


            HorizontalLayout horizontalLayout = new HorizontalLayout(Vector2.Zero);

            if (mission.Icon != null)
            {
                ImageControl image = new ImageControl(mission.Icon, Vector2.Zero, Vector2.One * 60);
                image.IsConsumingInput = false;

                horizontalLayout.AddChild(image);
                //chain.Insert(0, image);
                //result.AddChain(Alignment.LeftToRight, image);
            }

            if (mission.IsDismissable)
            {
                ImageControl deleteControl = new ImageControl(Sprite.Get("trash1"), Vector2.Zero, Vector2.One * 50);
                deleteControl.Data = new Tuple<Scene, Mission>(scene, mission);
                deleteControl.Action += delegate (GuiControl source, CursorInfo cursorLocation) {
                    var tuple = source.Data as Tuple<Scene, Mission>;
                    tuple.Item1.RemoveMission(tuple.Item2);
                    source.Parent.Parent.Parent.Parent.RemoveChild(source.Parent.Parent); //FIx
                    ActivityManager.Inst.AddToast("Mission was dismissed", 60);
                };

                horizontalLayout.AddChild(deleteControl);
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
            return new MissionTestActivity();
        }
    }
}
