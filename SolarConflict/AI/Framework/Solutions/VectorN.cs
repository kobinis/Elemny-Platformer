using SolarConflict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace AILab.Utils
{
    [Serializable]
    public class VectorN //can be a static functions working on a 1d array, TODO: remove class
    {   
  
        //to do:operator overloading + * []...
        private float[] data;
        

        public int Length
        {
            get { return data.Length; }            
        }       

        public VectorN(int length)
        {
            data = new float[length];
        }

        public VectorN(VectorN vectorN):this(vectorN.Length)
        {
            CopyVector(vectorN, this);
        }

        public static VectorN PointMultiply(VectorN sig1, VectorN sig2)
        {
            VectorN ans = new VectorN(sig1.Length);
            for (int i = 0; i < ans.Length; i++)
            {
                ans.data[i] = sig1.data[i] * sig2.data[i];
            }
            return ans;
        }

        public static float ScalarProduct(VectorN sig1, VectorN sig2)
        {
            float ans = 0;
            for (int i = 0; i < Math.Min(sig1.Length, sig2.Length); i++)
            {
                ans += sig1.data[i] * sig2.data[i];
            }
            return ans;
        }

        public static void CopyVector(VectorN source, VectorN destenation)
        {
            source.data.CopyTo(destenation.data, 0);
        }
               
        public void Mutate(float range, int levels, Random rand)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (rand.Next(10) == 0)
                    data[i] = 0;
                else
                    data[i] += ((rand.Next(-levels, levels + 1) / (float)levels) * range);
               
            }         
        }

        public void Mult(float r)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= r;
            }
        }       

        public void SetValue(int ind, float value)
        {
            data[ind] = value;
        }

        public float GetValue(int ind)
        {
            return data[ind];
        }

    }
}

