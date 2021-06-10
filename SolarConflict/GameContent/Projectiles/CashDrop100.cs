using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class CashDrop100
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.InitColor = new InitColorConst(new Color(100,255,100));
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOut;
            profile.TextureID = "add6";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "20";
            profile.UpdateSize = null; // new UpdateSizeGrow(1.1f);
            profile.InitMaxLifetime = new InitFloatConst(60 * 30);  // 1/60 of a second
            profile.Mass = 0.1f;
            //   profile.ImpactEmitterID = "EmitterPickupFx";
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Money, 100));
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.Draw = new ProjectileDrawRotateWithTime(-0.1f, 0.1f, "add6", "add6"); //new ProjectileDrawRotateWithTime();
            profile.VelocityInertia = 0.98f;
            profile.CollusionUpdateList.Add(new ImpactEndOnObjectType());
            //  profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.1f, 10);
            profile.ObjectType |= GameObjectType.Collectible;
            //profile.CollideWithMask = GameObjectType.Agent;
            return profile;
        }
    }
}
