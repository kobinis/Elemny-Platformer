using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.GameContent.Activities.Games
{
    public class OnScreenDriffterSystem : AgentSystem
    {
        

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if(!gameEngine.Camera.IsOnScreen(agent, 1))
            {
                agent.SetMeterValue(MeterType.Energy, 0);
            }

            if (agent.Position.Y > 0)
                agent.ApplyForce(-Vector2.UnitY*20, 10);

            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }

    class NightRaid:Scene
    {
        const int WIDTH = 640;
        const int HIGHT = 400;

        private Canvas _groundCanvas;
        private Texture2D _ground;
        private GameObject _player;

        private List<GameObject> _enemys;

        public NightRaid()
        {
            GraphicsSettings.UseLighting = true;
            _groundCanvas = new Canvas(WIDTH, HIGHT/3, Game1.sb.GraphicsDevice);            
            for (int x = 0; x < _groundCanvas.Width; x++)
            {
                float amp = _groundCanvas.Height / 8;
                int yHight = (int)amp / 2;// Math.Max( (int) ((Math.Cos(x / 20f  ) + Math.Cos(x / 27f)) * 0.5f * amp + amp) ,0);
                for (int y = yHight; y < _groundCanvas.Height; y++)
                {
                    float a = y / (float)_groundCanvas.Height;
                    Color color = Color.Lerp(Color.Yellow, Color.Brown, a);

                    _groundCanvas.SetPixel(x, y, color);
                }

            }

            _groundCanvas.SetData();
            _ground = _groundCanvas.GetTexture();

            _player = AddGameObject("HouseA", new Vector2(ActivityManager.ScreenCenter.X, ActivityManager.ScreenSize.Y), 0, FactionType.Player, AgentControlType.Player);
            Camera.Zoom = 0.5f;
            CameraManager.ZoomType = CameraZoomType.Custom;
            CameraManager.MovmentType = CameraMovmentType.Custom;
            _enemys = new List<GameObject>();
        }

        int wavecount = 0;
        public override void UpdateScript(InputState inputState)
        {
            bool newWave = true;            
            foreach (var item in _enemys)
            {
                if (item.IsActive)
                    newWave = false;
            }

            if(newWave && GameEngine.FrameCounter % 100 == 0 )
            {
                _player.SetMeterValue(MeterType.Shield, 10000);
                _enemys.Clear();
                GameEngine.Level = wavecount;
                for (int i = 0; i < 2; i++)
                {
                   var agent = AddGameObject("Skill_Gen", Vector2.Zero - Vector2.UnitX * 200 * i - Vector2.UnitY * 1000, 0, FactionType.Pirates1) as Agent;
                    agent.AddSystem(new OnScreenDriffterSystem());
                    _enemys.Add(agent);

                }
                wavecount++;
            }
        }


        public override void Draw(SpriteBatch sb)
        {
            Vector2 screenSize = ActivityManager.ScreenSize;
            float scale = screenSize.X / WIDTH;
            Point size = new Point((int)screenSize.X, (int)Math.Round(_ground.Height * scale));
            Rectangle groundRect = new Rectangle(0, (int)(screenSize.Y - _ground.Height ), size.X, size.Y);
            _player.Position = Camera.GetWorldPos( new Vector2(screenSize.X / 2, groundRect.Y));
            base.Draw(sb);

            //sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullClockwise);
            sb.Begin();
            sb.Draw(_ground, groundRect, Color.White);
            sb.End();
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new NightRaid();
        }
    }
}
