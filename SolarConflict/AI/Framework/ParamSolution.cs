using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework
{

    class ParamSolution : ISolution
    {
        public class MetaParams
        {
            public float Delta { get; set; }
            public int InitRangeFactor = 2;

            public float GetDelta(int index)
            {
                return Delta;
            }

            public float GetInitRange(int index)
            {
                return GetDelta(index) * InitRangeFactor;
            }
        }

        public IParamSolution Solution { get; set; }
        public MetaParams EvolutionParams { get; set; }

        public float Fitness { get; set; }

        public ISolution GetClone()
        {
            return Solution.Clone();
        }

        public ISolution Cross(Random random, ISolution solution)
        {
            var clone = this.GetClone() as ParamSolution;
            var parametersA = Solution.GetParams();
            var parametersB = (solution as IParamSolution).GetParams();
            for (int i = 0; i < parametersA.Length; i++)
            {
                if(random.Next(2) == 1)
                {
                    parametersA[i] = parametersB[i];
                }
            }
            clone.Solution.SetParams(parametersA);
            return clone;
        }

        public ISolution Mutate(Random random, float factor)
        {
            var clone = this.GetClone() as ParamSolution;
            var parameters = Solution.GetParams();
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] +=  (float)(random.Next(3) - 1) * EvolutionParams.GetDelta(i) * factor; //Add clamp
            }
            clone.Solution.SetParams(parameters);
            return clone;
        }

        public void Randomize(Random random, float factor, int range)
        {
            var clone = this.GetClone() as ParamSolution;
            var parameters = Solution.GetParams();
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = (float)(random.Next(range*2+1) - range) * EvolutionParams.GetInitRange(i) * factor; //Add clamp
            }            
            Solution.SetParams(parameters);
        }
    }
}
