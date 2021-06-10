using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace PaintPlay
{
    class MyMouse
    {
        public MouseState myMouse, lastMouse;

        public MyMouse()
        {
            myMouse = new MouseState();
            lastMouse = new MouseState();
        }

        public bool LeftClick
        {
            get { return lastMouse.LeftButton == ButtonState.Released && myMouse.LeftButton == ButtonState.Pressed; }
        }

        public bool RightClick
        {
            get { return lastMouse.RightButton == ButtonState.Released && myMouse.RightButton == ButtonState.Pressed; }
        }

        public Vector2 Pos
        {
            get { return new Vector2(myMouse.X, myMouse.Y); }
        }

        public Vector2 LastPos
        {
            get { return new Vector2(lastMouse.X, lastMouse.Y); }
        }



        /*public MouseState State;
        {
            get { return mymous; }
        }*/

        public int X
        {
            get { return myMouse.X; }
        }

        public int Y
        {
            get { return myMouse.Y; }
        }

        public int LastX
        {
            get { return lastMouse.X; }
        }

        public int LastY
        {
            get { return lastMouse.Y; }
        }

        public bool GetLeft()
        {
            return myMouse.LeftButton == ButtonState.Pressed;
        }

        public bool GetRight()
        {
            return myMouse.RightButton == ButtonState.Pressed;
        }

        public MouseState GetMouseState()
        {
            return myMouse;
        }


        public void Update()
        {

            lastMouse = myMouse;
            myMouse = Mouse.GetState();
        }

    }
}
