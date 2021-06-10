using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework.PopulationOperators
{
    /// <summary>
    /// Selects the top N% of population
    /// </summary>
    public class RankSelectionOperator :  IPopulationOperator
    {
        public float SelectionFactor { get; set; }
        public RankSelectionOperator(float selectionFactor = 0.3f)
        {
            SelectionFactor = selectionFactor;
        }

        public List<ISolution> Operate(List<ISolution> population, SolverCluster cluster)
        {
            List<ISolution> newPopulation = new List<ISolution>();
            int numberOfElements = Math.Min( (int)(population.Count * SelectionFactor + 0.5f), population.Count);
            for (int i = 0; i < numberOfElements; i++)
            {
                newPopulation.Add(population[i]);
            }
            return newPopulation;
        }
    }
}
