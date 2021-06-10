using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;

using System;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict
{  
    class ItemQuickStart
    {
        const float GLOBAL_PRICE = 1f;

        public static Item Make(ItemData data)
        {
            return new Item(Profile(data));
        }

        public static ItemProfile Profile(ItemData data)
        {
            ItemProfile profile = Profile(data.Name, null, data.Level, data.IconID, data.EquippedTextureId, data.Size, data.BuyPrice, data.SellRatio, data.MaxStack);
            profile.SlotType = data.SlotType;            
            //if(data.SecounderyIconID == null && data.Level <= 10 && data.Level > 0)
            if(data.Level > 0 && data.Level <= 10)
                data.SecounderyIconID = "lv" + data.Level;
            profile.IconSecondarySprite = TextureBank.Inst.GetSprite(data.SecounderyIconID);
            profile.IsRetainedOnDeath = data.IsRatiendOnDeath;
            profile.BreaksCloaking = data.BreaksClocking;
            profile.Category = data.Category;
            profile.Level = data.Level;
            profile.CraftingStationType = data.CraftingStationType;
            profile.IsWorkingInInventory = data.IsWorkingFromInventory;
            return profile;
        }

        public static ItemProfile Profile(string name, string description, int level, string textureId, string equippedTextureId = null,
                                          SizeType size = SizeType.Small, int price = 350, float sellRatio = 0.25f, int maxStack = 1)
        {
            ItemProfile profile = new ItemProfile();
            profile.Name = name;
            profile.DescriptionText = description;
            profile.SlotType = SlotType.None;
            profile.IsConsumed = false;
            profile.IsActivatable = true;
            profile.IsShownOnHUD = false;            
            profile.MaxStack = maxStack;
            profile.ItemSize = size;
            profile.Level = level;                    
            profile.BuyPrice = price;            
            profile.SellPrice = (int)Math.Round(price * sellRatio);            
            //Display - possibly change to sprite?
            profile.IconTextureID = textureId;
            profile.TextureColor = Color.White;
         //   profile.BackgroundTextureProxy = Sprite.Get("glow128"); // maybe if legendary also change texture
            if(profile.IconSprite != null)
                profile.TextureScale = 60f / (float)Math.Max(profile.IconSprite.Width, profile.IconSprite.Height);
            profile.DisplayRotation = 0;
            profile.BackgroundColor = Color.White;
         //   profile.BackgroundScale = 20f / (float)profile.BackgroundTextureProxy.Width;
            profile.EquippedSprite = Sprite.Get(equippedTextureId);
            if (profile.EquippedSprite != null)
            {
                profile.EquippedTextureScale = 20f/ (float)profile.EquippedSprite.Height;
                //( (int)(profile.ItemSize + 2) * 20) / (float)profile.EquippedTexture.Height;
            }
            profile.CollectedEmitter = ContentBank.Inst.GetEmitter("EmitterPickupFx");
            profile.MaxStack = maxStack;
            
            return profile;
        }





        public static ItemProfile MakeAmmoProfile(string name, string description, string textureID)
        {
            ItemProfile profile = ItemQuickStart.Profile(name, description, 0, textureID, null);
            profile.SlotType = SlotType.Ammo;
            profile.ItemSize = SizeType.Small;            
            profile.IsActivatable = false;
            profile.BuyPrice = 50;
            profile.SellPrice = 50;
            profile.MaxStack = 30;
            return profile;
        }
    }
}
