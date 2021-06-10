//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    class MultiMissileLauncher
//    {
//        public static Item Make() {
//            return WeaponQuickStart.Make(MakeData());            
//        }

//        public static WeaponData MakeData() { 
//            WeaponData weaponData = new WeaponData("Multi-Missile Launcher");
//            weaponData.ItemData.Description = "Shoots a barrage of missiles";
//            weaponData.ItemData.TextureId = "MissilesItem"; 
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 0;
//            weaponData.ShotEmitterID = typeof(MissileBarrage1).Name;
//            weaponData.ItemData.BuyPrice = 1200;
//            weaponData.Cooldown = 60 * 4;
//            weaponData.IsTurreted = true;
            
//            return weaponData;
//        }
//    }
//}
