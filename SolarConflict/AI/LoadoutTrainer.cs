//using SolarConflict.AI.Framework;
//using SolarConflict.AI.Framework.PopulationOperators;
//using SolarConflict.AI.GameAI;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;

//namespace SolarConflict.AI
//{
//    /// <summary>
//    /// This class takes a loadout and trains it
//    /// </summary>
//    public class LoadoutTrainer
//    {
//        int popSize = 15;
//        SolverCluster solverCluster;
//        public ParameterControl bestControl;
//        ShipEvaluation eval;
        

//        public LoadoutTrainer(String trainedShipID)
//        {
//            string trainerShipID = "MediumShip1B";//"MediumShip1A";
//            //RepopulateOperator 
//            var population = CratePopulation(popSize);
//            solverCluster = new SolverCluster(population);

//            eval = new ShipEvaluation(trainedShipID, trainerShipID, 0);
//            FeedbackEventOperator feedback = new FeedbackEventOperator();
//            feedback.BestSloutionEvent += FeedbackHandler;
//            RepopulateOperator repop = new RepopulateOperator(popSize, 0.6f);
//            GenerationOperator go = new GenerationOperator(new EvaluationOperator(eval, true), feedback, repop);
//            solverCluster.PopulationOperators.Add(go);
//        }

//        public float GetFitness()
//        {
//            if (bestControl == null)
//                return float.MinValue;
//            return bestControl.Fitness;
//        }

//        public void TrainOneGeneration()
//        {
//            solverCluster.RunOneGeneration();
//        }

//        public void FeedbackHandler(ISolution sol)
//        {

//            bestControl = sol as ParameterControl;
//        }

//        private List<ISolution> CratePopulation(int size)
//        {
//            Random random = new Random();
//            var population = new List<ISolution>();
//            for (int i = 0; i < size; i++)
//            {
//                var solution = new ParameterControl();
//                solution.Randomize(random, 0, 5);
//                population.Add(solution);
//            }
//            population.Add(new ParameterControl());
//            population.Add(new ParameterControl());
//            population.Add(new ParameterControl());
//            return population;
//        }
        

//        public GameEngine MakeGameEngine(out Agent trainedShip, out Agent trainerShip)
//        {    
//            GameEngine gameEngine = new GameEngine(new Camera());
//            eval.SetupGameEngine(gameEngine, bestControl, out trainedShip, out trainerShip);
//            return gameEngine;     
//        }
        
//    }
//}
