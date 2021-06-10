using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class MiniMissileLauncher //TODO: change to MiniMissile
    {
        public static Item Make()
        {            
            WeaponData data = new WeaponData("Energy-Missile Launcher", 3, "RocketLauncherItem");           
            data.ShotEmitter = MissileGroup();
            data.ActiveTime = 5;
            data.MidCooldownTime = 1;
            data.ShotSpeed = 25;
            data.SoundEffectEmitterID = null;
            data.Cooldown = 60;
            data.ItemData.Level = 3;
            data.ItemData.BuyPrice = 5000;
            data.Range = 1580;
            data.EnergyCost = 200;
          //  data.SoundEffectEmitterID = "sound_missilefx";

            //data.AmmoType = ItemCategory.Missiles;
            Item item = WeaponQuickStart.Make(data);
          //  item.Profile.Level = 0;
            return item;
        }

        public static ParamEmitter MissileGroup()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "MiniMissile";
            emitter.MinNumberOfGameObjects = 2;
            emitter.RangeNumberOfGameObject = 0;
            emitter.VelocityAngleRange = 10;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 5;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            return emitter;
        }
    }
}
