using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using XnaUtils.Graphics;

namespace XnaUtils.SimpleGui.Controllers
{
    [Serializable]
    public class ImageControl : GuiControl
    {
        //public bool IsShowFrame { get; set; }
       // public bool IsUpdateSize;
       // public Sprite Image { get; set; }

        public ImageControl(Sprite sprite, Vector2 position, Vector2 size) //Add Image rotation
            : base(position, size)
        {
            this.Sprite = sprite;
            this.ControlColor = Color.White;
            //IsUpdateSize = true;
        }       

        private Vector2 CalculateRealSize(Sprite sprite)
        {
            Vector2 realSize = new Vector2();

            float hw = sprite.Width * 0.5f;
            float hh = sprite.Height * 0.5f;

            if (hw / hh > halfWidth / halfHeight) //move to MyMath
            {
                realSize.X = halfWidth;
                realSize.Y = halfWidth / hw * hh;
            }
            else
            {
                realSize.Y = halfHeight;
                realSize.X = halfHeight / hh * hw;
            }

            //realSize =  new Vector2(halfWidth,halfHeight);

            return realSize;
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            Sprite sprite = Sprite;
            if(IsPressed && PressedSprite != null)
            {
                sprite = PressedSprite;
            }
            if (sprite != null)
            {
                Vector2 realSize = CalculateRealSize(sprite); //only if changed
                Color controlColor = ControlColor;
                Color shadeColor = new Color(50, 50, 50, 50);

                if (color.HasValue)
                {
                    Vector4 colorVec = color.Value.ToVector4();
                    controlColor = new Color(ControlColor.ToVector4() * colorVec);
                    shadeColor = new Color(shadeColor.ToVector4() * colorVec);
                }

                if (Disable)
                    controlColor = controlColor * GuiManager.DisableShade;

                Rectangle rectangle = new Rectangle((int)(Position.X - realSize.X), (int)(Position.Y - realSize.Y), (int)realSize.X * 2, (int)realSize.Y * 2);                
                sb.Draw(sprite, rectangle, null, controlColor, 0, Vector2.Zero, SpriteEffects.None,0);
                //if (IsUpdateSize)
                //    this.HalfSize = new Vector2(rectangle.Width/2, rectangle.Height/2);

            }
        }        
    }
}
