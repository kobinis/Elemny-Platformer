//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace XnaUtils.Framework.Graphics
//{
//    public class Palette
//    {        
//        protected Color[] data;
        
//        public int Length
//        {
//            get { return data.Length; }        
//        }


//        public Palette(int length)
//        {
//            data = new Color[length];
//        }
        
//        public Color this[int i]
//        {
//            get
//            {          
//                return data[i];
//            }
//            set
//            {
//                data[i] = value;
//            }
//         }

//        public void Invert()
//        {
//            for (int i = 0; i < Length; i++)
//            {
//                Color color = data[i];
//                data[i] = new Color((byte)255 - color.R, (byte)255 - color.G, (byte)255 - color.B, color.A);
//            }
//        }
        
//        //save
//        //load
        
//    }
//}
