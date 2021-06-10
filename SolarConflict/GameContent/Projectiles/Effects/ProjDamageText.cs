using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Bla
{
    class ProjDamageText
    {
        public static ProjectileProfile  Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.AlphaFront;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.Sprite = null;
            projectileProfile.ScaleMult = 1f; 
            projectileProfile.Draw = new DrawDamageText();

            projectileProfile.InitSize = new InitFloatConst(5);
            projectileProfile.UpdateSize = null;            
            projectileProfile.CollisionSpec = CollisionSpec.SpecEmpty;            

            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.IsEffectedByForce = false;
            projectileProfile.InitColor = new InitColorConst(Color.Red);
            projectileProfile.InitMaxLifetime = new InitFloatConst(50);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;

        }
    }
}
