//using SolarConflict.Framework.Agents.Systems.EmitterCallers;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;

//namespace SolarConflict.GameContent.Items.AAAInProgress
//{
//    class VoidBeam2
//    {
//        public static Item Make()
//        {
//            ItemData itemData = new ItemData("Void Beam!!!", 11, "VoidSiegegun", "VoidSiegegun");
          
//            itemData.SlotType = SlotType.Weapon | SlotType.Turret;
           
//            Item item = ItemQuickStart.Make(itemData);
//            int maxRange = 1000;
//            BeamSystem system = new BeamSystem();
//            system.EmitterID = "Beam1";
//            system.EffectEmitterID = "VoidBeamFlash";
//            item.System = system;
//            item.Profile.EquippedTextureScale = 1;
//            item.Profile.MaximalRange = maxRange;
//            return item;
//        }
//    }
//}
