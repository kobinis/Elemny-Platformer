using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintPlay.XnaUtils
{
    public delegate float Norma(float x, float y); //possibly double, move to shape bank

    static class ShapeBank
    {
        public static float CircleNorma(float x, float y)
        {
            return (float)Math.Sqrt(x * x + y * y);
            //return (float)Math.Sqrt( x * x + y * y);
           // return Math.Max(Math.Abs(x), Math.Abs(y));
           // return 0.5f * ((float)Math.Cos(Math.Atan2(y, x) * 5) + 2);
            //return (float)Math.Sqrt(Math.Sqrt(x*x*x*x+y*y*y*y));//
       /*{*/
           //return Math.Max(Math.Abs(x), Math.Abs(y));//(float)Math.Sqrt( x * x + y * y);
          // return (float)Math.Sqrt(Math.Sqrt(x*x*x*x+y*y*y*y));//
       /*    
       }*/
        }

        public static float RoundedRectNorma(float x, float y)
        {
            return (float)Math.Sqrt(Math.Sqrt(x * x * x * x + y * y * y * y));
        }


        public static float StopsignNorma(float x, float y)
        {
            return Math.Max(RectangleNorma(x,y), Norma1(x*0.7f,y*0.7f));
        }

        public static float RectangleNorma(float x, float y) //norma inf
        {
            return Math.Max(Math.Abs(x), Math.Abs(y));
        }

        public static float Norma1(float x, float y)
        {
            return Math.Abs(x) + Math.Abs(y);
        }

    }
}
