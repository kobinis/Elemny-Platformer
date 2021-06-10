using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class ShieldFx1
    {       
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "shockwave2";
            projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width - 6) * 2.2f;
            projectileProfile.InitSize = new InitFloatParentSize(); // change name to parent
            projectileProfile.UpdateSize = null;
            projectileProfile.InitMaxLifetime = new InitFloatConst(20);//texture from param
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0); //new MoveWithParent();
            projectileProfile.InitColor = new InitColorConst(Color.White);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;
        }
        
    }
}
