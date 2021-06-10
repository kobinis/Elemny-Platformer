using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    /// <summary>
    /// Fireball with trail, stuns and damages, used as shots and lava asteroid
    /// </summary>
    class FireballShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Mass = 1;
            profile.MovementLogic = new MoveForward(0.3f, 20);
            profile.TextureID = "add8";
            profile.InitMaxLifetime = new InitFloatRandom(60, 30);
            profile.InitSizeID = "10";
            //profile.ColorLogicID = "FadeInOut"; //maybe FadeinOut
            profile.CollisionType = CollisionType.Collide1;
            profile.DrawType = DrawType.None;
            //  profile.InitColor = new InitColorRandomHue();
            profile.UpdateSize = UpdateSizeFunc.MidValues(10, 80, 75);
            profile.UpdateEmitter = MakeTrail();// "GenericTrail";
            // profile.TimeOutEmitter = FireworksExp();
            profile.Light = Lights.MediumLight(Color.LightYellow);
            profile.CollisionSpec = new CollisionSpec(200, 0.5f, 10);
            profile.ObjectType |= GameObjectType.EnergyProjectile;
            profile.CollideWithMask = GameObjectType.Ship | GameObjectType.Agent | GameObjectType.Asteroid;
            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
            profile.CollisionType = CollisionType.CollideAll;
            profile.ImpactEmitterID = "FlameAoe";             
            return profile;
        }

        //public static ProjectileProfile Explotion()
        //{
        //    ProjectileProfile profile = new ProjectileProfile();
        //    profile.DrawType = DrawType.Additive;
        //    profile.CollisionType = CollisionType.Effects;
                        
        //    profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
        //    profile.ColorLogic = ColorUpdater.FadeOut;
        //    profile.TextureID = "add2";
        //    profile.Draw = new ProjectileDrawRotateWithTime(0f, 0f, "add2");

        //    profile.CollisionWidth = profile.Sprite.Width - 5;
        //    profile.InitSizeID = "15";
        //    profile.UpdateSize = new UpdateSizeGrow(4, 1.01f);
        //    profile.InitMaxLifetime = new InitFloatConst(50);
        //    profile.Mass = 0.1f;
        //    profile.VelocityInertia = 0.99f;
        //    profile.CollisionSpec = new CollisionSpec(1f, 0f); //1
        //    profile.IsDestroyedOnCollision = false;
        //    profile.IsEffectedByForce = false;
        //    //profile.ApplyTags("effect", "explosion", "medium");
        //    //  profile.ApplyTags("energy", "medium");
        //    profile.Light = Lights.LargeLight(Color.White);
        //    return profile;
        //}

        public static ProjectileProfile MakeTrail()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.InitColor = new InitColorParent();
            profile.InitSize = new InitFloatParentSize();
            profile.VelocityInertia = 0.5f;
            profile.TextureID = "add8";
            //profile.InitSizeID = "20";
            profile.InitMaxLifetimeID = "65";
            profile.ColorLogicID = "FadeOut";
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            profile.Draw = new ProjectileDrawRotateWithTime(0.1f, 0.102f, "add8", "add8");
          //  profile.IsDestroyedWhenParentDestroyed = true;
            return profile;
        }

    }
}
