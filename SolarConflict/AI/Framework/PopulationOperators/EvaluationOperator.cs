using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework.PopulationOperators
{
    /// <summary>
    /// Evaluates the fitness of the population
    /// </summary>
    public class EvaluationOperator : IPopulationOperator
    {
        public EvaluationOperator(IEvaluation evaluation, bool reEvaluateSolutions = false)
        {
            Evaluation = evaluation;
            ReEvaluateSolutions = reEvaluateSolutions;
        }

        public bool ReEvaluateSolutions = true;
        public IEvaluation Evaluation { get; set; }
        public List<ISolution> Operate(List<ISolution> population, SolverCluster cluster)
        {
            List<ISolution> newPopulation = new List<ISolution>();
            if (ReEvaluateSolutions)
            {
                foreach (var item in population)
                {
                    item.Fitness = Evaluation.Evaluate(item, cluster);
                    newPopulation.Add(item);
                }
            }
            else
            {
                foreach (var item in population)
                {
                    if (item.Fitness > float.MinValue)
                    {
                        item.Fitness = Evaluation.Evaluate(item,cluster);
                    }
                    newPopulation.Add(item);
                }
            }
            return newPopulation;
        }
    }
}
