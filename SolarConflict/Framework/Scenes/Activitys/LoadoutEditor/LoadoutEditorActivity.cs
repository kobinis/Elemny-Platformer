using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.Scenes.Components.Editors
{
    public class LoadoutEditorActivity : Activity
    {
        private GuiManager gui;
        private CursorItemControl cursorItem;
        
        private List<Inventory> _inventories;
        private AgentSlotsControl _itemSlotCont;
        private Scene _scene;
        private GridControl _loadNamesSelect;
        private Agent _playerShip;
        private TextBoxControl _textBox;
        private string _loadoutName;
        private SlotType _currentSlotType;
        private string _loadoutPath;
     //   private GuiControl _filterItems;

        public LoadoutEditorActivity(Scene scene, Agent playerShip, string loadoutPath)
        {

            _loadoutPath = Consts.GetLoadoutPath( loadoutPath);
            this._playerShip = playerShip;
            _loadoutName = _playerShip.ID;
            this._scene = scene;
            gui = new GuiManager();      
            _inventories = new List<Inventory>();

            gui.Root = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
            cursorItem = new CursorItemControl( new Vector2(100 * 1.2f));
            cursorItem.Sprite = null;



            _currentSlotType = SlotType.Weapon;

            cursorItem.SetData("Player", _playerShip);
            if (cursorItem.Item != null)
            {
                //cursorItem.Item.Position = scene.FindPlayer().Position;
                cursorItem.Item.Position = scene.GameEngine.Camera.GetWorldPos(scene.InputState.Cursor.Position);
                

                    //_cursorItem.GetData("Player")
                scene.GameEngine.AddGameObject(cursorItem.Item); //maybe addli
            }

           
            CreateInventoryGUI(_playerShip, _inventories, _currentSlotType, cursorItem);
            

            IsPopup = true;
           
            //GuiControl exit = new GuiControl(Vector2.Zero, Vector2.One * 100);
            ////exit.Position = new Vector2(activityManager
            //guiHolder.root.AddChild(exit);
        }      

        public override void OnEnter(ActivityParameters parameters)
        {
            base.OnEnter(parameters);
            
            //_playerShip = parameters.DataParams["ship"] as Agent;
            activityLifetime = 0;
        }

        public override void OnResume(ActivityParameters parameters = null)
        {
            activityLifetime = 0;
        }

        

        private void CategorySelectHandler(GuiControl source, CursorInfo cursorLocation)
        {
            _currentSlotType = (SlotType)Enum.Parse(typeof(SlotType), source.UserData);          
            CreateInventoryGUI(_playerShip, _inventories, _currentSlotType, cursorItem);
        }

        public bool ItemFilter(Item item)
        {
            return true|| (item.Category & ItemCategory.Final) > 0;
        }

        private void CreateItemsTypesControl()
        {
            var itemCategories = Enum.GetValues(typeof(SlotType));
            // Create Input Select 
            GridControl itemCategoriesSelect = new GridControl(1, itemCategories.Length, new Vector2(200, 50));
            gui.Root.AddChild(itemCategoriesSelect);
            itemCategoriesSelect.Position = new Vector2(ActivityManager.ScreenRectangle.Width - itemCategoriesSelect.HalfSize.X - 10, itemCategoriesSelect.HalfSize.Y + 20);
            // Create Text Control for each category
            foreach (var item in itemCategories)
            {
                TextControl textControl = new TextControl(item.ToString(), Game1.font);
                textControl.IsShowFrame = true;
                textControl.Action += CategorySelectHandler;
                textControl.UserData = item.ToString();
                itemCategoriesSelect.AddChild(textControl);
                textControl.CursorOn += gui.ToolTipHandler;
            }
        }

        private static Inventory CreateInventory(SlotType type)
        {
            //var items = ContentBank.Inst.GetAllItemsCopied(type).OrderBy(item => item.Profile.ItemSize).ThenBy(item => item.BuyPrice);
            var items = ContentBank.Inst.GetAllItemsCopied(type).OrderBy(item => item.Name).ToList();
            if (type == SlotType.Turret)
            {
                items.RemoveAll(item => (item.SlotType & SlotType.Weapon) > 0);
            }
            //items.RemoveAll(item =>  (item.Category & ItemCategory.Final) == 0);
            Inventory inventory = new Inventory(items.Count());
            if (type != SlotType.All)
                items.OrderBy(item => item.Profile.AmmoType != ItemCategory.None).ThenBy(item => item.Name).ThenBy(item => item.Profile.ItemSize).Do(i => { i.SetStack(999); inventory.AddItem(i); });
            else
                items.Do(i => { i.SetStack(999); inventory.AddItem(i); });
            return inventory;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _scene.background.Draw(_scene.Camera);
            if (gui.Root != null)
            {
                gui.Draw();// change;
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                cursorItem.Draw(spriteBatch); //change it
                spriteBatch.End();
            }
        }
        int activityLifetime = 0;
        public override void Update(InputState inputState)
        {
            if (_scene.InputState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F11))
            {
                CreateInventoryGUI(_playerShip, _inventories, _currentSlotType, cursorItem);
                RefreshPlayerShip();
            }
            CursorInfo cursor = _scene.InputState.Cursor;
            cursorItem.Position = cursor.Position;
            gui.Update(_scene.InputState);

            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Delete))
            {
                cursorItem.Item = null;
            }


            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Delete))
                cursorItem.Item = null;

            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();

            activityLifetime++;
        }

        private void RefreshPlayerShip()
        {
            for (int i = 0; i < _playerShip.ItemSlotsContainer.Count; i++)
            {
                if(_playerShip.ItemSlotsContainer[i].Item != null)
                {
                    _playerShip.ItemSlotsContainer[i].Item = ContentBank.Inst.GetItem(_playerShip.ItemSlotsContainer[i].Item.ID, true);
                }
            }
            
        }



        public void CreateInventoryGUI(Agent playerShip, List<Inventory> inventoryList, SlotType slotType, CursorItemControl cursorItem)
        {
            _inventories.Clear();
            _inventories.Add(CreateInventory(slotType));

            _textBox = null;

            if (_playerShip != null)
            {
                _inventories.Add(_playerShip.GetInventory());
            }

            gui.Root = new GuiLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 20));



            if (playerShip != null && playerShip.ItemSlotsContainer != null)
            {
                _itemSlotCont = new AgentSlotsControl(playerShip, cursorItem, gui);
                gui.Root.AddChild(_itemSlotCont);
            }

            int count = 0;
            foreach (var inv in inventoryList)
            {
                InventoryControl inventoryControl;
                if (count == 0)
                {
                    inventoryControl = new InventoryControl(inv, cursorItem, Vector2.Zero, gui, lineNum: 5);
                    inventoryControl.TakeClone = true;
                    inventoryControl.ControlColor = new Color(100, 200, 255, 200);
                    inventoryControl.CursorOverColor = new Color(100, 200, 255, 200);
                }
                else
                {
                    inventoryControl = new InventoryControl(inv, cursorItem, Vector2.Zero, gui);
                }
                count++;
                inventoryControl.CursorOn += gui.ToolTipHandler;
                gui.Root.AddChild(inventoryControl);
            }

            CreateItemsTypesControl();

            var save = new TextControl("Save", Game1.menuFont);
            //save.Sprite = Sprite.Get("Btn_Button5-2_h");

            if (_itemSlotCont != null)
            {
                save.IsShowFrame = true;
                gui.Root.AddChild(save);
                save.Position = new Vector2(_itemSlotCont.Position.X - _itemSlotCont.HalfSize.X - save.HalfSize.X - 110, _itemSlotCont.Position.Y);
                save.Action += Save_Action;

                var load = new TextControl("Load", Game1.menuFont);
                load.IsShowFrame = true;
                gui.Root.AddChild(load);
                load.Action += Load_Action;
                load.Position = new Vector2(_itemSlotCont.Position.X - _itemSlotCont.HalfSize.X - load.HalfSize.X - 110,
               _itemSlotCont.Position.Y + save.HalfSize.Y + save.HalfSize.Y + 10);

                //_filterItems = new GuiControl(Vector2.Zero, Vector2.One * 20);
                //_filterItems.IsToggleable = true;
                //gui.Root.AddChild(_filterItems);
                //_filterItems.Position = new Vector2(_itemSlotCont.Position.X - _itemSlotCont.HalfSize.X - load.HalfSize.X - 110,
                //_itemSlotCont.Position.Y + save.HalfSize.Y + 90 + 10);
                

            }
            //  guiHolder.root.AddChild(cursorItem);                                   
        }

        #region Save Logic

        private void SaveHandler(GuiControl source, CursorInfo cursorLocation)
        {
            
            string saveName = _textBox.Text;
            AgentLoadout agentLoadout = new AgentLoadout(_playerShip);
            agentLoadout.ID = saveName;
            string path = string.Format("{0}/{1}/", _loadoutPath, _playerShip.ID);

            if (ContentBank.Inst.AddLoadout(agentLoadout))
            {
                _loadoutName = saveName;
                SaveLoadManager.Instance().Save(path, saveName  +".json", agentLoadout);
                gui.Root.RemoveChild(_textBox);
                _textBox = null;
                ActivityManager.Inst.AddToast("Saved", 60 * 5);
                ContentBank.Inst.AddLoadout(agentLoadout);
                //DebugStuff.LoadoutsCreatedThisSession.Add(agentLoadout.ID);
            }
            else
            {
                ActivityManager.Inst.AddToast("Can't save, an object by that name already exists", 60 * 10);
            }
        }

        private void Save_Action(GuiControl source, CursorInfo cursorLocation)
        {
            if (_loadNamesSelect == null && _textBox == null)
            {
                _textBox = new TextBoxControl(_loadoutName, Game1.font);
                _textBox.IsAutoResize = true;
                _textBox.IsShowFrame = true;
                _textBox.Action += SaveHandler;
                gui.Root.AddChild(_textBox);
                _textBox.Position = Vector2.One * 200 + _textBox.HalfSize;
            }
        }

        #endregion


        #region Load Logic

        private void Load_Action(GuiControl source, CursorInfo cursorLocation)
        {
            if (_loadNamesSelect == null && _textBox == null)
            {
                ShowFileNamesToLoad();
            }
        }

        private void ShowFileNamesToLoad()
        {
            string path = Path.Combine(_loadoutPath, _playerShip.ID); //TODO: change to path.combine

            if (Directory.Exists(path))
            {
                string[] fileNames = Directory.GetFiles(path);
                MakeInputSelect(fileNames);
            }
            else
            {
                MakeInputSelect(new string[0]);
            }
        }

        private void MakeInputSelect(string[] fileNames)
        {
            // Create Input Select 
            _loadNamesSelect = new GridControl(1, 10, new Vector2(200, 50));
            gui.Root.AddChild(_loadNamesSelect);
            _loadNamesSelect.Position = Vector2.One * 10 + _loadNamesSelect.HalfSize;

            // Create Text Control for each file
            for (int i = 0; i < fileNames.Length; i++)
            {
                AddTextControl(Path.GetFileNameWithoutExtension(fileNames[i]), fileNames[i], LoadNameSelectHandler);

            }

            AddTextControl("BACK", null, LoadNameSelectHandler);
        }

        private void LoadNameSelectHandler(GuiControl source, CursorInfo cursorLocation)
        {
            if (source.UserData != null)
            {
                LoadFile(source.UserData);
            }

            //gui.Root.RemoveChild(_loadNamesSelect);
            _loadNamesSelect = null;
        }

        private void LoadFile(string path)
        {
            // TODO: add toast message "loaded"
            var agentLoadout = (AgentLoadout)SaveLoadManager.Instance().Load<AgentLoadout>(path);
            _playerShip.Inventory.Clear();
            agentLoadout.EquipInventory(_playerShip); // TODO: use agentLoadout to create new "playership" which load to it. //agentLoadout.MakeGameObject()
            agentLoadout.EquipLoadout(_playerShip);
            _loadoutName = Path.GetFileNameWithoutExtension(path);// agentLoadout.ID;
        }

        private void AddTextControl(string text, string userData, ActionEventHandler actionHandler)
        {
          //  HorizontalLayout layout = new HorizontalLayout(Vector2.Zero, 5);
            
            TextControl textControl = new TextControl(text, Game1.font);
            textControl.IsShowFrame = true;
            textControl.Action += actionHandler;            
            textControl.UserData = userData;
            textControl.CursorOn += gui.ToolTipHandler;
           // layout.AddChild(textControl);
            _loadNamesSelect.AddChild(textControl);
            // ConfirmationControl deleate = new ConfirmationControl();
            // textControl.AddChild(deleate);
        }

        #endregion
    }
}
