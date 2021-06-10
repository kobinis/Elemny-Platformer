//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    class MineSpreader
//    {
//        public static Item Make() {
//            return WeaponQuickStart.Make(MakeData());
//        }

//        public static WeaponData MakeData()
//        {
//            var weaponData = new WeaponData("Mine Spreader");
//            weaponData.ItemData.Description = "Lays a minefield in the direction it's pointed";
//            weaponData.ItemData.IconID = "replace";
//            weaponData.ItemData.EquippedTextureId = null;
//            weaponData.ItemData.Level = 9;
//            weaponData.ShotEmitterID = typeof(MineSpreaderEmitter).Name;
//            weaponData.Cooldown = 60*30;
//            weaponData.IsTurreted = true;
//            weaponData.ItemData.BuyPrice = 8350;
//            return weaponData;
//        }
//    }
//}
