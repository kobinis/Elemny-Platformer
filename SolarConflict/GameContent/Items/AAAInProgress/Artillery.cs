using Microsoft.Xna.Framework;
using SolarConflict.Framework.Emitters;
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
    class Artillery
    {
        public static Item Make()
        {
            //, "Emits a cone of fire."
            WeaponData data = new WeaponData("Artillery MK I", 4, "Artillery", "Artillery");
            data.ShotEmitter = new OnTargetEmitter(MakeShot( 600, 0), 10000);
            data.ItemData.SecounderyIconID = "lvl" + data.ItemData.Level.ToString();
            data.EnergyCost = 10;
            data.ShotSpeed = 0;
            data.SoundEffectEmitterID = null;
            data.Cooldown = 60*5;
            data.EffectEmitterID = null;
            data.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(data.ItemData.Level) * 3f);
            Item item = WeaponQuickStart.Make(data);
            //item.Profile.Level = 0;
            return item;
        }

        public static ProjectileProfile MakeShot(float damage, float force)
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = new UpdateColorSwitch(Color.Black, new Color(255,10,10));
            profile.TextureID = "marker";
            profile.CollisionWidth = profile.Sprite.Height;
            profile.InitSizeID = "400";           
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = Utility.Frames(1.5f).ToString();
            profile.Mass = 40f;
            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
            profile.CollisionSpec = new CollisionSpec(0,0);
            profile.CollisionSpec.IsDamaging = true;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.TimeOutEmitter = MakeSecounderyShot(damage, force);
            profile.VelocityInertia = 0;
            float delta = MathHelper.PiOver4 / Utility.Frames(1.5f);
            profile.Draw = new ProjectileDrawRotateWithTime(delta, -delta, "marker", "marker");
            // profile.ApplyTags(new Vector3(0.145f, 0.93f, 0.49f), "energy", "medium", "bright");
            return profile;
        }

        public static IEmitter MakeSecounderyShot(float damage, float force)
        {
            GroupEmitter ge = new GroupEmitter();
            ge.EmitType = GroupEmitter.EmitterType.All;

            ge.AddEmitter("ExplosionParentSizedFx1");


            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            
            profile.TextureID = "bigshockwave";
            profile.CollisionWidth = profile.Sprite.Height;
            int size = 400;
            profile.InitSizeID = "1";
            int lifetime = 50;

            profile.InitMaxLifetime = new InitFloatConst(lifetime);
            profile.UpdateSize = new UpdateSizeGrow(size/(float)lifetime,1);
            
            profile.Mass = 40f;
            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
            profile.CollisionSpec = new CollisionSpec(damage /(float)lifetime, force);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            //profile.TimeOutEmitter =
            // profile.ApplyTags(new Vector3(0.145f, 0.93f, 0.49f), "energy", "medium", "bright");
            ge.AddEmitter(profile);
            return ge;
        }



    }
}
