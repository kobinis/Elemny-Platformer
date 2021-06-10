using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class Fireflys
    {
        //Flows targets nearby, provides light
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            
            //profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "lightglow";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "15";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatRandom(60*90, 60*3);
            profile.InitHitPointsID = "70";            
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.VelocityInertia = 1;            
            profile.Mass = 0.1f;
            profile.MovementLogic = new MoveToTargetFuzzy(ProjectileTargetType.Parent, 0.08f, 20); //0.8, 20
            profile.CollideWithMask = GameObjectType.EnergyProjectile;
            profile.ObjectType |= GameObjectType.Ship;
            profile.HitPointZeroEmiiterID = "Lumite";
            //profile.CollisionType = CollisionType.CollideAll;
            profile.Light = new PointLight(new Vector3(1, 1, 0.6f), 600, 1); //Lights.MediumLight(new Color(255, 255, 150));
            return profile;
        }
    }
}
