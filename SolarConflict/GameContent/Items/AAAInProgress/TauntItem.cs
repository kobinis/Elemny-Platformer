using SolarConflict.Framework.Emitters;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items
{
    class TauntItem
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Taunt");
            weaponData.ItemData.IconID = "attention";
            weaponData.ItemData.EquippedTextureId = null;
            weaponData.ItemData.Level = 4;
            weaponData.ItemData.SecounderyIconID = null;

            weaponData.Range = 3000;
            weaponData.ShotSpeed = 0;
            weaponData.ShotLifetime = 10;
            weaponData.Cooldown = 10;
            weaponData.ActiveTime = 1;
            weaponData.ShotEmitter = new TauntEmitter(weaponData.Range);
            weaponData.EnergyCost = 0;
            //2 weaponData.IsTurreted = false;
            weaponData.ItemData.BuyPrice = 1000;
            weaponData.KickbackForce = 0;
            weaponData.SoundEffectEmitterID = null;
            weaponData.EffectEmitterID = null;

            // weaponData.EffectEmitterID = "EmitterFxSmoke";
            //TODO: speed mult = 0;

            Item item = WeaponQuickStart.Make(weaponData);
            item.Profile.Category |= ItemCategory.NonAI;
            item.Profile.SlotType = SlotType.Utility;
            return item;
        }

    }
}
