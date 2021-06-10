//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.NewContent.Projectiles;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Projectiles;
//using SolarConflict.GameContent.Utils;

//namespace SolarConflict.GameContent.Items
//{
//    class TestItem
//    {
//        public static Item Make()
//        {            
//            WeaponData weaponData = new WeaponData("Test Item");            
//            //weaponData.ItemData.Description = "Test your stuff";
//            weaponData.ItemData.IconID = "replace";
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 11;
//            weaponData.ShotEmitterID = "MiniMissile";
//            weaponData.Cooldown = 60;
//            //weaponData.IsTurreted = true;
//            weaponData.ShotSpeed = 20;
//            Item item = WeaponQuickStart.Make(weaponData);
//            item.Profile.SlotType = SlotType.Utility | SlotType.Turret;
//            return item;
//        }
//    }
//}
