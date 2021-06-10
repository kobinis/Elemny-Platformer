using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Emitters;
using XnaUtils.Graphics;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Emitters.Effects;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// An egg that after a delay spwans a boss worm
    /// </summary>
    class WormEgg
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
           
            profile.TextureID = "egg"; //TODO: maybe change to countdown animation
            profile.CollisionWidth = profile.Sprite.Width - 5; //adjusts colider size to display size
            profile.InitSizeID = "100";
            profile.InitMaxLifetime = new InitFloatConst(60 * 7); //7 sec delay
            profile.Mass = 2f;
            // profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;
            profile.CollisionType = CollisionType.CollideAll;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
           
            profile.VelocityInertia = 0.99f;
           
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.CollisionSpec.IsDamaging = true;
         
            profile.UpdateList.Add(new UpdateParentNull());
            //Add gibs and green blod
            GroupEmitter gp = new GroupEmitter();
           
            gp.AddEmitter("BigBloodSplashFx");
            gp.AddEmitter("BigWorm");
            gp.AddEmitter("sound_bigwormspwan");

            profile.TimeOutEmitter = gp;
            profile.HitPointZeroEmiiter = gp;

            profile.InitHitPointsID = "150";
            return profile;
        }
    }
}
