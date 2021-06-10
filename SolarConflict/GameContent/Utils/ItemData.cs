using Microsoft.Xna.Framework;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Utils
{
    public struct ItemData
    {
        public string Name { get; set; }
        public string IconID { get; set; }
        public Color TextureColor { get; set; }
        public string SecounderyIconID { get; set; }
        public int Level { get; set; }
        public SlotType SlotType { get; set; }
        public ItemCategory Category { get; set; }
        public string EquippedTextureId { get; set; }
        public int BuyPrice { get; set; }
        public float SellRatio { get; set; }
        public SizeType Size { get; set; }
        public bool BreaksClocking { get; set; }
        public int MaxStack { get; set; }
        public bool IsRatiendOnDeath { get; set; }
        public bool IsWorkingFromInventory { get; set; }
        public CraftingStationType CraftingStationType { get; set; }
        
        public ItemData(string name, int level = 11, string iconID = null, string equippedTextureId = null, string secounderyIconID = null)
        {
            Name = name;
            IconID = iconID;
            Level = level;
            EquippedTextureId = equippedTextureId;
            TextureColor = Color.White;
            SecounderyIconID = secounderyIconID;
            BuyPrice = 0;
            SellRatio = 0.8f;
            Size = SizeType.Small;
            BreaksClocking = false;
            SlotType = SlotType.None;
            MaxStack = 1;
            Category = ItemCategory.None;
            IsRatiendOnDeath = false;
            CraftingStationType = CraftingStationType.None;
            IsWorkingFromInventory = false;
        }
    }

}
