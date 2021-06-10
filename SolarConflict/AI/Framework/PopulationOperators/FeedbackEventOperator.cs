using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework.PopulationOperators
{
    public delegate void GetBestSloution(ISolution  solution);
    class FeedbackEventOperator : IPopulationOperator
    {
        public event GetBestSloution BestSloutionEvent;
        public List<ISolution> Operate(List<ISolution> population, SolverCluster cluster)
        {
            var solution = population.Maximal(p => p.Fitness).GetClone() as ISolution;
            BestSloutionEvent?.Invoke(solution);
            return population;
        }
    }
}
