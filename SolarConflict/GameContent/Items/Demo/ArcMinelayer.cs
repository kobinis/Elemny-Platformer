//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items {
//    class ArcMinelayer {
//        public static Item Make() {
//            WeaponData weaponData = new WeaponData("Arc Minelayer");
//            weaponData.ItemData.Description = "Deploys high-explosive mines in an arc";
//            weaponData.ItemData.IconID = "KineticMineLauncher";
//            weaponData.ItemData.EquippedTextureId = "item3";
//            weaponData.ItemData.Level = 1;
//            weaponData.ShotEmitterID = typeof(MineArc).Name;
//            weaponData.ShotSpeed = 8; //5
//            weaponData.ItemData.BuyPrice = 1000;
//            weaponData.Cooldown = Utility.Frames(4f);
            
//            Item item = WeaponQuickStart.Make(weaponData);
//            SystemHolder system = new SystemHolder(item.System, Vector2.Zero, 0);
//            item.System = system;
//            return item;
//        }
//    }
//}
