using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    class WormSegment
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ObjectType |= GameObjectType.PotentialTarget;
            profile.AggroRange = 4500;
            profile.DrawType = DrawType.Alpha;            
            profile.TextureID = "WormSegment1"; 
            profile.CollisionWidth = profile.Sprite.Width+5;
            profile.InitHitPointsID = "100";
            profile.InitSizeID = "20";
            profile.InitMaxLifetime = new InitFloatConst(60 * 60*2); 
            profile.Mass = 0.5f;            
            profile.CollisionType = CollisionType.CollideAll;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.InitColor = new InitColorConst(new Color(200,255,200,255));

            var movement = new UpdateIfParent();
            movement.UpdateParentActive = new MoveToTarget(ProjectileTargetType.Parent, 6f, 8, true);
            movement.UpdateParentNullOrNotActive = new MoveForward(0.4f, 6.5f);

            profile.MovementLogic = movement;
            profile.VelocityInertia = 0.8f;
            var rotationLogic = new UpdateIfParent();
            rotationLogic.UpdateParentActive = new UpdateRotationForward(0.1f);  //new UpdateRotationParent();
            rotationLogic.UpdateParentNullOrNotActive = new UpdateRotationHoming(0.08f, 5, ProjectileTargetType.Enemy);
            profile.RotationLogic = rotationLogic;
            profile.CollisionSpec = new CollisionSpec(5, 5f);
            profile.CollisionSpec.IsDamaging = true;
            
            var draw = new DrawIfParent();
            draw.DrawParentActive = new ProjectileDrawScale((Sprite)"WormSegment1");
            draw.DrawParentNullOrNotActive = new ProjectileDrawScale((Sprite)"WormHead1");


            //draw.DrawParentActive = new ProjectileDrawScale((Sprite)"body1");
            //var drawAni = new ProjectileDrawAni();
            //drawAni.AddTextureId("head0");
            //drawAni.AddTextureId("head1");
            //drawAni.AddTextureId("head2");
            //drawAni.AddTextureId("head3");
            //drawAni.paramMult = 0.1f;
            //draw.DrawParentNullOrNotActive = drawAni;
            //profile.UpdateList.Add(new UpdateParamSumVelocity());


            GroupEmitter hitpointsZeroEmitter = new GroupEmitter();
            hitpointsZeroEmitter.EmitType = GroupEmitter.EmitterType.All;
            hitpointsZeroEmitter.AddEmitter("Biomass");
            hitpointsZeroEmitter.AddEmitter("BloodSplashFx1");
          //  hitpointsZeroEmitter.AddEmitter("sound_splat");
            hitpointsZeroEmitter.AddEmitter("ParentFadeFx");

            profile.Draw = draw;
            profile.TimeOutEmitterID = typeof(ParentFadeFx).Name;
            profile.HitPointZeroEmiiter = hitpointsZeroEmitter;
              
            profile.FactionLogic = ProjectileProfile.FactionLogicType.Const;
            profile.FactionType = Framework.FactionType.Vile;
            return profile;
        }
    }
}
