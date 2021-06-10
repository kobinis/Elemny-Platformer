using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.SimpleGui.Controllers;
using System.Runtime.Serialization;
using XnaUtils;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    /// <summary>
    /// Show an indecator for  the player faction money and higlelights (and grows) in green or red when value changed had changed
    /// </summary>
    [Serializable]
    class MoneyIndicator : IHudComponent
    {
        [NonSerialized]
        private RichTextControl textControl;
        private float value;
        private float colorAlpha;        

        public MoneyIndicator()
        {
            InitNonSerialized();
        }

        public void Update(Scene scene, Agent player)
        {            
            colorAlpha *= 0.94f;
            float newValue = scene.GetPlayerFaction().GetMeter(MeterType.Money).Value;
            if(value != newValue)
                colorAlpha = Math.Sign(newValue - value);
            value = newValue;
            textControl.Text = value.ToString() + "#image{money}";
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            textControl.Position = pos;
            //textControl.Position = new Microsoft.Xna.Framework.Vector2(ActivityManager.ScreenSize.X - textControl.HalfSize.X - 10, 5 + textControl.HalfSize.Y);
            Color color = Color.White;
            if (colorAlpha >= 0)
                color = Color.Lerp(Color.White, Color.Green, colorAlpha);
            else
                color = Color.Lerp(Color.White, Color.Red, -colorAlpha);
            color.A = (byte)(Math.Abs(colorAlpha) * 255);
            textControl.TextColor = color;
            spriteBatch.Begin();
            textControl.Draw(spriteBatch);
            spriteBatch.End();            
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            InitNonSerialized();
        }

        private void InitNonSerialized()
        {
            textControl = new RichTextControl("0", isShowFrame: true);
           // textControl.Position = new Microsoft.Xna.Framework.Vector2(ActivityManager.ScreenSize.X - textControl.HalfSize.X - 10, 5 + textControl.HalfSize.Y);
        }

        public Rectangle GetSize()
        {
            return FMath.GetRectangle(Vector2.Zero, textControl.HalfSize * 2);
        }

    }
}
