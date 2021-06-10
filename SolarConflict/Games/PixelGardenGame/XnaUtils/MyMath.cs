using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintPlay
{
    static class MyMath
    {
        public static Random rand = new Random();

        public static float Trapz(float t)
        {
            return Math.Max(Math.Min(2f - Math.Abs(t), 1f), 0); 
        }

        public static double DegDiff(double deg1, double deg2)
        {
            return DegAbs(deg2 - deg1) * DegSign(deg2 - deg1);
        }

        public static bool ToggleBool(bool value)
        {
            if (value) return
                false;
            else
                return true;
        }

        public static double DegAbs(double deg)
        {
            return Math.Abs(RealMod(Math.Abs(deg) + Math.PI, 2.0 * Math.PI) - Math.PI);
        }

        public static double DegSign(double deg)
        {
            return Math.Sign(deg) * Math.Sign(RealMod(Math.Abs(deg) + Math.PI, 2 * Math.PI) - Math.PI);
        }

        public static double Frac(double value)
        {
            return value - (float)Math.Truncate(value);
        }

        public static float Frac(float value)
        {
            return (float)((decimal)value - Decimal.Truncate((decimal)value));
        }

        public static double RealMod(double x, double y)
        {
            double res = x / y;
            return (res - Math.Truncate(res)) * y;
        }

        //Prob

        public static bool Bern(float p)
        {
            return rand.NextDouble() < p;
        }


    }
}
