using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    class BlobSegment
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.AggroRange = 4500;
            profile.DrawType = DrawType.Additive;
            profile.TextureID = "BlobSegment";
            profile.CollisionWidth = profile.Sprite.Width-10;
            profile.InitHitPointsID = "300";
            profile.InitSizeID = "50";
            profile.InitMaxLifetime = new InitFloatConst(60 * 60 * 2);
            profile.Mass = 0.5f;
            profile.CollisionType = CollisionType.CollideAll;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.InitColor = new InitColorConst(new Color(200, 255, 200, 255));
            var movement = new UpdateIfTarget(); //TODO: Change to ancestor is active
            movement.UpdateParentActive = new MoveToTarget(ProjectileTargetType.LivePrimeAncestor, 4f, 5, true);
            movement.UpdateParentNullOrNotActive = new MoveToTarget(ProjectileTargetType.AnyPotentialTarget, 4f, 5f, false);
            profile.MovementLogic = movement;
            profile.VelocityInertia = 0.8f;
            profile.RotationLogic = new UpdateRotationForward(0.1f); //Change to rotate to last target// or rotate twards forward
            profile.CollisionSpec = new CollisionSpec(2, 5f);
            profile.CollisionSpec.IsDamaging = true;
            profile.ScaleMult = 0.05f;
            var draw = new DrawIfTarget();
            draw.DrawActive = new ProjectileDrawScale((Sprite)"BlobSegment");
            draw.DrawNullOrNotActive = new ProjectileDrawScale((Sprite)"BlobHead");
            profile.Draw = draw;
            GroupEmitter hitPointZeroEmiiter = new GroupEmitter();                       
            hitPointZeroEmiiter.AddEmitter("ParentFadeAddtiveFx");
            //TODO: add sound
            //Add gibs

            GroupEmitter ge = new GroupEmitter();
            ge.RefVelocityMult = 0.8f;
            ge.EmitType = GroupEmitter.EmitterType.All;
            ge.AddEmitter("Biomass");
            ge.AddEmitter("EmitterDebris1");
            hitPointZeroEmiiter.AddEmitter(ge);
            profile.HitPointZeroEmiiter = hitPointZeroEmiiter;
            profile.FactionLogic = ProjectileProfile.FactionLogicType.Const;
            profile.FactionType = Framework.FactionType.Vile;
            //profile.HitPointZeroEmiiterID = "ProjShipwreck1"; //TODO: add
            return profile;
        }
    }
}
