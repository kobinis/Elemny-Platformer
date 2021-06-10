using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework
{
    interface IParamSolution
    {
        float[] GetParams();
        void SetParams(float[] parameters);
        ISolution Clone();
    }
}
