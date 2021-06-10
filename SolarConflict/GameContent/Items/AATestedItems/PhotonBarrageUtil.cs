using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class PhotonBarrageUtil
    {
        public static Item Make() //TODO: change to photon torpedo, needs energy
        {
            Item item = PhotonBarrageWeapon1.Make();
            item.Profile.Level = 9;
            item.Profile.IconSecondarySprite = Sprite.Get("lv9");
            item.Profile.SlotType |= SlotType.Utility;
            return item;
        }
    }
}
