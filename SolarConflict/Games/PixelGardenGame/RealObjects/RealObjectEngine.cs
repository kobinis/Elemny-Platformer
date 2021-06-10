//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using PaintPlay.XnaUtils.Input;
//using Microsoft.Xna.Framework.Input;

//namespace PaintPlay
//{
//    enum ObjectTypes { Sun, Moon, RainCloud, SnowCloud, SeedBag, PortalIn, PortalOut }
//    static class RealObjectEngine
//    {
//        public static RealObject[] objects;
//        public static int time = 0;

//        public static bool rec = false;

//        public static void Init()
//        {
//            objects = new RealObject[4];
//            objects[0] = new RealSun();
//             objects[1] = new RealMoon();            
//             objects[2] = new RainCloud();
//             objects[3] = new RealFireworks();
//      /*     objects[1] = new RealMoon();
         
//            objects[3] = new SnowCloud();*/
//            time = 0;

//        }

//        public static void Update(GardenLogic gLogic, List<TouchState> inputs)
//        {
//            time++;
//          /*  if (time % 2 == 0)
//            {

                
//               foreach (var item in inputs)
//                {
//                    float minDis = 10000;
//                    RealObject bestObj = null;

//                    if (item.ID == 101 || GestureUtils.GetPress() || true)
//                    {
//                        foreach (var rObj in RealObjectEngine.objects)
//                        {
//                            if (Math.Abs(rObj.area - item.PhysicalArea) < minDis)
//                            {
//                                minDis = Math.Abs(rObj.area - item.PhysicalArea);
//                                bestObj = rObj;
//                            }
//                        }



                       
//                    }


//                    if (minDis < 0.5)
//                    {
//                        bestObj.SetLife();
//                        TouchState state = new TouchState();
//                        state.Position = GestureUtils.GetPos();
//                        bestObj.contact = state;
//                    }
//                }
//            }*/

//            KeyboardState keystate = Keyboard.GetState();
//         /*   if(keystate.IsKeyDown(Keys.Q))
//                rec =true;
//            if (keystate.IsKeyDown(Keys.A))
//                rec = false;*/

//            rec = Game1.idleTime < 30;
            
//            if ( rec)
//            {

//                int index = int.Parse(Game1.tag);
//                RealObject bestObj = RealObjectEngine.objects[index];
//                bestObj.SetLife();
//                TouchState state = new TouchState();
//                state.Position = Game1.objPos;
//                bestObj.contact = state;
//            }

//            foreach (var item in objects)
//            {
//                if (item.life > 0)
//                    item.Update(gLogic);
//            }

//        }

//        public static void Draw()
//        {
//            foreach (var item in objects)
//            {
//                if (item.life > 0)
//                    item.Draw();
//            }

//        }

        

        

        

//    }
//}
