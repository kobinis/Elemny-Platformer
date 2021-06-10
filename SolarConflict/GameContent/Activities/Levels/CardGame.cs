using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework.Input;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework;

namespace SolarConflict.GameContent.Activities.Levels
{
    class CardGame : Activity
    {
        public class CardGameEngine
        {
            public static Random rng = new Random();
            public int DelarHealth = 45;
            public int PlayerHelath = 20;
            public Deck delarsDeck;
            public Card delarsCard;
            public Deck playerDeck;
            public Hand playerHand;

            public CardGameEngine()
            {
                delarsDeck = new Deck();
                delarsCard = null;
                playerDeck = new Deck();
                playerHand = new Hand();
                StartGame();
                for (int i = 0; i < 4; i++)
                {
                    DrawToHand();
                }
            }

            private void StartGame()
            {
                Update();
                
            }

            public void Update()
            {
                delarsCard = delarsDeck.cards.Dequeue();
                DrawToHand();
            }

            public void DrawToHand()
            {
                playerHand.cards.Add(playerDeck.cards.Dequeue());
            }
            

        }
            



        public class Deck
        {
            public Queue<Card> cards;

            public Deck()
            {
                cards = new Queue<Card>();
                List<Card> cardList = new List<Card>();
                for (int i = 2; i <= 14; i++)
                {
                    var card = new Card(i, i.ToString() + "_of_clubs");
                    cardList.Add(card);
                }
                for (int i = 2; i <= 14; i++)
                {
                    var card = new Card(i, i.ToString() + "_of_clubs");
                    cardList.Add(card);
                }
                cardList.Add(new Card(14, "14_of_spades"));
                cardList.Add(new Card(14, "14_of_spades"));
                for (int i = 2; i <= 14; i++)
                {
                    var card = new Card(i, i.ToString() + "_of_clubs");
                    cardList.Add(card);
                }
                for (int i = 2; i <= 14; i++)
                {
                    var card = new Card(i, i.ToString() + "_of_clubs");
                    cardList.Add(card);
                }
                cardList.Add(new Card(14, "14_of_spades"));
                cardList.Add(new Card(14, "14_of_spades"));
                FMath.Shuffle(cardList, FMath.Rand);
                foreach (var item in cardList)
                {
                    cards.Enqueue(item);
                }
                
            }
        }

        public class Hand
        {
            public int CardLimit = 6;
            public List<Card> cards;      
            public Hand()
            {
                cards = new List<Card>();
            }      
        }

        public class Card
        {
            public Card(int value, String spriteID)
            {
                Value = value;
                Sprite = Sprite.Get(spriteID);
            }
            public int Value;
            public Sprite Sprite;            
        }
        Background background;
        Camera camera;
        GameEngine gameEngine;
        GuiManager gui;
        CardGameEngine game;
        VerticalLayout layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
        ImageControl delarsCard;
        HorizontalLayout hand = new HorizontalLayout(Vector2.Zero);
        //List<Keys> _keys = new List<Keys> { Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.K, Keys.D7, Keys.D8, Keys.D9 };       
        List<Keys> _keys = new List<Keys> { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        RichTextControl delarsHP;
        RichTextControl playerHP;
        public CardGame()
        {
            background = new Background(8);
            camera = new Camera();
            gameEngine = new GameEngine(camera);
            gui = new GuiManager();
            game = new CardGameEngine();
            delarsHP = new RichTextControl(game.DelarHealth.ToString(), Game1.menuFont, true);
            playerHP = new RichTextControl(game.PlayerHelath.ToString(), Game1.menuFont, true);
            layout.AddChild(delarsHP);
            delarsCard = new ImageControl(game.delarsCard.Sprite, Vector2.Zero, game.delarsCard.Sprite.Size * 0.4f);
            delarsCard.ControlColor = Color.LightGray;
            layout.AddChild(delarsCard);
            gui.AddControl(layout);
            MakeCards();            
            layout.AddChild(hand);
            
            layout.AddChild(playerHP);
        }

        private void MakeCards()
        {                                                                                                                                   
            hand.RemoveAllChildren();
            int i = 0;
            Color[] colors = new Color[] { Color.Magenta, Color.Orange, Color.Green, Color.Blue, Color.Yellow };
            foreach (var item in game.playerHand.cards)
            {
                var cardLayout = new VerticalLayout(Vector2.Zero);
                var handCard = new ImageControl(item.Sprite, Vector2.Zero, game.delarsCard.Sprite.Size * 0.4f);
                handCard.ControlColor = Color.LightGray;
                cardLayout.AddChild(handCard);
                //ImageControl colorMark = new ImageControl(Sprite.Get("blank"), Vector2.Zero, new Vector2(handCard.Width, 50));
                //colorMark.ControlColor = colors[i];
                RichTextControl colorMark = new RichTextControl((i + 1).ToString(), Game1.menuFont, true);
                colorMark.TextColor = colors[i];
                cardLayout.AddChild(colorMark);
                hand.AddChild(cardLayout);
                i++;
            }

            delarsHP.Text = "#simage{void2,200,200}\n" + game.DelarHealth.ToString();
            playerHP.Text = game.PlayerHelath.ToString();
        }

        public override void Draw(SpriteBatch sb)
        {
            background.Draw(camera);            
            gui.Draw(Color.White);
            gameEngine.Draw(camera.SpriteBatch);
        }

        public override void Update(InputState inputState)
        {
            gameEngine.Update(inputState);
            gui.Update(inputState);
            delarsCard.Sprite = game.delarsCard.Sprite;            
            if (inputState.IsKeyPressed(Keys.Escape))
                ActivityManager.Inst.Back();
            //ConsoleKey read   
            //Console.ReadKey(false);
            //xcvbnknb
            for (int i = 0; i < _keys.Count; i++)
            {
                if (inputState.IsKeyPressed(_keys[i]))
                {
                    if(game.playerHand.cards.Count > i)
                    {
                        
                        if(game.playerHand.cards[i].Value > game.delarsCard.Value)
                        {
                            gameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                            gameEngine.AddGameObject("sound_exp2", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                            gameEngine.AddGameObject("BloodSplashFx1", Framework.FactionType.Neutral, camera.GetWorldPos(delarsHP.Position));
                            //.exp2
                            game.DelarHealth -= 2;
                        }

                        if (game.playerHand.cards[i].Value == game.delarsCard.Value)
                        {
                            game.DelarHealth -= 1;
                            game.PlayerHelath -= 1;
                        }

                       if (game.playerHand.cards[i].Value < game.delarsCard.Value)
                        {
                            gameEngine.AddGameObject("BloodSplashFx1", Framework.FactionType.Neutral, camera.GetWorldPos(playerHP.Position));
                            //playerHP.Position
                            game.PlayerHelath -= 2;
                        }
                        if(game.DelarHealth <= 0)
                        {

                            gameEngine.AddGameObject("VictoryImage", Framework.FactionType.Alliance, Vector2.Zero);// camera.GetWorldPos(delarsHP.Position));
                            gameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                            gameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                            gameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                        }

                        if (game.PlayerHelath <= 0)
                        {
                            gameEngine.AddGameObject("DefeatImage", Framework.FactionType.Alliance, Vector2.Zero);// camera.GetWorldPos(delarsHP.Position));
                            //gameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                            //gameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                            //gameEngine.AddGameObject("FireworksSource", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
                        }

                        game.playerHand.cards.RemoveAt(i);
                        game.Update();
                        MakeCards();

                    }
                    
                }
            }
            //if (game.DelarHealth <= 0)
            //{
            //    gameEngine.AddGameObject("Fireworks", Framework.FactionType.Neutral, Vector2.UnitY * ActivityManager.ScreenSize.Y * 0.5f, -90);
            //}
            
        }


        public static Activity ActivityProvider(string parameters)
        {
            //MethodBase.GetCurrentMethod().DeclaringType.GetConstructor(System.Type.EmptyTypes).Invoke()
            return new CardGame();
        }
    }
}
