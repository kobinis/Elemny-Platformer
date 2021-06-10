//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using PaintPlay.XnaUtils.Input;


//namespace PaintPlay
//{
//    class RealObject
//    {
//        public Vector2 position;
//        protected Texture2D texture;
//        protected float textureScale = 1f;
//        public float rotation;

//        public float area;
//        public int life;
//        public TouchState contact;

//        public RealObject()
//        {
//            area = 10000000;
//        }

//        public virtual void SetLife()
//        {
//            life = 5;
//        }


//        public virtual void Update(GardenLogic gLogic)
//        {
//            life--;
//            position = position = Game1.objPos; //new Vector2(contact.Position.X, contact.Position.Y);
           
//        }

//        public virtual void Draw()
//        {
//            MyGraphics.sb.Draw(texture, position, null, Color.White, rotation,
//            new Vector2(texture.Width / 2, texture.Height / 2), textureScale, SpriteEffects.None,0 );
//        }

//        public virtual void DrawLearn(Vector2 position)
//        {
//            MyGraphics.sb.Draw(texture, position, null, Color.White, rotation,
//            new Vector2(texture.Width / 2, texture.Height / 2),textureScale, SpriteEffects.None, 0);
//        }

//    }
//}
