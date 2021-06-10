//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace SolarConflict.GameContent.Items
//{
//    class NuclearLauncherItem
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemCommon.CommonProfile();
//            profile.DescriptionText = "Nuclear Launcher\nLaunches a nuke!";
//            profile.IconTextureID = "nuclear"; //Fix?? 
//            profile.IsConsumed = false;            
//            profile.IsActivatable = true;
//            profile.SlotType = SlotType.Weapon;            
           
//            Item item = new Item(profile);

//            EmitterCallerSystem mainGun = new EmitterCallerSystem(ControlSignals.None, "NuclearMissile"); //add Nuke Ammo
//            mainGun.CooldownTime = 30;
//            mainGun.velocity = Vector2.UnitX * 10f;
//            mainGun.ActivationCheck.AddCost(MeterType.Energy, 3); //remove            
//            mainGun.refVelocityMult = 1;
//            mainGun.SelfImpactSpec = new CollisionSpec();
//            mainGun.SelfImpactSpec.Force = 0.1f;

//            item.System = mainGun; ;

//            return item;
//        }
//    }
//}
