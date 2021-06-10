using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;

namespace SolarConflict.GameContent.Activities
{
    //Runs another activity with parameters from a file
    class ParamsFromFile:Activity
    {
        public ParamsFromFile()
        {
            IsPopup = true;
            
        }

        public static Activity ActivityProvider(string param)
        {
            return new ParamsFromFile();
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public override void Update(InputState inputState)
        {
            throw new NotImplementedException();
        }
    }
}
