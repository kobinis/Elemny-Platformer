//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using XnaUtils.SimpleGui;
//using XnaUtils.Input;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    [Serializable]
//    class MusicGui : GuiControl
//    {
//        private Rectangle musicRect;
//        private Vector2 startPosition;
//        private Color textColor = Color.Purple;
//        private GuiControl[] volumeBars;
//        private GuiControl volumeUp, volumeDown;
//        private const int maxVolume = 10;
//        private float volumeDelta;

//        public MusicGui()
//        {
//            musicRect = new Rectangle(65, 50, 530, 100);
//            startPosition = new Vector2(musicRect.X + 10, musicRect.Y + 10);
//            volumeDelta = 1 / (float)maxVolume;
//            volumeBars = new GuiControl[maxVolume];
//            for (int i = 0; i < maxVolume; i++)
//            {
//                volumeBars[i] = new GuiControl();
//                volumeBars[i].Sprite = Sprite.Get("emptyframe");
//                volumeBars[i].ControlColor = textColor;
//                volumeBars[i].Action += OnVolumeChange;
//                volumeBars[i].Index = i;
//                volumeBars[i].DrawShade = false;
//                AddChild(volumeBars[i]);
//            }
//            volumeDown = new GuiControl();
//            volumeDown.Sprite = Sprite.Get("minus");
//            volumeDown.ControlColor = textColor;
//            volumeDown.Action += OnVolumeDown;
//            volumeDown.DrawShade = false;
//            AddChild(volumeDown);
//            volumeUp = new GuiControl();
//            volumeUp.Sprite = Sprite.Get("plus");
//            volumeUp.ControlColor = textColor;
//            volumeUp.Action += OnVolumeUp;
//            volumeUp.DrawShade = false;
//            AddChild(volumeUp);
//        }
//        //TODO: Ron - instead of explicitly changing the volume in MusicEngine, need to add functions in it for Inc/Dec/SetVolume, and call those
//        private void OnVolumeUp(GuiControl source, CursorInfo cursorLocation)
//        {
//            var currentVolume = MusicEngine.Instance.MasterVolume;
//            if (currentVolume + volumeDelta <= 1f)
//            {
//                MusicEngine.Instance.MasterVolume += volumeDelta;
//            }
//            else
//            {
//                MusicEngine.Instance.MasterVolume = 1f;
//            }
//            UpdatePreferences();
//        }

//        private void OnVolumeDown(GuiControl source, CursorInfo cursorLocation)
//        {
//            var currentVolume = MusicEngine.Instance.MasterVolume;
//            if (currentVolume - volumeDelta >= 0f)
//            {
//                MusicEngine.Instance.MasterVolume -= volumeDelta;
//            }
//            else
//            {
//                MusicEngine.Instance.MasterVolume = 0;
//            }
//            UpdatePreferences();
//        }

//        private void OnVolumeChange(GuiControl source, CursorInfo cursorLocation)
//        {
//            MusicEngine.Instance.MasterVolume = (source.Index + 1) / 10.0f;
//            UpdatePreferences();
//        }

//        private void UpdatePreferences()
//        {
//            PreferencesManager.Inst.PrefsStorage.MusicVolume = MusicEngine.Instance.MasterVolume;
//            PreferencesManager.Inst.SavePreferences();
//        }

//        public override void Draw(SpriteBatch sb, Color? color = null)
//        {
//            float yoffset = 0;
//            sb.Draw(Sprite.Get("greybar"), musicRect, new Color(255, 255, 255, 100));  //draw music box border
//            sb.DrawString(Game1.font, PlotTexts.MusicTitle, startPosition, Color.Purple);  //draw title
//            var stringSize = Game1.font.MeasureString(PlotTexts.MusicTitle);
//            yoffset += stringSize.Y + 5;
//            Vector2 position = new Vector2(startPosition.X, startPosition.Y + yoffset);
//            sb.DrawString(Game1.font, PlotTexts.MusicVolume, position, Color.Purple);  //draw volume line
//            position.X += Game1.font.MeasureString(PlotTexts.MusicVolume).X;
//            float wholeVolume = MusicEngine.Instance.MasterVolume * 10f;    //some weird floating point shit happens if I do this line and the next in 1 step
//            int volume = (int)Math.Round(wholeVolume);
//            int xoffset = 0;
//            volumeDown.Width = volumeDown.Height = (int)stringSize.Y - 3;
//            volumeDown.Position = new Vector2((int)position.X + volumeDown.HalfSize.X + xoffset, (int)position.Y + volumeDown.HalfSize.Y);
//            volumeDown.Draw(sb);
//            xoffset += (int)stringSize.Y;
//            int i = 0;
//            for (i = 0; i < volume; i++)
//            {
//                volumeBars[i].Sprite = Sprite.Get("fullframe");
//                volumeBars[i].Width = volumeBars[i].Height = (int)stringSize.Y - 3;
//                volumeBars[i].Position = new Vector2((int)position.X + volumeBars[i].HalfSize.X + xoffset, (int)position.Y + volumeBars[i].HalfSize.Y);
//                volumeBars[i].Draw(sb);
//                //sb.Draw(Sprite.Get("fullframe"), new Rectangle((int)position.X + xoffset, (int)position.Y, (int)stringSize.Y - 3, (int)stringSize.Y - 3), textColor);
//                xoffset += (int)stringSize.Y;
//            }
//            for (; i < maxVolume; i++)
//            {
//                volumeBars[i].Sprite = Sprite.Get("emptyframe");
//                volumeBars[i].Width = volumeBars[i].Height = (int)stringSize.Y - 3;
//                volumeBars[i].Position = new Vector2((int)position.X + volumeBars[i].HalfSize.X + xoffset, (int)position.Y + volumeBars[i].HalfSize.Y);
//                volumeBars[i].Draw(sb);
//                //sb.Draw(Sprite.Get("emptyframe"), new Rectangle((int)position.X + xoffset, (int)position.Y, (int)stringSize.Y - 3, (int)stringSize.Y - 3), textColor);
//                xoffset += (int)stringSize.Y;
//            }
//            volumeUp.Width = volumeUp.Height = (int)stringSize.Y - 3;
//            volumeUp.Position = new Vector2((int)position.X + volumeDown.HalfSize.X + xoffset, (int)position.Y + volumeDown.HalfSize.Y);
//            volumeUp.Draw(sb);
//            xoffset += (int)stringSize.Y;
//            yoffset += stringSize.Y + 5;
//            //sb.DrawString(Game1.font, "Music Engine. volume: " + MusicEngine.Instance.CurrentVolume, new Vector2(100, 50), Color.Purple);
//            //sb.DrawString(Game1.font, "Play position: " + MediaPlayer.PlayPosition, new Vector2(100, 70), Color.Purple);
//        }

//        public override void Update(InputState inputState)
//        {
//            base.Update(inputState);
//        }
//    }
//}
