using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework
{
    public interface IEvaluation    
    {
        float Evaluate(ISolution trainable, SolverCluster cluster);
    }
}
