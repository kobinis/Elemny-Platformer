//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace PaintPlay
//{
//    class RainCloud : RealObject
//    {


//        public RainCloud()
//        {
//            rotation = 0;
//            texture = MyGraphics.GetTexture("cl");
//            this.textureScale = 0.5f;
//        }

       

//        public override void Update(GardenLogic gLogic)
//        {
//            base.Update(gLogic);
//            //this.rotation = this.contact.Orientation;
//            int x = (int)(position.X*(gLogic.mcga.Width/(float)MyGraphics.screenRect.Width));
//            int y = (int)(position.Y*(gLogic.mcga.Height/(float)MyGraphics.screenRect.Height));
//            GPixel pixel = new GPixel();
//            pixel.type = PixelType.Snow;
//            int r = MyMath.rand.Next(50);
//            pixel.color = Color.White;
            
//            for (int i = x-12; i < x+12; i++)
//            {
//                pixel.param1 = MyMath.rand.Next(3) - 1;
//                pixel.param2 = MyMath.rand.Next(3) - 1;
//                if(MyMath.rand.Next(4)==0)
//                    GardenHelper.Line(i - 1, y, i + 1, y , pixel, gLogic.grid);
//            }
            
//        }

//        public override void Draw()
//        {
//            base.Draw();
//        }

        
//    }
//}
