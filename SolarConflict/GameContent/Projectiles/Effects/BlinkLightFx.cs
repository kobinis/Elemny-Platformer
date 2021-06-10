using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class BlinkLightFx
    {
        public static ProjectileProfile Make()
        {            
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.AlphaFront;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.TextureID = "smallLight";            
            projectileProfile.InitSize = new InitFloatConst(20);            
            projectileProfile.InitMaxLifetime = new InitFloatConst(15);
            projectileProfile.MovementLogic = new MoveWithParent();
            projectileProfile.InitColor = new InitColorConst(Color.LightYellow);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;
        }
    }
}
