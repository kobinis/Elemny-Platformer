//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils.SimpleGui;
//using SolarConflict.XnaUtils.SimpleGui;
//using XnaUtils.SimpleGui.Controllers;
//using Microsoft.Xna.Framework.Input;
//using System.Diagnostics;
//using SolarConflict.Framework.Utils;
//using SolarConflict.Framework.Scenes.Activitys;
//using XnaUtils.Graphics;

////TODO: Show default res, and write what is the res, show tooltip explaing that this is the current res of your screen
////Im Windowd selection the current state needs to be marked
//namespace SolarConflict.GameContent.Activities
//{
//    class InGameGraphicsSettingsActivity : SceneActivity
//    {
//        GuiManager _gui;

//        public InGameGraphicsSettingsActivity() : base(false)
//        {
//            _cover = Sprite.Get("cover2");
//            // Semi-KLUDGE: get current windowed mode from preferences, instead of by querying the actual window
//            // Preferences _should_ agree with the current game state on Activity construction, so this isn't too bad (if they disagree, it's an error)                        
//            CreateGui();
//        }

//        void CreateGui()
//        {
//            _gui = new GuiManager();



//            //  bool isFullscreen = GraphicsSettings.IsFullscreen;
//            var fullScreen = new RichTextControl("Fullscreen");
//            fullScreen.IsPressed = GraphicsSettings.IsFullscreen;
//            fullScreen.TextColor = Color.Black;
//            fullScreen.Sprite = Sprite.Get("guif8");
//            fullScreen.PressedControlColor = Color.LimeGreen;
//            fullScreen.IsToggleable = true;
//            fullScreen.Action += FullScreen_Action;
//            fullScreen.IsShowFrame = true;
//            fullScreen.CursorOverColor = Color.Yellow;
//            fullScreen.TooltipText = "Toggle fullscreen mode";
//            fullScreen.CursorOn += _gui.ToolTipHandler;

//            //GraphicsSettings.IsBorderless
//            var windowed = new RichTextControl("Borderless"); //_windowedMode == WindowedMode.Bordered ? 0 : 1;
//            windowed.IsPressed = GraphicsSettings.IsBorderless;
//            windowed.TextColor = Color.Black;
//            windowed.Sprite = Sprite.Get("guif8");
//            windowed.IsToggleable = true;
//            windowed.IsShowFrame = true;
//            windowed.PressedControlColor = Color.LimeGreen;
//            windowed.CursorOverColor = Color.Yellow;
//            windowed.Action += Windowed_Action;
//            windowed.TooltipText = "Toggle Borderless/Bordered window";
//            windowed.CursorOn += _gui.ToolTipHandler;
//            //   windowed.Action += (source, cursorInfo) => SetWindowedModeInstanceMethod(WindowedMode.Bordered);
//            //    radio.AddControl(windowed);

//            var lighting = new RichTextControl("Lighting");
//            lighting.IsPressed = GraphicsSettings.UseLighting;
//            lighting.TextColor = Color.Black;
//            lighting.Sprite = Sprite.Get("guif8");
//            lighting.PressedControlColor = Color.LimeGreen;
//            lighting.IsToggleable = true;
//            lighting.Action += Lighting_Action; ;
//            lighting.IsShowFrame = true;
//            lighting.CursorOverColor = Color.Yellow;
//            lighting.TooltipText = "Toggle lights (Effects performance)";
//            lighting.CursorOn += _gui.ToolTipHandler;

//            var postprocessing = new RichTextControl("Postprocessing");
//            postprocessing.IsPressed = GraphicsSettings.IsPostprocessing;
//            postprocessing.TextColor = Color.Black;
//            postprocessing.Sprite = Sprite.Get("guif8");
//            postprocessing.PressedControlColor = Color.LimeGreen;
//            postprocessing.IsToggleable = true;
//            postprocessing.Action += (s, c) => { GraphicsSettings.IsPostprocessing = s.IsPressed; };
//            postprocessing.IsShowFrame = true;
//            postprocessing.CursorOverColor = Color.Yellow;
//            postprocessing.TooltipText = "Toggle Postprocessing";
//            postprocessing.CursorOn += _gui.ToolTipHandler;

//            HorizontalLayout flagsLayout = new HorizontalLayout(Vector2.Zero);
//            flagsLayout.AddChilds(fullScreen, windowed, lighting, postprocessing);


//            var layout = new VerticalLayout(ActivityManager.ScreenCenter);

//            layout.AddChild(flagsLayout);
//            _gui.Root = layout;
//        }

//        private void Lighting_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
//        {
//            GraphicsSettings.UseLighting = source.IsPressed;
//            GraphicsSettings.SetGraphics();
//        }

//        private void Windowed_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
//        {
//            GraphicsSettings.IsBorderless = source.IsPressed;
//            GraphicsSettings.SetGraphics();
//            ActivityManager.ApplyGraphicsChanges();
//            //CreateGui();
//        }

//        private void FullScreen_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
//        {
//            GraphicsSettings.IsFullscreen = source.IsPressed;
//            GraphicsSettings.SetGraphics();
//            ActivityManager.ApplyGraphicsChanges();
//            //CreateGui();
//        }

//        public override ActivityParameters OnBack()
//        {
//            // RestoreSettings();
//            SettingsManager.Inst.Save();

//            return base.OnBack();
//        }

//        public override ActivityParameters OnLeave()
//        {
//            SaveSettings();
//            return base.OnLeave();
//        }

//        void SaveSettings()
//        {
//            SettingsManager.Inst.Save();
//            ActivityManager.Inst.AddToast("Settings saved", Utility.Frames(1f));
//        }

//        public override void Update(InputState inputState)
//        {
//            base.Update(inputState);
//            _gui.Update(inputState);
//        }

//        public override void Draw(SpriteBatch sb)
//        {
//            base.DrawBackground(sb);
//            _gui.Draw();
//            base.Draw(sb);
//        }

//        public static Activity ActivityProvider(string param) => new InGameGraphicsSettingsActivity();
//    }
//}
