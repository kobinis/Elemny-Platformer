using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using System.Collections.Generic;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using XnaUtils.Graphics;
using System.Threading;
using System.IO;
using XnaUtils.Framework.Graphics;
using System.Linq;
using System.Diagnostics;

namespace SolarConflict.Framework.Scenes.Components
{

    public class DisclaimerActivity : Activity
    {
        public string[] loadingText = { "Loading..", "Tip: Add ships to your fleet", "Tip: You can always warp home", "Tip: you can place 4 items in your hotbar", "Tip: You can change the binding of weapons slots", "Searching for the Auryn", "Destroying Stars", "Building Von Neumann probes", "Loading..."};
        public bool IsLoadingOver;
        private GuiManager _gui;
        private TextControl _backButton;
        private RichTextControl _pressSpace;
        private Camera _camera;
        private Thread _thread;
        private Texture2D _cover1;        
        Texture2D _endCover;
        RichTextControl _message;
        int _currentLoadingMessage;
        private const int LOADING_MESSEAGE_TIME = 240;
        private int loadingMessageCooldown;

        int _currentSlide;   
        private string[] _slides;

        int _timer;

        public DisclaimerActivity(Thread thread)
        {
            _thread = thread;
            IsLoadingOver = false;
            

            _camera = new Camera();                        
            InitSlides();
            _cover1 = Game1.cover2; 
            _endCover = Game1.cover1;
            CreateGui();

            //_loadingMessageChangeTime = _timer + _loadingMessageDuration;
        }

        void CreateGui() {
            _gui = new GuiManager();
            _gui.Root = new GuiControl();

            var slide = _slides[_currentSlide];

            var layout = new VerticalLayout(Vector2.Zero);
            layout.IsUpdatingPosition = true;
            _message = new RichTextControl(slide, Game1.orbitron14Black);
            _message.TextColor = Color.White;
            _message.Shadow = new TextShadow(offset: 2);
            _message.MaxLineWidth = ActivityManager.ScreenSize.X * 0.85f * 0.5f;
           // _message.Font = Game1.orbitron14Black;
            _message.Parser.Scale = 2f;
            layout.AddChild(_message);
            _pressSpace = new RichTextControl("\nPress space to continue");
            _pressSpace.Shadow = new TextShadow();
            layout.AddChild(_pressSpace);            
            layout.Spacing = 20f;
            layout.Position = ActivityManager.ScreenCenter; ;

            _gui.Root.AddChild(layout);

            var backButtonText = IsLoadingOver ? "Start Game" : loadingText[_currentLoadingMessage];
            _backButton = new TextControl(backButtonText);//, Game1.menuFont);
            _backButton.Sprite = GuiManager.BackTexture;
            _backButton.CursorOverColor = _backButton.ControlColor;
            _backButton.IsShowFrame = true;
            _backButton.Position = new Vector2(ActivityManager.ScreenCenter.X, ActivityManager.ScreenSize.Y - _backButton.HalfSize.Y - 60);

            _gui.Root.AddChild(_backButton);            
        }

        void InitSlides() {
            _slides = new string[] {
                 "In the 21st century, mankind embarked on a journey to conquer the stars. "+
                 "Manned planetary missions began, with probes dispatched to the far corners of the galaxy. " +
                 "We needed an answer to the question: Are we alone in the universe? "+
                 "As it turns out, we were. And thus began the expansion of our race throughout the milky way. ",
                 "After a golden era of expansion came conflict. With nothing to stop mankind, we soon reverted to our old ways. " +
                 "Humanity evolved, but conflict was in our DNA: warring factions, failed expeditions, and fragile alliances that never last. ",
                 "Now things have escalated again, with a supernova taking out an inhabited star system. " +
                 "Someone threw a spanner in the works. And as you are that spanner, it's time to get some answers."
            };            
        }

        void NextSlide() {
            if (_currentSlide == _slides.Length-1)
            {
                _pressSpace.Text = string.Empty;
                _message.Text = string.Empty;
                // No more slides
                return;
            }
            _currentSlide++;
            _message.Text = _slides[_currentSlide];
        }
        
        public override void Update(InputState inputState)
        {
            _timer++; // TODO: do we actually enforce framerate here, or just in GameEngine?

            if (_timer % 100 == 0 && !_thread.IsAlive)
            {
                IsLoadingOver = true;
                _backButton.Text = " Start Game ";
                _backButton.CursorOverColor = Color.Yellow;
            }

            if (IsLoadingOver) {
                // Do we wanna advance to the game/main menu?
                // We can do so by pressing space after the slide show ends
                var startWithSpace = _currentSlide == _slides.Length - 1 && inputState.IsKeyPressed(Keys.Space);

                // Or we could just press the start control or some other keys
                if (startWithSpace || _backButton.IsPressed || inputState.IsKeyPressed(Keys.Escape) || inputState.IsKeyPressed(Keys.Enter)) {
                    // Continue to main menu
                    var menuActivity = ActivityManager.Inst.SwitchActivity("MainMenu", string.Empty, false);
                    menuActivity.OnEnter(null);
                    ActivityManager.Inst.DefaultActivity = menuActivity;
                }
            }

            loadingMessageCooldown--;
            // Update loading message            
            if (loadingMessageCooldown <= 0 && !IsLoadingOver) {
                ++_currentLoadingMessage;
                loadingMessageCooldown = LOADING_MESSEAGE_TIME;
                _backButton.Text = loadingText[_currentLoadingMessage % loadingText.Length];              
            }

            // Update slides
            if (inputState.IsKeyPressed(Keys.Space))
                NextSlide();            
               
            _gui.Update(inputState);            
            
            
        }

        float alpha = 0;
        public override void Draw(SpriteBatch sb)
        {
            if(IsLoadingOver)
            {
                alpha = Math.Min(alpha + 0.01f, 1);
            }
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            GraphicsUtils.DrawBackground(_cover1, sb);
            GraphicsUtils.DrawBackground(_endCover, sb, new Color(1f, 1f, 1f, alpha));
            sb.End();
            _gui.Draw();
        }                
    }
}
