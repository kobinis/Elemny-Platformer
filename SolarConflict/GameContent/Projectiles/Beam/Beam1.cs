using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Projectiles
{
    class Beam1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Light = Lights.MediumLight(Color.Purple);
            profile.DrawType = DrawType.Beams;
   //         profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "VoidBeam01";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "5";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(1);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0;
            profile.MovementLogic = new MoveToRaycast(1000);
            profile.ImpactEmitterID = "EmitterImpactFxVoid1";
           // profile.ImpactEmitter = new ParticleSystemEmitter()
            profile.CollisionSpec = new CollisionSpec(3, 0);
            profile.Draw = new DrawBeam();
           // profile.MovementLogic = null;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
