using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.SimpleGui;
using SolarConflict.XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.Scenes.Activitys;
using XnaUtils.Graphics;
using SolarConflict.XnaUtils;

//TODO: Show default res, and write what is the res, show tooltip explaing that this is the current res of your screen
//Im Windowd selection the current state needs to be marked
namespace SolarConflict.GameContent.Activities
{
    class GraphicsSettingsActivity : SceneActivity
    {
        public static readonly Vector2 MinimumResolution = new Vector2(1200, 700);

        GuiManager _gui;
        List<Vector2> _supportedResolutions;
        private bool _hideResControl;

        public GraphicsSettingsActivity(bool hideRes) : base(false)
        {
            _hideResControl = hideRes;
            _cover = Sprite.Get("cover2");
            _supportedResolutions = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes
                .Select(m => new Vector2(m.Width, m.Height))
                .ToSet() // eliminate duplicates
                .Where(r => r.X >= MinimumResolution.X && r.Y >= MinimumResolution.Y)
                .OrderByDescending(r => r.X).ThenByDescending(r => r.Y)
                .ToList();
            //   _supportedResolutions.Add(Vector2.Zero);
            Debug.Assert(_supportedResolutions.Count > 0, "No valid resolutions");

            // Semi-KLUDGE: get current windowed mode from preferences, instead of by querying the actual window
            // Preferences _should_ agree with the current game state on Activity construction, so this isn't too bad (if they disagree, it's an error)                        
            CreateGui();
        }

        void CreateGui()
        {
            _gui = new GuiManager();

            

          //  bool isFullscreen = GraphicsSettings.IsFullscreen;
            var fullScreen = new RichTextControl( "Fullscreen");
            fullScreen.IsPressed = GraphicsSettings.IsFullscreen;
            fullScreen.TextColor = Color.Black;
            fullScreen.Sprite = Sprite.Get("guif8");
            fullScreen.PressedControlColor = Color.LimeGreen;
            fullScreen.IsToggleable = true;
            fullScreen.Action += FullScreen_Action;
            fullScreen.IsShowFrame = true;
            fullScreen.CursorOverColor = Color.Yellow;
            fullScreen.TooltipText = "Toggle fullscreen mode";
            fullScreen.CursorOn += _gui.ToolTipHandler;

            //GraphicsSettings.IsBorderless
            var windowed = new RichTextControl("Borderless"); //_windowedMode == WindowedMode.Bordered ? 0 : 1;
            windowed.IsPressed = GraphicsSettings.IsBorderless;
            windowed.TextColor = Color.Black;
            windowed.Sprite = Sprite.Get("guif8");
            windowed.IsToggleable = true;
            windowed.IsShowFrame = true;
            windowed.PressedControlColor = Color.LimeGreen;
            windowed.CursorOverColor = Color.Yellow;
            windowed.Action += Windowed_Action;
            windowed.TooltipText = "Toggle Borderless/Bordered window";
            windowed.CursorOn += _gui.ToolTipHandler;
            //   windowed.Action += (source, cursorInfo) => SetWindowedModeInstanceMethod(WindowedMode.Bordered);
            //    radio.AddControl(windowed);

            var lighting = new RichTextControl("Lighting");
            lighting.IsPressed = GraphicsSettings.UseLighting;
            lighting.TextColor = Color.Black;
            lighting.Sprite = Sprite.Get("guif8");
            lighting.PressedControlColor = Color.LimeGreen;
            lighting.IsToggleable = true;
            lighting.Action += Lighting_Action; ;
            lighting.IsShowFrame = true;
            lighting.CursorOverColor = Color.Yellow;
            lighting.TooltipText = "Toggle lights (Effects performance)";
            lighting.CursorOn += _gui.ToolTipHandler;

            var postprocessing = new RichTextControl("Postprocessing");
            postprocessing.IsPressed = GraphicsSettings.IsPostprocessing;
            postprocessing.TextColor = Color.Black;
            postprocessing.Sprite = Sprite.Get("guif8");
            postprocessing.PressedControlColor = Color.LimeGreen;
            postprocessing.IsToggleable = true;
            postprocessing.Action += Postprocessing_Action;
            postprocessing.IsShowFrame = true;
            postprocessing.CursorOverColor = Color.Yellow;
            postprocessing.TooltipText = "Toggle Postprocessing (Effects performance)";
            postprocessing.CursorOn += _gui.ToolTipHandler;


            var verSync = new RichTextControl("Vertical Sync");
            verSync.IsPressed = GraphicsSettings.IsVerticalSync;
            verSync.TextColor = Color.Black;
            verSync.Sprite = Sprite.Get("guif8");
            verSync.PressedControlColor = Color.LimeGreen;
            verSync.IsToggleable = true;
            verSync.Action += VerSync_Action; ;
            verSync.IsShowFrame = true;
            verSync.CursorOverColor = Color.Yellow;
            verSync.TooltipText = "Toggle Vertical Sync (Can effects performance)";
            verSync.CursorOn += _gui.ToolTipHandler;

            HorizontalLayout flagsLayout = new HorizontalLayout(Vector2.Zero);
            flagsLayout.AddChilds(fullScreen, windowed);
            HorizontalLayout flagsLayout2 = new HorizontalLayout(Vector2.Zero);
            flagsLayout2.AddChilds(verSync, lighting, postprocessing);

            var grid = new ScrollableGrid(1, 4, new Vector2(400f, 100f));        
            _supportedResolutions.Do(r =>
            {
                var control = new RichTextControl($"{(int)r.X} x {(int)r.Y}");
                control.IsToggleable = true;
                control.TextColor = Color.Black;
                control.Sprite = Sprite.Get("guif8");
                control.CursorOverColor = Color.Yellow;
                control.PressedControlColor = Color.LimeGreen;
                if (r == ActivityManager.ScreenSize)  //_currentRes)
                {
                    control.IsPressed = true;
                }
                control.IsShowFrame = true;
                control.Data = r;
                control.Action += (source, cursorInfo) =>
                {
                    Vector2 res = (Vector2)source.Data;
                    if (GraphicsSettings.ResWidth != (int)res.X || GraphicsSettings.ResHeight != (int)res.Y)
                    {
                        GraphicsSettings.ResWidth = (int)res.X;
                        GraphicsSettings.ResHeight = (int)res.Y;
                        GraphicsSettingsUtils.SetResolution();
                        ActivityManager.Inst.AddToast($"{GraphicsSettings.ResWidth} X {GraphicsSettings.ResHeight} ", 100);
                        CreateGui();
                    }
                };
                grid.AddChild(control);
            });

            //var saveChanges = new RichTextControl(" Save changes ", null, true);
            //saveChanges.TextColor = Color.Black;
            //saveChanges.Sprite = Sprite.Get("guif8");
            //saveChanges.CursorOverColor = Color.Yellow;
            //saveChanges.Action += (source, cursorInfo) => SaveSettings();

            var cancel = new RichTextControl(" Revert ", null, true);
            cancel.TextColor = Color.Black;
            cancel.Sprite = Sprite.Get("guif8");
            cancel.CursorOverColor = Color.Yellow;
            cancel.Action += (source, cursorInfo) => RestoreSettings();
            var layout = new VerticalLayout(ActivityManager.ScreenCenter);

            layout.AddChild(flagsLayout);
            layout.AddChild(flagsLayout2);
            if (!_hideResControl)
            {
                layout.AddChild(grid);
            }
            //layout.AddChild(saveChanges);
            RadioSelectionGroup fpsRadio = new RadioSelectionGroup();
            fpsRadio.SelectedControlIndex = (int)Math.Max( (1000/ActivityManager.Inst.Game.TargetElapsedTime.Milliseconds / 30) -1,0);
            HorizontalLayout fpsLayout = new HorizontalLayout(Vector2.Zero);

            var fps30 = new RichTextControl("30 FPS",isShowFrame:true);
            
            fps30.Action += (s, e) => { ActivityManager.Inst.Game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f /60); };
            fps30.PressedControlColor = Color.Yellow;
            fps30.IsToggleable = true;
            fpsRadio.AddControl(fps30);
            fpsLayout.AddChild(fps30);
            var fps60 = new RichTextControl("60 FPS", isShowFrame: true);
            fps60.Action += (s, e) => { ActivityManager.Inst.Game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 61); };
            fps60.PressedControlColor = Color.Yellow;
            fps60.IsToggleable = true;
            fpsRadio.AddControl(fps60);
            fpsLayout.AddChild(fps60);

            var fps120 = new RichTextControl("90 FPS", isShowFrame: true);
            fps120.Action += (s, e) => { ActivityManager.Inst.Game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 91); };
            fps120.PressedControlColor = Color.Yellow;
            fps120.IsToggleable = true;
            fpsRadio.AddControl(fps120);
            fpsLayout.AddChild(fps120);
            layout.AddChild(fpsLayout);
            layout.AddChild(cancel);
            _gui.Root = layout;
        }

        private void VerSync_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            GraphicsSettings.IsVerticalSync = source.IsPressed;
            GraphicsSettingsUtils.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = GraphicsSettings.IsVerticalSync;
            GraphicsSettingsUtils.GraphicsDeviceManager.ApplyChanges();
        }

        private void Postprocessing_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            GraphicsSettings.IsPostprocessing = source.IsPressed;
        }

        private void Lighting_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            GraphicsSettings.UseLighting = source.IsPressed;
            //GraphicsSettings.SetGraphics();
        }

        private void Windowed_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            GraphicsSettings.IsBorderless = source.IsPressed;
            GraphicsSettingsUtils.Borderless(ActivityManager.Inst.Game);
            GraphicsSettingsUtils.GraphicsDeviceManager.ApplyChanges();
            ActivityManager.Inst.AddToast(GraphicsSettings.IsBorderless ? "Set to Borderless" : "Set to Border", 100);
        }

        private void FullScreen_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            GraphicsSettings.IsFullscreen = source.IsPressed;
            GraphicsSettingsUtils.GraphicsDeviceManager.IsFullScreen = GraphicsSettings.IsFullscreen;
            GraphicsSettingsUtils.GraphicsDeviceManager.ApplyChanges();
            ActivityManager.Inst.AddToast(GraphicsSettings.IsFullscreen ? "Set to Fullscreen" : "Set to Windowed", 100);
        }

        public override ActivityParameters OnBack()
        {
            // RestoreSettings();
            SettingsManager.Inst.Save();

            return base.OnBack();
        }

        public override ActivityParameters OnLeave()
        {
         //   RestoreSettings();
            return base.OnLeave();
        }

        void RestoreSettings()
        {
            SettingsManager.Inst.Load();
            GraphicsSettingsUtils.SetResolution();
            //GraphicsSettings.SetGraphics();
            //ActivityManager.ApplyGraphicsChanges();
            CreateGui();
        }

        void SaveSettings()
        {        
            SettingsManager.Inst.Save();
            ActivityManager.Inst.AddToast("Settings saved", Utility.Frames(1f));
        }
      
        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            _gui.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.DrawBackground(sb);
            _gui.Draw();
            base.Draw(sb);
        }

        public static Activity ActivityProvider(string param) => new GraphicsSettingsActivity(param == "HideRes");
    }
}
