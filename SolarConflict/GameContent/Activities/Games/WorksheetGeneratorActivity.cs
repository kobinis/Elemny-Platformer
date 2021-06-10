using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Games
{
    public class WorkshhetProfile
    {
        int min1 = 2;
        int min2 = 3;
        int max1 = 50;
        int max2 = 50;

        
    }

    public class Exercise
    {
        public int Term1;
        public int Term2;

    }

    class WorksheetGeneratorActivity: Activity
    {

        public override void Update(InputState inputState)
        {
            
        }

        public override void Draw(SpriteBatch sb)
        {
           
        }



        public static Activity ActivityProvider(string parameters)
        {
            return new WorksheetGeneratorActivity();
        }


    }
}
