using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework
{
    class EvaluationGroup : IEvaluation
    {
        public List<IEvaluation> Evaluators { get; private set; }

        public EvaluationGroup()
        {
            Evaluators = new List<IEvaluation>();
        }

        public float Evaluate(ISolution trainable, SolverCluster cluster)
        {
            float res = 0;
            foreach (var item in Evaluators)
            {
                res += item.Evaluate(trainable, cluster);
            }

            return res / Evaluators.Count;
        }
    }
}
