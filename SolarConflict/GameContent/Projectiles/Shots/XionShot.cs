using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots {
    
    class XionShot {
        public static ProjectileProfile Make() {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "disrupter";
            profile.CollisionWidth = profile.Sprite.Height - 5; // why -5?
            profile.InitSizeID = "25";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = Utility.Frames(1.5f).ToString();
            profile.Mass = 40f;
            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
            profile.CollisionSpec = new CollisionSpec(220f, 20f);
            // ^ TEMP. Knockback looks ridiculous with low-mass asteroids, and our physics can't handle massive ones
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = true;
           // profile.ApplyTags(new Vector3(0.145f, 0.93f, 0.49f), "energy", "medium", "bright");
            return profile;
        }
    }
}
