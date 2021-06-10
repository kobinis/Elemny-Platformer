//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using XnaUtils;

//namespace SolarConflict.Framework.Scenes
//{
//    [Serializable]
//    public class MessageBox
//    {
//        private static Color MESSAGE_COLOR = Color.DimGray;             // the color of the messages
//        private static Vector2 MessagePos;                              // the beginning of the message's text
//        public enum MessageDuration : int { SHORT = 3, NORMAL = 5 };    // the number of seconds to leave the message on screen


//        [Serializable]
//        public class Message
//        {
//            public string Text { get; private set; }    // message's text
//            public int Age { get; private set; }        // the number of frames the message lived

//            public Message(string text, MessageDuration duration)
//            {
//                Text = text;
//                Age = ((int)duration) * Text.Length * 50;
//            }

//            public void Update()
//            {
//                if (Age > 0)
//                {
//                    Age--;
//                }
//            }

//            public bool IsDone()
//            {
//                return Age <= 0;
//            }
//        } // Message

//        private List<Message> Messages;

//        public MessageBox()
//        {
//            Vector2 screen = ActivityManager.ScreenSize;
//            MessagePos = new Vector2(150, screen.Y - 75);
//            Messages = new List<Message>();
//            Messages.Add(new Message("Test message", MessageDuration.NORMAL));
//            Messages.Add(new Message("dslkfgjsdlfkgj sdlfkgj ;sdlfkgj ;sdlfkgj ;lsdkfjg", MessageDuration.SHORT));
//        }

//        public void Update()
//        {
//            if (Messages.Count == 0) return;
//            Message msg = Messages[0];
//            msg.Update();
//            if (msg.IsDone())
//            {
//                Messages.RemoveAt(0);
//            }
//        }

//        public void Draw(SpriteBatch sb)
//        {
//            if (Messages.Count == 0) return;
//            Message msg = Messages[0];
//            SpriteFont font = Game1.font;

//            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
//            DrawMessageText(msg.Text, sb, font);
//            sb.End();
//        }

//        public void DrawMessageText(string text, SpriteBatch sb, SpriteFont font)
//        {
//            sb.DrawString(font, text, MessagePos, MESSAGE_COLOR, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
//        }

//        void SendMessage(string message, MessageDuration duration = MessageDuration.NORMAL)
//        {
//            Messages.Add(new Scenes.MessageBox.Message(message, duration));
//        }
//    }
//}
