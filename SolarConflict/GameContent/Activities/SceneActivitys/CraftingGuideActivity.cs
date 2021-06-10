using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.Framework.GUI;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework;
using XnaUtils.Input;
using XnaUtils.Graphics;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.AI.GameAI;
using SolarConflict.Framework.Scenes;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    /// <summary>
    /// Guide on crafting
    /// </summary>
    public class CraftingGuideActivity : SceneActivity
    {
       // private List<TutorialGoal> tutorialGoals;
      //  private Sprite guiMarking = Sprite.Get("invArrow");

        private CursorItemControl _cursorItem;
        private Agent playerShip;
      //  private RichTextControl moneyControl;
        private GuiManager gui;
        private AgentSlotsControl agentSlotsControl;
        private InventoryControl inventoryControl;              
        private ItemControl _materialControl;
       

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);                   
            gui = new GuiManager();
            playerShip = _scene.PlayerAgent;
            bool hideCrafting = parameters.ParamDictionary.ContainsKey("hide_crafting");
            _cursorItem = new CursorItemControl( new Vector2(100 * 0.6f));
            _cursorItem.Sprite = null;
            if (playerShip != null && playerShip.Inventory != null && GameplaySettings.AutoSortInventory) //TODO: only when auto sort is on
                playerShip.Inventory.Sort();
            gui.Root = MakeGui(playerShip, _cursorItem, gui, hideCrafting);
            AddHelp(TextBank.Inst.GetString("HelpInventory"));
            //_materialControl
            _materialControl.CursorOn += (s, c) => { _cursorItem.Filter = FilterMaterials; };
            gui.AddControl(_cursorItem);
        }

        public bool FilterMaterials(Item item)
        {
            if(item != null && (item.Category & ItemCategory.Material) > 0)
            {
                return true;
            }
            return false;
        }

        public GuiControl MakeGui(Agent player, CursorItemControl cursor, GuiManager gui, bool hideCrafting = false)
        {
            
            var guiControl = new ControlsGroup();
            HorizontalLayout mainHorizontalLayout = new HorizontalLayout(ActivityManager.ScreenSize / 2);
            mainHorizontalLayout.Spacing = 2;
            guiControl.AddChild(mainHorizontalLayout);
            VerticalLayout verticalLayout = new VerticalLayout(Vector2.Zero);
            mainHorizontalLayout.AddChild(verticalLayout);
            GuiControl playerControl = GuiControlFactory.MakeAgentControl(player, cursor, gui, out agentSlotsControl, out inventoryControl);
            verticalLayout.AddChild(playerControl);
            List<Agent> agents = new List<Agent>();
            agents.Add(player);
            List<Inventory> invList = new List<Inventory>();
            invList.Add(player.Inventory);
            var allyAgent = GuiControlFactory.GetAllyAgent(_scene, player);

            verticalLayout.AddChild( MakeGuideControl(_cursorItem,gui,out _materialControl));



            //if (!hideCrafting)
            //    verticalLayout.AddChild(MakeCommandsControl(player.Inventory, allyAgent, playerControl.Width));

            //if (!hideCrafting)
            //    mainHorizontalLayout.AddChild(GuiControlFactory.MakeCraftingGui(player, cursor, gui, _scene.GameEngine, agents, invList, _scene, out _craftingControl, true, new Point(2, 6)));

            return guiControl;
        }

        public static GuiControl MakeGuideControl(ItemControl cursorItem, GuiManager gui, out ItemControl materialControl)
        {
            VerticalLayout layout = new VerticalLayout(Vector2.Zero);
            
            HorizontalLayout materialCraftingLayout = new HorizontalLayout(Vector2.Zero); //new RelativeLayout();
            materialControl = new ItemControl(null, Vector2.Zero, Vector2.One * 60);
            materialControl.CursorOn += gui.ToolTipHandler;
            materialControl.Data = cursorItem;
            materialControl.Action += (GuiControl source, CursorInfo cursorLocation) =>
            {
                ItemControl cursorControl = (ItemControl)source.Data;
                ItemControl matControl = source as ItemControl;
                if (cursorControl.Item == null || (cursorControl.Item.Category & ItemCategory.CraftingMaterial) > 0)
                {
                    Item item = matControl.Item;
                    matControl.Item = cursorControl.Item;
                    cursorControl.Item = item;
                }
                else
                    ActivityManager.Inst.AddToast("Place Crafting Material", 30,Color.Red);

            };
            materialControl.LogicFunction = (GuiControl control, InputState input) =>
            {
                
                ItemControl cursorControl = (ItemControl)control.Data;
                control.SetAllColors(Palette.SlotColor, true);
                if (cursorItem.Item != null || cursorItem.Data != null)
                {

                    var itemToCheck = cursorItem.Item;
                    if (itemToCheck == null)
                    {
                        itemToCheck = cursorItem.Data as Item;
                    }
                   // ItemSlotControl slotControl = item as ItemSlotControl;
                    if ((itemToCheck.Category & ItemCategory.CraftingMaterial) > 0)
                    {
                        control.SetAllColors(Palette.SlotEnabledColor, true);
                    }
                }
            };
            materialCraftingLayout.AddChild(materialControl);//AddChild(_materialControl, HorizontalAlignment.Left, VerticalAlignment.Down, playerControl, 0); //Chack 
            RichTextControl textControl = new RichTextControl(" <- place material here");
            materialCraftingLayout.AddChild(textControl);//AddChild(text, HorizontalAlignment.Right, VerticalAlignment.Center, _materialControl);
            layout.AddChild(materialCraftingLayout);

            ScrollableGrid craftingRecipes = new ScrollableGrid(10, 4, Vector2.One * 70 * GuiManager.Scale, (int)(10 * GuiManager.Scale) + 1);
            layout.AddChild(craftingRecipes);
            craftingRecipes.Data = new Tuple<ItemControl,Dictionary<string,List<Recipe>>>(materialControl, ContentBank.Inst.GetAllRecipeMatrials());
            craftingRecipes.LogicFunction = (GuiControl control, InputState input) =>
            {
                ScrollableGrid grid = control as ScrollableGrid;
                var itemAndRecipes = control.Data as Tuple<ItemControl, Dictionary<string, List<Recipe>>>;
                if (itemAndRecipes.Item1.Item != null)
                {
                    if(itemAndRecipes.Item1.Item.ID != grid.UserData)
                    {
                        grid.UserData = itemAndRecipes.Item1.Item.ID;
                        if (itemAndRecipes.Item2.ContainsKey(grid.UserData))
                        {
                            grid.RemoveAllChildren();
                            var recipeList = itemAndRecipes.Item2[grid.UserData];
                            foreach (var item in recipeList)
                            {
                                var recipe = new CraftingRecipeControl(item, new Dictionary<string, int>(), Vector2.Zero, grid.ControlSize);
                                recipe.IsShowCraftingStation = true;
                                recipe.SetAllColors(Color.White, false);
                                recipe.DisableMult = 0.9f;
                                recipe.CursorOn += gui.ToolTipHandler;
                                grid.AddChild(recipe);
                            }
                        }
                        
                    }
                }
                else
                {
                    grid.UserData = null;
                    if (grid.Count > 0)
                        grid.RemoveAllChildren();
                }
                    
            };

            return layout;
        }

        public override void Update(InputState inputState)
        {
            base.Update(inputState);
         
          
         //   _cursorItem.Position = _scene.InputState.Cursor.Position;
          //  _cursorItem.ItemUnderCursorCooldown++;
            gui.Update(_scene.InputState);
          
            //ActivityManager.Inst.AddToast(inputState.Cursor.IsActive.ToString(), 50);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            if (gui.Root != null)
            {
                gui.Draw();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                _cursorItem.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(spriteBatch);
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);        
            //spriteBatch.End();
        }
        
        private void CallOnExit()
        {
            SettingsManager.Inst.Save();
            playerShip.control.controlAi = ParameterControl.MakeAIFromAgent(playerShip); //***
            if (_cursorItem.Item != null) //TODO: add if shift is held drop the item or maybe move the item do scene cursour item
            {
                ////_scene.CursorItem = cursorItem.Item;
                if (playerShip.Inventory.HasRoom(_cursorItem.Item.ID, _cursorItem.Item.Stack))// and not drop to space
                {
                    playerShip.AddItemToInventory(_cursorItem.Item);
                }
                else
                {
                    _cursorItem.Item.Position = this.playerShip.Position;//_scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                    _scene.GameEngine.AddGameObject(_cursorItem.Item);
                }
                //_cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                //_scene.GameEngine.AddGameObject(_cursorItem.Item);
            }

            if (_materialControl.Item != null) //TODO: add if shift is held drop the item or maybe move the item do scene cursour item
            {
                ////_scene.CursorItem = cursorItem.Item;
                if (playerShip.Inventory.HasRoom(_materialControl.Item.ID, _materialControl.Item.Stack))// and not drop to space
                {
                    playerShip.AddItemToInventory(_materialControl.Item);
                }
                else
                {
                    _materialControl.Item.Position = this.playerShip.Position;  // _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                    _scene.GameEngine.AddGameObject(_materialControl.Item);
                }
                //_cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                //_scene.GameEngine.AddGameObject(_cursorItem.Item);
            }



        }

        public override ActivityParameters OnLeave()
        {
            CallOnExit();
            return base.OnLeave();
        }

        public override ActivityParameters OnBack()
        {
            CallOnExit();
            return base.OnBack();
        }

        private static void Control_Action(GuiControl source, CursorInfo cursorLocation)
        {
            var tuple = source.Data as Tuple<CraftingControl, SlotType>;
            tuple.Item1.FilterBySlotType(tuple.Item2);
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new CraftingGuideActivity();
        }
    }
}

