using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Activities
{
    class TestPerformance : Activity
    {
        
        private GameEngine _gameEngine;
        private Camera _camera;

        private int _iterationCounter;
        private int _testIterationNumber = 1000;
        private long _testTimeInTicks;

        private Stopwatch _stopwatch;
        private Stopwatch _drawStopwatch;
        private string _text;
        Background background;
        CameraManager cm;

        public TestPerformance()
        {
            _camera = new Camera();
            _camera.Zoom = 0.3f;
            _gameEngine = new GameEngine(_camera, seed: 1543);
            _iterationCounter = 0;
            _testTimeInTicks = 0;
            _stopwatch = new Stopwatch();
            _drawStopwatch = new Stopwatch();
            background = new Background(1);
            cm = new CameraManager();
            cm.MovmentType = CameraMovmentType.Manual;
            cm.ZoomType = CameraZoomType.Manual;
            AddGameObjects();
        }

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
        }


        public override void OnEnter(ActivityParameters parameters)
        {
           
        }

        public override void Update(InputState inputState)
        {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
            //if(_gameEngine.PotentialTargets.Count > 0)
            //{
            //    _camera.Position = _gameEngine.PotentialTargets[0].Position;
            //}
            if(_iterationCounter == 0)
            {
                //DebugUtils.StartWatch("Test0");
            }
            if (_iterationCounter < _testIterationNumber)
            {
                if (_iterationCounter < 100)
                    _gameEngine.Update(inputState);
                else
                {

                    _stopwatch.Start();
                    _gameEngine.Update(InputState.EmptyState);
                    _stopwatch.Stop();
                }
            }
            else
            {
                cm.Update(_gameEngine.Camera, _gameEngine, inputState);
                _gameEngine.Update(inputState);
               
            }
            _iterationCounter++;
            if(_iterationCounter == _testIterationNumber)
            {

                _testTimeInTicks = _stopwatch.ElapsedMilliseconds;
                _text = " Net: " + _testTimeInTicks.ToString();
            }
          
            
        }

        public override void Draw(SpriteBatch sb)
        {
            //if (GraphicsSettings.IsPostprocessing)
            //{                
            //    ActivityManager.GraphicsDevice.SetRenderTarget(ActivityManager.renderTargetFullA);
            //}
            background.Draw(_gameEngine.Camera);
            _gameEngine.Draw(sb);
            string text = _iterationCounter.ToString();
            if (_iterationCounter > _testIterationNumber)
                text = _text;
            sb.Begin();
            sb.DrawString(Game1.font, text, Vector2.One * 80, Color.White);
            sb.End();
        }

        private void AddGameObjects()
        {
            int numberOfShips = 200; //20
            //string[] ships = new string[] { "Vespa", "Myrmidon", "Lancer1", "Enlil" , "Chickenhawk" }; //, "Mandate"
            string[] ships = new string[] { "PrologShip1" };
            for (int i = 0; i < numberOfShips; i++)
            {
                int space = 1000;
                FactionType faction = (FactionType)((i/600 % 2) + 1);
                Vector2 pos = new Vector2((i % 6) * space, (i / 6) * space);
                string shipId = ships[i/2 % ships.Length];
                var ship = ContentBank.Inst.GetGameObjectFactory(shipId).MakeGameObject(_gameEngine, null, faction, pos, Vector2.Zero, 0);
                _gameEngine.AddGameObject(ship);
            }

            Random rand = new Random(312412);
            for (int i = 0; i < 10000; i++)
            {
                Vector2 pos = new Vector2(rand.Next(-100000, 100000), rand.Next(-100000, 100000));
                ContentBank.Inst.GetEmitter("Asteroid1").Emit(_gameEngine, null, i %2 == 0 ? FactionType.Empire : FactionType.Federation, pos, Vector2.Zero, 0);
            }

        }

        public static Activity ActivityProvider(string parameters = "")
        {
            return new TestPerformance();
        }

    }
}
