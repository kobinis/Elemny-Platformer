using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.GameContent.Activities.Tests
{
    class GridActivity : Activity
    {
        private int sizeX = 55;
        private int sizeY = 25;
        private float size = 100;
        private float[,] heightGrid;
        private Vector2[,] positions;
        private SpriteBatch sb;
        private Texture2D pixelTexture;
        private SetPixel pixelMethod;
        private float time;

        public GridActivity()
        {
            pixelMethod = new SetPixel(PutPixel);
            heightGrid = new float[sizeX, sizeY];
            positions = new Vector2[sizeX, sizeY];
            pixelTexture = TextureBank.Inst.GetTexture("pixel");
            sb = Game1.sb;
        }

        protected override void Init(ActivityParameters parameters)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {                    
                    positions[x, y] = new Vector2(x-5, y-5) * size;
                }
            }
        }

        public override void Update(InputState inputState)
        {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
            time += 1;
            UpdateFunction(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            ActivityManager.GraphicsDevice.Clear(Color.Black);
            Color color = new Color(200, 250, 250);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            for (int y = 0; y < sizeY-2; y++)
            {
                for (int x = 0; x < sizeX-2; x++)
                {
                    Vector2 point1 = positions[x, y];
                    Vector2 point2 = positions[x, y + 1];
                    Vector2 point3 = positions[x + 1, y];                    
                    Vector2 point4 = positions[x + 1, y];
                    GraphicsUtils.Line(sb, point1, point2, color, 16f, pixelMethod);
                    GraphicsUtils.Line(sb, point1, point3, color, 16f, pixelMethod);                    
                  //  sb.Draw(pixelTexture, point1, Color.LightYellow);
                }
            }
            sb.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int y = 0; y < sizeY - 2; y++)
            {
                for (int x = 0; x < sizeX - 2; x++)
                {
                    Vector2 point1 = positions[x, y];
                    //sb.Draw(pixelTexture, point1, Color.Black);
                }
            }
            sb.End();
        }

        public static Activity ActivityProvider(string parameters = "")
        {
            return new GridActivity();
        }

        private void UpdateFunction(InputState inputState)
        {

            Vector2 center = inputState.Cursor.Position;
            //new Vector2(sizeX / 2f, sizeY / 2f);
            for (int y = 1; y < sizeY - 1; y++)
            {
                for (int x = 1; x < sizeX - 1; x++)
                {
                    float diff = (new Vector2(x - 5, y - 5) * size - center).Length();
                    heightGrid[x, y] = Math.Max(500 - diff, 0) * 0.5f;
                }
            }


            for (int y = 1; y < sizeY-1; y++)
            {
                for (int x = 1; x < sizeX-1; x++)
                {
                    
                    float dx = heightGrid[x + 1, y] - heightGrid[x - 1, y];
                    float dy = heightGrid[x, y +1] - heightGrid[x, y - 1];

                    positions[x, y] = new Vector2(x-5, y-5) * size - new Vector2(dx,dy);
                }
            }
        }

        public void PutPixel(SpriteBatch sb, Vector2 point, Color color)
        {
            sb.Draw(pixelTexture, point, color);
        }


    }
}
