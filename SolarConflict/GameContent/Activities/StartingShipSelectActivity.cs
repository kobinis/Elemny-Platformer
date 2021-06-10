using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using XnaUtils.SimpleGui.Controllers;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.GUI;

namespace SolarConflict.GameContent.Activities
{
    /// <summary>
    /// Activity for selecting starting ship
    /// </summary>
    class StartingShipSelectActivity : Activity
    {
        GuiManager gui;
        private string testActivity;
        private string targetActivity;
        //RadioSelectionGroup radioGroup;
        private string loadoutsString;
        private RadioSelectionGroup shipRadio;
        List<string> ships;



        protected override void Init(ActivityParameters parameters)
        {
            
            testActivity = "TestShipScene";
            targetActivity = parameters.GetParam("target_activity");
            loadoutsString = parameters.GetParam("loadouts");
            ships = parameters.GetList("loadouts");
            gui = new GuiManager();
            //List<string> ships = new List<string> { "PrologShip1B", "PrologShip2B" };
            VerticalLayout layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f, showFrame:true);
            layout.Spacing = 5;
            var title = new RichTextControl("Select Starting Ship:", Game1.menuFont);
            //title.IsShowFrame = true;
            layout.AddChild(title);

            layout.AddChild(MakeShipSelectionControl(ships, gui, out shipRadio));
            layout.AddChild(MakeDifficultyControl(gui));
            layout.AddChild(MakeSkipTutorialControl(gui));
            title.Width = layout.Width;

            VerticalLayout spacingLayout = new VerticalLayout(Vector2.Zero);

            RichTextControl startGameControl = new RichTextControl(" \n     Start Game!     \n ", isShowFrame: true);
            startGameControl.ControlColor = new Color(100, 255, 200);
            startGameControl.CursorOverColor = Color.Yellow;
            startGameControl.Action += SwitchActivityToTarget;
            spacingLayout.AddChild(startGameControl);
            layout.AddChild(spacingLayout);

            gui.AddControl(layout);
            gui.Update(InputState.EmptyState);

        }

        private GuiControl MakeSkipTutorialControl(GuiManager gui)
        {
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero);
            var toggle = new ImageControl(Sprite.Get("unselected"), Vector2.Zero, Vector2.One * 60 * GuiManager.Scale);
            toggle.PressedSprite = Sprite.Get("selected");
            toggle.IsPressed = GameplaySettings.SkipTutorial;
            toggle.CursorOn += gui.ToolTipHandler;
            toggle.TooltipText = TextBank.Inst.GetTextAsset("GameplaySettings.SkipTutorial").Text;
            toggle.IsToggleable = true;            
            toggle.LogicFunction = (controlArg, inputState) => { GameplaySettings.SkipTutorial = controlArg.IsPressed; };
            RichTextControl richTextControl = new RichTextControl("Skip Tutorial");
            layout.AddChilds(toggle, richTextControl);
            return layout;
        }


        public static GuiControl MakeDifficultyControl(GuiManager gui)
        {
            VerticalLayout layout = new VerticalLayout(Vector2.Zero, showFrame:true);
            RichTextControl title = new RichTextControl("Difficulty");
            //    VerticalLayout difficultyLayout = new VerticalLayout(Vector2.Zero, showFrame:false);
            layout.IsResizeChildrenHorizontally = true;
            layout.AddChilds(title);
            RadioSelectionGroup radio = new RadioSelectionGroup();
            radio.SelectedControlIndex = (int)GameplaySettings.Difficulty;
            string[] text = { "Easy", "Normal" };
            for (int i = 0; i < 2; i++)
            {
                var control = new RichTextControl(text[i], null, true);
                control.Index = i;
                control.TooltipText = ((GameDifficulty)i).GetUserName();
                control.CursorOn += gui.ToolTipHandler;
                control.IsToggleable = true;
                control.PressedControlColor = Color.Yellow;
                control.Action += (c, m) => { GameplaySettings.Difficulty = (GameDifficulty)c.Index; };
                radio.AddControl(control);
                layout.AddChild(control);
            }
            return layout;
        }


        private GuiControl MakeShipSelectionControl(List<string> loadouts, GuiManager gui, out RadioSelectionGroup radio)
        {
           // radioGroup = new RadioSelectionGroup();
            HorizontalLayout shipLayout = new HorizontalLayout(Vector2.Zero);
            shipLayout.ShowFrame = false;
            radio = new RadioSelectionGroup();
            for (int i = 0; i < loadouts.Count; i++)
            {
                var shipcontrol = MakeShipControl(loadouts[i], i, gui);
                radio.AddControl(shipcontrol);
                shipLayout.AddChild(shipcontrol);
            }
            return shipLayout;
        }

        private GuiControl MakeShipControl(string loadoutID, int index, GuiManager gui)
        {            
            VerticalLayout layout = new VerticalLayout(Vector2.Zero);
            layout.ShowFrame = true;
            layout.PressedControlColor = Color.Yellow;
            layout.IsToggleable = true;
            var loadout = ContentBank.Inst.GetLoadout(loadoutID);
            string tooltip;
            if((loadout.Agent.gameObjectType & GameObjectType.NonRotating) > 0)
            {
                tooltip = TextBank.Inst.GetTextAsset("NonRotatingInfo").Text;
            }
            else
            {
                tooltip = TextBank.Inst.GetTextAsset("RotatingInfo").Text;
            }
            LoadoutControl control = new LoadoutControl(loadout, Vector2.Zero, Vector2.One*170);
            control.IsConsumingInput = false;
            control.CursorOn += gui.ToolTipHandler;
            control.TooltipText = tooltip;
            //control.Action += SwitchActivityToTarget;
            layout.AddChild(control);
            layout.Index = index;
            layout.UserData = loadoutID;
            //RichTextControl playControl = new RichTextControl("Play!");
            //playControl.CursorOn += gui.ToolTipHandler;
            //playControl.TooltipText = tooltip;
            //playControl.IsShowFrame = true;
            //playControl.UserData = targetActivity;
            //playControl.Action += SwitchActivity;
            //playControl.CursorOverColor = control.CursorOverColor;
            //layout.AddChild(playControl);
            RichTextControl testControl = new RichTextControl("Test");
            testControl.CursorOverColor = control.CursorOverColor;
            testControl.IsShowFrame = true;
            testControl.UserData = testActivity;
            testControl.Action += TestShipActivity;
            testControl.CursorOn += gui.ToolTipHandler;
            testControl.TooltipText = "Test ship before you decide.";
            layout.AddChild(testControl);
            return layout;
        }

        private void SwitchActivityToTarget(GuiControl source, CursorInfo cursorLocation)
        {
            ActivityParameters activityParams = new ActivityParameters();
            activityParams.ParamDictionary.Add("loadout", ships[shipRadio.SelectedControlIndex]);
            activityParams.ParamDictionary.Add("loadouts", loadoutsString);
            activityParams.ParamDictionary.Add("index", shipRadio.SelectedControlIndex.ToString());
            activityParams.ParamDictionary.Add("target_activity", targetActivity);
            ActivityManager.Inst.SwitchActivity(targetActivity, activityParams, false);
        }

        private void TestShipActivity(GuiControl source, CursorInfo cursorLocation)
        {
            ActivityParameters activityParams = new ActivityParameters();
            activityParams.ParamDictionary.Add("loadout", source.Parent.UserData);
            activityParams.ParamDictionary.Add("loadouts", loadoutsString);
            activityParams.ParamDictionary.Add("index", source.Parent.Index.ToString());
            activityParams.ParamDictionary.Add("target_activity", targetActivity);
            ActivityManager.Inst.SwitchActivity(source.UserData, activityParams, true);
        }

        public override void Update(InputState inputState)
        {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
            gui.Update(inputState);
        }

        private void DrawBrackground(SpriteBatch sb, Texture2D cover)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            GraphicsUtils.DrawBackground(cover, sb);
            sb.End();
        }

        public override void Draw(SpriteBatch sb)
        {
            DrawBrackground(sb, TextureBank.Inst.GetTexture("cover2"));
            gui.Draw();
        }



        public static Activity ActivityProvider(string parameters)
        {
            return new StartingShipSelectActivity();
        }
    }
}
