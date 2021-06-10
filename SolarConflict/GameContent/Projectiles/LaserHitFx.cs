using Microsoft.Xna.Framework;
using SolarConflict.GameContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class LaserHitFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();            
            //display
            profile.DrawType = DrawType.Additive;
            profile.TextureID = "light";            
            profile.CollisionWidth = profile.Sprite.Width;            
            
            profile.ColorLogicID = "FadeOutSlow";

            profile.InitMaxLifetimeID = "2";

            profile.InitSizeID = "InitFloatRandom,6,10";
            
            profile.UpdateSize = new UpdateSizeGrow(-0.1f);



            profile.CollisionType = CollisionType.Effects; //updateOnScreen

           //  profile.ApplyTags(Color.Red.ToVector3(), "effect", "energy", "small");

            return profile;
        }
    }
}
