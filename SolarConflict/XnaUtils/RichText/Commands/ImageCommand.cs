using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict;
using XnaUtils.Graphics;
using System.Diagnostics;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    /// <remarks>Usage #image{color,sprites} or #image{sprites}, where color is a triple of ints in [0, 255] and sprites is the bar-separated ids of one or more sprites,
    /// for example #image{255,100,0,shield} or #image{shield|level4}
    /// 
    /// If given multiple images, fits them all to the width of the first image (and the current height)</remarks>
    [Serializable]
    internal class ImageCommand : ITextElement
    {
        public const string Command = "image";
        //Spaceing
        Color? _color;
        Sprite[] _sprites;
        //private int? _width;
        //private int? _height;
        
        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            color = (color ?? _color) ?? Color.White;
            if((_sprites?.Count() ?? 0) > 0)
            {
                float height = Math.Max(parser.LineHeight, parser.CurrentHeight);
                if (height == 0)
                {
                    var defaultCharacter = parser.CurrentFont.DefaultCharacter.HasValue ? parser.CurrentFont.DefaultCharacter.Value : ' ';
                    height = parser.CurrentFont.MeasureString(defaultCharacter.ToString()).Y; // default height                                
                }
                                                    
                var rectangle = new Rectangle((int)(parser.CurrentPosition.X + position.X), (int)(parser.CurrentPosition.Y + position.Y), _sprites.First().Width, (int)height);                    
                var size = Sprite.DrawIcon(spriteBatch, rectangle, color.Value, false, 1f, _sprites);
                
                parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
                parser.CurrentPosition += new Vector2(size.X, 0);
            }
        }

        /// <remarks>The current size, which is based on previously-parsed stuff. If nothing parsed or value is otherwise zero, defaults to a value
        /// based on the size of the current font's default character.</remarks>        
        public Vector2 GetSize(RichTextParser parser)
        {            
            
            var height = Math.Max(parser.LineHeight, parser.CurrentHeight);
            if (height == 0) {
                var defaultCharacter = parser.CurrentFont.DefaultCharacter.HasValue ? parser.CurrentFont.DefaultCharacter.Value : ' ';
                height = parser.CurrentFont.MeasureString(defaultCharacter.ToString()).Y; // default height                                
            }

            Vector2 size = FMath.FitSize(new Vector2(_sprites.First().Width, _sprites.First().Height), new Vector2(_sprites.First().Width, height));            
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size;
        }

        public void ParseParameters(string parameters)
        {
            var split = parameters.Split(',').ToArray();


            if (split.Length > 1) {
                // Comma-separated elements, the first three must describe the color (and the last the images)
                Debug.Assert(split.Length == 4, $"Invalid image tag parameters: {parameters}");

                _color = new Color(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            }
            _sprites = split.Last().Split('|').Select(s => Sprite.Get(s)).ToArray();
        }
    }
}
