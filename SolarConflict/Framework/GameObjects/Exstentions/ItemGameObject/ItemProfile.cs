using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;

using XnaUtils.Graphics;
using SolarConflict.Framework.Utils;

namespace SolarConflict
{
    /// <summary>
    /// ItemProfile - Hold data common to all instances of the same item type
    /// </summary>
    [Serializable]
    public class ItemProfile : ICloneable
    {
        static Color[] levelColor = new Color[]
        {
            Color.Gray,                 // level 0
            new Color(20,255,0),
            new Color(0,112,221),
            new Color(163, 53, 238),    // level 3
            Color.Orange,
            Color.Gold,
            Color.Pink,                  // level 6
            Color.CadetBlue,
            Color.Silver,
            Color.BlanchedAlmond,
            Color.Red,
            Color.Red,
            Color.Red,
            Color.Red,
            Color.Red,
            Color.Red,
            Color.Red,
            Color.Red,
        };

        public const int ParentPickupCooldown = 45;

        /// <summary>
        /// Id - used to identify the object in the item bank and the emitter bank, don't touch it!
        /// </summary>
        public string Id;
        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name;
        /// <summary>
        /// Description of the item - what it dose/ what it is used for
        /// </summary>
        public string DescriptionText; //change name to description

        public string StatsText;

        public String NamePrefix;//TODO: remove
        public string DescriptionSuffix;

        /// <summary>
        /// Flavour text
        /// </summary>
        public string FlavourText;
        /// <summary>
        /// The slots the item can be equipped in
        /// </summary>
        public SlotType SlotType;
        /// <summary>
        /// The type of the item e.g. gun, engine, shield etc. Mainly used for determine if the item can go in a slot
        /// </summary>
        public ItemCategory Category;
        /// <summary>
        /// Is the item consumed when used
        /// </summary>
        public bool IsConsumed;
        /// <summary>
        /// Will the item get an activation command when it's item slot is activated, if false, item is passive
        /// </summary>
        public bool IsActivatable;
        /// <summary>
        /// Works from inventory
        /// </summary>
        public bool IsWorkingInInventory;       
        /// <summary>
        /// The maximum times an item can be stacked
        /// </summary>
        public int MaxStack;
        /// <summary>Time the item can survive in space (timer reset if it's picked up)</summary>
        public int MaxLifetime;
        /// <summary>
        /// The size of the object in item slot - used to determine if an item can be equipped in an item slot
        /// </summary>
        public SizeType ItemSize;
        /// <summary>
        ///  if true item will break cloaking on activation
        /// </summary>
        public bool BreaksCloaking;
        /// <summary>
        /// The level of the item (0-...)
        /// </summary>
        public int Level; 
        /// <summary>
        /// If true, item will respawn with the ship (if it is respawned), otherwise it's dropped on death
        /// </summary>
        public bool IsRetainedOnDeath;

        public bool IsShownOnHUD;
               
        public float BuyPrice;
        public float SellPrice;

        public ItemCategory AmmoType;


        //Display
        public Sprite IconSecondarySprite;
        public Sprite IconSprite;
        public Color TextureColor = Color.White;
        public float TextureScale;
        public float DisplayRotation;
        public Color BackgroundColor;
        //public Sprite BackgroundSprite;
        public float BackgroundScale;
        public Sprite EquippedSprite;
        public float EquippedTextureScale;

        //Collected Emitter         
        public IEmitter CollectedEmitter;

        //Ammo:
        public IEmitter AmmoEmitter;

        public CraftingStationType CraftingStationType { get; set; }

        //For AI
        public float MaximalRange = 0;

        public bool IsQuickUse
        {
            get { return IsActivatable & IsWorkingInInventory; }
        }

        public CollisionSpec CollisionInfo;
        


        public ItemProfile()
        {
            BreaksCloaking = false;
            Level = 11;
            MaxLifetime = Utility.Frames(60f * 30f);
            MaxLifetime = Utility.Frames(5f); // TEMP
            //Item = Sprite.Get("glow128");
            CollisionInfo = CollisionSpec.SpecEmpty;
        }

        public object Clone() {
            return MemberwiseClone();
        }

        public string IconTextureID
        {
            get { return IconSprite.ID; }
            set { IconSprite = Sprite.Get(value); }
        }

        public string EquippedTextureID
        {
            get { return EquippedSprite.ID; }
            set { EquippedSprite = Sprite.Get(value); }
        }

        public Color GetColor()
        {
            int level = Level;
            return levelColor[Math.Min(level, levelColor.Count() - 1)];
        }

        public static Color GetLevelColor(int level)
        {
            if (level >= 11)
                return Color.Red;
            
            return levelColor[level];
        }

        public ItemProfile GetWorkingCopy()
        {
            ItemProfile clone = MemberwiseClone() as ItemProfile;
            return clone;
        }
    }


}
