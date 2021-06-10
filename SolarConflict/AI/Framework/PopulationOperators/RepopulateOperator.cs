using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework.PopulationOperators
{
    class RepopulateOperator : IPopulationOperator
    {
        public int PopulationSize { get; set; }
        public float MutationRatio { get; set; }
        //public float OldPopulationRate { get; set; } // =1
        private float _mutationFactor = 2;

        public RepopulateOperator(int populationSize = 200, float mutationRatio = 0.5f)
        {
            PopulationSize = populationSize;
            MutationRatio = mutationRatio;
        }
        
        public List<ISolution> Operate(List<ISolution> population, SolverCluster cluster)
        {
            List<ISolution> newPopulation = new List<ISolution>(population.Count);
            for (int i = 0; i < population.Count; i++)
            {
                newPopulation.Add(population[i].GetClone());
            }
            
            int crossNum = Math.Min((int)((PopulationSize- population.Count) * (1f-MutationRatio) + 0.5f), PopulationSize - population.Count);            
            int corssCounter = 0;
            int crossFirstIndex = 0;
            while(corssCounter < crossNum)
            {
                int crossMaxNum = Math.Max( (int)(crossNum / Math.Pow(2, crossFirstIndex + 1)),1);
                int crossSecoundIndex = 0;
                while (corssCounter < crossNum && crossSecoundIndex < crossMaxNum)
                {
                    if (crossFirstIndex != crossSecoundIndex)
                    {
                        newPopulation.Add(population[crossFirstIndex % population.Count].Cross(cluster.Random, population[crossSecoundIndex % population.Count]));
                        corssCounter++;
                        crossSecoundIndex++;
                    }
                    crossFirstIndex++;
                }
                
            }
            int mutationNum = PopulationSize - corssCounter;
            for (int i = 0; i < mutationNum; i++) //TODO: change it to be proportional to the fitness score
            {
                newPopulation.Add(population[i % population.Count].Mutate(cluster.Random, _mutationFactor  )); //TODO: change
            }
            return newPopulation;
        }
    }
}
