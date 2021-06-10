using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Redux.Utilities.Managers;
using System;
using System.Runtime.Serialization;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.XnaUtils.SimpleGui.Controllers
{
    /// <summary>
    /// Editable text
    /// </summary>
    public class TextBoxControl : GuiControl
    {
        
        private Texture2D _cursorTexture;

        protected string _text;
        public bool IsAutoResize;
        private int _spacing = 5;
        public Color TextColor { get; set; }

        //change to IFont
        [NonSerialized]
        protected SpriteFont Font; //TODO: Change
        public bool IsShowFrame { get; set; }
        public virtual bool IsFocus { get; set; }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (IsAutoResize)
                    UpdateSize();
            }
        }

        public TextBoxControl(string text, SpriteFont font = null) : base(Vector2.Zero, Vector2.One)
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
            if (text != null)
                size = font.MeasureString(text);
            Width = size.X + _spacing * 2;
            Height = size.Y + _spacing * 2;

            IsShowFrame = false;
            IsAutoResize = false;
            IsFocus = true;
            DontInvoke = false;
            ActivationKey = Keys.Enter;
            IsFocus = true;
            
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

        public override void UpdateLogic(InputState inputState)
        {
           
            if(inputState.Cursor.IsPressedLeft | inputState.Cursor.IsPressed)
            {
                IsFocus = inputState.Cursor.ActiveGuiControl == this;
            }
            IsFocus = true;

            if (IsFocus)
            {
                if (inputState.keyboardManager.IsBackspaceActive && Text.Length > 0)
                {
                    Text = Text.Remove(Text.Length - 1);
                }
                else
                {
                    string deltaText = inputState.TextBuffer;
                    Text += deltaText;
                }

                if (inputState.IsKeyPressed(Keys.Escape))
                    IsFocus = false;
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
                Vector2 size = Font.MeasureString(_text+".");
                string text = _text;
                if (IsFocus && ActivityManager.Inst.GameTime.TotalGameTime.Milliseconds / 250 % 2 == 0)
                    text += ".";
                sb.DrawString(Font, text, Position - size * 0.5f, drawColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext context)
        {
            Font = FontBank.DefaultFont;
        }
    }
}
