using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.GUI;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using XnaUtils;
using XnaUtils.Input;
using XnaUtils.SimpleGui;

namespace SolarConflict
{
    
    public class CraftingControl :ScrollableGrid //Add Title and Icon, maybe craft to cursorItem ???
    {
        private CursorItemControl _cursorItem;
        private List<Inventory> _inventorys;
        private List<Agent> _agents;
        private GuiManager _guiManager;
        private bool _remakeGui;        
        private int _time = 0;
        public CraftingStationType _craftingStation;
        private Vector2 _itemSize;
        private bool _addCraftingStationsOnAgent;
        Dictionary<string, int> _itemCount;
        private SlotType _slotType;

        public CraftingControl(int sizeX, int sizeY, Vector2 binSize,List<Agent> agents, List<Inventory> inventory, CursorItemControl cursorItem, Vector2 inPosition, GuiManager guiManager , CraftingStationType craftingStation, bool addCraftingStationsOnAgent = true)
            : base(sizeX, sizeY, binSize)
        {
            _itemSize = binSize;
            this._inventorys = inventory;
            _agents = agents;
            this._guiManager = guiManager;
            Position = inPosition;
            _addCraftingStationsOnAgent = addCraftingStationsOnAgent;
          //  this.inventory = inventory;
            this._cursorItem = cursorItem;
            _craftingStation = craftingStation;
            _itemCount = new Dictionary<string, int>();
            Inventory.CountItems(_inventorys, _itemCount);
            _slotType = SlotType.All;
            MakeGui(_guiManager, _craftingStation, _addCraftingStationsOnAgent, _slotType);
        }
        
        private void MakeGui(GuiManager guiHolder, CraftingStationType craftingStation, bool addCraftingStationsOnAgent , SlotType slotType)
        {
            this.RemoveAllChildren();
           
            var recipeList = GetRecipeList(slotType, craftingStation, addCraftingStationsOnAgent);      
            for (int i = 0; i < recipeList.Count; i++) 
            {                               
                Recipe recipe = recipeList[i];
                CraftingRecipeControl itemControl = new CraftingRecipeControl(recipe, _itemCount, Vector2.Zero, _itemSize);                
                itemControl.ControlColor = CraftingRecipeControl._uncraftableItemColor;
                itemControl.CursorOn += guiHolder.ToolTipHandler;
                itemControl.Index = i;
                itemControl.CursorOn += CraftingHandler;
                itemControl.CursorOn += CursorOverHandler;
                AddChild(itemControl);
            }
        }

        public List<Recipe> GetRecipeList(SlotType slotType, bool addCraftingStationsOnAgent)
        {
            return GetRecipeList(slotType, _craftingStation, addCraftingStationsOnAgent);
        }

        public List<Recipe> GetRecipeList(SlotType slotType, CraftingStationType craftingStationType, bool addCraftingStationsOnAgent)
        {
            if (addCraftingStationsOnAgent)
            {
                craftingStationType |= GetCraftingStationType(_agents);
                if (_cursorItem.Item != null)
                    craftingStationType |= _cursorItem.Item.GetCraftingStationType();
            }
            List<Recipe> recipeList = ContentBank.Inst.GetAllRecipes(craftingStationType);
            List<Recipe> craftableRecipes = recipeList.Where(r => r.IsCraftable(_inventorys)).OrderByDescending(r => r.CraftedItem.ID).ToList();
            List<Recipe> uncraftable = recipeList.Where(r => !r.IsCraftable(_inventorys)).OrderByDescending(r => r.CraftedItem.ID).ToList();
            recipeList = craftableRecipes;
            recipeList.AddRange(uncraftable);
            if(slotType != SlotType.All)
                recipeList = recipeList.Where(r => (r.CraftedItem.SlotType & slotType) > 0 || (slotType == SlotType.None && r.CraftedItem.SlotType == SlotType.None)).ToList();
            return recipeList;
        }

        public void FilterBySlotType(SlotType slotType)
        {
            _slotType = slotType;
            MakeGui(_guiManager, _craftingStation, _addCraftingStationsOnAgent, _slotType);
            this.Update(InputState.EmptyState);
        }

        public CraftingStationType GetCraftingStationType(List<Agent> agents)
        {
            CraftingStationType craftingStation = CraftingStationType.None;
            foreach (var agent in agents)
            {
                craftingStation |= agent.GetCraftingStationType();                
            }
            return craftingStation;
        }

        private void CursorOverHandler(GuiControl source, CursorInfo cursorLocation)
        {
            _cursorItem.ItemUnderCursor = (source as CraftingRecipeControl).CraftingRecipe.CraftedItem;
        }

        public override void UpdateLogic(global::XnaUtils.InputState inputState)
        {
            base.UpdateLogic(inputState);
            Inventory.CountItems(_inventorys, _itemCount);
            _time++;
            if (_remakeGui && !inputState.Cursor.IsPressedLeft && !inputState.Cursor.IsPressedRight)
            {
                 MakeGui(_guiManager, _craftingStation, _addCraftingStationsOnAgent, _slotType); //Don't 
                _remakeGui = false;
                Update(InputState.EmptyState);
            }

            foreach (var item in GetChildren())
            {
                CraftingRecipeControl control = item as CraftingRecipeControl;
                ItemFilter filter = _cursorItem.Filter;
                if (GameplaySettings.IsFilter && filter != null && control.CraftingRecipe.CraftedItem != null && filter(control.CraftingRecipe.CraftedItem))
                    control.HighlightColor = Color.White;
                else
                    control.HighlightColor = Color.Transparent;
            }
        }


        const int ITEM_CRAFTED = 0;
        const int MATERIALS_MISSING = 1;
        const int NO_ROOM = 2;

        public void CraftingHandler(GuiControl recipieControl, CursorInfo cursorInfo)
        {
            Agent player = _cursorItem.GetData("Player") as Agent;
            CraftingRecipeControl control = recipieControl as CraftingRecipeControl;
            if (cursorInfo.OnPressLeft | cursorInfo.OnPressRight)
            {
                Recipe craftingRecipe = control.CraftingRecipe;
                int res = CraftItem(craftingRecipe);
                switch (res)
                {
                    case ITEM_CRAFTED:
                       // _remakeGui = true;
                        if(cursorInfo.OnPressRight && player != null && player.ItemSlotsContainer != null)
                        {
                            int? index = player.ItemSlotsContainer.FindItemSlot(_cursorItem.Item, false);
                            if(index.HasValue)
                            {
                                var tmpItem = player.ItemSlotsContainer[index.Value].Item;
                                player.ItemSlotsContainer[index.Value].Item = _cursorItem.Item;
                                _cursorItem.Item = tmpItem;
                            }
                            else
                            {

                            }

                        }
                        ItemCraftedFeedbak(control);
                        break;
                    case NO_ROOM:
                        ActivityManager.Inst.AddToast(UIElmentsTexts.InventoryIsFull, 80);
                        break;
                    case MATERIALS_MISSING:
                        ActivityManager.Inst.AddToast(Color.Red.ToTag("Missing materials"), 80);
                        break;
                }
                
            }

            //if (cursorInfo.IsPressedRight)
            //{
            //    int pressCounter = control.RightInputPressed;
            //    if (_time % Math.Max(10 - pressCounter / 4, 1) == 0)
            //    {
            //        Recipe craftingRecipe = control.CraftingRecipe;
            //        int res = CraftItem(craftingRecipe);
            //        switch (res)
            //        {
            //            case ITEM_CRAFTED:
            //                ItemCraftedFeedbak(control);
            //                break;
            //            case NO_ROOM:
            //                ActivityManager.Inst.AddToast(UIElmentsTexts.InventoryIsFull, 80);
            //                break;
            //            case MATERIALS_MISSING:
            //                ActivityManager.Inst.AddToast(Color.Red.ToTag("Missing materials"), 80);
            //                break;
            //        }
            //    }
            //}
           

            
            if (DebugUtils.Mode == ModeType.Debug && cursorInfo.OnPressRight) //DEBUG
            {
                Recipe craftingRecipe = control.CraftingRecipe;
                {
                    foreach (var item in craftingRecipe.CraftingCostList)
                    {
                        for (int i = 0; i < item.Amount; i++)
                        {
                            _inventorys[0]?.AddItem(item.ItemNeeded.Id);
                        }
                    }
                }
            }


        }



        private int CraftItem(Recipe craftingRecipe)
        {
            if(craftingRecipe.IsCraftable(_inventorys))
            {
                if (Item.CanStack(_cursorItem.Item, craftingRecipe.CraftedItem.ID, craftingRecipe.AmountRecived))
                {
                    Item craftedItem = craftingRecipe.Craft(null, _inventorys);                    
                    _cursorItem.AddItem(craftedItem);
                    return ITEM_CRAFTED;
                }
                else
                {
                    if(_inventorys[0].HasRoom(_cursorItem.Item.ID, _cursorItem.Item.Stack))
                    {
                        _inventorys[0].AddItem(_cursorItem.Item);
                        _cursorItem.Item = null;
                        Item craftedItem = craftingRecipe.Craft(null, _inventorys);
                        _cursorItem.AddItem(craftedItem);
                        return ITEM_CRAFTED;
                    }
                }
                return NO_ROOM;
            }
            return MATERIALS_MISSING;            
        }

        private void ItemCraftedFeedbak(CraftingRecipeControl control)
        {
            
            if((control.CraftingRecipe.CraftedItem.GetCraftingStationType() | _craftingStation) != _craftingStation)
                _remakeGui = true;
            control.blinkAlpha = 1;
            
            ActivityManager.Inst.AddToast(control.CraftingRecipe.CraftedItem.IconTag +" "+ control.CraftingRecipe.CraftedItem.Name + " Crafted", 60 * 2);
            ActivityManager.Inst.SoundEngine.AddSoundToQue("itemcrafted", 1);

            //Play sound, check blink
        }

        private void ItemNotCraftedFeedback()
        {
            ActivityManager.Inst.AddToast("Item not crafted!", 60 * 2, new Color(250,50,50,200));
        }
    }
}
