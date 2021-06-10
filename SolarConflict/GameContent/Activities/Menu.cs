using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework;
using SolarConflict.Framework.Menu;
using SolarConflict.Framework.Scenes.Activitys;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities
{
    /// <summary>
    /// Menu Screen
    /// </summary> 
    [Serializable]
    class Menu : Activity //change it, use Gui, Model View Controller
    {
        [NonSerialized] // TEMP
        private SpriteFont _menuFont;
        private string _parameters;

        //string ReturnActivityProviderName;
        //string ReturnActivityProviderParams;
        
        [NonSerialized] // TEMP
        private GuiManager _gui;
        public Background _background;
        [NonSerialized] // TEMP
        private Camera _camera;
        private Sprite _logo;
        private MenuData _menuData;
        public bool IsConfirmQuitNeeded = false;
        [NonSerialized] // TEMP
        private GuiManager _exitConfirm;
        private bool _isExitActive;
        private bool _isGamePaused = false;
        private MenuEntry _selectetEntry;
        public bool ShowBackground = true;

       // protected VerticalLayout _guiLayout;
        protected BackButton _back;

        public Menu(MenuData data)
        {
           // IsPopup = false;
            Init();
            _menuData = data;
            //_back.ActivationKey = Keys.None;
            MenuInit(data);
        }

        public Menu(string path, bool isPopup = false)
        {
            this.IsPopup = isPopup;
            Init();
            MenuInit(MenuData.LoadSettings(path));
           // MusicEngine.Instance.PlaySong(MusicEngine.MENU_SONG);
        }

        private void Init()
        {
            _logo = Sprite.Get("newlogo");
            _camera = new Camera();
        }
        

        public void RemoveMenuEntry(string displayText)
        {

            var menuEntry = _menuData.MenuEntryList.Find(entry => entry.DisplayText == displayText);
            if (menuEntry != null)
            {
                _menuData.MenuEntryList.Remove(menuEntry);
                MenuInit(_menuData);
            }
        }

        public override void Update(InputState inputState)
        {
            //base.Update(inputState);
            MenuUpdate(inputState);            
        }

        public override void Draw(SpriteBatch sb)
        {
            if(ShowBackground)
                _background.Draw(_camera);
            if (menuData.ShowLogo)
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                Vector2 pos = new Vector2((ActivityManager.ScreenWidth - _logo.Width * 1f) / 2, 30);
                sb.Draw(_logo, pos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                sb.End();
            }
            MenuDraw(sb);
            //base.Draw(sb);
        }

        public void InitScript(Scene scene, string parameters)
        {
            this._parameters = parameters;
            MenuInit(menuData);   //Check??
        }

        //public override void Reset()
        //{
        //    MusicEngine.Instance.StopFade(); //or menu song            
        //  //  MenuInit(parameters);  
        //}


        public void CursorOnEventHandler(GuiControl source, CursorInfo cursorLocation) //change
        {
            if (cursorLocation.OnPressLeft)
            {                
                _selectetEntry = menuData.MenuEntryList[int.Parse(source.UserData)];
                string name = _selectetEntry.ActivityName;
                ActivityParameters param = _selectetEntry.GetActivityParameters();                
                if (menuData.MenuEntryList[selectedItemIndex].activity != null)
                {
                    ActivityManager.Inst.SwitchActivity(menuData.MenuEntryList[selectedItemIndex].activity);
                }
                else
                {
                    ActivityManager.Inst.SwitchActivity(name, param);
                }
            }
        }        

        // bool menuInit;
        MenuData menuData;
        int selectedItemIndex;


        public void MenuInit(MenuData data)
        {
            _menuData = data;
            _menuFont = Game1.menuFont;
            CreateExitQuistion();

            //List<Sprite> backgroundTextures = new List<Sprite>(); //remove            
            //backgroundTextures.Add("tile3".ToSprite());
            _background = new Background(1);

            selectedItemIndex = 0;
            menuData = data;

            _gui = new GuiManager();
            //_gui.Root = new TextControl(menuData.Title, _menuFont);
            //_gui.Root.Position = new Vector2(ActivityManager.ScreenRectangle.Width / 2, 40);

            var gc = new GuiControl();
            gc.Sprite = null;
            _gui.Root = gc;

            var menuControl = _menuData.MakeGui(_gui, false, Sprite.Get("guiframe"), new Color(0, 22, 26, 200));
            menuControl.Position = new Vector2(ActivityManager.ScreenSize.X / 2, ActivityManager.ScreenSize.Y / 2);// - menuControl.HalfSize.Y);
            if (_menuData.ShowLogo)
                menuControl.Position = new Vector2(menuControl.Position.X, menuControl.Position.Y * 1.2f);
             
            _gui.Root.AddChild(menuControl);
           
            //for (int i = 0; i < menuData.MenuEntryList.Count; i++)
            //{
            //    string text = menuData.MenuEntryList[i].DisplayText;
            //    float scale = 1.6f;
            //    Vector2 textSize = Game1.font.MeasureString(text); //_menuFont.MeasureString(text);
            //    float ySize = (menuData.MenuEntryList.Count - 1) * (textSize.Y * scale + 10);
            //    //Vector2 position = new Vector2(_sb.GraphicsDevice.Viewport.Width / 2f, _sb.GraphicsDevice.Viewport.Height / 2f + i * (textSize.Y * scale + 10) - ySize / 2);

            //    RichTextControl textControl = new RichTextControl(text);//, _menuFont);
            //                                                    //  textControl.TextureDesign = (GuiDesign)GameGuiDesign.Menu;
            //    textControl.Position = new Vector2(0, _sb.GraphicsDevice.Viewport.Height / 2f + i * (textSize.Y * scale + 10) - ySize / 2);
            //    textControl.Width = _sb.GraphicsDevice.Viewport.Width / 4f;
            //    textControl.Data = menuData.MenuEntryList[i].Data;
            //    textControl.TextColor = Color.White;
            //    textControl.CursorOverColor = Color.Yellow;
            //    textControl.IsShowFrame = true;

            //    if (menuData.MenuEntryList[i].TooltipText != null)
            //    {
            //        textControl.TooltipText = menuData.MenuEntryList[i].TooltipText;
            //        textControl.CursorOn += _gui.ToolTipHandler;
            //    }

            //    if (menuData.MenuEntryList[i].Action != null)
            //    {
            //        textControl.Action += menuData.MenuEntryList[i].Action;
            //    }

            //    textControl.UserData = (i).ToString();
            //    _gui.Root.AddChild(textControl);
            //    textControl.CursorOn += CursorOnEventHandler;
            //    textControl.IsShowFrame = true;
            //}           
        }

        private void CreateExitQuistion()
        {
            _exitConfirm = new GuiManager();
            _isExitActive = false;
            _exitConfirm.Root = new GuiControl(Vector2.Zero, new Vector2(700, 300));
            Color color = _exitConfirm.Root.ControlColor;
            color.A = 250;
            _exitConfirm.Root.ControlColor = color;
            _exitConfirm.Root.CursorOverColor = color;

            GuiLayout layout = new GuiLayout(Vector2.Zero);         
            layout.baseVertical = -150;
            _exitConfirm.Root.AddChild(layout);

            TextControl quitTextControl = new TextControl(UIElmentsTexts.QuitQuistonMenu, _menuFont);
            TextControl yesControl = CreateExitYesButton(_menuFont);
            //  yesControl.TextureDesign = (GuiDesign)GameGuiDesign.Button;
            //  yesControl.Width = 204;
            //    yesControl.Height = 75;
            TextControl noControl = CreateExitNoButton(_menuFont);
            // noControl.TextureDesign = (GuiDesign)GameGuiDesign.Button;
            //    noControl.Width = 204;
            //   noControl.Height = 75;
            //TODO: make make them both the size of the bigger one

            layout.AddChild(quitTextControl);
            layout.AddChild(noControl);
            layout.AddChild(yesControl);
            _exitConfirm.Root.Position = new Vector2(ActivityManager.ScreenRectangle.Width / 2, ActivityManager.ScreenRectangle.Height/2);
        }

        private TextControl CreateExitNoButton(SpriteFont _menuFont)
        {
            TextControl noControl = new TextControl(UIElmentsTexts.Continue, _menuFont);
            noControl.ActivationKey = Keys.Escape;
            noControl.IsShowFrame = true;
            noControl.Action += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                CloseExitQuistion();
            };

            return noControl;
        }

        private static TextControl CreateExitYesButton(SpriteFont font)
        {
            TextControl yesControl = new TextControl(UIElmentsTexts.Quit, font);
            yesControl.IsShowFrame = true;
            yesControl.ActivationKey = Keys.Enter;
            yesControl.Action += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                ActivityManager.Inst.Back();
            };
            return yesControl;
        }

        public void AskExit(string text = null)
        {
            _isGamePaused = true;
            _isExitActive = true;
        }

        private void CloseExitQuistion()
        {
            _isGamePaused = false;
            _isExitActive = false;
        }

        public bool MenuUpdate(InputState inputState)
        {
            if (_isExitActive)
            {
                _exitConfirm.Update(inputState);
            }
            else
            {
                if (inputState.IsKeyPressed(Keys.Escape)) //??
                {
                    if (IsConfirmQuitNeeded)
                    {

                        AskExit();
                    }
                    else
                        ActivityManager.Inst.Back();
                }
            }

            if (!_isGamePaused)
            {
                _gui.Update(inputState);

                _camera.Position.X = _camera.Position.X + 2f;
            }

            return false;
        }

        public void MenuDraw(SpriteBatch sb)
        {
            _gui.Draw(new Color(200,255,255,200));
            if (_isExitActive)
                _exitConfirm.Draw();
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new Menu(parameters);
        }

        public static Activity PopupMenuActivityProvider(string parameters)
        {
            return new Menu(parameters, true);
        }



        public override void OnEnter(ActivityParameters parameters)
        {
        }

        public override void OnResume(ActivityParameters parameters = null)
        {
            MenuInit(_menuData);
            _isGamePaused = false;
            _isExitActive = false;
        }

        public override ActivityParameters OnBack()
        {
            return base.OnBack();
        }

        public override ActivityParameters OnLeave()
        {
            ActivityParameters parameters = _selectetEntry?.GetActivityParameters();
            return parameters;
        }
    }

    
}

    

   
