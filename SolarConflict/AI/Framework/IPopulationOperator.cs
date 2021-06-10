using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework
{
    public interface IPopulationOperator
    {
       List<ISolution> Operate(List<ISolution> population, SolverCluster cluster);
    }
}
