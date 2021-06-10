//using System;
//using System.Linq;
//using System.Text;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using AILab.Utils;

//namespace SolarConflict.AI.Framework
//{
//    [Serializable]
//    public class NeuralNetwork : ISolution
//    {
//        private static Random _rand = new Random();
//        public int MemoryNum
//        {
//            get { return memory.Length; }
//        }
//        public double? Fitness { get; set; }

//        private int inputNum; 
//        private int outputNum;
//        public VectorN[] netData;
//        private VectorN[] memoryNetData;
//        private float[] memory;
//        private VectorN inputVec;
//        private float[] outputs;

//        int ClampValue = 100000;

//        public NeuralNetwork(int inputNum, int outputNum, int memoryNum)
//        {
//            this.inputNum = inputNum;
//            this.outputNum = outputNum;

//            netData = new VectorN[outputNum];
//            memoryNetData = new VectorN[memoryNum];
//            memory = new float[memoryNum];
//            outputs = new float[outputNum];

//            for (int i = 0; i < outputNum; i++)
//            {
//                netData[i] = new VectorN(inputNum + memoryNum + 1);
//            }

//            for (int i = 0; i < memoryNum; i++)
//            {
//                memoryNetData[i] = new VectorN(inputNum + memoryNum + 1);
//            }

//            inputVec = new VectorN(inputNum + memoryNetData.Length + 1);
//        }


//        public NeuralNetwork(NeuralNetwork network) : this(network.inputNum, network.outputNum, network.memory.Length)
//        {
//            CopyFrom(network);
//        }

//        public float[] Evaluate(float[] inputs)
//        {
//            for (int i = 0; i < inputNum; i++) //copys input to input vector
//            {
//                inputVec.SetValue(i, inputs[i]);
//            }

//            for (int i = 0; i < memoryNetData.Length; i++) //copys memory to input vector
//            {
//                inputVec.SetValue(i + inputNum, memory[i]);
//            }

//            inputVec.SetValue(inputNum + memoryNetData.Length, 1); //adding a constant to the vector (can be done once)

//            for (int i = 0; i < netData.Length; i++) //performs calculation for outputs
//            {
//                outputs[i] = Math.Min(Math.Max(VectorN.ScalarProduct(netData[i], inputVec), -ClampValue), ClampValue);
//            }

//            for (int i = 0; i < memoryNetData.Length; i++) //performs calculation for inputs
//            {
//                memory[i] = Math.Min(Math.Max(VectorN.ScalarProduct(memoryNetData[i], inputVec), -ClampValue), ClampValue);
//            }
//            return outputs;
//        }

//        public int GetInputNum()
//        {
//            return inputNum;
//        }

//        public void CopyFrom(NeuralNetwork source)
//        {
//            for (int i = 0; i < netData.Length; i++)
//            {
//                VectorN.CopyVector(source.netData[i], netData[i]);
//            }

//            for (int i = 0; i < memory.Length; i++)
//            {
//                VectorN.CopyVector(source.memoryNetData[i], memoryNetData[i]);
//            }

//            for (int i = 0; i < memory.Length; i++)
//            {
//                memory[i] = 0;
//            }
//        }

//        public void MutateInPlace(float range, int levels)
//        {
//            for (int i = 0; i < netData.Length; i++)            
//                netData[i].Mutate(range, levels, _rand);


//            for (int i = 0; i < memoryNetData.Length; i++)            
//                memoryNetData[i].Mutate(range, levels, _rand);


//            for (int i = 0; i < memory.Length; i++)    
//                memory[i] = 0;
//        }

//        public void Reset()
//        {
//            for (int i = 0; i < netData.Length; i++)
//                netData[i] = new VectorN(netData[i].Length);

//            for (int i = 0; i < memoryNetData.Length; i++)
//                memoryNetData[i] = new VectorN(memoryNetData[i].Length);

//            for (int i = 0; i < memory.Length; i++)
//                memory[i] = 0;
//        }

//        public void Mutate(int factor)
//        {
//            factor += 2;
//            MutateInPlace(factor, factor * 4);
//        }


//        public void CrossInPlace(ISolution obj1)
//        {
//            NeuralNetwork nn = (NeuralNetwork)obj1;
//            for (int i = 0; i < netData.Length; i++)
//            {

//                    VectorN.CopyVector(nn.netData[i], netData[i]);                
//            }
//        }



//        public ISolution Clone()
//        {
//            NeuralNetwork network = new NeuralNetwork(this);            
//            return network;
//        }

//        public void Save(string path)
//        {
//            FileStream stream = File.Create(path);
//            var formatter = new BinaryFormatter();         
//            formatter.Serialize(stream, this);
//            stream.Close();
//        }

//        public static NeuralNetwork Load(string path)
//        {
//            var formatter = new BinaryFormatter();
//            FileStream stream = File.OpenRead(path);        
//            var nnet = (NeuralNetwork)formatter.Deserialize(stream);
//            stream.Close();
//            return nnet;
//        }

//        public ISolution Mutate(float factor)
//        {
//            var result = Clone() as NeuralNetwork;
//            result.MutateInPlace(factor, 8);
//            return result;
//        }

//        public ISolution Cross(ISolution solution)
//        {
//            var result = Clone() as NeuralNetwork;
//            result.CrossInPlace(solution);
//            return result;
//        }

//        public void Randomize(float factor, int levels)
//        {
//            Reset();
//            MutateInPlace(factor, levels);
//        }

//        public void Randomize(float factor)
//        {
//            Randomize(factor, 8);
//        }
//    }
//}
