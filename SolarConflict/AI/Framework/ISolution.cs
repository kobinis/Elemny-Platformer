using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework
{
    public interface ISolution    
    {
        float Fitness { get; set; }
        ISolution Mutate(Random random, float factor);
        ISolution Cross(Random random, ISolution solution);
        //void Randomize(Random random, float factor, int range);
        ISolution GetClone();
    }
}
