using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Temp
{
    class TimeBombLauncherItem
    {
        //Boss item - boss that shoots bombs that needs to be dstroyed
        public static Item Make() //TODO: maybe needs ammo,
        {
            //WeaponData data = new WeaponData("Time Bomb Launcher");

            //data
            ItemProfile profile = ItemCommon.CommonProfile();
            profile.Name = "Time Bomb Launcher";
            profile.DescriptionText = "A bomb that implodes after 10 sec";
            profile.IconTextureID = "spikeball";
            profile.IsConsumed = false;            
            profile.IsActivatable = true;
            profile.SlotType = SlotType.Weapon | SlotType.Turret;
            profile.ItemSize = SizeType.Small;
            profile.Level = 9;
            profile.BuyPrice = 19000;
            profile.SellPrice = 9000;

            Item item = new Item(profile);

            EmitterCallerSystem mainGun = new EmitterCallerSystem(ControlSignals.None, typeof(TimeBomb).Name); //add Nuke Ammo
            mainGun.CooldownTime = 60 * 30;
            mainGun.velocity = Vector2.UnitX * 5f;
            mainGun.ActivationCheck.AddCost(MeterType.Energy, 3); //remove            
            mainGun.refVelocityMult = 1;
            mainGun.SelfImpactSpec = new CollisionSpec();
            mainGun.SelfImpactSpec.Force = 0.1f;

            item.System = mainGun; ;

            return item;
        }

        public ProjectileProfile MakeShot()
        {
            return null;
        }
    }
}
