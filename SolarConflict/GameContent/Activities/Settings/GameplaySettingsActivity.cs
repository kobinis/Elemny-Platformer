using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities.Settings
{
    class GameplaySettingsActivity : Activity
    {
        private GuiManager _gui;
        private Texture2D _cover;


        public GameplaySettingsActivity()
        {
            IsPopup = true;
        }

        protected override void Init(ActivityParameters parameters)
        {
            _cover = TextureBank.Inst.GetTexture("cover2");
            _gui = new GuiManager();
            _gui.AddControl(MakeGui(_gui));

            var back = new BackButton(Vector2.Zero, Vector2.One * 45);
            back.Position = Vector2.One * 10 + back.HalfSize;
            back.ActivationKey = Keys.Escape;
            back.ControlColor = Palette.GuiFrame;
            _gui.AddControl(back);
        }

        public override void Update(InputState inputState)
        {
            _gui.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            GraphicsUtils.DrawBackground(_cover, sb);
            sb.End();
            _gui.Draw();

        }

        public static GuiControl MakeGui(GuiManager gui)
        {
            //    public static bool AutoSortInventory = true;
            //public static bool IsFilter = true;
            //public static bool AutoSave = true;
            //public static bool KeepInventory { get { return Difficulty == GameDifficulty.Easy; } set { } }
            //public static GameDifficulty Difficulty = GameDifficulty.Normal;
            //public static bool SkipTutorial = false;

            //public static bool DirectionalControl = true; //Reletive, Directional, Fly by Wire
            VerticalLayout layout = new VerticalLayout(Vector2.Zero);
            layout.IsResizeChildrenHorizontally = true;

            layout.Position = ActivityManager.ScreenCenter;
            layout.AddChild(StartingShipSelectActivity.MakeDifficultyControl(gui));
            layout.AddChild(MakeCheckboxControl("Fly by Wire", "alot of stuff", gui, GameplaySettings.DirectionalControl,
                (s, c) => { GameplaySettings.DirectionalControl = s.IsPressed; }));

            layout.AddChild(MakeCheckboxControl("Auto Save", "alot of stuff", gui, GameplaySettings.AutoSave,
                (s, c) => { GameplaySettings.AutoSave = s.IsPressed; }));

            //MakeCheckboxControl("")
            return layout;

        }

        public override ActivityParameters OnBack()
        {
            SettingsManager.Inst.Save();
            return null;
        }



        public static GuiControl MakeCheckboxControl(string text, string tooltip, GuiManager gui, bool isPressed, ActionEventHandler action)
        {
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero, showFrame:true);    
            var toggle = new ImageControl(Sprite.Get("unselected"), Vector2.Zero, Vector2.One * 60 * GuiManager.Scale);
            toggle.PressedSprite = Sprite.Get("selected");
            toggle.IsPressed = isPressed;
            toggle.CursorOn += gui.ToolTipHandler;
            toggle.TooltipText = tooltip;
            toggle.IsToggleable = true;
            toggle.Action += action;
            RichTextControl textControl = new RichTextControl(text);
            layout.AddChilds(toggle, textControl);
            return layout;
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new GameplaySettingsActivity();
        }
    }
}
