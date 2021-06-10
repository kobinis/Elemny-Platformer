//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.AI.Framework
//{
//    class ParameterControlOperations
//    {
//        public void MutateInPlace(Random random)
//        {
//            Fitness = float.MinValue;
//            ShotSpeed = MathHelper.Clamp(ShotSpeed * (float)Math.Pow(2, random.Next(3)) * 0.5f, 4, 512);
//            MinimalDistance = MathHelper.Clamp(ShotSpeed * (float)Math.Pow(2, random.Next(3)) * 0.5f, 4, 4048);
//            for (int i = 0; i < enemyActionParams.Count; i++)
//            {
//                enemyActionParams[i].MaxRange = MathHelper.Clamp(enemyActionParams[i].MaxRange * (float)Math.Pow(2, random.Next(3)) * 0.5f, 4, 8096);
//                enemyActionParams[i].AngleRange = MathHelper.Clamp(enemyActionParams[i].AngleRange, -1, 1);
//            }
//        }

//        public ISolution Mutate(Random random, float factor)
//        {
//            ParameterControl clone = (ParameterControl)GetClone();
//            clone.MutateInPlace(random);
//            return clone;
//        }

//        public ISolution Cross(Random random, ISolution solution)
//        {
//            ParameterControl clone = (ParameterControl)GetClone();
//            clone.Fitness = float.MinValue;
//            ParameterControl otherSolution = (ParameterControl)solution;
//            if (random.Next(2) == 0)
//                clone.ShotSpeed = otherSolution.ShotSpeed;
//            if (random.Next(2) == 0)
//                clone.MinimalDistance = otherSolution.MinimalDistance;
//            for (int i = 0; i < clone.enemyActionParams.Count; i++)
//            {
//                if (random.Next(2) == 0)
//                    clone.enemyActionParams[i] = otherSolution.enemyActionParams[i].GetClone();
//            }
//            return clone;
//        }

//        public void AddDefalutControls()
//        {
//            //TODO: eqip agent with defalut gear, generate AI
//            throw new NotImplementedException();
//        }

//        public void Randomize(Random random, float factor, int range)
//        {
//            enemyActionParams.Clear();
//            AddDefalutControls();
//            for (int i = 0; i < range + 1; i++)
//            {
//                MutateInPlace(random);
//            }
//        }

//        public ISolution GetClone()
//        {
//            ParameterControl control = (ParameterControl)MemberwiseClone();
//            control.enemyActionParams = new List<ActionParams>();
//            foreach (var param in this.enemyActionParams)
//            {
//                control.enemyActionParams.Add(param.GetClone());
//            }
//            return control;
//        }
//    }
//}
