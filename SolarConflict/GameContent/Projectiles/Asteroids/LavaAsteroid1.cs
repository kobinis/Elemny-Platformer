using Microsoft.Xna.Framework;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Utils.QuickStart;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class LavaAsteroid1
    {
        public static ProjectileProfile Make()
        {

            var profile = AsteroidQuickStart.MakeEmptyAsteroid(new Color(255,245,245));
            profile.Name = "Obsidian Asteroid";
            profile.DrawType = DrawType.Lit;
            var loot = ContentBank.Get("LavaAsteroidLoot1");

            ParamEmitter lootParam = new ParamEmitter(loot);
            lootParam.RotationType = ParamEmitter.EmitterRotation.Random;
            lootParam.RotationRange = 360;
            lootParam.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            lootParam.VelocityAngleRange = 360;
            lootParam.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            lootParam.VelocityMagMin = 1;
            lootParam.VelocityMagRange = 10;
            lootParam.MinNumberOfGameObjects = 1;
            lootParam.RangeNumberOfGameObject = 2;                        

            GroupEmitter emitterGroup = new GroupEmitter();
            emitterGroup.AddEmitter("FxEmitterRockExp");            
            emitterGroup.EmitType = GroupEmitter.EmitterType.All;
            emitterGroup.AddEmitter(lootParam);
            var param = ParamEmitter.MakeSpreadParam(7, 4);
            param.EmitterID = "FireballShot";
            param.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
            emitterGroup.AddEmitter(param);

            profile.HitPointZeroEmiiter = emitterGroup;            

            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            
                
            profile.InitColor = new InitColorConst(new Color(255, 170, 170));
            profile.Light = new PointLight(new Color(255, 120, 120).ToVector3(), 2000, 10);            
            return profile;
        }
    }
}
