using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaUtils.Graphics;
using XnaUtils.Framework.Graphics;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Framework.Scenes.Activitys;

namespace SolarConflict.GameContent.Activities
{
    /// <summary>
    /// An activity for setting the sound and the music volume
    /// </summary>
    class VolumeSettingsActivity : SceneActivity
    {
        const float DefaultMusicVolume = 0.3f;

        private GuiManager gui;
        // private float scale = 1;
        private Texture2D cover;

        public VolumeSettingsActivity()
        {
            IsPopup = true;
        }

        protected override void Init(ActivityParameters parameters)
        {
            //ActivityManager.InputState.Resrt;
            //  preferences = SettingsManager.Inst.Preferences;
            cover = TextureBank.Inst.GetTexture("cover2");
            gui = new GuiManager();
            var layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
            layout.HalfSize = ActivityManager.ScreenSize * 0.5f;
            layout.ShowFrame = true;
            gui.AddControl(layout);

            RichTextControl title = new RichTextControl("Volume settings", Game1.menuFont);
            layout.AddChild(title);

            float sliderLength = (int)(ActivityManager.ScreenSize.X * 0.8f);
            float sliderWidth = (int)(Math.Max(ActivityManager.ScreenSize.Y * 0.05f, 15));
            Vector2 size = new Vector2(sliderLength, sliderWidth);
            var controlersLayout = new VerticalLayout(Vector2.Zero);
            

            VerticalLayout musicLayout = new VerticalLayout(Vector2.Zero);
            RichTextControl musicTitle = new RichTextControl("Music Volume");
            musicLayout.AddChild(musicTitle);
            var musicSlider = new HorizontalSliderControl(size, "guif8");            
            musicSlider.Value = VolumeSettings.MusicVolume;
            musicSlider.OnValueChange += MusicSlider_Action;   
            musicLayout.AddChild(musicSlider);
            controlersLayout.AddChild(musicLayout);

            VerticalLayout soundLayout = new VerticalLayout(Vector2.Zero);
            RichTextControl soundTitle = new RichTextControl("Effects Volume");
            soundLayout.AddChild(soundTitle);
            var soundSlider = new HorizontalSliderControl(size, "guif8");            
            soundSlider.Value = VolumeSettings.EffectsVolume;//preferences.GetFloat(soundSlider.UserData, 1);
            soundSlider.OnValueChange += SoundSlider_OnValueChange;
            soundLayout.AddChild(soundSlider);
            controlersLayout.AddChild(soundLayout);

            VerticalLayout dialogLayout = new VerticalLayout(Vector2.Zero);
            RichTextControl dialogTitle = new RichTextControl("Dialog Volume");
            dialogLayout.AddChild(dialogTitle);
            var dialogSlider = new HorizontalSliderControl(size, "guif8");            
            dialogSlider.Value = VolumeSettings.DialogVolume; //preferences.GetFloat(dialogSlider.UserData, 1);            
            dialogSlider.OnValueChange += DialogSlider_OnValueChange;
            dialogLayout.AddChild(dialogSlider);
        //    controlersLayout.AddChild(dialogLayout);

            layout.AddChild(controlersLayout);

        }

        private void DialogSlider_OnValueChange(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            VolumeSettings.DialogVolume = (source as HorizontalSliderControl).Value;
        }

        private void SoundSlider_OnValueChange(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            VolumeSettings.EffectsVolume = (source as HorizontalSliderControl).Value;
        }

        private void MusicSlider_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            VolumeSettings.MusicVolume = (source as HorizontalSliderControl).Value;
        }

        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            //if (inputState.IsKeyPressed(Keys.Escape))
            //{
            //    ActivityManager.Back();
            //}
            gui.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            GraphicsUtils.DrawBackground(cover, sb);
            sb.End();
            gui.Draw();
            base.Draw(sb);
        }

        public override ActivityParameters OnBack()
        {
            SettingsManager.Inst.Save();
            return null;
        }


        public static Activity ActivityProvider(string parameters)
        {
            return new VolumeSettingsActivity();
        }

    }
}
