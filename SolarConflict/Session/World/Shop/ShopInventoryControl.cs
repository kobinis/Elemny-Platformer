using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using XnaUtils.Input;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework.Utils;
using XnaUtils;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.Framework.GUI;

namespace SolarConflict
{
    [Serializable]
    public class ShopInventoryControl : GuiControl, IInventoryControl
    {

        public Inventory Inventory { get { return _inventory; } }
        public ItemControl[] ItemControls { get { return _itemControls; } }

        ItemControl _cursorItem;
        Inventory _inventory;
        ItemControl[] _itemControls;        
        GameObject _playerShip;
        private Dictionary<string, float> _itemPriceMultiplier;
        /// <summary>{itemID -> netAmountBought}, with negative amounts indicating net sales. Used for refunds</summary>
        private Dictionary<string, int> _transaction;
        /// <summary>These items are infinitely renewed by the shop.</summary>
        private HashSet<string> _renewables;

        public ShopInventoryControl(Inventory inventory, ItemControl cursorItem, GuiManager guiHolder, GameObject playerShip, Dictionary<string, float> itemPriceMultiplier = null, float itemSize = 70)
            : base()
        {
            _transaction = new Dictionary<string, int>();
            _itemPriceMultiplier = itemPriceMultiplier; //maybe copy values
            _playerShip = playerShip;            
            _inventory = inventory;
            _cursorItem = cursorItem;

            _renewables = _inventory.Items.Where(i => i != null).Select(i => i.ID).ToSet();

            MakeGui(guiHolder, (int)itemSize);
            
            /*  this.ControlColor = new Color(100, 100, 240, 255);
              this.PressedControlColor = ControlColor;
              this.CursorOverlColor = ControlColor;*/
        }

        private void MakeGui(GuiManager guiHolder, int itemSize)
        {
            const int rad = 4; //remove
            const int itemNumInLine = rad * 2 + 1;            
            const int space = 15;

            int numberOfLines;

            int w = (itemNumInLine) * (itemSize + space) + 20;
            int h = (int)(Math.Ceiling(_inventory.Size / (float)itemNumInLine) * (itemSize + space) + 20); //maybe with Min(maxH) and add scrolling

            numberOfLines = (int)Math.Ceiling(_inventory.Size / (float)itemNumInLine) - 1;

            Width = w;
            Height = h;
            _itemControls = new ItemControl[_inventory.Size];

            for (int i = 0; i < _inventory.Size; i++) //incpsulate in inventory Controller
            {
                float dx = i % itemNumInLine - (itemNumInLine - 1) * 0.5f;
                float dy = i / itemNumInLine - numberOfLines * 0.5f;
                Vector2 position = new Vector2((itemSize + space) * dx, (itemSize + space) * dy);
                Item item = _inventory.GetItem(i);
                float priceMult = 1;

                if(_itemPriceMultiplier != null && item != null && _itemPriceMultiplier.ContainsKey(item.ID))
                {
                    priceMult = _itemPriceMultiplier[item.ID];
                }

                ItemControl itemControl = new ItemControl(item, position, new Vector2(itemSize), priceMult);
                itemControl.ControlColor = Color.DarkGoldenrod;
                
                itemControl.CursorOn += guiHolder.ToolTipHandler;
                itemControl.Index = i;
                itemControl.CursorOn += ItemGuiAction;
                //itemControl.DepthOffset = 0.4f;
                _itemControls[i] = itemControl;
                AddChild(itemControl);
            }
        }




        public float GetItemPriceMult(Item item)
        {            
            if(item == null || _itemPriceMultiplier == null || !_itemPriceMultiplier.ContainsKey(item.ID))
            {
                return 1;
            }
            return _itemPriceMultiplier[item.ID];
        }

        public override void UpdateLogic(global::XnaUtils.InputState inputState)
        {
            for (int i = 0; i < _inventory.Size; i++) //incpsulate in inventory Controller
            {
                Item item = _inventory.GetItem(i);
                _itemControls[i].SetItem(item, GetItemPriceMult(item));

                if (item != null)
                {
                    
                    _itemControls[i].IsDisabled = item.GetStackBuyPrice( GetItemPriceMult(item)) > _playerShip?.GetMeter(MeterType.Money).Value;
                    
                }                 
            }
        }

        private void ItemGuiAction(GuiControl source, CursorInfo cursorLocation) //copy terraria bhiviour
        {
            _cursorItem.Data = (source as ItemControl).Item;
            ItemControl itemControl = (ItemControl)source;
            if (cursorLocation.OnPressLeft)// take on press/ putt on relese  || cursorLocation.OnReleaseLeft) //on leftClick 
            {
                LeftClick(itemControl);
            }

            if (cursorLocation.OnPressRight)
            {

                RightClick(itemControl);
            }
        }

        //move it as a static to Item
        public bool CanStack(Item target, Item source)
        {
            if (target == null)
                return true;
            if (source == null)
                return false;
            return target.Stack < target.MaxStack && target.ID == source.ID; //change
        }

        private void TryBuy(ItemControl itemControl) {
            // Can we find the item? Can we pick it up (nothing held or can stack with held item)?
            if (itemControl.Item != null && CanStack(_cursorItem.Item, itemControl.Item)) {                
                var itemID = itemControl.Item.ID;
                var money = _playerShip.GetMeterValue(MeterType.Money);

                // Are we reclaiming an item we tried to sell?
                var buyPrice = (int)Math.Round(itemControl.Item.Profile.BuyPrice * GetItemPriceMult(itemControl.Item));
                var sellPrice = (int)Math.Round(itemControl.Item.Profile.SellPrice);
                var price = _transaction.Get(itemID, 0) < 0 ? sellPrice : buyPrice;

                // Can we afford it?
                if (itemControl.Item.Stack > 0 && CanStack(_cursorItem.Item, itemControl.Item) && money >= price) {

                    if (_cursorItem.Item != null)
                        _cursorItem.Item.Stack++;
                    else {
                        _cursorItem.Item = itemControl.Item.GetWorkingCopy();
                        _cursorItem.Item.Stack = 1;
                    }

                    ActivityManager.Inst.SoundEngine.AddSoundToQue("hit1", 1);

                    // Is this from our infinitely renewable stock?
                    if (!_renewables.Contains(itemID)) {
                        // Nope. Remove from inventory
                        _inventory.RemoveItem(itemControl.Item.ID);

                        if (itemControl.Item.Stack <= 0)
                            itemControl.Item = null;
                    }

                    // Execute transaction
                    if (!_transaction.ContainsKey(itemID))
                        _transaction[itemID] = 0;
                    ++_transaction[itemID];
                    //Utility.Log($"Bought {itemID} for {price}{(_transaction.Get(itemID, 0) < 1 ? " (refunded sale)" : "")}, {_transaction[itemID]} in cart (buy/sell {buyPrice}/{sellPrice})");

                    _playerShip.SetMeterValue(MeterType.Money, money - price);
                }
            }
        }

        private void LeftClick(ItemControl itemControl)
        {            
            float money = _playerShip.GetMeterValue(MeterType.Money);            
            
            if (itemControl.Item != null && (_cursorItem.Item == null || (_cursorItem.Item.ID != itemControl.Item.ID))) {
                // Trying to buy                
                TryBuy(itemControl);
            }
            else {
                // Trying to sell
                if (_cursorItem?.Item == null)
                    return;
                
                // Are we actually holding something to sell, or is this one of those Jedi mind trick deals?
                if (_cursorItem?.Item?.Stack > 0) {
                    var itemID = _cursorItem.Item.ID;
                    var buyPrice = (int)Math.Round(_cursorItem.Item.Profile.BuyPrice * GetItemPriceMult(_cursorItem.Item));
                    var sellPrice = (int)Math.Round(_cursorItem.Item.Profile.SellPrice);

                    // Are we just selling, or is are we returning items from our cart? Or both?                    
                    var amountGiven = _cursorItem.Item.Stack;
                    var amountRefunded = Math.Min(amountGiven, Math.Max(0, _transaction.Get(itemID, 0)));

                    var price = amountRefunded * buyPrice + (amountGiven - amountRefunded) * sellPrice;

                    // Do we have an infinite number of these? If so, don't change the storefront, just toss it in the back with the rest of them
                    if (!_renewables.Contains(itemID)) {
                        // We don't, add it to the storefront
                        _inventory.AddItem(_cursorItem.Item);
                        //_inventory.SetItem(itemControl.Index, _cursorItem.Item);
                    }

                    // Execute transaction
                    if (!_transaction.ContainsKey(itemID))
                        _transaction[itemID] = 0;
                    _transaction[itemID] -= amountGiven;
                    //Utility.Log($"Sold {amountGiven} stacks of {itemID} ({amountRefunded} refunded) for S${price}, {_transaction[itemID]} in cart (buy/sell {buyPrice}/{sellPrice})");

                    _playerShip.SetMeterValue(MeterType.Money, money + price);
                    _cursorItem.Item = null;
                }
            }       
        }

        private void RightClick(ItemControl itemControl)
        {
            TryBuy(itemControl);
        }

    }
}
