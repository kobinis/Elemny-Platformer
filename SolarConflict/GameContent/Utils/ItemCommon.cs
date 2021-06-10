using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict
{
    class ItemCommon
    {
        //TODO: remove
        public static ItemProfile CommonProfile()
        {
            ItemProfile profile = new ItemProfile();
            //Text;
            profile.BackgroundColor = new Color(255, 255, 255, 50);
            profile.MaxStack = 1;
            profile.IsConsumed = false;
            profile.IsActivatable = true;            
            profile.SlotType = SlotType.None;
            profile.ItemSize = 0;
            //profile.Texture = ActivityManger.
            profile.TextureScale = 0.5f;
            profile.DisplayRotation = 0;
            //  profile.BackgroundTextureProxy = Sprite.Get("glow128"); // change to white glow
            profile.BackgroundScale = 0.5f;
            profile.EquippedSprite = null;
            profile.EquippedTextureScale = 1;
            profile.CollectedEmitter = ContentBank.Inst.GetEmitter("EmitterPickupFx");
            profile.Level = 11;
            return profile;
        }

    }
}
