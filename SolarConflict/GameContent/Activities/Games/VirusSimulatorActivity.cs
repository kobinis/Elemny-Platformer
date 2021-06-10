using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Games
{
    class VirusSimulatorActivity : Activity
    {



        public VirusSimulatorActivity()
        {

        }

        public override void Update(InputState inputState)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new VirusSimulatorActivity();
        }
    }
}
