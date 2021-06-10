using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class Shield
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "Shield";
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add11";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSize = new InitFloatParentSize(1.8f, 0);            
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.InitColor = new InitColorConst(Color.LightBlue);
            profile.Mass = 0.1f;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.IsDestroyedOnCollision = false;
            profile.IsDestroyedWhenParentDestroyed = true;
            profile.IsEffectedByForce = false;
            profile.CollisionType = CollisionType.CollideAll;
            return profile;
        }
    }
}
