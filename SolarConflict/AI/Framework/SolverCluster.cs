using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework
{
    public class SolverCluster
    {

        public Random Random { get { return _random; } }
        public List<IPopulationOperator> PopulationOperators { get; set; }

        private Random _random;
        private List<ISolution> _population;


        public SolverCluster(List<ISolution> population)
        {
            _random = new Random(); //TODO: can have problems with the same seed;
            PopulationOperators = new List<IPopulationOperator>();
            _population = population;
        }

        public void RunOneGeneration()
        {
            foreach (var popOperator in PopulationOperators)
            {
                _population = popOperator.Operate(_population, this);
            }            
        }
        
               
    }
}
