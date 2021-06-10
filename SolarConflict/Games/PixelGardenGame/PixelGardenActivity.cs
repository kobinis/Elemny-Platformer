using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.GameContent.Activities.Games;
using SolarConflict.Games.PixelGardenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Games
{
    class PixelGardenActivity : Activity
    {
        PixelGardenEngine gardenEngine;
        PlayerAgent player;

        public PixelGardenActivity()
        {
            gardenEngine = new PixelGardenEngine();
            player = new PlayerAgent();

            player = new PlayerAgent();
            player.Position = new Vector2(200, 10);
            gardenEngine.gameEngine.AddGameObject(player);
        }

        public override void Update(InputState inputState)
        {
            gardenEngine.Update(inputState);
            //player.Update();
            gardenEngine.offsetX = (int)player.Position.X - gardenEngine.logic.sx / 2;
            gardenEngine.offsetY = (int)player.Position.Y - gardenEngine.logic.sy / 2;

            gardenEngine.camera.Position = player.Position;

            if(inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                GameObject shot = gardenEngine.gameEngine.AddGameObject("Shot1", FactionType.Player, player.Position, 0);
                shot.Velocity = -Vector2.UnitY * 5;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            gardenEngine.Draw(sb); ;
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new PixelGardenActivity();
        }
    }
}
