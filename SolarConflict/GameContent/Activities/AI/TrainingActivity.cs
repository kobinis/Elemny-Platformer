//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils;
//using SolarConflict.AI.Framework;
//using SolarConflict.AI;
//using SolarConflict.AI.Framework.PopulationOperators;
//using SolarConflict.Framework.Utils;
//using System.Threading;
//using Microsoft.Xna.Framework;
//using AILab.Utils;

//namespace SolarConflict.GameContent.Activities.AI
//{
//    class TrainingActivity : Activity
//    {
//        private enum State { Start, Simulation, ShowingAgent }
//        private State _state;
//        SolverCluster _cluster;
//        List<parametre> _solutions;

//        private GameEngine _gameEngine;
//        private Thread _thread;
//        ShipEvaluation _evaluation;
//        Agent _trainedShip, _trainingShip;
//        string _traindShipID = "PrologueShip1"; //"PirateMediumB1";//"PrologueShip1";


//        private TrainingActivity()
//        {

//        }

//        public override void OnEnter(ActivityParameters parameters)
//        {
//            if (parameters != null && parameters.ParamDictionary.ContainsKey("LoadoutID"))
//            {
//                this._traindShipID = parameters.ParamDictionary["LoadoutID"];
//            }

//            var popSize = 10;
//            var population = new List<ISolution>(popSize);
//            Enumerable.Range(0, popSize).Do(i => population.Add(new ControlNeuralNetwork()));
//            population.Do(n => n.Randomize(4));

//            _cluster = new SolverCluster(population);
//            _solutions = new List<ControlNeuralNetwork>();

//            var evaluator = new EvaluationOperator();

//            _evaluation = new ShipEvaluation(_traindShipID, "StartingShip1", 0);// ("StartingShip1", "tutorialEnemy", 0);
//            var evaluationGroup = new EvaluationGroup();
//            evaluationGroup.Evaluators.Add(_evaluation);
//            evaluationGroup.Evaluators.Add(new ShipEvaluation(_traindShipID, "DeployableTurret1", 0));
//            evaluationGroup.Evaluators.Add(new ShipEvaluation(_traindShipID, "StartingShip1", 0));

//            //    evaluationGroup.Evaluators.Add(new ShipEvaluation(_traindShipID, "StartingShip1", 0));



//            evaluator.Evaluation = evaluationGroup;

//            var feedbackEvent = new FeedbackEventOperator();
//            feedbackEvent.BestSloutionEvent += FeedbackCallback;

//            _cluster.PopulationOperators = new List<IPopulationOperator>();
//            _cluster.PopulationOperators.Add(new GenerationOperator(evaluator, feedbackEvent, new RepopulateOperator(10)));

//            _gameEngine = new GameEngine(new Camera(ActivityManager.SpriteBatch));
//            _thread = new Thread(StartTraining);
//            _state = State.ShowingAgent;
//            var sol = new ControlNeuralNetwork();
//            sol.Fitness = 20;
//            _solutions.Add(sol);
//            _evaluation.SetupGameEngine(_gameEngine, _solutions.Last(), out _trainedShip, out _trainingShip);
//            _state = State.Start;
//            _thread.Start();
//        }

//        public void StartTraining()
//        {
//            _cluster.RunOneGeneration();
//        }

//        private void FeedbackCallback(ISolution solution)
//        {
//            _solutions.Add(solution as ControlNeuralNetwork);
//        }

//        public override bool Update(InputState inputState)
//        {
//            switch (_state)
//            {
//                case State.Start:
//                    if (_thread.IsAlive)
//                    {
//                        _state = State.Simulation;
//                    }
//                    break;
//                case State.Simulation:
//                    if (!_thread.IsAlive)
//                    {
//                        _state = State.ShowingAgent;
//                        _evaluation.SetupGameEngine(_gameEngine, _solutions.Last(), out _trainedShip, out _trainingShip);
//                    }
//                    break;
//                case State.ShowingAgent:
//                    _gameEngine.Update();
//                    if (_evaluation.CheckIfEvaluationEnded(_gameEngine, _trainedShip, _trainingShip) || inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
//                    {
//                        _state = State.Start;
//                        _thread = new Thread(StartTraining);
//                        _thread.Start();
//                    }
//                    break;
//                default:
//                    break;
//            }
//            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
//            {
//                AgentLoadout loadout = ContentBank.Inst.GetEmitter(_traindShipID) as AgentLoadout;
//                if (loadout != null)
//                {
//                    loadout.AiKey = 200;
//                    _solutions.Last().ID = 200;
//                    AIBank.Inst.AddControl(_solutions.Last());
//                }

//                ActivityManager.Back();
//            }
//            return false;
//        }

//        public override void Draw(SpriteBatch sb)
//        {
//            switch (_state)
//            {
//                case State.Start:
//                    break;
//                case State.Simulation:

//                    break;
//                case State.ShowingAgent:
//                    if (_trainedShip != null && !_thread.IsAlive)
//                    {
//                        _gameEngine.Camera.Zoom = 0.2f;
//                        _gameEngine.Camera.Position = _trainedShip.Position;
//                        _gameEngine.Draw(sb);
//                    }
//                    break;
//                default:
//                    break;
//            }

//            sb.Begin();
//            for (int i = 0; i < _solutions.Count; i++)
//            {
//                sb.DrawString(Game1.font, "Sol:" + _solutions[i].Fitness.ToString(), Vector2.One * 10 + Vector2.UnitY * 40 * i, Color.White);
//            }
//            sb.End();
//            if (_solutions.Count > 0)
//                DrawNeuralNetwork(Game1.sb, _solutions.Last()._neuralNetwork);
//        }

//        private static void DrawNeuralNetwork(SpriteBatch sb, NeuralNetwork neuralNetwork)
//        {
//            sb.Begin();
//            VectorN[] netdata = neuralNetwork.netData;
//            for (int i = 0; i < netdata.Length; i++)
//            {
//                for (int j = 0; j < netdata[i].Length; j++)
//                {
//                    Vector2 pos = Vector2.One * 300 + new Vector2(j * 100, i * 50);
//                    string text = ", " + netdata[i].GetValue(j).ToString();
//                    sb.DrawString(Game1.font, text, pos, Color.White);
//                }
//            }
//            sb.End();
//        }

//        public static Activity ActivityProvider(string param)
//        {
//            return new TrainingActivity();
//        }
//    }
//}
