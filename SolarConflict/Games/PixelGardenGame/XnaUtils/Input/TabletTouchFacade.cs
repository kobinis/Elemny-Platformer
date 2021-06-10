//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using System.Windows.Forms;


//namespace PaintPlay.XnaUtils.Input
//{
//    class TabletTouchFacade: InputFacade //change to static or singelton
//    {

//        MyMouse mouse;
//        Vector2 firstPosition, prevPos;
//        Dictionary<int, Vector2> firstPos;
//        Dictionary<int, Windows7.Multitouch.TouchEventArgs> prevTouchs;
//        Dictionary<int, Windows7.Multitouch.TouchEventArgs> touchs;
//        Windows7.Multitouch.TouchHandler handler;
        
//        List<TouchState> touchList;



//        public TabletTouchFacade(GameWindow Window)
//        {
//            mouse = new MyMouse();
//            touchList = new List<TouchState>(1);
//            firstPosition = new Vector2();

//            touchs = new Dictionary<int, Windows7.Multitouch.TouchEventArgs>(2);
//            prevTouchs = new Dictionary<int, Windows7.Multitouch.TouchEventArgs>(2);
//            firstPos = new Dictionary<int, Vector2>(2);

//            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
//            handler = Windows7.Multitouch.WinForms.Factory.CreateHandler<Windows7.Multitouch.TouchHandler>(form);
//            handler.DisablePalmRejection = true;
//            //handler.GestureNotify += new EventHandler<Windows7.Multitouch.GestureNotifyEventArgs>(handler_Notify);
//            handler.TouchDown += new EventHandler<Windows7.Multitouch.TouchEventArgs>(handler_TouchDown);
//            handler.TouchMove += new EventHandler<Windows7.Multitouch.TouchEventArgs>(handler_TouchMove);
//            handler.TouchUp += new EventHandler<Windows7.Multitouch.TouchEventArgs>(handler_TouchUp);
//        }


//        void handler_TouchUp(object sender, Windows7.Multitouch.TouchEventArgs e)
//        {
            
//            //Debug.WriteLine("Up " + e.Id + " " + e.Location);
//            //pos.Add(new Vector2(e.Location.X, e.Location.Y))    
//            touchs.Remove(e.Id);            
//        }

//        void handler_TouchMove(object sender, Windows7.Multitouch.TouchEventArgs e)
//        {
            
//            //touchs.Add(e.Id, e);
//            touchs[e.Id] = e;
//        }

//        void handler_TouchDown(object sender, Windows7.Multitouch.TouchEventArgs e)
//        {
//            touchs.Add(e.Id, e);           
//        }

        

//        public override void Update()
//        {
//            mouse.Update();
//            if (mouse.LeftClick)
//            {
//                firstPosition = mouse.Pos;
//                prevPos = mouse.Pos;
//            }
//            touchList.Clear(); 
//            foreach (var item in touchs)
//            {
//                TouchState state = new TouchState();
//                state.ID = item.Value.Id;
//                state.Position = new Vector2(item.Value.Location.X, item.Value.Location.Y);

//                if (prevTouchs.ContainsKey(state.ID))
//                {
//                    Windows7.Multitouch.TouchEventArgs prevTouchArgs = prevTouchs[state.ID];
//                    state.PreviousPosition = new Vector2(prevTouchArgs.Location.X, prevTouchArgs.Location.Y);
//                    //firstPosition[state.ID] =
//                    state.FirstPosition = firstPos[state.ID];
//                }
//                else
//                {
//                    state.PreviousPosition = state.Position;
//                    //Dictionary<int, Windows7.Multitouch.TouchEventArgs> prevTouchs;
//                    firstPos.Add(state.ID, state.Position);                    
//                    state.FirstPosition = state.Position;                    
//                    state.OnPress = true;
//                }
//                touchList.Add(state);
//            }

//            foreach (var item in prevTouchs)
//            {
//                if (!touchs.ContainsKey(item.Key))
//                {
//                    TouchState state = new TouchState();
//                    state.ID = item.Value.Id;
//                    state.Position = new Vector2(item.Value.Location.X, item.Value.Location.Y);
//                    state.PreviousPosition = state.Position;
//                    state.OnRelease = true;
//                    state.FirstPosition = firstPos[state.ID];
//                    firstPos.Remove(state.ID);
//                    touchList.Add(state);
//                }
                
//            }

//            prevTouchs.Clear();
//            foreach (var item in touchs)
//            {
//                prevTouchs.Add(item.Key, item.Value);
//            }                 
            
//        }

//        public override void Draw()
//        {
//            foreach (TouchState touch in touchList)
//            {
//                //MyGraphics.Circle(touch.Position, 1, Color.Red);
//            }
//        }


//        public override List<TouchState> GetTouchColection() //return a copy
//        {            
//            TouchState state;
            
//            if (touchList.Count==0 &&( mouse.GetLeft() || mouse.GetRight() || mouse.lastMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed))
//            {
//                state = new TouchState();
//                state.ID = 100;
//                state.FirstPosition = firstPosition;
//                state.Position = mouse.Pos;
//                state.Rotation = 0;
//                state.PreviousPosition = prevPos;
//                state.PreviousRotation = 0;
//                state.Size = 4;
//                state.IsFingerOrPen = true;
//                state.OnPress = mouse.LeftClick || mouse.RightClick;
//                state.OnRelease = ( mouse.lastMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed  ||
//                    mouse.lastMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)&& !mouse.GetLeft() && !mouse.GetRight();
                           
//                touchList.Add(state);
//                prevPos = mouse.Pos;
//            }

//            /*
//            if (GestureUtils.GetPress())
//            {
//                state = new TouchState();
//                state.Position = GestureUtils.GetPos();
//                if (GestureUtils.GetClick())
//                    state.PreviousPosition = state.Position;
//                else
//                    state.PreviousPosition = GestureUtils.GetPrevPos();

//                state.OnPress = GestureUtils.GetClick();

//                state.FirstPosition = GestureUtils.firstPos;
//                state.ID = 101;

//                touchList.Add(state);
//            }
//            else
//            {
//                if (GestureUtils.GetPrevPress())
//                {
//                    state = new TouchState();
//                    state.Position = GestureUtils.GetPrevPos();                   
//                    state.PreviousPosition = state.Position;
//                    state.OnPress = GestureUtils.GetClick();
//                    state.FirstPosition = GestureUtils.firstPos;
//                    state.OnRelease = true;
//                    state.ID = 101;
//                    touchList.Add(state);
//                }
//            }*/


//            return touchList;
//        }
//    }
//}
