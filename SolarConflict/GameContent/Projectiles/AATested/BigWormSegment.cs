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
    class BigWormSegment
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ObjectType |= GameObjectType.PotentialTarget;
            profile.AggroRange = 4500;
            profile.DrawType = DrawType.Alpha;
            profile.TextureID = "body1";
            profile.CollisionWidth = profile.Sprite.Width + 5;
            profile.InitHitPointsID = "800";
            profile.InitSizeID = "200";
            profile.InitMaxLifetime = new InitFloatConst(60 * 60 * 2);
            profile.Mass = 0.9f;
            profile.CollisionType = CollisionType.CollideAll;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.InitColor = new InitColorConst(new Color(200, 255, 200, 255));
            var movement = new UpdateIfParent();
            movement.UpdateParentActive = new MoveToTarget(ProjectileTargetType.Parent, 7, 20, true);
            movement.UpdateParentNullOrNotActive = new MoveForward(5, 20);
            profile.MovementLogic = movement;
            profile.VelocityInertia = 0.8f;
            var rotationLogic = new UpdateIfParent();
            rotationLogic.UpdateParentActive = new UpdateRotationForward(0.1f);  //new UpdateRotationParent();
            rotationLogic.UpdateParentNullOrNotActive = new UpdateRotationHoming(0.05f, 5,ProjectileTargetType.Player);
            profile.RotationLogic = rotationLogic;
            profile.CollisionSpec = new CollisionSpec(5, 5f);
            profile.CollisionSpec.IsDamaging = true;
            var draw = new DrawIfParent();
            draw.DrawParentActive = new ProjectileDrawScale((Sprite)"body1");
            draw.DrawParentNullOrNotActive = new ProjectileDrawScale((Sprite)"body1");


            //draw.DrawParentActive = new ProjectileDrawScale((Sprite)"body1");
            var drawAni = new ProjectileDrawAni();
            drawAni.AddTextureId("head0");
            drawAni.AddTextureId("head1");
            drawAni.AddTextureId("head2");
            drawAni.AddTextureId("head3");
            drawAni.lifeTimeMult = 0.05f;
            draw.DrawParentNullOrNotActive = drawAni;
           // profile.UpdateList.Add(new UpdateParamSum(0.01f)); //chn

           // GroupEmitter hitPointZeroEmiiter = new GroupEmitter();
           // hitPointZeroEmiiter.AddEmitter(typeof(ParentFadeFx).Name);

            GroupEmitter hitPointZeroEmiiter = new GroupEmitter();
            hitPointZeroEmiiter.EmitType = GroupEmitter.EmitterType.All;
            hitPointZeroEmiiter.AddEmitter("BigWormLoot");
            hitPointZeroEmiiter.AddEmitter("BigBloodSplashFx");
            hitPointZeroEmiiter.AddEmitter("sound_splat");
          //  hitPointZeroEmiiter.AddEmitter(ge);

            profile.Draw = draw;
            profile.TimeOutEmitterID = typeof(ParentFadeFx).Name;
            profile.HitPointZeroEmiiter = hitPointZeroEmiiter;

            profile.FactionLogic = ProjectileProfile.FactionLogicType.Const;
            profile.FactionType = Framework.FactionType.Vile;
            return profile;
        }
    }
}
