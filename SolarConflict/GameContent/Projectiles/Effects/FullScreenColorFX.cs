using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class FullScreenColorFX
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.TextureID = "blank";            
            profile.InitSizeID = "200";
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;            
            profile.DrawType = DrawType.Additive;
            DrawAllScreen draw = new DrawAllScreen();
            draw.OverlayColor = new Color(100,200,100,100);
            draw.AlphaOffset = 0.3f;
            profile.Draw = draw;
            profile.InitMaxLifetime = new InitFloatConst(20);
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            profile.CollideWithMask = GameObjectType.None;
            return profile;
        }
    }
}
