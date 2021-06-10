using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using XnaUtils;
using XnaUtils.Graphics;
using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict
{
    // TODO: ?make ItemSlotsContainer derived  from list
    [Serializable]
    public class ItemSlotsContainer : IEnumerable<ItemSlot>
    {        
        #region Constructors
        public ItemSlotsContainer()
        {
            _itemSlots = new List<ItemSlot>();
        }
        #endregion
        #region Fields
        private List<ItemSlot> _itemSlots;        
        #endregion
        #region Properties
        public int AgentSlotsCount { get; private set; }
        /// <summary>
        /// The total amount of slots (basic and normal)
        /// </summary>
        public int Count
        {
            get { return _itemSlots.Count; }
        }
        public int BasicSlotsCount
        {
            get { return _itemSlots.Count - AgentSlotsCount; }
        }

        public ItemSlot this[int i]
        {
            get
            {    
                return _itemSlots[i];
            }
            set
            {
                _itemSlots[i] = value;
            }
        }
        #endregion
        #region Public Methods
        public ItemSlot AddAgentSlot(SlotType type, SizeType size, Vector2 position, float rotation, ControlSignals activationSignal = ControlSignals.None, Vector2? displayPos = null)
        {
            Debug.Assert(size <= SizeType.Gigantic);
            Debug.Assert(_itemSlots.Count <= AgentSlotsCount, "AgentSlots must be added before basicSlots");            
            AgentSlotsCount++;
            rotation = MathHelper.ToRadians(rotation);
            ItemSlot itemSlot = new ItemSlot(type,size,position, rotation, activationSignal, displayPos);
            _itemSlots.Add(itemSlot);
            return itemSlot;
        }

        public void AddAgentSlot(ItemSlot itemSlot)
        {
            AgentSlotsCount++;
            _itemSlots.Add(itemSlot);
        }

        public ItemSlot AddBasicSlot(SlotType type, SizeType size, ControlSignals activationSignal = ControlSignals.None)
        {
            Debug.Assert(size <= SizeType.Gigantic);
            ItemSlot itemSlot = new ItemSlot(type, size, Vector2.Zero, 0, activationSignal);
            _itemSlots.Add(itemSlot);            
            return itemSlot;
        }

        public IEnumerator<ItemSlot> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
                yield return this[i];
        }

        public ItemSlotsContainer GetWorkingCopy()
        {
            ItemSlotsContainer clone = (ItemSlotsContainer)MemberwiseClone();
            clone._itemSlots = new List<ItemSlot>();
            for (int i = 0; i < this._itemSlots.Count; i++)
			{
                clone._itemSlots.Add(_itemSlots[i].GetWorkingCopy()); //Check
			}
            return clone;
        }



        public bool EquipItem(string itemID, bool isEmpty = true)
        {
            Item item = ContentBank.Inst.GetItem(itemID, true);
            return EquipItem(item, isEmpty);
        }

        /// <summary>
        /// Finds a place and equip an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isEmpty"></param>
        /// <returns></returns>
        public bool EquipItem(Item item, bool isEmpty = true)
        {
            int? index = FindItemSlot(item, isEmpty);
            if(index.HasValue)
            {
                this[index.Value].EquipItem(item);
                item.IsActive = false;
                return true;
            }
            return false;
        }

        public int? FindItem(string itemID)
        {
            int? ans = null;
            for (int i = 0; i < Count; i++)
            {
                if (_itemSlots[i].Item != null && _itemSlots[i].Item.ID == itemID)
                    return i;
            }
            return ans;
        }

        //public void Illuminate(IEnumerable<PointLight> lights) {
        //    for (int i = 0; i < AgentSlotsCount; i++)
        //        _itemSlots[i].Illuminate(lights);
        //}
        #endregion

        #region Update/Draw

        public void UpdateBasicSlots(GameEngine gameEngine, Agent agent)
        {
            for (int i = AgentSlotsCount; i < _itemSlots.Count; i++)            
                _itemSlots[i].Update(gameEngine, agent);            
        }

        public void UpdateBodySlots(GameEngine gameEngine, Agent agent)
        {            
            int randomStart = gameEngine.Rand.Next(AgentSlotsCount);
            int direction = gameEngine.Rand.Next(2);
            if (direction == 0)
            {
                for (int i = 0; i < AgentSlotsCount; i++)
                {
                    int index = (i + randomStart) % AgentSlotsCount;
                    _itemSlots[index].Update(gameEngine, agent);
                }
            }
            else
            {
                for (int i = AgentSlotsCount-1; i >= 0; i--)
                {
                    int index = (i + randomStart) % AgentSlotsCount;
                    _itemSlots[index].Update(gameEngine, agent);
                }
            }
        }

        public void Draw(Camera camera, Agent agent, DrawType drawType = DrawType.Alpha)
        {
            for (int i = 0; i < AgentSlotsCount; i++)            
                _itemSlots[i].Draw(camera, agent, drawType);
            
        }

        internal void DrawInGUI(SpriteBatch sb, Vector2 pos, float scale, float rotation)
        {
            for (int i = 0; i < AgentSlotsCount; i++)
                _itemSlots[i].DrawInGui(sb, pos, scale, rotation);
        }

        public void ClearItems(string ID = null)
        {
            foreach (var slot in _itemSlots)
            {
                if(ID == null || (slot.Item != null && slot.Item.ID == ID))
                    slot.Item = null;
            }
            
        }        
        #endregion

        public List<ItemSlot> GetItemSlotsList()
        {
            return _itemSlots;
        }

        public int GetSlotNumber(SlotType slotType)
        {
            int count = 0;
            foreach (var item in _itemSlots)
            {
                if((item.Type & slotType) > 0)
                {
                    count++;
                }
            }
            return count;
        }

        public int? FindItemSlot(Item item, bool isEmpty)
        {
            int? ans = null;
            double maxScore = 0;
            for (int i = 0; i < Count; i++)
            {
                double score = CalcScore(_itemSlots[i], item);
                if (score > maxScore && (!isEmpty || _itemSlots[i].Item == null))
                {
                    ans = i;
                    maxScore = score;
                }
            }
            return ans;
        }

        //Score: Cannot Equip, Inventory, Can Equip But will Downgrade, The The same, Will Improve, Place in Empty, Place in the Least good Empty (Utils)
        public static double CalcScore(ItemSlot slot, Item item)
        {
            double score = 10000;
            if(item != null && slot.CanEquip(item))
            {
                if ((slot.Type & SlotType.Weapon) > 0)
                    score -= 4;
                if ((slot.Type & SlotType.Turret) > 0)
                    score -= 4;
                if ((slot.Type & SlotType.MainEngine) > 0)
                    score -= 3;
                if ((slot.Type & SlotType.Engine) > 0)
                    score -= 2;
                if ((slot.Type & SlotType.Utility) > 0)
                    score -= 1;
                score -= slot.Position.Length() / 10000;
                if (slot.Item != null)
                {
                    score -= 500;
                    score -= slot.Item.Level * 5;
                    if (slot.Item.SlotType != item.SlotType)
                        score -= 2;
                }
                return Math.Max(score, 1);
            }
            return 0;
        }           

    }
}
