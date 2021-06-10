using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaUtils.Input
{
    

    public class InputFacade
    {
        InputState inputState;
        

        public void Update(GameTime gameTime)
        {
            inputState.Update(gameTime, null);
        }

        public void Draw()
        {

        }
      
        public InputState GetInputState()
        {
            return inputState;
        }

        public InputState GetEmptyInputState()
        {
            return InputState.EmptyState;
        }
        
    }
}
