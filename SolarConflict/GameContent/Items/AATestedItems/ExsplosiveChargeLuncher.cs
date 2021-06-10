//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.Temp
//{
//    class ExsplosiveChargeLuncher
//    {
//        public static Item Make() //TODO: maybe needs ammo,
//        {
//            ItemProfile profile = ItemCommon.CommonProfile();
//            profile.Name = "ChargeLauncher";
//            profile.DescriptionText = "Shoots a exsplosive charge that needs to take damage to detonate";
//            profile.IconTextureID = "spikeball";
//            profile.IsConsumed = false;            
//            profile.IsActivatable = true;
//            profile.SlotType = SlotType.Weapon;
//            profile.ItemSize = SizeType.Small;

//            Item item = new Item(profile);

//            EmitterCallerSystem mainGun = new EmitterCallerSystem(ControlSignals.None, typeof(DamageActivatedBomb).Name); //add Nuke Ammo
//            mainGun.CooldownTime = 60;
//            mainGun.velocity = Vector2.UnitX * 15f;
//            mainGun.ActivationCheck.AddCost(MeterType.Energy, 3); //remove            
//            mainGun.refVelocityMult = 1;
//            mainGun.SelfImpactSpec = new CollisionSpec();
//            mainGun.SelfImpactSpec.Force = 0.1f;

//            item.System = mainGun; ;

//            return item;
//        }
//    }
//}
