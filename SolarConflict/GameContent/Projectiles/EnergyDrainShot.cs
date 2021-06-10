using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class EnergyDrainShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.InitColor = new InitColorConst(Color.Gray);

            profile.TextureID = "add3";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "50";
            //profile.UpdateSize = new UpdateSizeGrow(0, 1.005f);
            profile.InitMaxLifetime = new InitFloatConst(60*3);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = "StunFx";            
            //profile.ImpactEmitterId = "FireExplosionFx";
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.AddEntry(MeterType.Energy, -200);
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.1f, 5);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            profile.Draw = new ProjectileDrawRotateWithTime(-0.03f, 0.031f, "add3");            

            return profile;
        }
    }
}
