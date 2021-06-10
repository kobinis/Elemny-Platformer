using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
        class HeavyShotTrail
        {
            public static ProjectileProfile Make()
            {
                ProjectileProfile profile = new ProjectileProfile();
                profile.DrawType = DrawType.Additive;
                profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
                profile.ColorLogic = ColorUpdater.FadeOut;
                profile.TextureID = "add2";
                //profile.Draw = new ProjectileDrawRotateWithTime(0.02f,0.022f);
                profile.CollisionWidth = profile.Sprite.Width - 5;
                profile.InitSizeID = "15";
                profile.UpdateSize = new UpdateSizeGrow(2, 1, 80);
                profile.InitMaxLifetime = new InitFloatConst(50);
                profile.Mass = 0.1f;                
                profile.CollisionSpec = new CollisionSpec(0.5f, 0f); //1
                profile.IsDestroyedOnCollision = true;
                profile.IsEffectedByForce = false;
                profile.Draw = new ProjectileDrawRotateWithTime(0f, 0f, "add2");
                return profile;
            }
        }
    }
