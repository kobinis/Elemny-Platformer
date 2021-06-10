using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict;
using SolarConflict.XnaUtils;
using System.Diagnostics;
using XnaUtils.Graphics;

namespace XnaUtils.SimpleGui.Controllers
{

    /// <summary>Describes a drop shadow, for text</summary>
    [Serializable]
    public class TextShadow {
        public Color Color;

        
        public int Offset;

        public TextShadow(int offset = 1, Color? color = null) {
            color = color ?? Color.Black;
            Offset = offset;
            Color = color.Value;
        }
    }

    [Serializable]
    public class RichTextControl : GuiControl //needs to derived from abstract class or interface IGuiControl
    {
        public TextShadow Shadow;

        private int _spacing = 10;
        public Color TextColor
        {
            get { return _parser.DefaultColor; }
            set { _parser.DefaultColor = value; }
        }
        public SpriteFont Font
        {
            get { return _parser.DefaultFont; }
            set { _parser.DefaultFont = value; }
        }

        public RichTextParser Parser
        {
            get { return _parser; }
        }

        public Color? TextHighlightColor { get; set; }

        public string Text
        {
            get { return _parser.Text; }
            set {
                _parser.Text = value;
                UpdateSize();
            }
        }
        public float MaxLineWidth {
            get {
                return _parser.MaxLineWidth;
            }
            set {
                _parser.MaxLineWidth = value;
                UpdateSize();
            }
        }

        public bool IsShowFrame { get; set; }
        RichTextParser _parser;

        public RichTextControl(string text, SpriteFont font = null, bool isShowFrame = false)
            : base(Vector2.Zero, Vector2.One)
        {
            if (font == null)
            {
                font = FontBank.DefaultFont;
            }
            this.IsShowFrame = isShowFrame;
            _parser = new RichTextParser(font);                        
            TextColor = Color.LightBlue;
            
          //  PressedControlColor = Color.Blue;
          // CursorOverColor = Color.Yellow;
            Text = text;

            Vector2 size = _parser.MeasureText();
            Width = size.X + _spacing * 2;
            Height = size.Y + _spacing * 2;
            Sprite = TextureBank.Inst.GetSprite("Small_ui");
        }

        private void UpdateSize() {
            if (Text == null)
                return;

            var size = _parser.MeasureText();
            
            Width = size.X + _spacing * 2;
            Height = size.Y + _spacing * 2;
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null) {
            
            if (IsShowFrame)
                base.DrawLogic(sb, color);

            if (Text == null)
                return;

            var size = _parser.MeasureText();

            Color backcolor = _parser.DefaultColor;
            if (TextHighlightColor.HasValue && this.IsCursorOn)
                _parser.DefaultColor = TextHighlightColor.Value;

            // Shadows
            if (Shadow != null) {
                // TEMP disregard color arg, override with shadow color field
                _parser.Draw(sb, Position - size * 0.5f - Vector2.UnitX * Shadow.Offset, Shadow.Color);
                _parser.Draw(sb, Position - size * 0.5f + Vector2.UnitX * Shadow.Offset, Shadow.Color);
                _parser.Draw(sb, Position - size * 0.5f - Vector2.UnitY * Shadow.Offset, Shadow.Color);
                _parser.Draw(sb, Position - size * 0.5f + Vector2.UnitY * Shadow.Offset, Shadow.Color);

                //if (Shadow.Mode == TextShadow.ShadowMode.Above || Shadow.Mode == TextShadow.ShadowMode.Both)
                //    _parser.Draw(sb, Position - size * 0.5f - new Vector2(Shadow.Offset, Shadow.Offset), Shadow.Color);
                //if (Shadow.Mode == TextShadow.ShadowMode.Below || Shadow.Mode == TextShadow.ShadowMode.Both)
                //    _parser.Draw(sb, Position - size * 0.5f + new Vector2(Shadow.Offset, Shadow.Offset), Shadow.Color);

            }

            // Text            
            _parser.Draw(sb, Position - size * 0.5f, color);
            _parser.DefaultColor = backcolor;
        }

    }
}
