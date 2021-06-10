using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.CameraControl.Zoom;
using SolarConflict.Framework.PlayersManagement;
using XnaUtils.SimpleGui;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework.Utils;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework.CameraControl.Movment;
using SolarConflict.XnaUtils;

namespace SolarConflict.GameContent.Activities
{
    class MainMenu:Activity
    {
        Background _background;
        GameEngine _gameEngine;
        GuiManager _gui;
        MenuData _menuData;
        ManualZoom _zoom;
        ManualMovement _movment;
        private Sprite _logo;

        float _chanceOfBlob = 0.01f;
        
        public override void OnResume(ActivityParameters parameters = null)
        {
            InitManMenu();
            InitGame();
        }

        public override void OnEnter(ActivityParameters parameters)
        {
            InitManMenu();
            InitGame();
        }

        protected override void Init(ActivityParameters parameters)
        {
            InitManMenu();
            InitGame();
        }

        public override ActivityParameters OnLeave()
        {
            _gameEngine = null;
            return base.OnLeave();  
        }



        public override void Draw(SpriteBatch sb)
        {
            if (GraphicsSettings.IsPostprocessing) //todo: change 
            {
                ActivityManager.GraphicsDevice.SetRenderTarget(GraphicsSettingsUtils.renderTargetFullA);
            }

            _background.Draw(_gameEngine.Camera);
        
            _gameEngine.Draw(sb);

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            Vector2 pos = new Vector2((ActivityManager.ScreenSize.X - _logo.Width) * 0.5f, 20);
            sb.Draw(_logo.Texture, pos, Color.White);
            sb.End();

            _gui.Draw();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            string buildName = DebugUtils.BuildName;
            if (DebugUtils.Mode != ModeType.Release)
                buildName += " " + DebugUtils.Mode.ToString();
            Vector2 size = Game1.font.MeasureString(buildName);
            sb.DrawString(Game1.font, buildName, new Vector2(sb.GraphicsDevice.Viewport.Width - size.X - 10, sb.GraphicsDevice.Viewport.Height - size.Y - 10), Color.White);
            sb.End();
        }

        public override void Update(InputState inputState)
        {
            _movment.Update(_gameEngine.Camera, null, null, _gameEngine, inputState);
            _zoom.Update(_gameEngine.Camera, null, null, _gameEngine, inputState);
            _gameEngine.Update(inputState);
            _gui.Update(inputState);
            
            //var faction = _gameEngine.GetSoleFaction();
            //if(faction == FactionType.Empire)
            //    AddShips(FactionType.Federation); ;
            //if (faction == FactionType.Federation)
            //    AddShips(FactionType.Empire); 
            //if (_gameEngine._collideAllParticles.Count == 0)
            //{
            //    AddShips();
            //}
            
        }

        private void InitManMenu()
        {
            _logo = Sprite.Get("newlogo");
            _background = new Background(FMath.Rand.Next(8), isRandom:true);
            _gui = new GuiManager();
            var gc = new GuiControl();
            gc.Sprite = null;
            _gui.Root = gc;

            if (!DebugUtils.DebugMenu)
            {
                if (PersistenceManager.Inst.GetAllSavesMetadata().Count == 0)
                    _menuData = MenuData.LoadSettings("MainMenuR.xml");
                else
                    _menuData = MenuData.LoadSettings("MainMenuContinueR.xml");
            }
            else
            {    
                   _menuData = MenuData.LoadSettings("MainMenuContinue.xml");
            }


            _menuData.ShowLogo = true;
            var menuControl = _menuData.MakeGui(_gui, false, null,  Color.White);
            menuControl.Position = new Vector2(ActivityManager.ScreenSize.X / 2, ActivityManager.ScreenSize.Y/2);// - menuControl.HalfSize.Y);
            if (_menuData.ShowLogo)
                menuControl.Position = new Vector2(menuControl.Position.X, menuControl.Position.Y * 1.2f);
            _gui.Root.AddChild(menuControl);

            
            var layout = new HorizontalLayout(Vector2.Zero);            
            Vector2 size = Vector2.One * 64;
            //var twitterControl = new ImageControl(Sprite.Get("twitterIcon"),Vector2.Zero, size);
            //twitterControl.TooltipText = "Twitter";
            //twitterControl.CursorOn += _gui.ToolTipHandler;
            //twitterControl.Action += (source, cursorInfo) => Process.Start("https://twitter.com/starsingularity");
            //layout.AddChild(twitterControl);

            var storeControl = new ImageControl(Sprite.Get("DiscordIcon"), Vector2.Zero, size);
            storeControl.TooltipText = "Go to Discord";
            storeControl.CursorOn += _gui.ToolTipHandler;
            storeControl.Action += (source, cursorInfo) => Process.Start("https://discord.gg/CY6hJDx");
            layout.AddChild(storeControl);

            var communityControl = new ImageControl(Sprite.Get("community"),Vector2.Zero, size);
            communityControl.TooltipText = "Game by Kobi Nistel, Yaniv Kahana and Ron Lange\n#line{}Music: Back In The Days by Chill Carrier\n#line{}" +
            "Art by Silviu Ploisteanu\n#line{}Click to go to community page";

            communityControl.CursorOn += _gui.ToolTipHandler;
            communityControl.Action += (source, cursorInfo) => Process.Start("https://steamcommunity.com/app/808800/");
            layout.AddChild(communityControl);

            layout.RefreshSize();

            layout.Position = new Vector2(ActivityManager.ScreenSize.X - layout.HalfSize.X, layout.HalfSize.Y);

            _gui.Root.AddChild(layout);            
        }

        private void InitGame()
        {
            _movment = new ManualMovement();
            _zoom = new ManualZoom();
            _zoom.TargetZoom = 0.35f;            
            Camera camera = new Camera();
            camera.Zoom = _zoom.TargetZoom;
            
            _gameEngine = new GameEngine(camera);
            _gameEngine.AddGameObject("Sun", FactionType.Neutral, Vector2.One * -500000);
            _gameEngine.PermanentLights.AddRange(_gameEngine.AddList);
            _gameEngine.AddList.Clear();

            _gameEngine.Level = FMath.Rand.Next(1, 6);
            _gameEngine.SoundEngine.maxRange = 10;
            _gameEngine.AddObjectRandomlyInCircle("Asteroid1", 40, 7000, 5);
            AddShips();
            _gameEngine.Update(InputState.EmptyState);
            // Chance of blob
            // TODO: check if blob already exists
            if (FMath.Rand.NextFloat() < _chanceOfBlob)
            {
                var pos = FMath.ToCartesian(3000, FMath.Rand.NextFloat() * MathHelper.TwoPi);
                ContentBank.Inst.GetEmitter("Blob1").Emit(_gameEngine, null, FactionType.Vile, pos, Vector2.Zero, 0);
            }
            if (FMath.Rand.NextFloat() < _chanceOfBlob)
            {
                var pos = FMath.ToCartesian(3000, FMath.Rand.NextFloat() * MathHelper.TwoPi);
                ContentBank.Inst.GetEmitter("Worm1").Emit(_gameEngine, null, FactionType.Vile, pos, Vector2.Zero, 0);
                ContentBank.Inst.GetEmitter("Worm1").Emit(_gameEngine, null, FactionType.Vile, pos, Vector2.Zero, 180);
            }
        }

        private void AddShips(FactionType faction = FactionType.None)
        {
            DummyObject dummy = new DummyObject();
            Random rand = FMath.Rand;
            // Regular ships
           // var loadouts = new List<string>() { "Empire1_Gen", "Empire2_Gen", "Empire3_Gen", "Empire4_Gen", "Federation1_Gen", "Federation2_Gen" };
            var loadouts = new List<string>() { "Empire1", "Empire2", "Empire3", "Empire4", "Federation1", "Federation2" };
            int n = 3;
            for (int i = 0; i < n; i++)
            {
                GameObject go = null;
                Vector2 pos = FMath.GetFormationPosition(i) * 300 - 5000 * Vector2.UnitX;
                if (faction == FactionType.Federation || faction == FactionType.None)
                {
                    go = ContentBank.Inst.GetEmitter(loadouts[rand.Next(loadouts.Count)]).Emit(_gameEngine, null, FactionType.Federation, pos, Vector2.Zero, 0);
                    go?.SetTarget(dummy, TargetType.Goal);
                }
                pos = FMath.GetFormationPosition(i, MathHelper.Pi) * 300 + 5000 * Vector2.UnitX;
                if (faction == FactionType.Empire || faction == FactionType.None)
                {
                    go = ContentBank.Inst.GetEmitter(loadouts[rand.Next(loadouts.Count)]).Emit(_gameEngine, null, FactionType.Empire, pos, Vector2.Zero, MathHelper.Pi);
                    go?.SetTarget(dummy, TargetType.Goal);
                }
            }

            
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new MainMenu();
        }

    }
}
