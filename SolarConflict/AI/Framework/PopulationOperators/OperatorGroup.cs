using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework.PopulationOperators
{
    class OperatorGroup : IPopulationOperator
    {
        List<IPopulationOperator> _operators;

        public OperatorGroup()
        {
            _operators = new List<IPopulationOperator>();
        }

        public void AddOperator(IPopulationOperator populationOperator)
        {
            _operators.Add(populationOperator);
        }

        public void Clear()
        {
            _operators.Clear();
        }

        public List<ISolution> Operate(List<ISolution> population, SolverCluster cluster)
        {
            List<ISolution> newPopulation = population;
            foreach (var popOperator in _operators)
            {
                newPopulation = popOperator.Operate(newPopulation, cluster);
            }
            return newPopulation;
        }
    }
}
