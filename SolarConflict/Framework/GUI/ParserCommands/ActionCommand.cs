using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.XnaUtils.RichText;

namespace SolarConflict {
    [Serializable]
    class ActionCommand : ITextElement {
        public const string Command = "action";

        Sprite _sprite;
        /// <remarks>Only used if _sprite is null</remarks>
        string _text;        

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color) {

            if (_sprite != null) {
                // Sprite defined, pretend we're an immage command
                color = color ?? Color.White;

                var height = Math.Max(parser.LineHeight, parser.CurrentHeight);
                if (height == 0) {
                    var defaultCharacter = parser.CurrentFont.DefaultCharacter.HasValue ? parser.CurrentFont.DefaultCharacter.Value : ' ';
                    height = parser.CurrentFont.MeasureString(defaultCharacter.ToString()).Y; // default height                                
                }

                var rectangle = new Rectangle((int)(parser.CurrentPosition.X + position.X), (int)(parser.CurrentPosition.Y + position.Y), _sprite.Width, (int)height);
                var size = Sprite.DrawIcon(spriteBatch, rectangle, color.Value, false, 1f, _sprite);

                parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
                parser.CurrentPosition += new Vector2(size.X, 0);
            } else {
                // No sprite, pretend we're a text command. A yellow one
                color = color ?? Color.Yellow;
               
                spriteBatch.DrawString(parser.CurrentFont, _text, parser.CurrentPosition + position, color.Value);
                GetSize(parser);
            }
        }        
      
        /// <remarks>The current size, which is based on previously-parsed stuff. If nothing parsed or value is otherwise zero, defaults to a value
        /// based on the size of the current font's default character.</remarks>        
        public Vector2 GetSize(RichTextParser parser) {

            if (_sprite != null) {
                // We're basically an ImageCommand
                var height = Math.Max(parser.LineHeight, parser.CurrentHeight);
                if (height == 0) {
                    var defaultCharacter = parser.CurrentFont.DefaultCharacter.HasValue ? parser.CurrentFont.DefaultCharacter.Value : ' ';
                    height = parser.CurrentFont.MeasureString(defaultCharacter.ToString()).Y; // default height                                
                }

                var size = FMath.FitSize(new Vector2(_sprite.Width, _sprite.Height), new Vector2(_sprite.Width, height));
                parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
                parser.CurrentPosition += new Vector2(size.X, 0);
                return size;
            }

            {
                // We're basically a TextCommand
                var size = parser.CurrentFont.MeasureString(_text);
                parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
                parser.CurrentPosition += new Vector2(size.X, 0);
                return size;
            }
        }

        public void ParseParameters(string parameters) {            
            // Try to parse a ControlSignal and find  corresponding keybinding and key icon
            ControlSignals signal;
            if (Enum.TryParse(parameters, out signal))
                if (KeysSettings.Data.KeyBindings.ContainsKey(signal)) {
                    var key = KeysSettings.Data.KeyBindings[signal];
                    _text = key.ToString();
                    _sprite = Sprite.Get(KeysSettings.KeyIconIDs.Get(key));
                }
                
            // If that didn't work, do the same with PlayerCommands
            if (_sprite == null) {
                PlayerCommand command;
                if (Enum.TryParse(parameters, out command))
                    if (KeysSettings.Data.CommandBindings.ContainsKey(command)) {
                        var key = KeysSettings.Data.CommandBindings[command];
                        _text = key.ToString();
                        _sprite = Sprite.Get(KeysSettings.KeyIconIDs.Get(key));
                    }
            }

            // Failing that, SceneComponents
            if (_sprite == null) {
                SceneComponentType component;
                if (Enum.TryParse(parameters, out component)) {
                    var key = SceneComponentSelector.GetKeyForComponent(component);

                    if (key.HasValue) {
                        _text = key.Value.ToString();
                        _sprite = Sprite.Get(KeysSettings.KeyIconIDs.Get(key.Value));
                    }
                }
            }

            if (_sprite == null && _text == null  && parameters == "lock")
            {
                var key = KeysSettings.Data.LockRotation;
                _text = key.ToString();
                _sprite = Sprite.Get(KeysSettings.KeyIconIDs.Get(key));
            }

            // If we didn't find a keybinding, just use the params as the text
            _text = _text ?? parameters;
        }
    }
}
