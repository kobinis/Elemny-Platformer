using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class MissileLauncher5
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Missile Launcher");
            // weaponData.ItemData.Description = "Lunches mines from your inventory";
            weaponData.ItemData.IconID = "RocketLauncher";
            weaponData.ItemData.SecounderyIconID = "lvl5";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = 5;
            weaponData.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(5) * 1.5f);
            weaponData.Cooldown = 30;
            weaponData.ActiveTime = 6;
            weaponData.MidCooldownTime = 2;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 65;
            weaponData.ItemData.SlotType = SlotType.Weapon | SlotType.Turret;
            weaponData.ItemData.Category = ItemCategory.AmmoWeapon;
            weaponData.AmmoType = ItemCategory.Missiles;
            weaponData.AmmoPramEmitter = MissileGroup();
            weaponData.ItemData.CraftingStationType = Framework.CraftingStationType.MissileAmmo;
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }

        public static ParamEmitter MissileGroup()
        {
            ParamEmitter emitter = new ParamEmitter();
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
