//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils;
//using XnaUtils.SimpleGui;
//using SolarConflict.CardGame;
//using XnaUtils.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace SolarConflict.GameContent.Activities.Games
//{
//    class CardGameActivity : Activity
//    {
//        Texture2D cardback = TextureBank.Inst.GetTexture("grey_panel");
//        Vector2 cardSize = new Vector2(60, 100)*1.5f;
        
//        GuiManager _gui;
//        CardGameEngine _game;
//        SpriteBatch sb;

//        public CardGameActivity()
//        {
//            _gui = new GuiManager();
//            _game = new CardGameEngine();
//            sb = Game1.SpriteBatch;
//        }

//        public override void Update(InputState inputState)
//        {
//            if(inputState.IsKeyPressed(Keys.Escape))
//            {
//                ActivityManager.Inst.Back();
//            }           

//            if(inputState.Cursor.OnReleaseLeft)
//            {
//                for (int i = 0; i < _game.Hand.Cards.Count; i++)
//                {
//                    if(GetCardRectangle(_game.Hand, i).Contains(inputState.Cursor.PostionAsPoint))
//                    {
//                        _game.PlayCard(i);
//                    }
//                }
//            }
//        }

//        public override void Draw(SpriteBatch sb)
//        {
//            ActivityManager.GraphicsDevice.Clear(Color.Azure);
//            sb.Begin();
//            DrawEnemyCard();
//            DrawHand(_game.Hand);
//            DrawPlayer(_game.Enemy, ActivityManager.ScreenCenter - Vector2.UnitY * cardSize.Y * 2);
//            DrawPlayer(_game.Player, ActivityManager.ScreenCenter + Vector2.UnitY * cardSize.Y * 1.5f);
//            sb.End();
//        }

//        private void DrawEnemyCard()
//        {
//            Rectangle rect = FMath.GetRectangle(ActivityManager.ScreenCenter - Vector2.UnitY * cardSize.Y * 1.1f, cardSize);
//            DrawCard(_game.EnemyCard, rect);
//        }

//        private void DrawHand(Hand hand)
//        {
//            float size = cardSize.X + 10;
//            for (int i = 0; i < hand.Cards.Count; i++)
//            {                                
//                DrawCard(hand.Cards[i], GetCardRectangle(hand, i));
//            }
//        }

//        public Rectangle GetCardRectangle(Hand hand, int i)
//        {
//            float size = cardSize.X + 10;
//            Vector2 pos = ActivityManager.ScreenSize * 0.5f + new Vector2((i+0.5f) * size - hand.Cards.Count * 0.5f * size, 0);
//            return new Rectangle((int)(pos.X - cardSize.X * 0.5f), (int)(pos.Y - cardSize.Y * 0.5f), (int)cardSize.X, (int)cardSize.Y);                        
//        }

//        private void DrawCard(Card card, Rectangle rect)
//        {
            
//            sb.Draw(cardback, rect, Color.White);
//            sb.DrawString(Game1.font, card.Value.ToString(), new Vector2(rect.X+5, rect.Y+5), Color.Black);
//        }

//        public void DrawPlayer(IPlayer player, Vector2 position)
//        {
//           // sb.Draw(cardback, rect, Color.White);
//            sb.DrawString(Game1.font, player.Hitpoints.ToString(), position, Color.Black);
//        }
        

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new CardGameActivity();
//        }


//    }
//}
