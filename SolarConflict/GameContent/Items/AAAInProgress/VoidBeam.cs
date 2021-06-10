using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.AAAInProgress
{
    class VoidBeam
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Void Beam");
            weaponData.ItemData.IconID = "VoidTrident";
            weaponData.ItemData.EquippedTextureId = "VoidTrident";
            weaponData.ItemData.Level = 9;
            weaponData.ItemData.SecounderyIconID = null;


            weaponData.Range = 5000;
            weaponData.ShotSpeed = 5000;
            weaponData.ShotLifetime = 1;
            weaponData.Cooldown = 1;
            weaponData.ActiveTime = 1;            
            weaponData.ShotEmitterID = "Beam1";
            weaponData.EnergyCost = 1;
            weaponData.EffectEmitterID = null;
            //2 weaponData.IsTurreted = false;
            weaponData.ItemData.BuyPrice = 200;
            weaponData.KickbackForce = 0f;
            weaponData.SoundEffectEmitterID = null;

             weaponData.ActivationEmitterID = "VoidBeamFlash";
            //TODO: speed mult = 0;

            Item item = WeaponQuickStart.Make(weaponData, true);
            item.Profile.Category |= ItemCategory.Final;
            item.Profile.Category |= ItemCategory.NonAI;
            return item;
        }
    }
}
