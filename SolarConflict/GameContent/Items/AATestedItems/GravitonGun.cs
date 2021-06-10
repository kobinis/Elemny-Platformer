using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class GravitonGun
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Graviton Gun");
            //weaponData.ItemData.Description = "Shots a projectile that upon impact crates a gravity well";
            weaponData.ItemData.IconID = "GravitonGunItem";
           // weaponData.ItemData.EquippedTextureId = "GravitonGun";
            weaponData.ItemData.Level = 5;
            weaponData.ActiveTime = 1;
            weaponData.Cooldown = Utility.Frames(10f);
            weaponData.ShotEmitter = MakeShot();
            weaponData.EnergyCost = 50;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 50;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level);
            //TODO: speed mult = 0;
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }

        public static ProjectileProfile MakeShot()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "space-mine-advanced";
            profile.Draw = new ProjectileDrawRotateWithTime(-0.11f, 0.1f, "space-mine-advanced", "space-mine-advanced");

            profile.InitSizeID = "60";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;
            // profile.RotationLogic = new UpdateRotationForward();
            profile.ImpactEmitterID = typeof(GravityWellAoe).Name;
            profile.TimeOutEmitterID = typeof(GravityWellAoe).Name;
            profile.CollisionSpec = new CollisionSpec(0, 0);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
