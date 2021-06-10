using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework.PopulationOperators
{
    public class SortOperator : IPopulationOperator
    {                
        public List<ISolution> Operate(List<ISolution> population, SolverCluster cluster)
        {
            float[] fitnessArray = new float[population.Count];
            ISolution[] newPopulation = new ISolution[population.Count];
            for (int i = 0; i < population.Count; i++)
            {
                fitnessArray[i] = -population[i].Fitness;
                newPopulation[i] = population[i];
            }                        
            Array.Sort(fitnessArray, newPopulation);
            //Array.Reverse(newPopulation); //TODO: only if maxsimaize
            return newPopulation.ToList();
        }
    }
}
