using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class SunFullscreenColorFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();            
            profile.TextureID = "blank";            
            //profile.InitSize = new InitFloatParentSize(2.5f,0);
            profile.InitSizeID = "5000";
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            DrawAllScreen draw = new DrawAllScreen();
            draw.OverlayColor = new Microsoft.Xna.Framework.Color(255, 100, 20);
            draw.AlphaOffset = 0.3f;
            profile.Draw = draw;
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
