using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Emitters;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class RepairShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "EnergyReplenishShot";
            profile.InitColor = new InitColorConst(new Color(100,255,100));
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add6";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "15";
            profile.UpdateSize = null; // new UpdateSizeGrow(1.1f);
            profile.InitMaxLifetime = new InitFloatConst(1000);  // 1/60 of a second
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(RepairFx).Name;
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Hitpoints, 2));
            profile.CollisionSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Shield, 2));
            profile.IsDestroyedOnCollision = true; 
            profile.IsEffectedByForce = false;
            profile.Draw = new ProjectileDrawRotateWithTime(-0.1f, 0.1f, "add6", "add6"); // new ProjectileDrawRotateWithTime();
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.AgentAncestor, 0.1f, 10);
            return profile;
        }
    }
}
