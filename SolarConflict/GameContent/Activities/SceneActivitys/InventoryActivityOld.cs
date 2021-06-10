//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using SolarConflict.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
//using SolarConflict.Framework.Scenes.Activitys;
//using SolarConflict.Framework.Scenes.GuiControls;
//using SolarConflict.Framework.Scenes.HudEngine;
//using SolarConflict.GameContent.Activities.SceneActivitys;
//using SolarConflict.XnaUtils.SimpleGui;
//using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
//using System;
//using System.Collections.Generic;
//using XnaUtils;
//using XnaUtils.Graphics;
//using XnaUtils.Input;
//using XnaUtils.SimpleGui;
//using XnaUtils.SimpleGui.Controllers;

//namespace SolarConflict.GameContent.Activities
//{

//    public class InventoryActivityOld : SceneActivity
//    {
//        private List<TutorialGoal> tutorialGoals; //= new TutorialGoal("StarDestroyerItem", 4);     
//        private Sprite guiMarking = Sprite.Get("invArrow");
 
//        protected ItemControl _cursorItem;
//        protected Agent _playerShip;
//        protected GuiManager _gui;
//        private RichTextControl _moneyControl;
//        private RichTextControl _description;
//        private GuiControl _recycleBin;
//        private SlotContainerControl _agentSlotsControl;
//        private InventoryControl _inventoryControl;


//        protected override void Init(ActivityParameters parameters)
//        {
//            base.Init(parameters);
//            tutorialGoals = new List<TutorialGoal>();
//            tutorialGoals.AddRange(_scene.MissionManager.GetTutorialGoals());
//            //TODO: Also get goals from meta world (Add reffrance to metaworld to scene)

//            _gui = new GuiManager();
//            _cursorItem = new ItemControl(null, Vector2.Zero, new Vector2(100 * 0.6f));
//            _cursorItem.Sprite = null;

//            HorizontalLayout mainHorizontalLayout = new HorizontalLayout(ActivityManager.ScreenSize / 2);
//            _gui.Root = new ControlsGroup();

//            _playerShip = _scene.FindPlayer() as Agent; //Fix
//            var inventoryControl = MakeInventoryGui(_playerShip, _cursorItem, _gui, _scene, out _description);
//            List<Inventory> invList = new List<Inventory>();
//           // invList.Add(_playerShip.GetInventory());
//            var allyInv = GetAllyInventory(_scene, _playerShip);
//            if (allyInv != null)
//                invList.Add(allyInv);
//            var craftingControl = MakeCraftingGui(_playerShip, _cursorItem, _gui, _scene.GameEngine, invList);
//            mainHorizontalLayout.AddChild(inventoryControl);
//            mainHorizontalLayout.AddChild(craftingControl);
//            _moneyControl = new RichTextControl(string.Empty);
//            _gui.Root.AddChild(_moneyControl);
//            _gui.Root.AddChild(mainHorizontalLayout);
//            _recycleBin = MakeRecyacleBin();
//            _recycleBin.ControlColor = Color.White;
//            _recycleBin.Position = new Vector2(_recycleBin.HalfSize.X + 50, ActivityManager.ScreenSize.Y - _recycleBin.HalfSize.Y - 50);
//            _recycleBin.CursorOn += _gui.ToolTipHandler;
//            _gui.Root.AddChild(_recycleBin);
//        }

//        private Inventory GetAllyInventory(Scene scene, Agent agent)
//        {
//            if (scene?.GameObjectUnderCursor?.GetFactionType() == Framework.FactionType.Player)
//            {
//                if (scene.GameObjectUnderCursor != agent && scene.GameObjectUnderCursor.GetInventory() != null && scene.GameObjectUnderCursor.GetInventory().Size > 0)
//                {
//                    return scene.GameObjectUnderCursor.GetInventory();
//                }
//            }
//            return null;
//        }

//        public override bool Update(InputState inputState)
//        {
//            base.Update(inputState);
//            _scene.SceneComponentSelector.Update(inputState);
//            _cursorItem.Data = null;

//            if (_playerShip != null)
//            {
//                int money = (int)_playerShip.GetMeterValue(MeterType.Money);
//                _moneyControl.Text = money.ToString() + "#image{coin}";
//                _moneyControl.Position = new Vector2(_moneyControl.HalfSize.X, _moneyControl.HalfSize.Y);
//            }
//            //if (_description != null && _playerShip != null)
//            //{
//            //    _description.Text = AgentUtils.DescribeStatsAndAbilities(_playerShip);
//            //}
//            CursorInfo cursor = _scene.InputState.Cursor;
//            _cursorItem.Position = cursor.Position;
//            _gui.Update(_scene.InputState);
//            return false;
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            DrawBackground(spriteBatch);
//            if (_gui.Root != null)
//            {
//                _gui.Draw();
//                spriteBatch.Begin();
//                _cursorItem.Draw(spriteBatch);        
//                spriteBatch.End();
//            }
//            base.Draw(spriteBatch);
//            spriteBatch.Begin();
//            DrawAllGoals();
//            spriteBatch.End();

//        }

//        public override ActivityParameters OnLeave()
//        {
//            return OnBack();
//        }

//        public override ActivityParameters OnBack()
//        {
//            if (_cursorItem.Item != null) //TODO: add if shift is held drop the item or maybe move the item do scene cursour item
//            {
//                ////_scene.CursorItem = cursorItem.Item;
//                if (_playerShip.Inventory.HasRoom(_cursorItem.Item.ID, _cursorItem.Item.Stack))// and not drop to space
//                {
//                    _playerShip.AddItemToInventory(_cursorItem.Item);
//                }
//                else
//                {
//                    _cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
//                    _scene.GameEngine.AddGameObject(_cursorItem.Item);
//                }
//                //_cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
//                //_scene.GameEngine.AddGameObject(_cursorItem.Item);
//            }

//            return base.OnBack();
//        }

//        private  GuiControl MakeInventoryGui(Agent agent, ItemControl cursor, GuiManager gui, Scene scene, out RichTextControl description)
//        {
//            var layout = new VerticalLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 20));
//            layout.IsResizeHorizontaly = true;
//            HorizontalLayout statsAndSlotsLayout = new HorizontalLayout(Vector2.Zero);
//            statsAndSlotsLayout.ShowFrame = true;
//            statsAndSlotsLayout.IsAutoUpadeSize = false;

//            // OPTION I: Stats control
//            var statsControl = new AgentStatsControl(agent, gui);
//            statsAndSlotsLayout.AddChild(statsControl);
//            statsControl.CursorOn += gui.ToolTipHandler;
//            // OPTION II: Loadout description
//            description = new RichTextControl(AgentUtils.DescribeStatsAndAbilities(agent));
//            description.IsShowFrame = false;
//            description.CursorOverColor = description.ControlColor;
//            //statsAndSlotsLayout.AddChild(description);            
//            _agentSlotsControl = new SlotContainerControl(agent, cursor, Vector2.Zero, gui);
//            statsAndSlotsLayout.AddChild(_agentSlotsControl);
//            layout.AddChild(statsAndSlotsLayout);

//            InventoryControl allyInventoryControl = null;
//            VerticalLayout allyInventoryLayout = null;

//            bool allyInventory = false;
//            if (scene?.GameObjectUnderCursor?.GetFactionType() == Framework.FactionType.Player)
//            {
//                if (scene.GameObjectUnderCursor != agent && scene.GameObjectUnderCursor.GetInventory() != null && scene.GameObjectUnderCursor.GetInventory().Size > 0)
//                {

//                    allyInventoryLayout = new VerticalLayout(Vector2.Zero, 5);
//                    //  allyInventoryLayout.ShowFrame = true;
//                    string text = $"#image{{{scene.GameObjectUnderCursor.GetSprite().ID}}} " + scene.GameObjectUnderCursor.Name;
//                    RichTextControl title = new RichTextControl(text);
//                    title.CursorOverColor = title.ControlColor;
//                    title.IsShowFrame = true;
//                    allyInventoryLayout.AddChild(title);
//                    allyInventoryControl = new InventoryControl(scene.GameObjectUnderCursor.GetInventory(), cursor, Vector2.Zero, gui, lineNum: 2);
//                    // allyInventoryControl.TextureDesign = (GuiDesign)2;
//                    title.HalfSize = new Vector2(allyInventoryControl.HalfSize.X, title.HalfSize.Y);
//                    allyInventoryLayout.AddChild(allyInventoryControl);
//                    allyInventory = true;
//                }
//            }

//            if (agent.Inventory != null)
//            {
               
//                int numberOflines = Math.Min((int)Math.Ceiling(agent.Inventory.Size / 9f), allyInventory ? 2 : 4);
//                var inventoryControl = new InventoryControl(agent.Inventory, cursor, Vector2.Zero, gui, lineNum: numberOflines);
//                statsAndSlotsLayout.AddChild(inventoryControl);
//                _inventoryControl = inventoryControl;
//            }

//            if (allyInventory)
//            {
//                layout.AddChild(allyInventoryLayout);
//            }

//            return layout;
//        }

//        private static GuiControl MakeCraftingGui(Agent agent, ItemControl cursor, GuiManager gui, GameEngine gameEngine, List<Inventory> inventoryList)
//        {
//            var layout = new VerticalLayout(Vector2.Zero);
//            RichTextControl titleControl = new RichTextControl("Crafting");// #image{helpicon}");
//            titleControl.IsShowFrame = true;
//            layout.AddChild(titleControl);

//            CraftingStationType craftingStations = agent.GetCraftingStationType();
//            List<GameObject> nearGameObjects = new List<GameObject>();
//            gameEngine.CollisionManager.GetAllObjectInRange(agent.Position, agent.Size + 1000, nearGameObjects);
//            foreach (var gameObject in nearGameObjects)
//            {
//                craftingStations |= gameObject.GetCraftingStationType();
//            }
//            //Inventory inventory = agent.Inventory;
//            CraftingControl craftingControl = new CraftingControl(2, 5, Vector2.One * 100, inventoryList, cursor, Vector2.Zero, gui, craftingStations);
//            craftingControl.CursorOn += gui.ToolTipHandler;
//            layout.AddChild(craftingControl);

//            titleControl.HalfSize = new Vector2(Math.Max(craftingControl.HalfSize.X, titleControl.HalfSize.X), titleControl.HalfSize.Y);

//            return layout;
//        }

//        private GuiControl MakeRecyacleBin()
//        {
//            GuiControl control = new GuiControl(Vector2.Zero, Vector2.One * 100);
//            //control.ControlColor = GuiManager.DefalutGuiColor;
//            //control.PressedControlColor = GuiManager.DefalutGuiColor;
//            ImageControl imageControl = new ImageControl(Sprite.Get("trash"), Vector2.Zero, Vector2.One * 90);
//            imageControl.ControlColor = GuiManager.DefalutGuiColor;
//            imageControl.IsConsumingInput = false;
//            control.TooltipText = "Place an item to recycle it.";
//            control.AddChild(imageControl);
//            control.CursorOn += delegate (GuiControl source, CursorInfo cursorLocation)
//            {
//                if (_cursorItem.Item != null)
//                {
//                    float money = _cursorItem.TotalSellPrice * 0.5f;
//                    source.TooltipText = "Recycle item for " + Palette.Highlight.ToTag(money.ToString()) + Sprite.Get("coin").ToTag();
//                }
//                else
//                {
//                    source.TooltipText = "Place an item to recycle it.";
//                }
//            };

//            control.Action += delegate (GuiControl source, CursorInfo cursorLocation)
//            {
//                if (_cursorItem != null)
//                {
//                    float money = _cursorItem.TotalSellPrice * 0.5f;
//                    _scene.GetPlayerFaction().AddValueToMeter(MeterType.Money, money);
//                    _cursorItem.Item = null;
//                }
//            };

//            return control;
//        }

//        private void Control_Action(GuiControl source, CursorInfo cursorLocation)
//        {
//            throw new NotImplementedException();
//        }

//        private ImageControl AddHelpIcon()
//        {
//            ImageControl helpText = new ImageControl(Sprite.Get("gui1"), Vector2.Zero, Vector2.One * 60);
//            helpText.TooltipText = @"* To equip an item, drag it to a green slot on the ship (and if needed put the replaced item in the inventory).
//* To split multiple items in the inventory press the right mouse button on the item.
//* Automatic items like 'Auto Repair Kit' don't need to be equipped in order to activate.
//* When you equip an item that needs activation (like an engine or weapon), 
//  right click the item if you want to change its activation key.
//* To use items that activate from inventory like warhead, put them in the 4 leftmost slots in the top row of your inventory. 
//  In battle, press the slot number (1-4) to activate the warhead.
//* Multiple identical items can be stacked in the same slot (depending on the item type).
//* If you want to get rid of an item drag it to the trash bin icon and recycle it for money value of 10% of its buy price.";
//            helpText.CursorOn += _gui.ToolTipHandler;
//            _gui.AddControl(helpText);
//            helpText.Position = new Vector2(10, 10) + helpText.HalfSize;
//            if (_playerShip != null)
//            {
//                int money = (int)_playerShip.GetMeterValue(MeterType.Money);
//                _moneyControl = new RichTextControl("Money: " + money.ToString() + "#image{coin}");
//                _moneyControl.IsShowFrame = true;
//                _moneyControl.Position = helpText.Position + Vector2.UnitX * (+helpText.Width + _moneyControl.HalfSize.X);
//                //    _guiHolder.AddControl(_moneyControl);
//            }
//            return helpText;
//        }

//        private void DrawAllGoals()
//        {
//            bool isDone = true;
//            foreach (var goal in tutorialGoals)
//            {
//                isDone &= DrawTutorialGoal(goal);
//            }

//            if (isDone && tutorialGoals.Count > 0)
//            {

//                ActivityManager.AddToast("Press ESC to go back", 10);  //Change    
//                HudUtils.DrawArrow(Game1.sb, guiMarking, _back.Position);
//            }
//        }

//        private bool DrawTutorialGoal(TutorialGoal goal)
//        {
//            if (goal == null)
//                return true;
//            bool isDone = false;
//            GuiControl destControl = null;
//            if (goal.IsDestInSlot)
//            {

//                var index = _agentSlotsControl.ItemSlotsContainer.FindItemSlot(ContentBank.Inst.GetItem(goal.ItemID, false));
//                if (index.HasValue)
//                {
//                    var slotControl = _agentSlotsControl.GetItemSlot(index.Value);
//                    if (slotControl.Item != null && slotControl.Item.ID == goal.ItemID)
//                        isDone = true;
//                    destControl = slotControl;
//                }

//            }
//            else
//            {
//                //var itemControl = _inventoryControl.GetChildren()[goal.DestIndex] as ItemControl;
//                //if (itemControl.Item != null && itemControl.Item.ID == goal.ItemID)
//                //    isDone = true;
//                //destControl = itemControl;
//            }
//            if (isDone)
//            {

//            }
//            else
//            {
//                if (_cursorItem.Item != null && _cursorItem.Item.ID == goal.ItemID)
//                {
//                    HudUtils.DrawArrow(Game1.sb, guiMarking, destControl.Position);
//                }
//                else
//                {
//                    int index = _inventoryControl.Inventory.FindItem(goal.ItemID);
//                    if (index >= 0)
//                    {

//                        HudUtils.DrawArrow(Game1.sb, guiMarking, _inventoryControl.ItemControls[index].Position);
//                    }
//                }
//            }
//            return isDone;
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new InventoryActivity();
//        }
//    }
//}
