using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.SimpleGui.Controllers;
using System.Runtime.Serialization;
using XnaUtils;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    [Serializable]
    class StarDateIndicator : IHudComponent
    {
        [NonSerialized]
        private RichTextControl textControl;

        public StarDateIndicator()
        {
            InitNonSerialized();
        }

        public void Update(Scene scene, Agent player)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            textControl.Position = pos;
            //textControl.Position = new Microsoft.Xna.Framework.Vector2(ActivityManager.ScreenSize.X - textControl.HalfSize.X - 10, 5 + textControl.HalfSize.Y);
            textControl.Text = scene.MetaWorld.Stardate.ToString();
           // textControl.Text = scene.gal
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
