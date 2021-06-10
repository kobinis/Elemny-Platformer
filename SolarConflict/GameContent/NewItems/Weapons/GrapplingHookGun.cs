//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.NewItems.Weapons
//{
//    class GrapplingHookGun
//    {
//        public static Item Make()
//        {
//            var cooldown = Utility.Frames(0.5f);
//            var energyCost = 60f;
//            var shotSpeed = 50f;

//            // Create the main system (and its projectile profile)
//            var harpoonSystem = new GrappleHookSystem();

//            harpoonSystem.ActivationCheck = new ActivationCheck();
//            harpoonSystem.ActivationCheck.costList.Add(MeterType.Energy, energyCost);

//            harpoonSystem.CooldownTime = cooldown;
//            harpoonSystem.MinTetherLength = 0f;
//            harpoonSystem.PullSpreedLimit = 350f;
//            harpoonSystem.SpringConstant = 0.4f;
//            harpoonSystem.SeekDuration = Utility.Frames(1.25f);
//            harpoonSystem.TetherDuration = Utility.Frames(6f);
//            harpoonSystem.Velocity = Vector2.UnitX * shotSpeed;
//            harpoonSystem.WinchRate = 2f;

//            // Use WeaponQuickstart to generate the item and its description and stuff, as though it were a simple gun firing the harpoon projectile
//            WeaponData weaponData = new WeaponData("Grappling Hook", 3, "HarpoonGunItem", null);
//            //, "Attaches a tether to the target, and winches it closer"
//            weaponData.ShotEmitter = harpoonSystem.HarpoonProfile;
//            weaponData.EnergyCost = energyCost;
//            weaponData.KickbackForce = 0.4f;
//            weaponData.Cooldown = cooldown;
//            weaponData.ShotSpeed = shotSpeed;
//            weaponData.ItemData.BuyPrice = 1600;
//            weaponData.ItemData.SellRatio = 0.5f;

//            var result = WeaponQuickStart.Make(weaponData);
//            result.Profile.Level = 5;
//            result.Profile.Category = ItemCategory.Gun;
//            // Replace the aforementioned item's system with our main system
//            result.System = harpoonSystem;
//            return result;
//        }
//    }
//}
