using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict;
using SolarConflict.XnaUtils;
using System.Runtime.Serialization;
using XnaUtils.Graphics;

namespace XnaUtils.SimpleGui.Controllers
{
    [Serializable]
    public class TextControl : GuiControl //needs to derived from abstract class or interface IGuiControl
    {
        protected string _text;
        public bool IsAutoResize;
        private int _spacing = 5;
        public Color TextColor { get; set; }


        //change to IFont
        [NonSerialized]
        protected SpriteFont Font; //TODO: Change
        public bool IsShowFrame { get; set; } 

        public TextControl(string text, SpriteFont font = null) 
            :base(Vector2.Zero, Vector2.One)
        {
            if (font == null)
            {
                font = FontBank.DefaultFont; // TODO: use default font from fonts bank
            }
            
            Font = font;            
            _text = text;            

            TextColor = Color.LightBlue;
            PressedControlColor = Color.Blue;
            CursorOverColor = Color.Yellow;
            Vector2 size = Vector2.Zero;
            try
            {
                if (text != null)
                    size = font.MeasureString(text);
            }
            catch (Exception)
            {

                size = Vector2.One * 10;
            }
           
            Width = size.X + _spacing * 2;
            Height = size.Y + _spacing * 2;

            IsShowFrame = false;
            IsAutoResize = true;
            Sprite = TextureBank.Inst.GetSprite("Small_ui");
        }

        private void UpdateSize()
        {
            if (_text != null)
            {
                Vector2 size = Font.MeasureString(_text);
                Width = size.X + _spacing * 2;
                Height = size.Y + _spacing * 2;
            }
        }

        public string Text
        {
            get { return _text; }
            set { 
                    _text = value;
                    if(IsAutoResize)
                        UpdateSize();
                }
        }


        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {            
            if (IsShowFrame)
                base.DrawLogic(sb, color);

            if (_text != null)
            {
                Color drawColor = TextColor;

                if (color != null)
                {
                    drawColor = new Color(TextColor.ToVector4() * color.Value.ToVector4());
                }
                Vector2 size = Font.MeasureString(_text);
                sb.DrawString(Font, _text, Position - size * 0.5f, drawColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext context)
        {
            Font = FontBank.DefaultFont;
        }

    }
}
