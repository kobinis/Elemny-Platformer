using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.AAAInProgress
{
    class FlareItem
    {
        public static Item Make()
        {
            //, "Emits a cone of fire."
            WeaponData data = new WeaponData("Flare system", 10, "salvage-drone", null);            
            data.ShotEmitter = MakeEmitter(MakeShot(600, 0));
            data.ItemData.SecounderyIconID = "lvl" + data.ItemData.Level.ToString();
            data.EnergyCost = 10;
            data.ShotSpeed = 0;
            data.SoundEffectEmitterID = null;
            data.Cooldown = 60 * 8;
            data.EffectEmitterID = null;
            data.ItemData.SlotType = SlotType.Utility;
            data.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(data.ItemData.Level) * 3f);
            data.ActiveTime = 90;
            data.MidCooldownTime = 6;
            data.ItemData.Category |= ItemCategory.NonAI;
            Item item = WeaponQuickStart.Make(data);            
            //item.Profile.Level = 0;
            return item;
        }

        public static IEmitter MakeEmitter(IEmitter shotEmitter)
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.Emitter = shotEmitter;
            emitter.MinNumberOfGameObjects = 5;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 15;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
            emitter.VelocityAngleBase = 180;
            emitter.VelocityAngleRange = 360;
            return emitter;
        }

        public static ProjectileProfile MakeShot(float damage, float force)
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
          //  profile.ColorLogic = new UpdateColorFade();
            profile.TextureID = "lightglow";
            profile.CollisionWidth = profile.Sprite.Height;
            profile.InitSizeID = "25";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = Utility.Frames(5f).ToString();
            profile.Mass = 40f;
            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
            profile.CollisionSpec = new CollisionSpec(0, 0);
            profile.CollisionSpec.IsDamaging = true;
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            profile.IsDestroyedOnCollision = false;
            //profile.TimeOutEmitter = MakeSecounderyShot(damage, force);
            profile.VelocityInertia = 0.98f;
            profile.InitHitPointsID = "100";
            profile.CollisionType = CollisionType.CollideAll;
         //   profile.UpdateEmitterID = "ProjFxSmoke1";
            profile.UpdateEmitterCooldownTime = 3;
            profile.TimeOutEmitterID = "FireSparkFx";
          //  profile.IsPotentialTarget = true;
            //   profile.Flags = GameObjectFlags.
            profile.ObjectType |= GameObjectType.IsProjectileTarget;
            return profile;
        }
    }
}
