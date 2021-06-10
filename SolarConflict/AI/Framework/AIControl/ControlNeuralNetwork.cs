//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using XnaUtils;
//using SolarConflict.AI.Framework;

//namespace SolarConflict
//{

//    [Serializable]
//    public class ControlNeuralNetwork : IAgentControl, ISolution
//    {
//        public struct OutputType
//        {
//            public ControlSignals Signal;
//            public bool IsRotation;
//            public OutputType(ControlSignals signal)
//            {
//                Signal = signal;
//                IsRotation = false;
//            }
//            public OutputType(bool rotation)
//            {
//                IsRotation = rotation;
//                Signal = ControlSignals.None;
//            }

//            //  User-defined conversion from double to Digit
//            public static implicit operator OutputType(ControlSignals signal)
//            {
//                return new OutputType(signal);
//            }

//            public override string ToString()
//            {
//                return IsRotation ? "Rotation" : Signal.ToString();
//            }
//        }

//        public int ID { get; set; }

//        public double? Fitness { get; set; }

//        private List<SensorType> _inputTypes;
//        private List<OutputType> _outputType;
//        private float[] _inputs;
//        private float[] _outputs;

//        public NeuralNetwork _neuralNetwork;

//        public ControlNeuralNetwork() : this(new List<SensorType> { SensorType.Freq1, SensorType.EnemyDistance, SensorType.EnemyAngle, SensorType.EnemyAngleAbslute, SensorType.DangerDistance, SensorType.DangerAngle, SensorType.DangerAngleAbsulute }
//        , new List<OutputType> { ControlSignals.Up, ControlSignals.Down, ControlSignals.Left, ControlSignals.Right, ControlSignals.Action1, ControlSignals.Action2, ControlSignals.Action3, ControlSignals.Action4, new OutputType(true) })
//        {
//            //_neuralNetwork.netData[8].SetValue(1, 1);
//            // _neuralNetwork.netData[0].SetValue(0, 1);

//        }

//        public ControlNeuralNetwork(List<SensorType> inputs, List<OutputType> outputs)
//        {
//            _inputTypes = new List<SensorType>(inputs);
//            _outputType = new List<OutputType>(outputs);
//            _inputs = new float[_inputTypes.Count];
//            _outputs = new float[_outputType.Count];
//            _neuralNetwork = new NeuralNetwork(_inputTypes.Count, _outputType.Count, 0);
//        }

//        public void SetNetworkValue(SensorType input, OutputType output, float value)
//        {
//            //_neuralNetwork
//            throw new Exception();
//        }


//        public ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
//        {
//            //GameObject target = agent.GetTarget(gameEngine, TargetType.Enemy);
//            ControlSignals controlSignals = 0;
//            if (agent.targetSelector.GetTarget(TargetType.Enemy) != null)
//            {

//                for (int i = 0; i < _inputTypes.Count; i++)
//                {
//                    _inputs[i] = Sensors.Inst.GetSensorValue(agent, gameEngine, _inputTypes[i]).Value;
//                }

//                _outputs = _neuralNetwork.Evaluate(_inputs);

//                for (int i = 0; i < _outputType.Count - 1; i++)
//                {
//                    if (!_outputType[i].IsRotation) //TODO: change
//                    {
//                        if (FMath.Bern(_outputs[i], gameEngine.Rand)) //Or change to threshold
//                            controlSignals |= _outputType[i].Signal;
//                    }

//                }

//                //gameEngine.Text = Sensors.Inst.GetSensorValue(agent, gameEngine, SensorType.EnemyAngle).Value.ToString();

//                float angle = agent.Rotation - Sensors.Inst.GetSensorValue(agent, gameEngine, SensorType.EnemyAngle).Value;
//                analogDirections[0] = FMath.ToCartesian(1, angle);
//                analogDirections[1] = analogDirections[0];
//                //gameEngine.Scene
//            }

//            if (float.IsNaN(agent.analogDiractions[1].X))
//                throw new Exception();
//            controlSignals |= ControlSignals.Brake;
//            return controlSignals;
//        }

//        public IAgentControl GetWorkingCopy()
//        {
//            return this;
//        }

//        public ISolution Mutate(float factor)
//        {
//            var result = Clone() as ControlNeuralNetwork;
//            result._neuralNetwork.MutateInPlace(factor, 8);
//            return result;
//        }

//        public ISolution Cross(ISolution solution)
//        {
//            var result = Clone() as ControlNeuralNetwork;
//            result._neuralNetwork.CrossInPlace((solution as ControlNeuralNetwork)._neuralNetwork);
//            return result;
//        }

//        public ISolution Clone()
//        {
//            var result = MemberwiseClone() as ControlNeuralNetwork;
//            result._neuralNetwork = _neuralNetwork.Clone() as NeuralNetwork;
//            return result;
//        }

//        public void Randomize(float factor)
//        {
//            _neuralNetwork.Randomize(factor, 4);
//        }
//    }
//}
