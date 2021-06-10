using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class SmalFlameFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOut;//ProjectileProfile.ColorUpdate.FadeInOut;
            profile.TextureID = "add8";
            profile.ScaleMult = 2.5f / (float)(profile.Sprite.Width);
            profile.InitSize = new InitFloatConst(15);
            profile.UpdateSize = new UpdateSizeGrowMult(0.9f);
            profile.InitMaxLifetimeID = "10";
            //profile.Light = Lights
            //profile.ApplyTags("effect", "explosion", "medium");
            // profile.ApplyTags("energy", "small");            
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
