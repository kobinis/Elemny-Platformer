using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;
using Microsoft.Xna.Framework;

namespace SolarConflict.GameContent.Projectiles
{
    class SmallVoidFlameFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOut;//ProjectileProfile.ColorUpdate.FadeInOut;
            profile.TextureID = "AddVoid1";
            profile.ScaleMult = 2.5f / (float)(profile.Sprite.Width);
            profile.InitSize = new InitFloatConst(15);
            profile.UpdateSize = new UpdateSizeGrowMult(0.9f);
            profile.InitMaxLifetimeID = "10";
            profile.Light = new PointLight(new Vector3(0.5f,0.0f,1.0f), 300, 0.25f); ;
            //profile.Light = Lights
            //profile.ApplyTags("effect", "explosion", "medium");
            // profile.ApplyTags("energy", "small");            
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
