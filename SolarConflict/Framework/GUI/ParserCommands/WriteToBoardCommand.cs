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

namespace SolarConflict
{
    [Serializable]
    class WriteToBoardCommand : ITextElement
    {
        public const string Command = "write";

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {            
        }

        /// <remarks>The current size, which is based on previously-parsed stuff. If nothing parsed or value is otherwise zero, defaults to a value
        /// based on the size of the current font's default character.</remarks>        
        public Vector2 GetSize(RichTextParser parser)
        {
            return Vector2.Zero;
        }

        public void ParseParameters(string parameters) //TODO: add try c
        {
            var keyValue = parameters.Split(':');
            if (keyValue.Count() > 1)
                MetaWorld.Inst.Blackboard[keyValue[0]] = keyValue[1];
            else
            {
                if (keyValue.Count() > 0)
                    MetaWorld.Inst.Blackboard[keyValue[0]] = string.Empty;
            }

        }
    }
}
