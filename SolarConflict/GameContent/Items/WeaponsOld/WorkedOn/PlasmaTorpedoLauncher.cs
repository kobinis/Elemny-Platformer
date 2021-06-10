//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Projectiles;
//using SolarConflict.GameContent.Projectiles.Shots;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.NewContent.Emitters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.Weapons.WorkedOn {
//    // TODO: might wanna make this a warhead, instead of a standalone weapon    
//    class PlasmaTorpedoLauncher {        
//        public static Item Make() {            
//            var weaponData = new WeaponData("Plasma Torpedo Launcher", "Launches torpedoes that ignites after a delay and leave a damaging trail as they accelerate", 0, "replace", "item2");
//            weaponData.ShotEmitterID = typeof(PlasmaTorpedoShot).Name;
//            weaponData.KickbackForce = 0.1f;
//            weaponData.Cooldown = Utility.Frames(4f);
//            weaponData.ShotSpeed = 35;
//            weaponData.ItemData.BuyPrice = 3000;
//            weaponData.ItemData.SellRatio = 0.5f;                        

//            return WeaponQuickStart.Make(weaponData);            
//        }        
//    }
//}
