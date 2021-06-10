using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.XnaUtils.SimpleGui.Controllers
{
    public class ColorSelectorControl : GuiControl //TODO: add has color changed and a callback
    {
        private Texture2D _texture;
        private Texture2D _dot;
        public Color SelectedColor;
        private Vector2 _selectedPosition;

        public ColorSelectorControl(int size, GraphicsDevice gd):base(Vector2.Zero, Vector2.One * size)
        {            
            Canvas canvas = new Canvas(size, size,  gd);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Color color = GetColorFromPos(x, y, true);
                    canvas.SetPixel(x, y, color);
                }
            }
            canvas.SetData();
            SelectedColor = Color.White;
            _texture = canvas.GetTexture();
            _dot = Sprite.Get("smallLight").Texture;
        }

        public Color GetColorFromPos(float x, float y, bool addFrame = false)
        {
            float halfSize = HalfSize.X;
            float rad = (float)Math.Sqrt(( Math.Pow((x - halfSize) / halfSize, 2) + Math.Pow((y - halfSize) / halfSize, 2)));
            float alpha = rad <= 1 ? 1 : 0;
            if (addFrame)
                alpha = MathHelper.Clamp( (1 - rad) * 20,0,1);
            float staturation = Math.Min(rad * 1.3f, 1);
            float hue = MathHelper.ToDegrees((float)Math.Atan2(y - halfSize, x - halfSize)) + 180;
            Color color = GraphicsUtils.HsvToRgb(hue, staturation, 1);
            color.A = (byte)(alpha * 255);                        
            return color;
        }
            

        public override void UpdateLogic(InputState inputState)
        {            
            if((inputState.Cursor.OnPressLeft || inputState.Cursor.OnPressRight))
            {
                Vector2 pos = inputState.Cursor.Position - this.Position+this.HalfSize;
                Color color = GetColorFromPos(pos.X, pos.Y);
                if (color.A > 0)
                {
                    SelectedColor = color;
                    _selectedPosition = pos - HalfSize;
                }
            }
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = default(Color?))
        {
            Rectangle rect = GetRectangle();
            sb.Draw(_texture, rect, Color.White);
            sb.Draw(_dot, Position + _selectedPosition, null, SelectedColor, 0, new Vector2(_dot.Width / 2f, _dot.Height / 2f), 1, SpriteEffects.None, 0);

            if (DrawFuction != null)
            {
                DrawFuction.Invoke(this, sb, color);
        }
    }

    }
}
