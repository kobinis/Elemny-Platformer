using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class ProximityMine
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.AlphaFront;
            profile.ColorLogic = null;
            profile.TextureID = "ball128";
            profile.CollisionWidth = profile.Sprite.Width - 2;
            profile.InitSizeID = "30";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatRandom(1200, 60);  // 1/60 of a second
            profile.Mass = 1f;
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.AddEntry(MeterType.Damage, 25f);
            profile.InitHitPointsID = "100"; //check it
            profile.IsDestroyedWhenParentDestroyed = false;
            profile.CollisionType = CollisionType.CollideAll;
            
            //can add damageaoe to endlifeemitter for aoe mines
            /*ImpacedEndAndEmmit endLifeEmitter = new ImpacedEndAndEmmit();
            endLifeEmitter.emmiter = ContentBank.Inst.GetEmitter("BoomerangItem"); //change to KineticMineItem
            profile.impactUpdateList.Add(endLifeEmitter);*/

            profile.ImpactEmitterID = "ExplosionParticleFx";

            profile.IsDestroyedOnCollision = true;
            profile.VelocityInertia = 0.8f;
            profile.TimeOutEmitterID = "FireExplosionFx";
            return profile;
        }
    }
}
