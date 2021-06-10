using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework;
using SolarConflict.AI.Framework;
using SolarConflict.AI.GameAI;

namespace SolarConflict.AI
{
    public class ShipEvaluation : IEvaluation
    {
        Random _rand;
        private readonly IGameObjectFactory _shipFactory;
        private readonly IGameObjectFactory _trainerShipFactory;

        int MaxTimeToSimulate = 60 * 300;
        public GameEngine _gameEngine;

        int aiId;
        /// <summary>
        /// Constructor to create the ship evaluation for 2 given ships (ship to train, the training ship - Enemy)
        /// </summary>
        public ShipEvaluation(string shipToTrainId, string trainerShipId, int trainerShipAi)
        {
            _shipFactory = ContentBank.Inst.GetGameObjectFactory(shipToTrainId);
            _trainerShipFactory = ContentBank.Inst.GetGameObjectFactory(trainerShipId);
            aiId = trainerShipAi;
            _rand = new Random();
            _gameEngine = new GameEngine(null);
            _gameEngine.Rand = _rand;
        }


        public float Evaluate(ISolution trainable, SolverCluster cluster)
        {
            // _gameEngine.Rand = cluster.Random;
            Agent trainedShip;
            Agent enemyAgent;
            _gameEngine = new GameEngine(new Camera());
            SetupGameEngine(_gameEngine, trainable, out trainedShip, out enemyAgent);

            while (trainedShip.IsActive && enemyAgent.IsActive && _gameEngine.FrameCounter < MaxTimeToSimulate)
            {
                _gameEngine.Update(null);
            }

            

            float score = trainedShip.GetNormalizedHitpoints() 
                - 1.5f * enemyAgent.GetNormalizedHitpoints() - _gameEngine.FrameCounter * 0.0001f;

            return score;
        }

        //public bool CheckIfEvaluationEnded(GameEngine gameEngine, Agent trainedShip, Agent enemyAgent)
        //{
        //    return trainedShip.IsNotActive || enemyAgent.IsNotActive || gameEngine.FrameCounter >= MaxTimeToSimulate;
        //}

        public void SetupGameEngine(GameEngine gameEngine, ISolution trainable, out Agent trainedShip, out Agent enemyAgent)
        {
            int initial_dist = 3500;
           // gameEngine.Clear();

            trainedShip = (Agent)_shipFactory.MakeGameObject(gameEngine, null, FactionType.Pirates1);
            trainedShip.control.controlAi = trainable as ParameterControl;
            trainedShip.Position = Vector2.UnitX * initial_dist;
            trainedShip.Rotation = gameEngine.Rand.NextFloat() * MathHelper.TwoPi;

            enemyAgent = (Agent)_trainerShipFactory.MakeGameObject(gameEngine, null, FactionType.Federation);
            //var ai = new SmartAI();
            //ai.MinimalDistance = -7000;
            //enemyAgent.control.controlAi = ai;
            //if (aiId != 0)
            //    enemyAgent.control.SetAIControl(aiId);

            gameEngine.AddGameObject(trainedShip);
            gameEngine.AddGameObject(enemyAgent);
            trainedShip.SetTarget(enemyAgent, TargetType.Enemy);
            trainedShip.SetAggroRange(50000, 90000, TargetType.Enemy);
            enemyAgent.SetAggroRange(50000, 90000, TargetType.Enemy);
        }

      
    }
}
