using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SolarConflict.XnaUtils.Input;

namespace SolarConflict.GameContent.Activities
{

    /// <summary>Draws all our text, fishes for errors</summary>
    class TestActivity : Activity
    {

        public TestActivity()
        {

        }

        bool _done;

        GKeys key;


        public override void Draw(SpriteBatch batch)
        {
            batch.Begin();

            var text = key.ToString();
            batch.DrawString(Game1.font, text, Vector2.One * 500, Color.White);
            batch.End();

            _done = true;
        }

        void DrawText(SpriteBatch batch, RichTextParser parser, string text)
        {
            // Draw with parser
            //Game1.font.
            text = key.ToString();
            batch.DrawString(Game1.font, text, Vector2.One * 500, Color.White);
        }

        public override void Update(InputState inputState)
        {

            var newKey = inputState.GetPressedKey();
            if (newKey.Key != Keys.None)
                key = newKey;
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new TestKeyPress();
        }
    }
}
