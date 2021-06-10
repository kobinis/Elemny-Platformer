using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaUtils.SimpleGui;
using XnaUtils.Input;
using XnaUtils;
using System.Runtime.Serialization;
using XnaUtils.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict.Framework.Scenes.Activitys
{
    [Serializable]
    public class SceneActivityProviderControl:GuiControl
    {
        public static Sprite glow = Sprite.Get("glow128");
        //public bool IsActive { get; set; }
        public String ActivityProvider { get; set; }
        public ActivityParameters ActivityParams { get; set; } //TODO: change to ActivityParams
        public bool IsPlayerNeeded { get; set; }
        public bool IsGlowing { get; set; }
        public SceneComponentSelector.Component ComponentData;
        public Activity Activity;        

        public SceneActivityProviderControl(Vector2 position, Vector2 size ):base(position, size)
        {
            //IsGlowing = true;
        }

        public override void Draw(SpriteBatch sb, Color? color = default(Color?))
        {
            base.Draw(sb, color);
            //if (IsGlowing)
            //{
            //    // BUG: this causes an overflow in the release build                
            //    Color col = Color.Orange;                
            //    byte alpha = (byte)((Math.Cos(Game1.time * 0.001) + 0.5f) * 0.5f * 255);
            //    col.A = alpha;
            //    sb.Draw(glow, Position, null, col, 0, new Vector2(glow.Width / 2, glow.Height / 2), 0.5f, SpriteEffects.None, 0);
            //}
        }
        
    }
}
