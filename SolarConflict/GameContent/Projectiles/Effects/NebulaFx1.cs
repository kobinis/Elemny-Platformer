using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using Microsoft.Xna.Framework;

namespace SolarConflict.GameContent.Emitters
{
    class NebulaFx1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();          
            profile.TextureID = "add2";
            //profile.DrawType = DrawType.AlphaFront;
            profile.DrawType = DrawType.Additive;
            profile.TextureID = "smoke2";
            //profile.InitColor = new i
            profile.InitColor = new InitColorConst(new Color(0.3755f, 0.25f, 0.5f,0.15f));
            profile.InitParam = new InitFloatRandom(0, 1);
            profile.Draw = new ProjectileDrawRotateWithTime(-0.01f, 0.0091f, "smoke2");
            profile.CollisionWidth = profile.Sprite.Width;
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen; //To do, add collision list type EffectAddOffScreen
            profile.CollideWithMask = GameObjectType.Agent;
            profile.CollisionSpec = new CollisionSpec(0.1f, 0);
            profile.CollisionSpec.AddEntry(MeterType.NebulaGas, 0.1f, ImpactType.Additive);
            profile.IsEffectedByForce = false;
            profile.IsDestroyedOnCollision = false;
            profile.InitSizeID = "500";
            profile.CollideWithMask = GameObjectType.Agent;
            profile.Flags = GameObjectFlags.UpdateOnlyOnScreen;
            return profile;
        }
    }
}
